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

namespace WF.Player
{
	using System;
	using System.Collections.ObjectModel;
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

		#endregion

		/// <summary>
		/// Handles the collection changed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Collection changed event arguments.</param>
		private void HandleCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			BottomLayout.Children.Clear();

			foreach (ToolButton t in buttons)
			{
				if (t is ToolTextButton)
				{
					var button = new Button() 
					{
						Text = (t is ToolTextButton) ? ((ToolTextButton)t).Text : null,
						TextColor = App.Colors.Tint,
						Image = (t is ToolIconButton) ? ((ToolIconButton)t).Icon : null,
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
				}

				if (t is ToolIconButton)
				{
					var button = new Button() 
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

			if (buttons.Count == 1)
			{
				// We only have one button, so set width to maximum width 
				buttons[0].Button.WidthRequest = BottomLayout.Width;
				return;
			}

			// So, now we know, that we have more than one text button, so calc correct size for buttons

			double deviceSpace = Device.OnPlatform<double>(15, 0, 0);

			// Calc whole width of text on all buttons
			double sumTextWidth = 0;
			foreach (ToolButton t in buttons)
			{
				if (t is ToolTextButton && t.IsEnabled)
				{
					sumTextWidth += DependencyService.Get<IMeasure>().ButtonTextSize(((ToolTextButton)t).Text);
				}
			}

			RelativeLayout relativeLayout;

			relativeLayout = new RelativeLayout() 
			{
				BackgroundColor = Color.Transparent,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			// We have text buttons in the toolbar, so use a relative layout instead of the stack layout
			BottomLayout.Children.Add(relativeLayout);

			double sumButtonWidth = 0;
			double maxWidth = BottomLayout.Width - ((buttons.Count - 1) * BottomLayout.Spacing);

			// Get max width of all buttons, so we could calc spacing between them
			foreach (ToolButton t in buttons)
			{
				double textWidth = DependencyService.Get<IMeasure>().ButtonTextSize(((ToolTextButton)t).Text);
				double width = (maxWidth * textWidth) / (sumTextWidth != 0 ? sumTextWidth : textWidth);
				double buttonWidth = textWidth + deviceSpace < width ? textWidth + deviceSpace : width;

				sumButtonWidth += buttonWidth;
			}

			double spacing = (maxWidth - sumButtonWidth) / (buttons.Count - 1) + BottomLayout.Spacing;
			Button lastButton = null;
			Button startButton = buttons[0].Button;
			Button endButton = buttons[buttons.Count - 1].Button;

			// We now have all relevant information, so we could place all buttons in relative layout
			foreach (ToolButton t in buttons)
			{
				double textWidth = DependencyService.Get<IMeasure>().ButtonTextSize(((ToolTextButton)t).Text);
				double width = (maxWidth * textWidth) / (sumTextWidth != 0 ? sumTextWidth : textWidth);

 				t.Button.WidthRequest = textWidth + deviceSpace < width ? textWidth + deviceSpace : width;

				if (t.Button == startButton)
				{
					t.Button.HorizontalOptions = LayoutOptions.Start;
					relativeLayout.Children.Add(t.Button, Constraint.Constant(0), Constraint.Constant(0));
				}
				else if (t.Button == endButton)
				{
					t.Button.HorizontalOptions = LayoutOptions.Start;
					relativeLayout.Children.Add(t.Button, Constraint.RelativeToParent((parent) => { return parent.Width - t.Button.Width; }), Constraint.Constant(0));
				}
				else
				{
					relativeLayout.Children.Add(
						t.Button, 
						Constraint.RelativeToView(
							lastButton, 
							(layout, sibling) =>
							{
								return sibling.X + sibling.Width + spacing;
							}),
						Constraint.Constant(0));
				}

				lastButton = t.Button;
			}
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
		public Button Button { get; set; }

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
					return Button.IsEnabled;
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
