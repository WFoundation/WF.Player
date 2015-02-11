// <copyright file="IUserDialogs.cs" company="Wherigo Foundation">
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

namespace WF.Player.Services.UserDialogs
{
	using System;
	using System.Threading.Tasks;

	public interface IUserDialogs
	{
		void Alert(string message, string title = null, string okText = "Ok");

		void Alert(AlertConfig config);

		void ActionSheet(ActionSheetConfig config);

		void Confirm(ConfirmConfig config);

		void Prompt(PromptConfig config);

		void Login(LoginConfig config);
		//void DateTimePrompt(DateTimePromptConfig config);
		//void DurationPrompt(DurationPromptConfig config);
		INetworkIndicator NetworkIndication(bool show = true);

		Task AlertAsync(string message, string title = null, string okText = "Ok");

		Task AlertAsync(AlertConfig config);

		Task<bool> ConfirmAsync(string message, string title = null, string okText = "Ok", string cancelText = "Cancel");

		Task<bool> ConfirmAsync(ConfirmConfig config);

		Task<LoginResult> LoginAsync(string title = "Login", string message = null);

		Task<LoginResult> LoginAsync(LoginConfig config);

		Task<PromptResult> PromptAsync(string message, string title = null, string okText = "Ok", string cancelText = "Cancel", string placeholder = "", InputType inputType = InputType.Default);

		Task<PromptResult> PromptAsync(PromptConfig config);
	}
}

