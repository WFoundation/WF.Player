// <copyright file="ExtendedMapViewRenderer.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.ExportRendererAttribute (typeof (WF.Player.Controls.ExtendedMap), typeof (WF.Player.Controls.Droid.ExtendedMapRenderer))]

namespace WF.Player.Controls.Droid
{
	using System;
	using System.Collections.Generic;
	using Xamarin.Forms.Maps.Android;
	using Android.Gms.Maps;
	using Android.Gms.Maps.Model;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;

	public class ExtendedMapRenderer : MapRenderer
	{
		Android.Gms.Maps.GoogleMap nativeMap;
		List<Android.Gms.Maps.Model.Polygon> polygons;
		List<Android.Gms.Maps.Model.Marker> polygonNames;
		List<Android.Gms.Maps.Model.Marker> points;

		public ExtendedMapRenderer()
		{
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Xamarin.Forms.View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				// First time init
				nativeMap = ((Android.Gms.Maps.MapView)Control).Map;

				nativeMap.MyLocationEnabled = true;
				nativeMap.UiSettings.ZoomControlsEnabled = false;
				nativeMap.UiSettings.MyLocationButtonEnabled = false;
				nativeMap.UiSettings.ScrollGesturesEnabled = true;
				nativeMap.UiSettings.ZoomGesturesEnabled = true;
				nativeMap.UiSettings.CompassEnabled = true;
				nativeMap.UiSettings.MapToolbarEnabled = false;
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (nativeMap == null)
			{
				return;
			}

			if (e.PropertyName == "MapOrientation")
			{
				UpdateMapOrientation();
			}

			if (e.PropertyName == "Polygons")
			{
				if (polygons != null)
				{
					foreach (var polygon in polygons)
					{
						polygon.Remove();
					}

					foreach (var polygonName in polygonNames)
					{
						polygonName.Remove();
					}

					polygons = null;
					polygonNames = null;
				}

				if (((ExtendedMap)Element).Polygons == null)
				{
					return;
				}

				polygons = new List<Polygon>();
				polygonNames = new List<Marker>();

				foreach (var z in ((ExtendedMap)Element).Polygons)
				{
					var polygonOptions = new Android.Gms.Maps.Model.PolygonOptions();

					polygonOptions.InvokeStrokeWidth(2f);
					polygonOptions.InvokeStrokeColor(Color.Red.MultiplyAlpha(0.7).ToAndroid());
					polygonOptions.InvokeFillColor(Color.Red.MultiplyAlpha(0.3).ToAndroid());

					foreach (var p in z.Points)
					{
						polygonOptions.Add(p.ToLatLng());
					}

					polygons.Add(nativeMap.AddPolygon(polygonOptions));

					var markerOptions = new MarkerOptions();

					markerOptions.SetPosition(z.Label.Point.ToLatLng());
					markerOptions.SetTitle(z.Label.Name);

					polygonNames.Add(nativeMap.AddMarker(markerOptions));
				}
			}

			if (e.PropertyName == "Points")
			{
			}
		}

		private void UpdateMapOrientation()
		{
			switch(((ExtendedMap)base.Element).MapOrientation)
			{
				case MapOrientation.NorthUp:
//					((MapView)base.Control).Or.UserTrackingMode = MKUserTrackingMode.None;
					return;
				case MapOrientation.HeadingUp:
//					((MapView)base.Control).UserTrackingMode = MKUserTrackingMode.FollowWithHeading;
					return;
				default:
					return;
			}
		}


	}
}

