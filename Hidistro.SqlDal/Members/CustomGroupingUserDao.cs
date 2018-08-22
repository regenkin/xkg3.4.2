using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class CustomGroupingUserDao
	{
		private Database database;

		public CustomGroupingUserDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<int> GetMemberGroupList(int userId)
		{
			IList<int> list = new List<int>();
			string query = "select GroupId from dbo.Vshop_CustomGroupingUser where UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["GroupId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public int GetGroupIdByUserId(int UserId, int groupId)
		{
			int result = 0;
			string str = string.Empty;
			if (groupId > 0)
			{
				str = " and GroupId=" + groupId;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 GroupId from Vshop_CustomGroupingUser where UserId=@UserId " + str);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			if (dataTable.Rows.Count > 0)
			{
				result = Globals.ToNum(dataTable.Rows[0]["GroupId"]);
			}
			return result;
		}

		public bool AddCustomGroupingUser(int UserId, int groupId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_CustomGroupingUser(GroupId,UserId)VALUES(@GroupId,@UserId)");
			this.database.AddInParameter(sqlStringCommand, "GroupId", System.Data.DbType.Int32, groupId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			bool result = Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0;
			this.UpdateGroupUserCount(groupId);
			return result;
		}

		public bool DelGroupUser(string UserId, int groupId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"Delete Vshop_CustomGroupingUser where GroupId=",
				groupId,
				" and UserId in (",
				UserId,
				")"
			}));
			bool result = Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0;
			this.UpdateGroupUserCount(groupId);
			return result;
		}

		public bool UpdateGroupUserCount(int groupId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_CustomGrouping set UserCount=(select count(0) from Vshop_CustomGroupingUser where GroupId=Vshop_CustomGrouping.id) where Id=" + groupId);
			return Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0;
		}
	}
}
