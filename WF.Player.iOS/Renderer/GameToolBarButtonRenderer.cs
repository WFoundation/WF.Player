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
using System.Drawing;
using MonoTouch.UIKit;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.CoreGraphics;
using WF.Player;
using WF.Player.Controls;
using WF.Player.Controls.iOS;
using MonoTouch.Foundation;

[assembly: ExportRendererAttribute (typeof (GameToolBarButton), typeof (GameToolBarButtonRenderer))]

namespace WF.Player.Controls.iOS
{
	public class GameToolBarButtonRenderer : FrameRenderer
	{
		public override void Draw (System.Drawing.RectangleF rect)
		{
			GameToolBarButton button = (GameToolBarButton)Element;

			var bounds = button.Bounds.ToRectangleF();

			base.Draw(rect);

			using (var context = UIGraphics.GetCurrentContext()) {
				// Save active state of context
				context.SaveState();

				// Draw selected marker
				if (button.Selected) {
//					using (CGPath path = new CGPath()) {
//
//						context.SetFillColor(button.SelectedColor.ToCGColor());
//						context.SetStrokeColor(button.SelectedColor.ToCGColor());
//						context.SetLineWidth(0.0f);
//
//						float center = (float)bounds.Width / 2.0f;
//						float size = 8.0f;
//
//						PointF[] points = new PointF[3];
//
//						points[0] = new PointF(center, size); //(float)bounds.Height - size);
//						points[1] = new PointF(center + size, 0f); // (float)bounds.Height);
//						points[2] = new PointF(center - size, 0f); // (float)bounds.Height);
//
//						path.AddLines(points);
//						path.CloseSubpath();
//
//						context.AddPath(path);
//						context.DrawPath(CGPathDrawingMode.FillStroke);
//					}
				}

				// Restore saved state of context
				context.RestoreState();
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == GameToolBarButton.SelectedProperty.PropertyName
			    || e.PropertyName == GameToolBarButton.SelectedColorProperty.PropertyName)
			{
				((GameToolBarButton)Element).Image.Selected = ((GameToolBarButton)Element).Selected;
				SetNeedsDisplay();
			}
		}
	}
}

