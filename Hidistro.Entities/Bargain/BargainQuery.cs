using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Bargain
{
	public class BargainQuery : Pagination
	{
		public string Title
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string Type
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}
	}
}
