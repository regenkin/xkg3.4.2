using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class Pager : WebControl
	{
		private string urlFormat;

		private int defaultPageSize = 10;

		private string aname = string.Empty;

		[Browsable(false)]
		public int PageIndex
		{
			get
			{
				int num = 1;
				if (!string.IsNullOrEmpty(this.Context.Request.QueryString[this.PageIndexFormat]))
				{
					int.TryParse(this.Context.Request.QueryString[this.PageIndexFormat], out num);
				}
				if (num <= 0)
				{
					return 1;
				}
				int num2 = this.CalculateTotalPages();
				if (num2 > 0 && num > num2)
				{
					return num2;
				}
				return num;
			}
		}

		public string PageIndexFormat
		{
			get;
			set;
		}

		[Browsable(false)]
		public int PageSize
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

		[Browsable(false)]
		public int TotalRecords
		{
			get
			{
				if (this.ViewState["TotalRecords"] == null)
				{
					return 0;
				}
				return (int)this.ViewState["TotalRecords"];
			}
			set
			{
				this.ViewState["TotalRecords"] = value;
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

		public bool ShowTotalPages
		{
			get;
			set;
		}

		public string BreakCssClass
		{
			get;
			set;
		}

		public string PrevCssClass
		{
			get;
			set;
		}

		public string CurCssClass
		{
			get;
			set;
		}

		public string NextCssClass
		{
			get;
			set;
		}

		public string SkipPanelCssClass
		{
			get;
			set;
		}

		public string SkipTxtCssClass
		{
			get;
			set;
		}

		public string SkipBtnCssClass
		{
			get;
			set;
		}

		public string Aname
		{
			get
			{
				if (!string.IsNullOrEmpty(this.aname) && !this.aname.StartsWith("#"))
				{
					this.aname = "#" + this.aname;
				}
				return this.aname;
			}
			set
			{
				this.aname = value;
			}
		}

		private bool HasPrevious
		{
			get
			{
				return this.PageIndex > 1;
			}
		}

		private bool HasNext
		{
			get
			{
				return this.PageIndex < this.CalculateTotalPages();
			}
		}

		public Pager()
		{
			this.PageIndexFormat = "pageindex";
			this.BreakCssClass = "page-break";
			this.PrevCssClass = "page-prev";
			this.CurCssClass = "page-cur";
			this.NextCssClass = "page-next";
			this.SkipPanelCssClass = "page-skip";
			this.SkipTxtCssClass = "text";
			this.SkipBtnCssClass = "button";
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
			this.urlFormat = this.Context.Request.RawUrl;
			if (this.urlFormat.IndexOf("?") >= 0)
			{
				string text = this.urlFormat.Substring(this.urlFormat.IndexOf("?") + 1);
				string[] array = text.Split(new char[]
				{
					Convert.ToChar("&")
				});
				this.urlFormat = this.urlFormat.Replace(text, "");
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					if (!text2.ToLower().StartsWith(this.PageIndexFormat.ToLower() + "="))
					{
						this.urlFormat = this.urlFormat + text2 + "&";
					}
				}
				this.urlFormat = this.urlFormat + this.PageIndexFormat + "=";
				return;
			}
			this.urlFormat = this.urlFormat + "?" + this.PageIndexFormat + "=";
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.TotalRecords <= 0)
			{
				return;
			}
			int num = this.CalculateTotalPages();
			this.RenderPrevious(writer);
			this.RenderPagingButtons(writer, num);
			if (num > 6 && this.PageIndex + 2 < num)
			{
				this.RenderMore(writer);
			}
			this.RenderNext(writer);
			if (this.ShowTotalPages)
			{
				this.RenderGotoPage(writer, num);
			}
		}

		private void RenderMore(HtmlTextWriter writer)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Span);
			webControl.Attributes.Add("class", this.BreakCssClass);
			webControl.Controls.Add(new LiteralControl("..."));
			webControl.RenderControl(writer);
		}

		private void RenderPrevious(HtmlTextWriter writer)
		{
			if (this.HasPrevious)
			{
				WebControl webControl = new WebControl(HtmlTextWriterTag.A);
				webControl.Controls.Add(new LiteralControl("上一页"));
				webControl.Attributes.Add("class", this.PrevCssClass);
				webControl.Attributes.Add("href", this.urlFormat + (this.PageIndex - 1).ToString(CultureInfo.InvariantCulture) + this.Aname);
				webControl.RenderControl(writer);
			}
		}

		private void RenderPagingButtons(HtmlTextWriter writer, int totalPages)
		{
			if (totalPages <= 6)
			{
				this.RenderButtonRange(writer, 1, totalPages);
				return;
			}
			int num = this.PageIndex - 3;
			int num2 = this.PageIndex + 2;
			if (num <= 0)
			{
				num2 -= num - 1;
				num = 1;
			}
			if (num2 > totalPages)
			{
				num2 = totalPages;
			}
			this.RenderButtonRange(writer, num, num2);
		}

		private void RenderButtonRange(HtmlTextWriter writer, int startIndex, int endIndex)
		{
			for (int i = startIndex; i <= endIndex; i++)
			{
				this.RenderButton(writer, i);
			}
		}

		private void RenderButton(HtmlTextWriter writer, int buttonIndex)
		{
			if (buttonIndex == this.PageIndex)
			{
				new LiteralControl(string.Concat(new string[]
				{
					"<span class=\"",
					this.CurCssClass,
					"\">",
					buttonIndex.ToString(CultureInfo.InvariantCulture),
					"</span>"
				})).RenderControl(writer);
				return;
			}
			WebControl webControl = new WebControl(HtmlTextWriterTag.A);
			webControl.Controls.Add(new LiteralControl(buttonIndex.ToString(CultureInfo.InvariantCulture)));
			webControl.Attributes.Add("href", this.urlFormat + buttonIndex.ToString(CultureInfo.InvariantCulture) + this.Aname);
			webControl.RenderControl(writer);
		}

		private void RenderNext(HtmlTextWriter writer)
		{
			if (this.HasNext)
			{
				WebControl webControl = new WebControl(HtmlTextWriterTag.A);
				webControl.Controls.Add(new LiteralControl("下一页"));
				webControl.Attributes.Add("class", this.NextCssClass);
				webControl.Attributes.Add("href", this.urlFormat + (this.PageIndex + 1).ToString(CultureInfo.InvariantCulture) + this.Aname);
				webControl.RenderControl(writer);
			}
		}

		private void RenderGotoPage(HtmlTextWriter writer, int totalPages)
		{
			WebControl webControl = new WebControl(HtmlTextWriterTag.Span);
			webControl.Attributes.Add("class", this.SkipPanelCssClass);
			webControl.Controls.Add(new LiteralControl(string.Format("第{0}/{1}页 共{2}记录", this.PageIndex, totalPages.ToString(CultureInfo.InvariantCulture), this.TotalRecords.ToString(CultureInfo.InvariantCulture))));
			WebControl webControl2 = new WebControl(HtmlTextWriterTag.Input);
			webControl2.Attributes.Add("type", "text");
			webControl2.Attributes.Add("class", this.SkipTxtCssClass);
			webControl2.Attributes.Add("value", this.PageIndex.ToString(CultureInfo.InvariantCulture));
			webControl2.Attributes.Add("size", "3");
			webControl2.Attributes.Add("id", "txtGoto");
			webControl.Controls.Add(webControl2);
			webControl.Controls.Add(new LiteralControl("页"));
			WebControl webControl3 = new WebControl(HtmlTextWriterTag.Input);
			webControl3.Attributes.Add("type", "button");
			webControl3.Attributes.Add("class", this.SkipBtnCssClass);
			webControl3.Attributes.Add("value", "确定");
			webControl3.Attributes.Add("onclick", string.Format("location.href=AppendParameter('{0}',  $.trim($('#txtGoto').val()));", this.PageIndexFormat));
			webControl.Controls.Add(webControl3);
			webControl.RenderControl(writer);
		}

		private int CalculateTotalPages()
		{
			if (this.TotalRecords == 0)
			{
				return 0;
			}
			int num = this.TotalRecords / this.PageSize;
			if (this.TotalRecords % this.PageSize > 0)
			{
				num++;
			}
			return num;
		}
	}
}
