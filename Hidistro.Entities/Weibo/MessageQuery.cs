using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Weibo
{
	public class MessageQuery : Pagination
	{
		public int Status
		{
			get;
			set;
		}

		public string Access_Token
		{
			get;
			set;
		}
	}
}
