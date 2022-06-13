import copy
import numpy as np

from hotstorage.hotstorage_model_pb2 import World, CraneSchedule, Handover, CraneMove, Stack, Block


def debug_search():
    w = World()
    w.Production = Stack()
    w.Production.Id = 0
    w.Production.MaxHeight = 4
    b = Block()
    b.Id = 37
    b.Ready = False
    w.Production.BottomToTop.append(b)

    stack = Stack()
    stack.Id = 1
    stack.MaxHeight = 8
    w.Buffers.append(stack)

    handover = Handover()
    handover.Id = 7
    handover.Ready = True
    w.Handover = handover

    plan_moves(w)


def plan_moves(world: World):
    initial = RFState(world)
    print("looking to find best move")
    moves, rating = initial.get_best_moves(list(), 5)
    print(f"found moves: {moves} with rating {rating}")
    return create_schedule_from_solution(world, moves)


def create_schedule_from_solution(world, moves):
    schedule = CraneSchedule()
    handover = world.Handover
    is_ready = handover.Ready
    for opt_mov in moves[:3]:
        if not is_ready and opt_mov.tgt == handover.Id:
            break
        move = CraneMove()
        move.BlockId = opt_mov.block
        move.SourceId = opt_mov.src
        move.TargetId = opt_mov.tgt
        schedule.Moves.append(move)
    if any(schedule.Moves):
        return schedule
    else:
        return None


class RFBlock:
    def __init__(self, id, ready):
        self.id = id
        self.ready = ready

    def print(self):
        print(f"B{self.id}: {self.ready}")


class RFStack:
    def __init__(self, id, max_height, blocks):
        self.id = id
        self.max_height = max_height
        self.blocks = blocks

    def top(self):
        return self.blocks[-1]

    def print(self):
        print("[\n")
        for block in self.blocks:
            print("\t")
            block.print()
            print("\n")

        print("]\n")

    def contains_ready(self):
        for block in self.blocks:
            if block.ready:
                return True


class RFState:
    def __init__(self, world: World):
        stacks = []
        self.prod = RFStack(world.Production.Id, world.Production.MaxHeight,
                            [RFBlock(block.Id, block.Ready) for block in world.Production.BottomToTop])

        for stack in world.Buffers:
            stacks.append(RFStack(stack.Id, stack.MaxHeight,
                                  [RFBlock(block.Id, block.Ready) for block in stack.BottomToTop]))

        self.handover = RFStack(world.Handover.Id, 1, [])
        self.moves = []
        self.buffers = stacks

    def print(self):
        print("{\n")
        print("\t")
        self.prod.print()
        for stack in self.buffers:
            print("\t")
            stack.print()

        self.handover.print()
        print("}")

    def handover_ready(self):
        return any(self.handover.blocks)

    def is_solved(self):
        return not any(self.not_empty_stacks())

    def not_empty_stacks(self):
        for stack in self.buffers:
            if any(stack.blocks):
                yield stack

    def not_full_stacks(self):
        for stack in self.buffers:
            if len(stack.blocks) < stack.max_height:
                yield stack

    def stacks_with_ready(self):
        for stack in self.buffers:
            if stack.contains_ready():
                yield stack

    def apply_move(self, move: CraneMove):
        result = copy.deepcopy(self)
        block = result.remove_block(move.SourceId)
        result.add_block(move.TargetId, block)
        result.moves.append(move)
        return result

    def remove_block(self, stack_id: int) -> RFBlock:
        if stack_id == self.prod.id:
            return self.prod.blocks.pop()
        else:
            for stack in self.buffers:
                if stack.id == stack_id:
                    return stack.blocks.pop()

    def add_block(self, stack_id: int, block: RFBlock):
        if stack_id != self.handover.id and stack_id != self.prod.id:
            for stack in self.buffers:
                if stack.id == stack_id:
                    stack.blocks.append(block)

    def all_possible_moves(self):
        possible = list()
        if self.is_solved():
            return possible

        if len(self.prod.blocks) > 0 and any(self.not_full_stacks()):
            target = self.not_full_stacks()
            move = CraneMove()
            move.SourceId = self.prod.id
            move.TargetId = next(target).id
            move.BlockId = self.prod.top().id
            possible.append(move)

        for src in self.stacks_with_ready():
            if src.top().ready and self.handover_ready:
                move = CraneMove()
                move.SourceId = src.id
                move.TargetId = self.handover.id
                move.BlockId = src.top().id
                possible.append(move)
                break

            for tgt in [stack for stack in self.not_full_stacks() if stack.id != src.id]:
                move = CraneMove()
                move.SourceId = src.id
                move.TargetId = tgt.id
                move.BlockId = src.top().id
                possible.append(move)

        return possible

    def get_best_moves(self, moves, depth) -> tuple[list, int]:
        # print(f"Looking for best moves at depth {depth}")
        if depth == 0:
            return moves, self.calculate_reward()
        else:
            best_rating = -100_000
            best_moves = list()
            for move in self.all_possible_moves():
                new_state = self.apply_move(move)
                moves.append(move)
                new_moves = tuple()
                new_moves, new_rating = new_state.get_best_moves(moves, depth - 1)

                if best_rating < new_rating:
                    best_moves = copy.deepcopy(new_moves)
                    best_rating = new_rating

            return best_moves, best_rating

    def calculate_reward(self):
        reward = 0
        current_buffer = list()
        for buffer in self.buffers:
            current_buffer.append(len(buffer.blocks))
            highest_ready_index = -1
            dist_to_top = 0

            for i in range(0, len(buffer.blocks)):
                block = buffer.blocks[i]
                if block.ready:
                    highest_ready_index = i
                    dist_to_top = 0
                else:
                    dist_to_top += 1

            if highest_ready_index != -1:
                reward -= 10 * dist_to_top

        std_dev = np.std(current_buffer)
        max_std_dev = np.std([0, self.buffers[0].max_height])
        buffer_reward = (1 - (std_dev / max_std_dev)) * 10
        reward += buffer_reward

        reward += 10 * (self.prod.max_height - len(self.prod.blocks))

        if len(self.handover.blocks) > 0:
            reward += 500

        return reward
