// <copyright file="PreferencesCommon.cs" company="Wherigo Foundation">
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
	using WF.Player.Common;
	using Xamarin.Forms;

	/// <summary>
	/// Preferences common.
	/// </summary>
	public class PreferencesCommon : IPreferences
	{
		#region Properties

		/// <summary>
		/// Gets the format type for coordinates.
		/// </summary>
		/// <value>The format coordinates.</value>
		public FormatCoordinates FormatCoordinates 
		{
			get 
			{
				return (FormatCoordinates)this.Get<int>(DefaultPreferences.FormatCoordinatesKey);
			}
		}

		/// <summary>
		/// Gets the unit for lengths.
		/// </summary>
		/// <value>The length of the unit.</value>
		public UnitLength UnitLength 
		{
			get 
			{
				return (UnitLength)this.Get<int>(DefaultPreferences.UnitLengthKey);
			}
		}

		/// <summary>
		/// Gets the text alignment for horizontal alignments of Xamarin.Forms objects.
		/// </summary>
		/// <value>The text alignment.</value>
		public TextAlignment TextAlignment 
		{
			get 
			{
				switch (this.Get<int>(DefaultPreferences.TextAlignmentKey)) 
				{
					case 0:
						return TextAlignment.Start;
					case 1:
						return TextAlignment.Center;
					case 2:
						return TextAlignment.End;
					default:
						return TextAlignment.Center;
				}
			}
		}

		/// <summary>
		/// Gets the image alignment for horizontal alignments of Xamarin.Forms objects.
		/// </summary>
		/// <value>The image alignment.</value>
		public TextAlignment ImageAlignment 
		{
			get 
			{
				switch (this.Get<int>(DefaultPreferences.ImageAlignmentKey)) 
				{
					case 0:
						return TextAlignment.Start;
					case 1:
						return TextAlignment.Center;
					case 2:
						return TextAlignment.End;
					default:
						return TextAlignment.Center;
				}
			}
		}

		/// <summary>
		/// Gets the image size alignment for horizontal alignments of Xamarin.Forms objects.
		/// </summary>
		/// <value>The image alignment.</value>
		public ImageResize ImageResize 
		{
			get 
			{
				return (ImageResize)this.Get<int>(DefaultPreferences.ImageResizeKey);
			}
		}

		/// <summary>
		/// Gets the size of the text for Xamarin.Forms objects.
		/// </summary>
		/// <value>The size of the text.</value>
		public double TextSize 
		{
			get 
			{
				return this.Get<double>(DefaultPreferences.TextSizeKey) == 0 ? Xamarin.Forms.Device.OnPlatform(18, 18, 18) : this.Get<double>(DefaultPreferences.TextSizeKey);
			}
		}

		#endregion

		/// <summary>
		/// Get preference for specified key.
		/// </summary>
		/// <param name="key">Key for prefernece value as string.</param>
		/// <typeparam name="T">Type parameter for result.</typeparam>
		/// <returns>>Preference for key as type T.</returns>
		public virtual T Get<T>(string key)
		{
			return default(T);
		}

		/// <summary>
		/// Set the specified key and preference value.
		/// </summary>
		/// <param name="key">Key for prefernece value as string.</param>
		/// <param name="value">Preference value.</param>
		/// <typeparam name="T">Type parameter of value.</typeparam>
		public virtual void Set<T>(string key, T value)
		{
		}
	}
}
