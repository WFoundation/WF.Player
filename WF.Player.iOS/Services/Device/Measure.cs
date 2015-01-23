// <copyright file="Measure.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.iOS.Services.Device.Measure))]

namespace WF.Player.iOS.Services.Device
{
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;
	using WF.Player.Services.Settings;
	using WF.Player.Services.Device;

	public class Measure : IMeasure
	{
		private static UIButton button;
		private const float textSize = 20f;

		public float ButtonTextSize(string text)
		{
			if (button == null) {
				button = new UIButton();
				button.Font = UIFont.SystemFontOfSize(Settings.FontSize);
			}

			NSString nsText = new NSString(text);
			return nsText.GetSizeUsingAttributes(new UIStringAttributes() { Font = UIFont.SystemFontOfSize(textSize) }).Width + button.ContentEdgeInsets.Left + button.ContentEdgeInsets.Right;
		}
	}
}

