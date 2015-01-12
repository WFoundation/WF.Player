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
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using WF.Player;
using WF.Player.Controls;
using WF.Player.Controls.Droid;

[assembly: ExportRendererAttribute(typeof(DirectionArrow), typeof(DirectionArrowRenderer))]

namespace WF.Player.Controls.Droid
{
	public class DirectionArrowRenderer : ViewRenderer
	{
		Paint _paintArrow;
		Paint _paintUnknown;
		Paint _paintCircle;
		global::Android.Graphics.Path _pathArrow;
		global::Android.Graphics.Path _pathUnknown;
		global::Android.Graphics.Path _pathInside;
		global::Android.Views.View _view;
		float _centerX;
		float _centerY;
		float _size;
		float _sizeSmall;

		public DirectionArrowRenderer()
		{
			_paintCircle = new Paint (PaintFlags.AntiAlias);
			_paintCircle.StrokeWidth = 0f;
			_paintCircle.SetStyle (Paint.Style.FillAndStroke);

			_paintArrow = new Paint (PaintFlags.AntiAlias);
			_paintArrow.StrokeWidth = 0f;
			_paintArrow.SetStyle (Paint.Style.FillAndStroke);

			_paintUnknown = new Paint (PaintFlags.AntiAlias);
			_paintUnknown.StrokeWidth = 10f;
			_paintUnknown.SetStyle (Paint.Style.FillAndStroke);

			UpdatePathInside ();

			UpdatePathUnknown ();
		}

		/// <summary>
		/// Raises the element changed event to set the native control for this element.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null)
			{
				// Set a normal view as underlaying control
				_view = new global::Android.Views.View(Context);
				_view.BringToFront();
				_view.SetWillNotCacheDrawing(true);
				SetNativeControl(_view);
			}
		}

		public override void Draw (Canvas canvas)
		{
			this.Control.BringToFront();

			base.Draw (canvas);

			// Draw
			canvas.DrawCircle (_centerX, _centerY, _centerX, _paintCircle);
			if (((DirectionArrow)Element).IsInside || double.IsPositiveInfinity(((DirectionArrow)Element).Direction))
			{
				// Inside
				canvas.DrawPath(_pathInside, _paintArrow);
			}
			else if (double.IsNegativeInfinity(((DirectionArrow)Element).Direction))
			{
				// Unknown position
				canvas.DrawPath(_pathUnknown, _paintUnknown);
			}
			else if (_pathArrow != null)
			{
				canvas.DrawPath(_pathArrow, _paintArrow);
			}
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			bool update = false;

			if (e.PropertyName == "Renderer") {
				UpdatePaintCircle();
				UpdatePaintArrow();
				UpdatePathArrow();
				UpdatePathInside();
				UpdatePathUnknown();
				update = true;
			}

			if (e.PropertyName == DirectionArrow.ArrowColorProperty.PropertyName) {
				UpdatePaintArrow ();
				update = true;
			}

			if (e.PropertyName == DirectionArrow.CircleColorProperty.PropertyName) {
				UpdatePaintCircle();
				update = true;
			}

			if (e.PropertyName == DirectionArrow.DirectionProperty.PropertyName) {
				UpdatePathArrow ();
				update = true;
			}

			if (!update)
				return;

			PostInvalidate ();
		}

		protected override void OnSizeChanged (int w, int h, int oldw, int oldh)
		{
			base.OnSizeChanged (w, h, oldw, oldh);

			_centerX = _centerY = w / 2f;
			_size = (w * 0.8f) / 2f;
			_sizeSmall = w * 0.5f / 2f;

			_paintArrow.TextSize = _size;

			UpdatePathArrow ();
			UpdatePathInside ();
			UpdatePathUnknown();
		}

		protected override void OnLayout (bool changed, int l, int t, int r, int b)
		{
			base.OnLayout (changed, l, t, r, b);

			Control.Layout (l, t, r, b);
		}

		#region Private Functions

		void UpdatePaintArrow ()
		{
			_paintArrow.Color = ((DirectionArrow)Element).ArrowColor.ToAndroid ();
			_paintUnknown.Color = ((DirectionArrow)Element).ArrowColor.ToAndroid ();
		}

		void UpdatePaintCircle ()
		{
			_paintCircle.Color = ((DirectionArrow)Element).CircleColor.ToAndroid ();
		}

		void UpdatePathArrow()
		{
			if (_pathArrow == null)
				_pathArrow = new global::Android.Graphics.Path ();
			else
				_pathArrow.Reset ();

			// Create arrow
			// We have a direction, so draw an arrow
			var direction = 180.0 - ((DirectionArrow)Element).Direction;
			if (direction < 0)
				direction += 360.0;
			direction = direction % 360.0;

			double rad1 = direction / 180.0 * Math.PI;
			double rad2 = (direction + 180.0 + 30.0) / 180.0 * Math.PI;
			double rad3 = (direction + 180.0 - 30.0) / 180.0 * Math.PI; 
			double rad4 = (direction + 180.0) / 180.0 * Math.PI; 

			PointF p1 = new PointF ((float)(_centerX + _size * Math.Sin (rad1)), (float)(_centerY + _size * Math.Cos (rad1)));
			PointF p2 = new PointF ((float)(_centerX + _size * Math.Sin (rad2)), (float)(_centerY + _size * Math.Cos (rad2)));
			PointF p3 = new PointF ((float)(_centerX + _size * Math.Sin (rad3)), (float)(_centerY + _size * Math.Cos (rad3)));
			PointF p4 = new PointF ((float)(_centerX + _sizeSmall * Math.Sin (rad4)), (float)(_centerY + _sizeSmall * Math.Cos (rad4)));

			_pathArrow.MoveTo (p1.X, p1.Y);
			_pathArrow.LineTo (p2.X, p2.Y);
			_pathArrow.LineTo (p4.X, p4.Y);
			_pathArrow.LineTo (p3.X, p3.Y);
			_pathArrow.LineTo (p1.X, p1.Y);
		}

		void UpdatePathInside()
		{
			if (_size <= 0)
				return;

			if (_pathInside == null)
				_pathInside = new global::Android.Graphics.Path ();
			else
				_pathInside.Reset ();

			float x1 = _centerX + _size / 1.41f;
			float x2 = _centerX + _size / 2.82f;
			float x3 = _centerX - _size / 2.82f;
			float x4 = _centerX - _size / 1.41f;

			float y1 = _centerY + _size / 1.41f;
			float y2 = _centerY + _size / 2.82f;
			float y3 = _centerY - _size / 2.82f;
			float y4 = _centerY - _size / 1.41f;

			_pathInside.MoveTo (x1, y2);
			_pathInside.LineTo (x2, y1);
			_pathInside.LineTo (_centerX, y2);
			_pathInside.LineTo (x3, y1);
			_pathInside.LineTo (x4, y2);
			_pathInside.LineTo (x3, _centerY);
			_pathInside.LineTo (x4, y3);
			_pathInside.LineTo (x3, y4);
			_pathInside.LineTo (_centerX, y3);
			_pathInside.LineTo (x2, y4);
			_pathInside.LineTo (x1, y3);
			_pathInside.LineTo (x2, _centerY);
			_pathInside.LineTo (x1, y2);
		}

		void UpdatePathUnknown()
		{
			if (_size <= 0)
				return;

			if (_pathUnknown == null)
				_pathUnknown = new global::Android.Graphics.Path ();
			else
				_pathUnknown.Reset ();

			float x1 = _centerX + _size * 0.5f;
			float x2 = _centerX - _size * 0.5f;

			float y1 = _centerY + _size * 0.5f;
			float y2 = _centerY - _size * 0.5f;

			_pathUnknown.MoveTo(x1, y1);
			_pathUnknown.LineTo(x2, y2);
			_pathUnknown.MoveTo(x1, y2);
			_pathUnknown.LineTo(x2, y1);
		}
		#endregion
	}
}
