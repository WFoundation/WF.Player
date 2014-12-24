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

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Hardware;
using Java.Lang;
using Xamarin.Forms;
using WF.Player.Services.Geolocation;
using WF.Player.Droid.Services.Geolocation;

[assembly: Dependency(typeof (Geolocator))]

namespace WF.Player.Droid.Services.Geolocation
{
	public class Geolocator : IGeolocator
	{
		static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		readonly LocationManager _locManager;
		readonly SensorManager _sensorManager;
		readonly object _positionSync = new object();
		readonly string[] _providers;
		string _headingProvider;
		Position _lastPosition;
		GeolocationListener _listener;
		Sensor _orientationSensor;
		Sensor _accelerometerSensor;

		#region Constructor

		public Geolocator()
		{
			_locManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
			_providers = _locManager.GetProviders(false).Where(s => s != LocationManager.PassiveProvider).ToArray();
			_sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService (Context.SensorService);
		}

		#endregion

		#region Events

		public event EventHandler<PositionErrorEventArgs> PositionError;

		public event EventHandler<PositionEventArgs> PositionChanged;

		public event EventHandler<PositionEventArgs> HeadingChanged;

		#endregion

		public bool IsListening
		{
			get { return _listener != null; }
		}

		public double DesiredAccuracy { get; set; }

		public bool SupportsHeading
		{
			get
			{
				return false;
				// if (this.headingProvider == null || !this.manager.IsProviderEnabled (this.headingProvider))
				// {
				// Criteria c = new Criteria { BearingRequired = true };
				// string providerName = this.manager.GetBestProvider (c, enabledOnly: false);
				//
				// LocationProvider provider = this.manager.GetProvider (providerName);
				//
				// if (provider.SupportsBearing())
				// {
				// this.headingProvider = providerName;
				// return true;
				// }
				// else
				// {
				// this.headingProvider = null;
				// return false;
				// }
				// }
				// else
				// return true;
			}
		}

		public bool IsGeolocationAvailable
		{
			get { return _providers.Length > 0; }
		}

		public bool IsGeolocationEnabled
		{
			get { return _providers.Any(_locManager.IsProviderEnabled); }
		}

		public Position LastKnownPosition
		{
			get {
				return this._lastPosition;
			}
		}

		public void StartListening(uint minTime, double minDistance)
		{
			StartListening(minTime, minDistance, false);
		}

		public void StartListening(uint minTime, double minDistance, bool includeHeading)
		{
			if (minTime < 0)
				throw new ArgumentOutOfRangeException("minTime");
			if (minDistance < 0)
				throw new ArgumentOutOfRangeException("minDistance");

			if (IsListening)
				throw new InvalidOperationException("This Geolocator is already listening");

			_listener = new GeolocationListener(_locManager, TimeSpan.FromMilliseconds(minTime), _providers);
			_listener.PositionChanged += OnPositionChanged;
			_listener.PositionError += OnPositionError;
			Looper looper = Looper.MyLooper() ?? Looper.MainLooper;
			for (int i = 0; i < _providers.Length; ++i)
				_locManager.RequestLocationUpdates(_providers[i], minTime, (float) minDistance, _listener, looper);

			// Add orientation event listener
			if (includeHeading) {
				_listener.OrientationChanged += OnHeadingChanged;

				_orientationSensor = _sensorManager.GetDefaultSensor(SensorType.Orientation);
				_accelerometerSensor = _sensorManager.GetDefaultSensor(SensorType.Accelerometer);

				_sensorManager.RegisterListener(_listener, _orientationSensor, SensorDelay.Ui);
				_sensorManager.RegisterListener(_listener, _accelerometerSensor, SensorDelay.Ui);
			}
		}

		public void StopListening()
		{
			if (_listener == null)
				return;

			if (_listener != null) {
				_listener.PositionChanged -= OnPositionChanged;
				_listener.PositionError -= OnPositionError;
				_listener.OrientationChanged -= OnHeadingChanged;
				for (int i = 0; i < _providers.Length; ++i)
					_locManager.RemoveUpdates (_listener);
				_listener = null;
			}
		}

		void OnHeadingChanged (object sender, HeadingEventArgs e)
		{
			// Ignore anything that might come in afterwards
			if (!IsListening) 
				return;

			lock (_positionSync)
			{
				if (_lastPosition == null)
					return;

				_lastPosition.Heading = System.Math.Round(e.Azimuth);

				var changed = HeadingChanged;

				if (changed != null)
					changed(this, new PositionEventArgs(_lastPosition));
			}
		}

		private void OnPositionChanged(object sender, PositionEventArgs e)
		{
			// Ignore anything that might come in afterwards
			if (!IsListening)
				return;

			lock (_positionSync)
			{
				_lastPosition = e.Position;

				EventHandler<PositionEventArgs> changed = PositionChanged;

				if (changed != null)
					changed(this, e);
			}
		}

		private void OnPositionError(object sender, PositionErrorEventArgs e)
		{
			StopListening();

			EventHandler<PositionErrorEventArgs> error = PositionError;

			if (error != null)
				error(this, e);
		}

		#region Private Functions

		internal static DateTimeOffset GetTimestamp(Location location)
		{
			return new DateTimeOffset(Epoch.AddMilliseconds(location.Time));
		}

		#endregion
	}
}
