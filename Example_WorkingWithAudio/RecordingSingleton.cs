using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Example_WorkingWithAudio
{
    public static class RecordingSingleton
    {
        private static List<Int16> samples = new List<short>();
        public static int lengthSamples;
        public static int lengthSeconds;

        private static bool requiresReload;
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

        public async static Task ProcessDisplayLinesAsync(int screenWidth)
        {
            await Task.Run(() => ProcessDisplayLines(screenWidth));
        }

        public static void ProcessDisplayLines(int screenWidth)
        {          

            ReadFileIntoSampleArray();
            
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
            requiresReload = false;
        }
        
        public static void ReadFileIntoSampleArray()
        {
            if (!requiresReload) return;

            string filePath = "/data/data/Example_WorkingWithAudio.Example_WorkingWithAudio/files/testAudio.mp4";
            byte[] buffer = null;
            
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            long totalBytes = new System.IO.FileInfo(filePath).Length;
            buffer = binaryReader.ReadBytes((Int32)totalBytes);
            fileStream.Close();
            fileStream.Dispose();
            binaryReader.Close();


            var sampleList = new List<Int16>();
            for (int n = 0; n < buffer.Length; n += 2)
            {
                Int16 sample = BitConverter.ToInt16(buffer, n);
                sampleList.Add(sample);
            }

            SetSamples(sampleList);
        }

        public static void NewRecordingMade()
        {
            requiresReload = true;
        }
    }
}