[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.WinPhone8.Services.UserDialogs.UserDialogService))]

namespace WF.Player.WinPhone8.Services.UserDialogs
{
	using System;
	using System.Linq;
	using Vernacular;
	using WF.Player.Services.UserDialogs;
	using Xamarin.Forms;

	public class UserDialogService : AbstractUserDialogs
	{
		public override void Alert(AlertConfig config)
		{
		}

		public override void ActionSheet(ActionSheetConfig config)
		{
		}

		public override void Confirm(ConfirmConfig config)
		{
		}

		public override void Login(LoginConfig config)
		{
		}

		public override void Prompt(PromptConfig config)
		{
		}

        protected override INetworkIndicator CreateNetworkIndicator()
        {
            throw new NotImplementedException();
        }
    }
}