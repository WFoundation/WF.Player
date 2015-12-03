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

			var emptyline = new ExtendedLabel 
				{
					Text = Catalog.Format("\n"),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 1.4,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

			var headline = new ExtendedLabel 
				{
					Text = Catalog.Format("# WF.Player  \nfor {0}", Device.OnPlatform("iOS", "Android", "WinPhone")),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 1.4,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};

			var version = new ExtendedLabel 
				{
					Text = Catalog.Format("__Version__  \n{0}", ((App)App.Current).PlatformHelper.ClientVersion),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};

			var copyright = new ExtendedLabel 
				{
					Text = Catalog.GetString("__Copyright by__  \nWherigo Foundation  \nDirk Weltz  \nBrice Clocher\n"),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 1.0,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};

			var credits = new ExtendedLabel 
				{
					Text = Catalog.GetString("__Credits__\nThis app uses the following software or parts of it"),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 0.8,
					FontAttributes = FontAttributes.Bold,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};
			
			var xamarin = new ExtendedLabel 
				{
					Text = Catalog.GetString("__Xamarin.iOS, Xamarin.Android and Xamarin.Forms__\nCopyright by Xamarin, Inc."),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 0.6,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};

			var cdhowie = new ExtendedLabel 
				{
					Text = Catalog.GetString("__Eluant__\nCopyright by cdhowie"),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 0.6,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};

			var groundspeak = new ExtendedLabel 
				{
					Text = Catalog.GetString("__Wherigo__\nCopyright by Groundspeak, Inc."),
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize * 0.6,
					LineBreakMode = LineBreakMode.WordWrap,
					XAlign = TextAlignment.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
					UseMarkdown = true,
				};

			var layout = new StackLayout 
				{
					Orientation = StackOrientation.Vertical,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					Spacing = 10,
					Padding = 20,
				};

			layout.Children.Add(headline);
			layout.Children.Add(version);
			layout.Children.Add(copyright);
			layout.Children.Add(emptyline);
			layout.Children.Add(credits);
			layout.Children.Add(xamarin);
			layout.Children.Add(cdhowie);
			layout.Children.Add(groundspeak);

			Content = new ScrollView {
				Content = layout,
			};
		}
	}
}

