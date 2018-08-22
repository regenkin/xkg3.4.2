using ControlPanel.WeiBo;
using ControlPanel.WeiXin;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.SaleSystem.Vshop;
using Hishop.AlipayFuwu.Api.Model;
using Hishop.AlipayFuwu.Api.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class AliPayFuwuApi : System.Web.IHttpHandler
	{
		private string appid;

		private SiteSettings siteSettings;

		private string UserInfo;

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			this.siteSettings = SettingsManager.GetMasterSettings(true);
			this.appid = this.siteSettings.AlipayAppid;
			AliOHHelper.log(context.Request.Form.ToString());
			if (AlipayFuwuConfig.appId.Length < 15 && !AlipayFuwuConfig.CommSetConfig(this.appid, context.Server.MapPath("~/"), "GBK"))
			{
				context.Response.Write(AlipayFuwuConfig.errstr);
				return;
			}
			if ("alipay.service.check".Equals(AliOHHelper.getRequestString("service", context)))
			{
				AliOHHelper.verifygw(context);
				return;
			}
			if ("alipay.mobile.public.message.notify".Equals(AliOHHelper.getRequestString("service", context)))
			{
				string requestString = AliOHHelper.getRequestString("biz_content", context);
				string xmlNode = AliOHHelper.getXmlNode(requestString, "EventType");
				string xmlNode2 = AliOHHelper.getXmlNode(requestString, "FromUserId");
				this.UserInfo = AliOHHelper.getXmlNode(requestString, "UserInfo");
				string xmlNode3 = AliOHHelper.getXmlNode(requestString, "ActionParam");
				AliOHHelper.getXmlNode(requestString, "AgreementId");
				AliOHHelper.getXmlNode(requestString, "AccountNo");
				AliOHHelper.getXmlNode(requestString, "AppId");
				AliOHHelper.getXmlNode(requestString, "CreateTime");
				string xmlNode4 = AliOHHelper.getXmlNode(requestString, "MsgType");
				string xmlNode5 = AliOHHelper.getXmlNode(requestString, "Content");
				string a;
				if ((a = xmlNode4) != null)
				{
					if (!(a == "event"))
					{
						if (!(a == "text"))
						{
							if (!(a == "image"))
							{
								goto IL_22F;
							}
							goto IL_1E3;
						}
					}
					else
					{
						try
						{
							this.replyAction(xmlNode2, xmlNode, xmlNode5, xmlNode3);
							goto IL_22F;
						}
						catch (System.Exception ex)
						{
							AliOHHelper.log(ex.Message.ToString());
							goto IL_22F;
						}
					}
					try
					{
						if (!string.IsNullOrEmpty(xmlNode2))
						{
							WeiXinHelper.UpdateRencentAliOpenID(xmlNode2);
						}
					}
					catch (System.Exception ex2)
					{
						AliOHHelper.log(ex2.Message.ToString());
					}
					try
					{
						this.replyAction(xmlNode2, "", xmlNode5, xmlNode3);
						goto IL_22F;
					}
					catch (System.Exception ex3)
					{
						AliOHHelper.log(ex3.Message.ToString());
						goto IL_22F;
					}
					IL_1E3:
					Articles articles = new Articles
					{
						toUserId = xmlNode2,
						msgType = "text",
						text = new MessageText
						{
							content = "服务窗暂时不接收图片消息"
						}
					};
					AliOHHelper.log(AliOHHelper.CustomSend(articles).Body);
				}
				IL_22F:
				AliOHHelper.verifyRequestFromAliPay(context, xmlNode2, this.appid);
			}
		}

		private void replyAction(string FromUserId, string eventType, string textContent, string ActionParam)
		{
			Articles articles = new Articles
			{
				toUserId = FromUserId,
				msgType = "text",
				text = new MessageText
				{
					content = "系统未找到相关信息！"
				}
			};
			if (eventType != "")
			{
				if ("follow".Equals(eventType))
				{
					if (!string.IsNullOrEmpty(FromUserId))
					{
						MemberProcessor.AddFuwuFollowUser(FromUserId);
					}
					string aliOHFollowRelayTitle = this.siteSettings.AliOHFollowRelayTitle;
					Hidistro.Entities.VShop.ReplyInfo subscribeReply = AliFuwuReplyHelper.GetSubscribeReply();
					if (subscribeReply != null)
					{
						if (subscribeReply.MessageType == MessageType.Text)
						{
							TextReplyInfo textReplyInfo = subscribeReply as TextReplyInfo;
							articles.text.content = textReplyInfo.Text;
						}
						else if (subscribeReply.ArticleID > 0)
						{
							ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(subscribeReply.ArticleID);
							if (articleInfo != null)
							{
								articles = this.GetAlipayArticlesFromArticleInfo(articleInfo, Globals.HostPath(System.Web.HttpContext.Current.Request.Url), FromUserId);
							}
							else
							{
								articles.text.content = aliOHFollowRelayTitle;
							}
						}
					}
					else
					{
						articles.text.content = aliOHFollowRelayTitle;
					}
				}
				else if ("unfollow".Equals(eventType))
				{
					if (!string.IsNullOrEmpty(FromUserId))
					{
						MemberProcessor.DelFuwuFollowUser(FromUserId);
					}
				}
				else if ("click".Equals(eventType))
				{
					int num = 0;
					if (ActionParam != "" && int.TryParse(ActionParam, out num) && num > 0)
					{
						Hidistro.Entities.VShop.MenuInfo fuwuMenu = VShopHelper.GetFuwuMenu(num);
						if (fuwuMenu != null)
						{
							Hidistro.Entities.VShop.ReplyInfo reply = AliFuwuReplyHelper.GetReply(fuwuMenu.ReplyId);
							if (reply != null)
							{
								ArticleInfo articleInfo2 = ArticleHelper.GetArticleInfo(reply.ArticleID);
								if (articleInfo2 != null)
								{
									articles = this.GetAlipayArticlesFromArticleInfo(articleInfo2, Globals.HostPath(System.Web.HttpContext.Current.Request.Url), FromUserId);
								}
							}
						}
					}
				}
				else if ("enter".Equals(eventType))
				{
					if (!string.IsNullOrEmpty(this.UserInfo))
					{
						MemberInfo openIdMember = MemberProcessor.GetOpenIdMember(FromUserId, "fuwu");
						if (openIdMember != null && openIdMember.AlipayLoginId.StartsWith("FW*"))
						{
							JObject jObject = JsonConvert.DeserializeObject(this.UserInfo) as JObject;
							string alipayLoginId = "";
							string text = "";
							if (jObject["logon_id"] != null)
							{
								alipayLoginId = jObject["logon_id"].ToString();
							}
							if (jObject["user_name"] != null)
							{
								text = jObject["user_name"].ToString();
							}
							if (text != "" && text != "")
							{
								openIdMember.AlipayLoginId = alipayLoginId;
								openIdMember.AlipayUsername = text;
								MemberProcessor.SetAlipayInfos(openIdMember);
							}
						}
					}
					if (!ActionParam.Contains("sceneId"))
					{
						return;
					}
					JObject jObject2 = JsonConvert.DeserializeObject(ActionParam) as JObject;
					if (jObject2["scene"]["sceneId"] != null)
					{
						string text2 = jObject2["scene"]["sceneId"].ToString();
						if (text2.StartsWith("bind"))
						{
							if (AlipayFuwuConfig.BindAdmin.Count > 10)
							{
								AlipayFuwuConfig.BindAdmin.Clear();
							}
							if (AlipayFuwuConfig.BindAdmin.ContainsKey(text2))
							{
								AlipayFuwuConfig.BindAdmin[text2] = FromUserId;
							}
							else
							{
								AlipayFuwuConfig.BindAdmin.Add(text2, FromUserId);
							}
							articles.text.content = "您正在尝试绑定服务窗管理员身份！";
						}
					}
				}
			}
			else if (textContent != "")
			{
				articles = null;
				System.Collections.Generic.IList<Hidistro.Entities.VShop.ReplyInfo> replies = AliFuwuReplyHelper.GetReplies(ReplyType.Keys);
				if (replies != null && replies.Count > 0)
				{
					foreach (Hidistro.Entities.VShop.ReplyInfo current in replies)
					{
						if (current != null)
						{
							if (current.MatchType == MatchType.Equal && current.Keys == textContent)
							{
								if (current.MessageType == MessageType.Text)
								{
									articles = new Articles
									{
										toUserId = FromUserId,
										msgType = "text",
										text = new MessageText
										{
											content = ""
										}
									};
									TextReplyInfo textReplyInfo2 = current as TextReplyInfo;
									articles.text.content = textReplyInfo2.Text;
									break;
								}
								if (current.ArticleID > 0)
								{
									ArticleInfo articleInfo3 = ArticleHelper.GetArticleInfo(current.ArticleID);
									if (articleInfo3 != null)
									{
										articles = this.GetAlipayArticlesFromArticleInfo(articleInfo3, Globals.HostPath(System.Web.HttpContext.Current.Request.Url), FromUserId);
										if (articles != null)
										{
											break;
										}
									}
								}
							}
							if (current.MatchType == MatchType.Like && current.Keys.Contains(textContent))
							{
								if (current.MessageType == MessageType.Text)
								{
									articles = new Articles
									{
										toUserId = FromUserId,
										msgType = "text",
										text = new MessageText
										{
											content = ""
										}
									};
									TextReplyInfo textReplyInfo3 = current as TextReplyInfo;
									articles.text.content = textReplyInfo3.Text;
									break;
								}
								if (current.ArticleID > 0)
								{
									ArticleInfo articleInfo4 = ArticleHelper.GetArticleInfo(current.ArticleID);
									if (articleInfo4 != null)
									{
										articles = this.GetAlipayArticlesFromArticleInfo(articleInfo4, Globals.HostPath(System.Web.HttpContext.Current.Request.Url), FromUserId);
										if (articles != null)
										{
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			if (articles == null)
			{
				System.Collections.Generic.IList<Hidistro.Entities.VShop.ReplyInfo> replies2 = AliFuwuReplyHelper.GetReplies(ReplyType.NoMatch);
				if (replies2 != null && replies2.Count > 0)
				{
					using (System.Collections.Generic.IEnumerator<Hidistro.Entities.VShop.ReplyInfo> enumerator2 = replies2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Hidistro.Entities.VShop.ReplyInfo current2 = enumerator2.Current;
							if (current2.MessageType == MessageType.Text)
							{
								articles = new Articles
								{
									toUserId = FromUserId,
									msgType = "text",
									text = new MessageText
									{
										content = ""
									}
								};
								TextReplyInfo textReplyInfo4 = current2 as TextReplyInfo;
								articles.text.content = textReplyInfo4.Text;
								break;
							}
							if (current2.ArticleID > 0)
							{
								ArticleInfo articleInfo5 = ArticleHelper.GetArticleInfo(current2.ArticleID);
								if (articleInfo5 != null)
								{
									articles = this.GetAlipayArticlesFromArticleInfo(articleInfo5, Globals.HostPath(System.Web.HttpContext.Current.Request.Url), FromUserId);
									if (articles != null)
									{
										break;
									}
								}
							}
						}
						goto IL_674;
					}
				}
				articles = new Articles
				{
					toUserId = FromUserId,
					msgType = "text",
					text = new MessageText
					{
						content = "系统未找到相关信息！"
					}
				};
			}
			IL_674:
			AliOHHelper.log(AliOHHelper.CustomSend(articles).Body);
		}

		private Articles GetAlipayArticlesFromArticleInfo(ArticleInfo articleInfo, string storeUrl, string FromUserid)
		{
			Articles articles = null;
			if (articleInfo != null)
			{
				articles = new Articles();
				articles.toUserId = FromUserid;
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
	}
}
