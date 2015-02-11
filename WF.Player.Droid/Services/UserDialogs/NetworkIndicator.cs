// <copyright file="NetworkIndicator.cs" company="Wherigo Foundation">
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

namespace WF.Player.Droid.Services.UserDialogs
{
	using System;
	using WF.Player.Services.UserDialogs;
	using Android.App;

	public class NetworkIndicator : INetworkIndicator
	{
		private readonly Activity activity;

		public NetworkIndicator(Activity activity)
		{
			this.activity = activity;
		}

		public bool IsShowing { get; private set; }

		public void Show()
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					this.IsShowing = true;
					this.activity.SetProgressBarVisibility(true);
					this.activity.SetProgressBarIndeterminateVisibility(true);
				});
		}

		public void Hide()
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					this.IsShowing = false;
					this.activity.SetProgressBarIndeterminateVisibility(false);
					this.activity.SetProgressBarVisibility(false);
				});
		}

		public void Dispose()
		{
			this.Hide();
		}
	}
}

