using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.SaleSystem.Vshop
{
	public class DistributorGradeBrower
	{
		public static bool SetAddCommission(int gradeid, decimal addcommission)
		{
			return new DistributorGradeDao().SetAddCommission(gradeid, addcommission);
		}

		public static bool ClearAddCommission()
		{
			return new DistributorGradeDao().ClearAddCommission();
		}

		public static DbQueryResult GetDistributorGradeRequest(DistributorGradeQuery query)
		{
			return new DistributorGradeDao().GetDistributorGradeRequest(query);
		}

		public static DistributorGradeInfo GetDistributorGradeInfo(int gradeid)
		{
			return new DistributorGradeDao().GetDistributorGradeInfo(gradeid);
		}

		public static System.Data.DataTable GetAllDistributorGrade()
		{
			return new DistributorGradeDao().GetAllDistributorGrade();
		}

		public static DistributorGradeInfo GetIsDefaultDistributorGradeInfo()
		{
			return new DistributorGradeDao().GetIsDefaultDistributorGradeInfo();
		}

		public static Dictionary<int, int> GetGradeCount(string ReferralStatus)
		{
			return new DistributorGradeDao().GetGradeCount(ReferralStatus);
		}

		public static bool IsExistsMinAmount(int gradeid, decimal minorderamount)
		{
			return new DistributorGradeDao().IsExistsMinAmount(gradeid, minorderamount);
		}

		public static bool UpdateDistributor(DistributorGradeInfo distributorgrade)
		{
			return new DistributorGradeDao().UpdateDistributor(distributorgrade);
		}

		public static bool CreateDistributorGrade(DistributorGradeInfo distributorgrade)
		{
			return new DistributorGradeDao().CreateDistributorGrade(distributorgrade);
		}

		public static bool SetGradeDefault(int gradeid)
		{
			return new DistributorGradeDao().SetGradeDefault(gradeid);
		}

		public static string DelOneGrade(int gradeid)
		{
			return new DistributorGradeDao().DelOneGrade(gradeid);
		}
	}
}
