// <copyright file="CartridgeTag.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014 Dirk Weltz (mail@wfplayer.com)
// </copyright>
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
namespace WF.Player.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using WF.Player.Core;
    using WF.Player.Core.Formats;
    using Xamarin.Forms;
    using PCLStorage;
    using Common;
    /// <summary>
    /// Provides a static metadata description and cache of a Cartridge.
    /// </summary>
    public class CartridgeTag : INotifyPropertyChanged
	{
		#region Constants

		/// <summary>
		/// The width of the small thumbnail minimum.
		/// </summary>
		public const int SmallThumbnailMinWidth = 173;
		/// <summary>
		/// The width of the big thumbnail minimum.
		/// </summary>
		public const int BigThumbnailMinWidth = 432;
		/// <summary>
		/// The thumb cache filename.
		/// </summary>
		private const string ThumbCacheFilename = "thumb.jpg";
		/// <summary>
		/// The poster cache filename.
		/// </summary>
		private const string PosterCacheFilename = "poster.jpg";

		#endregion

		#region Fields

		/// <summary>
		/// The thumbnail.
		/// </summary>
		private ImageSource thumbnail;
		/// <summary>
		/// The poster.
		/// </summary>
		private ImageSource poster;
		/// <summary>
		/// The icon.
		/// </summary>
		private ImageSource icon;
		/// <summary>
		/// The savegames.
		/// </summary>
		private List<CartridgeSavegame> savegames;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Models.CartridgeTag"/> class from cartridge meta data.
		/// </summary>
		/// <param name="cart">Cartridge for this tag.</param>
		public CartridgeTag(Cartridge cart)
		{
			if (cart == null)
			{
				throw new ArgumentNullException("cart");
			}
			// Basic metadata.
			Cartridge = cart;
			// Add savegames
			this.ImportSavegames();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets the Cartridge object.
		/// </summary>The width of the small thumbnail minimum.
		public Cartridge Cartridge { get; private set; }
		/// <summary>
		/// Gets the cached thumbnail icon for the Cartridge.
		/// </summary>
		public ImageSource Thumbnail
		{
			get
			{
				return this.thumbnail;
			}
			private set
			{
				if (this.thumbnail != value)
				{
					this.thumbnail = value;
					this.RaisePropertyChanged("Thumbnail");
				}
			}
		}
		/// <summary>
		/// Gets the cached poster image for the Cartridge.
		/// </summary>
		public ImageSource Poster
		{
			get
			{
				return this.poster;
			}
			private set
			{
				if (this.poster != value)
				{
					this.poster = value;
					this.RaisePropertyChanged("Poster");
				}
			}
		}
		/// <summary>
		/// Gets the cached poster image for the Cartridge.
		/// </summary>
		public ImageSource Icon
		{
			get
			{
				return this.icon == null ? this.poster : this.icon;
			}
			private set
			{
				if (this.icon != value)
				{
					this.icon = value;
					this.RaisePropertyChanged("Icon");
				}
			}
		}
		/// <summary>
		/// Gets the available savegames for the cartridge.
		/// </summary>The width ofThe width of the small thumbnail minimum. the small thumbnail minimum.
		public IEnumerable<CartridgeSavegame> Savegames
		{
			get
			{
				return this.savegames ?? (this.savegames = new List<CartridgeSavegame>());
			}
		}
		// TODO
		// public IEnumerable<HistoryEntry> History
		// {
		// get
		// {
		// return App.Database.Table<HistoryEntry>().Where(he => he.Filename == Filename);
		// }
		// }
		/// <summary>
		/// Gets a value indicating whether this instance has a local gwc file.
		/// </summary>
		/// <value><c>true</c> if this instance is offline; otherwise, <c>false</c>.</value>
		public bool IsOffline
		{
			get
			{
				return Cartridge != null ? !string.IsNullOrEmpty(Cartridge.Filename) : false;
			}
		}
		/// <summary>
		/// Gets a value indicating whether this instance is only online and has no local gwc file.
		/// </summary>
		/// <value><c>true</c> if this instance is online; otherwise, <c>false</c>.</value>
		public bool IsOnline
		{
			get
			{
				return !this.IsOffline;
			}
		}
		public Color TextColor
		{
			get
			{
				return App.Colors.Text;
			}
		}

		#endregion

		#region Cartridge

		public async void Remove()
		{
			var cartridgeFile = Cartridge.Filename;
			var cartridgeSave = Cartridge.SaveFilename;
			var cartridgeLog = Cartridge.LogFilename;

            ExistenceCheckResult found;
            IFile file;

            found = await FileSystem.Current.LocalStorage.CheckExistsAsync(cartridgeLog);
			if (found == ExistenceCheckResult.FileExists)
			{
				file = await FileSystem.Current.LocalStorage.GetFileAsync(cartridgeLog);
                await file.DeleteAsync();

			}

            found = await FileSystem.Current.LocalStorage.CheckExistsAsync(cartridgeSave);
            if (found == ExistenceCheckResult.FileExists)
            {
                file = await FileSystem.Current.LocalStorage.GetFileAsync(cartridgeSave);
                await file.DeleteAsync();
            }

            found = await FileSystem.Current.LocalStorage.CheckExistsAsync(cartridgeFile);
            if (found == ExistenceCheckResult.FileExists)
            {
                file = await FileSystem.Current.LocalStorage.GetFileAsync(cartridgeFile);
                await file.DeleteAsync();
            }

			this.RaisePropertyChanged("Removed");
		}

		#endregion

		#region Savegames

		/// <summary>
		/// Exports a savegame to the isolated storage and adds it to this tag.
		/// </summary>
		/// <param name="cs">The savegame to add.</param>
		public void AddSavegame(CartridgeSavegame cs)
		{
			// Sanity check: a savegame with similar name should
			// not exist.
			CartridgeSavegame sameNameCS;
			if ((sameNameCS = this.Savegames.SingleOrDefault(c => c.Metadata != null && cs.Metadata != null && c.Metadata.SaveName == cs.Metadata.SaveName)) != null)
			{
				System.Diagnostics.Debug.WriteLine("CartridgeTag: Removing savegame to make room for new one: " + sameNameCS.Metadata.SaveName);
				// Removes the previous savegame that bears the same name.
				this.RemoveSavegame(sameNameCS);
			}
			// Adds the savegame.
			this.savegames.Add(cs);
			// Notifies of a change.
			this.RaisePropertyChanged("Savegames");
		}
		/// <summary>
		/// Removes a savegame's contents from the isolated storage and removes
		/// it from this tag.
		/// </summary>
		/// <param name="cs">Cartridge savegame to remove.</param>">
		public void RemoveSavegame(CartridgeSavegame cs)
		{
			// Removes the savegame.
			this.savegames.Remove(cs);
			// Makes sure the savegame is cleared from cache.
			cs.RemoveFromStore();
			// Notifies of a change.
			this.RaisePropertyChanged("Savegames");
		}
		/// <summary>
		/// Gets a savegame of this cartridge by name, or null if none
		/// is found.
		/// </summary>
		/// <param name="name">Name of the savegame to find.</param>
		/// <returns>The savegame, or null if it wasn't found.</returns>
		public CartridgeSavegame GetSavegameByNameOrDefault(string name)
		{
			return this.Savegames.SingleOrDefault(cs => cs.Metadata.SaveName == name);
		}

		#endregion

		#region Logs

		/// <summary>
		/// Creates a new log file for this cartridge tag.
		/// </summary>
		/// <returns>New log file for this cartridge.</returns>
		public GWL CreateLogFile()
		{
			// Creates a file in the logs folder.
			string filename = string.Format(
				"{0}.{1:yyyyMMddHHmmss}.gwl",
				Path.GetFileNameWithoutExtension(Cartridge.Filename),
				DateTime.Now.ToLocalTime());
            // Create log path
            var found = PCLStorage.FileSystem.Current.LocalStorage.CheckExistsAsync(App.PathForLogs);
            found.RunSynchronously();

			if (found.Result == ExistenceCheckResult.NotFound)
			{
                var dir = PCLStorage.FileSystem.Current.LocalStorage.CreateFolderAsync(App.PathForLogs, CreationCollisionOption.ReplaceExisting);
                dir.RunSynchronously();
			}
            // Creates a logger for this file.
            var openFile = PCLStorage.FileSystem.Current.LocalStorage.CreateFileAsync(Path.Combine(App.PathForLogs, filename), CreationCollisionOption.ReplaceExisting);
            openFile.RunSynchronously();
            var file = openFile.Result.OpenAsync(PCLStorage.FileAccess.ReadAndWrite);
            file.RunSynchronously();

            return new GWL(file.Result);
		}
		/// <summary>
		/// Removes the log file.
		/// </summary>
		/// <returns>Returns true, if the file was deleted.</returns>
		/// <param name="filename">Filename of log file to delete.</param>
		public bool RemoveLogFile(string filename)
		{
			// Get path for logs
			var dir = PCLStorage.FileSystem.Current.LocalStorage.GetFolderAsync(App.PathForLogs);
            dir.RunSynchronously();

            var files = dir.Result.GetFilesAsync();
            files.RunSynchronously();

            foreach(var file in files.Result)
            {
                if (file.Name.StartsWith(filename, StringComparison.OrdinalIgnoreCase))
                {
                    var deleteFile = PCLStorage.FileSystem.Current.LocalStorage.GetFileAsync(file.Name);
                    deleteFile.RunSynchronously();

                    deleteFile.Result.DeleteAsync().RunSynchronously();
                }
            }

            return files.Result != null;
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Imports the savegames.
		/// </summary>
		private async void ImportSavegames()
		{
			List<CartridgeSavegame> savegames = new List<CartridgeSavegame>();

			// Get path for savegames
			var dir = await PCLStorage.FileSystem.Current.GetFolderFromPathAsync(App.PathForSavegames);

			// Get cartridge filename without extension
			var cartFilename = Path.GetFileNameWithoutExtension(Cartridge.Filename);

            // Get all files, which start with the same string as cartridge filename and end with gws
            var files = await PCLStorage.FileSystem.Current.LocalStorage.GetFilesAsync();

			// For each file, imports its metadata.
			foreach (var file in files.Where<IFile>((f) => f.Name.StartsWith(cartFilename, StringComparison.OrdinalIgnoreCase) && f.Name.EndsWith(".gws", StringComparison.OrdinalIgnoreCase)))
			{
				try
				{
					var cs = CartridgeSavegame.FromStore(this, file.Name);
					// Only add savegame, if it belongs to this cartridge
					if (cs.Metadata.CartridgeName == Cartridge.Name && cs.Metadata.CartridgeCreateDate == Cartridge.CreateDate)
					{
						savegames.Add(cs);
					}
				}
				catch (Exception ex)
				{
					// Outputs the exception.
					System.Diagnostics.Debug.WriteLine("CartridgeTag: WARNING: Exception during savegame import.");
					// DebugUtils.DumpException(ex);
				}
			}
			// Sets the savegame list.
			this.savegames = savegames;
			this.RaisePropertyChanged("Savegames");
		}

		#endregion

		/// <summary>
		/// Raises the property changed.
		/// </summary>
		/// <param name="propName">Property name.</param>
		private void RaisePropertyChanged(string propName)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					if (PropertyChanged != null)
					{
						PropertyChanged(this, new PropertyChangedEventArgs(propName));
					}
				});
		}
	}
}