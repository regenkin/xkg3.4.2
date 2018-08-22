using ControlPanel.WeiBo;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliFuwu
{
	public class ReplyEdit : AdminPage
	{
		protected string htmlTitle = "新增自动回复";

		protected string htmlInfo = string.Empty;

		protected int replyID = Globals.RequestQueryNum("ID");

		protected string type = Globals.RequestQueryStr("type");

		protected System.Web.UI.HtmlControls.HtmlForm aspnetForm;

		protected System.Web.UI.WebControls.Literal litInfo;

		protected System.Web.UI.WebControls.TextBox txtKeys;

		protected System.Web.UI.WebControls.RadioButtonList rbtlMatchType;

		protected System.Web.UI.WebControls.HiddenField hdfMessageType;

		protected System.Web.UI.WebControls.HiddenField hdfArticleID;

		protected System.Web.UI.WebControls.HiddenField hdfIsOldArticle;

		protected ucUeditor fkContent;

		protected System.Web.UI.WebControls.Button btnSave;

		protected ReplyEdit() : base("m11", "fwp06")
		{
		}

		[PrivilegeCheck(Privilege.Summary)]
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (this.type == "getarticleinfo")
				{
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
					int num = Globals.RequestFormNum("articleid");
					if (num > 0)
					{
						ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(num);
						if (articleInfo != null)
						{
							System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
							switch (articleInfo.ArticleType)
							{
							case ArticleType.News:
								s = string.Concat(new object[]
								{
									"{\"type\":\"1\",\"articletype\":",
									(int)articleInfo.ArticleType,
									",\"title\":\"",
									ReplyEdit.String2Json(articleInfo.Title),
									"\",\"date\":\"",
									ReplyEdit.String2Json(articleInfo.PubTime.ToString("M月d日")),
									"\",\"imgurl\":\"",
									ReplyEdit.String2Json(articleInfo.ImageUrl),
									"\",\"memo\":\"",
									ReplyEdit.String2Json(articleInfo.Memo),
									"\"}"
								});
								goto IL_301;
							case ArticleType.List:
							{
								System.Collections.Generic.IList<ArticleItemsInfo> itemsInfo = articleInfo.ItemsInfo;
								foreach (ArticleItemsInfo current in itemsInfo)
								{
									stringBuilder.Append(string.Concat(new string[]
									{
										"{\"title\":\"",
										ReplyEdit.String2Json(current.Title),
										"\",\"imgurl\":\"",
										ReplyEdit.String2Json(current.ImageUrl),
										"\"},"
									}));
								}
								s = string.Concat(new object[]
								{
									"{\"type\":\"1\",\"articletype\":",
									(int)articleInfo.ArticleType,
									",\"title\":\"",
									ReplyEdit.String2Json(articleInfo.Title),
									"\",\"date\":\"",
									ReplyEdit.String2Json(articleInfo.PubTime.ToString("M月d日")),
									"\",\"imgurl\":\"",
									ReplyEdit.String2Json(articleInfo.ImageUrl),
									"\",\"items\":[",
									stringBuilder.ToString().Trim(new char[]
									{
										','
									}),
									"]}"
								});
								goto IL_301;
							}
							}
							s = string.Concat(new object[]
							{
								"{\"type\":\"1\",\"articletype\":",
								(int)articleInfo.ArticleType,
								",\"title\":\"",
								ReplyEdit.String2Json(articleInfo.Title),
								"\",\"date\":\"",
								ReplyEdit.String2Json(articleInfo.PubTime.ToString("M月d日")),
								"\",\"imgurl\":\"",
								ReplyEdit.String2Json(articleInfo.ImageUrl),
								"\",\"memo\":\"",
								ReplyEdit.String2Json(articleInfo.Content),
								"\"}"
							});
						}
					}
					IL_301:
					base.Response.Write(s);
					base.Response.End();
					return;
				}
				if (this.type == "subscribe" && this.replyID == 0)
				{
					this.replyID = AliFuwuReplyHelper.GetSubscribeID(0);
					if (this.replyID > 0)
					{
						base.Response.Redirect("replyedit.aspx?type=subscribe&id=" + this.replyID);
						base.Response.End();
					}
					this.rbtlMatchType.SelectedIndex = 0;
					if (string.IsNullOrEmpty(this.htmlInfo))
					{
						this.htmlInfo = "<div class=\"exit-shop-info\">内容区</div>";
					}
					this.litInfo.Text = this.htmlInfo;
					return;
				}
				if (this.replyID > 0)
				{
					this.htmlTitle = "修改自动回复";
					Hidistro.Entities.VShop.ReplyInfo reply = AliFuwuReplyHelper.GetReply(this.replyID);
					if (reply != null)
					{
						MessageType messageType = reply.MessageType;
						if (ReplyType.NoMatch == reply.ReplyType)
						{
							this.txtKeys.Text = "*";
						}
						else if (ReplyType.Subscribe == reply.ReplyType)
						{
							this.txtKeys.Text = "";
							if (this.type != "subscribe")
							{
								base.Response.Redirect("replyedit.aspx?type=subscribe&id=" + this.replyID);
								base.Response.End();
							}
						}
						else
						{
							this.txtKeys.Text = reply.Keys.Trim();
						}
						for (int i = 0; i < this.rbtlMatchType.Items.Count; i++)
						{
							if (this.rbtlMatchType.Items[i].Value == ((int)reply.MatchType).ToString())
							{
								this.rbtlMatchType.Items[i].Selected = true;
								break;
							}
						}
						this.hdfMessageType.Value = ((int)reply.MessageType).ToString();
						this.hdfArticleID.Value = reply.ArticleID.ToString();
						int articleID = reply.ArticleID;
						switch (messageType)
						{
						case MessageType.Text:
						{
							TextReplyInfo textReplyInfo = AliFuwuReplyHelper.GetReply(this.replyID) as TextReplyInfo;
							if (textReplyInfo != null)
							{
								string text = textReplyInfo.Text;
								text = System.Text.RegularExpressions.Regex.Replace(text, "</?([^>^a^p]*)>", "");
								text = System.Text.RegularExpressions.Regex.Replace(text, "<img([^>]*)>", "");
								text = text.Replace("</p>", "\r\n");
								text = text.Replace("<p>", "");
								this.fkContent.Text = text;
							}
							break;
						}
						case MessageType.News:
							if (articleID <= 0)
							{
								this.hdfIsOldArticle.Value = "1";
								NewsReplyInfo newsReplyInfo = AliFuwuReplyHelper.GetReply(this.replyID) as NewsReplyInfo;
								if (newsReplyInfo != null && newsReplyInfo.NewsMsg != null && newsReplyInfo.NewsMsg.Count != 0)
								{
									this.htmlInfo = string.Concat(new string[]
									{
										"<div class=\"mate-inner\"><h3 id=\"singelTitle\">",
										newsReplyInfo.NewsMsg[0].Title,
										"</h3><span>",
										newsReplyInfo.LastEditDate.ToString("M月d日"),
										"</span><div class=\"mate-img\"><img id=\"img1\" src=\"",
										newsReplyInfo.NewsMsg[0].PicUrl,
										"\" class=\"img-responsive\"></div><div class=\"mate-info\" id=\"Lbmsgdesc\">",
										newsReplyInfo.NewsMsg[0].Description,
										"</div><div class=\"red-all clearfix\"><strong class=\"fl\">查看全文</strong><em class=\"fr\">&gt;</em></div></div>"
									});
								}
							}
							break;
						case MessageType.List:
							if (articleID <= 0)
							{
								this.hdfIsOldArticle.Value = "1";
								NewsReplyInfo newsReplyInfo2 = AliFuwuReplyHelper.GetReply(this.replyID) as NewsReplyInfo;
								if (newsReplyInfo2 != null)
								{
									System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
									if (newsReplyInfo2.NewsMsg != null && newsReplyInfo2.NewsMsg.Count > 0)
									{
										int num2 = 0;
										foreach (NewsMsgInfo current2 in newsReplyInfo2.NewsMsg)
										{
											num2++;
											if (num2 == 1)
											{
												stringBuilder2.Append(string.Concat(new string[]
												{
													"<div class=\"mate-inner top\">                 <div class=\"mate-img\" >                     <img id=\"img1\" src=\"",
													current2.PicUrl,
													"\" class=\"img-responsive\">                     <div class=\"title\" id=\"title1\">",
													current2.Title,
													"</div>                 </div>             </div>"
												}));
											}
											else
											{
												stringBuilder2.Append(string.Concat(new string[]
												{
													"             <div class=\"mate-inner\">                 <div class=\"child-mate\">                     <div class=\"child-mate-title clearfix\">                         <div class=\"title\">",
													current2.Title,
													"</div>                         <div class=\"img\">                             <img src=\"",
													current2.PicUrl,
													"\" class=\"img-responsive\">                         </div>                     </div>                 </div>             </div>"
												}));
											}
										}
										this.htmlInfo = stringBuilder2.ToString();
									}
								}
							}
							break;
						}
					}
					else
					{
						base.Response.Redirect("replyonkey.aspx");
						base.Response.End();
					}
				}
				if (string.IsNullOrEmpty(this.htmlInfo))
				{
					this.htmlInfo = "<div class=\"exit-shop-info\">内容区</div>";
				}
				this.litInfo.Text = this.htmlInfo;
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			int num = Globals.ToNum(this.hdfMessageType.Value);
			int num2 = Globals.ToNum(this.hdfArticleID.Value);
			MessageType messageType = (MessageType)num;
			int num3 = Globals.ToNum(this.rbtlMatchType.SelectedValue);
			if (string.IsNullOrEmpty(this.txtKeys.Text.Trim()) && this.type != "subscribe")
			{
				this.ShowMsg("请输入关键词！", false);
				return;
			}
			if (this.txtKeys.Text.Trim().Length > 50)
			{
				this.ShowMsg("关键词必须少于50个字！", false);
				return;
			}
			if (num3 == 0)
			{
				this.ShowMsg("请选择匹配类型！", false);
				return;
			}
			if (num2 < 1 && messageType != MessageType.Text)
			{
				if (this.hdfIsOldArticle.Value == "0")
				{
					this.ShowMsg("请先选择图文！", false);
					return;
				}
				if (this.replyID > 0)
				{
					num2 = AliFuwuReplyHelper.GetArticleIDByOldArticle(this.replyID, messageType);
				}
			}
			switch (messageType)
			{
			case MessageType.Text:
			{
				if (this.fkContent.Text.Length > 1000)
				{
					this.ShowMsg("回复内容必须1000字以内！", false);
					return;
				}
				TextReplyInfo textReplyInfo = new TextReplyInfo();
				textReplyInfo.Keys = this.txtKeys.Text.Trim();
				textReplyInfo.MatchType = ((num3 == 2) ? MatchType.Equal : MatchType.Like);
				if (textReplyInfo.Keys == "*")
				{
					textReplyInfo.ReplyType = ReplyType.NoMatch;
					if (AliFuwuReplyHelper.GetNoMatchReplyID(this.replyID) > 0)
					{
						this.ShowMsg("无关键字回复回复内容已存在！", false);
						return;
					}
				}
				else if (this.type == "subscribe")
				{
					if (AliFuwuReplyHelper.GetSubscribeID(this.replyID) > 0)
					{
						this.ShowMsg("首次关注回复内容已存在！", false);
						return;
					}
					textReplyInfo.ReplyType = ReplyType.Subscribe;
					textReplyInfo.Keys = "";
				}
				else
				{
					textReplyInfo.ReplyType = ReplyType.Keys;
					if (AliFuwuReplyHelper.HasReplyKey(textReplyInfo.Keys, this.replyID))
					{
						this.ShowMsg("该关键词已存在！", false);
						return;
					}
				}
				textReplyInfo.MessageType = messageType;
				textReplyInfo.IsDisable = false;
				textReplyInfo.ArticleID = num2;
				string text = this.fkContent.Text;
				text = System.Text.RegularExpressions.Regex.Replace(text, "</?([^>^a^p]*)>", "");
				text = System.Text.RegularExpressions.Regex.Replace(text, "<img([^>]*)>", "");
				text = text.Replace("<p>", "");
				text = text.Replace("</p>", "\r");
				text = text.Trim(new char[]
				{
					'\r'
				});
				text = text.Replace("\r", "\r\n");
				textReplyInfo.Text = text;
				textReplyInfo.Id = this.replyID;
				if (string.IsNullOrEmpty(textReplyInfo.Text))
				{
					this.ShowMsg("请填写文本内容！", false);
					return;
				}
				if (this.replyID > 0)
				{
					AliFuwuReplyHelper.UpdateReply(textReplyInfo);
				}
				else
				{
					AliFuwuReplyHelper.SaveReply(textReplyInfo);
				}
				break;
			}
			case MessageType.News:
			case MessageType.List:
			{
				NewsReplyInfo newsReplyInfo = new NewsReplyInfo();
				newsReplyInfo.Keys = this.txtKeys.Text.Trim();
				newsReplyInfo.MatchType = ((num3 == 2) ? MatchType.Equal : MatchType.Like);
				if (newsReplyInfo.Keys == "*")
				{
					newsReplyInfo.ReplyType = ReplyType.NoMatch;
					if (AliFuwuReplyHelper.GetNoMatchReplyID(this.replyID) > 0)
					{
						this.ShowMsg("无关键词回复已存在！", false);
						return;
					}
				}
				else if (this.type == "subscribe")
				{
					newsReplyInfo.ReplyType = ReplyType.Subscribe;
					newsReplyInfo.Keys = "";
					if (AliFuwuReplyHelper.GetSubscribeID(this.replyID) > 0)
					{
						this.ShowMsg("首次关注回复已存在！", false);
						return;
					}
				}
				else
				{
					newsReplyInfo.ReplyType = ReplyType.Keys;
					if (AliFuwuReplyHelper.HasReplyKey(newsReplyInfo.Keys, this.replyID))
					{
						this.ShowMsg("该关键词已存在！", false);
						return;
					}
				}
				newsReplyInfo.MessageType = messageType;
				newsReplyInfo.IsDisable = false;
				newsReplyInfo.ArticleID = num2;
				newsReplyInfo.Id = this.replyID;
				if (num3 < 1)
				{
					this.ShowMsg("请选择类型！", false);
					return;
				}
				if (this.replyID > 0)
				{
					AliFuwuReplyHelper.UpdateReply(newsReplyInfo);
				}
				else
				{
					AliFuwuReplyHelper.SaveReply(newsReplyInfo);
				}
				break;
			}
			}
			if (this.replyID > 0)
			{
				this.ShowMsgAndReUrl("自动回复修改成功！", true, "replyonkey.aspx");
				return;
			}
			this.ShowMsgAndReUrl("自动回复添加成功！", true, "replyonkey.aspx");
		}

		private static string String2Json(string s)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int i = 0;
			while (i < s.Length)
			{
				char c = s.ToCharArray()[i];
				char c2 = c;
				if (c2 <= '"')
				{
					switch (c2)
					{
					case '\b':
						stringBuilder.Append("\\b");
						break;
					case '\t':
						stringBuilder.Append("\\t");
						break;
					case '\n':
						stringBuilder.Append("\\n");
						break;
					case '\v':
						goto IL_C0;
					case '\f':
						stringBuilder.Append("\\f");
						break;
					case '\r':
						stringBuilder.Append("\\r");
						break;
					default:
						if (c2 != '"')
						{
							goto IL_C0;
						}
						stringBuilder.Append("\\\"");
						break;
					}
				}
				else if (c2 != '/')
				{
					if (c2 != '\\')
					{
						goto IL_C0;
					}
					stringBuilder.Append("\\\\");
				}
				else
				{
					stringBuilder.Append("\\/");
				}
				IL_C8:
				i++;
				continue;
				IL_C0:
				stringBuilder.Append(c);
				goto IL_C8;
			}
			return stringBuilder.ToString();
		}
	}
}
