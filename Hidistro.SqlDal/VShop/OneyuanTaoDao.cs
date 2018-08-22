using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.VShop
{
	public class OneyuanTaoDao
	{
		private class MermberCanbuyNumInfo
		{
			public int t
			{
				get;
				set;
			}

			public int b
			{
				get;
				set;
			}
		}

		private Database database;

		public static object LuckNumObj = new object();

		public OneyuanTaoDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public OneyuanTaoInfo GetOneyuanTaoInfoById(string ActivityId)
		{
			string query = "select * from Vshop_OneyuanTao_Detail where ActivityId=@ActivityId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			OneyuanTaoInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<OneyuanTaoInfo>(dataReader);
			}
			return result;
		}

		public string[] ChangeJoinStringArray(string[] ActivityIds)
		{
			for (int i = 0; i < ActivityIds.Length; i++)
			{
				ActivityIds[i] = "'" + ActivityIds[i] + "'";
			}
			return ActivityIds;
		}

		public IList<OneyuanTaoInfo> GetOneyuanTaoInfoByIdList(string[] ActivityIds)
		{
			ActivityIds = this.ChangeJoinStringArray(ActivityIds);
			string query = "select * from Vshop_OneyuanTao_Detail where ActivityId in(" + string.Join(",", ActivityIds) + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<OneyuanTaoInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<OneyuanTaoInfo>(dataReader);
			}
			return result;
		}

		public IList<OneyuanTaoInfo> GetOneyuanTaoInfoNotCalculate()
		{
			string query = "select * from Vshop_OneyuanTao_Detail where IsEnd=0 and HasCalculate=0 and (EndTime<GETDATE() or (ReachType=1 and FinishedNum>=ReachNum )) ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<OneyuanTaoInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<OneyuanTaoInfo>(dataReader);
			}
			return result;
		}

		public bool AddOneyuanTao(OneyuanTaoInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_Detail(ActivityId,IsOn,Title,StartTime,EndTime,HeadImgage,ReachType,\r\n            ActivityDec,ProductId,ProductPrice,ProductImg,ProductTitle,SkuId,PrizeNumber,EachPrice\r\n           ,EachCanBuyNum,FitMember,DefualtGroup,CustomGroup,ReachNum,FinishedNum)VALUES");
			stringBuilder.Append("(@ActivityId,@IsOn,@Title,@StartTime,@EndTime,@HeadImgage,@ReachType,@ActivityDec,@ProductId,");
			stringBuilder.Append("@ProductPrice,@ProductImg,@ProductTitle,@SkuId,@PrizeNumber,@EachPrice,@EachCanBuyNum,");
			stringBuilder.Append("@FitMember,@DefualtGroup,@CustomGroup,@ReachNum,@FinishedNum)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, info.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "IsOn", System.Data.DbType.Boolean, info.IsOn);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, info.Title);
			this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, info.StartTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "HeadImgage", System.Data.DbType.String, info.HeadImgage);
			this.database.AddInParameter(sqlStringCommand, "ActivityDec", System.Data.DbType.String, info.ActivityDec);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, info.ProductId);
			this.database.AddInParameter(sqlStringCommand, "ProductPrice", System.Data.DbType.Decimal, info.ProductPrice);
			this.database.AddInParameter(sqlStringCommand, "ProductImg", System.Data.DbType.String, info.ProductImg);
			this.database.AddInParameter(sqlStringCommand, "ProductTitle", System.Data.DbType.String, info.ProductTitle);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, info.SkuId);
			this.database.AddInParameter(sqlStringCommand, "PrizeNumber", System.Data.DbType.Int32, info.PrizeNumber);
			this.database.AddInParameter(sqlStringCommand, "EachCanBuyNum", System.Data.DbType.Int32, info.EachCanBuyNum);
			this.database.AddInParameter(sqlStringCommand, "FitMember", System.Data.DbType.String, info.FitMember);
			this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, info.DefualtGroup);
			this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, info.CustomGroup);
			this.database.AddInParameter(sqlStringCommand, "ReachNum", System.Data.DbType.Int32, info.ReachNum);
			this.database.AddInParameter(sqlStringCommand, "FinishedNum", System.Data.DbType.Int32, info.FinishedNum);
			this.database.AddInParameter(sqlStringCommand, "EachPrice", System.Data.DbType.Decimal, info.EachPrice);
			this.database.AddInParameter(sqlStringCommand, "ReachType", System.Data.DbType.Int32, info.ReachType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int SetIsAllRefund(List<string> ActivivyIds)
		{
			string[] array = ActivivyIds.ToArray<string>();
			array = this.ChangeJoinStringArray(array);
			string text = "update Vshop_OneyuanTao_Detail set IsAllRefund=1 where IsAllRefund=0 and HasCalculate=1 ";
			text += " and IsSuccess=0 and ActivityId in( select ActivityId  from  Vshop_OneyuanTao_Detail where ";
			text = text + " ActivityId in(" + string.Join(",", array) + ")  ";
			text += " and  ActivityId not in(select ActivityId from Vshop_OneyuanTao_ParticipantMember  ";
			text = text + " where IsPay=1 and IsRefund=0 and ActivityId in(" + string.Join(",", array) + ") ) )";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool UpdateOneyuanTao(OneyuanTaoInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder("Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Title=@Title,StartTime=@StartTime,EndTime=@EndTime,\r\n           HeadImgage=@HeadImgage,ReachType=@ReachType,ActivityDec=@ActivityDec,ProductId=@ProductId,ProductPrice=@ProductPrice,\r\n           ProductImg=@ProductImg,ProductTitle=@ProductTitle,SkuId=@SkuId,PrizeNumber=@PrizeNumber,EachPrice=@EachPrice\r\n           ,EachCanBuyNum=@EachCanBuyNum,FitMember=@FitMember,DefualtGroup=@DefualtGroup,CustomGroup=@CustomGroup,ReachNum=@ReachNum,\r\n           FinishedNum=@FinishedNum where ActivityId=@ActivityId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "IsOn", System.Data.DbType.Boolean, info.IsOn);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, info.Title);
			this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, info.StartTime);
			this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, info.EndTime);
			this.database.AddInParameter(sqlStringCommand, "HeadImgage", System.Data.DbType.String, info.HeadImgage);
			this.database.AddInParameter(sqlStringCommand, "ActivityDec", System.Data.DbType.String, info.ActivityDec);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, info.ProductId);
			this.database.AddInParameter(sqlStringCommand, "ProductPrice", System.Data.DbType.Decimal, info.ProductPrice);
			this.database.AddInParameter(sqlStringCommand, "ProductImg", System.Data.DbType.String, info.ProductImg);
			this.database.AddInParameter(sqlStringCommand, "ProductTitle", System.Data.DbType.String, info.ProductTitle);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, info.SkuId);
			this.database.AddInParameter(sqlStringCommand, "PrizeNumber", System.Data.DbType.Int32, info.PrizeNumber);
			this.database.AddInParameter(sqlStringCommand, "EachCanBuyNum", System.Data.DbType.Int32, info.EachCanBuyNum);
			this.database.AddInParameter(sqlStringCommand, "FitMember", System.Data.DbType.String, info.FitMember);
			this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, info.DefualtGroup);
			this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, info.CustomGroup);
			this.database.AddInParameter(sqlStringCommand, "ReachNum", System.Data.DbType.Int32, info.ReachNum);
			this.database.AddInParameter(sqlStringCommand, "FinishedNum", System.Data.DbType.Int32, info.FinishedNum);
			this.database.AddInParameter(sqlStringCommand, "EachPrice", System.Data.DbType.Decimal, info.EachPrice);
			this.database.AddInParameter(sqlStringCommand, "ReachType", System.Data.DbType.Int32, info.ReachType);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, info.ActivityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOneyuanTaoIsOn(string ActivityId, bool IsOn)
		{
			bool flag = false;
			bool flag2 = false;
			string text = "Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,HasCalculate=@HasCalculate,Isend=@Isend where ActivityId=@ActivityId";
			if (!IsOn)
			{
				flag = true;
				flag2 = true;
			}
			else
			{
				text = "if  exists (select ActivityId from Vshop_OneyuanTao_Detail where ActivityId=@ActivityId and StartTime>GETDATE())\r\n                       Update  Vshop_OneyuanTao_Detail set StartTime=GETDATE(),IsOn=@IsOn,Isend=@Isend where ActivityId=@ActivityId\r\n                     else\r\n                       Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Isend=@Isend where ActivityId=@ActivityId ";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text.ToString());
			this.database.AddInParameter(sqlStringCommand, "IsOn", System.Data.DbType.Boolean, IsOn);
			this.database.AddInParameter(sqlStringCommand, "HasCalculate", System.Data.DbType.Boolean, flag2);
			this.database.AddInParameter(sqlStringCommand, "Isend", System.Data.DbType.Boolean, flag);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int BatchSetOneyuanTaoIsOn(string[] ActivityIds, bool IsOn)
		{
			bool flag = false;
			ActivityIds = this.ChangeJoinStringArray(ActivityIds);
			string str = string.Join(",", ActivityIds);
			string text = "Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Isend=@Isend where Isend=0 and ActivityId in(" + str + ")";
			if (!IsOn)
			{
				flag = true;
			}
			else
			{
				text = "Update  Vshop_OneyuanTao_Detail set StartTime=GETDATE() where  Isend=0 \r\n                       and StartTime>GETDATE() and ActivityId in(" + str + ")";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text.ToString());
				this.database.ExecuteNonQuery(sqlStringCommand);
				text = "Update  Vshop_OneyuanTao_Detail set IsOn=@IsOn,Isend=@Isend where   Isend=0  and  ActivityId in(" + str + ")";
			}
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(text.ToString());
			this.database.AddInParameter(sqlStringCommand2, "IsOn", System.Data.DbType.Boolean, IsOn);
			this.database.AddInParameter(sqlStringCommand2, "Isend", System.Data.DbType.Boolean, flag);
			return this.database.ExecuteNonQuery(sqlStringCommand2);
		}

		public bool SetOneyuanTaoFinishedNum(string ActivityId, int Addnum = 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (Addnum > 0)
			{
				stringBuilder.Append("Update  Vshop_OneyuanTao_Detail set FinishedNum=FinishedNum+@Addnum where ActivityId=@ActivityId");
			}
			else
			{
				stringBuilder.Append("declare @fnum int;select @fnum =isnull(SUM(BuyNum),0) from Vshop_OneyuanTao_ParticipantMember where ActivityId=@ActivityId and IsPay=1;\r\n                           Update  Vshop_OneyuanTao_Detail set FinishedNum=@fnum where ActivityId=@ActivityId;");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Addnum", System.Data.DbType.Int32, Addnum);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOneyuanTaoPrizeTime(string ActivityId, DateTime PrizeTime, string PrizeInfoJson)
		{
			StringBuilder stringBuilder = new StringBuilder("Update  Vshop_OneyuanTao_Detail set PrizeTime=@PrizeTime,PrizeCountInfo=@PrizeInfoJson,IsEnd=1,IsSuccess=1,HasCalculate=1,IsOn=0 where ActivityId=@ActivityId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "PrizeTime", System.Data.DbType.DateTime, PrizeTime);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "PrizeInfoJson", System.Data.DbType.String, PrizeInfoJson);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetOneyuanTaoHasCalculate(string ActivityId)
		{
			StringBuilder stringBuilder = new StringBuilder("Update  Vshop_OneyuanTao_Detail set IsEnd=1,HasCalculate=1,IsOn=0 where ActivityId=@ActivityId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetErrPrizeCountInfo(string ActivityId, string PrizeCountInfo)
		{
			StringBuilder stringBuilder = new StringBuilder("Update  Vshop_OneyuanTao_Detail set IsEnd=1,HasCalculate=1,IsOn=0,PrizeCountInfo=@PrizeCountInfo where ActivityId=@ActivityId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "PrizeCountInfo", System.Data.DbType.String, PrizeCountInfo);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string getPrizeCountInfo(string ActivityId)
		{
			string query = "select PrizeCountInfo from Vshop_OneyuanTao_Detail Where ActivityId=@ActivityId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result = "";
			if (obj != null && obj != DBNull.Value)
			{
				result = obj.ToString();
			}
			return result;
		}

		public bool DeleteOneyuanTao(string ActivityId)
		{
			StringBuilder stringBuilder = new StringBuilder("delete from Vshop_OneyuanTao_Detail where FinishedNum=0 and ActivityId=@ActivityId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int BatchDeleteOneyuanTao(string[] ActivityIds)
		{
			ActivityIds = this.ChangeJoinStringArray(ActivityIds);
			string query = "delete from Vshop_OneyuanTao_Detail where FinishedNum=0 and ActivityId in(" + string.Join(",", ActivityIds) + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetOneyuanTaoTotalNum(out int hasStart, out int waitStart, out int hasEnd)
		{
			int result = 0;
			string query = "select\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail)  total,\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail\r\n  where IsOn=1  and  StartTime<GETDATE() and EndTime>=GETDATE() and IsEnd=0) as hasStart,\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail\r\n  where  IsOn=1 and IsEnd=0 and StartTime>GETDATE()) as waitStart,\r\n  (select COUNT(ActivityId) from Vshop_OneyuanTao_Detail\r\n  where EndTime<GETDATE() or IsEnd=1) as hasEnd";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				hasStart = (int)dataTable.Rows[0]["HasStart"];
				waitStart = (int)dataTable.Rows[0]["waitStart"];
				hasEnd = (int)dataTable.Rows[0]["hasEnd"];
				result = (int)dataTable.Rows[0]["total"];
			}
			else
			{
				hasStart = 0;
				waitStart = 0;
				hasEnd = 0;
			}
			return result;
		}

		public int GetRefundTotalNum(out int Refundnum)
		{
			int result = 0;
			string query = " select\r\n (select count(pid) from vw_Vshop_OneyuanPartInList where  IsPay=1 and HasCalculate=1 and IsSuccess=0 and IsRefund=0) as r,\r\n  (select count(pid) from vw_Vshop_OneyuanPartInList where IsRefund=1) as f";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			System.Data.DataTable dataTable;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = (int)dataTable.Rows[0]["r"];
				Refundnum = (int)dataTable.Rows[0]["f"];
			}
			else
			{
				Refundnum = 0;
			}
			return result;
		}

		public DbQueryResult GetOneyuanTao(OneyuanTaoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.title))
			{
				stringBuilder.AppendFormat(" and  Title like '%{0}%' ", query.title);
			}
			if (query.ReachType > 0)
			{
				stringBuilder.AppendFormat(" and ReachType={0} ", query.ReachType);
			}
			if (query.state > 0)
			{
				string arg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				if (query.state == 1)
				{
					stringBuilder.AppendFormat(" and IsOn=1  and  StartTime<'{0}' and EndTime>='{0}' and IsEnd=0 ", arg);
				}
				else if (query.state == 2)
				{
					stringBuilder.AppendFormat(" and IsOn=1 and IsEnd=0 and StartTime>'{0}'", arg);
				}
				else if (query.state == 3)
				{
					stringBuilder.AppendFormat(" and (EndTime<'{0}' or IsEnd=1) ", arg);
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Vshop_OneyuanTao_Detail", "ActivityId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public bool AddParticipant(OneyuanTaoParticipantInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_ParticipantMember(Pid,UserId,BuyTime,BuyNum,IsPay,ActivityId,TotalPrice,SkuId,SkuIdStr,ProductPrice)VALUES");
			stringBuilder.Append("(@Pid,@UserId,@BuyTime,@BuyNum,@IsPay,@ActivityId,@TotalPrice,@SkuId,@SkuIdStr,@ProductPrice)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, info.Pid);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, info.UserId);
			this.database.AddInParameter(sqlStringCommand, "BuyTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "BuyNum", System.Data.DbType.Int32, info.BuyNum);
			this.database.AddInParameter(sqlStringCommand, "IsPay", System.Data.DbType.Boolean, false);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, info.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "TotalPrice", System.Data.DbType.Decimal, info.TotalPrice);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, info.SkuId);
			this.database.AddInParameter(sqlStringCommand, "SkuIdStr", System.Data.DbType.String, info.SkuIdStr);
			this.database.AddInParameter(sqlStringCommand, "ProductPrice", System.Data.DbType.Decimal, info.ProductPrice);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddInitParticipantInfo(int creatNum = 60)
		{
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_ParticipantMember(Pid,UserId,BuyTime,BuyNum,IsPay,ActivityId,TotalPrice,SkuId,SkuIdStr,ProductPrice)VALUES");
			stringBuilder.Append("(@Pid,@UserId,@BuyTime,@BuyNum,@IsPay,@ActivityId,@TotalPrice,@SkuId,@SkuIdStr,@ProductPrice)");
			Random random = new Random();
			List<DateTime> list = new List<DateTime>();
			for (int i = 0; i < creatNum; i++)
			{
				DateTime dateTime = DateTime.Now.AddDays(-3.0).AddHours((double)random.Next(72)).AddMilliseconds((double)random.Next(999));
				while (list.Contains(dateTime))
				{
					dateTime = dateTime.AddMilliseconds((double)random.Next(999));
				}
				list.Add(dateTime);
			}
			list.Sort();
			int num = 0;
			foreach (DateTime dateTime in list)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				//DateTime dateTime;
				this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, "B" + dateTime.ToString("yyMMddHHmmssfff"));
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, "0");
				this.database.AddInParameter(sqlStringCommand, "BuyTime", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "BuyNum", System.Data.DbType.Int32, 1);
				this.database.AddInParameter(sqlStringCommand, "IsPay", System.Data.DbType.Boolean, true);
				this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, "A0");
				this.database.AddInParameter(sqlStringCommand, "TotalPrice", System.Data.DbType.Decimal, 1);
				this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, "");
				this.database.AddInParameter(sqlStringCommand, "SkuIdStr", System.Data.DbType.String, "");
				this.database.AddInParameter(sqlStringCommand, "ProductPrice", System.Data.DbType.Decimal, 1);
				this.database.ExecuteNonQuery(sqlStringCommand);
				num++;
			}
			return true;
		}

		public bool SetPayinfo(OneyuanTaoParticipantInfo info)
		{
			bool result;
			if (this.CreatLuckNum(info.Pid, info.UserId, info.ActivityId, info.BuyNum))
			{
				string text = "Update Vshop_OneyuanTao_ParticipantMember set IsPay=1, PayTime=@PayTime,PayWay=@PayWay,PayNum=@PayNum,Remark=@Remark";
				text += " where Pid=@Pid";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "PayWay", System.Data.DbType.String, info.PayWay);
				this.database.AddInParameter(sqlStringCommand, "PayTime", System.Data.DbType.DateTime, DateTime.Now);
				this.database.AddInParameter(sqlStringCommand, "PayNum", System.Data.DbType.String, info.PayNum);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, info.Remark);
				this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, info.Pid);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			else
			{
				Globals.Debuglog("支付回调失败了！", "_Debuglog.txt");
				result = false;
			}
			return result;
		}

		public bool Setout_refund_no(string Pid, string out_refund_no)
		{
			string text = "Update Vshop_OneyuanTao_ParticipantMember set out_refund_no=@out_refund_no ";
			text += " where Pid=@Pid";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "out_refund_no", System.Data.DbType.String, out_refund_no);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, Pid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool CreatLuckNum(string Pid, int UserId, string ActivityId, int BuyNum)
		{
			List<LuckInfo> list = new List<LuckInfo>();
			bool result = false;
			lock (OneyuanTaoDao.LuckNumObj)
			{
				int maxLuckNum = this.GetMaxLuckNum(ActivityId);
				for (int i = 1; i <= BuyNum; i++)
				{
					LuckInfo item = new LuckInfo
					{
						UserId = UserId,
						ActivityId = ActivityId,
						Pid = Pid,
						PrizeNum = (maxLuckNum + i).ToString()
					};
					list.Add(item);
				}
				if (list.Count > 0 && this.AddLuckInfo(list))
				{
					result = true;
				}
			}
			return result;
		}

		public int GetMaxLuckNum(string ActivityId)
		{
			string query = string.Format("select MAX(PrizeNum) from Vshop_OneyuanTao_WinningRecord where ActivityId='{0}'", ActivityId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int num = 0;
			if (obj != null && obj != DBNull.Value)
			{
				int.TryParse(obj.ToString().Trim(), out num);
			}
			if (num == 0)
			{
				num = 10000000;
			}
			return num;
		}

		public bool SetRefundinfo(OneyuanTaoParticipantInfo info)
		{
			string text = "Update Vshop_OneyuanTao_ParticipantMember set IsRefund=1,RefundErr=0,RefundTime=@RefundTime,RefundNum=@RefundNum,Remark=@Remark";
			text += " where Pid=@Pid";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "RefundTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "RefundNum", System.Data.DbType.String, info.RefundNum);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, info.Remark);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, info.Pid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetRefundinfoErr(OneyuanTaoParticipantInfo info)
		{
			string text = "Update Vshop_OneyuanTao_ParticipantMember set RefundErr=1,Remark=@Remark ";
			text += " where Pid=@Pid ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, info.Remark);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, info.Pid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int MermberCanbuyNum(string ActivityId, int userid)
		{
			int result = 0;
			string query = " select\r\n  (select EachCanBuyNum from Vshop_OneyuanTao_Detail where ActivityId=@ActivityId) as t,\r\n  (select isnull(SUM(BuyNum),0) from Vshop_OneyuanTao_ParticipantMember where ActivityId=@ActivityId and UserId=@UserId) as b";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			OneyuanTaoDao.MermberCanbuyNumInfo mermberCanbuyNumInfo;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				mermberCanbuyNumInfo = ReaderConvert.ReaderToModel<OneyuanTaoDao.MermberCanbuyNumInfo>(dataReader);
			}
			if (mermberCanbuyNumInfo != null)
			{
				result = mermberCanbuyNumInfo.t - mermberCanbuyNumInfo.b;
			}
			return result;
		}

		public OneyuanTaoParticipantInfo GetAddParticipant(int UserId, string Pid = "", string payNum = "")
		{
			string query = "";
			if (UserId > 0)
			{
				query = "select top 1 * from Vshop_OneyuanTao_ParticipantMember where UserId=@UserId";
			}
			else if (Pid != "")
			{
				query = "select top 1 * from Vshop_OneyuanTao_ParticipantMember where Pid=@Pid";
			}
			else if (payNum != "")
			{
				query = "select top 1 * from Vshop_OneyuanTao_ParticipantMember where PayNum=@PayNum";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, Pid);
			this.database.AddInParameter(sqlStringCommand, "PayNum", System.Data.DbType.String, payNum);
			OneyuanTaoParticipantInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<OneyuanTaoParticipantInfo>(dataReader);
			}
			return result;
		}

		public bool IsExistAlipayRefundNUm(string batch_no)
		{
			string query = "select top 1 RefundNum from Vshop_OneyuanTao_ParticipantMember where RefundNum=@RefundNum";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "RefundNum", System.Data.DbType.String, batch_no);
			bool result = false;
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null && obj != DBNull.Value)
			{
				result = true;
			}
			return result;
		}

		public IList<OneyuanTaoParticipantInfo> GetParticipantList(string ActivityId, int[] UserIds = null, string[] PIds = null)
		{
			string text = "";
			if (!string.IsNullOrEmpty(ActivityId))
			{
				text = string.Format("select * from Vshop_OneyuanTao_ParticipantMember where ActivityId='{0}' ", ActivityId);
			}
			else if (UserIds != null)
			{
				text = "select * from Vshop_OneyuanTao_ParticipantMember where UserIds in(" + string.Join<int>(",", UserIds) + ")";
			}
			else if (PIds != null)
			{
				text = "select * from Vshop_OneyuanTao_ParticipantMember where Pid in(" + string.Join(",", PIds) + ")";
			}
			IList<OneyuanTaoParticipantInfo> result;
			if (text == "")
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				IList<OneyuanTaoParticipantInfo> list = null;
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					list = ReaderConvert.ReaderToList<OneyuanTaoParticipantInfo>(dataReader);
				}
				result = list;
			}
			return result;
		}

		public IList<OneyuanTaoParticipantInfo> GetRefundParticipantList(string[] PIds)
		{
			PIds = this.ChangeJoinStringArray(PIds);
			string query = "select IsPay,IsRefund,TotalPrice,Pid,PayWay,IsWin,PayNum,out_refund_no from Vshop_OneyuanTao_ParticipantMember where Pid in(" + string.Join(",", PIds) + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			IList<OneyuanTaoParticipantInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<OneyuanTaoParticipantInfo>(dataReader);
			}
			return result;
		}

		public List<string> GetParticipantPids(string ActivityId, bool IsPay = true, bool IsRefund = false, string PayWay = "alipay")
		{
			string query = "select top 1000 Pid from Vshop_OneyuanTao_ParticipantMember where  ActivityId=@ActivityId and IsPay=@IsPay and IsRefund=@IsRefund and PayWay=@PayWay";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "PayWay", System.Data.DbType.String, PayWay);
			this.database.AddInParameter(sqlStringCommand, "IsPay", System.Data.DbType.Boolean, IsPay);
			this.database.AddInParameter(sqlStringCommand, "IsRefund", System.Data.DbType.Boolean, IsRefund);
			List<string> list = new List<string>();
			System.Data.DataTable dataTable = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					list.Add(dataRow["Pid"].ToString());
				}
			}
			return list;
		}

		public IList<Top50ParticipantInfo> GetTop50ParticipantList(DateTime PrizeTime, int topnum = 50)
		{
			string query = "SELECT TOP " + topnum + " Pid,p.UserId,BuyTime,isnull(UserName,'SYSUSER') as UserName FROM Vshop_OneyuanTao_ParticipantMember p\r\n                         Left join aspnet_Members m on p.UserId=m.UserId where BuyTime<@PrizeTime and IsPay=1 order by Pid desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "PrizeTime", System.Data.DbType.DateTime, PrizeTime);
			IList<Top50ParticipantInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<Top50ParticipantInfo>(dataReader);
			}
			return result;
		}

		public Top50ParticipantInfo GetNextParticipant(DateTime PrizeTime, string TopPid)
		{
			string query = "SELECT TOP 1 Pid,p.UserId,BuyTime,isnull(UserName,'SYSUSER') as UserName FROM Vshop_OneyuanTao_ParticipantMember p\r\n                         Left join aspnet_Members m on p.UserId=m.UserId where BuyTime<@PrizeTime and IsPay=1 and Pid<@TopPid order by Pid desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "PrizeTime", System.Data.DbType.DateTime, PrizeTime);
			this.database.AddInParameter(sqlStringCommand, "TopPid", System.Data.DbType.String, TopPid);
			Top50ParticipantInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<Top50ParticipantInfo>(dataReader);
			}
			return result;
		}

		public int GetParticipantCount()
		{
			string query = "SELECT count(Pid)FROM Vshop_OneyuanTao_ParticipantMember  where IsPay=1";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = (int)obj;
			}
			return result;
		}

		public string GetSkuStrBySkuId(string Skuid, bool ShowAttribute = true)
		{
			string query = "SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId\r\nleft join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId\r\nAND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)";
			string text = string.Empty;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, Skuid);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (ShowAttribute && dataReader["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["AttributeName"]) && dataReader["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["ValueStr"]))
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							dataReader["AttributeName"],
							"：",
							dataReader["ValueStr"],
							"/"
						});
					}
					else
					{
						text = text + dataReader["ValueStr"] + "/";
					}
				}
			}
			return text;
		}

		public DbQueryResult GetOneyuanPartInDataTable(OneyuanTaoPartInQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" and  UserName like '%{0}%' ", query.UserName);
			}
			if (!string.IsNullOrEmpty(query.Atitle))
			{
				stringBuilder.AppendFormat(" and  title like '%{0}%' ", query.Atitle);
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				stringBuilder.AppendFormat(" and  CellPhone like '%{0}%' ", query.CellPhone);
			}
			if (!string.IsNullOrEmpty(query.PayWay))
			{
				stringBuilder.AppendFormat(" and  PayWay = '{0}' ", query.PayWay);
			}
			if (!string.IsNullOrEmpty(query.ActivityId))
			{
				stringBuilder.AppendFormat(" and ActivityId='{0}' ", query.ActivityId);
			}
			if (!string.IsNullOrEmpty(query.Pid))
			{
				stringBuilder.AppendFormat(" and Pid='{0}' ", query.Pid);
			}
			if (query.UserId > 0)
			{
				stringBuilder.AppendFormat(" and UserId={0} ", query.UserId);
			}
			if (query.IsPay > -1)
			{
				stringBuilder.AppendFormat(" and IsPay={0} ", query.IsPay);
			}
			if (query.state > 0)
			{
				if (query.state == 1)
				{
					stringBuilder.Append(" and (IsEnd=0 and EndTime>GETDATE()) ");
				}
				else if (query.state == 2)
				{
					stringBuilder.Append(" and (IsEnd=1 or  EndTime<GETDATE()) ");
				}
				else if (query.state == 3)
				{
					stringBuilder.Append(" and IsWin=1");
				}
				else if (query.state == 4)
				{
					stringBuilder.Append(" and IsRefund=1");
				}
				else if (query.state == 5)
				{
					stringBuilder.Append(" and (HasCalculate=1 and IsSuccess=0 and IsRefund=0)");
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Vshop_OneyuanPartInList", "Pid", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}

		public bool AddLuckInfo(LuckInfo info)
		{
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO Vshop_OneyuanTao_WinningRecord(UserId,ActivityId,Pid,PrizeNum,IsWin)VALUES");
			stringBuilder.Append("(@UserId,@ActivityId,@Pid,@PrizeNum,@IsWin)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, info.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Boolean, info.UserId);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, info.Pid);
			this.database.AddInParameter(sqlStringCommand, "PrizeNum", System.Data.DbType.String, info.PrizeNum);
			this.database.AddInParameter(sqlStringCommand, "IsWin", System.Data.DbType.Boolean, false);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool AddLuckInfo(List<LuckInfo> infoList)
		{
			bool result;
			if (infoList.Count == 0)
			{
				result = false;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LuckInfo current in infoList)
				{
					stringBuilder.AppendLine("INSERT INTO Vshop_OneyuanTao_WinningRecord(UserId,ActivityId,Pid,PrizeNum,IsWin)");
					stringBuilder.AppendLine(string.Concat(new object[]
					{
						"select  ",
						current.UserId,
						",'",
						current.ActivityId,
						"','",
						current.Pid,
						"','",
						current.PrizeNum,
						"',0 ;"
					}));
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}

		public bool setWin(string PrizeNum, string AcitivityId)
		{
			string text = string.Format("update Vshop_OneyuanTao_WinningRecord set IsWin=1 where PrizeNum='{0}' and ActivityId='{1}'", PrizeNum, AcitivityId);
			string str = string.Format("update Vshop_OneyuanTao_ParticipantMember set IsWin=1 where Pid in( select pid  from  Vshop_OneyuanTao_WinningRecord where PrizeNum='{0}' and ActivityId='{1}')", PrizeNum, AcitivityId);
			text = text + "; " + str;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelParticipantMember(string ActivityId, bool delAll = true)
		{
			string query = "delete from Vshop_OneyuanTao_ParticipantMember where IsPay=0 and ActivityId=@ActivityId";
			if (!delAll)
			{
				if (string.IsNullOrEmpty(ActivityId))
				{
					query = "delete from Vshop_OneyuanTao_ParticipantMember where IsPay=0 and BuyTime<@BuyTime and ActivityId<>'A0' ";
				}
				else
				{
					query = "delete from Vshop_OneyuanTao_ParticipantMember where IsPay=0 and BuyTime<@BuyTime and ActivityId=@ActivityId";
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "BuyTime", System.Data.DbType.String, DateTime.Now.AddMinutes(-30.0));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public IList<LuckInfo> getWinnerLuckInfoList(string ActivityId, string Pid = "")
		{
			string query;
			if (string.IsNullOrEmpty(Pid))
			{
				query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum,1 as IsWin  from vw_Vshop_OneyuanWinnerList r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.ActivityId=@ActivityId order by Pid Desc";
			}
			else
			{
				query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum,1 as IsWin  from vw_Vshop_OneyuanWinnerList r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.Pid=@Pid and r.ActivityId=@ActivityId order by Pid Desc";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, Pid);
			IList<LuckInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<LuckInfo>(dataReader);
			}
			return result;
		}

		public IList<LuckInfo> getLuckInfoList(bool IsWin, string ActivityId)
		{
			string query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum from Vshop_OneyuanTao_WinningRecord r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.IsWin=@IsWin and r.ActivityId=@ActivityId order by Pid Desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "IsWin", System.Data.DbType.Boolean, IsWin);
			IList<LuckInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<LuckInfo>(dataReader);
			}
			return result;
		}

		public IList<LuckInfo> getLuckInfoListByAId(string ActivityId, int UserId)
		{
			string query = "select r.*,m.UserHead,m.UserName,BuyTime,BuyNum from Vshop_OneyuanTao_WinningRecord r,aspnet_Members m,Vshop_OneyuanTao_ParticipantMember p where r.UserId=m.UserId and r.Pid=p.pid  and r.ActivityId=@ActivityId and r.UserId=@UserId order by Pid Desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.String, ActivityId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			IList<LuckInfo> result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<LuckInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable PrizesDeliveryRecord(string Pid)
		{
			string query = "select * from Hishop_PrizesDeliveryRecord where Pid=@Pid";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "Pid", System.Data.DbType.String, Pid);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
	}
}
