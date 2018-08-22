using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class SetMemberRange : System.Web.UI.UserControl
	{
		private string _grade = "-1";

		private string _defualtgroup = "-1";

		private string _customgroup = "-1";

		protected System.Web.UI.WebControls.HiddenField txt_Grades;

		protected System.Web.UI.WebControls.HiddenField txt_DefualtGroup;

		protected System.Web.UI.WebControls.HiddenField txt_CustomGroup;

		public string Grade
		{
			get
			{
				return this._grade;
			}
			set
			{
				this._grade = value;
			}
		}

		public string DefualtGroup
		{
			get
			{
				return this._defualtgroup;
			}
			set
			{
				this._defualtgroup = value;
			}
		}

		public string CustomGroup
		{
			get
			{
				return this._customgroup;
			}
			set
			{
				this._customgroup = value;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
	}
}
