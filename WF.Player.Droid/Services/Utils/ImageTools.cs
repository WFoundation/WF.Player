// <copyright file="Image.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.Utils.ImageTools))]

namespace WF.Player.Droid.Services.Utils
{
	using System;
	using System.IO;
	using Android.Content.Res;
	using Android.Graphics;
	using Android.Util;
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
			// Found at http://forums.xamarin.com/discussion/7153/bitmap-resizing

			// First decode with inJustDecodeBounds=true to check dimensions
			var options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;
			options.InPurgeable = true;

			using (var image = BitmapFactory.DecodeByteArray(originalImageData, 0, originalImageData.Length, options))
			{
				image.Recycle();
			}

			var metrics = new DisplayMetrics();
			int width = (int)(reqWidth * metrics.Density);
			int height = (int)(reqHeight * metrics.Density);

			if (width == 0 && height == 0)
			{
				width = options.OutWidth;
				height = options.OutHeight;
			}
			else if (width == 0)
			{
				width = options.OutWidth * height / options.OutHeight;
			}
			else if (height == 0)
			{
				height = options.OutHeight * width / options.OutWidth;
			}

			// Calculate inSampleSize
			if (options.OutHeight > height || options.OutWidth > width)
			{
				options.InSampleSize = width > height ? options.OutHeight / height : options.OutWidth / width;
			}
			else
			{
				options.InSampleSize = 1;
			}

			// Decode bitmap with inSampleSize set
			options.InJustDecodeBounds = false;
			options.InPurgeable = true;

			// Decode image
			var bitmap = BitmapFactory.DecodeByteArray(originalImageData, 0, originalImageData.Length, options);

			// Resize image
			var bitmapScalled = Bitmap.CreateScaledBitmap(bitmap, reqWidth, reqHeight, true);

			// Recycle original image, so there is no memory leak
			bitmap.Recycle();

			// Convert new image to byte array
			using (var ms = new MemoryStream())
			{
				// Write new image to memory stream
				bitmapScalled.Compress(Bitmap.CompressFormat.Png, 90, ms);

				// Convert memory stream to byte array
				byte[] bitmapData = ms.ToArray();

				// Recycle image, so there is no memory leak
				bitmapScalled.Recycle();

				return bitmapData;
			}
		}
	}
}

