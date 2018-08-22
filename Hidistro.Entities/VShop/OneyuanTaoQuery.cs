using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class OneyuanTaoQuery : Pagination
	{
		public string title
		{
			get;
			set;
		}

		public int ReachType
		{
			get;
			set;
		}

		public int state
		{
			get;
			set;
		}
	}
}
