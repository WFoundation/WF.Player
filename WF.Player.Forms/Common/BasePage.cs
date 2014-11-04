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
	using System;
	using Xamarin.Forms;

	/// <summary>
	/// Base page.
	/// </summary>
	public class BasePage : ContentPage
	{
		/// <summary>
		/// Bindable property for back button.
		/// </summary>
		public static readonly BindableProperty HasBackButtonProperty = BindableProperty.Create<BasePage, bool>(p => p.HasBackButton, true);

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.BasePage"/> class.
		/// </summary>
		public BasePage()
		{
			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			BackgroundColor = App.Colors.Background;
		}

		#endregion

		#region Properties

		#region HasBackButton

		/// <summary>
		/// Gets or sets a value indicating whether this instance has back button.
		/// </summary>
		/// <value><c>true</c> if this instance has back button; otherwise, <c>false</c>.</value>
		public bool HasBackButton
		{
			get
			{
				return (bool)GetValue(HasBackButtonProperty);
			}

			set
			{
				SetValue(HasBackButtonProperty, value);
			}
		}

		#endregion

		#endregion

		#region Events

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing()
		{
			App.CurrentPage = this;

			base.OnAppearing();

			IsBusy = false;

			if (BindingContext is BaseViewModel)
			{
				((BaseViewModel)BindingContext).OnAppearing();
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
