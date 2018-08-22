using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Weibo;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Weibo
{
	public class ReplyDao
	{
		private Database database;

		public ReplyDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public IList<ReplyInfo> GetReplyInfo(int ReplyKeyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Reply WHERE ReplyKeyId = @ReplyKeyId");
			this.database.AddInParameter(sqlStringCommand, "ReplyKeyId", System.Data.DbType.Int32, ReplyKeyId);
			IList<ReplyInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ReplyInfo>(dataReader);
			}
			return result;
		}

		public IList<ReplyInfo> GetReplyTypeInfo(int Type)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Reply WHERE Type = @Type");
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, Type);
			IList<ReplyInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ReplyInfo>(dataReader);
			}
			return result;
		}

		public ReplyInfo GetReplyInfoMes(int id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_Reply WHERE id = @id");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, id);
			ReplyInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ReplyInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateReplyInfo(ReplyInfo replyInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_Reply SET EditDate=@EditDate,Content=@Content,Type=@Type,ReceiverType=@ReceiverType,Displayname=@Displayname,Summary=@Summary,Image=@Image,Url=@Url,ArticleId=@ArticleId  WHERE id = @id");
			this.database.AddInParameter(sqlStringCommand, "EditDate", System.Data.DbType.DateTime, DateTime.Now.ToString());
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, replyInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, replyInfo.Type);
			this.database.AddInParameter(sqlStringCommand, "ReceiverType", System.Data.DbType.String, replyInfo.ReceiverType);
			this.database.AddInParameter(sqlStringCommand, "Displayname", System.Data.DbType.String, replyInfo.Displayname);
			this.database.AddInParameter(sqlStringCommand, "Summary", System.Data.DbType.String, replyInfo.Summary);
			this.database.AddInParameter(sqlStringCommand, "Image", System.Data.DbType.String, replyInfo.Image);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, replyInfo.Url);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, replyInfo.ArticleId);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, replyInfo.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SaveReplyInfo(ReplyInfo replyInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_Reply (ReplyKeyId,IsDisable,EditDate,Content,Type,ReceiverType,Displayname,Summary,Image,Url,ArticleId) VALUES(@ReplyKeyId,@IsDisable,@EditDate,@Content,@Type,@ReceiverType,@Displayname,@Summary,@Image,@Url,@ArticleId)");
			this.database.AddInParameter(sqlStringCommand, "ReplyKeyId", System.Data.DbType.String, replyInfo.ReplyKeyId);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, replyInfo.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "EditDate", System.Data.DbType.DateTime, DateTime.Now.ToString());
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, replyInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, replyInfo.Type);
			this.database.AddInParameter(sqlStringCommand, "ReceiverType", System.Data.DbType.String, replyInfo.ReceiverType);
			this.database.AddInParameter(sqlStringCommand, "Displayname", System.Data.DbType.String, replyInfo.Displayname);
			this.database.AddInParameter(sqlStringCommand, "Summary", System.Data.DbType.String, replyInfo.Summary);
			this.database.AddInParameter(sqlStringCommand, "Image", System.Data.DbType.String, replyInfo.Image);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, replyInfo.Url);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, replyInfo.ArticleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool DeleteReplyInfo(int ReplyInfoid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Weibo_Reply WHERE id = @id");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, ReplyInfoid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<ReplyKeyInfo> GetTopReplyInfos(int Type)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Weibo_ReplyKeys  where Type=" + Type + "   ORDER BY id ASC");
			IList<ReplyKeyInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ReplyKeyInfo>(dataReader);
			}
			return result;
		}

		public bool UpdateReplyKeyInfo(ReplyKeyInfo replyKeyInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_ReplyKeys SET Keys = @Keys  WHERE id = @id");
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, replyKeyInfo.Keys);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, replyKeyInfo.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool UpdateMatching(ReplyKeyInfo replyKeyInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Weibo_ReplyKeys SET Matching = @Matching  WHERE id = @id");
			this.database.AddInParameter(sqlStringCommand, "Matching", System.Data.DbType.Int32, replyKeyInfo.Matching);
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, replyKeyInfo.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		public bool SaveReplyKeyInfo(ReplyKeyInfo replyKeyInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Weibo_ReplyKeys (Keys,Type,Matching) VALUES(@Keys, @Type,0)");
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, replyKeyInfo.Keys);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, replyKeyInfo.Type);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}

		private int GetAllWeibo_ReplyKeysCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(*) from Weibo_ReplyKeys");
			return 1 + Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool DeleteReplyKeyInfo(int ReplyKeyInfoid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE Weibo_ReplyKeys WHERE id = @id");
			this.database.AddInParameter(sqlStringCommand, "id", System.Data.DbType.Int32, ReplyKeyInfoid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetReplyAll(int type)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  from vw_Hishop_ReplyKeysReply where type=" + type);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetWeibo_Reply(int type)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  from Weibo_Reply where type=" + type);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
	}
}
