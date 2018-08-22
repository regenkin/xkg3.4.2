using System;

namespace Hidistro.Core.Urls
{
	public abstract class UrlReWriteProvider
	{
		private static readonly UrlReWriteProvider _instance;

		static UrlReWriteProvider()
		{
			UrlReWriteProvider._instance = new HiUrlReWriter();
		}

		public static UrlReWriteProvider Instance()
		{
			if (UrlReWriteProvider._instance == null)
			{
				throw new Exception("UrlReWriteProvider could not be loaded");
			}
			return UrlReWriteProvider._instance;
		}

		public abstract string RewriteUrl(string path, string queryString);
	}
}
