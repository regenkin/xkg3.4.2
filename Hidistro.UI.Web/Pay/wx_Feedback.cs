using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hishop.Weixin.Pay;
using Hishop.Weixin.Pay.Notify;
using System;
using System.Web.UI;

namespace Hidistro.UI.Web.Pay
{
	public class wx_Feedback : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.Response.Write("success");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			NotifyClient notifyClient;
			if (masterSettings.EnableSP)
			{
				notifyClient = new NotifyClient(masterSettings.Main_AppId, masterSettings.WeixinAppSecret, masterSettings.Main_Mch_ID, masterSettings.Main_PayKey, true, masterSettings.WeixinAppId, masterSettings.WeixinPartnerID);
			}
			else
			{
				notifyClient = new NotifyClient(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret, masterSettings.WeixinPartnerID, masterSettings.WeixinPartnerKey, false, "", "");
			}
			FeedBackNotify feedBackNotify = notifyClient.GetFeedBackNotify(base.Request.InputStream);
			if (feedBackNotify == null)
			{
				return;
			}
			string msgType;
			if ((msgType = feedBackNotify.MsgType) != null)
			{
				if (!(msgType == "request"))
				{
					if (msgType == "confirm")
					{
						feedBackNotify.MsgType = "已完成";
					}
				}
				else
				{
					feedBackNotify.MsgType = "未处理";
				}
			}
			FeedBackInfo feedBack = VShopHelper.GetFeedBack(feedBackNotify.FeedBackId);
			if (feedBack != null)
			{
				VShopHelper.UpdateFeedBackMsgType(feedBackNotify.FeedBackId, feedBackNotify.MsgType);
				return;
			}
			VShopHelper.SaveFeedBack(new FeedBackInfo
			{
				AppId = feedBackNotify.AppId,
				ExtInfo = feedBackNotify.ExtInfo,
				FeedBackId = feedBackNotify.FeedBackId,
				MsgType = feedBackNotify.MsgType,
				OpenId = feedBackNotify.OpenId,
				Reason = feedBackNotify.Reason,
				Solution = feedBackNotify.Solution,
				TransId = feedBackNotify.TransId
			});
		}
	}
}
