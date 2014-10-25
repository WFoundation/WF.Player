// <copyright file="Colors.cs" company="Wherigo Foundation">
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
	using WF.Player.Services.Preferences;
	using Xamarin.Forms;

	/// <summary>
	/// Special Colors.
	/// </summary>
	public class Colors
	{
		/// <summary>
		/// Preferences to use.
		/// </summary>
		private IPreferences prefs;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Colors"/> class.
		/// </summary>
		public Colors()
		{
			this.prefs = DependencyService.Get<IPreferences>();
		}

		#endregion

		/// <summary>
		/// Gets a value indicating whether this player use dark theme.
		/// </summary>
		/// <value><c>true</c> if this instance is using dark theme; otherwise, <c>false</c>.</value>
		public bool IsDarkTheme
		{
			get
			{
				if (this.prefs.Get<int>(DefaultPreferences.DisplayThemeKey) == 0)
				{
					// Light theme
					return false;
				}
				else
				{
					return true;
				}
			}
		}

		/// <summary>
		/// Gets the background color.
		/// </summary>
		/// <value>Background color.</value>
		public Color Background
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Color.Black;
				}
				else
				{
					return Color.White;
				}
			}
		}

		/// <summary>
		/// Gets the text color.
		/// </summary>
		/// <value>Text color.</value>
		public Color Text
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Color.White;
				}
				else
				{
					return Color.Black;
				}
			}
		}

		/// <summary>
		/// Gets the bar color.
		/// </summary>
		/// <value>Bar color.</value>
		public Color Bar
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					int color = Device.OnPlatform(0x30, 0x50, 0x30);

					return Color.FromRgb(color, color, color);
				}
				else
				{
					int color = Device.OnPlatform(0xF6, 0xE0, 0xF6);

					return Color.FromRgb(color, color, color);
				}
			}
		}

		/// <summary>
		/// Gets the bar text color.
		/// </summary>
		/// <value>Bar text color.</value>
		public Color BarText
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Color.White;
				}
				else
				{
					return Color.Black;
				}
			}
		}

		/// <summary>
		/// Gets the tint color.
		/// </summary>
		/// <value>The tint.</value>
		public Color Tint
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Device.OnPlatform<Color>(Color.FromRgb(0x00, 0x7A, 0xFF), Color.FromRgb(0x33, 0xB5, 0xE5), Color.FromRgb(0x00, 0x7A, 0xFF));
				}
				else
				{
					return Device.OnPlatform<Color>(Color.FromRgb(0x00, 0x7A, 0xFF), Color.FromRgb(0x33, 0xB5, 0xE5), Color.FromRgb(0x00, 0x7A, 0xFF));
				}
			}
		}

		/// <summary>
		/// Gets the background color for buttons.
		/// </summary>
		/// <value>The background buttons.</value>
		public Color BackgroundButtons
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Color.FromRgb(0x80, 0x80, 0x80);
				}
				else
				{
					return Color.FromRgb(0x80, 0x80, 0x80);
				}
			}
		}

		/// <summary>
		/// Gets the background color for direction circle.
		/// </summary>
		/// <value>The direction background.</value>
		public Color DirectionBackground
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Device.OnPlatform<Color>(Color.FromRgb(0xA8, 0xD1, 0xFF), Color.FromRgb(0xaf, 0xe2, 0xf5), Color.FromRgb(0xA8, 0xD1, 0xFF)); // 0xFF,0xAA,0xAA
				}
				else
				{
					return Device.OnPlatform<Color>(Color.FromRgb(0xA8, 0xD1, 0xFF), Color.FromRgb(0xaf, 0xe2, 0xf5), Color.FromRgb(0xA8, 0xD1, 0xFF)); // 0xFF,0xAA,0xAA
				}
			}
		}

		/// <summary>
		/// Gets the color for direction arrow.
		/// </summary>
		/// <value>The color of the direction.</value>
		public Color DirectionColor
		{
			get
			{
				if (this.IsDarkTheme)
				{
					// Light theme
					return Device.OnPlatform<Color>(Color.FromRgb(0x00, 0x7A, 0xFF), Color.FromRgb(0x33, 0xB5, 0xE5), Color.FromRgb(0x00, 0x7A, 0xFF)); // 0xFF,0xAA,0xAA
				}
				else
				{
					return Device.OnPlatform<Color>(Color.FromRgb(0x00, 0x7A, 0xFF), Color.FromRgb(0x33, 0xB5, 0xE5), Color.FromRgb(0x00, 0x7A, 0xFF));
				}
			}
		}
	}
}
