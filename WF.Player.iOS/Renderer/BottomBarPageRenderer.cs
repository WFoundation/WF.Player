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
using MonoTouch.UIKit;
using MonoTouch.Foundation;

[assembly: ExportRendererAttribute(typeof(BottomBarPage), typeof(BottomBarPageRenderer))]

namespace WF.Player.iOS
{
	public class BottomBarPageRenderer : PageRenderer
	{
		/// <summary>
		/// The observer for hiding keyboard.
		/// </summary>
		private NSObject observerHideKeyboard;

		/// <summary>
		/// The observer for showing keyboard.
		/// </summary>
		private NSObject observerShowKeyboard;

		public override void ViewWillAppear(bool animated)
		{
			if (NavigationController != null)
			{
				NavigationController.SetNavigationBarHidden(false, false);
			}

			base.ViewWillAppear(animated);

			// Register for keyboard notifications
			observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);

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
//
//			UIBarButtonItem *back = [[UIBackButtonItem alloc] initWithTitle:@"\U000025C0\U0000FE0E" style:UIBarButtonItemStylePlain target:self action:@selector(back:)];
//			[self.navigationItem setLeftBarButtonItem:back]
//			var backButton = new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender, args) => {});
//			backButton.Title = @"\U000025C0\U0000FE0E";
//			var temp = this.NavigationController.NavigationItem.LeftBarButtonItem;
//			NavigationItem.SetLeftBarButtonItem(backButton, false);
		}

		public override void ViewWillDisappear (bool animated)
		{
			animated = false;

			base.ViewWillDisappear (animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver(observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver(observerShowKeyboard);
		}

		/// <summary>
		/// Raised when keyboard is shown or hidden.
		/// </summary>
		/// <param name="notification">Notification.</param>
		private void OnKeyboardNotification (NSNotification notification)
		{
			if (!IsViewLoaded) return;

			var frameBegin = UIKeyboard.FrameBeginFromNotification(notification);
			var frameEnd = UIKeyboard.FrameEndFromNotification(notification);
			var bounds = Element.Bounds;
			var newBounds = new Rectangle(bounds.Left, bounds.Top, bounds.Width, bounds.Height - frameBegin.Top + frameEnd.Top);

			Element.Layout(newBounds);

			// Workaround, because ScrollView isn't set correctly (parts are not visible, if content was scrolled while keyboard is shown)
			if (Element is GameInputView)
			{
				var contentLayout = ((GameInputView)Element).ContentLayout;
				bounds = contentLayout.Bounds;
				bounds.Height += 1;
				contentLayout.Layout(bounds);
			}
		}
	}
}

