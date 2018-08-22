using Aop.Api.Response;
using ControlPanel.WeiBo;
using ControlPanel.WeiXin;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.Entities.WeiXin;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.AliFuwu
{
	public class SendAllEdit : AdminPage
	{
		private string errcode = string.Empty;

		protected string htmlInfo = string.Empty;

		protected int sendID = Globals.RequestQueryNum("ID");

		protected int LocalArticleID = Globals.RequestQueryNum("aid");

		protected string type = Globals.RequestQueryStr("type");

		protected System.Web.UI.HtmlControls.HtmlForm aspnetForm;

		protected System.Web.UI.WebControls.Literal litInfo;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected System.Web.UI.WebControls.HiddenField hdfSendID;

		protected System.Web.UI.WebControls.HiddenField hdfMessageType;

		protected System.Web.UI.WebControls.HiddenField hdfArticleID;

		protected System.Web.UI.WebControls.HiddenField hdfIsOldArticle;

		protected ucUeditor fkContent;

		protected SendAllEdit() : base("m11", "fwp03")
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
									this.String2Json(articleInfo.Title),
									"\",\"date\":\"",
									this.String2Json(articleInfo.PubTime.ToString("M月d日")),
									"\",\"imgurl\":\"",
									this.String2Json(articleInfo.ImageUrl),
									"\",\"memo\":\"",
									this.String2Json(articleInfo.Memo),
									"\"}"
								});
								goto IL_30E;
							case ArticleType.List:
							{
								System.Collections.Generic.IList<ArticleItemsInfo> itemsInfo = articleInfo.ItemsInfo;
								foreach (ArticleItemsInfo current in itemsInfo)
								{
									stringBuilder.Append(string.Concat(new string[]
									{
										"{\"title\":\"",
										this.String2Json(current.Title),
										"\",\"imgurl\":\"",
										this.String2Json(current.ImageUrl),
										"\"},"
									}));
								}
								s = string.Concat(new object[]
								{
									"{\"type\":\"1\",\"articletype\":",
									(int)articleInfo.ArticleType,
									",\"title\":\"",
									this.String2Json(articleInfo.Title),
									"\",\"date\":\"",
									this.String2Json(articleInfo.PubTime.ToString("M月d日")),
									"\",\"imgurl\":\"",
									this.String2Json(articleInfo.ImageUrl),
									"\",\"items\":[",
									stringBuilder.ToString().Trim(new char[]
									{
										','
									}),
									"]}"
								});
								goto IL_30E;
							}
							}
							s = string.Concat(new object[]
							{
								"{\"type\":\"1\",\"articletype\":",
								(int)articleInfo.ArticleType,
								",\"title\":\"",
								this.String2Json(articleInfo.Title),
								"\",\"date\":\"",
								this.String2Json(articleInfo.PubTime.ToString("M月d日")),
								"\",\"imgurl\":\"",
								this.String2Json(articleInfo.ImageUrl),
								"\",\"memo\":\"",
								this.String2Json(articleInfo.Content),
								"\"}"
							});
						}
					}
					IL_30E:
					base.Response.Write(s);
					base.Response.End();
					return;
				}
				if (this.type == "postdata")
				{
					base.Response.ContentType = "application/json";
					this.sendID = Globals.RequestFormNum("sendid");
					int num2 = Globals.RequestFormNum("sendtype");
					int num3 = Globals.RequestFormNum("msgtype");
					int articleid = Globals.RequestFormNum("articleid");
					string title = Globals.RequestFormStr("title");
					string content = Globals.RequestFormStr("content");
					int isoldarticle = Globals.RequestFormNum("isoldarticle");
					string text = this.SavePostData(num3, articleid, title, content, isoldarticle, this.sendID, true);
					string s2;
					if (string.IsNullOrEmpty(text))
					{
						MessageType messageType = (MessageType)num3;
						string arg_3DF_0 = string.Empty;
						Articles Articles = new Articles();
						Articles.msgType = "text";
						string storeUrl = Globals.HostPath(System.Web.HttpContext.Current.Request.Url);
						if (messageType == MessageType.List || messageType == MessageType.News)
						{
							this.sendID = Globals.ToNum(this.SavePostData(num3, articleid, title, content, isoldarticle, this.sendID, false));
							ArticleInfo articleInfo2 = ArticleHelper.GetArticleInfo(articleid);
							if (articleInfo2 == null)
							{
								s2 = "{\"type\":\"0\",\"tips\":\"素材不存在了\"}";
								base.Response.Write(s2);
								base.Response.End();
							}
							Articles = this.GetAlipayArticlesFromArticleInfo(articleInfo2, storeUrl);
						}
						else
						{
							this.sendID = Globals.ToNum(this.SavePostData(num3, articleid, title, content, isoldarticle, this.sendID, false));
							Articles.text = new MessageText
							{
								content = Globals.StripHtmlXmlTags(content)
							};
						}
						SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
						if (AlipayFuwuConfig.appId.Length < 15)
						{
							AlipayFuwuConfig.CommSetConfig(masterSettings.AlipayAppid, base.Server.MapPath("~/"), "GBK");
						}
						if (num2 == 1)
						{
							AlipayMobilePublicMessageTotalSendResponse alipayMobilePublicMessageTotalSendResponse = AliOHHelper.TotalSend(Articles);
							if (!alipayMobilePublicMessageTotalSendResponse.IsError && alipayMobilePublicMessageTotalSendResponse.Code == "200")
							{
								s2 = "{\"type\":\"1\",\"tips\":\"服务窗群发成功，请于一天后到服务窗后台查询送达结果！\"}";
								string msgid = "";
								if (!string.IsNullOrEmpty(alipayMobilePublicMessageTotalSendResponse.Data) && alipayMobilePublicMessageTotalSendResponse.Data.Length > 50)
								{
									msgid = alipayMobilePublicMessageTotalSendResponse.Data.Substring(0, 49);
								}
								int alypayUserNum = WeiXinHelper.getAlypayUserNum();
								WeiXinHelper.UpdateMsgId(this.sendID, msgid, 1, alypayUserNum, alypayUserNum, "");
							}
							else
							{
								s2 = "{\"type\":\"0\",\"tips\":\"" + alipayMobilePublicMessageTotalSendResponse.Msg + "\"}";
								WeiXinHelper.UpdateMsgId(this.sendID, "", 2, 0, 0, alipayMobilePublicMessageTotalSendResponse.Body);
							}
						}
						else
						{
							System.Collections.Generic.List<string> sendList = new System.Collections.Generic.List<string>();
							System.Data.DataTable rencentAliOpenID = WeiXinHelper.GetRencentAliOpenID();
							if (rencentAliOpenID != null)
							{
								for (int i = 0; i < rencentAliOpenID.Rows.Count; i++)
								{
									sendList.Add(rencentAliOpenID.Rows[i][0].ToString());
								}
							}
							if (sendList.Count > 0)
							{
								WeiXinHelper.UpdateMsgId(this.sendID, "", 0, 0, sendList.Count, "");
								new System.Threading.Thread(()=>
								{
									try
									{
										bool flag = false;
										foreach (string current3 in sendList)
										{
											if (current3.Length > 16)
											{
												Articles.toUserId = current3;
												AlipayMobilePublicMessageCustomSendResponse alipayMobilePublicMessageCustomSendResponse = AliOHHelper.CustomSend(Articles);
												if (alipayMobilePublicMessageCustomSendResponse != null && alipayMobilePublicMessageCustomSendResponse.IsError)
												{
													AliOHHelper.log(alipayMobilePublicMessageCustomSendResponse.Body);
												}
												else
												{
													flag = true;
													WeiXinHelper.UpdateAddSendCount(this.sendID, 1, -1);
												}
												System.Threading.Thread.Sleep(10);
											}
										}
										if (flag)
										{
											WeiXinHelper.UpdateAddSendCount(this.sendID, 0, 1);
										}
										else
										{
											WeiXinHelper.UpdateAddSendCount(this.sendID, 0, 2);
										}
										System.Threading.Thread.Sleep(10);
									}
									catch (System.Exception ex)
									{
										AliOHHelper.log(ex.Message.ToString());
									}
								}).Start();
								s2 = "{\"type\":\"1\",\"tips\":\"信息正在后台推送中，请稍后刷新群发列表查看结果\"}";
							}
							else
							{
								s2 = "{\"type\":\"0\",\"tips\":\"暂时没有关注的用户可以发送信息\"}";
							}
						}
					}
					else
					{
						s2 = "{\"type\":\"0\",\"tips\":\"" + text + "\"}";
					}
					base.Response.Write(s2);
					base.Response.End();
					return;
				}
				if (this.sendID > 0)
				{
					this.hdfSendID.Value = this.sendID.ToString();
					SendAllInfo sendAllInfo = WeiXinHelper.GetSendAllInfo(this.sendID);
					if (sendAllInfo != null)
					{
						MessageType messageType2 = sendAllInfo.MessageType;
						this.hdfMessageType.Value = ((int)sendAllInfo.MessageType).ToString();
						int articleID = sendAllInfo.ArticleID;
						this.hdfArticleID.Value = articleID.ToString();
						this.txtTitle.Text = sendAllInfo.Title;
						switch (messageType2)
						{
						case MessageType.Text:
							this.fkContent.Text = sendAllInfo.Content;
							break;
						case MessageType.News:
							if (articleID <= 0)
							{
								this.hdfIsOldArticle.Value = "1";
								NewsReplyInfo newsReplyInfo = ReplyHelper.GetReply(this.sendID) as NewsReplyInfo;
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
								NewsReplyInfo newsReplyInfo2 = ReplyHelper.GetReply(this.sendID) as NewsReplyInfo;
								if (newsReplyInfo2 != null)
								{
									System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
									if (newsReplyInfo2.NewsMsg != null && newsReplyInfo2.NewsMsg.Count > 0)
									{
										int num4 = 0;
										foreach (NewsMsgInfo current2 in newsReplyInfo2.NewsMsg)
										{
											num4++;
											if (num4 == 1)
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
						base.Response.Redirect("sendalllist.aspx");
						base.Response.End();
					}
				}
				else if (this.LocalArticleID > 0)
				{
					ArticleInfo articleInfo3 = ArticleHelper.GetArticleInfo(this.LocalArticleID);
					if (articleInfo3 != null)
					{
						this.hdfArticleID.Value = this.LocalArticleID.ToString();
						this.hdfMessageType.Value = ((int)articleInfo3.ArticleType).ToString();
					}
				}
				if (string.IsNullOrEmpty(this.htmlInfo))
				{
					this.htmlInfo = "<div class=\"exit-shop-info\">内容区</div>";
				}
				this.litInfo.Text = this.htmlInfo;
			}
		}

		private string GetKFSendImageJson(string openid, ArticleInfo articleinfo)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (articleinfo != null && articleinfo != null)
			{
				switch (articleinfo.ArticleType)
				{
				case ArticleType.News:
					break;
				case (ArticleType)3:
					goto IL_200;
				case ArticleType.List:
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						"{\"title\":\"",
						this.String2Json(articleinfo.Title),
						"\",\"description\":\"",
						this.String2Json(articleinfo.Memo),
						"\",\"url\":\"",
						this.String2Json(this.FormatUrl(articleinfo.Url)),
						"\",\"picurl\":\"",
						this.String2Json(this.FormatUrl(articleinfo.ImageUrl)),
						"\"}"
					}));
					System.Collections.Generic.IList<ArticleItemsInfo> itemsInfo = articleinfo.ItemsInfo;
					using (System.Collections.Generic.IEnumerator<ArticleItemsInfo> enumerator = itemsInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ArticleItemsInfo current = enumerator.Current;
							stringBuilder.Append(string.Concat(new string[]
							{
								",{\"title\":\"",
								this.String2Json(current.Title),
								"\",\"description\":\"\",\"url\":\"",
								this.String2Json(this.FormatUrl(current.Url)),
								"\",\"picurl\":\"",
								this.String2Json(this.FormatUrl(current.ImageUrl)),
								"\"}"
							}));
						}
						goto IL_200;
					}
					break;
				}
				default:
					goto IL_200;
				}
				stringBuilder.Append(string.Concat(new string[]
				{
					"{\"title\":\"",
					this.String2Json(articleinfo.Title),
					"\",\"description\":\"",
					this.String2Json(articleinfo.Memo),
					"\",\"url\":\"",
					this.String2Json(this.FormatUrl(articleinfo.Url)),
					"\",\"picurl\":\"",
					this.String2Json(this.FormatUrl(articleinfo.ImageUrl)),
					"\"}"
				}));
			}
			IL_200:
			return string.Concat(new string[]
			{
				"{\"touser\":\"",
				openid,
				"\",\"msgtype\":\"news\",\"news\":{\"articles\": [",
				stringBuilder.ToString(),
				"]}}"
			});
		}

		private string SavePostData(int msgType, int articleid, string title, string content, int isoldarticle, int sendid, bool isonlycheck)
		{
			string result = string.Empty;
			if (string.IsNullOrEmpty(title))
			{
				return "请输入标题！";
			}
			if (articleid < 1 && msgType != 1)
			{
				if (isoldarticle == 0)
				{
					return "请先选择图文！";
				}
				if (sendid > 0 && !isonlycheck)
				{
					articleid = ReplyHelper.GetArticleIDByOldArticle(sendid, (MessageType)msgType);
				}
			}
			if (!isonlycheck)
			{
				SendAllInfo sendAllInfo = new SendAllInfo();
				if (sendid > 0)
				{
					sendAllInfo = WeiXinHelper.GetSendAllInfo(sendid);
				}
				sendAllInfo.Title = title;
				sendAllInfo.MessageType = (MessageType)msgType;
				sendAllInfo.ArticleID = articleid;
				sendAllInfo.Content = content;
				sendAllInfo.SendState = 0;
				sendAllInfo.SendTime = System.DateTime.Now;
				sendAllInfo.SendCount = 0;
				string s = WeiXinHelper.SaveSendAllInfo(sendAllInfo, 1);
				int num = Globals.ToNum(s);
				if (num == 0)
				{
					return "服务窗群发保存失败！";
				}
				if (num > 0)
				{
					result = num.ToString();
				}
			}
			return result;
		}

		private Articles GetAlipayArticlesFromArticleInfo(ArticleInfo articleInfo, string storeUrl)
		{
			Articles articles = null;
			if (articleInfo != null)
			{
				articles = new Articles();
				articles.msgType = "image-text";
				articles.articles = new System.Collections.Generic.List<article>();
				string text = articleInfo.ImageUrl;
				if (!text.ToLower().StartsWith("http"))
				{
					text = storeUrl + text;
				}
				string text2 = Globals.StripAllTags(articleInfo.Content);
				if (text2.Length > 30)
				{
					text2 = text2.Substring(0, 30);
				}
				article item = new article
				{
					actionName = "立即查看",
					title = articleInfo.Title,
					desc = text2,
					imageUrl = text,
					url = articleInfo.Url
				};
				articles.articles.Add(item);
				if (articleInfo.ItemsInfo != null && articleInfo.ItemsInfo.Count > 0)
				{
					foreach (ArticleItemsInfo current in articleInfo.ItemsInfo)
					{
						string text3 = current.ImageUrl;
						if (!text3.ToLower().StartsWith("http"))
						{
							text3 = storeUrl + text3;
						}
						string text4 = Globals.StripAllTags(current.Content);
						if (text4.Length > 30)
						{
							text4 = text4.Substring(0, 30);
						}
						article item2 = new article
						{
							actionName = "立即查看",
							title = current.Title,
							desc = text4,
							imageUrl = text3,
							url = current.Url
						};
						articles.articles.Add(item2);
					}
				}
			}
			return articles;
		}

		private string FormatUrl(string url)
		{
			string result = url;
			if (url.StartsWith("/"))
			{
				result = "http://" + Globals.DomainName + url;
			}
			return result;
		}

		private string String2Json(string s)
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

		private string FormatSendContent(string content)
		{
			string text = System.Text.RegularExpressions.Regex.Replace(content, "</?([^>^a^p]*)>", "");
			text = System.Text.RegularExpressions.Regex.Replace(text, "<img([^>]*)>", "");
			text = text.Replace("<p>", "");
			text = text.Replace("</p>", "\r");
			text = text.Trim(new char[]
			{
				'\r'
			});
			return text.Replace("\r", "\r\n");
		}
	}
}
