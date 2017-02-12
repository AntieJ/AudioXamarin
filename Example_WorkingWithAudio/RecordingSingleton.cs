using System;
using System.Collections.Generic;

namespace Example_WorkingWithAudio
{
    public static class RecordingSingleton
    {
        private static List<Int16> samples = new List<short>();

        public static List<Int16> GetSamples()
        {
            return samples;
        }

        public static void SetSamples(List<Int16> toSet)
        {
            samples = toSet;
        }
    }
}