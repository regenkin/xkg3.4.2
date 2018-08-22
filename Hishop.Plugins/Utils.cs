using System;
using System.IO;
using System.Reflection;
using System.Web;

namespace Hishop.Plugins
{
	public static class Utils
	{
		public static string ApplicationPath
		{
			get
			{
				string text = "/";
				if (HttpContext.Current != null)
				{
					text = HttpContext.Current.Request.ApplicationPath;
				}
				string result;
				if (text == "/")
				{
					result = string.Empty;
				}
				else
				{
					result = text;
				}
				return result;
			}
		}

		public static string GetResourceContent(string sFileName)
		{
			string result;
			try
			{
				string text = "";
				using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(sFileName))
				{
					using (StreamReader streamReader = new StreamReader(manifestResourceStream))
					{
						text = streamReader.ReadToEnd();
					}
				}
				result = text;
			}
			catch (Exception ex)
			{
				throw new Exception(string.Concat(new object[]
				{
					"Could not read resource \"",
					sFileName,
					"\": ",
					ex
				}));
			}
			return result;
		}
	}
}
