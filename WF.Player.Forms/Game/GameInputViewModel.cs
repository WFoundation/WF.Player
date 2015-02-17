// <copyright file="GameInputViewModel.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player
{
	using System;
	using System.IO;
	using Vernacular;
	using WF.Player.Core;
	using WF.Player.Services.BarCode;
	using WF.Player.Services.UserDialogs;
	using Xamarin.Forms;

	/// <summary>
	/// Game input view model.
	/// </summary>
	public class GameInputViewModel : BaseViewModel
	{
		/// <summary>
		/// The name of the input property.
		/// </summary>
		public const string InputPropertyName = "Input";

		/// <summary>
		/// The name of the text property.
		/// </summary>
		public const string TextPropertyName = "Text";

		/// <summary>
		/// The name of the has text property.
		/// </summary>
		public const string HasTextPropertyName = "HasText";

		/// <summary>
		/// The name of the has no text property.
		/// </summary>
		public const string HasNoTextPropertyName = "HasNoText";

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
		/// The name of the has entry property.
		/// </summary>
		public const string HasEntryPropertyName = "HasEntry";

		/// <summary>
		/// The name of the input text property.
		/// </summary>
		public const string InputTextPropertyName = "InputText";

		/// <summary>
		/// The name of the scanner clicked property.
		/// </summary>
		public const string ScannerClickedPropertyName = "ScannerClicked";

		/// <summary>
		/// The name of the button clicked property.
		/// </summary>
		public const string ButtonClickedPropertyName = "ButtonClicked";

		/// <summary>
		/// The name of the placeholder property.
		/// </summary>
		public const string PlaceholderPropertyName = "Placeholder";

		/// <summary>
		/// The input.
		/// </summary>
		private Input input;

		/// <summary>
		/// The input text.
		/// </summary>
		private string inputText;

		#region Properties

		#region Input

		/// <summary>
		/// Gets or sets the input.
		/// </summary>
		/// <value>The input.</value>
		public Input Input 
		{
			get 
			{
				return this.input;
			}

			set
			{
				// Set property
				SetProperty<Input>(ref this.input, value, InputPropertyName);

				InputText = string.Empty;

				// Call all events
				#if __HTML__
				NotifyPropertyChanged (HtmlSourcePropertyName);
				#else
				NotifyPropertyChanged(HasTextPropertyName);
				NotifyPropertyChanged(HasNoTextPropertyName);
				NotifyPropertyChanged(TextPropertyName);
				NotifyPropertyChanged(ImageSourcePropertyName);
				NotifyPropertyChanged(HasImagePropertyName);
				NotifyPropertyChanged(HasEntryPropertyName);
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
				return this.input != null ? this.input.Text : string.Empty;
			}
		}

		#endregion

		#region HasText

		/// <summary>
		/// Gets a value indicating whether this instance has text.
		/// </summary>
		/// <value><c>true</c> if this instance has text; otherwise, <c>false</c>.</value>
		public bool HasText 
		{
			get 
			{
				return this.input != null ? !string.IsNullOrWhiteSpace(this.input.Text) : false;
			}
		}

		#endregion

		#region HasNoText

		/// <summary>
		/// Gets a value indicating whether this instance has no text.
		/// </summary>
		/// <value><c>true</c> if this instance has no text; otherwise, <c>false</c>.</value>
		public bool HasNoText 
		{
			get 
			{
				return this.input != null ? string.IsNullOrWhiteSpace(this.input.Text) : true;
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
				if (!HasImage)
				{
					return null;
				}

				return App.Game.GetImageSourceForMedia(Input.Image);
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
				return this.input != null && this.input.Image != null && this.input.Image.Data != null; 
			}
		}

		#endregion

		#region HtmlSource

		/// <summary>
		/// Gets the description of object as Html.
		/// </summary>
		/// <value>The Html representation of the description.</value>
		public HtmlWebViewSource HtmlSource
		{
			get
			{
				var result = new HtmlWebViewSource();

				if (!string.IsNullOrEmpty(Input.Html))
				{
					result.Html = ConverterToHtml.FromHtml(Input.Html);
				}
				else
				{
					if (!string.IsNullOrEmpty(Input.Markdown))
					{
						result.Html = ConverterToHtml.FromMarkdown(Input.Markdown, Settings.TextAlignment, Input.Image);
					}
					else
					{
						result.Html = ConverterToHtml.FromText(Input.Text, Settings.TextAlignment, Input.Image);
					}
				}

				return result;
			}
		}

		#endregion

		#region HasEntry

		/// <summary>
		/// Gets a value indicating whether this input object has a entry field.
		/// </summary>
		/// <value><c>true</c> if this input object has an entry field; otherwise, <c>false</c>.</value>
		public bool HasEntry
		{ 
			get
			{ 
				return this.input != null && this.input.InputType == WF.Player.Core.InputType.Text; 
			}
		}

		#endregion

		#region InputText

		/// <summary>
		/// Gets or sets a value which correspondens to the editEntry text.
		/// </summary>
		/// <value>Text of the entry filed.</value>
		public string InputText
		{ 
			get
			{ 
				return this.inputText; 
			}

			set
			{
				SetProperty<string>(ref this.inputText, value, InputTextPropertyName);
			}
		}

		#endregion

		#region Placeholder

		/// <summary>
		/// Gets a placeholder text for the entry field.
		/// </summary>
		/// <value>Text of placeholder.</value>
		public string Placeholder
		{ 
			get
			{ 
				return Catalog.GetString("Write input here...", "A short text, because of size. If it is to long, than entry filed hides Ok button");
			}
		}

		#endregion

		#region ButtonClicked

		/// <summary>
		/// Gets the button clicked command for text input "Ok" button.
		/// </summary>
		/// <value>The button clicked command.</value>
		public Xamarin.Forms.Command ButtonClicked
		{
			get
			{
				return new Xamarin.Forms.Command((param) => HandleButtonClicked(this));
			}
		}

		#endregion

		#region ButtonClicked

		/// <summary>
		/// Gets the scanner clicked command.
		/// </summary>
		/// <value>The button clicked command.</value>
		public Xamarin.Forms.Command ScannerClicked
		{
			get
			{
				return new Xamarin.Forms.Command((param) => HandleScannerClicked(this));
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

			#if __HTML__
			NotifyPropertyChanged (HtmlSourcePropertyName);
			#else
			NotifyPropertyChanged(HasTextPropertyName);
			NotifyPropertyChanged(HasNoTextPropertyName);
			NotifyPropertyChanged(TextPropertyName);
			NotifyPropertyChanged(ImageSourcePropertyName);
			NotifyPropertyChanged(HasImagePropertyName);
			NotifyPropertyChanged(HasEntryPropertyName);
			#endif

			UpdateCommands();
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		public override void OnDisappearing()
		{
			base.OnDisappearing();
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Handles the click of the button.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		private void HandleButtonClicked(object sender)
		{
			// Get active view
			var view = (GameInputView)App.GameNavigation.CurrentPage;

			if (this.input.InputType == WF.Player.Core.InputType.Unknown)
			{
				App.Game.ShowScreen(ScreenType.Last, null);
				this.input.GiveResult(null);
			}

			if (this.input.InputType == WF.Player.Core.InputType.Text)
			{
				// Click for button
				App.Click();

				// Remove the input screen
				App.Game.ShowScreen(ScreenType.Last, null);

				// Handle input of dialog
				this.input.GiveResult(inputText);
			}

			if (this.input.InputType == WF.Player.Core.InputType.MultipleChoice)
			{
				var cfg = new ActionSheetConfig().SetTitle(Catalog.GetString("Choose"));

				foreach (var c in this.input.Choices)
				{
					cfg.Add(
						c, 
						() =>
						{
							MultipleChoiceSelected(c);
						});
				}

				cfg.Cancel = new ActionSheetOption(Catalog.GetString("Cancel"), App.Click);

				UserDialogs.Instance.ActionSheet(cfg);
			}
		}

		/// <summary>
		/// Handles the scanner click.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		private async void HandleScannerClicked(object sender)
		{
			App.Click();

			var scanner = DependencyService.Get<IBarCodeScanner>();
			var config = new BarCodeReadConfiguration();

			config.CancelText = Catalog.GetString("Cancel");
			config.FlashlightText = Catalog.GetString("Flash");

			var result = await scanner.Read(config, System.Threading.CancellationToken.None);

			if (result.Success) 
			{
				InputText = result.Code;
			}
		}


		/// <summary>
		/// Called when the user has selected an answer from the multiple choice question.
		/// </summary>
		/// <param name="result">Result of the multiple choice question.</param>
		private void MultipleChoiceSelected(object result)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					// Click for button
					App.Click();

					// Remove the input screen
					App.Game.ShowScreen(ScreenType.Last, null);

					// Handle input of dialog
					this.input.GiveResult((string)result);
				});
		}

		/// <summary>
		/// Updates the commands.
		/// </summary>
		private void UpdateCommands()
		{
			if (!(App.GameNavigation.CurrentPage is GameInputView))
			{
				return;
			}

			if (Input == null)
			{
				return;
			}

			// Get active view
			var view = (GameInputView)App.GameNavigation.CurrentPage;

			string text = string.Empty;

			if (this.input.InputType == WF.Player.Core.InputType.Text)
			{
				view.BottomLayout.Children.Clear();
				view.BottomLayout.Children.Add(view.BottomEntry);

				return;
			}

			// Show unknown button and nothing else
			if (this.input.InputType == WF.Player.Core.InputType.Unknown)
			{
				view.Buttons.Clear();
				view.Buttons.Add(new ToolTextButton(Catalog.GetString("Unknown"), new Xamarin.Forms.Command(() => {
					App.Game.ShowScreen(ScreenType.Last, null);
					this.input.GiveResult(null);})));

				return;
			}

			// For multiple choice add all items to bottom layout
			if (this.input.InputType == WF.Player.Core.InputType.MultipleChoice)
			{
				view.OverflowMenuText = Catalog.GetString("Choose");

				view.Buttons.Clear();

				foreach (var c in this.input.Choices)
				{
					view.Buttons.Add(new ToolTextButton(c, new Xamarin.Forms.Command((param) => { MultipleChoiceSelected(param); })));
				}
			}
		}

		#endregion
	}
}
