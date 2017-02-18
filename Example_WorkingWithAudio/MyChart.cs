using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Views;
using Android.Util;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Threading;

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
            RecordingSingleton.ProcessDisplayLines(canvas.Width);
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