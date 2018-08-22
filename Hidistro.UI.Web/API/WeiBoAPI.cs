using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Entities.Weibo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

namespace Hidistro.UI.Web.API
{
	public class WeiBoAPI : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
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
			case "friends_timeline":
				this.friends_timeline(context);
				return;
			case "userinfo":
				this.userinfo(context);
				return;
			case "statusesupdate":
				this.statusesupdate(context);
				return;
			case "usertimeline":
				this.user_timeline(context);
				return;
			case "getfriends":
				this.getfriends(context);
				return;
			case "sendtouidmessage":
				this.SendToUIDMessage(context);
				return;
			case "commentscreate":
				this.commentscreate(context);
				return;
			case "repost":
				this.repost(context);
				return;
			case "createmenu":
				this.createmenu(context);
				return;
			case "showemenu":
				this.showemenu(context);
				return;
			case "deletemenu":
				this.deletemenu(context);
				return;
			case "sendmessage":
				this.sendmessage(context);
				break;

				return;
			}
		}

		public string SendToUIDMessage(string msgtype, string displayname, string summary, string image, string url, string Content, string ArticleId)
		{
			string result = "{\"text\": \"" + Content + "\"}";
			if (msgtype == "articles")
			{
				if (summary == "")
				{
					summary = displayname;
				}
				string text = string.Concat(new string[]
				{
					"{\"display_name\": \"",
					displayname,
					"\",\"summary\":\"",
					summary,
					"\",\"image\":\"",
					image,
					"\",\"url\":\"",
					url,
					"\"},"
				});
				System.Collections.Generic.IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(ArticleId));
				if (articleItems.Count > 0)
				{
					string text2 = "";
					foreach (ArticleItemsInfo current in articleItems)
					{
						if (current.Content.Trim() == "")
						{
							text2 = current.Title;
						}
						string text3 = text;
						text = string.Concat(new string[]
						{
							text3,
							"{\"display_name\": \"",
							current.Title,
							"\",\"summary\":\"",
							text2,
							"\",\"image\":\"http://",
							Globals.DomainName,
							current.ImageUrl,
							"\",\"url\":\"",
							current.Url,
							"\"},"
						});
					}
				}
				text = text.Substring(0, text.Length - 1);
				result = "{\"articles\": [" + text + "]}";
			}
			return result;
		}

		public void sendmessage(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string type = context.Request["msgtype"];
			string receiver_id = context.Request["SenderId"];
			string s = context.Request["MessageId"];
			string msgtype = context.Request["msgtype"];
			string text = context.Request["displayname"];
			string summary = context.Request["summary"];
			string image = context.Request["image"];
			string url = context.Request["url"];
			string text2 = context.Request["Content"];
			string text3 = context.Request["ArticleId"];
			string data = this.SendToUIDMessage(msgtype, text, summary, image, url, text2, text3);
			string text4 = weiBo.sendmessage(type, receiver_id, data);
			JObject jObject = JObject.Parse(text4);
			if (jObject["result"] != null && jObject["result"].ToString() == "true")
			{
				MessageInfo messageInfo = new MessageInfo();
				if (!string.IsNullOrEmpty(text3))
				{
					messageInfo.ArticleId = int.Parse(text3);
				}
				messageInfo.DisplayName = text;
				messageInfo.Summary = summary;
				messageInfo.SenderMessage = text2;
				messageInfo.Url = url;
				messageInfo.Image = image;
				messageInfo.SenderDate = System.DateTime.Now;
				messageInfo.MessageId = int.Parse(s);
				messageInfo.Status = 1;
				WeiboHelper.UpdateMessage(messageInfo);
			}
			context.Response.Write(text4);
		}

		public void repost(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.repost(context.Request["id"], context.Request["comment"]);
			context.Response.Write(s);
		}

		public void commentscreate(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.commentscreate(context.Request["id"], context.Request["comment"]);
			context.Response.Write(s);
		}

		public void friends_timeline(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.friends_timeline(int.Parse(context.Request["page"]));
			context.Response.Write(s);
		}

		public void getfriends(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.getfriends();
			context.Response.Write(s);
		}

		public void SendToUIDMessage(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string msgtype = context.Request["msgtype"];
			string displayname = context.Request["displayname"];
			string summary = context.Request["summary"];
			string image = context.Request["image"];
			string url = context.Request["url"];
			string content = context.Request["Content"];
			string articleId = context.Request["ArticleId"];
			string s = weiBo.SendToUIDMessage(msgtype, displayname, summary, image, url, content, articleId);
			context.Response.Write(s);
		}

		public void user_timeline(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.user_timeline(int.Parse(context.Request["page"]));
			context.Response.Write(s);
		}

		public void userinfo(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.userinfo();
			context.Response.Write(s);
		}

		public void statusesupdate(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = "";
			if (!string.IsNullOrEmpty(context.Request["status"]))
			{
				s = weiBo.statusesupdate(context.Request["status"].ToString().Replace("alert", "Alert"), context.Request["img"]);
			}
			context.Response.Write(s);
		}

		public void createmenu(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = "";
			if (!string.IsNullOrEmpty(context.Request["comment"]))
			{
				s = weiBo.createmenu(context.Request["comment"]);
			}
			context.Response.Write(s);
		}

		public void showemenu(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.showemenu();
			context.Response.Write(s);
		}

		public void deletemenu(System.Web.HttpContext context)
		{
			WeiBo weiBo = new WeiBo();
			string s = weiBo.deletemenu();
			context.Response.Write(s);
		}
	}
}
