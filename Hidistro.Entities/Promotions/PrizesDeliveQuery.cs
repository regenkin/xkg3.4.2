using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Promotions
{
	public class PrizesDeliveQuery : Pagination
	{
		public int RecordType
		{
			get;
			set;
		}

		public int Status
		{
			get;
			set;
		}

		public string Pid
		{
			get;
			set;
		}

		public int? GameType
		{
			get;
			set;
		}

		public string StartDate
		{
			get;
			set;
		}

		public string EndDate
		{
			get;
			set;
		}

		public string ReggionId
		{
			get;
			set;
		}

		public string Receiver
		{
			get;
			set;
		}

		public string ProductName
		{
			get;
			set;
		}

		public string LogId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string ActivityTitle
		{
			get;
			set;
		}

		public string Tel
		{
			get;
			set;
		}

		public int PrizeType
		{
			get;
			set;
		}

		public string DeliveryTime
		{
			get;
			set;
		}

		public string ReceiveTime
		{
			get;
			set;
		}

		public string ExpressName
		{
			get;
			set;
		}

		public string CourierNumber
		{
			get;
			set;
		}

		public string ReggionPath
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public int Id
		{
			get;
			set;
		}

		public int? GameId
		{
			get;
			set;
		}

		public int? IsUsed
		{
			get;
			set;
		}
	}
}
