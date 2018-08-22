using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetFolderTree : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			int type = Globals.RequestFormNum("type");
			context.Response.Write(this.GetTreeListJson(type));
		}

		public string GetTreeListJson(int type)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"status\":1,");
			stringBuilder.Append("\"data\":{");
			stringBuilder.Append("\"total\":" + this.GetImgCount(type) + ",");
			stringBuilder.Append("\"tree\":[");
			stringBuilder.Append(this.GetImgTypeJson(type));
			stringBuilder.Append("]");
			stringBuilder.Append("},");
			stringBuilder.Append("\"msg\":\"\"");
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}

		private int GetImgCount(int type)
		{
			int totalRecords;
			if (type == 3)
			{
				totalRecords = ProductHelper.GetProductsImgList(this.GetProductQuery(0)).TotalRecords;
			}
			else
			{
				totalRecords = GalleryHelper.GetPhotoList("", new int?(0), 10, PhotoListOrder.UploadTimeDesc, type, 20).TotalRecords;
			}
			return totalRecords;
		}

		public ProductQuery GetProductQuery(int categoryid)
		{
			if (categoryid > 0)
			{
				return new ProductQuery
				{
					PageSize = 1,
					PageIndex = 1,
					SortOrder = SortAction.Desc,
					SortBy = "ProductName",
					MaiCategoryPath = categoryid.ToString()
				};
			}
			return new ProductQuery
			{
				PageSize = 1,
				PageIndex = 1,
				SortOrder = SortAction.Desc,
				SortBy = "ProductName"
			};
		}

		public string GetImgTypeJson(int type)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (type == 3)
			{
				System.Collections.Generic.IList<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
				for (int i = 0; i < mainCategories.Count; i++)
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"name\":\"" + mainCategories[i].Name + "\",");
					stringBuilder.Append("\"parent_id\":" + mainCategories[i].ParentCategoryId + ",");
					stringBuilder.Append("\"id\":" + mainCategories[i].CategoryId + ",");
					stringBuilder.Append("\"picNum\":" + ProductHelper.GetProductsImgList(this.GetProductQuery(mainCategories[i].CategoryId)).TotalRecords);
					stringBuilder.Append("},");
				}
			}
			else
			{
				System.Data.DataTable photoCategories = GalleryHelper.GetPhotoCategories(type);
				for (int j = 0; j < photoCategories.Rows.Count; j++)
				{
					stringBuilder.Append("{");
					stringBuilder.Append("\"name\":\"" + photoCategories.Rows[j]["CategoryName"] + "\",");
					stringBuilder.Append("\"parent_id\":0,");
					stringBuilder.Append("\"id\":" + photoCategories.Rows[j]["CategoryId"] + ",");
					stringBuilder.Append("\"picNum\":" + GalleryHelper.GetPhotoList("", new int?(System.Convert.ToInt32(photoCategories.Rows[j]["CategoryId"])), 10, PhotoListOrder.UploadTimeDesc, type, 20).TotalRecords);
					stringBuilder.Append("},");
				}
			}
			return stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
		}
	}
}
