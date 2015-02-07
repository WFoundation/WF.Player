// <copyright file="BarCodeFormat.cs" company="Wherigo Foundation">
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

	public enum BarCodeFormat
	{
		AZTEC = 1,
		CODABAR = 2,
		CODE_39 = 4,
		CODE_93 = 8,
		CODE_128 = 16,
		DATA_MATRIX = 32,
		EAN_8 = 64,
		EAN_13 = 128,
		ITF = 256,
		MAXICODE = 512,
		PDF_417 = 1024,
		QR_CODE = 2048,
		RSS_14 = 4096,
		RSS_EXPANDED = 8192,
		UPC_A = 16384,
		UPC_E = 32768,
		UPC_EAN_EXTENSION = 65536,
		MSI = 131072,
		PLESSEY = 262144,
		All_1D = MSI | UPC_E | UPC_A | RSS_EXPANDED | RSS_14 | ITF | EAN_13 | EAN_8 | CODE_128 | CODE_93 | CODE_39 | CODABAR
	}
}
