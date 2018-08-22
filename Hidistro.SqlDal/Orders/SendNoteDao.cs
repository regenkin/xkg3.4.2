using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Orders;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Orders
{
	public class SendNoteDao
	{
		private Database database;

		public SendNoteDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool SaveSendNote(SendNoteInfo note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" insert into Hishop_OrderSendNote(NoteId,OrderId,Operator,Remark) values(@NoteId,@OrderId,@Operator,@Remark)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult GetAllSendNote(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_OrderSendNote", "OrderId", stringBuilder.ToString(), "*");
		}

		public bool DelSendNote(string noteId)
		{
			string query = string.Format("DELETE FROM Hishop_OrderSendNote WHERE NoteId='{0}'", noteId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
