// <copyright file="MapSource.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player
{
	/// <summary>
	/// Map source types.
	/// </summary>
	public enum MapSource : int
	{
		/// <summary>
		/// Google maps streets.
		/// </summary>
		GoogleMaps,

		/// <summary>
		/// Google satellite view.
		/// </summary>
		GoogleSatellite,

		/// <summary>
		/// Google terrain.
		/// </summary>
		GoogleTerrain,

		/// <summary>
		/// Google hybrid.
		/// </summary>
		GoogleHybrid,

		/// <summary>
		/// Open street map.
		/// </summary>
		OpenStreetMap,

		/// <summary>
		/// Open cycle map.
		/// </summary>
		OpenCycleMap,

		/// <summary>
		/// Offline map.
		/// </summary>
		Offline,

		/// <summary>
		/// Cartridge containing map.
		/// </summary>
		Cartridge,

		/// <summary>
		/// No map.
		/// </summary>
		None
	}
}
