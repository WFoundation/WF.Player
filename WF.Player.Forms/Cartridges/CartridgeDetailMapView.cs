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
using Xamarin.Forms.Maps;

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
	public class CartridgeDetailMapView : CartridgeDetailBasePage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeDetailDescriptionView"/> class.
		/// </summary>
		/// <param name="viewModel">View model.</param>
		public CartridgeDetailMapView(CartridgeDetailViewModel viewModel) : base(viewModel)
		{
			Title = Catalog.GetString("Map");

			this.DirectionView.SetBinding(DirectionArrow.DirectionProperty, CartridgeDetailViewModel.DirectionPropertyName);
			this.DistanceView.SetBinding(Label.TextProperty, CartridgeDetailViewModel.DistanceTextPropertyName, BindingMode.OneWay);

//			this.DirectionView.SetBinding(DirectionArrow.DirectionProperty, CartridgeDetailViewModel.DirectionPropertyName);
//			this.DistanceView.SetBinding(Label.TextProperty, CartridgeDetailViewModel.DistancePropertyName, BindingMode.OneWay, new ConverterToDistance());

			var mapViewModel = new MapViewModel();

			var mapView = new MapView(mapViewModel) 
				{
					Padding = 0,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			if (mapViewModel.Map.VisibleRegion == null)
			{
				if (App.GPS.LastKnownPosition != null)
				{
					mapViewModel.Map.VisibleRegion = MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(App.GPS.LastKnownPosition.Latitude, App.GPS.LastKnownPosition.Longitude), Xamarin.Forms.Maps.Distance.FromMeters(1000));
				}
				else
				{
					mapViewModel.Map.VisibleRegion = MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(0, 0), Xamarin.Forms.Maps.Distance.FromMeters(1000));
				}
			}

			if (!viewModel.IsPlayAnywhere)
			{
				mapViewModel.StartingLocation = viewModel.StartingLocation;
			}

			((StackLayout)ContentLayout).Children.Add(mapView);
		}

		#endregion
	}
}
