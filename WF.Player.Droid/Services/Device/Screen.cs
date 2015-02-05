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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.Device.Screen))]

namespace WF.Player.Droid.Services.Device
{
	using WF.Player.Services.Device;

	/// <summary>
	/// Screen implementation.
	/// </summary>
	public class Screen : IScreen
	{
		#region IScreen implementation

		///<summary>
		///Sets the screen to always the on.
		///</summary>
		///<param name="flag">If set to <c>true</c>, than the screen is always on.</param>
		public void AlwaysOn(bool flag)
		{
			((MainActivity)Xamarin.Forms.Forms.Context).Window.DecorView.KeepScreenOn = flag;
		}

		#endregion
	}
}

