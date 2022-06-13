import math

from gym import Env, spaces
from enum import Enum
import numpy as np
import cv2

from tf_test.WorldState import WorldState

font = cv2.FONT_HERSHEY_COMPLEX_SMALL

BLOCK_WIDTH = 140
BLOCK_HEIGHT = 50


class BlockState(Enum):
    EMPTY = 0,
    BLOCK_NOT_READY = 1,
    BLOCK_READY = 2,
    BLOCK_OVERDUE = 3


class Block(object):
    def __init__(self):
        self.state = BlockState.EMPTY
        self.width = 50
        self.height = 10

    def __setstate__(self, state):
        self.state = state

    def __getstate__(self):
        return self.state


class HSEnv(Env):
    def __init__(self):
        super(HSEnv, self).__init__()
        self.observation_space = spaces.Tuple((
            spaces.Discrete(12), # -> Production Stack
            spaces.Discrete(36), # -> Buffer Stack
            spaces.Discrete(36), # -> Buffer Stack
            spaces.Discrete(36), # -> Buffer Stack
            spaces.Discrete(36), # -> Buffer Stack
            spaces.Discrete(36), # -> Buffer Stack
            spaces.Discrete(36), # -> Buffer Stack
            spaces.Discrete(2)   # -> Handover Stack
        ))

        self.action_space = spaces.Discrete(42)

        self.canvas = np.ones((1200, 800, 3)) * 1

        self.sim_state = WorldState()
        self.ep_return = 0
        self.reward = 0

    def draw_elements_on_canvas(self):
        self.canvas = np.ones((600, 1200, 3)) * 1

        cursor_x = 10
        cursor_y = 550

        for block in self.sim_state.production.blocks:
            self.canvas = cv2.putText(self.canvas, f"B{block.id}: {block.ready}", (cursor_x, cursor_y), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)
            cursor_y -= BLOCK_HEIGHT

        for buffer in self.sim_state.buffers:
            cursor_y = 550
            cursor_x += BLOCK_WIDTH
            for block in buffer.blocks:
                self.canvas = cv2.putText(self.canvas, f"B{block.id}: {block.ready}", (cursor_x, cursor_y), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)
                cursor_y -= BLOCK_HEIGHT

        for block in self.sim_state.handover.blocks:
            cursor_y = 550
            cursor_x += BLOCK_WIDTH
            self.canvas = cv2.putText(self.canvas, f"B{block.id}: {block.ready}", (cursor_x, cursor_y), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)

        self.canvas = cv2.putText(self.canvas, f"Steps: {self.ep_return} | Reward: {self.reward}", (10, 20), font, 0.8, (0, 0, 0), 1, cv2.LINE_AA)

    def reset(self):
        self.sim_state = WorldState()

        self.ep_return = 0
        self.reward = 0

        self.draw_elements_on_canvas()
        return self.canvas

    def render(self, mode="human"):
        assert mode in ["human", "rgb_array"], "Invalid mode"
        if mode == "human":
            cv2.imshow("Game", self.canvas)
            cv2.waitKey(2000)

        elif mode == "rgb_array":
            return self.canvas

    def close(self):
        cv2.destroyAllWindows()

    def map_action(self, action):
        if action < 6:
            return 0, action + 1
        else:
            source_id = math.floor(action / 6)
            target_id = (action % 6) + 2
            if target_id <= source_id:
                target_id -= 1
            return source_id, target_id

    def step(self, action):
        done = False

        assert self.action_space.contains(action), "Invalid action"
        source_id, target_id = self.map_action(action)
        self.sim_state.move(source_id, target_id)

        self.ep_return += 1

        self.draw_elements_on_canvas()

        if len(self.sim_state.production.blocks) >= self.sim_state.production.max_height:
            done = True

        return self.canvas, self.reward, done, []
