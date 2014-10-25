using System;
using MonoTouch.Foundation;
using WF.Player.Services.Preferences;
using Xamarin.Forms;

[assembly: Dependency(typeof(WF.Player.iOS.Services.Preferences.PreferencesIOS))]
namespace WF.Player.iOS.Services.Preferences
{
	public class PreferencesIOS : PreferencesCommon
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

			switch (typeof(T).Name) {
				case "String":
					result = (T)Convert.ChangeType(NSUserDefaults.StandardUserDefaults.StringForKey(key), typeof(T));
					break;
				case "Int64":
				case "Int32":
				case "Int16":
					result = (T)Convert.ChangeType(NSUserDefaults.StandardUserDefaults.IntForKey(key), typeof(T));
					break;
				case "Double":
					result = (T)Convert.ChangeType(NSUserDefaults.StandardUserDefaults.DoubleForKey(key), typeof(T));
					break;
				case "Single":
					result = (T)Convert.ChangeType(NSUserDefaults.StandardUserDefaults.FloatForKey(key), typeof(T));
					break;
				case "Boolean":
					result = (T)Convert.ChangeType(NSUserDefaults.StandardUserDefaults.BoolForKey(key), typeof(T));
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
			switch (typeof(T).Name) {
				case "string":
					NSUserDefaults.StandardUserDefaults.SetString ((string)Convert.ChangeType(value, typeof(T)), key);
					break;
				case "int":
					NSUserDefaults.StandardUserDefaults.SetInt ((int)Convert.ChangeType(value, typeof(T)), key);
					break;
				case "double":
					NSUserDefaults.StandardUserDefaults.SetDouble ((double)Convert.ChangeType(value, typeof(T)), key);
					break;
				case "float":
					NSUserDefaults.StandardUserDefaults.SetFloat ((float)Convert.ChangeType(value, typeof(T)), key);
					break;
				case "bool":
					NSUserDefaults.StandardUserDefaults.SetBool ((bool)Convert.ChangeType(value, typeof(T)), key);
					break;
			}

			NSUserDefaults.StandardUserDefaults.Synchronize();
		}

		#endregion
	}
}

