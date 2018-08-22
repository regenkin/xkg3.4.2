using System;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.Fenxiao
{
	public class test : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = null;
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<return_msg><!\\[CDATA\\[(?<code>(.*))\\]\\]></return_msg>");
			System.Text.RegularExpressions.Match match = regex.Match(text);
			string arg_1A_0 = string.Empty;
			if (match.Success)
			{
				text = match.Groups["code"].Value;
			}
			base.Response.Write(text);
		}
	}
}
