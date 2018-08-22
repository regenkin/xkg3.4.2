using ControlPanel.Settings;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Settings;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_NavMenu : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			string userAgent = context.Request.UserAgent;
			context.Response.ContentType = "text/plain";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			System.Collections.Generic.IList<MenuInfo> allMenu = this.GetAllMenu(masterSettings.ShopMenuStyle);
			string guidePage = masterSettings.GuidePageSet;
			if (userAgent.ToLower().Contains("alipay"))
			{
				guidePage = masterSettings.AliPayFuwuGuidePageSet;
			}
			string s = JsonConvert.SerializeObject(new
			{
				status = 1,
				msg = "",
				Phone = this.GetPhone(),
				GuidePage = guidePage,
				ShopDefault = masterSettings.ShopDefault,
				MemberDefault = masterSettings.MemberDefault,
				GoodsType = masterSettings.GoodsType,
				GoodsCheck = masterSettings.GoodsCheck,
				ActivityMenu = masterSettings.ActivityMenu,
				DistributorsMenu = masterSettings.DistributorsMenu,
				GoodsListMenu = masterSettings.GoodsListMenu,
				BrandMenu = masterSettings.BrandMenu,
				ShopMenuStyle = masterSettings.ShopMenuStyle,
				menuList = allMenu
			});
			context.Response.Write(s);
		}

		public string GetPhone()
		{
			int currentDistributorId = Globals.GetCurrentDistributorId();
			if (currentDistributorId == 0)
			{
				return SettingsManager.GetMasterSettings(true).ShopTel;
			}
			MemberInfo member = MemberProcessor.GetMember(currentDistributorId, true);
			if (member != null)
			{
				return member.CellPhone;
			}
			return SettingsManager.GetMasterSettings(true).ShopTel;
		}

		public System.Collections.Generic.IList<MenuInfo> GetAllMenu(string shopMenuStyle)
		{
			System.Collections.Generic.IList<MenuInfo> list = new System.Collections.Generic.List<MenuInfo>();
			return MenuHelper.GetMenus(shopMenuStyle);
		}
	}
}
