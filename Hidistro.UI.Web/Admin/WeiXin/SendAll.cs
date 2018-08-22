using Hidistro.UI.ControlPanel.Utility;
using Hishop.Weixin.MP.Api;
using System;
using System.Data;
using System.Text;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	public class SendAll : AdminPage
	{
		protected SendAll() : base("m06", "wxp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		public string Send(string access_token, string openid)
		{
			string arg_10_0 = base.Request["type"];
			string arg_21_0 = base.Request["data"];
			string postData = "{\"touser\":\"" + openid + "\",\"msgtype\":\"text\",\"text\": {\"content\":\"欢迎您的来访！\"}}";
			string msg = NewsApi.KFSend(access_token, postData);
			string jsonValue = NewsApi.GetJsonValue(msg, "media_id");
			string text = NewsApi.Send(access_token, NewsApi.CreateImageNewsJson(jsonValue));
			if (string.IsNullOrWhiteSpace(text))
			{
				return "{\"code\":0,\"msg\":\"type参数错误\"}";
			}
			string jsonValue2 = NewsApi.GetJsonValue(text, "errcode");
			string jsonValue3 = NewsApi.GetJsonValue(text, "errmsg");
			if (jsonValue2 == "0")
			{
				return "{\"code\":1,\"msg\":\"\"}";
			}
			return string.Concat(new string[]
			{
				"{\"code\":0,\"msg\":\"errcode:",
				jsonValue2,
				", errmsg:",
				jsonValue3,
				"\"}"
			});
		}

		public static string GetArticlesJsonStr(System.Data.DataTable dt)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"articles\":[");
			int num = 0;
			foreach (System.Data.DataRow dataRow in dt.Rows)
			{
				string text = dataRow["media_id"].ToString();
				if (!string.IsNullOrEmpty(text))
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"thumb_media_id\":\"" + text + "\",");
					stringBuilder.Append("\"author\":\"" + dataRow["Author"].ToString() + "\",");
					stringBuilder.Append("\"title\":\"" + dataRow["Title"].ToString() + "\",");
					stringBuilder.Append("\"content_source_url\":\"" + dataRow["TextUrl"].ToString() + "\",");
					stringBuilder.Append("\"content\":\"" + dataRow["Content"].ToString() + "\",");
					stringBuilder.Append("\"digest\":\"" + dataRow["Content"].ToString() + "\",");
					if (num == dt.Rows.Count - 1)
					{
						stringBuilder.Append("\"show_cover_pic\":\"1\"}");
					}
					else
					{
						stringBuilder.Append("\"show_cover_pic\":\"1\"},");
					}
				}
				num++;
			}
			stringBuilder.Append("]}");
			return stringBuilder.ToString();
		}
	}
}
