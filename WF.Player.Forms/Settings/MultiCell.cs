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

		public MultiCell()
		{
//			Height = 180;

			var grid = new Grid 
				{
//					BackgroundColor = Color.Yellow,
					Padding = Device.OnPlatform<Thickness>(new Thickness(0, 10, 10, 10), new Thickness(18, 10, 10, 10), new Thickness(0, 10, 10, 10)),
					RowSpacing = 6,
					HorizontalOptions = LayoutOptions.Fill,
					VerticalOptions = LayoutOptions.Fill,
//					HeightRequest = 80,
					ColumnDefinitions = new ColumnDefinitionCollection
						{
							#if __IOS__
							new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
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

			text = new Label {
				Text = "Sound",
				TextColor = Color.Black,
				FontSize = 17,
				FontFamily = Font.Default.FontFamily,
				HorizontalOptions = LayoutOptions.Start,
			};

			grid.Children.Add(text, 0, 0);

			detail = new Label {
				Text = "This is the description\nwith two lines.",
				TextColor = Color.Gray,
				LineBreakMode = LineBreakMode.WordWrap,
				FontSize = 12,
				FontFamily = Font.Default.FontFamily,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Start,
			};

			#if __IOS__
			grid.Children.Add(detail, 0, 1);
			#endif
			#if __ANDROID__
			grid.Children.Add(detail, 0, 2, 1, 2);
			#endif

			current = new Label {
				Text = "Current value",
				TextColor = Color.Gray,
				LineBreakMode = LineBreakMode.TailTruncation,
				FontSize = 17,
				FontFamily = Font.Default.FontFamily,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = Device.OnPlatform<LayoutOptions>(LayoutOptions.Center, LayoutOptions.Start, LayoutOptions.Center),
			};

			#if __IOS__
			grid.Children.Add(current, 1, 2, 0, 2);
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
			}
		}

		public string Key { get; set; }

		public int DefaultValue { get; set; }

		public string[] Items { get; set; }

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
			current.Text = Items[Settings.Current.GetValueOrDefault<int>(Key, DefaultValue)];
		}
	}
}

