using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Timers;
using Android.Webkit;
using System.Collections.Generic;

namespace RazorSVG
{

	[Activity (Label = "RazorSVG", MainLauncher = true, ScreenOrientation=Android.Content.PM.ScreenOrientation.Portrait)]
	public class Activity1 : Activity
	{
		int count = 1;

		protected SeekBar CurrentValueSeekBar1, UpdateSpeedSeekBar, CurrentValueSeekBar2;

		protected TextView CurrentValueLabel1, UpdateSpeedLabel, CurrentValueLabel2;

		protected Button StartStopButton;

		protected Timer UpdateTimer;

		protected WebView GraphWebview;

		protected List<int> GraphValues1, GraphValues2;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//initalizing UI
			InitLabelReferences ();
			InitWebViewReference();
			InitCurrentValueSeekBar1 ();
			InitCurrentValueSeekBar2 ();
			InitUpdateSpeedSeekBar ();
			InitStartStopButton();

			//initial updating of labels
			UpdateSpeedLabelText();
			UpdateCurrentValue1LabelText();

			//Initializing update timer
			InitUpdateTimer();


			UpdateStartStopButtonLabel();

			var template = new TestGraph();
			var str = template.GenerateString ();
			GraphWebview.Settings.JavaScriptEnabled = true;

			GraphWebview.LoadData (str, "text/html", null);

			GraphValues1 = new List<int>();
			GraphValues2 = new List<int>();


		}

		protected void InitLabelReferences ()
		{
			CurrentValueLabel1 = FindViewById<TextView> (Resource.Id.currentValueLabel1);
			CurrentValueLabel2 = FindViewById<TextView> (Resource.Id.currentValueLabel2);
			UpdateSpeedLabel = FindViewById<TextView> (Resource.Id.updateSpeedLabel);
		}

		void InitWebViewReference ()
		{
			GraphWebview = FindViewById<WebView> (Resource.Id.svgWebView);
		}

		protected void InitUpdateSpeedSeekBar ()
		{
			UpdateSpeedSeekBar = FindViewById<SeekBar> (Resource.Id.updateSpeedSeekBar);
			UpdateSpeedSeekBar.Max = 60;
			UpdateSpeedSeekBar.Progress = 24;
			UpdateSpeedSeekBar.ProgressChanged += HandleUpdateSpeedChanged;
		}


		protected void InitCurrentValueSeekBar1 ()
		{
			CurrentValueSeekBar1 = FindViewById<SeekBar> (Resource.Id.currentValueSeekBar1);
			CurrentValueSeekBar1.Max = 400;
			CurrentValueSeekBar1.Progress = 50;
			CurrentValueSeekBar1.ProgressChanged += HandleCurrentValue1Changed;
		}


		protected void InitCurrentValueSeekBar2 ()
		{
			CurrentValueSeekBar2 = FindViewById<SeekBar> (Resource.Id.currentValueSeekBar2);
			CurrentValueSeekBar2.Max = 400;
			CurrentValueSeekBar2.Progress = 25;
			CurrentValueSeekBar2.ProgressChanged += HandleCurrentValue2Changed;
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
			if (UpdateTimer.Enabled) {
				UpdateTimer.Stop ();

			} else {
				UpdateTimer.Start ();
			}
			UpdateStartStopButtonLabel();
		}

		protected void UpdateCurrentValue1LabelText ()
		{
			CurrentValueLabel1.Text = String.Format("Current Value ({0})", CurrentValueSeekBar1.Progress.ToString());
		}

		protected void UpdateCurrentValue2LabelText ()
		{
			CurrentValueLabel2.Text = String.Format("Current Value ({0})", CurrentValueSeekBar2.Progress.ToString());
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
			StartStopButton.Text = (UpdateTimer.Enabled) ?"Stop":"Start";
		}

		protected void HandleCurrentValue1Changed (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			UpdateCurrentValue1LabelText();
		}
		protected void HandleCurrentValue2Changed (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			UpdateCurrentValue2LabelText();
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
			GraphValues1.Add(CurrentValueSeekBar1.Progress);
			GraphValues2.Add(CurrentValueSeekBar2.Progress);
			RunOnUiThread(UpdateWebView);
		}


		protected void UpdateWebView()
		{
			GraphWebview.LoadUrl("javascript:updatePathData1('"+ ConvertListToPathData(GraphValues1) + "')");
			GraphWebview.LoadUrl("javascript:updatePathData2('"+ ConvertListToPathData(GraphValues2) + "')");
		}

		protected string ConvertListToPathData(List<int> values)
		{
			if(values.Count > 250){
				values.RemoveRange(0, values.Count - 250);
			}
			string pathDataStr = "M";
			for(int i = 0; i < values.Count; i++){
				pathDataStr += String.Format("{0} {1} ", i.ToString(), values[i].ToString());
			}
			var furthestX = (values.Count - 1).ToString ();
			pathDataStr += String.Format ("{0} 400 ", furthestX);
			pathDataStr += "0 400 ";
			pathDataStr += String.Format("0 {0} ", values[0].ToString());
			pathDataStr += "Z";
			return pathDataStr;
		}
	}
}


