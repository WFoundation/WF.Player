// <copyright file="BarCode.cs" company="Wherigo Foundation">
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
using UIKit;
using WF.Player.Controls;
using CoreGraphics;

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.iOS.Services.BarCode.BarCodeScanner))]

namespace WF.Player.iOS.Services.BarCode
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Xamarin.Forms;
	using WF.Player.Services.BarCode;
	using ZXing;
	using ZXing.Common;
	using ZXing.Mobile;

	public class BarCodeScanner : IBarCodeScanner 
	{
		public BarCodeScanner() 
		{
			var def = MobileBarcodeScanningOptions.Default;

			BarCodeReadConfiguration.Default = new BarCodeReadConfiguration 
				{
					AutoRotate = def.AutoRotate,
					CharacterSet = def.CharacterSet,
					DelayBetweenAnalyzingFrames = def.DelayBetweenAnalyzingFrames,
					InitialDelayBeforeAnalyzingFrames = def.InitialDelayBeforeAnalyzingFrames,
					PureBarcode = def.PureBarcode,
					TryHarder = def.TryHarder,
					TryInverted = def.TryInverted,
					UseFrontCameraIfAvailable = def.UseFrontCameraIfAvailable
				};
		}

		public async Task<BarCodeResult> Read(BarCodeReadConfiguration config, CancellationToken cancelToken) 
		{
			config = config ?? BarCodeReadConfiguration.Default;

			var controller = ((UIViewController)((ExtendedNavigationPage)App.GameNavigation).ViewController);
			var scanner = new MobileBarcodeScanner(controller);

			scanner.UseCustomOverlay = true;
			scanner.CustomOverlay = new BarCodeScannerOverlay(new CGRect(0, 0, (nfloat)App.GameNavigation.Bounds.Width, (nfloat)App.GameNavigation.Bounds.Height), "", "", config.CancelText, config.FlashlightText, () => scanner.Cancel(), () => scanner.ToggleTorch());

			scanner.CancelButtonText = config.CancelText;
			scanner.FlashButtonText = config.FlashlightText;

			cancelToken.Register(scanner.Cancel);

			var result = await scanner.Scan(this.GetXingConfig(config));

			return (result == null || String.IsNullOrWhiteSpace(result.Text)
				? BarCodeResult.Fail
				: new BarCodeResult(result.Text, FromXingFormat(result.BarcodeFormat))
			);
		}

		private static BarCodeFormat FromXingFormat(ZXing.BarcodeFormat format) 
		{
			return (BarCodeFormat)Enum.Parse(typeof(BarCodeFormat), format.ToString());
		}

		private MobileBarcodeScanningOptions GetXingConfig(BarCodeReadConfiguration cfg) 
		{
			var opts = new MobileBarcodeScanningOptions 
				{
					AutoRotate = cfg.AutoRotate,
					CharacterSet = cfg.CharacterSet,
					DelayBetweenAnalyzingFrames = cfg.DelayBetweenAnalyzingFrames,
					InitialDelayBeforeAnalyzingFrames = cfg.InitialDelayBeforeAnalyzingFrames,
					PureBarcode = cfg.PureBarcode,
					TryHarder = cfg.TryHarder,
					TryInverted = cfg.TryInverted,
					UseFrontCameraIfAvailable = cfg.UseFrontCameraIfAvailable
				};

			if (cfg.Formats != null && cfg.Formats.Count > 0) 
			{
				opts.PossibleFormats = cfg.Formats
					.Select(x => (BarcodeFormat)(int)x)
					.ToList();
			}

			return opts;
		}
	}
}
