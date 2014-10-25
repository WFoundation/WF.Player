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
using Android.Media;
using Android.Content;

[assembly: Dependency(typeof(WF.Player.Droid.Services.Device.Sound))]

namespace WF.Player.Droid.Services.Device
{
	public class Sound : ISound
	{
		MediaPlayer _soundPlayer;

		public Sound ()
		{
		}

		public async void PlaySound(Media media)
		{
			if (media == null || media.Data == null || media.Data.Length == 0)
				return;

			if (_soundPlayer != null) {
				_soundPlayer.Stop ();
				_soundPlayer.Reset ();
			} else {
				_soundPlayer = new MediaPlayer ();
			}

			try {
				// Open file and read from FileOffset FileSize bytes for the media
				using (Java.IO.RandomAccessFile file = new Java.IO.RandomAccessFile (media.FileName, "r")) {
					await _soundPlayer.SetDataSourceAsync(file.FD,media.FileOffset,media.FileSize);
					file.Close();
				}

				// Start media
				if (_soundPlayer != null) {
					_soundPlayer.Prepare();
					_soundPlayer.Start ();
				} else
					throw new InvalidCastException(String.Format ("Audio file format of media {0} is not valid", media.Name));
			} catch (Exception ex) {
				String s = ex.ToString();
			}
		}

		public void PlayAlert()
		{
			var alarm = RingtoneManager.GetRingtone(Forms.Context, RingtoneManager.GetDefaultUri(RingtoneType.Alarm));
			alarm.Play ();
		}

		public void PlayKeyboardSound(int duration = 250)
		{
			AudioManager am = (AudioManager)Forms.Context.GetSystemService(Context.AudioService);

			float vol = 1f; //This will be half of the default system sound

			am.PlaySoundEffect(SoundEffect.KeyClick, vol);
		}

		public void StopSound()
		{
			if (_soundPlayer != null && _soundPlayer.IsPlaying) {
				_soundPlayer.Stop ();
				_soundPlayer.Release();
				_soundPlayer = null;
			}
		}

	}
}

