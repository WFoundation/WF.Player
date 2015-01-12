// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz <mail@wfplayer.com>
//
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

[assembly: Xamarin.Forms.ExportRendererAttribute(typeof(WF.Player.Controls.ExtendedNavigationPage), typeof(WF.Player.Controls.Droid.ExtendedNavigationPageRenderer))]

namespace WF.Player.Controls.Droid
{
	using System;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.Android;
	using WF.Player.Droid;
	using System.Drawing;
	using Android.App;
	using WF.Player.Controls;

	public class ExtendedNavigationPageRenderer : NavigationRenderer
	{
		private bool animation;

		public ExtendedNavigationPageRenderer() : base()
		{
			animation = true;
		}

		protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
		{
			base.OnElementChanged(e);

			animation = ((ExtendedNavigationPage)Element).Animation;

//			((NavigationPage)Element).Pushed += HandlePushed;
//			((NavigationPage)Element).Popped += HandlePopped;
		}

//		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
//		{
//			base.OnElementPropertyChanged(sender, e);
//
//			if (e.PropertyName == "ShowBackButton" && !animation)
//			{
//				var tmp = Context;
//				UpdateBackButton(((ExtendedNavigationPage)Element).ShowBackButton);
//			}
//		}

		protected override System.Threading.Tasks.Task<bool> OnPopViewAsync (Page page, bool animated)
		{
			return base.OnPopViewAsync (page, animation);
		}

		protected override System.Threading.Tasks.Task<bool> OnPushAsync (Page page, bool animated)
		{
			return base.OnPushAsync (page, animation);
		}

//		private void HandlePushed(object sender, NavigationEventArgs e)
//		{
//			UpdateBackButton(((ExtendedNavigationPage)Element).ShowBackButton);
//		}
//
//		private void HandlePopped(object sender, NavigationEventArgs e)
//		{
//			UpdateBackButton(((ExtendedNavigationPage)Element).ShowBackButton);
//		}

//		protected override void Dispose(bool disposing)
//		{
//			if (disposing)
//			{
//				var navigationPage = (NavigationPage)Element;
//
//				navigationPage.Pushed -= HandlePushed;
//				navigationPage.Popped -= HandlePopped;
//			}
//
//			base.Dispose(disposing);
//		}

//		private void UpdateBackButton(bool show)
//		{
//			var bar = ((FormsApplicationActivity)Context).ActionBar;
//
//			bar.SetDisplayOptions(show ? ActionBarDisplayOptions.ShowHome : 0, ActionBarDisplayOptions.ShowHome);
//			bar.SetDisplayOptions(show ? ActionBarDisplayOptions.HomeAsUp : 0, ActionBarDisplayOptions.HomeAsUp);
//			bar.SetDisplayShowHomeEnabled(show);
//			bar.SetDisplayHomeAsUpEnabled(show);
//			bar.SetHomeButtonEnabled(show);
//		}
	}
}
