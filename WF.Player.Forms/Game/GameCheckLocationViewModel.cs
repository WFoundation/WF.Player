// <copyright file="GameCheckLocationViewModel.cs" company="Wherigo Foundation">
// 	WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// 	Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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

namespace WF.Player
{
	using Vernacular;
	using WF.Player.Models;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;

	/// <summary>
	/// Game check location view model.
	/// </summary>
	public class GameCheckLocationViewModel : BaseViewModel
	{
		#region Public

		/// <summary>
		/// The name of the position property.
		/// </summary>
		public const string PositionPropertyName = "Position";

		/// <summary>
		/// The name of the button text property.
		/// </summary>
		public const string ButtonTextPropertyName = "ButtonText";

		/// <summary>
		/// The name of the button text color property.
		/// </summary>
		public const string ButtonTextColorPropertyName = "ButtonTextColor";

		/// <summary>
		/// The name of the is running property.
		/// </summary>
		public const string IsRunningPropertyName = "IsRunning";

		/// <summary>
		/// The start name of the command property.
		/// </summary>
		public const string StartCommandPropertyName = "StartCommand";

		#endregion

		#region Private

		/// <summary>
		/// The cartridge, which should be started.
		/// </summary>
		private CartridgeTag cartridgeTag;

		/// <summary>
		/// The savegame.
		/// </summary>
		private CartridgeSavegame savegame;

		/// <summary>
		/// The actual position.
		/// </summary>
		private Position position;

		/// <summary>
		/// Flag for cartridge is started or not.
		/// </summary>
		private bool started = false;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameCheckLocationViewModel"/> class.
		/// </summary>
		/// <param name="tag">CartridgeTag to use.</param>
		/// <param name="savegame">Savegame object to use for restore.</param>
		/// <param name="lastPage">Info about which was the last page before call of check location</param>
		public GameCheckLocationViewModel(CartridgeTag tag, CartridgeSavegame savegame = null, Page lastPage = null)
		{
			this.cartridgeTag = tag;
			this.savegame = savegame;

			App.GPS.PositionChanged += OnPositionChanged;
			Position = App.GPS.LastKnownPosition;
		}

		#endregion

		#region Properties

		#region Position

		/// <summary>
		/// Gets the Position from the actual location.
		/// </summary>
		/// <value>The Position.</value>
		public Position Position 
		{
			get 
			{
				return position;
			}

			internal set 
			{
				SetProperty<Position>(ref position, value, PositionPropertyName);
				NotifyPropertyChanged(ButtonTextPropertyName);
				NotifyPropertyChanged(ButtonTextColorPropertyName);
			}
		}

		#endregion

		#region ButtonText

		/// <summary>
		/// Gets othe text of the start button.
		/// </summary>
		/// <value>The button text.</value>
		public string ButtonText 
		{
			get 
			{
				if (Position != null && Position.Accuracy <= 30.0) 
				{
					return Catalog.GetString("Start");
				} 
				else 
				{
					return Catalog.GetString("Start anyway");
				}
			}
		}

		#endregion

		#region ButtonText

		/// <summary>
		/// Gets othe text of the start button.
		/// </summary>
		/// <value>The button text.</value>
		public Color ButtonTextColor 
		{
			get 
			{
				if (Position != null && Position.Accuracy <= 30.0) 
				{
					return Color.Green;
				} 
				else 
				{
					return Color.Red;
				}
			}
		}

		#endregion

		#endregion

		#region Commands

		#region Start Command

		/// <summary>
		/// Gets the start command.
		/// </summary>
		/// <value>The start command.</value>
		public Xamarin.Forms.Command StartCommand
		{
			get
			{
				return new Xamarin.Forms.Command(async (sender) =>
					{
						App.GPS.PositionChanged -= OnPositionChanged;

						// Create GameModel
						App.Game = new GameModel(this.cartridgeTag);

						// Create game main view with model
						var gameMainViewModel = new GameMainViewModel(App.Game);
						var gameMainView = new GameMainView(gameMainViewModel);

						// Create a new navigation page for the game
						App.GameNavigation = new ExtendedNavigationPage(gameMainView)
							{
								BarBackgroundColor = App.Colors.Bar,
								BarTextColor = App.Colors.BarText,
							};

						// Remove check location from screen 
						App.Navigation.PopAsync(false);

						// Push main view to screen
						await App.Navigation.CurrentPage.Navigation.PushModalAsync(App.GameNavigation, true);

						// Remove check location from screen 
//						App.Navigation.Navigation.RemovePage(App.Navigation.Navigation.NavigationStack[App.Navigation.Navigation.NavigationStack.Count-1]);

						gameMainViewModel.Refresh();

						started = true;

						// StartGame
						await App.Game.StartAsync(this.savegame);
					});
			}
		}

		#endregion

		#endregion

		public override void OnAppearing()
		{
			base.OnAppearing();

//			if (started)
//			{
//				started = false;
//				App.Navigation.Navigation.PopAsync();
//			}
		}

		#region Private Functions

		/// <summary>
		/// Handles the event, when position changed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position event args.</param>
		private void OnPositionChanged(object sender, PositionEventArgs e)
		{
			Position = e.Position;
		}

		#endregion
	}
}
