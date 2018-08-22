using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Hidistro.Core.Urls
{
	public class ReWrittenUrl
	{
		private string _name;

		private string _path;

		private string _pattern;

		private Regex _regex = null;

		public string Pattern
		{
			get
			{
				return this._pattern;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public string Path
		{
			get
			{
				return this._path;
			}
		}

		public ReWrittenUrl(string name, string pattern, string path)
		{
			this._name = name;
			this._path = path;
			this._pattern = pattern;
			this._regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}

		public bool IsMatch(string url)
		{
			return this._regex.IsMatch(url);
		}

		public virtual string Convert(string url, string qs)
		{
			if (qs != null && qs.StartsWith("?"))
			{
				qs = qs.Replace("?", "&");
			}
			return string.Format(CultureInfo.InvariantCulture, "{0}{1}", new object[]
			{
				this._regex.Replace(url, this._path),
				qs
			});
		}
	}
}
