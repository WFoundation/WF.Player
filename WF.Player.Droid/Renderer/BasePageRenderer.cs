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
using Android.App;
using WF.Player;
using WF.Player.Droid;

[assembly: ExportRendererAttribute(typeof(BasePage), typeof(BasePageRenderer))]

namespace WF.Player.Droid
{
	public class BasePageRenderer : PageRenderer
	{
		protected override void OnDraw (global::Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);

			var element = (BasePage)Element;
			var actionBar = ((Activity) Context).ActionBar;

			if (App.GameNavigation != null )
			{
				// Do this, because all other places are to late
				if (App.GameNavigation.CurrentPage is GameCheckLocationView)
				{
					App.GameNavigation.ShowBackButton = true;
					Invalidate();
				}

				actionBar.SetHomeButtonEnabled (App.GameNavigation.ShowBackButton);			// Don't activate button behavior
				actionBar.SetDisplayHomeAsUpEnabled (App.GameNavigation.ShowBackButton);	// Don't show back arrow
				actionBar.SetDisplayShowHomeEnabled (App.GameNavigation.ShowBackButton);	// Don't show back arrow and icon
			}
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);

			if (App.Colors.IsDarkTheme)
				((Activity) this.Context).SetTheme (Resource.Style.AppTheme_Dark);
			else
				((Activity) this.Context).SetTheme (Resource.Style.AppTheme_Light);
		}
	}
}

