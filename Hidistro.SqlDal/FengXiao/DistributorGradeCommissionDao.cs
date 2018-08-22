using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.FenXiao;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Text;

namespace Hidistro.SqlDal.FengXiao
{
	public class DistributorGradeCommissionDao
	{
		private Database database;

		public DistributorGradeCommissionDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult DistributorGradeCommission(DistributorGradeCommissionQuery query)
		{
			string table = "vw_Hishop_DistributorGradeCommission";
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.Title))
			{
				stringBuilder.AppendFormat(" StoreName LIKE '%{0}%' ", DataHelper.CleanSearchString(query.Title));
			}
			if (query.StartTime.HasValue)
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" PubTime>='{0}' ", query.StartTime.Value.ToString("yyyy-MM-dd"));
			}
			if (query.EndTime.HasValue)
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" PubTime<'{0}' ", query.EndTime.Value.AddDays(1.0).ToString("yyyy-MM-dd"));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, table, "ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
	}
}
