using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ActivitiesDao
	{
		private Database database;

		public ActivitiesDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetActivitiesList(ActivitiesQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.ActivitiesName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" ActivitiesName LIKE '%{0}%'", DataHelper.CleanSearchString(query.ActivitiesName));
			}
			if (!string.IsNullOrEmpty(query.State.ToString()))
			{
				if (query.State == "1")
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat(" datediff(dd,'{0}',StartTime)<=0 and datediff(dd,'{0}',EndTIme)>=0", DateTime.Now.ToShortDateString());
				}
				else if (query.State == "2")
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat(" datediff(dd,'{0}',StartTime)>0 ", DateTime.Now.ToShortDateString());
				}
				else
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat(" datediff(dd,'{0}',EndTIme)<0 ", DateTime.Now.ToShortDateString());
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Activities ", "ActivitiesId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM Hishop_Categories WHERE CategoryId = Hishop_Activities.ActivitiesType) AS CategoriesName");
		}

		public int AddActivities(ActivitiesInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO Hishop_Activities(").Append("ActivitiesName,ActivitiesType,MeetMoney,ReductionMoney,StartTime,EndTIme,ActivitiesDescription,Type)").Append(" VALUES (").Append("@ActivitiesName,@ActivitiesType,@MeetMoney,@ReductionMoney,@StartTime,@EndTime,@ActivitiesDescription,@Type)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivitiesName", System.Data.DbType.String, activity.ActivitiesName);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesType", System.Data.DbType.Int32, activity.ActivitiesType);
			this.database.AddInParameter(sqlStringCommand, "MeetMoney", System.Data.DbType.Decimal, activity.MeetMoney);
			this.database.AddInParameter(sqlStringCommand, "ReductionMoney", System.Data.DbType.Decimal, activity.ReductionMoney);
			this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, activity.StartTime);
			this.database.AddInParameter(sqlStringCommand, "EndTIme", System.Data.DbType.DateTime, activity.EndTIme);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", System.Data.DbType.String, activity.ActivitiesDescription);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, activity.Type);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdateActivities(ActivitiesInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Activities SET ").Append("ActivitiesName=@ActivitiesName,").Append("ActivitiesType=@ActivitiesType,").Append("MeetMoney=@MeetMoney,").Append("ReductionMoney=@ReductionMoney,").Append("StartTime=@StartTime,").Append("EndTIme=@EndTIme,").Append("ActivitiesDescription=@ActivitiesDescription").Append(" WHERE ActivitiesId=@ActivitiesId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivitiesName", System.Data.DbType.String, activity.ActivitiesName);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesType", System.Data.DbType.Int32, activity.ActivitiesType);
			this.database.AddInParameter(sqlStringCommand, "MeetMoney", System.Data.DbType.Decimal, activity.MeetMoney);
			this.database.AddInParameter(sqlStringCommand, "ReductionMoney", System.Data.DbType.Decimal, activity.ReductionMoney);
			this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, activity.StartTime);
			this.database.AddInParameter(sqlStringCommand, "EndTIme", System.Data.DbType.DateTime, activity.EndTIme);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", System.Data.DbType.String, activity.ActivitiesDescription);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, activity.ActivitiesId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteActivities(int ActivitiesId)
		{
			string query = "DELETE FROM Hishop_Activities WHERE ActivitiesId=@ActivitiesId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, ActivitiesId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<ActivitiesInfo> GetActivitiesInfo(string ActivitiesId)
		{
			IList<ActivitiesInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_Activities WHERE ActivitiesId={0}", ActivitiesId));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ActivitiesInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetType(int Types)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and Type=" + Types + " order by MeetMoney asc");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool UpdateActivitiesTakeEffect(string activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Activities SET ").Append("TakeEffect=TakeEffect+1").Append(" WHERE ActivitiesId IN (" + activity + ")");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
