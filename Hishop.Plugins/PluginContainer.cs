using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class PluginContainer
	{
		protected static volatile Cache pluginCache = HttpRuntime.Cache;

		protected abstract string PluginLocalPath
		{
			get;
		}

		protected abstract string PluginVirtualPath
		{
			get;
		}

		protected abstract string IndexCacheKey
		{
			get;
		}

		protected abstract string TypeCacheKey
		{
			get;
		}

		protected PluginContainer()
		{
			PluginContainer.pluginCache.Remove(this.IndexCacheKey);
			PluginContainer.pluginCache.Remove(this.TypeCacheKey);
		}

		protected void VerifyIndex()
		{
			if (PluginContainer.pluginCache.Get(this.IndexCacheKey) == null)
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode xmlNode = xmlDocument.CreateElement("Plugins");
				this.BuildIndex(xmlDocument, xmlNode);
				xmlDocument.AppendChild(xmlNode);
				PluginContainer.pluginCache.Insert(this.IndexCacheKey, xmlDocument, new CacheDependency(this.PluginLocalPath));
			}
		}

		private void BuildIndex(XmlDocument catalog, XmlNode mapNode)
		{
			if (Directory.Exists(this.PluginLocalPath))
			{
				string[] files = Directory.GetFiles(this.PluginLocalPath, "*.dll", SearchOption.AllDirectories);
				string fullName = typeof(IPlugin).FullName;
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string filename = array[i];
					Assembly assembly = Assembly.Load(PluginContainer.LoadPlugin(filename));
					Type[] exportedTypes = assembly.GetExportedTypes();
					for (int j = 0; j < exportedTypes.Length; j++)
					{
						Type t = exportedTypes[j];
						if (PluginContainer.CheckIsPlugin(t, fullName))
						{
							this.AddPlugin(t, filename, catalog, mapNode);
						}
					}
				}
			}
		}

		private Type GetPlugin(string baseName, string name, string attname)
		{
			Hashtable hashtable = this.GetPluginCache();
			name = name.ToLower();
			Type type = hashtable[name] as Type;
			Type result;
			if (type == null)
			{
				if (PluginContainer.pluginCache.Get(this.IndexCacheKey) == null)
				{
					result = null;
					return result;
				}
				XmlDocument xmlDocument = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
				XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode(string.Concat(new string[]
				{
					"//",
					baseName,
					"/item[@",
					attname,
					"='",
					name,
					"']"
				}));
				if (xmlNode == null || !File.Exists(xmlNode.Attributes["file"].Value))
				{
					result = null;
					return result;
				}
				Assembly assembly = Assembly.Load(PluginContainer.LoadPlugin(xmlNode.Attributes["file"].Value));
				type = assembly.GetType(xmlNode.Attributes["identity"].Value, false, true);
				if (type != null)
				{
					hashtable[name] = type;
				}
			}
			result = type;
			return result;
		}

		internal virtual Type GetPlugin(string baseName, string name)
		{
			return this.GetPlugin(baseName, name, "identity");
		}

		internal virtual Type GetPluginWithNamespace(string baseName, string name)
		{
			return this.GetPlugin(baseName, name, "namespace");
		}

		private Hashtable GetPluginCache()
		{
			Hashtable hashtable = PluginContainer.pluginCache.Get(this.TypeCacheKey) as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				PluginContainer.pluginCache.Insert(this.TypeCacheKey, hashtable, new CacheDependency(this.PluginLocalPath));
			}
			return hashtable;
		}

		private void AddPlugin(Type t, string filename, XmlDocument catalog, XmlNode mapNode)
		{
			XmlNode xmlNode = mapNode.SelectSingleNode(t.BaseType.Name);
			if (xmlNode == null)
			{
				xmlNode = catalog.CreateElement(t.BaseType.Name);
				mapNode.AppendChild(xmlNode);
			}
			XmlNode xmlNode2 = catalog.CreateElement("item");
			XmlAttribute xmlAttribute = catalog.CreateAttribute("identity");
			xmlAttribute.Value = t.FullName.ToLower();
			xmlNode2.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = catalog.CreateAttribute("file");
			xmlAttribute2.Value = filename;
			xmlNode2.Attributes.Append(xmlAttribute2);
			PluginAttribute pluginAttribute = (PluginAttribute)Attribute.GetCustomAttribute(t, typeof(PluginAttribute));
			if (pluginAttribute != null)
			{
				XmlAttribute xmlAttribute3 = catalog.CreateAttribute("name");
				xmlAttribute3.Value = pluginAttribute.Name;
				xmlNode2.Attributes.Append(xmlAttribute3);
				XmlAttribute xmlAttribute4 = catalog.CreateAttribute("seq");
				xmlAttribute4.Value = ((pluginAttribute.Sequence > 0) ? pluginAttribute.Sequence.ToString(CultureInfo.InvariantCulture) : "0");
				xmlNode2.Attributes.Append(xmlAttribute4);
				ConfigablePlugin configablePlugin = Activator.CreateInstance(t) as ConfigablePlugin;
				XmlAttribute xmlAttribute5 = catalog.CreateAttribute("logo");
				if (string.IsNullOrEmpty(configablePlugin.Logo) || configablePlugin.Logo.Trim().Length == 0)
				{
					xmlAttribute5.Value = "";
				}
				else
				{
					xmlAttribute5.Value = this.PluginVirtualPath + "/images/" + configablePlugin.Logo.Trim();
				}
				xmlNode2.Attributes.Append(xmlAttribute5);
				XmlAttribute xmlAttribute6 = catalog.CreateAttribute("shortDescription");
				xmlAttribute6.Value = configablePlugin.ShortDescription;
				xmlNode2.Attributes.Append(xmlAttribute6);
				XmlAttribute xmlAttribute7 = catalog.CreateAttribute("description");
				xmlAttribute7.Value = configablePlugin.Description;
				xmlNode2.Attributes.Append(xmlAttribute7);
			}
			XmlAttribute xmlAttribute8 = catalog.CreateAttribute("namespace");
			xmlAttribute8.Value = t.Namespace.ToLower();
			xmlNode2.Attributes.Append(xmlAttribute8);
			if (pluginAttribute != null && pluginAttribute.Sequence > 0)
			{
				XmlNode xmlNode3 = PluginContainer.FindNode(xmlNode.ChildNodes, pluginAttribute.Sequence);
				if (xmlNode3 == null)
				{
					xmlNode.AppendChild(xmlNode2);
				}
				else
				{
					xmlNode.InsertBefore(xmlNode2, xmlNode3);
				}
			}
			else
			{
				xmlNode.AppendChild(xmlNode2);
			}
		}

		private static XmlNode FindNode(XmlNodeList nodeList, int sequence)
		{
			XmlNode result;
			if (nodeList == null || nodeList.Count == 0 || sequence <= 0)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < nodeList.Count; i++)
				{
					if (int.Parse(nodeList[i].Attributes["seq"].Value) > sequence)
					{
						result = nodeList[i];
						return result;
					}
				}
				result = null;
			}
			return result;
		}

		private static byte[] LoadPlugin(string filename)
		{
			byte[] array;
			using (FileStream fileStream = new FileStream(filename, FileMode.Open))
			{
				array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			return array;
		}

		private static bool CheckIsPlugin(Type t, string interfaceName)
		{
			bool result;
			try
			{
				if (t == null || !t.IsClass || !t.IsPublic || t.IsAbstract || t.GetInterface(interfaceName) == null)
				{
					result = false;
				}
				else
				{
					result = true;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public abstract PluginItemCollection GetPlugins();

		public abstract PluginItem GetPluginItem(string fullName);

		protected PluginItem GetPluginItem(string baseName, string fullName)
		{
			PluginItem result = null;
			XmlDocument xmlDocument = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
			XmlNode xmlNode = xmlDocument.SelectSingleNode(string.Concat(new string[]
			{
				"//",
				baseName,
				"/item[@identity='",
				fullName,
				"']"
			}));
			if (xmlNode != null)
			{
				result = new PluginItem
				{
					FullName = xmlNode.Attributes["identity"].Value,
					DisplayName = xmlNode.Attributes["name"].Value,
					Logo = xmlNode.Attributes["logo"].Value,
					ShortDescription = xmlNode.Attributes["shortDescription"].Value,
					Description = xmlNode.Attributes["description"].Value
				};
			}
			return result;
		}

		protected PluginItemCollection GetPlugins(string baseName)
		{
			PluginItemCollection pluginItemCollection = new PluginItemCollection();
			XmlDocument xmlDocument = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//" + baseName + "/item");
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					PluginItem item = new PluginItem
					{
						FullName = xmlNode.Attributes["identity"].Value,
						DisplayName = xmlNode.Attributes["name"].Value,
						Logo = xmlNode.Attributes["logo"].Value,
						ShortDescription = xmlNode.Attributes["shortDescription"].Value,
						Description = xmlNode.Attributes["description"].Value
					};
					pluginItemCollection.Add(item);
				}
			}
			return pluginItemCollection;
		}
	}
}
