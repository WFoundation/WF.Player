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
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using WF.Player.Controls;
using WF.Player.Controls.Droid;

[assembly: ExportRendererAttribute (typeof (ExtendedLabel), typeof (ExtendedLabelRenderer))]

namespace WF.Player.Controls.Droid
{
	public class ExtendedLabelRenderer : LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement == null)
			{
				// The first time OnElementPropertyChanged isn't called for Text, so call it by ourselfs
				OnElementPropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs("Text"));
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName.Equals("Text") && Control != null)
			{
				var html = ((ExtendedLabel)Element).TextExt;

				Control.TextFormatted = global::Android.Text.Html.FromHtml(html);

				return;
			}

			base.OnElementPropertyChanged(sender, e);
		}

		private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{

		}
	}
}

