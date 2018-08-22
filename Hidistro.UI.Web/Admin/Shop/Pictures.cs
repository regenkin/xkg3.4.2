using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Pictures : AdminPage
	{
		private string keyWordIName = string.Empty;

		private string keyOrder;

		private int? typeId = null;

		private int pageIndex;

		public string GlobalsPath = Globals.ApplicationPath;

		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected System.Web.UI.WebControls.TextBox txtWordName;

		protected System.Web.UI.WebControls.Button btnImagetSearch;

		protected System.Web.UI.WebControls.DataList photoDataList;

		protected Pager pager;

		protected UpImg uploader1;

		protected Pictures() : base("m01", "00000")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string a = Globals.RequestFormStr("posttype");
			if (a == "togallery")
			{
				base.Response.ContentType = "application/json";
				string s = "{\"type\":\"0\",\"tips\":\"操作失败\"}";
				string text = Globals.RequestFormStr("photourl");
				if (!string.IsNullOrEmpty(text))
				{
					System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(base.Server.MapPath(text));
					System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
					bitmap.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
					GalleryHelper.AddPhote(0, "wb" + System.DateTime.Now.ToString("yyyyMMddHHmmss"), text, (int)memoryStream.Length);
					s = "{\"type\":\"1\",\"tips\":\"操作成功\"}";
					memoryStream.Dispose();
				}
				base.Response.Write(s);
				base.Response.End();
				return;
			}
			this.btnImagetSearch.Click += new System.EventHandler(this.btnImagetSearch_Click);
			if (!base.IsPostBack)
			{
				this.LoadParameters();
				this.BindImageData();
			}
		}

		private void btnImagetSearch_Click(object sender, System.EventArgs e)
		{
			this.keyWordIName = this.txtWordName.Text;
			this.BindImageData();
		}

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordIName"]))
			{
				this.keyWordIName = Globals.UrlDecode(this.Page.Request.QueryString["keyWordIName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyWordSel"]))
			{
				this.keyOrder = Globals.UrlDecode(this.Page.Request.QueryString["keyWordSel"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["imageTypeId"], out value))
			{
				this.typeId = new int?(value);
			}
		}

		private void BindImageData()
		{
			this.pageIndex = this.pager.PageIndex;
			PhotoListOrder order = PhotoListOrder.UploadTimeDesc;
			DbQueryResult photoList = GalleryHelper.GetPhotoList(this.keyWordIName, this.typeId, this.pageIndex, order, 0, 20);
			this.photoDataList.DataSource = photoList.Data;
			this.photoDataList.DataBind();
			this.pager.TotalRecords = photoList.TotalRecords;
		}

		public static string Html_ToClient(string Str)
		{
			if (Str == null)
			{
				return null;
			}
			if (Str != string.Empty)
			{
				return System.Web.HttpContext.Current.Server.HtmlDecode(Str.Trim());
			}
			return string.Empty;
		}

		public static string TruncStr(string str, int maxSize)
		{
			str = Pictures.Html_ToClient(str);
			if (str != string.Empty)
			{
				int num = 0;
				System.Text.ASCIIEncoding aSCIIEncoding = new System.Text.ASCIIEncoding();
				byte[] bytes = aSCIIEncoding.GetBytes(str);
				for (int i = 0; i <= bytes.Length - 1; i++)
				{
					if (bytes[i] == 63)
					{
						num += 2;
					}
					else
					{
						num++;
					}
					if (num > maxSize)
					{
						str = str.Substring(0, i);
						break;
					}
				}
				return str;
			}
			return string.Empty;
		}
	}
}
