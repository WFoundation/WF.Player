// <copyright file="IMeasure.cs" company="Wherigo Foundation">
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

namespace WF.Player.Services.Device
{
	/// <summary>
	/// Interface for measurement of different objects.
	/// </summary>
	public interface IMeasure
	{
		/// <summary>
		/// Calculates size of text on a Button.
		/// </summary>
		/// <returns>Size of text.</returns>
		/// <param name="text">Text for calculation.</param>
		float ButtonTextSize(string text);
	}
}
