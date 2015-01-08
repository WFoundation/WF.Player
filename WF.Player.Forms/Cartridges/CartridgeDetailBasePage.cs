// <copyright file="CartridgeDetailBasePage.cs" company="Wherigo Foundation">
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
using Xamarin.Forms;

namespace WF.Player
{
	using System;
	using WF.Player.Core;
	using WF.Player.Core.Utils;

	/// <summary>
	/// Cartridge detail base page, which is holding routing/resume/start buttons and direction, 
	/// if the cartridge isn't play anywhere.
	/// </summary>
	public class CartridgeDetailBasePage : DirectionBarPage
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.CartridgeDetailBasePage"/> class.
		/// </summary>
		/// <param name="viewModel">View model.</param>
		public CartridgeDetailBasePage(CartridgeDetailViewModel viewModel)
		{
			this.BindingContext = viewModel;

			// Show empty string as back button title (default "Back" would be null as string)
			NavigationPage.SetBackButtonTitle(this, string.Empty);

			var buttonResume = new ToolIconButton("IconResume.png", viewModel.ResumeCommand);
			var buttonStart = new ToolIconButton("IconPlay.png", viewModel.StartCommand);

			// Activate bottom items if it isn't a play anywhere cartridge
			if (viewModel.IsPlayAnywhere)
			{
				Buttons.Add(buttonResume);
				Buttons.Add(buttonStart);
				DirectionLayout.IsVisible = false;
				DirectionSpaceLayout.IsVisible = false;
			}
			else
			{
				Buttons.Add(new ToolIconButton("IconRouting.png", viewModel.RoutingCommand));
				Buttons.Add(buttonResume);
				Buttons.Add(buttonStart);
				DirectionLayout.IsVisible = true;
				DirectionSpaceLayout.IsVisible = true;
			}

			// Exists a save file
			// TODO
			if (false)
			{
				buttonResume.IsEnabled = true;
			}
			else
			{
				buttonResume.IsEnabled = false;
			}
		}

		#endregion
	}
}