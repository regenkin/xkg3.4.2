using System;

namespace Hishop.Plugins
{
	public class FailedEventArgs : EventArgs
	{
		public string Message
		{
			get;
			private set;
		}

		public FailedEventArgs(string message)
		{
			this.Message = message;
		}
	}
}
