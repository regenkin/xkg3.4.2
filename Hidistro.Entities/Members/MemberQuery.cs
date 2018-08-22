using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Members
{
	public class MemberQuery : Pagination
	{
		public string Username
		{
			get;
			set;
		}

		public string Realname
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public int? GradeId
		{
			get;
			set;
		}

		public bool? IsApproved
		{
			get;
			set;
		}

		public System.DateTime? StartTime
		{
			get;
			set;
		}

		public System.DateTime? EndTime
		{
			get;
			set;
		}

		public System.DateTime? RegisterStartTime
		{
			get;
			set;
		}

		public System.DateTime? RegisterEndTime
		{
			get;
			set;
		}

		public int? OrderNumber
		{
			get;
			set;
		}

		public decimal? OrderMoney
		{
			get;
			set;
		}

		public string CharSymbol
		{
			get;
			set;
		}

		public string ClientType
		{
			get;
			set;
		}

		public bool? HasVipCard
		{
			get;
			set;
		}

		public UserStatus? Stutas
		{
			get;
			set;
		}

		public string UserBindName
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public int? GroupId
		{
			get;
			set;
		}

		public decimal? TradeMoneyStart
		{
			get;
			set;
		}

		public decimal? TradeMoneyEnd
		{
			get;
			set;
		}

		public int? TradeNumStart
		{
			get;
			set;
		}

		public int? TradeNumEnd
		{
			get;
			set;
		}

		public string GradeIds
		{
			get;
			set;
		}

		public string GroupIds
		{
			get;
			set;
		}
	}
}
