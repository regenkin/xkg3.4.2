using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Domain.Menu;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ManageMenu : AdminPage
	{
		protected System.Web.UI.WebControls.Button BtnSave;

		protected ManageMenu() : base("m06", "wxp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
		}

		private SingleButton BuildMenu(MenuInfo menu)
		{
			switch (menu.BindType)
			{
			case BindType.Key:
				return new SingleClickButton
				{
					name = menu.Name,
					key = menu.MenuId.ToString()
				};
			case BindType.Topic:
			case BindType.HomePage:
			case BindType.ProductCategory:
			case BindType.ShoppingCar:
			case BindType.OrderCenter:
			case BindType.MemberCard:
			case BindType.Url:
				return new SingleViewButton
				{
					name = menu.Name,
					url = menu.Url
				};
			}
			return new SingleClickButton
			{
				name = menu.Name,
				key = "None"
			};
		}

		private void BtnSave_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<MenuInfo> initMenus = VShopHelper.GetInitMenus();
			Hishop.Weixin.MP.Domain.Menu.Menu menu = new Hishop.Weixin.MP.Domain.Menu.Menu();
			foreach (MenuInfo current in initMenus)
			{
				if (current.Chilren == null || current.Chilren.Count == 0)
				{
					menu.menu.button.Add(this.BuildMenu(current));
				}
				else
				{
					SubMenu subMenu = new SubMenu
					{
						name = current.Name
					};
					foreach (MenuInfo current2 in current.Chilren)
					{
						subMenu.sub_button.Add(this.BuildMenu(current2));
					}
					menu.menu.button.Add(subMenu);
				}
			}
			string json = JsonConvert.SerializeObject(menu.menu);
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.IsNullOrEmpty(masterSettings.WeixinAppId) || string.IsNullOrEmpty(masterSettings.WeixinAppSecret))
			{
				base.Response.Write("<script>alert('您的服务号配置存在问题，请您先检查配置！');location.href='wxconfig.aspx'</script>");
				return;
			}
			string text = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
			text = JsonConvert.DeserializeObject<Token>(text).access_token;
			string text2 = MenuApi.CreateMenus(text, json);
			if (text2.Contains("\"ok\""))
			{
				this.ShowMsg("自定义菜单已同步到微信，24小时内生效！", true);
				return;
			}
			this.ShowMsg("操作失败!服务号配置信息错误或没有微信自定义菜单权限", false);
		}
	}
}
