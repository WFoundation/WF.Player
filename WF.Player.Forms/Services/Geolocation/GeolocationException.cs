using System;

namespace WF.Player.Services.Geolocation
{
	public class GeolocationException : Exception
	{
		public GeolocationException (GeolocationError error) 
			: base ("A geolocation error occured: " + error)
		{
			if (!Enum.IsDefined (typeof (GeolocationError), error))
				throw new ArgumentException ("error is not a valid GelocationError member", "error");

			Error = error;
		}

		public GeolocationException (GeolocationError error, Exception innerException)
			: base ("A geolocation error occured: " + error, innerException)
		{
			if (!Enum.IsDefined (typeof (GeolocationError), error))
				throw new ArgumentException ("error is not a valid GelocationError member", "error");

			Error = error;
		}

		public GeolocationError Error
		{
			get;
			private set;
		}
	}

	public class PositionErrorEventArgs
		: EventArgs
	{
		public PositionErrorEventArgs (GeolocationError error)
		{
			Error = error;
		}

		public GeolocationError Error
		{
			get;
			private set;
		}
	}

	public enum GeolocationError
	{
		/// <summary>
		/// The provider was unable to retrieve any position data.
		/// </summary>
		PositionUnavailable,

		/// <summary>
		/// The app is not, or no longer, authorized to receive location data.
		/// </summary>
		Unauthorized
	}
}