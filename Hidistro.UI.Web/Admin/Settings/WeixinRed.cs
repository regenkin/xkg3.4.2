using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class WeixinRed : AdminPage
	{
		protected bool _enable;

		private string _dataPath = "~/Pay/Cert";

		protected Script Script1;

		protected Script Script5;

		protected Script Script7;

		protected Script Script6;

		protected Script Script4;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.FileUpload fileUploader;

		protected System.Web.UI.HtmlControls.HtmlGenericControl labfilename;

		protected System.Web.UI.WebControls.TextBox txt_key;

		protected WeixinRed() : base("m06", "wxp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				this._enable = masterSettings.EnableWeixinRed;
				this.txt_key.Text = masterSettings.WeixinCertPassword;
				this.labfilename.InnerText = ((masterSettings.WeixinCertPath != "") ? "已上传" : "");
			}
		}

		private bool IsAllowableFileType(string FileName)
		{
			string text = ".p12";
			return text.IndexOf(System.IO.Path.GetExtension(FileName).ToLower()) != -1;
		}

		protected void Unnamed_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			string path = this.Page.Request.MapPath(this._dataPath);
			masterSettings.WeixinCertPassword = this.txt_key.Text;
			if (this.fileUploader.PostedFile.FileName != "")
			{
				if (!this.IsAllowableFileType(this.fileUploader.PostedFile.FileName))
				{
					this.ShowMsg("请上传正确的文件", false);
					return;
				}
				string path2 = System.DateTime.Now.ToString("yyyyMMddhhmmss") + System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
				this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(path, path2));
				if (masterSettings.WeixinCertPath != "")
				{
					System.IO.File.Delete(masterSettings.WeixinCertPath);
				}
				masterSettings.WeixinCertPath = System.IO.Path.Combine(path, path2);
			}
			else if (string.IsNullOrEmpty(masterSettings.WeixinCertPath))
			{
				this.ShowMsg("请上传正确的文件", false);
				return;
			}
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true);
		}
	}
}
