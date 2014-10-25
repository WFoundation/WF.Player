// <copyright file="DirectionArrow.cs" company="Wherigo Foundation">
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

namespace WF.Player.Controls
{
	using System;
	using Vernacular;
	using Xamarin.Forms;

	/// <summary>
	/// Direction arrow.
	/// </summary>
	public class DirectionArrow : View
	{
		/// <summary>
		/// Bindable property for distance.
		/// </summary>
		public static readonly BindableProperty DistanceProperty = BindableProperty.Create<DirectionArrow, double>(p => p.Distance, 0.0);

		/// <summary>
		/// Bindable property for direction.
		/// </summary>
		public static readonly BindableProperty DirectionProperty = BindableProperty.Create<DirectionArrow, double>(p => p.Direction, 0.0);

		/// <summary>
		/// Binable property for distance text.
		/// </summary>
		public static readonly BindableProperty DistanceTextProperty = BindableProperty.Create<DirectionArrow, string>(p => p.DistanceText, Catalog.GetString("Unknown"));

		/// <summary>
		/// Bindable property for inside.
		/// </summary>
		public static readonly BindableProperty IsInsideProperty = BindableProperty.Create<DirectionArrow, bool>(p => p.IsInside, false);

		/// <summary>
		/// Bindable property for circle color.
		/// </summary>
		public static readonly BindableProperty CircleColorProperty = BindableProperty.Create<DirectionArrow, Color>(p => p.CircleColor, Color.Transparent);

		/// <summary>
		/// Binadable property for arrow color.
		/// </summary>
		public static readonly BindableProperty ArrowColorProperty = BindableProperty.Create<DirectionArrow, Color>(p => p.ArrowColor, Color.Transparent);

		#region Properties

		#region Direction

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public double Direction
		{
			get
			{
				return (double)GetValue(DirectionProperty);
			}

			set
			{
				SetValue(DirectionProperty, value);
			}
		}

		#endregion

		#region Distance

		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		public double Distance
		{
			get
			{
				return (double)GetValue(DistanceProperty);
			}

			set
			{
				SetValue(DistanceProperty, value);
			}
		}

		#endregion

		#region DistanceText

		/// <summary>
		/// Gets the distance text.
		/// </summary>
		/// <value>The distance.</value>
		public string DistanceText
		{
			get
			{
				if (IsInside)
				{
					return Catalog.GetString("Inside");
				}
				else if (!double.IsNaN(Direction))
				{
					return 	Converter.NumberToBestLength(Distance);
				}
				else
				{
					return Catalog.GetString("Unknown");
				}
			}
		}

		#endregion

		#region IsInside

		/// <summary>
		/// Gets or sets a value indicating whether this instance is inside a zone.
		/// </summary>
		/// <value><c>true</c> if this instance is inside of a zone; otherwise, <c>false</c>.</value>
		public bool IsInside
		{
			get 
			{
				return (bool)GetValue(IsInsideProperty);
			}

			set 
			{
				SetValue(IsInsideProperty, value);
			}
		}

		#endregion

		#region CircleColor

		/// <summary>
		/// Gets or sets the color of the circle arround the arrow.
		/// </summary>
		/// <value>The color of the circle.</value>
		public Color CircleColor
		{
			get
			{
				return (Color)GetValue(CircleColorProperty);
			}

			set
			{
				SetValue(CircleColorProperty, value);
			}
		}

		#endregion

		#region ArrowColor

		/// <summary>
		/// Gets or sets the color of the arrow.
		/// </summary>
		/// <value>The color of the arrow.</value>
		public Color ArrowColor
		{
			get
			{
				return (Color)GetValue(ArrowColorProperty);
			}

			set
			{
				SetValue(ArrowColorProperty, value);
			}
		}

		#endregion

		#endregion
	}
}
