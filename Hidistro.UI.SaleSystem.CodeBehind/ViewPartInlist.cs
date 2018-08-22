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
	public class ViewPartInlist : VMemberTemplatedWebControl
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
				this.SkinName = "Skin-ViewPartInlist.html";
			}
			base.OnInit(e);
		}

		private void DoAction(string Action)
		{
			int num = Globals.RequestFormNum("pageIndex");
			string text = Globals.RequestFormStr("ActivityId");
			string s;
			if (num > 0 && !string.IsNullOrEmpty(text))
			{
				DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(new OneyuanTaoPartInQuery
				{
					PageIndex = num,
					PageSize = 10,
					ActivityId = text,
					IsPay = 1,
					SortBy = "Pid"
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
			PageTitle.AddSiteNameTitle("夺宝记录");
		}
	}
}
