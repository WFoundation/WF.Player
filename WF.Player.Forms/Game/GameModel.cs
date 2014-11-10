// <copyright file="GameModel.cs" company="Wherigo Foundation">
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
	using System.Collections.ObjectModel;
	using System.IO;
	using System.Text;
	using System.Threading;
	using Acr.XamForms.UserDialogs;
	using WF.Player.Core;
	using WF.Player.Core.Engines;
	using WF.Player.Core.Formats;
	using WF.Player.Models;
	using WF.Player.Services.Device;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;

	/// <summary>
	/// Game model.
	/// </summary>
	public class GameModel
	{
		/// <summary>
		/// The cartridge tag.
		/// </summary>
		private CartridgeTag cartridgeTag;

		/// <summary>
		/// The game main view.
		/// </summary>
		private GameMainView gameMainView;

		/// <summary>
		/// The engine.
		/// </summary>
		private Engine engine;

		/// <summary>
		/// The log file.
		/// </summary>
		private GWL logFile;

		/// <summary>
		/// The log level.
		/// </summary>
		private LogLevel logLevel = LogLevel.Error;

		/// <summary>
		/// The sound.
		/// </summary>
		private ISound sound;

		/// <summary>
		/// The vibration.
		/// </summary>
		private IVibration vibration;

		/// <summary>
		/// The timer.
		/// </summary>
		private System.Threading.Timer timer;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameModel"/> class.
		/// </summary>
		/// <param name="cartridge">Cartridge handled by this instance.</param>
		public GameModel(CartridgeTag tag)
		{
			cartridgeTag = tag;

			// Create Engine
			this.CreateEngine(tag.Cartridge);
		}

		#endregion

		/// <summary>
		/// Occurs when display changed.
		/// </summary>
		public event EventHandler<DisplayChangedEventArgs> DisplayChanged;

		#region Properties

		/// <summary>
		/// Gets the cartridge.
		/// </summary>
		/// <value>The cartridge.</value>
		public Cartridge Cartridge
		{
			get
			{
				return this.engine.Cartridge;
			}
		}

		/// <summary>
		/// Gets the state of the game.
		/// </summary>
		/// <value>The state of the game.</value>
		public EngineGameState GameState
		{
			get
			{
				return this.engine.GameState;
			}
		}

		/// <summary>
		/// Gets the active visible zones.
		/// </summary>
		/// <value>The active visible zones.</value>
		public WherigoCollection<Zone> ActiveVisibleZones
		{
			get
			{
				return this.engine.ActiveVisibleZones;
			}
		}

		/// <summary>
		/// Gets the visible objects.
		/// </summary>
		/// <value>The visible objects.</value>
		public WherigoCollection<Thing> VisibleObjects
		{
			get
			{
				return this.engine.VisibleObjects;
			}
		}

		/// <summary>
		/// Gets the visible inventory.
		/// </summary>
		/// <value>The visible inventory.</value>
		public WherigoCollection<Thing> VisibleInventory
		{
			get
			{
				return this.engine.VisibleInventory;
			}
		}

		/// <summary>
		/// Gets the active visible tasks.
		/// </summary>
		/// <value>The active visible tasks.</value>
		public WherigoCollection<Task> ActiveVisibleTasks
		{
			get
			{
				return this.engine.ActiveVisibleTasks;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Start this instance.
		/// </summary>
		public async void Start()
		{
			// Create game screens
			var gameMainViewModel = new GameMainViewModel(this);
			this.gameMainView = new GameMainView(gameMainViewModel);

			var navi = new NavigationPage(this.gameMainView);

			navi.BarBackgroundColor = App.Colors.Bar;
			navi.BarTextColor = App.Colors.BarText;

			await App.CurrentPage.Navigation.PushModalAsync(navi);

			var pos = App.GPS.LastKnownPosition ?? new Position(0, 0);
			this.engine.RefreshLocation(pos.Latitude, pos.Longitude, pos.Altitude ?? 0, pos.Accuracy ?? double.NaN);

			this.engine.Start();

			App.GPS.PositionChanged += this.HandlePositionChanged;

			gameMainViewModel.Update();

			this.gameMainView.IsBusy = false;
		}

		/// <summary>
		/// Restore the specified saveFilename.
		/// </summary>
		/// <param name="saveFilename">Save filename.</param>
		public void Restore(CartridgeSavegame savegame)
		{
			if (File.Exists(Path.Combine(App.PathForSavegames, savegame.Filename)))
			{
				this.engine.Restore(new FileStream(savegame.Filename, FileMode.Open));
			}

			this.Start();
		}

		/// <summary>
		/// Pause the engine.
		/// </summary>
		public void Pause()
		{
			if (this.engine != null)
			{
				this.engine.Pause();
			}
		}

		/// <summary>
		/// Start engine after a pause.
		/// </summary>
		public void Resume()
		{
			if (this.engine != null)
			{
				var pos = App.GPS.LastKnownPosition;
				this.engine.RefreshLocation(pos.Latitude, pos.Longitude, pos.Altitude ?? 0, pos.Accuracy ?? double.NaN);
				this.engine.Resume();
			}
		}

		/// <summary>
		/// Save this instance.
		/// </summary>
		public void Save(string name = "Ingame saving")
		{
			App.CurrentPage.IsBusy = true;

			// Create a new savegame name for this cartridge tag
			var cs = new CartridgeSavegame(cartridgeTag);

			// Save game
			this.engine.Save(new FileStream(cs.Filename, FileMode.Create), name);

			// Add savegame, which is now in store, to cartridge tag
			cartridgeTag.AddSavegame(cs);

			App.CurrentPage.IsBusy = false;
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void Stop()
		{
			this.DestroyEngine();
		}

		#endregion

		#region Events, called from engine

		/// <summary>
		/// Raises the cartridge complete event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Wherigo event arguments.</param>
		public void OnCartridgeComplete(object sender, WherigoEventArgs args)
		{
			// TODO: Implement
			// throw new NotImplementedException ();
		}

		/// <summary>
		/// Raises the attribute changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Attribute changed event arguments.</param>
		public void OnAttributeChanged(object sender, AttributeChangedEventArgs e)
		{
			if (e.Object is UIObject)
			{
				this.RaiseDisplayChanged("Property", (UIObject)e.Object, e.PropertyName);
			}
		}

		/// <summary>
		/// Raises the inventory changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Inventory changed event arguments.</param>
		public void OnInventoryChanged(object sender, InventoryChangedEventArgs e)
		{
		}

		/// <summary>
		/// Raises the zone state changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Zone state changed event args.</param>
		public void OnZoneStateChanged(object sender, ZoneStateChangedEventArgs e)
		{
		}

		/// <summary>
		/// Raises the get input event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="input">Object event arguments.</param>
		public void OnGetInput(object sender, ObjectEventArgs<Input> input)
		{
			this.ShowScreen(ScreenType.Dialog, input.Object);
		}

		/// <summary>
		/// Raises the log message event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Log message event arguments.</param>
		public void OnLogMessage(object sender, LogMessageEventArgs args)
		{
			this.LogMessage(args.Level, args.Message);
		}

		/// <summary>
		/// Raises the play alert event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Wherigo event arguments.</param>
		public void OnPlayAlert(object sender, WherigoEventArgs e)
		{
			if (this.sound != null)
			{
				this.sound.PlayAlert();
			}

			if (this.vibration != null)
			{
				this.vibration.Vibrate();
			}
		}

		/// <summary>
		/// Raises the play media event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="mediaObj">Media object.</param>
		public void OnPlayMedia(object sender, ObjectEventArgs<Media> mediaObj)
		{
			try
			{
				if (this.sound != null)
				{
					this.sound.PlaySound(mediaObj.Object);
				}
			}
			catch (InvalidCastException e)
			{
				this.LogMessage(LogLevel.Error, e.Message);
			}
		}

		/// <summary>
		/// Raises the save cartridge event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Saving event arguments.</param>
		public void OnSaveCartridge(object sender, SavingEventArgs args)
		{
			this.Save();

			if (args.CloseAfterSave)
			{
				// Close log file
				this.DestroyEngine();

				// Leave game
				App.CurrentPage.Navigation.PopModalAsync();
			}
		}

		/// <summary>
		/// Raises the show message box event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Message box event arguments.</param>
		public void OnShowMessageBox(object sender, MessageBoxEventArgs args)
		{
			this.ShowScreen(ScreenType.Dialog, args);
		}

		/// <summary>
		/// Raises the show screen event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Screen event arguments.</param>
		public void OnShowScreen(object sender, ScreenEventArgs args)
		{
			this.ShowScreen((ScreenType)args.Screen, args.Object);
		}

		/// <summary>
		/// Raises the show status text event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Status text event arguments.</param>
		public void OnShowStatusText(object sender, StatusTextEventArgs args)
		{
			DependencyService.Get<IUserDialogService>().Toast(args.Text);
		}

		/// <summary>
		/// Raises the stop sound event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="args">Wherigo event arguments.</param>
		public void OnStopSound(object sender, WherigoEventArgs args)
		{
			if (this.sound != null)
			{
				this.sound.StopSound();
			}
		}

		#endregion

		#region Public Functions

		/// <summary>
		/// Shows the screen.
		/// </summary>
		/// <param name="screenType">Screen type.</param>
		/// <param name="obj">Object to show.</param>
		public void ShowScreen(ScreenType screenType, object obj)
		{
			// If we are no longer playing
			if (this.engine == null || this.engine.GameState != EngineGameState.Playing)
			{
				return;
			}

			// Is active screen an input
			if (App.CurrentPage is GameInputView)
			{
				// Cancel input
//				((GameInputViewModel)((GameInputView)App.Navigation.CurrentPage).BindingContext).Input.GiveResult(null);
			}

			switch (screenType)
			{
				case ScreenType.Last:
					this.timer = new System.Threading.Timer(
						(sender) =>
						{
							Device.BeginInvokeOnMainThread(async () =>
								{
									if (this.timer != null)
									{
										this.timer.Dispose();
										this.timer = null;
									}

									await App.CurrentPage.Navigation.PopAsync();
								});
						}, 
						null, 
						150, 
						Timeout.Infinite);
					break;
				case ScreenType.Main:
				case ScreenType.Locations:
				case ScreenType.Items:
				case ScreenType.Inventory:
				case ScreenType.Tasks:
					if (this.timer != null)
					{
						this.timer.Dispose();
						this.timer = null;
					}

					App.CurrentPage.Navigation.PopToRootAsync();

					// If main screen selected, don't change the active list
					if (screenType == ScreenType.Main || screenType == ScreenType.Locations || screenType == ScreenType.Items)
					{
						((GameMainViewModel)this.gameMainView.BindingContext).ActiveScreen = ScreenType.Locations;
					}

					if (screenType == ScreenType.Inventory)
					{
						((GameMainViewModel)this.gameMainView.BindingContext).ActiveScreen = ScreenType.Inventory;
					}

					if (screenType == ScreenType.Tasks)
					{
						((GameMainViewModel)this.gameMainView.BindingContext).ActiveScreen = ScreenType.Tasks;
					}

					break;
				case ScreenType.Details:
					if (this.timer != null)
					{
						this.timer.Dispose();
						this.timer = null;
					}

					if (obj is UIObject)
					{
						// If active screen is detail screen than replace only active object
						if (App.CurrentPage is GameDetailView)
						{
							((GameDetailViewModel)((GameDetailView)App.CurrentPage).BindingContext).ActiveObject = (UIObject)obj;
							return;
						}

						// Remove page (could only be a MessageBox or an Input)
						if (!(App.CurrentPage is GameMainView) && !(App.CurrentPage is GameDetailView))
						{
							App.CurrentPage.Navigation.PopAsync();
						}

						// If active screen is detail screen than replace only active object
						if (App.CurrentPage is GameDetailView)
						{
							((GameDetailViewModel)((GameDetailView)App.CurrentPage).BindingContext).ActiveObject = (UIObject)obj;
							return;
						}
						else
						{
							// Create new detail screen and put it onto the screen
							var gameDetailView = new GameDetailView(new GameDetailViewModel() 
							{
								ActiveObject = (UIObject)obj,
							});
							App.CurrentPage.Navigation.PushAsync(gameDetailView);
						}
					}

					break;
				case ScreenType.Dialog:
					if (this.timer != null)
					{
						this.timer.Dispose();
						this.timer = null;
					}

					if (obj is MessageBoxEventArgs)
					{
						// If active screen is already a messagebox than replace only content
						if (App.CurrentPage is GameMessageboxView)
						{
							((GameMessageboxViewModel)((GameMessageboxView)App.CurrentPage).BindingContext).MessageBox = ((MessageBoxEventArgs)obj).Descriptor;
							return;
						}
						else
						{
							// Remove all screens until we find a detail screen or the main screen
							if (!(App.CurrentPage is GameMainView) && !(App.CurrentPage is GameDetailView))
							{
								App.CurrentPage.Navigation.PopAsync();
							}

							// Create new messagebox and put it onto the screen
							var gameMessageboxView = new GameMessageboxView(new GameMessageboxViewModel() 
							{
								MessageBox = ((MessageBoxEventArgs)obj).Descriptor,
							});
							App.CurrentPage.Navigation.PushAsync(gameMessageboxView);
						}
					}

					if (obj is Input)
					{
						// If active screen is already an input than replace only content
						if (App.CurrentPage is GameInputView)
						{
							((GameInputViewModel)((GameInputView)App.CurrentPage).BindingContext).Input = (Input)obj;
							return;
						}
						else
						{
							// Remove all screens until we find a detail screen or the main screen
							if (!(App.CurrentPage is GameMainView) && !(App.CurrentPage is GameDetailView))
							{
								App.CurrentPage.Navigation.PopAsync();
							}

							// Create new input and put it onto the screen
							var gameInputView = new GameInputView(new GameInputViewModel() 
							{
								Input = (Input)obj,
							});
							App.CurrentPage.Navigation.PushAsync(gameInputView);
						}
					}

					break;
			}
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="level">Level of log message.</param>
		/// <param name="message">Message to log.</param>
		private void LogMessage(LogLevel level, string message)
		{
			if (this.logFile == null)
			{
				this.logFile = cartridgeTag.CreateLogFile();
				this.logFile.MinimalLogLevel = logLevel;
			}

			if (level <= this.logLevel)
			{
				this.logFile.TryWriteLogEntry(logLevel, message, engine);
			}

			// TODO: Remove
			Console.WriteLine(message);
		}

		/// <summary>
		/// Creates the engine.
		/// </summary>
		/// <param name="cartridge">Cartridge handled by this instance.</param>
		private void CreateEngine(Cartridge cartridge)
		{
			if (this.engine != null)
			{
				this.DestroyEngine();
			}

			// Get device sound and vibration
			this.sound = DependencyService.Get<ISound>();
			this.vibration = DependencyService.Get<IVibration>();

			// Get os helper
			var helper = DependencyService.Get<IPlatformHelper>();

			this.engine = new Engine(helper);

			// Set all events for engine
			this.engine.AttributeChanged += this.OnAttributeChanged;
			this.engine.InventoryChanged += this.OnInventoryChanged;
			this.engine.ZoneStateChanged += this.OnZoneStateChanged;
			this.engine.CartridgeCompleted += this.OnCartridgeComplete;
			this.engine.InputRequested += this.OnGetInput;
			this.engine.LogMessageRequested += this.OnLogMessage;
			this.engine.PlayAlertRequested += this.OnPlayAlert;
			this.engine.PlayMediaRequested += this.OnPlayMedia;
			this.engine.SaveRequested += this.OnSaveCartridge;
			this.engine.ShowMessageBoxRequested += this.OnShowMessageBox;
			this.engine.ShowScreenRequested += this.OnShowScreen;
			this.engine.ShowStatusTextRequested += this.OnShowStatusText;
			this.engine.StopSoundsRequested += this.OnStopSound;

			// Open logFile first time
			this.logFile = cartridgeTag.CreateLogFile();
			this.logFile.MinimalLogLevel = logLevel;

			this.engine.Init(new FileStream(Path.Combine(App.PathForCartridges, Path.GetFileName(cartridge.Filename)), FileMode.Open), cartridge);
		}

		/// <summary>
		/// Destroies the engine.
		/// </summary>
		private void DestroyEngine()
		{
			if (this.engine != null)
			{
				this.engine.Stop();
				this.engine.Reset();

				this.engine.AttributeChanged -= this.OnAttributeChanged;
				this.engine.InventoryChanged -= this.OnInventoryChanged;
				this.engine.ZoneStateChanged -= this.OnZoneStateChanged;
				this.engine.CartridgeCompleted -= this.OnCartridgeComplete;
				this.engine.InputRequested -= this.OnGetInput;
				this.engine.LogMessageRequested -= this.OnLogMessage;
				this.engine.PlayAlertRequested -= this.OnPlayAlert;
				this.engine.PlayMediaRequested -= this.OnPlayMedia;
				this.engine.SaveRequested -= this.OnSaveCartridge;
				this.engine.ShowMessageBoxRequested -= this.OnShowMessageBox;
				this.engine.ShowScreenRequested -= this.OnShowScreen;
				this.engine.ShowStatusTextRequested -= this.OnShowStatusText;
				this.engine.StopSoundsRequested -= this.OnStopSound;

				this.engine.Dispose();

				this.engine = null;
			}
		}

		#endregion

		#region Internal Event Raiser

		/// <summary>
		/// Raises the display changed.
		/// </summary>
		/// <param name="what">What changed.</param>
		/// <param name="obj">Object that changed.</param>
		/// <param name="property">Property that changed.</param>
		private void RaiseDisplayChanged(string what, UIObject obj = null, string property = null)
		{
			if (this.engine == null || this.engine.GameState != EngineGameState.Playing)
			{
				return;
			}

			var handler = this.DisplayChanged;
			if (handler != null)
			{
				handler(this, new DisplayChangedEventArgs(what, obj, property));
			}
		}

		#endregion

		#region Handler

		/// <summary>
		/// Handles the position changed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position changed event arguments.</param>
		private void HandlePositionChanged(object sender, PositionEventArgs e)
		{
			if (this.engine != null && e != null && e.Position != null)
			{
				this.engine.RefreshLocation(e.Position.Latitude, e.Position.Longitude, (double)e.Position.Altitude, (double)e.Position.Accuracy);
			}
		}

		#endregion
	}

	/// <summary>
	/// Display changed event arguments.
	/// </summary>
	public class DisplayChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.DisplayChangedEventArgs"/> class.
		/// </summary>
		/// <param name="what">What changed.</param>
		/// <param name="obj">Object which changed.</param>
		/// <param name="property">Property which changed.</param>
		public DisplayChangedEventArgs(string what = null, UIObject obj = null, string property = null)
		{
			What = what;
			UIObject = obj;
			PropertyName = property;
		}

		/// <summary>
		/// Gets the what.
		/// </summary>
		/// <value>The what.</value>
		public string What { get; private set; }

		/// <summary>
		/// Gets the user interface object.
		/// </summary>
		/// <value>The user interface object.</value>
		public UIObject UIObject { get; private set; }

		/// <summary>
		/// Gets the name of the property.
		/// </summary>
		/// <value>The name of the property.</value>
		public string PropertyName { get; private set; }
	}
}
