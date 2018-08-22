using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;

namespace Hidistro.Core.Urls
{
	public class SiteUrlsData
	{
		private bool enableHtmRewrite;

		private string extension;

		private NameValueCollection _paths = null;

		private NameValueCollection _reversePaths = null;

		private string _locationFilter;

		private LocationSet _locationSet = null;

		public LocationSet LocationSet
		{
			get
			{
				return this._locationSet;
			}
		}

		public string LocationFilter
		{
			get
			{
				return this._locationFilter;
			}
		}

		public NameValueCollection Paths
		{
			get
			{
				return this._paths;
			}
		}

		public NameValueCollection ReversePaths
		{
			get
			{
				return this._reversePaths;
			}
		}

		public SiteUrlsData(string siteUrlsXmlFile)
		{
			this._paths = new NameValueCollection();
			this._reversePaths = new NameValueCollection();
			this.Initialize(siteUrlsXmlFile);
		}

		protected XmlDocument CreateDoc(string siteUrlsXmlFile)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(siteUrlsXmlFile);
			return xmlDocument;
		}

		protected void Initialize(string siteUrlsXmlFile)
		{
			string text = Globals.ApplicationPath;
			if (text != null)
			{
				text = text.Trim();
			}
			XmlDocument xmlDocument = this.CreateDoc(siteUrlsXmlFile);
			this.enableHtmRewrite = (string.Compare(xmlDocument.SelectSingleNode("SiteUrls").Attributes["enableHtmRewrite"].Value, "true", true) == 0);
			this.extension = xmlDocument.SelectSingleNode("SiteUrls").Attributes["extension"].Value;
			XmlNode basePaths = xmlDocument.SelectSingleNode("SiteUrls/locations");
			this._locationSet = this.CreateLocationSet(basePaths, text);
			this._locationFilter = this._locationSet.Filter;
			XmlNode transformers = xmlDocument.SelectSingleNode("SiteUrls/transformers");
			ListDictionary transforms = SiteUrlsData.CreateTransformers(transformers);
			XmlNode urls = xmlDocument.SelectSingleNode("SiteUrls/urls");
			this.CreateUrls(urls, transforms);
		}

		private static ListDictionary CreateTransformers(XmlNode transformers)
		{
			ListDictionary listDictionary = new ListDictionary();
			foreach (XmlNode xmlNode in transformers.ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					string value = xmlNode.Attributes["key"].Value;
					string value2 = xmlNode.Attributes["value"].Value;
					if (!string.IsNullOrEmpty(value))
					{
						listDictionary[value] = value2;
					}
				}
			}
			return listDictionary;
		}

		private void CreateUrls(XmlNode urls, ListDictionary transforms)
		{
			foreach (XmlNode xmlNode in urls.ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					bool flag = this.enableHtmRewrite && xmlNode.Attributes["htmRewrite"] != null && string.Compare(xmlNode.Attributes["htmRewrite"].Value, "true", true) == 0;
					string value = xmlNode.Attributes["name"].Value;
					XmlAttribute xmlAttribute = xmlNode.Attributes["navigateUrl"];
					if (xmlAttribute != null)
					{
						this._paths.Add(value, xmlAttribute.Value);
					}
					else
					{
						string value2 = xmlNode.Attributes["location"].Value;
						string text = null;
						string text2 = null;
						XmlAttribute xmlAttribute2 = xmlNode.Attributes["vanity"];
						XmlAttribute xmlAttribute3 = xmlNode.Attributes["pattern"];
						Location location = this._locationSet.FindLocationByName(value2);
						if (xmlAttribute2 != null && xmlAttribute3 != null)
						{
							text2 = location.Path + xmlAttribute3.Value;
							text = location.PhysicalPath + xmlAttribute2.Value;
						}
						string text3;
						if (flag)
						{
							text3 = xmlNode.Attributes["path"].Value.Replace(".aspx", this.extension);
							if (string.IsNullOrEmpty(text2) || string.IsNullOrEmpty(text))
							{
								text2 = location.Path + xmlNode.Attributes["path"].Value;
								text = location.PhysicalPath + xmlNode.Attributes["path"].Value;
							}
							text2 = text2.Replace(".aspx", this.extension);
						}
						else
						{
							text3 = xmlNode.Attributes["path"].Value;
						}
						foreach (string text4 in transforms.Keys)
						{
							text3 = text3.Replace(text4, transforms[text4].ToString());
							if (!string.IsNullOrEmpty(text2))
							{
								text2 = text2.Replace(text4, transforms[text4].ToString());
							}
							if (!string.IsNullOrEmpty(text))
							{
								text = text.Replace(text4, transforms[text4].ToString());
							}
						}
						this._paths.Add(value, location.Path + text3);
						string text5 = location.Path + text3;
						if (Globals.ApplicationPath.Length > 0)
						{
							text5 = text5.Replace(Globals.ApplicationPath, "").ToLower(CultureInfo.InvariantCulture);
						}
						this._reversePaths.Add(text5, value);
						if (!string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text))
						{
							location.Add(new ReWrittenUrl(value2 + "." + value, text2, text));
						}
					}
				}
			}
		}

		private LocationSet CreateLocationSet(XmlNode basePaths, string globalPath)
		{
			LocationSet locationSet = new LocationSet();
			foreach (XmlNode xmlNode in basePaths.ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment)
				{
					XmlAttribute xmlAttribute = xmlNode.Attributes["name"];
					XmlAttribute xmlAttribute2 = xmlNode.Attributes["path"];
					XmlAttribute xmlAttribute3 = xmlNode.Attributes["physicalPath"];
					XmlAttribute xmlAttribute4 = xmlNode.Attributes["exclude"];
					XmlAttribute xmlAttribute5 = xmlNode.Attributes["type"];
					if (xmlAttribute != null && xmlAttribute2 != null)
					{
						string text = null;
						if (xmlAttribute3 != null)
						{
							text = globalPath + xmlAttribute3.Value;
						}
						bool flag = xmlAttribute4 != null && bool.Parse(xmlAttribute4.Value);
						string text2 = globalPath + xmlAttribute2.Value;
						Location location;
						if (xmlAttribute5 == null)
						{
							location = new Location(text2, text, flag);
						}
						else
						{
							Type type = Type.GetType(xmlAttribute5.Value);
							location = (Activator.CreateInstance(type, new object[]
							{
								text2,
								text,
								flag
							}) as Location);
						}
						locationSet.Add(xmlAttribute.Value, location);
					}
				}
			}
			return locationSet;
		}

		public string FormatUrl(string name)
		{
			return this.FormatUrl(name, null);
		}

		public virtual string FormatUrl(string name, params object[] parameters)
		{
			string result;
			if (parameters == null)
			{
				result = this.Paths[name].Trim();
			}
			else
			{
				result = string.Format(CultureInfo.InvariantCulture, this.Paths[name].Trim(), parameters);
			}
			return result;
		}

		private static string ResolveUrl(string path)
		{
			string result;
			if (Globals.ApplicationPath.Length > 0)
			{
				result = Globals.ApplicationPath + path;
			}
			else
			{
				result = path;
			}
			return result;
		}
	}
}
