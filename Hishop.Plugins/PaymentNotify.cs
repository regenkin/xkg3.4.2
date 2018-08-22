using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Hishop.Plugins
{
	public abstract class PaymentNotify : IPlugin
	{
		public event EventHandler NotifyVerifyFaild;

		public event EventHandler Payment;

		public event EventHandler<FinishedEventArgs> Finished;

		public string ReturnUrl
		{
			get;
			set;
		}

		public static PaymentNotify CreateInstance(string name, NameValueCollection parameters)
		{
			PaymentNotify result;
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
				PaymentPlugins paymentPlugins = PaymentPlugins.Instance();
				Type plugin = paymentPlugins.GetPlugin("PaymentRequest", name);
				if (plugin == null)
				{
					result = null;
				}
				else
				{
					Type pluginWithNamespace = paymentPlugins.GetPluginWithNamespace("PaymentNotify", plugin.Namespace);
					if (pluginWithNamespace == null)
					{
						result = null;
					}
					else
					{
						result = (Activator.CreateInstance(pluginWithNamespace, args) as PaymentNotify);
					}
				}
			}
			return result;
		}

		protected virtual void OnFinished(bool isMedTrade)
		{
			if (this.Finished != null)
			{
				this.Finished(this, new FinishedEventArgs(isMedTrade));
			}
		}

		protected virtual void OnNotifyVerifyFaild()
		{
			if (this.NotifyVerifyFaild != null)
			{
				this.NotifyVerifyFaild(this, null);
			}
		}

		protected virtual void OnPayment()
		{
			if (this.Payment != null)
			{
				this.Payment(this, null);
			}
		}

		public abstract void VerifyNotify(int timeout, string configXml);

		public abstract string GetOrderId();

		public abstract string GetGatewayOrderId();

		public abstract decimal GetOrderAmount();

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

		public abstract void WriteBack(HttpContext context, bool success);

		public virtual string GetRemark1()
		{
			return string.Empty;
		}

		public virtual string GetRemark2()
		{
			return string.Empty;
		}
	}
}
