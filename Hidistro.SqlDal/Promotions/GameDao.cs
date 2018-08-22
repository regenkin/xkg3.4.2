using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class GameDao
	{
		private Database _database = null;

		public GameDao()
		{
			this._database = DatabaseFactory.CreateDatabase();
		}

		public bool Create(GameInfo model, out int gameId)
		{
			gameId = -1;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO [Hishop_PromotionGame]\r\n                       ([GameType]\r\n                       ,[GameTitle]\r\n                       ,[Description]\r\n                       ,[BeginTime]\r\n                       ,[EndTime]\r\n                       ,[ApplyMembers]\r\n                       ,[DefualtGroup]\r\n                       ,[CustomGroup]\r\n                       ,[NeedPoint]\r\n                       ,[GivePoint]\r\n                       ,[OnlyGiveNotPrizeMember]\r\n                       ,[PlayType]\r\n                       ,[NotPrzeDescription]\r\n                       ,[GameUrl]\r\n                       ,[GameQRCodeAddress]\r\n                       ,[Status],[KeyWork]\r\n                       ,[LimitEveryDay]\r\n                       ,[MaximumDailyLimit]\r\n                       ,[PrizeRate]\r\n                       ,[MemberCheck])\r\n                        VALUES ");
			stringBuilder.Append("(@GameType\r\n                       ,@GameTitle\r\n                       ,@Description\r\n                       ,@BeginTime\r\n                       ,@EndTime\r\n                       ,@ApplyMembers\r\n                       ,@DefualtGroup\r\n                       ,@CustomGroup\r\n                       ,@NeedPoint\r\n                       ,@GivePoint\r\n                       ,@OnlyGiveNotPrizeMember\r\n                       ,@PlayType\r\n                       ,@NotPrzeDescription\r\n                       ,@GameUrl\r\n                       ,@GameQRCodeAddress\r\n                       ,@Status,@KeyWork\r\n                       ,@LimitEveryDay\r\n                       ,@MaximumDailyLimit\r\n                       ,@PrizeRate\r\n                       ,@MemberCheck);");
			stringBuilder.Append("select @@identity;");
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(stringBuilder.ToString());
			this._database.AddInParameter(sqlStringCommand, "@GameType", System.Data.DbType.Int32, (int)model.GameType);
			this._database.AddInParameter(sqlStringCommand, "@GameTitle", System.Data.DbType.String, model.GameTitle);
			this._database.AddInParameter(sqlStringCommand, "@Description", System.Data.DbType.String, model.Description);
			this._database.AddInParameter(sqlStringCommand, "@BeginTime", System.Data.DbType.DateTime, model.BeginTime);
			this._database.AddInParameter(sqlStringCommand, "@EndTime", System.Data.DbType.DateTime, model.EndTime);
			this._database.AddInParameter(sqlStringCommand, "@ApplyMembers", System.Data.DbType.String, model.ApplyMembers);
			this._database.AddInParameter(sqlStringCommand, "@DefualtGroup", System.Data.DbType.String, model.DefualtGroup);
			this._database.AddInParameter(sqlStringCommand, "@CustomGroup", System.Data.DbType.String, model.CustomGroup);
			this._database.AddInParameter(sqlStringCommand, "@NeedPoint", System.Data.DbType.Int32, model.NeedPoint);
			this._database.AddInParameter(sqlStringCommand, "@GivePoint", System.Data.DbType.Int32, model.GivePoint);
			this._database.AddInParameter(sqlStringCommand, "@OnlyGiveNotPrizeMember", System.Data.DbType.Boolean, model.OnlyGiveNotPrizeMember);
			this._database.AddInParameter(sqlStringCommand, "@PlayType", System.Data.DbType.Int32, (int)model.PlayType);
			this._database.AddInParameter(sqlStringCommand, "@NotPrzeDescription", System.Data.DbType.String, model.NotPrzeDescription);
			this._database.AddInParameter(sqlStringCommand, "@GameUrl", System.Data.DbType.String, model.GameUrl);
			this._database.AddInParameter(sqlStringCommand, "@GameQRCodeAddress", System.Data.DbType.String, model.GameQRCodeAddress);
			this._database.AddInParameter(sqlStringCommand, "@Status", System.Data.DbType.Int32, 0);
			this._database.AddInParameter(sqlStringCommand, "@KeyWork", System.Data.DbType.String, model.KeyWork);
			this._database.AddInParameter(sqlStringCommand, "@LimitEveryDay", System.Data.DbType.String, model.LimitEveryDay);
			this._database.AddInParameter(sqlStringCommand, "@MaximumDailyLimit", System.Data.DbType.String, model.MaximumDailyLimit);
			this._database.AddInParameter(sqlStringCommand, "@PrizeRate", System.Data.DbType.String, model.PrizeRate);
			this._database.AddInParameter(sqlStringCommand, "@MemberCheck", System.Data.DbType.String, model.MemberCheck);
			try
			{
				object obj = this._database.ExecuteScalar(sqlStringCommand);
				if (obj != null)
				{
					if (!int.TryParse(obj.ToString(), out gameId))
					{
						gameId = -1;
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return gameId > 0;
		}

		public bool Update(GameInfo model, bool isNotStart = true)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Update [Hishop_PromotionGame] set [GameTitle]=@GameTitle\r\n                       ,[Description]=@Description\r\n                       ,[BeginTime]=@BeginTime\r\n                       ,[EndTime]=@EndTime\r\n                       ,[ApplyMembers]=@ApplyMembers\r\n                       ,[DefualtGroup]=@DefualtGroup\r\n                       ,[CustomGroup]=@CustomGroup\r\n                       ,[NeedPoint]=@NeedPoint\r\n                       ,[GivePoint]=@GivePoint\r\n                       ,[OnlyGiveNotPrizeMember]=@OnlyGiveNotPrizeMember\r\n                       ,[PlayType]=@PlayType\r\n                       ,[NotPrzeDescription]=@NotPrzeDescription\r\n                       ,[GameUrl]=@GameUrl\r\n                       ,[GameQRCodeAddress]=@GameQRCodeAddress\r\n                       ,[LimitEveryDay]=@LimitEveryDay\r\n                       ,[MaximumDailyLimit]=@MaximumDailyLimit\r\n                       ,[PrizeRate]=@PrizeRate\r\n                       ,[MemberCheck]=@MemberCheck\r\n                       Where GameId=@GameId\r\n                        ");
			if (isNotStart)
			{
				stringBuilder.Append(" and getdate()< BeginTime");
			}
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(stringBuilder.ToString());
			this._database.AddInParameter(sqlStringCommand, "@GameTitle", System.Data.DbType.String, model.GameTitle);
			this._database.AddInParameter(sqlStringCommand, "@Description", System.Data.DbType.String, model.Description);
			this._database.AddInParameter(sqlStringCommand, "@BeginTime", System.Data.DbType.DateTime, model.BeginTime);
			this._database.AddInParameter(sqlStringCommand, "@EndTime", System.Data.DbType.DateTime, model.EndTime);
			this._database.AddInParameter(sqlStringCommand, "@ApplyMembers", System.Data.DbType.String, model.ApplyMembers);
			this._database.AddInParameter(sqlStringCommand, "@DefualtGroup", System.Data.DbType.String, model.DefualtGroup);
			this._database.AddInParameter(sqlStringCommand, "@CustomGroup", System.Data.DbType.String, model.CustomGroup);
			this._database.AddInParameter(sqlStringCommand, "@NeedPoint", System.Data.DbType.Int32, model.NeedPoint);
			this._database.AddInParameter(sqlStringCommand, "@GivePoint", System.Data.DbType.Int32, model.GivePoint);
			this._database.AddInParameter(sqlStringCommand, "@OnlyGiveNotPrizeMember", System.Data.DbType.Boolean, model.OnlyGiveNotPrizeMember);
			this._database.AddInParameter(sqlStringCommand, "@PlayType", System.Data.DbType.Int32, (int)model.PlayType);
			this._database.AddInParameter(sqlStringCommand, "@NotPrzeDescription", System.Data.DbType.String, Globals.SubStr(model.NotPrzeDescription, 100, ""));
			this._database.AddInParameter(sqlStringCommand, "@GameUrl", System.Data.DbType.String, model.GameUrl);
			this._database.AddInParameter(sqlStringCommand, "@GameQRCodeAddress", System.Data.DbType.String, model.GameQRCodeAddress);
			this._database.AddInParameter(sqlStringCommand, "@LimitEveryDay", System.Data.DbType.Int32, model.LimitEveryDay);
			this._database.AddInParameter(sqlStringCommand, "@MaximumDailyLimit", System.Data.DbType.Int32, model.MaximumDailyLimit);
			this._database.AddInParameter(sqlStringCommand, "@PrizeRate", System.Data.DbType.Double, model.PrizeRate);
			this._database.AddInParameter(sqlStringCommand, "@MemberCheck", System.Data.DbType.Int32, model.MemberCheck);
			this._database.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, model.GameId);
			bool result;
			try
			{
				int num = this._database.ExecuteNonQuery(sqlStringCommand);
				result = (num > 0);
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}

		public bool Delete(params int[] gameIds)
		{
			string arg = string.Join<int>(",", gameIds);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Delete From Hishop_PromotionGameResultMembersLog where GameId in({0});", arg);
			stringBuilder.AppendFormat("Delete From Hishop_PromotionGamePrizes where GameId in({0});", arg);
			stringBuilder.AppendFormat("Delete From Hishop_PromotionGame where GameId in({0});", arg);
			stringBuilder.AppendFormat("Delete From Hishop_PromotionWinningPool where GameId in({0});", arg);
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(stringBuilder.ToString());
			int num = this._database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool UpdateStatus(int gameId, GameStatus status)
		{
			string query = "Update Hishop_PromotionGame set [Status]=@Status where GameId=@GameId";
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
			this._database.AddInParameter(sqlStringCommand, "@Status", System.Data.DbType.Int32, (int)status);
			this._database.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			int num = this._database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool UpdateOutOfDateStatus()
		{
			string query = "Update Hishop_PromotionGame set [Status]=1 where EndTime<getdate()";
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
			int num = this._database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public GameInfo GetModelByGameId(int gameId)
		{
			string query = "SELECT * FROM [Hishop_PromotionGame] where GameId=@GameId";
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
			this._database.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			GameInfo result;
			using (System.Data.IDataReader dataReader = this._database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GameInfo>(dataReader);
			}
			return result;
		}

		public GameInfo GetModelByGameId(string keyWord)
		{
			string query = "SELECT * FROM [Hishop_PromotionGame] where KeyWork=@KeyWork";
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
			this._database.AddInParameter(sqlStringCommand, "@KeyWork", System.Data.DbType.String, keyWord);
			GameInfo result;
			using (System.Data.IDataReader dataReader = this._database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GameInfo>(dataReader);
			}
			return result;
		}

		public IEnumerable<GameInfo> GetLists(GameSearch search)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM [vw_Hishop_PromotionGame] where 1=1 ");
			if (!string.IsNullOrEmpty(search.Status))
			{
				stringBuilder.AppendFormat(" and [Status]={0}", search.Status);
			}
			if (search.BeginTime.HasValue)
			{
				stringBuilder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime);
			}
			if (search.EndTime.HasValue)
			{
				stringBuilder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime);
			}
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(stringBuilder.ToString());
			IEnumerable<GameInfo> result;
			using (System.Data.IDataReader dataReader = this._database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<GameInfo>(dataReader);
			}
			return result;
		}

		public DbQueryResult GetGameList(GameSearch search)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (search.GameType.HasValue)
			{
				stringBuilder.AppendFormat(" and [GameType]={0}", search.GameType);
			}
			if (!string.IsNullOrEmpty(search.Status))
			{
				stringBuilder.AppendFormat(" and [Status]={0}", search.Status);
			}
			if (search.BeginTime.HasValue)
			{
				stringBuilder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime);
			}
			if (search.EndTime.HasValue)
			{
				stringBuilder.AppendFormat(" and [EndTime]>'{0}'", search.EndTime);
			}
			string selectFields = "GameId, GameType, GameTitle, Description, BeginTime, EndTime, ApplyMembers,DefualtGroup,CustomGroup, NeedPoint, GivePoint, OnlyGiveNotPrizeMember, PlayType, NotPrzeDescription, GameUrl, GameQRCodeAddress, Status";
			return DataHelper.PagingByTopnotin(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "Hishop_PromotionGame", "GameId", stringBuilder.ToString(), selectFields);
		}

		public GameInfo GetGameInfoById(int gameId)
		{
			string query = "Select * From [Hishop_PromotionGame] where GameId=@GameId";
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
			this._database.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			GameInfo result;
			using (System.Data.IDataReader dataReader = this._database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GameInfo>(dataReader);
			}
			return result;
		}

		public DbQueryResult GetGameListByView(GameSearch search)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (search.GameType.HasValue)
			{
				stringBuilder.AppendFormat(" and [GameType]={0}", search.GameType);
			}
			if (!string.IsNullOrEmpty(search.Status))
			{
				if (search.Status == "1")
				{
					stringBuilder.AppendFormat(" and ([Status]={0} or EndTime<getdate())", search.Status);
				}
				else
				{
					stringBuilder.AppendFormat(" and ([Status]={0} and EndTime>getdate())", search.Status);
				}
			}
			if (search.BeginTime.HasValue)
			{
				stringBuilder.AppendFormat(" and [BeginTime]>='{0}'", search.BeginTime.Value.ToString("yyyy-MM-dd"));
			}
			if (search.EndTime.HasValue)
			{
				stringBuilder.AppendFormat(" and [EndTime]<'{0}'", search.EndTime.Value.AddDays(1.0).ToString("yyyy-MM-dd"));
			}
			string selectFields = " GameID, GameType,GameTitle, BeginTime ,EndTime,PlayType,GameUrl,GameQRCodeAddress ,Status,TotalCount,PrizeCount,LimitEveryDay,MaximumDailyLimit ";
			return DataHelper.PagingByTopnotin(search.PageIndex, search.PageSize, search.SortBy, search.SortOrder, search.IsCount, "vw_Hishop_PromotionGame", "GameId", stringBuilder.ToString(), selectFields);
		}

		public bool GetPrizesDeliveInfo(PrizesDeliveQuery query, out string GameTitle, out int UserId)
		{
			GameTitle = "";
			UserId = 0;
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand("select top 1 GameTitle, UserId from [vw_Hishop_PrizesDeliveryRecord]   where LogId=" + query.LogId.ToString() + " and  ID=" + query.Id.ToString());
			System.Data.DataTable dataTable = this._database.ExecuteDataSet(sqlStringCommand).Tables[0];
			bool result;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				GameTitle = Convert.ToString(dataTable.Rows[0]["GameTitle"]);
				UserId = Convert.ToInt32(dataTable.Rows[0]["UserId"]);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		public bool DeletePrizesDelivery(int[] ids)
		{
			string query = string.Concat(new string[]
			{
				"delete from Hishop_PromotionGameResultMembersLog where LogId in(select LogId from Hishop_PrizesDeliveryRecord where id in(",
				string.Join<int>(",", ids),
				"));delete from Hishop_PrizesDeliveryRecord where id in(",
				string.Join<int>(",", ids),
				")"
			});
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(query);
			int num = this._database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public System.Data.DataTable GetPrizesDeliveryNum()
		{
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand("Select (SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=0 ) st0,(SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=1 ) st1,(SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=2 ) st2,(SELECT count(LogId) FROM vw_Vshop_ProductPrizeLIst_WithDelievelyInfo where status=3 ) st3");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this._database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public bool UpdatePrizesDelivery(PrizesDeliveQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			System.Data.Common.DbCommand sqlStringCommand;
			if (query.Id == 0)
			{
				stringBuilder.Append("insert into Hishop_PrizesDeliveryRecord");
				stringBuilder.Append("(Receiver,Tel,LogId,ReggionPath,Address,status,RecordType,Pid)VALUES");
				stringBuilder.Append("(@Receiver,@Tel,@LogId,@ReggionPath,@Address,@Status,@RecordType,@Pid)");
				sqlStringCommand = this._database.GetSqlStringCommand(stringBuilder.ToString());
				this._database.AddInParameter(sqlStringCommand, "@Receiver", System.Data.DbType.String, query.Receiver);
				this._database.AddInParameter(sqlStringCommand, "@Tel", System.Data.DbType.String, query.Tel);
				this._database.AddInParameter(sqlStringCommand, "@LogId", System.Data.DbType.Int32, query.LogId);
				this._database.AddInParameter(sqlStringCommand, "@ReggionPath", System.Data.DbType.String, query.ReggionPath);
				this._database.AddInParameter(sqlStringCommand, "@Address", System.Data.DbType.String, query.Address);
				this._database.AddInParameter(sqlStringCommand, "@RecordType", System.Data.DbType.String, query.RecordType);
				if (string.IsNullOrEmpty(query.Pid))
				{
					query.Pid = "0";
				}
				this._database.AddInParameter(sqlStringCommand, "@Pid", System.Data.DbType.String, query.Pid);
				if (query.Address.Length > 5 && query.ReggionPath.Length > 0 && query.Receiver.Length > 1 && query.Tel.Length > 7)
				{
					this._database.AddInParameter(sqlStringCommand, "@Status", System.Data.DbType.Int16, 1);
				}
				else
				{
					this._database.AddInParameter(sqlStringCommand, "@Status", System.Data.DbType.Int16, 0);
				}
			}
			else
			{
				stringBuilder.Append("Update Hishop_PrizesDeliveryRecord ");
				if (query.Status == 0)
				{
					stringBuilder.Append("set status=status");
				}
				else
				{
					stringBuilder.Append("set Status=@Status");
				}
				if (!string.IsNullOrEmpty(query.Receiver))
				{
					stringBuilder.Append(",Receiver=@Receiver");
				}
				if (!string.IsNullOrEmpty(query.Tel))
				{
					stringBuilder.Append(",Tel=@Tel");
				}
				if (!string.IsNullOrEmpty(query.ReggionPath))
				{
					stringBuilder.Append(",ReggionPath=@ReggionPath");
				}
				if (!string.IsNullOrEmpty(query.ExpressName))
				{
					stringBuilder.Append(",ExpressName=@ExpressName");
				}
				if (!string.IsNullOrEmpty(query.CourierNumber))
				{
					stringBuilder.Append(",CourierNumber=@CourierNumber");
				}
				if (!string.IsNullOrEmpty(query.Address))
				{
					stringBuilder.Append(",Address=@Address");
				}
				DateTime now = DateTime.Now;
				if (DateTime.TryParse(query.DeliveryTime, out now))
				{
					stringBuilder.AppendFormat(",DeliveryTime='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
				}
				if (DateTime.TryParse(query.ReceiveTime, out now))
				{
					stringBuilder.AppendFormat(",ReceiveTime='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
				}
				stringBuilder.Append(" where Id=@Id");
				if (query.Status == 3 && query.LogId != "0")
				{
					stringBuilder.Append(";update Hishop_PromotionGameResultMembersLog set IsUsed=1 where LogId=" + query.LogId);
				}
				sqlStringCommand = this._database.GetSqlStringCommand(stringBuilder.ToString());
				this._database.AddInParameter(sqlStringCommand, "@Status", System.Data.DbType.Int16, query.Status);
				this._database.AddInParameter(sqlStringCommand, "@Address", System.Data.DbType.String, query.Address);
				this._database.AddInParameter(sqlStringCommand, "@ExpressName", System.Data.DbType.String, query.ExpressName);
				this._database.AddInParameter(sqlStringCommand, "@CourierNumber", System.Data.DbType.String, query.CourierNumber);
				this._database.AddInParameter(sqlStringCommand, "@ReggionPath", System.Data.DbType.String, query.ReggionPath);
				this._database.AddInParameter(sqlStringCommand, "@Receiver", System.Data.DbType.String, query.Receiver);
				this._database.AddInParameter(sqlStringCommand, "@Tel", System.Data.DbType.String, query.Tel);
				this._database.AddInParameter(sqlStringCommand, "@Id", System.Data.DbType.Int32, query.Id);
			}
			int num = this._database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public DbQueryResult GetPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.PrizeType > -1)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" PrizeType = {0}", query.PrizeType);
			}
			if (!string.IsNullOrEmpty(query.Pid))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" PID = {0}", query.Pid);
			}
			if (query.Id > 0)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" Id = {0}", query.Id);
			}
			if (query.UserId > 0)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" UserId = {0}", query.UserId);
			}
			if (!string.IsNullOrEmpty(query.LogId))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" LogId = {0}", query.LogId);
			}
			if (query.Status >= 0)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat("status = {0}", query.Status);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" ProductName like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			if (!string.IsNullOrEmpty(query.Receiver))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat("( Receiver = '{0}'", DataHelper.CleanSearchString(query.Receiver));
				stringBuilder.AppendFormat(" OR RealName = '{0}' )", DataHelper.CleanSearchString(query.Receiver));
			}
			if (!string.IsNullOrEmpty(query.ReggionId))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" ',' + ReggionPath + ',' like '%,{0},%'", DataHelper.CleanSearchString(query.ReggionId));
			}
			DateTime now = DateTime.Now;
			if (DateTime.TryParse(query.StartDate, out now))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" PlayTime>='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
			}
			if (DateTime.TryParse(query.EndDate, out now))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" PlayTime<='{0}'", now.ToString("yyyy-MM-dd 23:59:59"));
			}
			if (ExtendLimits != "")
			{
				stringBuilder.Append(ExtendLimits);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_PrizesDeliveryRecord", "LogId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, selectFields);
		}

		public DbQueryResult GetAllPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.Status >= 0)
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat("status = {0}", query.Status);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" ProductName like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			if (!string.IsNullOrEmpty(query.ActivityTitle))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" Title like '%{0}%'", DataHelper.CleanSearchString(query.ActivityTitle));
			}
			if (!string.IsNullOrEmpty(query.Receiver))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" Receiver = '{0}'", DataHelper.CleanSearchString(query.Receiver));
			}
			if (!string.IsNullOrEmpty(query.ReggionId))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" ',' + ReggionPath + ',' like '%,{0},%'", DataHelper.CleanSearchString(query.ReggionId));
			}
			DateTime now = DateTime.Now;
			if (DateTime.TryParse(query.StartDate, out now))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" WinTime>='{0}'", now.ToString("yyyy-MM-dd 00:00:00"));
			}
			if (DateTime.TryParse(query.EndDate, out now))
			{
				stringBuilder.Append(" AND ");
				stringBuilder.AppendFormat(" WinTime<='{0}'", now.ToString("yyyy-MM-dd 23:59:59"));
			}
			if (ExtendLimits != "")
			{
				stringBuilder.Append(ExtendLimits);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Vshop_ProductPrizeLIst_WithDelievelyInfo", "WinTime", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, selectFields);
		}

		public List<GameWinningPool> GetWinningPoolList(int gameId)
		{
			string text = "SELECT * FROM Hishop_PromotionWinningPool where IsReceive=0 and GameId=" + gameId;
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(text.ToString());
			List<GameWinningPool> result;
			using (System.Data.IDataReader dataReader = this._database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<GameWinningPool>(dataReader).ToList<GameWinningPool>();
			}
			return result;
		}

		public bool UpdateWinningPoolIsReceive(int winningPoolId)
		{
			string text = "update Hishop_PromotionWinningPool set IsReceive=1 where WinningPoolId=" + winningPoolId;
			System.Data.Common.DbCommand sqlStringCommand = this._database.GetSqlStringCommand(text.ToString());
			return this._database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
