using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Hishop.Plugins
{
	public class PluginItemCollection
	{
		private Dictionary<string, PluginItem> plugins = new Dictionary<string, PluginItem>();

		public int Count
		{
			get
			{
				return this.plugins.Count;
			}
		}

		public PluginItem[] Items
		{
			get
			{
				PluginItem[] array = new PluginItem[this.plugins.Count];
				this.plugins.Values.CopyTo(array, 0);
				return array;
			}
		}

		public void Add(PluginItem item)
		{
			if (!this.plugins.ContainsKey(item.FullName))
			{
				this.plugins.Add(item.FullName, item);
			}
		}

		public void Remove(string fullName)
		{
			this.plugins.Remove(fullName);
		}

		public bool ContainsKey(string fullName)
		{
			return this.plugins.ContainsKey(fullName);
		}

		public virtual string ToJsonString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.AppendFormat("\"qty\":{0}", this.plugins.Count.ToString(CultureInfo.InvariantCulture));
			if (this.plugins.Count > 0)
			{
				stringBuilder.Append(",\"items\":[");
				foreach (string current in this.plugins.Keys)
				{
					PluginItem pluginItem = this.plugins[current];
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"FullName\":\"{0}\",\"DisplayName\":\"{1}\",\"Logo\":\"{2}\",\"ShortDescription\":\"{3}\",\"Description\":\"{4}\"", new object[]
					{
						pluginItem.FullName,
						pluginItem.DisplayName,
						pluginItem.Logo,
						pluginItem.ShortDescription,
						pluginItem.Description
					});
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]");
			}
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		public virtual string ToXmlString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<xml>");
			stringBuilder.AppendFormat("<qty>{0}</qty>", this.plugins.Count.ToString(CultureInfo.InvariantCulture));
			foreach (string current in this.plugins.Keys)
			{
				PluginItem pluginItem = this.plugins[current];
				stringBuilder.Append("<item>");
				stringBuilder.AppendFormat("<FullName>{0}</FullName><DisplayName>{1}</DisplayName><Logo>{2}</Logo><ShortDescription>{3}</ShortDescription><Description>{4}</Description>", new object[]
				{
					pluginItem.FullName,
					pluginItem.DisplayName,
					pluginItem.Logo,
					pluginItem.ShortDescription,
					pluginItem.Description
				});
				stringBuilder.Append("</item>");
			}
			stringBuilder.Append("</xml>");
			return stringBuilder.ToString();
		}
	}
}
