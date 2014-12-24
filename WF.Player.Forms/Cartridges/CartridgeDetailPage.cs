// <copyright file="CartridgeDetailPage.cs" company="Wherigo Foundation">
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
	using System.Collections.Generic;
	using System.IO;
	using Vernacular;
	using WF.Player.Controls;
	using WF.Player.Core;
	using Xamarin.Forms;

	/// <summary>
	/// Cartridge detail page.
	/// </summary>
	public class CartridgeDetailPage : CartridgeDetailBasePage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeDetailPage"/> class.
		/// </summary>
		/// <param name="viewModel">View model.</param>
		public CartridgeDetailPage(CartridgeDetailViewModel viewModel) : base(viewModel)
		{
			this.SetBinding(ContentPage.TitleProperty, CartridgeDetailViewModel.NamePropertyName);

			this.DirectionView.SetBinding(DirectionArrow.DirectionProperty, CartridgeDetailViewModel.DirectionPropertyName);
			this.DistanceView.SetBinding(Label.TextProperty, CartridgeDetailViewModel.DistanceTextPropertyName, BindingMode.OneWay);

			var layoutScroll = new ScrollView() 
			{
				Orientation = ScrollOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};

			var layout = new StackLayout() 
				{
					Orientation = StackOrientation.Vertical,
					Padding = 10,
					Spacing = 10,
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
				};

			// Header
			var label = new Label 
			{
				XAlign = App.Prefs.TextAlignment,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Font = App.Fonts.Header,
				TextColor = App.Colors.Text,
			};

			label.SetBinding(Label.TextProperty, CartridgeDetailViewModel.NamePropertyName);

			layout.Children.Add(label);

			// Author
			label = new Label 
			{
				XAlign = App.Prefs.TextAlignment,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				Font = App.Fonts.Small,
				TextColor = App.Colors.Text,
			};

			label.SetBinding(Label.TextProperty, CartridgeDetailViewModel.AuthorPropertyName);

			layout.Children.Add(label);

			// Poster
			var poster = new ExtendedImage 
				{
					Aspect = Aspect.AspectFit,
				};

			poster.SetBinding(Image.SourceProperty, CartridgeDetailViewModel.PosterSourcePropertyName);
			poster.SetBinding(Image.IsVisibleProperty, CartridgeDetailViewModel.HasPosterPropertyName);

			layout.Children.Add(poster);

			var listSource = new List<MenuEntry>() 
			{
				new MenuEntry(Catalog.GetString("Description"), App.Colors.Text, HandleDescriptionClicked),
				new MenuEntry(Catalog.GetString("Details"), App.Colors.Text, HandleDetailsClicked),
				new MenuEntry(Catalog.GetString("Attributes"), App.Colors.Text, HandleAttributesClicked),
				new MenuEntry(Catalog.GetString("Map"), App.Colors.Text, HandleMapClicked),
				new MenuEntry(Catalog.GetString("History"), App.Colors.Text, HandleHistoryClicked),
				new MenuEntry(Catalog.GetString("Logs"), App.Colors.Text, HandleLogsClicked),
			};

			var list = new ListView() 
				{
					ItemsSource = listSource,
					HeightRequest = listSource.Count * 44,
				};

			var cell = new DataTemplate(typeof(AccessoryCell));

			cell.SetBinding(AccessoryCell.TextProperty, "Text");
			cell.SetBinding(AccessoryCell.TextColorProperty, "TextColor");

			list.ItemTemplate = cell;
			list.ItemTapped += (object sender, ItemTappedEventArgs e) => 
				{
					// Get selected MenuEntry
					MenuEntry me = (MenuEntry)e.Item;

					// If MenuEntry is null (unselected item), than leave
					if (me == null)
					{
						return;
					}

					// Call handler of this MenuEntry
					me.Handler(sender, e);

					// Deselect MenuEntry
					list.SelectedItem = null;
				};

			layout.Children.Add(list);

			layoutScroll.Content = layout;
			((StackLayout)ContentLayout).Children.Add(layoutScroll);
		}

		#endregion

		#region Event Handlers

		/// <summary>
		/// Handles the description clicked.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleDescriptionClicked(object sender, EventArgs e)
		{
			App.Navigation.PushAsync(new CartridgeDetailDescriptionView((CartridgeDetailViewModel)this.BindingContext));
		}

		/// <summary>
		/// Handles the details clicked.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Event argumeeeents.</param>
		private void HandleDetailsClicked(object sender, EventArgs e)
		{
			Console.WriteLine("Details");
		}

		/// <summary>
		/// Handles the attributes clicked.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleAttributesClicked(object sender, EventArgs e)
		{
			Console.WriteLine("Attributes");
		}

		/// <summary>
		/// Handles the attributes clicked.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleMapClicked(object sender, EventArgs e)
		{
			App.Navigation.PushAsync(new CartridgeDetailMapView((CartridgeDetailViewModel)this.BindingContext));
		}

		/// <summary>
		/// Handles the history clicked.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleHistoryClicked(object sender, EventArgs e)
		{
			Console.WriteLine("History");
		}

		/// <summary>
		/// Handles the logs clicked.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Event arguments.</param>
		private void HandleLogsClicked(object sender, EventArgs e)
		{
			Console.WriteLine("Logs");
		}

		#endregion
	}

	/// <summary>
	/// Menu entry.
	/// </summary>
	public class MenuEntry
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.MenuEntry"/> class.
		/// </summary>
		/// <param name="text">Text of menu entry.</param>
		/// <param name="color">Color of menu entry.</param>
		/// <param name="handler">Event handler.</param>
		public MenuEntry(string text, Color color, EventHandler handler)
		{
			this.Text = text;
			this.TextColor = color;
			this.Handler = handler;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; private set; }

		/// <summary>
		/// Gets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public Color TextColor { get; private set; }

		/// <summary>
		/// Gets the handler.
		/// </summary>
		/// <value>The handler.</value>
		public EventHandler Handler { get; private set; }

		#endregion
	}
}
