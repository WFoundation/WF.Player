// <copyright file="AboutPage.cs" company="Wherigo Foundation">
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

namespace WF.Player.SettingsPage
{
	using System;
	using Vernacular;
	using WF.Player.Controls;
	using WF.Player.Services.Settings;
	using Xamarin.Forms;

	public class AboutPage : ContentPage
	{
		public AboutPage()
		{
			Title = Catalog.GetString("About");
			BackgroundColor = App.Colors.Background;

			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			var headline = new ExtendedLabel 
				{
					Text = Catalog.GetString("WF.Player"),
					TextColor = App.Colors.Text,
					FontAttributes = FontAttributes.Bold,
					FontSize = Settings.FontSize * 2.8,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

//			var platform = new ExtendedLabel 
//				{
//					#if __IOS__
//					Text = Catalog.GetString("for iOS"),
//					#endif
//					#if __ANDROID__
//					Text = Catalog.GetString("for Android"),
//					#endif
//					TextColor = App.Colors.Text,
//					FontSize = Settings.FontSize * 1.2,
//					LineBreakMode = LineBreakMode.WordWrap,
//					XAlign = TextAlignment.Center,
//					HorizontalOptions = LayoutOptions.FillAndExpand,
//					VerticalOptions = LayoutOptions.Fill,
//				};
//
			var copyright = new ExtendedLabel 
				{
					Text = Catalog.GetString("Copyright by\nWherigo Foundation\nDirk Weltz\nBrice Clocher"),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 0.8,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

			var version = new ExtendedLabel 
				{
					Text = Catalog.Format("Version\n{0}", ((App)App.Current).PlatformHelper.ClientVersion),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

			var layout = new StackLayout 
				{
					Orientation = StackOrientation.Vertical,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Spacing = 20,
					Padding = 20,
				};

			layout.Children.Add(headline);
//			layout.Children.Add(platform);
			layout.Children.Add(version);
			layout.Children.Add(copyright);

			Content = new ScrollView {
				Content = layout,
			};
		}
	}
}

