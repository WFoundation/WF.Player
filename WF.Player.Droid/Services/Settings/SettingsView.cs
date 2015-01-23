// <copyright file="SettingsViewAndroid.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.SettingsViewAndroid))]

namespace WF.Player.Droid.Services
{
	using Android.App;
	using Android.Content;
	using WF.Player.Droid.Preferences;
	using WF.Player.Services.Settings;
	using Xamarin.Forms;

	/// <summary>
	/// Implements ISettingView for Android.
	/// </summary>
	public class SettingsViewAndroid : ISettingsView
	{
		/// <summary>
		/// Show Preferences Screen.
		/// </summary>
		public void Show()
		{
			Intent intent = new Intent(Forms.Context, typeof(PreferencesActivity));

			((Activity)Forms.Context).StartActivityForResult(intent, 1);

			if (App.Colors.IsDarkTheme) 
			{
				Forms.Context.SetTheme(Droid.Resource.Style.AppTheme_Dark);
			} 
			else 
			{
				Forms.Context.SetTheme(Droid.Resource.Style.AppTheme_Light);
			}

			Settings.Current.Changed();
		}
	}
}
