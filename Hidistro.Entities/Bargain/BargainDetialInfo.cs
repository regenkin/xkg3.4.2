using System;

namespace Hidistro.Entities.Bargain
{
	public class BargainDetialInfo
	{
		public int Id
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public int BargainId
		{
			get;
			set;
		}

		public int Number
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public int NumberOfParticipants
		{
			get;
			set;
		}

		public System.DateTime CreateDate
		{
			get;
			set;
		}

		public string Sku
		{
			get;
			set;
		}

		public int IsDelete
		{
			get;
			set;
		}
	}
}
