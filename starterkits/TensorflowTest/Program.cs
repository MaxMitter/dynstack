using System;
using Tensorflow;
using static Tensorflow.Binding;


namespace TensorflowTest {
    class Program {
        static void Main(string[] args) {
            var hello = tf.constant("Hello TensowFlow!");
            Console.WriteLine(hello);
        }

        static void Model() {
            const int img_h = 28;
            const int img_w = 28;
            int n_classes = 10;
            int n_channels = 1;
        }

        Datasets mnist;
        static void PrepareData() {

        }
    }
}
