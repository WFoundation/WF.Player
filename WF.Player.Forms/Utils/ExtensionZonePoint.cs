// <copyright file="ExtensionZonePoint.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>

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

using WF.Player.Core;

namespace WF.Player.Utils
{
	public static class ExtensionZonePoint
	{
        public static Plugin.Geolocator.Abstractions.Position ToPosition(this ZonePoint zp)
        {
            var result = new Plugin.Geolocator.Abstractions.Position();

            result.Latitude = zp.Latitude;
            result.Longitude = zp.Longitude;

            return result;
        }
    }
}

