// <copyright file="ICommonHelper.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
//   Copyright (C) 2012-2015  Brice Clocher (mangatome@gmail.com)
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
using System.IO;

namespace WF.Player.Common
{
	/// <summary>
	/// Interface for platform specific things for WF.Player.Common.
	/// </summary>
	public interface ICommonPlatformHelper
	{
		#region Properties

		/// <summary>
		/// Gets the path for cartridge files, save files and log files.
		/// </summary>
		/// <value>The path for cartridge files.</value>
		string PathForFiles { get; }

		/// <summary>
		/// Gets the path for database files.
		/// </summary>
		/// <value>The path for save files.</value>
		string PathForDatabase { get; }

		/// <summary>
		/// Gets the SQLite database connection.
		/// </summary>
		/// <value>The database.</value>
		SQLite.Net.SQLiteConnection Database { get; }

		/// <summary>
		/// Gets the stream for demo cartridge.
		/// </summary>
		/// <value>The stream for demo cartridge.</value>
		Stream StreamForDemoCartridge { get; }

		#endregion

		#region Methods

		#endregion
	}
}
