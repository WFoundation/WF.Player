// <copyright file="ProgressAggregator.cs" company="Wherigo Foundation">
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

namespace WF.Player.Utils
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	/// <summary>
	/// A thread-safe aggregator for various sources of progress that deliver an overview
	/// of progress.
	/// </summary>
	public class ProgressAggregator : INotifyPropertyChanged
	{
		#region Fields

		/// <summary>
		/// The indeterminate progresses.
		/// </summary>
		private Dictionary<object, bool> indeterminateProgresses = new Dictionary<object, bool>();

		/// <summary>
		/// The working sources queue.
		/// </summary>
		private List<object> workingSourcesQueue = new List<object>();

		/// <summary>
		/// The sync root.
		/// </summary>
		private object syncRoot = new object();

		#endregion

		#region Events

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets a value indicating whether this aggregator has any working source of progress.
		/// </summary>
		public bool HasWorkingSource
		{
			get
			{
				lock (this.syncRoot)
				{
					return this.workingSourcesQueue.Count > 0;
				}
			}
		}

		/// <summary>
		/// Gets the working source that is currently at the top of the queue.
		/// </summary>
		public object FirstWorkingSource
		{
			get
			{
				return this.workingSourcesQueue.FirstOrDefault();
			}
		}

		#endregion

		#region Indexers

		/// <summary>
		/// Gets or sets if a source of progress is active or not.
		/// </summary>
		/// <param name="key">The key corresponding to the source of progress.</param>
		/// <returns>True if the source of progress is known and is active,
		/// false if it is not known or is not active.</returns>
		public bool this[object key]
		{
			get
			{
				// Unknown sources of progress return false.
				bool value = false;
				bool hasValue = false;

				lock (this.syncRoot)
				{
					hasValue = this.indeterminateProgresses.TryGetValue(key, out value);
				}

				return hasValue && value;
			}

			set
			{
				// Only keeps a reference to active sources of progress.
				if (value)
				{
					lock (this.syncRoot)
					{
						this.indeterminateProgresses[key] = value; 
					}

					this.EnsureWorkingSource(key);
				}
				else if (this.indeterminateProgresses.ContainsKey(key))
				{
					lock (this.syncRoot)
					{
						this.indeterminateProgresses.Remove(key); 
					}

					this.RemoveWorkingSource(key);
				}
			}
		}

		#endregion

		/// <summary>
		/// Raises the property changed.
		/// </summary>
		/// <param name="propName">Property name.</param>
		private void RaisePropertyChanged(string propName)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}

		/// <summary>
		/// Removes the working source.
		/// </summary>
		/// <param name="key">Key to use.</param>
		private void RemoveWorkingSource(object key)
		{
			// Removes the source and determines if the top changed.
			object currentTop;
			object newTop;
			lock (this.syncRoot)
			{
				currentTop = this.workingSourcesQueue.FirstOrDefault();
				this.workingSourcesQueue.Remove(key);
				newTop = this.workingSourcesQueue.FirstOrDefault(); 
			}

			// Raises an event if the top changed.
			if (currentTop != newTop)
			{
				this.RaisePropertyChanged("FirstWorkingSource");

				if (currentTop == null || newTop == null)
				{
					this.RaisePropertyChanged("HasWorkingSource");
				}
			}
		}

		/// <summary>
		/// Ensures the working source.
		/// </summary>
		/// <param name="key">Key to use.</param>
		private void EnsureWorkingSource(object key)
		{
			// Makes sure the key is only once in the list, and on top.
			bool hadSource;

			lock (this.syncRoot)
			{
				hadSource = this.workingSourcesQueue.Count > 0;
				int index = this.workingSourcesQueue.IndexOf(key);
				if (index == 0)
				{
					// The working source is already top. Nothing to do.
					return;
				}

				if (index > 0)
				{
					this.workingSourcesQueue.Remove(key);
				}

				this.workingSourcesQueue.Add(key);
			}

			// Raises events.
			this.RaisePropertyChanged("FirstWorkingSource");
			if (!hadSource)
			{
				this.RaisePropertyChanged("HasWorkingSource");
			}
		}
	}
}
