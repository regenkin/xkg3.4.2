using System;
using System.Globalization;
using System.Xml;

namespace Hidistro.Core.Entities
{
	public class SiteSettings
	{
		public string Main_PayKey
		{
			get;
			set;
		}

		public string Main_Mch_ID
		{
			get;
			set;
		}

		public string Main_AppId
		{
			get;
			set;
		}

		public bool EnableSP
		{
			get;
			set;
		}

		public string ShopMenuStyle
		{
			get;
			set;
		}

		public bool GoodsCheck
		{
			get;
			set;
		}

		public bool GoodsType
		{
			get;
			set;
		}

		public bool MemberDefault
		{
			get;
			set;
		}

		public bool ShopDefault
		{
			get;
			set;
		}

		public bool ActivityMenu
		{
			get;
			set;
		}

		public bool DistributorsMenu
		{
			get;
			set;
		}

		public bool GoodsListMenu
		{
			get;
			set;
		}

		public bool BrandMenu
		{
			get;
			set;
		}

		public bool EnableShopMenu
		{
			get;
			set;
		}

		public bool ByRemind
		{
			get;
			set;
		}

		public bool SubscribeReply
		{
			get;
			set;
		}

		public bool CustomReply
		{
			get;
			set;
		}

		public bool EnableSaleService
		{
			get;
			set;
		}

		public bool OpenManyService
		{
			get;
			set;
		}

		public string SiteUrl
		{
			get;
			set;
		}

		public string Theme
		{
			get;
			set;
		}

		public string ServiceMeiQia
		{
			get;
			set;
		}

		public string VTheme
		{
			get;
			set;
		}

		public string CheckCode
		{
			get;
			set;
		}

		public string ShopTel
		{
			get;
			set;
		}

		public string LogoUrl
		{
			get;
			set;
		}

		public string SiteName
		{
			get;
			set;
		}

		public string Footer
		{
			get;
			set;
		}

		public string RegisterAgreement
		{
			get;
			set;
		}

		public string EmailSender
		{
			get;
			set;
		}

		public string EmailSettings
		{
			get;
			set;
		}

		public bool EmailEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.EmailSender) && !string.IsNullOrEmpty(this.EmailSettings) && this.EmailSender.Trim().Length > 0 && this.EmailSettings.Trim().Length > 0;
			}
		}

		public string SMSSender
		{
			get;
			set;
		}

		public string SMSSettings
		{
			get;
			set;
		}

		public bool SMSEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.SMSSender) && !string.IsNullOrEmpty(this.SMSSettings) && this.SMSSender.Trim().Length > 0 && this.SMSSettings.Trim().Length > 0;
			}
		}

		public int DecimalLength
		{
			get;
			set;
		}

		public string YourPriceName
		{
			get;
			set;
		}

		public string DefaultProductImage
		{
			get;
			set;
		}

		public string DefaultProductThumbnail1
		{
			get;
			set;
		}

		public string DefaultProductThumbnail2
		{
			get;
			set;
		}

		public string DefaultProductThumbnail3
		{
			get;
			set;
		}

		public string DefaultProductThumbnail4
		{
			get;
			set;
		}

		public string DefaultProductThumbnail5
		{
			get;
			set;
		}

		public string DefaultProductThumbnail6
		{
			get;
			set;
		}

		public string DefaultProductThumbnail7
		{
			get;
			set;
		}

		public string DefaultProductThumbnail8
		{
			get;
			set;
		}

		public bool Disabled
		{
			get;
			set;
		}

		public decimal PointsRate
		{
			get;
			set;
		}

		public string ShowCopyRight
		{
			get;
			set;
		}

		public int OrderShowDays
		{
			get;
			set;
		}

		public int CloseOrderDays
		{
			get;
			set;
		}

		public int FinishOrderDays
		{
			get;
			set;
		}

		public int MaxReturnedDays
		{
			get;
			set;
		}

		public decimal TaxRate
		{
			get;
			set;
		}

		public bool EnabledCnzz
		{
			get;
			set;
		}

		public string CnzzUsername
		{
			get;
			set;
		}

		public string CnzzPassword
		{
			get;
			set;
		}

		public string WeixinAppId
		{
			get;
			set;
		}

		public string WeixinAppSecret
		{
			get;
			set;
		}

		public string WeixinToken
		{
			get;
			set;
		}

		public string WeixinPaySignKey
		{
			get;
			set;
		}

		public string WeixinPartnerID
		{
			get;
			set;
		}

		public string WeixinPartnerKey
		{
			get;
			set;
		}

		public bool IsValidationService
		{
			get;
			set;
		}

		public bool IsAutoToLogin
		{
			get;
			set;
		}

		public string WeixinNumber
		{
			get;
			set;
		}

		public bool EnableWeixinRed
		{
			get;
			set;
		}

		public string WeixinLoginUrl
		{
			get;
			set;
		}

		public string WeiXinCodeImageUrl
		{
			get;
			set;
		}

		public string VipCardBG
		{
			get;
			set;
		}

		public string VipCardLogo
		{
			get;
			set;
		}

		public string VipCardQR
		{
			get;
			set;
		}

		public string VipCardPrefix
		{
			get;
			set;
		}

		public string VipCardName
		{
			get;
			set;
		}

		public bool VipRequireName
		{
			get;
			set;
		}

		public bool VipRequireMobile
		{
			get;
			set;
		}

		public bool VipRequireQQ
		{
			get;
			set;
		}

		public bool VipRequireAdress
		{
			get;
			set;
		}

		public bool VipEnableCoupon
		{
			get;
			set;
		}

		public string VipRemark
		{
			get;
			set;
		}

		public bool EnablePodRequest
		{
			get;
			set;
		}

		public bool EnableAlipayRequest
		{
			get;
			set;
		}

		public bool EnableWeiXinRequest
		{
			get;
			set;
		}

		public bool EnableOffLineRequest
		{
			get;
			set;
		}

		public bool EnableWapShengPay
		{
			get;
			set;
		}

		public string OffLinePayContent
		{
			get;
			set;
		}

		public bool EnableCommission
		{
			get;
			set;
		}

		public string DistributorDescription
		{
			get;
			set;
		}

		public bool DistributorApplicationCondition
		{
			get;
			set;
		}

		public bool EnableDistributorApplicationCondition
		{
			get;
			set;
		}

		public string DistributorProducts
		{
			get;
			set;
		}

		public string DistributorProductsDate
		{
			get;
			set;
		}

		public string DistributorBackgroundPic
		{
			get;
			set;
		}

		public string DistributorLogoPic
		{
			get;
			set;
		}

		public string SaleService
		{
			get;
			set;
		}

		public string ShopIntroduction
		{
			get;
			set;
		}

		public string MentionNowMoney
		{
			get;
			set;
		}

		public string ApplicationDescription
		{
			get;
			set;
		}

		public bool IsRequestDistributor
		{
			get;
			set;
		}

		public int FinishedOrderMoney
		{
			get;
			set;
		}

		public int RegisterDistributorsPoints
		{
			get;
			set;
		}

		public int OrdersPoints
		{
			get;
			set;
		}

		public bool EnableGuidePageSet
		{
			get;
			set;
		}

		public string GuidePageSet
		{
			get;
			set;
		}

		public bool EnableAliPayFuwuGuidePageSet
		{
			get;
			set;
		}

		public string AliPayFuwuGuidePageSet
		{
			get;
			set;
		}

		public string ManageOpenID
		{
			get;
			set;
		}

		public string WeixinCertPath
		{
			get;
			set;
		}

		public string WeixinCertPassword
		{
			get;
			set;
		}

		public string GoodsPic
		{
			get;
			set;
		}

		public string GoodsName
		{
			get;
			set;
		}

		public string GoodsDescription
		{
			get;
			set;
		}

		public string ShopHomePic
		{
			get;
			set;
		}

		public string ShopHomeName
		{
			get;
			set;
		}

		public string ShopHomeDescription
		{
			get;
			set;
		}

		public string ShopSpreadingCodePic
		{
			get;
			set;
		}

		public string ShopSpreadingCodeName
		{
			get;
			set;
		}

		public string ShopSpreadingCodeDescription
		{
			get;
			set;
		}

		public string ChinaBank_mid
		{
			get;
			set;
		}

		public string ChinaBank_MD5
		{
			get;
			set;
		}

		public string ChinaBank_DES
		{
			get;
			set;
		}

		public bool ChinaBank_Enable
		{
			get;
			set;
		}

		public string AlipayAppid
		{
			get;
			set;
		}

		public string AliOHFollowRelayTitle
		{
			get;
			set;
		}

		public int IsAddCommission
		{
			get;
			set;
		}

		public string AddCommissionStartTime
		{
			get;
			set;
		}

		public string AddCommissionEndTime
		{
			get;
			set;
		}

		public string Alipay_mid
		{
			get;
			set;
		}

		public string Alipay_mName
		{
			get;
			set;
		}

		public string Alipay_Pid
		{
			get;
			set;
		}

		public string Alipay_Key
		{
			get;
			set;
		}

		public string OfflinePay_BankCard_Name
		{
			get;
			set;
		}

		public string OfflinePay_BankCard_CardNo
		{
			get;
			set;
		}

		public string OfflinePay_BankCard_BankName
		{
			get;
			set;
		}

		public string OfflinePay_Alipay_id
		{
			get;
			set;
		}

		public string ShenPay_mid
		{
			get;
			set;
		}

		public string ShenPay_key
		{
			get;
			set;
		}

		public string Access_Token
		{
			get;
			set;
		}

		public string App_Secret
		{
			get;
			set;
		}

		public int sign_EverDayScore
		{
			get;
			set;
		}

		public bool open_signContinuity
		{
			get;
			set;
		}

		public int sign_StraightDay
		{
			get;
			set;
		}

		public int sign_RewardScore
		{
			get;
			set;
		}

		public bool sign_score_Enable
		{
			get;
			set;
		}

		public bool shopping_score_Enable
		{
			get;
			set;
		}

		public int shopping_Score
		{
			get;
			set;
		}

		public bool shopping_reward_Enable
		{
			get;
			set;
		}

		public double shopping_reward_OrderValue
		{
			get;
			set;
		}

		public int shopping_reward_Score
		{
			get;
			set;
		}

		public bool share_score_Enable
		{
			get;
			set;
		}

		public int share_Score
		{
			get;
			set;
		}

		public int PointToCashRate
		{
			get;
			set;
		}

		public bool PonitToCash_Enable
		{
			get;
			set;
		}

		public decimal PonitToCash_MaxAmount
		{
			get;
			set;
		}

		public string DrawPayType
		{
			get;
			set;
		}

		public bool BatchAliPay
		{
			get;
			set;
		}

		public bool BatchWeixinPay
		{
			get;
			set;
		}

		public int BatchWeixinPayCheckRealName
		{
			get;
			set;
		}

		public int SignWhere
		{
			get;
			set;
		}

		public int SignPoint
		{
			get;
			set;
		}

		public int SignWherePoint
		{
			get;
			set;
		}

		public bool IsRegisterSendCoupon
		{
			get;
			set;
		}

		public int RegisterSendCouponId
		{
			get;
			set;
		}

		public DateTime? RegisterSendCouponBeginTime
		{
			get;
			set;
		}

		public DateTime? RegisterSendCouponEndTime
		{
			get;
			set;
		}

		public bool ShareAct_Enable
		{
			get;
			set;
		}

		public int ActiveDay
		{
			get;
			set;
		}

		public string MemberRoleContent
		{
			get;
			set;
		}

		public SiteSettings(string siteUrl)
		{
			this.SiteUrl = siteUrl;
			this.Theme = "default";
			this.VTheme = "default";
			this.Disabled = false;
			this.SiteName = "普方分销";
			this.LogoUrl = "/utility/pics/logo.jpg";
			this.ShopTel = "";
			this.DefaultProductImage = "/utility/pics/none.gif";
			this.DefaultProductThumbnail1 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail2 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail3 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail4 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail5 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail6 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail7 = "/utility/pics/none.gif";
			this.DefaultProductThumbnail8 = "/utility/pics/none.gif";
			this.WeiXinCodeImageUrl = "/Storage/master/WeiXinCodeImageUrl.jpg";
			this.VipCardBG = "/Storage/master/Vipcard/vipbg.png";
			this.VipCardQR = "/Storage/master/Vipcard/vipqr.jpg";
			this.VipCardPrefix = "100000";
			this.VipRequireName = true;
			this.VipRequireMobile = true;
			this.EnablePodRequest = true;
			this.CustomReply = true;
			this.SubscribeReply = true;
			this.ByRemind = true;
			this.DecimalLength = 2;
			this.PointsRate = 1m;
			this.ShowCopyRight = "";
			this.OrderShowDays = 7;
			this.CloseOrderDays = 3;
			this.FinishOrderDays = 7;
			this.MaxReturnedDays = 15;
			this.OpenManyService = false;
			this.BatchAliPay = false;
			this.BatchWeixinPay = false;
			this.BatchWeixinPayCheckRealName = 2;
			this.DrawPayType = "";
			this.AlipayAppid = "";
			this.AliOHFollowRelayTitle = "";
			this.IsAddCommission = 0;
			this.Main_PayKey = "";
			this.Main_Mch_ID = "";
			this.Main_AppId = "";
			this.EnableSP = false;
		}

		public void WriteToXml(XmlDocument doc)
		{
			XmlNode root = doc.SelectSingleNode("Settings");
			SiteSettings.SetNodeValue(doc, root, "SiteUrl", this.SiteUrl);
			SiteSettings.SetNodeValue(doc, root, "Theme", this.Theme);
			SiteSettings.SetNodeValue(doc, root, "VTheme", this.VTheme);
			SiteSettings.SetNodeValue(doc, root, "ServiceMeiQia", this.ServiceMeiQia);
			SiteSettings.SetNodeValue(doc, root, "DecimalLength", this.DecimalLength.ToString(CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "DefaultProductImage", this.DefaultProductImage);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail1", this.DefaultProductThumbnail1);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail2", this.DefaultProductThumbnail2);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail3", this.DefaultProductThumbnail3);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail4", this.DefaultProductThumbnail4);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail5", this.DefaultProductThumbnail5);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail6", this.DefaultProductThumbnail6);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail7", this.DefaultProductThumbnail7);
			SiteSettings.SetNodeValue(doc, root, "DefaultProductThumbnail8", this.DefaultProductThumbnail8);
			SiteSettings.SetNodeValue(doc, root, "App_Secret", this.App_Secret);
			SiteSettings.SetNodeValue(doc, root, "CheckCode", this.CheckCode);
			SiteSettings.SetNodeValue(doc, root, "Access_Token", this.Access_Token);
			SiteSettings.SetNodeValue(doc, root, "Disabled", this.Disabled ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "Footer", this.Footer);
			SiteSettings.SetNodeValue(doc, root, "RegisterAgreement", this.RegisterAgreement);
			SiteSettings.SetNodeValue(doc, root, "ShopTel", this.ShopTel);
			SiteSettings.SetNodeValue(doc, root, "LogoUrl", this.LogoUrl);
			SiteSettings.SetNodeValue(doc, root, "OrderShowDays", this.OrderShowDays.ToString(CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "ShowCopyRight", this.ShowCopyRight);
			SiteSettings.SetNodeValue(doc, root, "CloseOrderDays", this.CloseOrderDays.ToString(CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "FinishOrderDays", this.FinishOrderDays.ToString(CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "MaxReturnedDays", this.MaxReturnedDays.ToString(CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "TaxRate", this.TaxRate.ToString(CultureInfo.InvariantCulture));
			SiteSettings.SetNodeValue(doc, root, "PointsRate", this.PointsRate.ToString("F"));
			SiteSettings.SetNodeValue(doc, root, "SiteName", this.SiteName);
			SiteSettings.SetNodeValue(doc, root, "YourPriceName", this.YourPriceName);
			SiteSettings.SetNodeValue(doc, root, "EmailSender", this.EmailSender);
			SiteSettings.SetNodeValue(doc, root, "EmailSettings", this.EmailSettings);
			SiteSettings.SetNodeValue(doc, root, "SMSSender", this.SMSSender);
			SiteSettings.SetNodeValue(doc, root, "SMSSettings", this.SMSSettings);
			SiteSettings.SetNodeValue(doc, root, "EnabledCnzz", this.EnabledCnzz ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "CnzzUsername", this.CnzzUsername);
			SiteSettings.SetNodeValue(doc, root, "CnzzPassword", this.CnzzPassword);
			SiteSettings.SetNodeValue(doc, root, "WeixinAppId", this.WeixinAppId);
			SiteSettings.SetNodeValue(doc, root, "WeixinAppSecret", this.WeixinAppSecret);
			SiteSettings.SetNodeValue(doc, root, "WeixinPaySignKey", this.WeixinPaySignKey);
			SiteSettings.SetNodeValue(doc, root, "WeixinPartnerID", this.WeixinPartnerID);
			SiteSettings.SetNodeValue(doc, root, "WeixinPartnerKey", this.WeixinPartnerKey);
			SiteSettings.SetNodeValue(doc, root, "IsValidationService", this.IsValidationService ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "IsAutoToLogin", this.IsAutoToLogin ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "WeixinToken", this.WeixinToken);
			SiteSettings.SetNodeValue(doc, root, "WeixinNumber", this.WeixinNumber);
			SiteSettings.SetNodeValue(doc, root, "WeixinLoginUrl", this.WeixinLoginUrl);
			SiteSettings.SetNodeValue(doc, root, "WeiXinCodeImageUrl", this.WeiXinCodeImageUrl);
			SiteSettings.SetNodeValue(doc, root, "VipCardBG", this.VipCardBG);
			SiteSettings.SetNodeValue(doc, root, "VipCardLogo", this.VipCardLogo);
			SiteSettings.SetNodeValue(doc, root, "VipCardQR", this.VipCardQR);
			SiteSettings.SetNodeValue(doc, root, "VipCardPrefix", this.VipCardPrefix);
			SiteSettings.SetNodeValue(doc, root, "VipCardName", this.VipCardName);
			SiteSettings.SetNodeValue(doc, root, "VipRequireName", this.VipRequireName ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRequireMobile", this.VipRequireMobile ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "CustomReply", this.CustomReply ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableSaleService", this.EnableSaleService ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ByRemind", this.ByRemind ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ShopMenuStyle", this.ShopMenuStyle);
			SiteSettings.SetNodeValue(doc, root, "EnableShopMenu", this.EnableShopMenu ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ShopDefault", this.ShopDefault ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "MemberDefault", this.MemberDefault ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "GoodsType", this.GoodsType ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "GoodsCheck", this.GoodsCheck ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ActivityMenu", this.ActivityMenu ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "DistributorsMenu", this.DistributorsMenu ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "GoodsListMenu", this.GoodsListMenu ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "BrandMenu", this.BrandMenu ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "SubscribeReply", this.SubscribeReply ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRequireQQ", this.VipRequireQQ ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRequireAdress", this.VipRequireAdress ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipEnableCoupon", this.VipEnableCoupon ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "VipRemark", this.VipRemark);
			SiteSettings.SetNodeValue(doc, root, "EnablePodRequest", this.EnablePodRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableCommission", this.EnableCommission ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableAlipayRequest", this.EnableAlipayRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWeiXinRequest", this.EnableWeiXinRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableOffLineRequest", this.EnableOffLineRequest ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableWapShengPay", this.EnableWapShengPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "OffLinePayContent", this.OffLinePayContent);
			SiteSettings.SetNodeValue(doc, root, "DistributorDescription", this.DistributorDescription);
			SiteSettings.SetNodeValue(doc, root, "DistributorBackgroundPic", this.DistributorBackgroundPic);
			SiteSettings.SetNodeValue(doc, root, "DistributorLogoPic", this.DistributorLogoPic);
			SiteSettings.SetNodeValue(doc, root, "SaleService", this.SaleService);
			SiteSettings.SetNodeValue(doc, root, "MentionNowMoney", this.MentionNowMoney);
			SiteSettings.SetNodeValue(doc, root, "ShopIntroduction", this.ShopIntroduction);
			SiteSettings.SetNodeValue(doc, root, "ApplicationDescription", this.ApplicationDescription);
			SiteSettings.SetNodeValue(doc, root, "AliPayFuwuGuidePageSet", this.AliPayFuwuGuidePageSet);
			SiteSettings.SetNodeValue(doc, root, "EnableAliPayFuwuGuidePageSet", this.EnableAliPayFuwuGuidePageSet ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "GuidePageSet", this.GuidePageSet);
			SiteSettings.SetNodeValue(doc, root, "EnableGuidePageSet", this.EnableGuidePageSet ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ManageOpenID", this.ManageOpenID);
			SiteSettings.SetNodeValue(doc, root, "WeixinCertPath", this.WeixinCertPath);
			SiteSettings.SetNodeValue(doc, root, "WeixinCertPassword", this.WeixinCertPassword);
			SiteSettings.SetNodeValue(doc, root, "GoodsPic", this.GoodsPic);
			SiteSettings.SetNodeValue(doc, root, "GoodsName", this.GoodsName);
			SiteSettings.SetNodeValue(doc, root, "GoodsDescription", this.GoodsDescription);
			SiteSettings.SetNodeValue(doc, root, "ShopHomePic", this.ShopHomePic);
			SiteSettings.SetNodeValue(doc, root, "ShopHomeName", this.ShopHomeName);
			SiteSettings.SetNodeValue(doc, root, "ShopHomeDescription", this.ShopHomeDescription);
			SiteSettings.SetNodeValue(doc, root, "ShopSpreadingCodePic", this.ShopSpreadingCodePic);
			SiteSettings.SetNodeValue(doc, root, "ShopSpreadingCodeName", this.ShopSpreadingCodeName);
			SiteSettings.SetNodeValue(doc, root, "ShopSpreadingCodeDescription", this.ShopSpreadingCodeDescription);
			SiteSettings.SetNodeValue(doc, root, "OpenManyService", this.OpenManyService ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "IsRequestDistributor", this.IsRequestDistributor ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "FinishedOrderMoney", this.FinishedOrderMoney.ToString());
			SiteSettings.SetNodeValue(doc, root, "DistributorApplicationCondition", this.DistributorApplicationCondition ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "EnableDistributorApplicationCondition", this.EnableDistributorApplicationCondition ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "DistributorProducts", this.DistributorProducts);
			SiteSettings.SetNodeValue(doc, root, "DistributorProductsDate", this.DistributorProductsDate);
			SiteSettings.SetNodeValue(doc, root, "RegisterDistributorsPoints", this.RegisterDistributorsPoints.ToString());
			SiteSettings.SetNodeValue(doc, root, "OrdersPoints", this.OrdersPoints.ToString());
			SiteSettings.SetNodeValue(doc, root, "ChinaBank_Enable", this.ChinaBank_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "ChinaBank_DES", this.ChinaBank_DES);
			SiteSettings.SetNodeValue(doc, root, "ChinaBank_MD5", this.ChinaBank_MD5);
			SiteSettings.SetNodeValue(doc, root, "ChinaBank_mid", this.ChinaBank_mid);
			SiteSettings.SetNodeValue(doc, root, "Alipay_Key", this.Alipay_Key);
			SiteSettings.SetNodeValue(doc, root, "Alipay_mid", this.Alipay_mid);
			SiteSettings.SetNodeValue(doc, root, "Alipay_mName", this.Alipay_mName);
			SiteSettings.SetNodeValue(doc, root, "Alipay_Pid", this.Alipay_Pid);
			SiteSettings.SetNodeValue(doc, root, "OfflinePay_Alipay_id", this.OfflinePay_Alipay_id);
			SiteSettings.SetNodeValue(doc, root, "OfflinePay_BankCard_Name", this.OfflinePay_BankCard_Name);
			SiteSettings.SetNodeValue(doc, root, "OfflinePay_BankCard_BankName", this.OfflinePay_BankCard_BankName);
			SiteSettings.SetNodeValue(doc, root, "OfflinePay_BankCard_CardNo", this.OfflinePay_BankCard_CardNo);
			SiteSettings.SetNodeValue(doc, root, "ShenPay_mid", this.ShenPay_mid);
			SiteSettings.SetNodeValue(doc, root, "ShenPay_key", this.ShenPay_key);
			SiteSettings.SetNodeValue(doc, root, "EnableWeixinRed", this.EnableWeixinRed ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "MemberRoleContent", this.MemberRoleContent);
			SiteSettings.SetNodeValue(doc, root, "sign_EverDayScore", this.sign_EverDayScore.ToString());
			SiteSettings.SetNodeValue(doc, root, "sign_StraightDay", this.sign_StraightDay.ToString());
			SiteSettings.SetNodeValue(doc, root, "sign_RewardScore", this.sign_RewardScore.ToString());
			SiteSettings.SetNodeValue(doc, root, "sign_score_Enable", this.sign_score_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "open_signContinuity", this.open_signContinuity ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "shopping_score_Enable", this.shopping_score_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "shopping_reward_Enable", this.shopping_reward_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "shopping_Score", this.shopping_Score.ToString());
			SiteSettings.SetNodeValue(doc, root, "shopping_reward_OrderValue", this.shopping_reward_OrderValue.ToString("F2"));
			SiteSettings.SetNodeValue(doc, root, "shopping_reward_Score", this.shopping_reward_Score.ToString());
			SiteSettings.SetNodeValue(doc, root, "share_score_Enable", this.share_score_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "share_Score", this.share_Score.ToString());
			SiteSettings.SetNodeValue(doc, root, "PonitToCash_Enable", this.PonitToCash_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "PointToCashRate", this.PointToCashRate.ToString());
			SiteSettings.SetNodeValue(doc, root, "PonitToCash_MaxAmount", this.PonitToCash_MaxAmount.ToString("F2"));
			SiteSettings.SetNodeValue(doc, root, "DrawPayType", this.DrawPayType.ToString());
			SiteSettings.SetNodeValue(doc, root, "BatchAliPay", this.BatchAliPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "BatchWeixinPay", this.BatchWeixinPay ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "BatchWeixinPayCheckRealName", this.BatchWeixinPayCheckRealName.ToString());
			SiteSettings.SetNodeValue(doc, root, "ShareAct_Enable", this.ShareAct_Enable ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "SignWhere", this.SignWhere.ToString());
			SiteSettings.SetNodeValue(doc, root, "SignWherePoint", this.SignWherePoint.ToString());
			SiteSettings.SetNodeValue(doc, root, "SignPoint", this.SignPoint.ToString());
			SiteSettings.SetNodeValue(doc, root, "ActiveDay", this.ActiveDay.ToString());
			SiteSettings.SetNodeValue(doc, root, "AlipayAppid", this.AlipayAppid.ToString());
			SiteSettings.SetNodeValue(doc, root, "AliOHFollowRelayTitle", this.AliOHFollowRelayTitle.ToString());
			SiteSettings.SetNodeValue(doc, root, "IsAddCommission", this.IsAddCommission.ToString());
			SiteSettings.SetNodeValue(doc, root, "AddCommissionStartTime", this.AddCommissionStartTime);
			SiteSettings.SetNodeValue(doc, root, "AddCommissionEndTime", this.AddCommissionEndTime);
			SiteSettings.SetNodeValue(doc, root, "IsRegisterSendCoupon", this.IsRegisterSendCoupon ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "RegisterSendCouponId", this.RegisterSendCouponId.ToString());
			SiteSettings.SetNodeValue(doc, root, "RegisterSendCouponBeginTime", this.RegisterSendCouponBeginTime.HasValue ? this.RegisterSendCouponBeginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
			SiteSettings.SetNodeValue(doc, root, "RegisterSendCouponEndTime", this.RegisterSendCouponEndTime.HasValue ? this.RegisterSendCouponEndTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : "");
			SiteSettings.SetNodeValue(doc, root, "EnableSP", this.EnableSP ? "true" : "false");
			SiteSettings.SetNodeValue(doc, root, "Main_Mch_ID", this.Main_Mch_ID);
			SiteSettings.SetNodeValue(doc, root, "Main_PayKey", this.Main_PayKey);
			SiteSettings.SetNodeValue(doc, root, "Main_AppId", this.Main_AppId);
		}

		public static SiteSettings FromXml(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("Settings");
			SiteSettings siteSettings = new SiteSettings(xmlNode.SelectSingleNode("SiteUrl").InnerText)
			{
				Theme = xmlNode.SelectSingleNode("Theme").InnerText,
				VTheme = xmlNode.SelectSingleNode("VTheme").InnerText,
				ServiceMeiQia = xmlNode.SelectSingleNode("ServiceMeiQia").InnerText,
				DecimalLength = int.Parse(xmlNode.SelectSingleNode("DecimalLength").InnerText),
				DefaultProductImage = xmlNode.SelectSingleNode("DefaultProductImage").InnerText,
				DefaultProductThumbnail1 = xmlNode.SelectSingleNode("DefaultProductThumbnail1").InnerText,
				DefaultProductThumbnail2 = xmlNode.SelectSingleNode("DefaultProductThumbnail2").InnerText,
				DefaultProductThumbnail3 = xmlNode.SelectSingleNode("DefaultProductThumbnail3").InnerText,
				DefaultProductThumbnail4 = xmlNode.SelectSingleNode("DefaultProductThumbnail4").InnerText,
				DefaultProductThumbnail5 = xmlNode.SelectSingleNode("DefaultProductThumbnail5").InnerText,
				DefaultProductThumbnail6 = xmlNode.SelectSingleNode("DefaultProductThumbnail6").InnerText,
				DefaultProductThumbnail7 = xmlNode.SelectSingleNode("DefaultProductThumbnail7").InnerText,
				DefaultProductThumbnail8 = xmlNode.SelectSingleNode("DefaultProductThumbnail8").InnerText,
				CheckCode = xmlNode.SelectSingleNode("CheckCode").InnerText,
				App_Secret = xmlNode.SelectSingleNode("App_Secret").InnerText,
				Access_Token = xmlNode.SelectSingleNode("Access_Token").InnerText,
				Disabled = bool.Parse(xmlNode.SelectSingleNode("Disabled").InnerText),
				Footer = xmlNode.SelectSingleNode("Footer").InnerText,
				RegisterAgreement = xmlNode.SelectSingleNode("RegisterAgreement").InnerText,
				LogoUrl = xmlNode.SelectSingleNode("LogoUrl").InnerText,
				ShopTel = xmlNode.SelectSingleNode("ShopTel").InnerText,
				ShowCopyRight = xmlNode.SelectSingleNode("ShowCopyRight").InnerText,
				OrderShowDays = int.Parse(xmlNode.SelectSingleNode("OrderShowDays").InnerText),
				CloseOrderDays = int.Parse(xmlNode.SelectSingleNode("CloseOrderDays").InnerText),
				FinishOrderDays = int.Parse(xmlNode.SelectSingleNode("FinishOrderDays").InnerText),
				MaxReturnedDays = int.Parse(xmlNode.SelectSingleNode("MaxReturnedDays").InnerText),
				TaxRate = decimal.Parse(xmlNode.SelectSingleNode("TaxRate").InnerText),
				PointsRate = decimal.Parse(xmlNode.SelectSingleNode("PointsRate").InnerText),
				SiteName = xmlNode.SelectSingleNode("SiteName").InnerText,
				SiteUrl = xmlNode.SelectSingleNode("SiteUrl").InnerText,
				YourPriceName = xmlNode.SelectSingleNode("YourPriceName").InnerText,
				EmailSender = xmlNode.SelectSingleNode("EmailSender").InnerText,
				EmailSettings = xmlNode.SelectSingleNode("EmailSettings").InnerText,
				SMSSender = xmlNode.SelectSingleNode("SMSSender").InnerText,
				SMSSettings = xmlNode.SelectSingleNode("SMSSettings").InnerText,
				EnabledCnzz = bool.Parse(xmlNode.SelectSingleNode("EnabledCnzz").InnerText),
				CnzzUsername = xmlNode.SelectSingleNode("CnzzUsername").InnerText,
				CnzzPassword = xmlNode.SelectSingleNode("CnzzPassword").InnerText,
				WeixinAppId = xmlNode.SelectSingleNode("WeixinAppId").InnerText,
				WeixinAppSecret = xmlNode.SelectSingleNode("WeixinAppSecret").InnerText,
				WeixinPaySignKey = xmlNode.SelectSingleNode("WeixinPaySignKey").InnerText,
				WeixinPartnerID = xmlNode.SelectSingleNode("WeixinPartnerID").InnerText,
				WeixinPartnerKey = xmlNode.SelectSingleNode("WeixinPartnerKey").InnerText,
				IsValidationService = bool.Parse(xmlNode.SelectSingleNode("IsValidationService").InnerText),
				IsAutoToLogin = bool.Parse(xmlNode.SelectSingleNode("IsAutoToLogin").InnerText),
				WeixinToken = xmlNode.SelectSingleNode("WeixinToken").InnerText,
				WeixinNumber = xmlNode.SelectSingleNode("WeixinNumber").InnerText,
				WeixinLoginUrl = xmlNode.SelectSingleNode("WeixinLoginUrl").InnerText,
				WeiXinCodeImageUrl = xmlNode.SelectSingleNode("WeiXinCodeImageUrl").InnerText,
				VipCardLogo = xmlNode.SelectSingleNode("VipCardLogo").InnerText,
				VipCardBG = xmlNode.SelectSingleNode("VipCardBG").InnerText,
				VipCardQR = xmlNode.SelectSingleNode("VipCardQR").InnerText,
				VipCardName = xmlNode.SelectSingleNode("VipCardName").InnerText,
				VipCardPrefix = xmlNode.SelectSingleNode("VipCardPrefix").InnerText,
				VipRequireName = bool.Parse(xmlNode.SelectSingleNode("VipRequireName").InnerText),
				VipRequireMobile = bool.Parse(xmlNode.SelectSingleNode("VipRequireMobile").InnerText),
				CustomReply = bool.Parse(xmlNode.SelectSingleNode("CustomReply").InnerText),
				EnableSaleService = bool.Parse(xmlNode.SelectSingleNode("EnableSaleService").InnerText),
				ByRemind = bool.Parse(xmlNode.SelectSingleNode("ByRemind").InnerText),
				EnableShopMenu = bool.Parse(xmlNode.SelectSingleNode("EnableShopMenu").InnerText),
				ShopDefault = bool.Parse(xmlNode.SelectSingleNode("ShopDefault").InnerText),
				ActivityMenu = bool.Parse(xmlNode.SelectSingleNode("ActivityMenu").InnerText),
				DistributorsMenu = bool.Parse(xmlNode.SelectSingleNode("DistributorsMenu").InnerText),
				GoodsListMenu = bool.Parse(xmlNode.SelectSingleNode("GoodsListMenu").InnerText),
				BrandMenu = bool.Parse(xmlNode.SelectSingleNode("BrandMenu").InnerText),
				MemberDefault = bool.Parse(xmlNode.SelectSingleNode("MemberDefault").InnerText),
				GoodsType = bool.Parse(xmlNode.SelectSingleNode("GoodsType").InnerText),
				GoodsCheck = bool.Parse(xmlNode.SelectSingleNode("GoodsCheck").InnerText),
				ShopMenuStyle = xmlNode.SelectSingleNode("ShopMenuStyle").InnerText,
				SubscribeReply = bool.Parse(xmlNode.SelectSingleNode("SubscribeReply").InnerText),
				VipRequireAdress = bool.Parse(xmlNode.SelectSingleNode("VipRequireAdress").InnerText),
				VipRequireQQ = bool.Parse(xmlNode.SelectSingleNode("VipRequireQQ").InnerText),
				VipEnableCoupon = bool.Parse(xmlNode.SelectSingleNode("VipEnableCoupon").InnerText),
				VipRemark = xmlNode.SelectSingleNode("VipRemark").InnerText,
				EnablePodRequest = bool.Parse(xmlNode.SelectSingleNode("EnablePodRequest").InnerText),
				EnableCommission = bool.Parse(xmlNode.SelectSingleNode("EnableCommission").InnerText),
				EnableAlipayRequest = bool.Parse(xmlNode.SelectSingleNode("EnableAlipayRequest").InnerText),
				EnableWeiXinRequest = bool.Parse(xmlNode.SelectSingleNode("EnableWeiXinRequest").InnerText),
				EnableOffLineRequest = bool.Parse(xmlNode.SelectSingleNode("EnableOffLineRequest").InnerText),
				EnableWapShengPay = bool.Parse(xmlNode.SelectSingleNode("EnableWapShengPay").InnerText),
				OffLinePayContent = xmlNode.SelectSingleNode("OffLinePayContent").InnerText,
				DistributorDescription = xmlNode.SelectSingleNode("DistributorDescription").InnerText,
				DistributorBackgroundPic = xmlNode.SelectSingleNode("DistributorBackgroundPic").InnerText,
				DistributorLogoPic = xmlNode.SelectSingleNode("DistributorLogoPic").InnerText,
				SaleService = xmlNode.SelectSingleNode("SaleService").InnerText,
				MentionNowMoney = xmlNode.SelectSingleNode("MentionNowMoney").InnerText,
				ShopIntroduction = xmlNode.SelectSingleNode("ShopIntroduction").InnerText,
				ApplicationDescription = xmlNode.SelectSingleNode("ApplicationDescription").InnerText,
				EnableGuidePageSet = bool.Parse(xmlNode.SelectSingleNode("EnableGuidePageSet").InnerText),
				GuidePageSet = xmlNode.SelectSingleNode("GuidePageSet").InnerText,
				EnableAliPayFuwuGuidePageSet = bool.Parse(xmlNode.SelectSingleNode("EnableAliPayFuwuGuidePageSet").InnerText),
				AliPayFuwuGuidePageSet = xmlNode.SelectSingleNode("AliPayFuwuGuidePageSet").InnerText,
				ManageOpenID = xmlNode.SelectSingleNode("ManageOpenID").InnerText,
				WeixinCertPath = xmlNode.SelectSingleNode("WeixinCertPath").InnerText,
				WeixinCertPassword = xmlNode.SelectSingleNode("WeixinCertPassword").InnerText,
				GoodsPic = xmlNode.SelectSingleNode("GoodsPic").InnerText,
				GoodsName = xmlNode.SelectSingleNode("GoodsName").InnerText,
				GoodsDescription = xmlNode.SelectSingleNode("GoodsDescription").InnerText,
				ShopHomePic = xmlNode.SelectSingleNode("ShopHomePic").InnerText,
				ShopHomeName = xmlNode.SelectSingleNode("ShopHomeName").InnerText,
				ShopHomeDescription = xmlNode.SelectSingleNode("ShopHomeDescription").InnerText,
				ShopSpreadingCodePic = xmlNode.SelectSingleNode("ShopSpreadingCodePic").InnerText,
				ShopSpreadingCodeName = xmlNode.SelectSingleNode("ShopSpreadingCodeName").InnerText,
				ShopSpreadingCodeDescription = xmlNode.SelectSingleNode("ShopSpreadingCodeDescription").InnerText,
				OpenManyService = bool.Parse(xmlNode.SelectSingleNode("OpenManyService").InnerText),
				IsRequestDistributor = bool.Parse(xmlNode.SelectSingleNode("IsRequestDistributor").InnerText),
				DistributorApplicationCondition = bool.Parse(xmlNode.SelectSingleNode("DistributorApplicationCondition").InnerText),
				EnableDistributorApplicationCondition = bool.Parse(xmlNode.SelectSingleNode("EnableDistributorApplicationCondition").InnerText),
				DistributorProducts = xmlNode.SelectSingleNode("DistributorProducts").InnerText,
				DistributorProductsDate = xmlNode.SelectSingleNode("DistributorProductsDate").InnerText,
				FinishedOrderMoney = int.Parse(xmlNode.SelectSingleNode("FinishedOrderMoney").InnerText),
				RegisterDistributorsPoints = int.Parse(xmlNode.SelectSingleNode("RegisterDistributorsPoints").InnerText),
				OrdersPoints = int.Parse(xmlNode.SelectSingleNode("OrdersPoints").InnerText),
				ChinaBank_DES = xmlNode.SelectSingleNode("ChinaBank_DES").InnerText,
				ChinaBank_Enable = bool.Parse(xmlNode.SelectSingleNode("ChinaBank_Enable").InnerText),
				ChinaBank_MD5 = xmlNode.SelectSingleNode("ChinaBank_MD5").InnerText,
				ChinaBank_mid = xmlNode.SelectSingleNode("ChinaBank_mid").InnerText,
				Alipay_Key = xmlNode.SelectSingleNode("Alipay_Key").InnerText,
				Alipay_mid = xmlNode.SelectSingleNode("Alipay_mid").InnerText,
				Alipay_mName = xmlNode.SelectSingleNode("Alipay_mName").InnerText,
				Alipay_Pid = xmlNode.SelectSingleNode("Alipay_Pid").InnerText,
				OfflinePay_BankCard_Name = xmlNode.SelectSingleNode("OfflinePay_BankCard_Name").InnerText,
				OfflinePay_BankCard_BankName = xmlNode.SelectSingleNode("OfflinePay_BankCard_BankName").InnerText,
				OfflinePay_BankCard_CardNo = xmlNode.SelectSingleNode("OfflinePay_BankCard_CardNo").InnerText,
				OfflinePay_Alipay_id = xmlNode.SelectSingleNode("OfflinePay_Alipay_id").InnerText,
				ShenPay_mid = xmlNode.SelectSingleNode("ShenPay_mid").InnerText,
				ShenPay_key = xmlNode.SelectSingleNode("ShenPay_key").InnerText,
				EnableWeixinRed = bool.Parse(xmlNode.SelectSingleNode("EnableWeixinRed").InnerText),
				MemberRoleContent = xmlNode.SelectSingleNode("MemberRoleContent").InnerText,
				sign_EverDayScore = int.Parse(xmlNode.SelectSingleNode("sign_EverDayScore").InnerText),
				sign_StraightDay = int.Parse(xmlNode.SelectSingleNode("sign_StraightDay").InnerText),
				sign_RewardScore = int.Parse(xmlNode.SelectSingleNode("sign_RewardScore").InnerText),
				sign_score_Enable = bool.Parse(xmlNode.SelectSingleNode("sign_score_Enable").InnerText),
				open_signContinuity = bool.Parse(xmlNode.SelectSingleNode("open_signContinuity").InnerText),
				shopping_reward_Enable = bool.Parse(xmlNode.SelectSingleNode("shopping_reward_Enable").InnerText),
				shopping_score_Enable = bool.Parse(xmlNode.SelectSingleNode("shopping_score_Enable").InnerText),
				shopping_Score = int.Parse(xmlNode.SelectSingleNode("shopping_Score").InnerText),
				shopping_reward_Score = int.Parse(xmlNode.SelectSingleNode("shopping_reward_Score").InnerText),
				shopping_reward_OrderValue = double.Parse(xmlNode.SelectSingleNode("shopping_reward_OrderValue").InnerText),
				share_score_Enable = bool.Parse(xmlNode.SelectSingleNode("share_score_Enable").InnerText),
				share_Score = int.Parse(xmlNode.SelectSingleNode("share_Score").InnerText),
				PonitToCash_Enable = bool.Parse(xmlNode.SelectSingleNode("PonitToCash_Enable").InnerText),
				PointToCashRate = int.Parse(xmlNode.SelectSingleNode("PointToCashRate").InnerText),
				PonitToCash_MaxAmount = decimal.Parse(xmlNode.SelectSingleNode("PonitToCash_MaxAmount").InnerText),
				BatchAliPay = bool.Parse(xmlNode.SelectSingleNode("BatchAliPay").InnerText),
				BatchWeixinPay = bool.Parse(xmlNode.SelectSingleNode("BatchWeixinPay").InnerText),
				DrawPayType = xmlNode.SelectSingleNode("DrawPayType").InnerText,
				BatchWeixinPayCheckRealName = int.Parse(xmlNode.SelectSingleNode("BatchWeixinPayCheckRealName").InnerText),
				ShareAct_Enable = bool.Parse(xmlNode.SelectSingleNode("ShareAct_Enable").InnerText),
				SignWhere = int.Parse(xmlNode.SelectSingleNode("SignWhere").InnerText),
				SignWherePoint = int.Parse(xmlNode.SelectSingleNode("SignWherePoint").InnerText),
				SignPoint = int.Parse(xmlNode.SelectSingleNode("SignPoint").InnerText),
				ActiveDay = int.Parse(xmlNode.SelectSingleNode("ActiveDay").InnerText),
				AlipayAppid = xmlNode.SelectSingleNode("AlipayAppid").InnerText,
				AliOHFollowRelayTitle = xmlNode.SelectSingleNode("AliOHFollowRelayTitle").InnerText,
				IsAddCommission = (xmlNode.SelectSingleNode("IsAddCommission").InnerText == "1") ? 1 : 0,
				AddCommissionStartTime = xmlNode.SelectSingleNode("AddCommissionStartTime").InnerText,
				AddCommissionEndTime = xmlNode.SelectSingleNode("AddCommissionEndTime").InnerText,
				IsRegisterSendCoupon = bool.Parse(xmlNode.SelectSingleNode("IsRegisterSendCoupon").InnerText),
				RegisterSendCouponId = int.Parse(xmlNode.SelectSingleNode("RegisterSendCouponId").InnerText),
				Main_PayKey = xmlNode.SelectSingleNode("Main_PayKey ").InnerText,
				Main_Mch_ID = xmlNode.SelectSingleNode("Main_Mch_ID ").InnerText,
				Main_AppId = xmlNode.SelectSingleNode("Main_AppId ").InnerText,
				EnableSP = bool.Parse(xmlNode.SelectSingleNode("EnableSP ").InnerText)
			};
			string innerText = xmlNode.SelectSingleNode("RegisterSendCouponBeginTime").InnerText;
			if (!string.IsNullOrWhiteSpace(innerText))
			{
				siteSettings.RegisterSendCouponBeginTime = new DateTime?(DateTime.Parse(innerText));
			}
			string innerText2 = xmlNode.SelectSingleNode("RegisterSendCouponEndTime").InnerText;
			if (!string.IsNullOrWhiteSpace(innerText2))
			{
				siteSettings.RegisterSendCouponEndTime = new DateTime?(DateTime.Parse(innerText2));
			}
			return siteSettings;
		}

		private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
		{
			XmlNode xmlNode = root.SelectSingleNode(nodeName);
			if (xmlNode == null)
			{
				xmlNode = doc.CreateElement(nodeName);
				root.AppendChild(xmlNode);
			}
			xmlNode.InnerText = nodeValue;
		}
	}
}
