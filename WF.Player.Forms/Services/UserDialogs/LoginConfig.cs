// <copyright file="LoginConfig.cs" company="Wherigo Foundation">
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
	using Vernacular;

	public class LoginConfig
	{
		public string Title { get; set; }

		public string Message { get; set; }

		public string OkText { get; set; }

		public string CancelText { get; set; }

		public string LoginValue { get; set; }

		public string LoginPlaceholder { get; set; }

		public string PasswordPlaceholder { get; set; }

		public Action<LoginResult> OnResult { get; set; }

		public LoginConfig()
		{
			this.Title = Catalog.GetString("Login");
			this.OkText = Catalog.GetString("Ok");
			this.CancelText = Catalog.GetString("Cancel");
			this.LoginPlaceholder = Catalog.GetString("User Name");
			this.PasswordPlaceholder = Catalog.GetString("Password");
		}
	}
}
