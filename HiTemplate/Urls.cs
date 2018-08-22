using System;
using System.Web;

namespace HiTemplate
{
	public static class Urls
	{
		public static string ApplicationPath
		{
			get
			{
				string text = "http://";
				text += HttpContext.Current.Request.Url.Host;
				if (HttpContext.Current.Request.Url.Port != 80)
				{
					text = text + ":" + HttpContext.Current.Request.Url.Port;
				}
				return text;
			}
		}
	}
}
