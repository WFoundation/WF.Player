// <copyright file="PinchScrollViewRenderer.cs" company="Wherigo Foundation">
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

using System;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using Xamarin.Forms;
using WF.Player.Controls;
using WF.Player.Controls.iOS;

[assembly: ExportRendererAttribute(typeof(PinchScrollView), typeof(PinchScrollViewRenderer))]

namespace WF.Player.Controls.iOS
{
	public class PinchScrollViewRenderer : ScrollViewRenderer
	{
		/// <summary>
		/// The zoom scale.
		/// </summary>
		private float zoomScale = 1;

		protected override void OnElementChanged(VisualElementChangedEventArgs e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null) {
				this.MaximumZoomScale = 4f;
				this.MinimumZoomScale = 1f;
				this.ViewForZoomingInScrollView += (UIScrollView sv) => { return this.Subviews[0]; };
				this.DidZoom += UpdateZoom;
			}
		}

		private void UpdateZoom(object sender, EventArgs ea)
		{
			zoomScale = this.ZoomScale;
		}
	}
}

