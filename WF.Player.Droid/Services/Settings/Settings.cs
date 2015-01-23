// <copyright file="Settings.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2015  James Montemagno
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.SettingsAndroid))]

namespace WF.Player.Droid.Services
{
	using System;
	using Android.Content;
	using Android.Preferences;
	using Android.App;
	using WF.Player.Services.Settings;

	/// <summary>
	/// Main Implementation for ISettings
	/// </summary>
	public class SettingsAndroid : ISettings
	{
		private static ISharedPreferences SharedPreferences { get; set; }

		private static ISharedPreferencesEditor SharedPreferencesEditor { get; set; }

		private readonly object locker = new object();

		/// <summary>
		/// Main Constructor
		/// </summary>
		public SettingsAndroid()
		{
			SharedPreferences = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
			SharedPreferencesEditor = SharedPreferences.Edit();
		}

		/// <summary>
		/// Gets the current value or the default that you specify.
		/// </summary>
		/// <typeparam name="T">Vaue of t (bool, int, float, long, string)</typeparam>
		/// <param name="key">Key for settings</param>
		/// <param name="defaultValue">default value if not set</param>
		/// <returns>Value or default</returns>
		public T GetValueOrDefault<T>(string key, T defaultValue = default(T))
		{
			lock (locker)
			{
				Type typeOf = typeof(T);

				if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					typeOf = Nullable.GetUnderlyingType(typeOf);
				}

				object value = null;

				var typeCode = Type.GetTypeCode(typeOf);

				switch (typeCode)
				{
					case TypeCode.Decimal:
						//Android doesn't have decimal in shared prefs so get string and convert
						var savedDecimal = SharedPreferences.GetString(key, string.Empty);
						if (string.IsNullOrWhiteSpace(savedDecimal))
						{
							value = Convert.ToDecimal(defaultValue, System.Globalization.CultureInfo.InvariantCulture);
						}
						else
						{
							value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
						}
						break;
					case TypeCode.Boolean:
						value = SharedPreferences.GetBoolean(key, Convert.ToBoolean(defaultValue));
						break;
					case TypeCode.Int64:
						value = (Int64)SharedPreferences.GetLong(key, (long)Convert.ToInt64(defaultValue, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.String:
						value = SharedPreferences.GetString(key, Convert.ToString(defaultValue));
						break;
					case TypeCode.Double:
						//Android doesn't have double, so must get as string and parse.
						var savedDouble = SharedPreferences.GetString(key, string.Empty);
						if (string.IsNullOrWhiteSpace(savedDouble))
						{
							value = Convert.ToDouble(defaultValue, System.Globalization.CultureInfo.InvariantCulture);
						}
						else
						{
							value = Convert.ToDouble(savedDouble, System.Globalization.CultureInfo.InvariantCulture);
						}
						break;
					case TypeCode.Int32:
						value = SharedPreferences.GetInt(key, Convert.ToInt32(defaultValue, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.Single:
						value = SharedPreferences.GetFloat(key, Convert.ToSingle(defaultValue, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.DateTime:
						var ticks = SharedPreferences.GetLong(key, -1);
						if (ticks == -1)
						{
							value = defaultValue;
						}
						else
						{
							value = new DateTime(ticks);
						}
						break;
					default:
						if (defaultValue is Guid)
						{
							var outGuid = Guid.Empty;
							Guid.TryParse(SharedPreferences.GetString(key, Guid.Empty.ToString()), out outGuid);
							value = outGuid;
						}
						else
						{
							throw new ArgumentException(string.Format("Value of type {0} is not supported.", value.GetType().Name));
						}
						break;
				}
				return null != value ? (T)value : defaultValue;
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
			Type typeOf = typeof(T);

			if (typeOf.IsGenericType && typeOf.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				typeOf = Nullable.GetUnderlyingType(typeOf);
			}

			var typeCode = Type.GetTypeCode(typeOf);

			return AddOrUpdateValue(key, value, typeCode);
		}

		/// <summary>
		/// Adds or updates a value
		/// </summary>
		/// <param name="key">key to update</param>
		/// <param name="value">value to set</param>
		/// <returns>True if added or update and you need to save</returns>
		/// <exception cref="NullReferenceException">If value is null, this will be thrown.</exception>
		private bool AddOrUpdateValue(string key, object value, TypeCode typeCode)
		{
			lock(locker)
			{
				switch (typeCode)
				{
					case TypeCode.Decimal:
						SharedPreferencesEditor.PutString(key, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.Boolean:
						SharedPreferencesEditor.PutBoolean(key, Convert.ToBoolean(value));
						break;
					case TypeCode.Int64:
						SharedPreferencesEditor.PutLong(key, (long)Convert.ToInt64(value, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.String:
						SharedPreferencesEditor.PutString(key, Convert.ToString(value));
						break;
					case TypeCode.Double:
						SharedPreferencesEditor.PutString(key, Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.Int32:
						SharedPreferencesEditor.PutInt(key, Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.Single:
						SharedPreferencesEditor.PutFloat(key, Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture));
						break;
					case TypeCode.DateTime:
						SharedPreferencesEditor.PutLong(key, (Convert.ToDateTime(value)).Ticks);
						break;
					default:
						if(value is Guid)
						{
							if(value == null)
								value = Guid.Empty;
							SharedPreferencesEditor.PutString(key, ((Guid)value).ToString());
						}
						else
						{
							throw new ArgumentException(string.Format("Value of type {0} is not supported.", value.GetType().Name));
						}
						break;
				}
				SharedPreferencesEditor.Commit();
			}
			return true;
		}

		/// <summary>
		/// Removes a desired key from the settings
		/// </summary>
		/// <param name="key">Key for setting</param>
		public void Remove(string key)
		{
			lock (locker)
			{
				SharedPreferencesEditor.Remove(key);
				SharedPreferencesEditor.Commit();
			}
		}

		/// <summary>
		/// Settingses are changed inside or outside of the player, so update things.
		/// </summary>
		public void Changed()
		{
			((App)Xamarin.Forms.Application.Current).CreateStyles();
		}
	}
}
