// <copyright file="Measure.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>
//
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
//
using System;
using Xamarin.Forms;
using WF.Player.Services.Device;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

[assembly: Dependency(typeof(WF.Player.iOS.Services.Device.Measure))]

namespace WF.Player.iOS.Services.Device
{
	public class Measure : IMeasure
	{
		static UIButton _button;

		public float ButtonTextSize(string text)
		{
			if (_button == null) {
				_button = new UIButton();
				_button.Font = UIFont.SystemFontOfSize((float)App.Prefs.TextSize * 0.8f);
			}

			NSString nsText = new NSString(text);
			return nsText.GetSizeUsingAttributes(new UIStringAttributes() { Font = UIFont.SystemFontOfSize((float)(App.Prefs.TextSize*0.8)) }).Width + _button.ContentEdgeInsets.Left + _button.ContentEdgeInsets.Right;
		}
	}
}

