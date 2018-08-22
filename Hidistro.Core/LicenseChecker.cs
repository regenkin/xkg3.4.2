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
	public static class LicenseChecker
	{
		private const string CacheCommercialKey = "FileCache_CommercialLicenser";

		public static void Check(out bool isValid, out bool expired, out int siteQty)
		{
			siteQty = 0;
			isValid = false;
			expired = true;
			XmlDocument xmlDocument = HiCache.Get("FileCache_CommercialLicenser") as XmlDocument;
			if (xmlDocument == null)
			{
				string text = HttpContext.Current.Request.MapPath("/config/Certificates.cer");
				if (!File.Exists(text))
				{
					return;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(File.ReadAllText(text));
				HiCache.Max("FileCache_CommercialLicenser", xmlDocument, new CacheDependency(text));
			}
			XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//Host");
			XmlNode xmlNode2 = xmlDocument.DocumentElement.SelectSingleNode("//LicenseDate");
			XmlNode xmlNode3 = xmlDocument.DocumentElement.SelectSingleNode("//Expires");
			XmlNode xmlNode4 = xmlDocument.DocumentElement.SelectSingleNode("//SiteQty");
			XmlNode xmlNode5 = xmlDocument.DocumentElement.SelectSingleNode("//Signature");
			if (string.Compare(xmlNode.InnerText, HttpContext.Current.Request.Url.Host, true, CultureInfo.InvariantCulture) == 0)
			{
				string s = string.Format(CultureInfo.InvariantCulture, "Host={0}&Expires={1}&SiteQty={2}&LicenseDate={3}", new object[]
				{
					HttpContext.Current.Request.Url.Host,
					xmlNode3.InnerText,
					xmlNode4.InnerText,
					xmlNode2.InnerText
				});
				using (RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider())
				{
					rSACryptoServiceProvider.FromXmlString(LicenseHelper.GetPublicKey());
					RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
					rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
					byte[] rgbSignature = Convert.FromBase64String(xmlNode5.InnerText);
					SHA1Managed sHA1Managed = new SHA1Managed();
					byte[] rgbHash = sHA1Managed.ComputeHash(Encoding.UTF8.GetBytes(s));
					isValid = rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
				}
				expired = (DateTime.Now > DateTime.Parse(xmlNode3.InnerText));
				if (isValid && !expired)
				{
					int.TryParse(xmlNode4.InnerText, out siteQty);
				}
			}
		}
	}
}
