using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.hieditor.ueditor.controls;
using System;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class OfflinePay : AdminPage
	{
		protected bool _enable;

		protected bool _podenable;

		protected string _content = "";

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected ucUeditor fkContent;

		protected OfflinePay() : base("m09", "szp04")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.fkContent.Text = this.siteSettings.OffLinePayContent;
				this._podenable = this.siteSettings.EnablePodRequest;
			}
			this._enable = this.siteSettings.EnableOffLineRequest;
		}

		private void SaveData()
		{
			if (string.IsNullOrEmpty(this.fkContent.Text))
			{
				this.ShowMsg("请输入内容！", false);
			}
			this.siteSettings.OffLinePayContent = this.fkContent.Text;
			SettingsManager.Save(this.siteSettings);
			this.ShowMsgAndReUrl("保存成功", true, "OfflinePay.aspx");
		}

		protected void Unnamed_Click(object sender, System.EventArgs e)
		{
			this.SaveData();
		}
	}
}
