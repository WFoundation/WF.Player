// <copyright file="BaseViewModel.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Runtime.CompilerServices;
	using WF.Player.Data;

	/// <summary>
	/// View model base class.
	/// </summary>
	/// <example>
	/// To implement observable property:
	/// private object propertyBackField;
	/// public object Property
	/// {
	/// get { return this.propertyBackField; }
	/// set
	/// {
	/// this.ChangeAndNotify(ref this.propertyBackField, value);
	/// }
	/// </example>
	public abstract class BaseViewModel : ObservableObject
	{
		/// <summary>
		/// The is busy.
		/// </summary>
		private bool isBusy;

		#region Events

		/// <summary>
		/// Occurs when property is changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets a value indicating whether this instance is busy.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance is busy; otherwise, <c>false</c>.
		/// </value>
		public bool IsBusy
		{
			get 
			{ 
				return isBusy; 
			}

			set
			{
				SetProperty<bool>(ref isBusy, value);
			}
		}

		#endregion

		#region Events

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		public virtual void OnAppearing ()
		{
		}

		/// <summary>
		/// Raises the appeared event.
		/// </summary>
		public virtual void OnAppeared ()
		{
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		public virtual void OnDisappearing ()
		{
		}

		#endregion

		#region Methods

		/// <summary>
		/// Unbind all handlers from property changed event.
		/// </summary>
		public void Unbind()
		{
			this.PropertyChanged = null;
		}

		#endregion
	}
}
