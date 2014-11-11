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
using Xamarin.Forms.Platform.iOS;
using WF.Player;
using WF.Player.iOS;

[assembly: ExportRendererAttribute(typeof(BottomBarPage), typeof(BottomBarPageRenderer))]

namespace WF.Player.iOS
{
	public class BottomBarPageRenderer : PageRenderer
	{
		public override void ViewWillAppear(bool animated)
		{
			if (NavigationController != null)
			{
				NavigationController.SetNavigationBarHidden(false, false);
			}

			base.ViewWillAppear(animated);

			// Did this, because Xamarin.Forms couldn't set HasBackButton correct.
			ViewController.ParentViewController.NavigationItem.SetHidesBackButton(!((BottomBarPage)this.Element).HasBackButton, false);

			if (NavigationController == null)
			{
				return;
			}

			// Did this, because Xamarin.Forms don't set the TintColor correct for ToolbarItems.
			foreach(var i in this.NavigationController.NavigationBar.Items)
				foreach (var o in i.RightBarButtonItems)
					o.TintColor = App.Colors.Tint.ToUIColor ();
		}

		public override void ViewDidAppear(bool animated)
		{
			animated = false;

			base.ViewDidAppear(animated);
		}

		public override void ViewWillDisappear (bool animated)
		{
			animated = false;

			base.ViewWillDisappear (animated);
		}
	}
}

