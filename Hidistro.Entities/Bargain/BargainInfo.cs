using System;

namespace Hidistro.Entities.Bargain
{
	public class BargainInfo
	{
		public int Id
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string ActivityCover
		{
			get;
			set;
		}

		public System.DateTime BeginDate
		{
			get;
			set;
		}

		public System.DateTime EndDate
		{
			get;
			set;
		}

		public string Remarks
		{
			get;
			set;
		}

		public string Status
		{
			get;
			set;
		}

		public System.DateTime CreateDate
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int ActivityStock
		{
			get;
			set;
		}

		public int PurchaseNumber
		{
			get;
			set;
		}

		public int BargainType
		{
			get;
			set;
		}

		public float BargainTypeMaxVlue
		{
			get;
			set;
		}

		public float BargainTypeMinVlue
		{
			get;
			set;
		}

		public decimal InitialPrice
		{
			get;
			set;
		}

		public bool IsCommission
		{
			get;
			set;
		}

		public decimal FloorPrice
		{
			get;
			set;
		}

		public int TranNumber
		{
			get;
			set;
		}
	}
}
