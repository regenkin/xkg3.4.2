using Hidistro.Core.Entities;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hidistro.Core
{
	public static class SettingsManager
	{
		private const string MasterSettingsCacheKey = "FileCache-MasterSettings";

		public static void Save(SiteSettings settings)
		{
			SettingsManager.SaveMasterSettings(settings);
			HiCache.Remove("FileCache-MasterSettings");
		}

		public static SiteSettings GetMasterSettings(bool cacheable)
		{
			if (!cacheable)
			{
				HiCache.Remove("FileCache-MasterSettings");
			}
			XmlDocument xmlDocument = HiCache.Get("FileCache-MasterSettings") as XmlDocument;
			SiteSettings result;
			if (xmlDocument == null)
			{
				string masterSettingsFilename = SettingsManager.GetMasterSettingsFilename();
				if (!File.Exists(masterSettingsFilename))
				{
					result = null;
					return result;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.Load(masterSettingsFilename);
				if (cacheable)
				{
					HiCache.Max("FileCache-MasterSettings", xmlDocument, new CacheDependency(masterSettingsFilename));
				}
			}
			result = SiteSettings.FromXml(xmlDocument);
			return result;
		}

		private static void SaveMasterSettings(SiteSettings settings)
		{
			string masterSettingsFilename = SettingsManager.GetMasterSettingsFilename();
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
			System.Web.HttpContext current = System.Web.HttpContext.Current;
			return (current != null) ? current.Request.MapPath("~/config/SiteSettings.config") : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\SiteSettings.config");
		}
	}
}
