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

using System;
using System.IO;
using System.Reflection;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Xamarin.Forms;
using WF.Player.Core.Engines;
using Vernacular;
using WF.Player.Interfaces;
using WF.Player.Services.Settings;

[assembly: Dependency(typeof(WF.Player.iOS.Services.Core.iOSPlatformHelper))]

namespace WF.Player.iOS.Services.Core
{
	public class iOSPlatformHelper : IFormsPlatformHelper
	{
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

		public iOSPlatformHelper()
		{
			// Get rootPath for cartridge files, save files and log files
			double version;
			double.TryParse(UIDevice.CurrentDevice.SystemVersion, out version);

			if (version >= 8)
			{
				rootPath = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User) [0].RelativePath;
				databasePath = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.LibraryDirectory, NSSearchPathDomain.User) [0].RelativePath;
			}
			else
			{
				rootPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				databasePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + ".." + System.IO.Path.DirectorySeparatorChar + "Library";
			}

			Settings.Current.AddOrUpdateValue<string>(Settings.CartridgePathKey, rootPath);

			try
			{
				entryAssemblyVersion = Version.Parse(Assembly.GetExecutingAssembly().GetName().Version.ToString());
			}
			catch (Exception)
			{
				entryAssemblyVersion = null;
			}
		}

		#endregion

		#region Members

		static Version entryAssemblyVersion; 

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
					var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();

					database = new SQLite.Net.SQLiteConnection(platform, Path.Combine(databasePath, "WF.Player.db3"));
				}

				return database;
			}
		}

		public Stream StreamForDemoCartridge
		{
			get
			{
				return System.IO.File.OpenRead(Path.Combine("Assets", "Wherigo Tutorial.gwc"));
			}
		}

		#endregion

		#region ICorePlatformHelper Properties

		public string CartridgeFolder
		{
			get { return ""; }
		}

		public string SavegameFolder
		{
			get { return "SaveGame"; }
		}

		public string LogFolder
		{
			get { return "Log"; }
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
			get { return Path.DirectorySeparatorChar.ToString(); }
		}

		public string Platform
		{
			get { return Environment.OSVersion.Platform.ToString(); }
		}

		public string Device
		{
			get { return String.Format(
					"iPhone {0}",
					Environment.OSVersion.Version.ToString()); }
		}

		public string DeviceId
		{
			get 
			{ 
				// Use MAC Adress of en0 as DeviceId
				foreach (var i in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces ())
					if (i.Id.Equals ("en0")) 
						return i.GetPhysicalAddress ().ToString ();
				return "unknown";
			}
		}

		public string ClientVersion
		{
			get { return entryAssemblyVersion != null ? String.Format("{0}.{1}", NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleShortVersionString").ToString(), NSBundle.MainBundle.ObjectForInfoDictionary("CFBundleVersion").ToString()) : "Unknown"; }
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
