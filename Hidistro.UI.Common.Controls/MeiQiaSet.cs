using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class MeiQiaSet : Literal
	{
		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (masterSettings.EnableSaleService)
			{
				CustomerServiceSettings masterSettings2 = CustomerServiceManager.GetMasterSettings(false);
				string text = string.Empty;
				StringBuilder stringBuilder = new StringBuilder();
				string text2 = string.Empty;
				if (!string.IsNullOrEmpty(masterSettings2.unitid) && !string.IsNullOrEmpty(masterSettings2.unit) && !string.IsNullOrEmpty(masterSettings2.password))
				{
					text = string.Format("<script src='//meiqia.com/js/mechat.js?unitid={0}&btn=hide' charset='UTF-8' async='async'></script>", masterSettings2.unitid);
					stringBuilder.Append("<script type=\"text/javascript\">");
					stringBuilder.Append("function mechatFuc()");
					stringBuilder.Append("{");
					stringBuilder.Append("$.get(\"/Api/Hi_Ajax_OnlineServiceConfig.ashx\", function (data) {");
					stringBuilder.Append("if (data != \"\") {");
					stringBuilder.Append("$(data).appendTo('head');");
					stringBuilder.Append("}");
					stringBuilder.Append("mechatClick();");
					stringBuilder.Append("});");
					stringBuilder.Append("}");
					stringBuilder.Append("</script>");
					text2 = "<!-- 在线客服 -->\n<div class=\"customer-service\" style=\"position:fixed;bottom:100px;right:10%;width:38px;height:38px;background:url(/Utility/pics/service.png?v1026) no-repeat;background-size:100%;cursor:pointer;\" onclick=\"javascript:mechatFuc();\"></div>";
					base.Text = string.Concat(new string[]
					{
						text,
						"\n",
						stringBuilder.ToString(),
						"\n",
						text2
					});
				}
			}
			base.Render(writer);
		}
	}
}
