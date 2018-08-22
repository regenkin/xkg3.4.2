using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class PageSize : WebControl
	{
		private string urlFormat;

		private int defaultPageSize = 10;

		[Browsable(false)]
		public int SelectedSize
		{
			get
			{
				int num = this.defaultPageSize;
				if (!string.IsNullOrEmpty(this.Context.Request.QueryString["pagesize"]))
				{
					int.TryParse(this.Context.Request.QueryString["pagesize"], out num);
				}
				if (num <= 0)
				{
					return this.defaultPageSize;
				}
				return num;
			}
		}

		public int DefaultPageSize
		{
			get
			{
				return this.defaultPageSize;
			}
			set
			{
				this.defaultPageSize = value;
			}
		}

		public string SelectedSizeCss
		{
			get;
			set;
		}

		public PageSize()
		{
			this.SelectedSizeCss = "selectthis";
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.urlFormat = this.Context.Request.RawUrl;
			if (this.Context.Request.QueryString.Count > 0)
			{
				this.urlFormat = this.urlFormat.Replace(this.Context.Request.Url.Query, "?");
				foreach (string text in this.Context.Request.QueryString.Keys)
				{
					if (string.Compare(text, "pagesize", true) != 0 && string.Compare(text, "pageindex", true) != 0)
					{
						string text2 = this.urlFormat;
						this.urlFormat = string.Concat(new string[]
						{
							text2,
							text,
							"=",
							this.Page.Server.UrlEncode(this.Context.Request.QueryString[text]),
							"&"
						});
					}
				}
			}
			this.urlFormat += (this.urlFormat.Contains("?") ? "pagesize=" : "?pagesize=");
		}

		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderButton(writer);
		}

		private void RenderButton(HtmlTextWriter writer)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.A);
			webControl.Controls.Add(new LiteralControl("10"));
			webControl.Attributes.Add("href", this.urlFormat + "10");
			if (this.SelectedSize == 10)
			{
				webControl.Attributes.Add("class", this.SelectedSizeCss);
			}
			webControl.RenderControl(writer);
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.A);
			webControl2.Controls.Add(new LiteralControl("20"));
			webControl2.Attributes.Add("href", this.urlFormat + "20");
			if (this.SelectedSize == 20)
			{
				webControl2.Attributes.Add("class", this.SelectedSizeCss);
			}
			webControl2.RenderControl(writer);
			WebControl webControl3 = new WebControl(HtmlTextWriterTag.A);
			webControl3.Controls.Add(new LiteralControl("40"));
			webControl3.Attributes.Add("href", this.urlFormat + "40");
			if (this.SelectedSize == 40)
			{
				webControl3.Attributes.Add("class", this.SelectedSizeCss);
			}
			webControl3.RenderControl(writer);
			WebControl webControl4 = new WebControl(HtmlTextWriterTag.A);
			webControl4.Controls.Add(new LiteralControl("100"));
			webControl4.Attributes.Add("href", this.urlFormat + "100");
			if (this.SelectedSize == 100)
			{
				webControl4.Attributes.Add("class", this.SelectedSizeCss);
			}
			webControl4.RenderControl(writer);
		}
	}
}
