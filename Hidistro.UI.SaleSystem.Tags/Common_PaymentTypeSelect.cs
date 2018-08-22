using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_PaymentTypeSelect : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			IList<PaymentModeInfo> paymentModes = ShoppingProcessor.GetPaymentModes();
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<button type=\"button\" class=\"btn btn-default dropdown-toggle\" data-toggle=\"dropdown\">请选择一种支付方式<span class=\"caret\"></span></button>");
			stringBuilder.AppendLine("<ul id=\"selectPaymentType\" class=\"dropdown-menu\" role=\"menu\">");
			string userAgent = this.Page.Request.UserAgent;
			if (masterSettings.EnableWeiXinRequest && userAgent.ToLower().Contains("micromessenger") && masterSettings.IsValidationService)
			{
				stringBuilder.AppendLine("<li><a href=\"#\" name=\"88\">微信支付</a></li>");
			}
			if (Globals.RequestQueryNum("bargainDetialId") <= 0)
			{
				if (masterSettings.EnableOffLineRequest)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"99\">线下付款</a></li>");
				}
				if (masterSettings.EnablePodRequest)
				{
					stringBuilder.AppendLine("<li><a href=\"#\" name=\"0\">货到付款</a></li>");
				}
			}
			if (paymentModes != null && paymentModes.Count > 0)
			{
				foreach (PaymentModeInfo current in paymentModes)
				{
					string xml = HiCryptographer.Decrypt(current.Settings);
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(xml);
					XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Partner");
					if (elementsByTagName.Count != 0)
					{
						if ((!userAgent.ToLower().Contains("micromessenger") || !masterSettings.IsValidationService) && masterSettings.EnableAlipayRequest && !string.IsNullOrEmpty(xmlDocument.GetElementsByTagName("Partner")[0].InnerText) && !string.IsNullOrEmpty(xmlDocument.GetElementsByTagName("Key")[0].InnerText) && !string.IsNullOrEmpty(xmlDocument.GetElementsByTagName("Seller_account_name")[0].InnerText))
						{
							stringBuilder.AppendFormat("<li><a href=\"#\" name=\"{0}\">{1}</a></li>", current.ModeId, current.Name).AppendLine();
						}
					}
					else if (masterSettings.EnableWapShengPay && !string.IsNullOrEmpty(xmlDocument.GetElementsByTagName("SenderId")[0].InnerText) && !string.IsNullOrEmpty(xmlDocument.GetElementsByTagName("SellerKey")[0].InnerText))
					{
						stringBuilder.AppendFormat("<li><a href=\"#\" name=\"{0}\">{1}</a></li>", current.ModeId, current.Name).AppendLine();
					}
				}
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
