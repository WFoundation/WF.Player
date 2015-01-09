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

using System;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using System.Windows.Input;

namespace WF.Player.Controls
{
	public class MapView : ContentView
	{
		private const int frame = 10;

		private RelativeLayout layout;
		private ExtendedMap map;
		private MapViewModel mapViewModel;

		public MapView(MapViewModel mapViewModel) : base()
		{
			MapSpan span;

			this.BindingContext = mapViewModel;

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

			#if __IOS__

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

			#endif

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

			// Do this, because Xamarin.Forms don't update the relative layout after calculationg the size of the direction stack.
			mapButtonType.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName == "Height" || e.PropertyName == "Width")
				{
					layout.ForceLayout();
				}
			};

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
				Constraint.Constant(38 + frame));

			#if __IOS__

			layout.Children.Add(mapButtonOrientation, 
				Constraint.Constant(frame),
				Constraint.RelativeToParent(p => 38 + frame + mapButtonCenter.Width + frame));

			#endif

			layout.Children.Add(mapButtonType, 
				Constraint.RelativeToParent(p => p.Width - frame - mapButtonType.Width),
				Constraint.Constant(38 + frame));

			var font = Device.OnPlatform(Font.SystemFontOfSize(16).WithAttributes(FontAttributes.Bold), Font.SystemFontOfSize(14), Font.SystemFontOfSize(16).WithAttributes(FontAttributes.Bold));
			// Create position entries at bottom of screen
			var latitude = new Label() 
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					XAlign = TextAlignment.Start,
					TextColor = Color.White,
					Font = font,
				};
			latitude.SetBinding(Label.TextProperty, MapViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToLatitude());

			var longitude = new Label() 
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					XAlign = TextAlignment.Start,
					TextColor = Color.White,
					Font = font,
				};
			longitude.SetBinding(Label.TextProperty, MapViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToLongitude());

			var altitude = new Label() 
				{
					XAlign = TextAlignment.End,
					TextColor = Color.White,
					Font = font,
				};
			altitude.SetBinding(Label.TextProperty, MapViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAltitude());

			var accuracy = new Label() 
				{
					XAlign = TextAlignment.End,
					TextColor = Color.White,
					Font = font,
				};
			accuracy.SetBinding(Label.TextProperty, MapViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAccuracy());

			var imageAltitude = new Image() 
				{
					WidthRequest = 16,
					HeightRequest = 16,
					Source = "IconAltitudeLight.png",
				};

			var imageAccuracy = new Image() 
				{
					WidthRequest = 16,
					HeightRequest = 16,
					Source = "IconAccuracyLight.png",
				};

			var layoutPosition = new Grid() 
				{
					BackgroundColor = Color.Gray.MultiplyAlpha(0.8),
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = new Thickness(10, Device.OnPlatform(4, 2, 4)),
					RowSpacing = 2,
				};

			layoutPosition.RowDefinitions = new RowDefinitionCollection {
				new RowDefinition { Height = 16 },
				new RowDefinition { Height = 16 }
			};

			layoutPosition.ColumnDefinitions = new ColumnDefinitionCollection {
				new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				new ColumnDefinition { Width = 16 }
			};

			layoutPosition.Children.Add(latitude, 0, 0);
			layoutPosition.Children.Add(longitude, 0, 1);
			layoutPosition.Children.Add(imageAltitude, 2, 0);
			layoutPosition.Children.Add(imageAccuracy, 2, 1);
			layoutPosition.Children.Add(altitude, 1, 0);
			layoutPosition.Children.Add(accuracy, 1, 1);

			layout.Children.Add(layoutPosition, 
				Constraint.Constant(0), //.RelativeToParent(p => p.Width - layoutBottomHori.Width - 8),
				Constraint.Constant(0), //RelativeToParent(p => p.Height - 44 - 8));
				Constraint.RelativeToParent(p => p.Width),
				Constraint.Constant(40));

			this.Content = layout;
		}
	}
}
