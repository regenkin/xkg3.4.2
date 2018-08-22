using System;
using System.Web;

namespace Hishop.Plugins
{
	public class OpenIdPlugins : PluginContainer
	{
		private static readonly object LockHelper = new object();

		private static volatile OpenIdPlugins instance = null;

		protected override string PluginLocalPath
		{
			get
			{
				return HttpContext.Current.Request.MapPath("~/plugins/openid");
			}
		}

		protected override string PluginVirtualPath
		{
			get
			{
				return Utils.ApplicationPath + "/plugins/openid";
			}
		}

		protected override string IndexCacheKey
		{
			get
			{
				return "plugin-openid-index";
			}
		}

		protected override string TypeCacheKey
		{
			get
			{
				return "plugin-openid-type";
			}
		}

		private OpenIdPlugins()
		{
		}

		public static OpenIdPlugins Instance()
		{
			if (OpenIdPlugins.instance == null)
			{
				lock (OpenIdPlugins.LockHelper)
				{
					if (OpenIdPlugins.instance == null)
					{
						OpenIdPlugins.instance = new OpenIdPlugins();
					}
				}
			}
			OpenIdPlugins.instance.VerifyIndex();
			return OpenIdPlugins.instance;
		}

		public override PluginItemCollection GetPlugins()
		{
			return base.GetPlugins("OpenIdService");
		}

		public override PluginItem GetPluginItem(string fullName)
		{
			return base.GetPluginItem("OpenIdService", fullName);
		}
	}
}
