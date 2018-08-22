using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Commodities
{
	public class TagDao
	{
		private Database database;

		public TagDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetTags()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_Tags");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public string GetTagName(int tagId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TagName  FROM  Hishop_Tags WHERE TagID = {0}", tagId));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null)
			{
				result = obj.ToString();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}

		public int AddTags(string tagname)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Tags VALUES(@TagName);SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "TagName", System.Data.DbType.String, Globals.SubStr(tagname, 8, ""));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}

		public bool UpdateTags(int tagId, string tagname)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Tags SET TagName=@TagName WHERE TagID=@TagID");
			this.database.AddInParameter(sqlStringCommand, "TagName", System.Data.DbType.String, Globals.SubStr(tagname, 8, ""));
			this.database.AddInParameter(sqlStringCommand, "TagID", System.Data.DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DeleteTags(int tagId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagID=@TagID;DELETE FROM Hishop_Tags WHERE TagID=@TagID;");
			this.database.AddInParameter(sqlStringCommand, "TagID", System.Data.DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetTags(string tagName)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TagID  FROM  Hishop_Tags WHERE TagName=@TagName");
			this.database.AddInParameter(sqlStringCommand, "TagName", System.Data.DbType.String, tagName);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = Convert.ToInt32(dataReader["TagID"].ToString());
			}
			return result;
		}

		public bool AddProductTags(int productId, IList<int> tagIds, System.Data.Common.DbTransaction tran)
		{
			bool flag = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag VALUES(@TagId,@ProductId)");
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32);
			foreach (int current in tagIds)
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				this.database.SetParameterValue(sqlStringCommand, "TagId", current);
				if (tran != null)
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand, tran) > 0);
				}
				else
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
				}
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}

		public bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			bool result;
			if (tran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
			}
			return result;
		}

		public string GetProductTagName(int productId)
		{
			string result = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select top 1 b.TagName from Hishop_ProductTag a inner join Hishop_Tags b on a.tagid=b.tagid where ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			if (dataTable.Rows.Count > 0)
			{
				result = dataTable.Rows[0]["TagName"].ToString();
			}
			return result;
		}
	}
}
