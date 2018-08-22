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
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
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

		protected SendAllEdit() : base("m06", "wxp10")
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
					string s2 = "{\"type\":\"1\",\"tips\":\"操作成功\"}";
					this.sendID = Globals.RequestFormNum("sendid");
					int num2 = Globals.RequestFormNum("sendtype");
					int num3 = Globals.RequestFormNum("msgtype");
					int articleid = Globals.RequestFormNum("articleid");
					string title = Globals.RequestFormStr("title");
					string content = Globals.RequestFormStr("content");
					int isoldarticle = Globals.RequestFormNum("isoldarticle");
					string text = this.SavePostData(num3, articleid, title, content, isoldarticle, this.sendID, true);
					if (string.IsNullOrEmpty(text))
					{
						MessageType messageType = (MessageType)num3;
						string text2 = string.Empty;
						SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
						string text3 = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
						text3 = JsonConvert.DeserializeObject<Token>(text3).access_token;
						switch (messageType)
						{
						case MessageType.News:
						case MessageType.List:
						{
							bool flag = true;
							ArticleInfo articleInfo2 = ArticleHelper.GetArticleInfo(articleid);
							if (articleInfo2.MediaId.Length < 1)
							{
								string text4 = NewsApi.GetMedia_IDByPath(text3, articleInfo2.ImageUrl);
								text4 = NewsApi.GetJsonValue(text4, "media_id");
								if (!string.IsNullOrEmpty(text4))
								{
									ArticleHelper.UpdateMedia_Id(0, articleInfo2.ArticleId, text4);
								}
								else
								{
									flag = false;
									s2 = "{\"type\":\"2\",\"tips\":\"" + NewsApi.GetErrorCodeMsg(NewsApi.GetJsonValue(text4, "errcode")) + "111111\"}";
								}
							}
							if (messageType == MessageType.List)
							{
								foreach (ArticleItemsInfo current2 in articleInfo2.ItemsInfo)
								{
									if (current2.MediaId == null || current2.MediaId.Length < 1)
									{
										string media_IDByPath = NewsApi.GetMedia_IDByPath(text3, current2.ImageUrl);
										string jsonValue = NewsApi.GetJsonValue(media_IDByPath, "media_id");
										if (jsonValue.Length == 0)
										{
											this.errcode = NewsApi.GetJsonValue(media_IDByPath, "errcode");
											flag = false;
											s2 = "{\"type\":\"2\",\"tips\":\"" + NewsApi.GetErrorCodeMsg(this.errcode) + "\"}";
											break;
										}
										ArticleHelper.UpdateMedia_Id(1, current2.Id, jsonValue);
									}
								}
							}
							if (!flag)
							{
								goto IL_91D;
							}
							string articlesJsonStr = this.GetArticlesJsonStr(articleInfo2);
							string msg = NewsApi.UploadNews(text3, articlesJsonStr);
							this.sendID = Globals.ToNum(this.SavePostData(num3, articleid, title, content, isoldarticle, this.sendID, false));
							string jsonValue2 = NewsApi.GetJsonValue(msg, "media_id");
							if (jsonValue2.Length <= 0)
							{
								this.errcode = NewsApi.GetJsonValue(msg, "errcode");
								s2 = "{\"type\":\"2\",\"tips\":\"" + NewsApi.GetErrorCodeMsg(this.errcode) + "！\"}";
								goto IL_91D;
							}
							if (num2 != 1)
							{
								System.Data.DataTable rencentOpenID = WeiXinHelper.GetRencentOpenID(100);
								int count = rencentOpenID.Rows.Count;
								int num4 = 0;
								string returnjsondata = string.Empty;
								for (int i = 0; i < rencentOpenID.Rows.Count; i++)
								{
									string msg2 = NewsApi.KFSend(text3, this.GetKFSendImageJson(rencentOpenID.Rows[i][0].ToString(), articleInfo2));
									this.errcode = NewsApi.GetJsonValue(msg2, "errcode");
									if (this.errcode == "0")
									{
										num4++;
									}
									else
									{
										returnjsondata = NewsApi.GetErrorCodeMsg(this.errcode);
									}
								}
								int sendstate = (num4 > 0) ? 1 : 2;
								WeiXinHelper.UpdateMsgId(this.sendID, "", sendstate, num4, count, returnjsondata);
								goto IL_91D;
							}
							text2 = NewsApi.SendAll(text3, NewsApi.CreateImageNewsJson(jsonValue2));
							if (string.IsNullOrWhiteSpace(text2))
							{
								s2 = "{\"type\":\"2\",\"tips\":\"type参数错误\"}";
								goto IL_91D;
							}
							string jsonValue3 = NewsApi.GetJsonValue(text2, "msg_id");
							if (!string.IsNullOrEmpty(jsonValue3))
							{
								WeiXinHelper.UpdateMsgId(this.sendID, jsonValue3, 0, 0, 0, "");
								goto IL_91D;
							}
							this.errcode = NewsApi.GetJsonValue(text2, "errcode");
							string errorCodeMsg = NewsApi.GetErrorCodeMsg(this.errcode);
							WeiXinHelper.UpdateMsgId(this.sendID, jsonValue3, 2, 0, 0, errorCodeMsg);
							s2 = "{\"type\":\"2\",\"tips\":\"" + errorCodeMsg + "!!\"}";
							goto IL_91D;
						}
						}
						this.sendID = Globals.ToNum(this.SavePostData(num3, articleid, title, content, isoldarticle, this.sendID, false));
						if (num2 == 1)
						{
							text2 = NewsApi.SendAll(text3, this.CreateTxtNewsJson(content));
							if (!string.IsNullOrWhiteSpace(text2))
							{
								string jsonValue4 = NewsApi.GetJsonValue(text2, "msg_id");
								if (jsonValue4.Length == 0)
								{
									this.errcode = NewsApi.GetJsonValue(text2, "errcode");
									string errorCodeMsg2 = NewsApi.GetErrorCodeMsg(this.errcode);
									WeiXinHelper.UpdateMsgId(this.sendID, jsonValue4, 2, 0, 0, errorCodeMsg2);
									s2 = "{\"type\":\"2\",\"tips\":\"" + errorCodeMsg2 + "\"}";
								}
								else
								{
									WeiXinHelper.UpdateMsgId(this.sendID, jsonValue4, 0, 0, 0, "");
								}
							}
							else
							{
								s2 = "{\"type\":\"2\",\"tips\":\"type参数错误\"}";
							}
						}
						else
						{
							System.Data.DataTable rencentOpenID2 = WeiXinHelper.GetRencentOpenID(100);
							int count2 = rencentOpenID2.Rows.Count;
							int num5 = 0;
							string returnjsondata2 = string.Empty;
							for (int j = 0; j < rencentOpenID2.Rows.Count; j++)
							{
								string msg3 = NewsApi.KFSend(text3, NewsApi.CreateKFTxtNewsJson(rencentOpenID2.Rows[j][0].ToString(), this.String2Json(this.FormatSendContent(content))));
								this.errcode = NewsApi.GetJsonValue(msg3, "errcode");
								if (this.errcode == "0")
								{
									num5++;
								}
								else
								{
									returnjsondata2 = NewsApi.GetErrorCodeMsg(this.errcode);
								}
							}
							int sendstate2 = (num5 > 0) ? 1 : 2;
							WeiXinHelper.UpdateMsgId(this.sendID, "", sendstate2, num5, count2, returnjsondata2);
							if (num5 == 0)
							{
								s2 = "{\"type\":\"0\",\"tips\":\"发送失败\"}";
							}
						}
					}
					else
					{
						s2 = "{\"type\":\"0\",\"tips\":\"" + text + "\"}";
					}
					IL_91D:
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
										int num6 = 0;
										foreach (NewsMsgInfo current3 in newsReplyInfo2.NewsMsg)
										{
											num6++;
											if (num6 == 1)
											{
												stringBuilder2.Append(string.Concat(new string[]
												{
													"<div class=\"mate-inner top\">                 <div class=\"mate-img\" >                     <img id=\"img1\" src=\"",
													current3.PicUrl,
													"\" class=\"img-responsive\">                     <div class=\"title\" id=\"title1\">",
													current3.Title,
													"</div>                 </div>             </div>"
												}));
											}
											else
											{
												stringBuilder2.Append(string.Concat(new string[]
												{
													"             <div class=\"mate-inner\">                 <div class=\"child-mate\">                     <div class=\"child-mate-title clearfix\">                         <div class=\"title\">",
													current3.Title,
													"</div>                         <div class=\"img\">                             <img src=\"",
													current3.PicUrl,
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
				string s = WeiXinHelper.SaveSendAllInfo(sendAllInfo, 0);
				int num = Globals.ToNum(s);
				if (num == 0)
				{
					return "微信群发保存失败！";
				}
				if (num > 0)
				{
					result = num.ToString();
				}
			}
			return result;
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

		public string CreateTxtNewsJson(string content)
		{
			string s = this.FormatSendContent(content);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"filter\":{\"is_to_all\":true},");
			stringBuilder.Append("\"text\":{\"content\":\"" + this.String2Json(s) + "\"},");
			stringBuilder.Append("\"msgtype\":\"text\"");
			stringBuilder.Append("}");
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

		public string GetArticlesJsonStr(ArticleInfo info)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"articles\":[");
			stringBuilder.Append("{");
			stringBuilder.Append("\"thumb_media_id\":\"" + info.MediaId + "\",");
			stringBuilder.Append("\"author\":\"\",");
			stringBuilder.Append("\"title\":\"" + this.String2Json(info.Title) + "\",");
			stringBuilder.Append("\"content_source_url\":\"" + this.String2Json(info.Url) + "\",");
			stringBuilder.Append("\"content\":\"" + this.String2Json(info.Content) + "\",");
			stringBuilder.Append("\"digest\":\"" + this.String2Json(info.Memo) + "\",");
			stringBuilder.Append("\"show_cover_pic\":\"1\"}");
			if (info.ArticleType == ArticleType.List)
			{
				foreach (ArticleItemsInfo current in info.ItemsInfo)
				{
					stringBuilder.Append(",{");
					stringBuilder.Append("\"thumb_media_id\":\"" + current.MediaId + "\",");
					stringBuilder.Append("\"author\":\"\",");
					stringBuilder.Append("\"title\":\"" + this.String2Json(current.Title) + "\",");
					stringBuilder.Append("\"content_source_url\":\"" + this.String2Json(current.Url) + "\",");
					stringBuilder.Append("\"content\":\"" + this.String2Json(current.Content) + "\",");
					stringBuilder.Append("\"digest\":\"\",");
					stringBuilder.Append("\"show_cover_pic\":\"0\"}");
				}
			}
			stringBuilder.Append("]}");
			return stringBuilder.ToString();
		}
	}
}
