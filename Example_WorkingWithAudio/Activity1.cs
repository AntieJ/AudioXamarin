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
		Button startLlPlayback = null;
		Button stopLlPlayback = null;
		bool haveRecording = false;
		bool isRecording = false;
		bool isPlaying = false;
		bool haveLlRecording = false;
		bool isLlRecording = false;
		bool isLlPlaying = false;
		TextView status = null;

		void disableAllButtons ()
		{
			startLlRecording.Enabled = false;
			stopLlRecording.Enabled = false;
			startLlPlayback.Enabled = false;
			stopLlPlayback.Enabled = false;
		}
		// Provides the policy for which buttons should be enabled for any particular state.
		// The rule is that any action that has been started must be ended before the user 
		// is allowed to do anything else.
		void handleButtonState ()
		{
			disableAllButtons ();
			

			if (isLlRecording) {
				stopLlRecording.Enabled = true;
				return;
			}
           
		
                
			if (isLlPlaying) {
				stopLlPlayback.Enabled = true;
				return;
			} else if (haveLlRecording) {
				startLlPlayback.Enabled = true;
			}
            
			startLlRecording.Enabled = true;
		}

		public void setStatus (String message)
		{
			status.Text = message;
		}

		public void resetPlayback ()
		{
			isPlaying = false;
			isLlPlaying = false;
			handleButtonState ();
		}

        public async Task ReloadActivity()
        {
            await RecordingSingleton.ProcessDisplayLinesAsync();


            var myChartView = this.FindViewById(Resource.Id.myChart1);
            myChartView.Invalidate();
            myChartView.RefreshDrawableState();
        }

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

            // Toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "AJ Toolbar";

            // Low-level operations.
            startLlRecording = FindViewById<Button> (Resource.Id.llStartRecordingButton);
			startLlRecording.Click += delegate {
               
				startOperationAsync (llRecordAudio);
                disableAllButtons (); 
                isLlRecording = true; 
				haveLlRecording = true; 
				handleButtonState ();
			};
  
			stopLlRecording = FindViewById<Button> (Resource.Id.llEndRecordingButton);
			stopLlRecording.Click += delegate {
				stopOperation (llRecordAudio);
				isLlRecording = false;
				llRecordAudio.RecordingStateChanged += (recording) => {
					if(!isRecording)
						handleButtonState ();

					llRecordAudio.RecordingStateChanged = null;
				};
			};
 
			startLlPlayback = FindViewById<Button> (Resource.Id.llStartPlaybackButton);

			startLlPlayback.Click += async delegate {
				await startOperationAsync (llPlayAudio); 
				disableAllButtons (); 
				isLlPlaying = true;  
				handleButtonState ();
			};

			stopLlPlayback = FindViewById<Button> (Resource.Id.llEndPlaybackButton);

			stopLlPlayback.Click += delegate {
				stopOperation (llPlayAudio);
				isLlPlaying = false;
				handleButtonState ();
			};

            var reloadButton = FindViewById<Button>(Resource.Id.reloadButton);

            reloadButton.Click += async delegate {
                await ReloadActivity();
            };

            handleButtonState ();
            
            
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

