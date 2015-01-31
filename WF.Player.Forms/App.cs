// <copyright file="App.cs" company="Wherigo Foundation">
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
using WF.Player.Services;
using WF.Player.Services.UserDialogs;
using Vernacular;
using WF.Player.Core;
using System.Diagnostics;

namespace WF.Player
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using WF.Player.Controls;
	using WF.Player.Interfaces;
	using WF.Player.Models;
	using WF.Player.Services.Settings;
	using WF.Player.Services.Device;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;

	/// <summary>
	/// Forms app.
	/// </summary>
	public class App : Application
	{
		/// <summary>
		/// The game model.
		/// </summary>
		private static GameModel gameModel;

		/// <summary>
		/// Active GPS object.
		/// </summary>
		private static IGeolocator gps;

		/// <summary>
		/// Settings for app.
		/// </summary>
		private static ISettings settings;

		/// <summary>
		/// Fonts for app.
		/// </summary>
		private static Fonts fonts;

		/// <summary>
		/// Colors for app.
		/// </summary>
		private static Colors colors;

		/// <summary>
		/// Chars for app.
		/// </summary>
		private static Chars chars;

		/// <summary>
		/// Sound interface for app.
		/// </summary>
		private static ISound sound;

		/// <summary>
		/// Vibration interface for app.
		/// </summary>
		private static IVibration vibrate;

		/// <summary>
		/// The platform helper.
		/// </summary>
		private IFormsPlatformHelper platformHelper;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.App"/> class.
		/// </summary>
		public App(IFormsPlatformHelper platformHelper) : base()
		{
			this.platformHelper = platformHelper;

			PathCartridges = platformHelper.PathForFiles;
			PathDatabase = platformHelper.PathForDatabase;

			MainPage = GetMainPage();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The documents path.
		/// </summary>
		public static string PathCartridges;

		/// <summary>
		/// The library path.
		/// </summary>
		public static string PathDatabase;

		/// <summary>
		/// Navigation page for outside of game navigation.
		/// </summary>
		public static ExtendedNavigationPage Navigation;

		/// <summary>
		/// Navigation page for inside of game navigation.
		/// </summary>
		public static ExtendedNavigationPage GameNavigation;

		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		/// <value>The game.</value>
		public static GameModel Game
		{
			get
			{
				return gameModel;
			}

			set
			{
				if (gameModel != value)
				{
					gameModel = value;
				}
			}
		}

		/// <summary>
		/// Gets the GP.
		/// </summary>
		/// <value>The GP.</value>
		public static IGeolocator GPS
		{
			get
			{
				if (gps == null)
				{
					gps = DependencyService.Get<IGeolocator>();
					gps.DesiredAccuracy = double.PositiveInfinity;
					gps.StartListening(500, 2.0, true);
				}

				return gps;
			}
		}

		/// <summary>
		/// Gets the fonts.
		/// </summary>
		/// <value>The fonts.</value>
		public static Fonts Fonts
		{
			get
			{
				if (fonts == null)
				{
					fonts = new Fonts();
				}

				return fonts;
			}
		}

		/// <summary>
		/// Gets the colors.
		/// </summary>
		/// <value>The colors.</value>
		public static Colors Colors
		{
			get
			{
				if (colors == null)
				{
					colors = new Colors();
				}

				return colors;
			}
		}

		/// <summary>
		/// Gets the chars.
		/// </summary>
		/// <value>The chars.</value>
		public static Chars Chars
		{
			get
			{
				if (chars == null)
				{
					chars = new Chars();
				}

				return chars;
			}
		}

		/// <summary>
		/// Gets the cartridge path.
		/// </summary>
		/// <remarks>If there is no entry in the prefs up to now, the tutorial 
		/// will be copied to the new selected place.</remarks>
		/// <value>The cartridge path.</value>
		public static string PathForCartridges
		{
			get
			{
				string cartridgePath = Settings.Current.GetValueOrDefault<string>(Settings.CartridgePathKey);

				// Did we find a path in the preferences?
				if (!string.IsNullOrEmpty(cartridgePath) && Directory.Exists(cartridgePath) != null)
				{
					return cartridgePath;
				}

				// We now start the first time, so get correct path and copy tutorial

				#if __ANDROID__
				// Get path to default external storage and get all directories in this default external storage
				var extDirs = Directory.GetDirectories(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);

				// Look for the default WF.Player directory
				foreach (var entry in extDirs)
				{
					if (entry.EndsWith("WF.Player", StringComparison.InvariantCultureIgnoreCase))
					{
						cartridgePath = entry;
					}
				}

				// If we don't find an entry, than there is perhaps a WhereYouGo directory
				if (string.IsNullOrEmpty(cartridgePath))
				{
					foreach (var entry in extDirs)
					{
						if (entry.EndsWith("WhereYouGo", StringComparison.InvariantCultureIgnoreCase))
						{
							cartridgePath = entry;
						}
					}
				}

				// There was no cartridge folder up to now, so create one
				if (string.IsNullOrEmpty(cartridgePath) && !Directory.Exists(cartridgePath))
				{
					cartridgePath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, "WF.Player");
					Directory.CreateDirectory(cartridgePath);
				}
				#endif
				#if __IOS__
				// iOS have a default folder for the app
				cartridgePath = PathCartridges;
				#endif

				// Save path
				Settings.Current.AddOrUpdateValue<string>(Settings.CartridgePathKey, cartridgePath);

				return cartridgePath;
			}
		}

		/// <summary>
		/// Gets the log path.
		/// </summary>
		/// <value>The log path.</value>
		public static string PathForLogs
		{
			get
			{
				var logsFolder = Path.Combine(PathCartridges, "Logs");

				if (!Directory.Exists(logsFolder))
				{
					Directory.CreateDirectory(logsFolder);
				}

				return logsFolder;
			}
		}

		/// <summary>
		/// Gets the path for savegames.
		/// </summary>
		/// <value>The path for savegames.</value>
		public static string PathForSavegames
		{
			get
			{
				var cartridgePath = PathForCartridges;
//				var cartridgeDir = new Acr.XamForms.Mobile.IO.Directory(cartridgePath);
//				Acr.XamForms.Mobile.IO.IDirectory savegameDir = null;
//
//				foreach (var dir in new Acr.XamForms.Mobile.IO.Directory(cartridgePath).Directories)
//				{
//					if (dir.Name.ToLower().Equals("savegames"))
//					{
//						savegameDir = dir;
//					}
//				}
//
//				if (savegameDir == null)
//				{
//					savegameDir = cartridgeDir.CreateSubdirectory("Savegames");
//				}
//
//				return savegameDir.FullName;
				return cartridgePath;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the main page.
		/// </summary>
		/// <returns>The main page.</returns>
		public static Page GetMainPage()
		{
			CheckFolder();

			var cartridges = new CartridgeStore();
			cartridges.SyncFromStore();

			// Create content page for cartridge list
			App.Navigation = new ExtendedNavigationPage(new CartridgeListPage(cartridges), true) 
				{
					BackgroundColor = App.Colors.Background,
					BarTextColor = App.Colors.BarText,
					BarBackgroundColor = App.Colors.Bar,
					ShowBackButton = true,
				};

			// Check for autosave file
			var gwsFilename = Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWSKey);
			var gwcFilename = Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWCKey);

			if (!string.IsNullOrEmpty(gwsFilename) && !string.IsNullOrEmpty(gwcFilename))
			{
				HandleAutosave();
			}
			else
			{
				// If an auto save file exists, delete it
				var filename = Path.Combine(App.PathForSavegames, "autosave.gws");

				if (File.Exists(filename))
				{
					File.Delete(filename);
				}
			}

			return App.Navigation;
		}

		/// <summary>
		/// Creates the styles for various views from the settings.
		/// </summary>
		public void CreateStyles()
		{
			// Get resource dictionary
			var resources = Application.Current.Resources ?? new ResourceDictionary ();

			// Create style for normal texts
			var normalStyle = new Style (typeof(ExtendedLabel)) 
				{
					Setters = {
						new Setter { Property = ExtendedLabel.TextColorProperty, Value = App.Colors.Text },
						new Setter { Property = ExtendedLabel.FontSizeProperty, Value = Settings.FontSize },
						new Setter { Property = ExtendedLabel.FontFamilyProperty, Value = Settings.FontFamily },
					}
				};

			if (resources.ContainsKey("NormalStyle"))
			{
				resources.Remove("NormalStyle");
			}

			resources.Add ("NormalStyle", normalStyle);

			Application.Current.Resources = resources;
		}

		public static void CheckFolder()
		{
			// If there isn't any gwc file in the path,
			// than copy Wherigo Tutorial to cartridges folder
			var files = Directory.GetFiles(PathForCartridges);

			if (files == null || !files.Any((file) => Path.GetExtension(file).EndsWith("gwc", StringComparison.InvariantCultureIgnoreCase)))
			{
				#if __ANDROID__
				using (var input = Forms.Context.Assets.Open("Wherigo Tutorial.gwc"))
				#endif
				#if __IOS__
				using (var input = File.OpenRead(Path.Combine("Assets", "Wherigo Tutorial.gwc")))
				#endif
				{
					var output = new FileStream(Path.Combine(PathCartridges, "Wherigo Tutorial.gwc"),FileMode.Create);

					input.CopyTo(output);

					output.Flush();
					output.Close();

					if (App.Navigation != null && App.Navigation.CurrentPage is CartridgeListPage)
					{
						((CartridgeListPage)App.Navigation.CurrentPage).RefreshCommand.Execute(null);
					}
				}
			}
		}

		/// <summary>
		/// Play click sounds and vibrate if that is selected in the preferences.
		/// </summary>
		public static void Click()
		{
			if (sound == null)
			{
				sound = DependencyService.Get<ISound>();
			}
				
			if (sound != null && Settings.Current.GetValueOrDefault<bool>(Settings.FeedbackSoundKey))
			{
				sound.PlayKeyboardSound();
			}

			if (vibrate == null)
			{
				vibrate = DependencyService.Get<IVibration>();
			}

			if (vibrate != null && Settings.Current.GetValueOrDefault<bool>(Settings.FeedbackVibrationKey))
			{
				vibrate.Vibrate(150);
			}
		}

		#endregion

		#region Events

		protected override void OnSleep()
		{
			Console.WriteLine("OnSleep");

			base.OnSleep();

			// Is there a game running?
			if (App.Game != null && App.Game.GameState == WF.Player.Core.Engines.EngineGameState.Playing)
			{
				// Create an autosave file
				App.Game.AutoSave();

				// And pause the game
				App.Game.Pause();
			}
		}

		protected override void OnResume()
		{
			Console.WriteLine("OnResume");

			base.OnResume();

			if (App.Game != null)
			{
				// Delete autosave information
				App.Game.AutoRemove();

				// And resume game
				App.Game.Resume();
			}
		}

		#endregion

		#region Private Methods

		private static async void HandleAutosave()
		{
			var gwcFilename = Path.Combine(App.PathForCartridges, Path.GetFileName(Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWCKey)));
			var gwsFilename = Path.Combine(App.PathForSavegames, Path.GetFileName(Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWSKey)));

			if (!File.Exists(gwsFilename) || !File.Exists(gwcFilename))
			{
				// Remove settings
				Settings.Current.Remove(Settings.AutosaveGWCKey);
				Settings.Current.Remove(Settings.AutosaveGWSKey);

				return;
			}

			bool result = await DependencyService.Get<IUserDialogService>().ConfirmAsync(Catalog.GetString("There is an automatical savefile from a cartridge you played before. Would you resume this last game?"), Catalog.GetString("Automatical savefile"), Catalog.GetString("Yes"), Catalog.GetString("No"));

			if (result)
			{
				var cartridge = new Cartridge(gwcFilename);
				var cartridgeTag = new CartridgeTag(cartridge);
				var cartridgeSavegame = CartridgeSavegame.FromStore(cartridgeTag, gwsFilename);

				// We have a autosave file, so start this cartridge

				// Create a new navigation page for the game
				App.GameNavigation = new ExtendedNavigationPage(new GameCheckLocationView(new GameCheckLocationViewModel(cartridgeTag, cartridgeSavegame, App.Navigation.CurrentPage)), false) {
					BarBackgroundColor = App.Colors.Bar,
					BarTextColor = App.Colors.BarText,
					ShowBackButton = true,
				};
				App.Navigation.CurrentPage.Navigation.PushModalAsync(App.GameNavigation);

				App.GameNavigation.ShowBackButton = true;
			}
			else
			{
				// Remove file from directory
				File.Delete(gwsFilename);
			}

			// Remove settings
			Settings.Current.Remove(Settings.AutosaveGWCKey);
			Settings.Current.Remove(Settings.AutosaveGWSKey);
		}

		#endregion
	}
}
