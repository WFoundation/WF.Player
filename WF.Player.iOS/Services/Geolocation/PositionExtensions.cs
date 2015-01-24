using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WF.Player.Core;
using WF.Player.Services.Geolocation;

namespace WF.Player.iOS.Services.Geolocation
{
	/// <summary>
	/// Position extensions
	/// </summary>
	public static class PositionExtensions
	{
		public static ZonePoint ToZonePoint(this Position p)
		{
			return new ZonePoint (p.Latitude, p.Longitude, p.Altitude ?? 0);
		}
	}
}