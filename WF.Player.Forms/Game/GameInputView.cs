// <copyright file="GameInputView.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using Vernacular;
	using WF.Player.Controls;
	using Xamarin.Forms;

	/// <summary>
	/// Game input view.
	/// </summary>
	public class GameInputView : ToolBarPage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameInputView"/> class.
		/// </summary>
		/// <param name="gameInputViewModel">Game input view model.</param>
		public GameInputView(GameInputViewModel gameInputViewModel) : base()
		{
			BindingContext = gameInputViewModel;

			NavigationPage.SetHasBackButton(this, false);

			BackgroundColor = App.Colors.Background;

			#if __HTML__

			var webView = new CustomWebView () {
				BackgroundColor = App.Colors.Background,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
			webView.SetBinding (WebView.SourceProperty, GameDetailViewModel.HtmlSourcePropertyName);
			webView.SizeChanged += (object sender, EventArgs e) => {
				webView.HeightRequest = 1;
			};

			((StackLayout)ContentLayout).Children.Add(webView);

			#else

			ScrollLayout = new PinchScrollView() 
				{
					Orientation = ScrollOrientation.Vertical,
					Padding = new Thickness(0, 0),
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var layout = new StackLayout() 
				{
//					BackgroundColor = App.Colors.Background,
					Orientation = StackOrientation.Vertical,
					Padding = new Thickness(10, 10),
					Spacing = 10,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var image = new ExtendedImage() 
				{
					Aspect = Aspect.AspectFit,
				};
			image.SetBinding(Image.SourceProperty, GameInputViewModel.ImageSourcePropertyName);
			image.SetBinding(Image.IsVisibleProperty, GameInputViewModel.HasImagePropertyName);

			layout.Children.Add(image);

			var description = new ExtendedLabel() 
				{
					TextColor = App.Colors.Text,
					Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize),
					XAlign = App.Prefs.TextAlignment,
					UseMarkdown = App.Game.UseMarkdown,
					VerticalOptions = LayoutOptions.Start,
				};
			description.SetBinding(ExtendedLabel.TextProperty, GameInputViewModel.TextPropertyName);
			description.SetBinding(ExtendedLabel.IsVisibleProperty, GameInputViewModel.HasTextPropertyName);

			layout.Children.Add(description);

			ScrollLayout.Content = layout;

			((StackLayout)ContentLayout).Children.Add(ScrollLayout);

			#endif

			BottomEntry = new StackLayout 
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = Device.OnPlatform(6, 2, 2),
				};

			var entry = new Entry 
				{
					#if __IOS__
					BackgroundColor = Color.FromRgb(223, 223, 223),
					TextColor = App.Colors.Tint,
					#endif
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Fill,
				};

			entry.SetBinding(Entry.TextProperty, GameInputViewModel.InputTextPropertyName, BindingMode.TwoWay);
			entry.SetBinding(Entry.PlaceholderProperty, GameInputViewModel.PlaceholderPropertyName);
			entry.SetBinding(Entry.IsVisibleProperty, GameInputViewModel.HasEntryPropertyName);

			BottomEntry.Children.Add(entry);

			var button = new Button 
				{
					Text = Catalog.GetString("Ok"),
					TextColor = App.Colors.Tint,
					BackgroundColor = Color.Transparent,
					#if __IOS__
					Font = Font.SystemFontOfSize(20),
					#endif
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.Center,
				};

			button.SetBinding(Button.CommandProperty, GameInputViewModel.ButtonClickedPropertyName); 

			BottomEntry.Children.Add(button);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the bottom entry.
		/// </summary>
		/// <value>The bottom entry.</value>
		public StackLayout BottomEntry { get; set; }

		public ScrollView ScrollLayout { get; private set; }

		#endregion
	}
}
