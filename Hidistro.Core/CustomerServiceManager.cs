using Hidistro.Core.Entities;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hidistro.Core
{
	public class CustomerServiceManager
	{
		private const string MasterSettingsCacheKey = "FileCache-CustomerServiceSettings";

		public static void Save(CustomerServiceSettings settings)
		{
			CustomerServiceManager.SaveMasterSettings(settings);
			HiCache.Remove("FileCache-CustomerServiceSettings");
		}

		public static CustomerServiceSettings GetMasterSettings(bool cacheable)
		{
			if (!cacheable)
			{
				HiCache.Remove("FileCache-CustomerServiceSettings");
			}
			XmlDocument xmlDocument = HiCache.Get("FileCache-CustomerServiceSettings") as XmlDocument;
			CustomerServiceSettings result;
			if (xmlDocument == null)
			{
				string masterSettingsFilename = CustomerServiceManager.GetMasterSettingsFilename();
				if (!File.Exists(masterSettingsFilename))
				{
					result = null;
					return result;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.Load(masterSettingsFilename);
				if (cacheable)
				{
					HiCache.Max("FileCache-CustomerServiceSettings", xmlDocument, new CacheDependency(masterSettingsFilename));
				}
			}
			result = CustomerServiceSettings.FromXml(xmlDocument);
			return result;
		}

		private static void SaveMasterSettings(CustomerServiceSettings settings)
		{
			string masterSettingsFilename = CustomerServiceManager.GetMasterSettingsFilename();
			XmlDocument xmlDocument = new XmlDocument();
			if (File.Exists(masterSettingsFilename))
			{
				xmlDocument.Load(masterSettingsFilename);
			}
			settings.WriteToXml(xmlDocument);
			xmlDocument.Save(masterSettingsFilename);
		}

		private static string GetMasterSettingsFilename()
		{
			HttpContext current = HttpContext.Current;
			return (current != null) ? current.Request.MapPath("~/config/CustomerService.config") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\CustomerService.config");
		}
	}
}
