// <copyright file="GameDetailViewModel.cs" company="Wherigo Foundation">
// WF.Player - A Wherigo Player which use the Wherigo Foundation Core.
// Copyright (C) 2012-2014  Dirk Weltz (mail@wfplayer.com)
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
// 	You should have received a copy of the GNU Lesser General Public License
// 	along with this program.  If not, see <http://www.gnu.org/licenses/>.
using WF.Player.Services.Settings;

namespace WF.Player
{
	using System;
	using System.IO;
	using WF.Player.Services.UserDialogs;
	using Vernacular;
	using WF.Player.Core;
	using WF.Player.Core.Utils;
	using WF.Player.Services.Device;
	using WF.Player.Services.Geolocation;
	using Xamarin.Forms;

	/// <summary>
	/// Game detail view model.
	/// </summary>
	public class GameDetailViewModel : BaseViewModel
	{
		/// <summary>
		/// The name of the name property.
		/// </summary>
		public const string NamePropertyName = "Name";

		/// <summary>
		/// The name of the description property.
		/// </summary>
		public const string DescriptionPropertyName = "Description";

		/// <summary>
		/// The name of the description property.
		/// </summary>
		public const string HasDescriptionPropertyName = "HasDescription";

		/// <summary>
		/// The name of the image source property.
		/// </summary>
		public const string ImageSourcePropertyName = "ImageSource";

		/// <summary>
		/// The name of the html source property.
		/// </summary>
		public const string HtmlSourcePropertyName = "HtmlSource";

		/// <summary>
		/// The name of the has image property.
		/// </summary>
		public const string HasImagePropertyName = "HasImage";

		/// <summary>
		/// The name of the position property.
		/// </summary>
		public const string PositionPropertyName = "Position";

		/// <summary>
		/// The name of the has direction property.
		/// </summary>
		public const string HasDirectionPropertyName = "HasDirection";

		/// <summary>
		/// The name of the direction property.
		/// </summary>
		public const string DirectionPropertyName = "Direction";

		/// <summary>
		/// The name of the distance property.
		/// </summary>
		public const string DistancePropertyName = "Distance";

		/// <summary>
		/// The active object.
		/// </summary>
		private UIObject activeObject;

		/// <summary>
		/// The geo math helper.
		/// </summary>
		private GeoMathHelper geoMathHelper;

		/// <summary>
		/// The position.
		/// </summary>
		private Position position;

		/// <summary>
		/// The location vector for direction to active object.
		/// </summary>
		private LocationVector vecDirection;

		/// <summary>
		/// The has direction.
		/// </summary>
		private bool hasDirection;

		/// <summary>
		/// The direction.
		/// </summary>
		private double direction;

		/// <summary>
		/// The distance.
		/// </summary>
		private double distance;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameDetailViewModel"/> class.
		/// </summary>
		public GameDetailViewModel(UIObject activeObject = null)
		{
			this.geoMathHelper = new GeoMathHelper();
			this.activeObject = activeObject;

			Position = App.GPS.LastKnownPosition;
		}

		#endregion

		#region Properties

		#region ActiveObject

		/// <summary>
		/// Gets or sets the active object.
		/// </summary>
		/// <value>The active object.</value>
		public UIObject ActiveObject 
		{
			get 
			{
				return this.activeObject;
			}

			set 
			{
				// Remove method to active object property changed events
				if (this.activeObject != null)
				{
					this.activeObject.PropertyChanged -= HandlePropertyChanged;
				}

				// Set property
				SetProperty<UIObject>(ref this.activeObject, value);

				// Only go on, if there is a valid active object
				if (this.activeObject == null) 
				{
					// If there isn't an active object, we don't need a update (problems with ShowScreen(Mainscreen) for "Catch me - if you can")
					App.GPS.HeadingChanged -= HandleHeadingChanged;
					App.GPS.PositionChanged -= HandlePositionChanged;
					return;
				}

				// Add method to active object property changed events
				this.activeObject.PropertyChanged += HandlePropertyChanged;

				vecDirection = null;

				// Update all views
				NotifyPropertyChanged(NamePropertyName);

				#if __HTML__
				NotifyPropertyChanged(HtmlSourcePropertyName);
				#else
				NotifyPropertyChanged(DescriptionPropertyName);
				NotifyPropertyChanged(HasDescriptionPropertyName);
				NotifyPropertyChanged(ImageSourcePropertyName);
				NotifyPropertyChanged(HasImagePropertyName);
				#endif

				UpdateHasDirection();
				UpdateDirection(true);
				UpdateCommands();
				}
			}

		#endregion

		#region Name

		/// <summary>
		/// Gets the name of the active object.
		/// </summary>
		/// <value>The name.</value>
		public string Name 
		{
			get 
			{
				if (this.activeObject == null)
				{
					return string.Empty;
				}

				if (this.activeObject is Task) 
				{
					if (((Task)this.activeObject).Complete) 
					{
						var result = this.activeObject.Name;
						switch (((Task)this.activeObject).CorrectState) 
						{
							case TaskCorrectness.Correct:
								result = App.Chars.TaskCorrect + result;
								break;
							case TaskCorrectness.NotCorrect:
								result = App.Chars.TaskNotCorrect + result;
								break;
							case TaskCorrectness.None:
								result = App.Chars.TaskNone + result;
								break;
						}

						return result;
					} 
					else
					{
						return this.activeObject.Name;
					}
				} 
				else
				{
					return this.activeObject.Name;
				}
			}
		}

		#endregion

		#region Description

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description 
		{
			get 
			{
				return this.activeObject != null ? this.activeObject.Description : string.Empty;
			}
		}

		#endregion

		#region HasDescription

		/// <summary>
		/// Gets a value indicating whether this instance has description.
		/// </summary>
		/// <value><c>true</c> if this instance has description; otherwise, <c>false</c>.</value>
		public bool HasDescription 
		{
			get 
			{
				// Do this, because if not, than the layout process isn't started for the direction arrow
				// and the direction arrow is placed mostly outside the screen
				if (!HasImage && (this.activeObject == null || string.IsNullOrEmpty(this.activeObject.Description)))
				{
					return true;
				}

				return this.activeObject != null && !string.IsNullOrEmpty(this.activeObject.Description);
			}
		}

		#endregion

		#region ImageSource

		/// <summary>
		/// Gets the poster of cartridge.
		/// </summary>
		/// <value>The poster.</value>
		public ImageSource ImageSource
		{
			get 
			{
				if (!HasImage)
				{
					return null;
				}

				return App.Game.GetImageSourceForMedia(ActiveObject.Media);
			}
		}

		#endregion

		#region HtmlSource

		/// <summary>
		/// Gets the description of object as Html.
		/// </summary>
		/// <value>The Html representation of the description.</value>
		public HtmlWebViewSource HtmlSource
		{
			get 
			{
				var result = new HtmlWebViewSource();

				if (!string.IsNullOrEmpty(ActiveObject.Html))
				{
					result.Html = ConverterToHtml.FromHtml(ActiveObject.Html);
				}
				else
				{
					if (!string.IsNullOrEmpty(ActiveObject.Markdown))
					{
						result.Html = ConverterToHtml.FromMarkdown(ActiveObject.Markdown, Settings.TextAlignment, ActiveObject.Media);
					}
					else
					{
						result.Html = ConverterToHtml.FromText(ActiveObject.Description, Settings.TextAlignment, ActiveObject.Media);
					}
				}

				return result;
			}
		}

		#endregion

		#region HasImage

		/// <summary>
		/// Gets a value indicating whether this cartridge has poster.
		/// </summary>
		/// <value><c>true</c> if this cartridge has poster; otherwise, <c>false</c>.</value>
		public bool HasImage 
		{ 
			get
			{ 
				return this.activeObject != null && this.activeObject.Media != null && this.activeObject.Media.Data != null; 
			}
		}

		#endregion

		#region Position

		/// <summary>
		/// Gets the position from the actuell location.
		/// </summary>
		/// <value>The Position.</value>
		public Position Position
		{
			get 
			{
				return this.position;
			}

			internal set 
			{
				SetProperty<Position>(ref this.position, value, PositionPropertyName);
			}
		}

		#endregion

		#region HasDirection

		/// <summary>
		/// Gets a value indicating whether this instance has direction.
		/// </summary>
		/// <value><c>true</c> if this instance has direction; otherwise, <c>false</c>.</value>
		public bool HasDirection 
		{
			get 
			{
				return this.hasDirection;
			}

			internal set 
			{
				SetProperty<bool>(ref this.hasDirection, value, HasDirectionPropertyName);
			}
		}

		#endregion

		#region Direction

		/// <summary>
		/// Gets the direction from the actuell location to the cartridge.
		/// </summary>
		/// <value>The distance.</value>
		public double Direction 
		{
			get 
			{
				return this.direction;
			}

			internal set 
			{
				SetProperty<double>(ref this.direction, value, DirectionPropertyName);
			}
		}

		#endregion

		#region Distance

		/// <summary>
		/// Gets the distance from the actuell location to the cartridge.
		/// </summary>
		/// <value>The distance.</value>
		public double Distance 
		{
			get 
			{
				return this.distance;
			}

			internal set 
			{
				SetProperty<double>(ref this.distance, value, DistancePropertyName);
			}
		}

		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		public override void OnAppearing()
		{
			base.OnAppearing();

			App.GPS.PositionChanged += HandlePositionChanged;
			App.GPS.HeadingChanged += HandleHeadingChanged;

			if (this.activeObject != null)
			{
				this.activeObject.PropertyChanged += HandlePropertyChanged;
			}

			vecDirection = null;

			NotifyPropertyChanged(NamePropertyName);

			#if __HTML__
			NotifyPropertyChanged(HtmlSourcePropertyName);
			#else
			NotifyPropertyChanged(DescriptionPropertyName);
			NotifyPropertyChanged(HasDescriptionPropertyName);
			NotifyPropertyChanged(HasImagePropertyName);
			NotifyPropertyChanged(ImageSourcePropertyName);
			#endif

			NotifyPropertyChanged(HasDirectionPropertyName);

			UpdateHasDirection();
			UpdateDirection(true);
		}

		/// <summary>
		/// Raises the appeared event.
		/// </summary>
		public override void OnAppeared()
		{
			base.OnAppeared();

			UpdateCommands();
		}

		/// <summary>
		/// Raises the disappearing event.
		/// </summary>
		public override void OnDisappearing()
		{
			// We are off screen, so don't listen to updates anymore
			if (this.activeObject != null)
			{
				this.activeObject.PropertyChanged -= HandlePropertyChanged;
			}

			App.GPS.HeadingChanged -= HandleHeadingChanged;
			App.GPS.PositionChanged -= HandlePositionChanged;

			base.OnDisappearing();
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Handles the property changed.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Property changed event arguments.</param>
		private void HandlePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (this.activeObject == null)
			{
				return;
			}

			// Commands are changed
			if (e.PropertyName == "Commands" || e.PropertyName == "ActiveCommands")
			{
				UpdateCommands();
			}

			if (e.PropertyName == "Container")
			{
				UpdateHasDirection();
			}

			// Points for Zone changed
			if (e.PropertyName == "Points")
			{
				UpdateDirection(true);
			}

			#if __HTML__

			if (e.PropertyName == "Description" || e.PropertyName == "Media")
			{
				NotifyPropertyChanged(HtmlSourcePropertyName);
			}

			#else

			if (e.PropertyName == "Media")
			{
				NotifyPropertyChanged(ImageSourcePropertyName);
			}

			#endif

			// Update in all other cases
			NotifyPropertyChanged(e.PropertyName);
		}

		/// <summary>
		/// Handles the position changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position event arguments.</param>
		private void HandlePositionChanged(object sender, PositionEventArgs e)
		{
			if (this.activeObject == null)
			{
				return;
			}

			Position = e.Position;

			UpdateDirection(true);
		}

		/// <summary>
		/// Handles the position changed event.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Position event arguments.</param>
		private void HandleHeadingChanged(object sender, PositionEventArgs e)
		{
			if (this.activeObject == null)
			{
				return;
			}

			Position = e.Position;

			UpdateDirection(false);
		}

		/// <summary>
		/// Handles the click of toolbar button, if there are more than one command active.
		/// </summary>
		/// <param name="obj">Object, which is clicked.</param>
		private void HandleCommandsClicked(object obj)
		{
			// Now show a list with all active commands
			var cfg = new ActionSheetConfig().SetTitle(Catalog.GetString("Actions"));

			foreach (WF.Player.Core.Command c in ((Thing)this.activeObject).ActiveCommands)
			{
				cfg.Add(
					c.Text, 
					() => 
					{
						ExecuteCommand(c);
					});
			}

			cfg.Cancel = new ActionSheetOption(Catalog.GetString("Cancel"), App.Click);

			UserDialogs.Instance.ActionSheet(cfg);
		}

		/// <summary>
		/// Executes the selected command.
		/// </summary>
		/// <param name="command">Command to execute.</param>
		private async void ExecuteCommand(WF.Player.Core.Command command)
		{
			if (command == null)
			{
				return;
			}

			// Notify user
			App.Click();

			if (command.CmdWith) 
			{
				if (command.TargetObjects.Count > 0) 
				{
					// There are one or more targets for this command
					var cfg = new ActionSheetConfig().SetTitle(command.Text);

					foreach (Thing t in command.TargetObjects)
					{
						cfg.Add(
							t.Name, 
							() => 
							{
								App.Click();
								command.Execute(t);
							});
					}

					cfg.Cancel = new ActionSheetOption(Catalog.GetString("Cancel"), App.Click);

					UserDialogs.Instance.ActionSheet(cfg);
				} 
				else 
				{
					// There are no target for this command
					await UserDialogs.Instance.AlertAsync(command.EmptyTargetListText, command.Text, Catalog.GetString("Ok"));
					App.Click();
				}
			} 
			else 
			{
				command.Execute();
			}
		}

		/// <summary>
		///  Update direction for active object
		/// </summary>
		/// <param name="updateDirection">Flag, if the direction should be calculated new.</param>
		private void UpdateDirection(bool updateDirection)
		{
			if (this.activeObject == null)
			{
				return;
			}

			if (Position == null)
			{
				Direction = double.NegativeInfinity;
				Distance = double.NegativeInfinity;

				return;
			}

			// Do it only for zones, where we are inside
			if (this.activeObject is Zone && ((Zone)this.activeObject).State == PlayerZoneState.Inside)
			{
				Direction = double.PositiveInfinity;
				Distance = double.PositiveInfinity;

				return;
			}

			double heading = 0;

			if (Position.Heading != null)
			{
				// Show always to north
				heading = 360.0 - (double)Position.Heading;
			}

			// Do it only for entries with ObjectLocation
			if (this.activeObject is Zone || this.activeObject.ObjectLocation != null)
			{
				if (updateDirection || vecDirection == null)
				{
					// Calculate values for this thing
					vecDirection = this.geoMathHelper.VectorToPoint(new ZonePoint(Position.Latitude, Position.Longitude, 0), this.activeObject.ObjectLocation);
				}

				// Set values
				Direction = (double)((vecDirection.Bearing + heading) % 360);
				Distance = vecDirection.Distance.Value;
			}
		}

		/// <summary>
		///  Update HasDirection for active object
		/// </summary>
		private void UpdateHasDirection()
		{
			if (this.activeObject == null)
			{
				return;
			}

			if (this.activeObject is Task)
			{
				HasDirection = false;

				return;
			}

			if (this.activeObject is Zone)
			{
				HasDirection = true;

				return;
			}

			if (this.activeObject is Thing)
			{
				HasDirection = this.activeObject != null && this.activeObject.ObjectLocation != null && !App.Game.VisibleInventory.Contains((Thing)this.activeObject);
			}
		}

		/// <summary>
		/// Updates the commands.
		/// </summary>
		private void UpdateCommands()
		{
			if (this.activeObject == null)
			{
				return;
			}

			if (!(App.GameNavigation.CurrentPage is GameDetailView))
			{
				return;
			}

			// Clear all buttons

			// Get active view
			var view = (GameDetailView)App.GameNavigation.CurrentPage;

			// Clear all buttons
			view.Buttons.Clear();

			// If there are commands, than create buttons
			if (this.activeObject is Thing)
			{
				Thing thing = (Thing)this.activeObject;

				// If there isn't a command, we are ready
				if (thing.ActiveCommands.Count == 0)
				{
					return;
				}

				// We have actions, so set the overflow menu text
				view.OverflowMenuText = Catalog.GetString("Actions");

				// Add all commands. Each command creates one button.
				foreach (WF.Player.Core.Command c in thing.ActiveCommands)
				{
					view.Buttons.Add(new ToolTextButton(c.Text, new Xamarin.Forms.Command(() => ExecuteCommand(c))));
				}
			}
		}

		#endregion
	}
}
