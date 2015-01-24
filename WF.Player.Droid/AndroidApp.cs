// <copyright file="AndroidApp.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player.Droid
{
	using System;
	using Android.App;
	using Android.Runtime;

	/// <summary>
	/// Android app.
	/// </summary>
	[Application]
	public class AndroidApp : Application
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.Droid.AndroidApp"/> class.
		/// </summary>
		/// <param name="javaReference">Java reference.</param>
		/// <param name="transfer">Transfer.</param>
		public AndroidApp(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
		{
		}

		/// <Docs>Called when the application is starting, before any activity, service,
		///  or receiver objects (excluding content providers) have been created.</Docs>
		/// <para tool="javadoc-to-mdoc">Called when the application is starting, before any activity, service,
		///  or receiver objects (excluding content providers) have been created.
		///  Implementations should be as quick as possible (for example using 
		///  lazy initialization of state) since the time spent in this function
		///  directly impacts the performance of starting the first activity,
		///  service, or receiver in a process.
		///  If you override this method, be sure to call super.onCreate().</para>
		/// <format type="text/html">[Android Documentation]</format>
		/// <since version="Added in API level 1"></since>
		/// <summary>
		/// Raises the create event.
		/// </summary>
		public override void OnCreate ()
		{
			base.OnCreate();
		}
	}
}

