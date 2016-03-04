[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Services.WP8.UserDialogs.UserDialogService))]

namespace WF.Player.Services.WP8.UserDialogs
{
    using System;
    using System.Linq;
    using Vernacular;
    using WF.Player.Services.UserDialogs;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Xamarin.Forms;

    public class UserDialogService : AbstractUserDialogs
	{
		public override void Alert(AlertConfig config)
		{
		}

		public override async void ActionSheet(ActionSheetConfig config)
		{
            var array = config
                .Options
                .Select(x => x.Text)
                .ToArray();

            var flyout = new ListPickerFlyout
            {
                ItemsSource = array,
                Placement = Windows.UI.Xaml.Controls.Primitives.FlyoutPlacementMode.Bottom,
            };

            await flyout.ShowAtAsync((FrameworkElement)config.Parent);

            if (flyout.SelectedItem == null) return;
            var itemPicked = flyout.SelectedValue.ToString();
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