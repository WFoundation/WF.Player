// <copyright file="CartridgeListCell.cs" company="Wherigo Foundation">
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
using WF.Player.Services.Device;
using WF.Player.Models;
using WF.Player.Services.Settings;
using Vernacular;
using WF.Player.Controls;

namespace WF.Player
{
	public class CartridgeListCell : ViewCell
	{
		private static IValueConverter convMediaToImageSource = new ConverterMediaToImageSource();

		private static IValueConverter convStringFormat = new ConverterStringFormat();

		public CartridgeListCell()
		{
			var grid = new Grid() {
				Padding = new Thickness(10, 10),
				ColumnSpacing = 10,
				ColumnDefinitions = new ColumnDefinitionCollection(),
				RowDefinitions = new RowDefinitionCollection(),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength((0.25 * DependencyService.Get<IScreen>().Width), GridUnitType.Absolute) });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

//			var boxPoster = new BoxView()
//				{
//					BackgroundColor = App.Colors.DirectionColor,
//					InputTransparent = true,
//					HorizontalOptions = LayoutOptions.FillAndExpand,
//					VerticalOptions = LayoutOptions.FillAndExpand,
//				};
//
//			grid.Children.Add(boxPoster, 0, 1, 0, 2);

			var imagePoster = new Image() 
				{
					Aspect = Aspect.AspectFit,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
				};

			imagePoster.SetBinding(Image.SourceProperty, CartridgeStore.CartridgePosterPropertyName, BindingMode.Default, convMediaToImageSource);

			grid.Children.Add(imagePoster, 0, 1, 0, 2);

//			var boxHeader = new BoxView()
//				{
//					BackgroundColor = App.Colors.DirectionBackground,
//					InputTransparent = true,
//					HorizontalOptions = LayoutOptions.FillAndExpand,
//					VerticalOptions = LayoutOptions.FillAndExpand,
//				};
//
//			grid.Children.Add(boxHeader, 1, 2, 0, 1);

			var layoutHeader = new StackLayout() 
				{
					Orientation = StackOrientation.Vertical,
					Padding = 0,
					Spacing = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

			var labelHeader = new ExtendedLabel() 
				{
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					XAlign = TextAlignment.Start,
					YAlign = TextAlignment.Start,
					LineBreakMode = LineBreakMode.WordWrap,
					UseMarkdown = false,
					TextColor = App.Colors.Text,
					FontSize = Device.OnPlatform<int>(18, 20, 18),
				};

			labelHeader.SetBinding(Label.TextProperty, CartridgeStore.CartridgeNamePropertyName);

			layoutHeader.Children.Add(labelHeader);

			grid.Children.Add(layoutHeader, 1, 2, 0, 1);

			var layoutDetail = new StackLayout() 
				{
					Orientation = StackOrientation.Vertical,
					Padding = 0,
					Spacing = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

			var labelAuthor = new ExtendedLabel() 
				{
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Start,
					LineBreakMode = LineBreakMode.TailTruncation,
					UseMarkdown = false,
					TextColor = App.Colors.Text,
					FontSize = Device.OnPlatform<int>(11, 12, 11),
				};

			labelAuthor.SetBinding(Label.TextProperty, CartridgeStore.CartridgeAuthorNamePropertyName, BindingMode.Default, convStringFormat, Catalog.GetString("Author:  {0}"));

			layoutDetail.Children.Add(labelAuthor);

			var labelVersion = new ExtendedLabel() 
				{
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.Start,
					UseMarkdown = false,
					TextColor = App.Colors.Text,
					FontSize = Device.OnPlatform<int>(11, 12, 11),
				};

			labelVersion.SetBinding(Label.TextProperty, CartridgeStore.CartridgeVersionPropertyName, BindingMode.Default, convStringFormat, Catalog.GetString("Version: {0}"));

			layoutDetail.Children.Add(labelVersion);

			var labelActivityType = new ExtendedLabel() 
				{
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					UseMarkdown = false,
					TextColor = App.Colors.Text,
					FontSize = Device.OnPlatform<int>(11, 12, 11),
				};

			labelActivityType.SetBinding(Label.TextProperty, CartridgeStore.CartridgeActivityTypePropertyName, BindingMode.Default, convStringFormat, Catalog.GetString("Activity: {0}"));

			layoutDetail.Children.Add(labelActivityType);

			grid.Children.Add(layoutDetail, 1, 2, 1, 2);

			View = grid;
		}
	}
}
