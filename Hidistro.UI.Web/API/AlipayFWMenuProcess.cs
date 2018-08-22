using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class AlipayFWMenuProcess : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
				break;

				return;
			}
		}

		public void GetTopMenus(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = "{";
			System.Collections.Generic.IList<MenuInfo> topFuwuMenus = VShopHelper.GetTopFuwuMenus();
			if (topFuwuMenus.Count <= 0)
			{
				text += "\"status\":\"-1\"";
				return;
			}
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				"\"status\":\"0\",\"shopmenustyle\":\"0\",\"enableshopmenu\":\"",
				true,
				"\",\"data\":["
			});
			foreach (MenuInfo current in topFuwuMenus)
			{
				System.Collections.Generic.IList<MenuInfo> fuwuMenusByParentId = VShopHelper.GetFuwuMenusByParentId(current.MenuId);
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"{\"menuid\": \"",
					current.MenuId,
					"\","
				});
				text += "\"childdata\":[";
				if (fuwuMenusByParentId.Count > 0)
				{
					foreach (MenuInfo current2 in fuwuMenusByParentId)
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
						text = text + "\"name\": \"" + current2.Name + "\",";
						text = text + "\"content\": \"" + current2.Content + "\"},";
					}
					text = text.Substring(0, text.Length - 1);
				}
				text += "],";
				text = text + "\"type\": \"" + current.Type + "\",";
				text = text + "\"name\": \"" + current.Name + "\",";
				text += "\"shopmenupic\": \"\",";
				text = text + "\"content\": \"" + current.Content + "\"},";
			}
			text = text.Substring(0, text.Length - 1);
			text += "]";
			text += "}";
			context.Response.Write(text);
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
			if (VShopHelper.DeleteFuwuMenu(menuId))
			{
				s = "{\"status\":\"0\"}";
			}
			context.Response.Write(s);
		}

		public void GetMenu(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = "{";
			int menuId = 0;
			if (!int.TryParse(context.Request["MenuId"], out menuId))
			{
				return;
			}
			MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(menuId);
			if (fuwuMenu != null)
			{
				text += "\"status\":\"0\",\"data\":[";
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"{\"menuid\": \"",
					fuwuMenu.MenuId,
					"\","
				});
				text = text + "\"type\": \"" + fuwuMenu.Type + "\",";
				text = text + "\"name\": \"" + fuwuMenu.Name + "\",";
				text += "\"shopmenupic\": \"\",";
				text = text + "\"content\": \"" + fuwuMenu.Content + "\"}";
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
			menuInfo.Bind = 8;
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
			if (VShopHelper.UpdateFuwuMenu(menuInfo))
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
				MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(num);
				fuwuMenu.MenuId = num;
				fuwuMenu.Name = context.Request["Name"];
				if (VShopHelper.UpdateFuwuMenu(fuwuMenu))
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
			menuInfo.Bind = 8;
			if (context.Request["ParentMenuId"] != null)
			{
				menuInfo.ParentMenuId = ((context.Request["ParentMenuId"] == "") ? 0 : int.Parse(context.Request["ParentMenuId"]));
			}
			else
			{
				menuInfo.ParentMenuId = 0;
			}
			menuInfo.Type = context.Request["Type"];
			if (VShopHelper.CanAddFuwuMenu(menuInfo.ParentMenuId))
			{
				if (VShopHelper.SaveFuwuMenu(menuInfo))
				{
					if (menuInfo.ParentMenuId > 0)
					{
						MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(menuInfo.ParentMenuId);
						if (fuwuMenu != null)
						{
							fuwuMenu.Bind = 0;
							fuwuMenu.Content = "";
							VShopHelper.UpdateFuwuMenu(fuwuMenu);
						}
					}
					s = "{\"status\":\"0\"}";
				}
			}
			else
			{
				s = "{\"status\":\"2\"}";
			}
			context.Response.Write(s);
		}
	}
}
