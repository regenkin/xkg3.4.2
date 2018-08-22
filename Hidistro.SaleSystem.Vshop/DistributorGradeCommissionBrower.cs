using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public class DistributorGradeCommissionBrower
	{
		public static bool AddCommission(DistributorGradeCommissionInfo info)
		{
			return new DistributorGradeCommissionDao().AddCommission(info);
		}
	}
}
