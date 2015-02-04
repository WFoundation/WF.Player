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
using WF.Player.Controls;
using System.Threading.Tasks;
using System.ComponentModel;
using WF.Player.Services.Settings;

namespace WF.Player
{
	using Vernacular;
	using WF.Player.Models;
	using Xamarin.Forms;

	/// <summary>
	/// Cartridge list page.
	/// </summary>
	public class CartridgeListPage : ContentPage, INotifyPropertyChanged
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
		private PullToRefreshListView list;

		/// <summary>
		/// The refresh command.
		/// </summary>
		private Command refreshCommand;

		/// <summary>
		/// The flag for is busy.
		/// </summary>
		private bool isBusy;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeListPage"/> class.
		/// </summary>
		/// <param name="cartridges">Cartridges to show.</param>
		public CartridgeListPage(CartridgeStore store)
		{
			this.cartridges = store;
			this.BindingContext = this;

			Title = Catalog.GetString("Cartridges");

			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			// Only show settings, if device is Android
			#if __ANDROID__

			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Settings"), null, () =>
				{
					DependencyService.Get<ISettingsView>().Show();
				}, ToolbarItemOrder.Secondary));
			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Feedback"), null, () =>
				HockeyApp.FeedbackManager.ShowFeedbackActivity(Forms.Context), 
				ToolbarItemOrder.Secondary));
			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Quit"), null, () =>
				{
					((WF.Player.Droid.MainActivity)Forms.Context).Exit(0);
				}, ToolbarItemOrder.Secondary));

			#endif

			#if __IOS__

//			var toolbarMenu = new ToolbarItem(Catalog.GetString("Menu"), null, () => { //"IconMenu.png", () => {
//				App.Click();
//				var cfg = new WF.Player.Services.UserDialogs.ActionSheetConfig().SetTitle(Catalog.GetString("Main Menu"));
//				cfg.Add(Catalog.GetString("Feedback"), () => HockeyApp.BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackListView());
//				cfg.Cancel = new WF.Player.Services.UserDialogs.ActionSheetOption(Catalog.GetString("Cancel"), App.Click);
//				DependencyService.Get<WF.Player.Services.UserDialogs.IUserDialogService>().ActionSheet(cfg);
//			});
//			this.ToolbarItems.Add (toolbarMenu);

			#endif

			layout = new StackLayout() 
			{
				BackgroundColor = Color.White,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			list = new PullToRefreshListView() 
			{
				BackgroundColor = App.Colors.Background,
				RowHeight = Device.OnPlatform<int>(104, 60, 60),
				ItemsSource = cartridges,
				ItemTemplate = new DataTemplate(typeof(TextCell)),
				Message = Catalog.GetString("Loading..."),
				RefreshCommand = this.RefreshCommand,
			};
			list.SetBinding<CartridgeListPage> (PullToRefreshListView.IsRefreshingProperty, vm => vm.IsBusy);

			list.ItemTemplate.SetBinding(TextCell.TextProperty, CartridgeStore.CartridgeNamePropertyName);
			list.ItemTemplate.SetBinding(TextCell.TextColorProperty, "TextColor");
			list.ItemTemplate.SetBinding(TextCell.DetailProperty, CartridgeStore.CartridgeAuthorNamePropertyName);
			list.ItemSelected += async (sender, e) =>
			{
				if (e.SelectedItem == null)
				{
					return;
				}

				App.Click();

				var cartridgeTag = (CartridgeTag)e.SelectedItem;
				var cartridgeDetailPage = new CartridgeDetailPage(new CartridgeDetailViewModel(cartridgeTag));

				await App.Navigation.PushAsync(cartridgeDetailPage, true);

				// Clear selection, so it is possible later select the same item
				list.SelectedItem = null;
			};

			layout.Children.Add(list);

			Content = layout;
		}

		#endregion

		#region Properties

		public bool IsBusy
		{
			get 
			{ 
				return isBusy; 
			}

			set
			{
				if (isBusy == value)
				{
					return;
				}

				isBusy = value;
				list.IsRefreshing = value;
				HandlePropertyChanged ("IsBusy");
			}
		}

		#endregion

		#region Commands

		public Command RefreshCommand
		{
			get 
			{ 
				return refreshCommand ?? (refreshCommand = new Command (async ()=> await ExecuteRefreshCommand())); 
			}
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

		#region Private Functions

		private async Task ExecuteRefreshCommand()
		{
			if (IsBusy)
			{
				return;
			}

			IsBusy = true;
			cartridges.Clear();
			cartridges.SyncFromStore();

			IsBusy = false;
		}

		#endregion

		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		public void HandlePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;

			if (handler != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

	} 
}
