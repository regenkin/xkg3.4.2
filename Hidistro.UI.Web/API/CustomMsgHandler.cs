using ControlPanel.WeiBo;
using ControlPanel.WeiXin;
using Hidistro.ControlPanel.Store;
using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.Entities.Weibo;
using Hidistro.SaleSystem.Vshop;
using Hishop.Weixin.MP;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Hishop.Weixin.MP.Handler;
using Hishop.Weixin.MP.Request;
using Hishop.Weixin.MP.Request.Event;
using Hishop.Weixin.MP.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class CustomMsgHandler : RequestHandler
	{
		public CustomMsgHandler(System.IO.Stream inputStream) : base(inputStream)
		{
		}

		public bool IsOpenManyService()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			return masterSettings.OpenManyService;
		}

		public AbstractResponse GotoManyCustomerService(AbstractRequest requestMessage)
		{
			WeiXinHelper.UpdateRencentOpenID(requestMessage.FromUserName);
			if (!this.IsOpenManyService())
			{
				return null;
			}
			return new AbstractResponse
			{
				FromUserName = requestMessage.ToUserName,
				ToUserName = requestMessage.FromUserName,
				MsgType = ResponseMsgType.transfer_customer_service
			};
		}

		public override AbstractResponse DefaultResponse(AbstractRequest requestMessage)
		{
			WeiXinHelper.UpdateRencentOpenID(requestMessage.FromUserName);
			Hidistro.Entities.VShop.ReplyInfo mismatchReply = ReplyHelper.GetMismatchReply();
			if (mismatchReply == null || this.IsOpenManyService())
			{
				return this.GotoManyCustomerService(requestMessage);
			}
			AbstractResponse response = this.GetResponse(mismatchReply, requestMessage.FromUserName);
			if (response == null)
			{
				return this.GotoManyCustomerService(requestMessage);
			}
			response.ToUserName = requestMessage.FromUserName;
			response.FromUserName = requestMessage.ToUserName;
			return response;
		}

		public override AbstractResponse OnTextRequest(TextRequest textRequest)
		{
			WeiXinHelper.UpdateRencentOpenID(textRequest.FromUserName);
			AbstractResponse keyResponse = this.GetKeyResponse(textRequest.Content, textRequest);
			if (keyResponse != null)
			{
				return keyResponse;
			}
			System.Collections.Generic.IList<Hidistro.Entities.VShop.ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Keys);
			if (replies == null || (replies.Count == 0 && this.IsOpenManyService()))
			{
				this.GotoManyCustomerService(textRequest);
			}
			foreach (Hidistro.Entities.VShop.ReplyInfo current in replies)
			{
				if (current.MatchType == MatchType.Equal && current.Keys == textRequest.Content)
				{
					AbstractResponse response = this.GetResponse(current, textRequest.FromUserName);
					response.ToUserName = textRequest.FromUserName;
					response.FromUserName = textRequest.ToUserName;
					AbstractResponse result = response;
					return result;
				}
				if (current.MatchType == MatchType.Like && current.Keys.Contains(textRequest.Content))
				{
					AbstractResponse response2 = this.GetResponse(current, textRequest.FromUserName);
					response2.ToUserName = textRequest.FromUserName;
					response2.FromUserName = textRequest.ToUserName;
					AbstractResponse result = response2;
					return result;
				}
			}
			return this.DefaultResponse(textRequest);
		}

		public override AbstractResponse OnEvent_SubscribeRequest(SubscribeEventRequest subscribeEventRequest)
		{
			string text = "";
			if (subscribeEventRequest.EventKey != null)
			{
				text = subscribeEventRequest.EventKey;
			}
			if (text.Contains("qrscene_"))
			{
				text = text.Replace("qrscene_", "").Trim();
				if (text == "1")
				{
					if (WeiXinHelper.BindAdminOpenId.Count > 10)
					{
						WeiXinHelper.BindAdminOpenId.Clear();
					}
					if (WeiXinHelper.BindAdminOpenId.ContainsKey(subscribeEventRequest.Ticket))
					{
						WeiXinHelper.BindAdminOpenId[subscribeEventRequest.Ticket] = subscribeEventRequest.FromUserName;
					}
					else
					{
						WeiXinHelper.BindAdminOpenId.Add(subscribeEventRequest.Ticket, subscribeEventRequest.FromUserName);
					}
					return new TextResponse
					{
						CreateTime = System.DateTime.Now,
						Content = "您正在扫描尝试绑定管理员身份，身份已识别",
						ToUserName = subscribeEventRequest.FromUserName,
						FromUserName = subscribeEventRequest.ToUserName
					};
				}
				ScanInfos scanInfosByTicket = ScanHelp.GetScanInfosByTicket(subscribeEventRequest.Ticket);
				Globals.Debuglog(text + ":" + subscribeEventRequest.Ticket, "_Debuglog.txt");
				if (!MemberProcessor.IsExitOpenId(subscribeEventRequest.FromUserName) && scanInfosByTicket != null && scanInfosByTicket.BindUserId > 0)
				{
					this.CreatMember(subscribeEventRequest.FromUserName, scanInfosByTicket.BindUserId);
					ScanHelp.updateScanInfosLastActiveTime(System.DateTime.Now, scanInfosByTicket.Sceneid);
				}
			}
			WeiXinHelper.UpdateRencentOpenID(subscribeEventRequest.FromUserName);
			Hidistro.Entities.VShop.ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
			if (subscribeReply == null)
			{
				return null;
			}
			subscribeReply.Keys = "登录";
			AbstractResponse response = this.GetResponse(subscribeReply, subscribeEventRequest.FromUserName);
			if (response == null)
			{
				this.GotoManyCustomerService(subscribeEventRequest);
			}
			response.ToUserName = subscribeEventRequest.FromUserName;
			response.FromUserName = subscribeEventRequest.ToUserName;
			return response;
		}

		private bool CreatMember(string OpenId, int ReferralUserId)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			string token_Message = TokenApi.GetToken_Message(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
			string urlToDecode = "";
			string userHead = "";
			string text = "";
			BarCodeApi.GetHeadImageUrlByOpenID(token_Message, OpenId, out text, out urlToDecode, out userHead);
			string generateId = Globals.GetGenerateId();
			MemberInfo memberInfo = new MemberInfo();
			memberInfo.GradeId = MemberProcessor.GetDefaultMemberGrade();
			memberInfo.UserName = Globals.UrlDecode(urlToDecode);
			memberInfo.OpenId = OpenId;
			memberInfo.CreateDate = System.DateTime.Now;
			memberInfo.SessionId = generateId;
			memberInfo.SessionEndTime = System.DateTime.Now.AddYears(10);
			memberInfo.UserHead = userHead;
			memberInfo.ReferralUserId = ReferralUserId;
			memberInfo.Password = HiCryptographer.Md5Encrypt("888888");
			Globals.Debuglog(JsonConvert.SerializeObject(memberInfo), "_Debuglog.txt");
			return MemberProcessor.CreateMember(memberInfo);
		}

		public override AbstractResponse OnEvent_ClickRequest(ClickEventRequest clickEventRequest)
		{
			WeiXinHelper.UpdateRencentOpenID(clickEventRequest.FromUserName);
			int menuId = System.Convert.ToInt32(clickEventRequest.EventKey);
			Hidistro.Entities.VShop.MenuInfo menu = VShopHelper.GetMenu(menuId);
			if (menu == null)
			{
				return null;
			}
			Hidistro.Entities.VShop.ReplyInfo reply = ReplyHelper.GetReply(menu.ReplyId);
			if (reply == null)
			{
				return null;
			}
			AbstractResponse keyResponse = this.GetKeyResponse(reply.Keys, clickEventRequest);
			if (keyResponse != null)
			{
				return keyResponse;
			}
			AbstractResponse response = this.GetResponse(reply, clickEventRequest.FromUserName);
			if (response == null)
			{
				this.GotoManyCustomerService(clickEventRequest);
			}
			response.ToUserName = clickEventRequest.FromUserName;
			response.FromUserName = clickEventRequest.ToUserName;
			return response;
		}

		public override AbstractResponse OnEvent_MassendJobFinishEventRequest(MassendJobFinishEventRequest massendJobFinishEventRequest)
		{
			AbstractResponse result = null;
			string text = string.Concat(new object[]
			{
				"公众号的微信号(加密的):",
				massendJobFinishEventRequest.ToUserName,
				",发送完成时间：",
				massendJobFinishEventRequest.CreateTime,
				"，过滤通过条数：",
				massendJobFinishEventRequest.FilterCount,
				"，发送失败的粉丝数：",
				massendJobFinishEventRequest.ErrorCount
			});
			string status;
			switch (status = massendJobFinishEventRequest.Status)
			{
			case "send success":
				text += "(发送成功)";
				goto IL_20C;
			case "send fail":
				text += "(发送失败)";
				goto IL_20C;
			case "err(10001)":
				text += "(涉嫌广告)";
				goto IL_20C;
			case "err(20001)":
				text += "(涉嫌政治)";
				goto IL_20C;
			case "err(20004)":
				text += "(涉嫌社会)";
				goto IL_20C;
			case "err(20002)":
				text += "(涉嫌色情)";
				goto IL_20C;
			case "err(20006)":
				text += "(涉嫌违法犯罪)";
				goto IL_20C;
			case "err(20008)":
				text += "(涉嫌欺诈)";
				goto IL_20C;
			case "err(20013)":
				text += "(涉嫌版权)";
				goto IL_20C;
			case "err(22000)":
				text += "(涉嫌互相宣传)";
				goto IL_20C;
			case "err(21000)":
				text += "(涉嫌其他)";
				goto IL_20C;
			}
			text = text + "(" + massendJobFinishEventRequest.Status + ")";
			IL_20C:
			WeiXinHelper.UpdateMsgId(0, massendJobFinishEventRequest.MsgId.ToString(), (massendJobFinishEventRequest.Status == "send success") ? 1 : 2, Globals.ToNum(massendJobFinishEventRequest.SentCount), Globals.ToNum(massendJobFinishEventRequest.TotalCount), text);
			return result;
		}

		public override AbstractResponse OnEvent_ScanRequest(ScanEventRequest scanEventRequest)
		{
			string eventKey = scanEventRequest.EventKey;
			if (eventKey == "1")
			{
				if (WeiXinHelper.BindAdminOpenId.Count > 10)
				{
					WeiXinHelper.BindAdminOpenId.Clear();
				}
				if (WeiXinHelper.BindAdminOpenId.ContainsKey(scanEventRequest.Ticket))
				{
					WeiXinHelper.BindAdminOpenId[scanEventRequest.Ticket] = scanEventRequest.FromUserName;
				}
				else
				{
					WeiXinHelper.BindAdminOpenId.Add(scanEventRequest.Ticket, scanEventRequest.FromUserName);
				}
				return new TextResponse
				{
					CreateTime = System.DateTime.Now,
					Content = "您正在扫描尝试绑定管理员身份，身份已识别",
					ToUserName = scanEventRequest.FromUserName,
					FromUserName = scanEventRequest.ToUserName
				};
			}
			ScanInfos scanInfosByTicket = ScanHelp.GetScanInfosByTicket(scanEventRequest.Ticket);
			Globals.Debuglog(eventKey + ":" + scanEventRequest.Ticket, "_Debuglog.txt");
			bool flag = MemberProcessor.IsExitOpenId(scanEventRequest.FromUserName);
			if (!flag && scanInfosByTicket != null && scanInfosByTicket.BindUserId > 0)
			{
				this.CreatMember(scanEventRequest.FromUserName, scanInfosByTicket.BindUserId);
			}
			if (scanInfosByTicket != null)
			{
				ScanHelp.updateScanInfosLastActiveTime(System.DateTime.Now, scanInfosByTicket.Sceneid);
			}
			if (flag)
			{
				return new TextResponse
				{
					CreateTime = System.DateTime.Now,
					Content = "您刚扫描了分销商公众号二维码！",
					ToUserName = scanEventRequest.FromUserName,
					FromUserName = scanEventRequest.ToUserName
				};
			}
			Hidistro.Entities.VShop.ReplyInfo subscribeReply = ReplyHelper.GetSubscribeReply();
			if (subscribeReply == null)
			{
				return null;
			}
			subscribeReply.Keys = "扫描";
			AbstractResponse response = this.GetResponse(subscribeReply, scanEventRequest.FromUserName);
			response.ToUserName = scanEventRequest.FromUserName;
			response.FromUserName = scanEventRequest.ToUserName;
			return response;
		}

		private void SaveLog(string LogInfo)
		{
			System.IO.StreamWriter streamWriter = System.IO.File.AppendText("\\Logty_Scan.txt");
			streamWriter.WriteLine(LogInfo);
			streamWriter.WriteLine(System.DateTime.Now);
			streamWriter.Flush();
			streamWriter.Close();
		}

		private AbstractResponse GetKeyResponse(string key, AbstractRequest request)
		{
			System.Collections.Generic.IList<Hidistro.Entities.VShop.ReplyInfo> replies = ReplyHelper.GetReplies(ReplyType.Vote);
			if (replies != null && replies.Count > 0)
			{
				foreach (Hidistro.Entities.VShop.ReplyInfo current in replies)
				{
					if (current.Keys == key)
					{
						VoteInfo voteById = StoreHelper.GetVoteById((long)current.ActivityId);
						if (voteById != null && voteById.IsBackup)
						{
							return new NewsResponse
							{
								CreateTime = System.DateTime.Now,
								FromUserName = request.ToUserName,
								ToUserName = request.FromUserName,
								Articles = new System.Collections.Generic.List<Article>
								{
									new Article
									{
										Description = voteById.VoteName,
										PicUrl = this.FormatImgUrl(voteById.ImageUrl),
										Title = voteById.VoteName,
										Url = string.Format("http://{0}/vshop/Vote.aspx?voteId={1}", System.Web.HttpContext.Current.Request.Url.Host, voteById.VoteId)
									}
								}
							};
						}
					}
				}
			}
			return null;
		}

		private string FormatImgUrl(string img)
		{
			if (!img.StartsWith("http"))
			{
				img = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, img);
			}
			return img;
		}

		public AbstractResponse GetResponse(Hidistro.Entities.VShop.ReplyInfo reply, string openId)
		{
			if (reply.MessageType == MessageType.Text)
			{
				TextReplyInfo textReplyInfo = reply as TextReplyInfo;
				TextResponse textResponse = new TextResponse();
				textResponse.CreateTime = System.DateTime.Now;
				textResponse.Content = textReplyInfo.Text;
				if (reply.Keys == "登录")
				{
					string arg = string.Format("http://{0}/Vshop/Login.aspx?SessionId={1}", System.Web.HttpContext.Current.Request.Url.Host, openId);
					textResponse.Content = textResponse.Content.Replace("$login$", string.Format("<a href=\"{0}\">一键登录</a>", arg));
				}
				return textResponse;
			}
			NewsResponse newsResponse = new NewsResponse();
			newsResponse.CreateTime = System.DateTime.Now;
			newsResponse.Articles = new System.Collections.Generic.List<Article>();
			if (reply.ArticleID > 0)
			{
				ArticleInfo articleInfo = ArticleHelper.GetArticleInfo(reply.ArticleID);
				if (articleInfo.ArticleType == ArticleType.News)
				{
					Article item = new Article
					{
						Description = articleInfo.Memo,
						PicUrl = this.FormatImgUrl(articleInfo.ImageUrl),
						Title = articleInfo.Title,
						Url = string.IsNullOrEmpty(articleInfo.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?sid={1}", System.Web.HttpContext.Current.Request.Url.Host, articleInfo.ArticleId) : articleInfo.Url
					};
					newsResponse.Articles.Add(item);
					return newsResponse;
				}
				if (articleInfo.ArticleType != ArticleType.List)
				{
					return newsResponse;
				}
				Article item2 = new Article
				{
					Description = articleInfo.Memo,
					PicUrl = this.FormatImgUrl(articleInfo.ImageUrl),
					Title = articleInfo.Title,
					Url = string.IsNullOrEmpty(articleInfo.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?sid={1}", System.Web.HttpContext.Current.Request.Url.Host, articleInfo.ArticleId) : articleInfo.Url
				};
				newsResponse.Articles.Add(item2);
				using (System.Collections.Generic.IEnumerator<ArticleItemsInfo> enumerator = articleInfo.ItemsInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ArticleItemsInfo current = enumerator.Current;
						item2 = new Article
						{
							Description = "",
							PicUrl = this.FormatImgUrl(current.ImageUrl),
							Title = current.Title,
							Url = string.IsNullOrEmpty(current.Url) ? string.Format("http://{0}/Vshop/ArticleDetail.aspx?iid={1}", System.Web.HttpContext.Current.Request.Url.Host, current.Id) : current.Url
						};
						newsResponse.Articles.Add(item2);
					}
					return newsResponse;
				}
			}
			foreach (NewsMsgInfo current2 in (reply as NewsReplyInfo).NewsMsg)
			{
				Article item3 = new Article
				{
					Description = current2.Description,
					PicUrl = string.Format("http://{0}{1}", System.Web.HttpContext.Current.Request.Url.Host, current2.PicUrl),
					Title = current2.Title,
					Url = string.IsNullOrEmpty(current2.Url) ? string.Format("http://{0}/Vshop/ImageTextDetails.aspx?messageId={1}", System.Web.HttpContext.Current.Request.Url.Host, current2.Id) : current2.Url
				};
				newsResponse.Articles.Add(item3);
			}
			return newsResponse;
		}
	}
}
