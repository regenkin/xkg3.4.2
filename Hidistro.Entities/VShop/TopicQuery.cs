using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class TopicQuery : Pagination
	{
		public bool? IsRelease
		{
			get;
			set;
		}

		public bool? IsincludeHomeProduct
		{
			get;
			set;
		}
	}
}
