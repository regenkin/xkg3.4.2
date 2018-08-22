using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorDetails : AdminPage
	{
		private int userid;

		protected HiImage ListImage1;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRealName;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCellPhone;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtUserName;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMicroName;

		protected HiImage StoreCode;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtStoreName;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtName;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtUrl;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtCreateTime;

		protected System.Web.UI.HtmlControls.HtmlGenericControl ReferralOrders;

		protected System.Web.UI.HtmlControls.HtmlGenericControl OrdersTotal;

		protected System.Web.UI.HtmlControls.HtmlGenericControl TotalReferral;

		protected System.Web.UI.HtmlControls.HtmlGenericControl ReferralBlance;

		protected System.Web.UI.HtmlControls.HtmlGenericControl ReferralRequestBalance;

		protected System.Web.UI.WebControls.Repeater reCommissions;

		protected Pager pager;

		protected DistributorDetails() : base("m05", "fxp03")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
			{
				this.Page.Response.Redirect("DistributorList.aspx");
			}
			this.ListImage1.ImageUrl = "/Templates/common/images/user.png";
			DistributorsQuery distributorsQuery = new DistributorsQuery();
			distributorsQuery.UserId = this.userid;
			distributorsQuery.ReferralStatus = -1;
			distributorsQuery.PageIndex = 1;
			distributorsQuery.PageSize = 1;
			distributorsQuery.SortOrder = SortAction.Desc;
			distributorsQuery.SortBy = "userid";
			Globals.EntityCoding(distributorsQuery, true);
			DbQueryResult distributors = VShopHelper.GetDistributors(distributorsQuery, null, null);
			System.Data.DataTable dataTable = new System.Data.DataTable();
			if (distributors.Data != null)
			{
				dataTable = (System.Data.DataTable)distributors.Data;
			}
			else
			{
				this.Page.Response.Redirect("DistributorList.aspx");
			}
			if (dataTable.Rows[0]["UserHead"] != System.DBNull.Value && dataTable.Rows[0]["UserHead"].ToString().Trim() != "")
			{
				this.ListImage1.ImageUrl = dataTable.Rows[0]["UserHead"].ToString();
			}
			this.txtCellPhone.InnerText = ((dataTable.Rows[0]["CellPhone"] == System.DBNull.Value) ? "" : ((string)dataTable.Rows[0]["CellPhone"]));
			this.txtStoreName.InnerText = (string)dataTable.Rows[0]["StoreName"];
			this.txtMicroName.InnerText = (string)dataTable.Rows[0]["UserName"];
			this.txtUserName.InnerText = (string)dataTable.Rows[0]["UserName"];
			this.txtRealName.InnerText = ((dataTable.Rows[0]["RealName"] == System.DBNull.Value) ? "" : ((string)dataTable.Rows[0]["RealName"]));
			this.txtCreateTime.InnerText = ((System.DateTime)dataTable.Rows[0]["CreateTime"]).ToString("yyyy-MM-dd HH:mm:ss");
			this.txtName.InnerText = ((dataTable.Rows[0]["Name"] == System.DBNull.Value) ? "" : ((string)dataTable.Rows[0]["Name"]));
			string text = Globals.HostPath(System.Web.HttpContext.Current.Request.Url) + "/Default.aspx?ReferralId=" + distributorsQuery.UserId;
			this.txtUrl.InnerText = text;
			this.StoreCode.ImageUrl = "http://s.jiathis.com/qrcode.php?url=" + text;
			this.OrdersTotal.InnerText = "￥" + System.Convert.ToDouble(dataTable.Rows[0]["OrdersTotal"]).ToString("0.00");
			this.ReferralOrders.InnerText = dataTable.Rows[0]["ReferralOrders"].ToString();
			this.ReferralBlance.InnerText = "￥" + System.Convert.ToDouble(dataTable.Rows[0]["ReferralBlance"]).ToString("0.00");
			this.ReferralRequestBalance.InnerText = "￥" + System.Convert.ToDouble(dataTable.Rows[0]["ReferralRequestBalance"]).ToString("0.00");
			decimal num = decimal.Parse(dataTable.Rows[0]["ReferralBlance"].ToString()) + decimal.Parse(dataTable.Rows[0]["ReferralRequestBalance"].ToString());
			this.TotalReferral.InnerText = "￥" + System.Convert.ToDouble(num.ToString()).ToString("0.00");
			this.BindData(distributorsQuery.UserId);
		}

		private void BindData(int UserId)
		{
			BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
			balanceDrawRequestQuery.CheckTime = "";
			balanceDrawRequestQuery.UserId = UserId.ToString();
			balanceDrawRequestQuery.RequestTime = "";
			balanceDrawRequestQuery.StoreName = "";
			balanceDrawRequestQuery.PageIndex = this.pager.PageIndex;
			balanceDrawRequestQuery.PageSize = this.pager.PageSize;
			balanceDrawRequestQuery.SortOrder = SortAction.Desc;
			balanceDrawRequestQuery.SortBy = "RequestTime";
			balanceDrawRequestQuery.RequestEndTime = "";
			balanceDrawRequestQuery.RequestStartTime = "";
			balanceDrawRequestQuery.IsCheck = "";
			Globals.EntityCoding(balanceDrawRequestQuery, true);
			DbQueryResult balanceDrawRequest = VShopHelper.GetBalanceDrawRequest(balanceDrawRequestQuery);
			this.reCommissions.DataSource = balanceDrawRequest.Data;
			this.reCommissions.DataBind();
			this.pager.TotalRecords = balanceDrawRequest.TotalRecords;
		}
	}
}
