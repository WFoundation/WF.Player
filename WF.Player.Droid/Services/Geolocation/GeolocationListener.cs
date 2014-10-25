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
using System.Threading.Tasks;
using Android.Locations;
using Android.OS;
using System.Threading;
using System.Collections.Generic;
using WF.Player.Services.Geolocation;
using WF.Player.Services.Preferences;
using Android.Views;
using Android.Hardware;
using Android.App;
using Android.Runtime;

namespace WF.Player.Droid.Services.Geolocation
{
	internal class GeolocationListener : Java.Lang.Object, ILocationListener, ISensorEventListener
	{
		// Base for time convertions from time in seconds since 1970-01-01 to DateTime
		readonly DateTime _baseTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		IList<string> _providers;
		readonly HashSet<string> _activeProviders = new HashSet<string>();
		readonly LocationManager _locManager;
		string _activeProvider;
		Location _lastLocation;
		TimeSpan _timePeriod;
		DateTime _lastDeclinationCalculation;
		double _lastGPSAzimuth;
		double _lastSensorAzimuth;
		double _azimuth;
		double _pitch;
		double _roll;
		double _aboveOrBelow = 0.0;

		#region Constructor

		public GeolocationListener (LocationManager manager, TimeSpan timePeriod, IList<string> providers)
		{
			this._locManager = manager;
			this._timePeriod = timePeriod;
			this._providers = providers;

			foreach (string p in providers)
			{
				if (manager.IsProviderEnabled (p))
					this._activeProviders.Add (p);
			}
		}

		#endregion

		#region Events

		public event EventHandler<PositionErrorEventArgs> PositionError;

		public event EventHandler<PositionEventArgs> PositionChanged;

		public event EventHandler<HeadingEventArgs> OrientationChanged;

		#endregion

		#region ILocationListener implementation

		public void OnLocationChanged (Location location)
		{
			if (location.Provider != this._activeProvider)
			{
				if (this._activeProvider != null && this._locManager.IsProviderEnabled (this._activeProvider))
				{
					LocationProvider pr = this._locManager.GetProvider (location.Provider);
					TimeSpan lapsed = GetTimeSpan (location.Time) - GetTimeSpan (this._lastLocation.Time);
					if (pr.Accuracy > this._locManager.GetProvider (this._activeProvider).Accuracy
						&& lapsed < _timePeriod.Add (_timePeriod))
					{
						location.Dispose();
						return;
					}
				}
				this._activeProvider = location.Provider;
			}
			var previous = Interlocked.Exchange (ref this._lastLocation, location);

			if (previous != null)
				previous.Dispose();

			var p = new Position();

			if (location.HasAccuracy)
				p.Accuracy = location.Accuracy;

			if (location.HasAltitude)
				p.Altitude = location.Altitude;

			if (location.HasBearing) {
				p.Heading = location.Bearing;
				_lastGPSAzimuth = location.Bearing;
			}

			if (location.HasSpeed)
				p.Speed = location.Speed;

			p.Longitude = location.Longitude;
			p.Latitude = location.Latitude;
			p.Timestamp = Geolocator.GetTimestamp (location);

			var changed = PositionChanged;

			if (changed != null)
				changed (this, new PositionEventArgs (p));
		}

		public void OnProviderDisabled (string provider)
		{
			if (provider == LocationManager.PassiveProvider)
				return;

			lock (this._activeProviders)
			{
				if (this._activeProviders.Remove (provider) && this._activeProviders.Count == 0)
					OnPositionError (new PositionErrorEventArgs (GeolocationError.PositionUnavailable));
			}
		}

		public void OnProviderEnabled (string provider)
		{
			if (provider == LocationManager.PassiveProvider)
				return;

			lock (this._activeProviders)
				this._activeProviders.Add (provider);	
		}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
			switch (status)
			{
				case Availability.Available:
					OnProviderEnabled (provider);
					break;
				case Availability.OutOfService:
					OnProviderDisabled (provider);
					break;
			}
		}

		void OnPositionError (PositionErrorEventArgs e)
		{
			var error = PositionError;

			if (error != null)
				error (this, e);
		}

		#endregion

		#region ISensorEventListener Implementation

		public void OnAccuracyChanged(Sensor sensor, SensorStatus status)
		{
		}

		/// <summary>
		/// Function, which is called, when the sensors change.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnSensorChanged(SensorEvent args)
		{
			switch (args.Sensor.Type) {
				case SensorType.MagneticField:
					break;
				case SensorType.Accelerometer:
					double filter = GetFilterValue();
					_aboveOrBelow = (args.Values[2] * filter) + (_aboveOrBelow * (1.0 - filter));
					break;
				case SensorType.Orientation:
					double azimuth = args.Values [0];
					// Fix to true bearing
					if (App.Prefs.Get<bool> (DefaultPreferences.SensorAzimuthKey)) {
						azimuth += Declination;
					}
					_azimuth = FilterValue (azimuth, _azimuth);
					_pitch = FilterValue (args.Values [1], _pitch);
					_roll = FilterValue (args.Values [2], _roll);
					_lastSensorAzimuth = _azimuth;
					double rollDef;
					if (_aboveOrBelow < 0) {
						if (_roll < 0) {
							rollDef = -180 - _roll;
						} else {
							rollDef = 180 - _roll;
						}
					} else {
						rollDef = _roll;
					}
					// Adjust the rotation matrix for the device orientation
					IWindowManager windowManager = Application.Context.GetSystemService ("window").JavaCast<IWindowManager> ();
					SurfaceOrientation screenRotation = windowManager.DefaultDisplay.Rotation;
					switch (screenRotation)
					{
						case SurfaceOrientation.Rotation0:
							// no need for change
							break;
						case SurfaceOrientation.Rotation90:
							_lastSensorAzimuth += 90;
							break;
						case SurfaceOrientation.Rotation180:
							_lastSensorAzimuth -= 180;
							break;
						case SurfaceOrientation.Rotation270:
							_lastSensorAzimuth -= 90;
							break;
					}
					SendOrientation(_pitch, rollDef);
					break;
			}
		}
		#endregion

		#region SensorEventListener Functions

		double _declination = 0;

		/// <summary>
		/// Gets the declination in degrees.
		/// </summary>
		/// <remarks>
		/// The declinations changes are very slow. So it is enough to calculat this very 5 minutes.
		/// </remarks>
		/// <value>The declination.</value>
		/// 
		public double Declination {
			get {
				DateTime time = DateTime.Now;

				// Compute this only if needed
				if (time.Subtract(_lastDeclinationCalculation).Seconds > 300) {
					using (GeomagneticField _geoField = new GeomagneticField((Single) _lastLocation.Latitude, (Single) _lastLocation.Longitude, (Single) _lastLocation.Altitude, time.Subtract(_baseTime).Milliseconds)) {
						// Save for later use
						_lastDeclinationCalculation = time;
						_declination = _geoField.Declination;
					}
				}

				return _declination;
			}
		}
			
		/// <summary>
		/// Filters the value to flatten the value by combining the actual and last value.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="valueActual">Actual value in degrees.</param>
		/// <param name="valueLast">Last value in degrees.</param>
		private double FilterValue(double valueActual, double valueLast)
		{
			if (valueActual < valueLast - 180.0) {
				valueLast -= 360.0;
			} else if (valueActual > valueLast + 180.0) {
				valueLast += 360.0;
			}
			double filter = GetFilterValue();
			return valueActual * filter + valueLast * (1.0 - filter);
		}

		/// <summary>
		/// Gets the filter value like it is set in the preferences.
		/// </summary>
		/// <returns>The filter value.</returns>
		double GetFilterValue()
		{
			switch (App.Prefs.Get<int>(DefaultPreferences.SensorOrientationFilterKey))
			{
				case 1: // PreferenceValues.VALUE_SENSORS_ORIENT_FILTER_LIGHT:
					return 0.20;
				case 2: // PreferenceValues.VALUE_SENSORS_ORIENT_FILTER_MEDIUM:
					return 0.06;
				case 3: // PreferenceValues.VALUE_SENSORS_ORIENT_FILTER_HEAVY:
					return 0.03;
			}
			return 1.0;
		}

		void SendOrientation(double pitch, double roll)
		{
			double azimuth;

			if (!App.Prefs.Get<bool>(DefaultPreferences.SensorHardwareCompassAutoChange) || (_lastLocation != null && _lastLocation.Speed < App.Prefs.Get<double>(DefaultPreferences.SensorHardwareCompassAutoChangeValue))) {
				if (!App.Prefs.Get<bool>(DefaultPreferences.SensorHardwareCompass))
					// Substract 90° because the bearing 0° is in direction east
					azimuth = _lastSensorAzimuth;
				else
					azimuth = _lastGPSAzimuth;
			} else {
				azimuth = _lastGPSAzimuth;
			}

			var handler = OrientationChanged;

			if (handler != null)
				handler(this, new HeadingEventArgs(azimuth, pitch, roll));
		}
		#endregion

		#region Private Functions

		TimeSpan GetTimeSpan (long time)
		{
			return new TimeSpan (TimeSpan.TicksPerMillisecond * time);
		}

		#endregion
	}

	#region EventArg Classes

	public sealed class HeadingEventArgs : EventArgs
	{
		double _azimuth;
		double _pitch;
		double _roll;

		public HeadingEventArgs(double azimuth, double pitch, double roll)
		{
			_azimuth = azimuth;
			_pitch = pitch;
			_roll = roll;
		}

		public double Azimuth {
			get { return _azimuth; }
		}

		public double Pitch {
			get { return _pitch; }
		}

		public double Roll {
			get { return _roll; }
		}
	}

	#endregion
}