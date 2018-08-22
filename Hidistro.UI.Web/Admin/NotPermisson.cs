using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class NotPermisson : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.Literal litMsg;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindDate();
			}
		}

		private void BindDate()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = base.Request.QueryString["type"];
			if (string.IsNullOrEmpty(text))
			{
				text = "1";
			}
			string a;
			if ((a = text) != null)
			{
				if (!(a == "1"))
				{
					if (!(a == "2"))
					{
						if (a == "3")
						{
							stringBuilder.Append("<li>系统出错了！</li>");
						}
					}
					else
					{
						stringBuilder.Append("<li>您没有访问该页面的权限！</li>");
					}
				}
				else
				{
					stringBuilder.Append("<li>操作出错了！</li>");
				}
			}
			string text2 = base.Request.QueryString["msg"];
			if (!string.IsNullOrEmpty(text2))
			{
				stringBuilder.AppendFormat("<li>{0}</li>", text2);
			}
			this.litMsg.Text = stringBuilder.ToString();
		}
	}
}
