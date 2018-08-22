using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Hidistro.Core
{
	public static class Express
	{
		public static string GetExpressType()
		{
			string result = "kuaidi100";
			string text = null;
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				text = current.Request.MapPath("~/Express.xml");
			}
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(text))
			{
				xmlDocument.Load(text);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("expressapi");
				if (xmlNode != null)
				{
					result = xmlNode.Attributes["usetype"].InnerText;
				}
			}
			return result;
		}

		public static string GetNewKey()
		{
			string result = "";
			string text = null;
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				text = current.Request.MapPath("~/Express.xml");
			}
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(text))
			{
				xmlDocument.Load(text);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("companys");
				if (xmlNode != null)
				{
					string value = xmlNode.Attributes["Kuaidi100NewKey"].Value;
					if (!string.IsNullOrEmpty(value))
					{
						result = value;
					}
				}
			}
			return result;
		}

		public static string GetDataByKuaidi100(string computer, string expressNo, int show = 0)
		{
			string text = "29833628d495d7a5";
			string value = "";
			string text2 = null;
			string text3 = "暂时没有此快递单号的信息";
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				text2 = current.Request.MapPath("~/Express.xml");
			}
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(text2))
			{
				xmlDocument.Load(text2);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("companys");
				if (xmlNode != null)
				{
					string value2 = xmlNode.Attributes["Kuaidi100NewKey"].Value;
					if (!string.IsNullOrEmpty(value2))
					{
						value = value2;
					}
				}
			}
			string result;
			if (string.IsNullOrEmpty(value))
			{
				string expressCode = Express.GetExpressCode(computer);
				string requestUriString = string.Concat(new string[]
				{
					"http://api.kuaidi100.com/api?id=",
					text,
					"&com=",
					expressCode,
					"&nu=",
					expressNo,
					"&show=0&muti=1&order=asc"
				});
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
				httpWebRequest.Timeout = 8000;
				HttpWebResponse httpWebResponse;
				try
				{
					httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				}
				catch
				{
					result = text3;
					return result;
				}
				if (httpWebResponse.StatusCode == HttpStatusCode.OK)
				{
					Stream responseStream = httpWebResponse.GetResponseStream();
					StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
					text3 = streamReader.ReadToEnd();
					text3 = text3.Replace("&amp;", "");
					text3 = text3.Replace("&nbsp;", "");
					text3 = text3.Replace("&", "");
				}
			}
			result = text3;
			return result;
		}

		public static string GetExpressCode(string computer)
		{
			string text = computer;
			string text2 = text;
			switch (text2)
			{
			case "AAE全球专递":
				text = "aae";
				break;
			case "安捷快递":
				text = "anjiekuaidi";
				break;
			case "安信达快递":
				text = "anxindakuaixi";
				break;
			case "百福东方":
				text = "baifudongfang";
				break;
			case "彪记快递":
				text = "biaojikuaidi";
				break;
			case "BHT":
				text = "bht";
				break;
			case "希伊艾斯快递":
				text = "cces";
				break;
			case "中国东方":
				text = "Coe";
				break;
			case "长宇物流":
				text = "changyuwuliu";
				break;
			case "大田物流":
				text = "datianwuliu";
				break;
			case "德邦物流":
				text = "debangwuliu";
				break;
			case "DPEX":
				text = "dpex";
				break;
			case "DHL":
				text = "dhl";
				break;
			case "D速快递":
				text = "dsukuaidi";
				break;
			case "fedex":
				text = "fedex";
				break;
			case "飞康达物流":
				text = "feikangda";
				break;
			case "凤凰快递":
				text = "fenghuangkuaidi";
				break;
			case "港中能达物流":
				text = "ganzhongnengda";
				break;
			case "广东邮政物流":
				text = "guangdongyouzhengwuliu";
				break;
			case "汇通快运":
				text = "huitongkuaidi";
				break;
			case "恒路物流":
				text = "hengluwuliu";
				break;
			case "华夏龙物流":
				text = "huaxialongwuliu";
				break;
			case "佳怡物流":
				text = "jiayiwuliu";
				break;
			case "京广速递":
				text = "jinguangsudikuaijian";
				break;
			case "急先达":
				text = "jixianda";
				break;
			case "佳吉物流":
				text = "jiajiwuliu";
				break;
			case "加运美":
				text = "jiayunmeiwuliu";
				break;
			case "快捷速递":
				text = "kuaijiesudi";
				break;
			case "联昊通物流":
				text = "lianhaowuliu";
				break;
			case "龙邦物流":
				text = "longbanwuliu";
				break;
			case "民航快递":
				text = "minghangkuaidi";
				break;
			case "配思货运":
				text = "peisihuoyunkuaidi";
				break;
			case "全晨快递":
				text = "quanchenkuaidi";
				break;
			case "全际通物流":
				text = "quanjitong";
				break;
			case "全日通快递":
				text = "quanritongkuaidi";
				break;
			case "全一快递":
				text = "quanyikuaidi";
				break;
			case "盛辉物流":
				text = "shenghuiwuliu";
				break;
			case "速尔物流":
				text = "suer";
				break;
			case "盛丰物流":
				text = "shengfengwuliu";
				break;
			case "天地华宇":
				text = "tiandihuayu";
				break;
			case "天天快递":
				text = "tiantian";
				break;
			case "TNT":
				text = "tnt";
				break;
			case "UPS":
				text = "ups";
				break;
			case "万家物流":
				text = "wanjiawuliu";
				break;
			case "文捷航空速递":
				text = "wenjiesudi";
				break;
			case "伍圆速递":
				text = "wuyuansudi";
				break;
			case "万象物流":
				text = "wanxiangwuliu";
				break;
			case "新邦物流":
				text = "xinbangwuliu";
				break;
			case "信丰物流":
				text = "xinfengwuliu";
				break;
			case "星晨急便":
				text = "xingchengjibian";
				break;
			case "鑫飞鸿物流":
				text = "xinhongyukuaidi";
				break;
			case "亚风速递":
				text = "yafengsudi";
				break;
			case "一邦速递":
				text = "yibangwuliu";
				break;
			case "优速物流":
				text = "youshuwuliu";
				break;
			case "远成物流":
				text = "yuanchengwuliu";
				break;
			case "圆通速递":
				text = "yuantong";
				break;
			case "源伟丰快递":
				text = "yuanweifeng";
				break;
			case "元智捷诚快递":
				text = "yuanzhijiecheng";
				break;
			case "越丰物流":
				text = "yuefengwuliu";
				break;
			case "韵达快递":
				text = "yunda";
				break;
			case "源安达":
				text = "yuananda";
				break;
			case "运通快递":
				text = "yuntongkuaidi";
				break;
			case "宅急送":
				text = "zhaijisong";
				break;
			case "中铁快运":
				text = "zhongtiewuliu";
				break;
			case "中通速递":
				text = "zhongtong";
				break;
			case "中邮物流":
				text = "zhongyouwuliu";
				break;
			case "顺丰物流":
				text = "shunfeng";
				break;
			}
			return text;
		}

		public static string GetDataByTaobaoTop(string computer, string expressNo)
		{
			return "暂时没有此快递单号的信息";
		}

		public static string GetExpressData(string computer, string expressNo, int show = 0)
		{
			string result;
			if (Express.GetExpressType() == "kuaidi100")
			{
				result = Express.GetDataByKuaidi100(computer, expressNo, show);
			}
			else
			{
				result = Express.GetDataByTaobaoTop(computer, expressNo);
			}
			return result;
		}

		public static void SubscribeExpress100(string companyCode, string number)
		{
			string newKey = Express.GetNewKey();
			if (!string.IsNullOrEmpty(newKey))
			{
				if (companyCode != "" && number != "")
				{
					WebClient webClient = new WebClient();
					NameValueCollection nameValueCollection = new NameValueCollection();
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"company\":\"{0}\",", companyCode);
					stringBuilder.AppendFormat("\"number\":\"{0}\",", number);
					stringBuilder.AppendFormat("\"key\":\"{0}\",", newKey);
					string text = "http://" + HttpContext.Current.Request.Url.Host + "/API/ExpressReturn.ashx?action=SaveExpressData";
					stringBuilder.AppendFormat("\"parameters\":{0}\"callbackurl\":\"{1}\"{2}", "{", text, "}");
					stringBuilder.Append("}");
					nameValueCollection.Add("schema", "json");
					nameValueCollection.Add("param", stringBuilder.ToString());
					try
					{
						byte[] bytes = webClient.UploadValues("http://www.kuaidi100.com/poll", "POST", nameValueCollection);
						string @string = Encoding.UTF8.GetString(bytes);
						Globals.Debuglog("returnUrl:" + text, "_Debuglog.txt");
						Globals.Debuglog(@string, "_Debuglog.txt");
					}
					catch (Exception var_7_13F)
					{
					}
				}
			}
		}
	}
}
