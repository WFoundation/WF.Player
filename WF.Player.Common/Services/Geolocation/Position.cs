using System;

namespace WF.Player.Services.Geolocation
{
	public class Position
	{
		public Position()
		{
		}

		public Position (Position position)
		{
			if (position == null)
				throw new ArgumentNullException ("position");

			Timestamp = position.Timestamp;
			Latitude = position.Latitude;
			Longitude = position.Longitude;
			Altitude = position.Altitude;
			AltitudeAccuracy = position.AltitudeAccuracy;
			Accuracy = position.Accuracy;
			Heading = position.Heading;
			Speed = position.Speed;
		}

		public Position(double lat, double lon, double alt = 0, double accuracy = double.NaN)
		{
			Timestamp = DateTime.Now;
			Latitude = lat;
			Longitude = lon;
			Altitude = alt;
			AltitudeAccuracy = 0;
			Accuracy = accuracy;
			Heading = 0;
			Speed = 0;
		}

		public DateTimeOffset Timestamp
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the latitude.
		/// </summary>
		public double Latitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the longitude.
		/// </summary>
		public double Longitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the altitude in meters relative to sea level.
		/// </summary>
		public double? Altitude
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential position error radius in meters.
		/// </summary>
		public double? Accuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the potential altitude error range in meters.
		/// </summary>
		/// <remarks>
		/// Not supported on Android, will always read 0.
		/// </remarks>
		public double? AltitudeAccuracy
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the heading in degrees relative to true North.
		/// </summary>
		public double? Heading
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the speed in meters per second.
		/// </summary>
		public double? Speed
		{
			get;
			set;
		}
	}

}