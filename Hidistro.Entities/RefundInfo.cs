using System;

namespace Hidistro.Entities
{
	public class RefundInfo
	{
		public enum Handlestatus
		{
			Applied = 1,
			Refunded,
			Refused,
			AuditNotThrough = 7,
			NoneAudit = 4,
			HasTheAudit,
			NoRefund,
			RefuseRefunded = 8
		}

		public int RefundId
		{
			get;
			set;
		}

		public int ReturnsId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public System.DateTime ApplyForTime
		{
			get;
			set;
		}

		public string RefundRemark
		{
			get;
			set;
		}

		public string Comments
		{
			get;
			set;
		}

		public System.DateTime HandleTime
		{
			get;
			set;
		}

		public string AdminRemark
		{
			get;
			set;
		}

		public string Operator
		{
			get;
			set;
		}

		public RefundInfo.Handlestatus HandleStatus
		{
			get;
			set;
		}

		public string Account
		{
			get;
			set;
		}

		public decimal RefundMoney
		{
			get;
			set;
		}

		public string SkuId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int RefundType
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string AuditTime
		{
			get;
			set;
		}

		public string RefundTime
		{
			get;
			set;
		}

		public int OrderItemID
		{
			get;
			set;
		}
	}
}
