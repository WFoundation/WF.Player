// <copyright file="SettingsPage.cs" company="Wherigo Foundation">
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

using System;
using Xamarin.Forms;
using Vernacular;
using WF.Player.Services.Settings;

namespace WF.Player.SettingsPage
{
	public class SettingsPage : ContentPage
	{
		private TextCell cellPath;
		private Color textColor;
		private Color backgroundColor;

		public SettingsPage()
		{
			Title = Catalog.GetString("Settings");
			BackgroundColor = App.Colors.Background;

			// Save for later use
			// Do this, because color could change while handling with settings :)
			textColor = App.Colors.Text;
			backgroundColor = App.Colors.Background;

			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			var cellTheme = new MultiCell {
				Text = Catalog.GetString("Theme"),
//				Detail = Catalog.GetString("Theme of display"),
				Items = new string[] { 
					Catalog.GetString("Light"), 
					Catalog.GetString("Dark"),
				},
				Key = Settings.DisplayThemeKey,
				DefaultValue = (int)0,
			};

			var sectionTheme = new TableSection(Catalog.GetString("Appearance")) 
				{
					cellTheme,
				};

			var cellTextAlignment = new MultiCell {
				Text = Catalog.GetString("Alignment"),
//				Detail = Catalog.GetString("Alignment of text"),
				Items = new string[] { 
					Catalog.GetString("Left"), 
					Catalog.GetString("Center"),
					Catalog.GetString("Right"),
				},
				Key = Settings.TextAlignmentKey,
				DefaultValue = (int)Settings.DefaultTextAlignment,
			};

			var cellTextSize = new EntryCell 
				{
					Label = Catalog.GetString("Size"),
					LabelColor = App.Colors.Text,
					Keyboard = Keyboard.Numeric,
					Text = Settings.Current.GetValueOrDefault<int>(Settings.TextSizeKey, Settings.DefaultFontSize).ToString(),
					XAlign = TextAlignment.End,
				};

			cellTextSize.Completed += (object sender, EventArgs e) =>
				{
					int value;

					if (int.TryParse(((EntryCell)sender).Text, out value))
					{
						Settings.Current.AddOrUpdateValue<int>(Settings.TextSizeKey, value);
					}
				};
			cellTextSize.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "Text")
				{
					int value;

					if (int.TryParse(((EntryCell)sender).Text, out value))
					{
						Settings.Current.AddOrUpdateValue<int>(Settings.TextSizeKey, value);
					}
				}
			};

			var sectionText = new TableSection(Catalog.GetString("Text")) 
				{
					cellTextAlignment,
					cellTextSize,
				};

			var cellImageAlignment = new MultiCell {
				Text = Catalog.GetString("Alignment"),
//				Detail = Catalog.GetString("Alignment of images"),
				Items = new string[] { 
					Catalog.GetString("Left"), 
					Catalog.GetString("Center"),
					Catalog.GetString("Right"),
				},
				Key = Settings.ImageAlignmentKey,
				DefaultValue = (int)Settings.DefaultImageAlignment,
			};

			var cellImageResize = new MultiCell {
				Text = Catalog.GetString("Resizing"),
//				Detail = Catalog.GetString("Resizing of images"),
				Items = new string[] { 
					Catalog.GetString("Don't resize"), 
					Catalog.GetString("Shrink only"),
					Catalog.GetString("Resize to screen width"),
					Catalog.GetString("Resize to max. half height"),
				},
				ShortItems = new string[] { 
					Catalog.GetString("Don't resize"), 
					Catalog.GetString("Shrink only"),
					Catalog.GetString("Screen width"),
					Catalog.GetString("Max. half height"),
				},
				Key = Settings.ImageResizeKey,
				DefaultValue = (int)Settings.DefaultImageResize,
			};

			var sectionImages = new TableSection(Catalog.GetString("Images")) 
				{
					cellImageAlignment,
					cellImageResize,
				};

			var cellFeedbackSound = new SwitchCell 
				{
					Text = Catalog.GetString("Sound"),
					On = Settings.Current.GetValueOrDefault<bool>(Settings.FeedbackSoundKey, false),
				};

			cellFeedbackSound.OnChanged += (object sender, ToggledEventArgs e) => Settings.Current.AddOrUpdateValue<bool>(Settings.FeedbackSoundKey, e.Value);

			var cellFeedbackVibration = new SwitchCell 
				{
					Text = Catalog.GetString("Vibration"),
					On = Settings.Current.GetValueOrDefault<bool>(Settings.FeedbackVibrationKey, false),
				};

			cellFeedbackVibration.OnChanged += (object sender, ToggledEventArgs e) => Settings.Current.AddOrUpdateValue<bool>(Settings.FeedbackVibrationKey, e.Value);

			var sectionFeedback = new TableSection(Catalog.GetString("Feedback")) 
				{
					cellFeedbackSound,
					cellFeedbackVibration,
				};

			var cellUnitDegrees = new MultiCell {
				Text = Catalog.GetString("Degrees"),
				//				Detail = Catalog.GetString("Resizing of images"),
				Items = new string[] { 
					Catalog.GetString("Decimal degrees (9.07538°)"), 
					Catalog.GetString("Decimal minutes (9° 04.523')"),
					Catalog.GetString("Decimal seconds (9° 04' 31.38\")"),
				},
				ShortItems = new string[] { 
					Catalog.GetString("Decimal degrees"), 
					Catalog.GetString("Decimal minutes"),
					Catalog.GetString("Decimal seconds"),
				},
				Key = Settings.FormatCoordinatesKey,
				DefaultValue = (int)Settings.DefaultFormatCoordinates,
			};

			var cellUnitAltitude = new MultiCell {
				Text = Catalog.GetString("Altitude"),
				//				Detail = Catalog.GetString("Resizing of images"),
				Items = new string[] { 
					Catalog.GetString("Meter"), 
					Catalog.GetString("Feet"),
				},
				Key = Settings.UnitAltitudeKey,
				DefaultValue = (int)Settings.DefaultUnitAltitude,
			};

			var cellUnitLength = new MultiCell {
				Text = Catalog.GetString("Length"),
				//				Detail = Catalog.GetString("Resizing of images"),
				Items = new string[] { 
					Catalog.GetString("Meter"), 
					Catalog.GetString("Feet"),
				},
				Key = Settings.UnitLengthKey,
				DefaultValue = (int)Settings.DefaultUnitLength,
			};

			var sectionUnits = new TableSection(Catalog.GetString("Units")) 
				{
					cellUnitDegrees,
					cellUnitAltitude,
					cellUnitLength,
				};

			var languages = new string[] {
				string.Empty,
				"en",
				"fi",
				"fr",
				"de"
			};

			var cellLanguage = new MultiCell {
				Text = Catalog.GetString("Language"),
				//				Detail = Catalog.GetString("Resizing of images"),
				Items = new string[] { 
					Catalog.GetString("Default"), 
					Catalog.GetString("English"), 
					Catalog.GetString("Finnish"),
					Catalog.GetString("French"),
					Catalog.GetString("German"),
				},
				Values = languages,
				Key = Settings.LanguageKey,
				DefaultValue = Array.IndexOf(languages, Settings.LanguageKey) < 0 ? 0 : Array.IndexOf(languages, Settings.LanguageKey),
			};

			var sectionLanguage = new TableSection(Catalog.GetString("Language")) 
				{
					cellLanguage,
				};

			#if __ANDROID__

			cellPath = new TextCell {
				Text = Catalog.GetString("Path for cartridges"),
				TextColor = App.Colors.Text,
				Detail = Settings.Current.GetValueOrDefault<string>(Settings.CartridgePathKey, null),
				DetailColor = Color.Gray,
			};

			cellPath.Command = new Command((sender) =>
				App.Navigation.Navigation.PushAsync(new FolderSelectionPage(Settings.Current.GetValueOrDefault<string>(Settings.CartridgePathKey, null), () =>
						{
							cellPath.Detail = Settings.Current.GetValueOrDefault<string>(Settings.CartridgePathKey, null);
					}, textColor, backgroundColor)));

			var sectionPath = new TableSection(Catalog.GetString("Path")) 
				{
					cellPath,
				};

			#endif

			var tableRoot = new TableRoot(Catalog.GetString("Settings")) 
				{
					sectionTheme,
					sectionText,
					sectionImages,
					sectionFeedback,
					sectionUnits,
					sectionLanguage,
					#if __ANDROID__
					sectionPath,
					#endif
				};

			var tableView = new TableView() 
				{
					BackgroundColor = App.Colors.Background,
					Intent = TableIntent.Settings,
					Root = tableRoot,
					HasUnevenRows = true,
				};

			Content = tableView;
		}
	}
}
