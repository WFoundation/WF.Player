// <copyright file="ExtendedMap.cs" company="Wherigo Foundation">
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
using Xamarin.Forms;

namespace WF.Player.Controls
{
	using System;
	using Xamarin.Forms.Maps;
	using WF.Player.Core;
	using System.Collections.Generic;
	using System.ComponentModel;

	public class ExtendedMap : Map
	{
		private MapSpan visibleRegion;
		private IEnumerable<MapPolygon> polygons;
		private IEnumerable<MapPoint> points;
		private MapOrientation mapOrientation;

		public ExtendedMap(MapSpan span) : base(span)
		{
			mapOrientation = MapOrientation.NorthUp;
		}

		#region Nested Classes

		public class MapPolygon
		{
			public IList<ZonePoint> Points { get; private set; }
			public MapPoint Label { get; private set; }

			public MapPolygon(IList<ZonePoint> points, MapPoint label)
			{
				Points = points;
				Label = label;
			}
		}

		internal MapSpan LastMoveToRegion
		{
			get;
			private set;
		}

//		public MapSpan VisibleRegion
//		{
//			get
//			{
//				return this.visibleRegion;
//			}
//			internal set
//			{
//				if(this.visibleRegion == value)
//				{
//					return;
//				}
//				if(value == null)
//				{
//					throw new ArgumentNullException("value");
//				}
//				this.OnPropertyChanging("VisibleRegion");
//				this.visibleRegion = value;
//				this.OnPropertyChanged("VisibleRegion");
//			}
//		}

		public class MapPoint
		{
			public ZonePoint Point { get; private set; }
			public string Name { get; private set; }

			public MapPoint(ZonePoint point, string name)
			{
				Point = point;
				Name = name;
			}
		}

//		public class MapViewRequestedEventArgs : EventArgs
//		{
//			public LocationRect TargetBounds { get; private set; }
//			public GeoCoordinate TargetCenter { get; private set; }
//			public double TargetZoomLevel { get; private set; }
//
//			public MapViewRequestedEventArgs(LocationRect locRect)
//			{
//				TargetBounds = locRect;
//			}
//			public MapViewRequestedEventArgs(GeoCoordinate center, double zoomLevel)
//			{
//				TargetCenter = center;
//				TargetZoomLevel = zoomLevel;
//			}
//		}

		#endregion

		#region Properties

		#region MapOrientation

		public static readonly BindableProperty MapOrientationProperty = BindableProperty.Create<ExtendedMap, MapOrientation>(p => p.MapOrientation, MapOrientation.NorthUp, BindingMode.OneWay);

		public MapOrientation MapOrientation
		{ 
			get
			{
				return mapOrientation;
			}

			set
			{
				if (value != mapOrientation)
				{
					mapOrientation = value;
					OnPropertyChanged("MapOrientation");
				}
			}
		}

		#endregion

		#region Polygons

		public IEnumerable<MapPolygon> Polygons
		{
			get
			{
				return polygons ?? new List<MapPolygon>();
			}
			set
			{
				if (value != polygons)
				{
					polygons = value;
					OnPropertyChanged("Polygons");
				}
			}
		}

		#endregion

		#region Points

		public IEnumerable<MapPoint> Points
		{
			get
			{
				if (points == null)
				{
					points = new List<MapPoint>();
				}
				return points;
			}
			set
			{
				if (value != polygons)
				{
					points = value;
					OnPropertyChanged("Points");
				}
			}
		}

		#endregion

		#endregion	
	}
}

