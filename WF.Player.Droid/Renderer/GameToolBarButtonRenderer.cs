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

using System;
using Xamarin.Forms;
using WF.Player;
using WF.Player.Droid;
using Xamarin.Forms.Platform.Android;
using WF.Player.Controls;
using WF.Player.Controls.Droid;
using Android.Graphics;
using Android.Content.Res;
using Android.Util;

[assembly: ExportRendererAttribute (typeof (GameToolBarButton), typeof (GameToolBarButtonRenderer))]

namespace WF.Player.Controls.Droid
{
	public class GameToolBarButtonRenderer : FrameRenderer
	{
		float _centerX;
		float _centerY;
		float _size;

		public override void Draw (Canvas canvas)
		{
			GameToolBarButton button = (GameToolBarButton)Element;

			var bounds = button.Bounds;

			base.Draw (canvas);

			if (button.Selected) {
				using (Paint paint = new Paint ()) {
					paint.Color = button.SelectedColor.ToAndroid();
					paint.StrokeWidth = 0f;
					paint.SetStyle (Paint.Style.FillAndStroke);

					PointF p1 = new PointF (_centerX, _size); //(float)bounds.Height - size);
					PointF p2 = new PointF (_centerX + _size, 0f); // (float)bounds.Height);
					PointF p3 = new PointF (_centerX - _size, 0f); // (float)bounds.Height);

					using (Path path = new Path ()) {
						path.MoveTo (p1.X, p1.Y);
						path.LineTo (p2.X, p2.Y);
						path.LineTo (p3.X, p3.Y);
						path.LineTo (p1.X, p1.Y);

						canvas.DrawPath (path, paint);
					}
				}
			}
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);

			_centerX = _centerY = w / 2f;
			_size = w * 0.1f;
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == GameToolBarButton.SelectedProperty.PropertyName
				|| e.PropertyName == GameToolBarButton.SelectedColorProperty.PropertyName)
				this.Invalidate ();
		}

		float DipToPixel(double dip)
		{
			return TypedValue.ApplyDimension(ComplexUnitType.Dip, (float)dip, Forms.Context.Resources.DisplayMetrics);
		}
	}
}

