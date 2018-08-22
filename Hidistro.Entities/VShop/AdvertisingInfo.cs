using System;

namespace Hidistro.Entities.VShop
{
	public class AdvertisingInfo
	{
		public int AdvertisingId
		{
			get;
			set;
		}

		public string title
		{
			get;
			set;
		}

		public string Content
		{
			get;
			set;
		}

		public System.DateTime AddedDate
		{
			get;
			set;
		}

		public bool IsShowFooter
		{
			get;
			set;
		}
	}
}
