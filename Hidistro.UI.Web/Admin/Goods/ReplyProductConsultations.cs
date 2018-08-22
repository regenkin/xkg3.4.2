using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class ReplyProductConsultations : AdminPage
	{
		private int consultationId;

		protected System.Web.UI.WebControls.Literal litUserName;

		protected FormatedTimeLabel lblTime;

		protected System.Web.UI.WebControls.Literal litConsultationText;

		protected ucUeditor fckReplyText;

		protected System.Web.UI.WebControls.Button btnReplyProductConsultation;

		protected ReplyProductConsultations() : base("m02", "spp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["ConsultationId"], out this.consultationId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyProductConsultation.Click += new System.EventHandler(this.btnReplyProductConsultation_Click);
			if (!this.Page.IsPostBack)
			{
				ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
				if (productConsultation == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litUserName.Text = productConsultation.UserName;
				this.litConsultationText.Text = productConsultation.ConsultationText;
				this.lblTime.Time = productConsultation.ConsultationDate;
			}
		}

		protected void btnReplyProductConsultation_Click(object sender, System.EventArgs e)
		{
			ProductConsultationInfo productConsultation = ProductCommentHelper.GetProductConsultation(this.consultationId);
			if (string.IsNullOrEmpty(this.fckReplyText.Text))
			{
				productConsultation.ReplyText = null;
			}
			else
			{
				productConsultation.ReplyText = this.fckReplyText.Text;
			}
			productConsultation.ReplyUserId = new int?(Globals.GetCurrentManagerUserId());
			productConsultation.ReplyDate = new System.DateTime?(System.DateTime.Now);
			ValidationResults validationResults = Validation.Validate<ProductConsultationInfo>(productConsultation, new string[]
			{
				"Reply"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			if (ProductCommentHelper.ReplyProductConsultation(productConsultation))
			{
				this.fckReplyText.Text = string.Empty;
				this.ShowMsg("回复商品咨询成功", true);
				base.Response.Redirect("ProductConsultations.aspx");
				return;
			}
			this.ShowMsg("回复商品咨询失败", false);
		}
	}
}
