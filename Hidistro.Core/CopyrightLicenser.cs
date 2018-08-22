using Hidistro.Core.Entities;
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hidistro.Core
{
	public sealed class CopyrightLicenser
	{
		public const string CacheCopyrightKey = "Hishop_SiteLicense";

		private CopyrightLicenser()
		{
		}

		public static bool CheckCopyright()
		{
			XmlDocument xmlDocument = HiCache.Get("Hishop_SiteLicense") as XmlDocument;
			HttpContext current = HttpContext.Current;
			bool result;
			if (xmlDocument == null)
			{
				string text;
				if (current != null)
				{
					text = current.Request.MapPath("~/config/Hishop.lic");
				}
				else
				{
					text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Hishop.lic");
				}
				if (!File.Exists(text))
				{
					result = false;
					return result;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(File.ReadAllText(text));
				HiCache.Max("Hishop_SiteLicense", xmlDocument, new CacheDependency(text));
			}
			XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//Host");
			XmlNode xmlNode2 = xmlDocument.DocumentElement.SelectSingleNode("//LicenseDate");
			XmlNode xmlNode3 = xmlDocument.DocumentElement.SelectSingleNode("//ExpiresDate");
			XmlNode xmlNode4 = xmlDocument.DocumentElement.SelectSingleNode("//Signature");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (string.Compare(xmlNode.InnerText, masterSettings.SiteUrl, true, CultureInfo.InvariantCulture) != 0)
			{
				result = false;
			}
			else
			{
				string s = string.Format(CultureInfo.InvariantCulture, "Host={0}&LicenseDate={1}&ExpiresDate={2}&Key={3}", new object[]
				{
					masterSettings.SiteUrl,
					xmlNode2.InnerText,
					xmlNode3.InnerText,
					masterSettings.CheckCode
				});
				bool flag = false;
				using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
				{
					rSACryptoServiceProvider.FromXmlString(LicenseHelper.GetPublicKey());
					RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
					rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
					byte[] rgbSignature = Convert.FromBase64String(xmlNode4.InnerText);
					SHA1Managed sHA1Managed = new SHA1Managed();
					byte[] rgbHash = sHA1Managed.ComputeHash(Encoding.UTF8.GetBytes(s));
					flag = rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
				}
				result = (flag && DateTime.Now < DateTime.Parse(xmlNode3.InnerText));
			}
			return result;
		}
	}
}
