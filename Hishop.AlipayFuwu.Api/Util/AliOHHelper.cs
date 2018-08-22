using Aop.Api;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Hishop.AlipayFuwu.Api.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Xml;

namespace Hishop.AlipayFuwu.Api.Util
{
	public class AliOHHelper
	{
		private class EventType
		{
			public const string Verifygw = "verifygw";

			public const string Follow = "follow";

			public const string UnFollow = "unfollow";

			public const string Click = "click";

			public const string Enter = "enter";
		}

		private static object LockLog = new object();

		public static string templateSendMessage(AliTemplateMessage templateMessage, string actionName = "查看详情")
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			stringBuilder.AppendFormat("\"toUserId\":\"{0}\",", templateMessage.Touser);
			stringBuilder.AppendFormat("\"template\":{{ \"templateId\":\"{0}\",", templateMessage.TemplateId.ToString());
			stringBuilder.AppendFormat("\"context\":{{ \"headColor\":\"{0}\",", templateMessage.Topcolor);
			stringBuilder.AppendFormat("\"actionName\":\"{0}\",", actionName);
			if (!string.IsNullOrEmpty(templateMessage.Url))
			{
				stringBuilder.AppendFormat("\"url\":\"{0}\",", templateMessage.Url);
			}
			foreach (AliTemplateMessage.MessagePart current in templateMessage.Data)
			{
				stringBuilder.AppendFormat("\"{0}\":{{\"value\":\"{1}\",\"color\":\"{2}\"}},", current.Name, current.Value, current.Color);
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("}}}");
			return stringBuilder.ToString();
		}

		public static string AlipayAuthUrl(string returnUrl, string app_id, string scope = "auth_userinfo")
		{
			return "https://openauth.alipay.com/oauth2/publicAppAuthorize.htm?app_id=" + app_id + "&auth_skip=false&scope=auth_userinfo&redirect_uri=" + HttpUtility.UrlEncode(returnUrl);
		}

		public static AlipayMobilePublicMenuGetResponse MenuGet()
		{
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			AlipayMobilePublicMenuGetRequest request = new AlipayMobilePublicMenuGetRequest();
			return aopClient.Execute<AlipayMobilePublicMenuGetResponse>(request);
		}

		public static AlipayMobilePublicMenuAddResponse MenuAdd(FWMenu menu)
		{
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicMenuAddResponse>(new AlipayMobilePublicMenuAddRequest
			{
				BizContent = AliOHHelper.SerializeObject(menu, true)
			});
		}

		public static AlipayMobilePublicMenuUpdateResponse MenuUpdate(FWMenu menu)
		{
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicMenuUpdateResponse>(new AlipayMobilePublicMenuUpdateRequest
			{
				BizContent = AliOHHelper.SerializeObject(menu, true)
			});
		}

		public static AlipaySystemOauthTokenResponse GetOauthTokenResponse(string auth_code)
		{
			AlipayOHClient alipayOHClient = new AlipayOHClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.alipay_public_key, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.merchant_public_key, "UTF-8");
			return alipayOHClient.OauthTokenRequest(auth_code);
		}

		public static AlipayUserUserinfoShareResponse GetAlipayUserUserinfo(string AccessToken)
		{
			AlipayOHClient alipayOHClient = new AlipayOHClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.alipay_public_key, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.merchant_public_key, "UTF-8");
			return alipayOHClient.GetAliUserInfo(AccessToken);
		}

		public static string SerializeObject(object Articles, bool IgnoreNull = true)
		{
			JsonSerializerSettings settings = new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore
			};
			string result;
			if (IgnoreNull)
			{
				result = JsonConvert.SerializeObject(Articles, settings);
			}
			else
			{
				result = JsonConvert.SerializeObject(Articles);
			}
			return result;
		}

		public static System.Collections.Generic.Dictionary<string, string> getAlipayRequstParams(HttpContext context)
		{
			return new System.Collections.Generic.Dictionary<string, string>
			{
				{
					"service",
					AliOHHelper.getRequestString("service", context)
				},
				{
					"sign_type",
					AliOHHelper.getRequestString("sign_type", context)
				},
				{
					"charset",
					AliOHHelper.getRequestString("charset", context)
				},
				{
					"biz_content",
					AliOHHelper.getRequestString("biz_content", context)
				},
				{
					"sign",
					AliOHHelper.getRequestString("sign", context)
				}
			};
		}

		public static void verifygw(HttpContext context)
		{
			System.Collections.Generic.Dictionary<string, string> alipayRequstParams = AliOHHelper.getAlipayRequstParams(context);
			string xml = alipayRequstParams["biz_content"];
			if (!AliOHHelper.verifySignAlipayRequest(alipayRequstParams))
			{
				AliOHHelper.verifygwResponse(false, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context);
			}
			if ("verifygw".Equals(AliOHHelper.getXmlNode(xml, "EventType")))
			{
				AliOHHelper.verifygwResponse(true, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context);
			}
		}

		public static bool verifySignAlipayRequest(System.Collections.Generic.Dictionary<string, string> param)
		{
			return AlipaySignature.RSACheckV2(param, AlipayFuwuConfig.alipay_public_key, AlipayFuwuConfig.charset);
		}

		public static void verifyRequestFromAliPay(HttpContext context, string ToUserId, string AppId)
		{
			System.Collections.Generic.Dictionary<string, string> alipayRequstParams = AliOHHelper.getAlipayRequstParams(context);
			string text = alipayRequstParams["biz_content"];
			if (!AliOHHelper.verifySignAlipayRequest(alipayRequstParams))
			{
				AliOHHelper.ReturnXmlResponse(false, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context, ToUserId, AppId);
			}
			else
			{
				AliOHHelper.ReturnXmlResponse(true, RsaKeyHelper.GetRSAKeyContent(AlipayFuwuConfig.merchant_public_key, true), context, ToUserId, AppId);
			}
		}

		public static string ReturnXmlResponse(bool _success, string merchantPubKey, HttpContext context, string ToUserId, string AppId)
		{
			context.Response.ContentType = "text/xml";
			context.Response.ContentEncoding = System.Text.Encoding.GetEncoding(AlipayFuwuConfig.charset);
			context.Response.Clear();
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", AlipayFuwuConfig.charset, null);
			xmlDocument.AppendChild(newChild);
			XmlElement newChild2 = xmlDocument.CreateElement("alipay");
			xmlDocument.AppendChild(newChild2);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("alipay");
			XmlElement xmlElement = xmlDocument.CreateElement("response");
			XmlElement xmlElement2 = xmlDocument.CreateElement("success");
			XmlElement xmlElement3 = xmlDocument.CreateElement("XML");
			if (_success)
			{
				XmlElement xmlElement4 = xmlDocument.CreateElement("MsgType");
				xmlElement4.InnerText = "ack";
				XmlElement xmlElement5 = xmlDocument.CreateElement("CreateTime");
				xmlElement5.InnerText = AliOHHelper.TransferToMilStartWith1970(System.DateTime.Now).ToString("F0");
				XmlElement xmlElement6 = xmlDocument.CreateElement("ToUserId");
				xmlElement6.InnerText = ToUserId;
				XmlElement xmlElement7 = xmlDocument.CreateElement("AppId");
				xmlElement7.InnerText = AppId;
				xmlElement3.AppendChild(xmlElement4);
				xmlElement3.AppendChild(xmlElement5);
				xmlElement3.AppendChild(xmlElement6);
				xmlElement3.AppendChild(xmlElement7);
				xmlElement.AppendChild(xmlElement3);
			}
			else
			{
				xmlElement2.InnerText = "false";
				xmlElement.AppendChild(xmlElement2);
				XmlElement xmlElement8 = xmlDocument.CreateElement("error_code");
				xmlElement8.InnerText = "VERIFY_FAILED";
				xmlElement.AppendChild(xmlElement8);
			}
			xmlNode.AppendChild(xmlElement);
			string innerText = AlipaySignature.RSASign(xmlElement.InnerXml, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.charset);
			XmlElement xmlElement9 = xmlDocument.CreateElement("sign");
			xmlElement9.InnerText = innerText;
			xmlNode.AppendChild(xmlElement9);
			XmlElement xmlElement10 = xmlDocument.CreateElement("sign_type");
			xmlElement10.InnerText = "RSA";
			xmlNode.AppendChild(xmlElement10);
			AliOHHelper.log(xmlDocument.InnerXml);
			context.Response.Output.Write(xmlDocument.InnerXml);
			context.Response.End();
			return null;
		}

		public static string verifygwResponse(bool _success, string merchantPubKey, HttpContext context)
		{
			context.Response.ContentType = "text/xml";
			context.Response.ContentEncoding = System.Text.Encoding.GetEncoding(AlipayFuwuConfig.charset);
			context.Response.Clear();
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", AlipayFuwuConfig.charset, null);
			xmlDocument.AppendChild(newChild);
			XmlElement newChild2 = xmlDocument.CreateElement("alipay");
			xmlDocument.AppendChild(newChild2);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("alipay");
			XmlElement xmlElement = xmlDocument.CreateElement("response");
			XmlElement xmlElement2 = xmlDocument.CreateElement("success");
			if (_success)
			{
				xmlElement2.InnerText = "true";
				xmlElement.AppendChild(xmlElement2);
			}
			else
			{
				xmlElement2.InnerText = "false";
				xmlElement.AppendChild(xmlElement2);
				XmlElement xmlElement3 = xmlDocument.CreateElement("error_code");
				xmlElement3.InnerText = "VERIFY_FAILED";
				xmlElement.AppendChild(xmlElement3);
			}
			XmlElement xmlElement4 = xmlDocument.CreateElement("biz_content");
			xmlElement4.InnerText = merchantPubKey;
			xmlElement.AppendChild(xmlElement4);
			xmlNode.AppendChild(xmlElement);
			string innerText = AlipaySignature.RSASign(xmlElement.InnerXml, AlipayFuwuConfig.merchant_private_key, AlipayFuwuConfig.charset);
			XmlElement xmlElement5 = xmlDocument.CreateElement("sign");
			xmlElement5.InnerText = innerText;
			xmlNode.AppendChild(xmlElement5);
			XmlElement xmlElement6 = xmlDocument.CreateElement("sign_type");
			xmlElement6.InnerText = "RSA";
			xmlNode.AppendChild(xmlElement6);
			context.Response.Output.Write(xmlDocument.InnerXml);
			context.Response.End();
			return null;
		}

		public static string getXmlNode(string xml, string node)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			string result = "";
			try
			{
				result = xmlDocument.SelectSingleNode("//" + node).InnerText.ToString();
			}
			catch (System.Exception var_2_35)
			{
			}
			return result;
		}

		public static string getRequestString(string key, HttpContext context)
		{
			string result = null;
			if (context.Request.Form.Get(key) != null && context.Request.Form.Get(key).ToString() != "")
			{
				result = context.Request.Form.Get(key).ToString();
			}
			else if (context.Request.QueryString[key] != null && context.Request.QueryString[key].ToString() != "")
			{
				result = context.Request.QueryString[key].ToString();
			}
			return result;
		}

		public static string GetUrlParam(System.Collections.Generic.Dictionary<string, string> param)
		{
			string text = "";
			if (param != null)
			{
				foreach (string current in param.Keys)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						current,
						"=",
						param[current],
						"&"
					});
				}
				text = text.Substring(0, text.LastIndexOf('&'));
			}
			return text;
		}

		public static System.Collections.Generic.Dictionary<string, string> getRequstParam(HttpContext context)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			if (context.Request.QueryString != null)
			{
				string[] allKeys = context.Request.QueryString.AllKeys;
				for (int i = 0; i < allKeys.Length; i++)
				{
					string text = allKeys[i];
				}
			}
			if (context.Request.Form != null)
			{
				for (int j = 0; j < context.Request.Params.Count; j++)
				{
					dictionary.Add(context.Request.Params.Keys[j].ToString(), context.Request.Params[j].ToString());
				}
			}
			return dictionary;
		}

		public static void log(string log)
		{
			if (AlipayFuwuConfig.writeLog)
			{
				lock (AliOHHelper.LockLog)
				{
					string str = System.DateTime.Now.ToString("yyyyMMdd") + "_Fuwulog.txt";
					string path = HttpRuntime.AppDomainAppPath.ToString() + "log/" + str;
					System.IO.StreamWriter streamWriter = System.IO.File.AppendText(path);
					streamWriter.WriteLine(System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ":" + log);
					streamWriter.WriteLine("---------------");
					streamWriter.Close();
				}
			}
		}

		public static double TransferToMilStartWith1970(System.DateTime dateTime)
		{
			System.DateTime d = new System.DateTime(1970, 1, 1);
			return (dateTime - d).TotalMilliseconds;
		}

		public static AlipayMobilePublicMessageCustomSendResponse CustomSend(Articles Articles)
		{
			AlipayMobilePublicMessageCustomSendRequest alipayMobilePublicMessageCustomSendRequest = new AlipayMobilePublicMessageCustomSendRequest();
			alipayMobilePublicMessageCustomSendRequest.BizContent = AliOHHelper.SerializeObject(Articles, true);
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicMessageCustomSendResponse>(alipayMobilePublicMessageCustomSendRequest);
		}

		public static AlipayMobilePublicMessageTotalSendResponse TotalSend(Articles Articles)
		{
			AlipayMobilePublicMessageTotalSendRequest alipayMobilePublicMessageTotalSendRequest = new AlipayMobilePublicMessageTotalSendRequest();
			alipayMobilePublicMessageTotalSendRequest.BizContent = AliOHHelper.SerializeObject(Articles, true);
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicMessageTotalSendResponse>(alipayMobilePublicMessageTotalSendRequest);
		}

		public static AlipayMobilePublicMessageSingleSendResponse TemplateSend(AliTemplateMessage templateMessage)
		{
			AlipayMobilePublicMessageSingleSendRequest alipayMobilePublicMessageSingleSendRequest = new AlipayMobilePublicMessageSingleSendRequest();
			alipayMobilePublicMessageSingleSendRequest.BizContent = AliOHHelper.templateSendMessage(templateMessage, "查看详情");
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicMessageSingleSendResponse>(alipayMobilePublicMessageSingleSendRequest);
		}

		public static AlipayMobilePublicQrcodeCreateResponse QrcodeSend(QrcodeInfo codeInfo)
		{
			AlipayMobilePublicQrcodeCreateRequest alipayMobilePublicQrcodeCreateRequest = new AlipayMobilePublicQrcodeCreateRequest();
			alipayMobilePublicQrcodeCreateRequest.BizContent = AliOHHelper.SerializeObject(codeInfo, true);
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicQrcodeCreateResponse>(alipayMobilePublicQrcodeCreateRequest);
		}

		public static AlipayMobilePublicFollowListResponse GetfollowList(string nextUserId)
		{
			AlipayMobilePublicFollowListRequest alipayMobilePublicFollowListRequest = new AlipayMobilePublicFollowListRequest();
			alipayMobilePublicFollowListRequest.BizContent = "{\"nextUserId\": \"" + nextUserId + "\"}";
			IAopClient aopClient = new DefaultAopClient(AlipayFuwuConfig.serverUrl, AlipayFuwuConfig.appId, AlipayFuwuConfig.merchant_private_key);
			return aopClient.Execute<AlipayMobilePublicFollowListResponse>(alipayMobilePublicFollowListRequest);
		}

		public static System.Collections.Generic.List<string> GetAllfollowList()
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			int num = 10000;
			string nextUserId = "";
			while (num == 10000)
			{
				AlipayMobilePublicFollowListResponse alipayMobilePublicFollowListResponse = AliOHHelper.GetfollowList(nextUserId);
				if (alipayMobilePublicFollowListResponse != null && !alipayMobilePublicFollowListResponse.IsError && alipayMobilePublicFollowListResponse.Data != null && int.TryParse(alipayMobilePublicFollowListResponse.Count, out num))
				{
					if (alipayMobilePublicFollowListResponse.Data.UserIdList != null)
					{
						nextUserId = alipayMobilePublicFollowListResponse.Data.UserIdList[0];
						foreach (string current in alipayMobilePublicFollowListResponse.Data.UserIdList)
						{
							list.Add(current);
						}
					}
				}
				else
				{
					num = 0;
				}
			}
			return list;
		}
	}
}
