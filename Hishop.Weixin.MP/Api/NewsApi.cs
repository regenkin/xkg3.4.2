using Hishop.Weixin.MP.Util;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Hishop.Weixin.MP.Api
{
	public class NewsApi
	{
		private static object LockLog = new object();

		public static string KFSend(string access_token, string postData)
		{
			return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/custom/send?access_token={0}", access_token), postData);
		}

		public static string Send(string access_token, string postData)
		{
			return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/send?access_token={0}", access_token), postData);
		}

		public static string SendAll(string access_token, string postData)
		{
			return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/sendall?access_token={0}", access_token), postData);
		}

		public static List<string> GetOpenIDs(string access_token)
		{
			List<string> list = new List<string>();
			List<string> openIDs = NewsApi.GetOpenIDs(access_token, null);
			list.AddRange(openIDs);
			while (openIDs.Count > 0)
			{
				openIDs = NewsApi.GetOpenIDs(access_token, openIDs[openIDs.Count - 1]);
				list.AddRange(openIDs);
			}
			return list;
		}

		public static string CreateImageNewsJson(string media_id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\"filter\":{\"is_to_all\":true},");
			stringBuilder.Append("\"msgtype\":\"mpnews\",");
			stringBuilder.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public static string CreateKFTxtNewsJson(string openid, string content)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				"{\"touser\":\"",
				openid,
				"\",\"msgtype\":\"text\",\"text\": {\"content\":\"",
				content,
				"\"}}"
			}));
			return stringBuilder.ToString();
		}

		public static string CreateKFImageNewsJson(string openid, string media_id)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new string[]
			{
				"{\"touser\":\"",
				openid,
				"\",\"msgtype\":\"image\",\"image\": {\"media_id\":\"",
				media_id,
				"\"}}"
			}));
			return stringBuilder.ToString();
		}

		public static string CreateImageNewsJson(string media_id, List<string> openidList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\"touser\":[");
			stringBuilder.Append(string.Join(",", openidList.ConvertAll<string>((string a) => "\"" + a + "\"").ToArray()));
			stringBuilder.Append("],");
			stringBuilder.Append("\"msgtype\":\"mpnews\",");
			stringBuilder.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public static string CreateTxtNewsJson(string media_id)
		{
			string str = "内容测试";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\"filter\":{\"is_to_all\":true},");
			stringBuilder.Append("\"text\":{\"content\":\"" + str + "\"},");
			stringBuilder.Append("\"mpnews\":{\"media_id\":\"" + media_id + "\"}");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public static List<string> GetOpenIDs(string access_token, string next_openid)
		{
			string url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", access_token, string.IsNullOrEmpty(next_openid) ? "" : next_openid);
			string text = new WebUtils().DoPost(url, "");
			int num = int.Parse(NewsApi.GetJsonValue(text, "count"));
			List<string> result;
			if (num > 0)
			{
				string text2 = "\"openid\":[";
				int num2 = text.IndexOf(text2) + text2.Length;
				int num3 = text.IndexOf("]", num2);
				string text3 = text.Substring(num2, num3 - num2).Replace("\"", "");
				result = text3.Split(new char[]
				{
					','
				}).ToList<string>();
			}
			else
			{
				result = new List<string>();
			}
			return result;
		}

		public static string HttpUploadFile(string url, string path)
		{
			HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
			CookieContainer cookieContainer = new CookieContainer();
			httpWebRequest.CookieContainer = cookieContainer;
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.Method = "POST";
			string str = DateTime.Now.Ticks.ToString("X");
			httpWebRequest.ContentType = "multipart/form-data;charset=utf-8;boundary=" + str;
			byte[] bytes = Encoding.UTF8.GetBytes("\r\n--" + str + "\r\n");
			byte[] bytes2 = Encoding.UTF8.GetBytes("\r\n--" + str + "--\r\n");
			int num = path.LastIndexOf("\\");
			string arg = path.Substring(num + 1);
			StringBuilder stringBuilder = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"file\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", arg));
			byte[] bytes3 = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath(path), FileMode.Open, FileAccess.Read);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			Stream requestStream = httpWebRequest.GetRequestStream();
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Write(bytes3, 0, bytes3.Length);
			requestStream.Write(array, 0, array.Length);
			requestStream.Write(bytes2, 0, bytes2.Length);
			requestStream.Close();
			HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.UTF8);
			return streamReader.ReadToEnd();
		}

		public static string UploadNews(string access_token, string postData)
		{
			return new WebUtils().DoPost(string.Format("https://api.weixin.qq.com/cgi-bin/media/uploadnews?access_token={0}", access_token), postData);
		}

		public static string UploadMedia(string access_token, string type, string path)
		{
			string url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, type);
			return NewsApi.HttpUploadFile(url, path);
		}

		public static string GetJsonValue(string msg, string Field)
		{
			string result = "";
			try
			{
				JObject jObject = JObject.Parse(msg);
				if (jObject[Field] != null)
				{
					result = jObject[Field].ToString();
				}
			}
			catch (Exception var_2_31)
			{
				NewsApi.Debuglog(msg, "_debuglogtext.txt");
			}
			return result;
		}

		public static void Debuglog(string log, string logname = "_Debuglog.txt")
		{
			lock (NewsApi.LockLog)
			{
				try
				{
					string str = DateTime.Now.ToString("yyyyMMdd") + logname;
					string path = HttpRuntime.AppDomainAppPath.ToString() + "log/" + str;
					StreamWriter streamWriter = File.AppendText(path);
					streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ":" + log);
					streamWriter.WriteLine("---------------");
					streamWriter.Close();
				}
				catch (Exception var_3_83)
				{
				}
			}
		}

		public static string GetMedia_IDByPath(string access_token, string path)
		{
			string result = string.Empty;
			string text = path;
			if (text.StartsWith("http"))
			{
				text = Regex.Replace(text, "(http|https)://([^/]*)", "");
			}
			if (text.StartsWith("/"))
			{
				try
				{
					result = NewsApi.UploadMedia(access_token, "image", text);
				}
				catch (Exception ex)
				{
					result = ex.ToString();
				}
			}
			else
			{
				result = "图片路径不对";
			}
			return result;
		}

		public static string GetArticlesJsonStr(string access_token, DataTable dt)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{\"articles\":[");
			int num = 0;
			string result;
			foreach (DataRow dataRow in dt.Rows)
			{
				string path = dataRow["ImgUrl"].ToString();
				if (!File.Exists(path))
				{
					result = "{\"code\":0,\"msg\":\"要发送的图片不存在\"}";
					return result;
				}
				string msg = NewsApi.UploadMedia(access_token, "image", path);
				string jsonValue = NewsApi.GetJsonValue(msg, "media_id");
				if (!string.IsNullOrEmpty(jsonValue))
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"thumb_media_id\":\"" + jsonValue + "\",");
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
			result = stringBuilder.ToString();
			return result;
		}

		public static string GetErrorCodeMsg(string errcode)
		{
			string result = string.Empty;
			switch (errcode)
			{
			case "45028":
				result = "发送条数已经用完";
				return result;
			case "-1":
				result = "系统繁忙，此时请开发者稍候再试";
				return result;
			case "0":
				result = "请求成功";
				return result;
			case "40001":
				result = "获取access_token时AppSecret错误，或者access_token无效。请开发者认真比对AppSecret的正确性，或查看是否正在为恰当的公众号调用接口";
				return result;
			case "40002":
				result = "不合法的凭证类型";
				return result;
			case "40003":
				result = "不合法的OpenID，请开发者确认OpenID（该用户）是否已关注公众号，或是否是其他公众号的OpenID";
				return result;
			case "40004":
				result = "不合法的媒体文件类型";
				return result;
			case "40005":
				result = "不合法的文件类型";
				return result;
			case "40006":
				result = "不合法的文件大小";
				return result;
			case "40007":
				result = "不合法的媒体文件id";
				return result;
			case "40008":
				result = "不合法的消息类型";
				return result;
			case "40009":
				result = "不合法的图片文件大小";
				return result;
			case "40010":
				result = "不合法的语音文件大小";
				return result;
			case "40011":
				result = "不合法的视频文件大小";
				return result;
			case "40012":
				result = "不合法的缩略图文件大小";
				return result;
			case "40013":
				result = "不合法的AppID，请开发者检查AppID的正确性，避免异常字符，注意大小写";
				return result;
			case "40014":
				result = "不合法的access_token，请开发者认真比对access_token的有效性（如是否过期），或查看是否正在为恰当的公众号调用接口";
				return result;
			case "40015":
				result = "不合法的菜单类型";
				return result;
			case "40016":
				result = "不合法的按钮个数";
				return result;
			case "40017":
				result = "不合法的按钮个数";
				return result;
			case "40018":
				result = "不合法的按钮名字长度";
				return result;
			case "40019":
				result = "不合法的按钮KEY长度";
				return result;
			case "40020":
				result = "不合法的按钮URL长度";
				return result;
			case "40021":
				result = "不合法的菜单版本号";
				return result;
			case "40022":
				result = "不合法的子菜单级数";
				return result;
			case "40023":
				result = "不合法的子菜单按钮个数";
				return result;
			case "40024":
				result = "不合法的子菜单按钮类型";
				return result;
			case "40025":
				result = "不合法的子菜单按钮名字长度";
				return result;
			case "40026":
				result = "不合法的子菜单按钮KEY长度";
				return result;
			case "40027":
				result = "不合法的子菜单按钮URL长度";
				return result;
			case "40028":
				result = "不合法的自定义菜单使用用户";
				return result;
			case "40029":
				result = "不合法的oauth_code";
				return result;
			case "40030":
				result = "不合法的refresh_token";
				return result;
			case "40031":
				result = "不合法的openid列表";
				return result;
			case "40032":
				result = "不合法的openid列表长度";
				return result;
			case "40033":
				result = "不合法的请求字符，不能包含\\uxxxx格式的字符";
				return result;
			case "40035":
				result = "不合法的参数";
				return result;
			case "40038":
				result = "不合法的请求格式";
				return result;
			case "40039":
				result = "不合法的URL长度";
				return result;
			case "40050":
				result = "不合法的分组id";
				return result;
			case "40051":
				result = "分组名字不合法";
				return result;
			case "40117":
				result = "分组名字不合法";
				return result;
			case "40118":
				result = "media_id大小不合法";
				return result;
			case "40119":
				result = "button类型错误";
				return result;
			case "40120":
				result = "button类型错误";
				return result;
			case "40121":
				result = "不合法的media_id类型";
				return result;
			case "40132":
				result = "微信号不合法";
				return result;
			case "40137":
				result = "不支持的图片格式";
				return result;
			case "41001":
				result = "缺少access_token参数";
				return result;
			case "41002":
				result = "缺少appid参数";
				return result;
			case "41003":
				result = "缺少refresh_token参数";
				return result;
			case "41004":
				result = "缺少secret参数";
				return result;
			case "41005":
				result = "缺少多媒体文件数据";
				return result;
			case "41006":
				result = "缺少media_id参数";
				return result;
			case "41007":
				result = "缺少子菜单数据";
				return result;
			case "41008":
				result = "缺少oauth code";
				return result;
			case "41009":
				result = "缺少openid";
				return result;
			case "42001":
				result = "access_token超时，请检查access_token的有效期，请参考基础支持-获取access_token中，对access_token的详细机制说明";
				return result;
			case "42002":
				result = "refresh_token超时";
				return result;
			case "42003":
				result = "oauth_code超时";
				return result;
			case "43001":
				result = "需要GET请求";
				return result;
			case "43002":
				result = "需要POST请求";
				return result;
			case "43003":
				result = "需要HTTPS请求";
				return result;
			case "43004":
				result = "需要接收者关注";
				return result;
			case "43005":
				result = "需要好友关系";
				return result;
			case "44001":
				result = "多媒体文件为空";
				return result;
			case "44002":
				result = "POST的数据包为空";
				return result;
			case "44003":
				result = "图文消息内容为空";
				return result;
			case "44004":
				result = "文本消息内容为空";
				return result;
			case "45001":
				result = "多媒体文件大小超过限制";
				return result;
			case "45002":
				result = "消息内容超过限制";
				return result;
			case "45003":
				result = "标题字段超过限制";
				return result;
			case "45004":
				result = "描述字段超过限制";
				return result;
			case "45005":
				result = "链接字段超过限制";
				return result;
			case "45006":
				result = "图片链接字段超过限制";
				return result;
			case "45007":
				result = "语音播放时间超过限制";
				return result;
			case "45008":
				result = "图文消息超过限制";
				return result;
			case "45009":
				result = "接口调用超过限制";
				return result;
			case "45010":
				result = "创建菜单个数超过限制";
				return result;
			case "45015":
				result = "回复时间超过限制";
				return result;
			case "45016":
				result = "系统分组，不允许修改";
				return result;
			case "45017":
				result = "分组名字过长";
				return result;
			case "45018":
				result = "分组数量超过上限";
				return result;
			case "46001":
				result = "不存在媒体数据";
				return result;
			case "46002":
				result = "不存在的菜单版本";
				return result;
			case "46003":
				result = "不存在的菜单数据";
				return result;
			case "46004":
				result = "不存在的用户";
				return result;
			case "47001":
				result = "解析JSON/XML内容错误";
				return result;
			case "48001":
				result = "api功能未授权，请确认公众号已获得该接口，可以在公众平台官网-开发者中心页中查看接口权限";
				return result;
			case "50001":
				result = "用户未授权该api";
				return result;
			case "50002":
				result = "用户受限，可能是违规后接口被封禁";
				return result;
			case "61451":
				result = "参数错误(invalid parameter)";
				return result;
			case "61452":
				result = "无效客服账号(invalid kf_account)";
				return result;
			case "61453":
				result = "客服帐号已存在(kf_account exsited)";
				return result;
			case "61454":
				result = "客服帐号名长度超过限制(仅允许10个英文字符，不包括@及@后的公众号的微信号)(invalid kf_acount length)";
				return result;
			case "61455":
				result = "客服帐号名包含非法字符(仅允许英文+数字)(illegal character in kf_account)";
				return result;
			case "61456":
				result = "客服帐号个数超过限制(10个客服账号)(kf_account count exceeded)";
				return result;
			case "61457":
				result = "无效头像文件类型(invalid file type)";
				return result;
			case "61450":
				result = "系统错误(system error)";
				return result;
			case "61500":
				result = "日期格式错误";
				return result;
			case "61501":
				result = "日期范围错误";
				return result;
			case "9001001":
				result = "POST数据参数不合法";
				return result;
			case "9001002":
				result = "远端服务不可用";
				return result;
			case "9001003":
				result = "Ticket不合法";
				return result;
			case "9001004":
				result = "获取摇周边用户信息失败";
				return result;
			case "9001005":
				result = "获取商户信息失败";
				return result;
			case "9001006":
				result = "获取OpenID失败";
				return result;
			case "9001007":
				result = "上传文件缺失";
				return result;
			case "9001008":
				result = "上传素材的文件类型不合法";
				return result;
			case "9001009":
				result = "上传素材的文件尺寸不合法";
				return result;
			case "9001010":
				result = "上传失败";
				return result;
			case "9001020":
				result = "帐号不合法";
				return result;
			case "9001021":
				result = "已有设备激活率低于50%，不能新增设备";
				return result;
			case "9001022":
				result = "设备申请数不合法，必须为大于0的数字";
				return result;
			case "9001023":
				result = "已存在审核中的设备ID申请";
				return result;
			case "9001024":
				result = "一次查询设备ID数量不能超过50";
				return result;
			case "9001025":
				result = "设备ID不合法";
				return result;
			case "9001026":
				result = "页面ID不合法";
				return result;
			case "9001027":
				result = "页面参数不合法";
				return result;
			case "9001028":
				result = "一次删除页面ID数量不能超过10";
				return result;
			case "9001029":
				result = "页面已应用在设备中，请先解除应用关系再删除";
				return result;
			case "9001030":
				result = "一次查询页面ID数量不能超过50";
				return result;
			case "9001031":
				result = "时间区间不合法";
				return result;
			case "9001032":
				result = "保存设备与页面的绑定关系参数错误";
				return result;
			case "9001033":
				result = "门店ID不合法";
				return result;
			case "9001034":
				result = "设备备注信息过长";
				return result;
			case "9001035":
				result = "设备申请参数不合法";
				return result;
			case "9001036":
				result = "查询起始值begin不合法 ";
				return result;
			}
			result = "未知错误";
			return result;
		}
	}
}
