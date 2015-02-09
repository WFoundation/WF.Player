// <copyright file="ToolBarPage.cs" company="Wherigo Foundation">
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
using WF.Player.Services.UserDialogs;
using Vernacular;

namespace WF.Player
{
	using System;
	using System.Collections.ObjectModel;
	using WF.Player.Controls;
	using WF.Player.Services.Device;
	using Xamarin.Forms;

	/// <summary>
	/// Tool bar page.
	/// </summary>
	public class ToolBarPage : BottomBarPage
	{
		/// <summary>
		/// The list of buttons.
		/// </summary>
		private ObservableCollection<ToolButton> buttons;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.ToolBarPage"/> class.
		/// </summary>
		public ToolBarPage() : base()
		{
			buttons = new ObservableCollection<ToolButton>();

			buttons.CollectionChanged += HandleCollectionChanged;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the list of buttons.
		/// </summary>
		/// <value>The buttons.</value>
		public ObservableCollection<ToolButton> Buttons
		{
			get
			{
				return buttons; 
			}
		}

		/// <summary>
		/// Gets or sets the overflow menu text.
		/// </summary>
		/// <value>The overflow menu text.</value>
		public string OverflowMenuText { get; set; }

		#endregion

		/// <summary>
		/// Handles the collection changed.
		/// </summary>
		/// <remarks>
		/// The bottom could only have one sort of buttons: text or icons.
		/// If there are more buttons with text than the bottom could hold, than the overflow menu is shown.
		/// </remarks>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Collection changed event arguments.</param>
		private void HandleCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			BottomLayout.Children.Clear();

			var textLayout = new StackLayout() {
				Orientation = StackOrientation.Horizontal,
				HorizontalOptions = LayoutOptions.Fill,
			};

			foreach (ToolButton t in buttons)
			{
				if (t is ToolTextButton)
				{
					var button = new ExtendedButton() 
					{
						Text = (t is ToolTextButton) ? ((ToolTextButton)t).Text : null,
						TextColor = App.Colors.Tint,
						Image = null,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = Color.Transparent,
						Command = new Command((parameter) => KeyClick(t.Command, parameter)),
						CommandParameter = (t is ToolTextButton) ? ((ToolTextButton)t).Text : null,
						#if __IOS__
						Font = Font.SystemFontOfSize(20),
						#endif
					};

					// Save for later use
					t.Button = button;

					// If only one text button, than add to stack layout
					if (buttons.Count <= 1)
					{
						BottomLayout.Children.Add(button);
					}
					else
					{
						textLayout.Children.Add(button);
					}
				}

				if (t is ToolIconButton)
				{
					var button = new ExtendedButton() 
					{
						Text = (t is ToolTextButton) ? ((ToolTextButton)t).Text : null,
						TextColor = App.Colors.Tint,
						Image = (t is ToolIconButton) ? ((ToolIconButton)t).Icon : null,
						HorizontalOptions = Device.OnPlatform<LayoutOptions>(LayoutOptions.FillAndExpand, LayoutOptions.CenterAndExpand, LayoutOptions.FillAndExpand),
						BackgroundColor = Color.Transparent,
						Command = new Command((parameter) => KeyClick(t.Command, parameter)),
						CommandParameter = (t is ToolIconButton) ? ((ToolIconButton)t).Icon : null,
					};

					// Save for later use
					t.Button = button;

					BottomLayout.Children.Add(button);
				}
			}

			if (buttons.Count == 0 || buttons[0] is ToolIconButton)
			{
				// If the first button is an icon, than we are ready
				return;
			}

			Thickness padding = new Thickness(10, 0);
			double spacing = 6;

			if (buttons.Count == 1)
			{
				// We only have one button, so set width to maximum width 
				BottomLayout.Padding = padding;
				buttons[0].Button.WidthRequest = BottomLayout.Width - padding.Left - padding.Right;

				return;
			}

			// So, now we know, that we have more than one text button.

			// Check, if all buttons fit into the bottom grid.
			double sumWidth = 0;

			// Get width of all buttons
			for (int i = 0; i < buttons.Count; i++)
			{
				sumWidth += ((ToolTextButton)buttons[i]).TextWidth;
			}

			// Add spacing between
			sumWidth += spacing * (buttons.Count - 1);

			// Now check, if all buttons want fit into the bottom line
			if (sumWidth > DependencyService.Get<IScreen>().Width - padding.Left - padding.Right)
			{
				// No. There are more buttons than space, so create an extra menu

				// Create button for extra menu
				var button = new ExtendedButton() 
					{
						Text = OverflowMenuText,
						TextColor = App.Colors.Tint,
						Image = null,
						HorizontalOptions = LayoutOptions.FillAndExpand,
						BackgroundColor = Color.Transparent,
						Command = new Command(HandleOverflowMenuClick),
						#if __IOS__
						Font = Font.SystemFontOfSize(20),
						#endif
					};

				BottomLayout.Children.Add(button);
			}
			else
			{
				// Yes. All buttons should fit into the bottom line.

				// Create grid for buttons
				Grid grid = new Grid() {
					VerticalOptions = LayoutOptions.FillAndExpand,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					RowDefinitions = {
						new RowDefinition { Height = GridLength.Auto },
					},
					ColumnSpacing = 0, //spacing,
				};

				// Create columns for buttons
				var colDefs = new ColumnDefinitionCollection();

//				if (sumWidth > BottomLayout.Width - padding.Left - padding.Right)
//				{
//					// Buttons are wider than possible space (could only happen with Messagebox), so change width of column according to space
//					colDefs.Add(new ColumnDefinition { Width = new GridLength((BottomLayout.Width - padding.Left - padding.Right) * Math.Ceiling(DependencyService.Get<IMeasure>().ButtonTextSize(buttons[0].Button.Text, buttons[0].Button.FontSize)) / sumWidth, GridUnitType.Absolute) });
//				}
//				else
//				{
					// Set coulmn width to real size of button
				colDefs.Add(new ColumnDefinition { Width = new GridLength(((ToolTextButton)buttons[0]).TextWidth + padding.Left + padding.Right, GridUnitType.Absolute) });
//				}

				// Set layout of first button
				buttons[0].Button.HorizontalOptions = LayoutOptions.StartAndExpand;

				// Set width (autosize) and layout of all buttons between first and last
				for (int i = 1; i < buttons.Count - 1; i++)
				{
					colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); //((ToolTextButton)buttons[i]).TextWidth + padding.Left + padding.Right, GridUnitType.Absolute) });
					buttons[i].Button.HorizontalOptions = LayoutOptions.CenterAndExpand;
				}

				if (buttons.Count == 2)
				{
					var width2 = ((ToolTextButton)buttons[1]).TextWidth;
					// If we only have two buttons, than set width to autosize
					colDefs.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }); // Width = GridLength.Auto }); // 
					buttons[buttons.Count-1].Button.HorizontalOptions = LayoutOptions.EndAndExpand;
				}
				else
				{
					// If we have more than 2 buttons, than set width to real size of button
					colDefs.Add(new ColumnDefinition { Width = new GridLength(((ToolTextButton)buttons[buttons.Count-1]).TextWidth + padding.Left + padding.Right, GridUnitType.Absolute) }); // Width = GridLength.Auto }); // 
					buttons[buttons.Count-1].Button.HorizontalOptions = LayoutOptions.EndAndExpand;
				}

				// Set column definitions for the buttons
				grid.ColumnDefinitions = colDefs;

				// Add all buttons to the grid
				for (int i = 0; i < buttons.Count; i++)
				{
//					grid.Children.Add(new BoxView() {BackgroundColor = Color.FromRgb(i*64, i*64, i*64),}, i, 0);
					grid.Children.Add(buttons[i].Button, i, 0);
				}

				// Add grid to BottomLayout
				BottomLayout.Padding = padding;
				BottomLayout.Children.Add(grid);
			}
		}

		/// <summary>
		/// Clicked on button.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="parameter">Parameter for command.</param>
		private void HandleOverflowMenuClick(object parameter)
		{
			// Play any sound, if allowed
			App.Click();

			// Create overflow menu
			// Now show a list with all active commands
			var cfg = new ActionSheetConfig().SetTitle(OverflowMenuText);

			foreach (var b in buttons)
			{
				cfg.Add(b.Button.Text, () => KeyClick(b.Command, b.Button.Text));
			}

			cfg.Cancel = new ActionSheetOption(Catalog.GetString("Cancel"), App.Click);

			DependencyService.Get<IUserDialogService>().ActionSheet(cfg);
		}

		/// <summary>
		/// Clicked on button.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		/// <param name="parameter">Parameter for command.</param>
		private void KeyClick(Command command, object parameter)
		{
			// Play any sound, if allowed
			App.Click();

			// Execute command
			command.Execute(parameter);
		}
	}

	/// <summary>
	/// Tool button class.
	/// </summary>
	public class ToolButton
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.ToolButton"/> class.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		public ToolButton(Command command)
		{
			Button = null;
			Command = command;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the button view.
		/// </summary>
		/// <value>The button.</value>
		public ExtendedButton Button { get; set; }

		/// <summary>
		/// Gets the command.
		/// </summary>
		/// <value>The command.</value>
		public Command Command { get; internal set; }

		/// <summary>
		/// Gets or sets a value indicating whether this button is enabled.
		/// </summary>
		/// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
		public bool IsEnabled
		{ 
			get
			{
				if (Button != null)
				{
					if (Command != null)
					{
						return Command.CanExecute(null);
					}
					else
					{
						return Button.IsEnabled;
					}
				}
				else
				{
					return false;
				}
			}

			set
			{ 
				if (Button != null)
				{
					Button.IsEnabled = value; 
				}
			} 
		}

		#endregion
	}

	/// <summary>
	/// Tool button class for a text button.
	/// </summary>
	public class ToolTextButton : ToolButton
	{
		private int textWidth;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.ToolTextButton"/> class.
		/// </summary>
		/// <param name="text">Text of button.</param>
		/// <param name="command">Command to execute.</param>
		public ToolTextButton(string text, Command command) : base(command)
		{
			Text = text;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text { get; internal set; }

		public int TextWidth
		{ 
			get
			{
				return (int)Math.Ceiling(DependencyService.Get<IMeasure>().ButtonTextSize(Button.Text, Button.FontSize));
			}
		}

		#endregion
	}

	/// <summary>
	/// Tool button class for a icon button.
	/// </summary>
	public class ToolIconButton : ToolButton
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.ToolIconButton"/> class.
		/// </summary>
		/// <param name="icon">Icon of button.</param>
		/// <param name="command">Command to execute.</param>
		public ToolIconButton(string icon, Command command) : base(command)
		{
			Icon = icon;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the icon.
		/// </summary>
		/// <value>The icon.</value>
		public string Icon { get; internal set; }

		#endregion
	}
}
