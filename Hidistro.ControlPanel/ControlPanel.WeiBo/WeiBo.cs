using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Weibo;
using Hishop.WeiBo.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace ControlPanel.WeiBo
{
	public class WeiBo
	{
		public class user
		{
			public string uid
			{
				get;
				set;
			}
		}

		private SinaWeiboClient sinaweibo;

		public WeiBo()
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			this.sinaweibo = new GetAuth().GetOpenAuthClient(masterSettings.Access_Token);
		}

		public string friends_timeline(int page)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("statuses/friends_timeline.json", new
				{
					page
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string user_timeline(int page)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("statuses/user_timeline.json", new
				{
					page
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string userinfo()
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				string uid = this.get_uid();
				if (string.IsNullOrEmpty(uid))
				{
					result = "{\"IsAuthorized\":\"0\"}";
				}
				else
				{
					HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("users/show.json", new
					{
						uid = this.get_uid()
					});
					result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				}
			}
			return result;
		}

		public string statusesupdate(string status, string img)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				FileInfo fileInfo = new FileInfo(HttpContext.Current.Server.MapPath(img));
				if (fileInfo.Exists)
				{
					HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("statuses/upload.json", new
					{
						status = status,
						pic = fileInfo
					});
					result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				}
				else
				{
					HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("statuses/update.json", new
					{
						status
					});
					result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				}
			}
			return result;
		}

		public string get_uid()
		{
			string text = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("account/get_uid.json", new
				{

				});
				text = httpResponseMessage.Content.ReadAsStringAsync().Result;
				text = JsonConvert.DeserializeObject<WeiBo.user>(text).uid;
			}
			return text;
		}

		public string getfriends()
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("friendships/friends.json", new
				{
					uid = this.get_uid()
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string commentscreate(string id, string comment)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("comments/create.json", new
				{
					id,
					comment
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string repost(string id, string comment)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("statuses/repost.json", new
				{
					id = id,
					status = comment
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string createmenu(string comment)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("https://m.api.weibo.com/2/messages/menu/create.json", new
				{
					menus = comment
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string showemenu()
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("https://m.api.weibo.com/2/messages/menu/show.json", new
				{

				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string deletemenu()
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("https://m.api.weibo.com/2/messages/menu/delete.json", new
				{

				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string sendmessage(string type, string receiver_id, string data)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpPost("https://m.api.weibo.com/2/messages/reply.json", new
				{
					type = type,
					receiver_id = receiver_id,
					data = HttpUtility.UrlDecode(data)
				});
				result = httpResponseMessage.Content.ReadAsStringAsync().Result;
			}
			return result;
		}

		public string userinfo(string id)
		{
			string result = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				string uid = this.get_uid();
				if (string.IsNullOrEmpty(uid))
				{
					result = "{\"IsAuthorized\":\"0\"}";
				}
				else
				{
					HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("users/show.json", new
					{
						uid = id
					});
					result = httpResponseMessage.Content.ReadAsStringAsync().Result;
				}
			}
			return result;
		}

		public string SendToUIDMessage(string msgtype, string displayname, string summary, string image, string url, string Content, string ArticleId)
		{
			string text = "{\"IsAuthorized\":\"0\"}";
			if (this.sinaweibo.IsAuthorized)
			{
				HttpResponseMessage httpResponseMessage = this.sinaweibo.HttpGet("https://m.api.weibo.com/2/messages/subscribers/get.json", new
				{

				});
				text = httpResponseMessage.Content.ReadAsStringAsync().Result;
				JObject jObject = JObject.Parse(text);
				string text2 = "{\"touser\": [";
				if (jObject["data"] != null)
				{
					JObject jObject2 = JObject.Parse(jObject["data"].ToString());
					if (jObject2["uids"] != null)
					{
						using (IEnumerator<JToken> enumerator = ((IEnumerable<JToken>)jObject2["uids"]).GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string str = (string)enumerator.Current;
								text2 = text2 + "\"" + str + "\",";
							}
						}
						text2 = text2.Substring(0, text2.Length - 1);
						string text3 = "\"text\": {\"content\": \"" + Content + "\"}";
						string text6;
						if (msgtype == "articles")
						{
							if (string.IsNullOrEmpty(summary))
							{
								summary = displayname;
							}
							string text4 = string.Concat(new string[]
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
							IList<ArticleItemsInfo> articleItems = ArticleHelper.GetArticleItems(int.Parse(ArticleId));
							if (articleItems.Count > 0)
							{
								foreach (ArticleItemsInfo current in articleItems)
								{
									string text5;
									if (string.IsNullOrEmpty(current.Content))
									{
										text5 = current.Title;
									}
									else
									{
										text5 = current.Content;
									}
									text6 = text4;
									text4 = string.Concat(new string[]
									{
										text6,
										"{\"display_name\": \"",
										current.Title,
										"\",\"summary\":\"",
										text5,
										"\",\"image\":\"http://",
										Globals.DomainName,
										current.ImageUrl,
										"\",\"url\":\"",
										current.Url,
										"\"},"
									});
								}
							}
							text4 = text4.Substring(0, text4.Length - 1);
							text3 = "\"articles\": [" + text4 + "]";
						}
						text6 = text2;
						text2 = string.Concat(new string[]
						{
							text6,
							"],",
							text3,
							",\"msgtype\": \"",
							msgtype,
							"\"}"
						});
						text = this.SendMsg(HttpUtility.UrlDecode(text2), "https://m.api.weibo.com/2/messages/sendall.json?access_token=" + this.sinaweibo.AccessToken);
					}
					else
					{
						text = "{\"result\":false\"}";
					}
				}
			}
			return text;
		}

		public string SendMsg(string sendall, string url)
		{
			string result;
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(sendall);
				HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
				Encoding uTF = Encoding.UTF8;
				httpWebRequest.Method = "POST";
				httpWebRequest.KeepAlive = false;
				httpWebRequest.AllowAutoRedirect = true;
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727; .NET CLR  3.0.04506.648; .NET CLR 3.5.21022; .NET CLR 3.0.4506.2152; .NET CLR 3.5.30729)";
				httpWebRequest.ContentLength = (long)bytes.Length;
				Stream requestStream = httpWebRequest.GetRequestStream();
				requestStream.Write(bytes, 0, bytes.Length);
				requestStream.Close();
				HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
				string text = streamReader.ReadToEnd();
				string text2 = text;
				streamReader.Close();
				result = text2;
			}
			catch
			{
				result = "{\"result\":false}";
			}
			return result;
		}
	}
}
