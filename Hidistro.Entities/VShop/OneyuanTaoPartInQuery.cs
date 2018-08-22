using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.VShop
{
	public class OneyuanTaoPartInQuery : Pagination
	{
		public string UserName
		{
			get;
			set;
		}

		public string ActivityId
		{
			get;
			set;
		}

		public int IsPay
		{
			get;
			set;
		}

		public string Pid
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int state
		{
			get;
			set;
		}

		public string Atitle
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public string PayWay
		{
			get;
			set;
		}
	}
}
