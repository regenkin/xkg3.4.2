using System;

namespace Hidistro.Entities.VShop
{
	public class OneyuanTaoWinRecordInfo
	{
		public int id
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		public string PrizeNum
		{
			get;
			set;
		}

		public bool IsDeliver
		{
			get;
			set;
		}

		public string ShippingId
		{
			get;
			set;
		}

		public string ShippingNum
		{
			get;
			set;
		}

		public string ShipingCompany
		{
			get;
			set;
		}

		public System.DateTime? ShippingTime
		{
			get;
			set;
		}

		public System.DateTime? ReceivedTime
		{
			get;
			set;
		}

		public bool IsReceived
		{
			get;
			set;
		}
	}
}
