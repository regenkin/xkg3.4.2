using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class ImageType : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected Grid ImageTypeList;

		protected System.Web.UI.WebControls.Button ImageTypeEdit;

		protected System.Web.UI.WebControls.TextBox txt_AddImageTypeName;

		protected System.Web.UI.WebControls.Button btn_AddImageType;

		protected ImageType() : base("m01", "dpp09")
		{
		}

		protected override void OnInitComplete(System.EventArgs e)
		{
			this.ImageTypeList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.ImageTypeList_RowDeleting);
			this.ImageTypeList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.ImageTypeList_RowCommand);
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btn_AddImageType.Click += new System.EventHandler(this.btn_AddImageType_Click);
			this.ImageTypeEdit.Click += new System.EventHandler(this.ImageTypeEdit_Click);
			if (!this.Page.IsPostBack)
			{
				this.GetImageType();
			}
		}

		private void GetImageType()
		{
			int type = Globals.RequestQueryNum("type");
			this.ImageTypeList.DataSource = GalleryHelper.GetPhotoCategories(type);
			this.ImageTypeList.DataBind();
		}

		private void btn_AddImageType_Click(object sender, System.EventArgs e)
		{
			string text = this.txt_AddImageTypeName.Text;
			if (text.Length == 0)
			{
				this.ShowMsg("分类名称不能为空", false);
				return;
			}
			if (text.Length > 20)
			{
				this.ShowMsg("分类名称长度限在20个字符以内", false);
				return;
			}
			bool flag = GalleryHelper.AddPhotoCategory(Globals.HtmlEncode(text));
			if (flag)
			{
				this.txt_AddImageTypeName.Text = "";
				this.ShowMsg("添加成功！", true);
				this.GetImageType();
				return;
			}
			this.ShowMsg("添加失败", false);
		}

		private void ImageTypeEdit_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.Dictionary<int, string> dictionary = new System.Collections.Generic.Dictionary<int, string>();
			for (int i = 0; i < this.ImageTypeList.Rows.Count; i++)
			{
				System.Web.UI.WebControls.GridViewRow gridViewRow = this.ImageTypeList.Rows[i];
				string text = ((System.Web.UI.WebControls.TextBox)gridViewRow.Cells[1].FindControl("ImageTypeName")).Text;
				if (text.Length > 20)
				{
					this.ShowMsg("分类长度限在20个字符以内", false);
					return;
				}
				int key = System.Convert.ToInt32(this.ImageTypeList.DataKeys[i].Value);
				dictionary.Add(key, Globals.HtmlEncode(text.ToString()));
			}
			try
			{
				int num = GalleryHelper.UpdatePhotoCategories(dictionary);
				if (num > 0)
				{
					this.ShowMsg("保存成功！", true);
				}
				else
				{
					this.ShowMsg("保存失败！", false);
				}
			}
			catch
			{
				this.ShowMsg("保存失败！", false);
			}
		}

		protected void ImageTypeList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int categoryId = (int)this.ImageTypeList.DataKeys[e.RowIndex].Value;
			if (GalleryHelper.DeletePhotoCategory(categoryId))
			{
				this.ShowMsg("删除成功!", true);
				this.GetImageType();
				return;
			}
			this.ShowMsg("删除分类失败", false);
		}

		protected void ImageTypeList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int categoryId = (int)this.ImageTypeList.DataKeys[rowIndex].Value;
			if (e.CommandName == "Fall")
			{
				if (rowIndex < this.ImageTypeList.Rows.Count - 1)
				{
					GalleryHelper.SwapSequence(categoryId, (int)this.ImageTypeList.DataKeys[rowIndex + 1].Value);
					this.GetImageType();
					return;
				}
			}
			else if (e.CommandName == "Rise" && rowIndex > 0)
			{
				GalleryHelper.SwapSequence(categoryId, (int)this.ImageTypeList.DataKeys[rowIndex - 1].Value);
				this.GetImageType();
			}
		}
	}
}
