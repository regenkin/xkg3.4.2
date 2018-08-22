using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.WeiXin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class RedpackSetting : AdminPage
	{
		protected bool enableWXRequest;

		private string _dataPath;

		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(false);

		private string action = Globals.RequestFormStr("action");

		protected System.Web.UI.WebControls.FileUpload fileUploader;

		protected System.Web.UI.HtmlControls.HtmlGenericControl labfilename;

		protected System.Web.UI.WebControls.TextBox txtCertPassword;

		protected System.Web.UI.WebControls.Button btnOK;

		protected RedpackSetting() : base("m06", "wxp09")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this._dataPath = this.Page.Request.MapPath("~/Pay/Cert");
			if (!base.IsPostBack)
			{
				if (this.action == "setenable")
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					string s = "{\"type\":\"1\",\"tips\":\"操作成功！\"}";
					try
					{
						this.siteSettings.EnableWeiXinRequest = (Globals.RequestFormNum("enable") == 1);
						SettingsManager.Save(this.siteSettings);
					}
					catch
					{
						s = "{\"type\":\"0\",\"tips\":\"操作失败！\"}";
					}
					base.Response.Write(s);
					base.Response.End();
				}
				this.labfilename.InnerText = ((this.siteSettings.WeixinCertPath != "") ? ("已上传：" + this.siteSettings.WeixinCertPath.Substring(this.siteSettings.WeixinCertPath.LastIndexOf("\\") + 1, this.siteSettings.WeixinCertPath.Length - this.siteSettings.WeixinCertPath.LastIndexOf("\\") - 1)) : "");
				this.txtCertPassword.Text = this.siteSettings.WeixinCertPassword;
			}
			this.enableWXRequest = this.siteSettings.EnableWeiXinRequest;
		}

		protected bool IsAllowableFileType(string FileName)
		{
			string text = ".p12";
			return text.IndexOf(System.IO.Path.GetExtension(FileName).ToLower()) != -1;
		}

		protected void btnOK_Click(object sender, System.EventArgs e)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			if (this.fileUploader.PostedFile.FileName != "")
			{
				if (!this.IsAllowableFileType(this.fileUploader.PostedFile.FileName))
				{
					this.ShowMsg("请上传正确的文件", false);
					return;
				}
				string path = System.DateTime.Now.ToString("yyyyMMddhhmmss") + System.IO.Path.GetFileName(this.fileUploader.PostedFile.FileName);
				this.fileUploader.PostedFile.SaveAs(System.IO.Path.Combine(this._dataPath, path));
				masterSettings.WeixinCertPath = System.IO.Path.Combine(this._dataPath, path);
			}
			masterSettings.WeixinCertPassword = this.txtCertPassword.Text;
			SettingsManager.Save(masterSettings);
			this.ShowMsg("设置成功", true);
		}
	}
}
