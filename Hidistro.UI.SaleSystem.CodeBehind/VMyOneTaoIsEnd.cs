using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VMyOneTaoIsEnd : VMemberTemplatedWebControl
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
				this.SkinName = "Skin-VMyOneTaoIsEnd.html";
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
					state = 2,
					SortBy = "BuyTime",
					IsPay = -1
				});
				DataTable dataTable = new DataTable();
				IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
				isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
				if (oneyuanPartInDataTable.Data != null)
				{
					dataTable = (DataTable)oneyuanPartInDataTable.Data;
					dataTable = (DataTable)oneyuanPartInDataTable.Data;
					dataTable.Columns.Add("LuckNumList");
					dataTable.Columns.Add("AState");
					foreach (DataRow dataRow in dataTable.Rows)
					{
						System.Collections.Generic.IList<LuckInfo> winnerLuckInfoList = OneyuanTaoHelp.getWinnerLuckInfoList(dataRow["ActivityId"].ToString(), "");
						if (winnerLuckInfoList != null)
						{
							dataRow["LuckNumList"] = JsonConvert.SerializeObject(winnerLuckInfoList, new JsonConverter[]
							{
								isoDateTimeConverter
							});
						}
						OneyuanTaoInfo oneyuanTaoInfo = OneyuanTaoHelp.DataRowToOneyuanTaoInfo(dataRow);
						OneTaoPrizeState prizeState = OneyuanTaoHelp.getPrizeState(oneyuanTaoInfo);
						dataRow["AState"] = prizeState;
						if (oneyuanTaoInfo.IsSuccess)
						{
							dataRow["AState"] = "已开奖";
						}
						else if (oneyuanTaoInfo.HasCalculate)
						{
							dataRow["AState"] = "已结束（开奖失败）";
						}
					}
				}
				string str = JsonConvert.SerializeObject(dataTable, new JsonConverter[]
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
