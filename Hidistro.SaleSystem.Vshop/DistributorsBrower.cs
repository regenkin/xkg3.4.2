using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.StatisticsReport;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Messages;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Orders;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.VShop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace Hidistro.SaleSystem.Vshop
{
	public class DistributorsBrower
	{
		public static string UpdateDistributorSuperior(int userid, int tosuperuserid)
		{
			string text = DistributorsBrower.IsCanUpdateDistributorSuperior(userid, tosuperuserid);
			if (text == "1")
			{
				text = new DistributorsDao().UpdateDistributorSuperior(userid, tosuperuserid);
			}
			return text;
		}

		public static string IsCanUpdateDistributorSuperior(int userid, int tosuperuserid)
		{
			string text = "1";
			string result;
			if (userid == tosuperuserid)
			{
				result = "不能将自己设置为自己的上级";
			}
			else
			{
				int num = tosuperuserid;
				while (true)
				{
					int distributorSuperiorId = DistributorsBrower.GetDistributorSuperiorId(num);
					if (distributorSuperiorId == num || distributorSuperiorId == 0)
					{
						break;
					}
					num = distributorSuperiorId;
					if (num == userid)
					{
						goto Block_4;
					}
				}
				goto IL_63;
				Block_4:
				text = "0";
				IL_63:
				if (text != "1")
				{
					text = "不能将同一主线上的下级分销商设置为自己的上级";
				}
				result = text;
			}
			return result;
		}

		public static int GetDistributorSuperiorId(int userid)
		{
			return new DistributorsDao().GetDistributorSuperiorId(userid);
		}

		public static bool AddDistributors(DistributorsInfo distributors)
		{
			DistributorsDao distributorsDao = new DistributorsDao();
			System.Data.DataTable customDistributorStatistic = distributorsDao.GetCustomDistributorStatistic(distributors.StoreName);
			bool result;
			if (customDistributorStatistic.Rows.Count > 0)
			{
				result = false;
			}
			else
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				distributors.DistributorGradeId = DistributorGrade.OneDistributor;
				distributors.ParentUserId = new int?(currentMember.UserId);
				distributors.UserId = currentMember.UserId;
				DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(true);
				if (currentDistributors != null)
				{
					if (!string.IsNullOrEmpty(currentDistributors.ReferralPath) && !currentDistributors.ReferralPath.Contains("|"))
					{
						distributors.ReferralPath = currentDistributors.ReferralPath + "|" + currentDistributors.UserId.ToString();
					}
					else if (!string.IsNullOrEmpty(currentDistributors.ReferralPath) && currentDistributors.ReferralPath.Contains("|"))
					{
						distributors.ReferralPath = currentDistributors.ReferralPath.Split(new char[]
						{
							'|'
						})[1] + "|" + currentDistributors.UserId.ToString();
					}
					else
					{
						distributors.ReferralPath = currentDistributors.UserId.ToString();
					}
					distributors.ParentUserId = new int?(currentDistributors.UserId);
					if (distributors.Logo == "")
					{
						if (!string.IsNullOrEmpty(currentDistributors.Logo))
						{
							distributors.Logo = currentDistributors.Logo;
						}
						else
						{
							SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
							distributors.Logo = masterSettings.DistributorLogoPic;
						}
					}
					if (currentDistributors.DistributorGradeId == DistributorGrade.OneDistributor)
					{
						distributors.DistributorGradeId = DistributorGrade.TowDistributor;
					}
					else if (currentDistributors.DistributorGradeId == DistributorGrade.TowDistributor)
					{
						distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
					}
					else
					{
						distributors.DistributorGradeId = DistributorGrade.ThreeDistributor;
					}
				}
				result = new DistributorsDao().CreateDistributor(distributors);
			}
			return result;
		}

		private static int IsExiteDistributorsByStoreName(string stroname)
		{
			return new DistributorsDao().IsExiteDistributorsByStoreName(stroname);
		}

		public static System.Data.DataTable GetDrawRequestNum(int[] CheckValues)
		{
			return new DistributorsDao().GetDrawRequestNum(CheckValues);
		}

		public static System.Data.DataTable GetCurrentDistributorsCommosion()
		{
			return new DistributorsDao().GetDistributorsCommosion(Globals.GetCurrentDistributorId());
		}

		public static System.Data.DataTable GetDistributorsCommosion(int userId, DistributorGrade distributorgrade)
		{
			return new DistributorsDao().GetDistributorsCommosion(userId, distributorgrade);
		}

		public static System.Data.DataTable GetCurrentDistributorsCommosion(int userId)
		{
			return new DistributorsDao().GetCurrentDistributorsCommosion(userId);
		}

		public static int GetDistributorNum(DistributorGrade grade)
		{
			return new DistributorsDao().GetDistributorNum(grade);
		}

		public static DistributorsInfo GetDistributorInfo(int distributorid)
		{
			return new DistributorsDao().GetDistributorInfo(distributorid);
		}

		public static int GetDownDistributorNum(string userid)
		{
			return new DistributorsDao().GetDownDistributorNum(userid);
		}

		public static DistributorsInfo GetCurrentDistributors(bool readCache = true)
		{
			return DistributorsBrower.GetCurrentDistributors(Globals.GetCurrentDistributorId(), readCache);
		}

		public static DistributorsInfo GetCurrentDistributors(int userId, bool readCache = true)
		{
			DistributorsInfo distributorsInfo = null;
			if (readCache)
			{
				distributorsInfo = (HiCache.Get(string.Format("DataCache-Distributor-{0}", userId)) as DistributorsInfo);
			}
			if (distributorsInfo == null || distributorsInfo.UserId == 0)
			{
				distributorsInfo = new DistributorsDao().GetDistributorInfo(userId);
				HiCache.Insert(string.Format("DataCache-Distributor-{0}", userId), distributorsInfo, 360, CacheItemPriority.Normal);
			}
			return distributorsInfo;
		}

		public static DistributorsInfo GetNowCurrentDistributors(int userId)
		{
			return new DistributorsDao().GetDistributorInfo(userId);
		}

		public static void RemoveDistributorCache(int userId)
		{
			HiCache.Remove(string.Format("DataCache-Distributor-{0}", userId));
		}

		public static DistributorsInfo GetUserIdDistributors(int userid)
		{
			return new DistributorsDao().GetDistributorInfo(userid);
		}

		public bool UpdateGradeId(ArrayList GradeIdList, ArrayList ReferralUserIdList)
		{
			return new DistributorsDao().UpdateGradeId(GradeIdList, ReferralUserIdList);
		}

		public static bool UpdateStoreCard(int userId, string imgUrl)
		{
			return new DistributorsDao().UpdateStoreCard(userId, imgUrl);
		}

		public static System.Data.DataTable OrderIDGetCommosion(string orderid)
		{
			return new DistributorsDao().OrderIDGetCommosion(orderid);
		}

		public static bool AddBalanceDrawRequest(BalanceDrawRequestInfo balancerequestinfo, MemberInfo memberinfo)
		{
			DistributorsInfo currentDistributors = DistributorsBrower.GetCurrentDistributors(false);
			bool result;
			if (memberinfo != null && !string.IsNullOrEmpty(memberinfo.RealName) && currentDistributors != null && currentDistributors.UserId > 0 && !string.IsNullOrEmpty(memberinfo.CellPhone))
			{
				if (!string.IsNullOrEmpty(balancerequestinfo.MerchantCode) && currentDistributors.RequestAccount != balancerequestinfo.MerchantCode)
				{
					new DistributorsDao().UpdateDistributorById(balancerequestinfo.MerchantCode, memberinfo.UserId);
				}
				balancerequestinfo.UserId = memberinfo.UserId;
				balancerequestinfo.UserName = memberinfo.UserName;
				if (balancerequestinfo.RequesType == 0 || balancerequestinfo.RequesType == 3)
				{
					balancerequestinfo.MerchantCode = memberinfo.OpenId;
				}
				else if (balancerequestinfo.MerchantCode.Length < 1)
				{
					balancerequestinfo.MerchantCode = currentDistributors.RequestAccount;
				}
				balancerequestinfo.CellPhone = memberinfo.CellPhone;
				result = new DistributorsDao().AddBalanceDrawRequest(balancerequestinfo);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static bool SetBalanceDrawRequestIsCheckStatus(int[] serialids, int checkValue, string Remark = null, string Amount = null)
		{
			bool flag = new DistributorsDao().SetBalanceDrawRequestIsCheckStatus(serialids, checkValue, Remark, Amount);
			if (flag && checkValue == -1)
			{
				try
				{
					for (int i = 0; i < serialids.Length; i++)
					{
						int num = serialids[i];
						BalanceDrawRequestInfo balanceDrawRequestById = DistributorsBrower.GetBalanceDrawRequestById(num.ToString());
						if (balanceDrawRequestById != null)
						{
							Messenger.SendWeiXinMsg_DrawCashReject(balanceDrawRequestById);
						}
					}
				}
				catch (Exception var_3_6E)
				{
				}
			}
			return flag;
		}

		public static BalanceDrawRequestInfo GetBalanceDrawRequestById(string serialids)
		{
			return new DistributorsDao().GetBalanceDrawRequestById(serialids);
		}

		public static int GetBalanceDrawRequestIsCheckStatus(int serialid)
		{
			return new DistributorsDao().GetBalanceDrawRequestIsCheckStatus(serialid);
		}

		public static Dictionary<int, int> GetMulBalanceDrawRequestIsCheckStatus(int[] serialids)
		{
			return new DistributorsDao().GetMulBalanceDrawRequestIsCheckStatus(serialids);
		}

		public static bool GetBalanceDrawRequestIsCheck(int serialid)
		{
			return new DistributorsDao().GetBalanceDrawRequestIsCheck(serialid);
		}

		public static System.Data.DataTable GetDistributorsCommission(DistributorsQuery query)
		{
			return new DistributorsDao().GetDistributorsCommission(query);
		}

		public static System.Data.DataTable GetDownDistributors(DistributorsQuery query, out int total)
		{
			return new DistributorsDao().GetDownDistributors(query, out total);
		}

		public static System.Data.DataTable GetThreeDistributors(DistributorsQuery query, out int total)
		{
			return new DistributorsDao().GetThreeDistributors(query, out total);
		}

		public static DbQueryResult GetDistributors(DistributorsQuery query)
		{
			return new DistributorsDao().GetDistributors(query, null, null);
		}

		public static System.Data.DataTable SelectDistributors(DistributorsQuery query)
		{
			return new DistributorsDao().SelectDistributors(query, null, null);
		}

		public static decimal GetUserCommissions(int userid, DateTime fromdatetime, string endtime = null, string storeName = null, string OrderNum = null, string level = "")
		{
			return new DistributorsDao().GetUserCommissions(userid, fromdatetime, endtime, storeName, OrderNum, level);
		}

		public static DbQueryResult GetCommissions(CommissionsQuery query)
		{
			return new DistributorsDao().GetCommissions(query);
		}

		public static bool UpdateDistributor(DistributorsInfo query)
		{
			int num = DistributorsBrower.IsExiteDistributorsByStoreName(query.StoreName);
			return (num == 0 || num == query.UserId) && new DistributorsDao().UpdateDistributor(query);
		}

		public static bool UpdateDistributorMessage(DistributorsInfo query)
		{
			int num = DistributorsBrower.IsExiteDistributorsByStoreName(query.StoreName);
			return (num == 0 || num == query.UserId) && new DistributorsDao().UpdateDistributorMessage(query);
		}

		public static bool SetRedpackRecordIsUsed(int id, bool issend)
		{
			return new SendRedpackRecordDao().SetRedpackRecordIsUsed(id, issend);
		}

		public static System.Data.DataTable GetNotSendRedpackRecord(int balancedrawrequestid)
		{
			return new SendRedpackRecordDao().GetNotSendRedpackRecord(balancedrawrequestid);
		}

		public static DbQueryResult GetSendRedpackRecordRequest(SendRedpackRecordQuery query)
		{
			return new SendRedpackRecordDao().GetSendRedpackRecordRequest(query);
		}

		public static bool HasDrawRequest(int serialid)
		{
			return new SendRedpackRecordDao().HasDrawRequest(serialid);
		}

		public static SendRedpackRecordInfo GetSendRedpackRecordByID(string id = null, string sid = null)
		{
			return new SendRedpackRecordDao().GetSendRedpackRecordByID(id, sid);
		}

		public static int GetRedPackTotalAmount(int balancedrawrequestid, int userid)
		{
			return new SendRedpackRecordDao().GetRedPackTotalAmount(balancedrawrequestid, userid);
		}

		public static string SendRedPackToBalanceDrawRequest(int serialid)
		{
			return new DistributorsDao().SendRedPackToBalanceDrawRequest(serialid);
		}

		public static bool FrozenCommision(int userid, string ReferralStatus)
		{
			try
			{
				MemberInfo member = new MemberDao().GetMember(userid);
				if (member != null)
				{
					if (ReferralStatus == "1")
					{
						Messenger.SendWeiXinMsg_AccountLockOrUnLock(member, true);
					}
					else if (ReferralStatus == "0")
					{
						Messenger.SendWeiXinMsg_AccountLockOrUnLock(member, false);
					}
					else if (ReferralStatus == "9")
					{
						Messenger.SendWeiXinMsg_DistributorCancel(member);
					}
				}
			}
			catch (Exception var_1_7A)
			{
			}
			bool result = new DistributorsDao().FrozenCommision(userid, ReferralStatus);
			DistributorsBrower.RemoveDistributorCache(userid);
			return result;
		}

		public static int FrozenCommisionChecks(string userids, string ReferralStatus)
		{
			int result = new DistributorsDao().FrozenCommisionChecks(userids, ReferralStatus);
			string[] array = userids.Trim(new char[]
			{
				','
			}).Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				int num = Globals.ToNum(s);
				if (num > 0)
				{
					MemberInfo member = new MemberDao().GetMember(num);
					if (member != null)
					{
						if (ReferralStatus == "1")
						{
							Messenger.SendWeiXinMsg_AccountLockOrUnLock(member, true);
						}
						else if (ReferralStatus == "0")
						{
							Messenger.SendWeiXinMsg_AccountLockOrUnLock(member, false);
						}
						else if (ReferralStatus == "9")
						{
							Messenger.SendWeiXinMsg_DistributorCancel(member);
						}
						DistributorsBrower.RemoveDistributorCache(num);
					}
				}
			}
			return result;
		}

		public static int EditCommisionsGrade(string userids, string Grade)
		{
			return new DistributorsDao().EditCommisionsGrade(userids, Grade);
		}

		public static bool EditDisbutosInfos(string userid, string QQNum, string CellPhone, string RealName, string Password)
		{
			return new DistributorsDao().EditDisbutosInfos(userid, QQNum, CellPhone, RealName, Password);
		}

		public static bool IsExitsCommionsRequest()
		{
			return new DistributorsDao().IsExitsCommionsRequest(Globals.GetCurrentDistributorId());
		}

		public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query, string[] extendChecks = null)
		{
			return new DistributorsDao().GetBalanceDrawRequest(query, extendChecks);
		}

		public static System.Data.DataSet GetDistributorOrder(OrderQuery query)
		{
			return new OrderDao().GetDistributorOrder(query);
		}

		public static System.Data.DataSet GetDistributorOrderByDetials(OrderQuery query)
		{
			return new OrderDao().GetDistributorOrderByDetials(query);
		}

		public static DbQueryResult GetDistributorOrderByStatus(OrderQuery query, int userId)
		{
			return new OrderDao().GetDistributorOrderByStatus(query, userId);
		}

		public static int GetDistributorOrderCount(OrderQuery query)
		{
			return new OrderDao().GetDistributorOrderCount(query);
		}

		public static void AddDistributorProductId(List<int> productList)
		{
			int currentMemberUserId = Globals.GetCurrentMemberUserId();
			if (currentMemberUserId > 0 && productList.Count > 0)
			{
				new DistributorsDao().RemoveDistributorProducts(productList, currentMemberUserId);
				foreach (int current in productList)
				{
					new DistributorsDao().AddDistributorProducts(current, currentMemberUserId);
				}
			}
		}

		public static void DeleteDistributorProductIds(List<int> productList)
		{
			int userId = DistributorsBrower.GetCurrentDistributors(true).UserId;
			if (userId > 0 && productList.Count > 0)
			{
				new DistributorsDao().RemoveDistributorProducts(productList, userId);
			}
		}

		public static bool setCommission(OrderInfo order, DistributorsInfo DisInfo)
		{
			bool result = false;
			decimal num = 0m;
			decimal num2 = 0m;
			string text = order.ReferralUserId.ToString();
			string orderId = order.OrderId;
			decimal d = 0m;
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			foreach (LineItemInfo current in order.LineItems.Values)
			{
				if (current.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
				{
					num2 += current.ItemsCommission;
					if (!string.IsNullOrEmpty(current.ItemAdjustedCommssion.ToString()) && current.ItemAdjustedCommssion > 0m)
					{
						if (!current.IsAdminModify)
						{
							num += current.ItemAdjustedCommssion;
						}
					}
					d += current.GetSubTotal() - current.DiscountAverage - current.ItemAdjustedCommssion;
				}
			}
			d -= order.AdjustedFreight;
			bool flag = false;
			decimal num3;
			if (flag)
			{
				num3 = num2;
			}
			else
			{
				num3 = num2 - num;
				if (num3 < 0m)
				{
					num3 = 0m;
				}
			}
			result = new DistributorsDao().UpdateCalculationCommission(text, text, orderId, d + order.AdjustedFreight, num3);
			try
			{
				if (order != null && num3 > 0m)
				{
					string userOpenIdByUserId = MemberProcessor.GetUserOpenIdByUserId(DisInfo.UserId);
					string aliUserOpenIdByUserId = MemberProcessor.GetAliUserOpenIdByUserId(DisInfo.UserId);
					Messenger.SendWeiXinMsg_OrderGetCommission(order, userOpenIdByUserId, aliUserOpenIdByUserId, num3);
				}
			}
			catch (Exception ex)
			{
				Globals.Debuglog("分佣问题：" + ex.Message, "_Debuglog.txt");
			}
			int notDescDistributorGrades = DistributorsBrower.GetNotDescDistributorGrades(text);
			if (notDescDistributorGrades > 0)
			{
				arrayList.Add(notDescDistributorGrades);
				arrayList2.Add(text);
				result = new DistributorsDao().UpdateGradeId(arrayList, arrayList2);
				if (DisInfo.DistriGradeId != notDescDistributorGrades)
				{
					DistributorsBrower.DistributorGradeChange(DisInfo, order.OrderId, notDescDistributorGrades);
				}
			}
			return result;
		}

		public static string GenerateOrderId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}

		public static void DistributorGradeChange(DistributorsInfo distributor, string orderid, int newDistributorGradeid)
		{
			DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(newDistributorGradeid);
			if (distributorGradeInfo != null && distributorGradeInfo.AddCommission > 0m)
			{
				try
				{
					MemberInfo member = MemberProcessor.GetMember(distributor.UserId, true);
					Messenger.SendWeiXinMsg_DistributorGradeChange(member, distributorGradeInfo.Name);
				}
				catch
				{
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (masterSettings.IsAddCommission == 1)
				{
					try
					{
						DateTime t = DateTime.Parse(masterSettings.AddCommissionStartTime);
						DateTime t2 = DateTime.Parse(masterSettings.AddCommissionEndTime).AddDays(1.0);
						if (DateTime.Now > t && DateTime.Now < t2)
						{
							decimal num = distributor.ReferralRequestBalance + distributor.ReferralBlance;
							DistributorGradeCommissionInfo distributorGradeCommissionInfo = new DistributorGradeCommissionInfo();
							distributorGradeCommissionInfo.UserId = distributor.UserId;
							distributorGradeCommissionInfo.Commission = distributorGradeInfo.AddCommission;
							distributorGradeCommissionInfo.PubTime = DateTime.Now;
							distributorGradeCommissionInfo.OperAdmin = "system";
							distributorGradeCommissionInfo.Memo = "升级奖励";
							distributorGradeCommissionInfo.OrderID = orderid;
							distributorGradeCommissionInfo.OldCommissionTotal = num;
							if (!string.IsNullOrEmpty(distributorGradeCommissionInfo.OrderID))
							{
								distributorGradeCommissionInfo.ReferralUserId = new OrderDao().GetOrderReferralUserId(distributorGradeCommissionInfo.OrderID);
							}
							else
							{
								distributorGradeCommissionInfo.OrderID = "U" + DistributorsBrower.GenerateOrderId();
							}
							distributorGradeCommissionInfo.CommType = 3;
							if (distributorGradeCommissionInfo.ReferralUserId == 0)
							{
								distributorGradeCommissionInfo.ReferralUserId = distributorGradeCommissionInfo.UserId;
								distributorGradeCommissionInfo.CommType = 4;
							}
							DistributorGradeCommissionBrower.AddCommission(distributorGradeCommissionInfo);
							NoticeInfo noticeInfo = new NoticeInfo();
							noticeInfo.Title = "恭喜分销商获得升级奖励佣金￥" + distributorGradeInfo.AddCommission.ToString("F2");
							StringBuilder stringBuilder = new StringBuilder();
							stringBuilder.Append(string.Concat(new string[]
							{
								"<p class='textlist'>恭喜<span style='color:#3D9BDF;'>",
								distributor.StoreName,
								"</span>自动升级为<span style='color:red;'>",
								distributorGradeInfo.Name,
								"</span>分销商",
								(num > 0m) ? ("(累计获得佣金" + num.ToString("F2") + "元)") : "",
								"，系统额外奖励佣金",
								distributorGradeInfo.AddCommission.ToString("F2"),
								"元！</p>"
							}));
							stringBuilder.Append(string.Concat(new string[]
							{
								"<p class='textlist'>自",
								t.ToString("yyyy年MM月dd日"),
								"至",
								t2.ToString("yyyy年MM月dd日"),
								"，分销商等级提升将获得系统奖励的额外佣金。</p>"
							}));
							stringBuilder.Append("<table class='table table-bordered' style='text-align: center;'><thead><tr class='firstRow'><th style='text-align:center;'>等级名称</th><th style='text-align:center;'>需要佣金</th><th style='text-align:center;'>奖励佣金</th></tr></thead><tbody>");
							System.Data.DataTable allDistributorGrade = DistributorGradeBrower.GetAllDistributorGrade();
							int count = allDistributorGrade.Rows.Count;
							for (int i = 0; i < count; i++)
							{
								stringBuilder.Append(string.Concat(new string[]
								{
									"<tr><td>",
									allDistributorGrade.Rows[i]["Name"].ToString(),
									"</td><td>￥",
									decimal.Parse(allDistributorGrade.Rows[i]["CommissionsLimit"].ToString()).ToString("F2"),
									"</td><td>￥",
									decimal.Parse(allDistributorGrade.Rows[i]["AddCommission"].ToString()).ToString("F2"),
									"</td></tr>"
								}));
							}
							stringBuilder.Append("</tbody></table>");
							noticeInfo.Memo = stringBuilder.ToString();
							noticeInfo.Author = "system";
							noticeInfo.AddTime = DateTime.Now;
							noticeInfo.IsPub = 1;
							noticeInfo.PubTime = new DateTime?(DateTime.Now);
							noticeInfo.SendType = 0;
							noticeInfo.SendTo = 0;
							NoticeBrowser.SaveNotice(noticeInfo);
						}
					}
					catch (Exception ex)
					{
						Globals.Debuglog("升级奖励异常" + ex.Message, "_Debuglog.txt");
					}
				}
			}
		}

		public static int GetDistributorGrades(string ReferralUserId)
		{
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(int.Parse(ReferralUserId));
			List<DistributorGradeInfo> source = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
			int result = 0;
			foreach (DistributorGradeInfo current in from item in source
			orderby item.CommissionsLimit descending
			select item)
			{
				if (userIdDistributors.DistriGradeId == current.GradeId)
				{
					result = 0;
					break;
				}
				if (current.CommissionsLimit <= userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance)
				{
					userIdDistributors.DistriGradeId = current.GradeId;
					result = current.GradeId;
					break;
				}
			}
			return result;
		}

		public static int GetNotDescDistributorGrades(string ReferralUserId)
		{
			int result = 0;
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(int.Parse(ReferralUserId));
			decimal num = userIdDistributors.ReferralBlance + userIdDistributors.ReferralRequestBalance;
			DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
			if (distributorGradeInfo != null && num < distributorGradeInfo.CommissionsLimit)
			{
				result = userIdDistributors.DistriGradeId;
			}
			else
			{
				List<DistributorGradeInfo> source = new DistributorsDao().GetDistributorGrades() as List<DistributorGradeInfo>;
				foreach (DistributorGradeInfo current in from item in source
				orderby item.CommissionsLimit descending
				select item)
				{
					if (userIdDistributors.DistriGradeId == current.GradeId)
					{
						result = userIdDistributors.DistriGradeId;
						break;
					}
					if (current.CommissionsLimit <= num)
					{
						result = current.GradeId;
						break;
					}
				}
			}
			return result;
		}

		public static bool UpdateCalculationCommission(OrderInfo order)
		{
			new MemberDao().SetOrderDate(order.UserId, 2);
			DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(order.ReferralUserId);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			bool result = false;
			if (userIdDistributors != null)
			{
				result = DistributorsBrower.setCommission(order, userIdDistributors);
				if (!string.IsNullOrEmpty(order.ReferralPath))
				{
					ArrayList arrayList = new ArrayList();
					decimal num = 0m;
					ArrayList arrayList2 = new ArrayList();
					string referralUserId = order.ReferralUserId.ToString();
					string orderId = order.OrderId;
					ArrayList arrayList3 = new ArrayList();
					decimal d = 0m;
					ArrayList arrayList4 = new ArrayList();
					string[] array = order.ReferralPath.Split(new char[]
					{
						'|'
					});
					if (array.Length == 1)
					{
						DistributorsInfo userIdDistributors2 = DistributorsBrower.GetUserIdDistributors(int.Parse(array[0]));
						if (userIdDistributors2 != null)
						{
							foreach (LineItemInfo current in order.LineItems.Values)
							{
								if (current.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
								{
									num += current.SecondItemsCommission;
									d += current.GetSubTotal();
								}
							}
							arrayList.Add(num);
							arrayList3.Add(d + order.AdjustedFreight);
							arrayList2.Add(userIdDistributors2.UserId);
							try
							{
								if (order != null && num > 0m)
								{
									string userOpenIdByUserId = MemberProcessor.GetUserOpenIdByUserId(userIdDistributors2.UserId);
									string aliUserOpenIdByUserId = MemberProcessor.GetAliUserOpenIdByUserId(userIdDistributors2.UserId);
									Messenger.SendWeiXinMsg_OrderGetCommission(order, userOpenIdByUserId, aliUserOpenIdByUserId, num);
								}
							}
							catch (Exception var_16_1FA)
							{
							}
							int notDescDistributorGrades = DistributorsBrower.GetNotDescDistributorGrades(userIdDistributors2.UserId.ToString());
							if (userIdDistributors2.DistriGradeId != notDescDistributorGrades)
							{
								DistributorsBrower.DistributorGradeChange(userIdDistributors2, order.OrderId, notDescDistributorGrades);
							}
						}
					}
					if (array.Length == 2)
					{
						DistributorsInfo userIdDistributors3 = DistributorsBrower.GetUserIdDistributors(int.Parse(array[0]));
						foreach (LineItemInfo current in order.LineItems.Values)
						{
							if (current.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
							{
								num += current.ThirdItemsCommission;
								d += current.GetSubTotal();
							}
						}
						arrayList.Add(num);
						arrayList3.Add(d + order.AdjustedFreight);
						arrayList2.Add(userIdDistributors3.UserId);
						try
						{
							if (order != null && num > 0m)
							{
								string userOpenIdByUserId = MemberProcessor.GetUserOpenIdByUserId(userIdDistributors3.UserId);
								string aliUserOpenIdByUserId = MemberProcessor.GetAliUserOpenIdByUserId(userIdDistributors3.UserId);
								Messenger.SendWeiXinMsg_OrderGetCommission(order, userOpenIdByUserId, aliUserOpenIdByUserId, num);
							}
						}
						catch (Exception var_16_1FA)
						{
						}
						int notDescDistributorGrades = DistributorsBrower.GetNotDescDistributorGrades(userIdDistributors3.UserId.ToString());
						if (userIdDistributors3.DistriGradeId != notDescDistributorGrades)
						{
							DistributorsBrower.DistributorGradeChange(userIdDistributors3, order.OrderId, notDescDistributorGrades);
						}
						DistributorsInfo userIdDistributors4 = DistributorsBrower.GetUserIdDistributors(int.Parse(array[1]));
						num = 0m;
						d = 0m;
						foreach (LineItemInfo current in order.LineItems.Values)
						{
							if (current.OrderItemsStatus.ToString() == OrderStatus.SellerAlreadySent.ToString())
							{
								num += current.SecondItemsCommission;
								d += current.GetSubTotal();
							}
						}
						arrayList.Add(num);
						arrayList3.Add(d + order.AdjustedFreight);
						arrayList2.Add(userIdDistributors4.UserId);
						try
						{
							if (order != null && num > 0m)
							{
								string userOpenIdByUserId = MemberProcessor.GetUserOpenIdByUserId(userIdDistributors4.UserId);
								string aliUserOpenIdByUserId = MemberProcessor.GetAliUserOpenIdByUserId(userIdDistributors4.UserId);
								Messenger.SendWeiXinMsg_OrderGetCommission(order, userOpenIdByUserId, aliUserOpenIdByUserId, num);
							}
						}
						catch (Exception var_16_1FA)
						{
						}
						int notDescDistributorGrades2 = DistributorsBrower.GetNotDescDistributorGrades(userIdDistributors4.UserId.ToString());
						if (userIdDistributors4.DistriGradeId != notDescDistributorGrades2)
						{
							DistributorsBrower.DistributorGradeChange(userIdDistributors4, order.OrderId, notDescDistributorGrades2);
						}
					}
					result = new DistributorsDao().UpdateTwoCalculationCommission(arrayList2, referralUserId, orderId, arrayList3, arrayList);
					for (int i = 0; i < arrayList2.Count; i++)
					{
						int notDescDistributorGrades3 = DistributorsBrower.GetNotDescDistributorGrades(arrayList2[i].ToString());
						arrayList4.Add(notDescDistributorGrades3);
					}
					result = new DistributorsDao().UpdateGradeId(arrayList4, arrayList2);
				}
				DistributorsBrower.RemoveDistributorCache(userIdDistributors.UserId);
			}
			OrderRedPagerBrower.CreateOrderRedPager(order.OrderId, order.GetTotal(), order.UserId);
			int num2 = Globals.IsNumeric(order.ActivitiesId) ? Globals.ToNum(order.ActivitiesId) : 0;
			if (num2 > 0)
			{
				Hidistro.SqlDal.VShop.ActivityDao activityDao = new Hidistro.SqlDal.VShop.ActivityDao();
				ActivityDetailInfo activityDetailInfo = activityDao.GetActivityDetailInfo(num2);
				if (activityDetailInfo != null)
				{
					int couponId = activityDetailInfo.CouponId;
					int integral = activityDetailInfo.Integral;
					if (couponId > 0)
					{
						CouponInfo coupon = ShoppingProcessor.GetCoupon(couponId.ToString());
						if (coupon != null)
						{
							CouponDao couponDao = new CouponDao();
							SendCouponResult sendCouponResult = couponDao.SendCouponToMember(couponId, order.UserId);
							try
							{
								if (order != null)
								{
									Messenger.SendWeiXinMsg_OrderGetCoupon(order);
								}
							}
							catch (Exception var_16_1FA)
							{
							}
						}
					}
					if (integral > 0)
					{
						new OrderDao().AddMemberPointNumber(integral, order, null);
						try
						{
							if (order != null)
							{
								Messenger.SendWeiXinMsg_OrderGetPoint(order, integral);
							}
						}
						catch (Exception var_16_1FA)
						{
						}
					}
				}
			}
			MemberProcessor.UpdateUserAccount(order);
			try
			{
				string text = "";
				DateTime orderDate = order.OrderDate;
				DateTime? payDate = order.PayDate;
				if (order.Gateway == "hishop.plugins.payment.podrequest")
				{
					payDate = new DateTime?(orderDate);
				}
				if (payDate.HasValue && payDate.Value.ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
				{
					bool flag = new ShopStatisticDao().StatisticsOrdersByRecDate(payDate.Value, UpdateAction.AllUpdate, 0, out text);
				}
			}
			catch
			{
			}
			return result;
		}

		public static System.Data.DataSet GetUserRanking(int userid)
		{
			return new DistributorsDao().GetUserRanking(userid);
		}

		public static string GetBalanceDrawRequestStatus(int status)
		{
			string result = "未知";
			switch (status)
			{
			case -1:
				result = "已驳回";
				break;
			case 0:
				result = "待审核";
				break;
			case 1:
				result = "已审核";
				break;
			case 2:
				result = "已发放";
				break;
			case 3:
				result = "付款异常";
				break;
			}
			return result;
		}
	}
}
