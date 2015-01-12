// <copyright file="ExtendedNavigationPage.cs" company="Wherigo Foundation">
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
using System;
using System.ComponentModel;

namespace WF.Player.Controls
{
	using System.Collections;
	using Xamarin.Forms;

	/// <summary>
	/// Extended navigation page.
	/// </summary>
	public class ExtendedNavigationPage : NavigationPage
	{
		/// <summary>
		/// The stack.
		/// </summary>
		private Stack stack;

		/// <summary>
		/// The transition.
		/// </summary>
		private bool transition;

		/// <summary>
		/// The sync root.
		/// </summary>
		private object syncRoot = new object();

		private bool showBackButton;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Controls.ExtendedNavigationPage"/> class.
		/// </summary>
		/// <param name="animation">If set to <c>true</c> animation.</param>
		public ExtendedNavigationPage(bool animation = true) : this(null, animation)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Controls.ExtendedNavigationPage"/> class.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="animation">If set to <c>true</c> animation.</param>
		public ExtendedNavigationPage(Page page, bool animation = true) : base(page)
		{
			stack = new Stack(8);

			Animation = animation;
			showBackButton = false;

			this.Popped += HandlePopped;
			this.PoppedToRoot += HandlePoppedToRoot;
			this.Pushed += HandlePushed;

			if (page != null)
			{
				lock (syncRoot)
				{
					stack.Push(page);
				}
			}
		}

		#endregion

		#region Properties

		public bool Animation { get; private set; }

		public bool ShowBackButton
		{ 
			get
			{
				return showBackButton;
			}

			set
			{
				showBackButton = value;

				Device.BeginInvokeOnMainThread(() => OnPropertyChanged("ShowBackButton"));
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="WF.Player.ExtendedNavigationPage"/> is transition.
		/// </summary>
		/// <value><c>true</c> if transition; otherwise, <c>false</c>.</value>
		public bool Transition
		{
			get
			{
				return transition;
			}
		}

		#endregion

		#region INavigation

		/// <summary>
		/// Pushs the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="page">Page to push.</param>
		public new System.Threading.Tasks.Task PushAsync(Page page)
		{
			lock (syncRoot)
			{
				transition = true;
				stack.Push(page);
			}

			System.Threading.Tasks.Task result = null;

			Device.BeginInvokeOnMainThread(async () =>
				{
					await base.Navigation.PushAsync(page, false);
				});

			return result;
		}

		/// <summary>
		/// Pops to root async.
		/// </summary>
		/// <returns>The to root async.</returns>
		public new System.Threading.Tasks.Task PopToRootAsync()
		{
			lock (syncRoot)
			{
				transition = true;

				while (stack.Count > 1)
				{
					stack.Pop();
				}
			}

			System.Threading.Tasks.Task result = null;

			Device.BeginInvokeOnMainThread(async () =>
				{
					lock (syncRoot)
					{
						transition = false;
					}
					await base.Navigation.PopToRootAsync(false);
				});

			return result;
		}

		/// <summary>
		/// Pops the async.
		/// </summary>
		/// <returns>The async.</returns>
		public new System.Threading.Tasks.Task PopAsync()
		{
			// First page must always on screen
			if (stack.Count == 1)
			{
				lock (syncRoot)
				{
					transition = false;
				}
				return null;
			}

			lock (syncRoot)
			{
				transition = true;
				stack.Pop();
			}

			System.Threading.Tasks.Task result = null;

			Device.BeginInvokeOnMainThread(async () =>
				{
					await base.Navigation.PopAsync(false);
				});

			return result;
		}

		/// <summary>
		/// Pushs the modal async.
		/// </summary>
		/// <returns>The modal async.</returns>
		/// <param name="page">Page to push.</param>
		public System.Threading.Tasks.Task PushModalAsync(Page page)
		{
			return this.Navigation.PushModalAsync(page, false);
		}

		/// <summary>
		/// Pops the modal async.
		/// </summary>
		/// <returns>The modal async.</returns>
		public System.Threading.Tasks.Task<Page> PopModalAsync()
		{
			return this.Navigation.PopModalAsync(false);
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles the pushed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Navigation event arguments.</param>
		private void HandlePushed(object sender, NavigationEventArgs e)
		{
			lock (syncRoot)
			{
				transition = false;
			}
		}

		/// <summary>
		/// Handles the popped.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Navigation event arguments.</param>
		private void HandlePopped(object sender, NavigationEventArgs e)
		{
			lock (syncRoot)
			{
				transition = false;
			}
		}

		/// <summary>
		/// Handles the popped to root.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Navigation event arguments.</param>
		private void HandlePoppedToRoot(object sender, NavigationEventArgs e)
		{
			lock (syncRoot)
			{
				transition = false;
			}
		}

		#endregion
	}
}
