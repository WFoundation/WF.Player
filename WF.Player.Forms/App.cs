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
using WF.Player.Core;
using WF.Player.Core.Formats;
using Acr.XamForms.Mobile;
using PCLStorage;
using WF.Player.Controls;

namespace WF.Player
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Vernacular;
	using WF.Player.Core.Live;
	using WF.Player.Models;
	using WF.Player.Services.Device;
	using WF.Player.Services.Geolocation;
	using WF.Player.Services.Preferences;
	using Xamarin.Forms;

	/// <summary>
	/// Forms app.
	/// </summary>
	public class App
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
		/// Preferences for app.
		/// </summary>
		private static IPreferences prefs;

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
		/// Gets the prefs.
		/// </summary>
		/// <value>The prefs.</value>
		public static IPreferences Prefs
		{
			get
			{
				if (prefs == null)
				{
					prefs = DependencyService.Get<IPreferences>();
				}

				return prefs;
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
		/// Gets the path for database.
		/// </summary>
		/// <value>The path for database.</value>
		public static SQLite.SQLiteConnection Database
		{
			get
			{
				var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Library");
				return new SQLite.SQLiteConnection(Path.Combine(path, "WF.Player.db3"));
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
				string cartridgePath = App.Prefs.Get<string>(DefaultPreferences.CartridgePathKey);

				// Did we find a path in the preferences?
				if (!string.IsNullOrEmpty(cartridgePath))
				{
					return cartridgePath;
				}

				// We now start the first time, so get correct path and copy tutorial

				#if __ANDROID__
				// Get path to default external storage
				var extPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

				// Get all directories in default external storage
				var extDir = new Acr.XamForms.Mobile.IO.Directory(extPath);

				// Look for the default WF.Player directory
				foreach (var entry in extDir.Directories)
				{
					if (entry.Name.Equals("WF.Player"))
					{
						cartridgePath = entry.FullName;
					}
				}

				// If we don't find an entry, than there is perhaps a WhereYouGo directory
				if (string.IsNullOrEmpty(cartridgePath))
				{
					foreach (var entry in extDir.Directories)
					{
						if (entry.Name.Equals("WhereYouGo"))
						{
							cartridgePath = entry.FullName;
						}
					}
				}

				// There was no cartridge folder up to now, so create one
				if (string.IsNullOrEmpty(cartridgePath))
				{
					cartridgePath = new Acr.XamForms.Mobile.IO.Directory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath).CreateSubdirectory("WF.Player").FullName;
				}
				#endif
				#if __IOS__
				// iOS have a default folder for the app
				cartridgePath = PathCartridges;
				#endif

				// Save path
				App.Prefs.Set<string>(DefaultPreferences.CartridgePathKey, cartridgePath);

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
				var folderCartridges = new FileSystemFolder(PathCartridges);

				folderCartridges.GetFoldersAsync(new System.Threading.CancellationToken()).ContinueWith((task) =>
					{
						if (!task.Result.Any((file) => file.Name.Equals("Logs")))
						{
							folderCartridges.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists, new System.Threading.CancellationToken()).Wait();
						}
					});

				return Path.Combine(PathCartridges, "Logs");
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
				var cartridgeDir = new Acr.XamForms.Mobile.IO.Directory(cartridgePath);
				Acr.XamForms.Mobile.IO.IDirectory savegameDir = null;

				foreach (var dir in new Acr.XamForms.Mobile.IO.Directory(cartridgePath).Directories)
				{
					if (dir.Name.ToLower().Equals("savegames"))
					{
						savegameDir = dir;
					}
				}

				if (savegameDir == null)
				{
					savegameDir = cartridgeDir.CreateSubdirectory("Savegames");
				}

				return savegameDir.FullName;
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

//			// Check for autosave file
//			var dir = new Acr.XamForms.Mobile.IO.Directory(App.PathForSavegames);
//			var gwsFilename = dir.Files.First((f) => f.Name.Equals("autosave.gws")).FullName;
//
//			if (gwsFilename != null)
//			{
//				var gwsMetadata = GWS.LoadMetadata(new FileStream(gwsFilename, FileMode.Open));
//				var gwcFilename = gwsMetadata.CartridgeName;
//
//				var cartridgeTag = new CartridgeTag(new Cartridge(filename.FullName));
//				var cartridgeSave = new CartridgeSavegame();
//
//				cartridgeSave.
//
//				// We have a autosave file, so start this cartridge
//				page.Navigation.PushAsync(new GameCheckLocationView(new GameCheckLocationViewModel(cartridgeTag, cartridgeSave, App.CurrentPage)));
//			}


			return App.Navigation;
		}

		public static async void CheckFolder()
		{

			// If there isn't any gwc file in the path,
			// than copy Wherigo Tutorial to cartridges folder
			var folder = new FileSystemFolder(PathCartridges);

			var files = await folder.GetFilesAsync(new System.Threading.CancellationToken());

			if (!files.Any((file) => Path.GetExtension(file.Name).EndsWith("gwc", StringComparison.InvariantCultureIgnoreCase)))
			{
				#if __ANDROID__
				using (var input = Forms.Context.Assets.Open("Wherigo Tutorial.gwc"))
				#endif
				#if __IOS__
				using (var input = System.IO.File.OpenRead(Path.Combine("Assets", "Wherigo Tutorial.gwc")))
				#endif
				{
					var file = await new FileSystemFolder(PathCartridges).CreateFileAsync("Wherigo Tutorial.gwc", CreationCollisionOption.ReplaceExisting, new System.Threading.CancellationToken());
					var output = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite, new System.Threading.CancellationToken());

					input.CopyTo(output);

					if (App.Navigation.CurrentPage is CartridgeListPage)
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
				
			if (sound != null && Prefs.Get<bool>(DefaultPreferences.FeedbackSoundKey))
			{
				sound.PlayKeyboardSound();
			}

			if (vibrate == null)
			{
				vibrate = DependencyService.Get<IVibration>();
			}

			if (vibrate != null && Prefs.Get<bool>(DefaultPreferences.FeedbackVibrationKey))
			{
				vibrate.Vibrate(150);
			}
		}

		#endregion

		#region Events

		public static void EnterBackground()
		{
			// Is there a game running?
			if (App.Game != null && App.Game.GameState == WF.Player.Core.Engines.EngineGameState.Playing)
			{
				// Create an autosave file
				App.Game.AutoSave();

				// And pause the game
				App.Game.Pause();
			}
		}

		public static void EnterForeground()
		{
			if (App.Game != null)
			{
				// Delete autosave information
				App.Game.AutoRemove();

				// And resume game
				App.Game.Resume();
			}
		}

		#endregion
	}
}
