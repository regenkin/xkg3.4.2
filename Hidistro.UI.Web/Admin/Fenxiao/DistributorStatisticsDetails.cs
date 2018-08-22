using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class DistributorStatisticsDetails : AdminPage
	{
		private int userid;

		private string StartTime = "";

		private string subLevel = "1";

		private string EndTime = "";

		protected decimal CurrentTotal;

		protected string CurrentStoreName = "";

		public int lastDay;

		protected string FristDisplay = "active";

		protected string SecondDisplay = "";

		private int i;

		private int rows;

		protected HiImage ListImage1;

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtStoreName;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button btnQueryLogs;

		protected System.Web.UI.WebControls.Button Button1;

		protected System.Web.UI.WebControls.Button Button4;

		protected System.Web.UI.HtmlControls.HtmlGenericControl OrdersTotal;

		protected System.Web.UI.HtmlControls.HtmlGenericControl ReferralOrders;

		protected System.Web.UI.HtmlControls.HtmlGenericControl BuyUsernums;

		protected System.Web.UI.HtmlControls.HtmlGenericControl BuyPrice;

		protected System.Web.UI.HtmlControls.HtmlGenericControl TotalReferral;

		protected System.Web.UI.WebControls.LinkButton Frist;

		protected System.Web.UI.WebControls.LinkButton Second;

		protected System.Web.UI.WebControls.Repeater reDistributor;

		protected Pager pager;

		protected DistributorStatisticsDetails() : base("m05", "fxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["UserId"], out this.userid))
			{
				this.Page.Response.Redirect("DistributorStatistics.aspx");
			}
			this.ListImage1.ImageUrl = "/admin/images/90x90.png";
			this.LoadParameters();
			this.reDistributor.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.reDistributor_ItemDataBound);
			System.Data.DataTable distributorSaleinfo = VShopHelper.GetDistributorSaleinfo(this.StartTime, this.EndTime, new int[]
			{
				this.userid
			});
			if (distributorSaleinfo != null && distributorSaleinfo.Rows.Count > 0)
			{
				string text = (string)distributorSaleinfo.Rows[0]["Logo"];
				if (System.IO.File.Exists(base.Server.MapPath(text)))
				{
					this.ListImage1.ImageUrl = text;
				}
				this.txtStoreName.InnerText = (string)distributorSaleinfo.Rows[0]["StoreName"];
				this.OrdersTotal.InnerText = "￥" + System.Convert.ToDouble(distributorSaleinfo.Rows[0]["OrderTotalSum"]).ToString("0.00");
				this.ReferralOrders.InnerText = distributorSaleinfo.Rows[0]["Ordernums"].ToString();
				decimal num = decimal.Parse(distributorSaleinfo.Rows[0]["CommTotalSum"].ToString());
				this.TotalReferral.InnerText = "￥" + System.Convert.ToDouble(num.ToString()).ToString("0.00");
				this.BuyUsernums.InnerText = distributorSaleinfo.Rows[0]["BuyUserIds"].ToString();
				this.BuyPrice.InnerText = (decimal.Parse(distributorSaleinfo.Rows[0]["OrderTotalSum"].ToString()) / decimal.Parse(distributorSaleinfo.Rows[0]["BuyUserIds"].ToString())).ToString("F2");
			}
			this.BindData(this.userid);
		}

		private void reDistributor_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litph");
				System.Data.DataRowView dataRowView = (System.Data.DataRowView)e.Item.DataItem;
				this.i++;
				this.rows = (this.pager.PageIndex - 1) * this.pager.PageSize + this.i;
				if (dataRowView["Ordernums"].ToString() == "0")
				{
					literal.Text = "\u3000";
					return;
				}
				if (this.rows == 1)
				{
					literal.Text = "<img src=\"../images/0001.gif\"></img>";
					return;
				}
				if (this.rows == 2)
				{
					literal.Text = "<img src=\"../images/0002.gif\"></img>";
					return;
				}
				if (this.rows == 3)
				{
					literal.Text = "<img src=\"../images/0003.gif\"></img>";
					return;
				}
				literal.Text = (int.Parse(literal.Text) + this.rows).ToString();
			}
		}

		private void BindData(int UserId)
		{
			DbQueryResult subDistributorsRankingsN = VShopHelper.GetSubDistributorsRankingsN(this.StartTime, this.EndTime, this.pager.PageSize, this.pager.PageIndex, this.userid, int.Parse(this.subLevel));
			System.Data.DataTable arg_44_0 = (System.Data.DataTable)subDistributorsRankingsN.Data;
			this.reDistributor.DataSource = subDistributorsRankingsN.Data;
			this.reDistributor.DataBind();
			this.pager.TotalRecords = subDistributorsRankingsN.TotalRecords;
			System.Data.DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(this.userid);
			this.Frist.Text = "一级分店(" + distributorsSubStoreNum.Rows[0]["firstV"] + ")";
			this.Second.Text = "二级分店(" + distributorsSubStoreNum.Rows[0]["secondV"] + ")";
		}

		protected void Second_Click(object sender, System.EventArgs e)
		{
			this.FristDisplay = "";
			this.SecondDisplay = "active";
			this.subLevel = "2";
			this.ReBind(true);
		}

		protected void Frist_Click(object sender, System.EventArgs e)
		{
			this.FristDisplay = "active";
			this.SecondDisplay = "";
			this.subLevel = "1";
			this.ReBind(true);
		}

		protected void Button1_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-6.0).ToString("yyyy-MM-dd");
			this.lastDay = 7;
			this.ReBind(true);
		}

		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartTime"]))
				{
					this.StartTime = base.Server.UrlDecode(this.Page.Request.QueryString["StartTime"]);
					this.calendarStartDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.StartTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["subLevel"]))
				{
					this.subLevel = base.Server.UrlDecode(this.Page.Request.QueryString["subLevel"]);
				}
				else
				{
					this.subLevel = "1";
				}
				if (this.subLevel == "1")
				{
					this.FristDisplay = "active";
					this.SecondDisplay = "";
				}
				else
				{
					this.FristDisplay = "";
					this.SecondDisplay = "active";
					this.subLevel = "2";
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndTime"]))
				{
					this.EndTime = base.Server.UrlDecode(this.Page.Request.QueryString["EndTime"]);
					this.calendarEndDate.SelectedDate = new System.DateTime?(System.DateTime.Parse(this.EndTime));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lastDay"]))
				{
					int.TryParse(this.Page.Request.QueryString["lastDay"], out this.lastDay);
					if (this.lastDay == 30)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FF00CC");
						return;
					}
					if (this.lastDay == 7)
					{
						this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("#FF00CC");
						this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
						return;
					}
					this.Button1.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					this.Button4.BorderColor = System.Drawing.ColorTranslator.FromHtml("");
					return;
				}
			}
			else
			{
				if (this.calendarStartDate.SelectedDate.HasValue)
				{
					this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
				}
				if (this.calendarEndDate.SelectedDate.HasValue)
				{
					this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
				}
			}
		}

		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("UserId", this.Page.Request.QueryString["UserId"]);
			nameValueCollection.Add("StartTime", this.StartTime);
			nameValueCollection.Add("EndTime", this.EndTime);
			nameValueCollection.Add("subLevel", this.subLevel);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("lastDay", this.lastDay.ToString());
			base.ReloadPage(nameValueCollection);
		}

		protected void Button4_Click1(object sender, System.EventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			this.EndTime = now.ToString("yyyy-MM-dd");
			this.StartTime = now.AddDays(-29.0).ToString("yyyy-MM-dd");
			this.lastDay = 30;
			this.ReBind(true);
		}

		protected void btnQueryLogs_Click(object sender, System.EventArgs e)
		{
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				this.EndTime = this.calendarEndDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				this.StartTime = this.calendarStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
			}
			this.lastDay = 0;
			this.ReBind(true);
		}
	}
}
