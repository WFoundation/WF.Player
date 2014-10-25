// <copyright file="CartridgeProviderSyncEventArgs.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Brice Clocher (contact@cybisoft.net)
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player.Models.Providers
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Arguments for a provider's synchronization of cartridges.
	/// </summary>
	public class CartridgeProviderSyncEventArgs : EventArgs
	{
		/// <summary>
		/// Gets or sets the file paths that were added to the
		/// isolated storage during the sync.
		/// </summary>
		public IEnumerable<string> AddedFiles { get; set; }

		/// <summary>
		/// Gets or sets the file paths that need to be removed from 
		/// the isolated storage.
		/// </summary>
		/// <remarks>
		/// They have not been removed yet to give time to callers
		/// to prevent the user from running a deleted cartridge.
		/// </remarks>
		public IEnumerable<string> ToRemoveFiles { get; set; }
	}
}
