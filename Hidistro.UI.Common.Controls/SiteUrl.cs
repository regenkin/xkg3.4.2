using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class SiteUrl : HyperLink
	{
		private string urlName;

		private string requstName;

		public string UrlName
		{
			get
			{
				return this.urlName;
			}
			set
			{
				this.urlName = value;
			}
		}

		public string RequstName
		{
			get
			{
				return this.requstName;
			}
			set
			{
				this.requstName = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (string.IsNullOrEmpty(base.NavigateUrl) && !string.IsNullOrEmpty(this.UrlName))
			{
				if (!string.IsNullOrEmpty(this.RequstName))
				{
					base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(this.UrlName, new object[]
					{
						this.Page.Request.QueryString[this.RequstName]
					});
				}
				else
				{
					base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl(this.UrlName);
				}
			}
			base.Render(writer);
		}
	}
}
