using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Installer
{
	public class Activation : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.HtmlControls.HtmlInputText txtcode;

		protected System.Web.UI.WebControls.Label lblErrMessage;

		protected System.Web.UI.WebControls.Button btnInstall;

		protected void Page_Load(object sender, System.EventArgs e)
		{
		}

		internal bool CheckCode(string code)
		{
			if (string.IsNullOrEmpty(code) || code.Length != 6)
			{
				return false;
			}
			string path = System.Web.HttpContext.Current.Server.MapPath("~/config/code.db3");
			try
			{
				using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path))
				{
					string text = streamReader.ReadToEnd();
					streamReader.Close();
					return text.Contains(code);
				}
			}
			catch
			{
			}
			return false;
		}

		protected void btnInstall_Click(object sender, System.EventArgs e)
		{
			string value = this.txtcode.Value;
			if (!string.IsNullOrEmpty(value) && this.CheckCode(value))
			{
				base.Response.Redirect("Install.aspx");
				return;
			}
			this.lblErrMessage.Text = "对不起，您的激活码错误！";
		}
	}
}
