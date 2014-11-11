// <copyright file="CartridgeListPage.cs" company="Wherigo Foundation">
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
	using Vernacular;
	using WF.Player.Models;
	using Xamarin.Forms;

	/// <summary>
	/// Cartridge list page.
	/// </summary>
	public class CartridgeListPage : ContentPage
	{
		/// <summary>
		/// The cartridges.
		/// </summary>
		private CartridgeStore cartridges;

		/// <summary>
		/// The layout.
		/// </summary>
		private StackLayout layout;

		/// <summary>
		/// The list.
		/// </summary>
		private ListView list;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeListPage"/> class.
		/// </summary>
		/// <param name="cartridges">Cartridges to show.</param>
		public CartridgeListPage(CartridgeStore store)
		{
			this.cartridges = store;

			Title = Catalog.GetString("Cartridges");
			NavigationPage.SetTitleIcon(this, "HomeIcon.png");

			// Only show settings, if device is Android
			#if __ANDROID__
			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Settings"), null, () =>
					{
						DependencyService.Get<IPreferencesView>().Show();
					}, ToolbarItemOrder.Secondary));
			#endif

			layout = new StackLayout() 
			{
				BackgroundColor = Color.White,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			list = new ListView() 
			{
				BackgroundColor = App.Colors.Background,
				RowHeight = Device.OnPlatform<int>(104, 60, 60),
				ItemsSource = cartridges,
				ItemTemplate = new DataTemplate(typeof(TextCell)),
			};

			list.ItemTemplate.SetBinding(TextCell.TextProperty, CartridgeStore.CartridgeNamePropertyName);
			list.ItemTemplate.SetBinding(TextCell.TextColorProperty, "TextColor");
			list.ItemTemplate.SetBinding(TextCell.DetailProperty, CartridgeStore.CartridgeAuthorNamePropertyName);
			list.ItemSelected += (sender, e) =>
			{
				if (e.SelectedItem == null)
				{
					return;
				}

				App.Click();

				var cartridgeTag = (CartridgeTag)e.SelectedItem;
				var cartridgeDetailPage = new CartridgeDetailPage(new CartridgeDetailViewModel(cartridgeTag));

				App.Navigation.PushAsync(cartridgeDetailPage);

				// Clear selection, so it is possible later select the same item
				list.SelectedItem = null;
			};

			layout.Children.Add(list);

			Content = layout;
		}

		#endregion

		#region Events

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		#endregion
	} 
}
