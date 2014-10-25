// <copyright file="Fonts.cs" company="Wherigo Foundation">
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
	/// Special fonts.
	/// </summary>
	public class Fonts
	{
		/// <summary>
		/// Preferences to use.
		/// </summary>
		private IPreferences prefs;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Fonts"/> class.
		/// </summary>
		public Fonts()
		{
			this.prefs = DependencyService.Get<IPreferences>();
		}

		#endregion

		/// <summary>
		/// Gets the font for header.
		/// </summary>
		/// <value>Font for header.</value>
		public Font Header
		{
			get
			{
				return Font.SystemFontOfSize(this.prefs.TextSize * 1.5).WithAttributes(FontAttributes.Bold);
			}
		}

		/// <summary>
		/// Gets the normal font.
		/// </summary>
		/// <value>Normal font.</value>
		public Font Normal
		{
			get
			{
				return Font.SystemFontOfSize(this.prefs.TextSize);
			}
		}

		/// <summary>
		/// Gets the small font.
		/// </summary>
		/// <value>Small font.</value>
		public Font Small 
		{
			get 
			{
				return Font.SystemFontOfSize(this.prefs.TextSize * 0.8);
			}
		}
	}
}
