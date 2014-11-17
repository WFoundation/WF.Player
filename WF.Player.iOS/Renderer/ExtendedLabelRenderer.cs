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
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using WF.Player.Controls;
using WF.Player.Controls.iOS;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

[assembly: ExportRendererAttribute (typeof (ExtendedLabel), typeof (ExtendedLabelRenderer))]

namespace WF.Player.Controls.iOS
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
				// http://forums.xamarin.com/discussion/15530/nsattributedstringdocumentattributes-exception-on-ios-6

				NSParagraphStyle ps = NSParagraphStyle.Default;

				NSDictionary dict = new NSMutableDictionary() { {
						UIStringAttributeKey.Font,
						((ExtendedLabel)Element).Font.ToUIFont()
					},
				};

				var attr = new NSAttributedStringDocumentAttributes(dict);
				var nsError = new NSError();

				// This line announces, that content is html.
				attr.DocumentType = NSDocumentType.HTML;
				attr.StringEncoding = NSStringEncoding.UTF8;

				var html = ((ExtendedLabel)Element).TextExt + Environment.NewLine;

				NSString htmlString = new NSString(html); 
				NSData htmlData = htmlString.DataUsingEncoding(NSStringEncoding.UTF8); 

				NSAttributedString attrStr = new NSAttributedString(htmlData, attr, out dict, ref nsError);

				Control.AttributedText = attrStr;
				Control.SetNeedsLayout();

				return;
			}

			if (e.PropertyName == "Height" || e.PropertyName == "Width")
			{
				// We calculate the correct height, because of the attributed string, Xamarin.Forms don't do it correct
				var width = (float)((ExtendedLabel)Element).Width;

				// Only do this, if we have a valid width
				if (width != -1)
				{
					var rect = Control.AttributedText.GetBoundingRect(new System.Drawing.SizeF(width, float.MaxValue), NSStringDrawingOptions.UsesLineFragmentOrigin | NSStringDrawingOptions.UsesFontLeading, null);

					if (rect.Height != ((ExtendedLabel)Element).Height)
					{
						((ExtendedLabel)Element).HeightRequest = rect.Height;
					}
				}
			}

			base.OnElementPropertyChanged(sender, e);
		}

		private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{

		}
	}
}

