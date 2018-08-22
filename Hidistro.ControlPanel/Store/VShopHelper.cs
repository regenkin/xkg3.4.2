using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.FenXiao;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.VShop;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;

namespace Hidistro.ControlPanel.Store
{
	public static class VShopHelper
	{
		private const string CacheKey = "Message-{0}";

		public static System.Data.DataTable GetHomeProducts()
		{
			return new HomeProductDao().GetHomeProducts();
		}

		public static bool AddHomeProdcut(int productId)
		{
			return new HomeProductDao().AddHomeProdcut(productId);
		}

		public static bool RemoveHomeProduct(int productId)
		{
			return new HomeProductDao().RemoveHomeProduct(productId);
		}

		public static bool RemoveAllHomeProduct()
		{
			return new HomeProductDao().RemoveAllHomeProduct();
		}

		public static bool UpdateHomeProductSequence(int ProductId, int displaysequence)
		{
			return new HomeProductDao().UpdateHomeProductSequence(ProductId, displaysequence);
		}

		public static IList<BannerInfo> GetAllBanners()
		{
			return new BannerDao().GetAllBanners();
		}

		public static IList<NavigateInfo> GetAllNavigate()
		{
			return new BannerDao().GetAllNavigate();
		}

		public static bool SaveTplCfg(TplCfgInfo info)
		{
			return new BannerDao().SaveTplCfg(info);
		}

		public static int GetCountBanner()
		{
			return new BannerDao().GetCountBanner();
		}

		public static bool UpdateTplCfg(TplCfgInfo info)
		{
			return new BannerDao().UpdateTplCfg(info);
		}

		public static TplCfgInfo GetTplCfgById(int id)
		{
			return new BannerDao().GetTplCfgById(id);
		}

		public static bool DelTplCfg(int id)
		{
			return new BannerDao().DelTplCfg(id);
		}

		public static void SwapTplCfgSequence(int bannerId, int replaceBannerId)
		{
			BannerDao bannerDao = new BannerDao();
			TplCfgInfo tplCfgById = bannerDao.GetTplCfgById(bannerId);
			TplCfgInfo tplCfgById2 = bannerDao.GetTplCfgById(replaceBannerId);
			if (tplCfgById != null && tplCfgById2 != null)
			{
				int displaySequence = tplCfgById.DisplaySequence;
				tplCfgById.DisplaySequence = tplCfgById2.DisplaySequence;
				tplCfgById2.DisplaySequence = displaySequence;
				bannerDao.UpdateTplCfg(tplCfgById);
				bannerDao.UpdateTplCfg(tplCfgById2);
			}
		}

		public static bool SaveActivity1(ActivityInfo activity)
		{
			int activityId = new ActivityDao().SaveActivity(activity);
			ReplyInfo replyInfo = new TextReplyInfo();
			replyInfo.Keys = activity.Keys;
			replyInfo.MatchType = MatchType.Equal;
			replyInfo.MessageType = MessageType.Text;
			replyInfo.ReplyType = ReplyType.SignUp;
			replyInfo.ActivityId = activityId;
			return new ReplyDao().SaveReply(replyInfo);
		}

		public static ActivityInfo GetActivity(int activityId)
		{
			return new ActivityDao().GetActivity(activityId);
		}

		public static IList<ActivityInfo> GetAllActivity()
		{
			return new ActivityDao().GetAllActivity();
		}

		public static IList<ActivitySignUpInfo> GetActivitySignUpById(int activityId)
		{
			return new ActivitySignUpDao().GetActivitySignUpById(activityId);
		}

		public static IList<MenuInfo> GetMenus()
		{
			IList<MenuInfo> list = new List<MenuInfo>();
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus();
			IList<MenuInfo> result;
			if (topMenus == null)
			{
				result = list;
			}
			else
			{
				foreach (MenuInfo current in topMenus)
				{
					list.Add(current);
					IList<MenuInfo> menusByParentId = menuDao.GetMenusByParentId(current.MenuId);
					if (menusByParentId != null)
					{
						foreach (MenuInfo current2 in menusByParentId)
						{
							list.Add(current2);
						}
					}
				}
				result = list;
			}
			return result;
		}

		public static IList<MenuInfo> GetMenusByParentId(int parentId)
		{
			return new MenuDao().GetMenusByParentId(parentId);
		}

		public static IList<MenuInfo> GetFuwuMenusByParentId(int parentId)
		{
			return new MenuDao().GetFuwuMenusByParentId(parentId);
		}

		public static MenuInfo GetMenu(int menuId)
		{
			return new MenuDao().GetMenu(menuId);
		}

		public static MenuInfo GetFuwuMenu(int menuId)
		{
			return new MenuDao().GetFuwuMenu(menuId);
		}

		public static IList<MenuInfo> GetTopMenus()
		{
			return new MenuDao().GetTopMenus();
		}

		public static IList<MenuInfo> GetTopFuwuMenus()
		{
			return new MenuDao().GetTopFuwuMenus();
		}

		public static bool CanAddMenu(int parentId)
		{
			IList<MenuInfo> menusByParentId = new MenuDao().GetMenusByParentId(parentId);
			bool result;
			if (menusByParentId == null || menusByParentId.Count == 0)
			{
				result = true;
			}
			else if (parentId == 0)
			{
				result = (menusByParentId.Count < 3);
			}
			else
			{
				result = (menusByParentId.Count < 5);
			}
			return result;
		}

		public static bool CanAddFuwuMenu(int parentId)
		{
			IList<MenuInfo> fuwuMenusByParentId = new MenuDao().GetFuwuMenusByParentId(parentId);
			bool result;
			if (fuwuMenusByParentId == null || fuwuMenusByParentId.Count == 0)
			{
				result = true;
			}
			else if (parentId == 0)
			{
				result = (fuwuMenusByParentId.Count < 3);
			}
			else
			{
				result = (fuwuMenusByParentId.Count < 5);
			}
			return result;
		}

		public static bool UpdateMenu(MenuInfo menu)
		{
			return new MenuDao().UpdateMenu(menu);
		}

		public static bool UpdateFuwuMenu(MenuInfo menu)
		{
			return new MenuDao().UpdateFuwuMenu(menu);
		}

		public static bool SaveMenu(MenuInfo menu)
		{
			return new MenuDao().SaveMenu(menu);
		}

		public static bool SaveFuwuMenu(MenuInfo menu)
		{
			return new MenuDao().SaveFuwuMenu(menu);
		}

		public static bool DeleteMenu(int menuId)
		{
			return new MenuDao().DeleteMenu(menuId);
		}

		public static bool DeleteFuwuMenu(int menuId)
		{
			return new MenuDao().DeleteFuwuMenu(menuId);
		}

		public static void SwapMenuSequence(int menuId, bool isUp)
		{
			new MenuDao().SwapMenuSequence(menuId, isUp);
		}

		public static IList<MenuInfo> GetInitMenus()
		{
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topMenus = menuDao.GetTopMenus();
			foreach (MenuInfo current in topMenus)
			{
				current.Chilren = menuDao.GetMenusByParentId(current.MenuId);
				if (current.Chilren == null)
				{
					current.Chilren = new List<MenuInfo>();
				}
			}
			return topMenus;
		}

		public static IList<MenuInfo> GetInitFuwuMenus()
		{
			MenuDao menuDao = new MenuDao();
			IList<MenuInfo> topFuwuMenus = menuDao.GetTopFuwuMenus();
			foreach (MenuInfo current in topFuwuMenus)
			{
				current.Chilren = menuDao.GetFuwuMenusByParentId(current.MenuId);
				if (current.Chilren == null)
				{
					current.Chilren = new List<MenuInfo>();
				}
			}
			return topFuwuMenus;
		}

		public static bool SaveAlarm(AlarmInfo info)
		{
			return new AlarmDao().Save(info);
		}

		public static bool DeleteAlarm(int id)
		{
			return new AlarmDao().Delete(id);
		}

		public static DbQueryResult GetAlarms(int pageIndex, int pageSize)
		{
			return new AlarmDao().List(pageIndex, pageSize);
		}

		public static bool SaveFeedBack(FeedBackInfo info)
		{
			return new FeedBackDao().Save(info);
		}

		public static FeedBackInfo GetFeedBack(int id)
		{
			return new FeedBackDao().Get(id);
		}

		public static FeedBackInfo GetFeedBack(string feedBackID)
		{
			return new FeedBackDao().Get(feedBackID);
		}

		public static bool DeleteFeedBack(int id)
		{
			return new FeedBackDao().Delete(id);
		}

		public static bool UpdateFeedBackMsgType(string feedBackId, string msgType)
		{
			return new FeedBackDao().UpdateMsgType(feedBackId, msgType);
		}

		public static DbQueryResult GetFeedBacks(int pageIndex, int pageSize, string msgType)
		{
			return new FeedBackDao().List(pageIndex, pageSize, msgType);
		}

		public static string UploadVipBGImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/Vipcard/vipbg" + Path.GetExtension(postedFile.FileName);
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static string UploadDefautBg(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetVshopSkinPath(null) + "/images/ad/DefautPageBg" + Path.GetExtension(postedFile.FileName);
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static string UploadWeiXinCodeImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/WeiXinCodeImageUrl" + Path.GetExtension(postedFile.FileName);
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static string UploadVipQRImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/Vipcard/vipqr" + Path.GetExtension(postedFile.FileName);
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static string UploadTopicImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile, "image"))
			{
				result = string.Empty;
			}
			else
			{
				string text = Globals.GetStoragePath() + "/topic/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}

		public static IList<MessageTemplate> GetAliFuWuMessageTemplates()
		{
			return new MessageTemplateHelperDao().GetAliFuWuMessageTemplates();
		}

		public static IList<MessageTemplate> GetMessageTemplates()
		{
			return new MessageTemplateHelperDao().GetMessageTemplates();
		}

		public static void UpdateSettings(IList<MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				new MessageTemplateHelperDao().UpdateSettings(templates);
				foreach (MessageTemplate current in templates)
				{
					HiCache.Remove(string.Format("Message-{0}", current.MessageType.ToLower()));
				}
			}
		}

		public static void UpdateAliFuWuSettings(IList<MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				new MessageTemplateHelperDao().UpdateAliFuWuSettings(templates);
				foreach (MessageTemplate current in templates)
				{
					HiCache.Remove(string.Format("Message-{0}", current.MessageType.ToLower()));
				}
			}
		}

		public static MessageTemplate GetMessageTemplate(string messageType)
		{
			MessageTemplate result;
			if (string.IsNullOrEmpty(messageType))
			{
				result = null;
			}
			else
			{
				result = new MessageTemplateHelperDao().GetMessageTemplate(messageType);
			}
			return result;
		}

		public static void UpdateTemplate(MessageTemplate template)
		{
			if (template != null)
			{
				new MessageTemplateHelperDao().UpdateTemplate(template);
				HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
			}
		}

		public static bool UpdateBalanceDrawRequest(int Id, string Remark)
		{
			HiCache.Remove(string.Format("DataCache-Distributor-{0}", Id));
			return new DistributorsDao().UpdateBalanceDrawRequest(Id, Remark, null);
		}

		public static bool UpdateBalanceDistributors(int UserId, decimal ReferralRequestBalance)
		{
			return new DistributorsDao().UpdateBalanceDistributors(UserId, ReferralRequestBalance);
		}

		public static DbQueryResult GetBalanceDrawRequest(BalanceDrawRequestQuery query)
		{
			return new DistributorsDao().GetBalanceDrawRequest(query, null);
		}

		public static DbQueryResult GetCommissions(CommissionsQuery query)
		{
			return new DistributorsDao().GetCommissions(query);
		}

		public static bool UpdateCommission(int UserId, decimal Commission, string CommRemark)
		{
			DistributorGradeCommissionInfo distributorGradeCommissionInfo = new DistributorGradeCommissionInfo();
			distributorGradeCommissionInfo.UserId = UserId;
			distributorGradeCommissionInfo.Commission = Commission;
			distributorGradeCommissionInfo.PubTime = DateTime.Now;
			distributorGradeCommissionInfo.OperAdmin = "system";
			distributorGradeCommissionInfo.Memo = CommRemark;
			distributorGradeCommissionInfo.OldCommissionTotal = 0m;
			distributorGradeCommissionInfo.ReferralUserId = UserId;
			distributorGradeCommissionInfo.OrderID = "A" + VShopHelper.GenerateOrderId();
			distributorGradeCommissionInfo.CommType = 5;
			return new DistributorsDao().AddCommission(distributorGradeCommissionInfo);
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

		public static DbQueryResult GetCommissionsWithStoreName(CommissionsQuery query, string subLevel = "")
		{
			return new DistributorsDao().GetCommissionsWithStoreName(query, subLevel);
		}

		public static int GetDownDistributorNum(string userid)
		{
			return new DistributorsDao().GetDownDistributorNum(userid);
		}

		public static int GetDownDistributorNumReferralOrders(string userid)
		{
			return new DistributorsDao().GetDownDistributorNumReferralOrders(userid);
		}

		public static DbQueryResult GetDistributors(DistributorsQuery query, string topUserId = null, string level = null)
		{
			return new DistributorsDao().GetDistributors(query, topUserId, level);
		}

		public static int IsExistUsers(string userIds)
		{
			return new DistributorsDao().IsExistUsers(userIds);
		}

		public static System.Data.DataTable GetDistributorsNum()
		{
			return new DistributorsDao().GetDistributorsNum();
		}

		public static System.Data.DataTable GetDistributorSaleinfo(string startTime, string endTime, int[] UserIds)
		{
			return new DistributorsDao().GetDistributorSaleinfo(startTime, endTime, UserIds);
		}

		public static DbQueryResult GetDistributorsRankings(string startTime, string endTime, int pgSize, int CurrPage)
		{
			return new DistributorsDao().GetDistributorsRankings(startTime, endTime, pgSize, CurrPage);
		}

		public static DbQueryResult GetCustomDistributorStatisticList()
		{
			return new DistributorsDao().GetCustomDistributorStatisticList();
		}

		public static bool InsertCustomDistributorStatistic(CustomDistributorStatistic custom)
		{
			return new DistributorsDao().InsertCustomDistributorStatistic(custom);
		}

		public static bool UpdateCustomDistributorStatistic(CustomDistributorStatistic custom)
		{
			return new DistributorsDao().UpdateCustomDistributorStatistic(custom);
		}

		public static System.Data.DataTable GetCustomDistributorStatistic(int id)
		{
			return new DistributorsDao().GetCustomDistributorStatistic(id);
		}

		public static System.Data.DataTable GetCustomDistributorStatistic(string storeName)
		{
			return new DistributorsDao().GetCustomDistributorStatistic(storeName);
		}

		public static bool DeleteCustomDistributorStatistic(string id)
		{
			return new DistributorsDao().DeleteCustomDistributorStatistic(id);
		}

		public static DbQueryResult GetSubDistributorsRankingsN(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
		{
			return new DistributorsDao().GetSubDistributorsRankingsN(startTime, endTime, pgSize, CurrPage, belongUserId, grade);
		}

		public static DbQueryResult GetSubDistributorsContribute(string startTime, string endTime, int pgSize, int CurrPage, int belongUserId, int grade)
		{
			return new DistributorsDao().GetSubDistributorsContribute(startTime, endTime, pgSize, CurrPage, belongUserId, grade);
		}

		public static System.Data.DataTable GetDistributorsSubStoreNum(int topUserId)
		{
			return new DistributorsDao().GetDistributorsSubStoreNum(topUserId);
		}

		public static int GetDistributorsSubStoreNumN(int topUserId, int grade, string startTime, string endTime)
		{
			return new DistributorsDao().GetDistributorsSubStoreNumN(topUserId, grade, startTime, endTime);
		}

		public static DistributorsInfo GetUserIdDistributors(int userid)
		{
			return new DistributorsDao().GetDistributorInfo(userid);
		}

		public static DbQueryResult GetActivitiesList(ActivitiesQuery query)
		{
			return new ActivitiesDao().GetActivitiesList(query);
		}

		public static int AddActivities(ActivitiesInfo activity)
		{
			return new ActivitiesDao().AddActivities(activity);
		}

		public static System.Data.DataTable GetType(int Types)
		{
			return new ActivitiesDao().GetType(Types);
		}

		public static bool UpdateActivities(ActivitiesInfo activity)
		{
			return new ActivitiesDao().UpdateActivities(activity);
		}

		public static bool DeleteActivities(int ActivitiesId)
		{
			return new ActivitiesDao().DeleteActivities(ActivitiesId);
		}

		public static IList<ActivitiesInfo> GetActivitiesInfo(string ActivitiesId)
		{
			return new ActivitiesDao().GetActivitiesInfo(ActivitiesId);
		}

		public static IList<DistributorGradeInfo> GetDistributorGradeInfos()
		{
			return new DistributorGradeDao().GetDistributorGradeInfos();
		}

		public static string GetCommissionPayType(string payType)
		{
			string result = "未定义";
			if (payType != null)
			{
				if (!(payType == "0"))
				{
					if (!(payType == "1"))
					{
						if (!(payType == "2"))
						{
							if (payType == "3")
							{
								result = "微信红包";
							}
						}
						else
						{
							result = "线下转帐";
						}
					}
					else
					{
						result = "支付宝";
					}
				}
				else
				{
					result = "微信钱包";
				}
			}
			return result;
		}

		public static string GetCommissionPayStatus(string ischeck)
		{
			string result = "未定义";
			if (ischeck != null)
			{
				if (!(ischeck == "0"))
				{
					if (!(ischeck == "1"))
					{
						if (ischeck == "2")
						{
							result = "已支付";
						}
					}
					else
					{
						result = "已审核";
					}
				}
				else
				{
					result = "未审核";
				}
			}
			return result;
		}

		public static System.Data.DataTable GetAdminUserMsgList()
		{
			return new MessageTemplateHelperDao().GetAdminUserMsgList(0);
		}

		public static System.Data.DataTable GetAdminUserMsgList(int userType)
		{
			return new MessageTemplateHelperDao().GetAdminUserMsgList(userType);
		}

		public static bool SaveAdminUserMsgList(bool IsInsert, MsgList myList, string OldUserOpenIdIfUpdate, out string RetInfo)
		{
			return new MessageTemplateHelperDao().SaveAdminUserMsgList(IsInsert, myList, OldUserOpenIdIfUpdate, out RetInfo);
		}

		public static bool DeleteAdminUserMsgList(MsgList myList, out string RetInfo)
		{
			return new MessageTemplateHelperDao().DeleteAdminUserMsgList(myList, out RetInfo);
		}

		public static System.Data.DataTable GetAdminUserMsgDetail(bool IsDistributor)
		{
			return new MessageTemplateHelperDao().GetAdminUserMsgDetail(IsDistributor);
		}

		public static void UpdateWeiXinMsgDetail(bool IsDistributor, IList<MsgDetail> templates)
		{
			new MessageTemplateHelperDao().UpdateWeiXinMsgDetail(IsDistributor, templates);
		}
	}
}
