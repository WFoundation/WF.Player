﻿// <copyright file="GameDetailView.cs" company="Wherigo Foundation">
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
using WF.Player.Services.Settings;

namespace WF.Player
{
	using WF.Player.Controls;
	using Xamarin.Forms;

	/// <summary>
	/// Game detail view.
	/// </summary>
	public class GameDetailView : DirectionBarPage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameDetailView"/> class.
		/// </summary>
		/// <param name="gameDetailViewModel">Game detail view model.</param>
		public GameDetailView(GameDetailViewModel gameDetailViewModel) : base()
		{
			BindingContext = gameDetailViewModel;

			App.GameNavigation.ShowBackButton = true;

			this.SetBinding(GameDetailView.TitleProperty, GameDetailViewModel.NamePropertyName);

			// Set binding for direction
			this.DirectionLayout.SetBinding(StackLayout.IsVisibleProperty, GameDetailViewModel.HasDirectionPropertyName);
			this.DirectionSpaceLayout.SetBinding(BoxView.IsVisibleProperty, GameDetailViewModel.HasDirectionPropertyName);

			this.DirectionView.SetBinding(DirectionArrow.DirectionProperty, GameDetailViewModel.DirectionPropertyName);
			this.DistanceView.SetBinding(Label.TextProperty, GameDetailViewModel.DistancePropertyName, BindingMode.OneWay, new ConverterToDistance());

			#if __HTML__

			var webView = new CustomWebView() 
				{
					BackgroundColor = App.Colors.Background,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			webView.SetBinding(WebView.SourceProperty, GameDetailViewModel.HtmlSourcePropertyName);

			((StackLayout)ContentLayout).Children.Add(webView);

			#else

			var scrollLayout = new PinchScrollView() 
				{
					Orientation = ScrollOrientation.Vertical,
					Padding = new Thickness(0, 0),
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var layout = new StackLayout() 
				{
					BackgroundColor = App.Colors.Background,
					Orientation = StackOrientation.Vertical,
					Padding = 10,
					Spacing = 10,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var image = new ExtendedImage() 
				{
					Aspect = Aspect.AspectFit,
					HorizontalOptions = Settings.ImageAlignment.ToLayoutOptions(),
				};

			image.SetBinding(Image.SourceProperty, GameDetailViewModel.ImageSourcePropertyName);
			image.SetBinding(VisualElement.IsVisibleProperty, GameDetailViewModel.HasImagePropertyName);

			layout.Children.Add(image);

			var description = new ExtendedLabel() 
				{
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize,
					FontFamily = Settings.FontFamily,
					XAlign = Settings.TextAlignment,
					UseMarkdown = App.Game.UseMarkdown,
				};

			description.SetBinding(Label.TextProperty, GameDetailViewModel.DescriptionPropertyName);
			description.SetBinding(VisualElement.IsVisibleProperty, GameDetailViewModel.HasDescriptionPropertyName);

			layout.Children.Add(description);

			scrollLayout.Content = layout;

			((StackLayout)ContentLayout).Children.Add(scrollLayout);

			#endif
		}

		#endregion

		#region Events

		/// <summary>
		/// Handle back button pressed event.
		/// </summary>
		/// <returns>True, because back button should be ignored.</returns>
		protected override bool OnBackButtonPressed()
		{
			App.Game.ShowScreen(ScreenType.Last, null);

			return true;
		}

		#endregion
	}
}
