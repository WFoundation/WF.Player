[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Services.UserDialogs.Droid.UserDialogService))]

namespace WF.Player.Services.UserDialogs.Droid
{
	using System.Linq;
	using Android.App;
	using Android.Text.Method;
	using Android.Widget;
	using Xamarin.Forms;

	public class UserDialogService : AbstractUserDialogService
	{

		public override void Alert(AlertConfig config)
		{
			Device.BeginInvokeOnMainThread(() => 
                new AlertDialog
				    .Builder(Forms.Context)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetPositiveButton(config.OkText, (o, e) =>
					{
						if (config.OnOk != null)
							config.OnOk();
					})
                    .Show()
			);
		}

		public override void ActionSheet(ActionSheetConfig config)
		{
			var array = config
                .Options
                .Select(x => x.Text)
                .ToArray();

			if (config.Cancel != null)
				Device.BeginInvokeOnMainThread(() => 
    	            new AlertDialog
        	            .Builder(Forms.Context)
            	        .SetTitle(config.Title)
                	    .SetItems(array, (sender, args) => config.Options[args.Which].Action())
						.SetNegativeButton(config.Cancel.Text, (sender, EventArgs) => config.Cancel.Action())
	                    .Show()
				);
			else
				Device.BeginInvokeOnMainThread(() => 
					new AlertDialog
					.Builder(Forms.Context)
					.SetTitle(config.Title)
					.SetItems(array, (sender, args) => config.Options[args.Which].Action())
					.Show()
				);
		}


		public override void Confirm(ConfirmConfig config)
		{
			Device.BeginInvokeOnMainThread(() => 
                new AlertDialog
                    .Builder(Forms.Context)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetPositiveButton(config.OkText, (o, e) => config.OnConfirm(true))
                    .SetNegativeButton(config.CancelText, (o, e) => config.OnConfirm(false))
                    .Show()
			);
		}


		//public override void DateTimePrompt(DateTimePromptConfig config) {
		//    var date = DateTime.Now;
		//    switch (config.SelectionType) {
                
		//        case DateTimeSelectionType.DateTime: // TODO
		//        case DateTimeSelectionType.Date:
		//            var datePicker = new DatePickerDialog(Utils.GetActivityContext(), (sender, args) => {
		//                date = args.Date;
		//            }, 1900, 1, 1);
		//            //picker.CancelEvent
		//            datePicker.DismissEvent += (sender, args) => config.OnResult(new DateTimePromptResult(date));
		//            datePicker.SetTitle(config.Title);
		//            datePicker.Show();
                    
		//            break;

		//        case DateTimeSelectionType.Time:
		//            var timePicker = new TimePickerDialog(Utils.GetActivityContext(), (sender, args) => {
		//                date = new DateTime(
		//                    date.Year,
		//                    date.Month,
		//                    date.Day,
		//                    args.HourOfDay,
		//                    args.Minute,
		//                    0
		//                );
		//            }, 0, 0, false); // takes 24 hour arg
		//            timePicker.DismissEvent += (sender, args) => config.OnResult(new DateTimePromptResult(date));
		//            timePicker.SetTitle(config.Title);
		//            timePicker.Show();
		//            break;
		//    }
		//}


		//public override void DurationPrompt(DurationPromptConfig config) {
		//    // TODO
		//    throw new NotImplementedException();
		//}


		public override void Prompt(PromptConfig config)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					var txt = new EditText(Forms.Context) {
						Hint = config.Placeholder
					};
					if (config.IsSecure)
                    //txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
                    txt.TransformationMethod = PasswordTransformationMethod.Instance;

					new AlertDialog
                    .Builder(Forms.Context)
                    .SetMessage(config.Message)
                    .SetTitle(config.Title)
                    .SetView(txt)
                    .SetPositiveButton(config.OkText, (o, e) =>
                        config.OnResult(new PromptResult {
								Ok = true, 
								Text = txt.Text
							})
					)
                    .SetNegativeButton(config.CancelText, (o, e) => 
                        config.OnResult(new PromptResult {
								Ok = false, 
								Text = txt.Text
							})
					)
                    .Show();
				});
		}
	}
}