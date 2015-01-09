// <copyright file="Direction.cs" company="Wherigo Foundation">
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

using System;
using Vernacular;

namespace WF.Player.Data
{
	public class DirectionInfo : ObservableObject
	{
		private double distanceValue;
		private double directionValue;
		private DirectionState state;

		#region Constructor

		public DirectionInfo() : this(0, 0, DirectionState.Unknown)
		{
		}

		public DirectionInfo(double distance, double direction, DirectionState state)
		{
			this.distanceValue = distance;
			this.directionValue = direction;
			this.state = state;
		}

		#endregion

		#region Properties

		#region DistanceValue

		public double DistanceValue
		{
			get
			{
				return distanceValue;
			}

			set
			{
				SetProperty<double>(ref distanceValue, value);
			}
		}

		#endregion

		#region DirectionValue

		public double DirectionValue
		{
			get
			{
				return directionValue;
			}

			set
			{
				SetProperty<double>(ref directionValue, value);
			}
		}

		#endregion

		#region DirectionState

		public DirectionState State
		{
			get
			{
				return state;
			}

			set
			{
				SetProperty<DirectionState>(ref state, value);
			}
		}

		#endregion

		#region DistanceText

		public string DistanceText
		{
			get
			{
				string result;

				switch(state)
				{
					case DirectionState.Inside:
						result = Catalog.GetString("Inside");
						break;
					case DirectionState.Unknown:
						result = " ";
						break;
					default:
						result = Converter.NumberToBestLength(DistanceValue);
						break;
				}

				return result;
			}
		}

		#endregion

		#endregion
	}
}