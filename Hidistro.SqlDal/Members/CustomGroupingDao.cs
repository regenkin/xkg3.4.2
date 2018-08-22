using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class CustomGroupingDao
	{
		private Database database;

		public CustomGroupingDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetCustomGroupingUser(int groupId)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.UserId,b.LastOrderDate,b.PayOrderDate,b.FinishOrderDate FROM Vshop_CustomGroupingUser a inner join aspnet_Members b on a.userid=b.userid where a.GroupId=" + groupId + " and b.Status=1 Order By UpdateTime desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetCustomGroupingTable()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Vshop_CustomGrouping Order By UpdateTime desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public IList<CustomGroupingInfo> GetCustomGroupingList()
		{
			string query = "SELECT * FROM Vshop_CustomGrouping Order By UpdateTime desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<CustomGroupingInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<CustomGroupingInfo>(dataReader);
			}
			return result;
		}

		public IList<CustomGroupingInfo> GetCustomGroupingList(string customGroupIds = "")
		{
			string text = "SELECT * FROM Vshop_CustomGrouping ";
			if (!string.IsNullOrEmpty(customGroupIds))
			{
				text = text + " where id in(" + customGroupIds + ");";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			IList<CustomGroupingInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<CustomGroupingInfo>(dataReader);
			}
			return result;
		}

		public int GetGroupIdByGroupName(string groupName, int groupId)
		{
			int result = 0;
			string str = string.Empty;
			if (groupId > 0)
			{
				str = " and ID<>" + groupId;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 Id from Vshop_CustomGrouping where GroupName=@GroupName " + str);
			this.database.AddInParameter(sqlStringCommand, "GroupName", System.Data.DbType.String, groupName);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			if (dataTable.Rows.Count > 0)
			{
				result = Globals.ToNum(dataTable.Rows[0]["ID"]);
			}
			return result;
		}

		public string AddCustomGrouping(CustomGroupingInfo customGroupingInfo)
		{
			string result = string.Empty;
			if (this.GetGroupIdByGroupName(customGroupingInfo.GroupName, 0) > 0)
			{
				result = "分组名称已存在";
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Vshop_CustomGrouping(GroupName,Memo)VALUES(@GroupName,@Memo);select @@identity;");
				this.database.AddInParameter(sqlStringCommand, "GroupName", System.Data.DbType.String, customGroupingInfo.GroupName);
				this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, customGroupingInfo.Memo);
				result = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand)).ToString();
			}
			return result;
		}

		public bool DelGroup(int groupid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"Delete Vshop_CustomGroupingUser where GroupId=",
				groupid,
				";Delete Vshop_CustomGrouping where Id=",
				groupid
			}));
			return Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)) > 0;
		}

		public string UpdateCustomGrouping(CustomGroupingInfo customGroupingInfo)
		{
			string result = string.Empty;
			if (this.GetGroupIdByGroupName(customGroupingInfo.GroupName, customGroupingInfo.Id) > 0)
			{
				result = "分组名称已存在";
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Vshop_CustomGrouping set GroupName=@GroupName where Id=" + customGroupingInfo.Id);
				this.database.AddInParameter(sqlStringCommand, "GroupName", System.Data.DbType.String, customGroupingInfo.GroupName);
				this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, customGroupingInfo.Memo);
				result = Globals.ToNum(this.database.ExecuteNonQuery(sqlStringCommand)).ToString();
			}
			return result;
		}

		public CustomGroupingInfo GetGroupInfoById(int groupId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Vshop_CustomGrouping where id=@Id");
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.String, groupId);
			CustomGroupingInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<CustomGroupingInfo>(dataReader);
			}
			return result;
		}
	}
}
