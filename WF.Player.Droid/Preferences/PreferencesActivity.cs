// <copyright file="PreferencesActivity.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player.Droid.Preferences
{
	using System;
	using Android;
	using Android.App;
	using Android.Content;
	using Android.Graphics;
	using Android.Graphics.Drawables;
	using Android.OS;
	using Android.Preferences;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;
	using WF.Player.Droid;

	/// <summary>
	/// Preferences activity.
	/// </summary>
	[Activity(Label = "Settings")]
	public class PreferencesActivity : PreferenceActivity
	{
		/// <summary>
		/// Raises the options item selected event.
		/// </summary>
		/// <Docs>The menu item that was selected.</Docs>
		/// <returns>To be added.</returns>
		/// <para tool="javadoc-to-mdoc">This hook is called whenever an item in your options menu is selected.
		///  The default implementation simply returns false to have the normal
		///  processing happen (calling the item's Runnable or sending a message to
		///  its Handler as appropriate). You can use this method for any items
		///  for which you would like to do processing without those other
		///  facilities.</para>
		/// <param name="item">Item selected.</param>
		public override bool OnOptionsItemSelected(global::Android.Views.IMenuItem item)
		{
			// Is back button pressed
			if (item.ItemId == global::Android.Resource.Id.Home)
			{
				this.SetResult(Result.Ok, new Intent());
				this.Finish();
				return true;
			}

			return base.OnOptionsItemSelected(item);
		}

		/// <summary>
		/// Raises the create event.
		/// </summary>
		/// <param name="bundle">Bundle to use.</param>
		protected override void OnCreate(Bundle bundle)
		{
			// Set color schema for activity
			if (App.Colors.IsDarkTheme)
			{
				this.SetTheme(Android.Resource.Style.ThemeHolo);
			}
			else
			{
				this.SetTheme(Android.Resource.Style.ThemeHoloLight);
			}

			base.OnCreate(bundle);

			this.ActionBar.SetHomeButtonEnabled(true);
			this.ActionBar.SetDisplayHomeAsUpEnabled(true);
			this.ActionBar.SetDisplayShowHomeEnabled(true);
			this.ActionBar.SetIcon(Droid.Resource.Drawable.HomeIcon);
			this.ActionBar.SetBackgroundDrawable(new ColorDrawable(App.Colors.Bar.ToAndroid()));

			// Create your activity here
			this.AddPreferencesFromResource(Droid.Resource.Layout.PreferencesView);
		}
	}
}
