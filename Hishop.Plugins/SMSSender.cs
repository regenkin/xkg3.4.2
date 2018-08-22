using System;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class SMSSender : ConfigablePlugin, IPlugin
	{
		public static SMSSender CreateInstance(string name, string configXml)
		{
			SMSSender result;
			if (string.IsNullOrEmpty(name))
			{
				result = null;
			}
			else
			{
				Type plugin = SMSPlugins.Instance().GetPlugin("SMSSender", name);
				if (plugin == null)
				{
					result = null;
				}
				else
				{
					SMSSender sMSSender = Activator.CreateInstance(plugin) as SMSSender;
					if (sMSSender != null && !string.IsNullOrEmpty(configXml))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.LoadXml(configXml);
						sMSSender.InitConfig(xmlDocument.FirstChild);
					}
					result = sMSSender;
				}
			}
			return result;
		}

		public static SMSSender CreateInstance(string name)
		{
			return SMSSender.CreateInstance(name, null);
		}

		public abstract bool Send(string cellPhone, string message, out string returnMsg, string speed = "0");

		public abstract bool Send(string cellPhone, string message, out string returnMsg);

		public abstract bool Send(string[] phoneNumbers, string message, out string returnMsg, string speed = "1");

		public abstract bool Send(string[] phoneNumbers, string message, out string returnMsg);
	}
}
