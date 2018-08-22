using Hidistro.Entities.Sales;
using Hidistro.SqlDal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Hidistro.Vshop
{
	public static class ExpressHelper
	{
		private static string path = HttpContext.Current.Request.MapPath("~/Express.xml");

		public static ExpressCompanyInfo FindNode(string company)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = string.Format("//company[@name='{0}']", company);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = company;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = xmlNode2.Attributes["TaobaoCode"].Value;
			}
			return expressCompanyInfo;
		}

		public static ExpressCompanyInfo FindNodeByCode(string code)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = string.Format("//company[@TaobaoCode='{0}']", code);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = xmlNode2.Attributes["name"].Value;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = code;
			}
			return expressCompanyInfo;
		}

		public static IList<ExpressCompanyInfo> GetAllExpress()
		{
			IList<ExpressCompanyInfo> list = new List<ExpressCompanyInfo>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				list.Add(new ExpressCompanyInfo
				{
					Name = xmlNode3.Attributes["name"].Value,
					Kuaidi100Code = xmlNode3.Attributes["Kuaidi100Code"].Value,
					TaobaoCode = xmlNode3.Attributes["TaobaoCode"].Value
				});
			}
			return list;
		}

		public static ExpressSet GetExpressSet()
		{
			ExpressSet expressSet = new ExpressSet();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			if (xmlNode2 != null)
			{
				expressSet.Key = xmlNode2.Attributes["Kuaidi100Key"].Value;
				expressSet.NewKey = xmlNode2.Attributes["Kuaidi100NewKey"].Value;
				expressSet.Url = xmlNode2.Attributes["Url"].Value;
			}
			return expressSet;
		}

		public static IList<string> GetAllExpressName()
		{
			IList<string> list = new List<string>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				list.Add(xmlNode3.Attributes["name"].Value);
			}
			return list;
		}

		public static System.Data.DataTable GetExpressTable()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			dataTable.Columns.Add("id", typeof(int));
			dataTable.Columns.Add("Name");
			dataTable.Columns.Add("Kuaidi100Code");
			dataTable.Columns.Add("TaobaoCode");
			dataTable.Columns.Add("New");
			int num = 0;
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				System.Data.DataRow dataRow = dataTable.NewRow();
				dataRow["id"] = num;
				dataRow["Name"] = xmlNode3.Attributes["name"].Value;
				dataRow["Kuaidi100Code"] = xmlNode3.Attributes["Kuaidi100Code"].Value;
				dataRow["TaobaoCode"] = xmlNode3.Attributes["TaobaoCode"].Value;
				if (xmlNode3.Attributes["New"] != null)
				{
					dataRow["New"] = xmlNode3.Attributes["New"].Value;
				}
				else
				{
					dataRow["New"] = "N";
				}
				dataTable.Rows.Add(dataRow);
				num++;
			}
			return dataTable;
		}

		public static void DeleteExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == name)
				{
					xmlNode2.RemoveChild(xmlNode3);
					break;
				}
			}
			xmlNode.Save(ExpressHelper.path);
		}

		public static void AddExpress(string name, string kuaidi100Code, string taobaoCode)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			XmlElement xmlElement = xmlNode.CreateElement("company");
			xmlElement.SetAttribute("name", name);
			xmlElement.SetAttribute("Kuaidi100Code", kuaidi100Code);
			xmlElement.SetAttribute("TaobaoCode", taobaoCode);
			xmlElement.SetAttribute("New", "Y");
			xmlNode2.AppendChild(xmlElement);
			xmlNode.Save(ExpressHelper.path);
		}

		public static bool IsExitExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			bool result;
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == name)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}

		public static void UpdateExpress(string oldcompanyname, string name, string kuaidi100Code, string taobaoCode)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == oldcompanyname)
				{
					xmlNode3.Attributes["name"].Value = name;
					xmlNode3.Attributes["Kuaidi100Code"].Value = kuaidi100Code;
					xmlNode3.Attributes["TaobaoCode"].Value = taobaoCode;
					break;
				}
			}
			xmlNode.Save(ExpressHelper.path);
		}

		public static void UpdateExpressUrlAndKey(string key, string url)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			if (xmlNode2 != null)
			{
				xmlNode2.Attributes["Kuaidi100NewKey"].Value = key;
				xmlNode2.Attributes["Url"].Value = url;
			}
			xmlNode.Save(ExpressHelper.path);
		}

		public static string GetDataByKuaidi100(string computer, string expressNo)
		{
			string key = "29833628d495d7a5";
			string text = "";
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			string str = "{\"message\": \"ok\",\"content\": ";
			if (xmlNode2 != null)
			{
				text = xmlNode2.Attributes["Kuaidi100NewKey"].Value;
			}
			if (!string.IsNullOrEmpty(text))
			{
				string expressDataList = new ExpressDataDao().GetExpressDataList(computer, expressNo);
				if (!string.IsNullOrEmpty(expressDataList))
				{
					str += expressDataList;
					str += ",\"type\":\"1\"";
				}
				else
				{
					str += ExpressHelper.GetContentByAPI(text, computer, expressNo);
					str += ",\"type\":\"2\"";
				}
			}
			else
			{
				str += ExpressHelper.GetContentByAPI(key, computer, expressNo);
				str += ",\"type\":\"2\"";
			}
			return str + "}";
		}

		private static string GetContentByAPI(string key, string computer, string expressNo)
		{
			string text = "";
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Concat(new string[]
			{
				"http://api.kuaidi100.com/api?id=",
				key,
				"&com=",
				computer,
				"&nu=",
				expressNo
			}));
			httpWebRequest.Timeout = 8000;
			HttpWebResponse httpWebResponse;
			string result;
			try
			{
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch
			{
				result = text;
				return result;
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
				text += streamReader.ReadToEnd();
				text = text.Replace("&amp;", "");
				text = text.Replace("&nbsp;", "");
				text = text.Replace("&", "");
			}
			result = text;
			return result;
		}

		public static string GetExpressData(string computer, string expressNo)
		{
			return ExpressHelper.GetDataByKuaidi100(computer, expressNo);
		}

		private static XmlDocument GetXmlNode()
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(ExpressHelper.path))
			{
				xmlDocument.Load(ExpressHelper.path);
			}
			return xmlDocument;
		}
	}
}
