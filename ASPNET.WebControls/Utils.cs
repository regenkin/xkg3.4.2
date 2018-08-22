using System;
using System.Web;

namespace ASPNET.WebControls
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
				if (text == "/")
				{
					return string.Empty;
				}
				return text;
			}
		}

		public static bool IsUrlAbsolute(string url)
		{
			if (url == null)
			{
				return false;
			}
			string[] array = new string[]
			{
				"about:",
				"file:///",
				"ftp://",
				"gopher://",
				"http://",
				"https://",
				"javascript:",
				"mailto:",
				"news:",
				"res://",
				"telnet://",
				"view-source:"
			};
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				if (url.StartsWith(value))
				{
					return true;
				}
			}
			return false;
		}
	}
}
