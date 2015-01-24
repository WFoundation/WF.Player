
// Found at https://gist.github.com/conceptdev/47986fd40c2f9a8b390a
//
// To insert it into XAML do the following
//
//<?xml version="1.0" encoding="UTF-8" ?>
//	<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
//		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
//		xmlns:i18n="clr-namespace:WF.Player.Localization;assembly=WFPlayeriOS"
//		x:Class="TodoXaml.TodoItemXaml">
//
//		<ContentPage.Content>
//			<StackLayout VerticalOptions="StartAndExpand">
//
//				<Label Text="{i18n:Translate Name}" />
//				<Entry Text="" x:Name="nameEntry" Placeholder="task name" />
//
//				<Label Text="{i18n:Translate Notes}" />
//				<Entry Text="{Binding Path=Notes}" x:Name="notesEntry" />
//
//				<Label Text="{i18n:Translate Done}" />
//				<Switch IsToggled="{Binding Path=Done}" x:Name="DoneSwitch" />
//
//				<Button Text="{i18n:Translate Save}" Clicked="OnSaveActivated" />
//				<Button Text="{i18n:Translate Delete}" Clicked="OnDeleteActivated" />
//
//				<Button Text="{i18n:Translate Cancel}" Clicked="OnCancelActivated" />
//
//				<Button Text="{i18n:Translate Speak}" Clicked="OnSpeakActivated" />
//
//			</StackLayout>
//		</ContentPage.Content>
//	</ContentPage>

using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Vernacular; 

namespace WF.Player.Localization
{
	// You exclude the 'Extension' suffix when using in Xaml markup
	[ContentProperty ("Text")]
	public class TranslateExtension : IMarkupExtension
	{
		public string Text { get; set; }
		public object ProvideValue (IServiceProvider serviceProvider)
		{
			if (Text == null)
				return null;

			// Do your translation lookup here, using whatever method you require
			var translated = Catalog.GetString (Text);

			return translated;
		}
	}
}