using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System;
using System.Data.Common;

namespace Hidistro.ControlPanel.Members
{
	public static class IntegralDetailHelp
	{
		public static bool AddIntegralDetail(IntegralDetailInfo point, System.Data.Common.DbTransaction dbTran = null)
		{
			return new IntegralDetailDao().AddIntegralDetail(point, dbTran);
		}

		public static DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
		{
			return new IntegralDetailDao().GetIntegralDetail(query);
		}
	}
}
