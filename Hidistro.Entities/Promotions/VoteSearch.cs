using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class VoteSearch : Pagination
	{
		public string Name
		{
			get;
			set;
		}

		public VoteStatus status
		{
			get;
			set;
		}
	}
}
