using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace Hidistro.Core.Urls
{
	public class Location : IEnumerable
	{
		private ArrayList _urls = new ArrayList();

		private Regex _regex = null;

		private string _path;

		private string _physicalPath;

		private bool _exclude = false;

		public int Count
		{
			get
			{
				return this._urls.Count;
			}
		}

		public bool Exclude
		{
			get
			{
				return this._exclude;
			}
		}

		public string PhysicalPath
		{
			get
			{
				return this._physicalPath;
			}
		}

		public string Path
		{
			get
			{
				return this._path;
			}
		}

		public void Add(ReWrittenUrl url)
		{
			this._urls.Add(url);
		}

		public IEnumerator GetEnumerator()
		{
			return this._urls.GetEnumerator();
		}

		public Location(string path, string physicalPath, bool exclude)
		{
			this._path = path;
			if (physicalPath == null)
			{
				this._physicalPath = path;
			}
			else
			{
				this._physicalPath = physicalPath;
			}
			this._exclude = exclude;
			if (!string.IsNullOrEmpty(path))
			{
				this._regex = new Regex(path, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			}
		}

		public bool IsMatch(string url)
		{
			return this._regex != null && this._regex.IsMatch(url);
		}

		public virtual string ReWriteUrl(string path, string queryString)
		{
			string result = null;
			if (this.Count > 0)
			{
				foreach (ReWrittenUrl reWrittenUrl in this)
				{
					if (reWrittenUrl.IsMatch(path))
					{
						result = reWrittenUrl.Convert(path, queryString);
						break;
					}
				}
			}
			return result;
		}
	}
}
