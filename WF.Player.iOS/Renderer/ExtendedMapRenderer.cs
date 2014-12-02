// <copyright file="ExtendedMapRenderer.cs" company="Wherigo Foundation">
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
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Maps;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using MonoTouch.CoreLocation;
using System.Collections.Generic;
using System.Drawing;
using WF.Player.Core;

[assembly: Xamarin.Forms.ExportRendererAttribute (typeof (WF.Player.Controls.ExtendedMap), typeof (WF.Player.Controls.iOS.ExtendedMapRenderer))]

namespace WF.Player.Controls.iOS
{
	using System;
	using MonoTouch.MapKit;
	using MonoTouch.UIKit;
	using Xamarin.Forms.Platform.iOS;

	public class ExtendedMapRenderer : ViewRenderer <ExtendedMap, MKMapView>
	{
		MKMapView nativeMap;
		MKPolygonRenderer polygonRenderer;
		List<MKPolygon> polygons;
		List<MKMapItem> polygonNames;
//		List<Android.Gms.Maps.Model.Marker> points;

		public ExtendedMapRenderer()
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ExtendedMap> e)
		{
			base.OnElementChanged(e);


			base.SetNativeControl(new MKMapView(RectangleF.Empty));

			ExtendedMap mapModel = (ExtendedMap)base.Element;
			MKMapView mkMapView = (MKMapView)base.Control;

			mkMapView.RegionChanged += delegate(object s, MKMapViewChangeEventArgs a)
				{
					if(this.Element == null)
					{
						return;
					}
					mapModel.VisibleRegion = new MapSpan(new Position(mkMapView.Region.Center.Latitude, mkMapView.Region.Center.Longitude), mkMapView.Region.Span.LatitudeDelta, mkMapView.Region.Span.LongitudeDelta);
				};

			MessagingCenter.Subscribe<Map, MapSpan>(this, "MapMoveToRegion", delegate(Map s, MapSpan a)
				{
					this.MoveToRegion(a);
				}, mapModel);

			if(mapModel.LastMoveToRegion != null)
			{
				this.MoveToRegion(mapModel.LastMoveToRegion);
			}

			this.UpdateMapType();
			this.UpdateIsShowingUser();
			this.UpdateHasScrollEnabled();
			this.UpdateHasZoomEnabled();

			((ObservableCollection<Pin>)mapModel.Pins).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);

			this.UpdatePins();

			mkMapView.Delegate = new MapDelegate();
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (base.Control == null)
			{
				return;
			}

			if(e.PropertyName == Map.MapTypeProperty.PropertyName)
			{
				this.UpdateMapType();
				return;
			}

			if(e.PropertyName == Map.IsShowingUserProperty.PropertyName)
			{
				this.UpdateIsShowingUser();
				return;
			}

			if(e.PropertyName == Map.HasScrollEnabledProperty.PropertyName)
			{
				this.UpdateHasScrollEnabled();
				return;
			}

			if(e.PropertyName == Map.HasZoomEnabledProperty.PropertyName)
			{
				this.UpdateHasZoomEnabled();
			}

			if (e.PropertyName == "Polygons")
			{
				MKMapView map = (MKMapView)base.Control;

				if (polygons != null)
				{
					foreach (var overlay in Control.Overlays)
					{
						if (overlay is MKPolygon && polygons.Contains((MKPolygon)overlay))
						{
							map.RemoveOverlay(overlay);
						}

					}

//					if (polygonNames is MKAnnotation && polygonNames.Contains((MKAnnotation)overlay))
//					{
//						map.RemoveOverlay(overlay);
//					}

					polygons = null;
					polygonNames = null;
				}

				if (((ExtendedMap)Element).Polygons == null)
				{
					return;
				}

				polygons = new List<MKPolygon>();
				polygonNames = new List<MKMapItem>();

				foreach (var z in ((ExtendedMap)Element).Polygons)
				{
					CLLocationCoordinate2D[] points = new CLLocationCoordinate2D[z.Points.Count];
					int i = 0;

					foreach (var p in z.Points)
					{
						points.SetValue(new CLLocationCoordinate2D(p.Latitude, p.Longitude), i++);
					}

					var polygon = MKPolygon.FromCoordinates(points);

					map.AddOverlay(polygon);

					polygons.Add(polygon);

//					var markerOptions = new MarkerOptions();
//
//					markerOptions.SetPosition(z.Label.Point.ToLatLng());
//					markerOptions.SetTitle(z.Label.Name);
//
//					polygonNames.Add(nativeMap.AddMarker(markerOptions));
				}
			}

			if (e.PropertyName == "Points")
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if(disposing)
			{
				Map map = (Map)base.Element;
				((ObservableCollection<Pin>)map.Pins).CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnCollectionChanged);
			}

			base.Dispose(disposing);
		}

		public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
		{
			return new SizeRequest(new Xamarin.Forms.Size(40, 40));
		}

		private void MoveToRegion(MapSpan mapSpan)
		{
			Position center = mapSpan.Center;
			MKCoordinateRegion region = new MKCoordinateRegion(new CLLocationCoordinate2D(center.Latitude, center.Longitude), new MKCoordinateSpan(mapSpan.LatitudeDegrees, mapSpan.LongitudeDegrees));

			((MKMapView)base.Control).SetRegion(region, true);
		}

		private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			this.UpdatePins();
		}

		private void UpdateHasScrollEnabled()
		{
			((MKMapView)base.Control).ScrollEnabled = ((Map)base.Element).HasScrollEnabled;
		}

		private void UpdateHasZoomEnabled()
		{
			((MKMapView)base.Control).ZoomEnabled = ((Map)base.Element).HasZoomEnabled;
		}

		private void UpdateIsShowingUser()
		{
			((MKMapView)base.Control).ShowsUserLocation = ((Map)base.Element).IsShowingUser;
		}

		private void UpdateMapType()
		{
			switch(((Map)base.Element).MapType)
			{
				case MapType.Street:
					((MKMapView)base.Control).MapType = MKMapType.Standard;
					return;
				case MapType.Satellite:
					((MKMapView)base.Control).MapType = MKMapType.Satellite;
					return;
				case MapType.Hybrid:
					((MKMapView)base.Control).MapType = MKMapType.Hybrid;
					return;
				default:
					return;
			}
		}

		private void UpdatePins()
		{
			((MKMapView)base.Control).RemoveAnnotations(((MKMapView)base.Control).Annotations);

			IList<Pin> pins = ((Map)base.Element).Pins;

			foreach(Pin current in pins)
			{
				((MKMapView)base.Control).AddAnnotation(new MKPointAnnotation {
					Coordinate = new CLLocationCoordinate2D(current.Position.Latitude, current.Position.Longitude),
					Title = current.Label,
					Subtitle = current.Address ?? string.Empty,
				});
			}
		}

		protected class MapDelegate : MKMapViewDelegate 
		{
			public override MKOverlayRenderer OverlayRenderer(MKMapView mapView, IMKOverlay overlay)
			{
				// NOTE: Don't call the base implementation on a Model class
				// see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events

				if (overlay is MKPolygon)
				{
					var polygon = (MKPolygon)overlay;

					var renderer = new MKPolygonRenderer(polygon)
						{ 
							LineWidth = 2,
							StrokeColor = UIColor.Red.ColorWithAlpha(0.7f),
							FillColor = UIColor.Red.ColorWithAlpha(0.3f),
						};

					return renderer;
				}

				return null;
			}
		}
	}
}

