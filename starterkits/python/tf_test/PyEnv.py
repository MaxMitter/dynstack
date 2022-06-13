from __future__ import absolute_import, division, print_function

import abc
import tensorflow as tf
import numpy as np

from tf_agents.environments import py_environment, utils, wrappers, suite_gym
from tf_agents.specs import array_spec
from tf_agents.trajectories import time_step as ts


class HsEnvironment(py_environment.PyEnvironment):

    def __init__(self):
        self._action_spec = array_spec.BoundedArraySpec(
            shape=(), dtype=np.int32, minimum=0, maximum=1, name='action')
        self._observation_spec = array_spec.BoundedArraySpec(
            shape=(1,), dtype=np.int32, minimum=0, name='observation')
        self._state = 0
        self._episode_ended = False

    def action_spec(self):
        return self._action_spec

    def observation_spec(self):
        return self._observation_spec

    def _reset(self) -> ts.TimeStep:
        self._state = 0
        self._episode_ended = False
        return ts.restart(np.array([self._state], dtype=np.int32))

    def _step(self, action) -> ts.TimeStep:
        if self._episode_ended:
            return self.reset()

        if action == 1:
            self._episode_ended = True
        elif action == 0:
            # TODO move
            self._state += 1
        else:
            raise ValueError("")

        if self._episode_ended or self._state == 10:
            reward = self._state - 1
            return ts.termination(np.array([self._state], dtype=np.int32), reward)
        else:
            return ts.transition(np.array([self._state], dtype=np.int32), reward=0.0, discount=1.0)


env = HsEnvironment()
utils.validate_py_environment(env, episodes=5)
