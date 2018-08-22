using Hidistro.Core;
using Hishop.Weixin.MP.Util;
using System;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class wx : System.Web.IHttpHandler
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
			System.Web.HttpRequest request = context.Request;
			string weixinToken = SettingsManager.GetMasterSettings(false).WeixinToken;
			string signature = request["signature"];
			string nonce = request["nonce"];
			string timestamp = request["timestamp"];
			string s = request["echostr"];
			if (request.HttpMethod == "GET")
			{
				if (CheckSignature.Check(signature, timestamp, nonce, weixinToken))
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
				CustomMsgHandler customMsgHandler = new CustomMsgHandler(request.InputStream);
				customMsgHandler.Execute();
				Globals.Debuglog(customMsgHandler.RequestDocument.ToString(), "_Debuglog.txt");
				context.Response.Write(customMsgHandler.ResponseDocument);
			}
			catch (System.Exception ex)
			{
				Globals.Debuglog(ex.Message, "_Debuglog.txt");
			}
		}
	}
}
