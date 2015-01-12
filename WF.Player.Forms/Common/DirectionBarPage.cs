// <copyright file="DirectionBarPage.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using System;
	using Vernacular;
	using WF.Player.Controls;
	using Xamarin.Forms;

	/// <summary>
	/// Direction bar page.
	/// </summary>
	public class DirectionBarPage : ToolBarPage
	{
		/// <summary>
		/// Bindable property for direction.
		/// </summary>
		public static readonly BindableProperty DirectionProperty = BindableProperty.Create<DirectionBarPage, double>(p => p.Direction, double.NegativeInfinity);

		/// <summary>
		/// The distance property.
		/// </summary>
		public static readonly BindableProperty DistanceProperty = BindableProperty.Create<DirectionBarPage, double>(p => p.Distance, double.NegativeInfinity);

		/// <summary>
		/// The distance text property.
		/// </summary>
		public static readonly BindableProperty DistanceTextProperty = BindableProperty.Create<DirectionBarPage, string>(p => p.DistanceText, " "); //Catalog.GetString("Unknown"));

		/// <summary>
		/// The size of the direction.
		/// </summary>
		private const double DirectionSize = 64;

		/// <summary>
		/// The layout for bottom bar.
		/// </summary>
		private StackLayout layout;

		/// <summary>
		/// The layout for direction.
		/// </summary>
		private StackLayout layoutDirection;

		/// <summary>
		/// The placeholder for direction.
		/// </summary>
		private BoxView placeholder;

		/// <summary>
		/// The direction arrow.
		/// </summary>
		private DirectionArrow direction;

		/// <summary>
		/// The distance text under direction arrow.
		/// </summary>
		private Label distance;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.DirectionBarPage"/> class.
		/// </summary>
		public DirectionBarPage()
		{
			layout = new StackLayout() 
			{
				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 4,
				IsClippedToBounds = false,
			};

			var layoutButtons = new StackLayout() 
			{
				BackgroundColor = Color.Transparent,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			layout.Children.Add(layoutButtons);

			// Placeholder for the DirectionArrow
			placeholder = new BoxView() 
			{
				Color = Color.Transparent,
				WidthRequest = DirectionSize,
			};

			layout.Children.Add(placeholder);

			// Add the new layout to the old one
			BottomLayout.Children.Add(layout);

			// Now replace the old bottom layout with a new one
			BottomLayout = layoutButtons;

			var relativeLayout = (RelativeLayout)ContentLayout.Parent;

			layoutDirection = new StackLayout() 
			{
				Padding = 0,
			};

			direction = new DirectionArrow() 
			{
				BackgroundColor = Color.Transparent, // This is mandatory for Android. If not, direction isn't drawn.
				CircleColor = App.Colors.DirectionBackground,
				ArrowColor = App.Colors.DirectionColor,
				HeightRequest = DirectionSize,
				WidthRequest = DirectionSize,
				HorizontalOptions = LayoutOptions.End,
			};

			distance = new Label() 
			{
				BackgroundColor = Color.Transparent,
				TextColor = App.Colors.Text,
				Text = string.Empty,
				HorizontalOptions = LayoutOptions.Center,
			};

			layoutDirection.Children.Add(direction);
			layoutDirection.Children.Add(distance);

			relativeLayout.Children.Add(
				layoutDirection,
				Constraint.RelativeToParent((parent) => parent.Width - DirectionSize - 4),
				Constraint.RelativeToParent((parent) => parent.Height - layoutDirection.Height - 4));

			// Do this, because Xamarin.Forms don't update the relative layout after calculationg the size of the direction stack.
			layoutDirection.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName == "Height")
				{
					relativeLayout.ForceLayout();
				}
			};

		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the direction view.
		/// </summary>
		/// <value>The direction view.</value>
		public DirectionArrow DirectionView
		{
			get
			{
				return direction;
			}
		}

		/// <summary>
		/// Gets the distance view.
		/// </summary>
		/// <value>The distance view.</value>
		public Label DistanceView 
		{
			get 
			{
				return distance;
			}
		}

		/// <summary>
		/// Gets the direction layout.
		/// </summary>
		/// <value>The direction layout.</value>
		public StackLayout DirectionLayout 
		{
			get 
			{ 
				return layoutDirection; 
			}
		}

		/// <summary>
		/// Gets the direction space layout.
		/// </summary>
		/// <value>The direction space layout.</value>
		public BoxView DirectionSpaceLayout 
		{
			get 
			{ 
				return placeholder; 
			}
		}

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
				direction.Direction = Direction;
			}
		}

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
				distance.Text = Converter.NumberToBestLength(Distance);
			}
		}

		/// <summary>
		/// Gets the distance text.
		/// </summary>
		/// <value>The distance text.</value>
		public string DistanceText
		{
			get
			{
				return (string)GetValue(DistanceTextProperty);
			}
		}

		#endregion
	}
}