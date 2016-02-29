// <copyright file="Settings.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2015  James Montemagno
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.WinPhone8.Services.SettingsWP8))]

namespace WF.Player.WinPhone8.Services
{
    using System;
    using WF.Player.Services.Settings;
    using Windows.Storage;
    /// <summary>
    /// Main implementation for ISettings
    /// </summary>
    public class SettingsWP8 : ISettings
	{
		private readonly object locker = new object();

        // Our isolated storage settings
        ApplicationDataContainer localSettings;


        /// <summary>
        /// Gets the current value or the default that you specify.
        /// </summary>
        /// <remarks>
        /// Found at http://blogs.msdn.com/b/glengordon/archive/2012/09/17/managing-settings-in-windows-phone-and-windows-8-store-apps.aspx
        /// </remarks>
        /// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
        /// <param name="key">Key for settings</param>
        /// <param name="defaultValue">default value if not set</param>
        /// <returns>Value or default</returns>
        public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
		{
			lock (locker)
			{
				localSettings = localSettings ?? ApplicationData.Current.LocalSettings;

                T value;

                // If the key exists, retrieve the value.
                if (localSettings.Values.ContainsKey(key))
                {
                    value = (T)localSettings.Values[key];
                }
                // Otherwise, use the default value.
                else
                {
                    value = defaultValue;
                }

                return value;
            }
		}

		/// <summary>
		/// Adds or updates a value
		/// </summary>
		/// <param name="key">key to update</param>
		/// <param name="value">value to set</param>
		/// <returns>True if added or update and you need to save</returns>
		public bool AddOrUpdateValue<T>(string key, T value)
		{
            bool valueChanged = false;

            localSettings = localSettings ?? ApplicationData.Current.LocalSettings;

            // If the key exists
            if (localSettings.Values.ContainsKey(key))
            {
                // If the value has changed
                if (localSettings.Values[key] != (object)value)
                {
                    // Store the new value
                    localSettings.Values[key] = value;
                    valueChanged = true;
                }
            }
            // Otherwise create the key.
            else
            {
                localSettings.Values.Add(key, value);
                valueChanged = true;
            }

            return valueChanged;
        }

		/// <summary>
		/// Removes a desired key from the settings
		/// </summary>
		/// <param name="key">Key for setting</param>
		public void Remove(string key)
		{
			lock (locker)
			{
                localSettings = localSettings ?? ApplicationData.Current.LocalSettings;

                if (localSettings.Values.ContainsKey(key))
                {
                    localSettings.Values.Remove(key);
                }
			}
		}

		/// <summary>
		/// Settings are changed inside or outside of the player, so update things.
		/// </summary>
		public void Changed()
		{
			if (Xamarin.Forms.Application.Current != null)
			{
				((App)Xamarin.Forms.Application.Current).CreateStyles();
			}
		}
	}
}

