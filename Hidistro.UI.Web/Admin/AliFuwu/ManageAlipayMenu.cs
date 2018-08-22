using Aop.Api.Response;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliFuwu
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ManageAlipayMenu : AdminPage
	{
		protected System.Web.UI.WebControls.Button BtnSave;

		protected ManageAlipayMenu() : base("m11", "fwp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (AlipayFuwuConfig.appId.Length < 16)
			{
				this.ShowMsgAndReUrl("请先绑定服务窗", false, "AliFuwuConfig.aspx");
				return;
			}
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
		}

		private FWButton BuildMenu(MenuInfo menu)
		{
			switch (menu.BindType)
			{
			case BindType.Key:
				return new FWButton
				{
					name = menu.Name,
					actionParam = menu.MenuId.ToString(),
					actionType = "out"
				};
			case BindType.Topic:
			case BindType.HomePage:
			case BindType.ProductCategory:
			case BindType.ShoppingCar:
			case BindType.OrderCenter:
			case BindType.MemberCard:
				return new FWButton
				{
					name = menu.Name,
					actionParam = menu.Url,
					actionType = "link"
				};
			case BindType.Url:
				return new FWButton
				{
					name = menu.Name,
					actionParam = menu.Content,
					actionType = "link"
				};
			}
			return new FWButton
			{
				name = menu.Name
			};
		}

		private void BtnSave_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MenuInfo> initFuwuMenus = VShopHelper.GetInitFuwuMenus();
			FWMenu fWMenu = new FWMenu();
			fWMenu.button = new System.Collections.Generic.List<FWButton>();
			System.Collections.Generic.List<FWButton> list = fWMenu.button as System.Collections.Generic.List<FWButton>;
			foreach (MenuInfo current in initFuwuMenus)
			{
				FWButton fWButton = this.BuildMenu(current);
				if (current.Chilren != null && current.Chilren.Count > 0)
				{
					fWButton.subButton = new System.Collections.Generic.List<FWButton>();
					foreach (MenuInfo current2 in current.Chilren)
					{
						(fWButton.subButton as System.Collections.Generic.List<FWButton>).Add(this.BuildMenu(current2));
					}
				}
				list.Add(fWButton);
			}
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string alipayAppid = masterSettings.AlipayAppid;
			if (!AlipayFuwuConfig.CommSetConfig(alipayAppid, base.Server.MapPath("~/"), "GBK"))
			{
				base.Response.Write("<script>alert('您的服务窗配置信息错误，请您先检查配置！');location.href='AliFuwuConfig.aspx'</script>");
				return;
			}
			AlipayMobilePublicMenuUpdateResponse alipayMobilePublicMenuUpdateResponse = AliOHHelper.MenuUpdate(fWMenu);
			if (alipayMobilePublicMenuUpdateResponse != null && !alipayMobilePublicMenuUpdateResponse.IsError && alipayMobilePublicMenuUpdateResponse.Code == "200")
			{
				this.ShowMsg("自定义菜单已同步到支付宝服务窗！", true);
				return;
			}
			this.ShowMsg("操作失败!" + alipayMobilePublicMenuUpdateResponse.Msg, false);
		}
	}
}
