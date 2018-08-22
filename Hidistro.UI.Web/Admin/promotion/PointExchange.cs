using ControlPanel.Promotions;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class PointExchange : AdminPage
	{
		protected int eId;

		protected bool bFinished;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.HiddenField hidpic;

		protected System.Web.UI.WebControls.HiddenField hidpicdel;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_img;

		protected SetMemberRange SetMemberRange;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.Button saveBtn;

		protected PointExchange() : base("m08", "yxp02")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && base.Request["id"].ToString().bInt(ref this.eId) && !this.Page.IsPostBack)
			{
				PointExChangeInfo pointExChangeInfo = PointExChangeHelper.Get(this.eId);
				this.txt_name.Text = pointExChangeInfo.Name;
				this.calendarStartDate.SelectedDate = new System.DateTime?(pointExChangeInfo.BeginDate);
				this.calendarEndDate.SelectedDate = new System.DateTime?(pointExChangeInfo.EndDate);
				this.txt_img.Text = pointExChangeInfo.ImgUrl;
				this.hidpic.Value = pointExChangeInfo.ImgUrl;
				System.Web.UI.WebControls.HiddenField hiddenField = this.SetMemberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField3 = this.SetMemberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
				this.SetMemberRange.Grade = pointExChangeInfo.MemberGrades;
				this.SetMemberRange.DefualtGroup = pointExChangeInfo.DefualtGroup;
				this.SetMemberRange.CustomGroup = pointExChangeInfo.CustomGroup;
				hiddenField.Value = pointExChangeInfo.MemberGrades;
				hiddenField2.Value = pointExChangeInfo.DefualtGroup;
				hiddenField3.Value = pointExChangeInfo.CustomGroup;
				if (pointExChangeInfo.EndDate < System.DateTime.Now)
				{
					this.bFinished = true;
					return;
				}
				this.bFinished = false;
			}
		}

		protected void saveBtn_Click(object sender, System.EventArgs e)
		{
			System.Web.UI.WebControls.HiddenField hiddenField = this.SetMemberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
			System.Web.UI.WebControls.HiddenField hiddenField3 = this.SetMemberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
			string value = hiddenField.Value;
			string value2 = hiddenField2.Value;
			string value3 = hiddenField3.Value;
			string text = this.txt_name.Text;
			System.DateTime? selectedDate = this.calendarStartDate.SelectedDate;
			System.DateTime? selectedDate2 = this.calendarEndDate.SelectedDate;
			string text2 = this.txt_img.Text;
			if (string.IsNullOrEmpty(text) || text.Length > 30)
			{
				this.ShowMsg("请输入活动名称，长度不能超过30个字符！", false);
				return;
			}
			if (value.Equals("-1") && value2.Equals("-1") && value3.Equals("-1"))
			{
				this.ShowMsg("请选择会员范围！", false);
				return;
			}
			if (selectedDate2 < selectedDate)
			{
				this.ShowMsg("结束时间不能早于开始时间！", false);
				return;
			}
			if (!selectedDate.HasValue || !selectedDate2.HasValue)
			{
				this.ShowMsg("开始时间或者结束时间不能为空！", false);
				return;
			}
			if (string.IsNullOrEmpty(text2))
			{
				this.ShowMsg("请上传封面图片！", false);
				return;
			}
			PointExChangeInfo pointExChangeInfo = new PointExChangeInfo();
			if (this.eId != 0)
			{
				pointExChangeInfo = PointExChangeHelper.Get(this.eId);
			}
			pointExChangeInfo.BeginDate = selectedDate.Value;
			pointExChangeInfo.EndDate = selectedDate2.Value;
			pointExChangeInfo.Name = text;
			pointExChangeInfo.MemberGrades = value;
			pointExChangeInfo.DefualtGroup = value2;
			pointExChangeInfo.CustomGroup = value3;
			pointExChangeInfo.ImgUrl = text2;
			int num = this.eId;
			string str = "";
			if (this.eId == 0)
			{
				pointExChangeInfo.ProductNumber = 0;
				int num2 = PointExChangeHelper.Create(pointExChangeInfo, ref str);
				if (num2 == 0)
				{
					this.ShowMsg("保存失败(" + str + ")", false);
					return;
				}
				num = num2;
				this.ShowMsg("保存成功！", true);
			}
			else
			{
				bool flag = PointExChangeHelper.Update(pointExChangeInfo, ref str);
				if (!flag)
				{
					this.ShowMsg("保存失败(" + str + ")", false);
					return;
				}
				this.ShowMsg("保存成功！", true);
			}
			base.Response.Redirect("AddProductToPointExchange.aspx?id=" + num.ToString());
		}
	}
}
