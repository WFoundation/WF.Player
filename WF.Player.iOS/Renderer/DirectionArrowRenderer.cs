// /// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// /// Copyright (C) 2012-2014  Dirk Weltz <mail@wfplayer.com>
// ///
// /// This program is free software: you can redistribute it and/or modify
// /// it under the terms of the GNU Lesser General Public License as
// /// published by the Free Software Foundation, either version 3 of the
// /// License, or (at your option) any later version.
// /// 
// /// This program is distributed in the hope that it will be useful,
// /// but WITHOUT ANY WARRANTY; without even the implied warranty of
// /// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// /// GNU Lesser General Public License for more details.
// /// 
// /// You should have received a copy of the GNU Lesser General Public License
// /// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
//
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;
using CoreGraphics;
using WF.Player.Controls;
using WF.Player.Controls.iOS;
using System.Collections.Generic;

[assembly: ExportRendererAttribute(typeof(DirectionArrow), typeof(DirectionArrowRenderer))]

namespace WF.Player.Controls.iOS
{
	public class DirectionArrowRenderer : ViewRenderer
	{
		public override void Draw (CGRect rect)
		{
			DirectionArrow dv = (DirectionArrow)Element;

			var bounds = Bounds;

			float width;
			float height;

			width = height = (float)Math.Min(Bounds.Width, Bounds.Height);

			float centerX = width / 2.0f;
			float centerY = height / 2.0f;
			float size = (width * 0.8f) / 2.0f;
			float sizeSmall = size * 0.6f;

			// Draw background circle
			using (var context = UIGraphics.GetCurrentContext ()) {
				context.SetFillColor (dv.CircleColor.ToCGColor ());
				context.SetStrokeColor (dv.CircleColor.ToCGColor ());
				context.SetLineWidth (0.0f);
				// Draw circle
				using (CGPath path = new CGPath ()) {

					// Set colors and line widhts
					context.SetLineWidth (0.0f);
					context.SetStrokeColor (dv.CircleColor.ToCGColor ());
					context.SetFillColor (dv.CircleColor.ToCGColor ());
					// Draw circle
					path.AddArc (centerX, centerY, centerX, 0.0f, (float)Math.PI * 2.0f, true);
					path.CloseSubpath ();
					// Draw path
					context.AddPath (path);
					context.DrawPath (CGPathDrawingMode.FillStroke);

				}
			}

			// Check, if we inside of the zone
			if (double.IsPositiveInfinity(dv.Direction)) {
				// Draw circle, because we are here
				using (var context = UIGraphics.GetCurrentContext ()) {
					context.SetFillColor (dv.ArrowColor.ToCGColor ());
					context.SetStrokeColor (dv.ArrowColor.ToCGColor ());
					context.SetLineWidth (5.0f);
					// Draw circle
					using (CGPath path = new CGPath ()) {
						// Draw circle
						path.AddArc (centerX, centerY, centerX * 0.3f, 0.0f, (float)Math.PI * 2.0f, true);
						path.CloseSubpath ();
						// Draw path
						context.AddPath (path);
						context.DrawPath (CGPathDrawingMode.FillStroke);
					}
				}

				return;
			}

			// Check, if the direction is invalid
			if (double.IsNegativeInfinity(dv.Direction)) {
				// Draw cross, because direction is invalid
				using (var context = UIGraphics.GetCurrentContext()) {
					context.SetFillColor(dv.ArrowColor.ToCGColor());
					context.SetStrokeColor(dv.ArrowColor.ToCGColor());
					context.SetLineWidth(0.1875f * width);
					context.SetLineCap(CGLineCap.Round);
					// Draw circle
					using (CGPath path = new CGPath ()) {
						// Draw cross
						var points = new List<CGPoint>(2);

						points.Add(new CGPoint(centerX - centerX * 0.4f, centerY - centerY * 0.4f));
						points.Add(new CGPoint(centerX + centerX * 0.4f, centerY + centerY * 0.4f));

						path.AddLines(points.ToArray());

						path.CloseSubpath ();
						context.AddPath (path);

						points = new List<CGPoint>(2);

						points.Add(new CGPoint(centerX - centerX * 0.4f, centerY + centerY * 0.4f));
						points.Add(new CGPoint(centerX + centerX * 0.4f, centerY - centerY * 0.4f));

						path.AddLines(points.ToArray());

						path.CloseSubpath ();
						context.AddPath (path);

						context.SetLineCap(CGLineCap.Round);
						context.DrawPath(CGPathDrawingMode.Stroke);
					}
				}

				return;
			}

			// We have a direction, so draw an arrow
			var direction = 180.0 - dv.Direction;
			if (direction < 0)
				direction += 360.0;
			direction = direction % 360.0;

			double rad1 = direction / 180.0 * Math.PI;
			double rad2 = (direction + 180.0 + 30.0) / 180.0 * Math.PI;
			double rad3 = (direction + 180.0 - 30.0) / 180.0 * Math.PI; 
			double rad4 = (direction + 180.0) / 180.0 * Math.PI; 

			CGPoint p1 = new CGPoint((float) (centerX + size * Math.Sin (rad1)), (float) (centerY + size * Math.Cos (rad1)));
			CGPoint p2 = new CGPoint((float) (centerX + size * Math.Sin (rad2)), (float) (centerY + size * Math.Cos (rad2)));
			CGPoint p3 = new CGPoint((float) (centerX + size * Math.Sin (rad3)), (float) (centerY + size * Math.Cos (rad3)));
			CGPoint p4 = new CGPoint((float) (centerX + sizeSmall * Math.Sin (rad4)), (float) (centerY + sizeSmall * Math.Cos (rad4)));

			using (var context = UIGraphics.GetCurrentContext ()) {

				// Draw first half of arrow
				using (CGPath path = new CGPath ()) {

					context.SetLineWidth (0.5f);
					context.SetStrokeColor (dv.ArrowColor.ToCGColor ()); //1f, 0, 0, 1);
					context.SetFillColor (dv.ArrowColor.ToCGColor ());

					path.AddLines (new CGPoint[] { p1, p2, p4 });
					path.CloseSubpath ();

					context.AddPath (path);
					context.DrawPath (CGPathDrawingMode.FillStroke);

				}

				// Draw second half of arrow
				using (CGPath path = new CGPath()) {

					context.SetStrokeColor (dv.ArrowColor.ToCGColor ());
					context.SetFillColor (dv.ArrowColor.ToCGColor ()); // was 0.5f
				
					path.AddLines (new CGPoint[] { p1, p3, p4 });
					path.CloseSubpath ();

					context.AddPath (path);
					context.DrawPath (CGPathDrawingMode.FillStroke);

				}
			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.NewElement == null)
			{
				return;
			}

			// Set a normal view as underlaying control
			var view = new UIView();

			SetNativeControl (view);
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			bool update = false;

			if (e.PropertyName == "Renderer") {
				update = true;
			}

			if (e.PropertyName == DirectionArrow.ArrowColorProperty.PropertyName) {
				update = true;
			}

			if (e.PropertyName == DirectionArrow.CircleColorProperty.PropertyName) {
				update = true;
			}

			if (e.PropertyName == DirectionArrow.DirectionProperty.PropertyName) {
				update = true;
			}

			if (!update)
				return;

			SetNeedsDisplay();
		}
	}
}

