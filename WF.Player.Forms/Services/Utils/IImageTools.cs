// <copyright file="IImageTools.cs" company="Wherigo Foundation">
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

namespace WF.Player.Services.Utils
{
	using System;

	/// <summary>
	/// I image tools.
	/// </summary>
	public interface IImageTools
	{
		/// <summary>
		/// Resize the specified image to width and height.
		/// </summary>
		/// <returns>Resized image as byte array.</returns>
		/// <param name="originalImageData">Image to resize as byte array.</param>
		/// <param name="reqWidth">Width in dip.</param>
		/// <param name="reqHeight">Height in dip.</param>
		byte[] Resize(byte[] originalImageData, int reqWidth, int reqHeight);
	}
}
