using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_Footer : SimpleTemplatedWebControl
	{
		private int isShowNav;

		public int IsShowNav
		{
			get
			{
				return this.isShowNav;
			}
			set
			{
				this.isShowNav = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "tags/skin-Common_Footer.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = string.Empty;
			string userAgent = this.Page.Request.UserAgent;
			if (userAgent.ToLower().Contains("micromessenger") || Globals.RequestQueryNum("istest") == 1)
			{
				text = "<script>WinXinShareMessage(wxinshare_title, wxinshare_desc, wxinshare_link, wxinshare_imgurl);</script>";
			}
			if (this.isShowNav == 1)
			{
				text += "<div style='padding-top:60px'></div><script>$(function () { jQuery.getJSON('/api/Hi_Ajax_NavMenu.ashx', function (settingjson) {   $(_.template($('#menu').html())(settingjson)).appendTo('body'); GetUICss();});  }) </script>";
			}
			Literal literal = (Literal)this.FindControl("litJs");
			literal.Text = text;
		}
	}
}
