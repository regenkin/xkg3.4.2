using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hidistro.Entities
{
	public static class RegionHelper
	{
		public static XmlNode GetRegion(int regionId)
		{
			return RegionHelper.FindNode(regionId);
		}

		public static string GetFullRegion(int currentRegionId, string separator)
		{
			XmlNode xmlNode = RegionHelper.FindNode(currentRegionId);
			string result;
			if (xmlNode == null)
			{
				result = currentRegionId.ToString();
			}
			else
			{
				string text = xmlNode.Attributes["name"].Value;
				XmlNode parentNode = xmlNode.ParentNode;
				while (parentNode.Name != "region")
				{
					text = parentNode.Attributes["name"].Value + separator + text;
					if (parentNode.Name == "city")
					{
						string value = parentNode.Attributes["id"].Value;
					}
					parentNode = parentNode.ParentNode;
				}
				result = text;
			}
			return result;
		}

		public static string GetCity(int currentRegionId)
		{
			XmlNode xmlNode = RegionHelper.FindNode(currentRegionId);
			string result;
			if (xmlNode == null)
			{
				result = currentRegionId.ToString();
			}
			else
			{
				string text = currentRegionId.ToString();
				XmlNode parentNode = xmlNode.ParentNode;
				while (parentNode.Name != "region")
				{
					if (parentNode.Name == "city")
					{
						text = parentNode.Attributes["id"].Value;
					}
					parentNode = parentNode.ParentNode;
				}
				if (text == "0")
				{
					text = currentRegionId.ToString();
				}
				result = text;
			}
			return result;
		}

		public static string GetFullPath(int currentRegionId)
		{
			XmlNode xmlNode = RegionHelper.FindNode(currentRegionId);
			string result;
			if (xmlNode == null)
			{
				result = string.Empty;
			}
			else
			{
				string text = xmlNode.Attributes["id"].Value;
				XmlNode parentNode = xmlNode.ParentNode;
				while (parentNode.Name != "region")
				{
					text = parentNode.Attributes["id"].Value + "," + text;
					parentNode = parentNode.ParentNode;
				}
				result = text;
			}
			return result;
		}

		public static int GetTopRegionId(int currentRegionId)
		{
			XmlNode xmlNode = RegionHelper.FindNode(currentRegionId);
			int result;
			if (xmlNode == null)
			{
				result = 0;
			}
			else
			{
				int num = currentRegionId;
				XmlNode parentNode = xmlNode.ParentNode;
				while (parentNode.Name != "region")
				{
					num = int.Parse(parentNode.Attributes["id"].Value);
					parentNode = parentNode.ParentNode;
				}
				result = num;
			}
			return result;
		}

		public static int GetRegionId(string county, string city, string province)
		{
			string xpath = string.Format("//province[@name='{0}']", province);
			XmlDocument regionDocument = RegionHelper.GetRegionDocument();
			XmlNode xmlNode = regionDocument.SelectSingleNode(xpath);
			int result;
			if (xmlNode != null)
			{
				int num = int.Parse(xmlNode.Attributes["id"].Value);
				xpath = string.Format("//province[@id='{0}']/city[@name='{1}']", num, city);
				xmlNode = xmlNode.SelectSingleNode(xpath);
				if (xmlNode != null)
				{
					num = int.Parse(xmlNode.Attributes["id"].Value);
					xpath = string.Format("//city[@id='{0}']/county[@name='{1}']", num, county);
					xmlNode = xmlNode.SelectSingleNode(xpath);
					if (xmlNode != null)
					{
						num = int.Parse(xmlNode.Attributes["id"].Value);
					}
				}
				result = num;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		private static XmlNode FindNode(int id)
		{
			string arg = id.ToString(System.Globalization.CultureInfo.InvariantCulture);
			string xpath = string.Format("//county[@id='{0}']", arg);
			XmlDocument regionDocument = RegionHelper.GetRegionDocument();
			XmlNode xmlNode = regionDocument.SelectSingleNode(xpath);
			XmlNode result;
			if (xmlNode != null)
			{
				result = xmlNode;
			}
			else
			{
				xpath = string.Format("//city[@id='{0}']", arg);
				xmlNode = regionDocument.SelectSingleNode(xpath);
				if (xmlNode != null)
				{
					result = xmlNode;
				}
				else
				{
					xpath = string.Format("//province[@id='{0}']", arg);
					xmlNode = regionDocument.SelectSingleNode(xpath);
					if (xmlNode != null)
					{
						result = xmlNode;
					}
					else
					{
						result = null;
					}
				}
			}
			return result;
		}

		public static System.Collections.Generic.Dictionary<int, string> GetAllRegionIds(bool readRegion, bool readprovince, bool readcity, bool readcounty)
		{
			System.Collections.Generic.Dictionary<int, string> dictionary = new System.Collections.Generic.Dictionary<int, string>();
			XmlDocument regionDocument = RegionHelper.GetRegionDocument();
			XmlNode xmlNode = regionDocument.SelectSingleNode("root");
			foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
			{
				if (readRegion)
				{
					dictionary.Add(int.Parse(xmlNode2.Attributes["id"].Value), xmlNode2.Attributes["name"].Value);
				}
				if (xmlNode2.ChildNodes.Count > 0)
				{
					foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
					{
						if (readprovince)
						{
							dictionary.Add(int.Parse(xmlNode3.Attributes["id"].Value), xmlNode3.Attributes["name"].Value);
						}
						if (xmlNode3.ChildNodes.Count > 0)
						{
							foreach (XmlNode xmlNode4 in xmlNode3.ChildNodes)
							{
								if (readcity)
								{
									dictionary.Add(int.Parse(xmlNode4.Attributes["id"].Value), xmlNode4.Attributes["name"].Value);
								}
								if (xmlNode4.ChildNodes.Count > 0)
								{
									foreach (XmlNode xmlNode5 in xmlNode4.ChildNodes)
									{
										if (readcounty)
										{
											dictionary.Add(int.Parse(xmlNode5.Attributes["id"].Value), xmlNode5.Attributes["name"].Value);
										}
									}
								}
							}
						}
					}
				}
			}
			return dictionary;
		}

		public static System.Collections.Generic.Dictionary<int, string> GetAllCitys()
		{
			System.Collections.Generic.Dictionary<int, string> dictionary = new System.Collections.Generic.Dictionary<int, string>();
			XmlDocument regionDocument = RegionHelper.GetRegionDocument();
			XmlNode xmlNode = regionDocument.SelectSingleNode("root");
			foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
			{
				if (xmlNode2.ChildNodes.Count > 0)
				{
					foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
					{
						string value = xmlNode3.Attributes["name"].Value;
						if (xmlNode3.ChildNodes.Count > 0)
						{
							foreach (XmlNode xmlNode4 in xmlNode3.ChildNodes)
							{
								dictionary.Add(int.Parse(xmlNode4.Attributes["id"].Value), value + "-" + xmlNode4.Attributes["name"].Value);
							}
						}
					}
				}
			}
			return dictionary;
		}

		public static System.Collections.Generic.Dictionary<int, string> GetRegions()
		{
			return RegionHelper.GetChildList("root");
		}

		public static System.Collections.Generic.Dictionary<int, string> GetProvinces(int regionId)
		{
			return RegionHelper.GetChildList(string.Format("root/region[@id='{0}']", regionId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
		}

		public static System.Collections.Generic.Dictionary<int, string> GetAllProvinces()
		{
			System.Collections.Generic.Dictionary<int, string> dictionary = new System.Collections.Generic.Dictionary<int, string>();
			XmlDocument regionDocument = RegionHelper.GetRegionDocument();
			XmlNodeList xmlNodeList = regionDocument.SelectNodes("//province");
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				dictionary.Add(int.Parse(xmlNode.Attributes["id"].Value), xmlNode.Attributes["name"].Value);
			}
			return dictionary;
		}

		public static System.Collections.Generic.Dictionary<int, string> GetCitys(int provinceId)
		{
			return RegionHelper.GetChildList(string.Format("root/region/province[@id='{0}']", provinceId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
		}

		public static System.Collections.Generic.Dictionary<int, string> GetCountys(int cityId)
		{
			return RegionHelper.GetChildList(string.Format("root/region/province/city[@id='{0}']", cityId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
		}

		private static System.Collections.Generic.Dictionary<int, string> GetChildList(string xpath)
		{
			System.Collections.Generic.Dictionary<int, string> dictionary = new System.Collections.Generic.Dictionary<int, string>();
			XmlDocument regionDocument = RegionHelper.GetRegionDocument();
			XmlNode xmlNode = regionDocument.SelectSingleNode(xpath);
			foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
			{
				dictionary.Add(int.Parse(xmlNode2.Attributes["id"].Value), xmlNode2.Attributes["name"].Value);
			}
			return dictionary;
		}

		public static string GetAllChild(int currentRegionId)
		{
			string text = currentRegionId.ToString();
			XmlNode xmlNode = RegionHelper.FindNode(currentRegionId);
			if (xmlNode != null)
			{
				foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
				{
					text = text + "," + xmlNode2.Attributes["id"].Value;
				}
			}
			return text;
		}

		private static XmlDocument GetRegionDocument()
		{
			XmlDocument xmlDocument = HiCache.Get("FileCache-Regions") as XmlDocument;
			if (xmlDocument == null)
			{
				string filename = HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/config/region.config");
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				HiCache.Max("FileCache-Regions", xmlDocument, new CacheDependency(filename));
			}
			return xmlDocument;
		}
	}
}
