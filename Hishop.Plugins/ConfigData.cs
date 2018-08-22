using System;
using System.Collections.Generic;
using System.Xml;

namespace Hishop.Plugins
{
	public class ConfigData
	{
		private readonly XmlDocument doc;

		private readonly XmlNode root;

		public bool IsValid
		{
			get;
			internal set;
		}

		public IList<string> ErrorMsgs
		{
			get;
			private set;
		}

		public bool NeedProtect
		{
			get;
			internal set;
		}

		internal XmlNodeList AttributeNodes
		{
			get
			{
				return this.root.ChildNodes;
			}
		}

		public string SettingsXml
		{
			get
			{
				return this.root.OuterXml;
			}
		}

		public ConfigData()
		{
			this.IsValid = true;
			this.ErrorMsgs = new List<string>();
			this.doc = new XmlDocument();
			this.root = this.doc.CreateElement("xml");
			this.doc.AppendChild(this.root);
		}

		public ConfigData(string xml)
		{
			this.IsValid = true;
			this.ErrorMsgs = new List<string>();
			this.doc = new XmlDocument();
			this.doc.LoadXml(xml);
			this.root = this.doc.FirstChild;
		}

		internal void Add(string attributeName, string val)
		{
			if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(val) && attributeName.Trim().Length > 0 && val.Length > 0)
			{
				XmlNode xmlNode = this.doc.CreateElement(attributeName);
				xmlNode.InnerText = val;
				this.root.AppendChild(xmlNode);
			}
		}
	}
}
