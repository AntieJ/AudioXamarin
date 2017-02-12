using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace Example_WorkingWithAudio
{
    public class MyChart : View
    {
        private readonly ShapeDrawable _shape;
        private Paint _paint;

        public MyChart(Context context, IAttributeSet attrs) : base(context)
        {
            _paint = new Paint();
            _paint.SetARGB(255, 255, 0, 0);

            _paint.SetStyle(Paint.Style.Stroke);
            _paint.StrokeWidth = 4;

        }

        protected override void OnDraw(Canvas canvas)
        {
            DrawGraph(canvas);
            DrawInfo(canvas);
        }

        private void DrawInfo(Canvas canvas)
        {
            var samplesLength = RecordingSingleton.lengthSamples;
            var secondsLength = RecordingSingleton.lengthSeconds;
            
            var stringToDisplay = string.Format("{0} samples. {1} seconds", samplesLength, secondsLength);

            var paint = new Paint();
            paint.SetARGB(255, 0, 255, 0);
            paint.SetStyle(Paint.Style.Fill);
            paint.StrokeWidth = 3;
            paint.TextSize = 30;
            paint.TextAlign= Paint.Align.Center;

            canvas.DrawText(stringToDisplay, 500, 900, paint);
        }

        private void DrawGraph(Canvas canvas)
        {
            var linesToDraw = RecordingSingleton.GetDisplayLines();
            var baseLine = RecordingSingleton.GetBaseLine();

            if (linesToDraw != null && baseLine != null)
            {
                canvas.DrawLine(baseLine.Start.X, baseLine.Start.Y, baseLine.End.X, baseLine.End.Y, GetBlue());//base line

                for (var i = 0; i < (linesToDraw.Length); i++)
                {
                    var line = linesToDraw[i];
                    canvas.DrawLine(line.Start.X, line.Start.Y, line.End.X, line.End.Y, GetRed());

                }
            }
        }

        private void DrawGraph(Canvas canvas, Int16[] buffer)
        {
            var screenWidth = 900;
            var baseLineY = 500;
            var lineHeightMax = 500;
            canvas.DrawLine(0, baseLineY, screenWidth, baseLineY, GetBlue());//base line


            var smallBuffer = new List<Int16>();
            var everyXSample = 50;//50;
            for (var i = 0; i < (buffer.Length); i += everyXSample)
            {
                smallBuffer.Add(buffer[i]);
            }
            var smallBufferArray = smallBuffer.Where(a => a < smallBuffer.Max() && a > smallBuffer.Min()).ToArray();

            for (var i = 0; i < (smallBufferArray.Length); i++)
            {
                float xAnchor = ((float)i / (float)smallBufferArray.Length) * screenWidth;

                float lineHeightPercent = (float)smallBufferArray[i] / (float)smallBufferArray.Max();
                float lineHeightScaled = (float)lineHeightPercent * (float)lineHeightMax;

                var lineHeight = (float)baseLineY - (float)lineHeightScaled;

                canvas.DrawLine(xAnchor, baseLineY, xAnchor, lineHeight, GetRed());

            }
        }       

        private Paint GetBlue()
        {
            var paint = new Paint();
            paint.SetARGB(255, 0, 0, 255);
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 4;

            return paint;
        }

        private Paint GetRed()
        {
            var paint = new Paint();
            paint.SetARGB(255, 255, 0, 0);
            paint.SetStyle(Paint.Style.Stroke);
            paint.StrokeWidth = 4;

            return paint;
        }

        

    }
}