using Hidistro.Entities;
using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class RoleDao
	{
		private Database database;

		public RoleDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddRole(RoleInfo role)
		{
			string query = "INSERT INTO aspnet_Roles (RoleName, Description,IsDefault) VALUES (@RoleName, @Description,@IsDefault);";
			if (role.IsDefault)
			{
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "RoleName", System.Data.DbType.String, role.RoleName);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, role.Description);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, role.IsDefault);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateRole(RoleInfo role)
		{
			string query = "UPDATE aspnet_Roles SET RoleName = @RoleName, Description = @Description,IsDefault=@IsDefault WHERE RoleId = @RoleId ;";
			if (role.IsDefault)
			{
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, role.RoleId);
			this.database.AddInParameter(sqlStringCommand, "RoleName", System.Data.DbType.String, role.RoleName);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, role.Description);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, role.IsDefault);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteRole(int roleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("if( select count(*) from aspnet_Managers where RoleId = @RoleId ) = 0 DELETE FROM aspnet_Roles WHERE RoleId = @RoleId");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, roleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool RoleExists(string roleName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Roles WHERE RoleName = @RoleName");
			this.database.AddInParameter(sqlStringCommand, "RoleName", System.Data.DbType.String, roleName);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public RoleInfo GetRole(int roleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Roles WHERE RoleId = @RoleId");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, roleId);
			RoleInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<RoleInfo>(dataReader);
			}
			return result;
		}

		public IList<RoleInfo> GetRoles()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Roles");
			IList<RoleInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<RoleInfo>(dataReader);
			}
			return result;
		}

		public IList<int> GetPrivilegeByRoles(int roleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PrivilegeInRoles  WHERE RoleId = @RoleId");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, roleId);
			IList<int> list = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["Privilege"]);
				}
			}
			return list;
		}

		public void AddPrivilegeInRoles(int roleId, string strPermissions)
		{
			string[] array = strPermissions.Split(new char[]
			{
				','
			});
			StringBuilder stringBuilder = new StringBuilder(" ");
			if (array != null && array.Length > 0)
			{
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string arg = array2[i];
					stringBuilder.AppendFormat("INSERT INTO Hishop_PrivilegeInRoles (RoleId, Privilege) VALUES (@RoleId, {0}); ", arg);
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.String, roleId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void ClearRolePrivilege(int roleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PrivilegeInRoles WHERE RoleId = @RoleId");
			this.database.AddInParameter(sqlStringCommand, "RoleId", System.Data.DbType.Int32, roleId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
