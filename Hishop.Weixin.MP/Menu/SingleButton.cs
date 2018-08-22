using System;

namespace Hishop.Weixin.MP.Domain.Menu
{
	public abstract class SingleButton : BaseButton
	{
		public string type
		{
			get;
			set;
		}

		public SingleButton(string theType)
		{
			this.type = theType;
		}
	}
}
