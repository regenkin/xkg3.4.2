using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class SendRedpackRecordDao
	{
		private Database database;

		public SendRedpackRecordDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int GetRedPackTotalAmount(int balancedrawrequestid, int userid)
		{
			string text = "select isnull(sum(Amount),0) from vshop_SendRedpackRecord where IsSend=1";
			if (balancedrawrequestid > 0)
			{
				text = text + " and BalanceDrawRequestID=" + balancedrawrequestid;
			}
			else if (userid > 0)
			{
				text = text + " and UserID=" + userid;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool SetRedpackRecordIsUsed(int id, bool issend)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE vshop_SendRedpackRecord set IsSend=@IsSend,SendTime=getdate() where ID=@ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
			this.database.AddInParameter(sqlStringCommand, "IsSend", System.Data.DbType.Boolean, issend);
			bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
			if (flag)
			{
				SendRedpackRecordInfo sendRedpackRecordByID = this.GetSendRedpackRecordByID(id.ToString(), null);
				if (sendRedpackRecordByID != null)
				{
					string query = string.Format("update Hishop_BalanceDrawRequest set IsCheck=2,CheckTime=getdate(), Remark='红包提现记录' where SerialID={0} and IsCheck=1 and not exists(select id from vshop_SendRedpackRecord a where a.IsSend=0 and a.BalanceDrawRequestID={0}) and exists(select id from vshop_SendRedpackRecord a where a.IsSend=1 and a.BalanceDrawRequestID={0})", sendRedpackRecordByID.BalanceDrawRequestID);
					sqlStringCommand = this.database.GetSqlStringCommand(query);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
			return flag;
		}

		public System.Data.DataTable GetNotSendRedpackRecord(int balancedrawrequestid)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from vshop_SendRedpackRecord where BalanceDrawRequestID=@BalanceDrawRequestID and IsSend=0");
			this.database.AddInParameter(sqlStringCommand, "BalanceDrawRequestID", System.Data.DbType.Int32, balancedrawrequestid);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public DbQueryResult GetSendRedpackRecordList(SendRedpackRecordQuery sendredpackrecordquery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" UserId={0} and BalanceDrawRequestID={1} ", sendredpackrecordquery.UserID);
			return DataHelper.PagingByRownumber(sendredpackrecordquery.PageIndex, sendredpackrecordquery.PageSize, sendredpackrecordquery.SortBy, sendredpackrecordquery.SortOrder, sendredpackrecordquery.IsCount, "vshop_SendRedpackRecord", "ID", stringBuilder.ToString(), "*");
		}

		public DbQueryResult GetSendRedpackRecordRequest(SendRedpackRecordQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.BalanceDrawRequestID > 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" BalanceDrawRequestID={0} ", query.BalanceDrawRequestID);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_SendRedpackRecord ", "ID", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public bool HasDrawRequest(int serialid)
		{
			bool result = false;
			string query = "select top 1 ID from vshop_SendRedpackRecord where BalanceDrawRequestID=" + serialid;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = true;
			}
			dataReader.Close();
			return result;
		}

		public SendRedpackRecordInfo GetSendRedpackRecordByID(string id = null, string sid = null)
		{
			SendRedpackRecordInfo result;
			if (id == null && sid == null)
			{
				result = null;
			}
			else
			{
				SendRedpackRecordInfo sendRedpackRecordInfo = null;
				string empty = string.Empty;
				int num = 0;
				string query;
				if (id != null)
				{
					if (!int.TryParse(id, out num))
					{
						result = null;
						return result;
					}
					query = string.Format("select * FROM vshop_SendRedpackRecord WHERE ID={0}", id);
				}
				else
				{
					if (!int.TryParse(sid, out num))
					{
						result = null;
						return result;
					}
					query = string.Format("select * FROM vshop_SendRedpackRecord WHERE BalanceDrawRequestID={0}", sid);
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						sendRedpackRecordInfo = DataMapper.PopulateSendRedpackRecordInfo(dataReader);
					}
				}
				result = sendRedpackRecordInfo;
			}
			return result;
		}

		public bool AddSendRedpackRecord(SendRedpackRecordInfo sendredpackinfo, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into vshop_SendRedpackRecord(BalanceDrawRequestID,UserID,OpenID,Amount,ActName,Wishing,ClientIP,IsSend,SendTime)values(@BalanceDrawRequestID,@UserID,@OpenID,@Amount,@ActName,@Wishing,@ClientIP,@IsSend,@SendTime)");
			this.database.AddInParameter(sqlStringCommand, "BalanceDrawRequestID", System.Data.DbType.Int32, sendredpackinfo.BalanceDrawRequestID);
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, sendredpackinfo.UserID);
			this.database.AddInParameter(sqlStringCommand, "OpenID", System.Data.DbType.String, sendredpackinfo.OpenID);
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Int32, sendredpackinfo.Amount);
			this.database.AddInParameter(sqlStringCommand, "ActName", System.Data.DbType.String, sendredpackinfo.ActName);
			this.database.AddInParameter(sqlStringCommand, "Wishing", System.Data.DbType.String, sendredpackinfo.Wishing);
			this.database.AddInParameter(sqlStringCommand, "ClientIP", System.Data.DbType.String, sendredpackinfo.ClientIP);
			this.database.AddInParameter(sqlStringCommand, "IsSend", System.Data.DbType.Int32, 0);
			this.database.AddInParameter(sqlStringCommand, "SendTime", System.Data.DbType.DateTime, DBNull.Value);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
	}
}
