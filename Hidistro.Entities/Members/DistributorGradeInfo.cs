using System;

namespace Hidistro.Entities.Members
{
	public class DistributorGradeInfo
	{
		public string Name
		{
			get;
			set;
		}

		public int GradeId
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public decimal CommissionsLimit
		{
			get;
			set;
		}

		public decimal FirstCommissionRise
		{
			get;
			set;
		}

		public decimal SecondCommissionRise
		{
			get;
			set;
		}

		public decimal ThirdCommissionRise
		{
			get;
			set;
		}

		public bool IsDefault
		{
			get;
			set;
		}

		public string Ico
		{
			get;
			set;
		}

		public decimal AddCommission
		{
			get;
			set;
		}
	}
}
