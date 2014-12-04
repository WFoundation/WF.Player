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
using Xamarin.Forms.Maps;

namespace WF.Player
{
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
		private GameToolBarButton buttonOverview;

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

			var toolbarMenu = new ToolbarItem (Catalog.GetString("Game Menu"), "IconMenu.png", () => {
				App.Click();
				var cfg = new Acr.XamForms.UserDialogs.ActionSheetConfig().SetTitle(Catalog.GetString("Game Menu"));
				cfg.Add(Catalog.GetString("Save"), () => ((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Save")));
				cfg.Add(Catalog.GetString("Quit"), () => ((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Quit")));
				cfg.Cancel = new Acr.XamForms.UserDialogs.ActionSheetOption(Catalog.GetString("Cancel"), App.Click);
				DependencyService.Get<Acr.XamForms.UserDialogs.IUserDialogService>().ActionSheet(cfg);
			});
			this.ToolbarItems.Add (toolbarMenu);

			#endif

			#if __ANDROID__

			var toolbarSave = new ToolbarItem(Catalog.GetString("Save"), "", () =>
				{ 
					App.Click();
					((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Save")); 
				}) {
				Order = ToolbarItemOrder.Secondary,
			};
			ToolbarItems.Add(toolbarSave);
			var toolbarQuit = new ToolbarItem(Catalog.GetString("Quit"), "", () =>
				{ 
					App.Click();
					((GameMainViewModel)BindingContext).HandleMenuAction(this, Catalog.GetString("Quit")); 
				}) {
				Order = ToolbarItemOrder.Secondary,
			};
			ToolbarItems.Add(toolbarQuit);

			#endif

			TapGestureRecognizer tapRecognizer;

			var buttonLayout = new StackLayout() 
			{
				BackgroundColor = App.Colors.BackgroundButtons,
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 72,
				MinimumHeightRequest = 72,
			};

			// Overview button
			this.buttonOverview = new GameToolBarButton("IconOverview.png") 
				{
					BackgroundColor = Color.Transparent,
					SelectedColor = App.Colors.Background,
					Padding = new Thickness(2, 10),
					HasShadow = false,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonOverview.SetBinding(GameToolBarButton.SelectedProperty, GameMainViewModel.IsOverviewSelectedPropertyName);

			tapRecognizer = new TapGestureRecognizer 
				{
					Command = gameMainViewModel.OverviewCommand,
					NumberOfTapsRequired = 1
				};

			this.buttonOverview.GestureRecognizers.Add(tapRecognizer);

			buttonLayout.Children.Add(this.buttonOverview);

			// You See button
			this.buttonYouSee = new GameToolBarButton("IconLocation.png") 
				{
					BackgroundColor = Color.Transparent,
					SelectedColor = App.Colors.Background,
					Padding = new Thickness(2, 10),
					HasShadow = false,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonYouSee.SetBinding(GameToolBarButton.SelectedProperty, GameMainViewModel.IsYouSeeSelectedPropertyName);
			this.buttonYouSee.Image.SetBinding(BadgeImage.NumberProperty, GameMainViewModel.YouSeeNumberPropertyName);

			tapRecognizer = new TapGestureRecognizer 
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
					Padding = new Thickness(2, 10),
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
			this.buttonTasks = new GameToolBarButton("IconTasks.png") 
				{
					BackgroundColor = Color.Transparent,
					SelectedColor = App.Colors.Background,
					Padding = new Thickness(2, 10),
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
					Padding = new Thickness(2, 10),
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
			listLayout.SetBinding(StackLayout.IsVisibleProperty, GameMainViewModel.IsMapNotSelectedPropertyName);

			var label = new Label() 
			{
				Text = Catalog.GetString("Please wait..."),
				BackgroundColor = Color.Transparent,
				TextColor = App.Colors.Text,
				Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				IsVisible = true,
			};
			label.SetBinding(Label.TextProperty, GameMainViewModel.EmptyListTextPropertyName);
			label.SetBinding(Label.IsVisibleProperty, GameMainViewModel.IsEmptyListTextVisiblePropertyName);

			listLayout.Children.Add(label);

			var overview = new ScrollView() 
				{
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					IsVisible = false,
				};
			overview.SetBinding(ScrollView.IsVisibleProperty, GameMainViewModel.IsOverviewVisiblePropertyName);

			var layoutOverview = new StackLayout() 
				{
					Orientation = StackOrientation.Vertical,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			var layoutOverviewYouSee = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = 10,
					Spacing = 10,
				};

			tapRecognizer = new TapGestureRecognizer 
				{
					Command = gameMainViewModel.YouSeeCommand,
					NumberOfTapsRequired = 1
				};

			layoutOverviewYouSee.GestureRecognizers.Add(tapRecognizer);

			var imageOverviewYouSee = new Image() 
				{
					Source = "IconLocation.png",
					VerticalOptions = LayoutOptions.Start,
				};

			layoutOverviewYouSee.Children.Add(imageOverviewYouSee);

			var labelOverviewYouSee = new ExtendedLabel() 
				{
					LineBreakMode = LineBreakMode.WordWrap,
					Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize * 0.8),
				};
			labelOverviewYouSee.SetBinding(ExtendedLabel.TextProperty, GameMainViewModel.YouSeeOverviewContentPropertyName);

			layoutOverviewYouSee.Children.Add(labelOverviewYouSee);

			layoutOverview.Children.Add(layoutOverviewYouSee);

			var layoutOverviewInventory = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = 10,
					Spacing = 10,
				};

			tapRecognizer = new TapGestureRecognizer 
				{
					Command = gameMainViewModel.InventoryCommand,
					NumberOfTapsRequired = 1
				};

			layoutOverviewInventory.GestureRecognizers.Add(tapRecognizer);

			var imageOverviewInventory = new Image() 
				{
					Source = "IconInventory.png",
					VerticalOptions = LayoutOptions.Start,
				};

			layoutOverviewInventory.Children.Add(imageOverviewInventory);

			var labelOverviewInventory = new ExtendedLabel() 
				{
					LineBreakMode = LineBreakMode.WordWrap,
					Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize * 0.8),
				};
			labelOverviewInventory.SetBinding(ExtendedLabel.TextProperty, GameMainViewModel.InventoryOverviewContentPropertyName);

			layoutOverviewInventory.Children.Add(labelOverviewInventory);

			layoutOverview.Children.Add(layoutOverviewInventory);

			var layoutOverviewTasks = new StackLayout()
				{
					Orientation = StackOrientation.Horizontal,
					VerticalOptions = LayoutOptions.Fill,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					Padding = 10,
					Spacing = 10,
				};

			tapRecognizer = new TapGestureRecognizer 
				{
					Command = gameMainViewModel.TasksCommand,
					NumberOfTapsRequired = 1
				};

			layoutOverviewTasks.GestureRecognizers.Add(tapRecognizer);

			var imageOverviewTasks = new Image() 
				{
					Source = "IconTasks.png",
					VerticalOptions = LayoutOptions.Start,
				};

			layoutOverviewTasks.Children.Add(imageOverviewTasks);

			var labelOverviewTasks = new ExtendedLabel() 
				{
					LineBreakMode = LineBreakMode.WordWrap,
					Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize * 0.8),
				};
			labelOverviewTasks.SetBinding(ExtendedLabel.TextProperty, GameMainViewModel.TasksOverviewContentPropertyName);

			layoutOverviewTasks.Children.Add(labelOverviewTasks);

			layoutOverview.Children.Add(layoutOverviewTasks);

			overview.Content = layoutOverview;

			listLayout.Children.Add(overview);

			var list = new ListView() 
			{
				BackgroundColor = Color.Transparent,
				ItemTemplate = new DataTemplate(typeof(GameMainCellView)),
				HasUnevenRows = false,
				RowHeight = 60,
				IsVisible = false,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
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

			// Map view
			var mapViewModel = new MapViewModel();

			var mapView = new MapView(mapViewModel)
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HeightRequest = 250,
				};
			mapView.SetBinding(MapView.IsVisibleProperty, GameMainViewModel.IsMapSelectedPropertyName);

			gameMainViewModel.Map = mapViewModel.Map;

			var layout = new StackLayout() 
			{
				BackgroundColor = Color.Transparent,
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			layout.Children.Add(listLayout);
			layout.Children.Add(mapView);
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
				WidthRequest = 16,
				HeightRequest = 16,
			};
			imageAltitude.SetBinding(Image.SourceProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAltitudeVisibility());

			var imageAccuracy = new Image() 
			{
				WidthRequest = 16,
				HeightRequest = 16,
			};
			imageAccuracy.SetBinding(Image.SourceProperty, GameMainViewModel.PositionPropertyName, BindingMode.OneWay, new ConverterToAccuracyVisibility());

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
