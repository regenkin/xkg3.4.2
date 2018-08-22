using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Text;

namespace Hidistro.Core.Urls
{
	public class LocationSet : IEnumerable
	{
		private ListDictionary locations = new ListDictionary();

		public string this[string name]
		{
			get
			{
				string result;
				if (null == name)
				{
					result = string.Empty;
				}
				else
				{
					result = ((Location)this.locations[name.ToLower(CultureInfo.InvariantCulture)]).Path;
				}
				return result;
			}
		}

		public string Filter
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (Location location in this.locations.Values)
				{
					if (location.Exclude && location.Path != null && location.Path.Length > 1)
					{
						stringBuilder.Append("|" + location.Path);
					}
				}
				string text = stringBuilder.ToString();
				if (text != null && text.Length > 0)
				{
					text = text.Substring(1);
				}
				return text;
			}
		}

		public void Add(string name, Location location)
		{
			if (null != name)
			{
				this.locations[name.ToLower(CultureInfo.InvariantCulture)] = location;
			}
		}

		public Location FindLocationByPath(string path)
		{
			Location result;
			foreach (Location location in this.locations.Values)
			{
				if (location.IsMatch(path))
				{
					result = location;
					return result;
				}
			}
			result = null;
			return result;
		}

		public Location FindLocationByName(string name)
		{
			Location result;
			if (null == name)
			{
				result = null;
			}
			else
			{
				name = name.ToLower(CultureInfo.InvariantCulture);
				foreach (string a in this.locations.Keys)
				{
					if (a == name)
					{
						result = (this.locations[name] as Location);
						return result;
					}
				}
				result = null;
			}
			return result;
		}

		public IEnumerator GetEnumerator()
		{
			return this.locations.GetEnumerator();
		}
	}
}
