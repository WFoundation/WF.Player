using System;
using ObjCRuntime;
using UIKit;
using Xamarin.Forms;
using Acr.XamForms.Mobile.iOS;

[assembly: Dependency(typeof(DeviceInfo))]

namespace WF.Player.Services.Mobile.iOS {

	public class DeviceInfo : IDeviceInfo 
	{

		public int ScreenHeight 
		{
			get { return (int)UIScreen.MainScreen.Bounds.Height; }
		}

		public int ScreenWidth 
		{
			get { return (int)UIScreen.MainScreen.Bounds.Width; }
		}

		public string DeviceId 
		{
			get { return UIDevice.CurrentDevice.IdentifierForVendor.AsString(); }
		}

		public string Manufacturer 
		{
			get { return "Apple"; }
		}

		public string Model 
		{
			get { return UIDevice.CurrentDevice.Model; }
		}

		private string os;
		public string OperatingSystem 
		{
			get 
			{
				this.os = this.os ?? String.Format("{0} {1}", UIDevice.CurrentDevice.SystemName, UIDevice.CurrentDevice.SystemVersion);
				return this.os;
			}
		}

		private double version;
		public double Version 
		{
			get 
			{
				if (double.TryParse(UIDevice.CurrentDevice.SystemVersion, out this.version))
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
			get { return UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Front); }
		}

		public bool IsRearCameraAvailable 
		{
			get { return UIImagePickerController.IsCameraDeviceAvailable(UIImagePickerControllerCameraDevice.Rear); }
		}

		public bool IsSimulator 
		{
			get { return (Runtime.Arch == Arch.SIMULATOR); }
		}
	}
}