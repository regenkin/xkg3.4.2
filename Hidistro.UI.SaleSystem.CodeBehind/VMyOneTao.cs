using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VMyOneTao : VMemberTemplatedWebControl
	{
		protected override void OnInit(System.EventArgs e)
		{
			string text = Globals.RequestFormStr("action");
			if (!string.IsNullOrEmpty(text))
			{
				this.DoAction(text);
				this.Page.Response.End();
			}
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-VMyOneTao.html";
			}
			base.OnInit(e);
		}

		private void DoAction(string Action)
		{
			int num = Globals.RequestFormNum("pageIndex");
			string s;
			if (num > 0)
			{
				DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(new OneyuanTaoPartInQuery
				{
					PageIndex = num,
					PageSize = 10,
					ActivityId = "",
					UserId = Globals.GetCurrentMemberUserId(),
					state = 1,
					SortBy = "BuyTime",
					IsPay = -1
				});
				IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
				isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
				string str = JsonConvert.SerializeObject(oneyuanPartInDataTable.Data, new JsonConverter[]
				{
					isoDateTimeConverter
				});
				s = "{\"state\":true,\"msg\":\"读取成功\",\"Data\":" + str + "}";
			}
			else
			{
				s = "{\"state\":false,\"msg\":\"参数不正确\"}";
			}
			this.Page.Response.ClearContent();
			this.Page.Response.ContentType = "application/json";
			this.Page.Response.Write(s);
			this.Page.Response.End();
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("我的夺宝");
		}
	}
}
