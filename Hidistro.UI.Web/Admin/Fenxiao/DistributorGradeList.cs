using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorGradeList : AdminPage
	{
		protected string LocalUrl = string.Empty;

		private string Name = "";

		private System.Collections.Generic.Dictionary<int, int> GradeCountDic;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected DistributorGradeList() : base("m05", "fxp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Button button = (System.Web.UI.WebControls.Button)e.Item.FindControl("lbtnDel");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("GradeSum");
				literal.Text = ((System.Data.DataRowView)e.Item.DataItem).Row["GradeId"].ToString();
				int key = int.Parse(literal.Text);
				if (this.GradeCountDic.ContainsKey(key))
				{
					literal.Text = this.GradeCountDic[key].ToString();
				}
				else
				{
					literal.Text = "0";
				}
				if (((System.Data.DataRowView)e.Item.DataItem).Row["IsDefault"].ToString() == "False")
				{
					return;
				}
				button.Enabled = false;
				button.Visible = false;
			}
		}

		private void ReBind(bool isSearch)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{
				{
					"Name",
					""
				},
				{
					"pageSize",
					"100"
				},
				{
					"pageIndex",
					"1"
				}
			});
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			int num = 0;
			int.TryParse(e.CommandArgument.ToString(), out num);
			string commandName;
			if (num > 0 && (commandName = e.CommandName) != null)
			{
				if (commandName == "setdefault")
				{
					DistributorGradeBrower.SetGradeDefault(num);
					this.ReBind(true);
					return;
				}
				if (!(commandName == "del"))
				{
					return;
				}
				string text = DistributorGradeBrower.DelOneGrade(num);
				string a;
				if ((a = text) != null)
				{
					if (a == "-1")
					{
						this.ShowMsg("不能删除，因为该等级下面已经有分销商！", false);
						return;
					}
					if (a == "1")
					{
						this.ShowMsg("分销商等级删除成功！", true);
						this.BindData();
						return;
					}
				}
				this.ShowMsg("删除失败", false);
			}
		}

		private void BindData()
		{
			DistributorGradeQuery distributorGradeQuery = new DistributorGradeQuery();
			distributorGradeQuery.Name = this.Name;
			distributorGradeQuery.SortBy = "GradeID";
			distributorGradeQuery.SortOrder = SortAction.Asc;
			Globals.EntityCoding(distributorGradeQuery, true);
			distributorGradeQuery.PageIndex = 1;
			distributorGradeQuery.PageSize = 100;
			DbQueryResult distributorGradeRequest = DistributorGradeBrower.GetDistributorGradeRequest(distributorGradeQuery);
			if (this.GradeCountDic == null)
			{
				this.GradeCountDic = DistributorGradeBrower.GetGradeCount("0");
			}
			this.rptList.DataSource = distributorGradeRequest.Data;
			this.rptList.DataBind();
		}

		protected string FormatCommissionRise(object commissionrise)
		{
			string result = string.Empty;
			decimal num = 0.00m;
			decimal.TryParse(commissionrise.ToString(), out num);
			if (num == 0.00m)
			{
				result = "-";
			}
			else
			{
				result = "+" + num + "%";
			}
			return result;
		}
	}
}
