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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.iOS.Services.SettingsIOS))]

namespace WF.Player.iOS.Services
{
	using System;
	using Foundation;
	using WF.Player.Services.Settings;

	/// <summary>
	/// Main implementation for ISettings
	/// </summary>
	public class SettingsIOS : ISettings
	{
		private readonly object locker = new object();

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
				var defaults = NSUserDefaults.StandardUserDefaults;

				if (defaults.ValueForKey(new NSString(key)) == null)
				{
					return defaultValue;
				}

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
						var savedDecimal = defaults.StringForKey(key);
						value = Convert.ToDecimal(savedDecimal, System.Globalization.CultureInfo.InvariantCulture);
						break;
					case TypeCode.Boolean:
						value = defaults.BoolForKey(key);
						break;
					case TypeCode.Int64:
						var savedInt64 = defaults.StringForKey(key);
						value = Convert.ToInt64(savedInt64, System.Globalization.CultureInfo.InvariantCulture);
						break;
					case TypeCode.Double:
						value = defaults.DoubleForKey(key);
						break;
					case TypeCode.String:
						value = defaults.StringForKey(key);
						break;
					case TypeCode.Int32:
						value = (Int32)defaults.IntForKey(key);
						break;
					case TypeCode.Single:
						value = (float)defaults.FloatForKey(key);
						break;
					case TypeCode.DateTime:
						var savedTime = defaults.StringForKey(key);
						var ticks = string.IsNullOrWhiteSpace(savedTime) ? -1 : Convert.ToInt64(savedTime, System.Globalization.CultureInfo.InvariantCulture);
						if (ticks == -1)
							value = defaultValue;
						else
							value = new DateTime(ticks);
						break;
					default:
						if (defaultValue is Guid)
						{
							var outGuid = Guid.Empty;
							var savedGuid = defaults.StringForKey(key);
							if(string.IsNullOrWhiteSpace(savedGuid))
							{
								value = outGuid;
							}
							else
							{
								Guid.TryParse(savedGuid, out outGuid);
								value = outGuid;
							}
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

		private bool AddOrUpdateValue(string key, object value, TypeCode typeCode)
		{
			lock(locker)
			{
				var defaults = NSUserDefaults.StandardUserDefaults;
				switch (typeCode)
				{
					case TypeCode.Decimal:
						defaults.SetString(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture), key);
						break;
					case TypeCode.Boolean:
						defaults.SetBool(Convert.ToBoolean(value), key);
						break;
					case TypeCode.Int64:
						defaults.SetString(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture), key);
						break;
					case TypeCode.Double:
						defaults.SetDouble(Convert.ToDouble(value, System.Globalization.CultureInfo.InvariantCulture), key);
						break;
					case TypeCode.String:
						defaults.SetString(Convert.ToString(value), key);
						break;
					case TypeCode.Int32:
						defaults.SetInt(Convert.ToInt32(value, System.Globalization.CultureInfo.InvariantCulture), key);
						break;
					case TypeCode.Single:
						defaults.SetFloat(Convert.ToSingle(value, System.Globalization.CultureInfo.InvariantCulture), key);
						break;
					case TypeCode.DateTime:
						defaults.SetString(Convert.ToString((Convert.ToDateTime(value)).Ticks), key);
						break;
					default:
						if (value is Guid)
						{
							if (value == null)
								value = Guid.Empty;
							defaults.SetString(((Guid)value).ToString(), key);
						}
						else
						{
							throw new ArgumentException(string.Format("Value of type {0} is not supported.", value.GetType().Name));
						}
						break;
				}
				try
				{
					defaults.Synchronize();
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
				}
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
				var defaults = NSUserDefaults.StandardUserDefaults;
				try
				{
					var nsString = new NSString(key);
					if (defaults.ValueForKey(nsString) != null)
					{
						defaults.RemoveObject(key);
						defaults.Synchronize();
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Settingses are changed inside or outside of the player, so update things.
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

