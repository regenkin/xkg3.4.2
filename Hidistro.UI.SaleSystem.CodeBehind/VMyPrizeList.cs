using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class VMyPrizeList : VMemberTemplatedWebControl
	{
		private VshopTemplatedRepeater rptList;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtTotal;

		private System.Web.UI.HtmlControls.HtmlInputHidden txtShowTabNum;

		private System.Web.UI.HtmlControls.HtmlSelect txtRevAddress;

		private System.Collections.Generic.Dictionary<int, string> CouponList = new System.Collections.Generic.Dictionary<int, string>();

		private VshopTemplatedRepeater rptAddress;

		private System.Web.UI.WebControls.Literal litShipTo;

		private System.Web.UI.WebControls.Literal litAddAddress;

		private System.Web.UI.WebControls.Literal litCellPhone;

		private System.Web.UI.WebControls.Literal litAddress;

		private System.Web.UI.WebControls.Literal litShowMes;

		private System.Web.UI.HtmlControls.HtmlInputHidden selectShipTo;

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "skin-vMyPrizeList.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			PageTitle.AddSiteNameTitle("我的奖品");
			this.rptList = (VshopTemplatedRepeater)this.FindControl("rptList");
			this.txtTotal = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtTotal");
			this.txtShowTabNum = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("txtShowTabNum");
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
			}
			this.litAddAddress.Text = " href='/Vshop/AddShippingAddress.aspx?returnUrl=" + Globals.UrlEncode(System.Web.HttpContext.Current.Request.Url.ToString()) + "'";
			string text = Globals.RequestQueryStr("ShowTab");
			if (string.IsNullOrEmpty(text))
			{
				text = "0";
			}
			PrizesDeliveQuery prizesDeliveQuery = new PrizesDeliveQuery();
			int pageIndex;
			if (!int.TryParse(this.Page.Request.QueryString["page"], out pageIndex))
			{
				pageIndex = 1;
			}
			int pageSize;
			if (!int.TryParse(this.Page.Request.QueryString["size"], out pageSize))
			{
				pageSize = 20;
			}
			string extendLimits;
			if (text == "0")
			{
				string str = System.DateTime.Now.ToString("yyyy-MM-dd");
				extendLimits = " and (status in(0,1,2) or playtime>'" + str + "') ";
			}
			else
			{
				extendLimits = " and (status=3 or status=4)";
			}
			this.txtShowTabNum.Value = text;
			prizesDeliveQuery.Status = -1;
			prizesDeliveQuery.SortBy = "LogId";
			prizesDeliveQuery.SortOrder = SortAction.Desc;
			prizesDeliveQuery.PageIndex = pageIndex;
			prizesDeliveQuery.PrizeType = -1;
			prizesDeliveQuery.PageSize = pageSize;
			prizesDeliveQuery.UserId = Globals.GetCurrentMemberUserId();
			Globals.EntityCoding(prizesDeliveQuery, true);
			DbQueryResult prizesDeliveryList = GameHelper.GetPrizesDeliveryList(prizesDeliveQuery, extendLimits, "*");
			this.rptList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.refriendscircle_ItemDataBound);
			this.CouponList.Clear();
			string text2 = "";
			DataTable dataTable = (DataTable)prizesDeliveryList.Data;
			if (dataTable != null && dataTable.Rows.Count > 1)
			{
				for (int i = 0; i < dataTable.Rows.Count; i++)
				{
					if (dataTable.Rows[i]["GiveCouponId"] != System.DBNull.Value && dataTable.Rows[i]["GiveCouponId"].ToString() != "")
					{
						text2 = text2 + "," + dataTable.Rows[i]["GiveCouponId"].ToString();
					}
				}
			}
			if (text2.Length > 1)
			{
				text2 = text2.Remove(0, 1);
				int[] couponId = System.Array.ConvertAll<string, int>(text2.Split(new char[]
				{
					','
				}), (string s) => int.Parse(s));
				DataTable couponsListByIds = CouponHelper.GetCouponsListByIds(couponId);
				if (couponsListByIds != null && couponsListByIds.Rows.Count > 0)
				{
					for (int i = 0; i < couponsListByIds.Rows.Count; i++)
					{
						int key = (int)couponsListByIds.Rows[i]["CouponId"];
						if (!this.CouponList.ContainsKey(key))
						{
							this.CouponList.Add(key, couponsListByIds.Rows[i]["CouponName"].ToString() + "[面值" + couponsListByIds.Rows[i]["CouponValue"].ToString() + "元]");
						}
					}
				}
			}
			this.rptList.DataSource = prizesDeliveryList.Data;
			this.rptList.DataBind();
			this.txtTotal.SetWhenIsNotNull(prizesDeliveryList.TotalRecords.ToString());
		}

		private void refriendscircle_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Literal literal = e.Item.Controls[0].FindControl("ItemHtml") as System.Web.UI.WebControls.Literal;
				DataRowView dataRowView = (DataRowView)e.Item.DataItem;
				if (dataRowView["GiveCouponId"] != System.DBNull.Value && dataRowView["GiveCouponId"].ToString() != "")
				{
					int key = int.Parse(dataRowView["GiveCouponId"].ToString());
					if (this.CouponList.ContainsKey(key))
					{
						literal.Text = " CouponInfo='" + this.CouponList[key] + "'";
					}
					else
					{
						literal.Text = " CouponInfo=''";
					}
				}
				else
				{
					literal.Text = " CouponInfo=''";
				}
			}
		}
	}
}
