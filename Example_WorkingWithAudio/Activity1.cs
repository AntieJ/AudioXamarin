using System;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;
using System.Threading.Tasks;
using Android.Content.PM;

namespace Example_WorkingWithAudio
{
    [Activity(Label = "Example_WorkingWithAudio", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
	public class WorkingWithAudioActivity : Activity
	{
		LowLevelPlayAudio llPlayAudio = new LowLevelPlayAudio ();
		LowLevelRecordAudio llRecordAudio = new LowLevelRecordAudio ();
		static public bool useNotifications = false;
		static Activity activity = null;

		static public Activity Activity {
			get { return (activity); }
		}
		// buttons.
		Button startLlRecording = null;
		Button stopLlRecording = null;
		bool isRecording = false;
		bool isLlRecording = false;
	
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "SnoreFix";

            var navToolbar = FindViewById<Toolbar>(Resource.Id.nav_toolbar);
            //navToolbar.Title = "Nav";
            navToolbar.InflateMenu(Resource.Menu.nav_menus);
            navToolbar.MenuItemClick += (sender, e) => {
                Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
                if(e.Item.TitleFormatted.ToString() == "Charts")
                {     
                    var intent = new Intent(this, typeof(ChartsActivity));
                    StartActivity(intent);
                }
            };

            //var reloadButton = FindViewById<Button>(Resource.Id.reloadButton);

            //reloadButton.Click += async delegate {
            //    await RecordingSingleton.ProcessDisplayLinesAsync();

            //    var intent = new Intent(this, typeof(ChartsActivity));
            //    StartActivity(intent);
            //};
            
            var button = FindViewById<ImageButton>(Resource.Id.recordingButton2);
            button.Click += (o, e) => {
                isLlRecording = !isLlRecording;
                if (isLlRecording)
                {
                    button.SetBackgroundResource(Resource.Drawable.recording_button);
                    startOperationAsync(llRecordAudio);
                    Toast.MakeText(this, "Recording!", ToastLength.Short).Show();
                }
                else
                {
                    button.SetBackgroundResource(Resource.Drawable.not_recording_button);
                    stopOperation(llRecordAudio);
                    llRecordAudio.RecordingStateChanged += (recording) => {
                        
                        llRecordAudio.RecordingStateChanged = null;
                    };
                    Toast.MakeText(this, "Stopped Recording!", ToastLength.Short).Show();

                }


            };
       
            
			activity = this;
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.top_menus, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Action selected: " + item.TitleFormatted,
                ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        async Task startOperationAsync(INotificationReceiver nRec)
        {
            await nRec.StartAsync();
        }

		void stopOperation (INotificationReceiver nRec)
		{
			nRec.Stop ();
		}
	}
}

