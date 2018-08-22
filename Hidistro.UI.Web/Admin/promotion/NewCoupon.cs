using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Ascx;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class NewCoupon : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.TextBox txt_name;

		protected System.Web.UI.WebControls.TextBox txt_Value;

		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdt_1;

		protected System.Web.UI.HtmlControls.HtmlInputRadioButton rdt_2;

		protected System.Web.UI.WebControls.TextBox txt_conditonVal;

		protected ucDateTimePicker calendarStartDate;

		protected ucDateTimePicker calendarEndDate;

		protected System.Web.UI.WebControls.TextBox txt_totalNum;

		protected SetMemberRange SetMemberRange;

		protected System.Web.UI.WebControls.DropDownList ddl_maxNum;

		protected NewCoupon() : base("m08", "yxp01")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				for (int i = 1; i <= 10; i++)
				{
					this.ddl_maxNum.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString() + "张", i.ToString()));
				}
			}
		}
	}
}
