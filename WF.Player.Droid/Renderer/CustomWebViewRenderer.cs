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
using WF.Player.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Webkit;

[assembly: ExportRendererAttribute (typeof (CustomWebView), typeof (CustomWebViewRenderer))]

namespace WF.Player.Droid
{
	// Found at http://forums.xamarin.com/discussion/22426/webview-horizontal-scrolling-issue

	public class CustomWebViewRenderer : WebRenderer
	{
		public CustomWebViewRenderer() : base()
		{
			this.SetBackgroundColor(App.Colors.Background.ToAndroid());
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.WebView> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null) {
				return;
			}

			// Lets get a reference to the native control
			var webView = (global::Android.Webkit.WebView)Control;

			// Do whatever you want to the WebView here!
			webView.SetInitialScale(-1);
			webView.Settings.JavaScriptEnabled = false;
			webView.Settings.DefaultZoom = WebSettings.ZoomDensity.Far;
			webView.Settings.SetSupportZoom(true);
			webView.Settings.BuiltInZoomControls = true;
			webView.Settings.DisplayZoomControls = false;
			webView.VerticalFadingEdgeEnabled = false;
		}
	}
}

