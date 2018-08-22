using ASPNET.WebControls;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class CouponsList : AdminPage
	{
		protected static bool bFininshed = false;

		protected static int pagesize = 10;

		protected static int pageIndex = 1;

		protected System.DateTime time = System.DateTime.Now;

		protected string reUrl = string.Empty;

		protected bool isFinished;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected PageSize hrefPageSize;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_minVal;

		protected System.Web.UI.WebControls.TextBox txt_maxVal;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.DropDownList ddlCouponType;

		protected System.Web.UI.WebControls.Button btnSeach;

		protected System.Web.UI.WebControls.Button btnDelete;

		protected System.Web.UI.WebControls.Button DelBtn;

		protected System.Web.UI.WebControls.TextBox txt_ids;

		protected Grid grdCoupondsList;

		protected Pager pager1;

		protected System.Web.UI.HtmlControls.HtmlInputHidden htxtRoleId;

		protected System.Web.UI.WebControls.TextBox txt_totalNum;

		protected System.Web.UI.WebControls.DropDownList ddl_maxNum;

		protected ucDateTimePicker calendarStartDate2;

		protected ucDateTimePicker calendarEndDate2;

		protected System.Web.UI.WebControls.Button btnSubmit;

		protected System.Web.UI.WebControls.TextBox txt_id;

		protected System.Web.UI.WebControls.TextBox txt_used;

		protected CouponsList() : base("m08", "yxp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.isFinished = (Globals.RequestQueryStr("bFininshed").ToLower() == "true");
			this.reUrl = base.Request.Url.ToString();
			this.pager1.DefaultPageSize = CouponsList.pagesize;
			this.btnSeach.Click += new System.EventHandler(this.btnImagetSearch_Click);
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			this.btnDelete.Click += new System.EventHandler(this.DelBtn_Click);
			if (!base.IsPostBack)
			{
				for (int i = 1; i <= 10; i++)
				{
					this.ddl_maxNum.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString() + "张", i.ToString()));
				}
				string[] allKeys = base.Request.Params.AllKeys;
				if (allKeys.Contains("pagesize") && !this.bInt(base.Request["pagesize"].ToString(), ref CouponsList.pagesize))
				{
					CouponsList.pagesize = 20;
				}
				this.pager1.DefaultPageSize = CouponsList.pagesize;
				if (allKeys.Contains("pageIndex") && !this.bInt(base.Request["pageIndex"].ToString(), ref CouponsList.pageIndex))
				{
					CouponsList.pageIndex = 1;
				}
				if (allKeys.Contains("bFininshed") && !this.bBool(base.Request["bFininshed"].ToString(), ref CouponsList.bFininshed))
				{
					CouponsList.bFininshed = false;
				}
				this.BindCouonType();
				this.BindData();
			}
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdCoupondsList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdCoupondsList_RowDeleting);
			this.grdCoupondsList.RowUpdating += new System.Web.UI.WebControls.GridViewUpdateEventHandler(this.grdCoupondsList_RowUpdating);
		}

		private void grdCoupondsList_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
		{
			int couponId = (int)this.grdCoupondsList.DataKeys[e.RowIndex].Value;
			CouponInfo coupon = CouponHelper.GetCoupon(couponId);
			if (coupon == null)
			{
				this.ShowMsg("没有找到这张优惠券，该优惠券可能已被删除!", false);
				return;
			}
			CouponHelper.setCouponFinished(couponId, !CouponsList.bFininshed);
			this.BindData();
			if (!CouponsList.bFininshed)
			{
				this.ShowMsg("该优惠券已结束!", true);
				return;
			}
			this.ShowMsg("该优惠券已启用!", true);
		}

		private void grdCoupondsList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int couponId = (int)this.grdCoupondsList.DataKeys[e.RowIndex].Value;
			if (!CouponHelper.DeleteCoupon(couponId))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了选择的优惠券", true);
		}

		private void BindData()
		{
			decimal? minValue = null;
			decimal? maxValue = null;
			System.DateTime? beginDate = null;
			System.DateTime? endDate = null;
			string text = this.txt_name.Text;
			decimal value = 0m;
			System.DateTime arg_45_0 = System.DateTime.Now;
			if (this.bDecimal(this.txt_minVal.Text, ref value))
			{
				minValue = new decimal?(value);
			}
			if (this.bDecimal(this.txt_maxVal.Text, ref value))
			{
				maxValue = new decimal?(value);
			}
			beginDate = this.calendarStartDate.SelectedDate;
			endDate = this.calendarEndDate.SelectedDate;
			string selectedValue = this.ddlCouponType.SelectedValue;
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
				SortOrder = SortAction.Desc,
				Finished = new bool?(CouponsList.bFininshed),
				SearchType = new int?(string.IsNullOrEmpty(selectedValue) ? 0 : int.Parse(selectedValue))
			});
			if (couponInfos != null)
			{
				System.Data.DataTable dataTable = (System.Data.DataTable)couponInfos.Data;
				if (dataTable.Rows.Count > 0)
				{
					dataTable.Columns.Add("useConditon");
					dataTable.Columns.Add("ReceivNum");
					dataTable.Columns.Add("expire");
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
						dataTable.Rows[i]["expire"] = (System.DateTime.Parse(dataTable.Rows[i]["EndDate"].ToString()) <= System.DateTime.Now);
					}
				}
				this.grdCoupondsList.DataSource = dataTable;
				this.grdCoupondsList.DataBind();
			}
			this.pager1.TotalRecords = couponInfos.TotalRecords;
		}

		protected void btnImagetSearch_Click(object sender, System.EventArgs e)
		{
			this.BindData();
		}

		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			int couponId = 0;
			int value = 0;
			int value2 = 1;
			System.DateTime arg_0B_0 = System.DateTime.Now;
			System.DateTime arg_11_0 = System.DateTime.Now;
			if (!this.bInt(this.txt_id.Text, ref couponId))
			{
				this.ShowMsg("没有找到这张优惠券的信息！", false);
			}
			if (!this.bInt(this.txt_totalNum.Text, ref value))
			{
				this.ShowMsg("请输入正确的发放总量！", false);
			}
			if (!this.bInt(this.ddl_maxNum.SelectedValue, ref value2))
			{
				value2 = 1;
			}
			if (!this.calendarStartDate2.SelectedDate.HasValue)
			{
				this.ShowMsg("请输入正确的生效时间！", false);
			}
			if (!this.calendarEndDate2.SelectedDate.HasValue)
			{
				this.ShowMsg("请输入正确的过期时间！", false);
			}
			CouponEdit couponEdit = new CouponEdit();
			couponEdit.maxReceivNum = new int?(value2);
			couponEdit.totalNum = new int?(value);
			couponEdit.begin = this.calendarStartDate2.SelectedDate;
			couponEdit.end = this.calendarEndDate2.SelectedDate;
			string text = "";
			string text2 = CouponHelper.UpdateCoupon(couponId, couponEdit, ref text);
			if (this.txt_used.Text == "true")
			{
				CouponHelper.setCouponFinished(couponId, !CouponsList.bFininshed);
				this.txt_used.Text = "";
				this.BindData();
				this.ShowMsg("启用优惠券成功！", true);
				return;
			}
			if (text2 == "1")
			{
				this.ShowMsgAndReUrl("编辑优惠券成功！", true, this.reUrl);
				return;
			}
			this.ShowMsg(text2, false);
		}

		private bool bDate(string val, ref System.DateTime i)
		{
			return !string.IsNullOrEmpty(val) && System.DateTime.TryParse(val, out i);
		}

		private bool bInt(string val, ref int i)
		{
			return !string.IsNullOrEmpty(val) && !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			return !string.IsNullOrEmpty(val) && decimal.TryParse(val, out i);
		}

		private bool bBool(string val, ref bool i)
		{
			return !string.IsNullOrEmpty(val) && bool.TryParse(val, out i);
		}

		protected void DelBtn_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdCoupondsList.Rows)
			{
				if (gridViewRow.RowIndex >= 0)
				{
					System.Web.UI.WebControls.CheckBox checkBox = gridViewRow.Cells[0].FindControl("cbId") as System.Web.UI.WebControls.CheckBox;
					if (checkBox.Checked)
					{
						list.Add(int.Parse(this.grdCoupondsList.DataKeys[gridViewRow.RowIndex].Value.ToString()));
					}
				}
			}
			if (list.Count <= 0)
			{
				this.ShowMsg("请至少选择一条要删除的数据！", false);
				return;
			}
			foreach (int current in list)
			{
				int num = 0;
				if (!this.bInt(current.ToString(), ref num))
				{
					this.ShowMsg("选择优惠券出错！", false);
					return;
				}
			}
			foreach (int current2 in list)
			{
				CouponHelper.DeleteCoupon(current2);
			}
			this.ShowMsg("删除优惠券成功！", true);
			this.BindData();
		}

		protected string GetCouponType(string types)
		{
			if (string.IsNullOrEmpty(types))
			{
				return "--";
			}
			string[] array = types.Split(new char[]
			{
				','
			});
			string text = string.Empty;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				if (!string.IsNullOrEmpty(text))
				{
					text += string.Format(",{0}", ((CouponType)int.Parse(s)).ToString());
				}
				else
				{
					text += ((CouponType)int.Parse(s)).ToString();
				}
			}
			return text;
		}

		private void BindCouonType()
		{
			this.ddlCouponType.Items.Clear();
			this.ddlCouponType.Items.Add(new System.Web.UI.WebControls.ListItem
			{
				Value = "",
				Text = "优惠券类型"
			});
			foreach (int num in System.Enum.GetValues(typeof(CouponType)))
			{
				this.ddlCouponType.Items.Add(new System.Web.UI.WebControls.ListItem
				{
					Value = num.ToString(),
					Text = ((CouponType)num).ToString()
				});
			}
		}

		protected void grdCoupondsList_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.CheckBox checkBox = e.Row.FindControl("cbId") as System.Web.UI.WebControls.CheckBox;
				System.Web.UI.WebControls.Button button = e.Row.FindControl("lkDelete") as System.Web.UI.WebControls.Button;
				int num = Globals.ToNum(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ReceiveNum"));
				if (num > 0)
				{
					checkBox.Enabled = false;
					button.Enabled = false;
				}
			}
		}
	}
}
