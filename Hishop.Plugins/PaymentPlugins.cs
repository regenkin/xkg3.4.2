using System;
using System.Web;

namespace Hishop.Plugins
{
	public class PaymentPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();

		private static volatile PaymentPlugins instance = null;

		protected override string PluginLocalPath
		{
			get
			{
				return HttpContext.Current.Request.MapPath("~/plugins/payment");
			}
		}

		protected override string PluginVirtualPath
		{
			get
			{
				return Utils.ApplicationPath + "/plugins/payment";
			}
		}

		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-payment-index";
			}
		}

		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-payment-type";
			}
		}

		private PaymentPlugins()
		{
		}

		public static PaymentPlugins Instance()
		{
			if (PaymentPlugins.instance == null)
			{
				lock (PaymentPlugins.LockHelper)
				{
					if (PaymentPlugins.instance == null)
					{
						PaymentPlugins.instance = new PaymentPlugins();
					}
				}
			}
			PaymentPlugins.instance.VerifyIndex();
			return PaymentPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("PaymentRequest");
		}

		public override PluginItem GetPluginItem(string fullName)
		{
			return base.GetPluginItem("PaymentRequest", fullName);
		}
	}
}
