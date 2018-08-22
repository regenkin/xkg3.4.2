using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VUserLogining : VMemberTemplatedWebControl
	{
		private System.Web.UI.HtmlControls.HtmlInputHidden hidurl;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-VUserLogining.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.hidurl = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hidurl");
			string text = System.Web.HttpContext.Current.Request.QueryString.ToString();
			text = Globals.UrlDecode(text);
			text = System.Text.RegularExpressions.Regex.Match(text, "(returnUrl=.*)", System.Text.RegularExpressions.RegexOptions.IgnoreCase).ToString();
			text = System.Text.RegularExpressions.Regex.Replace(text, "(returnUrl=)", "", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			this.hidurl.Value = text;
		}
	}
}
