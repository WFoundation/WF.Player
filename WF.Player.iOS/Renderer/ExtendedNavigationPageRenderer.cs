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
using WF.Player.iOS;
using MonoTouch.UIKit;
using MonoTouch.CoreGraphics;
using System.Drawing;
using System.Linq;

[assembly: ExportRendererAttribute(typeof(WF.Player.Controls.ExtendedNavigationPage), typeof(WF.Player.Controls.iOS.ExtendedNavigationPageRenderer))]

namespace WF.Player.Controls.iOS
{
	public class ExtendedNavigationPageRenderer : NavigationRenderer
	{
		private bool animation;
		private UIBarButtonItem leftIconBarButtonItem;

		public ExtendedNavigationPageRenderer()
		{
			animation = true;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// TODO
			// Color of navigation bar should change, if the settings are changed

			// Create a one color background image
			UIImage image;

			// Create image
			SizeF size = new SizeF(1, 1);
			UIGraphics.BeginImageContext(size);
			using (CGContext context = UIGraphics.GetCurrentContext()) {
				context.SetFillColor(App.Colors.Bar.ToCGColor());
				context.FillRect(new RectangleF(0, 0, size.Width, size.Height));
				image = UIGraphics.GetImageFromCurrentImageContext();
			}
			UIGraphics.EndImageContext();

			// Set values for the NavigationBar
			NavigationBar.TintColor = App.Colors.Tint.ToUIColor();
			NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
			NavigationBar.BarStyle = App.Colors.IsDarkTheme ? UIBarStyle.Black : UIBarStyle.Default;

			if (!animation)
			{
				CreateBackButton();
			}
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			animation = ((ExtendedNavigationPage)Element).Animation;

			if (!animation)
			{
				((NavigationPage)Element).PropertyChanged += OnElementPropertyChanged;
				((NavigationPage)Element).Pushed += HandlePushed;
				((NavigationPage)Element).Popped += HandlePopped;
			}
		}

		private void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "ShowBackButton" && !animation)
			{
				var item = NavigationBar.Items.Last();

				if (((ExtendedNavigationPage)Element).ShowBackButton)
				{
					if (leftIconBarButtonItem == null)
					{
						CreateBackButton();
					}

					item.LeftItemsSupplementBackButton = false;
					item.LeftBarButtonItem = leftIconBarButtonItem;
				}
				else
				{
					item.LeftBarButtonItem = null;
				}
			}
		}

		private void HandlePushed(object sender, NavigationEventArgs e)
		{
			var item = NavigationBar.Items.Last();

			// Do this, because Xamarin.Forms don't set the TintColor for the first time correct.
			foreach (var navBarItem in NavigationBar.Items)
			{
				foreach (var navBarRightItem in navBarItem.RightBarButtonItems)
				{
					navBarRightItem.TintColor = App.Colors.Tint.ToUIColor();
				}
			}

			if (((ExtendedNavigationPage)Element).ShowBackButton)
			{
				if (leftIconBarButtonItem == null)
				{
					CreateBackButton();
				}

				item.LeftItemsSupplementBackButton = false;
				item.LeftBarButtonItem = leftIconBarButtonItem;
			}
		}

		private void HandlePopped(object sender, NavigationEventArgs e)

		{
			var item = NavigationBar.Items.Last();

			if (((ExtendedNavigationPage)Element).ShowBackButton)
			{
				if (leftIconBarButtonItem == null)
				{
					CreateBackButton();
				}

				item.LeftItemsSupplementBackButton = false;
				item.LeftBarButtonItem = leftIconBarButtonItem;
			}
		}

		private void HandleBackButton(object sender, EventArgs e)
		{
			var navigationPage = (NavigationPage)Element;

			if (App.GameNavigation != null)
			{
				if (App.Game != null)
				{
					// We are in the game
					BeginInvokeOnMainThread(() => App.Game.ShowScreen(ScreenType.Last, null));
				}
				else
				{
					// We are on the check location screen
					// Remove active screen
					App.GameNavigation.CurrentPage.Navigation.PopModalAsync();
					App.GameNavigation = null;
				}
			}
			else
			{
				navigationPage.PopAsync(animation);
			}
		}

		private void CreateBackButton()
		{
			if (NavigationBar.Items.Count() == 0)
			{
				return;
			}

			// Handle new back button without animation
			UINavigationItem item = NavigationBar.Items[0];

			UIImage icon = UIImage.FromBundle("IconBack.png");

			var button = UIButton.FromType(UIButtonType.System);

			button.Bounds = new RectangleF(0, 0, icon.Size.Width, icon.Size.Height);
			button.SetImage(icon, UIControlState.Normal);
			button.ImageEdgeInsets = new UIEdgeInsets(0.5f, -8, -0.5f, 8);

			button.AddTarget(HandleBackButton, UIControlEvent.TouchUpInside);
			leftIconBarButtonItem = new UIBarButtonItem(button);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				var navigationPage = (NavigationPage)Element;

				navigationPage.PropertyChanged -= OnElementPropertyChanged;
				navigationPage.Pushed -= HandlePushed;
				navigationPage.Popped -= HandlePopped;

				if (leftIconBarButtonItem != null)
				{
					leftIconBarButtonItem.Dispose();
					leftIconBarButtonItem = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
