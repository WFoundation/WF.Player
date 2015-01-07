// <copyright file="BadgeImageRenderer.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

[assembly: Xamarin.Forms.ExportRendererAttribute(typeof(WF.Player.Controls.BadgeImage), typeof(WF.Player.Controls.Droid.BadgeImageRenderer))]

namespace WF.Player.Controls.Droid
{
	using Android.Graphics;
	using Android.Graphics.Drawables;
	using Android.Util;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;
	using WF.Player.Controls;

	/// <summary>
	/// Badge image renderer.
	/// </summary>
	public class BadgeImageRenderer : ImageRenderer
	{
		private ColorMatrixColorFilter filterColor;
		private ColorMatrixColorFilter filterGray;

		protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged(e);

			if (Element == null)
			{
				return;
			}

			var matrixColor = new ColorMatrix();

			filterColor = new ColorMatrixColorFilter(matrixColor);

			var matrixGray = new ColorMatrix();

			matrixGray.SetSaturation(0);

			filterGray = new ColorMatrixColorFilter(matrixGray);
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Property changed event arguments.</param>
		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			if (e.PropertyName == BadgeImage.NumberProperty.PropertyName)
			{
				this.Invalidate();
			}

			if (e.PropertyName == BadgeImage.SelectedProperty.PropertyName)
			{
				this.Invalidate();
			}
		}

		/// <Docs>The Canvas to which the View is rendered.</Docs>
		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas to draw onto.</param>
		public override void Draw(Canvas canvas)
		{
			BadgeImage image = (BadgeImage)Element;

			// Set color of icon
			if (Control.Drawable != null)
			{
				if (image.Selected)
				{
					Control.Drawable.SetColorFilter(filterColor);
				}
				else
				{
					Control.Drawable.SetColorFilter(filterGray);
				}
			}

			base.Draw(canvas);

			if (image.Number > 0)
			{
				using (Paint paint = new Paint())
				{
					paint.Color = image.Selected ? Xamarin.Forms.Color.Red.ToAndroid() : Xamarin.Forms.Color.Gray.ToAndroid();
					paint.StrokeWidth = 0f;
					paint.SetStyle(Paint.Style.FillAndStroke);

					// Calc text size
					paint.TextSize = (int)this.DipToPixel(16);
					paint.FakeBoldText = true;

					string text = image.Number.ToString();
					Rect textBounds = new Rect(0, 0, 0, 0);

					paint.GetTextBounds(text, 0, text.Length, textBounds);

					float textWidth = paint.MeasureText(text);
					float textHeight = textBounds.Height();

					float badgeWidth = textWidth + this.DipToPixel(9);
					float badgeHeight = textHeight + this.DipToPixel(9);

					if (badgeWidth < badgeHeight)
					{
						badgeWidth = badgeHeight;
					}

					double offsetX = (image.Bounds.Width - image.Bounds.Width) / 2;
					double offsetY = (image.Bounds.Height - image.Bounds.Height) / 2;

					float left = this.DipToPixel(image.Bounds.Width) - badgeWidth;
					float top = 1;
					float right = left + badgeWidth;
					float bottom = top + badgeHeight;
					float radius = (badgeHeight / 2f) - 1f;

					using (Path path = new Path())
					{
						canvas.DrawRoundRect(new RectF(left, top, left + badgeWidth, top + badgeHeight), radius, radius, paint);

						paint.Color = Xamarin.Forms.Color.White.ToAndroid();

						canvas.DrawText(image.Number.ToString(), left + ((badgeWidth - textWidth) / 2) - 1, bottom - ((badgeHeight - textHeight) / 2), paint);
					}
				}
			}
		}

		/// <summary>
		/// Dips to pixel.
		/// </summary>
		/// <returns>The to pixel.</returns>
		/// <param name="dip">Value to convert.</param>
		private float DipToPixel(double dip)
		{
			return TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)dip, Forms.Context.Resources.DisplayMetrics);
		}
	}
}
