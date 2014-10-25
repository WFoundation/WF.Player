using System;
using WF.Player.Services.Geolocation;

namespace WF.Player
{
	public interface IApp
	{
		IGeolocator GPS { get; }
	}
}
