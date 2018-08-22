using Hidistro.Core;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VOneyuanTaoResult : VMemberTemplatedWebControl
	{
		protected string DataJson = "DataJson=null";

		private System.Web.UI.WebControls.Literal LitDataJson;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VOneyuanTaoResult.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			string text = Globals.RequestQueryStr("vaid");
			this.LitDataJson = (System.Web.UI.WebControls.Literal)this.FindControl("LitDataJson");
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
					this.LitDataJson.Text = this.DataJson;
				}
				else
				{
					this.LitDataJson.Text = this.DataJson;
				}
			}
			else
			{
				this.LitDataJson.Text = this.DataJson;
			}
			PageTitle.AddSiteNameTitle("开奖计算详情");
		}
	}
}
