using Hidistro.Entities.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Store
{
	public class RolePermissionDao
	{
		private Database db;

		public RolePermissionDao()
		{
			this.db = DatabaseFactory.CreateDatabase();
		}

		public bool AddRolePermission(IList<RolePermissionInfo> models, int roleId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Delete From aspnet_RolePermissions where RoleId={0};", roleId);
			if (models.Count > 0)
			{
				int num = models.Count<RolePermissionInfo>();
				for (int i = 0; i < num; i++)
				{
					stringBuilder.Append("Insert into aspnet_RolePermissions([PermissionId],[RoleId]) values ");
					stringBuilder.AppendFormat("('{0}',{1})", models[i].PermissionId, models[i].RoleId);
					stringBuilder.Append(";");
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.db.GetSqlStringCommand(stringBuilder.ToString());
			return this.db.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public List<RolePermissionInfo> GetPermissionsByRoleId(int roleId)
		{
			List<RolePermissionInfo> list = new List<RolePermissionInfo>();
			string query = "SELECT [PermissionId],[RoleId] FROM [aspnet_RolePermissions] where RoleId=@RoleId";
			System.Data.Common.DbCommand sqlStringCommand = this.db.GetSqlStringCommand(query);
			this.db.AddInParameter(sqlStringCommand, "@RoleId", System.Data.DbType.Int32, roleId);
			using (System.Data.IDataReader dataReader = this.db.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new RolePermissionInfo
					{
						PermissionId = dataReader["PermissionId"].ToString(),
						RoleId = roleId
					});
				}
			}
			return list;
		}
	}
}
