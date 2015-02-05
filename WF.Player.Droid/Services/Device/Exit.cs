// <copyright file="Exit.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.Dependency(typeof(WF.Player.Droid.Services.Device.Exit))]

namespace WF.Player.Droid.Services.Device
{
	using Android.Content;
	using WF.Player.Services.Device;
	using Xamarin.Forms;

	public class Exit : IExit
	{
		#region IExit implementation

		///<summary>
		///Exit the running app with exitCode.
		///</summary>
		///<param name="exitCode">Exit code.</param>
		public void ExitApp(int exitCode)
		{
			Intent intent = new Intent(Intent.ActionMain);
			intent.AddCategory(Intent.CategoryHome);
			intent.SetFlags(ActivityFlags.ClearTop);
			Forms.Context.StartActivity(intent);
			((MainActivity)Forms.Context).Finish();
			System.Environment.Exit(exitCode);
		}

		#endregion
	}
}

