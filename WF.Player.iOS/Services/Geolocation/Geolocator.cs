//
// Copyright 2011-2013, Xamarin Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System;
using MonoTouch.CoreLocation;
using System.Threading.Tasks;
using System.Threading;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using WF.Player.Services.Geolocation;
using Vernacular;

[assembly: Dependency(typeof(WF.Player.iOS.Services.Geolocation.Geolocator))]

namespace WF.Player.iOS.Services.Geolocation
{
	public class Geolocator : IGeolocator
	{
		UIDeviceOrientation? _orientation;
		NSObject _observer;

		public Geolocator()
		{
			this.manager = GetManager();
			this.manager.AuthorizationChanged += OnAuthorizationChanged;
			this.manager.Failed += OnFailed;

//			if (UIDevice.CurrentDevice.CheckSystemVersion (6, 0))
				this.manager.LocationsUpdated += OnLocationsUpdated;
//			else
//				this.manager.UpdatedLocation += OnUpdatedLocation;

			this.manager.UpdatedHeading += OnUpdatedHeading;
		}

		public event EventHandler<PositionErrorEventArgs> PositionError;

		public event EventHandler<PositionEventArgs> PositionChanged;

		public event EventHandler<PositionEventArgs> HeadingChanged;

		public double DesiredAccuracy
		{
			get;
			set;
		}

		public bool IsListening
		{
			get { return this.isListening; }
		}

		public bool SupportsHeading
		{
			get { return CLLocationManager.HeadingAvailable; }
		}

		public bool IsGeolocationAvailable
		{
			get { return true; } // all iOS devices support at least wifi geolocation
		}

		public bool IsGeolocationEnabled
		{
			get { return CLLocationManager.Status == CLAuthorizationStatus.Authorized; }
		}

		public Position LastKnownPosition
		{
			get {
				return _position;
			}
		}

		public Task<Position> GetPositionAsync (int timeout)
		{
			return GetPositionAsync (timeout, CancellationToken.None, false);
		}

		public Task<Position> GetPositionAsync (int timeout, bool includeHeading)
		{
			return GetPositionAsync (timeout, CancellationToken.None, includeHeading);
		}

		public Task<Position> GetPositionAsync (CancellationToken cancelToken)
		{
			return GetPositionAsync (Timeout.Infinite, cancelToken, false);
		}

		public Task<Position> GetPositionAsync (CancellationToken cancelToken, bool includeHeading)
		{
			return GetPositionAsync (Timeout.Infinite, cancelToken, includeHeading);
		}

		public Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken)
		{
			return GetPositionAsync (timeout, cancelToken, false);
		}

		public Task<Position> GetPositionAsync (int timeout, CancellationToken cancelToken, bool includeHeading)
		{
			if (timeout <= 0 && timeout != Timeout.Infinite)
				throw new ArgumentOutOfRangeException ("timeout", "Timeout must be positive or Timeout.Infinite");

			TaskCompletionSource<Position> tcs;
			if (!IsListening)
			{
				var m = GetManager();

				tcs = new TaskCompletionSource<Position> (m);
				var singleListener = new GeolocationSingleUpdateDelegate (m, DesiredAccuracy, includeHeading, timeout, cancelToken);
				m.Delegate = singleListener;

				m.StartUpdatingLocation ();
				if (includeHeading && SupportsHeading)
					m.StartUpdatingHeading ();

				return singleListener.Task;
			}
			else
			{
				tcs = new TaskCompletionSource<Position>();
				if (this._position == null)
				{
					EventHandler<PositionErrorEventArgs> gotError = null;
					gotError = (s,e) =>
					{
						tcs.TrySetException (new GeolocationException (e.Error));
						PositionError -= gotError;
					};

					PositionError += gotError;

					EventHandler<PositionEventArgs> gotPosition = null;
					gotPosition = (s, e) =>
					{
						tcs.TrySetResult (e.Position);
						PositionChanged -= gotPosition;
					};

					PositionChanged += gotPosition;
				}
				else
					tcs.SetResult (this._position);
			}

			return tcs.Task;
		}

		/// <summary>
		/// Starts the update process of the Geolocator.
		/// </summary>
		/// <param name="minTime">Minimum time in milliseconds.</param>
		/// <param name="minDistance">Minimum distance in meters.</param>
		public void StartListening (uint minTime, double minDistance)
		{
			StartListening (minTime, minDistance, false);
		}

		/// <summary>
		/// Starts the update process of the Geolocator.
		/// </summary>
		/// <param name="minTime">Minimum time in milliseconds.</param>
		/// <param name="minDistance">Minimum distance in meters.</param>
		/// <param name="includeHeading">Should the Geolocator update process update heading too.</param>
		public void StartListening (uint minTime, double minDistance, bool includeHeading)
		{
			if (minTime < 0)
				throw new ArgumentOutOfRangeException ("minTime");
			if (minDistance < 0)
				throw new ArgumentOutOfRangeException ("minDistance");
			if (this.isListening)
				throw new InvalidOperationException ("Already listening");

			if(!CLLocationManager.LocationServicesEnabled || CLLocationManager.Status != CLAuthorizationStatus.Authorized)
			{
				throw new NotImplementedException ("GPS not existing or not authorized");
			}

			this.isListening = true;
			this.manager.DesiredAccuracy = CLLocation.AccurracyBestForNavigation;
			this.manager.DistanceFilter = minDistance;
			this.manager.StartUpdatingLocation ();

			if (includeHeading && CLLocationManager.HeadingAvailable) {
				this.manager.HeadingFilter = 1;
				this.manager.StartUpdatingHeading ();
			}

			UIDevice.CurrentDevice.BeginGeneratingDeviceOrientationNotifications();
			_observer = NSNotificationCenter.DefaultCenter.AddObserver (UIDevice.OrientationDidChangeNotification, OnDidRotate);
		}

		/// <summary>
		/// Stops the update process of the Geolocator.
		/// </summary>
		public void StopListening ()
		{
			if (!this.isListening)
				return;

			this.isListening = false;
			if (CLLocationManager.HeadingAvailable)
				this.manager.StopUpdatingHeading ();

			this.manager.StopUpdatingLocation ();
			this._position = null;

			NSNotificationCenter.DefaultCenter.RemoveObserver (_observer);
			UIDevice.CurrentDevice.EndGeneratingDeviceOrientationNotifications();

			_observer.Dispose();
		}

		private readonly CLLocationManager manager;
		private bool isListening;
		private Position _position;

		private CLLocationManager GetManager()
		{
			CLLocationManager m = null;
			new NSObject().InvokeOnMainThread (() => m = new CLLocationManager());
			return m;
		}

		/// <summary>
		/// Raises the did rotate event.
		/// </summary>
		/// <remarks>
		/// Had to do this, because didn't find a property where to get the actual orientation.
		/// Seems to be the easies method.
		/// </remarks>
		/// <param name="notice">Notice.</param>
		private void OnDidRotate(NSNotification notice)
		{
			UIDevice device = (UIDevice)notice.Object;

			if (device.Orientation != UIDeviceOrientation.FaceUp && device.Orientation != UIDeviceOrientation.FaceDown && device.Orientation != UIDeviceOrientation.Unknown)
				_orientation = device.Orientation;
		}

		private void OnUpdatedHeading (object sender, CLHeadingUpdatedEventArgs e)
		{
			if (e.NewHeading.MagneticHeading == -1)
				return;

			Position p = (this._position == null) ? new Position () : new Position (this._position);

			// If HeadingAccuracy is below 0, than the heading is invalid
			if (e.NewHeading.HeadingAccuracy < 0)
				return;

			var newHeading = e.NewHeading.MagneticHeading;

			p.Heading = CheckDeviceOrientationForHeading(newHeading);

			this._position = p;

			OnHeadingChanged (new PositionEventArgs (p));
		}

		private void OnLocationsUpdated (object sender, CLLocationsUpdatedEventArgs e)
		{
			foreach (CLLocation location in e.Locations)
				UpdatePosition (location);
		}

		private void OnUpdatedLocation (object sender, CLLocationUpdatedEventArgs e)
		{
			UpdatePosition (e.NewLocation);
		}

		private void UpdatePosition (CLLocation location)
		{
			Position p = (this._position == null) ? new Position () : new Position (this._position);

			if (location.HorizontalAccuracy > -1)
			{
				p.Accuracy = location.HorizontalAccuracy;
				p.Latitude = location.Coordinate.Latitude;
				p.Longitude = location.Coordinate.Longitude;
			}

			if (location.VerticalAccuracy > -1)
			{
				p.Altitude = location.Altitude;
				p.AltitudeAccuracy = location.VerticalAccuracy;
			}

			if (location.Speed > -1)
				p.Speed = location.Speed;

			p.Timestamp = new DateTimeOffset (location.Timestamp);

			this._position = p;

			OnPositionChanged (new PositionEventArgs (p));

			location.Dispose();
		}

		private void OnFailed (object sender, MonoTouch.Foundation.NSErrorEventArgs e)
		{
			if ((CLError)e.Error.Code == CLError.Network)
				OnPositionError (new PositionErrorEventArgs (GeolocationError.PositionUnavailable));
		}

		private void OnAuthorizationChanged (object sender, CLAuthorizationChangedEventArgs e)
		{
			if (e.Status == CLAuthorizationStatus.Denied || e.Status == CLAuthorizationStatus.Restricted)
				OnPositionError (new PositionErrorEventArgs (GeolocationError.Unauthorized));
		}

		private void OnPositionChanged (PositionEventArgs e)
		{
			var changed = PositionChanged;

			if (changed != null)
				changed (this, e);
		}

		private void OnHeadingChanged (PositionEventArgs e)
		{
			var changed = HeadingChanged;

			if (changed != null)
				changed (this, e);
		}

		private void OnPositionError (PositionErrorEventArgs e)
		{
			StopListening();

			var error = PositionError;

			if (error != null)
				error (this, e);
		}

		double CheckDeviceOrientationForHeading(double heading)
		{
			double realHeading = heading;

			switch (_orientation) {
				case UIDeviceOrientation.Portrait:
					break;
				case UIDeviceOrientation.PortraitUpsideDown:
					realHeading = realHeading + 180.0f;
					break;
				case UIDeviceOrientation.LandscapeLeft:
					realHeading = realHeading + 90.0f;
					break;
				case UIDeviceOrientation.LandscapeRight:
					realHeading = realHeading - 90.0f;
					break;
				default:
					break;
			}
	
			while (realHeading < 0)
				realHeading += 360.0;

			realHeading = realHeading % 360;

			return realHeading;
		}
	}
}