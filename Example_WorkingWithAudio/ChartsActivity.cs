using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using System.Threading;

namespace Example_WorkingWithAudio
{
    [Activity(Label = "ChartsActivity", ScreenOrientation = ScreenOrientation.Portrait)]
    public class ChartsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);            
            SetContentView(Resource.Layout.Charts);
            ThreadPool.QueueUserWorkItem(o => init());

            //var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //SetActionBar(toolbar);
            //ActionBar.Title = "SnoreFix";
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            //ActionBar.SetHomeButtonEnabled(true);

            var topToolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            //navToolbar.Title = "Nav";
            
            //SetActionBar(topToolbar);
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
            topToolbar.InflateMenu(Resource.Menu.top_menus_chart);
            topToolbar.MenuItemClick += (sender, e) => {
                if (e.Item.TitleFormatted.ToString() == "Back")
                {
                    //var intent = new Intent(this, typeof(WorkingWithAudioActivity));
                    //StartActivity(intent);
                    OnBackPressed();
                }
            };

            var chartView = FindViewById<View>(Resource.Id.myChart1);
            chartView.Touch += ChartTouch;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId != Android.Resource.Id.Home)
                return base.OnOptionsItemSelected(item);
            Finish();
            return true;
        }

        private void ChartTouch(object sender, View.TouchEventArgs touchEventArgs)
        {
            var x = touchEventArgs.Event.RawX;
            Console.WriteLine(x);
        }

        private void init()
        {
            //this is in the wrong place as I need to pass in canvas size.
            //it should be done in the view itself
            RecordingSingleton.ReadFileIntoSampleArray();
            RunOnUiThread(() => updateChartAndText());
        }

        private void updateChartAndText()
        {
            //this is wrong as I'm loading this chart view twice?
            var myChartView = FindViewById(Resource.Id.myChart1);
            myChartView.Invalidate();
            myChartView.RefreshDrawableState();

            var loadingText = FindViewById<TextView>(Resource.Id.loadingText);
            loadingText.Text = "";
        }
    }
}