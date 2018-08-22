using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ImageData : AdminPage
	{
		private int orderby;

		protected string localUrl = string.Empty;

		private string keyWordIName = string.Empty;

		private string keyOrder;

		private int? typeId = null;

		private int pageIndex;

		public string GlobalsPath = Globals.ApplicationPath;

		protected Script Script2;

		protected Script Script7;

		protected Script Script4;

		protected Script Script1;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Button btnHiddenDel;

		protected System.Web.UI.HtmlControls.HtmlButton btnDelete1;

		protected ImageOrderDropDownList ImageOrder;

		protected System.Web.UI.WebControls.TextBox txtWordName;

		protected System.Web.UI.WebControls.Button btnImagetSearch;

		protected ImageTypeLabel ImageTypeID;

		protected System.Web.UI.WebControls.Label lblImageData;

		protected System.Web.UI.WebControls.Repeater rptList;

		protected Pager pager;

		protected System.Web.UI.WebControls.HiddenField ReImageDataNameId;

		protected System.Web.UI.WebControls.TextBox ReImageDataName;

		protected System.Web.UI.WebControls.Button btnSaveImageDataName;

		protected System.Web.UI.WebControls.HiddenField RePlaceImg;

		protected System.Web.UI.WebControls.HiddenField RePlaceId;

		protected System.Web.UI.WebControls.FileUpload FileUpload;

		protected ImageDataGradeDropDownList dropImageFtp;

		protected System.Web.UI.WebControls.HiddenField hdfSelIDList;

		protected System.Web.UI.WebControls.Button btnMoveImageData;

		protected ImageData() : base("m01", "dpp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.localUrl = base.Request.Url.ToString();
			this.btnHiddenDel.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnSaveImageDataName.Click += new System.EventHandler(this.btnSaveImageDataName_Click);
			this.btnMoveImageData.Click += new System.EventHandler(this.btnMoveImageData_Click);
			this.btnImagetSearch.Click += new System.EventHandler(this.btnImagetSearch_Click);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.ImageOrder.DataBind();
				this.dropImageFtp.DataBind();
				this.BindImageData();
			}
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

		private void btnImagetSearch_Click(object sender, System.EventArgs e)
		{
			this.keyWordIName = this.txtWordName.Text;
			this.BindImageData();
		}

		private void BindImageData()
		{
			int type = Globals.RequestQueryNum("type");
			this.pageIndex = this.pager.PageIndex;
			this.orderby = Globals.RequestQueryNum("orderby");
			switch (this.orderby)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
				this.ImageOrder.SelectedValue = new int?(this.orderby);
				break;
			}
			PhotoListOrder order = (PhotoListOrder)System.Enum.ToObject(typeof(PhotoListOrder), this.orderby);
			DbQueryResult photoList = GalleryHelper.GetPhotoList(this.keyWordIName, this.typeId, this.pageIndex, order, type, 18);
			this.rptList.DataSource = photoList.Data;
			this.rptList.DataBind();
			this.pager.TotalRecords = photoList.TotalRecords;
			this.lblImageData.Text = this.pager.TotalRecords.ToString();
		}

		private void btnMoveImageData_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			int pTypeId = Globals.ToNum(this.dropImageFtp.SelectedItem.Value);
			string[] array = this.hdfSelIDList.Value.Trim(new char[]
			{
				','
			}).Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				int num = Globals.ToNum(s);
				if (num > 0)
				{
					list.Add(num);
				}
			}
			if (GalleryHelper.MovePhotoType(list, pTypeId) > 0)
			{
				this.ShowMsg("图片移动成功！", true);
			}
			this.BindImageData();
		}

		private void btnSaveImageDataName_Click(object sender, System.EventArgs e)
		{
			string text = this.ReImageDataName.Text;
			if (string.IsNullOrEmpty(text) || text.Length > 30)
			{
				this.ShowMsg("图片名称不能为空长度限制在30个字符以内！", false);
				return;
			}
			int photoId = System.Convert.ToInt32(this.ReImageDataNameId.Value);
			GalleryHelper.RenamePhoto(photoId, text);
			this.BindImageData();
		}

		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			bool flag = true;
			string[] array = this.hdfSelIDList.Value.Trim(new char[]
			{
				','
			}).Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				try
				{
					int photoId = Globals.ToNum(s);
					if (!GalleryHelper.DeletePhoto(photoId))
					{
						flag = false;
					}
				}
				catch
				{
					this.ShowMsg("删除文件错误", false);
					this.BindImageData();
				}
			}
			if (flag)
			{
				this.ShowMsg("删除图片成功", true);
			}
			this.BindImageData();
		}

		public static string TruncStr(string str, int maxSize)
		{
			str = ImageData.Html_ToClient(str);
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
	}
}
