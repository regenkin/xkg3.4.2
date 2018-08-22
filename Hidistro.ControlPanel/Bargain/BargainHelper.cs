using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Bargain;
using Hidistro.SqlDal.Bargain;
using System;
using System.Data;
using System.Text;

namespace Hidistro.ControlPanel.Bargain
{
	public static class BargainHelper
	{
		public static DbQueryResult GetBargainList(BargainQuery query)
		{
			return new BargainDao().GetBargainList(query);
		}

		public static bool UpdateNumberById(int bargainDetialId, int number, out int relNumber)
		{
			bool result = false;
			relNumber = number;
			BargainInfo bargainInfoByDetialId = BargainHelper.GetBargainInfoByDetialId(bargainDetialId);
			if (bargainInfoByDetialId != null)
			{
				int purchaseNumber = bargainInfoByDetialId.PurchaseNumber;
				int num = bargainInfoByDetialId.ActivityStock - bargainInfoByDetialId.TranNumber;
				if (num < relNumber)
				{
					relNumber = num;
				}
				if (purchaseNumber < relNumber)
				{
					relNumber = purchaseNumber;
				}
				if (relNumber > 0)
				{
					result = new BargainDao().UpdateNumberById(bargainDetialId, relNumber);
				}
			}
			return result;
		}

		public static DbQueryResult GetMyBargainList(BargainQuery query)
		{
			return new BargainDao().GetMyBargainList(query);
		}

		public static bool UpdateBargain(BargainInfo bargain)
		{
			return new BargainDao().UpdateBargain(bargain);
		}

		public static BargainInfo GetBargainInfoByDetialId(int bargainDetialId)
		{
			return new BargainDao().GetBargainInfoByDetialId(bargainDetialId);
		}

		public static BargainStatisticalData GetBargainStatisticalDataInfo(int bargainId)
		{
			return new BargainDao().GetBargainStatisticalDataInfo(bargainId);
		}

		public static int GetTotal(BargainQuery query)
		{
			return new BargainDao().GetTotal(query);
		}

		public static System.Data.DataTable GetAllBargain()
		{
			return new BargainDao().GetAllBargain();
		}

		public static bool DeleteBargainById(string ids)
		{
			return new BargainDao().DeleteBargainById(ids);
		}

		public static string IsCanBuyByBarginId(int bargainId)
		{
			return new BargainDao().IsCanBuyByBarginId(bargainId);
		}

		public static string IsCanBuyByBarginDetailId(int bargainDetailId)
		{
			return new BargainDao().IsCanBuyByBarginDetailId(bargainDetailId);
		}

		public static System.Data.DataTable GetBargainById(string ids)
		{
			return new BargainDao().GetBargainById(ids);
		}

		public static bool InsertBargain(BargainInfo bargain)
		{
			return new BargainDao().InsertBargain(bargain);
		}

		public static BargainInfo GetBargainInfo(int id)
		{
			return new BargainDao().GetBargainInfo(id);
		}

		public static System.Data.DataTable GetHelpBargainDetials(int bargainDetialId)
		{
			return new BargainDao().GetHelpBargainDetials(bargainDetialId);
		}

		public static bool ActionIsEnd(int bargainDetialId)
		{
			return new BargainDao().ActionIsEnd(bargainDetialId);
		}

		public static int GetHelpBargainDetialCount(int bargainDetialId)
		{
			return new BargainDao().GetHelpBargainDetialCount(bargainDetialId);
		}

		public static BargainDetialInfo GetBargainDetialInfo(int id)
		{
			return new BargainDao().GetBargainDetialInfo(id);
		}

		public static bool InsertBargainDetial(BargainDetialInfo bargainDetial, out int bargainDetialId)
		{
			return new BargainDao().InsertBargainDetial(bargainDetial, out bargainDetialId);
		}

		public static string InsertHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
		{
			string result = string.Empty;
			string text = new BargainDao().IsCanBuyByBarginId(helpBargainDetial.BargainId);
			if (text != "1")
			{
				result = text;
			}
			else
			{
				if (new BargainDao().InsertHelpBargainDetial(helpBargainDetial))
				{
					new BargainDao().UpdateBargainDetial(helpBargainDetial);
				}
				result = "1";
			}
			return result;
		}

		public static bool UpdateBargainDetial(HelpBargainDetialInfo helpBargainDetial)
		{
			return new BargainDao().UpdateBargainDetial(helpBargainDetial);
		}

		public static bool ExistsHelpBargainDetial(HelpBargainDetialInfo helpBargainDetial)
		{
			return new BargainDao().ExistsHelpBargainDetial(helpBargainDetial);
		}

		public static int HelpBargainCount(int bargainId)
		{
			return new BargainDao().HelpBargainCount(bargainId);
		}

		public static BargainDetialInfo GetBargainDetialInfo(int bargainId, int userId)
		{
			return new BargainDao().GetBargainDetialInfo(bargainId, userId);
		}

		public static HelpBargainDetialInfo GeHelpBargainDetialInfo(int bargainDetialId, int userId)
		{
			return new BargainDao().GeHelpBargainDetialInfo(bargainDetialId, userId);
		}

		public static bool UpdateBargain(int bargainId, DateTime endDate)
		{
			return new BargainDao().UpdateBargain(bargainId, endDate);
		}

		public static string GetLinkHtml(string id, string status, string stockNumber = "0", string tranNumber = "0")
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (status != null)
			{
				if (!(status == "进行中"))
				{
					if (!(status == "未开始"))
					{
						if (status == "已结束")
						{
							stringBuilder.Append("<a class='btn btn-danger btn-xs end' href='javascript:void(0)'>已结束</a>");
						}
					}
					else
					{
						stringBuilder.Append("<a class='btn btn-danger btn-xs end' href='javascript:void(0)'>未开始</a>");
					}
				}
				else if (Globals.ToNum(stockNumber) <= Globals.ToNum(tranNumber))
				{
					stringBuilder.Append("<a class='btn btn-danger btn-xs end' href='javascript:void(0)'>卖光了</a>");
				}
				else
				{
					stringBuilder.Append("<a class='btn btn-danger btn-xs' href='BargainDetial.aspx?id=" + id + "'>马上参与</a>");
				}
			}
			return stringBuilder.ToString();
		}

		public static string GetDay(object hour)
		{
			int num = (int)hour;
			string result;
			if (num < 24)
			{
				result = "即将结束";
			}
			else
			{
				result = "还剩：" + ((num % 24 > 0) ? (num / 24 + 1) : (num / 24)).ToString() + "天";
			}
			return result;
		}
	}
}
