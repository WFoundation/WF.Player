// <copyright file="BarCodeResult.cs" company="Wherigo Foundation">
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
	public class BarCodeResult 
	{
		public bool Success { get; private set; }

		public string Code { get; private set; }

		public BarCodeFormat Format { get; private set; }

		public static BarCodeResult Fail { get; private set; }

		static BarCodeResult() {
			Fail = new BarCodeResult 
				{
					Success = false
				};
		}

		private BarCodeResult() 
		{ 
		}

		public BarCodeResult(string code, BarCodeFormat format) 
		{
			this.Success = true;
			this.Code = code;
			this.Format = format;
		}
	}
}
