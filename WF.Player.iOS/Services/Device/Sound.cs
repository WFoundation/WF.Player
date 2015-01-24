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

using System;
using Xamarin.Forms;
using WF.Player.Services.Device;
using WF.Player.Core;
using Foundation;
using AVFoundation;
using AudioToolbox;
using UIKit;

[assembly: Dependency(typeof(WF.Player.iOS.Services.Device.Sound))]

namespace WF.Player.iOS.Services.Device
{
	public class Sound : ISound
	{
		static SystemSound _click = SystemSound.FromFile("Sounds/tap.aif");

		AVAudioPlayer _soundPlayer;

		public Sound ()
		{
		}

		public void PlaySound(Media media)
		{
			NSError error;

			if (_soundPlayer != null) {
				_soundPlayer.Stop();
				_soundPlayer = null;
			}

			if (media == null || media.Data == null || media.Data.Length == 0)
				return;

			try {
				_soundPlayer = AVAudioPlayer.FromData(NSData.FromArray (media.Data), out error);
			}
			catch (Exception e) {
			}

			if (_soundPlayer != null)
				_soundPlayer.Play ();
//			else
//				throw new InvalidCastException(String.Format ("Audio file format of media {0} is not valid",media.Name));
		}

		public void PlayAlert()
		{
			// TODO
//			_sound.PlayAlertSound();
//			SystemSound.FromFile("Sounds/tap.aif").PlayAlertSound();
		}

		public void PlayKeyboardSound(int duration = 250)
		{
			_click.PlaySystemSound();
		}

		public void StopSound()
		{
			if (_soundPlayer != null && _soundPlayer.Playing) {
				_soundPlayer.Stop ();
				_soundPlayer = null;
			}
		}

	}
}

