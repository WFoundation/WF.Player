// <copyright file="MultiCell.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
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
using Vernacular;

namespace WF.Player.SettingsPage
{
	using System;
	using Xamarin.Forms;
	using WF.Player.Controls;

	public class MultiCell : ViewCell
	{
		private int itemIndex;
		private Label text;
		private Label detail;
		private Label current;

		public MultiCell(bool hasDetails = false)
		{
//			Height = 180;

			var grid = new Grid 
				{
					BackgroundColor = App.Colors.Background,
					Padding = Device.OnPlatform<Thickness>(new Thickness(0, 10, 10, 10), new Thickness(18, 10, 18, 10), new Thickness(0, 10, 10, 10)),
					RowSpacing = 6,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
//					HeightRequest = 80,
					ColumnDefinitions = new ColumnDefinitionCollection
						{
							#if __IOS__
							new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
							#endif
							#if __ANDROID__
							new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
							#endif
							new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
						},
					RowDefinitions = new RowDefinitionCollection
						{
							new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
							new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) },
						},
				};

			if (!hasDetails)
			{
				// If we don't have any details, than delete row for details
				grid.RowDefinitions.RemoveAt(1);
			}

			text = new Label {
				TextColor = App.Colors.Text,
				FontSize = Device.OnPlatform<int>(17, 14, 17),
				FontFamily = Font.Default.FontFamily,
				HorizontalOptions = LayoutOptions.Start,
			};

			grid.Children.Add(text, 0, 0);

			if (hasDetails)
			{
				detail = new Label {
					TextColor = Color.Gray,
					LineBreakMode = LineBreakMode.WordWrap,
					FontSize = 12,
					FontFamily = Font.Default.FontFamily,
					HorizontalOptions = LayoutOptions.Start,
					VerticalOptions = LayoutOptions.Start,
					IsVisible = false,
				};

				#if __IOS__
				grid.Children.Add(detail, 0, 1);
				#endif
				#if __ANDROID__
				grid.Children.Add(detail, 0, 2, 1, 2);
				#endif
			}

			current = new Label {
				TextColor = Color.Gray,
				LineBreakMode = LineBreakMode.TailTruncation,
				FontSize = 17,
				FontFamily = Font.Default.FontFamily,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = Device.OnPlatform<LayoutOptions>(LayoutOptions.Center, LayoutOptions.Start, LayoutOptions.Center),
			};

			#if __IOS__
			if (hasDetails)
			{
				grid.Children.Add(current, 1, 2, 0, 2);
			}
			else
			{
				grid.Children.Add(current, 1, 2, 0, 1);
			}
			#endif
			#if __ANDROID__
			grid.Children.Add(current, 1, 2, 0, 1);
			#endif

			View = grid;
		}

		public string Text
		{
			get
			{
				return text.Text;
			}

			set
			{
				text.Text = value;
			}
		}

		public string Detail
		{
			get
			{
				return detail.Text;
			}

			set
			{
				detail.Text = value;

				if (!string.IsNullOrEmpty(detail.Text))
				{
					detail.IsVisible = true;
				}
			}
		}

		public string Key { get; set; }

		public int DefaultValue { get; set; }

		public string[] Items { get; set; }

		public string[] ShortItems { get; set; }

		protected override void OnAppearing()
		{
			base.OnAppearing();

			Update();
		}

		protected override void OnTapped()
		{
			base.OnTapped();

			#if __IOS__

			App.Navigation.Navigation.PushAsync(new SelectionPage(this, text.Text, Items, Key, DefaultValue));

			#endif

			#if __ANDROID__

			var builder = new Android.App.AlertDialog.Builder(Forms.Context);

			builder.SetTitle(text.Text);
			builder.SetSingleChoiceItems(Items, Settings.Current.GetValueOrDefault<int>(Key, DefaultValue), (sender, args) => { 
				Settings.Current.AddOrUpdateValue<int>(Key, args.Which);
				Update(); 
			});

			var dialog = builder.Create();

			dialog.SetButton(Catalog.GetString("Ok"), (s, e) => { dialog.Dismiss(); });

			dialog.Show();

			#endif
		}

		public void Update()
		{
			if (ShortItems != null)
			{
				current.Text = ShortItems[Settings.Current.GetValueOrDefault<int>(Key, DefaultValue)];
			}
			else
			{
				current.Text = Items[Settings.Current.GetValueOrDefault<int>(Key, DefaultValue)];
			}
		}
	}
}

