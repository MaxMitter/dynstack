using CustomLogger;
using DynStacking;
using DynStacking.HotStorage.DataModel;
using Google.Protobuf;
using Google.Protobuf.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static csharp.HS_Sync.RFState;

namespace csharp.HS_Sync {
  public class SyncHSPlanner : IPlanner {
    private int seqNr = 0;
    public PolicyFunction RewardFunction { get; set; } = null;

    public Logger Logger { get; set; }

    public byte[] PlanMoves(byte[] worldData, OptimizerType opt) {
      return PlanMoves(World.Parser.ParseFrom(worldData), opt)?.ToByteArray();
    }

    public World LastState { get; private set; }

    public void ResetLastState() {
      LastState = null;
    }

    public CraneSchedule PlanMoves(World world, OptimizerType opt) {
      if (world.Buffers == null || (world.Crane.Schedule.Moves?.Count ?? 0) > 0) {
        if (world.Buffers == null)
          Console.WriteLine($"Cannot calculate, incomplete world.");
        return null;
      }

      var schedule = new CraneSchedule() { SequenceNr = seqNr++ };
      var initial = new RFState(world);
      
      initial.RewardFunction = RewardFunction;
      LastState = world;
      var sw = Stopwatch.StartNew();
      var solution = initial.GetBestMovesBeam(6, 5);
      sw.Stop();
      Logger?.LogDebug($"Got Solution in {sw.Elapsed}ms: {solution.Item1.FormatOutput()}");

      var list = solution.Item1.ConsolidateMoves();

      Logger?.LogDebug($"After consolidating moves: {list.FormatOutput()}");

      if (list.Count() <= 0)
        Logger?.LogDebug($"World state: {world.FormatOutput()}");
      if (solution != null)
        schedule.Moves.AddRange(list.Take(3)
                                .TakeWhile(move => world.Handover.Ready || move.TargetId != world.Handover.Id));

      if (schedule.Moves.Count > 0) {
        return schedule;
      } else {
        return null;
      }
    }
  }
}
