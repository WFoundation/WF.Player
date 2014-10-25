// <copyright file="Chars.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player
{
	using System;
	using System.Text;

	/// <summary>
	/// Special chars.
	/// </summary>
	public class Chars
	{
		/// <summary>
		/// Gets unicode character for task state unknown.
		/// </summary>
		public string TaskNone
		{
			get
			{
				return Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9C, 0x93 });    	// UTF-8 2713
			}
		}

		/// <summary>
		/// Gets unicode character for task state correct.
		/// </summary>
		public string TaskCorrect
		{
			get
			{
				return Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9C, 0x93 });    	// UTF-8 2713
			}
		}

		/// <summary>
		/// Gets unicode character for task state not correct.
		/// </summary>
		public string TaskNotCorrect
		{
			get
			{
				return Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9C, 0x97 });  	// UTF-8 2717
			}
		}

		/// <summary>
		/// Gets unicode character for infinite.
		/// </summary>
		public string Infinite
		{
			get
			{
				return Encoding.UTF8.GetString(new byte[] { 0xE2, 0x88, 0x9E });  		// UTF-8 221E
			}
		}

		/// <summary>
		/// Gets unicode character for checked.
		/// </summary>
		public string Checked
		{
			get
			{
				return Encoding.UTF8.GetString(new byte[] { 0xE2, 0x9C, 0x93 });    		// UTF-8 2713
			}
		}
	}
}
