// <copyright file="CartridgeTag.cs" company="Wherigo Foundation">
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
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.IO;
	using System.Linq;
	using WF.Player.Core;
	using WF.Player.Core.Formats;
	using Xamarin.Forms;

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
		//		public IEnumerable<HistoryEntry> History
		//		{
		//			get
		//			{
		//				return App.Database.Table<HistoryEntry>().Where(he => he.Filename == Filename);
		//			}
		//		}

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

			// Creates a logger for this file.
			return new GWL(new FileStream(Path.Combine(App.PathForLogs, filename), FileMode.Create));
		}

		/// <summary>
		/// Removes the log file.
		/// </summary>
		/// <returns>Returns true, if the file was deleted.</returns>
		/// <param name="filename">Filename of log file to delete.</param>
		public bool RemoveLogFile(string filename)
		{
			// Get path for savegames
			var dir = new Acr.XamForms.Mobile.IO.Directory(App.PathForLogs);

			var file = dir.Files.SingleOrDefault((f) => f.Name.StartsWith(filename));

			if (file != null)
			{
				file.Delete();
				return true;
			}

			return false;
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Imports the savegames.
		/// </summary>
		private void ImportSavegames()
		{
			List<CartridgeSavegame> savegames = new List<CartridgeSavegame>();

			// Get path for savegames
			var dir = new Acr.XamForms.Mobile.IO.Directory(App.PathForSavegames);

			// Get cartridge filename without extension
			var cartFilename = Path.GetFileNameWithoutExtension(Cartridge.Filename);

			// Get all files, which start with the same string as cartridge filename and end with gws
			var files = dir.Files.Where((f) => f.FullName.StartsWith(cartFilename, StringComparison.InvariantCultureIgnoreCase) && f.FullName.EndsWith(".gws", StringComparison.InvariantCultureIgnoreCase));

			// For each file, imports its metadata.
			foreach (var file in files)
			{
				try
				{
					var cs = CartridgeSavegame.FromStore(this, file.FullName);

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
