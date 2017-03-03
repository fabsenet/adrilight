using System;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace adrilight.benchmarks
{
    public class FadeNonLinearBenchmarks
    {
        private const int OPS_COUNT = 1000;

        private static readonly Random _random = new Random(1);

        [Benchmark(Baseline = true, OperationsPerInvoke = OPS_COUNT)]
        public static void FadeNonLinear_Baseline()
        {
            for (int i = 0; i < OPS_COUNT; i++)
            {
                float c = _random.Next(0, 256);
                FadeNonLinear1(c);
            }
        }
        private static float FadeNonLinear1(float color)
        {
            const float factor = 80f;
            return 256f * ((float)Math.Pow(factor, color / 256f) - 1f) / (factor - 1);
        }
        private static byte FadeNonLinear2(float color)
        {
            const float factor = 80f;
            return (byte) (256f * ((float)Math.Pow(factor, color / 256f) - 1f) / (factor - 1));
        }

        [Benchmark(OperationsPerInvoke = OPS_COUNT)]
        public static void FadeNonLinear_Byte()
        {
            for (int i = 0; i < OPS_COUNT; i++)
            {
                float c = _random.Next(0, 256);
                FadeNonLinear2(c);
            }
        }

        private static readonly byte[] _cached256 = Enumerable.Range(0, 256)
            .Select(n => FadeNonLinear2(n))
            .ToArray();

        [Benchmark(OperationsPerInvoke = OPS_COUNT)]
        public static void FadeNonLinear_Byte_Cached256_byte()
        {
            for (int i = 0; i < OPS_COUNT; i++)
            {
                float c = _random.Next(0, 256);
                FadeNonLinear_Lookup256_byte(c);
            }
        }

        private static byte FadeNonLinear_Lookup256_int(float color)
        {
            return _cached256[(int)color];
        }
        [Benchmark(OperationsPerInvoke = OPS_COUNT)]
        public static void FadeNonLinear_Byte_Cached256_int()
        {
            for (int i = 0; i < OPS_COUNT; i++)
            {
                float c = _random.Next(0, 256);
                FadeNonLinear_Lookup256_int(c);
            }
        }

        private static byte FadeNonLinear_Lookup256_byte(float color)
        {
            return _cached256[(byte)color];
        }




        private static readonly byte[] _cached2560 = Enumerable.Range(0, 2560)
            .Select(n => FadeNonLinear2(n/10f))
            .ToArray();

        [Benchmark(OperationsPerInvoke = OPS_COUNT)]
        public static void FadeNonLinear_Byte_Cached2560()
        {
            for (int i = 0; i < OPS_COUNT; i++)
            {
                float c = _random.Next(0, 256);
                FadeNonLinear_Lookup2560(c);
            }
        }

        private static byte FadeNonLinear_Lookup2560(float color)
        {
            return _cached2560[(int)(color*10)];
        }
    }
}