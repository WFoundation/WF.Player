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
	using System;
	using Xamarin.Forms;

	/// <summary>
	/// Game tool bar button.
	/// </summary>
	public class GameToolBarButton : Frame
	{
		/// <summary>
		/// The name of the selected property.
		/// </summary>
		public const string SelectedPropertyName = "Selected";

		/// <summary>
		/// The name of the selected color property.
		/// </summary>
		public const string SelectedColorPropertyName = "SelectedColor";

		/// <summary>
		/// Bindable property for selected.
		/// </summary>
		public static readonly BindableProperty SelectedProperty = BindableProperty.Create<GameToolBarButton, bool>(p => p.Selected, false);

		/// <summary>
		/// Bindable property for selected color.
		/// </summary>
		public static readonly BindableProperty SelectedColorProperty = BindableProperty.Create<GameToolBarButton, Color>(p => p.SelectedColor, Color.White);

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
			var layout = new RelativeLayout() 
			{
				Padding = 0,
			};

			image = new BadgeImage() 
			{
				BackgroundColor = Color.Transparent,
				Aspect = Aspect.AspectFit,
				Source = imageName,
			};

			layout.Children.Add(
				image, 
				Constraint.RelativeToParent((parent) =>
					{
						return 4;
					}),
				Constraint.RelativeToParent((parent) =>
					{
						return 4;
					}));

			Content = layout;
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

		#region Selected

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="WF.Player.Controls.GameToolBarButton"/> is selected.
		/// </summary>
		/// <value><c>true</c> if selected; otherwise, <c>false</c>.</value>
		public bool Selected
		{
			get
			{
				return (bool)GetValue(SelectedProperty);
			}

			set
			{
				SetValue(SelectedProperty, value);
			}
		}

		#endregion

		#region SelectedColor

		/// <summary>
		/// Gets or sets the color of the selected.
		/// </summary>
		/// <value>The color of the selected.</value>
		public Color SelectedColor
		{
			get
			{
				return (Color)GetValue(SelectedColorProperty);
			}

			set
			{
				SetValue(SelectedColorProperty, value);
			}
		}

		#endregion

		#endregion
	}
}
