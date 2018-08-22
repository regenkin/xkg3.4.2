using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class EditLimitedTimeDiscount : AdminPage
	{
		public int id;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txtActivityName;

		protected ucDateTimePicker dateBeginTime;

		protected ucDateTimePicker dateEndTime;

		protected System.Web.UI.WebControls.TextBox txtDescription;

		protected System.Web.UI.WebControls.TextBox txtLimitNumber;

		protected SetMemberRange memberRange;

		protected System.Web.UI.WebControls.Button btnSaveAndNext;

		protected System.Web.UI.WebControls.Button btnSave;

		protected EditLimitedTimeDiscount() : base("m08", "yxp24")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.id = Globals.RequestQueryNum("id");
				if (this.id > 0)
				{
					LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(this.id);
					if (discountInfo != null)
					{
						this.txtActivityName.Text = discountInfo.ActivityName;
						this.dateBeginTime.Text = discountInfo.BeginTime.ToString();
						this.dateEndTime.Text = discountInfo.EndTime.ToString();
						this.txtDescription.Text = discountInfo.Description;
						this.txtLimitNumber.Text = discountInfo.LimitNumber.ToString();
						System.Web.UI.WebControls.HiddenField hiddenField = this.memberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
						System.Web.UI.WebControls.HiddenField hiddenField2 = this.memberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
						System.Web.UI.WebControls.HiddenField hiddenField3 = this.memberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
						this.memberRange.Grade = discountInfo.ApplyMembers;
						this.memberRange.CustomGroup = discountInfo.CustomGroup;
						this.memberRange.DefualtGroup = discountInfo.DefualtGroup;
						hiddenField.Value = discountInfo.ApplyMembers;
						hiddenField2.Value = discountInfo.DefualtGroup;
						hiddenField3.Value = discountInfo.CustomGroup;
					}
				}
			}
		}

		protected void btnSave_Click(object sender, System.EventArgs e)
		{
			string text = this.txtActivityName.Text;
			System.DateTime? textToDate = this.dateBeginTime.TextToDate;
			System.DateTime? textToDate2 = this.dateEndTime.TextToDate;
			string text2 = this.txtDescription.Text;
			int limitNumber = 0;
			System.Web.UI.WebControls.HiddenField hiddenField = this.memberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField2 = this.memberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField3 = this.memberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("活动名称不能为空！", false);
				return;
			}
			if (!textToDate.HasValue || !textToDate2.HasValue)
			{
				this.ShowMsg("开始时间和结束时间都不能为空！", false);
				return;
			}
			if (textToDate.Value >= textToDate2.Value)
			{
				this.ShowMsg("开始时间不能大于或等于结束时间！", false);
				return;
			}
			if (!int.TryParse(this.txtLimitNumber.Text, out limitNumber))
			{
				this.ShowMsg("每人限购格式不对！", false);
				return;
			}
			if (hiddenField.Value == "-1" && hiddenField2.Value == "-1" && hiddenField3.Value == "-1")
			{
				this.ShowMsg("请选择适用会员！", false);
				return;
			}
			LimitedTimeDiscountInfo limitedTimeDiscountInfo = new LimitedTimeDiscountInfo();
			limitedTimeDiscountInfo.ActivityName = text;
			limitedTimeDiscountInfo.BeginTime = textToDate.Value;
			limitedTimeDiscountInfo.EndTime = textToDate2.Value;
			limitedTimeDiscountInfo.Description = text2;
			limitedTimeDiscountInfo.LimitNumber = limitNumber;
			limitedTimeDiscountInfo.ApplyMembers = hiddenField.Value;
			limitedTimeDiscountInfo.DefualtGroup = hiddenField2.Value;
			limitedTimeDiscountInfo.CustomGroup = hiddenField3.Value;
			limitedTimeDiscountInfo.CreateTime = System.DateTime.Now;
			limitedTimeDiscountInfo.Status = 1.ToString();
			int num = Globals.RequestQueryNum("id");
			if (num > 0)
			{
				limitedTimeDiscountInfo.LimitedTimeDiscountId = num;
				LimitedTimeDiscountHelper.UpdateLimitedTimeDiscount(limitedTimeDiscountInfo);
			}
			this.ShowMsgAndReUrl("保存成功！", true, "EditLimitedTimeDiscount.aspx?id=" + Globals.RequestQueryNum("id"));
		}

		protected void btnSaveAndNext_Click(object sender, System.EventArgs e)
		{
			string text = this.txtActivityName.Text;
			System.DateTime? textToDate = this.dateBeginTime.TextToDate;
			System.DateTime? textToDate2 = this.dateEndTime.TextToDate;
			string text2 = this.txtDescription.Text;
			int limitNumber = 0;
			System.Web.UI.WebControls.HiddenField hiddenField = this.memberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField2 = this.memberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField3 = this.memberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("活动名称不能为空！", false);
				return;
			}
			if (!textToDate.HasValue || !textToDate2.HasValue)
			{
				this.ShowMsg("开始时间和结束时间都不能为空！", false);
				return;
			}
			if (textToDate.Value >= textToDate2.Value)
			{
				this.ShowMsg("开始时间不能大于或等于结束时间！", false);
				return;
			}
			if (!int.TryParse(this.txtLimitNumber.Text, out limitNumber))
			{
				this.ShowMsg("每人限购格式不对！", false);
				return;
			}
			if (hiddenField.Value == "-1" && hiddenField2.Value == "-1" && hiddenField3.Value == "-1")
			{
				this.ShowMsg("请选择适用会员！", false);
				return;
			}
			int num = LimitedTimeDiscountHelper.AddLimitedTimeDiscount(new LimitedTimeDiscountInfo
			{
				ActivityName = text,
				BeginTime = textToDate.Value,
				EndTime = textToDate2.Value,
				Description = text2,
				LimitNumber = limitNumber,
				ApplyMembers = hiddenField.Value,
				DefualtGroup = hiddenField2.Value,
				CustomGroup = hiddenField3.Value,
				CreateTime = System.DateTime.Now,
				Status = 1.ToString()
			});
			if (num > 0)
			{
				this.ShowMsgAndReUrl("添加成功", true, "LimitedTimeDiscountAddProduct.aspx?id=" + num);
			}
		}
	}
}
