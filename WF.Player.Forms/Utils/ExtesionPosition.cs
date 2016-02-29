// <copyright file="ExtensionPosition.cs" company="Wherigo Foundation">
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

using Plugin.Geolocator.Abstractions;
using WF.Player.Core;

namespace WF.Player.Utils
{
    public static class ExtensionPosition
    {
        public static Position FromZonePoint(this Position position, ZonePoint zp)
        {
            position.Latitude = zp.Latitude;
            position.Longitude = zp.Longitude;

            return position;
        }

        public static Position FromValues(this Position position, double latitude, double longitude)
        {
            position.Latitude = latitude;
            position.Longitude = longitude;

            return position;
        }
    }
}

