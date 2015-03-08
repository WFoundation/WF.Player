// <copyright file="Language.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.iOS.Services.Device.LanguageSetter))]

namespace WF.Player.iOS.Services.Device
{
	using System;
	using Foundation;
	using Vernacular;
	using WF.Player.Services.Device;
	using WF.Player.Services.Settings;

	/// <summary>
	/// Language interface to change language while app is running.
	/// </summary>
	public class LanguageSetter : ILanguageSetter
	{
		/// <summary>
		/// The lang bundle.
		/// </summary>
		NSBundle langBundle;

		#region ILanguage implementation

		public void Update()
		{
			// Check if a language is in the preferences that we have
			var prefLang = NSUserDefaults.StandardUserDefaults.ArrayForKey("AppleLanguages")[0].ToString();
			var settingsLang = Settings.Current.GetValueOrDefault<string>(Settings.LanguageKey, string.Empty);

			var lang = string.IsNullOrEmpty(settingsLang) ? prefLang : settingsLang;

			// We don't want to have english as default language, because it is the development language
			if (!lang.Equals("en") && Array.IndexOf(NSBundle.MainBundle.Localizations, lang) >= 0)
			{
				langBundle = NSBundle.FromPath(NSBundle.MainBundle.PathForResource(lang, "lproj"));
			}
			else
			{
				// We want to use the default language
				langBundle = null;
			}

			// Activate Vernacular Catalog
			Catalog.Implementation = new ResourceCatalog 
				{
					GetResourceById = id => {
						if (langBundle == null)
							return null;
						var resource = 	langBundle.LocalizedString(id, null);
						return resource == id ? null : resource;
					},
				};
		}

		#endregion
	}
}

