using Android.App;
using WF.Player.Services.UserDialogs;
using Android.Widget;
using Android.Text;
using Android.Text.Method;
using Android.Views;
using Xamarin.Forms;
using System.Linq;

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.UserDialogs.UserDialogService))]

namespace WF.Player.Droid.Services.UserDialogs
{
	using System;
	using System.Threading;
	using WF.Player.Services.UserDialogs;

	public class UserDialogService : AbstractUserDialogs
	{
		private readonly Func<Activity> getTopActivity;

		public UserDialogService(Func<Activity> getTopActivity)
		{
			this.getTopActivity = getTopActivity;
		}

		public override void Alert(AlertConfig config)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				new AlertDialog
				.Builder(this.getTopActivity())
				.SetCancelable(false)
				.SetMessage(config.Message)
				.SetTitle(config.Title)
				.SetPositiveButton(config.OkText, (o, e) => config.OnOk.TryExecute())
				.Show()
			);
		}

		public override void ActionSheet(ActionSheetConfig config)
		{
			var array = config
				.Options
				.Select(x => x.Text)
				.ToArray();
			var dlg = new AlertDialog
				.Builder(this.getTopActivity())
				.SetCancelable(false)
				.SetTitle(config.Title);
			dlg.SetItems(array, (sender, args) => config.Options[args.Which].Action.TryExecute());
			if (config.Destructive != null)
				dlg.SetNegativeButton(config.Destructive.Text, (sender, e) => config.Destructive.Action.TryExecute());
			if (config.Cancel != null)
				dlg.SetNeutralButton(config.Cancel.Text, (sender, e) => config.Cancel.Action.TryExecute());
			Utils.RequestMainThread(() => dlg.Show());
		}

		public override void Confirm(ConfirmConfig config)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				new AlertDialog
				.Builder(this.getTopActivity())
				.SetCancelable(false)
				.SetMessage(config.Message)
				.SetTitle(config.Title)
				.SetPositiveButton(config.OkText, (o, e) => config.OnConfirm(true))
				.SetNegativeButton(config.CancelText, (o, e) => config.OnConfirm(false))
				.Show()
			);
		}

		public override void Login(LoginConfig config)
		{
			var context = this.getTopActivity();
			var txtUser = new EditText(context) {
				Hint = config.LoginPlaceholder,
				InputType = InputTypes.TextVariationVisiblePassword,
				Text = config.LoginValue ?? String.Empty
			};
			var txtPass = new EditText(context) {
				Hint = config.PasswordPlaceholder ?? "*"
			};
			this.SetInputType(txtPass, InputType.Password);
			var layout = new LinearLayout(context) {
				Orientation = Orientation.Vertical
			};
			txtUser.SetMaxLines(1);
			txtPass.SetMaxLines(1);
			layout.AddView(txtUser, ViewGroup.LayoutParams.MatchParent);
			layout.AddView(txtPass, ViewGroup.LayoutParams.MatchParent);
			Utils.RequestMainThread(() =>
				{
					var dialog = new AlertDialog
					.Builder(this.getTopActivity())
					.SetCancelable(false)
					.SetTitle(config.Title)
					.SetMessage(config.Message)
					.SetView(layout)
					.SetPositiveButton(config.OkText, (o, e) =>
						config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, true))
					             )
					.SetNegativeButton(config.CancelText, (o, e) =>
						config.OnResult(new LoginResult(txtUser.Text, txtPass.Text, false))
					             )
					.Create();
					dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
					dialog.Show();
				});
		}

		public override void Prompt(PromptConfig config)
		{
			Utils.RequestMainThread(() =>
				{
					var activity = this.getTopActivity();
					var txt = new EditText(activity) {
						Hint = config.Placeholder
					};
					if (config.InputType != InputType.Default)
						txt.SetMaxLines(1);
					this.SetInputType(txt, config.InputType);
					var dialog = new AlertDialog
					.Builder(activity)
					.SetCancelable(false)
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
					             ).Create();
					dialog.Window.SetSoftInputMode(SoftInput.StateVisible);
					dialog.Show();
				});
		}

		protected override INetworkIndicator CreateNetworkIndicator()
		{
			return new NetworkIndicator(this.getTopActivity());
		}

		protected virtual void SetInputType(TextView txt, InputType inputType)
		{
			switch (inputType)
			{
				case InputType.Email:
					txt.InputType = InputTypes.TextVariationEmailAddress;
					break;
				case InputType.Number:
					txt.InputType = InputTypes.ClassNumber;
					break;
				case InputType.Password:
					txt.TransformationMethod = PasswordTransformationMethod.Instance;
					txt.InputType = InputTypes.ClassText | InputTypes.TextVariationPassword;
					break;
			}
		}
	}

	public static class Utils
	{
		public static void RequestMainThread(Action action)
		{
			if (Android.App.Application.SynchronizationContext == SynchronizationContext.Current)
				action();
			else
				Android.App.Application.SynchronizationContext.Post(x =>
					{
						try
						{
							action();
						}
						catch
						{
						}
					}, null);
		}
	}
}