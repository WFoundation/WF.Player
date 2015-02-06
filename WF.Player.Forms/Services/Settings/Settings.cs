// <copyright file="Settings.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
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
	using Xamarin.Forms;

	/// <summary>
	/// Common implemention of ISettings.
	/// </summary>
	public static class Settings
	{
		#region Constants

		/// <summary>
		/// Key for cartridge path.
		/// </summary>
		public const string CartridgePathKey = "CartridgePath";

		/// <summary>
		/// Key for autosave file.
		/// </summary>
		public const string AutosaveGWSKey = "AutosaveGWS";

		/// <summary>
		/// Key for autosave file.
		/// </summary>
		public const string AutosaveGWCKey = "AutosaveGWC";

		// Display

		/// <summary>
		/// Key for display theme.
		/// </summary>
		public const string DisplayThemeKey = "DisplayTheme";

		/// <summary>
		/// Key for text alignment.
		/// </summary>
		public const string TextAlignmentKey = "TextAlignment";

		/// <summary>
		/// Key for text alignment.
		/// </summary>
		public const string FontFamilyKey = "FontFamily";

		/// <summary>
		/// Key for font size.
		/// </summary>
		public const string TextSizeKey = "TextSize";

		/// <summary>
		/// Key for image alignment.
		/// </summary>
		public const string ImageAlignmentKey = "ImageAlignment";

		/// <summary>
		/// Key for image resize.
		/// </summary>
		public const string ImageResizeKey = "ImageResize";

		/// <summary>
		/// Key for screen energie saving.
		/// </summary>
		public const string ScreenEnergieKey = "ScreenEnergie";

		/// <summary>
		/// Key for feedback sound.
		/// </summary>
		public const string FeedbackSoundKey = "FeedbackSound";

		/// <summary>
		/// Key for feedback vibration.
		/// </summary>
		public const string FeedbackVibrationKey = "FeedbackVibration";

		// Localication

		/// <summary>
		/// Key for language.
		/// </summary>
		public const string LanguageKey = "Language";

		/// <summary>
		/// Key for format of coordinates.
		/// </summary>
		public const string FormatCoordinatesKey = "FormatCoordinates";

		/// <summary>
		/// Key for length unit.
		/// </summary>
		public const string UnitLengthKey = "UnitLength";

		/// <summary>
		/// Key for altitude unit.
		/// </summary>
		public const string UnitAltitudeKey = "UnitAltitude";

		/// <summary>
		/// Key for speed unit.
		/// </summary>
		public const string UnitSpeedKey = "UnitSpeed";

		/// <summary>
		/// Key for angle unit.
		/// </summary>
		public const string UnitAngleKey = "UnitAngle";

		// Sensors

		/// <summary>
		/// Key for last known position latitude.
		/// </summary>
		public const string LastKnownPositionLatitudeKey = "LastKnownPositionLatitude";

		/// <summary>
		/// Key for last known position longitude.
		/// </summary>
		public const string LastKnownPositionLongitudeKey = "LastKnownPositionLongitude";

		/// <summary>
		/// Key for last known position altitude.
		/// </summary>
		public const string LastKnownPositionAltitudeKey = "LastKnownPositionAltitude";

		/// <summary>
		/// Key for azimuth of sensor.
		/// </summary>
		public const string SensorAzimuthKey = "SensorAzimuth";

		/// <summary>
		/// Key for sensor orientation filter.
		/// </summary>
		public const string SensorOrientationFilterKey = "SensorOrientationFilter";

		/// <summary>
		/// Key for hardware sensor compass auto change.
		/// </summary>
		public const string SensorHardwareCompassAutoChange = "SensorHardwareCompassAutoChange";

		/// <summary>
		/// Key for hardware sensor compass auto change value.
		/// </summary>
		public const string SensorHardwareCompassAutoChangeValue = "SensorHardwareCompassAutoChangeValue";

		/// <summary>
		/// Key for hardware sensor compass.
		/// </summary>
		public const string SensorHardwareCompass = "SensorHardwareCompass";

		#endregion

		#region Default Values

		public static string DefaultFontFamily = Font.Default.FontFamily;

		public static int DefaultFontSize = Device.OnPlatform(18, 18, 18);

		public static TextAlignment DefaultTextAlignment = TextAlignment.Start;

		public static UnitLength DefaultUnitLength = UnitLength.Meter;

		public static FormatCoordinates DefaultFormatCoordinates = FormatCoordinates.DecimalMinutes;

		public static ImageResize DefaultImageResize = ImageResize.NoResize;

		public static TextAlignment DefaultImageAlignment = TextAlignment.Center;

		#endregion

		#region Shortcuts

		public static bool IsDarkTheme
		{
			get
			{
				return Current.GetValueOrDefault<int>(DisplayThemeKey, 0) != 0;
			}
		}

		public static string FontFamily
		{
			get
			{
				return Current.GetValueOrDefault<string>(FontFamilyKey, DefaultFontFamily);
			}
		}

		public static int FontSize
		{
			get
			{
				return Current.GetValueOrDefault<int>(TextSizeKey, DefaultFontSize);
			}
		}

		public static TextAlignment TextAlignment
		{
			get
			{
				return Current.GetValueOrDefault<TextAlignment>(TextAlignmentKey, DefaultTextAlignment);
			}
		}

		public static UnitLength UnitLength
		{
			get
			{
				return Current.GetValueOrDefault<UnitLength>(UnitLengthKey, DefaultUnitLength);
			}
		}

		public static FormatCoordinates FormatCoordinates
		{
			get
			{
				return Current.GetValueOrDefault<FormatCoordinates>(FormatCoordinatesKey, DefaultFormatCoordinates);
			}
		}

		public static ImageResize ImageResize
		{
			get
			{
				return Current.GetValueOrDefault<ImageResize>(ImageResizeKey, DefaultImageResize);
			}
		}

		public static TextAlignment ImageAlignment
		{
			get
			{
				return Current.GetValueOrDefault<TextAlignment>(ImageAlignmentKey, DefaultImageAlignment);
			}
		}

		#endregion

		#region ISettings Current Implementation

		private static Lazy<ISettings> settings = new Lazy<ISettings>(CreateSettings, System.Threading.LazyThreadSafetyMode.PublicationOnly);

		/// <summary>
		/// Current settings to use
		/// </summary>
		public static ISettings Current
		{
			get
			{
				ISettings ret = settings.Value;

				if (ret == null)
				{
					throw NotImplementedInReferenceAssembly();
				}

				return ret;
			}
		}

		private static ISettings CreateSettings()
		{
			return DependencyService.Get<ISettings>();
		}

		internal static Exception NotImplementedInReferenceAssembly()
		{
			return new NotImplementedException("This functionality is not implemented in the portable version of this assembly. You should reference the Xam.Plugins.Settings NuGet package from your main application project in order to reference the platform-specific implementation.");
		}

		#endregion
	}
}

