using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Comments
{
	public class ProductConsultationDao
	{
		private Database database;

		public ProductConsultationDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
			if (consultationQuery.Type == ConsultationReplyType.NoReply)
			{
				stringBuilder.Append(" AND ReplyUserId IS NULL ");
			}
			else if (consultationQuery.Type == ConsultationReplyType.Replyed)
			{
				stringBuilder.Append(" AND ReplyUserId IS NOT NULL");
			}
			if (consultationQuery.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0}", consultationQuery.ProductId);
			}
			if (consultationQuery.UserId > 0)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", consultationQuery.UserId);
			}
			if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
			}
			if (consultationQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (CategoryId = {0}", consultationQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
			}
			if (consultationQuery.HasReplied.HasValue)
			{
				if (consultationQuery.HasReplied.Value)
				{
					stringBuilder.AppendFormat(" AND ReplyText is not null", new object[0]);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ReplyText is null", new object[0]);
				}
			}
			return DataHelper.PagingByRownumber(consultationQuery.PageIndex, consultationQuery.PageSize, consultationQuery.SortBy, consultationQuery.SortOrder, consultationQuery.IsCount, "vw_Hishop_ProductConsultations", "ProductId", stringBuilder.ToString(), "*");
		}

		public int GetProductConsultationsCount(int productId, bool includeUnReplied)
		{
			StringBuilder stringBuilder = new StringBuilder("SELECT count(1) FROM Hishop_ProductConsultations WHERE ProductId =" + productId);
			if (!includeUnReplied)
			{
				stringBuilder.Append(" AND ReplyText is not null");
			}
			return (int)this.database.ExecuteScalar(System.Data.CommandType.Text, stringBuilder.ToString());
		}

		public ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			ProductConsultationInfo result = null;
			string query = "SELECT * FROM Hishop_ProductConsultations WHERE ConsultationId=@ConsultationId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int32, consultationId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ProductConsultationInfo>(dataReader);
			}
			return result;
		}

		public bool InsertProductConsultation(ProductConsultationInfo productConsultation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductConsultations(ProductId, UserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productConsultation.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, productConsultation.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, productConsultation.UserName);
			this.database.AddInParameter(sqlStringCommand, "UserEmail", System.Data.DbType.String, productConsultation.UserEmail);
			this.database.AddInParameter(sqlStringCommand, "ConsultationText", System.Data.DbType.String, productConsultation.ConsultationText);
			this.database.AddInParameter(sqlStringCommand, "ConsultationDate", System.Data.DbType.DateTime, productConsultation.ConsultationDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId");
			this.database.AddInParameter(sqlStringCommand, "ReplyText", System.Data.DbType.String, productConsultation.ReplyText);
			this.database.AddInParameter(sqlStringCommand, "ReplyDate", System.Data.DbType.DateTime, productConsultation.ReplyDate);
			this.database.AddInParameter(sqlStringCommand, "ReplyUserId", System.Data.DbType.Int32, productConsultation.ReplyUserId);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int32, productConsultation.ConsultationId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int DeleteProductConsultation(int consultationId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductConsultations WHERE consultationId = @consultationId");
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int64, consultationId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
