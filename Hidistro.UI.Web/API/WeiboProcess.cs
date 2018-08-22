using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Weibo;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class WeiboProcess : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
			case "getmenu":
				this.GetMenu(context);
				return;
			case "delmenu":
				this.delmenu(context);
				return;
			case "savemenu":
				this.savemenu(context);
				return;
			case "reply":
				this.reply(context);
				return;
			case "replydel":
				this.replydel(context);
				return;
			case "editreply":
				this.editreply(context);
				return;
			case "addreply":
				this.addreply(context);
				return;
			case "editmessage":
				this.editmessage(context);
				return;
			case "setenable":
				this.setenable(context);
				return;
			case "getmessageinfo":
				this.GetMessageInfo(context);
				break;

				return;
			}
		}

		public void setenable(System.Web.HttpContext context)
		{
			string s = "{\"status\":\"1\"}";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (context.Request["type"] == "1")
			{
				masterSettings.CustomReply = bool.Parse(context.Request["enable"]);
			}
			if (context.Request["type"] == "2")
			{
				masterSettings.SubscribeReply = bool.Parse(context.Request["enable"]);
			}
			if (context.Request["type"] == "3")
			{
				masterSettings.ByRemind = bool.Parse(context.Request["enable"]);
			}
			SettingsManager.Save(masterSettings);
			context.Response.Write(s);
		}

		public void editreply(System.Web.HttpContext context)
		{
			ReplyInfo replyInfo = new ReplyInfo();
			string s = "{\"status\":\"0\"}";
			replyInfo.Id = int.Parse(context.Request["id"]);
			replyInfo.EditDate = System.DateTime.Now;
			replyInfo.ReceiverType = context.Request["ReceiverType"];
			replyInfo.Type = int.Parse(context.Request["Type"]);
			if (replyInfo.ReceiverType == "text")
			{
				replyInfo.Content = context.Request["Content"];
			}
			else
			{
				replyInfo.Url = context.Request["Url"];
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string text;
				if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
				{
					text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
				}
				else
				{
					text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[]
					{
						'|'
					})[0];
				}
				replyInfo.Image = text;
				if (string.IsNullOrEmpty(context.Request["Image"]) && string.IsNullOrEmpty(context.Request["Display_name"]) && string.IsNullOrEmpty(context.Request["Summary"]))
				{
					replyInfo.Displayname = masterSettings.SiteName;
					replyInfo.Summary = (string.IsNullOrEmpty(masterSettings.ShopIntroduction) ? masterSettings.SiteName : masterSettings.ShopIntroduction);
				}
				else
				{
					replyInfo.Image = (string.IsNullOrEmpty(context.Request["Image"]) ? text : context.Request["Image"]);
					replyInfo.Displayname = context.Request["Display_name"];
					replyInfo.Summary = context.Request["Summary"];
					replyInfo.ArticleId = int.Parse(context.Request["ArticleId"]);
				}
			}
			if (WeiboHelper.UpdateReplyInfo(replyInfo))
			{
				s = "{\"status\":\"1\"}";
			}
			context.Response.Write(s);
		}

		public void editmessage(System.Web.HttpContext context)
		{
			ReplyInfo replyInfo = new ReplyInfo();
			string text = "{\"status\":\"0\"}";
			replyInfo = WeiboHelper.GetReplyInfoMes(int.Parse(context.Request["id"].ToString()));
			if (replyInfo != null)
			{
				text = "{\"status\":\"1\",";
				text = text + "\"Content\":\"" + Globals.String2Json(replyInfo.Content) + "\",";
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"\"Type\":\"",
					replyInfo.Type,
					"\","
				});
				text = text + "\"ReceiverType\":\"" + replyInfo.ReceiverType + "\",";
				text = text + "\"Displayname\":\"" + Globals.String2Json(replyInfo.Displayname) + "\",";
				text = text + "\"Summary\":\"" + Globals.String2Json(replyInfo.Summary) + "\",";
				text = text + "\"Image\":\"" + replyInfo.Image + "\",";
				text = text + "\"Url\":\"" + replyInfo.Url + "\",";
				object obj2 = text;
				text = string.Concat(new object[]
				{
					obj2,
					"\"Article\":\"",
					replyInfo.ArticleId,
					"\","
				});
				object obj3 = text;
				text = string.Concat(new object[]
				{
					obj3,
					"\"ReplyKeyId\":\"",
					replyInfo.ReplyKeyId,
					"\""
				});
				text += "}";
			}
			context.Response.Write(text);
		}

		public void addreply(System.Web.HttpContext context)
		{
			ReplyInfo replyInfo = new ReplyInfo();
			string s = "{\"status\":\"0\"}";
			replyInfo.EditDate = System.DateTime.Now;
			replyInfo.IsDisable = true;
			replyInfo.Type = int.Parse(context.Request["Type"]);
			replyInfo.ReplyKeyId = int.Parse(context.Request["ReplyKeyId"]);
			replyInfo.ReceiverType = context.Request["ReceiverType"];
			if (replyInfo.ReceiverType == "text")
			{
				replyInfo.Content = context.Request["Content"];
			}
			else
			{
				replyInfo.Url = context.Request["Url"];
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				string text;
				if (!string.IsNullOrEmpty(masterSettings.ShopHomePic))
				{
					text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.ShopHomePic;
				}
				else
				{
					text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + masterSettings.DistributorBackgroundPic.Split(new char[]
					{
						'|'
					})[0];
				}
				replyInfo.Image = text;
				if (string.IsNullOrEmpty(context.Request["Image"]) && string.IsNullOrEmpty(context.Request["Display_name"]) && string.IsNullOrEmpty(context.Request["Summary"]))
				{
					replyInfo.Displayname = masterSettings.SiteName;
					replyInfo.Summary = (string.IsNullOrEmpty(masterSettings.ShopIntroduction) ? masterSettings.SiteName : masterSettings.ShopIntroduction);
				}
				else
				{
					replyInfo.Image = (string.IsNullOrEmpty(context.Request["Image"]) ? text : context.Request["Image"]);
					replyInfo.Displayname = context.Request["Display_name"];
					replyInfo.Summary = context.Request["Summary"];
					replyInfo.ArticleId = int.Parse(context.Request["ArticleId"]);
				}
			}
			if (WeiboHelper.SaveReplyInfo(replyInfo))
			{
				s = "{\"status\":\"1\"}";
			}
			context.Response.Write(s);
		}

		public void replydel(System.Web.HttpContext context)
		{
			string s = "{\"status\":\"0\"}";
			if (WeiboHelper.DeleteReplyInfo(int.Parse(context.Request["id"])))
			{
				s = "{\"status\":\"1\"}";
			}
			context.Response.Write(s);
		}

		public void GetMessageInfo(System.Web.HttpContext context)
		{
			string text = "{\"status\":\"0\"}";
			MessageInfo messageInfo = WeiboHelper.GetMessageInfo(int.Parse(context.Request["MessageId"]));
			if (messageInfo != null)
			{
				text = "{\"status\":\"1\",";
				text = text + "\"SenderMessage\":\"" + Globals.String2Json(messageInfo.SenderMessage) + "\",";
				text = text + "\"DisplayName\":\"" + Globals.String2Json(messageInfo.DisplayName) + "\",";
				text = text + "\"Summary\":\"" + Globals.String2Json(messageInfo.Summary) + "\",";
				text = text + "\"Image\":\"" + messageInfo.Image + "\",";
				text = text + "\"Url\":\"" + messageInfo.Url + "\",";
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"\"ArticleId\":\"",
					messageInfo.ArticleId,
					"\""
				});
				text += "}";
			}
			context.Response.Write(text);
		}

		public void reply(System.Web.HttpContext context)
		{
			ReplyKeyInfo replyKeyInfo = new ReplyKeyInfo();
			string s = "{\"status\":\"0\"}";
			replyKeyInfo.Id = int.Parse(context.Request["ID"]);
			replyKeyInfo.Matching = context.Request["Matching"];
			if (WeiboHelper.UpdateMatching(replyKeyInfo))
			{
				s = "{\"status\":\"1\"}";
			}
			context.Response.Write(s);
		}

		public void savemenu(System.Web.HttpContext context)
		{
			string text = "{";
			System.Collections.Generic.IList<MenuInfo> topMenus = WeiboHelper.GetTopMenus();
			if (topMenus.Count <= 0)
			{
				return;
			}
			text += "\"button\":[";
			foreach (MenuInfo current in topMenus)
			{
				System.Collections.Generic.IList<MenuInfo> menusByParentId = WeiboHelper.GetMenusByParentId(current.MenuId);
				text = text + "{\"name\": \"" + Globals.String2Json(current.Name) + "\",";
				if (menusByParentId.Count > 0)
				{
					text += "\"sub_button\":[";
					foreach (MenuInfo current2 in menusByParentId)
					{
						text = text + "{\"type\": \"" + current2.Type + "\",";
						text = text + "\"name\": \"" + Globals.String2Json(current2.Name) + "\",";
						if (current2.Type == "click")
						{
							text = text + "\"key\": \"" + Globals.String2Json(current2.Content) + "\"},";
						}
						else
						{
							text = text + "\"url\": \"" + Globals.String2Json(current2.Content) + "\"},";
						}
					}
					text = text.Substring(0, text.Length - 1);
					text += "]},";
				}
				else
				{
					text = text + "\"type\": \"" + current.Type + "\",";
					if (current.Type == "click")
					{
						text = text + "\"key\": \"" + Globals.String2Json(current.Content) + "\"},";
					}
					else
					{
						text = text + "\"url\": \"" + Globals.String2Json(current.Content) + "\"},";
					}
				}
			}
			text = text.Substring(0, text.Length - 1);
			text += "]";
			text += "}";
			WeiBo weiBo = new WeiBo();
			text = weiBo.createmenu(text);
			context.Response.Write(text);
		}

		public void delmenu(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string s = "{\"status\":\"1\"}";
			int num = 0;
			if (!int.TryParse(context.Request["MenuId"], out num))
			{
				return;
			}
			System.Collections.Generic.IList<MenuInfo> menusByParentId = WeiboHelper.GetMenusByParentId(num);
			if (menusByParentId.Count > 0)
			{
				s = "{\"status\":\"2\"}";
			}
			else if (WeiboHelper.DeleteMenu(num))
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
			menuInfo = WeiboHelper.GetMenu(menuId);
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
			if (WeiboHelper.UpdateMenu(menuInfo))
			{
				s = "{\"status\":\"0\"}";
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
			if (WeiboHelper.CanAddMenu(menuInfo.ParentMenuId))
			{
				if (WeiboHelper.SaveMenu(menuInfo))
				{
					s = "{\"status\":\"0\"}";
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
			System.Collections.Generic.IList<MenuInfo> topMenus = WeiboHelper.GetTopMenus();
			if (topMenus.Count <= 0)
			{
				text += "\"status\":\"-1\"";
				return;
			}
			text += "\"status\":\"0\",\"data\":[";
			foreach (MenuInfo current in topMenus)
			{
				System.Collections.Generic.IList<MenuInfo> menusByParentId = WeiboHelper.GetMenusByParentId(current.MenuId);
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"{\"menuid\": \"",
					current.MenuId,
					"\","
				});
				text += "\"childdata\":[";
				if (menusByParentId.Count > 0)
				{
					foreach (MenuInfo current2 in menusByParentId)
					{
						object obj2 = text;
						text = string.Concat(new object[]
						{
							obj2,
							"{\"menuid\": \"",
							current2.MenuId,
							"\","
						});
						object obj3 = text;
						text = string.Concat(new object[]
						{
							obj3,
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
				text = text + "\"content\": \"" + Globals.String2Json(current.Content) + "\"},";
			}
			text = text.Substring(0, text.Length - 1);
			text += "]";
			text += "}";
			context.Response.Write(text);
		}
	}
}
