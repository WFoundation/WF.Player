// <copyright file="GameMessageboxViewModel.cs" company="Wherigo Foundation">
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
	using System.IO;
	using System.Text;
	using System.Threading;
	using Vernacular;
	using WF.Player.Core;
	using WF.Player.Services;
	using Xamarin.Forms;

	/// <summary>
	/// Game messagebox view model.
	/// </summary>
	public class GameMessageboxViewModel : BaseViewModel
	{
		/// <summary>
		/// The name of the text property.
		/// </summary>
		public const string TextPropertyName = "Text";

		/// <summary>
		/// The name of the image source property.
		/// </summary>
		public const string ImageSourcePropertyName = "ImageSource";

		/// <summary>
		/// The name of the has image property.
		/// </summary>
		public const string HasImagePropertyName = "HasImage";

		/// <summary>
		/// The name of the html source property.
		/// </summary>
		public const string HtmlSourcePropertyName = "HtmlSource";

		/// <summary>
		/// The messagebox.
		/// </summary>
		private MessageBox messagebox;

		#region Properties

		#region ActiveObject

		/// <summary>
		/// Gets or sets the message box.
		/// </summary>
		/// <value>The message box.</value>
		public MessageBox MessageBox 
		{
			get 
			{
				return this.messagebox;
			}

			set 
			{
				// Set property
				SetProperty<MessageBox>(ref this.messagebox, value);

				// Update all views
				#if __HTML__
				NotifyPropertyChanged(HtmlSourcePropertyName);
				#else
				NotifyPropertyChanged(TextPropertyName);
				NotifyPropertyChanged(ImageSourcePropertyName);
				NotifyPropertyChanged(HasImagePropertyName);
				#endif

				UpdateCommands();
			}
		}

		#endregion

		#region Text

		/// <summary>
		/// Gets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text 
		{
			get 
			{
				return this.messagebox != null ? this.messagebox.Text : string.Empty;
			}
		}

		#endregion

		#region ImageSource

		/// <summary>
		/// Gets the poster of cartridge.
		/// </summary>
		/// <value>The poster.</value>
		public ImageSource ImageSource
		{
			get 
			{
				if (!this.HasImage) 
				{
					return null;
				}

				return ImageSource.FromStream(() => 
						{
							return new MemoryStream(MessageBox.Image.Data);
						});
			}
		}

		#endregion

		#region HasImage

		/// <summary>
		/// Gets a value indicating whether this cartridge has poster.
		/// </summary>
		/// <value><c>true</c> if this cartridge has poster; otherwise, <c>false</c>.</value>
		public bool HasImage 
		{ 
			get 
			{ 
				return this.messagebox != null && this.messagebox.Image != null && this.messagebox.Image.Data != null; 
			}
		}

		#endregion

		#region HtmlSource

		/// <summary>
		/// Gets the poster of cartridge.
		/// </summary>
		/// <value>The poster.</value>
		public HtmlWebViewSource HtmlSource
		{
			get 
			{
				var result = new HtmlWebViewSource();

				if (!string.IsNullOrEmpty(MessageBox.Html))
				{
					result.Html = ConverterToHtml.FromHtml(MessageBox.Html);
				}
				else if (!string.IsNullOrEmpty(MessageBox.Markdown)) 
				{
					result.Html = ConverterToHtml.FromMarkdown(MessageBox.Markdown, MessageBox.Image);
				}
				else 
				{
					result.Html = ConverterToHtml.FromText(MessageBox.Text, MessageBox.Image);
				}

				return result;
			}
		}

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		public override void OnAppearing()
		{
			base.OnAppearing();

			// Update all views
			#if __HTML__
			NotifyPropertyChanged(HtmlSourcePropertyName);
			#else
			NotifyPropertyChanged(TextPropertyName);
			NotifyPropertyChanged(HasImagePropertyName);
			NotifyPropertyChanged(ImageSourcePropertyName);
			#endif

			UpdateCommands();
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Handles the click of the first button.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		private void HandleFirstButtonClicked(object sender)
		{
			RemoveMessageBox();

			this.messagebox.GiveResult(MessageBoxResult.FirstButton);
		}

		/// <summary>
		/// Handles the click of the first button.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		private void HandleSecondButtonClicked(object sender)
		{
			RemoveMessageBox();

			this.messagebox.GiveResult(MessageBoxResult.SecondButton);
		}

		/// <summary>
		/// Removes the message box.
		/// </summary>
		private void RemoveMessageBox()
		{
			App.Game.ShowScreen(ScreenType.Last, null);
		}

		/// <summary>
		/// Updates the commands.
		/// </summary>
		private void UpdateCommands()
		{
			if (!(App.GameNavigation.CurrentPage is GameMessageboxView)) 
			{
				return;
			}

			// Get active view
			var view = (GameMessageboxView)App.GameNavigation.CurrentPage;

			view.Buttons.Clear();

			if (this.messagebox != null) 
			{
				view.Buttons.Add(new ToolTextButton(this.messagebox.FirstButtonLabel == null ? Texts.TextOk : this.messagebox.FirstButtonLabel, new Xamarin.Forms.Command(HandleFirstButtonClicked)));
				if (this.messagebox.SecondButtonLabel != null) 
				{
					view.Buttons.Add(new ToolTextButton(this.messagebox.SecondButtonLabel, new Xamarin.Forms.Command(HandleSecondButtonClicked)));
				}
			}
		}

		#endregion
	}
}
