using System;
using System.Threading.Tasks;
using System.Threading;

namespace WF.Player.Services.Geolocation
{
	public interface IGeolocator
	{
		event EventHandler<PositionErrorEventArgs> PositionError;

		event EventHandler<PositionEventArgs> PositionChanged;

		event EventHandler<PositionEventArgs> HeadingChanged;

		double DesiredAccuracy
		{
			get;
			set;
		}

		bool IsListening {
			get;
		}

		bool SupportsHeading {
			get;
		}

		bool IsGeolocationAvailable {
			get;
		}

		bool IsGeolocationEnabled {
			get;
		}

		Position LastKnownPosition {
			get;
		}

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		void StartListening (uint minTime, double minDistance);

		/// <summary>
		/// Start listening to location changes
		/// </summary>
		/// <param name="minTime">Minimum interval in milliseconds</param>
		/// <param name="minDistance">Minimum distance in meters</param>
		/// <param name="includeHeading">Include heading information</param>
		void StartListening (uint minTime, double minDistance, bool includeHeading);

		/// <summary>
		/// Stop listening to location changes
		/// </summary>
		void StopListening ();
	}
}