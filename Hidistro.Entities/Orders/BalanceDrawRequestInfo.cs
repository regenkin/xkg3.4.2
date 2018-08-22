using System;

namespace Hidistro.Entities.Orders
{
	public class BalanceDrawRequestInfo
	{
		public int SerialId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int RequesType
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public System.DateTime RequestTime
		{
			get;
			set;
		}

		public decimal Amount
		{
			get;
			set;
		}

		public string AccountName
		{
			get;
			set;
		}

		public string BankName
		{
			get;
			set;
		}

		public string MerchantCode
		{
			get;
			set;
		}

		public string Remark
		{
			get;
			set;
		}

		public string IsCheck
		{
			get;
			set;
		}

		public System.DateTime CheckTime
		{
			get;
			set;
		}

		public string CellPhone
		{
			get;
			set;
		}

		public string StoreName
		{
			get;
			set;
		}

		public string UserOpenId
		{
			get;
			set;
		}
	}
}
