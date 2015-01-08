// <copyright file="BottomBarPage.cs" company="Wherigo Foundation">
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
	using Xamarin.Forms;

	/// <summary>
	/// Bottom bar page.
	/// </summary>
	public class BottomBarPage : BasePage
	{
		/// <summary>
		/// The layout.
		/// </summary>
		private RelativeLayout layout;

		/// <summary>
		/// The content layout.
		/// </summary>
		private Layout contentLayout;

		/// <summary>
		/// The bottom layout.
		/// </summary>
		private StackLayout bottomLayout;

		private StackLayout lineLayout;

		/// <summary>
		/// The height of the bar.
		/// </summary>
		private int barHeight = Device.OnPlatform<int>(44, 44, 44);

		/// <summary>
		/// The color of the bar.
		/// </summary>
		private Color barColor = App.Colors.Bar;

		private float keyboardHeight = 0;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.BottomBarPage"/> class.
		/// </summary>
		public BottomBarPage() : base()
		{
			BackgroundColor = App.Colors.Background;

			layout = new RelativeLayout() 
			{
				BackgroundColor = Color.Transparent,
				Padding = 0,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				IsClippedToBounds = false,
			};

			contentLayout = new StackLayout() 
			{
				BackgroundColor = Color.Transparent,
				Padding = new Thickness(0),
			};

			bottomLayout = new StackLayout() 
			{
				BackgroundColor = App.Colors.Bar,
				HeightRequest = barHeight,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Orientation = StackOrientation.Horizontal,
				IsClippedToBounds = false,
			};

			layout.Children.Add(
				contentLayout,
				Constraint.Constant(0),
				Constraint.Constant(0),
				Constraint.RelativeToParent((parent) =>
					{
						return parent.Width;
					}),
				Constraint.RelativeToParent((parent) =>
					{
						return parent.Height - barHeight - keyboardHeight;
					}));

			#if __IOS__
			// Dark grey line on iOS
			lineLayout = new StackLayout () 
			{
				Padding = new Thickness(0, 0),
				BackgroundColor = App.Colors.IsDarkTheme ? Color.FromRgb(0x26, 0x26, 0x26) : Color.FromRgb (0xAE, 0xAE, 0xAE),
				HeightRequest = 0.5f,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			layout.Children.Add(
				lineLayout,
				Constraint.Constant(0),
				Constraint.RelativeToParent((parent) => { return parent.Height - barHeight - keyboardHeight - 0.5f; }),
				Constraint.RelativeToParent((parent) => { return parent.Width; }),
				Constraint.Constant(0.5f));
			#endif

			layout.Children.Add(
				bottomLayout, 
				Constraint.Constant(0),
				Constraint.RelativeToParent((parent) =>
					{
						return parent.Height - barHeight - keyboardHeight;
					}),
				Constraint.RelativeToParent((parent) =>
					{
						return parent.Width;
					}),
				Constraint.Constant(barHeight));

			Content = layout;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the content layout.
		/// </summary>
		/// <value>The content layout.</value>
		public Layout ContentLayout
		{
			get
			{ 
				return contentLayout; 
			}

			set
			{
				if (contentLayout != value)
				{
					contentLayout = value;
					contentLayout.ForceLayout();
				}
			}
		}

		/// <summary>
		/// Gets or sets the bottom layout.
		/// </summary>
		/// <value>The bottom layout.</value>
		public StackLayout BottomLayout
		{
			get
			{ 
				return bottomLayout;
			}

			protected set
			{
				if (!bottomLayout.Equals(value))
				{
					bottomLayout = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets the height of the bar.
		/// </summary>
		/// <value>The height of the bar.</value>
		public int BarHeight
		{
			get
			{
				return barHeight;
			}

			set
			{
				if (barHeight != value)
				{
					barHeight = value;
					layout.Children.Remove(bottomLayout);
					layout.Children.Add(
						bottomLayout, 
						Constraint.Constant(0),
						Constraint.RelativeToParent((parent) =>
							{
								return parent.Height - barHeight - keyboardHeight;
							}),
						Constraint.RelativeToParent((parent) =>
							{
								return parent.Width;
							}),
						Constraint.Constant(barHeight));
				}
			}
		}

		/// <summary>
		/// Gets or sets the color of the bar.
		/// </summary>
		/// <value>The color of the bar.</value>
		public Color BarColor
		{
			get
			{
				return barColor;
			}

			set
			{
				if (barColor != value)
				{
					barColor = value;
					bottomLayout.BackgroundColor = barColor;
				}
			}
		}

		public float KeyboardHeight
		{
			get
			{
				return keyboardHeight;
			}

			set
			{
				if (keyboardHeight != value)
				{
					keyboardHeight = value;

					var bounds = ContentLayout.Bounds;

					bounds.Height = layout.Height - barHeight - keyboardHeight;

					ContentLayout.LayoutTo(bounds, 200);

					#if __IOS__

					bounds = lineLayout.Bounds;

					bounds.Top = layout.Height - barHeight - keyboardHeight - 0.5f;

					lineLayout.LayoutTo(bounds, 200);

					#endif

					bounds = BottomLayout.Bounds;

					bounds.Top = layout.Height - barHeight - keyboardHeight;

					BottomLayout.LayoutTo(bounds, 200);

					// Force layout
					ContentLayout.ForceLayout();
					BottomLayout.ForceLayout();
				}
			}
		}

		#endregion
	}
}
