using csharp.HS_Sync;
using DynStacking.HotStorage.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csharp.HS_Self {

    public class Block {
        public Block(int id, bool ready, TimeStamp due) {
            Id = id;
            Ready = ready;
            Due = due;
        }
        public int Id { get; }
        public bool Ready { get; }
        public TimeStamp Due { get; }
    }

    public class Stack {
        public int Id { get; }
        public int MaxHeight { get; }
        public Stack<Block> Blocks { get; }

        public Stack(DynStacking.HotStorage.DataModel.Stack stack) {
            Id = stack.Id;
            MaxHeight = stack.MaxHeight;
            Blocks = new Stack<Block>(stack.BottomToTop.Select(b => new Block(b.Id, b.Ready, b.Due)));
        }

        public Stack(Handover stack) {
            Id = stack.Id;
            MaxHeight = 1;
            Blocks = new Stack<Block>();
            if (stack.Block != null)
                Blocks.Push(new Block(stack.Block.Id, stack.Block.Ready, stack.Block.Due));
        }

        public Stack(Stack other) {
            Id = other.Id;
            MaxHeight = other.MaxHeight;
            Blocks = new Stack<Block>(other.Blocks.Reverse());
        }

        public Block Top => Blocks.Peek();
    }

    public class RFState {
        public List<CraneMove> Moves { get; }
        private Stack Production { get; }
        private List<Stack> Buffers { get; }
        private Stack Handover { get; }

        public RFState(World world) {
            Moves = new List<CraneMove>();
            Production = new Stack(world.Production);
            Handover = new Stack(world.Handover);
            Buffers = new List<Stack>();
            Buffers.AddRange(world.Buffers.Select(buf => new Stack(buf)));
        }

        public RFState(RFState other) {
            Moves = other.Moves.ToList();
            Handover = new Stack(other.Handover);
            Production = new Stack(other.Production);
            Buffers = new List<Stack>();
            Buffers.AddRange(other.Buffers.Select(buf => new Stack(buf)));
        }

        public double CalculateReward(int handovers) {
            double reward = 0;
            List<int> currentBuffer = new List<int>();
            foreach (var buffer in Buffers) {
                currentBuffer.Add(buffer.Blocks.Count);
                var highestReadyIndex = -1;
                var distToTop = 0;
                var bufferList = buffer.Blocks.ToArray();
                for (int i = 0; i < buffer.Blocks.Count; i++) {
                    var block = bufferList[i];
                    if (block.Ready) {
                        highestReadyIndex = i;
                        distToTop = 0;
                    } else {
                        distToTop++;
                    }
                }
                if (highestReadyIndex != -1)
                    reward -= 10 * distToTop;
            }

            var stdDev = currentBuffer.StdDev();
            var maxStdDev = new List<int> { 0, Buffers.First().MaxHeight }.StdDev();
            var bufferReward = (1 - (stdDev / maxStdDev)) * 10;
            reward += bufferReward;

            reward += 10 * (Production.MaxHeight - Production.Blocks.Count);

            if (Handover.Blocks.Count > 0)
                reward += 500;

            reward += 500 * handovers;

            return reward;
        }

        public double CalculateMoveReward(CraneMove move) {
            double reward = 0;
            var oldState = this;
            var newState = oldState.Apply(move);

            if (move.TargetId == Handover.Id)
                reward += 500;

            if (move.SourceId == Production.Id) {
                reward += 50;
                if (!StacksWithReady.Any(stack => stack.Id == move.TargetId))
                    reward += 50;
            }

            reward -= (newState.OverReadyBlocks() - oldState.OverReadyBlocks()) * 10;
            reward -= (newState.OverBlockDue(new TimeStamp() { MilliSeconds = 240000 }) 
                        - oldState.OverBlockDue(new TimeStamp() { MilliSeconds = 240000 })) * 10;


            return reward;
        }

        private int OverReadyBlocks() {
            int ret = 0;
            foreach(var stack in Buffers) {
                var count = 0;
                foreach(var block in stack.Blocks) {
                    if (!block.Ready)
                        count++;
                    else {
                        ret += count;
                        count = 0;
                    }
                }
            }

            return ret;
        }

        private int OverBlockDue(TimeStamp target) {
            int ret = 0;
            foreach (var stack in Buffers) {
                var count = 0;
                foreach (var block in stack.Blocks) {
                    if (block.Due.MilliSeconds < target.MilliSeconds)
                        count++;
                    else {
                        ret += count;
                        count = 0;
                    }
                }
            }

            return ret;
        }

        public Tuple<List<CraneMove>, double> GetBestMoves(List<CraneMove> moves, int depth, int handovers) {
            if (depth == 0) {
                return new Tuple<List<CraneMove>, double>(moves, this.CalculateReward(handovers));
            } else {
                double bestRating = int.MinValue;
                List<CraneMove> bestMoves = new List<CraneMove>();
                foreach (var move in this.GetAllPossibleMoves(depth)) {
                    if (!moves.Any(m => move.BlockId == m.BlockId && move.SourceId == m.SourceId && move.TargetId == m.TargetId)) {
                        var newState = new RFState(this.Apply(move));
                        moves.Add(move);
                        Tuple<List<CraneMove>, double> newMoves = null;
                        if (move.TargetId == Handover.Id)
                            newMoves = newState.GetBestMoves(moves, depth - 1, handovers + 1);
                        else
                            newMoves = newState.GetBestMoves(moves, depth - 1, handovers);

                        if (bestMoves == null || bestRating < newMoves.Item2) {
                            bestRating = newMoves.Item2;
                            bestMoves = new List<CraneMove>(newMoves.Item1);
                        }
                        moves.Remove(move);
                    }
                }
                return new Tuple<List<CraneMove>, double>(bestMoves, bestRating);
            }
        }


        public bool IsSolved => !Production.Blocks.Any() && !NotEmptyStacks.Any();
        IEnumerable<Stack> NotFullStacks => Buffers.Where(b => b.Blocks.Count < b.MaxHeight);
        IEnumerable<Stack> NotEmptyStacks => Buffers.Where(b => b.Blocks.Count > 0);
        IEnumerable<Stack> StacksWithReady => Buffers.Where(b => b.Blocks.Any(block => block.Ready));
        bool HandoverReady => !Handover.Blocks.Any();

        public List<CraneMove> GetAllPossibleMoves(int depth) {
            var possible = new List<CraneMove>();
            if (IsSolved) return possible;

            if (Production.Blocks.Count > 0 && NotFullStacks.Any()) {
                var target = NotFullStacks.First();
                possible.Add(new CraneMove {
                    SourceId = Production.Id,
                    TargetId = target.Id,
                    BlockId = Production.Top.Id
                });
            }

            foreach (var srcStack in StacksWithReady) {
                if (srcStack.Top.Ready && HandoverReady) {
                    possible.Add(new CraneMove {
                        SourceId = srcStack.Id,
                        TargetId = Handover.Id,
                        BlockId = srcStack.Top.Id
                    });
                    break;
                }

                foreach (var tgtStack in NotFullStacks.Where(stack => stack.Id != srcStack.Id)) {
                    possible.Add(new CraneMove {
                        SourceId = srcStack.Id,
                        TargetId = tgtStack.Id,
                        BlockId = srcStack.Top.Id
                    });
                }
            }

            return possible;
        }

        public Block RemoveBlock(int stackId) {
            if (stackId == Production.Id)
                return Production.Blocks.Pop();
            else
                return Buffers.First(b => b.Id == stackId).Blocks.Pop();
        }

        public void AddBlock(int stackId, Block block) {
            if (stackId != Handover.Id && stackId != Production.Id) {
                Buffers.First(b => b.Id == stackId).Blocks.Push(block);
            } else {
                // Production should never be a target
                // If handover is the target, pretend the Block dissappears immediatly
            }
        }

        public RFState Apply(CraneMove move) {
            var result = new RFState(this);
            var block = result.RemoveBlock(move.SourceId);
            result.AddBlock(move.TargetId, block);
            result.Moves.Add(move);
            return result;
        }
    }
}
