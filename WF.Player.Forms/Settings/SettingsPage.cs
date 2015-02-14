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
		public SettingsPage()
		{
			Title = Catalog.GetString("Settings");

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
					Label = Catalog.GetString("Textsize"),
					LabelColor = App.Colors.Text,
					Keyboard = Keyboard.Numeric,
					Text = Settings.Current.GetValueOrDefault<float>(Settings.TextSizeKey, Settings.DefaultFontSize).ToString(),
					XAlign = TextAlignment.End,
				};

			cellTextSize.Completed += (object sender, EventArgs e) =>
				{
					float value;

					if (float.TryParse(((EntryCell)sender).Text, out value))
					{
						Settings.Current.AddOrUpdateValue<float>(Settings.TextSizeKey, value);
					}
				};
			cellTextSize.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if (e.PropertyName == "Text")
				{
					float value;

					if (float.TryParse(((EntryCell)sender).Text, out value))
					{
						Settings.Current.AddOrUpdateValue<float>(Settings.TextSizeKey, value);
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

			var tableRoot = new TableRoot(Catalog.GetString("Settings")) 
				{
					sectionTheme,
					sectionText,
					sectionImages,
					sectionFeedback,
				};

//			Content = new TableView {
//				Intent = TableIntent.Settings,
//				Root = new TableRoot("Table Title") {
//					new TableSection("Section 1 Title") {
//						new TextCell {
//							Text = "TextCell Text",
//							Detail = "TextCell Detail"
//						},
//						new EntryCell {
//							Label = "EntryCell:",
//							Placeholder = "default keyboard",
//							Keyboard = Keyboard.Default
//						}
//					},
//					new TableSection("Section 2 Title") {
//						new EntryCell {
//							Label = "Another EntryCell:",
//							Placeholder = "phone keyboard",
//							Keyboard = Keyboard.Telephone
//						},
//						new SwitchCell {
//							Text = "SwitchCell:"
//						}
//					}
//				}
//			};

			var tableView = new TableView() 
				{
					Intent = TableIntent.Settings,
					Root = tableRoot,
					HasUnevenRows = true,
				};

			Content = tableView;
		}

//		float value;
//
//		if (float.TryParse(entry.Text, out value))
//		{
//			return value;
//		}
//		else
//		{
//			return 0;
//		}

		void HandleTapped (object sender, EventArgs e)
		{

		}
	}
}
