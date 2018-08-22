using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.SqlDal.Weibo;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class ReplyDao
	{
		private Database database;

		public ReplyDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public Hidistro.Entities.VShop.MessageInfo GetMessage(int messageId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Vshop_Message WHERE MsgID =@MsgID");
			this.database.AddInParameter(sqlStringCommand, "MsgID", System.Data.DbType.Int32, messageId);
			Hidistro.Entities.VShop.MessageInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<Hidistro.Entities.VShop.MessageInfo>(dataReader);
			}
			return result;
		}

		public void DeleteNewsMsg(int id)
		{
			StringBuilder stringBuilder = new StringBuilder(" delete from vshop_Message where MsgID=@MsgID");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "MsgID", System.Data.DbType.Int32, id);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public Hidistro.Entities.VShop.ReplyInfo GetReply(int id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vshop_Reply WHERE ReplyId = @ReplyId");
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, id);
			Hidistro.Entities.VShop.ReplyInfo replyInfo = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					replyInfo = this.ReaderBind(dataReader);
					switch (replyInfo.MessageType)
					{
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						object obj = dataReader["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						replyInfo = textReplyInfo;
						break;
					}
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.Id);
						replyInfo = newsReplyInfo;
						break;
					}
					}
				}
			}
			return replyInfo;
		}

		public bool DeleteReply(int id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE vshop_Reply WHERE ReplyId = @ReplyId;DELETE vshop_Message WHERE ReplyId = @ReplyId");
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SaveReply(Hidistro.Entities.VShop.ReplyInfo reply)
		{
			bool result = false;
			switch (reply.MessageType)
			{
			case MessageType.Text:
				result = this.SaveTextReply(reply as TextReplyInfo);
				break;
			case MessageType.News:
			case MessageType.List:
				result = this.SaveNewsReply(reply as NewsReplyInfo);
				break;
			}
			return result;
		}

		private bool SaveNewsReply(NewsReplyInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_Reply(");
			stringBuilder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ArticleID)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type,@ArticleID)");
			stringBuilder.Append(";select @@IDENTITY");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, model.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", System.Data.DbType.Int32, (int)model.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, (int)model.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.Int32, (int)model.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, model.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", System.Data.DbType.DateTime, model.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", System.Data.DbType.String, model.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, "");
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "ArticleID", System.Data.DbType.Int32, model.ArticleID);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return true;
		}

		private bool SaveTextReply(TextReplyInfo model)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into vshop_Reply(");
			stringBuilder.Append("Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId,ArticleID)");
			stringBuilder.Append(" values (");
			stringBuilder.Append("@Keys,@MatchType,@ReplyType,@MessageType,@IsDisable,@LastEditDate,@LastEditor,@Content,@Type,@ActivityId,@ArticleID)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, model.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", System.Data.DbType.Int32, (int)model.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, (int)model.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.Int32, (int)model.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, model.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", System.Data.DbType.DateTime, model.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", System.Data.DbType.String, model.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, model.Text);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, 1);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, model.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "ArticleID", System.Data.DbType.Int32, model.ArticleID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateReply(Hidistro.Entities.VShop.ReplyInfo reply)
		{
			bool result;
			switch (reply.MessageType)
			{
			case MessageType.Text:
				result = this.UpdateTextReply(reply as TextReplyInfo);
				return result;
			case MessageType.News:
			case MessageType.List:
				result = this.UpdateNewsReply(reply as NewsReplyInfo);
				return result;
			}
			result = this.UpdateTextReply(reply as TextReplyInfo);
			return result;
		}

		private bool UpdateNewsReply(NewsReplyInfo reply)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("Keys=@Keys,");
			stringBuilder.Append("MatchType=@MatchType,");
			stringBuilder.Append("ReplyType=@ReplyType,");
			stringBuilder.Append("MessageType=@MessageType,");
			stringBuilder.Append("IsDisable=@IsDisable,");
			stringBuilder.Append("LastEditDate=@LastEditDate,");
			stringBuilder.Append("LastEditor=@LastEditor,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("ArticleID=@ArticleID,");
			stringBuilder.Append("Type=@Type");
			stringBuilder.Append(" where ReplyId=@ReplyId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, reply.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", System.Data.DbType.Int32, (int)reply.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, (int)reply.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.Int32, (int)reply.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, reply.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", System.Data.DbType.DateTime, reply.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", System.Data.DbType.String, reply.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, "");
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "ArticleID", System.Data.DbType.Int32, reply.ArticleID);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, reply.Id);
			this.database.ExecuteNonQuery(sqlStringCommand);
			return true;
		}

		private bool UpdateTextReply(TextReplyInfo reply)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("Keys=@Keys,");
			stringBuilder.Append("MatchType=@MatchType,");
			stringBuilder.Append("ReplyType=@ReplyType,");
			stringBuilder.Append("MessageType=@MessageType,");
			stringBuilder.Append("IsDisable=@IsDisable,");
			stringBuilder.Append("LastEditDate=@LastEditDate,");
			stringBuilder.Append("LastEditor=@LastEditor,");
			stringBuilder.Append("Content=@Content,");
			stringBuilder.Append("Type=@Type,");
			stringBuilder.Append("ActivityId=@ActivityId,");
			stringBuilder.Append("ArticleID=@ArticleID ");
			stringBuilder.Append(" where ReplyId=@ReplyId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, reply.Keys);
			this.database.AddInParameter(sqlStringCommand, "MatchType", System.Data.DbType.Int32, (int)reply.MatchType);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, (int)reply.ReplyType);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.Int32, (int)reply.MessageType);
			this.database.AddInParameter(sqlStringCommand, "IsDisable", System.Data.DbType.Boolean, reply.IsDisable);
			this.database.AddInParameter(sqlStringCommand, "LastEditDate", System.Data.DbType.DateTime, reply.LastEditDate);
			this.database.AddInParameter(sqlStringCommand, "LastEditor", System.Data.DbType.String, reply.LastEditor);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, reply.Text);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, reply.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "ArticleID", System.Data.DbType.Int32, reply.ArticleID);
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, reply.Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GetNoMatchReplyID(int compareid)
		{
			string str = string.Empty;
			if (compareid > 0)
			{
				str = " and ReplyId<>" + compareid;
			}
			string query = "select top 1 ReplyId from vshop_Reply where ReplyType=@ReplyType " + str + "  order by ReplyId desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, 4);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public int GetSubscribeID(int compareid)
		{
			string str = string.Empty;
			if (compareid > 0)
			{
				str = " and ReplyId<>" + compareid;
			}
			string query = "select top 1 ReplyId from vshop_Reply where ReplyType=@ReplyType " + str + " order by ReplyId desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, 1);
			return Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
		}

		public bool HasReplyKey(string key)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM vshop_Reply WHERE Keys = @Keys");
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, key);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool HasReplyKey(string key, int replyid)
		{
			string str = string.Empty;
			if (replyid > 0)
			{
				str = " and ReplyId<>" + replyid;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM vshop_Reply WHERE Keys = @Keys " + str);
			this.database.AddInParameter(sqlStringCommand, "Keys", System.Data.DbType.String, key);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool UpdateReplyRelease(int id)
		{
			Hidistro.Entities.VShop.ReplyInfo reply = this.GetReply(id);
			StringBuilder stringBuilder = new StringBuilder();
			if (reply.IsDisable)
			{
				if ((reply.ReplyType & ReplyType.NoMatch) == ReplyType.NoMatch)
				{
					stringBuilder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 4);
				}
				if ((reply.ReplyType & ReplyType.Subscribe) == ReplyType.Subscribe)
				{
					stringBuilder.AppendFormat("update  vshop_Reply set IsDisable = 1 where ReplyType&{0}>0;", 1);
				}
			}
			stringBuilder.Append("update vshop_Reply set ");
			stringBuilder.Append("IsDisable=~IsDisable");
			stringBuilder.Append(" where ReplyId=@ReplyId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<Hidistro.Entities.VShop.ReplyInfo> GetAllReply()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId,ArticleID");
			stringBuilder.Append(" FROM vshop_Reply order by Replyid desc ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			List<Hidistro.Entities.VShop.ReplyInfo> list = new List<Hidistro.Entities.VShop.ReplyInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					Hidistro.Entities.VShop.ReplyInfo replyInfo = this.ReaderBind(dataReader);
					object obj;
					switch (replyInfo.MessageType)
					{
					case MessageType.Text:
					{
						TextReplyInfo textReplyInfo = replyInfo as TextReplyInfo;
						obj = dataReader["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					}
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.Id);
						list.Add(newsReplyInfo);
						break;
					}
					case (MessageType)3:
						goto IL_ED;
					default:
						goto IL_ED;
					}
					continue;
					IL_ED:
					TextReplyInfo textReplyInfo2 = replyInfo as TextReplyInfo;
					obj = dataReader["Content"];
					if (obj != null && obj != DBNull.Value)
					{
						textReplyInfo2.Text = obj.ToString();
					}
					list.Add(textReplyInfo2);
				}
			}
			return list;
		}

		public IList<Hidistro.Entities.VShop.ReplyInfo> GetReplies(ReplyType type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,Keys,MatchType,ReplyType,MessageType,IsDisable,LastEditDate,LastEditor,Content,Type,ActivityId,ArticleID ");
			stringBuilder.Append(" FROM vshop_Reply ");
			stringBuilder.Append(" where ReplyType = @ReplyType and IsDisable=0");
			stringBuilder.Append(" order by replyid desc");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ReplyType", System.Data.DbType.Int32, (int)type);
			List<Hidistro.Entities.VShop.ReplyInfo> list = new List<Hidistro.Entities.VShop.ReplyInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					Hidistro.Entities.VShop.ReplyInfo replyInfo = this.ReaderBind(dataReader);
					TextReplyInfo textReplyInfo;
					object obj;
					switch (replyInfo.MessageType)
					{
					case MessageType.Text:
						textReplyInfo = (replyInfo as TextReplyInfo);
						obj = dataReader["Content"];
						if (obj != null && obj != DBNull.Value)
						{
							textReplyInfo.Text = obj.ToString();
						}
						list.Add(textReplyInfo);
						break;
					case MessageType.News:
					case MessageType.List:
					{
						NewsReplyInfo newsReplyInfo = replyInfo as NewsReplyInfo;
						newsReplyInfo.NewsMsg = this.GetNewsReplyInfo(newsReplyInfo.Id);
						list.Add(newsReplyInfo);
						break;
					}
					case (MessageType)3:
						goto IL_11F;
					default:
						goto IL_11F;
					}
					continue;
					IL_11F:
					textReplyInfo = (replyInfo as TextReplyInfo);
					obj = dataReader["Content"];
					if (obj != null && obj != DBNull.Value)
					{
						textReplyInfo.Text = obj.ToString();
					}
					list.Add(textReplyInfo);
				}
			}
			return list;
		}

		public IList<NewsMsgInfo> GetNewsReplyInfo(int replyid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReplyId,MsgID,Title,ImageUrl,Url,Description,Content from vshop_Message ");
			stringBuilder.Append(" where ReplyId=@ReplyId ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ReplyId", System.Data.DbType.Int32, replyid);
			List<NewsMsgInfo> list = new List<NewsMsgInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(this.ReaderBindNewsRelpy(dataReader));
				}
			}
			return list;
		}

		private NewsMsgInfo ReaderBindNewsRelpy(System.Data.IDataReader dataReader)
		{
			NewsMsgInfo newsMsgInfo = new NewsMsgInfo();
			object obj = dataReader["MsgID"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Id = (int)obj;
			}
			obj = dataReader["Title"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Title = dataReader["Title"].ToString();
			}
			obj = dataReader["ImageUrl"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.PicUrl = dataReader["ImageUrl"].ToString();
			}
			obj = dataReader["Url"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Url = dataReader["Url"].ToString();
			}
			obj = dataReader["Description"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Description = dataReader["Description"].ToString();
			}
			obj = dataReader["Content"];
			if (obj != null && obj != DBNull.Value)
			{
				newsMsgInfo.Content = dataReader["Content"].ToString();
			}
			return newsMsgInfo;
		}

		public Hidistro.Entities.VShop.ReplyInfo ReaderBind(System.Data.IDataReader dataReader)
		{
			Hidistro.Entities.VShop.ReplyInfo replyInfo = null;
			object obj = dataReader["MessageType"];
			if (obj != null && obj != DBNull.Value)
			{
				if ((MessageType)obj == MessageType.Text)
				{
					replyInfo = new TextReplyInfo();
				}
				else
				{
					replyInfo = new NewsReplyInfo();
				}
			}
			obj = dataReader["ReplyId"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.Id = (int)obj;
			}
			replyInfo.Keys = dataReader["Keys"].ToString();
			obj = dataReader["MatchType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.MatchType = (MatchType)obj;
			}
			obj = dataReader["ReplyType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ReplyType = (ReplyType)obj;
			}
			obj = dataReader["MessageType"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.MessageType = (MessageType)obj;
			}
			obj = dataReader["IsDisable"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.IsDisable = (bool)obj;
			}
			obj = dataReader["LastEditDate"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.LastEditDate = (DateTime)obj;
			}
			replyInfo.LastEditor = dataReader["LastEditor"].ToString();
			obj = dataReader["ActivityId"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ActivityId = (int)obj;
			}
			obj = dataReader["ArticleID"];
			if (obj != null && obj != DBNull.Value)
			{
				replyInfo.ArticleID = (int)obj;
			}
			return replyInfo;
		}

		public int GetArticleIDByOldArticle(int replyid, MessageType msgtype)
		{
			int result = 0;
			ArticleInfo articleInfo = new ArticleInfo();
			switch (msgtype)
			{
			case MessageType.News:
			{
				NewsReplyInfo newsReplyInfo = this.GetReply(replyid) as NewsReplyInfo;
				if (newsReplyInfo != null && newsReplyInfo.NewsMsg != null && newsReplyInfo.NewsMsg.Count != 0)
				{
					articleInfo.Title = newsReplyInfo.NewsMsg[0].Title;
					articleInfo.ArticleType = ArticleType.News;
					articleInfo.Content = newsReplyInfo.NewsMsg[0].Content;
					articleInfo.ImageUrl = newsReplyInfo.NewsMsg[0].PicUrl;
					articleInfo.Url = newsReplyInfo.NewsMsg[0].Url;
					articleInfo.Memo = newsReplyInfo.NewsMsg[0].Description;
					articleInfo.PubTime = DateTime.Now;
					if (articleInfo.Url.Length > 10)
					{
						articleInfo.LinkType = LinkType.Userdefined;
					}
					else
					{
						articleInfo.LinkType = LinkType.ArticleDetail;
					}
					result = new ArticleDao().AddSingerArticle(articleInfo);
				}
				break;
			}
			case MessageType.List:
			{
				NewsReplyInfo newsReplyInfo = this.GetReply(replyid) as NewsReplyInfo;
				articleInfo.Title = newsReplyInfo.NewsMsg[0].Title;
				articleInfo.ArticleType = ArticleType.List;
				articleInfo.Content = newsReplyInfo.NewsMsg[0].Content;
				articleInfo.ImageUrl = newsReplyInfo.NewsMsg[0].PicUrl;
				articleInfo.Url = newsReplyInfo.NewsMsg[0].Url;
				articleInfo.Memo = newsReplyInfo.NewsMsg[0].Description;
				articleInfo.PubTime = DateTime.Now;
				if (articleInfo.Url.Length > 10)
				{
					articleInfo.LinkType = LinkType.Userdefined;
				}
				else
				{
					articleInfo.LinkType = LinkType.ArticleDetail;
				}
				List<ArticleItemsInfo> list = new List<ArticleItemsInfo>();
				if (newsReplyInfo.NewsMsg != null && newsReplyInfo.NewsMsg.Count > 0)
				{
					int num = 0;
					foreach (NewsMsgInfo current in newsReplyInfo.NewsMsg)
					{
						num++;
						if (num != 1)
						{
							ArticleItemsInfo articleItemsInfo = new ArticleItemsInfo();
							articleItemsInfo.Title = current.Title;
							articleItemsInfo.Content = current.Content;
							articleItemsInfo.ImageUrl = current.PicUrl;
							articleItemsInfo.Url = current.Url;
							articleItemsInfo.ArticleId = 0;
							if (articleItemsInfo.Url.Length > 10)
							{
								articleItemsInfo.LinkType = LinkType.Userdefined;
							}
							else
							{
								articleItemsInfo.LinkType = LinkType.ArticleDetail;
							}
							list.Add(articleItemsInfo);
						}
					}
				}
				articleInfo.ItemsInfo = list;
				result = new ArticleDao().AddMultiArticle(articleInfo);
				break;
			}
			}
			return result;
		}
	}
}
