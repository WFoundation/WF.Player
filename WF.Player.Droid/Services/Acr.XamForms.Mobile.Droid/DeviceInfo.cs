using System;
using Android.Content;
using Android.Content.PM;
using Android.Content.Res;
using Android.Telephony;
using Xamarin.Forms;
using Acr.XamForms.Mobile.Droid;

[assembly: Dependency(typeof(DeviceInfo))]

namespace Acr.XamForms.Mobile.Droid 
{
	public class DeviceInfo : IDeviceInfo 
	{
		private readonly Lazy<string> deviceId;
		private readonly Lazy<int> screenHeight;
		private readonly Lazy<int> screenWidth;

		public DeviceInfo() 
		{
			this.screenHeight = new Lazy<int>(() => {
				var d = Resources.System.DisplayMetrics;
				return (int)(d.HeightPixels / d.Density);
			});
			this.screenWidth = new Lazy<int>(() => {
				var d = Resources.System.DisplayMetrics;
				return (int)(d.WidthPixels / d.Density);
			});
			this.deviceId = new Lazy<string>(() => {
				var tel = (TelephonyManager)Forms.Context.ApplicationContext.GetSystemService(Context.TelephonyService);
				return tel.DeviceId;
			});
		}

		public int ScreenHeight 
		{
			get { return this.screenHeight.Value; }
		}

		public int ScreenWidth 
		{
			get { return this.screenWidth.Value; }
		}

		public string DeviceId 
		{
			get { return this.deviceId.Value; }
		}

		public string Manufacturer 
		{
			get { return Android.OS.Build.Manufacturer; }
		}

		public string Model 
		{
			get { return Android.OS.Build.Model; }
		}

		private string os;
		public string OperatingSystem {
			get 
			{
				this.os = this.os ?? String.Format("{0} - SDK: {1}", Android.OS.Build.VERSION.Release, Android.OS.Build.VERSION.SdkInt);
				return this.os;
			}
		}

		private double version;
		public double Version {
			get 
			{
				if (double.TryParse(Android.OS.Build.VERSION.Release, out this.version)) // Android.OS.Build.VERSION.SdkInt
				{
					return this.version;
				}
				else
				{
					return double.NaN;
				}
			}
		}

		public bool IsFrontCameraAvailable 
		{
			get { return Forms.Context.ApplicationContext.PackageManager.HasSystemFeature(PackageManager.FeatureCameraFront); }
		}

		public bool IsRearCameraAvailable 
		{
			get { return Forms.Context.ApplicationContext.PackageManager.HasSystemFeature(PackageManager.FeatureCamera); }
		}

		public bool IsSimulator 
		{
			get { return Android.OS.Build.Product.Equals("google_sdk"); }
		}
	}
}