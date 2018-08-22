using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class PrizeResultDao
	{
		private Database _db = null;

		public PrizeResultDao()
		{
			this._db = DatabaseFactory.CreateDatabase();
		}

		public bool IsCanPrize(int gameId, int userid)
		{
			System.Data.Common.DbCommand storedProcCommand = this._db.GetStoredProcCommand("cp_IsCanPrize");
			this._db.AddInParameter(storedProcCommand, "@GameId", System.Data.DbType.Int32, gameId);
			this._db.AddInParameter(storedProcCommand, "@UserId", System.Data.DbType.Int32, userid);
			this._db.AddOutParameter(storedProcCommand, "@Result", System.Data.DbType.Int32, 4);
			this._db.ExecuteNonQuery(storedProcCommand);
			object value = storedProcCommand.Parameters["@Result"].Value;
			bool result;
			if (value != null && !string.IsNullOrEmpty(value.ToString()))
			{
				CanPrizeError canPrizeError = (CanPrizeError)int.Parse(value.ToString());
				if (canPrizeError != CanPrizeError.可以玩)
				{
					throw new Exception(canPrizeError.ToString());
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool AddPrizeLog(PrizeResultInfo model)
		{
			System.Data.Common.DbCommand storedProcCommand = this._db.GetStoredProcCommand("cp_GamePrize");
			this._db.AddInParameter(storedProcCommand, "@GameId", System.Data.DbType.Int32, model.GameId);
			this._db.AddInParameter(storedProcCommand, "@PrizeId", System.Data.DbType.Int32, model.PrizeId);
			this._db.AddInParameter(storedProcCommand, "@UserId", System.Data.DbType.Int32, model.UserId);
			this._db.AddOutParameter(storedProcCommand, "@Result", System.Data.DbType.Int32, 4);
			this._db.ExecuteNonQuery(storedProcCommand);
			object value = storedProcCommand.Parameters["@Result"].Value;
			return value != null && !string.IsNullOrEmpty(value.ToString()) && int.Parse(value.ToString()) > 0;
		}

		public bool UpdatePrizeLogStatus(int logId)
		{
			string query = "Update Hishop_PromotionGameResultMembersLog set IsUsed=1 where LogId=@LogId and IsUsed=0";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@LogId", System.Data.DbType.Int32, logId);
			int num = this._db.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public IList<PrizeResultViewInfo> GetPrizeLogLists(int gameId, int pageIndex, int pageSize)
		{
			List<PrizeResultViewInfo> list = new List<PrizeResultViewInfo>();
			string selectFields = "LogId, PlayTime, UserId, UserName,Prize, PrizeGrade, PrizeType, GivePoint, GiveCouponId, GiveShopBookId,PrizeId ";
			string filter = string.Format(" GameId={0} and  PrizeId!=0 ", gameId);
			System.Data.DataTable dataTable = DataHelper.PagingByTopnotin(pageIndex, pageSize, "LogId", SortAction.Desc, false, "vw_Hishop_PrizesRecord", "LogId", filter, selectFields).Data as System.Data.DataTable;
			if (dataTable != null)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					list.Add(new PrizeResultViewInfo
					{
						LogId = int.Parse(dataRow["LogId"].ToString()),
						PlayTime = DateTime.Parse(dataRow["PlayTime"].ToString()),
						UserId = int.Parse(dataRow["UserId"].ToString()),
						UserName = dataRow["UserName"].ToString(),
						PrizeType = (PrizeType)int.Parse(dataRow["PrizeType"].ToString()),
						PrizeGrade = (PrizeGrade)int.Parse(dataRow["PrizeGrade"].ToString()),
						GivePoint = int.Parse(dataRow["GivePoint"].ToString()),
						GiveCouponId = dataRow["GiveCouponId"].ToString(),
						GiveShopBookId = dataRow["GiveShopBookId"].ToString(),
						PrizeName = dataRow["Prize"].ToString()
					});
				}
			}
			return list;
		}

		public DbQueryResult GetPrizeLogLists(PrizesDeliveQuery query)
		{
			string selectFields = "LogId, PlayTime, UserId, UserName, PrizeGrade, PrizeType, GivePoint, GiveCouponId, GiveShopBookId,Prize ";
			StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
			if (query.GameId.HasValue)
			{
				stringBuilder.AppendFormat(" and GameId={0}", query.GameId);
			}
			if (query.IsUsed.HasValue)
			{
				stringBuilder.AppendFormat(" and IsUsed={0}", query.IsUsed);
			}
			if (!string.IsNullOrEmpty(query.StartDate))
			{
				stringBuilder.AppendFormat(" and PlayTime>='{0}'", query.StartDate);
			}
			if (!string.IsNullOrEmpty(query.EndDate))
			{
				stringBuilder.AppendFormat(" and PlayTime<'{0}'", query.EndDate);
			}
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PrizesRecord", "LogId", stringBuilder.ToString(), selectFields);
		}
	}
}
