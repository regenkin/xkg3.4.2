using System;
using System.Text.RegularExpressions;

namespace Hidistro.Core.Urls
{
	public class HiUrlReWriter : UrlReWriteProvider
	{
		private static Regex ReWriteFilter;

		static HiUrlReWriter()
		{
			HiUrlReWriter.ReWriteFilter = null;
			HiUrlReWriter.ReWriteFilter = new Regex(Globals.GetSiteUrls().LocationFilter, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		public override string RewriteUrl(string path, string queryString)
		{
			string result = null;
			if (!HiUrlReWriter.ReWriteFilter.IsMatch(path))
			{
				Location location = Globals.GetSiteUrls().LocationSet.FindLocationByPath(path);
				if (location != null)
				{
					result = location.ReWriteUrl(path, queryString);
				}
			}
			return result;
		}
	}
}
