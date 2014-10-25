// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz <mail@wfplayer.com>
//
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

using System;
using Xamarin.Forms;
using WF.Player.Services.Geolocation;
using Android.Content;

[assembly: Dependency(typeof(WF.Player.Droid.Services.Routing.NativeRouting))]

namespace WF.Player.Droid.Services.Routing
{
	public class NativeRouting : IRouting
	{
		public void StartRouting(string name, Position pos) 
		{
			// TODO: Conversion from lat/lon to string should be on all devices with point "." instead of ","
			string uri = String.Format("google.navigation:q={0},{1}({2})", pos.Latitude.ToString().Replace(",", "."), pos.Longitude.ToString().Replace(",", "."), name);

			Intent intent = new Intent(Intent.ActionView, global::Android.Net.Uri.Parse("google.navigation:q=0,0"));

			// Do we have a navigation app?
			if (intent.ResolveActivity(Forms.Context.PackageManager) == null)
				return;

			Forms.Context.StartActivity(intent);
		}
	}
}

