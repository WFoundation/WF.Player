// <copyright file="LanguageSetter.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.Device.LanguageSetter))]

namespace WF.Player.Droid.Services.Device
{
	using Android.Content.Res;
	using Android.Util;
	using Vernacular;
	using WF.Player.Services.Device;
	using WF.Player.Services.Settings;

	public class LanguageSetter : ILanguageSetter
	{
		#region ILanguageSetter implementation

		public void Update()
		{
			if (!string.IsNullOrEmpty(Settings.Current.GetValueOrDefault<string>(Settings.LanguageKey, string.Empty)))
			{
				Resources standardResources = Xamarin.Forms.Forms.Context.Resources;
				AssetManager assets = standardResources.Assets;
				DisplayMetrics metrics = standardResources.DisplayMetrics;
				Configuration config = new Configuration(standardResources.Configuration);
				config.Locale = new Java.Util.Locale(Settings.Current.GetValueOrDefault<string>(Settings.LanguageKey, string.Empty));
				Resources defaultResources = new Resources(assets, metrics, config);

				Catalog.Implementation = new AndroidCatalog(defaultResources, typeof(Resource.String));
			}
			else
			{
				Catalog.Implementation = new AndroidCatalog(Xamarin.Forms.Forms.Context.Resources, typeof(Resource.String));
			}
		}

		#endregion
	}
}

