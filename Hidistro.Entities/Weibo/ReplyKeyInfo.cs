using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Weibo
{
	public class ReplyKeyInfo : Pagination
	{
		public int Id
		{
			get;
			set;
		}

		public int Type
		{
			get;
			set;
		}

		public string Keys
		{
			get;
			set;
		}

		public string Matching
		{
			get;
			set;
		}
	}
}
