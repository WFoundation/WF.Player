// <copyright file="GameMainView.cs" company="Wherigo Foundation">
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
	using Acr.XamForms.UserDialogs;
	using Vernacular;
	using WF.Player.Controls;
	using Xamarin.Forms;

	/// <summary>
	/// Game main view.
	/// </summary>
	public class GameMainView : BottomBarPage
	{
		/// <summary>
		/// The button you see.
		/// </summary>
		private GameToolBarButton buttonYouSee;

		/// <summary>
		/// The button inventory.
		/// </summary>
		private GameToolBarButton buttonInventory;

		/// <summary>
		/// The button tasks.
		/// </summary>
		private GameToolBarButton buttonTasks;

		/// <summary>
		/// The button map.
		/// </summary>
		private GameToolBarButton buttonMap;

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameMainView"/> class.
		/// </summary>
		/// <param name="gameMainViewModel">Game main view model.</param>
		public GameMainView(GameMainViewModel gameMainViewModel) : base()
		{
			BindingContext = gameMainViewModel;

			HasBackButton = false;
			this.SetBinding(GameMainView.TitleProperty, GameMainViewModel.TitelPropertyName);
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			#if __IOS__
				var toolbarMenu = new ToolbarItem (Catalog.GetString("Game Menu"), "IconMenu.png", async () => {
					App.Click();
					var cfg = new ActionSheetConfig().SetTitle(Catalog.GetString("Game Menu"));
					cfg.Add(Catalog.GetString("Save"), () => ((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Save")));
					cfg.Add(Catalog.GetString("Quit"), () => ((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Quit")));
					cfg.Add(Catalog.GetString("Cancel"), () => App.Click());
					DependencyService.Get<IUserDialogService>().ActionSheet(cfg);
				});
				this.ToolbarItems.Add (toolbarMenu);
			#endif
			#if __ANDROID__
			var toolbarSave = new ToolbarItem(Catalog.GetString("Save"), "", async () =>
				{ 
					App.Click();
					((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Save")); 
				}) {
				Order = ToolbarItemOrder.Secondary,
			};
			ToolbarItems.Add(toolbarSave);
			var toolbarQuit = new ToolbarItem(Catalog.GetString("Quit"), "", async () =>
				{ 
					App.Click();
					((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Quit")); 
				}) {
				Order = ToolbarItemOrder.Secondary,
			};
			ToolbarItems.Add(toolbarQuit);
			#endif

			var buttonLayout = new StackLayout() 
			{
				BackgroundColor = App.Colors.BackgroundButtons,
				Orientation = StackOrientation.Horizontal,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			// You See button
			this.buttonYouSee = new GameToolBarButton("IconLocation.png") 
			{
				BackgroundColor = Color.Transparent,
				SelectedColor = App.Colors.Background,
				Padding = 10,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			this.buttonYouSee.SetBinding(GameToolBarButton.SelectedProperty, GameMainViewModel.IsYouSeeSelectedPropertyName);
			this.buttonYouSee.Image.SetBinding(BadgeImage.NumberProperty, GameMainViewModel.YouSeeNumberPropertyName);

			var tapRecognizer = new TapGestureRecognizer 
			{
				Command = gameMainViewModel.YouSeeCommand,
				NumberOfTapsRequired = 1
			};

			this.buttonYouSee.GestureRecognizers.Add(tapRecognizer);

			buttonLayout.Children.Add(this.buttonYouSee);

			// Inventory button
			this.buttonInventory = new GameToolBarButton("IconInventory.png") 
			{
				BackgroundColor = Color.Transparent,
				SelectedColor = App.Colors.Background,
				Padding = 10,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			this.buttonInventory.SetBinding(GameToolBarButton.SelectedProperty, GameMainViewModel.IsInventorySelectedPropertyName);
			this.buttonInventory.Image.SetBinding(BadgeImage.NumberProperty, GameMainViewModel.InventoryNumberPropertyName);

			tapRecognizer = new TapGestureRecognizer 
			{
				Command = gameMainViewModel.InventoryCommand,
				NumberOfTapsRequired = 1
			};

			this.buttonInventory.GestureRecognizers.Add(tapRecognizer);

			buttonLayout.Children.Add(this.buttonInventory);

			// Tasks button
			this.buttonTasks = new GameToolBarButton("IconTask.png") 
			{
				BackgroundColor = Color.Transparent,
				SelectedColor = App.Colors.Background,
				Padding = 10,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			this.buttonTasks.SetBinding(GameToolBarButton.SelectedProperty, GameMainViewModel.IsTasksSelectedPropertyName);
			this.buttonTasks.Image.SetBinding(BadgeImage.NumberProperty, GameMainViewModel.TasksNumberPropertyName);

			tapRecognizer = new TapGestureRecognizer 
			{
				Command = gameMainViewModel.TasksCommand,
				NumberOfTapsRequired = 1
			};

			this.buttonTasks.GestureRecognizers.Add(tapRecognizer);

			buttonLayout.Children.Add(this.buttonTasks);

			// Map button
			this.buttonMap = new GameToolBarButton("IconMap.png") 
			{
				BackgroundColor = Color.Transparent,
				SelectedColor = App.Colors.Background,
				Padding = 10,
				HasShadow = false,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			this.buttonMap.SetBinding(GameToolBarButton.SelectedProperty, GameMainViewModel.IsMapSelectedPropertyName);

			tapRecognizer = new TapGestureRecognizer 
			{
				Command = gameMainViewModel.MapCommand,
				NumberOfTapsRequired = 1
			};

			this.buttonMap.GestureRecognizers.Add(tapRecognizer);

			buttonLayout.Children.Add(this.buttonMap);

			// Create layout for ListView
			var listLayout = new StackLayout() 
			{
				BackgroundColor = Color.Transparent,
				Padding = 0,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var label = new Label() 
			{
				Text = Catalog.GetString("Please wait..."),
				BackgroundColor = Color.Transparent,
				TextColor = App.Colors.Text,
				Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				IsVisible = false,
			};
			label.SetBinding(Label.TextProperty, GameMainViewModel.EmptyListTextPropertyName);
			label.SetBinding(Label.IsVisibleProperty, GameMainViewModel.IsEmptyListTextVisiblePropertyName);

			listLayout.Children.Add(label);

			var list = new ListView() 
			{
				BackgroundColor = Color.Transparent,
				ItemTemplate = new DataTemplate(typeof(GameMainCellView)),
				HasUnevenRows = false,
				RowHeight = 60,
				IsVisible = false,
			};
			list.SetBinding(ListView.IsVisibleProperty, GameMainViewModel.IsListVisiblePropertyName);
			list.SetBinding(ListView.ItemsSourceProperty, GameMainViewModel.GameMainListPropertyName);
			list.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
			{
				// Get selected MenuEntry
				GameMainCellViewModel entry = (GameMainCellViewModel)e.SelectedItem;

				// If MenuEntry is null (unselected item), than leave
				if (entry == null)
				{
					return;
					}

				App.Click();

				if (entry.UIObject.HasOnClick)
				{
					// Object has a OnClick event, so call this
					entry.UIObject.CallOnClick();
				}
				else
				{
					// Show detail screen for object
					App.Game.ShowScreen(ScreenType.Details, entry.UIObject);
				}

				// Deselect MenuEntry
				list.SelectedItem = null;
			};

			listLayout.Children.Add(list);

			var layout = new StackLayout() 
			{
				BackgroundColor = App.Colors.Background,
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			layout.Children.Add(listLayout);
			layout.Children.Add(buttonLayout);

			((StackLayout)ContentLayout).Children.Add(layout);

			// Create position entries at bottom of screen
			var latitude = new Label() 
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				XAlign = TextAlignment.Start,
				TextColor = App.Colors.Text,
			};
			latitude.SetBinding(Label.TextProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToLatitude());

			var longitude = new Label() 
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				XAlign = TextAlignment.Start,
				TextColor = App.Colors.Text,
			};
			longitude.SetBinding(Label.TextProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToLongitude());

			var altitude = new Label() 
			{
				XAlign = TextAlignment.End,
				TextColor = App.Colors.Text,
			};
			altitude.SetBinding(Label.TextProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAltitude());

			var accuracy = new Label() 
			{
				XAlign = TextAlignment.End,
				TextColor = App.Colors.Text,
			};
			accuracy.SetBinding(Label.TextProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAccuracy());

			var imageAltitude = new Image() 
			{
				Source = App.Colors.IsDarkTheme ? "IconAltitudeLight" : "IconAltitudeDark",
				WidthRequest = 16,
				HeightRequest = 16,
			};
			imageAltitude.SetBinding(Image.IsVisibleProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAltitudeVisibility());

			var imageAccuracy = new Image() 
			{
				Source = App.Colors.IsDarkTheme ? "IconAccuracyLight" : "IconAccuracyDark",
				WidthRequest = 16,
				HeightRequest = 16,
			};
			imageAccuracy.SetBinding(Image.IsVisibleProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAccuracyVisibility());

			var layoutBottomVert1 = new StackLayout() 
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(10, 2),
				Spacing = 0,
			};

			layoutBottomVert1.Children.Add(latitude);
			layoutBottomVert1.Children.Add(longitude);

			var layoutBottomVert2 = new StackLayout() 
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(0, 2),
				Spacing = 0,
			};

			layoutBottomVert2.Children.Add(altitude);
			layoutBottomVert2.Children.Add(accuracy);

			var layoutBottomVert3 = new StackLayout() 
			{
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.Center,
				Padding = new Thickness(10, 2),
				Spacing = 4,
			};

			layoutBottomVert3.Children.Add(imageAltitude);
			layoutBottomVert3.Children.Add(imageAccuracy);

			var layoutBottomHori = new StackLayout() 
			{
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Spacing = 0,
			};

			layoutBottomHori.Children.Add(layoutBottomVert1);
			layoutBottomHori.Children.Add(layoutBottomVert2);
			layoutBottomHori.Children.Add(layoutBottomVert3);

			BottomLayout.Children.Add(layoutBottomHori);
		}

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing()
		{
			App.CurrentPage = this;

			base.OnAppearing();

			this.SetBinding(GameMainView.TitleProperty, GameMainViewModel.TitelPropertyName);
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
		}
	}
}
