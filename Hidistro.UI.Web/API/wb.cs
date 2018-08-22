using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Weibo;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class wb : System.Web.IHttpHandler
	{
		public enum MessageType
		{
			text,
			position,
			voice,
			image,
			mesevent,
			mention
		}

		private string app_secret = "";

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			System.Web.HttpRequest request = context.Request;
			this.app_secret = SettingsManager.GetMasterSettings(false).App_Secret;
			string signature = request["signature"];
			string nonce = request["nonce"];
			string timestamp = request["timestamp"];
			string s = request["echostr"];
			if (request.HttpMethod == "GET")
			{
				bool flag = this.ValidateSHA(signature, nonce, timestamp);
				if (flag)
				{
					context.Response.Write(s);
				}
				else
				{
					context.Response.Write("");
				}
				context.Response.End();
				return;
			}
			try
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				string text = "";
				byte[] array = new byte[request.InputStream.Length];
				request.InputStream.Read(array, 0, array.Length);
				string text2 = System.Text.Encoding.UTF8.GetString(array);
				text2 = System.Web.HttpContext.Current.Server.UrlDecode(text2);
				stringBuilder.Append(text2);
				System.IO.StreamWriter streamWriter = System.IO.File.AppendText(context.Server.MapPath("wberror.txt"));
				streamWriter.WriteLine(text2);
				streamWriter.WriteLine(System.DateTime.Now + "-1");
				streamWriter.Flush();
				streamWriter.Close();
				int messageId = 0;
				JObject jObject = JObject.Parse(text2);
				if ((jObject["type"] != null && jObject["type"].ToString() == wb.MessageType.text.ToString()) || (jObject["type"] != null && jObject["type"].ToString() == wb.MessageType.image.ToString()) || (jObject["type"] != null && jObject["type"].ToString() == wb.MessageType.voice.ToString()))
				{
					MessageInfo messageInfo = new MessageInfo();
					messageInfo.Created_at = System.DateTime.Now.ToString();
					messageInfo.Receiver_id = jObject["receiver_id"].ToString();
					messageInfo.Sender_id = jObject["sender_id"].ToString();
					messageInfo.Text = jObject["text"].ToString();
					messageInfo.Type = jObject["type"].ToString();
					messageInfo.Status = 0;
					if (jObject["data"] != null)
					{
						JObject jObject2 = JObject.Parse(jObject["data"].ToString());
						if (jObject2["tovfid"] != null)
						{
							messageInfo.Tovfid = jObject2["tovfid"].ToString();
						}
						if (jObject2["vfid"] != null)
						{
							messageInfo.Vfid = jObject2["vfid"].ToString();
						}
					}
					messageInfo.Access_Token = SettingsManager.GetMasterSettings(false).Access_Token;
					messageId = WeiboHelper.SaveMessage(messageInfo);
				}
				if (jObject["type"] != null && jObject["type"].ToString() == wb.MessageType.text.ToString())
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					if (masterSettings.CustomReply)
					{
						string senderId = jObject["receiver_id"].ToString();
						string receiverId = jObject["sender_id"].ToString();
						System.Data.DataView defaultView = WeiboHelper.GetReplyAll(1).DefaultView;
						if (defaultView.Count > 0)
						{
							defaultView.RowFilter = "Keys='" + jObject["text"].ToString() + "'";
							if (defaultView.Count > 0)
							{
								System.Random random = new System.Random();
								int recordIndex = random.Next(0, defaultView.Count);
								if (defaultView[recordIndex]["ReceiverType"].ToString() == "text")
								{
									text = wb.generateReplyMsg(wb.textMsg(defaultView[recordIndex]["Content"].ToString(), messageId), "text", senderId, receiverId);
								}
								else
								{
									text = wb.generateReplyMsg(wb.articleMsg(defaultView[recordIndex]["Display_name"].ToString(), defaultView[recordIndex]["Summary"].ToString(), defaultView[recordIndex]["Image"].ToString(), defaultView[recordIndex]["Url"].ToString(), defaultView[recordIndex]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
								}
							}
							else
							{
								defaultView.RowFilter = "Keys like '%" + jObject["text"].ToString() + "%'";
								if (defaultView.Count > 0)
								{
									System.Random random2 = new System.Random();
									int recordIndex2 = random2.Next(0, defaultView.Count);
									if (defaultView[recordIndex2]["ReceiverType"].ToString() == "text")
									{
										text = wb.generateReplyMsg(wb.textMsg(defaultView[recordIndex2]["Content"].ToString(), messageId), "text", senderId, receiverId);
									}
									else
									{
										text = wb.generateReplyMsg(wb.articleMsg(defaultView[recordIndex2]["Display_name"].ToString(), defaultView[recordIndex2]["Summary"].ToString(), defaultView[recordIndex2]["Image"].ToString(), defaultView[recordIndex2]["Url"].ToString(), defaultView[recordIndex2]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
									}
								}
							}
						}
					}
				}
				if (jObject["type"] != null && jObject["type"].ToString() == "event")
				{
					SiteSettings masterSettings2 = SettingsManager.GetMasterSettings(false);
					JObject jObject3 = JObject.Parse(jObject["data"].ToString());
					if (jObject3["subtype"].ToString().Trim() == "click")
					{
						System.IO.StreamWriter streamWriter2 = System.IO.File.AppendText(context.Server.MapPath("wberror.txt"));
						streamWriter2.WriteLine(masterSettings2.SubscribeReply ? "是" : "否");
						streamWriter2.WriteLine(System.DateTime.Now + "-6");
						streamWriter2.Flush();
						streamWriter2.Close();
					}
					else if (masterSettings2.SubscribeReply && jObject3["subtype"].ToString().Trim() == "subscribe")
					{
						string senderId = jObject["receiver_id"].ToString();
						string receiverId = jObject["sender_id"].ToString();
						System.Data.DataView defaultView2 = WeiboHelper.GetWeibo_Reply(2).DefaultView;
						if (defaultView2.Count > 0)
						{
							System.Random random3 = new System.Random();
							int recordIndex3 = random3.Next(0, defaultView2.Count);
							if (defaultView2[recordIndex3]["ReceiverType"].ToString() == "text")
							{
								text = wb.generateReplyMsg(wb.textMsg(defaultView2[recordIndex3]["Content"].ToString(), messageId), "text", senderId, receiverId);
							}
							else
							{
								text = wb.generateReplyMsg(wb.articleMsg(defaultView2[recordIndex3]["Display_name"].ToString(), defaultView2[recordIndex3]["Summary"].ToString(), defaultView2[recordIndex3]["Image"].ToString(), defaultView2[recordIndex3]["Url"].ToString(), defaultView2[recordIndex3]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
							}
						}
					}
				}
				if (jObject["type"] != null && jObject["type"].ToString() == "mention")
				{
					SiteSettings masterSettings3 = SettingsManager.GetMasterSettings(false);
					if (masterSettings3.ByRemind)
					{
						JObject.Parse(jObject["data"].ToString());
						string senderId = jObject["receiver_id"].ToString();
						string receiverId = jObject["sender_id"].ToString();
						System.Data.DataView defaultView3 = WeiboHelper.GetWeibo_Reply(3).DefaultView;
						if (defaultView3.Count > 0)
						{
							System.Random random4 = new System.Random();
							int recordIndex4 = random4.Next(0, defaultView3.Count);
							if (defaultView3[recordIndex4]["ReceiverType"].ToString() == "text")
							{
								text = wb.generateReplyMsg(wb.textMsg(defaultView3[recordIndex4]["Content"].ToString(), messageId), "text", senderId, receiverId);
							}
							else
							{
								text = wb.generateReplyMsg(wb.articleMsg(defaultView3[recordIndex4]["Display_name"].ToString(), defaultView3[recordIndex4]["Summary"].ToString(), defaultView3[recordIndex4]["Image"].ToString(), defaultView3[recordIndex4]["Url"].ToString(), defaultView3[recordIndex4]["ArticleId"].ToString(), messageId), "articles", senderId, receiverId);
							}
						}
					}
				}
				context.Response.Write(text);
				System.IO.StreamWriter streamWriter3 = System.IO.File.AppendText(context.Server.MapPath("wberror.txt"));
				streamWriter3.WriteLine(text);
				streamWriter3.WriteLine(System.DateTime.Now + "-2");
				streamWriter3.Flush();
				streamWriter3.Close();
			}
			catch (System.Exception ex)
			{
				System.IO.StreamWriter streamWriter4 = System.IO.File.AppendText(context.Server.MapPath("wberror.txt"));
				streamWriter4.WriteLine(ex.Message);
				streamWriter4.WriteLine(ex.StackTrace);
				streamWriter4.WriteLine(System.DateTime.Now + "-3");
				streamWriter4.Flush();
				streamWriter4.Close();
			}
		}

		private static string articleMsg(string display_name, string summary, string image, string url, string ArticleId, int MessageId)
		{
			new JObject();
			string text = "[";
			if (summary == "")
			{
				summary = display_name;
			}
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				"{\"display_name\":\"",
				Globals.String2Json(display_name),
				"\",\"summary\":\"",
				Globals.String2Json(summary),
				"\",\"image\":\"",
				image,
				"\",\"url\":\"",
				url,
				"\"},"
			});
			if (!string.IsNullOrEmpty(ArticleId) && int.Parse(ArticleId) > 0)
			{
				System.Collections.Generic.IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(ArticleId));
				if (articleItems.Count > 0)
				{
					foreach (ArticleItemsInfo current in articleItems)
					{
						string s = "";
						if (current.Content.Trim() == "")
						{
							s = current.Title;
						}
						string text3 = text;
						text = string.Concat(new string[]
						{
							text3,
							"{\"display_name\": \"",
							Globals.String2Json(current.Title),
							"\",\"summary\":\"",
							Globals.String2Json(s),
							"\",\"image\":\"http://",
							Globals.DomainName,
							current.ImageUrl,
							"\",\"url\":\"",
							current.Url,
							"\"},"
						});
					}
				}
			}
			WeiboHelper.UpdateMessage(new MessageInfo
			{
				SenderDate = System.DateTime.Now,
				DisplayName = display_name,
				Summary = summary,
				Image = image,
				Url = url,
				ArticleId = int.Parse(ArticleId),
				Status = 2,
				MessageId = MessageId
			});
			text = text.Substring(0, text.Length - 1);
			text += "]";
			return "{\"articles\":" + text.ToString() + "}";
		}

		private static string textMsg(string text, int MessageId)
		{
			JObject jObject = new JObject();
			jObject.Add("text", text);
			WeiboHelper.UpdateMessage(new MessageInfo
			{
				SenderDate = System.DateTime.Now,
				SenderMessage = text,
				Status = 2,
				MessageId = MessageId
			});
			return jObject.ToString();
		}

		private static string generateReplyMsg(string data, string type, string senderId, string receiverId)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append(string.Concat(new string[]
			{
				"\"result\":true,\"sender_id\":",
				senderId,
				",\"receiver_id\":",
				receiverId,
				",\"type\":\"",
				type,
				"\",\"data\":\"",
				System.Web.HttpContext.Current.Server.UrlEncode(data),
				"\""
			}));
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		private bool ValidateSHA(string signature, string nonce, string timestamp)
		{
			if (signature == null || nonce == null || timestamp == null)
			{
				return false;
			}
			byte[] bytes = System.Text.Encoding.Default.GetBytes(timestamp + nonce + this.app_secret);
			byte[] value = System.Security.Cryptography.SHA1.Create().ComputeHash(bytes);
			string value2 = System.BitConverter.ToString(value).Replace("-", "");
			return signature.ToUpper().Equals(value2);
		}
	}
}
