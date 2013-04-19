using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Timers;

namespace RazorSVG
{
	[Activity (Label = "RazorSVG", MainLauncher = true)]
	public class Activity1 : Activity
	{
		int count = 1;

		protected SeekBar CurrentValueSeekBar, UpdateSpeedSeekBar;

		protected TextView CurrentValueLabel, UpdateSpeedLabel;

		protected Button StartStopButton;

		protected Timer UpdateTimer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//initalizing UI
			InitLabelReferences ();
			InitCurrentValueSeekBar ();
			InitUpdateSpeedSeekBar ();
			InitStartStopButton();

			//initial updating of labels
			UpdateSpeedLabelText();
			UpdateCurrentValueLabelText();

			//Initializing update timer
			InitUpdateTimer();


			UpdateStartStopButtonLabel();

		}

		protected void InitLabelReferences ()
		{
			CurrentValueLabel = FindViewById<TextView> (Resource.Id.currentValueLabel);
			UpdateSpeedLabel = FindViewById<TextView> (Resource.Id.updateSpeedLabel);
		}

		protected void InitUpdateSpeedSeekBar ()
		{
			UpdateSpeedSeekBar = FindViewById<SeekBar> (Resource.Id.updateSpeedSeekBar);
			UpdateSpeedSeekBar.Max = 60;
			UpdateSpeedSeekBar.Progress = 24;
			UpdateSpeedSeekBar.ProgressChanged += HandleUpdateSpeedChanged;
		}


		protected void InitCurrentValueSeekBar ()
		{
			CurrentValueSeekBar = FindViewById<SeekBar> (Resource.Id.currentValueSeekBar);
			CurrentValueSeekBar.Max = 100;
			CurrentValueSeekBar.Progress = 50;
			CurrentValueSeekBar.ProgressChanged += HandleCurrentValueChanged;
		}

		protected void InitUpdateTimer ()
		{
			UpdateTimer = new Timer ();
			UpdateTimer.Elapsed += HandleTimerUpdateElapsed;
			UpdateTimerInterval ();
		}

		protected void InitStartStopButton ()
		{
			StartStopButton = FindViewById<Button> (Resource.Id.startStopButton);
			StartStopButton.Click += HandleStartStopClick;
		
		}

		void HandleStartStopClick (object sender, EventArgs e)
		{
			if(UpdateTimer.Enabled == true){
				UpdateTimer.Stop();

			}else{
				UpdateTimer.Start();
			}
			UpdateStartStopButtonLabel();
		}

		protected void UpdateCurrentValueLabelText ()
		{
			CurrentValueLabel.Text = String.Format("Current Value ({0})", CurrentValueSeekBar.Progress.ToString());
		}

		protected void UpdateSpeedLabelText ()
		{
			UpdateSpeedLabel.Text = String.Format("Update Speed ({0})", UpdateSpeedSeekBar.Progress.ToString());
		}

		protected void UpdateTimerInterval ()
		{

			var timesPerSecond = UpdateSpeedSeekBar.Progress;
			UpdateTimer.Interval = 1000 / timesPerSecond;

		}

		protected void UpdateStartStopButtonLabel ()
		{
			StartStopButton.Text = (UpdateTimer.Enabled == true)?"Stop":"Start";
		}

		protected void HandleCurrentValueChanged (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			UpdateCurrentValueLabelText();
		}
		protected void HandleUpdateSpeedChanged (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			if(UpdateSpeedSeekBar.Progress == 0){
				UpdateSpeedSeekBar.Progress = 1;
			}
			UpdateTimerInterval();
			UpdateSpeedLabelText();

		}
		protected void HandleTimerUpdateElapsed (object sender, ElapsedEventArgs e)
		{
			Console.WriteLine("Tick");
		}
	}
}


