// <copyright file="GameInputViewRenderer.cs" company="Wherigo Foundation">
//   WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
//   Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
// </copyright>
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

[assembly: Xamarin.Forms.ExportRendererAttribute (typeof (WF.Player.GameInputView), typeof (WF.Player.iOS.GameInputViewRenderer))]

namespace WF.Player.iOS
{
	using System;
	using Xamarin.Forms;
	using WF.Player;
	using WF.Player.iOS;
	using MonoTouch.Foundation;
	using MonoTouch.UIKit;

	public class GameInputViewRenderer : BottomBarPageRenderer
	{
		private double originalSize;
		private NSObject observerHideKeyboard;
		private NSObject observerShowKeyboard;

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);

			// register for keyboard notifications
			observerHideKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, OnKeyboardNotification);
			observerShowKeyboard = NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillShowNotification, OnKeyboardNotification);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			NSNotificationCenter.DefaultCenter.RemoveObserver(observerHideKeyboard);
			NSNotificationCenter.DefaultCenter.RemoveObserver(observerShowKeyboard);
		}

		private void OnKeyboardNotification (NSNotification notification)
		{
			if (!IsViewLoaded) return;

			//Check if the keyboard is becoming visible
			var visible = notification.Name == UIKeyboard.WillShowNotification;

			//Pass the notification, calculating keyboard height, etc.
			bool landscape = InterfaceOrientation == UIInterfaceOrientation.LandscapeLeft || InterfaceOrientation == UIInterfaceOrientation.LandscapeRight;
			var keyboardFrame = visible
				? UIKeyboard.FrameEndFromNotification(notification)
				: UIKeyboard.FrameBeginFromNotification(notification);

			OnKeyboardChanged (visible, landscape ? keyboardFrame.Width : keyboardFrame.Height);
		}

		/// <summary>
		/// Override this method to apply custom logic when the keyboard is shown/hidden
		/// </summary>
		/// <param name='visible'>
		/// If the keyboard is visible
		/// </param>
		/// <param name='keyboardHeight'>
		/// Calculated height of the keyboard (width not generally needed here)
		/// </param>
		protected virtual void OnKeyboardChanged (bool visible, float keyboardHeight)
		{
			var element = ((GameInputView)Element);

			element.KeyboardHeight = visible ? keyboardHeight : 0;

			// Do this, because the ScrollView isn't handled correct with Xamarin.Forms
			var bounds = element.ContentLayout.Bounds;

			element.ScrollLayout.LayoutTo(bounds, 0);
		}

	}
}

