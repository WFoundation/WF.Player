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
	using System.Collections.Generic;
	using System.IO;
	using System.Threading;
	using WF.Player.Core;
	using WF.Player.Core.Engines;
	using WF.Player.Core.Formats;
	using WF.Player.Models;
	using WF.Player.Services.Device;
	using WF.Player.Services.Geolocation;
	using WF.Player.Services.Preferences;
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

		/// <summary>
		/// The timer lock object.
		/// </summary>
		private object timerLock = new object();

		/// <summary>
		/// The screen.
		/// </summary>
		private Queue<Screens> screenQueue;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameModel"/> class.
		/// </summary>
		/// <param name="tag">CartridgeTag handled by this instance.</param>
		public GameModel(CartridgeTag tag)
		{
			this.cartridgeTag = tag;
			this.screenQueue = new Queue<Screens>(8);
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when display changed.
		/// </summary>
		public event EventHandler<DisplayChangedEventArgs> DisplayChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the cartridge.
		/// </summary>
		/// <value>The cartridge.</value>
		public Cartridge Cartridge
		{
			get
			{
				return this.engine != null ? this.engine.Cartridge : null;
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
				return this.engine != null ? this.engine.GameState : EngineGameState.Uninitialized;
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
		/// <returns>The task.</returns>
		/// <param name="savegame">Savegame object.</param> 
		public async System.Threading.Tasks.Task StartAsync(CartridgeSavegame savegame = null)
		{
			App.GameNavigation.CurrentPage.IsBusy = true;

			App.GameNavigation.Popped += (sender, e) => this.HandlePagePopped();
			App.GameNavigation.PoppedToRoot += (sender, e) => this.HandlePagePopped();
			App.GameNavigation.Pushed += (sender, e) => this.HandlePagePushed();

			// Create Engine
			await this.CreateEngine(this.cartridgeTag.Cartridge);

			// If there is a valid savefile, than open it
			if (savegame != null && File.Exists(Path.Combine(App.PathForSavegames, savegame.Filename)))
			{
				this.engine.Restore(new FileStream(savegame.Filename, FileMode.Open));
			}

			// Init for position
			var pos = App.GPS.LastKnownPosition ?? new Position(0, 0);
			this.engine.RefreshLocation(pos.Latitude, pos.Longitude, pos.Altitude ?? 0, pos.Accuracy ?? double.NaN);

			App.GPS.PositionChanged += this.OnPositionChanged;

			await System.Threading.Tasks.Task.Run(() => this.engine.Start());

			App.GameNavigation.CurrentPage.IsBusy = false;
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
		/// Save the specified name and autosaving.
		/// </summary>
		/// <param name="name">Name of savefile.</param>
		/// <param name="autosaving">If set to <c>true</c> autosaving.</param>
		public CartridgeSavegame Save(string name = "Ingame saving", bool autosaving = false)
		{
			App.GameNavigation.CurrentPage.IsBusy = true;

			// Create a new savegame name for this cartridge tag
			var cs = new CartridgeSavegame(this.cartridgeTag);

			// Save game
			this.engine.Save(new FileStream(cs.Filename, FileMode.Create), name);

			// Add savegame, which is now in store, to cartridge tag
			if (!autosaving)
			{
				this.cartridgeTag.AddSavegame(cs);
			}

			App.GameNavigation.CurrentPage.IsBusy = false;

			return cs;
		}

		/// <summary>
		/// Stop this instance.
		/// </summary>
		public void Stop()
		{
			this.DestroyEngine();
		}

		#endregion

		#region Autosave

		/// <summary>
		/// Autosave the current game.
		/// </summary>
		public void AutoSave()
		{
			var cs = this.Save("Autosave", true);

			App.Prefs.Set<string>(DefaultPreferences.AutosaveGWSKey, cs.Filename);
			App.Prefs.Set<string>(DefaultPreferences.AutosaveGWCKey, this.cartridgeTag.Cartridge.Filename);
		}

		/// <summary>
		/// Delete autosave information and file.
		/// </summary>
		public void AutoRemove()
		{
			if (string.IsNullOrEmpty(App.Prefs.Get<string>(DefaultPreferences.AutosaveGWSKey)))
			{
				return;
			}

			var filename = App.Prefs.Get<string>(DefaultPreferences.AutosaveGWSKey);

			// Delete files
			var file = new Acr.XamForms.Mobile.IO.File(filename);

			if (file.Exists)
			{
				file.Delete();
			}

			// Delete entries in preferences
			App.Prefs.Set<string>(DefaultPreferences.AutosaveGWSKey, string.Empty);
			App.Prefs.Set<string>(DefaultPreferences.AutosaveGWCKey, string.Empty);
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
				App.GameNavigation.CurrentPage.Navigation.PopModalAsync();
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
			this.screenQueue.Enqueue(new Screens(screenType, obj));

			this.HandleScreenQueue();
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Handles event, that page is popped.
		/// </summary>
		private void HandlePagePopped()
		{
			if (App.GameNavigation.CurrentPage is BasePage)
			{
				((BasePage)App.GameNavigation.CurrentPage).OnAppeared();
			}

			this.HandleScreenQueue();
		}

		/// <summary>
		/// Handles event, that page is pushed.
		/// </summary>
		private void HandlePagePushed()
		{
			if (App.GameNavigation.CurrentPage is BasePage)
			{
				((BasePage)App.GameNavigation.CurrentPage).OnAppeared();
			}

			this.HandleScreenQueue();
		}

		/// <summary>
		/// Handles the screen queue.
		/// </summary>
		private void HandleScreenQueue()
		{
			// If we are no longer playing
			if (this.engine == null || (this.engine.GameState != EngineGameState.Playing && this.engine.GameState != EngineGameState.Starting))
			{
				return;
			}

			// Changes the screen in this moment
			if (App.GameNavigation.Transition)
			{
				return;
			}

			// Is active screen an input
			if (App.GameNavigation.CurrentPage is GameInputView)
			{
				// Cancel input
				// ((GameInputViewModel)((GameInputView)App.GameNavigation.CurrentPage).BindingContext).Input.GiveResult(null);
			}

			// Is there a delayed MessageBox on the screen
			if (this.timer != null)
			{
				lock (this.timerLock)
				{
					this.timer.Dispose();
					this.timer = null;
				}

				if (this.screenQueue.Count == 0 || !(this.screenQueue.Peek().ScreenType == ScreenType.Dialog && this.screenQueue.Peek().Object is MessageBoxEventArgs))
				{
					// Only delete MessageBox, if the next isn't a MessageBox
					if (!(App.GameNavigation.CurrentPage is GameMainView))
					{
						App.GameNavigation.PopAsync();
					}

					// Return and wait for popped event
					return;
				}
			}

			// Check if a detail screen is visible, that shouldn't
			if (App.GameNavigation.CurrentPage is GameDetailView)
			{
				var activeObject = ((GameDetailViewModel)App.GameNavigation.CurrentPage.BindingContext).ActiveObject;

				// Check, if detail screen is shown with a invalid or invisible object
				if (activeObject != null && !activeObject.Visible)
				{
					if (!(App.GameNavigation.CurrentPage is GameMainView))
					{
						App.GameNavigation.PopAsync();
					}

					return;
				}

				// Check if detail screen is shown with an object without container
				if (!(activeObject is Task) && !(activeObject is Zone) && ((Thing)activeObject).Container == null)
				{
					if (!(App.GameNavigation.CurrentPage is GameMainView))
					{
						App.GameNavigation.PopAsync();
					}

					return;
				}
			}

			if (this.screenQueue.Count == 0)
			{
				// Nothing to do
				return;
			}

			// Get next screen to display
			var screen = this.screenQueue.Peek();

			switch (screen.ScreenType)
			{
				case ScreenType.Last:
					if (App.GameNavigation.CurrentPage is GameMessageboxView)
					{
						// We have a MessageBox on screen, so wait some time, if perhaps another MessageBox arrives
						lock (this.timerLock)
						{
							this.screenQueue.Dequeue();

							this.timer = new System.Threading.Timer((sender) => this.HandleScreenQueue(), null, 150, Timeout.Infinite);
						}
					}
					else
					{
						screenQueue.Dequeue();

						if (!(App.GameNavigation.CurrentPage is GameMainView))
						{
							App.GameNavigation.PopAsync();
						}

						return;
					}

					break;
				case ScreenType.Main:
				case ScreenType.Locations:
				case ScreenType.Items:
				case ScreenType.Inventory:
				case ScreenType.Tasks:
					// Check, if main view is visible
					if (!(App.GameNavigation.CurrentPage is GameMainView))
					{
						App.GameNavigation.PopToRootAsync();

						// Return and wait for popped event
						return;
					}

					if (screen.ScreenType == ScreenType.Main)
					{
						((GameMainViewModel)App.GameNavigation.CurrentPage.BindingContext).ActiveScreen = ScreenType.Main;
					}

					if (screen.ScreenType == ScreenType.Locations || screen.ScreenType == ScreenType.Items)
					{
						((GameMainViewModel)App.GameNavigation.CurrentPage.BindingContext).ActiveScreen = ScreenType.Locations;
					}

					if (screen.ScreenType == ScreenType.Inventory)
					{
						((GameMainViewModel)App.GameNavigation.CurrentPage.BindingContext).ActiveScreen = ScreenType.Inventory;
					}

					if (screen.ScreenType == ScreenType.Tasks)
					{
						((GameMainViewModel)App.GameNavigation.CurrentPage.BindingContext).ActiveScreen = ScreenType.Tasks;
					}

					this.screenQueue.Dequeue();

					break;
				case ScreenType.Details:
					if (screen.Object is UIObject)
					{
						// If active screen is detail screen than replace only active object
						if (App.GameNavigation.CurrentPage is GameDetailView)
						{
							this.screenQueue.Dequeue();

							((GameDetailViewModel)((GameDetailView)App.GameNavigation.CurrentPage).BindingContext).ActiveObject = (UIObject)screen.Object;

							return;
						}

						// Remove page (could only be a MessageBox or an Input)
						if (!(App.GameNavigation.CurrentPage is GameMainView) && !(App.GameNavigation.CurrentPage is GameDetailView))
						{
							App.GameNavigation.PopAsync();

							// Return and wait for popped event
							return;
						}

						// If active screen is detail screen than replace only active object
						if (App.GameNavigation.CurrentPage is GameDetailView)
						{
							this.screenQueue.Dequeue();

							((GameDetailViewModel)((GameDetailView)App.GameNavigation.CurrentPage).BindingContext).ActiveObject = (UIObject)screen.Object;
						}
						else
						{
							// Create new detail screen and put it onto the screen
							var gameDetailView = new GameDetailView(new GameDetailViewModel() 
								{
									ActiveObject = (UIObject)screen.Object,
								});

							this.screenQueue.Dequeue();

							App.GameNavigation.PushAsync(gameDetailView);
						}
					}

					break;
				case ScreenType.Dialog:
					if (screen.Object is MessageBoxEventArgs)
					{
						// If active screen is already a messagebox than replace only content
						if (App.GameNavigation.CurrentPage is GameMessageboxView)
						{
							this.screenQueue.Dequeue();

							((GameMessageboxViewModel)((GameMessageboxView)App.GameNavigation.CurrentPage).BindingContext).MessageBox = ((MessageBoxEventArgs)screen.Object).Descriptor;
						}
						else
						{
							// Remove all screens until we find a detail screen or the main screen
							if (!(App.GameNavigation.CurrentPage is GameMainView) && !(App.GameNavigation.CurrentPage is GameDetailView))
							{
								if (!(App.GameNavigation.CurrentPage is GameMainView))
								{
									App.GameNavigation.PopAsync();
								}

								// Return and wait for popped event
								return;
							}

							// Create new messagebox and put it onto the screen
							var gameMessageboxView = new GameMessageboxView(new GameMessageboxViewModel() 
								{
									MessageBox = ((MessageBoxEventArgs)screen.Object).Descriptor,
								});

							// Remove entry from screen queue
							this.screenQueue.Dequeue();

							// Bring messagebox to screen
							App.GameNavigation.PushAsync(gameMessageboxView);
						}
					}

					if (screen.Object is Input)
					{
						// If active screen is already an input than replace only content
						if (App.GameNavigation.CurrentPage is GameInputView)
						{
							this.screenQueue.Dequeue();

							((GameInputViewModel)((GameInputView)App.GameNavigation.CurrentPage).BindingContext).Input = (Input)screen.Object;
						}
						else
						{
							// Remove all screens until we find a detail screen or the main screen
							if (!(App.GameNavigation.CurrentPage is GameMainView) && !(App.GameNavigation.CurrentPage is GameDetailView))
							{
								if (!(App.GameNavigation.CurrentPage is GameMainView))
								{
									App.GameNavigation.PopAsync();
								}

								// Return and wait for popped event
								return;
							}

							// Create new input and put it onto the screen
							var gameInputView = new GameInputView(new GameInputViewModel() 
								{
									Input = (Input)screen.Object,
								});

							// Remove entry from screen queue
							this.screenQueue.Dequeue();

							// Bring input to screen
							App.GameNavigation.PushAsync(gameInputView);
						}
					}

					break;
			}
		}

		/// <summary>
		/// Logs the message.
		/// </summary>
		/// <param name="level">Level of log message.</param>
		/// <param name="message">Message to log.</param>
		private void LogMessage(LogLevel level, string message)
		{
			if (this.logFile == null)
			{
				this.logFile = this.cartridgeTag.CreateLogFile();
				this.logFile.MinimalLogLevel = this.logLevel;
			}

			if (level <= this.logLevel)
			{
				this.logFile.TryWriteLogEntry(this.logLevel, message, this.engine);
			}

			// TODO: Remove
			Console.WriteLine(message);
		}

		/// <summary>
		/// Creates the engine.
		/// </summary>
		/// <returns>The task.</returns>
		/// <param name="cartridge">Cartridge handled by this instance.</param>
		private async System.Threading.Tasks.Task CreateEngine(Cartridge cartridge)
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
			this.engine.PropertyChanged += this.OnPropertyChanged;

			// Open logFile first time
			this.logFile = this.cartridgeTag.CreateLogFile();
			this.logFile.MinimalLogLevel = this.logLevel;

			await System.Threading.Tasks.Task.Run(() => this.engine.Init(new FileStream(Path.Combine(App.PathCartridges, Path.GetFileName(cartridge.Filename)), FileMode.Open), cartridge));
		}

		/// <summary>
		/// Destroies the engine.
		/// </summary>
		private void DestroyEngine()
		{
			if (this.engine != null)
			{
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
				this.engine.PropertyChanged -= this.OnPropertyChanged;

				this.engine.Stop();
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
		private void OnPositionChanged(object sender, PositionEventArgs e)
		{
			if (this.engine != null && e != null && e.Position != null)
			{
				System.Threading.Tasks.Task.Run(() => this.engine.RefreshLocation(e.Position.Latitude, e.Position.Longitude, (double)e.Position.Altitude, (double)e.Position.Accuracy));
			}
		}

		/// <summary>
		/// Handles the property changed event from the engine.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Property changed event arguments.</param>
		private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			// If GameState changes to Playing, than update screen
			if (e.PropertyName == "GameState" && this.GameState == EngineGameState.Playing)
			{
				this.RaiseDisplayChanged("GameState");
			}

			if (e.PropertyName.Equals("IsBusy") && App.GameNavigation != null && App.GameNavigation.CurrentPage != null)
			{
				App.GameNavigation.CurrentPage.IsBusy = this.engine.IsBusy;
			}
		}

		#endregion

		#region Screens class

		/// <summary>
		/// Class for screens in screen queue.
		/// </summary>
		private class Screens
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="Screens"/> class.
			/// </summary>
			/// <param name="screenType">Screen type.</param>
			/// <param name="obj">Object for detail screen.</param>
			public Screens(ScreenType screenType, object obj)
			{
				this.ScreenType = screenType;
				this.Object = obj;
			}

			/// <summary>
			/// Gets the type of the screen.
			/// </summary>
			/// <value>The type of the screen.</value>
			public ScreenType ScreenType { get; private set; }

			/// <summary>
			/// Gets the object.
			/// </summary>
			/// <value>The object.</value>
			public object Object { get; private set; }
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
