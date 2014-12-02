﻿// <copyright file="ExtendedMap.cs" company="Wherigo Foundation">
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

using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Windows.Input;

namespace WF.Player.Controls
{
	public class MapView : ContentView
	{
		private const int frame = 16;

		private RelativeLayout layout;
		private ExtendedMap map;
		private MapViewModel mapViewModel;

		public MapView(MapViewModel mapViewModel) : base()
		{
			MapSpan span;

			map = new ExtendedMap(mapViewModel.VisibleRegion) 
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					IsShowingUser = true,
				};

			this.mapViewModel = mapViewModel;
			this.mapViewModel.Map = map;

			var tapRecognizer = new TapGestureRecognizer 
				{
					Command = this.mapViewModel.CommandButtonCenterPressed,
					NumberOfTapsRequired = 1
				};

			var mapButtonCenter = new Image() {
				BindingContext = mapViewModel,
				Source = "IconMapCenter.png",
			};
			mapButtonCenter.GestureRecognizers.Add(tapRecognizer);

			tapRecognizer = new TapGestureRecognizer 
				{
					Command = this.mapViewModel.CommandButtonOrientationPressed,
					NumberOfTapsRequired = 1
				};

			var mapButtonOrientation = new Image() {
				BindingContext = mapViewModel,
				Source = "IconMapNorth.png",
			};
			mapButtonOrientation.SetBinding(Image.SourceProperty, MapViewModel.MapOrientationSourcePropertyName);
			mapButtonOrientation.GestureRecognizers.Add(tapRecognizer);

			tapRecognizer = new TapGestureRecognizer 
				{
					Command = this.mapViewModel.CommandButtonTypePressed,
					NumberOfTapsRequired = 1
				};

			var mapButtonType = new Image() {
				BindingContext = mapViewModel,
				Source = "IconMapType.png",
			};
			mapButtonType.GestureRecognizers.Add(tapRecognizer);

			layout = new RelativeLayout() 
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			layout.Children.Add(map,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToView(layout, (p, v) => v.Width),
				Constraint.RelativeToView(layout, (p, v) => v.Height));

			layout.Children.Add(mapButtonCenter, 
				Constraint.Constant(frame),
				Constraint.Constant(frame));

			layout.Children.Add(mapButtonOrientation, 
				Constraint.Constant(frame),
				Constraint.RelativeToParent(p => frame + mapButtonCenter.Width + frame));

			layout.Children.Add(mapButtonType, 
				Constraint.RelativeToParent(p => p.Width - frame - mapButtonType.Width),
				Constraint.Constant(frame));

			this.Content = layout;
		}
	}
}