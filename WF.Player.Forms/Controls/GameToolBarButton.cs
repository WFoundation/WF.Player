// <copyright file="GameToolBarButton.cs" company="Wherigo Foundation">
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
	using Xamarin.Forms;

	/// <summary>
	/// Game tool bar button.
	/// </summary>
	public class GameToolBarButton : Frame
	{
		/// <summary>
		/// Badge image.
		/// </summary>
		private BadgeImage image;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Controls.GameToolBarButton"/> class.
		/// </summary>
		/// <param name="imageName">Image name.</param>
		public GameToolBarButton(string imageName) : base()
		{
			HasShadow = false;
			BackgroundColor = Color.Transparent;
			Padding = new Thickness(2, 4, 2, 2);

			image = new BadgeImage() 
			{
				Selected = false,
				BackgroundColor = Color.Transparent,
				Aspect = Aspect.AspectFit,
				Source = imageName,
			};

			Content = image; // layout;
		}

		#endregion
			
		#region Properties

		#region Image

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <value>The image.</value>
		public BadgeImage Image
		{
			get
			{
				return image;
			}
		}

		#endregion

		#endregion
	}
}
