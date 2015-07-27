// <copyright file="CartridgeStore.cs" company="Wherigo Foundation">
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

namespace WF.Player.Models
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using System.Text.RegularExpressions;
	using WF.Player.Core;
	using WF.Player.Models.Providers;
	using WF.Player.Utils;
	using Xamarin.Forms;

	/// <summary>
	/// A store for Cartridges and their related data.
	/// </summary>
	public class CartridgeStore : List<CartridgeTag>, INotifyCollectionChanged, INotifyPropertyChanged
	{
		/// <summary>
		/// The name of the cartridge name property.
		/// </summary>
		public const string CartridgeNamePropertyName = "Cartridge.Name";

		/// <summary>
		/// The name of the cartridge author name property.
		/// </summary>
		public const string CartridgeAuthorNamePropertyName = "Cartridge.AuthorName";

		/// <summary>
		/// The name of the cartridge poster source property.
		/// </summary>
		public const string CartridgePosterPropertyName = "Cartridge.Poster";

		/// <summary>
		/// The name of the cartridge poster source property.
		/// </summary>
		public const string CartridgeIconPropertyName = "Cartridge.Icon";

		/// <summary>
		/// The version of the cartridge property.
		/// </summary>
		public const string CartridgeVersionPropertyName = "Cartridge.Version";

		/// <summary>
		/// The activity type of the cartridge property.
		/// </summary>
		public const string CartridgeActivityTypePropertyName = "Cartridge.ActivityType";

		#region Members

		/// <summary>
		/// The is busy aggregator.
		/// </summary>
		private ProgressAggregator isBusyAggregator = new ProgressAggregator();

		/// <summary>
		/// The providers.
		/// </summary>
		private List<ICartridgeProvider> providers = new List<ICartridgeProvider>();

		/// <summary>
		/// The sync root.
		/// </summary>
		private object syncRoot = new object();

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Models.CartridgeStore"/> class.
		/// </summary>
		public CartridgeStore() : base()
		{
			// Registers event handlers.
			isBusyAggregator.PropertyChanged += new PropertyChangedEventHandler(OnIsBusyAggregatorPropertyChanged);

			// Adds some cartridge providers.
			AddDefaultProviders();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Occurs when collection changed.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		#endregion

		#region Properties

		// TODO: Remove later
		public Color TextColor
		{
			get
			{
				return App.Colors.Text;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this instance is busy loading cartridges.
		/// </summary>
		public bool IsBusy
		{
			get
			{
				return isBusyAggregator.HasWorkingSource;
			}
		}

		/// <summary>
		/// Gets an enumeration of the cartridge providers that can download
		/// cartridges for this instance.
		/// </summary>
		public IEnumerable<ICartridgeProvider> Providers
		{
			get
			{
				return providers;
			}
		}

		#endregion

		#region Tag Retrieval

		/// <summary>
		/// Gets the single tag for a Cartridge.
		/// </summary>
		/// <param name="cartridge">The Cartridge to get the tag for.</param>
		/// <returns>CartridgeTag, which was found or a default.</returns>
		public CartridgeTag GetCartridgeTagOrDefault(Cartridge cartridge)
		{
			lock (syncRoot)
			{
				return this.SingleOrDefault(ct => ct.Cartridge.Filename == cartridge.Filename); 
			}
		}

		/// <summary>
		/// Gets the single tag for a cartridge filename and a GUID.
		/// </summary>
		/// <remarks>This method looks for a tag with similar GUID. If not found,
		/// it tries to load the cartridge at the specified filename and returns
		/// its tag if the GUIDs match.</remarks>
		/// <param name="filename">Filename of the cartridge.</param>
		/// <returns>Null if the tag was not found or the GUIDs didn't match.</returns>
		public CartridgeTag GetCartridgeTagOrDefault(string filename)
		{
			CartridgeTag tag = null;
			lock (syncRoot)
			{
				// Tries to get the tag if it is registered already.
				tag = this.SingleOrDefault(ct => ct.Cartridge.Filename == filename);
			}

			// If the tag is found, returns it.
			if (tag != null)
			{
				return tag;
			}

			// Tries to accept the tag from filename.
			tag = AcceptCartridge(filename);

			// Only returns the tag if both GUIDs match.
			return tag;
		}

		#endregion

		#region Sync From IsoStore

		/// <summary>
		/// Synchronizes the store from the Isolated Storage.
		/// </summary>
		public void SyncFromStore()
		{
			// TODO: Removed, because of a problem with iOS. Update of list always crashes. So do it synchron.

//			BackgroundWorker bw = new BackgroundWorker();
//
//			bw.DoWork += (o, e) =>
//			{
				SyncFromStoreCore(false);
//			};
//
//			bw.RunWorkerAsync();
		}

		/// <summary>
		/// Syncs from store core.
		/// </summary>
		/// <param name="asyncEachCartridge">If set to <c>true</c> async each cartridge.</param>
		private void SyncFromStoreCore(bool asyncEachCartridge)
		{
			// Imports all GWC files from the directory.
			var files = new DirectoryInfo(App.PathForCartridges).GetFiles();

			foreach (var file in files.Where((f) => f.Extension.EndsWith("gwc", StringComparison.InvariantCultureIgnoreCase)))
			{
				var filename = file.FullName;

				// Check filename
//				if (!Regex.IsMatch(file.FullName, @"[-](\d{14}).gwc"))
//				{
//					// New file, so convert file name
//					// Append date and time of first time, this file is checked
//					filename = Path.Combine(App.PathForCartridges, string.Format("{0}-{1:yyyyMMddhhmmss}.gwc", Path.GetFileNameWithoutExtension(file.FullName), DateTime.Now.ToUniversalTime()));
//					file.MoveTo(filename);
//				}

				// Accept the GWC.
				if (asyncEachCartridge)
				{
					AcceptCartridgeAsync(filename);
				}
				else
				{
					AcceptCartridge(filename);
				}
			}
		}

		#endregion

		#region Sync From Providers

		/// <summary>
		/// Starts syncing all linked providers that are not syncing.
		/// </summary>
		public void SyncFromProviders()
		{
			foreach (ICartridgeProvider provider in providers)
			{
				if (provider.IsLinked && !provider.IsSyncing)
				{
					provider.BeginSync();
				}
			}
		}

		#endregion

		#region Tag Acceptance

		/// <summary>
		/// Ensures that a cartridge is not present in the store.
		/// </summary>
		/// <param name="filename">Filename of the cartridge to remove.</param>
		/// <remarks>This method does not effectively remove the cartridge
		/// from the isolated storage.</remarks>
		private void RejectCartridge(string filename)
		{
			System.Diagnostics.Debug.WriteLine("CartridgeStore: Trying to reject cartridge " + filename);

			// Updates the progress.
			string businessTag = "reject" + filename;
			isBusyAggregator[businessTag] = true;

			lock (syncRoot)
			{
				// Gets the existing cartridge if it was found.
				CartridgeTag existingTag = this.SingleOrDefault(cc => cc.Cartridge.Filename == filename);

				// Removes the tag if it was found.
				if (existingTag != null)
				{
					this.Remove(existingTag);
					RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, existingTag));
				}
			}

			// Updates the progress.
			isBusyAggregator[businessTag] = false;
		}

		/// <summary>
		/// Ensures asynchronously that a cartridge is present in the store.
		/// </summary>
		/// <param name="filename">Filename of the cartridge to consider.</param>
		private void AcceptCartridgeAsync(string filename)
		{
			BackgroundWorker bw = new BackgroundWorker();

			bw.DoWork += new System.ComponentModel.DoWorkEventHandler((o, e) => AcceptCartridge(filename));

			bw.RunWorkerAsync();
		}

		/// <summary>
		/// Ensures that a cartridge is present in the store.
		/// </summary>
		/// <param name="filename">Filename of the cartridge to consider.</param>
		/// <returns>The CartridgeContext for this cartridge from the store, or a new CartridgeContext
		/// if there was none in store for this cartridge.</returns>
		private CartridgeTag AcceptCartridge(string filename)
		{
			System.Diagnostics.Debug.WriteLine("CartridgeStore: Trying to accept cartridge " + filename);

			bool isAborted = false;

			// Refreshes the progress.
			string businessTag = "accept:" + filename;
			isBusyAggregator[businessTag] = true;

			// Creates a cartridge object.
			Cartridge cart = new Cartridge(filename);

			// Loads the cartridge.
			var file = new FileInfo(Path.Combine(App.PathForCartridges, filename));

			// File exist check.
			if (!file.Exists)
			{
				System.Diagnostics.Debug.WriteLine("CartridgeStore: WARNING: Cartridge file not found: " + filename);

				isAborted = true;
			}

			// Loads the metadata.
			if (!isAborted)
			{
				using (var fs = new FileStream(Path.Combine(App.PathForCartridges, filename), FileMode.Open, FileAccess.Read))
				{
					try
					{
						WF.Player.Core.Formats.CartridgeLoaders.LoadMetadata(fs, cart);
					}
					catch (Exception ex)
					{
						// This cartridge seems improper to loading.
						// Let's just dump the exception and return.
						// TODO
//							DebugUtils.DumpException(ex, dumpOnBugSenseToo: true);
						System.Diagnostics.Debug.WriteLine("CartridgeStore: WARNING: Loading failed, ignored : " + filename);
						isAborted = true;
					}
				} 
			}

			CartridgeTag existingCC;
			CartridgeTag newCC = null;

			if (!isAborted)
			{
				// Returns the existing cartridge if it was found.
				lock (syncRoot)
				{
					existingCC = this.SingleOrDefault(cc => cc.Cartridge.Filename == filename);
				}

				if (existingCC != null)
				{
					// Refreshes the progress.
					isBusyAggregator[businessTag] = false;

					return existingCC;
				}

				// The cartridge does not exist in the store yet. Creates an entry for it.
				newCC = new CartridgeTag(cart);

				newCC.PropertyChanged += HandlePropertyChanged;

				// Adds the context to the store.
				Device.BeginInvokeOnMainThread(() =>
					{
						lock (syncRoot)
						{
							this.Add(newCC);
							RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newCC));
						}
					});
			}

			// Refreshes the progress.
			isBusyAggregator[businessTag] = false;

			// Returns the new cartridge context or null if the operation
			// was aborted.
			return newCC;
		}

		private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Removed")
			{
				RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, (CartridgeTag)sender));
			}
		}

		/// <summary>
		/// Accepts the savegame.
		/// </summary>
		/// <param name="filename">Filename of savegame.</param>
		private void AcceptSavegame(string filename)
		{
			System.Diagnostics.Debug.WriteLine("CartridgeStore: Trying to accept savegame " + filename);

			// Refreshes the progress.
			string businessTag = "accept:" + filename;
			isBusyAggregator[businessTag] = true;

			// Gets the cartridge this savegame is associated with.
			bool isAborted = false;
			WF.Player.Core.Formats.GWS.Metadata saveMetadata = null;
			var file = new FileInfo(Path.Combine(App.PathForSavegames, filename));

			// File exist check.
			if (!file.Exists)
			{
				System.Diagnostics.Debug.WriteLine("CartridgeStore: WARNING: Savegame file not found: " + filename);

				isAborted = true;
			}

			if (!isAborted)
			{
				using (var fs = new FileStream(Path.Combine(App.PathForSavegames, filename), FileMode.Open, FileAccess.Read))
				{
					saveMetadata = WF.Player.Core.Formats.GWS.LoadMetadata(fs);
				} 
			}

			if (!isAborted)
			{
				// For each matching tag, creates an associated savegame and copies the file to each
				// tag's content folder.
				List<CartridgeTag> matches;

				lock (syncRoot)
				{
					matches = this.Where(ct => ct.Cartridge.Name == saveMetadata.CartridgeName).ToList();
				}

				foreach (CartridgeTag tag in matches)
				{
					// Creates a savegame.
					CartridgeSavegame save = new CartridgeSavegame(tag, Path.GetFileName(filename));

					// Adds the savegame to its tag.
					tag.AddSavegame(save);
				} 
			}

			// Refreshes the progress.
			isBusyAggregator[businessTag] = false;
		}

		#endregion

		#region Provider Management

		/// <summary>
		/// Adds the default providers.
		/// </summary>
		private void AddDefaultProviders()
		{
			// AddProvider(new SkyDriveCartridgeProvider() { IsBackgroundDownloadAllowed = true });
		}

		/// <summary>
		/// Adds the provider.
		/// </summary>
		/// <param name="provider">Provider to use.</param>
		private void AddProvider(ICartridgeProvider provider)
		{
			// Sanity check.
			if (providers.Any(p => p.ServiceName == provider.ServiceName))
			{
				throw new InvalidOperationException("A provider with same ServiceName already exists. " + provider.ServiceName);
			}

			// Registers event handlers.
			provider.PropertyChanged += new PropertyChangedEventHandler(OnProviderPropertyChanged);
			provider.SyncCompleted += new EventHandler<CartridgeProviderSyncEventArgs>(OnProviderSyncCompleted);
			provider.SyncProgress += new EventHandler<CartridgeProviderSyncEventArgs>(OnProviderSyncProgress);
			provider.SyncAborted += new EventHandler<CartridgeProviderSyncAbortEventArgs>(OnProviderSyncAborted);

			// Sets the provider up.
			//			provider.IsoStoreCartridgesPath = String.Format("{0}/From {1}", IsoStoreCartridgesPath, provider.ServiceName);
			//			provider.IsoStoreCartridgeContentPath = CartridgeTag.GlobalSavegamePath;

			// Adds the provider to the list.
			providers.Add(provider);

			// Notifies the list has changed.
			RaisePropertyChanged(new PropertyChangedEventArgs("Providers"));
		}

		/// <summary>
		/// Raises the provider sync aborted event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Cartridge provider sync event arguments.</param>
		private void OnProviderSyncAborted(object sender, CartridgeProviderSyncAbortEventArgs e)
		{
			isBusyAggregator[sender] = false;
		}

		/// <summary>
		/// Raises the provider sync completed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Cartridge provider sync event arguments.</param>
		private void OnProviderSyncCompleted(object sender, CartridgeProviderSyncEventArgs e)
		{
			ProcessSyncEvent(e);

			isBusyAggregator[sender] = false;
		}

		/// <summary>
		/// Raises the provider sync progress event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Cartridge provider sync event arguments.</param>
		private void OnProviderSyncProgress(object sender, CartridgeProviderSyncEventArgs e)
		{
			isBusyAggregator[sender] = true;

			ProcessSyncEvent(e);
		}

		/// <summary>
		/// Processes the sync event.
		/// </summary>
		/// <param name="e">Cartridge provider sync event arguments.</param>
		private void ProcessSyncEvent(CartridgeProviderSyncEventArgs e)
		{
			// Accepts the files that have been added.
			List<string> filesToRemove = new List<string>();
			foreach (string filename in e.AddedFiles.Where(s => s.EndsWith(".gwc", StringComparison.InvariantCultureIgnoreCase)))
			{
				AcceptCartridge(filename);
			}

			foreach (string filename in e.AddedFiles.Where(s => s.EndsWith(".gws", StringComparison.InvariantCultureIgnoreCase)))
			{
				// Copies this savegame to the content folders of each cartridge
				// whose name matches the cartridge name in the savegame metadata.
				AcceptSavegame(filename);

				// Marks the file to be deleted.
				filesToRemove.Add(filename);
			}

			// Rejects and removes the files marked to be removed.
			filesToRemove.AddRange(e.ToRemoveFiles);

			foreach (string filename in filesToRemove)
			{
				// Removes the file from the list.
				if (filename.EndsWith(".gwc", StringComparison.InvariantCultureIgnoreCase))
				{
					RejectCartridge(filename); 
				}

				// Deletes the file in the store.
				var file = new FileInfo(Path.Combine(App.PathForCartridges, filename));

				if (file.Exists)
				{
					file.Delete();
				}
			}
		}

		/// <summary>
		/// Raises the provider property changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Property changed event arguments.</param>
		private void OnProviderPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			ICartridgeProvider provider = (ICartridgeProvider)sender;

			if (e.PropertyName == "IsLinked" && provider.IsLinked)
			{
				// The provider is now linked. Start syncing.
				provider.BeginSync();
			}
			else if (e.PropertyName == "IsSyncing")
			{
				isBusyAggregator[provider] = provider.IsSyncing;
			}
		}

		#endregion

		/// <summary>
		/// Raises the is busy aggregator property changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Property changed event arguments.</param>
		private void OnIsBusyAggregatorPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "HasWorkingSource")
			{
				// Relays the event.
				RaisePropertyChanged(new PropertyChangedEventArgs("IsBusy"));
			}
		}

		/// <summary>
		/// Raises the collection changed.
		/// </summary>
		/// <param name="args">Arguments.</param>
		private void RaiseCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			var handle = CollectionChanged;

			if (handle != null)
			{
				lock (syncRoot)
				{
					Device.BeginInvokeOnMainThread(() => handle(this, args));
				}
			}
		}

		/// <summary>
		/// Raises the property changed.
		/// </summary>
		/// <param name="args">Arguments.</param>
		private void RaisePropertyChanged(PropertyChangedEventArgs args)
		{
			var handle = PropertyChanged;

			if (handle != null)
			{
				lock (syncRoot)
				{
					Device.BeginInvokeOnMainThread(() => handle(this, args));
				}
			}
		}
	}
}
