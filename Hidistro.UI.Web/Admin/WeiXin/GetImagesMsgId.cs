using ControlPanel.WeiBo;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hishop.Weixin.MP.Api;
using Hishop.Weixin.MP.Domain;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	public class GetImagesMsgId : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = ArticleHelper.GetNoImgMsgIdArticleList();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string text = TokenApi.GetToken(masterSettings.WeixinAppId, masterSettings.WeixinAppSecret);
			text = JsonConvert.DeserializeObject<Token>(text).access_token;
			if (dataTable.Rows.Count > 0)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					string text2 = NewsApi.GetMedia_IDByPath(text, dataTable.Rows[i]["ImageUrl"].ToString());
					text2 = NewsApi.GetJsonValue(text2, "media_id");
					if (!string.IsNullOrEmpty(text2))
					{
						ArticleHelper.UpdateMedia_Id(0, Globals.ToNum(dataTable.Rows[i]["ArticleId"].ToString()), text2);
					}
				}
			}
			dataTable = ArticleHelper.GetNoImgMsgIdArticleItemList();
			if (dataTable.Rows.Count > 0)
			{
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					string text3 = NewsApi.GetMedia_IDByPath(text, dataTable.Rows[j]["ImageUrl"].ToString());
					text3 = NewsApi.GetJsonValue(text3, "media_id");
					if (!string.IsNullOrEmpty(text3))
					{
						ArticleHelper.UpdateMedia_Id(1, Globals.ToNum(dataTable.Rows[j]["ID"].ToString()), text3);
					}
				}
			}
			System.Web.HttpContext.Current.Response.Write("document.write('');");
			System.Web.HttpContext.Current.Response.End();
		}
	}
}
