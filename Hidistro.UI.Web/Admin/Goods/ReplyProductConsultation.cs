using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class ReplyProductConsultation : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			int num = 0;
			int.TryParse(context.Request["id"].ToString(), out num);
			string text = context.Request["content"].ToString();
			if (num <= 0 || string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"type\":\"success\",\"data\":\"回复商品咨询失败\"}");
				return;
			}
			ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(num);
			productConsultation.ReplyText = text;
			productConsultation.ReplyUserId = new int?(Globals.GetCurrentManagerUserId());
			productConsultation.ReplyDate = new System.DateTime?(System.DateTime.Now);
			ValidationResults validationResults = Validation.Validate<ProductConsultationInfo>(productConsultation, new string[]
			{
				"Reply"
			});
			string text2 = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text2 += Formatter.FormatErrorMessage(current.Message);
				}
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + text2 + "\"}");
			}
			if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
			{
				context.Response.Write("{\"type\":\"success\",\"data\":\"\"}");
				return;
			}
			context.Response.Write("{\"type\":\"success\",\"data\":\"回复商品咨询失败\"}");
		}
	}
}
