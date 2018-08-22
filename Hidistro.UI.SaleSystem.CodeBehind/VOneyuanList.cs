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
	public class VOneyuanList : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater repList;

		private int Otype = 1;

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
				this.SkinName = "Skin-OneyuanList.html";
			}
			base.OnInit(e);
		}

		private void DoAction(string Action)
		{
			string s = "{\"state\":false,\"msg\":\"未定义操作\"}";
			int num = Globals.RequestFormNum("pageIndex");
			if (num > 0)
			{
				this.Otype = Globals.RequestQueryNum("Otype");
				OneyuanTaoQuery oneyuanTaoQuery = new OneyuanTaoQuery();
				oneyuanTaoQuery.PageIndex = num;
				oneyuanTaoQuery.PageSize = 10;
				oneyuanTaoQuery.title = "";
				oneyuanTaoQuery.state = 1;
				oneyuanTaoQuery.ReachType = 0;
				if (this.Otype == 0)
				{
					this.Otype = 1;
				}
				if (this.Otype == 3)
				{
					oneyuanTaoQuery.SortBy = "(FinishedNum*1.0/ReachNum)";
				}
				else if (this.Otype == 2)
				{
					oneyuanTaoQuery.SortBy = "ActivityId";
				}
				else if (this.Otype == 4)
				{
					oneyuanTaoQuery.state = 3;
					oneyuanTaoQuery.SortBy = "ActivityId";
				}
				else
				{
					this.Otype = 1;
					oneyuanTaoQuery.SortBy = "FinishedNum";
				}
				DbQueryResult oneyuanTao = OneyuanTaoHelp.GetOneyuanTao(oneyuanTaoQuery);
				IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
				isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
				string str = JsonConvert.SerializeObject(oneyuanTao.Data, new JsonConverter[]
				{
					isoDateTimeConverter
				});
				s = "{\"state\":true,\"msg\":\"读取成功\",\"Data\":" + str + "}";
			}
			this.Page.Response.ClearContent();
			this.Page.Response.ContentType = "application/json";
			this.Page.Response.Write(s);
			this.Page.Response.End();
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("一元夺宝");
		}
	}
}
