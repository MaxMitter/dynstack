from HS_Env import HSEnv

from tf_agents.environments import suite_gym, tf_environment

num_iterations = 250
collect_episodes_per_iteration = 2
replay_buffer_capacity = 2000

fc_layer_params = (100,)

learning_rate = 1e-3
log_interval = 25
num_eval_episodes = 10
eval_interval = 50

env = HSEnv()
env.reset()

print(env.observation_space)

print(env.action_space)

train_py_env = HSEnv()
train_env = tf_environment.TFEnvironment(train_py_env)
