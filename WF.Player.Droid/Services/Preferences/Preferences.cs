using System;
using WF.Player.Services.Preferences;
using Xamarin.Forms;
using Android.Preferences;
using Android.Content;

[assembly: Dependency(typeof(WF.Player.Droid.Services.Preferences.PreferencesAndroid))]

namespace WF.Player.Droid.Services.Preferences
{
	public class PreferencesAndroid : PreferencesCommon
	{
		#region Methods

		/// <summary>
		/// Get preference for specified key.
		/// </summary>
		/// <param name="key">Key for prefernece value as string.</param>
		/// <typeparam name="T">Type parameter for result.</typeparam>
		public override T Get<T>(string key)
		{
			T result = default(T);

			ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Forms.Context);

			switch (typeof(T).Name) {
				case "String":
					result = (T)Convert.ChangeType(prefs.GetString(key, default(string)), typeof(T));
					break;
				case "Int64":
					result = (T)Convert.ChangeType(prefs.GetLong(key, default(long)), typeof(T));
					break;
				case "Int32":
				case "Int16":
					result = (T)Convert.ChangeType(prefs.GetInt(key, default(int)), typeof(T));
					break;
				case "Double":
					result = (T)Convert.ChangeType(prefs.GetFloat(key, default(float)), typeof(T));
					break;
				case "Single":
					result = (T)Convert.ChangeType(prefs.GetFloat(key, default(float)), typeof(T));
					break;
				case "Boolean":
					result = (T)Convert.ChangeType(prefs.GetBoolean(key, default(bool)), typeof(T));
					break;
			}

			return result;
		}

		/// <summary>
		/// Set the specified key and preference value.
		/// </summary>
		/// <param name="key">Key for prefernece value as string.</param>
		/// <param name="value">Preference value.</param>
		/// <typeparam name="T">Type parameter of value.</typeparam>
		public override void Set<T>(string key, T value)
		{
			ISharedPreferencesEditor prefs = PreferenceManager.GetDefaultSharedPreferences(Xamarin.Forms.Forms.Context).Edit();

			switch (typeof(T).Name.ToLower()) {
				case "string":
					prefs.PutString (key, (string)Convert.ChangeType(value, typeof(T)));
					break;
				case "int":
					prefs.PutInt(key, (int)Convert.ChangeType(value, typeof(T)));
					break;
				case "double":
					prefs.PutFloat (key, (float)Convert.ChangeType(value, typeof(T)));
					break;
				case "float":
					prefs.PutFloat (key, (float)Convert.ChangeType(value, typeof(T)));
					break;
				case "bool":
					prefs.PutBoolean (key, (bool)Convert.ChangeType(value, typeof(T)));
					break;
			}

			prefs.Commit();
		}

		#endregion
	}
}

