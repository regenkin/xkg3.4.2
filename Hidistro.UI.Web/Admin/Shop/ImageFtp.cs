using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ImageFtp : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected ImageDataGradeDropDownList dropImageFtp;

		protected System.Web.UI.WebControls.FileUpload FileUpload;

		protected System.Web.UI.WebControls.Button btnSaveImageFtp;

		protected ImageFtp() : base("m01", "dpp08")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveImageFtp.Click += new System.EventHandler(this.btnSaveImageFtp_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropImageFtp.DataBind();
			}
		}

		private void btnSaveImageFtp_Click(object sender, System.EventArgs e)
		{
			string str = Globals.GetStoragePath() + "/gallery";
			int categoryId = System.Convert.ToInt32(this.dropImageFtp.SelectedItem.Value);
			int num = 0;
			int num2 = 0;
			new System.Text.StringBuilder();
			System.Web.HttpFileCollection files = base.Request.Files;
			for (int i = 0; i < files.Count; i++)
			{
				System.Web.HttpPostedFile httpPostedFile = files[i];
				if (httpPostedFile.ContentLength > 0)
				{
					num++;
					try
					{
						string text = System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + System.IO.Path.GetExtension(httpPostedFile.FileName);
						string text2 = httpPostedFile.FileName.Substring(httpPostedFile.FileName.LastIndexOf("\\") + 1);
						string photoName = text2.Substring(0, text2.LastIndexOf("."));
						string text3 = System.DateTime.Now.ToString("yyyyMM").Substring(0, 6);
						string text4 = str + "/" + text3 + "/";
						int contentLength = httpPostedFile.ContentLength;
						string path = base.Request.MapPath(text4);
						string photoPath = "/Storage/master/gallery/" + text3 + "/" + text;
						System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path);
						if (ResourcesHelper.CheckPostedFile(httpPostedFile, "image"))
						{
							if (!directoryInfo.Exists)
							{
								directoryInfo.Create();
							}
							httpPostedFile.SaveAs(base.Request.MapPath(text4 + text));
							if (GalleryHelper.AddPhote(categoryId, photoName, photoPath, contentLength))
							{
								num2++;
							}
						}
					}
					catch
					{
					}
				}
			}
			if (num == 0)
			{
				this.ShowMsg("至少需要选择一个图片文件！", false);
				return;
			}
			this.ShowMsg("成功上传了" + num2.ToString() + "个文件！", true);
		}
	}
}
