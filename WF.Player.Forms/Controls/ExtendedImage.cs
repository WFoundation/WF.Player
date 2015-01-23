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
using WF.Player.Services.Settings;

namespace WF.Player.Controls
{
	using System;
	using Xamarin.Forms;

	/// <summary>
	/// Extended image.
	/// </summary>
	public class ExtendedImage : Image
	{
		/// <param name="propertyName">The name of the property that changed.</param>
		/// <summary>
		/// Call this method from a child class to notify that a change happened on a property.
		/// </summary>
		protected override void OnPropertyChanged(string propertyName)
		{
			if (propertyName.Equals("Source"))
			{
			}

			base.OnPropertyChanged(propertyName);
		}

		/// <summary>
		/// This method is called during the measure pass of a layout cycle to get the desired size of an element.
		/// </summary>
		/// <returns>New size of element.</returns>
		/// <param name="widthConstraint">The available width for the element to use.</param>
		/// <param name="heightConstraint">The available height for the element to use.</param>
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
				if (Settings.ImageResize == ImageResize.NoResize)
				{
					// We don't have to resize the image
					Aspect = Aspect.AspectFit;

					return sizeRequest;
				}

				if (Settings.ImageResize == ImageResize.ShrinkWidth && sizeRequest.Request.Width > widthConstraint)
				{
					// Images, which are bigger than width should be shrinked
					Aspect = Aspect.AspectFill;

					var aspect = sizeRequest.Request.Height / sizeRequest.Request.Width;

					sizeRequest.Request = new Size(widthConstraint, widthConstraint * aspect);
					sizeRequest.Minimum = sizeRequest.Request;

					return sizeRequest;
				}

				if (Settings.ImageResize == ImageResize.ResizeWidth)
				{
					// Images, which are smaller/bigger than width should be inflated/shrinked
					Aspect = Aspect.AspectFill;

					var aspect = sizeRequest.Request.Height / sizeRequest.Request.Width;

					sizeRequest.Request = new Size(widthConstraint, widthConstraint * aspect);
					sizeRequest.Minimum = sizeRequest.Request;

					return sizeRequest;
				}

				if (Settings.ImageResize == ImageResize.ResizeHeight)
				{
					// Images, which are heighter than half of the screen should be shrinked
					Aspect = Aspect.AspectFit;

					var aspect = sizeRequest.Request.Height / sizeRequest.Request.Width;

					// TODO: Get correct screen size
					if (widthConstraint * aspect > 320)
					{
						sizeRequest.Request = new Size(320 / aspect, 320);
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
