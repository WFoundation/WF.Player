///
/// WF.Player.Core - A Wherigo Player Core for different platforms.
/// Copyright (C) 2012-2014  Dirk Weltz <web@weltz-online.de>
/// Copyright (C) 2012-2014  Brice Clocher <contact@cybisoft.net>
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
using System.IO;
using System.Linq;
using System.Reflection;
using WF.Player.Interfaces;
using Windows.ApplicationModel;
using Xamarin.Forms;

[assembly: Dependency(typeof(WF.Player.WP8.Services.Core.WP8PlatformHelper))]

namespace WF.Player.WP8.Services.Core
{
	/// <summary>
	/// A standard Windows Phone implementation of IPlatformHelper.
	/// </summary>
	public class WP8PlatformHelper : IFormsPlatformHelper
    {
		#region Constructors

		static WP8PlatformHelper()
		{
            _entryAssemblyVersion = Package.Current.Id.Version;
		} 

		#endregion

		#region Fields
		
		private static PackageVersion _entryAssemblyVersion; 

		#endregion

		#region Properties

		public virtual string CartridgeFolder
		{
			get { return "Cartridges"; }
		}

		public virtual string SavegameFolder
		{
			get { return "Savegames"; }
		}

		public virtual string LogFolder
		{
			get { return "Logs"; }
		}

        public virtual string Ok
        {
            get { return "Ok"; }
        }

        public string EmptyYouSeeListText
        {
            get { return "Empty"; }
        }

        public string EmptyInventoryListText
        {
            get { return "Empty"; }
        }

        public string EmptyTasksListText
        {
            get { return "Empty"; }
        }

        public string EmptyZonesListText
        {
            get { return "Empty"; }
        }

        public string EmptyTargetListText
        {
            get { return "Empty"; }
        }

		public string PathSeparator
		{
			get { return "\\"; }
		}

		public string Platform
		{
			get { return "WinPhone 8.1"; }
		}

		public string Device
		{
			get
			{
                return String.Format(
                    "Windows Phone {0}/{1}",
                    "8.1", //Environment.OSVersion.Version.ToString(2),
                    "unknown"); // Microsoft.Phone.Info.DeviceStatus.DeviceManufacturer);
			}
		}

		public string DeviceId
		{
			get
			{
                //// Retrieves the phone's device Id (requires permission ID_CAP_IDENTITY_DEVICE).
                //object idHash;
                //if (!Microsoft.Phone.Info.DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out idHash))
                //{
                //    return DefaultPlatformHelper.UnknownValue;
                //}
                //return Convert.ToBase64String((byte[])idHash);

                // DeviceId is not returned for privacy reasons.
                return "?";
			}
		}

		public virtual string ClientVersion
		{
			get
			{
				return _entryAssemblyVersion.ToString() ?? "unknown";
			}

			// The value is set by the static constructor in order to catch the UI thread's 
			// calling assembly's version.
		}

		public bool CanDispatchOnUIThread
		{
			get { return true; }
		}

        public string PathForFiles
        {
            get
            {
                return "";
            }
        }

        public string PathForDatabase
        {
            get
            {
                return "Data";
            }
        }

        public SQLite.Net.SQLiteConnection Database
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Stream StreamForDemoCartridge
        {
            get
            {
                return typeof(WP8PlatformHelper).GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Wherigo Tutorial.gwc");
            }
        }

        #endregion

        #region Methods

        public void BeginDispatchOnUIThread(Action action)
		{
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => action());
		} 

		#endregion
    }
}
