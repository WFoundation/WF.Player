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
using WF.Player.Services.Settings;

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

			App.GameNavigation.ShowBackButton = false;

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
					Padding = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var layout = new StackLayout() 
				{
					Orientation = StackOrientation.Vertical,
					Padding = 10,
					Spacing = 10,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var image = new ExtendedImage() 
				{
					Aspect = Aspect.AspectFit,
					HorizontalOptions = Settings.ImageAlignment.ToLayoutOptions(),
				};

			image.SetBinding(Image.SourceProperty, GameInputViewModel.ImageSourcePropertyName);
			image.SetBinding(VisualElement.IsVisibleProperty, GameInputViewModel.HasImagePropertyName);

			layout.Children.Add(image);

			var description = new ExtendedLabel() 
				{
					TextColor = App.Colors.Text,
					FontSize = Settings.FontSize,
					FontFamily = Settings.FontFamily,
					XAlign = Settings.TextAlignment,
					UseMarkdown = App.Game.UseMarkdown,
				};

			description.SetBinding(Label.TextProperty, GameInputViewModel.TextPropertyName);
			description.SetBinding(VisualElement.IsVisibleProperty, GameInputViewModel.HasTextPropertyName);

			layout.Children.Add(description);

			ScrollLayout.Content = layout;

			((StackLayout)ContentLayout).Children.Add(ScrollLayout);

			#endif

			var scanner = new Image 
				{
					BackgroundColor = Color.Transparent,
					Source = "IconScan.png",
					Aspect = Aspect.AspectFit,
					#if __IOS__
//					WidthRequest = 42,
//					FontSize = 20,
//					FontFamily = Settings.FontFamily,
					#endif
					HorizontalOptions = LayoutOptions.Start,
					VerticalOptions = LayoutOptions.Center,
				};

			var tapRecognizer = new TapGestureRecognizer 
				{
					Command = ((GameInputViewModel)BindingContext).ScannerClicked,
					NumberOfTapsRequired = 1
				};

			scanner.GestureRecognizers.Add(tapRecognizer);

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
			entry.SetBinding(VisualElement.IsVisibleProperty, GameInputViewModel.HasEntryPropertyName);

			var button = new Button 
				{
					BackgroundColor = Color.Transparent,
					Text = Catalog.GetString("Ok"),
					TextColor = App.Colors.Tint,
					#if __IOS__
					FontSize = 20,
					FontFamily = Settings.FontFamily,
					#endif
					HorizontalOptions = LayoutOptions.End,
					VerticalOptions = LayoutOptions.Center,
				};

			button.SetBinding(Button.CommandProperty, GameInputViewModel.ButtonClickedPropertyName); 

//			BottomEntry.Children.Add(button);

			BottomEntry = new StackLayout 
				{
					Orientation = StackOrientation.Horizontal,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = Device.OnPlatform(new Thickness(6, 6, 6, 6), new Thickness(6, 2, 2, 2), 2),
					Children = {scanner, entry, button },
				};
			}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the bottom entry.
		/// </summary>
		/// <value>The bottom entry.</value>
		public StackLayout BottomEntry { get; set; }

		/// <summary>
		/// Gets the scroll layout.
		/// </summary>
		/// <value>The scroll layout.</value>
		public PinchScrollView ScrollLayout { get; private set; }

		#endregion

		#region Events

		/// <summary>
		/// Handle back button pressed event.
		/// </summary>
		/// <returns>True, because back button should be ignored.</returns>
		protected override bool OnBackButtonPressed()
		{
			return true;
		}

		#endregion
	}
}
