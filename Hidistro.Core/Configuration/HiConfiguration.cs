using Hidistro.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Xml;

namespace Hidistro.Core.Configuration
{
	public class HiConfiguration
	{
		private const string ConfigCacheKey = "FileCache-Configuragion";

		private const string filesPath = "/";

		private short smtpServerConnectionLimit = -1;

		private AppLocation app;

		private SSLSettings ssl = SSLSettings.Ignore;

		private RolesConfiguration roleConfiguration;

		private readonly XmlDocument xmlDoc;

		private readonly Dictionary<string, string> supportedLanguages = new Dictionary<string, string>();

		private readonly Dictionary<string, string> integratedApplications = new Dictionary<string, string>();

		private int usernameMinLength = 3;

		private int usernameMaxLength = 20;

		private string usernameRegex = "[一-龥a-zA-Z0-9]+[一-龥_a-zA-Z0-9]*";

		private string emailEncoding = "utf-8";

		private int shippingAddressQuantity = 5;

		private int passwordMaxLength = 16;

		private string emailRegex = "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3,4}){1,2})";

		private string adminFolder = "admin";

		private bool useUniversalCode = false;

		public static readonly int[,] ThumbnailSizes = new int[,]
		{
			{
				10,
				10
			},
			{
				22,
				22
			},
			{
				40,
				40
			},
			{
				100,
				100
			},
			{
				160,
				160
			},
			{
				310,
				310
			}
		};

		public string FilesPath
		{
			get
			{
				return "/";
			}
		}

		public RolesConfiguration RolesConfiguration
		{
			get
			{
				return this.roleConfiguration;
			}
		}

		public int QueuedThreads
		{
			get
			{
				return 2;
			}
		}

		public SSLSettings SSL
		{
			get
			{
				return this.ssl;
			}
		}

		public Dictionary<string, string> SupportedLanguages
		{
			get
			{
				return this.supportedLanguages;
			}
		}

		public Dictionary<string, string> IntegratedApplications
		{
			get
			{
				return this.integratedApplications;
			}
		}

		public AppLocation AppLocation
		{
			get
			{
				return this.app;
			}
		}

		public short SmtpServerConnectionLimit
		{
			get
			{
				return this.smtpServerConnectionLimit;
			}
		}

		public int UsernameMinLength
		{
			get
			{
				return this.usernameMinLength;
			}
		}

		public int UsernameMaxLength
		{
			get
			{
				return this.usernameMaxLength;
			}
		}

		public string UsernameRegex
		{
			get
			{
				return this.usernameRegex;
			}
		}

		public string EmailEncoding
		{
			get
			{
				return this.emailEncoding;
			}
		}

		public int ShippingAddressQuantity
		{
			get
			{
				return this.shippingAddressQuantity;
			}
		}

		public int PasswordMaxLength
		{
			get
			{
				return this.passwordMaxLength;
			}
		}

		public string EmailRegex
		{
			get
			{
				return this.emailRegex;
			}
		}

		public string AdminFolder
		{
			get
			{
				return this.adminFolder;
			}
		}

		public bool UseUniversalCode
		{
			get
			{
				return this.useUniversalCode;
			}
		}

		public HiConfiguration(XmlDocument doc)
		{
			this.xmlDoc = doc;
			this.LoadValuesFromConfigurationXml();
		}

		public XmlNode GetConfigSection(string nodePath)
		{
			return this.xmlDoc.SelectSingleNode(nodePath);
		}

		public static HiConfiguration GetConfig()
		{
			HiConfiguration hiConfiguration = HiCache.Get("FileCache-Configuragion") as HiConfiguration;
			if (hiConfiguration == null)
			{
				HttpContext current = HttpContext.Current;
				string filename = null;
				if (current != null)
				{
					try
					{
						filename = current.Request.MapPath("~/config/Hishop.config");
					}
					catch
					{
						filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\Hishop.config");
					}
				}
				else
				{
					filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\Hishop.config");
				}
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				hiConfiguration = new HiConfiguration(xmlDocument);
				HiCache.Max("FileCache-Configuragion", hiConfiguration, new CacheDependency(filename));
			}
			return hiConfiguration;
		}

		internal void LoadValuesFromConfigurationXml()
		{
			XmlNode configSection = this.GetConfigSection("Hishop/Core");
			XmlAttributeCollection attributes = configSection.Attributes;
			this.GetAttributes(attributes);
			foreach (XmlNode xmlNode in configSection.ChildNodes)
			{
				if (xmlNode.Name == "Languages")
				{
					this.GetLanguages(xmlNode);
				}
				if (xmlNode.Name == "appLocation")
				{
					this.GetAppLocation(xmlNode);
				}
				if (xmlNode.Name == "IntegratedApplications")
				{
					this.GetIntegratedApplications(xmlNode);
				}
			}
			if (this.app == null)
			{
				this.app = AppLocation.Default();
			}
			if (this.roleConfiguration == null)
			{
				this.roleConfiguration = new RolesConfiguration();
			}
		}

		internal void GetAttributes(XmlAttributeCollection attributeCollection)
		{
			XmlAttribute xmlAttribute = attributeCollection["smtpServerConnectionLimit"];
			if (xmlAttribute != null)
			{
				this.smtpServerConnectionLimit = short.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
			}
			else
			{
				this.smtpServerConnectionLimit = -1;
			}
			xmlAttribute = attributeCollection["ssl"];
			if (xmlAttribute != null)
			{
				this.ssl = (SSLSettings)Enum.Parse(typeof(SSLSettings), xmlAttribute.Value, true);
			}
			xmlAttribute = attributeCollection["usernameMinLength"];
			if (xmlAttribute != null)
			{
				this.usernameMinLength = int.Parse(xmlAttribute.Value);
			}
			xmlAttribute = attributeCollection["usernameMaxLength"];
			if (xmlAttribute != null)
			{
				this.usernameMaxLength = int.Parse(xmlAttribute.Value);
			}
			xmlAttribute = attributeCollection["usernameRegex"];
			if (xmlAttribute != null)
			{
				this.usernameRegex = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["emailEncoding"];
			if (xmlAttribute != null)
			{
				this.emailEncoding = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["shippingAddressQuantity"];
			if (xmlAttribute != null)
			{
				this.shippingAddressQuantity = int.Parse(xmlAttribute.Value);
			}
			xmlAttribute = attributeCollection["passwordMaxLength"];
			if (xmlAttribute != null)
			{
				this.passwordMaxLength = int.Parse(xmlAttribute.Value);
			}
			if (this.passwordMaxLength < Membership.Provider.MinRequiredPasswordLength)
			{
				this.passwordMaxLength = 16;
			}
			xmlAttribute = attributeCollection["emailRegex"];
			if (xmlAttribute != null)
			{
				this.emailRegex = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["adminFolder"];
			if (xmlAttribute != null)
			{
				this.adminFolder = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["useUniversalCode"];
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true"))
			{
				this.useUniversalCode = true;
			}
		}

		internal void GetIntegratedApplications(XmlNode node)
		{
			foreach (XmlNode xmlNode in node.ChildNodes)
			{
				if (!this.integratedApplications.ContainsKey(xmlNode.Attributes["applicationName"].Value))
				{
					this.integratedApplications.Add(xmlNode.Attributes["applicationName"].Value, xmlNode.Attributes["implement"].Value);
				}
			}
		}

		internal void GetLanguages(XmlNode node)
		{
			foreach (XmlNode xmlNode in node.ChildNodes)
			{
				if (string.Compare(xmlNode.Attributes["enabled"].Value, "true", false, CultureInfo.InvariantCulture) == 0 && !this.supportedLanguages.ContainsKey(xmlNode.Attributes["key"].Value))
				{
					this.supportedLanguages.Add(xmlNode.Attributes["key"].Value, xmlNode.Attributes["name"].Value);
				}
			}
		}

		internal void GetAppLocation(XmlNode node)
		{
			this.app = AppLocation.Create(node);
		}
	}
}
