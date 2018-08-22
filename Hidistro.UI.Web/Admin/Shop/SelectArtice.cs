using ASPNET.WebControls;
using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Weibo;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class SelectArtice : AdminPage
	{
		protected string htmlMenuTitleAdd = string.Empty;

		protected string ArticleTitle = string.Empty;

		private int pageno;

		protected int recordcount;

		protected int articletype;

		private string title = string.Empty;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtSearchText;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected SelectArtice() : base("m01", "00000")
		{
		}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.articletype = Globals.RequestQueryNum("type");
            switch (this.articletype)
            {
                case 2:
                    this.htmlMenuTitleAdd = "单";
                    break;
                case 4:
                    this.htmlMenuTitleAdd = "多";
                    break;
            }
            this.articletype = 0;
            OnPageLoad();
        }

        private void OnPageLoad()
        {
            if (!base.IsPostBack)
            {
                this.pageno = Globals.RequestQueryNum("pageindex");
                if (this.pageno < 1)
                {
                    this.pageno = 1;
                }
                string text = Globals.RequestQueryStr("key");
                if (!string.IsNullOrEmpty(text))
                {
                    this.ArticleTitle = text;
                    this.txtSearchText.Text = text;
                }
                this.articletype = Globals.RequestQueryNum("type");
                this.pageno = Globals.RequestQueryNum("pageindex");
                if (this.pageno < 1)
                {
                    this.pageno = 1;
                }
                switch (this.articletype)
                {
                    case 2:
                    case 4:
                        this.BindData(this.articletype, this.pageno, this.ArticleTitle);
                        break;
                    case 3:
                        this.BindData(this.articletype, this.pageno, this.ArticleTitle);
                        break;
                }
                this.articletype = 0;
            }
        }

        private void BindData(int articletype, int pageno, string title)
		{
			ArticleQuery articleQuery = new ArticleQuery();
			articleQuery.Title = title;
			articleQuery.ArticleType = articletype;
			articleQuery.SortBy = "PubTime";
			articleQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(articleQuery, true);
			articleQuery.PageIndex = pageno;
			articleQuery.PageSize = this.pager.PageSize;
			DbQueryResult articleRequest = ArticleHelper.GetArticleRequest(articleQuery);
			this.rptList.DataSource = articleRequest.Data;
			this.rptList.DataBind();
			int totalRecords = articleRequest.TotalRecords;
			this.pager.TotalRecords = totalRecords;
			this.recordcount = totalRecords;
			if (this.pager.TotalRecords <= this.pager.PageSize)
			{
				this.pager.Visible = false;
			}
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string text = "SelectArtice.aspx";
			string text2 = string.Empty;
			if (this.articletype > 0)
			{
				text2 = "&type=" + this.articletype;
			}
			string text3 = this.txtSearchText.Text.Trim();
			if (!string.IsNullOrEmpty(text3))
			{
				text2 = "&key=" + base.Server.UrlEncode(text3);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "?" + text2.Trim(new char[]
				{
					'&'
				});
			}
			base.Response.Redirect(text);
			base.Response.End();
		}
	}
}
