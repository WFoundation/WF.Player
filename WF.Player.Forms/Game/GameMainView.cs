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
	public class GameMainView : BasePage
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

			NavigationPage.SetBackButtonTitle(this, string.Empty);

			this.SetBinding(GameMainView.TitleProperty, GameMainViewModel.TitelPropertyName);

			#if __IOS__

			var toolbarMenu = new ToolbarItem(Catalog.GetString("Menu"), null, () => { //"IconMenu.png", () => {
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

			var grid = new Grid() {
				RowSpacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			grid.RowDefinitions = new RowDefinitionCollection {
				new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
				new RowDefinition { Height = 60 }
			};

			grid.ColumnDefinitions = new ColumnDefinitionCollection {
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
			};

			var bottomLayout = new StackLayout() {
				Spacing = 0,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.Fill,
			};

			#if __IOS__

			// Dark grey line on iOS
			var line = new BoxView () 
				{
					BackgroundColor = App.Colors.IsDarkTheme ? Color.FromRgb(0x26, 0x26, 0x26) : Color.FromRgb (0xAE, 0xAE, 0xAE),
					HeightRequest = 0.5f,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			bottomLayout.Children.Add(line);

			#endif

			TapGestureRecognizer tapRecognizer;

			var buttonLayout = new StackLayout() 
			{
				BackgroundColor = App.Colors.Bar,
					Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				HeightRequest = 60, //72,
				MinimumHeightRequest = 60, //72,
			};

			// Overview button
			this.buttonOverview = new GameToolBarButton("IconOverview.png") 
				{
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonOverview.Image.SetBinding(BadgeImage.SelectedProperty, GameMainViewModel.IsOverviewSelectedPropertyName);

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
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonYouSee.Image.SetBinding(BadgeImage.SelectedProperty, GameMainViewModel.IsYouSeeSelectedPropertyName);
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
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonInventory.Image.SetBinding(BadgeImage.SelectedProperty, GameMainViewModel.IsInventorySelectedPropertyName);
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
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonTasks.Image.SetBinding(BadgeImage.SelectedProperty, GameMainViewModel.IsTasksSelectedPropertyName);
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
					HorizontalOptions = LayoutOptions.CenterAndExpand,
				};
			this.buttonMap.Image.SetBinding(BadgeImage.SelectedProperty, GameMainViewModel.IsMapSelectedPropertyName);

			tapRecognizer = new TapGestureRecognizer 
			{
				Command = gameMainViewModel.MapCommand,
				NumberOfTapsRequired = 1
			};

			this.buttonMap.GestureRecognizers.Add(tapRecognizer);

			buttonLayout.Children.Add(this.buttonMap);

			bottomLayout.Children.Add(buttonLayout);

			var labelEmpty = new Label() 
			{
				Text = Catalog.GetString("Please wait..."),
				BackgroundColor = Color.Transparent,
				TextColor = App.Colors.Text,
				Font = App.Fonts.Normal.WithSize(App.Prefs.TextSize),
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};
			labelEmpty.SetBinding(Label.TextProperty, GameMainViewModel.EmptyListTextPropertyName);
			labelEmpty.SetBinding(Label.IsVisibleProperty, GameMainViewModel.IsEmptyListTextVisiblePropertyName);

			var overviewScroll = new ScrollView() 
				{
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};
			overviewScroll.SetBinding(ScrollView.IsVisibleProperty, GameMainViewModel.IsOverviewVisiblePropertyName);

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
					UseMarkdown = true,
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
					UseMarkdown = true,
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
					UseMarkdown = true,
				};
			labelOverviewTasks.SetBinding(ExtendedLabel.TextProperty, GameMainViewModel.TasksOverviewContentPropertyName);

			layoutOverviewTasks.Children.Add(labelOverviewTasks);

			layoutOverview.Children.Add(layoutOverviewTasks);

			overviewScroll.Content = layoutOverview;

			var list = new ListView() 
			{
				BackgroundColor = Color.Transparent,
				ItemTemplate = new DataTemplate(typeof(GameMainCellView)),
				HasUnevenRows = false,
				RowHeight = 60,
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

			// Map view
			var mapViewModel = new MapViewModel();

			var mapView = new MapView(mapViewModel)
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			mapView.SetBinding(MapView.IsVisibleProperty, GameMainViewModel.IsMapSelectedPropertyName);

			gameMainViewModel.MapViewModel = mapViewModel;

			var layout = new StackLayout() 
				{
					BackgroundColor = Color.Transparent,
					Spacing = 0,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			layout.Children.Add(labelEmpty);
			layout.Children.Add(overviewScroll);
			layout.Children.Add(list);
			layout.Children.Add(mapView);

			grid.Children.Add(layout, 0, 0);
			grid.Children.Add(bottomLayout, 0, 1);

			Content = grid;
		}
	}
}
