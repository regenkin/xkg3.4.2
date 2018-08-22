using Hidistro.Core.Entities;
using Hidistro.Entities.FenXiao;
using Hidistro.SqlDal.FengXiao;
using System;

namespace Hidistro.ControlPanel.FengXiao
{
	public class DistributorGradeCommissionHelper
	{
		public static DbQueryResult DistributorGradeCommission(DistributorGradeCommissionQuery query)
		{
			return new DistributorGradeCommissionDao().DistributorGradeCommission(query);
		}
	}
}
