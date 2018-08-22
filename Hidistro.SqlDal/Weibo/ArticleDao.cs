using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Weibo;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Weibo
{
	public class ArticleDao
	{
		private Database database;

		private string articledetailUrl = "http://" + Globals.DomainName + Globals.ApplicationPath + "/vshop/ArticleDetail.aspx?$1";

		public ArticleDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool UpdateMedia_Id(int type, int id, string mediaid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (type == 0)
			{
				stringBuilder.Append("UPDATE vshop_Article SET mediaid=@mediaid WHERE ArticleId=@ID");
			}
			else
			{
				stringBuilder.Append("UPDATE vshop_ArticleItems SET mediaid=@mediaid WHERE Id=@ID");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, id);
			this.database.AddInParameter(sqlStringCommand, "mediaid", System.Data.DbType.String, mediaid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int AddSingerArticle(ArticleInfo article)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO vshop_Article(").Append("Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,IsShare)").Append(" VALUES (@Title,@ArticleType,@LinkType,@Content,@ImageUrl,@Url,@Memo,@PubTime,@IsShare);select @@identity");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "ArticleType", System.Data.DbType.Int32, article.ArticleType);
			this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, article.LinkType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, article.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "IsShare", System.Data.DbType.Boolean, article.IsShare);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, article.Memo);
			this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, DateTime.Now);
			int num = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
			if (article.LinkType == LinkType.ArticleDetail)
			{
				string query = "update vshop_Article set Url=@Url where ArticleId=@ArticleId";
				sqlStringCommand = this.database.GetSqlStringCommand(query);
				article.ArticleId = num;
				article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
				this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
				this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return num;
		}

		public int AddMultiArticle(ArticleInfo article)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO vshop_Article(").Append("Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,IsShare)").Append(" VALUES (@Title,@ArticleType,@LinkType,@Content,@ImageUrl,@Url,@Memo,@PubTime,@IsShare);select @@identity");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "ArticleType", System.Data.DbType.Int32, article.ArticleType);
			this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, article.LinkType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, article.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, article.Memo);
			this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "IsShare", System.Data.DbType.Boolean, article.IsShare);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num;
			if (int.TryParse(obj.ToString(), out num))
			{
				if (article.LinkType == LinkType.ArticleDetail)
				{
					string query = "update vshop_Article set Url=@Url where ArticleId=@ArticleId";
					sqlStringCommand = this.database.GetSqlStringCommand(query);
					article.ArticleId = num;
					article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
					this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
					this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
				foreach (ArticleItemsInfo current in article.ItemsInfo)
				{
					stringBuilder = new StringBuilder();
					stringBuilder.Append("insert into vshop_ArticleItems(");
					stringBuilder.Append("ArticleId,Title,Content,ImageUrl,Url,LinkType)");
					stringBuilder.Append(" values (");
					stringBuilder.Append("@ArticleId,@Title,@Content,@ImageUrl,@Url,@LinkType);select @@identity");
					sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, num);
					this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, current.Title);
					this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, current.Content);
					this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, current.ImageUrl);
					this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, current.Url);
					this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, current.LinkType);
					int id = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
					if (current.LinkType == LinkType.ArticleDetail)
					{
						string query = "update vshop_ArticleItems set Url=@Url where Id=@Id";
						sqlStringCommand = this.database.GetSqlStringCommand(query);
						current.Id = id;
						current.Url = this.articledetailUrl.Replace("$1", "iid=" + current.Id.ToString());
						this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, current.Url);
						this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, current.Id);
						this.database.ExecuteNonQuery(sqlStringCommand);
					}
				}
			}
			return num;
		}

		public bool UpdateSingleArticle(ArticleInfo article)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_Article SET ").Append("Title=@Title,").Append("ArticleType=@ArticleType,").Append("LinkType=@LinkType,").Append("Content=@Content,").Append("ImageUrl=@ImageUrl,").Append("Url=@Url,").Append("Memo=@Memo,").Append("IsShare=@IsShare,").Append("PubTime=@PubTime").Append(" WHERE ArticleId=@ArticleId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (article.LinkType == LinkType.ArticleDetail)
			{
				article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
			}
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "ArticleType", System.Data.DbType.Int32, article.ArticleType);
			this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, article.LinkType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, article.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, article.Memo);
			this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, article.PubTime);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
			this.database.AddInParameter(sqlStringCommand, "IsShare", System.Data.DbType.Boolean, article.IsShare);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool UpdateMultiArticle(ArticleInfo article)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE vshop_Article SET ").Append("Title=@Title,").Append("ArticleType=@ArticleType,").Append("LinkType=@LinkType,").Append("Content=@Content,").Append("ImageUrl=@ImageUrl,").Append("Url=@Url,").Append("Memo=@Memo,").Append("IsShare=@IsShare,").Append("PubTime=@PubTime").Append(" WHERE ArticleId=@ArticleId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "ArticleType", System.Data.DbType.Int32, article.ArticleType);
			this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, article.LinkType);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, article.ImageUrl);
			this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, article.Memo);
			this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, article.PubTime);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
			this.database.AddInParameter(sqlStringCommand, "IsShare", System.Data.DbType.Boolean, article.IsShare);
			bool flag = this.database.ExecuteNonQuery(sqlStringCommand) > 0;
			if (flag)
			{
				foreach (ArticleItemsInfo current in article.ItemsInfo)
				{
					current.ArticleId = article.ArticleId;
					if (current.LinkType == LinkType.ArticleDetail)
					{
						current.Url = this.articledetailUrl.Replace("$1", "iid=" + current.Id.ToString());
					}
					this.UpdateArticleItem(current);
				}
				string text = "delete from vshop_ArticleItems WHERE ArticleId=@ArticleId and PubTime<>@PubTime";
				if (article.LinkType == LinkType.ArticleDetail)
				{
					text += ";update vshop_Article set Url=@Url where ArticleId=@ArticleId";
				}
				sqlStringCommand = this.database.GetSqlStringCommand(text);
				article.Url = this.articledetailUrl.Replace("$1", "sid=" + article.ArticleId.ToString());
				this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, article.Url);
				this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
				this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, article.PubTime);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return flag;
		}

		public IList<ArticleItemsInfo> GetArticleItems(int articleid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select Id,ArticleId,Title,Content,ImageUrl,Url,LinkType,MediaId,PubTime from vshop_ArticleItems ");
			stringBuilder.Append(" where ArticleId=@ArticleId order by ID asc ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleid);
			IList<ArticleItemsInfo> result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<ArticleItemsInfo>(dataReader);
			}
			return result;
		}

		public ArticleItemsInfo GetArticleItemsInfo(int itemid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			ArticleItemsInfo articleItemsInfo = new ArticleItemsInfo();
			stringBuilder.Append("select Id,ArticleId,Title,Content,ImageUrl,Url,LinkType,PubTime,mediaid from vshop_ArticleItems ");
			stringBuilder.Append(" where Id=@Id order by ID asc ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, itemid);
			System.Data.DataTable dataTable = this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
			ArticleItemsInfo result;
			if (dataTable.Rows.Count > 0)
			{
				articleItemsInfo.Id = int.Parse(dataTable.Rows[0]["ID"].ToString());
				articleItemsInfo.Title = dataTable.Rows[0]["Title"].ToString();
				articleItemsInfo.LinkType = (LinkType)int.Parse(dataTable.Rows[0]["LinkType"].ToString());
				articleItemsInfo.ArticleId = int.Parse(dataTable.Rows[0]["ArticleId"].ToString());
				articleItemsInfo.ImageUrl = dataTable.Rows[0]["ImageUrl"].ToString();
				articleItemsInfo.Url = dataTable.Rows[0]["Url"].ToString();
				articleItemsInfo.Content = dataTable.Rows[0]["Content"].ToString();
				articleItemsInfo.PubTime = DateTime.Parse(dataTable.Rows[0]["PubTime"].ToString());
				if (dataTable.Rows[0]["MediaId"] != null)
				{
					articleItemsInfo.MediaId = dataTable.Rows[0]["MediaId"].ToString();
				}
				else
				{
					articleItemsInfo.MediaId = "";
				}
				result = articleItemsInfo;
			}
			else
			{
				result = null;
			}
			return result;
		}

		public ArticleInfo GetArticleInfo(int articleid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ArticleId,Title,ArticleType,LinkType,Content,ImageUrl,Url,Memo,PubTime,MediaId,IsShare from vshop_Article ");
			stringBuilder.Append(" where ArticleId=@ArticleId ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleid);
			ArticleInfo result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = this.ReaderBind(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetNoImgMsgIdArticleList()
		{
			string query = "select top 10 ArticleId,ImageUrl from vshop_Article where len(MediaId)<5 or MediaId is null";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetNoImgMsgIdArticleItemList()
		{
			string query = "select top 10 ID,ImageUrl from vshop_ArticleItems  where len(MediaId)<5 or MediaId is null";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public bool DeleteArticle(int articleId)
		{
			string query = "DELETE FROM vshop_Article WHERE ArticleId=@ArticleId;DELETE FROM vshop_ArticleItems WHERE ArticleId=@ArticleId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataSet ArticleIsInWeiXinReply(int articleId)
		{
			string query = string.Concat(new object[]
			{
				"select count(0) from vshop_Reply where ArticleID=",
				articleId,
				";select count(0) from Weibo_Reply where ArticleID=",
				articleId,
				";select count(0) from vshop_AliFuwuReply where ArticleID=",
				articleId
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}

		public DbQueryResult GetArticleRequest(ArticleQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.ArticleType > 0)
			{
				stringBuilder.AppendFormat(" ArticleType = {0} ", query.ArticleType);
			}
			if (query.IsShare >= 0)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" IsShare = {0} ", query.IsShare);
			}
			if (!string.IsNullOrEmpty(query.Title))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("( Title LIKE '%{0}%' or Memo like '%{0}%' or exists (select 1 from  vshop_ArticleItems where title like '%{0}%' and ArticleId=vshop_Article.ArticleId))", DataHelper.CleanSearchString(query.Title));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vshop_Article ", "ArticleId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public ArticleInfo ReaderBind(System.Data.IDataReader dataReader)
		{
			ArticleInfo articleInfo = null;
			if (dataReader.Read())
			{
				articleInfo = new ArticleInfo();
				object obj = dataReader["ArticleId"];
				if (obj != null && obj != DBNull.Value)
				{
					articleInfo.ArticleId = (int)obj;
				}
				articleInfo.Title = dataReader["Title"].ToString();
				obj = dataReader["ArticleType"];
				if (obj != null && obj != DBNull.Value)
				{
					articleInfo.ArticleType = (ArticleType)obj;
				}
				obj = dataReader["LinkType"];
				if (obj != null && obj != DBNull.Value)
				{
					articleInfo.LinkType = (LinkType)obj;
				}
				articleInfo.Content = dataReader["Content"].ToString();
				articleInfo.ImageUrl = dataReader["ImageUrl"].ToString();
				articleInfo.Url = dataReader["Url"].ToString();
				articleInfo.Memo = dataReader["Memo"].ToString();
				articleInfo.IsShare = (bool)dataReader["IsShare"];
				obj = dataReader["PubTime"];
				if (obj != null && obj != DBNull.Value)
				{
					articleInfo.PubTime = (DateTime)obj;
				}
				if (articleInfo.ArticleType == ArticleType.List)
				{
					articleInfo.ItemsInfo = this.GetArticleItems(articleInfo.ArticleId);
				}
				obj = dataReader["MediaId"];
				if (obj != null && obj != DBNull.Value)
				{
					articleInfo.MediaId = obj.ToString();
				}
				else
				{
					articleInfo.MediaId = "";
				}
			}
			return articleInfo;
		}

		public void UpdateArticleItem(ArticleItemsInfo iteminfo)
		{
			string query = string.Empty;
			if (iteminfo.Id > 0)
			{
				query = "update vshop_ArticleItems set Title=@Title,Content=@Content,ImageUrl=@ImageUrl,Url=@Url,LinkType=@LinkType,PubTime=@PubTime where Id=@Id";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, iteminfo.Id);
				this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, iteminfo.Title);
				this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, iteminfo.Content);
				this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, iteminfo.ImageUrl);
				this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, iteminfo.Url);
				this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, iteminfo.LinkType);
				this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, iteminfo.PubTime);
				int id = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("insert into vshop_ArticleItems(");
				stringBuilder.Append("ArticleId,Title,Content,ImageUrl,Url,LinkType,PubTime)");
				stringBuilder.Append(" values (");
				stringBuilder.Append("@ArticleId,@Title,@Content,@ImageUrl,@Url,@LinkType,@PubTime);select @@identity");
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, iteminfo.ArticleId);
				this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, iteminfo.Title);
				this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, iteminfo.Content);
				this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, iteminfo.ImageUrl);
				this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, iteminfo.Url);
				this.database.AddInParameter(sqlStringCommand, "LinkType", System.Data.DbType.Int32, iteminfo.LinkType);
				this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, iteminfo.PubTime);
				int id = int.Parse(this.database.ExecuteScalar(sqlStringCommand).ToString());
				if (iteminfo.LinkType == LinkType.ArticleDetail)
				{
					string query2 = "update vshop_ArticleItems set Url=@Url where Id=@Id";
					sqlStringCommand = this.database.GetSqlStringCommand(query2);
					iteminfo.Id = id;
					iteminfo.Url = this.articledetailUrl.Replace("$1", "iid=" + iteminfo.Id.ToString());
					this.database.AddInParameter(sqlStringCommand, "Url", System.Data.DbType.String, iteminfo.Url);
					this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, iteminfo.Id);
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
			}
		}
	}
}
