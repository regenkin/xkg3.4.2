using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class OrderSplitDao
	{
		private Database database;

		public OrderSplitDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public OrderSplitInfo GetOrderSplitInfo(int id)
		{
			string query = "select * from vshop_OrderSplit where ID=" + id;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			OrderSplitInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<OrderSplitInfo>(dataReader);
			}
			return result;
		}

		public OrderSplitInfo GetOrderSplitInfoByOrderIDAndNum(int orderidnum, string oldorderid)
		{
			string query = "select * from vshop_OrderSplit where OldOrderID=@OldOrderID and OrderIDNum=@OrderIDNum";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OrderIDNum", System.Data.DbType.Int32, orderidnum);
			this.database.AddInParameter(sqlStringCommand, "OldOrderID", System.Data.DbType.String, oldorderid);
			OrderSplitInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<OrderSplitInfo>(dataReader);
			}
			return result;
		}

		public bool DelOrderSplitByOrderID(string oldorderid, System.Data.Common.DbTransaction dbTran = null)
		{
			string query = "delete from vshop_OrderSplit where OldOrderID=@OldOrderID";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OldOrderID", System.Data.DbType.String, oldorderid);
			bool result;
			if (dbTran == null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			return result;
		}

		public bool DelOrderSplitInfo(int id)
		{
			string query = "delete  from vshop_OrderSplit where ID=" + id;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateOrderSplitInfo(OrderSplitInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_OrderSplit SET ").Append("OrderIDNum=@OrderIDNum,").Append("ItemList=@ItemList,").Append("UpdateTime=@UpdateTime").Append(" WHERE ID=@ID");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderIDNum", System.Data.DbType.Int32, info.OrderIDNum);
			this.database.AddInParameter(sqlStringCommand, "ItemList", System.Data.DbType.String, info.ItemList);
			this.database.AddInParameter(sqlStringCommand, "UpdateTime", System.Data.DbType.DateTime, info.UpdateTime);
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, info.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateOrderSplitFright(OrderSplitInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_OrderSplit SET ").Append("AdjustedFreight=@AdjustedFreight ").Append(" WHERE ID=@ID");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Decimal, info.AdjustedFreight);
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, info.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<OrderSplitInfo> GetOrderSplitItems(string orderid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select Id,OrderIDNum,OldOrderID,ItemList,UpdateTime,AdjustedFreight from vshop_OrderSplit ");
			stringBuilder.Append(" where OldOrderID=@OldOrderID order by ID asc ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OldOrderID", System.Data.DbType.String, orderid);
			IList<OrderSplitInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<OrderSplitInfo>(dataReader);
			}
			return result;
		}

		public int GetMaxOrderIDNum(string orderid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select isnull(Max(OrderIDNum),0) from vshop_OrderSplit ");
			stringBuilder.Append(" where OldOrderID=@OldOrderID ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OldOrderID", System.Data.DbType.String, orderid);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public int NewOrderSplit(OrderSplitInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_OrderSplit(OrderIDNum,OldOrderID,ItemList,UpdateTime,AdjustedFreight) ");
			stringBuilder.Append(" values(@OrderIDNum,@OldOrderID,@ItemList,@UpdateTime,@AdjustedFreight);select @@identity ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderIDNum", System.Data.DbType.Int32, info.OrderIDNum);
			this.database.AddInParameter(sqlStringCommand, "OldOrderID", System.Data.DbType.String, info.OldOrderId);
			this.database.AddInParameter(sqlStringCommand, "ItemList", System.Data.DbType.String, info.ItemList);
			this.database.AddInParameter(sqlStringCommand, "UpdateTime", System.Data.DbType.DateTime, info.UpdateTime);
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Decimal, info.AdjustedFreight);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}
	}
}
