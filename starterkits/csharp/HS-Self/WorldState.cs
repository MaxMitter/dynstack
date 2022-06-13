using DynStacking.HotStorage.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp.HS_Self {
    public class WorldState {
        public World World { get; set; }

        public WorldState(World world) {
            this.World = world;
        }

        public WorldState(WorldState source) {
            this.World = new World(source.World);
        }

        public List<CraneMove> Moves { get; set; } = new List<CraneMove>();

        public bool IsSolved() {
            foreach (var buffer in World.Buffers) {
                if (buffer.BottomToTop.Count > 0)
                    return false;
            }

            return true;
        }

        public WorldState ApplyMove(CraneMove move) {
            var result = new WorldState(this);
            //PrintInfo(result.World);
            //PrintInfo(move);
            if (move.SourceId == result.World.Production.Id) {
                var block = this.World.Production.BottomToTop.First(bl => bl.Id == move.BlockId);
                result.World.Production.BottomToTop.Remove(block);
                result.World.Buffers.First(buff => buff.BottomToTop.Count < buff.MaxHeight).BottomToTop.Add(block);
            } else if (move.TargetId == result.World.Handover.Id) {
                var block = this.World.Buffers.First(buff => buff.Id == move.SourceId).BottomToTop.First(bl => bl.Id == move.BlockId);
                result.World.Buffers.First(buff => buff.Id == move.SourceId).BottomToTop.Remove(block);
                result.World.Handover.Block = block;
            } else {
                var block = this.World.Buffers.First(buff => buff.Id == move.SourceId).BottomToTop.First(bl => bl.Id == move.BlockId);
                result.World.Buffers.First(buff => buff.Id == move.SourceId).BottomToTop.Remove(block);
                result.World.Buffers.First(buff => buff.BottomToTop.Count < buff.MaxHeight).BottomToTop.Add(block);
            }

            result.Moves.Add(move);
            return result;
        }

        public List<CraneMove> GetAllPossibleMoves() {
            var result = new List<CraneMove>();

            if (World.Production.BottomToTop.Count > 0) {
                foreach (var block in World.Production.BottomToTop) {
                    foreach (var buffer in World.Buffers) {
                        if (buffer.BottomToTop.Count < buffer.MaxHeight) {
                            var move = new CraneMove();
                            move.SourceId = World.Production.Id;
                            move.BlockId = block.Id;
                            move.TargetId = buffer.Id;
                            result.Add(move);
                        }
                    }
                }
            }

            foreach (var sourceBuffer in World.Buffers) {
                if (sourceBuffer.BottomToTop.Count != 0) {
                    var block = sourceBuffer.BottomToTop[sourceBuffer.BottomToTop.Count - 1];
                    if (block.Ready) {
                        var move = new CraneMove();
                        move.SourceId = sourceBuffer.Id;
                        move.BlockId = block.Id;
                        move.TargetId = World.Handover.Id;
                        result.Add(move);
                    }

                    foreach (var targetBuffer in World.Buffers) {
                        if (sourceBuffer.Id != targetBuffer.Id) {
                            if (targetBuffer.BottomToTop.Count < targetBuffer.MaxHeight) {
                                var move = new CraneMove();
                                move.SourceId = sourceBuffer.Id;
                                move.BlockId = block.Id;
                                move.TargetId = targetBuffer.Id;
                                result.Add(move);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static void PrintInfo(World world) {
            Console.WriteLine("---------- World ----------");
            Console.WriteLine($"Production stack: ID {world.Production.Id} | Use {world.Production.BottomToTop.Count} | Max {world.Production.MaxHeight}");
            foreach (var buffer in world.Buffers) {
                Console.WriteLine($"Buffer stack: ID {buffer.Id} | Use {buffer.BottomToTop.Count} | Max {buffer.MaxHeight}");
            }
            Console.WriteLine($"Handover stack: ID {world.Handover.Id} | Ready {world.Handover.Ready}");
        }

        public static void PrintInfo(CraneMove move) {
            Console.WriteLine("---------- Move ----------");
            Console.WriteLine($"Source ID: {move.SourceId} | Target ID: {move.TargetId} | Block ID: {move.BlockId}");
        }
    }
}
