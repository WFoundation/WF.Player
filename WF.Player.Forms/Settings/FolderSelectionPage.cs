// <copyright file="CartridgeFolderSelection.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
// </copyright>
//
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
//
using System;
using Xamarin.Forms;
using Vernacular;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using WF.Player.Services.Settings;
using WF.Player.Services.UserDialogs;
using WF.Player.Models;

namespace WF.Player.SettingsPage
{
	public class FolderSelectionPage : ContentPage
	{
		private string path;
		private Action updateAction;
		private readonly ListView list;

		private class PathItem
		{
			public PathItem(string name, string path)
			{
				Name = name;
				Path = path;
			}

			public string Name { get; set; }
			public string Path { get; set; }
		}

		public FolderSelectionPage(string path, Action updateAction, Color textColor, Color backgroundColor) : base()
		{
			this.path = path;
			this.updateAction = updateAction;

			Title = path;

			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("New folder"), null, () =>
				{
					App.Click();
					var cfg = new PromptConfig();
					cfg.Message = Catalog.GetString("New folder name");
					cfg.Title = Catalog.GetString("New folder");
					cfg.OnResult = (result) =>
						{
							Device.BeginInvokeOnMainThread(async () => 
								{
									App.Click();
									if (result.Ok)
									{
                                        var dir = await PCLStorage.FileSystem.Current.LocalStorage.CreateFolderAsync(Path.Combine(path, result.Text), PCLStorage.CreationCollisionOption.OpenIfExists);
										this.path = Path.Combine(path, result.Text);
										UpdateDirectories();
									}
								});
						};
					UserDialogs.Instance.Prompt(cfg);
				}, ToolbarItemOrder.Secondary));

			var cell = new DataTemplate(typeof(TextCell));
			cell.SetValue(TextCell.TextColorProperty, textColor);
			cell.SetBinding(TextCell.TextProperty, "Name");

			list = new ListView() {
				BackgroundColor = backgroundColor,
				RowHeight = 50,
				HasUnevenRows = false,
				ItemTemplate = cell,
				ItemsSource = new PathItem[] {},
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			list.ItemTapped += (object sender, ItemTappedEventArgs e) => {
				var pathItem = ((PathItem)e.Item);
				this.path = pathItem.Path;

				Settings.Current.AddOrUpdateValue<string>(Settings.CartridgePathKey, this.path);

				if(updateAction != null)
				{
					updateAction();
				}

				UpdateDirectories();
			};

//			((StackLayout)ContentLayout).Children.Add(list);
			Content = list;

			UpdateDirectories();
		}

//		protected override bool OnBackButtonPressed()
//		{
//			App.Navigation.Navigation.PopModalAsync();
//
//			return true;
//		}

//		private void SelectCommand()
//		{
//			Settings.Current.AddOrUpdateValue<string>(Settings.CartridgePathKey, path);
//
//			if (updateAction != null)
//			{
//				updateAction();
//			}
//
//			App.Navigation.Navigation.PopAsync();
//		}

//		private void CancelCommand()
//		{
//			App.Navigation.Navigation.PopModalAsync();
//		}

		private async void UpdateDirectories()
		{
			List<PathItem> dirs = new List<PathItem>();

			// Set title of activity
			Title = path;

            // Check, if there is a parent directory
            var dir = await PCLStorage.FileSystem.Current.LocalStorage.GetFolderAsync(path);

            var relativePath = dir.Path.Substring(PCLStorage.FileSystem.Current.LocalStorage.Path.Length);

            if (relativePath.Length > 1)
			{
				dirs.Add(new PathItem(string.Format("<{0}>", Catalog.GetString("Parent directory")), "TODO"));
			}

            // Add all other directories
            var folders = await dir.GetFoldersAsync();

			foreach (PCLStorage.IFolder folder in folders)
			{
				dirs.Add(new PathItem(folder.Name, folder.Path));
			}

			list.ItemsSource = dirs.ToArray();
		}
	}
}

