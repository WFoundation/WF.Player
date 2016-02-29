// <copyright file="UserDialogs.cs" company="Wherigo Foundation">
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
#if __ANDROID__
	using Android.App;
	using WF.Player.Droid.Services.UserDialogs;
#endif
#if __IOS__
	using WF.Player.iOS.Services.UserDialogs;
#endif
#if __WINPHONE8__
    using WF.Player.WinPhone8.Services.UserDialogs;
#endif

    public static class UserDialogs
	{
		#if __ANDROID__
		public static void Init(Func<Activity> getActivity)
		{
			if (Instance == null)
				Instance = new UserDialogService(getActivity);
		}

		public static void Init(Activity activity)
		{
			if (Instance != null)
				return;
			var app = Application.Context.ApplicationContext as Application;
			if (app == null)
				throw new Exception("Application Context is not an application");
			ActivityMonitor.CurrentTopActivity = activity;
			app.RegisterActivityLifecycleCallbacks(new ActivityMonitor());
			Instance = new UserDialogService(() => ActivityMonitor.CurrentTopActivity);
		}
		#else
		public static void Init() 
		{
			if (Instance == null)
			{
				Instance = new UserDialogService();
			}
		}
		#endif
		public static IUserDialogs Instance { get; set; }

		internal static void TryExecute(this Action action)
		{
			if (action != null)
				action();
		}
	}
}

