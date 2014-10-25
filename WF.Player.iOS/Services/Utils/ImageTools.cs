// <copyright file="ImageTools.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>
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

namespace WF.Player.iOS.Services.Utils
{
	using System;
	using System.Drawing;
	using MonoTouch.CoreGraphics;
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;
	using WF.Player.Services.Utils;

	/// <summary>
	/// Image tools.
	/// </summary>
	public class ImageTools : IImageTools
	{
		public ImageTools()
		{
		}

		/// <summary>
		/// Resize the image in byte array originalImageData to reqWidth and reqHeight.
		/// </summary>
		/// <remarks>If one of width or height is 0, the image is resized preserving the ratio.</remarks>
		/// <param name="originalImageData">Original image as byte array.</param>
		/// <param name="reqWidth">Req width.</param>
		/// <param name="reqHeight">Req height.</param>
		public byte[] Resize(byte[] originalImageData, int reqWidth, int reqHeight)
		{
			// Found at http://forums.xamarin.com/discussion/5697/resize-an-image-while-retaining-quality
			// and at http://bortolu.wordpress.com/2014/03/21/xamarin-c-how-to-convert-byte-array-to-uiimage-with-an-extension-method/

			if (originalImageData == null) 
			{
				return null;
			}

			UIImage image = null;

			try
			{
				image = new UIImage(NSData.FromArray(originalImageData));
			}
			catch (Exception)
			{
				return null;
			}

			UIGraphics.BeginImageContext(new System.Drawing.SizeF(reqWidth, reqHeight));

			CGContext context = UIGraphics.GetCurrentContext();
			context.InterpolationQuality = CGInterpolationQuality.None;

			context.TranslateCTM(0, reqHeight);
			context.ScaleCTM(1f, -1f);

			context.DrawImage(new RectangleF(0, 0, reqWidth, reqHeight), image.CGImage);

			image.Dispose();
			image = null;

			using (var scaledImage = UIGraphics.GetImageFromCurrentImageContext())
			{
				UIGraphics.EndImageContext();

				if (scaledImage == null)
				{
					return null;
				}

				NSData data = null;

				try
				{
					data = scaledImage.AsPNG();
					return data.ToArray();
				}
				catch (Exception)
				{
					return null;
				}
				finally
				{
					data.Dispose();
					data = null;
				}
			}
		}
	}
}

