using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using System;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Web.Admin.Settings.flex
{
	public class GetPrintData : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["shipperId"];
			string text2 = base.Request.Form["orderId"];
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
			{
				int shipperId = 0;
				if (int.TryParse(text, out shipperId))
				{
					ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
					if (shipper != null)
					{
						OrderInfo orderInfo = OrderHelper.GetOrderInfo(text2);
						if (orderInfo != null)
						{
							this.WriteOrderInfo(orderInfo, shipper);
						}
					}
				}
			}
		}

		private void WriteOrderInfo(OrderInfo order, ShippersInfo shipper)
		{
			string fullRegion = RegionHelper.GetFullRegion(order.RegionId, ",");
			string[] array = fullRegion.Split(new char[]
			{
				','
			});
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<nodes>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-姓名</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.ShipTo);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-电话</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.TelPhone + "_");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-手机</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.CellPhone + "_");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-邮编</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.ZipCode + "_");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地址</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.Address);
			stringBuilder.AppendLine("</item>");
			if (array.Length > 0)
			{
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>收货人-地区1级</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", array[0]);
				stringBuilder.AppendLine("</item>");
			}
			if (array.Length > 1)
			{
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>收货人-地区2级</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", array[1]);
				stringBuilder.AppendLine("</item>");
			}
			if (array.Length > 2)
			{
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>收货人-地区3级</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", array[2]);
				stringBuilder.AppendLine("</item>");
			}
			if (shipper != null)
			{
				string fullRegion2 = RegionHelper.GetFullRegion(shipper.RegionId, ",");
				string[] array2 = fullRegion2.Split(new char[]
				{
					','
				});
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>发货人-姓名</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", shipper.ShipperName);
				stringBuilder.AppendLine("</item>");
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>发货人-手机</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", shipper.CellPhone + "_");
				stringBuilder.AppendLine("</item>");
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>发货人-电话</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", shipper.TelPhone + "_");
				stringBuilder.AppendLine("</item>");
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>发货人-地址</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", shipper.Address);
				stringBuilder.AppendLine("</item>");
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>发货人-邮编</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", shipper.Zipcode + "_");
				stringBuilder.AppendLine("</item>");
				if (array2.Length > 0)
				{
					stringBuilder.AppendLine("<item>");
					stringBuilder.AppendLine("<name>发货人-地区1级</name>");
					stringBuilder.AppendFormat("<rename>{0}</rename>", array2[0]);
					stringBuilder.AppendLine("</item>");
				}
				if (array2.Length > 1)
				{
					stringBuilder.AppendLine("<item>");
					stringBuilder.AppendLine("<name>发货人-地区2级</name>");
					stringBuilder.AppendFormat("<rename>{0}</rename>", array2[1]);
					stringBuilder.AppendLine("</item>");
				}
				if (array2.Length > 2)
				{
					stringBuilder.AppendLine("<item>");
					stringBuilder.AppendLine("<name>发货人-地区3级</name>");
					stringBuilder.AppendFormat("<rename>{0}</rename>", array2[2]);
					stringBuilder.AppendLine("</item>");
				}
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-订单号</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.OrderId);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-总金额</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.GetTotal() + "_");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-物品总重量</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.Weight);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-备注</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order.ManagerRemark);
			stringBuilder.AppendLine("</item>");
			string text = "";
			if (order.LineItems != null && order.LineItems.Count > 0)
			{
				foreach (LineItemInfo current in order.LineItems.Values)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"货号 ",
						current.SKU,
						" ",
						current.SKUContent,
						" ×",
						current.ShipmentQuantity,
						"\n"
					});
				}
				text = text.Replace("；", "").Replace(";", "").Replace("：", ":");
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-详情</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", text);
			stringBuilder.AppendLine("</item>");
			if (order.ShippingDate == System.DateTime.Parse("0001-1-1"))
			{
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>订单-送货时间</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", "null");
				stringBuilder.AppendLine("</item>");
			}
			else
			{
				stringBuilder.AppendLine("<item>");
				stringBuilder.AppendLine("<name>订单-送货时间</name>");
				stringBuilder.AppendFormat("<rename>{0}</rename>", order.ShippingDate);
				stringBuilder.AppendLine("</item>");
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>网店名称</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", SettingsManager.GetMasterSettings(true).SiteName);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>自定义内容</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", "null");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("</nodes>");
			base.Response.Write(stringBuilder.ToString());
		}
	}
}
