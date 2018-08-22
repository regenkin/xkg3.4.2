using ASPNET.WebControls;
using Hidistro.ControlPanel.Bargain;
using Hidistro.Core.Entities;
using Hidistro.Entities.Bargain;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Bargain
{
	public class ManagerBargain : AdminPage
	{
		private string productName;

		private string title;

		private string type;

		protected Script Script5;

		protected Script Script6;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Literal ListActive;

		protected System.Web.UI.WebControls.Literal Listfrozen;

		protected System.Web.UI.WebControls.Literal Literal1;

		protected System.Web.UI.WebControls.Literal Literal2;

		protected System.Web.UI.WebControls.TextBox txtTitle;

		protected System.Web.UI.WebControls.TextBox txtProductName;

		protected System.Web.UI.WebControls.Button btnSearchButton;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.Button btnDeleteCheck;

		protected System.Web.UI.WebControls.Repeater grdBargainList;

		protected Pager pager;

		protected ucDateTimePicker calendarBeginDate;

		protected ucDateTimePicker calendarEndDate;

		public ManagerBargain() : base("m08", "yxp21")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.ViewState["Type"] = ((base.Request.QueryString["Type"] != null) ? base.Request.QueryString["Type"] : null);
				this.type = ((this.ViewState["Type"] == null) ? "all" : this.ViewState["Type"].ToString());
				this.BindData();
			}
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.grdBargainList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdBargainList_ItemCommand);
			this.btnDeleteCheck.Click += new System.EventHandler(this.btnDeleteCheck_Click);
		}

		private void grdBargainList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			string commandName = e.CommandName;
			string text = e.CommandArgument.ToString();
			if (commandName == "Delete" && !string.IsNullOrEmpty(text))
			{
				bool flag = BargainHelper.DeleteBargainById(text);
				if (flag)
				{
					this.ShowMsg("删除成功", true);
					this.BindData();
				}
			}
		}

		private void btnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的砍价活动！", false);
				return;
			}
			text = text.TrimEnd(new char[]
			{
				','
			});
			System.Data.DataTable bargainById = BargainHelper.GetBargainById(text);
			if (bargainById.Rows.Count > 0)
			{
				this.ShowMsg("删除的砍价活动中有正在进行的，不能删除", false);
				return;
			}
			BargainHelper.DeleteBargainById(text);
			this.ShowMsg("删除成功", true);
			this.BindData();
		}

		protected bool GetStatus(object obj)
		{
			bool result = false;
			if (obj.ToString() == "进行中")
			{
				result = false;
			}
			if (obj.ToString() == "未开始")
			{
				result = true;
			}
			if (obj.ToString() == "已结束")
			{
				result = true;
			}
			return result;
		}

		protected string GetEditHtml(object id, object status, object endDate, object beginDate)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string a;
			if ((a = status.ToString()) != null)
			{
				if (!(a == "进行中"))
				{
					if (!(a == "未开始"))
					{
						if (a == "已结束")
						{
							stringBuilder.Append("<a href='#' class='btn btn-info btn-xs mb5 inputw50' style='display:none'>编辑</a>");
						}
					}
					else
					{
						stringBuilder.Append("<a href='/admin/Bargain/AddBargain.aspx?Id=" + id.ToString() + "' class='btn btn-info btn-xs mb5 inputw50' style='display: '>编辑</a>");
					}
				}
				else
				{
					stringBuilder.Append(string.Concat(new object[]
					{
						"<a href='#' onclick=\"OpenEdit('",
						id.ToString(),
						"','",
						(System.DateTime)endDate,
						"','",
						(System.DateTime)beginDate,
						"');\" class='btn btn-info btn-xs mb5 inputw50' style='display: '>编辑</a>"
					}));
				}
			}
			return stringBuilder.ToString();
		}

		private void BindData()
		{
			BargainQuery bargainQuery = new BargainQuery();
			bargainQuery.ProductName = this.productName;
			bargainQuery.Title = this.title;
			bargainQuery.Type = this.type;
			bargainQuery.PageSize = this.pager.PageSize;
			bargainQuery.PageIndex = this.pager.PageIndex;
			this.pager.TotalRecords = BargainHelper.GetTotal(bargainQuery);
			DbQueryResult bargainList = BargainHelper.GetBargainList(bargainQuery);
			this.grdBargainList.DataSource = bargainList.Data;
			this.grdBargainList.DataBind();
		}

		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
				{
					this.productName = base.Server.UrlDecode(this.Page.Request.QueryString["productName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["title"]))
				{
					this.title = base.Server.UrlDecode(this.Page.Request.QueryString["title"]);
				}
				this.txtProductName.Text = this.productName;
				this.txtTitle.Text = this.title;
				return;
			}
			this.productName = this.txtProductName.Text;
			this.title = this.txtTitle.Text;
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", this.txtProductName.Text);
			nameValueCollection.Add("title", this.txtTitle.Text);
			nameValueCollection.Add("type", (this.ViewState["Type"] != null) ? this.ViewState["Type"].ToString() : "");
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
