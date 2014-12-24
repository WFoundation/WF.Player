// <copyright file="CartridgeDetailViewModel.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using System;
	using System.IO;
	using Vernacular;
	using WF.Player.Core;
	using WF.Player.Core.Utils;
	using WF.Player.Models;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;

	/// <summary>
	/// Cartridge detail view model.
	/// </summary>
	public class CartridgeDetailViewModel : BaseViewModel
	{
		/// <summary>
		/// The name of the name property.
		/// </summary>
		public const string NamePropertyName = "Name";

		/// <summary>
		/// The name of the description property.
		/// </summary>
		public const string DescriptionPropertyName = "Description";

		/// <summary>
		/// The name of the author property.
		/// </summary>
		public const string AuthorPropertyName = "Author";

		/// <summary>
		/// The name of the is play anywhere property.
		/// </summary>
		public const string IsPlayAnywherePropertyName = "IsPlayAnywhere";

		/// <summary>
		/// The name of the poster source property.
		/// </summary>
		public const string PosterSourcePropertyName = "PosterSource";

		/// <summary>
		/// The name of the has poster property.
		/// </summary>
		public const string HasPosterPropertyName = "HasPoster";

		/// <summary>
		/// The name of the direction property.
		/// </summary>
		public const string DirectionPropertyName = "Direction";

		/// <summary>
		/// The name of the distance property.
		/// </summary>
		public const string DistancePropertyName = "Distance";

		/// <summary>
		/// The name of the distance text property.
		/// </summary>
		public const string DistanceTextPropertyName = "DistanceText";

		/// <summary>
		/// The name of the map view model property.
		/// </summary>
		public const string MapViewModelPropertyName = "MapViewModel";

		/// <summary>
		/// The name of the routing command property.
		/// </summary>
		public const string RoutingCommandPropertyName = "RoutingCommand";

		/// <summary>
		/// The name of the resume command property.
		/// </summary>
		public const string ResumeCommandPropertyName = "ResumeCommand";

		/// <summary>
		/// The start name of the command property.
		/// </summary>
		public const string StartCommandPropertyName = "StartCommand";

		/// <summary>
		/// The cartridge.
		/// </summary>
		private CartridgeTag cartridgeTag;

		/// <summary>
		/// The starting location of cartridge.
		/// </summary>
		private ZonePoint target;

		/// <summary>
		/// The geo math helper.
		/// </summary>
		private GeoMathHelper geoMathHelper;

		/// <summary>
		/// The direction.
		/// </summary>
		private double direction;

		/// <summary>
		/// The distance.
		/// </summary>
		private double distance;

		/// <summary>
		/// The map view model.
		/// </summary>
		private MapViewModel mapViewModel;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeDetailViewModel"/> class.
		/// </summary>
		/// <param name="tag">CartridgeTag to show.</param>
		public CartridgeDetailViewModel(CartridgeTag tag)
		{
			this.cartridgeTag = tag;

			this.geoMathHelper = new GeoMathHelper();
			this.target = new ZonePoint(cartridgeTag.Cartridge.StartingLocationLatitude, cartridgeTag.Cartridge.StartingLocationLongitude, 0);

			Direction = double.NaN;
			Distance = double.NaN;

			if (App.GPS.LastKnownPosition != null)
			{
				HandlePositionChanged(this, new PositionEventArgs(App.GPS.LastKnownPosition));
			}
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the cartridge name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{
			get
			{
				return cartridgeTag.Cartridge.Name;
			}
		}

		/// <summary>
		/// Gets the cartridge long description.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get
			{
				return string.IsNullOrEmpty(cartridgeTag.Cartridge.LongDescription) ? Catalog.GetString("No description available") : cartridgeTag.Cartridge.LongDescription;
			}
		}

		/// <summary>
		/// Gets the full author author (name and comapny) with a trailing "By ".
		/// </summary>
		/// <value>The author.</value>
		public string Author
		{
			get
			{
				var author = cartridgeTag.Cartridge.AuthorName + ((!string.IsNullOrWhiteSpace(cartridgeTag.Cartridge.AuthorName) & !string.IsNullOrWhiteSpace(cartridgeTag.Cartridge.AuthorCompany)) ? " / " : string.Empty) + cartridgeTag.Cartridge.AuthorCompany;
				author = string.IsNullOrWhiteSpace(author) ? string.Empty : (Catalog.GetString("By") + " " + author);

				return author;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is play anywhere.
		/// </summary>
		/// <value><c>true</c> if this instance is play anywhere; otherwise, <c>false</c>.</value>
		public bool IsPlayAnywhere
		{
			get
			{
				return cartridgeTag.Cartridge.IsPlayAnywhere;
			}
		}

		/// <summary>
		/// Gets the poster of cartridge.
		/// </summary>
		/// <value>The poster.</value>
		public ImageSource PosterSource
		{
			get
			{
				if (!HasPoster)
				{
					return null;
				}

				return ImageSource.FromStream(() =>
					{
						return new MemoryStream(cartridgeTag.Cartridge.Poster.Data);
					});
			}
		}

		/// <summary>
		/// Gets a value indicating whether this cartridge has poster.
		/// </summary>
		/// <value><c>true</c> if this cartridge has poster; otherwise, <c>false</c>.</value>
		public bool HasPoster
		{ 
			get
			{ 
				return cartridgeTag.Cartridge.Poster != null && cartridgeTag.Cartridge.Poster.Data != null; 
			}
		}

		/// <summary>
		/// Gets the direction from the actuell location to the cartridge.
		/// </summary>
		/// <value>The distance.</value>
		public double Direction
		{
			get
			{
				return direction;
			}

			internal set
			{
				SetProperty<double>(ref direction, value, DirectionPropertyName);
			}
		}

		/// <summary>
		/// Gets the distance from the actuell location to the cartridge.
		/// </summary>
		/// <value>The distance.</value>
		public double Distance
		{
			get
			{
				return distance;
			}

			internal set
			{
				SetProperty<double>(ref distance, value, DistancePropertyName);
				NotifyPropertyChanged(DistanceTextPropertyName);
			}
		}

		/// <summary>
		/// Gets the distance text from the actuell location to the cartridge.
		/// </summary>
		/// <value>The distance.</value>
		public string DistanceText
		{
			get
			{
				if (!double.IsNaN(Direction))
				{
					return 	Converter.NumberToBestLength(Distance);
				}
				else
				{
					return Catalog.GetString("Unknown");
				}
			}
		}

		public MapViewModel MapViewModel
		{
			get
			{
				return mapViewModel;
			}

			set
			{
				SetProperty<MapViewModel>(ref mapViewModel, value, MapViewModelPropertyName);
			}
		}

		#endregion

		#region Commands

		#region Routing Command

		/// <summary>
		/// Gets the routing command.
		/// </summary>
		/// <value>The routinThe name of the routing command property.g command.</value>
		public Xamarin.Forms.Command RoutingCommand
		{
			get
			{
				return new Xamarin.Forms.Command(() =>
					{
						// Cartridge location
						var routing = DependencyService.Get<IRouting>();

						routing.StartRouting(cartridgeTag.Cartridge.Name, new Position(cartridgeTag.Cartridge.StartingLocationLatitude, cartridgeTag.Cartridge.StartingLocationLongitude));
					});
			}
		}

		#endregion

		#region Resume Command

		/// <summary>
		/// Gets the resume command.
		/// </summary>
		/// <value>The resume command.</value>
		public Xamarin.Forms.Command ResumeCommand
		{
			get
			{
				return new Xamarin.Forms.Command((sender) =>
					{
						CartridgeSavegame cs = null;

						App.Navigation.PushAsync(new GameCheckLocationView(new GameCheckLocationViewModel(cartridgeTag, cs, App.Navigation.CurrentPage)));
					});
			}
		}

		#endregion

		#region Start Command

		/// <summary>
		/// Gets the start command.
		/// </summary>
		/// <value>The start command.</value>
		public Xamarin.Forms.Command StartCommand
		{
			get
			{
				return new Xamarin.Forms.Command((sender) =>
					{
						App.Navigation.PushAsync(new GameCheckLocationView(new GameCheckLocationViewModel(cartridgeTag, null, App.Navigation.CurrentPage)));
					});
			}
		}

		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		public override void OnAppearing()
		{
			App.GPS.PositionChanged += HandlePositionChanged;
			App.GPS.HeadingChanged += HandlePositionChanged;
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		public override void OnDisappearing()
		{
			App.GPS.HeadingChanged -= HandlePositionChanged;
			App.GPS.PositionChanged -= HandlePositionChanged;
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Handles the position changed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position event arguments.</param>
		private void HandlePositionChanged(object sender, WF.Player.Services.Geolocation.PositionEventArgs e)
		{
			double heading = 0;

			if (e.Position.Heading != null)
			{
				// Show always to north
				heading = 360.0 - (double)e.Position.Heading;
			}

			var vec = geoMathHelper.VectorToPoint(new ZonePoint(e.Position.Latitude, e.Position.Longitude, 0), target);

			Direction = (double)((vec.Bearing + heading) % 360);
			Distance = vec.Distance.Value;
		}

		#endregion
	}
}
