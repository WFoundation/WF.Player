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
using UIKit;
using Foundation;
using Xamarin.Forms;
using CoreGraphics;
using CoreGraphics;
using WF.Player.Controls;
using WF.Player.Controls.iOS;
using CoreImage;

[assembly: ExportRendererAttribute (typeof (BadgeImage), typeof (BadgeImageRenderer))]

namespace WF.Player.Controls.iOS
{
	public class BadgeImageRenderer : ImageRenderer
	{
		UIImage image;

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (Element == null)
			{
				return;
			}

			image = this.Control.Image;

			UpdateImage();
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == BadgeImage.NumberProperty.PropertyName) {
				UpdateImage();
				SetNeedsDisplay();
			}

			if (e.PropertyName == BadgeImage.SelectedProperty.PropertyName) {
				UpdateImage();
				SetNeedsDisplay();
			}
		}

		void UpdateImage()
		{
			UIImage newImage;
			string number = ((BadgeImage)Element).Number.ToString();
			CGSize size = new CGSize(image.Size.Width, image.Size.Height);

			// Begin a graphics context of sufficient size
			UIGraphics.BeginImageContextWithOptions(size, false, 0f);

			// Draw original image into the context
			if (((BadgeImage)Element).Selected)
			{
				image.Draw(new CGPoint(0, 0));
			}
			else
			{
				// Found at http://iosdevelopertips.com/graphics/convert-an-image-uiimage-to-grayscale.html

				// Create image rectangle with current image width/height
				CGRect imageRect = new CGRect(new CGPoint(0, 0), new CGSize(image.Size.Width * image.CurrentScale, image.Size.Width * image.CurrentScale));

				// Grayscale color space
				CGColorSpace colorSpace = CGColorSpace.CreateDeviceGray();
				CGImage grayImage;
				CGImage mask;

				// Create bitmap content with current image size and grayscale colorspace
				using (var context = new CGBitmapContext(null, (int)image.Size.Width * (int)image.CurrentScale, (int)image.Size.Height * (int)image.CurrentScale, 8, 0, colorSpace, CGImageAlphaInfo.None))
				{

					// Draw image into current context, with specified rectangle
					// using previously defined context (with grayscale colorspace)
					context.DrawImage(imageRect, image.CGImage);

					/* changes start here */
					// Create bitmap image info from pixel data in current context
					grayImage = context.ToImage();

					// release the colorspace and graphics context
					colorSpace.Dispose();
				}

				// Make a new alpha-only graphics context
				using (var context = new CGBitmapContext(null, (int)image.Size.Width * (int)image.CurrentScale, (int)image.Size.Height * (int)image.CurrentScale, 8, 0, CGColorSpace.Null, CGImageAlphaInfo.Only))
				{

					// Draw image into context with no colorspace
					context.DrawImage(imageRect, image.CGImage);

					// Create alpha bitmap mask from current context
					mask = context.ToImage();
				}

				// Make UIImage from grayscale image with alpha mask
				UIImage grayScaleImage = new UIImage(grayImage.WithMask(mask), image.CurrentScale, image.Orientation); //image.CurrentScale

				// Release the CG images
				grayImage.Dispose();
				mask.Dispose();

				grayScaleImage.Draw(new CGPoint(0, 0));
			}

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
					float top = 0; //(float)(size.Height - badgeHeight);

					using (UIBezierPath path = UIBezierPath.FromRoundedRect(new CGRect(left, top, badgeWidth, badgeHeight), UIRectCorner.AllCorners, new CGSize(10, 10))) 
					{
						var color = ((BadgeImage)Element).Selected ? Color.Red.ToCGColor() : Color.FromRgb(192, 192, 192).ToCGColor();

						context.SetFillColor(color);
						context.SetStrokeColor(color);
						context.SetLineWidth(0.0f);

						context.AddPath(path.CGPath);
						context.DrawPath(CGPathDrawingMode.FillStroke);
					}

					text.DrawString(new CGPoint(left + (badgeWidth - textSize.Width) / 2f, top), attr);

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

