using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_MemberCardCondition : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			StringBuilder stringBuilder = new StringBuilder();
			if (masterSettings.VipRequireName)
			{
				stringBuilder.Append("<input id=\"txtName\" type=\"text\" class=\"mod_input\" placeholder=\"您的姓名\">");
			}
			if (masterSettings.VipRequireMobile)
			{
				stringBuilder.Append("<input id=\"txtPhone\" type=\"tel\" class=\"mod_input\" placeholder=\"您的联系电话\">");
			}
			if (masterSettings.VipRequireQQ)
			{
				stringBuilder.Append("<input id=\"txtQQ\" type=\"tel\" class=\"mod_input\" placeholder=\"您的QQ号码\">");
			}
			if (masterSettings.VipRequireAdress)
			{
				stringBuilder.Append("<input id=\"txtAddress\" type=\"text\" class=\"mod_input\" placeholder=\"您的联系地址\">");
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
