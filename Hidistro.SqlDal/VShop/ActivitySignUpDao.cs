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
	public class ActivitySignUpDao
	{
		private Database database;

		public ActivitySignUpDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
		{
			string query = "SELECT * FROM vshop_ActivitySignUp WHERE ActivityId = @ActivityId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			IList<ActivitySignUpInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ActivitySignUpInfo>(dataReader);
			}
			return result;
		}

		public string SaveActivitySignUp1(ActivitySignUpInfo info)
		{
			string text = string.Empty;
			ActivityDao activityDao = new ActivityDao();
			ActivityInfo activity = activityDao.GetActivity(info.ActivityId);
			if (activity == null)
			{
				text = "活动不存在";
			}
			else
			{
				int maxValue = activity.MaxValue;
				if (maxValue > 0)
				{
					string query = "select count(0) from vshop_ActivitySignUp where  ActivityId=@ActivityId";
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
					this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, info.ActivityId);
					int num = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
					if (maxValue <= num)
					{
						text = "已经达到了报名上限";
					}
				}
				if (string.IsNullOrEmpty(text))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("IF NOT EXISTS (select 1 from vshop_ActivitySignUp WHERE ActivityId=@ActivityId and UserId=@UserId) ").Append("INSERT INTO vshop_ActivitySignUp(").Append("ActivityId,UserId,UserName,RealName,SignUpDate").Append(",Item1,Item2,Item3,Item4,Item5)").Append(" VALUES (").Append("@ActivityId,@UserId,@UserName,@RealName,@SignUpDate").Append(",@Item1,@Item2,@Item3,@Item4,@Item5)");
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.AddInParameter(sqlStringCommand2, "ActivityId", System.Data.DbType.Int32, info.ActivityId);
					this.database.AddInParameter(sqlStringCommand2, "UserId", System.Data.DbType.Int32, info.UserId);
					this.database.AddInParameter(sqlStringCommand2, "UserName", System.Data.DbType.String, info.UserName);
					this.database.AddInParameter(sqlStringCommand2, "RealName", System.Data.DbType.String, info.RealName);
					this.database.AddInParameter(sqlStringCommand2, "SignUpDate", System.Data.DbType.DateTime, info.SignUpDate);
					this.database.AddInParameter(sqlStringCommand2, "Item1", System.Data.DbType.String, info.Item1);
					this.database.AddInParameter(sqlStringCommand2, "Item2", System.Data.DbType.String, info.Item2);
					this.database.AddInParameter(sqlStringCommand2, "Item3", System.Data.DbType.String, info.Item3);
					this.database.AddInParameter(sqlStringCommand2, "Item4", System.Data.DbType.String, info.Item4);
					this.database.AddInParameter(sqlStringCommand2, "Item5", System.Data.DbType.String, info.Item5);
					text = ((this.database.ExecuteNonQuery(sqlStringCommand2) > 0) ? "1" : "你已经报过名了,请勿重复报名");
				}
			}
			return text;
		}
	}
}
