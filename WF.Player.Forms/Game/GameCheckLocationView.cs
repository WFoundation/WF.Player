// <copyright file="GameCheckLocationView.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using Vernacular;
	using Xamarin.Forms;

	/// <summary>
	/// Game check location view.
	/// </summary>
	public class GameCheckLocationView : ToolBarPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameCheckLocationView"/> class.
		/// </summary>
		/// <param name="viewModel">View model.</param>
		public GameCheckLocationView(GameCheckLocationViewModel viewModel) : base()
		{
			BindingContext = viewModel;

			Title = Catalog.GetString("GPS Check");

			NavigationPage.SetBackButtonTitle(this, string.Empty);
			NavigationPage.SetHasBackButton(this, false);

			var layout = new StackLayout() 
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 10,
				Padding = new Thickness(10, 10),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var label = new Label() 
			{
				Text = Catalog.GetString("For much fun with the cartridge, you should wait for a good accuracy of your GPS signal."),
				TextColor = App.Colors.Text,
				BackgroundColor = Color.Transparent,
				LineBreakMode = LineBreakMode.WordWrap,
				XAlign = App.Prefs.TextAlignment,
				Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize),
			};

			layout.Children.Add(label);

			layout.Spacing = 20;

			var coordinates = new Label() 
			{
				Text = string.Format(Catalog.GetString("Current Coordinates\n{0}"), Catalog.GetString("Unknown")),
				TextColor = App.Colors.Text,
				BackgroundColor = Color.Transparent,
				LineBreakMode = LineBreakMode.WordWrap,
				XAlign = TextAlignment.Center,
				Font = App.Fonts.Normal.WithAttributes(FontAttributes.Bold).WithSize(App.Prefs.TextSize),
			};
			coordinates.SetBinding(Label.TextProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToLatLon(), Catalog.GetString("Current Coordinates\n{0}"));

			layout.Children.Add(coordinates);

			var accuracy = new Label() 
			{
				Text = string.Format(Catalog.GetString("Current Accuracy\n{0}"), Catalog.GetString("Unknown")),
				TextColor = App.Colors.Text,
				BackgroundColor = Color.Transparent,
				LineBreakMode = LineBreakMode.WordWrap,
				XAlign = TextAlignment.Center,
				Font = App.Fonts.Normal.WithAttributes(FontAttributes.Bold).WithSize(App.Prefs.TextSize),
			};
			accuracy.SetBinding(Label.TextProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAccuracy(), Catalog.GetString("Current Accuracy\n{0}"));

			layout.Children.Add(accuracy);

			var scrollLayout = new ScrollView() 
			{
				Orientation = ScrollOrientation.Vertical,
				Padding = 0,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Content = layout,
			};

			((StackLayout)ContentLayout).Children.Add(scrollLayout);

			Buttons.Add(new ToolTextButton(Catalog.GetString("Start anyway"), viewModel.StartCommand));

			Buttons[0].Button.SetBinding(Button.TextProperty, GameCheckLocationViewModel.ButtonTextPropertyName);
			Buttons[0].Button.SetBinding(Button.TextColorProperty, GameCheckLocationViewModel.ButtonTextColorPropertyName);
		}
	}
}
