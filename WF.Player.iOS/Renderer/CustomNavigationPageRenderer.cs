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

[assembly: ExportRendererAttribute(typeof(NavigationPage), typeof(CustomNavigationPageRenderer))]

namespace WF.Player.iOS
{
	public class CustomNavigationPageRenderer : NavigationRenderer
	{
		public CustomNavigationPageRenderer()
		{
//			NavigationBar.BarStyle = App.Colors.IsDarkTheme ? UIBarStyle.Black : UIBarStyle.Default;
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
				context.SetFillColorWithColor (App.Colors.Bar.ToCGColor());
				context.FillRect(new RectangleF(0, 0, size.Width, size.Height));
				image = UIGraphics.GetImageFromCurrentImageContext();
			}
			UIGraphics.EndImageContext();

			// Set values for the NavigationBar
			NavigationBar.TintColor = App.Colors.Tint.ToUIColor();
			NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
			NavigationBar.BarStyle = App.Colors.IsDarkTheme ? UIBarStyle.Black : UIBarStyle.Default;
//			UIApplication.SharedApplication.SetStatusBarHidden (false, false);
		}

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			var backButton = new UIBarButtonItem();
			backButton.Title = "Test";

			NavigationItem.LeftBarButtonItem = backButton;
		}
	}
}

