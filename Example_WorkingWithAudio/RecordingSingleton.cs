using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Example_WorkingWithAudio
{
    public static class RecordingSingleton
    {
        private static List<Int16> samples = new List<short>();
        public static int lengthSamples;
        public static int lengthSeconds;

        private static SimpleLine[] displayLines;
        private static SimpleLine baseLine;

        public static List<Int16> GetSamples()
        {
            return samples;
        }

        public static void SetSamples(List<Int16> toSet)
        {
            samples = toSet;
            lengthSamples = samples.Count;
            lengthSeconds = lengthSamples / 11025;
        }

        public static SimpleLine[] GetDisplayLines()
        {
            return displayLines;
        }

        public static SimpleLine GetBaseLine()
        {
            return baseLine;
        }

        public async static Task ProcessDisplayLinesAsync()
        {
            await Task.Run(() => ProcessDisplayLines());
        }

        public static void ProcessDisplayLines()
        {
            var screenWidth = 900;
            var baseLineY = 500;
            var lineHeightMax = 500;

            baseLine = new SimpleLine
            {
                Start = new Point(0, baseLineY),
                End = new Point(screenWidth, baseLineY)
            };

            var smallBuffer = new List<Int16>();
            var everyXSample = 50;//50;
            for (var i = 0; i < (samples.ToArray().Length); i += everyXSample)
            {
                smallBuffer.Add(samples[i]);
            }
            var smallBufferArray = smallBuffer.Where(a => a < smallBuffer.Max() && a > smallBuffer.Min()).ToArray();

            var sampleLines = new SimpleLine[smallBufferArray.Length];


            for (var i = 0; i < (smallBufferArray.Length); i++)
            {
                float xAnchor = ((float)i / (float)smallBufferArray.Length) * screenWidth;

                float lineHeightPercent = (float)smallBufferArray[i] / (float)smallBufferArray.Max();
                float lineHeightScaled = (float)lineHeightPercent * (float)lineHeightMax;

                var lineHeight = (float)baseLineY - (float)lineHeightScaled;

                sampleLines[i] = new SimpleLine
                {
                    Start = new Point((int)xAnchor, baseLineY),
                    End = new Point((int)xAnchor, (int)lineHeight)
                };

            }

            displayLines =  sampleLines;
        }
    }
}