using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin
{
	public class ImageReplace : AdminPage
	{
		protected System.Web.UI.WebControls.HiddenField RePlaceImg;

		protected System.Web.UI.WebControls.HiddenField RePlaceId;

		protected System.Web.UI.WebControls.FileUpload FileUpload1;

		protected System.Web.UI.WebControls.Button btnSaveImageData;

		protected ImageReplace() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack && !string.IsNullOrEmpty(this.Page.Request.QueryString["imgsrc"]) && !string.IsNullOrEmpty(this.Page.Request.QueryString["imgId"]))
			{
				string value = Globals.HtmlDecode(this.Page.Request.QueryString["imgsrc"]);
				string value2 = Globals.HtmlDecode(this.Page.Request.QueryString["imgId"]);
				this.RePlaceImg.Value = value;
				this.RePlaceId.Value = value2;
			}
			this.btnSaveImageData.Click += new System.EventHandler(this.btnSaveImageData_Click);
		}

		protected void btnSaveImageData_Click(object sender, System.EventArgs e)
		{
			string value = this.RePlaceImg.Value;
			int photoId = System.Convert.ToInt32(this.RePlaceId.Value);
			string photoPath = GalleryHelper.GetPhotoPath(photoId);
			string a = photoPath.Substring(photoPath.LastIndexOf("."));
			string b = string.Empty;
			string text = string.Empty;
			try
			{
				System.Web.HttpFileCollection files = base.Request.Files;
				System.Web.HttpPostedFile httpPostedFile = files[0];
				b = System.IO.Path.GetExtension(httpPostedFile.FileName);
				if (a != b)
				{
					this.ShowMsgToTarget("上传图片类型与原文件类型不一致！", false, "parent");
				}
				else
				{
					string str = Globals.GetStoragePath() + "/gallery";
					text = photoPath.Substring(photoPath.LastIndexOf("/") + 1);
					string text2 = value.Substring(value.LastIndexOf("/") - 6, 6);
					string text3 = string.Empty;
					if (text2.ToLower().Contains("weibo"))
					{
						text3 = Globals.GetStoragePath() + "/weibo/";
					}
					else
					{
						text3 = str + "/" + text2 + "/";
					}
					int contentLength = httpPostedFile.ContentLength;
					string path = base.Request.MapPath(text3);
					//text2 + "/" + text;
					System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path);
					if (!directoryInfo.Exists)
					{
						directoryInfo.Create();
					}
					if (!ResourcesHelper.CheckPostedFile(httpPostedFile, "image"))
					{
						this.ShowMsgToTarget("文件上传的类型不正确！", false, "parent");
					}
					else if (contentLength >= 2048000)
					{
						this.ShowMsgToTarget("图片文件已超过网站限制大小！", false, "parent");
					}
					else
					{
						httpPostedFile.SaveAs(base.Request.MapPath(text3 + text));
						GalleryHelper.ReplacePhoto(photoId, contentLength);
						this.CloseWindow();
					}
				}
			}
			catch
			{
				this.ShowMsgToTarget("替换文件错误!", false, "parent");
			}
		}

		protected virtual void CloseWindow()
		{
			string text = Globals.RequestQueryStr("reurl");
			if (string.IsNullOrEmpty(text))
			{
				text = "imagedata.aspx";
			}
			string str = "parent.location.href='" + text + "'";
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>" + str + "</script>");
			}
		}
	}
}
