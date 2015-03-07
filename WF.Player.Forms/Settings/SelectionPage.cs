// <copyright file="SelectionPage.cs" company="Wherigo Foundation">
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
using WF.Player.Services.Settings;

namespace WF.Player.SettingsPage
{
	public class SelectionPage : ContentPage
	{
		private string[] items;

		private string key;

		private int defaultValue;

		private MultiCell cell;

		private CheckCell[] cells;

		private TableSection section;

		public SelectionPage(MultiCell cell, string title, string[] items, string key, int defaultValue)
		{
			this.cell = cell;
			this.items = items;
			this.key = key;
			this.defaultValue = defaultValue;

			Title = Catalog.GetString(title);

			NavigationPage.SetTitleIcon(this, "HomeIcon.png");
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			section = new TableSection();

			var active = 0;

			if (cell.Values == null)
			{
				active = Settings.Current.GetValueOrDefault<int>(key, defaultValue);
			}
			else
			{
				active = Array.IndexOf(cell.Values, Settings.Current.GetValueOrDefault<string>(key, cell.Values[defaultValue]));
				active = active < 0 ? 0 : active;
			}

			cells = new CheckCell[items.Length];

			for(int i = 0; i < items.Length; i++)
			{
				cells[i] = new CheckCell
					{
						Text = items[i],
						TextColor = App.Colors.Text,
					};

				cells[i].Index = i;
				cells[i].Checkmark = i == active;
				cells[i].Tapped += HandleTapped;

				section.Add(cells[i]);
			}

			var tableRoot = new TableRoot() 
				{
					section,
				};

			var tableView = new TableView() 
				{
					BackgroundColor = App.Colors.Background,
					Intent = TableIntent.Settings,
					Root = tableRoot,
					HasUnevenRows = true,
				};

			Content = tableView;
		}

		void HandleTapped (object sender, EventArgs e)
		{
			var oldActive = 0;

			if (cell.Values == null)
			{
				oldActive = Settings.Current.GetValueOrDefault<int>(key, defaultValue);
			}
			else
			{
				oldActive = Array.IndexOf(cell.Values, Settings.Current.GetValueOrDefault<string>(key, cell.Values[defaultValue]));
				oldActive = oldActive < 0 ? 0 : oldActive;
			}

			var newActive = ((CheckCell)sender).Index;

			cells[oldActive].Checkmark = false;
			cells[newActive].Checkmark = true;

			if (cell.Values == null)
			{
				Settings.Current.AddOrUpdateValue<int>(key, newActive);
			}
			else
			{
				Settings.Current.AddOrUpdateValue<string>(key, cell.Values[newActive]);
			}

			cell.Update();
		}
	}
}

