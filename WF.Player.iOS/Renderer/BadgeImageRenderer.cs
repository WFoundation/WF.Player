// // <copyright file="BadgeImageRenderer.cs" company="Wherigo Foundation">
// //   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// //   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// // </copyright>
//
// // This program is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as
// // published by the Free Software Foundation, either version 3 of the
// // License, or (at your option) any later version.
// //
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using Xamarin.Forms.Platform.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Xamarin.Forms;
using System.Drawing;
using MonoTouch.CoreGraphics;
using WF.Player.Controls;
using WF.Player.Controls.iOS;

[assembly: ExportRendererAttribute (typeof (BadgeImage), typeof (BadgeImageRenderer))]

namespace WF.Player.Controls.iOS
{
	public class BadgeImageRenderer : ImageRenderer
	{
		UIImage _image;

		public override void Draw(System.Drawing.RectangleF rect)
		{
			base.Draw(rect);

//			BadgeImage image = (BadgeImage)Element;
//
//			if (image.Number <= 0)
//				return;

//			using (var context = UIGraphics.GetCurrentContext()) 
//			{
//				// Save active state of context
//				context.SaveState();
//
//				// Calc text size
//				float fontSize = 14f;
//
//				var text = new NSString(image.Number.ToString()); //, UIFont.BoldSystemFontOfSize(fontSize), Color.White.ToUIColor(), Color.Red.ToUIColor());
//				var attr = new UIStringAttributes();
//
//				attr.Font = Font.SystemFontOfSize(fontSize).WithAttributes(FontAttributes.Bold).ToUIFont();
//				attr.ForegroundColor = Color.White.ToUIColor();
//				attr.BackgroundColor = Color.Transparent.ToUIColor();
//
//				var textSize = text.GetSizeUsingAttributes(attr);
//
//				var badgeWidth = textSize.Width + 9;
//				var badgeHeight = textSize.Height + 9;
//
//				if (badgeWidth < badgeHeight)
//					badgeWidth = badgeHeight;
//
//				float left = (float)(rect.Width - badgeWidth);
//				float top = (float)(rect.Height - badgeHeight);
//
//				using (UIBezierPath path = UIBezierPath.FromRoundedRect(new RectangleF(left, top, badgeWidth, badgeHeight), UIRectCorner.AllCorners, new System.Drawing.SizeF(10, 10))) {
//					context.SetFillColor(Color.Red.ToCGColor());
//					context.SetStrokeColor(Color.Red.ToCGColor());
//					context.SetLineWidth(0.0f);
//
//					context.AddPath(path.CGPath);
//					context.DrawPath(CGPathDrawingMode.FillStroke);
//				}
//
//				text.DrawString(new PointF(left + textSize.Width, top), attr);
//
//				// Restore saved state of context
//				context.RestoreState();
//			}
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			_image = this.Control.Image;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == BadgeImage.NumberProperty.PropertyName) {
				UpdateImage();
				SetNeedsDisplay();
			}
		}

		void UpdateImage()
		{
			UIImage newImage;
			string number = ((BadgeImage)Element).Number.ToString();
			SizeF size = new SizeF(_image.Size.Width, _image.Size.Height);

			// Begin a graphics context of sufficient size
			UIGraphics.BeginImageContextWithOptions(size, false, 0f);

			// Draw original image into the context
			_image.Draw(new PointF(0, 0));

			// Get the context for CoreGraphics
			using (var context = UIGraphics.GetCurrentContext()) {

				if (((BadgeImage)Element).Number > 0) {
					// Save active state of context
					context.SaveState();

					// Calc text size
					float fontSize = 18f;

					var text = new NSString(number); //, UIFont.BoldSystemFontOfSize(fontSize), Color.White.ToUIColor(), Color.Red.ToUIColor());
					var attr = new UIStringAttributes();

					attr.Font = Font.SystemFontOfSize(fontSize).ToUIFont(); //WithAttributes(FontAttributes.Bold).ToUIFont();
					attr.ForegroundColor = Color.White.ToUIColor();
					attr.BackgroundColor = Color.Transparent.ToUIColor();
					attr.ParagraphStyle = new NSParagraphStyle();

					var textSize = text.GetSizeUsingAttributes(attr);

					var badgeWidth = textSize.Width + 9;
					var badgeHeight = textSize.Height + 0;

					if (badgeWidth < badgeHeight)
						badgeWidth = badgeHeight;

					float left = (float)(size.Width - badgeWidth);
					float top = (float)(size.Height - badgeHeight);

					using (UIBezierPath path = UIBezierPath.FromRoundedRect(new RectangleF(left, top, badgeWidth, badgeHeight), UIRectCorner.AllCorners, new System.Drawing.SizeF(10, 10))) {
						context.SetFillColor(Color.Red.ToCGColor());
						context.SetStrokeColor(Color.Red.ToCGColor());
						context.SetLineWidth(0.0f);

						context.AddPath(path.CGPath);
						context.DrawPath(CGPathDrawingMode.FillStroke);
					}

					text.DrawString(new PointF(left + (badgeWidth - textSize.Width) / 2f, top), attr);

					// Restore saved state of context
					context.RestoreState();
				}

				// make image out of bitmap context
				newImage = UIGraphics.GetImageFromCurrentImageContext();
			}

			// free the context
			UIGraphics.EndImageContext();

			this.Control.Image = newImage;
		}
	}
}

