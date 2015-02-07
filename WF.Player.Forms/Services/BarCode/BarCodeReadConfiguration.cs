// <copyright file="BarCodeReadConfiguration.cs" company="Wherigo Foundation">
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

namespace WF.Player.Services.BarCode
{
	using System;
	using System.Collections.Generic;

	public class BarCodeReadConfiguration
	{
		private static BarCodeReadConfiguration @default;

		public static BarCodeReadConfiguration Default 
		{
			get 
			{
				@default = @default ?? new BarCodeReadConfiguration();
				return @default;
			}

			set 
			{
				if (value == null)
				{
					throw new ArgumentNullException("Default barcode read options cannot be null");
				}
				@default = value;
			}
		}

		public string TopText { get; set; }

		public string BottomText { get; set; }

		public string FlashlightText { get; set; }

		public string CancelText { get; set; }

		public bool? AutoRotate { get; set; }

		public string CharacterSet { get; set; }

		public int DelayBetweenAnalyzingFrames { get; set; }

		public bool? PureBarcode { get; set; }

		public int InitialDelayBeforeAnalyzingFrames { get; set; }

		public bool? TryHarder { get; set; }

		public bool? TryInverted { get; set; }

		public bool? UseFrontCameraIfAvailable { get; set; }

		public List<BarCodeFormat> Formats { get; set; }

		public BarCodeReadConfiguration() 
		{
			this.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
			this.BottomText = "Wait for the barcode to automatically scan";
			this.Formats = new List<BarCodeFormat>(3);
		}
	}
}
