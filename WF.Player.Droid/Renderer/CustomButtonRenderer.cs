// <copyright file="ButtonRenderer.cs" company="Wherigo Foundation">
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

using System;
using Xamarin.Forms;
using WF.Player.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;

[assembly: ExportRendererAttribute (typeof (Button), typeof (CustomButtonRenderer))]

namespace WF.Player.Droid
{
	public class CustomButtonRenderer : ButtonRenderer
	{
		int _viewWidth;
		int _viewHeight;
		float _textBaseline;
		Paint _textPaint;

		public CustomButtonRenderer()
		{
		}

		// Found at http://stackoverflow.com/questions/13884603/dynamic-button-text-size

		public override void Draw(Canvas canvas)
		{
			// let the ImageButton paint background as normal
			base.Draw(canvas);

			// draw the text
			// position is centered on width
			// and the baseline is calculated to be positioned from the
			// view bottom
			canvas.DrawText(((Button)Element).Text, _viewWidth/2, _viewHeight - _textBaseline, _textPaint);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			Control.SetSingleLine(true);
			Control.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
			Control.SetPadding(10, Control.PaddingTop, 10, Control.PaddingBottom);

			_textPaint = Control.Paint;
		}

		protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged(w, h, oldw, oldh);

			if (((Button)Element).Text == null)
				return;

			// save view size
			_viewWidth = w;
			_viewHeight = h;

			// first determine font point size
			AdjustTextSize();
		}

		void AdjustTextSize() 
		{
			if (((Button)Element).Font.FontSize != 0)
				_textPaint.TextSize = (float)((Button)Element).Font.FontSize;

			_textPaint.TextScaleX = 1.0f;
			Rect bounds = new Rect();

			// Ask the paint for the bounding rect if it were to draw this text
			_textPaint.GetTextBounds(((Button)Element).Text, 0, ((Button)Element).Text.Length, bounds);

			// Get the height of text that would have been produced
			int textHeight = bounds.Bottom - bounds.Top;
			int textWidth = bounds.Right - bounds.Left;


			float targetHeight = (float)_viewHeight - Control.PaddingTop - Control.PaddingBottom;
			float targetWidth = (float)_viewWidth - Control.PaddingLeft - Control.PaddingTop; // - 16;

			if (0 < targetWidth && targetWidth < textWidth)
				_textPaint.TextScaleX = targetWidth / textWidth < 0.5f ? 0.5f : targetWidth / textWidth;
		}
	}
}

