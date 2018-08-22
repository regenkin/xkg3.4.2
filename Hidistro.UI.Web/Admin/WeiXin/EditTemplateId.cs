using Hidistro.ControlPanel.Store;
using Hidistro.Entities.VShop;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	public class EditTemplateId : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtTemplateId;

		protected System.Web.UI.WebControls.Button btnSaveEmailTemplet;

		protected EditTemplateId() : base("m06", "wxp06")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSaveEmailTemplet.Click += new System.EventHandler(this.btnSaveEmailTemplet_Click);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.InitShow();
			}
		}

		private void btnSaveEmailTemplet_Click(object sender, System.EventArgs e)
		{
			string text = this.txtTemplateId.Text.Trim();
			string messageType = base.Request["MessageType"];
			MessageTemplate messageTemplate = VShopHelper.GetMessageTemplate(messageType);
			messageTemplate.WeixinTemplateId = text;
			if (string.IsNullOrEmpty(text))
			{
				messageTemplate.SendWeixin = false;
			}
			else
			{
				messageTemplate.SendWeixin = true;
			}
			try
			{
				VShopHelper.UpdateTemplate(messageTemplate);
				this.ShowMsgAndReUrl("保存模板Id成功", true, "messagetemplets.aspx");
			}
			catch
			{
				this.ShowMsg("保存模板Id失败", false);
			}
		}

		private void InitShow()
		{
			string messageType = base.Request["MessageType"];
			MessageTemplate messageTemplate = VShopHelper.GetMessageTemplate(messageType);
			this.txtTemplateId.Text = messageTemplate.WeixinTemplateId;
		}
	}
}
