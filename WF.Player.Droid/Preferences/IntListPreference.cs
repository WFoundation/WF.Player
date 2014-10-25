// <copyright file="IntListPreference.cs" company="Wherigo Foundation">
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

// ListPreference for Int values.
// Based on an articel of Kevin Vance (http://kvance.livejournal.com/1039349.html)

namespace WF.Player.Droid.Preferences
{
	using System;
	using Android.Content;
	using Android.Preferences;
	using Android.Util;

	/// <summary>
	/// Int list preference.
	/// </summary>
	public class IntListPreference : ListPreference
	{
		/// <summary>
		/// The shared preferences.
		/// </summary>
		private static ISharedPreferences sharedPreferences;

		/// <summary>
		/// The shared preferences editor.
		/// </summary>
		private static ISharedPreferencesEditor sharedPreferencesEditor;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Preferences.IntListPreference"/> class.
		/// </summary>
		/// <param name="context">Context to use.</param>
		/// <param name="attrs">Set of attributes.</param>
		public IntListPreference(Context context, IAttributeSet attrs) : base(context, attrs) 
		{
			sharedPreferences = PreferenceManager.GetDefaultSharedPreferences(context);
			sharedPreferencesEditor = sharedPreferences.Edit();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Preferences.IntListPreference"/> class.
		/// </summary>
		/// <param name="context">Context to use.</param>
		public IntListPreference(Context context) : base(context)
		{
		}

		#endregion

		/// <summary>
		/// Persists the string.
		/// </summary>
		/// <returns><c>true</c>, if string was persisted, <c>false</c> otherwise.</returns>
		/// <param name="value">Value to set.</param>
		protected override bool PersistString(string value)
		{
			if (value == null)
			{
				return false;
			}
			else
			{
				return sharedPreferencesEditor.PutInt(Key, Convert.ToInt32(value)).Commit();
			}
		}

		/// <summary>
		/// Gets the persisted string.
		/// </summary>
		/// <returns>The persisted string.</returns>
		/// <param name="defaultReturnValue">Default return value.</param>
		protected override string GetPersistedString(string defaultReturnValue)
		{
			if (sharedPreferences.Contains(this.Key))
			{
				// _sharedPreferences.Edit().Remove(Key).Commit();
				int intValue = sharedPreferences.GetInt(Key, Convert.ToInt32(defaultReturnValue));
				return intValue.ToString();
			}
			else
			{
				return defaultReturnValue;
			}
		}
	} 
}
