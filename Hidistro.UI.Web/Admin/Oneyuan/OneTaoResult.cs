using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.ControlPanel.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Oneyuan
{
	public class OneTaoResult : AdminPage
	{
		protected string DataJson = "DataJson=null";

		protected System.Web.UI.HtmlControls.HtmlGenericControl txtEditInfo;

		protected OneTaoViewTab ViewTab1;

		protected OneTaoResult() : base("m08", "yxp20")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.QueryString["vaid"];
			if (!string.IsNullOrEmpty(text))
			{
				OneyuanTaoInfo oneyuanTaoInfoById = OneyuanTaoHelp.GetOneyuanTaoInfoById(text);
				if (oneyuanTaoInfoById != null)
				{
					string prizeCountInfo = oneyuanTaoInfoById.PrizeCountInfo;
					oneyuanTaoInfoById.PrizeCountInfo = "";
					IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
					isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
					this.DataJson = "DataJson=" + JsonConvert.SerializeObject(oneyuanTaoInfoById, new JsonConverter[]
					{
						isoDateTimeConverter
					});
					System.Collections.Generic.IList<LuckInfo> luckInfoList = OneyuanTaoHelp.getLuckInfoList(true, text);
					string str = JsonConvert.SerializeObject(luckInfoList, new JsonConverter[]
					{
						isoDateTimeConverter
					});
					if (!string.IsNullOrEmpty(prizeCountInfo))
					{
						this.DataJson = this.DataJson + ";\r\n PrizeCountInfo=" + prizeCountInfo;
					}
					if (luckInfoList.Count > 0)
					{
						this.DataJson = this.DataJson + ";\r\nWinInfo=" + str;
					}
				}
			}
		}
	}
}
