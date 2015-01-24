// <copyright file="GameMainViewModel.cs" company="Wherigo Foundation">
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
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using WF.Player.Services.UserDialogs;
	using Vernacular;
	using WF.Player.Core;
	using WF.Player.Core.Utils;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;
	using Xamarin.Forms.Maps;

	/// <summary>
	/// Game main view model.
	/// </summary>
	public class GameMainViewModel : BaseViewModel
	{
		/// <summary>
		/// The name of the active screen property.
		/// </summary>
		public const string ActiveScreenPropertyName = "ActiveScreen";

		/// <summary>
		/// The name of the position property.
		/// </summary>
		public const string PositionPropertyName = "Position";

		/// <summary>
		/// The name of the game main list property.
		/// </summary>
		public const string GameMainListPropertyName = "GameMainList";

		/// <summary>
		/// The name of the is you see selected property.
		/// </summary>
		public const string IsBackbuttonVisiblePropertyName = "IsBackbuttonVisible";

		/// <summary>
		/// The name of the is you see selected property.
		/// </summary>
		public const string IsOverviewSelectedPropertyName = "IsOverviewSelected";

		/// <summary>
		/// The name of the is you see selected property.
		/// </summary>
		public const string IsYouSeeSelectedPropertyName = "IsYouSeeSelected";

		/// <summary>
		/// The name of the is inventory selected property.
		/// </summary>
		public const string IsInventorySelectedPropertyName = "IsInventorySelected";

		/// <summary>
		/// The name of the is tasks selected property.
		/// </summary>
		public const string IsTasksSelectedPropertyName = "IsTasksSelected";

		/// <summary>
		/// The name of the is map selected property.
		/// </summary>
		public const string IsMapSelectedPropertyName = "IsMapSelected";

		/// <summary>
		/// The name of the is map not selected property.
		/// </summary>
		public const string IsMapNotSelectedPropertyName = "IsMapNotSelected";

		/// <summary>
		/// The name of the has list icons property.
		/// </summary>
		public const string HasListIconsPropertyName = "HasListIcons";

		/// <summary>
		/// The name of the is overview visible property.
		/// </summary>
		public const string IsOverviewVisiblePropertyName = "IsOverviewVisible";

		/// <summary>
		/// The name of the is list visible property.
		/// </summary>
		public const string IsListVisiblePropertyName = "IsListVisible";

		/// <summary>
		/// The name of the is empty list text visible property.
		/// </summary>
		public const string IsEmptyListTextVisiblePropertyName = "IsEmptyListTextVisible";

		/// <summary>
		/// The name of the titel property.
		/// </summary>
		public const string TitelPropertyName = "Titel";

		/// <summary>
		/// The empty name of the list text property.
		/// </summary>
		public const string EmptyListTextPropertyName = "EmptyListText";

		/// <summary>
		/// The name of the you see number property.
		/// </summary>
		public const string YouSeeNumberPropertyName = "YouSeeNumber";

		/// <summary>
		/// The name of the inventory number property.
		/// </summary>
		public const string InventoryNumberPropertyName = "InventoryNumber";

		/// <summary>
		/// The name of the tasks number property.
		/// </summary>
		public const string TasksNumberPropertyName = "TasksNumber";

		/// <summary>
		/// The name of the you see overview content property.
		/// </summary>
		public const string YouSeeOverviewContentPropertyName = "YouSeeOverviewContent";

		/// <summary>
		/// The name of the inventory overview content property.
		/// </summary>
		public const string InventoryOverviewContentPropertyName = "InventoryOverviewContent";

		/// <summary>
		/// The name of the tasks overview content property.
		/// </summary>
		public const string TasksOverviewContentPropertyName = "TasksOverviewContent";

		/// <summary>
		/// The name of the you see command property.
		/// </summary>
		public const string OverviewCommandPropertyName = "OverviewCommand";

		/// <summary>
		/// The name of the you see command property.
		/// </summary>
		public const string YouSeeCommandPropertyName = "YouSeeCommand";

		/// <summary>
		/// The name of the inventory command property.
		/// </summary>
		public const string InventoryCommandPropertyName = "InventoryCommand";

		/// <summary>
		/// The name of the tasks command property.
		/// </summary>
		public const string TasksCommandPropertyName = "TasksCommand";

		/// <summary>
		/// The name of the map command property.
		/// </summary>
		public const string MapCommandPropertyName = "MapCommand";

		/// <summary>
		/// The game model.
		/// </summary>
		private GameModel gameModel;

		/// <summary>
		/// The map view model.
		/// </summary>
		private MapViewModel mapViewModel;

		/// <summary>
		/// The geo math helper.
		/// </summary>
		private GeoMathHelper geoMathHelper;

		/// <summary>
		/// The active screen.
		/// </summary>
		private ScreenType activeScreen;

		/// <summary>
		/// The position.
		/// </summary>
		private WF.Player.Services.Geolocation.Position position;

		/// <summary>
		/// The game main list.
		/// </summary>
		private List<GameMainCellViewModel> gameMainList = new List<GameMainCellViewModel>();

		/// <summary>
		/// The has list icons.
		/// </summary>
		private bool hasListIcons;

		/// <summary>
		/// The content of the you see overview on main screen.
		/// </summary>
		private string youSeeOverviewContent = string.Format("## {0}{1}{2}", Catalog.GetString("You See"), Environment.NewLine, Catalog.GetString("Nothing of interest"));

		/// <summary>
		/// The content of the inventory overview on main screen.
		/// </summary>
		private string inventoryOverviewContent = string.Format("## {0}{1}{2}", Catalog.GetString("Inventory"), Environment.NewLine, Catalog.GetString("No items"));

		/// <summary>
		/// The content of the tasks overview on main screen.
		/// </summary>
		private string tasksOverviewContent = string.Format("## {0}{1}{2}", Catalog.GetString("Tasks"), Environment.NewLine, Catalog.GetString("No new tasks"));

		/// <summary>
		/// You see number.
		/// </summary>
		private int youSeeNumber;

		/// <summary>
		/// The inventory number.
		/// </summary>
		private int inventoryNumber;

		/// <summary>
		/// The tasks number.
		/// </summary>
		private int tasksNumber;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameMainViewModel"/> class.
		/// </summary>
		/// <param name="gameModel">Game model.</param>
		public GameMainViewModel(GameModel gameModel)
		{
			this.gameModel = gameModel;
			this.geoMathHelper = new GeoMathHelper();

			Position = App.GPS.LastKnownPosition;

			this.gameModel.DisplayChanged += OnDisplayChanged;

			IsBusy = true;
		}

		#endregion

		#region Properties

		#region ActiveScreen

		/// <summary>
		/// Gets or sets the active screen.
		/// </summary>
		/// <value>The active screen.</value>
		public ScreenType ActiveScreen
		{
			get
			{
				return this.activeScreen;
			}

			set
			{
				if (this.activeScreen != value)
				{
					var lastScreen = this.activeScreen;
					this.activeScreen = value;

					// Notify old screen about update
					switch (lastScreen)
					{
						case ScreenType.Main:
							NotifyPropertyChanged(IsOverviewSelectedPropertyName);
							break;
						case ScreenType.Locations:
						case ScreenType.Items:
							NotifyPropertyChanged(IsYouSeeSelectedPropertyName);
							break;
						case ScreenType.Inventory:
							NotifyPropertyChanged(IsInventorySelectedPropertyName);
							break;
						case ScreenType.Tasks:
							NotifyPropertyChanged(IsTasksSelectedPropertyName);
							break;
						case ScreenType.Map:
							NotifyPropertyChanged(IsMapSelectedPropertyName);
							NotifyPropertyChanged(IsMapNotSelectedPropertyName);
							break;
					}

					// Notify new screen about update
					switch (this.activeScreen)
					{
						case ScreenType.Main:
							NotifyPropertyChanged(IsOverviewSelectedPropertyName);
							App.GameNavigation.ShowBackButton = false;
							break;
						case ScreenType.Locations:
						case ScreenType.Items:
							NotifyPropertyChanged(IsYouSeeSelectedPropertyName);
							App.GameNavigation.ShowBackButton = true;
							break;
						case ScreenType.Inventory:
							NotifyPropertyChanged(IsInventorySelectedPropertyName);
							App.GameNavigation.ShowBackButton = true;
							break;
						case ScreenType.Tasks:
							NotifyPropertyChanged(IsTasksSelectedPropertyName);
							App.GameNavigation.ShowBackButton = true;
							break;
						case ScreenType.Map:
							NotifyPropertyChanged(IsMapSelectedPropertyName);
							NotifyPropertyChanged(IsMapNotSelectedPropertyName);
							App.GameNavigation.ShowBackButton = true;
							break;
					}

					NotifyPropertyChanged(IsBackbuttonVisiblePropertyName);
				}

				Refresh();
			}
		}

		#endregion

		#region Position

		/// <summary>
		/// Gets Position from the actual location.
		/// </summary>
		/// <value>The Position.</value>
		public WF.Player.Services.Geolocation.Position Position
		{
			get
			{
				return this.position;
			}

			internal set
			{
				SetProperty<WF.Player.Services.Geolocation.Position>(ref this.position, value, PositionPropertyName);
			}
		}

		#endregion

		#region Map

		/// <summary>
		/// Gets or sets the map view model.
		/// </summary>
		/// <value>The map.</value>
		public MapViewModel MapViewModel
		{ 
			get
			{
				return mapViewModel;
			}

			set
			{
				if (mapViewModel != value)
				{
					mapViewModel = value;
					mapViewModel.Position = null;
				}
			}
		}

		#endregion

		#region Name

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name 
		{
			get 
			{
				return this.gameModel.Cartridge.Name;
			}
		}

		#endregion

		#region Main Screen List

		/// <summary>
		/// Gets the game main list.
		/// </summary>
		/// <value>The game main list.</value>
		public List<GameMainCellViewModel> GameMainList
		{
			get 
			{
				return this.gameMainList;
			}
		}

		#endregion

		#region IsBackbuttonVisible

		/// <summary>
		/// Gets a value indicating whether backbutton should be visible.
		/// </summary>
		/// <value><c>true</c> if backbutton should be visible; otherwise, <c>false</c>.</value>
		public bool IsBackbuttonVisible
		{
			get 
			{
				return this.activeScreen != ScreenType.Main; 
			}
		}

		#endregion

		#region IsOverviewSelected

		/// <summary>
		/// Gets a value indicating whether overview is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is you see selected; otherwise, <c>false</c>.</value>
		public bool IsOverviewSelected
		{
			get 
			{
				return this.activeScreen == ScreenType.Main; 
			}
		}

		#endregion

		#region IsYouSeeSelected

		/// <summary>
		/// Gets a value indicating whether this instance is you see selected.
		/// </summary>
		/// <value><c>true</c> if this instance is you see selected; otherwise, <c>false</c>.</value>
		public bool IsYouSeeSelected
		{
			get 
			{
				return this.activeScreen == ScreenType.Locations || this.activeScreen == ScreenType.Items; 
			}
		}

		#endregion

		#region IsInventorySelected

		/// <summary>
		/// Gets a value indicating whether this instance is inventory selected.
		/// </summary>
		/// <value><c>true</c> if this instance is inventory selected; otherwise, <c>false</c>.</value>
		public bool IsInventorySelected
		{
			get 
			{
				return this.activeScreen == ScreenType.Inventory;
			}
		}

		#endregion

		#region IsTasksSelected

		/// <summary>
		/// Gets a value indicating whether this instance is tasks selected.
		/// </summary>
		/// <value><c>true</c> if this instance is tasks selected; otherwise, <c>false</c>.</value>
		public bool IsTasksSelected
		{
			get 
			{
				return this.activeScreen == ScreenType.Tasks;
			}
		}

		#endregion

		#region IsMapSelected

		/// <summary>
		/// Gets a value indicating whether this instance is map selected.
		/// </summary>
		/// <value><c>true</c> if this instance is map selected; otherwise, <c>false</c>.</value>
		public bool IsMapSelected
		{
			get 
			{
				return this.activeScreen == ScreenType.Map;
			}
		}

		#endregion

		#region IsMapNotSelected

		/// <summary>
		/// Gets a value indicating whether this instance is map not selected.
		/// </summary>
		/// <value><c>true</c> if this instance is map not selected; otherwise, <c>false</c>.</value>
		public bool IsMapNotSelected
		{
			get 
			{
				return this.activeScreen != ScreenType.Map;
			}
		}

		#endregion

		#region HasListIcons

		/// <summary>
		/// Gets a value indicating whether this instance has list icons.
		/// </summary>
		/// <value><c>true</c> if this instance has list icons; otherwise, <c>false</c>.</value>
		public bool HasListIcons
		{
			get 
			{
				return this.hasListIcons;
			}
		}

		#endregion

		#region IsOverviewVisible

		/// <summary>
		/// Gets a value indicating whether overview is visible.
		/// </summary>
		/// <value><c>true</c> if overview is visible; otherwise, <c>false</c>.</value>
		public bool IsOverviewVisible
		{
			get 
			{
				return this.activeScreen == ScreenType.Main && gameModel.GameState == WF.Player.Core.Engines.EngineGameState.Playing;
			}
		}

		#endregion

		#region IsListVisible

		/// <summary>
		/// Gets a value indicating whether this instance is list visible.
		/// </summary>
		/// <value><c>true</c> if this instance is list visible; otherwise, <c>false</c>.</value>
		public bool IsListVisible
		{
			get 
			{
				// Never show list for map and main screen
				if (this.activeScreen == ScreenType.Map || this.activeScreen == ScreenType.Main)
				{
					return false;
				}
				else
				{
					return GameMainList.Count > 0;
				}
			}
		}

		#endregion

		#region IsEmptyListTextVisible

		/// <summary>
		/// Gets a value indicating whether this instance is empty list text visible.
		/// </summary>
		/// <value><c>true</c> if this instance is empty list text visible; otherwise, <c>false</c>.</value>
		public bool IsEmptyListTextVisible
		{
			get 
			{
				// Never show empty text for map
				if (this.activeScreen == ScreenType.Map || (this.activeScreen == ScreenType.Main && gameModel.GameState == WF.Player.Core.Engines.EngineGameState.Playing))
				{
					return false;
				}
				else
				{
					return GameMainList.Count == 0;
				}
			}
		}

		#endregion

		#region Titel

		/// <summary>
		/// Gets the titel.
		/// </summary>
		/// <value>The titel.</value>
		public string Titel
		{
			get 
			{
				if (this.activeScreen == ScreenType.Main)
				{
					return Catalog.GetString("Overview");
				}

				if (IsYouSeeSelected)
				{
					return Catalog.GetString("You See");
				}

				if (this.activeScreen == ScreenType.Inventory)
				{
					return Catalog.GetString("Inventory");
				}

				if (this.activeScreen == ScreenType.Tasks)
				{
					return Catalog.GetString("Tasks");
				}

				if (this.activeScreen == ScreenType.Map)
				{
					return Catalog.GetString("Map");
				}

				return string.Empty;
			}
		}

		#endregion

		#region EmptyListText

		/// <summary>
		/// Gets the empty list text.
		/// </summary>
		/// <value>The empty list text.</value>
		public string EmptyListText
		{
			get 
			{
				if (this.gameModel == null || this.gameModel.Cartridge == null)
				{
					return Catalog.GetString("Loading...");
				}

				if (IsYouSeeSelected)
				{
					return this.gameModel.Cartridge.EmptyYouSeeListText;
				}

				if (this.activeScreen == ScreenType.Inventory)
				{
					return this.gameModel.Cartridge.EmptyInventoryListText;
				}

				if (this.activeScreen == ScreenType.Tasks)
				{
					return this.gameModel.Cartridge.EmptyTasksListText;
				}

				return string.Empty;
			}
		}

		#endregion

		#region YouSeeOverviewContent

		/// <summary>
		/// Gets or sets the content of the you see overview.
		/// </summary>
		/// <value>The content of the you see overview.</value>
		public string YouSeeOverviewContent
		{
			get
			{
				return this.youSeeOverviewContent;
			}

			set
			{
				SetProperty<string>(ref this.youSeeOverviewContent, value, YouSeeOverviewContentPropertyName);
			}
		}

		#endregion

		#region InventoryOverviewContent

		/// <summary>
		/// Gets or sets the content of the inventory overview.
		/// </summary>
		/// <value>The content of the inventory overview.</value>
		public string InventoryOverviewContent
		{
			get
			{
				return this.inventoryOverviewContent;
			}

			set
			{
				SetProperty<string>(ref this.inventoryOverviewContent, value, InventoryOverviewContentPropertyName);
			}
		}

		#endregion

		#region TasksOverviewContent

		/// <summary>
		/// Gets or sets the content of the tasks overview.
		/// </summary>
		/// <value>The content of the tasks overview.</value>
		public string TasksOverviewContent
		{
			get
			{
				return this.tasksOverviewContent;
			}

			set
			{
				SetProperty<string>(ref this.tasksOverviewContent, value, TasksOverviewContentPropertyName);
			}
		}

		#endregion

		#region YouSeeNumber

		/// <summary>
		/// Gets or sets you see number.
		/// </summary>
		/// <value>You see number.</value>
		public int YouSeeNumber
		{
			get
			{
				return this.youSeeNumber; 
			}

			set
			{
				SetProperty<int>(ref this.youSeeNumber, value, YouSeeNumberPropertyName);
			}
		}

		#endregion

		#region InventoryNumber

		/// <summary>
		/// Gets or sets the inventory number.
		/// </summary>
		/// <value>The inventory number.</value>
		public int InventoryNumber
		{
			get
			{
				return this.inventoryNumber;
			}

			set
			{
				SetProperty<int>(ref this.inventoryNumber, value, InventoryNumberPropertyName);
			}
		}

		#endregion

		#region TasksNumber

		/// <summary>
		/// Gets or sets the tasks number.
		/// </summary>
		/// <value>The tasks number.</value>
		public int TasksNumber
		{
			get
			{
				return this.tasksNumber;
			}

			set
			{
				SetProperty<int>(ref this.tasksNumber, value, TasksNumberPropertyName);
			}
		}

		#endregion

		#endregion

		#region Commands

		#region Overview Command

		/// <summary>
		/// Gets overview command.
		/// </summary>
		/// <value>Overview command.</value>
		public Xamarin.Forms.Command OverviewCommand
		{
			get
			{
				return new Xamarin.Forms.Command(() => ExecuteScreenChanged(ScreenType.Main));
			}
		}

		#endregion

		#region You See Command

		/// <summary>
		/// Gets you see command.
		/// </summary>
		/// <value>You see command.</value>
		public Xamarin.Forms.Command YouSeeCommand
		{
			get
			{
				return new Xamarin.Forms.Command(() => ExecuteScreenChanged(ScreenType.Locations));
			}
		}

		#endregion

		#region Inventory Command

		/// <summary>
		/// Gets the inventory command.
		/// </summary>
		/// <value>The inventory command.</value>
		public Xamarin.Forms.Command InventoryCommand
		{
			get
			{
				return new Xamarin.Forms.Command(() => ExecuteScreenChanged(ScreenType.Inventory));
			}
		}

		#endregion

		#region Tasks Command

		/// <summary>
		/// Gets the tasks command.
		/// </summary>
		/// <value>The tasks command.</value>
		public Xamarin.Forms.Command TasksCommand
		{
			get
			{
				return new Xamarin.Forms.Command(() => ExecuteScreenChanged(ScreenType.Tasks));
			}
		}

		#endregion

		#region Map Command

		/// <summary>
		/// Gets the map command.
		/// </summary>
		/// <value>The map command.</value>
		public Xamarin.Forms.Command MapCommand
		{
			get
			{
				return new Xamarin.Forms.Command(() => ExecuteMapCommand());
			}
		}

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Handles the menu action.
		/// </summary>
		/// <param name="page">Active page.</param>
		/// <param name="action">Action of menu selected.</param>
		public async void HandleMenuAction(Page page, string action)
		{
			// Notify user
			App.Click();

			if (action == Catalog.GetString("Quit"))
			{
				bool result = await DependencyService.Get<IUserDialogService>().ConfirmAsync(Catalog.GetString("Would you like to save the current game?"), Catalog.GetString("Save Game"), Catalog.GetString("Yes"), Catalog.GetString("No"));

				App.Click();

				if (result)
				{
					// Ask for savegame name
					var cfg = new PromptConfig();
					cfg.Message = Catalog.GetString("Provide a comment to identify this savefile");
					cfg.Title = Catalog.GetString("Comment Savefile");
					cfg.OnResult = (savegameResult) =>
					{
						Device.BeginInvokeOnMainThread(() =>
							{
								App.Click();
								if (savegameResult.Ok)
								{
									this.gameModel.Save(savegameResult.Text);

									// Stop engine
									this.gameModel.Stop();

									App.Game = null;

									// Remove active screen
									App.GameNavigation.CurrentPage.Navigation.PopModalAsync();
									App.GameNavigation = null;
								}
							});
					};
					DependencyService.Get<IUserDialogService>().Prompt(cfg);
				}
				else
				{
					// Stop engine
					this.gameModel.Stop();

					App.Game = null;

					// Remove active screen
					App.GameNavigation.CurrentPage.Navigation.PopModalAsync();
					App.GameNavigation = null;
				}
			}

			if (action == Catalog.GetString("Save"))
			{
				// Ask for savegame name
				var cfg = new PromptConfig();
				cfg.Message = Catalog.GetString("Provide a comment to identify this savefile");
				cfg.Title = Catalog.GetString("Comment Savefile");
				cfg.OnResult = (savegameResult) =>
					{
						Device.BeginInvokeOnMainThread(() =>
							{
								App.Click();
								if (savegameResult.Ok)
								{
									this.gameModel.Save(savegameResult.Text);
								}
							});
					};
				DependencyService.Get<IUserDialogService>().Prompt(cfg);
			}
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		public void Refresh()
		{
			OnDisplayChanged();
			OnLayoutChanged();
		}

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		public override void OnAppearing()
		{
			base.OnAppearing();

			App.GPS.PositionChanged += OnPositionChanged;
			App.GPS.HeadingChanged += OnHeadingChanged;

			Refresh();
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		public override void OnDisappearing()
		{
			base.OnDisappearing();

			App.GPS.HeadingChanged -= OnHeadingChanged;
			App.GPS.PositionChanged -= OnPositionChanged;
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Executes the view changed command when botton pressed.
		/// </summary>
		/// <param name="screen">Type of screen to display.</param>
		private void ExecuteScreenChanged(ScreenType screen)
		{
			// Notify user
			App.Click();

			ActiveScreen = screen;

			Refresh();
		}

		/// <summary>
		/// Executes the map command.
		/// </summary>
		private void ExecuteMapCommand()
		{
			// Notify user
			App.Click();

			if (MapViewModel.Map.VisibleRegion == null)
			{
				if (App.GPS.LastKnownPosition != null)
				{
					MapViewModel.Map.VisibleRegion = MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(App.GPS.LastKnownPosition.Latitude, App.GPS.LastKnownPosition.Longitude), Xamarin.Forms.Maps.Distance.FromMeters(1000));
				}
				else
				{
					MapViewModel.Map.VisibleRegion = MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(0, 0), Xamarin.Forms.Maps.Distance.FromMeters(1000));
				}
			}

			ActiveScreen = ScreenType.Map;

			Refresh();
		}

		/// <summary>
		/// Handles the layout changed, which is called, when screen changes.
		/// </summary>
		private void OnLayoutChanged()
		{
			NotifyPropertyChanged(TitelPropertyName);
			NotifyPropertyChanged(IsListVisiblePropertyName);
			NotifyPropertyChanged(IsOverviewVisiblePropertyName);

			if (this.activeScreen != ScreenType.Main && this.activeScreen != ScreenType.Map)
			{
				NotifyPropertyChanged(EmptyListTextPropertyName);
				NotifyPropertyChanged(IsEmptyListTextVisiblePropertyName);
			}

			if (this.activeScreen == ScreenType.Main || this.activeScreen == ScreenType.Map)
			{
				NotifyPropertyChanged(IsEmptyListTextVisiblePropertyName);
			}
		}

		/// <summary>
		/// Handles the display changed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Display changed event arguments.</param>
		private void OnDisplayChanged(object sender = null, DisplayChangedEventArgs e = null)
		{
			if (e == null)
			{
				e = new DisplayChangedEventArgs();
			}

			// TODO: Remove
//			Console.WriteLine("{0}: {1}", e.What, e.PropertyName);

			// If GameState change, update the main screen
			if (e.What == "GameState" && gameModel.GameState == WF.Player.Core.Engines.EngineGameState.Playing)
			{
				Refresh();

				return;
			}

			if (IsMapSelected)
			{
				RefreshMap(e);

				return;
			}

			// Check, if there is something we should update 
			if (e.What == "Property")
			{
				if (e.PropertyName == "Points")
				{
					var entry = GameMainList.FirstOrDefault(obj => obj.UIObject == e.UIObject);

					if (entry != null)
					{
						entry.VectorToObject = null;

						RefreshDirections(false);
					}
				}

				var ret = false;

				ret = ret || (e.PropertyName == "ActiveVisibleZones" && (IsYouSeeSelected || IsOverviewSelected));
				ret = ret || (e.PropertyName == "VisibleObjects" && (IsYouSeeSelected || IsOverviewSelected));
				ret = ret || (e.PropertyName == "VisibleInventory" && (IsInventorySelected || IsOverviewSelected));
				ret = ret || (e.PropertyName == "ActiveVisibleTasks" && (IsTasksSelected || IsOverviewSelected));
				ret = ret || (e.PropertyName == "Name" && IsOverviewSelected);

				if (!ret)
				{
					return;
				}
			}

			if (this.gameModel == null || this.gameModel.GameState != WF.Player.Core.Engines.EngineGameState.Playing)
			{
				return;
			}

			RefreshButtons();

			if (activeScreen != ScreenType.Main && activeScreen != ScreenType.Map)
			{
				RefreshListContent();
			}

			if (activeScreen == ScreenType.Main)
			{
				RefreshMainScreen();
			}

			if (!IsOverviewVisible)
			{
				RefreshDirections(true);
			}
		}

		/// <summary>
		/// Handles the position changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position changed event arguments.</param>
		private void OnPositionChanged(object sender, PositionEventArgs e)
		{
			Position = e.Position;
			MapViewModel.Position = e.Position;

			RefreshDirections(true);
		}

		/// <summary>
		/// Handles the position changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position changed event arguments.</param>
		private void OnHeadingChanged(object sender, PositionEventArgs e)
		{
			Position = e.Position;
			MapViewModel.Position = e.Position;

			RefreshDirections(false);
		}

		/// <summary>
		///  Update directions for all visible list entries
		/// </summary>
		/// <param name="updateDirection">Flag, if direction should be calculated new.</param>
		private void RefreshDirections(bool updateDirection)
		{
			if (Position == null)
			{
				// Set all directions to undefined, because we don't have a signal
				if (IsYouSeeSelected)
				{
					foreach (var entry in GameMainList)
					{
						entry.Direction = double.NegativeInfinity;
						entry.Distance = double.NegativeInfinity;
					}
				}

				return;
			}

			// Recalc all directions and distances when position changes, but only, if You See screen is active
			if (IsYouSeeSelected)
			{
				double heading = 0;

				if (Position != null && Position.Heading != null)
				{
					// Show always to north
					heading = 360.0 - (double)Position.Heading;
				}

				foreach (var entry in GameMainList)
				{
					// Do it only for entries with ObjectLocation
					if (entry.UIObject.ObjectLocation != null)
					{
						if (updateDirection || entry.VectorToObject == null)
						{
							// Calculate values for this thing
							entry.VectorToObject = this.geoMathHelper.VectorToPoint(new ZonePoint(Position.Latitude, Position.Longitude, 0), entry.UIObject.ObjectLocation);
						}

						// Set values
						entry.Direction = (double)((entry.VectorToObject.Bearing + heading) % 360);
						entry.Distance = entry.VectorToObject.Distance.Value;
					}
				}
			}
		}

		/// <summary>
		/// Refreshs the content of the list.
		/// </summary>
		private void RefreshListContent()
		{
			List<GameMainCellViewModel> listItems = null;
			bool hasListIcons = false;
			bool hasDirections = false;

			switch (this.activeScreen)
			{
				case ScreenType.Locations:
				case ScreenType.Items:
					var zones = this.gameModel.ActiveVisibleZones;

					listItems = new List<GameMainCellViewModel>();

					foreach (Zone z in zones)
					{
						listItems.Add(new GameMainCellViewModel(z.Name, App.Colors.Text, z));
						hasListIcons = hasListIcons || (z.Icon != null && z.Icon.Data != null);
					}

					var objects = this.gameModel.VisibleObjects;

					foreach (Thing t in objects)
					{
						listItems.Add(new GameMainCellViewModel(t.Name, App.Colors.Text, t));
						hasListIcons = hasListIcons || (t.Icon != null && t.Icon.Data != null);
					}

					hasDirections = true;
					break;
				case ScreenType.Inventory:
					var inventory = this.gameModel.VisibleInventory;

					listItems = new List<GameMainCellViewModel>();

					foreach (Thing t in inventory)
					{
						listItems.Add(new GameMainCellViewModel(t.Name, App.Colors.Text, t));
						hasListIcons = hasListIcons || (t.Icon != null && t.Icon.Data != null);
					}

					break;
				case ScreenType.Tasks:
					var tasks = this.gameModel.ActiveVisibleTasks;

					listItems = new List<GameMainCellViewModel>();

					foreach (Task t in tasks)
					{
						listItems.Add(new GameMainCellViewModel(t.Name, App.Colors.Text, t));
						hasListIcons = hasListIcons || (t.Icon != null && t.Icon.Data != null);
					}

					break;
			}

			this.hasListIcons = hasListIcons;

			if (activeScreen != ScreenType.Main && activeScreen != ScreenType.Map)
			{
				// Set icon visible flag
				foreach (var o in listItems)
				{
					o.ShowIcon = this.hasListIcons;
					o.ShowDirection = hasDirections && (o.UIObject.ObjectLocation != null || o.UIObject is Zone);
				}

				this.gameMainList = listItems;
				NotifyPropertyChanged(GameMainListPropertyName);
			}
		}

		/// <summary>
		/// Refreshs the buttons.
		/// </summary>
		private void RefreshButtons()
		{
			YouSeeNumber = this.gameModel.ActiveVisibleZones.Count + this.gameModel.VisibleObjects.Count;
			InventoryNumber = this.gameModel.VisibleInventory.Count;
			TasksNumber = this.gameModel.ActiveVisibleTasks.Count;
		}

		/// <summary>
		/// Refreshs content of main screen.
		/// </summary>
		private void RefreshMainScreen()
		{
			// Create content for You See on main screen
			string content = string.Format("## {0}", Catalog.GetString("You See"));

			if (YouSeeNumber > 0)
			{
				content = content + string.Format(" [{0}]", YouSeeNumber);
			}

			content = content + Environment.NewLine;

			var listYouSee = this.gameModel.ActiveVisibleZones.Select(o => o.Name).ToList();

			listYouSee.AddRange(this.gameModel.VisibleObjects.Select(o => o.Name).ToList());

			if (listYouSee.Count > 0)
			{
				content = content + string.Join(", ", listYouSee);
			}
			else
			{
				content = content + this.gameModel.Cartridge.EmptyYouSeeListText;
			}

			YouSeeOverviewContent = content;

			// Create content for Inventory on main screen
			content = string.Format("## {0}", Catalog.GetString("Inventory"));

			if (InventoryNumber > 0)
			{
				content = content + string.Format(" [{0}]", InventoryNumber);
			}

			content = content + Environment.NewLine;

			var listInventory = this.gameModel.VisibleInventory.Select(o => o.Name).ToList();

			if (listInventory.Count > 0)
			{
				content = content + string.Join(", ", listInventory);
			}
			else
			{
				content = content + this.gameModel.Cartridge.EmptyInventoryListText;
			}

			InventoryOverviewContent = content;

			// Create content for Tasks on main screen
			content = string.Format("## {0}", Catalog.GetString("Tasks"));

			if (TasksNumber > 0)
			{
				content = content + string.Format(" [{0}]", TasksNumber);
			}

			content = content + Environment.NewLine;

			var listTasks = this.gameModel.ActiveVisibleTasks.Select(o => o.Name).ToList();

			if (listTasks.Count > 0)
			{
				content = content + string.Join(", ", listTasks);
			}
			else
			{
				content = content + this.gameModel.Cartridge.EmptyTasksListText;
			}

			TasksOverviewContent = content;
		}

		/// <summary>
		/// Refreshs the map.
		/// </summary>
		/// <param name="e">Event arguments.</param>
		private void RefreshMap(DisplayChangedEventArgs e = null)
		{
			if (e == null)
			{
				// Refresh the whole map
			}

			// Task are not interessting
			if (e.UIObject is Task)
			{
				return;
			}

			if (e.What == null || (e.What == "Property" && e.PropertyName == "Points"))
			{
				var polygons = new List<WF.Player.Controls.ExtendedMap.MapPolygon>();

				foreach (var z in App.Game.ActiveVisibleZones)
				{
					polygons.Add(new WF.Player.Controls.ExtendedMap.MapPolygon(z.Points, new WF.Player.Controls.ExtendedMap.MapPoint(z.ObjectLocation, z.Name)));
				}

				MapViewModel.Map.Polygons = polygons;
			}

			// We are only interessted in the following changes
			// Visible
			// Active
			// Name
			// Points
			// ObjectLocation
			if (e.What != "Visible" && e.What != "Active" && e.What != "Name" && e.What != "Points" && e.What != "ObjectLocation")
			{
				return;
			}
		}

		#endregion

		#region OverviewEntry Class

		/// <summary>
		/// Class for a overview entry.
		/// </summary>
		private class OverviewEntry
		{
			/// <summary>
			/// Gets or sets the header.
			/// </summary>
			/// <value>The header.</value>
			public string Header { get; set; }

			/// <summary>
			/// Gets or sets the content.
			/// </summary>
			/// <value>The content.</value>
			public string Content { get; set; }
		}

		#endregion
	}
}