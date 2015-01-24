using System;

namespace Acr.XamForms.Mobile 
{
	public interface IDeviceInfo 
	{
		int ScreenHeight { get; }

		int ScreenWidth { get; }

		string DeviceId { get; }

		string Manufacturer { get; }

		string Model { get; }

		string OperatingSystem { get; }

		double Version { get; }

		bool IsFrontCameraAvailable { get; }

		bool IsRearCameraAvailable { get; }

		bool IsSimulator { get; }
	}
}