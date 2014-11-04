﻿// <copyright file="GameInputViewModel.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading;
	using Acr.XamForms.UserDialogs;
	using Vernacular;
	using WF.Player.Core;
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

				// Call all events
				#if __HTML__
				NotifyPropertyChanged (HtmlSourcePropertyName);
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
				return this.input != null ? this.input.Text : string.Empty;
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

				return ImageSource.FromStream(() =>
					{
						return new MemoryStream(Input.Image.Data);
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
						result.Html = ConverterToHtml.FromMarkdown(Input.Markdown, Input.Image);
					}
					else
					{
						result.Html = ConverterToHtml.FromText(Input.Text, Input.Image);
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
				return this.input != null && this.input.InputType == InputType.Text; 
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
				return Catalog.GetString("Write your input here...");
			}
		}

		#endregion

		#region ButtonClicked

		public Xamarin.Forms.Command ButtonClicked
		{
			get
			{
				return new Xamarin.Forms.Command((param) => HandleButtonClicked(this));
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
			NotifyPropertyChanged(TextPropertyName);
			NotifyPropertyChanged(HasImagePropertyName);
			NotifyPropertyChanged(ImageSourcePropertyName);
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
		/// <param name="sender">Sender iof event.</param>
		private async void HandleButtonClicked(object sender)
		{
			// Get active view
			var view = (GameInputView)App.CurrentPage;

			if (this.input.InputType == InputType.Unknown)
			{
				App.Game.ShowScreen(ScreenType.Last, null);
				this.input.GiveResult(null);
			}

			if (this.input.InputType == InputType.Text)
			{
				App.Click();

				App.Game.ShowScreen(ScreenType.Last, null);
				this.input.GiveResult(inputText);

//				var cfg = new PromptConfig();
//				cfg.Message = this.input.Text;
//				cfg.Title = string.Empty;
//				cfg.OnResult = (result) =>
//				{
//					Device.BeginInvokeOnMainThread(() =>
//						{
//							App.Click();
//							if (result.Ok)
//							{
//								App.Game.ShowScreen(ScreenType.Last, null);
//								this.input.GiveResult(result.Text);
//							}
//						});
//				};
//
//				DependencyService.Get<IUserDialogService>().Prompt(cfg);
			}

			if (this.input.InputType == InputType.MultipleChoice)
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

				cfg.Add(Catalog.GetString("Cancel"), () => App.Click());

				DependencyService.Get<IUserDialogService>().ActionSheet(cfg);
			}
		}

		/// <summary>
		/// Called when the user has selected an answer from the multiple choice question.
		/// </summary>
		/// <param name="result">Result of the multiple choice question.</param>
		private async void MultipleChoiceSelected(string result)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					App.Click();
					App.Game.ShowScreen(ScreenType.Last, null);
					this.input.GiveResult(result);
				});
		}

		/// <summary>
		/// Updates the commands.
		/// </summary>
		private void UpdateCommands()
		{
			if (!(App.CurrentPage is GameInputView))
			{
				return;
			}

			if (Input == null)
			{
				return;
			}

			// Get active view
			var view = (GameInputView)App.CurrentPage;

			string text = string.Empty;

			if (this.input.InputType == InputType.Text)
			{
//				text = Catalog.GetString("Answer");
				view.BottomLayout.Children.Clear();
				view.BottomLayout.Children.Add(view.BottomEntry);
				return;
			}

			if (this.input.InputType == InputType.MultipleChoice)
			{
				text = Catalog.GetString("Choose");
			}

			if (this.input.InputType == InputType.Unknown)
			{
				text = Catalog.GetString("Unknown");
			}

			view.Buttons.Add(new ToolTextButton(text, new Xamarin.Forms.Command(HandleButtonClicked)));
		}

		#endregion
	}
}
