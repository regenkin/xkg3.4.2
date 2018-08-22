using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ActivityDao
	{
		private Database database;

		public ActivityDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public ActivityDetailInfo GetActivityDetailInfo(int Id)
		{
			ActivityDetailInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities_Detail WHERE ID = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ActivityDetailInfo>(dataReader);
			}
			return result;
		}

		public int SaveActivity(Hidistro.Entities.VShop.ActivityInfo activity)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO vshop_Activity(").Append("Name,Description,StartDate,EndDate,CloseRemark,Keys").Append(",MaxValue,PicUrl,Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@Name,@Description,@StartDate,@EndDate,@CloseRemark,@Keys").Append(",@MaxValue,@PicUrl,@Item1,@Item2,@Item3,@Item4,@Item5)").Append(";select @@IDENTITY");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, activity.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, activity.Description);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, activity.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, activity.EndDate);
			this.database.AddInParameter(sqlStringCommand, "CloseRemark", System.Data.DbType.String, activity.CloseRemark);
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, activity.Keys);
			this.database.AddInParameter(sqlStringCommand, "MaxValue", System.Data.DbType.Int32, activity.MaxValue);
			this.database.AddInParameter(sqlStringCommand, "PicUrl", System.Data.DbType.String, activity.PicUrl);
			this.database.AddInParameter(sqlStringCommand, "Item1", System.Data.DbType.String, activity.Item1);
			this.database.AddInParameter(sqlStringCommand, "Item2", System.Data.DbType.String, activity.Item2);
			this.database.AddInParameter(sqlStringCommand, "Item3", System.Data.DbType.String, activity.Item3);
			this.database.AddInParameter(sqlStringCommand, "Item4", System.Data.DbType.String, activity.Item4);
			this.database.AddInParameter(sqlStringCommand, "Item5", System.Data.DbType.String, activity.Item5);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			int.TryParse(obj.ToString(), out result);
			return result;
		}

		public Hidistro.Entities.VShop.ActivityInfo GetActivity(int activityId)
		{
			string query = "SELECT * FROM vshop_Activity WHERE ActivityId=@ActivityId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			Hidistro.Entities.VShop.ActivityInfo result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<Hidistro.Entities.VShop.ActivityInfo>(dataReader);
			}
			return result;
		}

		public IList<Hidistro.Entities.VShop.ActivityInfo> GetAllActivity()
		{
			string query = "SELECT *, (SELECT Count(ActivityId) FROM vshop_ActivitySignUp WHERE ActivityId = a.ActivityId) AS CurrentValue FROM vshop_Activity a ORDER BY ActivityId DESC";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<Hidistro.Entities.VShop.ActivityInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<Hidistro.Entities.VShop.ActivityInfo>(dataReader);
			}
			return result;
		}
	}
}
