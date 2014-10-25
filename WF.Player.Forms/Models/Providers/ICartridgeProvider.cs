// <copyright file="ICartridgeProvider.cs" company="Wherigo Foundation">
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
	using System.ComponentModel;

	/// <summary>
	/// Describes a provider that can download files from remote
	/// storage into isolated storage.
	/// </summary>
	public interface ICartridgeProvider : INotifyPropertyChanged
	{
		/// <summary>
		/// Raised when a synchronization has completed.
		/// </summary>
		/// <remarks>The event arguments recapitulate all the
		/// files that have been added or marked for deletion,
		/// even those that were mentioned in <code>SyncProgress</code>
		/// events for the current sync.</remarks>
		event EventHandler<CartridgeProviderSyncEventArgs> SyncCompleted;

		/// <summary>
		/// Raised when progress information about a current sync
		/// operations is available.
		/// </summary>
		event EventHandler<CartridgeProviderSyncEventArgs> SyncProgress;

		/// <summary>
		/// Raised when the synchronization has aborted because it
		/// timed out.
		/// </summary>
		event EventHandler<CartridgeProviderSyncAbortEventArgs> SyncAborted;

		/// <summary>
		/// Gets the user-displayable name of the service.
		/// </summary>
		string ServiceName { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is linked with valid credentials
		/// and has valid session information..
		/// </summary>
		/// <value><c>true</c> if this instance is linked; otherwise, <c>false</c>.</value>
		bool IsLinked { get; }

		/// <summary>
		/// Gets a value indicating whether this instance is syncing.
		/// </summary>
		/// <value><c>true</c> if this instance is syncing; otherwise, <c>false</c>.</value>
		bool IsSyncing { get; }

		/// <summary>
		/// Gets or sets the path to isolated storage where cartridges
		/// downloaded by this provider are being stored.
		/// </summary>
		string IsoStoreCartridgesPath { get; set; }

		/// <summary>
		/// Gets or sets the path to isolated storage where extra cartridge
		/// content download by this provider is being stored.
		/// </summary>
		string IsoStoreCartridgeContentPath { get; set; }

		/// <summary>
		/// Starts to sync this provider's isolated storage folder with
		/// the contents of the remote storage.
		/// </summary>
		/// <remarks>
		/// This method only downloads new files and removes old files.
		/// No change is performed on the remote storage.
		/// </remarks>
		void BeginSync();

		/// <summary>
		/// Starts to link this provider with an account if it is not
		/// already linked.
		/// </summary>
		/// <remarks>
		/// This method can trigger navigational changes in the app,
		/// eventually displaying a custom sign-in experience.
		/// </remarks>
		void BeginLink();
	}
}
