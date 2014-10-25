///
/// WF.Player.iPhone - A Wherigo Player for iPhone which use the Wherigo Foundation Core.
/// Copyright (C) 2012-2014  Dirk Weltz <web@weltz-online.de>
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
/// 
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
/// 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using MonoTouch.CoreLocation;
using MonoTouch.CoreGraphics;
using Google.Maps;
using Vernacular;
using WF.Player.Core;
using WF.Player.Core.Engines;
using WF.Player.Core.Utils;

namespace WF.Player.iOS
{
	public class ScreenMap : UIViewController
	{
//		float zoom = 16f;
//		bool headingOrientation = false;
//		Engine engine;
//		ScreenController ctrl;
//		Thing thing;
//		MapView mapView;
//		UrlTileLayer osmLayer;
//		UrlTileLayer ocmLayer;
//		UIButton btnCenter;
//		UIButton btnOrientation;
//		UIButton btnMapType;
//		UIWebView webLegacy;
//		Dictionary<int,Overlay> overlays = new Dictionary<int, Overlay> ();
//		Dictionary<int,Marker> markers = new Dictionary<int, Marker> ();
//		string[] properties = {"Name", "Icon", "Active", "Visible", "ObjectLocation"};
//
//		public ScreenMap (ScreenController ctrl, Thing t)
//		{
//			this.ctrl = ctrl;
//			this.engine = ctrl.Engine;
//			this.thing = t;
//
//			// OS specific details
//			if (new Version (UIDevice.CurrentDevice.SystemVersion) >= new Version(7,0)) 
//			{
//				// Code that uses features from Xamarin.iOS 7.0
//				this.EdgesForExtendedLayout = UIRectEdge.None;
//			}
//
//			// Create URLTileLayers
//			osmLayer = UrlTileLayer.FromUrlConstructor (
//				(uint x, uint y, uint zoom) => {
//					string url = String.Format("http://a.tile.openstreetmap.org/{0}/{1}/{2}.png", zoom, x, y);
//					return NSUrl.FromString(url);
//				}
//			); 
//			osmLayer.ZIndex = 0;
//
//			ocmLayer = UrlTileLayer.FromUrlConstructor (
//				(uint x, uint y, uint zoom) => {
//					string url = String.Format("http://c.tile.opencyclemap.org/cycle/{0}/{1}/{2}.png", zoom, x, y);
//					return NSUrl.FromString(url);
//				}
//			); 
//			ocmLayer.ZIndex = 0;
//		}
//		
//		public override void ViewDidLoad ()
//		{
//			base.ViewDidLoad ();
//
//			// Perform any additional setup after loading the view, typically from a nib.
//			NavigationItem.SetLeftBarButtonItem(new UIBarButtonItem(Catalog.GetString("Back"),UIBarButtonItemStyle.Plain, (sender,args) => { 
//				ctrl.ButtonPressed(null); 
//				ctrl.RemoveScreen(ScreenType.Map); 
//			}), false);
//			NavigationItem.LeftBarButtonItem.TintColor = Colors.NavBarButton;
//			NavigationItem.SetHidesBackButton(false, false);
//
//			// Get zoom factor
//			zoom = NSUserDefaults.StandardUserDefaults.FloatForKey("MapZoom");
//
//			// Get heading orientation
//			headingOrientation = NSUserDefaults.StandardUserDefaults.BoolForKey("HeadingOrientation");
//
//			if (zoom == 0f)
//				zoom = 16f;
//
//			// Create camera position
//			CameraPosition camera;
//			CoordBounds bounds;
//
//			if (thing != null && !(thing is Zone) && thing.ObjectLocation != null)
//				camera = CameraPosition.FromCamera( thing.ObjectLocation.Latitude, thing.ObjectLocation.Longitude, zoom);
//			else {
//				// Set camera to mylocation, perhaps there is no other position
//				camera = CameraPosition.FromCamera (engine.Latitude, engine.Longitude, zoom);
//				if (thing != null && thing is Zone) {
//					bounds = ((Zone)thing).Bounds;
//					if (bounds != null) {
//						camera = CameraPosition.FromCamera (bounds.Left + (bounds.Right - bounds.Left) / 2.0, bounds.Bottom + (bounds.Top - bounds.Bottom) / 2.0, zoom);
//					}
//				} else {
//					camera = CameraPosition.FromCamera (engine.Latitude, engine.Longitude, zoom);
//				}
//			}
//
//			// Init MapView
//			mapView = MapView.FromCamera (RectangleF.Empty, camera);
//			mapView.MyLocationEnabled = true;
//			mapView.SizeToFit ();
//			mapView.AutoresizingMask = UIViewAutoresizing.All;
//			mapView.Frame = new RectangleF (0, 0, View.Frame.Width, View.Frame.Height);
//			mapView.MyLocationEnabled = true;
//			mapView.Settings.CompassButton = false;
//			mapView.Settings.MyLocationButton = false;
//			mapView.Settings.RotateGestures = false;
//			mapView.Settings.TiltGestures = false;
//
//			mapView.TappedOverlay += OnTappedOverlay;
//			mapView.TappedInfo += OnTappedInfo;
//			mapView.CameraPositionChanged += OnCameraPositionChanged;
//
//			View.AddSubview(mapView);
//
//			if (thing == null) {
//				// Show all
//				bounds = engine.Bounds;
//				if (bounds != null) {
//					camera = mapView.CameraForBounds (new CoordinateBounds (new CLLocationCoordinate2D (bounds.Left, bounds.Top), new CLLocationCoordinate2D (bounds.Right, bounds.Bottom)), new UIEdgeInsets (30f, 30f, 30f, 30f));
//					mapView.Camera = camera;
//				}
//			}
//
//			btnCenter = UIButton.FromType (UIButtonType.RoundedRect);
//			btnCenter.Tag = 1;
//			btnCenter.Frame = new RectangleF (12f, 12f, 36f, 36f);
//			btnCenter.TintColor = UIColor.White;
//			btnCenter.SetBackgroundImage(Images.BlueButton, UIControlState.Normal);
//			btnCenter.SetBackgroundImage(Images.BlueButtonHighlight, UIControlState.Highlighted);
//			btnCenter.SetImage (Images.ButtonCenter, UIControlState.Normal);
//			btnCenter.ContentMode = UIViewContentMode.Center;
//			btnCenter.TouchUpInside += OnTouchUpInside;
//
//			View.AddSubview (btnCenter);
//
//			btnOrientation = UIButton.FromType (UIButtonType.RoundedRect);
//			btnOrientation.Tag = 2;
//			btnOrientation.Frame = new RectangleF (12f, 61f, 36f, 36f);
//			btnOrientation.TintColor = UIColor.White;
//			btnOrientation.SetBackgroundImage(Images.BlueButton, UIControlState.Normal);
//			btnOrientation.SetBackgroundImage(Images.BlueButtonHighlight, UIControlState.Highlighted);
//			btnOrientation.SetImage ((headingOrientation ? Images.ButtonOrientation : Images.ButtonOrientationNorth), UIControlState.Normal);
//			btnOrientation.ContentMode = UIViewContentMode.Center;
//			btnOrientation.TouchUpInside += OnTouchUpInside;
//
//			View.AddSubview (btnOrientation);
//
//			btnMapType = UIButton.FromType (UIButtonType.RoundedRect);
//			btnMapType.Tag = 3;
//			btnMapType.Frame = new RectangleF (mapView.Frame.Width - 12f - 36f, 12f, 36f, 36f);
//			btnMapType.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleBottomMargin;
//			btnMapType.TintColor = UIColor.White;
//			btnMapType.SetBackgroundImage(Images.BlueButton, UIControlState.Normal);
//			btnMapType.SetBackgroundImage(Images.BlueButtonHighlight, UIControlState.Highlighted);
//			btnMapType.SetImage (Images.ButtonMapType, UIControlState.Normal);
//			btnMapType.ContentMode = UIViewContentMode.Center;
//			btnMapType.TouchUpInside += OnTouchUpInside;
//
//			View.AddSubview (btnMapType);
//
//			webLegacy = new UIWebView();
//			webLegacy.Frame = new RectangleF (mapView.Frame.Width - 2f - 150f, mapView.Frame.Height - 2f - 20f, 150f, 20f);
//			webLegacy.AutoresizingMask = UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;
//			webLegacy.BackgroundColor = UIColor.Clear;
//			webLegacy.Opaque = false;
//			webLegacy.ScrollView.ScrollEnabled = false;
//			webLegacy.ScalesPageToFit = true;
//			webLegacy.ShouldStartLoad = delegate (UIWebView webView, NSUrlRequest request, UIWebViewNavigationType navigationType) {
//				if (navigationType == UIWebViewNavigationType.LinkClicked) {
//					UIApplication.SharedApplication.OpenUrl(request.Url);
//					return false;
//				}
//				return true;
//			};
//
//			View.AddSubview (webLegacy);
//
//			// Set map source
//			SetMapSource((MapSource)NSUserDefaults.StandardUserDefaults.IntForKey("MapSource"));
//
//			Refresh ();
//		}
//
//		public override void ViewWillAppear (bool animated)
//		{
//			base.ViewWillAppear (animated);
//
//			mapView.StartRendering ();
//			StartEvents ();
//		}
//
//		public override void ViewWillDisappear (bool animated)
//		{	
//			StopEvents ();
//			mapView.StopRendering ();
//
//			// Save zoom factor
//			NSUserDefaults.StandardUserDefaults.SetFloat(mapView.Camera.Zoom, "MapZoom");
//
//			base.ViewWillDisappear (animated);
//		}
//
//		public void Refresh(Thing refreshThing = null)
//		{
//			if (refreshThing != null) {
//				// Only one thing needs an update
//
//			} else {
//				// All things must be updated
//
//				// All zones
//				WherigoCollection<Zone> zones = ctrl.Engine.ActiveVisibleZones;
//
//				foreach (Zone z in zones)
//					CreateZone (z);
//
//				// All items
//				WherigoCollection<Thing> things = ctrl.Engine.VisibleObjects;
//
//				foreach (Thing t in things)
//					CreateThing(t);//					createThing (t);
//			}
//
//			if (thing != null)
//				NavigationItem.Title = thing.Name;
//		}
//
//		#region Private Functions
//
//		void OnCameraPositionChanged (object sender, GMSCameraEventArgs e)
//		{
//			float maxZoom = Google.Maps.Constants.MaxZoomLevel;
//
//			if (osmLayer.Map != null || ocmLayer.Map != null)
//				maxZoom = 17.3f;
//
//			if (e.Position.Zoom > maxZoom)
//				mapView.MoveCamera(CameraUpdate.ZoomToZoom(maxZoom));
//		}
//
//		void OnTappedOverlay (object sender, GMSOverlayEventEventArgs e)
//		{
//			var objIndex = overlays.FirstOrDefault(x => x.Value == e.Overlay).Key;
//		}
//
//		void OnTappedInfo (object sender, GMSMarkerEventEventArgs e)
//		{
//			var obj = engine.GetWherigoObject<Thing> (markers.FirstOrDefault (x => x.Value == e.Marker).Key);
//
//			if (ctrl.activeScreen == ScreenType.Details && ctrl.activeObject == obj) {
//				ctrl.RemoveScreen (ScreenType.Details);
//				ctrl.ShowScreen(ScreenType.Details, obj); 
//			} else
//				ctrl.ShowScreen(ScreenType.Details, obj); 
//		}
//
//		void OnTouchUpInside (object sender, EventArgs e)
//		{
//			ctrl.ButtonPressed (null);
//
//			if (sender is UIButton && ((UIButton)sender).Tag == 1) {
//				// Ask, which to show
//				var thingOffset = 1;
//				UIActionSheet actionSheet = new UIActionSheet (Catalog.GetString ("Focus on"));
//				if (thing != null) {
//					actionSheet.AddButton (thing.Name);
//					thingOffset = 0;
//				}
//				actionSheet.AddButton (Catalog.GetString ("Playing area"));
//				actionSheet.AddButton (Catalog.GetString ("Location"));
//				actionSheet.AddButton (Catalog.GetString ("Cancel"));
//				actionSheet.CancelButtonIndex = 3 - thingOffset;       // Black button
//				actionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
//					CameraUpdate cu = null;
//					if (b.ButtonIndex == 0 - thingOffset) {
//						// Location of thing is selected and thing is a zone
//						if (thing is Zone) {
//							var bounds = ((Zone)thing).Bounds;
////								cu = CameraUpdate.FitBounds(new CoordinateBounds(new CLLocationCoordinate2D(lat1, lon1),new CLLocationCoordinate2D(lat2, lon2)),30f);
//							cu = CameraUpdate.SetTarget(new CLLocationCoordinate2D(bounds.Left + (bounds.Right - bounds.Left) / 2.0, bounds.Bottom + (bounds.Top - bounds.Bottom) / 2.0));
//						} else {
//							// Location of thing is selected and thing is no zone
//							if (thing.ObjectLocation != null) {
//								cu = CameraUpdate.SetTarget(new CLLocationCoordinate2D(thing.ObjectLocation.Latitude,thing.ObjectLocation.Longitude));
//							}
//						}
//					}
//					if (b.ButtonIndex == 1 - thingOffset) {
//						var bounds = engine.Bounds;
//						if (bounds != null)
//							cu = CameraUpdate.FitBounds(new CoordinateBounds(new CLLocationCoordinate2D(bounds.Left, bounds.Top),new CLLocationCoordinate2D(bounds.Right, bounds.Bottom)),30f);
//					}
//					if (b.ButtonIndex == 2 - thingOffset) {
//						// Location of player is selected
//						cu = CameraUpdate.SetTarget(new CLLocationCoordinate2D(engine.Latitude,engine.Longitude));
//					}
//					if (cu != null)
//						mapView.MoveCamera(cu);
//				};
//				actionSheet.ShowInView (View);
//			}
//			if  (sender is UIButton && ((UIButton)sender).Tag == 2) {
//				// Check, if north should be on top
//				headingOrientation = !headingOrientation;
//				NSUserDefaults.StandardUserDefaults.SetBool (headingOrientation, "HeadingOrientation");
//
//				if (headingOrientation) {
//					((UIButton)sender).SetImage (Images.ButtonOrientation, UIControlState.Normal);
//					ctrl.LocatitionManager.UpdatedHeading += OnUpdateHeading;
//				} else {
//					((UIButton)sender).SetImage (Images.ButtonOrientationNorth, UIControlState.Normal);
//					ctrl.LocatitionManager.UpdatedHeading -= OnUpdateHeading;
//					mapView.AnimateToBearing (0.0);
//				}
//			}
//			if  (sender is UIButton && ((UIButton)sender).Tag == 3) {
//				// Change map type
//				// Ask, which to show
//				UIActionSheet actionSheet = new UIActionSheet (Catalog.GetString ("Type of map"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.Normal ? Strings.Checked + " " : "") + Catalog.GetString ("Google Maps"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.Satellite ? Strings.Checked + " " : "") + Catalog.GetString ("Google Satellite"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.Terrain ? Strings.Checked + " " : "") + Catalog.GetString ("Google Terrain"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.Hybrid ? Strings.Checked + " " : "") + Catalog.GetString ("Google Hybrid"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.None && osmLayer.Map != null ? Strings.Checked + " " : "") + Catalog.GetString ("OpenStreetMap"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.None && ocmLayer.Map != null ? Strings.Checked + " " : "") + Catalog.GetString ("OpenCycleMap"));
//				actionSheet.AddButton ((mapView.MapType == MapViewType.None && osmLayer.Map == null && ocmLayer.Map == null ? Strings.Checked + " " : "") + Catalog.GetString ("None"));
//				actionSheet.AddButton (Catalog.GetString ("Cancel"));
//				actionSheet.CancelButtonIndex = 7;       // Black button
//				actionSheet.Clicked += delegate(object a, UIButtonEventArgs b) {
//					switch (b.ButtonIndex) {
//					case 0:
//						SetMapSource(MapSource.GoogleMaps);
//						break;
//					case 1:
//						SetMapSource(MapSource.GoogleSatellite);
//						break;
//					case 2:
//						SetMapSource(MapSource.GoogleTerrain);
//						break;
//					case 3:
//						SetMapSource(MapSource.GoogleHybrid);
//						break;
//					case 4:
//						// OpenStreetMap
//						SetMapSource(MapSource.OpenStreetMap);
//						break;
//					case 5:
//						// OpenCycleMap
//						SetMapSource(MapSource.OpenCycleMap);
//						break;
//					case 6:
//						SetMapSource(MapSource.None);
//						break;
//					}
//				};
//				actionSheet.ShowInView (View);
//			}
//		}
//
//		void StartEvents()
//		{
//			engine.AttributeChanged += OnAttributeChanged;
//			engine.InventoryChanged += OnPropertyChanged;
//			engine.ZoneStateChanged += OnZoneStateChanged;
//			engine.PropertyChanged += OnPropertyChanged;
//			if (headingOrientation)
//				ctrl.LocatitionManager.UpdatedHeading += OnUpdateHeading;
//		}
//
//		void StopEvents()
//		{
//			engine.AttributeChanged -= OnAttributeChanged;
//			engine.InventoryChanged -= OnPropertyChanged;
//			engine.ZoneStateChanged -= OnZoneStateChanged;
//			engine.PropertyChanged -= OnPropertyChanged;
//			if (headingOrientation)
//				ctrl.LocatitionManager.UpdatedHeading -= OnUpdateHeading;
//		}
//
//		public void OnPropertyChanged(object sender,  EventArgs e)
//		{
//			bool newItems = false;
//
//			newItems |= e is InventoryChangedEventArgs;
//			newItems |= e is AttributeChangedEventArgs && ((AttributeChangedEventArgs)e).PropertyName.Equals("Active");
//			newItems |= e is AttributeChangedEventArgs && ((AttributeChangedEventArgs)e).PropertyName.Equals("Visible");
//			newItems |= e is PropertyChangedEventArgs && ((PropertyChangedEventArgs)e).PropertyName.Equals("Active");
//			newItems |= e is PropertyChangedEventArgs && ((PropertyChangedEventArgs)e).PropertyName.Equals("Visible");
//
//			// Check, if one of the visible entries changed
////			if (!(e is PropertyChangedEventArgs) || (e is PropertyChangedEventArgs && properties.Contains(((PropertyChangedEventArgs)e).PropertyName)))
////				Refresh(((PropertyChangedEventArgs)e).e.newItems);
//		}
//
//		public void OnAttributeChanged(object sender,  AttributeChangedEventArgs e)
//		{
//			if (properties.Contains(e.PropertyName)) {
//				if (e.Object is Zone)
//					CreateZone(e.Object as Zone);
//			}
//		}
//
//		public void OnUpdateHeading (object sender, CLHeadingUpdatedEventArgs e)
//		{
//			if (headingOrientation)
//				mapView.AnimateToBearing(e.NewHeading.MagneticHeading);
//		}
//
//		public void OnZoneStateChanged(object sender,  ZoneStateChangedEventArgs e)
//		{
//			foreach (Zone z in e.Zones) {
//				if (z.Active && z.Visible) {
//					CreateZone (z);
//				} else {
//					if (overlays.ContainsKey (z.ObjIndex)) {
//						Overlay polygon;
//						overlays.TryGetValue (z.ObjIndex, out polygon);
//						if (polygon != null) 
//							polygon.Map = null;
//						overlays.Remove (z.ObjIndex);
//						Marker marker;
//						markers.TryGetValue (z.ObjIndex, out marker);
//						if (marker != null)
//							marker.Map = null;
//						markers.Remove (z.ObjIndex);
//					}
//				}
//			}
//		}
//
//		void SetMapSource (MapSource ms)
//		{
//			switch (ms) {
//			case MapSource.GoogleMaps:
//				mapView.MapType = MapViewType.Normal;
//				webLegacy.LoadHtmlString("",new NSUrl(""));
//				osmLayer.Map = null;
//				ocmLayer.Map = null;
//				break;
//			case MapSource.GoogleSatellite:
//				mapView.MapType = MapViewType.Satellite;
//				webLegacy.LoadHtmlString("",new NSUrl(""));
//				osmLayer.Map = null;
//				ocmLayer.Map = null;
//				break;
//			case MapSource.GoogleTerrain:
//				mapView.MapType = MapViewType.Terrain;
//				webLegacy.LoadHtmlString("",null);
//				osmLayer.Map = null;
//				ocmLayer.Map = null;
//				break;
//			case MapSource.GoogleHybrid:
//				mapView.MapType = MapViewType.Hybrid;
//				webLegacy.LoadHtmlString("",null);
//				osmLayer.Map = null;
//				ocmLayer.Map = null;
//				break;
//			case MapSource.OpenStreetMap:
//				// OpenStreetMap
//				mapView.MapType = MapViewType.None;
//				webLegacy.LoadHtmlString("<html><head></head><body style=\"font-family:sans-serif; margin:0 auto;text-align:left;background-color: transparent; color:#808080 \"><hfill>Data, imagery and map information provided by<br>MapQuest, <a href=\"http://www.openstreetmap.org/copyright\">OpenStreetMap</a> and contributors, <a href=\"http://wiki.openstreetmap.org/wiki/Legal_FAQ#I_would_like_to_use_OpenStreetMap_maps._How_should_I_credit_you.#\">ODbL</a></body></html>",new NSUrl(""));
//				webLegacy.SizeToFit();
//				osmLayer.Map = mapView;
//				ocmLayer.Map = null;
//				break;
//			case MapSource.OpenCycleMap:
//				// OpenCycleMap
//				mapView.MapType = MapViewType.None;
//				webLegacy.LoadHtmlString ("<html><head></head><body style=\"font-family:sans-serif; margin:0 auto;text-align:left;background-color: transparent; color:#808080 \"><hfill>Data, imagery and map information provided by<br>MapQuest, <a href=\"http://www.openstreetmap.org/copyright\">OpenStreetMap</a> and contributors, <a href=\"http://wiki.openstreetmap.org/wiki/Legal_FAQ#I_would_like_to_use_OpenStreetMap_maps._How_should_I_credit_you.#\">ODbL</a></body></html>", new NSUrl (""));
//				webLegacy.SizeToFit();
//				osmLayer.Map = null;
//				ocmLayer.Map = mapView;
//				break;
//			case MapSource.None:
//				mapView.MapType = MapViewType.None;
//				webLegacy.LoadHtmlString ("", new NSUrl (""));
//				osmLayer.Map = null;
//				ocmLayer.Map = null;
//				break;
//			}
//
//			// Save map source
//			NSUserDefaults.StandardUserDefaults.SetInt((int)ms, "MapSource");
//		}
//
//		void CreateThing (Thing t)
//		{
//			Marker marker;
//
//			// If the thing don't have a ObjectLocation, than don't draw it
//			if (t.ObjectLocation == null)
//				return;
//
//			if (!markers.TryGetValue (t.ObjIndex, out marker)) {
//				marker = new Marker () {
//					Tappable = true,
//					Map = mapView
//				};
//				if (thing is Character) {
//					marker.Icon = t.Icon != null && t.Icon == null ? UIImage.LoadFromData (NSData.FromArray (t.Icon.Data)) : Images.IconMapCharacter;
//					marker.GroundAnchor = t.Icon != null && t.Icon == null ? new PointF (0.5f, 0.5f) : new PointF (0.3f, 0.92f);
//					marker.InfoWindowAnchor = t.Icon != null && t.Icon == null ? new PointF (0.5f, 0.0f) : new PointF (0.3f, 0.0f);
//				} else {
//					marker.Icon = t.Icon != null && t.Icon == null ? UIImage.LoadFromData (NSData.FromArray (t.Icon.Data)) : Images.IconMapItem;
//					marker.GroundAnchor = t.Icon != null && t.Icon == null ? new PointF (0.5f, 0.5f) : new PointF (0.2f, 0.92f);
//					marker.InfoWindowAnchor = t.Icon != null && t.Icon == null ? new PointF (0.5f, 0.0f) : new PointF (0.2f, 0.0f);
//				}
//				markers.Add(t.ObjIndex, marker);
//			}
//			marker.Title = (t.Name == null ? "" : t.Name);
//
//			var inventory = (WherigoCollection<Thing>)t.Inventory;
//
//			if (inventory.Count > 0) {
//				StringBuilder s = new StringBuilder ();
//
//				foreach (Thing thing in inventory) {
//					s.Append ((s.Length > 0 ? ", " : "") + (thing.Name == null ? "" : thing.Name));
//				}
//
//				marker.Snippet = Catalog.Format(Catalog.GetString("Contains {0}"), s.ToString());
//			}
//
//			marker.ZIndex = 100;
//			((Marker)marker).Position = new CLLocationCoordinate2D(t.ObjectLocation.Latitude, t.ObjectLocation.Longitude);
//		}
//
//		void CreateZone(Zone z)
//		{
//			Overlay polygon;
//			Marker marker;
//
//			if (!z.Active || !z.Visible) {
//				if (overlays.ContainsKey (z.ObjIndex)) {
//					overlays.TryGetValue (z.ObjIndex, out polygon);
//					if (polygon != null) 
//						polygon.Map = null;
//					overlays.Remove (z.ObjIndex);
//					markers.TryGetValue (z.ObjIndex, out marker);
//					if (marker != null)
//						marker.Map = null;
//					markers.Remove (z.ObjIndex);
//				}
//				return;
//			}
//
//			if (!overlays.TryGetValue(z.ObjIndex, out polygon)) {
//				polygon = new Polygon () {
//					FillColor = Colors.ZoneFill,
//					StrokeColor = Colors.ZoneStroke,
//					StrokeWidth = 2,
//					Tappable = true,
//					Map = mapView
//				};
//				overlays.Add (z.ObjIndex, polygon);
//			}
//
//			if (!markers.TryGetValue(z.ObjIndex, out marker)) {
//				marker = new Marker () {
//					Tappable = true,
//					Icon = (z.Icon != null ? UIImage.LoadFromData (NSData.FromArray (z.Icon.Data)) : Images.IconMapZone),
//					GroundAnchor = z.Icon != null ? new PointF(0.5f, 0.5f) : new PointF(0.075f, 0.95f),
//					InfoWindowAnchor = z.Icon != null ? new PointF(0.5f, 0.5f) : new PointF(0.075f, 0.0f),
//					Map = mapView
//				};
//				markers.Add (z.ObjIndex, marker);
//			}
//
//			polygon.Title = z.Name;
//			marker.Title = z.Name;
//
//			var inventory = (WherigoCollection<Thing>)z.Inventory;
//
//			if (inventory.Count > 0) {
//				StringBuilder s = new StringBuilder ();
//
//				foreach (Thing thing in inventory) {
//					s.Append ((s.Length > 0 ? ", " : "") + (thing.Name == null ? "" : thing.Name));
//				}
//
//				marker.Snippet = Catalog.Format(Catalog.GetString("Contains {0}"), s.ToString());
//			}
//				
//			MutablePath path = new MutablePath ();;
//			WherigoCollection<ZonePoint> points = z.Points;
//
//			double lat = 0;
//			double lon = 0;
//
//			foreach (ZonePoint zp in points) {
//				lat += zp.Latitude;
//				lon += zp.Longitude;
//				path.AddLatLon (zp.Latitude, zp.Longitude);
//			}
//
//			((Polygon)polygon).Path = path;
//			polygon.ZIndex = 50;
//
//			marker.Position = new CLLocationCoordinate2D ((float)lat / (float)points.Count, (float)lon / (float)points.Count);
//			marker.ZIndex = 100;
//		}
//
//		#endregion
//
//	}
	}
}