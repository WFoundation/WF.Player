// <copyright file="Screen.cs" company="Wherigo Foundation">
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
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.WP8.Services.Device.Screen))]

namespace WF.Player.WP8.Services.Device
{
    using System;
    using WF.Player.Services.Device;
    /// <summary>
    /// Screen implementation.
    /// </summary>
    public class Screen : IScreen
	{
		private readonly Lazy<int> height;
		private readonly Lazy<int> width;

		public Screen()
		{
            var scaleFactor = Windows.Graphics.Display.DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;

            this.height = new Lazy<int>(() => Convert.ToInt32(Windows.UI.Xaml.Window.Current.Bounds.Height * scaleFactor));
            this.width = new Lazy<int>(() => Convert.ToInt32(Windows.UI.Xaml.Window.Current.Bounds.Width * scaleFactor));
		}

		#region IScreen implementation

		/// <summary>
		/// Gets the height of screen.
		/// </summary>
		/// <value>The height.</value>
		public int Height
		{
			get
			{
				return this.height.Value;
			}
		}

		/// <summary>
		/// Gets the width of screen.
		/// </summary>
		/// <value>The width.</value>
		public int Width 
		{
			get 
			{ 
				return this.width.Value;
			}
		}

		///<summary>
		///Sets the screen to always the on.
		///</summary>
		///<param name="flag">If set to <c>true</c>, than the screen is always on.</param>
		public void AlwaysOn(bool flag)
		{
            var display = new Windows.System.Display.DisplayRequest();

            if (flag)
            {
                display.RequestActive();
            }
            else
            {
                display.RequestRelease();
            }
		}

		#endregion
	}
}

