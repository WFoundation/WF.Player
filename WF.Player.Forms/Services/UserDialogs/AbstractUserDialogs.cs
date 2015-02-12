// <copyright file="AbstractUserDialogs.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2015  Dirk Weltz (mail@wfplayer.com)
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
using System.Threading.Tasks;

namespace WF.Player.Services.UserDialogs
{
	using System;

	public abstract class AbstractUserDialogs : IUserDialogs
	{
		public abstract void Alert(AlertConfig config);

		public abstract void ActionSheet(ActionSheetConfig config);

		public abstract void Confirm(ConfirmConfig config);

		public abstract void Login(LoginConfig config);

		public abstract void Prompt(PromptConfig config);

		protected abstract INetworkIndicator CreateNetworkIndicator();

		public virtual void Alert(string message, string title, string okText)
		{
			this.Alert(new AlertConfig {
					Message = message,
					Title = title,
					OkText = okText
				});
		}

		public virtual INetworkIndicator NetworkIndication(bool show = true)
		{
			var indicator = this.CreateNetworkIndicator();
			if (show)
				indicator.Show();
			return indicator;
		}

		public virtual Task AlertAsync(string message, string title, string okText)
		{
			var tcs = new TaskCompletionSource<object>();
			this.Alert(new AlertConfig {
					Message = message,
					Title = title,
					OkText = okText,
					OnOk = () => tcs.TrySetResult(null)
				});
			return tcs.Task;
		}

		public virtual Task AlertAsync(AlertConfig config)
		{
			var tcs = new TaskCompletionSource<object>();
			config.OnOk = () => tcs.TrySetResult(null);
			this.Alert(config);
			return tcs.Task;
		}

		public virtual Task<bool> ConfirmAsync(string message, string title, string okText, string cancelText)
		{
			var tcs = new TaskCompletionSource<bool>();
			this.Confirm(new ConfirmConfig {
					Message = message,
					Title = title,
					CancelText = cancelText,
					OkText = okText,
					OnConfirm = x => tcs.TrySetResult(x)
				});
			return tcs.Task;
		}

		public virtual Task<bool> ConfirmAsync(ConfirmConfig config)
		{
			var tcs = new TaskCompletionSource<bool>();
			config.OnConfirm = x => tcs.TrySetResult(x);
			this.Confirm(config);
			return tcs.Task;
		}

		public virtual Task<LoginResult> LoginAsync(string title, string message)
		{
			return this.LoginAsync(new LoginConfig {
					Title = title,
					Message = message
				});
		}

		public virtual Task<LoginResult> LoginAsync(LoginConfig config)
		{
			var tcs = new TaskCompletionSource<LoginResult>();
			config.OnResult = x => tcs.TrySetResult(x);
			this.Login(config);
			return tcs.Task;
		}

		public virtual Task<PromptResult> PromptAsync(string message, string title, string okText, string cancelText, string placeholder, InputType inputType)
		{
			var tcs = new TaskCompletionSource<PromptResult>();
			this.Prompt(new PromptConfig {
					Message = message,
					Title = title,
					CancelText = cancelText,
					OkText = okText,
					Placeholder = placeholder,
					InputType = inputType,
					OnResult = x => tcs.TrySetResult(x)
				});
			return tcs.Task;
		}

		public virtual Task<PromptResult> PromptAsync(PromptConfig config)
		{
			var tcs = new TaskCompletionSource<PromptResult>();
			config.OnResult = x => tcs.TrySetResult(x);
			this.Prompt(config);
			return tcs.Task;
		}
	}
}

