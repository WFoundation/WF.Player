using System;
using System.Linq;
using WF.Player.Services.UserDialogs.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserDialogService))]

namespace WF.Player.Services.UserDialogs.iOS
{
	public class UserDialogService : AbstractUserDialogService
	{

		public override void ActionSheet(ActionSheetConfig config)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					var action = new UIActionSheet(config.Title);
					config.Options.ToList().ForEach(x => action.AddButton(x.Text));
					if (config.Cancel != null)
					{
						action.AddButton(config.Cancel.Text);
						action.CancelButtonIndex = config.Options.Count;
					}
					action.Clicked += (sender, btn) =>
					{
						if (btn != null && btn.ButtonIndex == config.Options.Count && config.Cancel != null && config.Cancel.Action != null)
							config.Cancel.Action();
						if (btn != null && btn.ButtonIndex >= 0 && btn.ButtonIndex < config.Options.Count)
							config.Options[(int)btn.ButtonIndex].Action();
					};
					var view = GetTopView();
					action.ShowInView(view);
				});
		}


		public override void Alert(AlertConfig config)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, null, config.OkText);
					if (config.OnOk != null)
						dlg.Clicked += (s, e) => config.OnOk();
                
					dlg.Show();
				});
		}


		public override void Confirm(ConfirmConfig config)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, config.CancelText, config.OkText);
					dlg.Clicked += (s, e) =>
					{
						var ok = (dlg.CancelButtonIndex != e.ButtonIndex);
						config.OnConfirm(ok);
					};
					dlg.Show();
				});
		}


		//public override void DateTimePrompt(DateTimePromptConfig config) {
		//    var sheet = new ActionSheetDatePicker {
		//        Title = config.Title,
		//        DoneText = config.OkText
		//    };

		//    switch (config.SelectionType) {
                
		//        case DateTimeSelectionType.Date:
		//            sheet.DatePicker.Mode = UIDatePickerMode.Date;
		//            break;

		//        case DateTimeSelectionType.Time:
		//            sheet.DatePicker.Mode = UIDatePickerMode.Time;
		//            break;

		//        case DateTimeSelectionType.DateTime:
		//            sheet.DatePicker.Mode = UIDatePickerMode.DateAndTime;
		//            break;
		//    }
            
		//    if (config.MinValue != null)
		//        sheet.DatePicker.MinimumDate = config.MinValue.Value;

		//    if (config.MaxValue != null)
		//        sheet.DatePicker.MaximumDate = config.MaxValue.Value;

		//    sheet.DateTimeSelected += (sender, args) => {
		//        // TODO: stop adjusting date/time
		//        config.OnResult(new DateTimePromptResult(sheet.DatePicker.Date));
		//    };

		//    var top = Utils.GetTopView();
		//    sheet.Show(top);
		//    //sheet.DatePicker.MinuteInterval
		//}


		//public override void DurationPrompt(DurationPromptConfig config) {
		//    var sheet = new ActionSheetDatePicker {
		//        Title = config.Title,
		//        DoneText = config.OkText
		//    };
		//    sheet.DatePicker.Mode = UIDatePickerMode.CountDownTimer;

		//    sheet.DateTimeSelected += (sender, args) => config.OnResult(new DurationPromptResult(args.TimeOfDay));

		//    var top = Utils.GetTopView();
		//    sheet.Show(top);
		//}


		public override void Prompt(PromptConfig config)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					var result = new PromptResult();
					var dlg = new UIAlertView(config.Title ?? String.Empty, config.Message, null, config.CancelText, config.OkText) {
						AlertViewStyle = config.IsSecure
                        ? UIAlertViewStyle.SecureTextInput 
                        : UIAlertViewStyle.PlainTextInput
					};
					var txt = dlg.GetTextField(0);
					txt.SecureTextEntry = config.IsSecure;
					txt.Placeholder = config.Placeholder;

					//UITextView = editable
					dlg.Clicked += (s, e) =>
					{
						result.Ok = (dlg.CancelButtonIndex != e.ButtonIndex);
						result.Text = txt.Text;
						config.OnResult(result);
					};
					dlg.Show();
				});
		}

		private UIWindow GetTopWindow() 
		{
			return UIApplication
				.SharedApplication
				.Windows
				.Reverse()
				.FirstOrDefault(x =>
					(float) x.WindowLevel == (float)UIWindowLevel.Normal &&
					!x.Hidden
					);
		}

		private UIView GetTopView()
		{
			return GetTopWindow().Subviews.Last();
		}

	}
}