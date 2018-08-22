using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class SuperiorSelect : AdminPage
	{
		protected string searchName = string.Empty;

		protected int userid = Globals.RequestQueryNum("userid");

		protected int ReferralUserId;

		protected string htmlStoreName = string.Empty;

		protected string htmlSuperName = "主站";

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtKey;

		protected System.Web.UI.WebControls.Button btnSearch;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected System.Web.UI.WebControls.Panel divEmpty;

		protected Pager pager;

		protected SuperiorSelect() : base("m05", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("posttype");
			if (a == "update")
			{
				base.Response.ContentType = "application/json";
				this.userid = Globals.RequestFormNum("userid");
				int tosuperuserid = Globals.RequestFormNum("touserid");
				string text = DistributorsBrower.UpdateDistributorSuperior(this.userid, tosuperuserid);
				string s;
				if (text == "1")
				{
					s = "{\"type\":\"1\",\"tips\":\"修改成功！\"}";
				}
				else
				{
					s = "{\"type\":\"0\",\"tips\":\"" + text + "\"}";
				}
				base.Response.Write(s);
				base.Response.End();
			}
			if (this.userid > 0)
			{
				DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(this.userid);
				if (distributorInfo == null)
				{
					this.divEmpty.Visible = true;
					return;
				}
				this.htmlStoreName = distributorInfo.StoreName;
				if (distributorInfo.ReferralUserId > 0 && distributorInfo.UserId != distributorInfo.ReferralUserId)
				{
					this.ReferralUserId = distributorInfo.ReferralUserId;
					distributorInfo = DistributorsBrower.GetDistributorInfo(distributorInfo.ReferralUserId);
					if (distributorInfo != null)
					{
						this.htmlSuperName = distributorInfo.StoreName;
					}
				}
				this.searchName = Globals.RequestQueryStr("key").Trim();
				if (!base.IsPostBack)
				{
					this.txtKey.Text = this.searchName;
					this.BindData(this.searchName);
				}
			}
		}

		private void BindData(string title)
		{
			DistributorsQuery distributorsQuery = new DistributorsQuery();
			distributorsQuery.GradeId = 0;
			distributorsQuery.StoreName = title;
			distributorsQuery.PageIndex = this.pager.PageIndex;
			distributorsQuery.PageSize = this.pager.PageSize;
			distributorsQuery.SortOrder = SortAction.Desc;
			distributorsQuery.SortBy = "userid";
			Globals.EntityCoding(distributorsQuery, true);
			DbQueryResult distributors = VShopHelper.GetDistributors(distributorsQuery, null, null);
			int totalRecords = distributors.TotalRecords;
			if (totalRecords > 0)
			{
				this.rptList.DataSource = distributors.Data;
				this.rptList.DataBind();
				this.pager.TotalRecords = distributors.TotalRecords;
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

		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			string s = this.txtKey.Text.Trim();
			base.Response.Redirect(string.Concat(new object[]
			{
				"superiorselect.aspx?userid=",
				this.userid,
				"&key=",
				base.Server.UrlEncode(s)
			}));
			base.Response.End();
		}

		protected string FormatOperBtn(object touserid, object storename)
		{
			string result = string.Empty;
			string text = DistributorsBrower.IsCanUpdateDistributorSuperior(this.userid, Globals.ToNum(touserid));
			if (text == "1" && this.ReferralUserId != System.Convert.ToInt32(touserid))
			{
				result = string.Concat(new object[]
				{
					"<input type='button' id='dist",
					touserid,
					"' class='btn btn-primary btn-xs' value='设为上级' onclick=\"setsuper(this,",
					touserid,
					",'",
					base.Server.HtmlEncode(storename.ToString()),
					"')\" />"
				});
			}
			else
			{
				result = "<span title='" + base.Server.HtmlEncode(text) + "'><input type='button' class='btn btn-primary btn-xs' style='background-color:#5cb85c;border-color:#4cae4c' value='设为上级' disabled='disabled'/></span>";
			}
			return result;
		}
	}
}
