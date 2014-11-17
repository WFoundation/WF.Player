// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz <mail@wfplayer.com>
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

using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using WF.Player.Droid;
using System.Drawing;
using Android.App;

[assembly: ExportRendererAttribute(typeof(NavigationPage), typeof(CustomNavigationRenderer))]

namespace WF.Player.Droid
{
	public class CustomNavigationRenderer : NavigationRenderer
	{
		protected override System.Threading.Tasks.Task<bool> OnPopViewAsync (Page page, bool animated)
		{
			animated = false;

			return base.OnPopViewAsync (page, animated);
		}

		protected override System.Threading.Tasks.Task<bool> OnPushAsync (Page page, bool animated)
		{
			animated = false;

			return base.OnPushAsync (page, animated);
		}
	}
}
