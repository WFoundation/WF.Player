// <copyright file="BasePage.cs" company="Wherigo Foundation">
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
	using Xamarin.Forms;

	/// <summary>
	/// Base page.
	/// </summary>
	public class BasePage : ContentPage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.BasePage"/> class.
		/// </summary>
		public BasePage()
		{
			// Set an empty icon as home icon
			NavigationPage.SetTitleIcon(this, "HomeIcon.png");

			// Set background color to default value
			BackgroundColor = App.Colors.Background;
		}

		#endregion

		#region Properties

		#endregion

		#region Events

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (BindingContext is BaseViewModel)
			{
				((BaseViewModel)BindingContext).OnAppearing();
			}
		}

		/// <summary>
		/// Raises the appeared event.
		/// </summary>
		public void OnAppeared()
		{
			IsBusy = false;

			if (BindingContext is BaseViewModel)
			{
				((BaseViewModel)BindingContext).OnAppeared();
			}
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		protected override void OnDisappearing()
		{
			IsBusy = false;

			base.OnDisappearing();

			if (BindingContext is BaseViewModel)
			{
				((BaseViewModel)BindingContext).OnDisappearing();
			}
		}

		#endregion
	}
}
