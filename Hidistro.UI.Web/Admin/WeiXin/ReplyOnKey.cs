using ControlPanel.WeiBo;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ReplyOnKey : AdminPage
	{
		protected System.Web.UI.WebControls.Repeater rptList;

		protected ReplyOnKey() : base("m06", "wxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindArticleCategory();
			}
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			int num = Globals.ToNum(e.CommandArgument.ToString());
			if (e.CommandName == "Delete")
			{
				ReplyHelper.DeleteReply(num);
				this.BindArticleCategory();
				this.ShowMsg("删除成功！", true);
				return;
			}
			if (e.CommandName == "Release")
			{
				ReplyHelper.UpdateReplyRelease(num);
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
				return;
			}
			if (e.CommandName == "Edit")
			{
				Hidistro.Entities.VShop.ReplyInfo reply = ReplyHelper.GetReply(num);
				if (reply != null)
				{
					switch (reply.MessageType)
					{
					case MessageType.Text:
						base.Response.Redirect(string.Format("replyedit.aspx?id={0}", num));
						break;
					case MessageType.News:
						base.Response.Redirect(string.Format("replyedit.aspx?id={0}", num));
						return;
					case (MessageType)3:
						break;
					case MessageType.List:
						base.Response.Redirect(string.Format("replyedit.aspx?id={0}", num));
						return;
					default:
						return;
					}
				}
			}
		}

		private void BindArticleCategory()
		{
			System.Collections.Generic.List<Hidistro.Entities.VShop.ReplyInfo> dataSource = ReplyHelper.GetAllReply().ToList<Hidistro.Entities.VShop.ReplyInfo>().FindAll((Hidistro.Entities.VShop.ReplyInfo a) => a.ReplyType < ReplyType.Wheel);
			this.rptList.DataSource = dataSource;
			this.rptList.DataBind();
		}

		protected string GetTitleShow(object messagetypename, object articleid, object responseid)
		{
			string text = string.Empty;
			int num = Globals.ToNum(articleid);
			int id = Globals.ToNum(responseid);
			string a;
			if ((a = messagetypename.ToString()) != null)
			{
				if (!(a == "多图文"))
				{
					if (!(a == "单图文"))
					{
						if (a == "文本")
						{
							TextReplyInfo textReplyInfo = ReplyHelper.GetReply(id) as TextReplyInfo;
							if (textReplyInfo != null)
							{
								text = textReplyInfo.Text;
								text = System.Text.RegularExpressions.Regex.Replace(text, "<[^>]+>", "");
								text = System.Text.RegularExpressions.Regex.Replace(text, "&[^;]+;", "");
								text = Globals.SubStr(text, 100, "...");
								if (string.IsNullOrEmpty(text) && textReplyInfo.Text.Contains("<img "))
								{
									text = "<span style='color:green;'>图文内容</span>";
								}
							}
						}
					}
					else if (num > 0)
					{
						ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(num);
						if (articleInfo != null)
						{
							int num2 = 1;
							text = string.Concat(new object[]
							{
								"<p>[<span style='color:green;'>图文",
								num2,
								"</span>] ",
								Globals.SubStr(articleInfo.Title, 40, "..."),
								"</p>"
							});
						}
					}
					else
					{
						NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(id) as NewsReplyInfo;
						if (newsReplyInfo != null)
						{
							System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
							if (newsReplyInfo.NewsMsg != null && newsReplyInfo.NewsMsg.Count > 0)
							{
								int num3 = 0;
								foreach (NewsMsgInfo current in newsReplyInfo.NewsMsg)
								{
									num3++;
									stringBuilder.Append(string.Concat(new object[]
									{
										"<p>[<span style='color:green;'>图文",
										num3,
										"</span>] ",
										Globals.SubStr(current.Title, 40, "..."),
										"</p>"
									}));
								}
							}
							text = stringBuilder.ToString();
						}
					}
				}
				else
				{
					if (num > 0)
					{
						ArticleInfo articleInfo2 = ArticleHelper.GetArticleInfo(num);
						if (articleInfo2 == null)
						{
							return text;
						}
						int num4 = 1;
						text = string.Concat(new object[]
						{
							"<p>[<span style='color:green;'>图文",
							num4,
							"</span>] ",
							Globals.SubStr(articleInfo2.Title, 40, "..."),
							"</p>"
						});
						if (articleInfo2.ItemsInfo == null)
						{
							return text;
						}
						using (System.Collections.Generic.IEnumerator<ArticleItemsInfo> enumerator2 = articleInfo2.ItemsInfo.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ArticleItemsInfo current2 = enumerator2.Current;
								num4++;
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									"<p>[<span style='color:green;'>图文",
									num4,
									"</span>] ",
									Globals.SubStr(current2.Title, 40, "..."),
									"</p>"
								});
							}
							return text;
						}
					}
					NewsReplyInfo newsReplyInfo2 = ReplyHelper.GetReply(id) as NewsReplyInfo;
					if (newsReplyInfo2 != null)
					{
						System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
						if (newsReplyInfo2.NewsMsg != null && newsReplyInfo2.NewsMsg.Count > 0)
						{
							int num5 = 0;
							foreach (NewsMsgInfo current3 in newsReplyInfo2.NewsMsg)
							{
								num5++;
								stringBuilder2.Append(string.Concat(new object[]
								{
									"<p>[<span style='color:green;'>图文",
									num5,
									"</span>] ",
									Globals.SubStr(current3.Title, 40, "..."),
									"</p>"
								}));
							}
						}
						text = stringBuilder2.ToString();
					}
				}
			}
			return text;
		}

		protected string GetReplyTypeName(object obj)
		{
			ReplyType replyType = (ReplyType)obj;
			string text = string.Empty;
			bool flag = false;
			if (ReplyType.Subscribe == (replyType & ReplyType.Subscribe))
			{
				text += "[<span style='color:orange;'>关注时回复</span>]";
				flag = true;
			}
			if (ReplyType.NoMatch == (replyType & ReplyType.NoMatch))
			{
				text += "[<span style='color:green;'>无匹配回复</span>]";
				flag = true;
			}
			if (ReplyType.Keys == (replyType & ReplyType.Keys))
			{
				text += "[关键字回复]";
				flag = true;
			}
			if (!flag)
			{
				text = replyType.ToShowText();
			}
			return text;
		}
	}
}
