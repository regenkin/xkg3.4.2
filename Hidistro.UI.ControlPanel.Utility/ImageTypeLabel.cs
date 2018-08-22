using Hidistro.ControlPanel.Store;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ImageTypeLabel : Literal
	{
		private int typeId = -1;

		public int TypeId
		{
			get
			{
				return this.typeId;
			}
			set
			{
				this.typeId = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string text = "<ul>";
			string str = string.Empty;
			DataTable photoCategories = GalleryHelper.GetPhotoCategories(this.typeId);
			int defaultPhotoCount = GalleryHelper.GetDefaultPhotoCount();
			string text2 = this.Page.Request.QueryString["ImageTypeId"];
			string empty = string.Empty;
			if (!string.IsNullOrEmpty(text2))
			{
				text2 = this.Page.Request.QueryString["ImageTypeId"];
			}
			if (text2 == "0")
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<li><a href=\"ImageData.aspx?ImageTypeId=0\"><s></s><strong>默认分类<span>(",
					defaultPhotoCount,
					")</span></strong></a></li>"
				});
			}
			else
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					"<li><a href=\"ImageData.aspx?ImageTypeId=0\"><s></s>默认分组<span>(",
					defaultPhotoCount,
					")</span></a></li>"
				});
			}
			foreach (DataRow dataRow in photoCategories.Rows)
			{
				if (dataRow["CategoryId"].ToString() == text2)
				{
					str = string.Format("<li><a href=\"ImageData.aspx?ImageTypeId={0}\"><s></s><strong>{1}</strong><span>({2})</span></a></li>", dataRow["CategoryId"], dataRow["CategoryName"], dataRow["PhotoCounts"].ToString());
				}
				else
				{
					str = string.Format("<li><a href=\"ImageData.aspx?ImageTypeId={0}\"><s></s>{1}<span>({2})</span></a></li>", dataRow["CategoryId"], dataRow["CategoryName"], dataRow["PhotoCounts"].ToString());
				}
				text += str;
			}
			text += "</ul>";
			base.Text = text;
			base.Render(writer);
		}
	}
}
