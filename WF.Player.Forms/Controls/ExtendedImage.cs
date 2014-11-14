// <copyright file="ExtendedImage.cs" company="Wherigo Foundation">
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

namespace WF.Player.Controls
{
	using System;
	using Xamarin.Forms;

	public class ExtendedImage : Image
	{
		protected override void OnPropertyChanged(string propertyName)
		{
			if (propertyName.Equals("Source"))
			{
			}

			base.OnPropertyChanged(propertyName);
		}

		protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
		{
			var sizeRequest = base.OnSizeRequest(widthConstraint, heightConstraint);

			if (sizeRequest.Request.IsZero)
			{
				return sizeRequest;
			}

			if (sizeRequest.Request.Width > 0 && sizeRequest.Request.Height > 0)
			{
				// Now we have the real size of the image
				if (App.Prefs.ImageResize == ImageResize.NoResize)
				{
					// We don't have to resize the image
					Aspect = Aspect.AspectFit;

					return sizeRequest;
				}

				if (App.Prefs.ImageResize == ImageResize.ShrinkWidth && sizeRequest.Request.Width > widthConstraint)
				{
					// Images, which are bigger than width should be shrinked
					Aspect = Aspect.AspectFill;

					var aspect = sizeRequest.Request.Height / sizeRequest.Request.Width;

					sizeRequest.Request = new Size(widthConstraint, widthConstraint * aspect);
					sizeRequest.Minimum = sizeRequest.Request;

					return sizeRequest;
				}

				if (App.Prefs.ImageResize == ImageResize.ResizeWidth)
				{
					// Images, which are smaller/bigger than width should be inflated/shrinked
					Aspect = Aspect.AspectFill;

					var aspect = sizeRequest.Request.Height / sizeRequest.Request.Width;

					sizeRequest.Request = new Size(widthConstraint, widthConstraint * aspect);
					sizeRequest.Minimum = sizeRequest.Request;

					return sizeRequest;
				}

				if (App.Prefs.ImageResize == ImageResize.ResizeHeight)
				{
					// Images, which are heighter than half of the screen should be shrinked
					Aspect = Aspect.AspectFit;

					var aspect = sizeRequest.Request.Height / sizeRequest.Request.Width;

					if (widthConstraint * aspect > 320)
					{
						sizeRequest.Request = new Size(320 / aspect, 320); //double.IsPositiveInfinity(heightConstraint) ? new Size(320 / aspect, 320) : new Size(widthConstraint * 320 / heightConstraint, 320);
						sizeRequest.Minimum = sizeRequest.Request;

						return sizeRequest;
					}

					sizeRequest.Request = new Size(widthConstraint, widthConstraint * aspect);
					sizeRequest.Minimum = sizeRequest.Request;

					return sizeRequest;
				}
			}

			return sizeRequest;
		}
	}
}

