using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class UserSignDao
	{
		private Database database;

		public UserSignDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int InsertUserSign(UserSign us)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" INSERT INTO [dbo].[Hishop_UserSign] ");
			stringBuilder.Append(" ([UserID],[SignDay],[Continued],[Stage]) ");
			stringBuilder.Append(" VALUES ");
			stringBuilder.Append(" (@UserID,@SignDay,@Continued,@Stage) ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, us.UserID);
			this.database.AddInParameter(sqlStringCommand, "SignDay", System.Data.DbType.Date, us.SignDay);
			this.database.AddInParameter(sqlStringCommand, "Continued", System.Data.DbType.Int32, 1);
			this.database.AddInParameter(sqlStringCommand, "Stage", System.Data.DbType.Int32, 0);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int UpdateUserSign(UserSign us)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" UPDATE Hishop_UserSign ");
			stringBuilder.Append(" SET  [SignDay] = @SignDay");
			stringBuilder.Append(" ,[Continued] = @Continued ");
			stringBuilder.Append(" ,[Stage] = @Stage ");
			stringBuilder.Append("  WHERE  ");
			stringBuilder.Append(" [UserID] = @UserID ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, us.UserID);
			this.database.AddInParameter(sqlStringCommand, "SignDay", System.Data.DbType.Date, us.SignDay);
			this.database.AddInParameter(sqlStringCommand, "Continued", System.Data.DbType.Int32, us.Continued);
			this.database.AddInParameter(sqlStringCommand, "Stage", System.Data.DbType.Int32, us.Stage);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public System.Data.DataTable SignInfoByUser(int userID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT *");
			stringBuilder.Append(" FROM [dbo].[Hishop_UserSign] ");
			stringBuilder.Append(" WHERE ");
			stringBuilder.Append(" [UserID]=").Append(userID);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
	}
}
