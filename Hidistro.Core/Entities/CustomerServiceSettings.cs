using System;
using System.Xml;

namespace Hidistro.Core.Entities
{
	public class CustomerServiceSettings
	{
		public string AppId
		{
			get;
			set;
		}

		public string AppSecret
		{
			get;
			set;
		}

		public string unitid
		{
			get;
			set;
		}

		public string unit
		{
			get;
			set;
		}

		public string password
		{
			get;
			set;
		}

		public string unitname
		{
			get;
			set;
		}

		public string activated
		{
			get;
			set;
		}

		public string logo
		{
			get;
			set;
		}

		public string url
		{
			get;
			set;
		}

		public string tel
		{
			get;
			set;
		}

		public string contact
		{
			get;
			set;
		}

		public string location
		{
			get;
			set;
		}

		public void WriteToXml(XmlDocument doc)
		{
			XmlNode root = doc.SelectSingleNode("Settings");
			CustomerServiceSettings.SetNodeValue(doc, root, "AppId", this.AppId);
			CustomerServiceSettings.SetNodeValue(doc, root, "AppSecret", this.AppSecret);
			CustomerServiceSettings.SetNodeValue(doc, root, "unitid", this.unitid);
			CustomerServiceSettings.SetNodeValue(doc, root, "unit", this.unit);
			CustomerServiceSettings.SetNodeValue(doc, root, "password", this.password);
			CustomerServiceSettings.SetNodeValue(doc, root, "unitname", this.unitname);
			CustomerServiceSettings.SetNodeValue(doc, root, "activated", this.activated);
			CustomerServiceSettings.SetNodeValue(doc, root, "logo", this.logo);
			CustomerServiceSettings.SetNodeValue(doc, root, "url", this.url);
			CustomerServiceSettings.SetNodeValue(doc, root, "tel", this.tel);
			CustomerServiceSettings.SetNodeValue(doc, root, "contact", this.contact);
			CustomerServiceSettings.SetNodeValue(doc, root, "location", this.location);
		}

		public static CustomerServiceSettings FromXml(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("Settings");
			return new CustomerServiceSettings
			{
				AppId = xmlNode.SelectSingleNode("AppId").InnerText,
				AppSecret = xmlNode.SelectSingleNode("AppSecret").InnerText,
				unitid = xmlNode.SelectSingleNode("unitid").InnerText,
				unit = xmlNode.SelectSingleNode("unit").InnerText,
				password = xmlNode.SelectSingleNode("password").InnerText,
				unitname = xmlNode.SelectSingleNode("unitname").InnerText,
				activated = xmlNode.SelectSingleNode("activated").InnerText,
				logo = xmlNode.SelectSingleNode("logo").InnerText,
				url = xmlNode.SelectSingleNode("url").InnerText,
				tel = xmlNode.SelectSingleNode("tel").InnerText,
				contact = xmlNode.SelectSingleNode("contact").InnerText,
				location = xmlNode.SelectSingleNode("location").InnerText
			};
		}

		private static void SetNodeValue(XmlDocument doc, XmlNode root, string nodeName, string nodeValue)
		{
			XmlNode xmlNode = root.SelectSingleNode(nodeName);
			if (xmlNode == null)
			{
				xmlNode = doc.CreateElement(nodeName);
				root.AppendChild(xmlNode);
			}
			xmlNode.InnerText = nodeValue;
		}
	}
}
