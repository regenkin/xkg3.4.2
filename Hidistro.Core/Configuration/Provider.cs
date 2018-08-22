using System;
using System.Collections.Specialized;
using System.Xml;

namespace Hidistro.Core.Configuration
{
	public class Provider
	{
		private string name;

		private string providerType;

		private NameValueCollection providerAttributes = new NameValueCollection();

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string Type
		{
			get
			{
				return this.providerType;
			}
		}

		public NameValueCollection Attributes
		{
			get
			{
				return this.providerAttributes;
			}
		}

		public Provider(XmlAttributeCollection attributes)
		{
			if (attributes != null)
			{
				this.name = attributes["name"].Value;
				this.providerType = attributes["type"].Value;
				foreach (XmlAttribute xmlAttribute in attributes)
				{
					if (xmlAttribute.Name != "name" && xmlAttribute.Name != "type")
					{
						this.providerAttributes.Add(xmlAttribute.Name, xmlAttribute.Value);
					}
				}
			}
		}
	}
}
