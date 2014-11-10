// <copyright file="MainActivity.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;
using Android.Preferences;
using WF.Player.Services.Preferences;


namespace WF.Player.Droid
{
	[Activity (Label = "WF.Player", 
//		MainLauncher = true, 
//		Theme="@android:style/Theme.NoTitleBar", 
		ConfigurationChanges=global::Android.Content.PM.ConfigChanges.Orientation | global::Android.Content.PM.ConfigChanges.ScreenSize
	)]
	public class MainActivity : AndroidActivity
	{
		const string HOCKEYAPP_APPID = "<YOUR-HOCKEY-APP-APPID>";

		protected override void OnCreate (Bundle bundle)
		{
			var isDarkTheme = PreferenceManager.GetDefaultSharedPreferences (this).GetInt (DefaultPreferences.DisplayThemeKey, default(int));

			if (isDarkTheme != 0) {
				this.SetTheme (Resource.Style.AppTheme_Dark);
			} else {
				this.SetTheme (Resource.Style.AppTheme_Light);
			}

			base.OnCreate (bundle);

			#if __HOCKEYAPP__

			//Register to with the Update Manager
			HockeyApp.UpdateManager.Register (this, HOCKEYAPP_APPID);

			#endif

			#if __INSIGHTS__

			Xamarin.Insights.Initialize("7cc331e1fae1f21a7646fb5df552ff8213bd8bc9", this);

			#endif

			// Init Xamarin.Forms
			Xamarin.Forms.Forms.Init (this, bundle);

			// Set default page for this activity
			SetPage (App.GetMainPage ());
		}

		/// <summary>
		/// Raises the activity result event.
		/// </summary>
		/// <Docs>The integer request code originally supplied to
		///  startActivityForResult(), allowing you to identify who this
		///  result came from.</Docs>
		/// <param name="requestCode">Request code.</param>
		/// <param name="resultCode">Result code.</param>
		/// <param name="data">An Intent, which can return result data to the caller
		///  (various data can be attached to Intent "extras").</param>
		protected override void OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			if (resultCode == Result.Ok) {
				if (App.Colors.IsDarkTheme) {
					this.SetTheme (Resource.Style.AppTheme_Dark);
				} else {
					this.SetTheme (Resource.Style.AppTheme_Light);
				}

				this.Recreate ();
			}

			base.OnActivityResult (requestCode, resultCode, data);
		}

		protected override void OnResume ()
		{
			base.OnResume ();

			#if __HOCKEYAPP__

			// Register for Crash detection / handling
			// You should do this in your main activity
			HockeyApp.CrashManager.Register (this, HOCKEYAPP_APPID);

			//Start Tracking usage in this activity
			HockeyApp.Tracking.StartUsage (this);

			#endif
		}

		protected override void OnPause ()
		{
			#if __HOCKEYAPP__

			//Stop Tracking usage in this activity
			HockeyApp.Tracking.StopUsage (this);

			#endif

			base.OnPause ();
		}
	}
}

