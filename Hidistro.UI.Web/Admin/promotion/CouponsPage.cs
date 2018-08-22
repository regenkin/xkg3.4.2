using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class CouponsPage : System.Web.UI.Page
	{
		protected static bool bFininshed = false;

		protected static bool bAllProduct = false;

		protected static int pagesize = 20;

		protected static int pageIndex = 1;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_minVal;

		protected System.Web.UI.WebControls.TextBox txt_maxVal;

		protected WebCalendar calendarStartDate;

		protected WebCalendar calendarEndDate;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected Grid grdCoupondsList;

		protected Pager pager1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSeach.Click += new System.EventHandler(this.btnImagetSearch_Click);
			if (!base.IsPostBack)
			{
				string[] allKeys = base.Request.Params.AllKeys;
				if (allKeys.Contains("pagesize") && !this.bInt(base.Request.Params["pagesize"].ToString(), ref CouponsPage.pagesize))
				{
					CouponsPage.pagesize = 20;
				}
				if (allKeys.Contains("pageIndex") && !this.bInt(base.Request.Params["pageIndex"].ToString(), ref CouponsPage.pageIndex))
				{
					CouponsPage.pageIndex = 1;
				}
				if (allKeys.Contains("bAllProduct") && !this.bBool(base.Request.Params["bAllProduct"].ToString(), ref CouponsPage.bAllProduct))
				{
					CouponsPage.bAllProduct = false;
				}
				if (allKeys.Contains("bFininshed") && !this.bBool(base.Request.Params["bFininshed"].ToString(), ref CouponsPage.bFininshed))
				{
					CouponsPage.bFininshed = false;
				}
				this.BindData();
			}
		}

		private void BindData()
		{
			decimal? minValue = null;
			decimal? maxValue = null;
			System.DateTime? beginDate = null;
			System.DateTime? endDate = null;
			string text = this.txt_name.Text;
			decimal value = 0m;
			System.DateTime now = System.DateTime.Now;
			if (this.bDecimal(this.txt_minVal.Text, ref value))
			{
				minValue = new decimal?(value);
			}
			if (this.bDecimal(this.txt_maxVal.Text, ref value))
			{
				maxValue = new decimal?(value);
			}
			if (this.bDate(this.calendarStartDate.Text, ref now))
			{
				beginDate = new System.DateTime?(now);
			}
			if (this.bDate(this.calendarEndDate.Text, ref now))
			{
				endDate = new System.DateTime?(now);
			}
			DbQueryResult couponInfos = CouponHelper.GetCouponInfos(new CouponsSearch
			{
				CouponName = text,
				minValue = minValue,
				maxValue = maxValue,
				beginDate = beginDate,
				endDate = endDate,
				IsCount = true,
				PageIndex = this.pager1.PageIndex,
				PageSize = this.pager1.PageSize,
				SortBy = "CouponId",
				SortOrder = SortAction.Desc
			});
			System.Data.DataTable dataTable = (System.Data.DataTable)couponInfos.Data;
			if (dataTable.Rows.Count > 0)
			{
				dataTable.Columns.Add("useConditon");
				dataTable.Columns.Add("ReceivNum");
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					decimal d = decimal.Parse(dataTable.Rows[i]["ConditionValue"].ToString());
					if (d == 0m)
					{
						dataTable.Rows[i]["useConditon"] = "不限制";
					}
					else
					{
						dataTable.Rows[i]["useConditon"] = "满" + d.ToString("F2") + "可使用";
					}
					string text2 = dataTable.Rows[i]["maxReceivNum"].ToString();
					if (text2 == "0")
					{
						dataTable.Rows[i]["ReceivNum"] = "无限制";
					}
					else
					{
						dataTable.Rows[i]["ReceivNum"] = text2 + "/张每人";
					}
				}
			}
			this.grdCoupondsList.DataSource = dataTable;
			this.grdCoupondsList.DataBind();
			this.pager1.TotalRecords = couponInfos.TotalRecords;
		}

		protected void btnImagetSearch_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		private bool bDate(string val, ref System.DateTime i)
		{
			return !string.IsNullOrEmpty(val) && System.DateTime.TryParse(val, out i);
		}

		private bool bBool(string val, ref bool i)
		{
			return !string.IsNullOrEmpty(val) && bool.TryParse(val, out i);
		}
	}
}
