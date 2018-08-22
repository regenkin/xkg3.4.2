using System;

namespace Hishop.Plugins
{
	public class FinishedEventArgs : EventArgs
	{
		public bool IsMedTrade
		{
			get;
			private set;
		}

		public FinishedEventArgs(bool isMedTrade)
		{
			this.IsMedTrade = isMedTrade;
		}
	}
}
