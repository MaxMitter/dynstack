using csharp.HS_Sync;
using DynStacking;
using DynStacking.HotStorage.DataModel;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace csharp.HS_Self {
  public class Planner : IPlanner {
    public byte[] PlanMoves(byte[] worldData, OptimizerType opt) {
      return PlanMoves(World.Parser.ParseFrom(worldData), opt)?.ToByteArray();
    }

    public void DebugApplication() {
      var w = new World();
      w.Production = new DynStacking.HotStorage.DataModel.Stack();
      w.Production.Id = 0;
      w.Production.MaxHeight = 4;

      w.Production.BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 37, Ready = false });

      w.Buffers.Add(new DynStacking.HotStorage.DataModel.Stack() { Id = 1, MaxHeight = 8 });
      w.Buffers.Add(new DynStacking.HotStorage.DataModel.Stack() { Id = 2, MaxHeight = 8 });
      w.Buffers.Add(new DynStacking.HotStorage.DataModel.Stack() { Id = 3, MaxHeight = 8 });
      w.Buffers.Add(new DynStacking.HotStorage.DataModel.Stack() { Id = 4, MaxHeight = 8 });
      w.Buffers.Add(new DynStacking.HotStorage.DataModel.Stack() { Id = 5, MaxHeight = 8 });
      w.Buffers.Add(new DynStacking.HotStorage.DataModel.Stack() { Id = 6, MaxHeight = 8 });

      w.Handover = new Handover();
      w.Handover.Id = 7;
      w.Handover.Ready = true;

      w.Crane = new Crane();
      w.Crane.Schedule = new CraneSchedule();

      w.Buffers.ElementAt(0).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 36, Ready = false, Due = new TimeStamp() { MilliSeconds = 1013000 } });
      w.Buffers.ElementAt(0).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 35, Ready = true, Due = new TimeStamp() { MilliSeconds = 541000 } });
      w.Buffers.ElementAt(0).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 12, Ready = false, Due = new TimeStamp() { MilliSeconds = 884000 } });
      w.Buffers.ElementAt(0).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 8, Ready = false, Due = new TimeStamp() { MilliSeconds = 1174000 } });
      w.Buffers.ElementAt(0).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 5, Ready = false, Due = new TimeStamp() { MilliSeconds = 1598000 } });

      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 30, Ready = false, Due = new TimeStamp() { MilliSeconds = 644000 } });
      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 28, Ready = false, Due = new TimeStamp() { MilliSeconds = 525000 } });
      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 18, Ready = false, Due = new TimeStamp() { MilliSeconds = 793000 } });
      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 17, Ready = false, Due = new TimeStamp() { MilliSeconds = 1145000 } });
      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 14, Ready = false, Due = new TimeStamp() { MilliSeconds = 1132000 } });
      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 9, Ready = false, Due = new TimeStamp() { MilliSeconds = 1554000 } });
      w.Buffers.ElementAt(1).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 2, Ready = false, Due = new TimeStamp() { MilliSeconds = 1443000 } });

      w.Buffers.ElementAt(2).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 23, Ready = false, Due = new TimeStamp() { MilliSeconds = 1191000 } });
      w.Buffers.ElementAt(2).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 16, Ready = false, Due = new TimeStamp() { MilliSeconds = 1208000 } });
      w.Buffers.ElementAt(2).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 1, Ready = false, Due = new TimeStamp() { MilliSeconds = 1705000 } });

      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 33, Ready = true, Due = new TimeStamp() { MilliSeconds = 253000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 29, Ready = false, Due = new TimeStamp() { MilliSeconds = 1604000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 26, Ready = false, Due = new TimeStamp() { MilliSeconds = 936000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 25, Ready = true, Due = new TimeStamp() { MilliSeconds = 318000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 21, Ready = false, Due = new TimeStamp() { MilliSeconds = 1119000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 19, Ready = false, Due = new TimeStamp() { MilliSeconds = 1049000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 11, Ready = false, Due = new TimeStamp() { MilliSeconds = 1043000 } });
      w.Buffers.ElementAt(3).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 4, Ready = false, Due = new TimeStamp() { MilliSeconds = 2187000 } });

      w.Buffers.ElementAt(4).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 34, Ready = true, Due = new TimeStamp() { MilliSeconds = 465000 } });
      w.Buffers.ElementAt(4).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 24, Ready = false, Due = new TimeStamp() { MilliSeconds = 661000 } });
      w.Buffers.ElementAt(4).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 20, Ready = false, Due = new TimeStamp() { MilliSeconds = 940000 } });
      w.Buffers.ElementAt(4).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 15, Ready = false, Due = new TimeStamp() { MilliSeconds = 991000 } });
      w.Buffers.ElementAt(4).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 6, Ready = false, Due = new TimeStamp() { MilliSeconds = 1331000 } });

      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 32, Ready = false, Due = new TimeStamp() { MilliSeconds = 1060000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 31, Ready = false, Due = new TimeStamp() { MilliSeconds = 1173000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 27, Ready = false, Due = new TimeStamp() { MilliSeconds = 548000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 22, Ready = false, Due = new TimeStamp() { MilliSeconds = 904000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 13, Ready = false, Due = new TimeStamp() { MilliSeconds = 1102000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 10, Ready = false, Due = new TimeStamp() { MilliSeconds = 1182000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 7, Ready = false, Due = new TimeStamp() { MilliSeconds = 2055000 } });
      w.Buffers.ElementAt(5).BottomToTop.Add(new DynStacking.HotStorage.DataModel.Block { Id = 3, Ready = false, Due = new TimeStamp() { MilliSeconds = 1664000 } });
      Console.WriteLine("------- Starting Situation -------");
      Console.WriteLine(w.FormatOutput());
      var state = new RFState(w);
      var m1 = new CraneMove() { SourceId = 2, TargetId = 1, BlockId = 2 };
      var m2 = new CraneMove() { SourceId = 2, TargetId = 3, BlockId = 2 };
      var m3 = new CraneMove() { SourceId = 2, TargetId = 4, BlockId = 2 };
      var m4 = new CraneMove() { SourceId = 2, TargetId = 5, BlockId = 2 };
      var m5 = new CraneMove() { SourceId = 2, TargetId = 6, BlockId = 2 };
      Console.WriteLine($"Move Test ({m1}): {state.CalculateMoveReward(m1)}");
      Console.WriteLine($"Move Test ({m2}): {state.CalculateMoveReward(m2)}");
      Console.WriteLine($"Move Test ({m3}): {state.CalculateMoveReward(m3)}");
      Console.WriteLine($"Move Test ({m4}): {state.CalculateMoveReward(m4)}");
      Console.WriteLine($"Move Test ({m5}): {state.CalculateMoveReward(m5)}");
    }

    private CraneSchedule PlanMoves(World world, OptimizerType opt) {
      if (world.Crane.Schedule.Moves.Count > 0) {
        Console.WriteLine("Schedule already filled");
        return null;
      }
      var schedule = new CraneSchedule();
      schedule.SequenceNr = 1;
      //Console.WriteLine("------------- World State -------------");
      //Console.WriteLine(world.FormatOutput());
      AnyHandoverMove(world, schedule);
      var initState = new RFState(world);
      var sw = Stopwatch.StartNew();
      var solution = initState.GetBestMoves(new List<CraneMove>(), 5, 0);
      sw.Stop();
      //Console.WriteLine($"Solution for this step: {solution.Item1.FormatOutput()}, RF: {solution.Item2}");
      Console.WriteLine($"Found solution in {sw.ElapsedMilliseconds}ms");
      schedule.Moves.AddRange(solution.Item1.Take(3)
          .TakeWhile(move => world.Handover.Ready || move.TargetId != world.Handover.Id));

      if (schedule.Moves.Count > 0) {
        schedule.SequenceNr = world.Crane.Schedule.SequenceNr;
        //Console.WriteLine("Returning Solution");
        return schedule;
      } else {
        //Console.WriteLine("No solution found");
        return null;
      }
    }

    public void AnyHandoverMove(World world, CraneSchedule schedule) {
      if (!world.Handover.Ready)
        return;

      foreach (var stack in world.Buffers) {
        var blocks = stack.BottomToTop.Count;
        if (blocks > 0) {
          var top = stack.BottomToTop[blocks - 1];
          if (top.Ready) {
            var move = new CraneMove();
            move.BlockId = top.Id;
            move.SourceId = stack.Id;
            move.TargetId = world.Handover.Id;
            schedule.Moves.Prepend(move);
          }
        }
      }
    }

    public CraneMove GetBestMove(World world) {
      CraneMove bestMove = null;
      double bestReward = 0;
      var stack = new Stack<WorldState>();
      stack.Push(new WorldState(world));

      while (stack.Count > 0) {
        var state = stack.Pop();
        if (state.Moves.Count > 10)
          break;

        var reward = CalculateReward(state.World);
        Console.WriteLine($"Best Reward: {bestReward} | Reward: {reward} | {(bestReward < reward ? "New best move found" : "")}");
        if (state.IsSolved() || bestReward < reward && state.Moves.Count > 0) {
          bestMove = state.Moves.First();
          Console.WriteLine($"New Best Move: {bestMove}");
          bestReward = reward;
        } else {
          foreach (var move in state.GetAllPossibleMoves()) {
            stack.Push(state.ApplyMove(move));
          }
        }
      }

      return bestMove;
    }

    public static double CalculateReward(World world) {
      var currentArrrival = world.Production.BottomToTop.Count;
      var arrivalTop = world.Production.MaxHeight;
      List<int> currentBuffer = new List<int>();
      foreach (var buffer in world.Buffers) {
        currentBuffer.Add(buffer.BottomToTop.Count);
      }
      var currentHandover = world.Handover.Ready ? 0 : 1;

      double reward = 0;

      if (currentHandover > 0)
        reward += 100;

      var stdDev = currentBuffer.StdDev();
      var maxStdDev = new List<int> { world.Buffers.First().MaxHeight, 0 }.StdDev();
      var bufferReward = Math.Pow(1 - (stdDev / maxStdDev), 0.5) * 10;
      reward = reward + bufferReward;

      reward += 10 * (arrivalTop - currentArrrival);

      return reward;
    }
  }
}
