using System;

namespace Hidistro.Entities.VShop
{
	[System.Serializable]
	public abstract class AbstractResponseMessage
	{
		public int MaterialId
		{
			get;
			set;
		}

		public string MsgType
		{
			get;
			set;
		}
	}
}
