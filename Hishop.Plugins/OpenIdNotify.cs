using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;

namespace Hishop.Plugins
{
	public abstract class OpenIdNotify : IPlugin
	{
		public event EventHandler<AuthenticatedEventArgs> Authenticated;

		public event EventHandler<FailedEventArgs> Failed;

		public static OpenIdNotify CreateInstance(string name, NameValueCollection parameters)
		{
			OpenIdNotify result;
			if (string.IsNullOrEmpty(name))
			{
				result = null;
			}
			else
			{
				object[] args = new object[]
				{
					parameters
				};
				OpenIdPlugins openIdPlugins = OpenIdPlugins.Instance();
				Type plugin = openIdPlugins.GetPlugin("OpenIdService", name);
				if (plugin == null)
				{
					result = null;
				}
				else
				{
					Type pluginWithNamespace = openIdPlugins.GetPluginWithNamespace("OpenIdNotify", plugin.Namespace);
					if (pluginWithNamespace == null)
					{
						result = null;
					}
					else
					{
						result = (Activator.CreateInstance(pluginWithNamespace, args) as OpenIdNotify);
					}
				}
			}
			return result;
		}

		public abstract void Verify(int timeout, string configXml);

		protected virtual void OnAuthenticated(string openId)
		{
			if (this.Authenticated != null)
			{
				this.Authenticated(this, new AuthenticatedEventArgs(openId));
			}
		}

		protected virtual void OnFailed(string message)
		{
			if (this.Failed != null)
			{
				this.Failed(this, new FailedEventArgs(message));
			}
		}

		protected virtual string GetResponse(string url, int timeout)
		{
			string result;
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Timeout = timeout;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (Stream responseStream = httpWebResponse.GetResponseStream())
				{
					using (StreamReader streamReader = new StreamReader(responseStream, Encoding.Default))
					{
						StringBuilder stringBuilder = new StringBuilder();
						while (-1 != streamReader.Peek())
						{
							stringBuilder.Append(streamReader.ReadLine());
						}
						result = stringBuilder.ToString();
					}
				}
			}
			catch (Exception ex)
			{
				result = "Error:" + ex.Message;
			}
			return result;
		}
	}
}
