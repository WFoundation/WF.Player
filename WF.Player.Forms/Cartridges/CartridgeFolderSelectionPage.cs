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

namespace WF.Player
{
	public class CartridgeFolderSelectionPage : ToolBarPage
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

		public CartridgeFolderSelectionPage(string path, Action updateAction) : base()
		{
			this.path = path;
			this.updateAction = updateAction;

			Title = path;

			this.ToolbarItems.Add(new ToolbarItem(Catalog.GetString("New folder"), null, () =>
				{
					App.Click();
					var cfg = new PromptConfig();
					cfg.Message = Catalog.GetString("New folder name");
					cfg.Title = Catalog.GetString("New folder");
					cfg.OnResult = (result) =>
						{
							Device.BeginInvokeOnMainThread(() =>
								{
									App.Click();
									if (result.Ok)
									{
										Directory.CreateDirectory(Path.Combine(path, result.Text));
										this.path = Path.Combine(path, result.Text);
										UpdateDirectories();
									}
								});
						};
					DependencyService.Get<IUserDialogService>().Prompt(cfg);
				}, ToolbarItemOrder.Secondary));

			var cell = new DataTemplate(typeof(TextCell));
			cell.SetValue(TextCell.TextColorProperty, Color.Black); //App.Colors.Text);
			cell.SetBinding(TextCell.TextProperty, "Name");

			list = new ListView() {
//				BackgroundColor = Color.Yellow,
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
				UpdateDirectories();
			};

			((StackLayout)ContentLayout).Children.Add(list);

			UpdateDirectories();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Buttons.Add(new ToolTextButton(Catalog.GetString("Select"), new Command(SelectCommand)));
			Buttons.Add(new ToolTextButton(Catalog.GetString("Cancel"), new Command(CancelCommand)));
		}

		protected override bool OnBackButtonPressed()
		{
			CancelCommand();

			return true;
		}

		private void SelectCommand()
		{
			Settings.Current.AddOrUpdateValue<string>(Settings.CartridgePathKey, path);

			if (updateAction != null)
			{
				updateAction();
			}

			App.Navigation.Navigation.PopModalAsync();
		}

		private void CancelCommand()
		{
			App.Navigation.Navigation.PopModalAsync();
		}

		private void UpdateDirectories()
		{
			List<PathItem> dirs = new List<PathItem>();

			// Set title of activity
			Title = path;

			// Check, if there is a parent directory
			if (Directory.GetParent(path) != null)
			{
				dirs.Add(new PathItem(string.Format("<{0}>", Catalog.GetString("Parent directory")), Directory.GetParent(path).FullName));
			}

			// Add all other directories
			foreach (string dir in Directory.GetDirectories(path))
			{
				dirs.Add(new PathItem(Path.GetFileName(dir), dir));
			}

			list.ItemsSource = dirs.ToArray();
		}
	}
}

