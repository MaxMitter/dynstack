using csharp.HS_Sync;
using DynStacking;
using DynStacking.HotStorage.DataModel;
using Google.Protobuf;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using CustomLogger;

namespace HeuristicOptimizer {
  class Program {

    static Logger Logger { get; set; }

    private static double[] vector;

    private static bool SimRunning = false;

    static void Main(string[] args) {
      var port = int.Parse(args[1]);
      //Logger = new Logger(@"C:\Users\P42674\Documents\HL_Problems\WD\test-24.05.txt", port.ToString());
      Logger = new Logger($"{DateTime.Now.ToString("yyyyMMdd")}", $"log-{port}.txt", port.ToString());
      Logger.SetLogLevel(LogLevel.INFO);
      double[] meanResult = { 3600, 0, 3600 };
      bool multiObjective = false;

      try {
        var planner = new SyncHSPlanner();
        var simPort = port + 5000;

        try {
          var values = args[0].Trim().Remove(args[0].LastIndexOf(';')).Split(';');
          vector = new double[values.Length];

          for (int i = 0; i < values.Length; i++) {
            vector[i] = double.Parse(values[i]);
          }

          if (args.Length > 2) {
            if (args[2].ToLower() == "multiobj")
              multiObjective = true;
          }

          Logger.LogInfo($"Input vector:", false);
          foreach (var v in vector)
            Logger.LogInfo($"{v}", false);
          Logger.NewLine();

          planner.RewardFunction = CurrentPolicyFunction;
          planner.Logger = Logger;

          using (var socket = new DealerSocket()) {
            socket.Connect("tcp://localhost:" + simPort);
            bool gotRequest = true;
            List<double> meanBlockingTime = new List<double>();
            List<double> meanBlocksDelivered = new List<double>();
            List<double> meanCraneMoves = new List<double>();

            for (int i = 0; i < 5; i++) {
              planner.ResetLastState();
              if (!SimRunning) {
                StartSimulation(simPort);
              }
              int simSteps = 0;

              while (true) {
                List<byte[]> request = null;
                gotRequest = socket.TryReceiveMultipartBytes(TimeSpan.FromSeconds(3), ref request);
                if (!gotRequest)
                  break;

                simSteps++;
                //var answer = planner.PlanMoves(request[2], OptimizerType.RuleBased);
                var sw = Stopwatch.StartNew();
                var world = World.Parser.ParseFrom(request[2]);
                if (world.Crane?.Schedule?.Moves?.Count > 0)
                  Logger.LogDebug($"Crane has moves {world.Crane?.Schedule?.Moves}");
                var result = planner.PlanMoves(World.Parser.ParseFrom(request[2]), OptimizerType.RuleBased);
                sw.Stop();
                Logger.LogDebug($"Calculated result for Worldtime {planner.LastState.Now}, took {sw.ElapsedMilliseconds}ms: {(result == null ? "empty result" : result.Moves)}");
                var answer = result?.ToByteArray();
                var msg = new NetMQMessage();
                msg.AppendEmptyFrame();
                msg.Append("crane");
                if (answer != null)
                  msg.Append(answer);
                else
                  msg.AppendEmptyFrame();

                socket.SendMultipartMessage(msg);
              }

              Logger.LogDebug("SimSteps taken: " + simSteps);
              Logger.LogInfo($"Calculated result: {planner?.LastState?.KPIs?.BlockedArrivalTime}");
              meanBlockingTime.Add(planner?.LastState?.KPIs?.BlockedArrivalTime ?? 3600.0);
              meanBlocksDelivered.Add(planner?.LastState?.KPIs?.TotalBlocksOnTime ?? 0);
              meanCraneMoves.Add(planner?.LastState?.KPIs?.CraneManipulations ?? 3600);
            }
            Logger.LogDebug($"results: {string.Join(", ", meanBlockingTime)}");

            meanResult[0] = meanBlockingTime.Average();
            meanResult[1] = meanBlocksDelivered.Average();
            meanResult[2] = meanCraneMoves.Average();
          }
        } catch (Exception ex) {
          Logger.LogError(ex.Message);
          Logger.LogError(ex.StackTrace);
          NetMQ.NetMQConfig.Cleanup(false);
        } finally {
          try {
            byte[] bytes;
            if (multiObjective) {
              var count = meanResult.Length * 8; // * 8 for double size
              bytes = new byte[count];
              var quality = meanResult;
              Buffer.BlockCopy(quality, 0, bytes, 0, count);
            } else {
              bytes = BitConverter.GetBytes(meanResult[0]);
            }

            Logger.LogInfo($"Returning quality : {meanResult[0]}, {meanResult[1]}, {meanResult[2]}");

            TcpClient client = new TcpClient();
            client.Connect("localhost", port);
            var ns = client.GetStream() as NetworkStream;
            ns.Socket.Send(bytes);
            ns.Close();
            client.Close();
          } catch (Exception ex) {
            Logger.LogError(ex.Message);
            Logger.LogError(ex.StackTrace);
          }
        }

        Logger.LogDebug("Done Executing...");
      } catch (Exception ex) {
        Logger.LogError(ex.Message);
        Logger.LogError(ex.StackTrace);
      }
    }

    public static Task StartSimulation(int port) {
      return Task.Run(() => {
        Logger.LogInfo("Preparing Simulation on port " + port);
        var processInfo = new ProcessStartInfo("dotnet");
        processInfo.UseShellExecute = false;
        processInfo.WorkingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        processInfo.RedirectStandardOutput = true;
        processInfo.RedirectStandardInput = true;
        processInfo.RedirectStandardError = true;

        var args = processInfo.ArgumentList;
        //args.Add("run");
        //args.Add("--no-build");
        //args.Add("--project");
        args.Add(@"C:\Users\P42674\source\repos\dynstack\simulation\DynStack.SimulationRunner\bin\Debug\net5.0\DynStack.SimulationRunner.dll");
        args.Add("--id");
        args.Add(Guid.Empty.ToString());
        args.Add("--sim");
        args.Add("HS");
        args.Add("--url");
        args.Add("tcp://localhost:8080");
        args.Add("--settings");
        args.Add(@"C:\Users\P42674\Downloads\setting-HS-Training-Train A - Medium 1-1e0174c5-182b-46e1-a2ae-3142e0838bb2.buf");
        args.Add("--syncurl");
        args.Add("tcp://localhost:" + port);
        args.Add("--simulateasync");

        Logger.LogDebug($"Sim Start {args}");

        var proc = Process.Start(processInfo);
        SimRunning = true;
        Logger.LogDebug("Simulation Started");
        proc.EnableRaisingEvents = true;
        proc.Exited += (sender, e) => {
          Program.SimRunning = false;
          Logger.LogDebug("Simulation completed");
        };
        proc.ErrorDataReceived += (sender, e) => {
          Logger.LogError("Error in Simulation: " + e.Data);
        };
        string stdout = proc.StandardOutput.ReadToEnd();
        string stderr = proc.StandardError.ReadToEnd();
        if (!String.IsNullOrEmpty(stderr)) {
          Logger.LogInfo(stdout);
          Logger.LogError(stderr);
        }
      });
    }

    public static double CurrentPolicyFunction(RFState state, CraneMove move) {
      double reward = 0;
      var oldState = new RFState(state);
      var newState = oldState.Apply(move);

      if (move.TargetId == state.Handover.Id) {
        reward += vector[0];
      } else {
        if (move.SourceId == state.Production.Id) {
          reward += vector[1];
          var productionFill = oldState.Production.Count / (double)oldState.Production.MaxHeight;

          if (productionFill >= 1)
            reward += vector[2];
          else if (productionFill >= 0.75)
            reward += vector[3];
          else if (productionFill > 0.25)
            reward += vector[4];

          if (oldState.Buffers.First(stack => stack.Id == move.TargetId).ContainsReady) {
            if (oldState.Buffers.First(stack => stack.Id == move.TargetId).Top().Ready)
              reward += vector[6]; // -
            else
              reward += vector[5]; // -
          }

          // TODO make sure move doesn't block a handover move in the future

          if (newState.SpaceInBuffer < newState.BlocksAboveHighestHandover) {
            reward += vector[12];
          }
        } else {
          var oldSourceBuffer = oldState.Buffers.First(stack => stack.Id == move.SourceId);
          var oldTargetBuffer = oldState.Buffers.First(stack => stack.Id == move.TargetId);
          var newSourceBuffer = newState.Buffers.First(stack => stack.Id == move.SourceId);
          var newTargetBuffer = newState.Buffers.First(stack => stack.Id == move.TargetId);

          if (!oldTargetBuffer.ContainsReady || oldTargetBuffer.IsEmpty)
            reward += vector[8];
          else if (oldTargetBuffer.ContainsReady) {
            if (oldTargetBuffer.Top().Ready)
              reward += vector[6]; // -
            else
              reward += vector[5]; // -
          }

          if (oldTargetBuffer.BlocksAboveReady() < newTargetBuffer.BlocksAboveReady()) {
            reward += vector[9]; // -
          }

          if (oldSourceBuffer.BlocksAboveReady() > newSourceBuffer.BlocksAboveReady()) {
            reward += vector[10];
          }

          if (oldSourceBuffer.ContainsReady) {
            reward += (oldSourceBuffer.MaxHeight - oldSourceBuffer.BlocksAboveReady()) * vector[11];
          }

          if (newSourceBuffer.Top() != null && newSourceBuffer.Top().Ready) {
            reward += vector[7];
          }
        }
      }
      return reward;
    }
  }
}
