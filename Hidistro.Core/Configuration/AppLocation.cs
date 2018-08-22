using Hidistro.Core.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace Hidistro.Core.Configuration
{
	public class AppLocation
	{
		private const string HttpContextAppLocation = "AppLocation";

		private Regex regex;

		private string pattern;

		private string defaultName;

		private Hashtable ht = new Hashtable();

		private IList<string> keys = new List<string>();

		public string CurrentName
		{
			get
			{
				HiApplication hiApplication = this.CurrentHiApplication();
				string result;
				if (hiApplication != null)
				{
					result = hiApplication.Name;
				}
				else
				{
					result = null;
				}
				return result;
			}
		}

		public ApplicationType CurrentApplicationType
		{
			get
			{
				HiApplication hiApplication = this.CurrentHiApplication();
				ApplicationType result;
				if (hiApplication != null)
				{
					result = hiApplication.ApplicationType;
				}
				else
				{
					result = ApplicationType.Unknown;
				}
				return result;
			}
		}

		public string Pattern
		{
			get
			{
				return this.pattern;
			}
			set
			{
				this.pattern = value;
				if (this.pattern != null)
				{
					this.regex = new Regex(this.pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
				}
			}
		}

		public string DefaultName
		{
			get
			{
				return this.defaultName;
			}
			set
			{
				this.defaultName = value;
			}
		}

		public bool IsKnownApplication
		{
			get
			{
				return this.CurrentApplicationType != ApplicationType.Unknown;
			}
		}

		internal void Add(HiApplication app)
		{
			if (!this.ht.Contains(app.Name))
			{
				this.ht.Add(app.Name, app);
				this.keys.Add(app.Name);
				return;
			}
			throw new Exception(string.Format(CultureInfo.InvariantCulture, "The HiApplication.Name ({0}) was not unique", new object[]
			{
				app.Name
			}));
		}

		internal HiApplication LookUp(string url)
		{
			HiApplication result;
			if (this.Pattern != null)
			{
				if (!this.regex.IsMatch(url))
				{
					result = null;
					return result;
				}
			}
			for (int i = 0; i < this.keys.Count; i++)
			{
				HiApplication hiApplication = this.ht[this.keys[i]] as HiApplication;
				if (hiApplication.IsMatch(url))
				{
					result = hiApplication;
					return result;
				}
			}
			if (this.DefaultName != null)
			{
				result = (this.ht[this.DefaultName] as HiApplication);
			}
			else
			{
				result = null;
			}
			return result;
		}

		internal HiApplication CurrentHiApplication()
		{
			HttpContext current = HttpContext.Current;
			HiApplication result;
			if (current == null)
			{
				result = null;
			}
			else
			{
				HiApplication hiApplication = current.Items["AppLocation"] as HiApplication;
				if (hiApplication == null)
				{
					hiApplication = this.LookUp(current.Request.Path);
					if (hiApplication != null)
					{
						current.Items.Add("AppLocation", hiApplication);
					}
				}
				result = hiApplication;
			}
			return result;
		}

		public bool IsName(string name)
		{
			return string.Compare(name, this.CurrentName, true, CultureInfo.InvariantCulture) == 0;
		}

		public bool IsApplicationType(ApplicationType applicationType)
		{
			return this.CurrentApplicationType == applicationType;
		}

		public static AppLocation Default()
		{
			AppLocation appLocation = new AppLocation();
			appLocation.Add(new HiApplication("/", "Common", ApplicationType.Common));
			return appLocation;
		}

		public static AppLocation Create(XmlNode node)
		{
			AppLocation result;
			if (node == null)
			{
				result = null;
			}
			else
			{
				AppLocation appLocation = new AppLocation();
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes != null)
				{
					foreach (XmlAttribute xmlAttribute in attributes)
					{
						if (xmlAttribute.Name == "pattern")
						{
							appLocation.Pattern = Globals.ApplicationPath + xmlAttribute.Value;
						}
						else if (xmlAttribute.Name == "defaultName")
						{
							appLocation.DefaultName = xmlAttribute.Value;
						}
					}
					for (int i = 0; i < node.ChildNodes.Count; i++)
					{
						XmlNode xmlNode = node.ChildNodes[i];
						if (xmlNode.Name == "add")
						{
							XmlAttributeCollection attributes2 = xmlNode.Attributes;
							if (attributes2 != null)
							{
								string text = Globals.ApplicationPath + attributes2["pattern"].Value;
								string value = attributes2["name"].Value;
								ApplicationType appType = (ApplicationType)Enum.Parse(typeof(ApplicationType), attributes2["type"].Value, true);
								appLocation.Add(new HiApplication(text, value, appType));
							}
						}
					}
				}
				result = appLocation;
			}
			return result;
		}
	}
}
