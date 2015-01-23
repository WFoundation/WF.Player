// <copyright file="CartridgeDetailDescriptionView.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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
	using System;
	using Vernacular;
	using WF.Player.Controls;
	using WF.Player.Core;
	using Xamarin.Forms;

	/// <summary>
	/// Cartridge detail description view.
	/// </summary>
	public class CartridgeDetailDescriptionView : CartridgeDetailBasePage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeDetailDescriptionView"/> class.
		/// </summary>
		/// <param name="viewModel">View model.</param>
		public CartridgeDetailDescriptionView(CartridgeDetailViewModel viewModel) : base(viewModel)
		{
			Title = Catalog.GetString("Description");

			this.DirectionView.SetBinding(DirectionArrow.DirectionProperty, CartridgeDetailViewModel.DirectionPropertyName);
			this.DistanceView.SetBinding(Label.TextProperty, CartridgeDetailViewModel.DistanceTextPropertyName, BindingMode.OneWay);

			var layoutScroll = new PinchScrollView() 
				{
					Orientation = ScrollOrientation.Vertical,
					Padding = 0,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			// Used for padding around the label
			var layout = new StackLayout() 
				{
					Padding = 10,
				};

			var label = new Label() 
				{
					XAlign = Settings.TextAlignment,
					FontSize = Settings.FontSize,
					FontFamily = Settings.FontFamily,
					TextColor = App.Colors.Text,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			label.SetBinding(Label.TextProperty, CartridgeDetailViewModel.DescriptionPropertyName);

			layout.Children.Add(label);

			layoutScroll.Content = layout;

			((StackLayout)ContentLayout).Children.Add(layoutScroll);
		}

		#endregion
	}
}
