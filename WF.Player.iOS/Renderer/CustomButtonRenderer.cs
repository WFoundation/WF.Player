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

[assembly: Xamarin.Forms.ExportRendererAttribute(typeof(Xamarin.Forms.Button), typeof(WF.Player.iOS.CustomButtonRenderer))]

namespace WF.Player.iOS
{
	using System;
	using UIKit;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	public class CustomButtonRenderer : ButtonRenderer
	{
		public CustomButtonRenderer()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged (e);

			UIButton button = (UIButton)Control;

			if (e.NewElement == null)
			{
				return;
			}

			// If only an image is displayed, than move it to the center of button
			if (String.IsNullOrEmpty (button.TitleLabel.Text) && button.ImageView.Image != null) {
				button.ImageEdgeInsets = new UIEdgeInsets(0.0f, 10.0f, 0.0f, 0.0f);
				button.TitleEdgeInsets = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
			} else {
				button.ImageEdgeInsets = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
				button.TitleEdgeInsets = new UIEdgeInsets(0.0f, 0.0f, 0.0f, 0.0f);
//				button.LineBreakMode = UILineBreakMode.TailTruncation;
			}

			button.TintColor = e.NewElement.IsEnabled ? App.Colors.Tint.ToUIColor () : Color.Black.ToUIColor();
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			UIButton button = (UIButton)Control;

			button.TintColor = Element.IsEnabled ? App.Colors.Tint.ToUIColor () : Color.Black.ToUIColor();
		}
	}
}

