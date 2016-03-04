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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.WP8.Services.Device.Measure))]

namespace WF.Player.WP8.Services.Device
{
    using System;
    using WF.Player.Services.Device;
    using Windows.UI.Xaml.Controls;

    public class Measure : IMeasure
	{
		#region IMeasure implementation

		public float ButtonTextSize(string text, double fontSize)
		{
            var b = new TextBlock();
            b.FontSize = fontSize;
            b.Text = text;
            b.Measure(new Windows.Foundation.Size(Xamarin.Forms.DependencyService.Get<IScreen>().Width, Xamarin.Forms.DependencyService.Get<IScreen>().Height));

            return (int)Math.Ceiling(b.DesiredSize.Width);
        }

		#endregion
	}
}

