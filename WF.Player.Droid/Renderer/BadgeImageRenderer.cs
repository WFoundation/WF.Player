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

using WF.Player.Controls;
using WF.Player.Controls.Droid;
using Xamarin.Forms;

[assembly: ExportRendererAttribute(typeof(BadgeImage), typeof(BadgeImageRenderer))]

namespace WF.Player.Controls.Droid
{
	using System;
	using Android.Content.Res;
	using Android.Graphics;
	using Android.Util;
	using WF.Player;
	using Xamarin.Forms.Platform.Android;

	/// <summary>
	/// Badge image renderer.
	/// </summary>
	public class BadgeImageRenderer : ImageRenderer
	{
		/// <summary>
		/// The center x.
		/// </summary>
		private float centerX;

		/// <summary>
		/// The center y.
		/// </summary>
		private float centerY;

		/// <summary>
		/// The size.
		/// </summary>
		private float size;

		/// <Docs>The Canvas to which the View is rendered.</Docs>
		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas to draw onto.</param>
		public override void Draw(Canvas canvas)
		{
			BadgeImage image = (BadgeImage)Element;

			base.Draw(canvas);

			if (image.Number > 0)
			{
				using (Paint paint = new Paint())
				{
					paint.Color = Xamarin.Forms.Color.Red.ToAndroid();
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
					float top = 0;
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

		/// <Docs>Current width of this view.</Docs>
		/// <summary>
		/// This is called during layout when the size of this view has changed.
		/// </summary>
		/// <para tool="javadoc-to-mdoc">This is called during layout when the size of this view has changed. If
		///  you were just added to the view hierarchy, you're called with the old
		///  values of 0.</para>
		/// <format type="text/html">[Android Documentation]</format>
		/// <since version="Added in API level 1"></since>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		/// <param name="oldw">Old width.</param>
		/// <param name="oldh">Old height.</param>
		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			this.centerX = this.centerY = w / 2f;
			this.size = w * 0.1f;
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
