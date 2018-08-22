using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Member
{
	[PrivilegeCheck(Privilege.MemberGrades)]
	public class MemberGrades : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected MemberGrades() : base("m04", "hyp03")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindMemberRanks();
			}
		}

		protected void rptList_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				bool flag = (bool)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "IsDefault");
				if (flag)
				{
					System.Web.UI.WebControls.Label label = e.Item.FindControl("lblSplit") as System.Web.UI.WebControls.Label;
					label.Visible = false;
					System.Web.UI.WebControls.Button button = e.Item.FindControl("btnDel") as System.Web.UI.WebControls.Button;
					button.Visible = false;
				}
			}
		}

		protected void rptList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			if (e.CommandName == "DeleteGrade")
			{
				int gradeId = Globals.ToNum(e.CommandArgument.ToString());
				if (MemberHelper.DeleteMemberGrade(gradeId))
				{
					this.BindMemberRanks();
					this.ShowMsg("已经成功删除选择的会员等级", true);
					return;
				}
				this.ShowMsg("不能删除默认的会员等级或有会员的等级", false);
			}
		}

		private void BindMemberRanks()
		{
			this.rptList.DataSource = MemberHelper.GetMemberGrades();
			this.rptList.DataBind();
		}

		public string SelectUserCountGrades(int gid)
		{
			return MemberHelper.SelectUserCountGrades(gid).ToString();
		}
	}
}
