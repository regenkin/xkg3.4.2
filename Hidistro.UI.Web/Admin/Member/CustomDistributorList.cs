using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	public class CustomDistributorList : AdminPage
	{
		protected System.Web.UI.WebControls.Repeater rptList;

		protected System.Web.UI.WebControls.TextBox txtgroupname;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hdgroupId;

		protected System.Web.UI.WebControls.Button btnupdategroup;

		protected System.Web.UI.WebControls.TextBox txtaddgroupname;

		protected System.Web.UI.WebControls.Button btnaddgroup;

		protected CustomDistributorList() : base("m04", "hyp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnaddgroup.Click += new System.EventHandler(this.btnaddgroup_Click);
			this.btnupdategroup.Click += new System.EventHandler(this.btnupdategroup_Click);
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		protected void btnaddgroup_Click(object sender, System.EventArgs e)
		{
			string text = Globals.HtmlEncode(this.txtaddgroupname.Text.Trim());
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("分组名称不允许为空！", false);
				return;
			}
			string text2 = CustomGroupingHelper.AddCustomGrouping(new CustomGroupingInfo
			{
				GroupName = text
			});
			if (Globals.ToNum(text2) > 0)
			{
				this.ShowMsg("添加商品分组成功！", true);
				this.BindData();
				return;
			}
			this.ShowMsg("添加分组失败，" + text2, false);
		}

		protected void btnupdategroup_Click(object sender, System.EventArgs e)
		{
			int num = Globals.ToNum(this.hdgroupId.Value.Trim());
			string text = Globals.HtmlEncode(this.txtgroupname.Text.Trim());
			if (Globals.ToNum(num) <= 0)
			{
				this.ShowMsg("选择的分组有误,请重新选择", false);
				return;
			}
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("分组名称不允许为空", false);
				return;
			}
			string text2 = CustomGroupingHelper.UpdateCustomGrouping(new CustomGroupingInfo
			{
				GroupName = text,
				Id = num
			});
			if (Globals.ToNum(text2) > 0)
			{
				this.ShowMsg("修改商品分组成功", true);
				this.BindData();
				return;
			}
			this.ShowMsg("修改商品分组失败，" + text2, false);
		}

		private void BindData()
		{
			this.rptList.DataSource = CustomGroupingHelper.GetCustomGroupingDataTable();
			this.rptList.DataBind();
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			string commandName;
			if ((commandName = e.CommandName) != null)
			{
				if (!(commandName == "delete"))
				{
					return;
				}
				int groupid = Globals.ToNum(e.CommandArgument.ToString());
				if (CustomGroupingHelper.DelGroup(groupid))
				{
					this.ShowMsg("成功删除了指定的分组", true);
					this.BindData();
					return;
				}
				this.ShowMsg("删除分组失败", false);
			}
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				int groupId = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Id");
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				System.Data.DataTable customGroupingUser = CustomGroupingHelper.GetCustomGroupingUser(groupId);
				if (customGroupingUser.Rows.Count > 0)
				{
					num = customGroupingUser.Select("LastOrderDate is null").Length;
					int activeDay = MemberHelper.GetActiveDay();
					num2 = customGroupingUser.Select(" PayOrderDate is not null and PayOrderDate >='" + System.DateTime.Now.AddDays((double)(-(double)activeDay)).ToString("yyyy-MM-dd HH:mm:ss") + "'").Length;
					num3 = customGroupingUser.Select(" PayOrderDate is null or PayOrderDate <'" + System.DateTime.Now.AddDays((double)(-(double)activeDay)).ToString("yyyy-MM-dd HH:mm:ss") + "'").Length;
				}
				System.Web.UI.WebControls.Literal literal = e.Item.FindControl("ltMemberNumList") as System.Web.UI.WebControls.Literal;
				literal.Text = string.Concat(new object[]
				{
					"<td>",
					num,
					"</td><td>",
					num2,
					"</td><td>",
					num3,
					"</td>"
				});
			}
		}
	}
}
