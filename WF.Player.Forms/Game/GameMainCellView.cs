// <copyright file="GameMainCellView.cs" company="Wherigo Foundation">
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
	/// Game main cell view.
	/// </summary>
	public class GameMainCellView : ViewCell
	{
		/// <summary>
		/// The hori layout.
		/// </summary>
		private StackLayout horiLayout;

		/// <summary>
		/// The name.
		/// </summary>
		private Label name;

		/// <summary>
		/// The icon.
		/// </summary>
		private Image icon;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameMainCellView"/> class.
		/// </summary>
		public GameMainCellView()
		{
			this.horiLayout = new StackLayout() 
				{
					BackgroundColor = Color.Transparent,
					Orientation = StackOrientation.Horizontal,
					Spacing = 10,
					Padding = new Thickness(10, 0, 0, 0),
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			this.icon = new Image() 
			{
				BackgroundColor = Color.Transparent,
				WidthRequest = 48,
				HeightRequest = 48,
				Aspect = Aspect.AspectFit,
			};
			this.icon.SetBinding(Image.SourceProperty, GameMainCellViewModel.IconSourcePropertyName);
			this.icon.SetBinding(Image.IsVisibleProperty, GameMainCellViewModel.ShowIconPropertyName);

			this.name = new Label() 
				{
					XAlign = TextAlignment.Start,
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.StartAndExpand,
					FontSize = Settings.FontSize,
					FontFamily = Settings.FontFamily,
					TextColor = App.Colors.Text,
				};
			this.name.SetBinding(Label.TextProperty, GameMainCellViewModel.NamePropertyName);

			var vertLayout = new StackLayout() 
			{
				WidthRequest = 54,
				Orientation = StackOrientation.Vertical,
				Spacing = 5,
				Padding = new Thickness(10, 5),
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.End,
			};
			vertLayout.SetBinding(StackLayout.IsVisibleProperty, GameMainCellViewModel.ShowDirectionPropertyName);

			var direction = new DirectionArrow() 
			{
				BackgroundColor = Color.Transparent,
				CircleColor = App.Colors.DirectionBackground,
				ArrowColor = App.Colors.DirectionColor,
				WidthRequest = 32,
				HeightRequest = 32,
				HorizontalOptions = LayoutOptions.End,
			};
			direction.SetBinding(DirectionArrow.DirectionProperty, GameMainCellViewModel.DirectionPropertyName);

			var distance = new Label() 
			{
				TextColor = App.Colors.Text,
				Font = Font.SystemFontOfSize(12),
				XAlign = TextAlignment.Center,
				HorizontalOptions = LayoutOptions.End,
			};
			distance.SetBinding(Label.TextProperty, GameMainCellViewModel.DistancePropertyName, BindingMode.OneWay, new ConverterToDistance());

			vertLayout.Children.Add(direction);
			vertLayout.Children.Add(distance);

			this.horiLayout.Children.Add(this.icon);
			this.horiLayout.Children.Add(this.name);
			this.horiLayout.Children.Add(vertLayout);

			View = this.horiLayout;
		}

		#endregion
	}
}
