///
/// WF.Player.Core - A Wherigo Player Core for different platforms.
/// Copyright (C) 2012-2013  Dirk Weltz <web@weltz-online.de>
/// Copyright (C) 2012-2013  Brice Clocher <contact@cybisoft.net>
///
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU Lesser General Public License as
/// published by the Free Software Foundation, either version 3 of the
/// License, or (at your option) any later version.
/// 
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
/// GNU Lesser General Public License for more details.
/// 
/// You should have received a copy of the GNU Lesser General Public License
/// along with this program.  If not, see <http://www.gnu.org/licenses/>.
///

using System;
using System.Linq;
using System.Reflection;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using WF.Player.Core.Engines;
using Vernacular;
using WF.Player.Interfaces;
using System.IO;

[assembly: Dependency(typeof(WF.Player.Droid.Services.Core.AndroidPlatformHelper))]

namespace WF.Player.Droid.Services.Core
{
	/// <summary>
	/// A standard Android implementation of IPlatformHelper.
	/// </summary>
	public class AndroidPlatformHelper : IFormsPlatformHelper
	{
		static PackageInfo pInfo;

		/// <summary>
		/// The root path for cartridge files, save files and log files.
		/// </summary>
		private string rootPath;

		/// <summary>
		/// The database path for the cartridge database.
		/// </summary>
		private string databasePath;

		/// <summary>
		/// The SQLite connection to database.
		/// </summary>
		private SQLite.Net.SQLiteConnection database;

		#region Constructors

		public AndroidPlatformHelper()
		{
			pInfo = Xamarin.Forms.Forms.Context.PackageManager.GetPackageInfo(Xamarin.Forms.Forms.Context.PackageName, PackageInfoFlags.Activities);

			string cartridgePath = App.Prefs.Get<string>(DefaultPreferences.CartridgePathKey);

			// Did we find a path in the preferences?
			if (!string.IsNullOrEmpty(cartridgePath))
			{
				rootPath = cartridgePath;

				return;
			}

			// Get path to default external storage
			var extPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;

			// Get all directories in default external storage
			var extDir = new PCLStorage.FileSystemFolder(extPath);
			var extDirsTask = extDir.GetFoldersAsync(System.Threading.CancellationToken.None);

			extDirsTask.RunSynchronously();

			var extDirs = extDirsTask.Result;

			// Look for the default WF.Player directory
			foreach (var entry in extDirs)
			{
				if (entry.Name.Equals("WF.Player"))
				{
					rootPath = entry.Path;
				}
			}

			// There was no root folder up to now, so create one
			if (string.IsNullOrEmpty(rootPath))
			{
				// TODO: Ask user for folder to use
//				rootPath = new Acr.XamForms.Mobile.IO.Directory(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath).CreateSubdirectory("WF.Player").FullName;
			}

			// Now we have the root path and could create the database path
			databasePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder);
		}

		#endregion

		#region Members

		private static Version EntryAssemblyVersion; 

		#endregion

		#region Properties

		#region ICommonPlatformHelper Properties

		/// <summary>
		/// Gets the path for cartridge files, save files and log files.
		/// </summary>
		/// <value>The path for cartridge files.</value>
		public string PathForFiles
		{
			get
			{
				return rootPath;
			}
		}

		/// <summary>
		/// Gets the path for database files.
		/// </summary>
		/// <value>The path for save files.</value>
		public string PathForDatabase
		{
			get
			{
				return databasePath;
			}
		}

		/// <summary>
		/// Gets the path for database.
		/// </summary>
		/// <value>The path for database.</value>
		public SQLite.Net.SQLiteConnection Database
		{
			get
			{
				if (databasePath == null)
				{
					throw new DirectoryNotFoundException("database");
				}

				if (database == null)
				{
					// Create a new database connection
					var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();

					database = new SQLite.Net.SQLiteConnection(platform, Path.Combine(databasePath, "WF.Player.db3"));
				}

				return database;
			}
		}

		public Stream StreamForDemoCartridge
		{
			get
			{
				return Forms.Context.Assets.Open("Wherigo Tutorial.gwc");
			}
		}

		#endregion

		#region ICorePlatformHelper Properties

		public virtual string CartridgeFolder
		{
			get { return "/"; }
		}

		public virtual string SavegameFolder
		{
			get { return "/Savegames"; }
		}

		public virtual string LogFolder
		{
			get { return "/Log"; }
		}

		public string Ok
		{
			get { return Catalog.GetString("Ok"); }
		}

		public string EmptyYouSeeListText
		{
			get { return Catalog.GetString("Nothing of interest"); }
		}

		public string EmptyInventoryListText 
		{
			get { return Catalog.GetString("No items"); }
		}

		public string EmptyTasksListText 
		{
			get { return Catalog.GetString("No new tasks"); }
		}

		public string EmptyZonesListText
		{
			get { return Catalog.GetString("Nowhere to go"); }
		}

		public string EmptyTargetListText 
		{
			get { return Catalog.GetString("Nothing available"); }
		}

		public string PathSeparator
		{
			get { return System.IO.Path.DirectorySeparatorChar.ToString(); }
		}

		public string Platform
		{
				get { return global::System.Environment.OSVersion.Platform.ToString(); }
		}

		public string Device
		{
			get
			{
				return String.Format(
					"Android {0}/{1}",
					Build.VERSION.Release,
					Build.Model);
			}
		}

		public string DeviceId
		{
			get
			{
				// TODO: Insert right DeviceId
				return "unknown";
			}
		}

		public virtual string ClientVersion
		{
			get
			{
				return string.Format("WF.Player.Android {0}.{1}", pInfo.VersionName, pInfo.VersionCode);
			}

			// The value is set by the static constructor in order to catch the UI thread's 
			// calling assembly's version.
		}

		public bool CanDispatchOnUIThread
		{
			get { return true; }
		}

		#endregion

		#endregion

		#region Methods

		public void BeginDispatchOnUIThread(Action action)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() => action());
		} 

		#endregion
	}
}
