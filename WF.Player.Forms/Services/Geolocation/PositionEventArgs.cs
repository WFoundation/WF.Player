using System;

namespace WF.Player.Services.Geolocation
{
	public class PositionEventArgs : EventArgs
	{
		public PositionEventArgs (Position position)
		{
			if (position == null)
				throw new ArgumentNullException ("position");

			Position = position;
		}

		public Position Position
		{
			get;
			private set;
		}
	}
}

