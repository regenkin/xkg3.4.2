using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class ConfigablePlugin
	{
		private const string RequiredMsg = "{0}为必填项";

		private const string CastMsg = "{0}的输入格式不正确，请按正确格式输入";

		protected abstract bool NeedProtect
		{
			get;
		}

		public abstract string Logo
		{
			get;
		}

		public abstract string ShortDescription
		{
			get;
		}

		public abstract string Description
		{
			get;
		}

		internal virtual XmlDocument GetMetaData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<xml></xml>");
			PropertyInfo[] properties = base.GetType().GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				ConfigElementAttribute configElementAttribute = (ConfigElementAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ConfigElementAttribute));
				if (configElementAttribute != null)
				{
					ConfigablePlugin.AppendAttrubiteNode(xmlDocument, configElementAttribute, propertyInfo);
				}
			}
			return xmlDocument;
		}

		public virtual ConfigData GetConfigData(NameValueCollection form)
		{
			ConfigData configData = new ConfigData();
			configData.NeedProtect = this.NeedProtect;
			PropertyInfo[] properties = base.GetType().GetProperties();
			PropertyInfo[] array = properties;
			int i = 0;
			while (i < array.Length)
			{
				PropertyInfo propertyInfo = array[i];
				string text = form[propertyInfo.Name] ?? "false";
				ConfigElementAttribute configElementAttribute = (ConfigElementAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ConfigElementAttribute));
				if (configElementAttribute != null)
				{
					if (!configElementAttribute.Nullable && (string.IsNullOrEmpty(text) || text.Length == 0))
					{
						configData.IsValid = false;
						configData.ErrorMsgs.Add(string.Format("{0}为必填项", configElementAttribute.Name));
					}
					else if (!string.IsNullOrEmpty(text))
					{
						try
						{
							Convert.ChangeType(text, propertyInfo.PropertyType);
						}
						catch (FormatException)
						{
							configData.IsValid = false;
							configData.ErrorMsgs.Add(string.Format("{0}的输入格式不正确，请按正确格式输入", configElementAttribute.Name));
							goto IL_11B;
						}
						configData.Add(propertyInfo.Name, text);
					}
				}
				IL_11B:
				i++;
				continue;
				goto IL_11B;
			}
			return configData;
		}

		protected virtual void InitConfig(XmlNode configXml)
		{
			PropertyInfo[] properties = base.GetType().GetProperties();
			PropertyInfo[] array = properties;
			for (int i = 0; i < array.Length; i++)
			{
				PropertyInfo propertyInfo = array[i];
				ConfigElementAttribute configElementAttribute = (ConfigElementAttribute)Attribute.GetCustomAttribute(propertyInfo, typeof(ConfigElementAttribute));
				if (configElementAttribute != null)
				{
					XmlNode xmlNode = configXml.SelectSingleNode(propertyInfo.Name);
					if (xmlNode != null && !string.IsNullOrEmpty(xmlNode.InnerText) && xmlNode.InnerText.Length > 0)
					{
						propertyInfo.SetValue(this, Convert.ChangeType(xmlNode.InnerText, propertyInfo.PropertyType), null);
					}
				}
			}
		}

		private static void AppendAttrubiteNode(XmlDocument doc, ConfigElementAttribute att, PropertyInfo property)
		{
			XmlNode xmlNode = doc.CreateElement("att");
			XmlAttribute xmlAttribute = doc.CreateAttribute("Property");
			xmlAttribute.Value = property.Name;
			xmlNode.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = doc.CreateAttribute("Name");
			xmlAttribute2.Value = (string.IsNullOrEmpty(att.Name) ? property.Name : att.Name);
			xmlNode.Attributes.Append(xmlAttribute2);
			XmlAttribute xmlAttribute3 = doc.CreateAttribute("Description");
			xmlAttribute3.Value = att.Description;
			xmlNode.Attributes.Append(xmlAttribute3);
			XmlAttribute xmlAttribute4 = doc.CreateAttribute("Nullable");
			xmlAttribute4.Value = att.Nullable.ToString();
			xmlNode.Attributes.Append(xmlAttribute4);
			XmlAttribute xmlAttribute5 = doc.CreateAttribute("InputType");
			xmlAttribute5.Value = ((int)att.InputType).ToString();
			xmlNode.Attributes.Append(xmlAttribute5);
			if (att.Options != null && att.Options.Length > 0)
			{
				XmlNode xmlNode2 = doc.CreateElement("Options");
				string[] options = att.Options;
				for (int i = 0; i < options.Length; i++)
				{
					string innerText = options[i];
					XmlNode xmlNode3 = doc.CreateElement("Item");
					xmlNode3.InnerText = innerText;
					xmlNode2.AppendChild(xmlNode3);
				}
				xmlNode.AppendChild(xmlNode2);
			}
			doc.SelectSingleNode("xml").AppendChild(xmlNode);
		}
	}
}
