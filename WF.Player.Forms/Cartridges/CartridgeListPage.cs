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
using WF.Player.Services.Settings;
using System.IO;
using WF.Player.Services.UserDialogs;
using WF.Player.Core;
using System;
using System.Linq;

namespace WF.Player
{
	using System.ComponentModel;
	using System.Threading.Tasks;
	using Vernacular;
	using WF.Player.Controls;
	using WF.Player.Models;
	using WF.Player.Services.Device;
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
		private ListView list;

		/// <summary>
		/// The refresh command.
		/// </summary>
		private Command refreshCommand;

		/// <summary>
		/// The flag for is busy.
		/// </summary>
		private bool isBusy;

		/// <summary>
		/// The sync flag for cartridges.
		/// </summary>
		private object syncCartridges = new object();

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeListPage"/> class.
		/// </summary>
		/// <param name="cartridges">Cartridges to show.</param>
		public CartridgeListPage(CartridgeStore store)
		{
			this.cartridges = store;

			this.cartridges.CollectionChanged += HandleCartridgesCollectionChanged;

			// If the store is empty, than update it
			if (this.cartridges.Count == 0)
			{
				UpdateCartridges();
			}

			this.BindingContext = this;

			Title = Catalog.GetString("Cartridges");

			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			// Only show settings, if device is Android
			#if __ANDROID__

			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Settings"), null, () =>
				{
					App.Navigation.Popped += HandleSettingsClosed;
					App.Navigation.Navigation.PushAsync(new SettingsPage.SettingsPage());
				}, ToolbarItemOrder.Secondary));
			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("About"), null, () =>
				{
					App.Navigation.Navigation.PushAsync(new SettingsPage.AboutPage());
				}, ToolbarItemOrder.Secondary));
			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Feedback"), null, () =>
				HockeyApp.FeedbackManager.ShowFeedbackActivity(Forms.Context), 
				ToolbarItemOrder.Secondary));
			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("Quit"), null, () =>
				{
					DependencyService.Get<IExit>().ExitApp(0);
				}, ToolbarItemOrder.Secondary));

			#endif

			#if __IOS__

			var toolbarMenu = new ToolbarItem(Catalog.GetString("Menu"), null, () => { //"IconMenu.png", () => {
				App.Click();
				App.Navigation.Popped += HandleSettingsClosed;
				var cfg = new WF.Player.Services.UserDialogs.ActionSheetConfig().SetTitle(Catalog.GetString("Main Menu"));
				cfg.Add(Catalog.GetString("Settings"), () => App.Navigation.Navigation.PushAsync(new SettingsPage.SettingsPage()));
				cfg.Add(Catalog.GetString("About"), () => App.Navigation.Navigation.PushAsync(new SettingsPage.AboutPage()));
				cfg.Add(Catalog.GetString("Feedback"), () => HockeyApp.BITHockeyManager.SharedHockeyManager.FeedbackManager.ShowFeedbackListView());
				cfg.Cancel = new WF.Player.Services.UserDialogs.ActionSheetOption(Catalog.GetString("Cancel"), App.Click);
				UserDialogs.Instance.ActionSheet(cfg);
			});
			this.ToolbarItems.Add (toolbarMenu);

			#endif

			layout = new StackLayout() 
				{
					BackgroundColor = App.Colors.Background,
					VerticalOptions = LayoutOptions.FillAndExpand,
				};

			var listCellHeight = (int)(((DependencyService.Get<IScreen>().Width * 0.25) - 20) * 1.25 + 30);

			list = new ListView() 
				{
					BackgroundColor = App.Colors.Background,
					RowHeight = Device.OnPlatform<int>(110, 120, 120),
					HasUnevenRows = false,
					SeparatorColor = App.Colors.SeparatorLine,
					ItemsSource = cartridges,
					ItemTemplate = new DataTemplate(typeof(CartridgeListCell)),
					IsPullToRefreshEnabled = true,
					RefreshCommand = this.RefreshCommand,
			};

//			list.SetBinding<CartridgeListPage> (ListView.IsRefreshingProperty, vm => vm.IsBusy);
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
				return refreshCommand ?? (refreshCommand = new Command(ExecuteRefreshCommand)); 
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

			// Check for autosave file
			var gwsFilename = Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWSKey);
			var gwcFilename = Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWCKey);

			if (!string.IsNullOrEmpty(gwsFilename) && !string.IsNullOrEmpty(gwcFilename))
			{
				HandleAutosave();
			}
			else
			{
				// If an auto save file exists, delete it
				var filename = Path.Combine(App.PathForSavegames, "autosave.gws");

				if (File.Exists(filename))
				{
					File.Delete(filename);
				}
			}
		}

		/// <summary>
		/// Handle back button pressed event on devices, that have one.
		/// </summary>
		/// <returns>True, because the back button pressed event is handled.</returns>
		/// <remarks>This is the first page, so exit app when going back.</remarks>
		protected override bool OnBackButtonPressed()
		{
			DependencyService.Get<IExit>().ExitApp(0);

			return true;
		}

		#endregion

		#region Private Methods

		private static async void HandleAutosave()
		{
			var gwcFilename = Path.Combine(App.PathForCartridges, Path.GetFileName(Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWCKey)));
			var gwsFilename = Path.Combine(App.PathForSavegames, Path.GetFileName(Settings.Current.GetValueOrDefault<string>(Settings.AutosaveGWSKey)));

			if (!File.Exists(gwsFilename) || !File.Exists(gwcFilename))
			{
				// Remove settings
				Settings.Current.Remove(Settings.AutosaveGWCKey);
				Settings.Current.Remove(Settings.AutosaveGWSKey);

				return;
			}

			bool result = await UserDialogs.Instance.ConfirmAsync(Catalog.GetString("There is an automatic savefile from a cartridge you played before. Would you resume this last game?"), Catalog.GetString("Automatical savefile"), Catalog.GetString("Yes"), Catalog.GetString("No"));

			if (result)
			{
				var cartridge = new Cartridge(gwcFilename);
				var cartridgeTag = new CartridgeTag(cartridge);
				var cartridgeSavegame = CartridgeSavegame.FromStore(cartridgeTag, gwsFilename);

				// We have a autosave file, so start this cartridge

				// Create a new navigation page for the game
				App.GameNavigation = new ExtendedNavigationPage(new GameCheckLocationView(new GameCheckLocationViewModel(cartridgeTag, cartridgeSavegame, App.Navigation.CurrentPage)), false) {
					BarBackgroundColor = App.Colors.Bar,
					BarTextColor = App.Colors.BarText,
					ShowBackButton = true,
				};
				App.Navigation.CurrentPage.Navigation.PushModalAsync(App.GameNavigation);

				App.GameNavigation.ShowBackButton = true;
			}
			else
			{
				// Remove file from directory
				File.Delete(gwsFilename);
			}

			// Remove settings
			Settings.Current.Remove(Settings.AutosaveGWCKey);
			Settings.Current.Remove(Settings.AutosaveGWSKey);
		}

		public void HandleSettingsClosed(object sender, EventArgs args)
		{
			if (sender is ExtendedNavigationPage && ((ExtendedNavigationPage)sender).CurrentPage is CartridgeListPage)
			{
				App.Navigation.Popped -= HandleSettingsClosed;

				App.Navigation.BackgroundColor = App.Colors.Background;
				App.Navigation.BarTextColor = App.Colors.BarText;
				App.Navigation.BarBackgroundColor = App.Colors.Bar;

				layout.BackgroundColor = App.Colors.Background;
				list.BackgroundColor = App.Colors.Background;

				// Update language
				DependencyService.Get<ILanguageSetter>().Update();

				// Update all texts
				Title = Catalog.GetString("Cartridges");

				#if __ANDROID__

				this.ToolbarItems[0].Text = Catalog.GetString("Settings");
				this.ToolbarItems[1].Text = Catalog.GetString("About");
				this.ToolbarItems[2].Text = Catalog.GetString("Feedback");
				this.ToolbarItems[3].Text = Catalog.GetString("Quit");

				#endif

				#if __IOS__

				this.ToolbarItems[0].Text = Catalog.GetString("Menu");

				#endif

				UpdateCartridges();
			}
		}

		private void HandleCartridgesCollectionChanged (object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
			{
				cartridges.Remove((CartridgeTag)e.OldItems[0]);
			}
		}

		private void UpdateCartridges()
		{
			lock (syncCartridges)
			{
				cartridges.Clear();
				cartridges.SyncFromStore();
			}
		}

		private void ExecuteRefreshCommand()
		{
			if (IsBusy)
			{
				list.IsRefreshing = false;
				return;
			}

			IsBusy = true;

			UpdateCartridges();

			IsBusy = false;

			list.EndRefresh();
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
