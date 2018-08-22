using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ManageNineImages : AdminPage
	{
		private string ShareDesc = "";

		protected System.Web.UI.WebControls.Literal NineTotal;

		protected System.Web.UI.WebControls.Repeater ShareRep;

		protected Pager pager;

		protected ManageNineImages() : base("m01", "dpp10")
		{
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["key"]))
			{
				this.ShareDesc = base.Server.UrlDecode(this.Page.Request.QueryString["key"]);
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			DbQueryResult nineImgsesList = ShareMaterialBrowser.GetNineImgsesList(new NineImgsesQuery
			{
				key = this.ShareDesc,
				SortBy = "id",
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortOrder = SortAction.Desc
			});
			this.ShareRep.DataSource = nineImgsesList.Data;
			this.ShareRep.DataBind();
			this.pager.TotalRecords = nineImgsesList.TotalRecords;
			this.NineTotal.Text = this.pager.TotalRecords.ToString();
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			this.ShowMsg("添加成功了", false);
		}
	}
}
