// <copyright file="MultiCellRenderer.cs" company="Wherigo Foundation">
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

[assembly: Xamarin.Forms.ExportRendererAttribute (typeof (WF.Player.SettingsPage.CheckCell), typeof (WF.Player.SettingsPage.iOS.CheckCellRenderer))]

namespace WF.Player.SettingsPage.iOS
{
	using UIKit;
	using Xamarin.Forms;
	using Xamarin.Forms.Platform.iOS;

	public class CheckCellRenderer : TextCellRenderer
	{
		public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
		{
			var cell = base.GetCell(item, reusableCell, tv);

			if (((CheckCell)item).Checkmark)
			{
				cell.Accessory = UIKit.UITableViewCellAccessory.Checkmark;
			}

			return cell;
		}

		protected override void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
		{
			base.HandlePropertyChanged(sender, args);

			if (args.PropertyName == "Checkmark")
			{
				((UITableViewCell)sender).Accessory = ((CheckCell)((CellTableViewCell)sender).Cell).Checkmark ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;
			}
		}
	}
}
