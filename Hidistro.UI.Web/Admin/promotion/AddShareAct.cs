using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddShareAct : AdminPage
	{
		protected int actId;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.DropDownList cmb_CouponList;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txt_MeetValue;

		protected System.Web.UI.WebControls.TextBox txt_Number;

		protected System.Web.UI.WebControls.HiddenField hidpic;

		protected System.Web.UI.WebControls.HiddenField hidpicdel;

		protected System.Web.UI.WebControls.TextBox txt_img;

		protected System.Web.UI.WebControls.TextBox txt_title;

		protected System.Web.UI.WebControls.TextBox txt_des;

		protected System.Web.UI.WebControls.Button saveBtn;

		protected System.Web.UI.WebControls.HiddenField shareActId;

		protected AddShareAct() : base("m08", "yxp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.saveBtn.Click += new System.EventHandler(this.Unnamed_Click);
			if (!base.IsPostBack)
			{
				System.Data.DataTable couponList = this.GetCouponList();
				if (couponList != null && couponList.Rows.Count > 0)
				{
					for (int i = 0; i < couponList.Rows.Count; i++)
					{
						System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(couponList.Rows[i]["CouponName"].ToString(), couponList.Rows[i]["CouponId"].ToString());
						this.cmb_CouponList.Items.Add(item);
					}
				}
				System.Web.UI.WebControls.ListItem item2 = new System.Web.UI.WebControls.ListItem("请选择", "0");
				this.cmb_CouponList.Items.Insert(0, item2);
				string[] allKeys = base.Request.Params.AllKeys;
				if (allKeys.Contains("id") && base.Request["id"].ToString().bInt(ref this.actId))
				{
					this.shareActId.Value = this.actId.ToString();
					ShareActivityInfo shareActivityInfo = new ShareActivityInfo();
					shareActivityInfo = ShareActHelper.GetAct(this.actId);
					this.txt_MeetValue.Text = shareActivityInfo.MeetValue.ToString("F2");
					this.txt_Number.Text = shareActivityInfo.CouponNumber.ToString();
					this.cmb_CouponList.SelectedValue = shareActivityInfo.CouponId.ToString();
					this.calendarStartDate.SelectedDate = new System.DateTime?(shareActivityInfo.BeginDate);
					this.calendarEndDate.SelectedDate = new System.DateTime?(shareActivityInfo.EndDate);
					this.txt_name.Text = shareActivityInfo.ActivityName;
					this.txt_img.Text = shareActivityInfo.ImgUrl;
					this.hidpic.Value = shareActivityInfo.ImgUrl;
					this.txt_title.Text = shareActivityInfo.ShareTitle;
					this.txt_des.Text = shareActivityInfo.Description;
				}
			}
		}

		private System.Data.DataTable GetCouponList()
		{
			return CouponHelper.GetUnFinishedCoupon(System.DateTime.Now, new CouponType?(CouponType.活动赠送));
		}

		protected void Unnamed_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.cmb_CouponList.SelectedValue) || this.cmb_CouponList.SelectedValue == "0")
			{
				this.ShowMsg("请选择优惠券！", false);
				return;
			}
			int couponId = int.Parse(this.cmb_CouponList.SelectedValue);
			System.DateTime date = this.calendarStartDate.SelectedDate.Value.Date;
			System.DateTime dateTime = this.calendarEndDate.SelectedDate.Value.Date.AddDays(1.0).AddSeconds(-1.0);
			string text = this.txt_MeetValue.Text;
			string text2 = this.txt_Number.Text;
			decimal meetValue = 0m;
			int couponNumber = 0;
			if (string.IsNullOrEmpty(this.txt_name.Text.Trim()))
			{
				this.ShowMsg("活动名称不能为空！", false);
				return;
			}
			if (dateTime < date)
			{
				this.ShowMsg("活动结束时间不能小于开始时间！", false);
				return;
			}
			CouponInfo coupon = CouponHelper.GetCoupon(couponId);
			if (coupon == null)
			{
				this.ShowMsg("优惠券不存在！", false);
				return;
			}
			if (dateTime > coupon.EndDate)
			{
				this.ShowMsg("活动结束时间不能大于优惠券的结束时间！", false);
				return;
			}
			if (!text.bDecimal(ref meetValue))
			{
				this.ShowMsg("订单满足金额输入错误！", false);
				return;
			}
			if (!text2.bInt(ref couponNumber))
			{
				this.ShowMsg("优惠券张数输入错误！", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txt_img.Text))
			{
				this.ShowMsg("请上传朋友圈显示图片！", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txt_title.Text.Trim()))
			{
				this.ShowMsg("朋友圈分享标题不能为空！", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txt_des.Text.Trim()))
			{
				this.ShowMsg("活动介绍不能为空！", false);
				return;
			}
			ShareActivityInfo shareActivityInfo = new ShareActivityInfo();
			this.actId = int.Parse(this.shareActId.Value);
			if (this.actId != 0)
			{
				shareActivityInfo = ShareActHelper.GetAct(this.actId);
			}
			shareActivityInfo.BeginDate = date;
			shareActivityInfo.EndDate = dateTime;
			shareActivityInfo.CouponId = couponId;
			shareActivityInfo.CouponNumber = couponNumber;
			shareActivityInfo.CouponName = coupon.CouponName;
			shareActivityInfo.MeetValue = meetValue;
			shareActivityInfo.ActivityName = this.txt_name.Text;
			shareActivityInfo.ImgUrl = this.txt_img.Text;
			shareActivityInfo.ShareTitle = this.txt_title.Text;
			shareActivityInfo.Description = this.txt_des.Text;
			if (this.actId != 0)
			{
				string text3 = "";
				ShareActHelper.Update(shareActivityInfo, ref text3);
				this.ShowMsg("修改成功！", true);
			}
			else
			{
				string text4 = "";
				ShareActHelper.Create(shareActivityInfo, ref text4);
				this.ShowMsg("保存成功！", true);
			}
			base.Response.Redirect("ShareActList.aspx");
		}
	}
}
