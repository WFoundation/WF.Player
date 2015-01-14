// <copyright file="GameMainCellViewModel.cs" company="Wherigo Foundation">
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
// You should have received a copy of the GNU Lesser General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace WF.Player
{
	using System;
	using System.ComponentModel;
	using System.IO;
	using WF.Player.Core;
	using Xamarin.Forms;
	using Vernacular;

	/// <summary>
	/// Game main cell view model.
	/// </summary>
	public class GameMainCellViewModel : BaseViewModel
	{
		/// <summary>
		/// The name of the name property.
		/// </summary>
		public const string NamePropertyName = "Name";

		/// <summary>
		/// The name of the color property.
		/// </summary>
		public const string ColorPropertyName = "Color";

		/// <summary>
		/// The name of the direction property.
		/// </summary>
		public const string DirectionPropertyName = "Direction";

		/// <summary>
		/// The name of the distance property.
		/// </summary>
		public const string DistancePropertyName = "Distance";

		/// <summary>
		/// The name of the user interface object property.
		/// </summary>
		public const string UIObjectPropertyName = "UIObject";

		/// <summary>
		/// The name of the icon source property.
		/// </summary>
		public const string IconSourcePropertyName = "IconSource";

		/// <summary>
		/// The name of the show icon property.
		/// </summary>
		public const string ShowIconPropertyName = "ShowIcon";

		/// <summary>
		/// The name of the show direction property.
		/// </summary>
		public const string ShowDirectionPropertyName = "ShowDirection";

		/// <summary>
		/// The color.
		/// </summary>
		private Color color;

		/// <summary>
		/// The direction.
		/// </summary>
		private double direction;

		/// <summary>
		/// The distance.
		/// </summary>
		private double distance;

		/// <summary>
		/// The user interface object.
		/// </summary>
		private UIObject uiObject;

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="WF.Player.GameMainCellViewModel"/> class.
		/// </summary>
		/// <param name="name">Name of uiobject.</param>
		/// <param name="color">Color of text.</param>
		/// <param name="uiObject">User interface object.</param>
		public GameMainCellViewModel(string name, Color color, UIObject uiObject)
		{
			this.color = color;
			this.uiObject = uiObject;
			this.uiObject.PropertyChanged += HandlePropertyChanged;
		}

		#endregion

		#region Properties

		#region Name

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name
		{ 
			get
			{
				if (UIObject is Task)
				{
					if (((Task)UIObject).Complete)
					{
						var result = this.uiObject.Name;
						switch (((Task)UIObject).CorrectState)
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
						return this.uiObject.Name;
					}
				}
				else
				{
					return this.uiObject.Name;
				}
			} 
		}

		#endregion

		#region TextColor

		/// <summary>
		/// Gets the color of the text.
		/// </summary>
		/// <value>The color of the text.</value>
		public Color TextColor
		{
			get
			{
				return this.color;
			}

			private set
			{
				SetProperty<Color>(ref this.color, value, ColorPropertyName);
			}
		}

		#endregion

		#region Direction

		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		/// <value>The direction.</value>
		public double Direction
		{ 
			get
			{
				if (uiObject is Zone && ((Zone)uiObject).State == PlayerZoneState.Inside)
				{
					return double.PositiveInfinity;
				}
				else
				{
					return this.direction;
				}
			}

			set
			{
				if (SetProperty<double>(ref this.direction, value, DirectionPropertyName))
				{
					if (ShowDirection == !double.IsNaN(this.direction))
					{
						ShowDirection = !double.IsNaN(this.direction);
						NotifyPropertyChanged(ShowDirectionPropertyName);
					}
				}
			}
		}

		#endregion

		#region Distance

		/// <summary>
		/// Gets or sets the distance.
		/// </summary>
		/// <value>The distance.</value>
		public double Distance
		{ 
			get
			{
				if (uiObject is Zone && ((Zone)uiObject).State == PlayerZoneState.Inside)
				{
					return double.PositiveInfinity;
				}
				else
				{
					return this.distance;
				}
			}

			set
			{
				SetProperty<double>(ref this.distance, value, DistancePropertyName);
			}
		}

		#endregion

		#region VectorToObject

		public LocationVector VectorToObject;

		#endregion

		#region UIObject

		/// <summary>
		/// Gets the user interface object.
		/// </summary>
		/// <value>The user interface object.</value>
		public UIObject UIObject
		{ 
			get
			{
				return this.uiObject;
			}

			private set
			{
				if (SetProperty<UIObject>(ref this.uiObject, value, UIObjectPropertyName))
				{
					NotifyPropertyChanged("Name");
					NotifyPropertyChanged("IconSource");
				}
			}
		}

		#endregion

		#region IconSource

		/// <summary>
		/// Gets the icon source.
		/// </summary>
		/// <value>The icon source.</value>
		public ImageSource IconSource
		{ 
			get
			{
				return App.Game.GetImageSourceForMedia(this.uiObject.Icon);
			}
		}

		#endregion

		#region HasIcon

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="WF.Player.GameMainCellViewModel"/> should show an icon.
		/// </summary>
		/// <value><c>true</c> if show icon; otherwise, <c>false</c>.</value>
		public bool ShowIcon { get; set; }

		#endregion

		#region HasDirection

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="WF.Player.GameMainCellViewModel"/> should show a direction.
		/// </summary>
		/// <value><c>true</c> if show direction; otherwise, <c>false</c>.</value>
		public bool ShowDirection { get; set; }

		#endregion

		#endregion

		#region Private Functions

		/// <summary>
		/// Handles property changes of the UIObject.
		/// </summary>
		/// <param name="sender">Sender of event.</param>
		/// <param name="e">Property changed event arguments.</param>
		private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
			{
				NotifyPropertyChanged("Name");
			}

			if (e.PropertyName == "Icon")
			{
				NotifyPropertyChanged("IconSource");
			}
		}

		#endregion
	}
}