// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz <mail@wfplayer.com>
//
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
using WF.Player.Services.Geolocation;
using MonoTouch.CoreLocation;
using MonoTouch.MapKit;
using MonoTouch.Foundation;

[assembly: Dependency(typeof(WF.Player.iOS.Services.Routing.NativeRouting))]

namespace WF.Player.iOS.Services.Routing
{
	public class NativeRouting : IRouting
	{
		public void StartRouting(string name, Position pos) 
		{
			CLLocationCoordinate2D coordinate = new CLLocationCoordinate2D(pos.Latitude, pos.Longitude);
			MKMapItem mapItemCartridgeLocation = new MKMapItem (new MKPlacemark (coordinate, (NSDictionary)null)) {
				Name = name,
			};

			// Current location
			MKMapItem mapItemCurrentLocation = MKMapItem.MapItemForCurrentLocation ();

			// Set coordinates from/to
			var mapItems = new MKMapItem[] { mapItemCurrentLocation, mapItemCartridgeLocation };

			// Call map to open with mode driving. Could also be Walking
			MKMapItem.OpenMaps (mapItems, new MKLaunchOptions () { DirectionsMode = MKDirectionsMode.Driving, });
		}
	}
}

