// <copyright file="MainActivity.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>

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
using WF.Player.Services.Settings;
using WF.Player.Droid.Services.Core;

namespace WF.Player.Droid
{
	using Android.App;
	using Android.Content;
	using Android.OS;
	using Vernacular;
	using Xamarin.Forms.Platform.Android;

	[Activity (Label = "WF.Player", 
		ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait,
		ConfigurationChanges=global::Android.Content.PM.ConfigChanges.Orientation | global::Android.Content.PM.ConfigChanges.ScreenSize
	)]
	public class MainActivity : FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			// Init Xamarin.Forms
			Xamarin.Forms.Forms.Init (this, bundle);
			Xamarin.FormsMaps.Init(this, bundle);

			if (Settings.IsDarkTheme) 
			{
				this.SetTheme (Resource.Style.AppTheme_Dark);
			}
			else
			{
				this.SetTheme (Resource.Style.AppTheme_Light);
			}

			base.OnCreate (bundle);

			Catalog.Implementation = new AndroidCatalog (Resources, typeof (Resource.String));

			#if __HOCKEYAPP__

			//Register to with the Update Manager
			HockeyApp.UpdateManager.Register (this, "acc974c814cad87cf7e01b8e8c4d5ece");

			#endif

			#if __INSIGHTS__

			Xamarin.Insights.Initialize("7cc331e1fae1f21a7646fb5df552ff8213bd8bc9", this);

			#endif

			App.PathCartridges = App.PathForCartridges;
			App.PathDatabase = App.PathForCartridges;

			// Create Xamarin.Forms App and load the first page
			LoadApplication(new App(new AndroidPlatformHelper()));

			this.Window.DecorView.KeepScreenOn = true;
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
			HockeyApp.CrashManager.Register (this, "acc974c814cad87cf7e01b8e8c4d5ece");

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

		public override void OnBackPressed()
		{
			// We are on the GameMainView, so we want to go back until main screen, 
			// but we should leave the game only by quit.
			if (App.GameNavigation != null && App.GameNavigation.CurrentPage is GameMainView)
			{
				App.Game.ShowScreen(ScreenType.Main, null);

				return;
			}

			if (App.Navigation != null && App.Navigation.CurrentPage is CartridgeListPage)
			{
				Exit(0);
			}

			// Go one page back
			if (App.GameNavigation != null)
			{
				// We are in the "in the game" navigation
				if (App.GameNavigation.CurrentPage is GameInputView || App.GameNavigation.CurrentPage is GameMessageboxView)
				{
					return;
				}

				App.Game.ShowScreen(ScreenType.Last, null);
			}
			else
			{
				// We are in the "out of game" navigation
				App.Navigation.PopAsync();
			}
		}

		/// <Docs>The menu item that was selected.</Docs>
		/// <returns>To be added.</returns>
		/// <para tool="javadoc-to-mdoc">This hook is called whenever an item in your options menu is selected.
		///  The default implementation simply returns false to have the normal
		///  processing happen (calling the item's Runnable or sending a message to
		///  its Handler as appropriate). You can use this method for any items
		///  for which you would like to do processing without those other
		///  facilities.</para>
		/// <summary>
		/// Raises the options item selected event.
		/// </summary>
		/// <param name="item">Item.</param>
		public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
		{
			// Handle back button for list pages, because of own back buttons
			if (App.GameNavigation != null)
			{
				// We ar in the game
				if (App.GameNavigation.CurrentPage is GameMainView && item.ItemId == global::Android.Resource.Id.Home)
				{
					App.Game.ShowScreen(ScreenType.Last, null);

					return true;
				}

				if (App.GameNavigation.CurrentPage is GameCheckLocationView && item.ItemId == global::Android.Resource.Id.Home)
				{
					// We are on the check location screen
					// Remove active screen
					App.GameNavigation.CurrentPage.Navigation.PopModalAsync();
					App.GameNavigation = null;

					return true;
				}
			}

			return base.OnOptionsItemSelected(item);
		}

		public void Exit(int exitCode)
		{
			Intent intent = new Intent(Intent.ActionMain);
			intent.AddCategory(Intent.CategoryHome);
			intent.SetFlags(ActivityFlags.ClearTop);
			StartActivity(intent);
			Finish();
			System.Environment.Exit(exitCode);
		}
	}
}

