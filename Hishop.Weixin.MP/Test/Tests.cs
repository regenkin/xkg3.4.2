using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Domain.Menu;
using System;
using System.Web.Script.Serialization;

namespace Hishop.Weixin.MP.Test
{
	public class Tests
	{
		private const string AppID = "wxe7322013e6e964b8";

		private const string AppSecret = "9e4e5617c1b543e3164befd1952716b0";

		public string GetToken()
		{
			string token = TokenApi.GetToken("wxe7322013e6e964b8", "9e4e5617c1b543e3164befd1952716b0");
			return new JavaScriptSerializer().Deserialize<Token>(token).access_token;
		}

		public string CreateMenu()
		{
			string token = this.GetToken();
			string menuJson = this.GetMenuJson();
			return MenuApi.CreateMenus(token, menuJson);
		}

		public string GetMenu()
		{
			string token = this.GetToken();
			return MenuApi.GetMenus(token);
		}

		public string DeleteMenu()
		{
			string token = this.GetToken();
			return MenuApi.DeleteMenus(token);
		}

		public string GetMenuJson()
		{
			Menu menu = new Menu();
			SingleClickButton item = new SingleClickButton
			{
				name = "热卖商品",
				key = "123"
			};
			SingleClickButton item2 = new SingleClickButton
			{
				name = "推荐商品",
				key = "SINGER"
			};
			SingleViewButton item3 = new SingleViewButton
			{
				name = "会员卡",
				url = "www.baidu.com"
			};
			SingleViewButton item4 = new SingleViewButton
			{
				name = "积分商城",
				url = "www.baidu.com"
			};
			SubMenu subMenu = new SubMenu
			{
				name = "个人中心"
			};
			subMenu.sub_button.Add(item3);
			subMenu.sub_button.Add(item4);
			menu.menu.button.Add(item);
			menu.menu.button.Add(item2);
			menu.menu.button.Add(subMenu);
			return new JavaScriptSerializer().Serialize(menu.menu);
		}
	}
}
