// <copyright file="IPreferences.cs" company="Wherigo Foundation">
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

namespace WF.Player.Services.Preferences
{
	using System;
	using Xamarin.Forms;

	/// <summary>
	/// Interface for preferences.
	/// </summary>
	public interface IPreferences
	{
		/// <summary>
		/// Gets the format for coordinates.
		/// </summary>
		/// <value>The format coordinates.</value>
		FormatCoordinates FormatCoordinates { get; }

		/// <summary>
		/// Gets the unit for lengths.
		/// </summary>
		/// <value>The length of the unit.</value>
		UnitLength UnitLength { get; }

		/// <summary>
		/// Gets the text alignment.
		/// </summary>
		/// <value>The text alignment.</value>
		TextAlignment TextAlignment { get; }

		/// <summary>
		/// Gets the image alignment.
		/// </summary>
		/// <value>The image alignment.</value>
		TextAlignment ImageAlignment { get; }

		/// <summary>
		/// Gets the image resize.
		/// </summary>
		/// <value>The image resize.</value>
		ImageResize ImageResize { get; }

		/// <summary>
		/// Gets the size of text.
		/// </summary>
		/// <value>The size of the text.</value>
		double TextSize { get; }

		/// <summary>
		/// Get preference for specified key.
		/// </summary>
		/// <param name="key">Key for prefernece value as string.</param>
		/// <typeparam name="T">Type parameter for result.</typeparam>
		/// <returns>>Preference for key in format T.</returns>
		T Get<T>(string key);

		/// <summary>
		/// Set the specified key and preference value.
		/// </summary>
		/// <param name="key">Key for prefernece value as string.</param>
		/// <param name="value">Preference value.</param>
		/// <typeparam name="T">Type parameter of value.</typeparam>
		void Set<T>(string key, T value);
	}
}