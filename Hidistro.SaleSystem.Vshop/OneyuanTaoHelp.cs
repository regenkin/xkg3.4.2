using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace Hidistro.SaleSystem.Vshop
{
	public static class OneyuanTaoHelp
	{
		private static string provOrderid = "";

		private static object Orderidlock = new object();

		private static object Calculate = new object();

		public static string getStateStr(OneyuanTaoInfo info)
		{
			return OneyuanTaoHelp.getOneTaoState(info).ToString();
		}

		public static OneyuanTaoInfo DataRowToOneyuanTaoInfo(System.Data.DataRow dr)
		{
			return ReaderConvert.DataRowToModel<OneyuanTaoInfo>(dr);
		}

		public static OneTaoPrizeState getPrizeState(OneyuanTaoInfo info)
		{
			OneTaoPrizeState result;
			if (info.IsSuccess)
			{
				result = OneTaoPrizeState.成功开奖;
			}
			else if (info.HasCalculate)
			{
				if (info.FinishedNum == 0)
				{
					result = OneTaoPrizeState.已关闭;
				}
				else if (info.IsAllRefund)
				{
					result = OneTaoPrizeState.已退款;
				}
				else if (!info.IsRefund || !info.IsAllRefund)
				{
					result = OneTaoPrizeState.待退款;
				}
				else
				{
					result = OneTaoPrizeState.NONE;
				}
			}
			else
			{
				result = OneTaoPrizeState.未开奖;
			}
			return result;
		}

		public static OneTaoState getOneTaoState(OneyuanTaoInfo info)
		{
			OneTaoState result;
			try
			{
				bool isOn = info.IsOn;
				bool isEnd = info.IsEnd;
				DateTime startTime = info.StartTime;
				DateTime endTime = info.EndTime;
				DateTime now = DateTime.Now;
				OneTaoState oneTaoState;
				if (info.IsSuccess)
				{
					oneTaoState = OneTaoState.已开奖;
				}
				else if (info.IsAllRefund)
				{
					oneTaoState = OneTaoState.退款完成;
				}
				else if (info.HasCalculate && info.FinishedNum > 0)
				{
					oneTaoState = OneTaoState.开奖失败;
				}
				else if (isOn && !isEnd && startTime < now && endTime > now)
				{
					oneTaoState = OneTaoState.进行中;
				}
				else if (isOn && !isEnd && startTime > now)
				{
					oneTaoState = OneTaoState.未开始;
				}
				else
				{
					oneTaoState = OneTaoState.已结束;
				}
				result = oneTaoState;
			}
			catch (Exception var_6_D2)
			{
				result = OneTaoState.NONE;
			}
			return result;
		}

		public static string getReachTypeStr(int ReachTyp)
		{
			string result = "";
			if (ReachTyp == 1)
			{
				result = "满份开奖";
			}
			else if (ReachTyp == 2)
			{
				result = "到期开奖";
			}
			else if (ReachTyp == 3)
			{
				result = "到期满份开奖";
			}
			return result;
		}

		public static string GetOrderNumber(bool isActivity = true)
		{
			string result;
			lock (OneyuanTaoHelp.Orderidlock)
			{
				string text = DateTime.Now.ToString("yyMMddHHmmssfff");
				if (text == OneyuanTaoHelp.provOrderid)
				{
					Thread.Sleep(1);
					text = DateTime.Now.ToString("yyMMddHHmmssfff");
				}
				OneyuanTaoHelp.provOrderid = text;
				if (isActivity)
				{
					text = "A" + text;
				}
				else
				{
					text = "B" + text;
				}
				result = text;
			}
			return result;
		}

		public static DbQueryResult GetOneyuanTao(OneyuanTaoQuery query)
		{
			return new OneyuanTaoDao().GetOneyuanTao(query);
		}

		public static IList<OneyuanTaoInfo> GetOneyuanTaoInfoByIdList(string[] ActivityIds)
		{
			return new OneyuanTaoDao().GetOneyuanTaoInfoByIdList(ActivityIds);
		}

		public static bool Setout_refund_no(string pid, string Refundoutid)
		{
			return new OneyuanTaoDao().Setout_refund_no(pid, Refundoutid);
		}

		public static OneyuanTaoInfo GetOneyuanTaoInfoById(string ActivityId)
		{
			return new OneyuanTaoDao().GetOneyuanTaoInfoById(ActivityId);
		}

		public static bool AddOneyuanTao(OneyuanTaoInfo info)
		{
			info.ActivityId = OneyuanTaoHelp.GetOrderNumber(true);
			return new OneyuanTaoDao().AddOneyuanTao(info);
		}

		public static int SetIsAllRefund(List<string> ActivivyIds)
		{
			return new OneyuanTaoDao().SetIsAllRefund(ActivivyIds);
		}

		public static bool UpdateOneyuanTao(OneyuanTaoInfo info)
		{
			return new OneyuanTaoDao().UpdateOneyuanTao(info);
		}

		public static bool SetOneyuanTaoIsOn(string ActivityId, bool IsOn)
		{
			return new OneyuanTaoDao().SetOneyuanTaoIsOn(ActivityId, IsOn);
		}

		public static int BatchSetOneyuanTaoIsOn(string[] ActivityIds, bool IsOn)
		{
			return new OneyuanTaoDao().BatchSetOneyuanTaoIsOn(ActivityIds, IsOn);
		}

		public static bool SetOneyuanTaoFinishedNum(string ActivityId, int Addnum = 0)
		{
			return new OneyuanTaoDao().SetOneyuanTaoFinishedNum(ActivityId, Addnum);
		}

		public static bool SetOneyuanTaoPrizeTime(string ActivityId, DateTime PrizeTime, string PrizeInfoJson)
		{
			return new OneyuanTaoDao().SetOneyuanTaoPrizeTime(ActivityId, PrizeTime, PrizeInfoJson);
		}

		public static bool SetOneyuanTaoHasCalculate(string ActivityId)
		{
			return new OneyuanTaoDao().SetOneyuanTaoHasCalculate(ActivityId);
		}

		public static string getPrizeCountInfo(string ActivityId)
		{
			return new OneyuanTaoDao().getPrizeCountInfo(ActivityId);
		}

		public static bool DeleteOneyuanTao(string ActivityId)
		{
			return new OneyuanTaoDao().DeleteOneyuanTao(ActivityId);
		}

		public static int BatchDeleteOneyuanTao(string[] ActivityIds)
		{
			return new OneyuanTaoDao().BatchDeleteOneyuanTao(ActivityIds);
		}

		public static bool AddParticipant(OneyuanTaoParticipantInfo info)
		{
			if (string.IsNullOrEmpty(info.Pid))
			{
				info.Pid = OneyuanTaoHelp.GetOrderNumber(false);
			}
			return new OneyuanTaoDao().AddParticipant(info);
		}

		public static bool AddInitParticipantInfo(int num = 50)
		{
			return new OneyuanTaoDao().AddInitParticipantInfo(num);
		}

		public static bool SetPayinfo(OneyuanTaoParticipantInfo info)
		{
			return new OneyuanTaoDao().SetPayinfo(info);
		}

		public static bool SetRefundinfo(OneyuanTaoParticipantInfo info)
		{
			return new OneyuanTaoDao().SetRefundinfo(info);
		}

		public static bool SetRefundinfoErr(OneyuanTaoParticipantInfo info)
		{
			return new OneyuanTaoDao().SetRefundinfoErr(info);
		}

		public static int MermberCanbuyNum(string Aid, int userid)
		{
			return new OneyuanTaoDao().MermberCanbuyNum(Aid, userid);
		}

		public static OneyuanTaoParticipantInfo GetAddParticipant(int UserId, string Pid = "", string payNum = "")
		{
			return new OneyuanTaoDao().GetAddParticipant(UserId, Pid, payNum);
		}

		public static bool IsExistAlipayRefundNUm(string batch_no)
		{
			return new OneyuanTaoDao().IsExistAlipayRefundNUm(batch_no);
		}

		public static int GetOneyuanTaoTotalNum(out int hasStart, out int waitStart, out int hasEnd)
		{
			return new OneyuanTaoDao().GetOneyuanTaoTotalNum(out hasStart, out waitStart, out hasEnd);
		}

		public static int GetRefundTotalNum(out int hasRefund)
		{
			return new OneyuanTaoDao().GetRefundTotalNum(out hasRefund);
		}

		public static IList<OneyuanTaoParticipantInfo> GetParticipantList(string ActivityId, int[] UserIds = null, string[] Ids = null)
		{
			return new OneyuanTaoDao().GetParticipantList(ActivityId, UserIds, Ids);
		}

		public static IList<OneyuanTaoParticipantInfo> GetRefundParticipantList(string[] PIds)
		{
			return new OneyuanTaoDao().GetRefundParticipantList(PIds);
		}

		public static List<string> GetParticipantPids(string ActivityId, bool IsPay = true, bool IsRefund = false, string PayWay = "alipay")
		{
			return new OneyuanTaoDao().GetParticipantPids(ActivityId, IsPay, IsRefund, PayWay);
		}

		public static string GetSkuStrBySkuId(string Skuid, bool ShowAttr = true)
		{
			return new OneyuanTaoDao().GetSkuStrBySkuId(Skuid, ShowAttr);
		}

		public static DbQueryResult GetOneyuanPartInDataTable(OneyuanTaoPartInQuery query)
		{
			return new OneyuanTaoDao().GetOneyuanPartInDataTable(query);
		}

		public static bool AddLuckInfo(LuckInfo info)
		{
			return new OneyuanTaoDao().AddLuckInfo(info);
		}

		public static bool AddLuckInfo(List<LuckInfo> infoList)
		{
			return new OneyuanTaoDao().AddLuckInfo(infoList);
		}

		public static bool setWin(string PrizeNum, string ActivityId)
		{
			return new OneyuanTaoDao().setWin(PrizeNum, ActivityId);
		}

		public static bool DelParticipantMember(string ActivityId, bool DelAll = true)
		{
			return new OneyuanTaoDao().DelParticipantMember(ActivityId, DelAll);
		}

		public static IList<LuckInfo> getLuckInfoList(bool IsWin, string ActivityId)
		{
			return new OneyuanTaoDao().getLuckInfoList(IsWin, ActivityId);
		}

		public static IList<LuckInfo> getLuckInfoListByAId(string ActivityId, int UserId)
		{
			return new OneyuanTaoDao().getLuckInfoListByAId(ActivityId, UserId);
		}

		public static IList<LuckInfo> getWinnerLuckInfoList(string ActivityId, string Pid = "")
		{
			return new OneyuanTaoDao().getWinnerLuckInfoList(ActivityId, Pid);
		}

		public static IList<OneyuanTaoInfo> GetOneyuanTaoInfoNotCalculate()
		{
			return new OneyuanTaoDao().GetOneyuanTaoInfoNotCalculate();
		}

		public static System.Data.DataTable PrizesDeliveryRecord(string Pid)
		{
			return new OneyuanTaoDao().PrizesDeliveryRecord(Pid);
		}

		public static string GetPrizesDeliveStatus(string status)
		{
			string result = "未定义";
			if (status != null)
			{
				if (!(status == "0"))
				{
					if (!(status == "1"))
					{
						if (!(status == "2"))
						{
							if (status == "3")
							{
								result = "已收货";
							}
						}
						else
						{
							result = "已发货";
						}
					}
					else
					{
						result = "待发货";
					}
				}
				else
				{
					result = "待填写收货地址";
				}
			}
			return result;
		}

		public static string CalculateWinner(string ActivityId = "")
		{
			string result = "0";
			if (Monitor.TryEnter(OneyuanTaoHelp.Calculate))
			{
				try
				{
					IList<OneyuanTaoInfo> list;
					if (string.IsNullOrEmpty(ActivityId))
					{
						list = new OneyuanTaoDao().GetOneyuanTaoInfoNotCalculate();
					}
					else
					{
						list = new List<OneyuanTaoInfo>();
						list.Add(OneyuanTaoHelp.GetOneyuanTaoInfoById(ActivityId));
					}
					foreach (OneyuanTaoInfo current in list)
					{
						result = "1";
						if (current.FinishedNum == 0)
						{
							OneyuanTaoHelp.SetOneyuanTaoHasCalculate(current.ActivityId);
						}
						else if (current.HasCalculate)
						{
							result = "success";
						}
						else if ((current.ReachType == 1 && current.FinishedNum >= current.ReachNum) || current.ReachType == 2 || (current.ReachType == 3 && current.FinishedNum >= current.ReachNum))
						{
							result = OneyuanTaoHelp.DoOneTaoDrawLottery(current);
						}
						else
						{
							result = "未满足开奖条件，开奖失败";
							OneyuanTaoHelp.DoOneTaoRefund(current);
						}
					}
				}
				catch (Exception var_3_134)
				{
					result = "0";
				}
				finally
				{
					Monitor.Exit(OneyuanTaoHelp.Calculate);
				}
			}
			else
			{
				result = "计算工作已启动，请等待计算开奖结果！";
			}
			return result;
		}

		private static string DoOneTaoDrawLottery(OneyuanTaoInfo WItem)
		{
			string result = "开奖失败";
			DateTime now = DateTime.Now;
			int participantCount = new OneyuanTaoDao().GetParticipantCount();
			if (participantCount < 50 + WItem.PrizeNumber)
			{
				OneyuanTaoHelp.AddInitParticipantInfo(50);
			}
			Dictionary<long, bool> dictionary = new Dictionary<long, bool>();
			dictionary.Clear();
			IList<Top50ParticipantInfo> top50ParticipantList = new OneyuanTaoDao().GetTop50ParticipantList(now, 50);
			while (true)
			{
				long num = 0L;
				foreach (Top50ParticipantInfo current in top50ParticipantList)
				{
					num += long.Parse(current.BuyTime.ToString("Hmmssfff"));
				}
				long num2 = num % long.Parse(WItem.FinishedNum.ToString());
				long key = 10000001L + num2;
				if (!dictionary.ContainsKey(key))
				{
					dictionary.Add(key, true);
					Top50ParticipantInfo top50ParticipantInfo = top50ParticipantList.Last<Top50ParticipantInfo>();
					top50ParticipantInfo.PrizeLuckInfo = string.Format("{0},{1},{2},{3}", new object[]
					{
						key.ToString(),
						num,
						WItem.FinishedNum,
						num2
					});
				}
				else
				{
					Top50ParticipantInfo top50ParticipantInfo = top50ParticipantList.Last<Top50ParticipantInfo>();
					top50ParticipantInfo.PrizeLuckInfo = string.Format("{0}重复,{1},{2},{3},", new object[]
					{
						key.ToString(),
						num,
						WItem.FinishedNum,
						num2
					});
				}
				if (dictionary.Count >= WItem.PrizeNumber)
				{
					break;
				}
				Top50ParticipantInfo nextParticipant = new OneyuanTaoDao().GetNextParticipant(now, top50ParticipantList.Last<Top50ParticipantInfo>().Pid);
				if (nextParticipant == null)
				{
					break;
				}
				top50ParticipantList.Add(nextParticipant);
			}
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
			if (dictionary.Count != WItem.PrizeNumber)
			{
				result = "订单数据不足，无法开出推定数量的奖项，请查看开奖未计算过程";
				new OneyuanTaoDao().SetErrPrizeCountInfo(WItem.ActivityId, JsonConvert.SerializeObject(top50ParticipantList, new JsonConverter[]
				{
					isoDateTimeConverter
				}));
			}
			else
			{
				result = "success";
				if (OneyuanTaoHelp.SetOneyuanTaoPrizeTime(WItem.ActivityId, now, JsonConvert.SerializeObject(top50ParticipantList, new JsonConverter[]
				{
					isoDateTimeConverter
				})))
				{
					foreach (KeyValuePair<long, bool> current2 in dictionary)
					{
						OneyuanTaoHelp.setWin(current2.Key.ToString(), WItem.ActivityId);
					}
				}
			}
			return result;
		}

		private static void DoOneTaoRefund(OneyuanTaoInfo WItem)
		{
			OneyuanTaoHelp.SetOneyuanTaoHasCalculate(WItem.ActivityId);
		}
	}
}
