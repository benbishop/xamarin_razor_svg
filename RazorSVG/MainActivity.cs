using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RazorSVG
{
	[Activity (Label = "RazorSVG", MainLauncher = true)]
	public class Activity1 : Activity
	{
		int count = 1;

		protected SeekBar CurrentValueSeekBar, UpdateSpeedSeekBar;

		protected TextView CurrentValueLabel, UpdateSpeedLabel;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			//initalizing UI
			InitLabelReferences ();
			InitCurrentValueSeekBar ();
			InitUpdateSpeedSeekBar ();

			//initial updating of labels
			UpdateSpeedLabelText();
			UpdateCurrentValueLabelText();

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

		protected void UpdateCurrentValueLabelText ()
		{
			CurrentValueLabel.Text = String.Format("Current Value ({0})", CurrentValueSeekBar.Progress.ToString());
		}

		protected void UpdateSpeedLabelText ()
		{
			UpdateSpeedLabel.Text = String.Format("Update Speed ({0})", UpdateSpeedSeekBar.Progress.ToString());
		}

		protected void HandleCurrentValueChanged (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			UpdateCurrentValueLabelText();
		}
		protected void HandleUpdateSpeedChanged (object sender, SeekBar.ProgressChangedEventArgs e)
		{
			UpdateSpeedLabelText();
		}
	}
}


