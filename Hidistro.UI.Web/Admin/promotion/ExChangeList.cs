using ASPNET.WebControls;
using ControlPanel.Promotions;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class ExChangeList : AdminPage
	{
		protected ExchangeStatus status;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Label lblAll;

		protected System.Web.UI.WebControls.Label lblIn;

		protected System.Web.UI.WebControls.Label lblEnd;

		protected System.Web.UI.WebControls.Label lblUnBegin;

		protected PageSize PageSize1;

		protected System.Web.UI.WebControls.Button btnDelete;

		protected System.Web.UI.WebControls.Button DelBtn;

		protected System.Web.UI.WebControls.TextBox txt_ids;

		protected System.Web.UI.WebControls.Repeater grdProducts;

		protected Pager pager;

		protected ExChangeList() : base("m08", "yxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSeach.Click += new System.EventHandler(this.btnSeach_Click);
			this.DelBtn.Click += new System.EventHandler(this.DelBtn_Click);
			this.grdProducts.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.grdProducts_ItemCommand);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("status"))
			{
				int num = 0;
				if (base.Request["status"].ToString().bInt(ref num))
				{
					this.status = (ExchangeStatus)num;
				}
			}
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			int totalRecords = 0;
			string text = this.txt_name.Text;
			System.Data.DataTable dataTable = PointExChangeHelper.Query(new ExChangeSearch
			{
				status = this.status,
				ProductName = text,
				IsCount = true,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				SortBy = "Id",
				SortOrder = SortAction.Desc
			}, ref totalRecords);
			if (dataTable != null)
			{
				dataTable.Columns.Add("sStatus");
				dataTable.Columns.Add("canChkStatus");
				if (dataTable.Rows.Count > 0)
				{
					for (int i = dataTable.Rows.Count - 1; i >= 0; i--)
					{
						System.DateTime t = System.DateTime.Parse(dataTable.Rows[i]["BeginDate"].ToString());
						System.DateTime t2 = System.DateTime.Parse(dataTable.Rows[i]["EndDate"].ToString());
						if (t > System.DateTime.Now)
						{
							dataTable.Rows[i]["sStatus"] = "未开始";
							dataTable.Rows[i]["canChkStatus"] = string.Empty;
						}
						else if (t2 >= System.DateTime.Now && t <= System.DateTime.Now)
						{
							dataTable.Rows[i]["sStatus"] = "进行中";
							dataTable.Rows[i]["canChkStatus"] = string.Empty;
						}
						else if (t2 < System.DateTime.Now)
						{
							dataTable.Rows[i]["sStatus"] = "已结束";
							dataTable.Rows[i]["canChkStatus"] = "disabled";
						}
						if (dataTable.Rows[i]["ExChangedNumber"].ToString() == "")
						{
							dataTable.Rows[i]["ExChangedNumber"] = "0";
						}
					}
				}
			}
			this.grdProducts.DataSource = dataTable;
			this.grdProducts.DataBind();
			this.pager.TotalRecords = totalRecords;
			this.CountTotal();
		}

		private void CountTotal()
		{
			int num = 0;
			ExChangeSearch exChangeSearch = new ExChangeSearch();
			exChangeSearch.status = ExchangeStatus.All;
			exChangeSearch.IsCount = true;
			exChangeSearch.PageIndex = this.pager.PageIndex;
			exChangeSearch.PageSize = this.pager.PageSize;
			exChangeSearch.SortBy = "Id";
			exChangeSearch.SortOrder = SortAction.Desc;
			System.Data.DataTable dataTable = PointExChangeHelper.Query(exChangeSearch, ref num);
			this.lblAll.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			exChangeSearch.status = ExchangeStatus.In;
			dataTable = PointExChangeHelper.Query(exChangeSearch, ref num);
			this.lblIn.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			exChangeSearch.status = ExchangeStatus.End;
			dataTable = PointExChangeHelper.Query(exChangeSearch, ref num);
			this.lblEnd.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			exChangeSearch.status = ExchangeStatus.unBegin;
			dataTable = PointExChangeHelper.Query(exChangeSearch, ref num);
			this.lblUnBegin.Text = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
		}

		protected void btnSeach_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

		protected void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				if (!array[i].bInt(ref num))
				{
					this.ShowMsg("选择活动出错！", false);
					return;
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				PointExChangeHelper.Delete(int.Parse(array[j]));
			}
			this.ShowMsg("删除活动成功！", true);
			this.BindData();
		}

		private void grdProducts_ItemCommand(object sender, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "Delete")
			{
				if (string.IsNullOrEmpty(e.CommandArgument.ToString()))
				{
					return;
				}
				int id = int.Parse(e.CommandArgument.ToString());
				PointExChangeHelper.Delete(id);
				this.ShowMsg("删除活动成功！", true);
				this.BindData();
			}
		}

		protected void DelBtn_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_ids.Text;
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				int num = 0;
				if (!array[i].bInt(ref num))
				{
					this.ShowMsg("选择活动出错！", false);
					return;
				}
			}
			for (int j = 0; j < array.Length; j++)
			{
				PointExChangeHelper.Delete(int.Parse(array[j]));
			}
			this.ShowMsg("删除活动成功！", true);
			this.BindData();
		}
	}
}
