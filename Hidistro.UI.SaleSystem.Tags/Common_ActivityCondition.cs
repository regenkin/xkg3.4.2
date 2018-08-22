using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_ActivityCondition : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			int num;
			int.TryParse(HttpContext.Current.Request.QueryString.Get("id"), out num);
			ActivityInfo activity = VshopBrowser.GetActivity(num);
			if (activity == null)
			{
				return;
			}
			string format = "<div class=\"qb_mb10 qb_flex\"><input type=\"text\" class=\"mod_input flex_box\" style=\"height:35px;\" placeholder=\"您的{0}\" id=\"{1}\"></div>";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<input type=\"hidden\" id=\"id\" value=\"{0}\">", num);
			if (!string.IsNullOrEmpty(activity.Item1))
			{
				stringBuilder.AppendFormat(format, activity.Item1, "item1");
			}
			if (!string.IsNullOrEmpty(activity.Item2))
			{
				stringBuilder.AppendFormat(format, activity.Item2, "item2");
			}
			if (!string.IsNullOrEmpty(activity.Item3))
			{
				stringBuilder.AppendFormat(format, activity.Item3, "item3");
			}
			if (!string.IsNullOrEmpty(activity.Item4))
			{
				stringBuilder.AppendFormat(format, activity.Item4, "item4");
			}
			if (!string.IsNullOrEmpty(activity.Item5))
			{
				stringBuilder.AppendFormat(format, activity.Item5, "item5");
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
