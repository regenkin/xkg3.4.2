using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class IntegralDetailDao
	{
		private Database database;

		public IntegralDetailDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddIntegralDetail(IntegralDetailInfo point, System.Data.Common.DbTransaction dbTran = null)
		{
			string query = string.Concat(new object[]
			{
				"INSERT INTO vshop_IntegralDetail  ([IntegralSourceType],[IntegralSource],[IntegralChange],[Remark],[Userid],[GoToUrl],[IntegralStatus]) VALUES(@IntegralSourceType,@IntegralSource,@IntegralChange,@Remark,@Userid,@GoToUrl,@IntegralStatus); UPDATE dbo.aspnet_Members SET Points=Points+ ",
				Convert.ToInt32(point.IntegralChange),
				" WHERE UserId=",
				point.Userid
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "IntegralSourceType", System.Data.DbType.Int32, point.IntegralSourceType);
			this.database.AddInParameter(sqlStringCommand, "IntegralSource", System.Data.DbType.String, point.IntegralSource);
			this.database.AddInParameter(sqlStringCommand, "IntegralChange", System.Data.DbType.Decimal, point.IntegralChange);
			this.database.AddInParameter(sqlStringCommand, "Userid", System.Data.DbType.Int32, point.Userid);
			this.database.AddInParameter(sqlStringCommand, "GoToUrl", System.Data.DbType.String, point.GoToUrl);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, point.Remark);
			this.database.AddInParameter(sqlStringCommand, "IntegralStatus", System.Data.DbType.Int32, point.IntegralStatus);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}

		public DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.IntegralSourceType > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" IntegralSourceType = {0}", query.IntegralSourceType);
			}
			if (query.IntegralStatus > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" IntegralStatus = {0}", query.IntegralStatus);
			}
			if (query.StartTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("  convert(date,TrateTime) >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartTime.Value));
			}
			if (query.EndTime.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("   convert(date,TrateTime)<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndTime.Value));
			}
			if (query.UserId > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserId = {0}", query.UserId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_IntegralDetail", "Id", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
	}
}
