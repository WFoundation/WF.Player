// <copyright file="ConverterToAltitude.cs" company="Wherigo Foundation">
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
	using System;
	using System.Globalization;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;

	/// <summary>
	/// Converter to altitude.
	/// </summary>
	public class ConverterToAltitude : IValueConverter
	{
		/// <param name="value">Value to convert.</param>
		/// <param name="targetType">Type of value.</param>
		/// <param name="parameter">Parameter for conversion.</param>
		/// <param name="culture">Culture to use while conversion.</param>
		/// <summary>
		/// Convert the specified value, targetType, parameter and culture.
		/// </summary>
		/// <returns>Returns the object in targetType format.</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Position pos = (Position)value;

			if (pos == null)
			{
				return string.Empty;
			}

			double alt = 0;

			if (pos.Altitude != null)
			{
				alt = (double)pos.Altitude;
			}

			if (alt == 0)
			{
				return " ";
			}

			// TODO: Use extra converter for altitude
			if (parameter is string)
			{
				return string.Format((string)parameter, Converter.NumberToBestLength(alt));
			}
			else
			{
				return Converter.NumberToBestLength(alt);
			}
		}

		/// <param name="value">Value to convert.</param>
		/// <param name="targetType">Type of value.</param>
		/// <param name="parameter">Parameter for conversion.</param>
		/// <param name="culture">Culture to use while conversion.</param>
		/// <summary>
		/// Converts the back.
		/// </summary>
		/// <returns>Returns the object in targetType format.</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException("ConverterToAltitude.ConvertBack");
		}
	}
}
