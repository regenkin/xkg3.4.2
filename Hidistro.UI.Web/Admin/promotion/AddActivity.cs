using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class AddActivity : AdminPage
	{
		protected ActivityInfo _act = new ActivityInfo();

		protected int _id;

		protected int IsView;

		protected string _json = "";

		protected bool isAllProduct;

		protected bool hasPartProductAct;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected SetMemberRange SetMemberRange;

		protected System.Web.UI.WebControls.DropDownList ddl_maxNum;

		protected System.Web.UI.HtmlControls.HtmlGenericControl productCount;

		protected AddActivity() : base("m08", "yxp05")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				for (int i = 1; i <= 10; i++)
				{
					this.ddl_maxNum.Items.Add(new System.Web.UI.WebControls.ListItem("每人参与" + i.ToString() + "次", i.ToString()));
				}
				this.ddl_maxNum.Items.Add(new System.Web.UI.WebControls.ListItem("不限", "0"));
				this.hasPartProductAct = ActivityHelper.HasPartProductAct();
			}
			this.IsView = Globals.RequestQueryNum("View");
			string[] allKeys = base.Request.Params.AllKeys;
			if (allKeys.Contains("id") && base.Request["id"].ToString().bInt(ref this._id))
			{
				this._act = ActivityHelper.GetAct(this._id);
				if (this._act == null)
				{
					this.ShowMsg("没有这个满减活动~", false);
				}
				if (this._act.StartTime < System.DateTime.Now)
				{
					this.IsView = 1;
				}
				this.txt_name.Text = this._act.ActivitiesName;
				this.calendarStartDate.SelectedDate = new System.DateTime?(this._act.StartTime);
				this.calendarEndDate.SelectedDate = new System.DateTime?(this._act.EndTime);
				this.ddl_maxNum.SelectedValue = this._act.attendTime.ToString();
				this._json = JsonConvert.SerializeObject(this._act.Details);
				this.isAllProduct = this._act.isAllProduct;
				System.Web.UI.WebControls.HiddenField hiddenField = this.SetMemberRange.FindControl("txt_Grades") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField2 = this.SetMemberRange.FindControl("txt_DefualtGroup") as System.Web.UI.WebControls.HiddenField;
				System.Web.UI.WebControls.HiddenField hiddenField3 = this.SetMemberRange.FindControl("txt_CustomGroup") as System.Web.UI.WebControls.HiddenField;
				this.SetMemberRange.Grade = this._act.MemberGrades;
				this.SetMemberRange.DefualtGroup = this._act.DefualtGroup;
				this.SetMemberRange.CustomGroup = this._act.CustomGroup;
				hiddenField.Value = this._act.MemberGrades;
				hiddenField2.Value = this._act.DefualtGroup;
				hiddenField3.Value = this._act.CustomGroup;
				System.Data.DataTable dataTable = ActivityHelper.QueryProducts(this._id);
				this.productCount.InnerText = ((dataTable != null) ? dataTable.Rows.Count.ToString() : "0");
			}
		}
	}
}
