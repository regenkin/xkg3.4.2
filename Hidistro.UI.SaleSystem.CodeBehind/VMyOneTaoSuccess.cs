using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class VMyOneTaoSuccess : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptAddress;

		private System.Web.UI.WebControls.Literal litShipTo;

		private System.Web.UI.WebControls.Literal litAddAddress;

		private System.Web.UI.WebControls.Literal litCellPhone;

		private System.Web.UI.WebControls.Literal litAddress;

		private System.Web.UI.WebControls.Literal litShowMes;

		private System.Web.UI.HtmlControls.HtmlInputHidden selectShipTo;

		private System.Web.UI.HtmlControls.HtmlInputHidden regionId;

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
				this.SkinName = "Skin-VMyOneTaoSuccess.html";
			}
			base.OnInit(e);
		}

		private void DoAction(string Action)
		{
			int num = Globals.RequestFormNum("pageIndex");
			string s;
			if (num > 0)
			{
				int userid = Globals.GetCurrentMemberUserId();
				DbQueryResult oneyuanPartInDataTable = OneyuanTaoHelp.GetOneyuanPartInDataTable(new OneyuanTaoPartInQuery
				{
					PageIndex = num,
					PageSize = 10,
					ActivityId = "",
					UserId = Globals.GetCurrentMemberUserId(),
					state = 3,
					SortBy = "BuyTime",
					IsPay = -1
				});
				DataTable dataTable = new DataTable();
				if (oneyuanPartInDataTable.Data != null)
				{
					dataTable = (DataTable)oneyuanPartInDataTable.Data;
					dataTable.Columns.Add("LuckNumList");
					dataTable.Columns.Add("PostSate");
					dataTable.Columns.Add("PostBtn");
					dataTable.Columns.Add("tabid");
					System.Collections.IEnumerator enumerator = dataTable.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DataRow Item = (DataRow)enumerator.Current;
							System.Collections.Generic.IList<LuckInfo> list = OneyuanTaoHelp.getLuckInfoList(true, Item["ActivityId"].ToString());
							list = (from t in list
							where t.UserId == userid && t.Pid == Item["Pid"].ToString()
							select t).ToList<LuckInfo>();
							Item["PostBtn"] = "0";
							Item["tabid"] = "0";
							if (list != null)
							{
								System.Collections.Generic.List<string> list2 = new System.Collections.Generic.List<string>();
								foreach (LuckInfo current in list)
								{
									list2.Add(current.PrizeNum);
								}
								Item["LuckNumList"] = string.Join(",", list2);
								DataTable dataTable2 = OneyuanTaoHelp.PrizesDeliveryRecord(Item["Pid"].ToString());
								if (dataTable2 == null || dataTable2.Rows.Count == 0)
								{
									Item["PostSate"] = "收货地址未确认";
								}
								else
								{
									Item["PostSate"] = OneyuanTaoHelp.GetPrizesDeliveStatus(dataTable2.Rows[0]["status"].ToString());
									Item["PostBtn"] = dataTable2.Rows[0]["status"].ToString();
									Item["tabid"] = dataTable2.Rows[0]["Id"].ToString();
								}
							}
						}
					}
					finally
					{
						System.IDisposable disposable = enumerator as System.IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
				}
				string str = JsonConvert.SerializeObject(dataTable, new JsonConverter[]
				{
					new IsoDateTimeConverter
					{
						DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
					}
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
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litCellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litCellPhone");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.rptAddress = (VshopTemplatedRepeater)this.FindControl("rptAddress");
			System.Collections.Generic.IList<ShippingAddressInfo> shippingAddresses = MemberProcessor.GetShippingAddresses();
			this.rptAddress.DataSource = from item in shippingAddresses
			orderby item.IsDefault
			select item;
			this.rptAddress.DataBind();
			this.litAddAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddAddress");
			this.selectShipTo = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("selectShipTo");
			this.regionId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("regionId");
			ShippingAddressInfo shippingAddressInfo = shippingAddresses.FirstOrDefault((ShippingAddressInfo item) => item.IsDefault);
			if (shippingAddressInfo == null)
			{
				shippingAddressInfo = ((shippingAddresses.Count > 0) ? shippingAddresses[0] : null);
			}
			if (shippingAddressInfo != null)
			{
				this.litShipTo.Text = shippingAddressInfo.ShipTo;
				this.litCellPhone.Text = shippingAddressInfo.CellPhone;
				this.litAddress.Text = shippingAddressInfo.Address;
				this.selectShipTo.SetWhenIsNotNull(shippingAddressInfo.ShippingId.ToString());
				this.regionId.SetWhenIsNotNull(shippingAddressInfo.RegionId.ToString());
			}
			this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()) + "'";
			PageTitle.AddSiteNameTitle("中奖记录");
		}
	}
}
