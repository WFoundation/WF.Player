// <copyright file="CartridgeSavegame.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
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
    using Common;
    using System;
    using System.IO;
    using System.Runtime.Serialization;
    using System.Threading.Tasks;
    using WF.Player.Core;
    using WF.Player.Core.Formats;

    /// <summary>
    /// Provides a static metadata description of a Cartridge savegame.
    /// </summary>
    public class CartridgeSavegame
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Models.CartridgeSavegame"/> class.
		/// </summary>
		/// <param name="tag">Tag of cartridge.</param>
		/// <param name="gwsFilename">Gws filename.</param>
		public CartridgeSavegame(CartridgeTag tag, string gwsFilename)
		{
            OpenSavegame(tag, gwsFilename);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Models.CartridgeSavegame"/> class.
		/// </summary>
		internal CartridgeSavegame()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Models.CartridgeSavegame"/> class.
		/// </summary>
		/// <param name="tag">Tag of cartridge.</param>
		internal CartridgeSavegame(CartridgeTag tag)
		{
			this.Tag = tag;
			this.Filename = this.CreateSavegameFilename(tag);
			this.Metadata = null;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the tag.
		/// </summary>
		/// <value>The tag.</value>
		public CartridgeTag Tag { get; internal set; }

		/// <summary>
		/// Gets the file path of the savegame.
		/// </summary>
		public string Filename { get; private set; }

		/// <summary>
		/// Gets the metadata.
		/// </summary>
		/// <value>The metadata.</value>
		public GWS.Metadata Metadata { get; private set; }

		/// <summary>
		/// Gets or sets a value indicating whether this savegame has been made automatically.
		/// </summary>
		public bool IsAutosave { get; set; }

		#endregion

		#region Static Functions

		/// <summary>
		/// Froms the store.
		/// </summary>
		/// <returns>The store.</returns>
		/// <param name="tag">Tag of cartridge.</param>
		/// <param name="filename">Name of file.</param>
		public static CartridgeSavegame FromStore(CartridgeTag tag, string filename)
		{
			return new CartridgeSavegame(tag, filename);
		}

		#endregion

		/// <summary>
		/// Creates or replaces the underlying savegame file and opens a writing
		/// stream for it.
		/// </summary>
		/// <returns>The stream to write the savegame on.</returns>
		public async Task<Stream> CreateOrReplace()
		{
            return await Storage.Current.GetStreamForWriting(Storage.Current.GetFullnameForSavegame(this.Filename), false);
		}

		/// <summary>
		/// Removes this savegame's files from the storage.
		/// </summary>
		public async void RemoveFromStore()
		{
            Storage.Current.RemoveFile(Storage.Current.GetFullnameForSavegame(this.Filename));
		}

        /// <summary>
        /// Opens the savegame and load the metadata
        /// </summary>
        /// <param name="tag">Tag of the cartridge</param>
        /// <param name="gwsFilename">Filename of the GWS file</param>
        private async void OpenSavegame(CartridgeTag tag, string gwsFilename)
        {
            var openFile = await PCLStorage.FileSystem.Current.LocalStorage.CreateFileAsync(gwsFilename, PCLStorage.CreationCollisionOption.OpenIfExists);
            var file = await openFile.OpenAsync(PCLStorage.FileAccess.Read);

            this.Tag = tag;
            this.Filename = gwsFilename;
            this.Metadata = GWS.LoadMetadata(file);
        }

        /// <summary>
        /// Creates the savegame filename.
        /// </summary>
        /// <returns>The savegame filename.</returns>
        /// <param name="tag">Tag of cartridge.</param>
        private string CreateSavegameFilename(CartridgeTag tag)
		{
			// TODO
			// If cartridge don't allow multiple save files, than return default name
			if (true)
			{
				return string.Format(
					"{0}.gws",
					Storage.Current.GetFullnameForSavegame(Path.GetFileNameWithoutExtension(tag.Cartridge.Filename)));
			}
			else
			{
				return string.Format(
					"{0}.{1}.gws",
					Storage.Current.GetFullnameForSavegame(Path.GetFileNameWithoutExtension(tag.Cartridge.Filename)),
					DateTime.Now.ToLocalTime().ToString("yyyyMMddHHmmss"));
			}
		}
	}
}
