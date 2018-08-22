using ControlPanel.Settings;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Settings;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class MenuProcess : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
			string text = context.Request["action"];
			string key;
			switch (key = text)
			{
			case "gettopmenus":
				this.GetTopMenus(context);
				return;
			case "addmenu":
				this.AddMenus(context);
				return;
			case "editmenu":
				this.EditMenus(context);
				return;
			case "updatename":
				this.updatename(context);
				return;
			case "getmenu":
				this.GetMenu(context);
				return;
			case "delmenu":
				this.delmenu(context);
				return;
			case "setenable":
				this.setenable(context);
				break;

				return;
			}
		}

		public void setenable(System.Web.HttpContext context)
		{
			string s = "{\"status\":\"1\"}";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.EnableShopMenu = bool.Parse(context.Request["enable"]);
			SettingsManager.Save(masterSettings);
			context.Response.Write(s);
		}

		public void delmenu(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"status\":\"1\"}";
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				return;
			}
			if (MenuHelper.DeleteMenu(menuId))
			{
				s = "{\"status\":\"0\"}";
			}
			context.Response.Write(s);
		}

		public void GetMenu(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = "{";
			MenuInfo menuInfo = new MenuInfo();
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				return;
			}
			menuInfo = MenuHelper.GetMenu(menuId);
			if (menuInfo != null)
			{
				text += "\"status\":\"0\",\"data\":[";
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"{\"menuid\": \"",
					menuInfo.MenuId,
					"\","
				});
				text = text + "\"type\": \"" + menuInfo.Type + "\",";
				text = text + "\"name\": \"" + Globals.String2Json(menuInfo.Name) + "\",";
				text = text + "\"shopmenupic\": \"" + menuInfo.ShopMenuPic + "\",";
				text = text + "\"content\": \"" + Globals.String2Json(menuInfo.Content) + "\"}";
				text += "]";
			}
			text += "}";
			context.Response.Write(text);
		}

		public void EditMenus(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"status\":\"1\"}";
			MenuInfo menuInfo = new MenuInfo();
			menuInfo.Content = context.Request["Content"];
			menuInfo.Name = context.Request["Name"];
			menuInfo.Type = context.Request["Type"];
			if (!string.IsNullOrEmpty(context.Request["ParentMenuId"]))
			{
				menuInfo.ParentMenuId = int.Parse(context.Request["ParentMenuId"]);
			}
			else
			{
				menuInfo.ParentMenuId = 0;
			}
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				return;
			}
			menuInfo.MenuId = menuId;
			menuInfo.ShopMenuPic = context.Request["ShopMenuPic"];
			if (MenuHelper.UpdateMenu(menuInfo))
			{
				s = "{\"status\":\"0\"}";
			}
			context.Response.Write(s);
		}

		public void updatename(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"status\":\"1\"}";
			int num = 0;
			if (!int.TryParse(context.Request["MenuId"], out num))
			{
				return;
			}
			if (num > 0)
			{
				MenuInfo menu = MenuHelper.GetMenu(num);
				menu.Name = context.Request["Name"];
				menu.MenuId = num;
				if (MenuHelper.UpdateMenu(menu))
				{
					s = "{\"status\":\"0\"}";
				}
			}
			context.Response.Write(s);
		}

		public void AddMenus(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"status\":\"1\"}";
			MenuInfo menuInfo = new MenuInfo();
			menuInfo.Content = context.Request["Content"].Trim();
			menuInfo.Name = context.Request["Name"].Trim();
			if (context.Request["ParentMenuId"] != null)
			{
				menuInfo.ParentMenuId = ((context.Request["ParentMenuId"] == "") ? 0 : int.Parse(context.Request["ParentMenuId"]));
			}
			else
			{
				menuInfo.ParentMenuId = 0;
			}
			menuInfo.Type = context.Request["Type"];
			menuInfo.ShopMenuPic = context.Request["ShopMenuPic"];
			if (MenuHelper.CanAddMenu(menuInfo.ParentMenuId))
			{
				int num = MenuHelper.SaveMenu(menuInfo);
				if (num > 0)
				{
					s = "{\"status\":\"0\",\"menuid\":\"" + num + "\"}";
				}
			}
			else
			{
				s = "{\"status\":\"2\"}";
			}
			context.Response.Write(s);
		}

		public void GetTopMenus(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = "{";
			System.Collections.Generic.IList<MenuInfo> topMenus = MenuHelper.GetTopMenus();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (topMenus.Count <= 0)
			{
				text += "\"status\":\"-1\"";
				return;
			}
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"\"status\":\"0\",\"shopmenustyle\":\"",
				masterSettings.ShopMenuStyle,
				"\",\"enableshopmenu\":\"",
				masterSettings.EnableShopMenu,
				"\",\"data\":["
			});
			foreach (MenuInfo current in topMenus)
			{
				System.Collections.Generic.IList<MenuInfo> menusByParentId = MenuHelper.GetMenusByParentId(current.MenuId);
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"{\"menuid\": \"",
					current.MenuId,
					"\","
				});
				text += "\"childdata\":[";
				if (menusByParentId.Count > 0)
				{
					foreach (MenuInfo current2 in menusByParentId)
					{
						object obj3 = text;
						text = string.Concat(new object[]
						{
							obj3,
							"{\"menuid\": \"",
							current2.MenuId,
							"\","
						});
						object obj4 = text;
						text = string.Concat(new object[]
						{
							obj4,
							"\"parentmenuid\": \"",
							current2.ParentMenuId,
							"\","
						});
						text = text + "\"type\": \"" + current2.Type + "\",";
						text = text + "\"name\": \"" + Globals.String2Json(current2.Name) + "\",";
						text = text + "\"content\": \"" + Globals.String2Json(current2.Content) + "\"},";
					}
					text = text.Substring(0, text.Length - 1);
				}
				text += "],";
				text = text + "\"type\": \"" + current.Type + "\",";
				text = text + "\"name\": \"" + Globals.String2Json(current.Name) + "\",";
				text = text + "\"shopmenupic\": \"" + current.ShopMenuPic + "\",";
				text = text + "\"content\": \"" + Globals.String2Json(current.Content) + "\"},";
			}
			text = text.Substring(0, text.Length - 1);
			text += "]";
			text += "}";
			context.Response.Write(text);
		}
	}
}
