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
using WF.Player;
using WF.Player.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRendererAttribute (typeof (CustomWebView), typeof (CustomWebViewRenderer))]

namespace WF.Player.iOS
{
	// Found at http://forums.xamarin.com/discussion/22426/webview-horizontal-scrolling-issue

	public class CustomWebViewRenderer : WebViewRenderer
	{
		public CustomWebViewRenderer() : base()
		{
		}

		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				return;
			}

			// Set new size for the UIWebView. Only set width, height is correct.
			Frame = new CoreGraphics.CGRect (0, 0, UIScreen.MainScreen.Bounds.Width, Frame.Height);

			var webView = this;

			// Allow user to zoom the content
			webView.ScalesPageToFit = true;
			// Don't bounce at the end of the screen
			webView.ScrollView.Bounces = false;
		}
	}
}

