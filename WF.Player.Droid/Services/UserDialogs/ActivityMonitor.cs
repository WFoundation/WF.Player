// <copyright file="ActivityMonitor.cs" company="Wherigo Foundation">
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

namespace WF.Player
{
	using System;
	using Android.App;
	using Android.Views;
	using Android.OS;

	internal class ActivityMonitor : Java.Lang.Object, Application.IActivityLifecycleCallbacks
	{
		public static Activity CurrentTopActivity { get; internal set; }

		public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
		{
			activity.RequestWindowFeature(WindowFeatures.Progress);
			activity.RequestWindowFeature(WindowFeatures.IndeterminateProgress);
			CurrentTopActivity = activity;
		}

		public void OnActivityResumed(Activity activity)
		{
			CurrentTopActivity = activity;
		}

		public void OnActivityPaused(Activity activity)
		{
		}

		public void OnActivityDestroyed(Activity activity)
		{
		}

		public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
		{
		}

		public void OnActivityStarted(Activity activity)
		{
		}

		public void OnActivityStopped(Activity activity)
		{
		}
	}
}
