// <copyright file="ISettings.cs" company="Wherigo Foundation">
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

namespace WF.Player.Services.Settings
{
	using System;

	/// <summary>
	/// Main interface for settings.
	/// </summary>
	public interface ISettings
	{
		/// <summary>
		/// Gets the current value or the default that you specify.
		/// </summary>
		/// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
		/// <param name="key">Key for settings</param>
		/// <param name="defaultValue">default value if not set</param>
		/// <returns>Value or default</returns>
		T GetValueOrDefault<T>(string key, T defaultValue = default(T));

		/// <summary>
		/// Adds or updates the value
		/// </summary>
		/// <param name="key">Key for settting</param>
		/// <param name="value">Value to set</param>
		/// <returns>True of was added or updated and you need to save it.</returns>
		bool AddOrUpdateValue<T>(string key, T value);

		/// <summary>
		/// Removes a desired key from the settings
		/// </summary>
		/// <param name="key">Key for setting</param>
		void Remove(string key);

		/// <summary>
		/// Settingses are changed inside or outside of the player, so update things.
		/// </summary>
		void Changed();
	}
}

