﻿// <copyright file="IScreen.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
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

// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace WF.Player.Services.Device
{
	/// <summary>
	/// Interface for screen.
	/// </summary>
	public interface IScreen
	{
		/// <summary>
		/// Gets the width of screen.
		/// </summary>
		/// <value>The width.</value>
		int Width { get; }

		/// <summary>
		/// Gets the height of screen.
		/// </summary>
		/// <value>The height.</value>
		int Height { get; }

		/// <summary>
		/// Sets the screen to always the on.
		/// </summary>
		/// <param name="flag">If set to <c>true</c>, than the screen is always on.</param>
		void AlwaysOn(bool flag);
	}
}

