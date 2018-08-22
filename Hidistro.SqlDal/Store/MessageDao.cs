using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class MessageDao
	{
		private Database database;

		public MessageDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetManagers(ManagerQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			if (query.RoleId != 0)
			{
				stringBuilder.AppendFormat(" AND RoleId = {0}", query.RoleId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Managers", "UserId", stringBuilder.ToString(), "*");
		}

		public ManagerInfo GetManager(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, userId);
			ManagerInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ManagerInfo>(dataReader);
			}
			return result;
		}

		public ManagerInfo GetManager(string userName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Managers WHERE UserName = @UserName");
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, userName);
			ManagerInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ManagerInfo>(dataReader);
			}
			return result;
		}

		public bool Create(ManagerInfo manager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Managers (RoleId, UserName, Password, Email, CreateDate) VALUES (@RoleId, @UserName, @Password, @Email, @CreateDate)");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, manager.RoleId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, manager.UserName);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, manager.Password);
			this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, manager.Email);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, manager.CreateDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteManager(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Managers WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Update(ManagerInfo manager)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Managers SET RoleId = @RoleId, UserName = @UserName, Password = @Password, Email = @Email WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, manager.UserId);
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, manager.RoleId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, manager.UserName);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, manager.Password);
			this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, manager.Email);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
