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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.Device.Measure))]

namespace WF.Player.Droid.Services.Device
{
	using System;
	using Android.Content.Res;
	using Android.Graphics;
	using WF.Player.Services.Device;
	using Xamarin.Forms;

	public class Measure : IMeasure
	{
		static global::Android.Widget.Button _button;

		public float ButtonTextSize(string text, double fontSize)
		{
			if (_button == null) {
				_button = new global::Android.Widget.Button(Forms.Context);
				_button.SetPadding(10, _button.PaddingTop, 10, _button.PaddingBottom);
//				_button.SetPadding(0, _button.PaddingTop, 0, _button.PaddingBottom);

			}

			if (fontSize != 0)
			{
				_button.TextSize = (float)fontSize;
			}

			var bounds = new Rect();
			_button.Paint.GetTextBounds(text, 0, text.Length, bounds);
			return (bounds.Left + bounds.Width() + _button.PaddingLeft + _button.PaddingRight) / Resources.System.DisplayMetrics.Density;
		}
	}
}

