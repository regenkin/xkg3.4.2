using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class UsersSelect : AdminPage
	{
		protected string adminname = string.Empty;

		protected string searchName = string.Empty;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtKey;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected System.Web.UI.WebControls.Panel divEmpty;

		protected Pager pager;

		protected UsersSelect() : base("m01", "dpp11")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.adminname = ManagerHelper.GetCurrentManager().UserName;
			string a = Globals.RequestFormStr("posttype");
			if (a == "sel")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"success\":\"1\",\"tips\":\"操作成功！\"}";
				int userid = Globals.RequestFormNum("userid");
				int num = Globals.RequestFormNum("issel");
				if (num == 1)
				{
					NoticeHelper.DelUser(userid, this.adminname);
				}
				else
				{
					NoticeHelper.AddUser(userid, this.adminname);
				}
				base.Response.Write(s);
				base.Response.End();
			}
			if (!base.IsPostBack)
			{
				this.searchName = Globals.RequestQueryStr("key");
				this.txtKey.Text = this.searchName;
				this.BindData(this.searchName);
			}
		}

		private void BindData(string title)
		{
			DbQueryResult members = MemberHelper.GetMembers(new MemberQuery
			{
				Username = title,
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				Stutas = new UserStatus?(UserStatus.Normal),
				EndTime = new System.DateTime?(System.DateTime.Now),
				StartTime = new System.DateTime?(System.DateTime.Now.AddDays((double)(-(double)SettingsManager.GetMasterSettings(false).ActiveDay))),
				CellPhone = (base.Request.QueryString["phone"] != null) ? base.Request.QueryString["phone"] : "",
				ClientType = (base.Request.QueryString["clientType"] != null) ? base.Request.QueryString["clientType"] : ""
			}, false);
			int totalRecords = members.TotalRecords;
			if (totalRecords > 0)
			{
				this.rptList.DataSource = members.Data;
				this.rptList.DataBind();
				this.pager.TotalRecords = (this.pager.TotalRecords = totalRecords);
				if (this.pager.TotalRecords <= this.pager.PageSize)
				{
					this.pager.Visible = false;
					return;
				}
			}
			else
			{
				this.divEmpty.Visible = true;
			}
		}

		protected string FormatOper(object userid, object adminname)
		{
			string result = string.Empty;
			if (NoticeHelper.GetUserIsSel(Globals.ToNum(userid), adminname.ToString()))
			{
				result = "<input type='button' class='btn btn-success btn-xs' value='已选' issel='1' userid='" + userid.ToString() + "' onclick='seluser(this)'/>";
			}
			else
			{
				result = "<input type='button' class='btn btn-primary btn-xs' value='选择' issel='0' userid='" + userid.ToString() + "' onclick='seluser(this)'/>";
			}
			return result;
		}

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string s = this.txtKey.Text.Trim();
			base.Response.Redirect("usersselect.aspx?admin=" + this.adminname + "&key=" + base.Server.UrlEncode(s));
			base.Response.End();
		}
	}
}
