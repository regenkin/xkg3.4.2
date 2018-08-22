using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.Shop
{
	public class Hi_Ajax_GetImgList : System.Web.IHttpHandler
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
			string listJson = this.GetListJson(context);
			context.Response.Write(listJson);
		}

		public string GetListJson(System.Web.HttpContext context)
		{
			int num = 21;
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{\"status\":1,");
			stringBuilder.Append("\"data\":[");
			int num2 = Globals.RequestFormNum("type");
			int num3 = Globals.RequestFormNum("id");
			string keyword = Globals.RequestFormStr("file_name");
			int pageCount;
			if (num2 == 3)
			{
				DbQueryResult goodsTable = this.GetGoodsTable(context, num, keyword, num3);
				pageCount = TemplatePageControl.GetPageCount(goodsTable.TotalRecords, num);
				stringBuilder.Append(this.GetGoodsListJson(goodsTable));
			}
			else
			{
				DbQueryResult photoList = GalleryHelper.GetPhotoList(keyword, new int?(num3), System.Convert.ToInt32(context.Request.Form["p"]), num, PhotoListOrder.UploadTimeDesc, num2);
				pageCount = TemplatePageControl.GetPageCount(photoList.TotalRecords, num);
				stringBuilder.Append(this.GetImgItemsJson(photoList, context));
			}
			string str = stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
			str += "],";
			str = str + "\"page\": \"" + this.GetPageHtml(pageCount, context) + "\",";
			str += "\"msg\": \"\"";
			return str + "}";
		}

		public string GetPageHtml(int pageCount, System.Web.HttpContext context)
		{
			int pageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]);
			return TemplatePageControl.GetPageHtml(pageCount, pageIndex);
		}

		public string GetImgItemsJson(DbQueryResult mamagerRecordset, System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Data.DataTable dataTable = (System.Data.DataTable)mamagerRecordset.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"id\":\"" + dataTable.Rows[i]["PhotoId"] + "\",");
				stringBuilder.Append(string.Concat(new object[]
				{
					"\"file\":\"http://",
					context.Request.Url.Authority,
					dataTable.Rows[i]["PhotoPath"],
					"\","
				}));
				stringBuilder.Append("\"name\":\"" + Globals.String2Json(dataTable.Rows[i]["PhotoName"].ToString()) + "\"");
				stringBuilder.Append("},");
			}
			return stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
		}

		public string GetGoodsListJson(DbQueryResult GoodsTable)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Data.DataTable dataTable = (System.Data.DataTable)GoodsTable.Data;
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"id\":\"" + dataTable.Rows[i]["ProductId"] + "\",");
				stringBuilder.Append("\"file\":\"" + dataTable.Rows[i]["img"] + "\",");
				stringBuilder.Append("\"name\":\"" + Globals.String2Json(dataTable.Rows[i]["ProductName"].ToString()) + "\"");
				stringBuilder.Append("},");
			}
			return stringBuilder.ToString().TrimEnd(new char[]
			{
				','
			});
		}

		public string GetImgItemJson()
		{
			return "";
		}

		public DbQueryResult GetGoodsTable(System.Web.HttpContext context, int pagesize, string keyword, int maincategoryid)
		{
			return ProductHelper.GetProductsImgList(this.GetProductQuery(context, pagesize, keyword, maincategoryid));
		}

		public ProductQuery GetProductQuery(System.Web.HttpContext context, int pagesize, string keyword, int maincategoryid)
		{
			return new ProductQuery
			{
				Keywords = keyword,
				PageSize = pagesize,
				PageIndex = (context.Request.Form["p"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["p"]),
				SortOrder = SortAction.Desc,
				SortBy = "ProductName",
				MaiCategoryPath = maincategoryid.ToString(),
				SaleStatus = (ProductSaleStatus)((context.Request.Form["status"] == null) ? 1 : System.Convert.ToInt32(context.Request.Form["status"]))
			};
		}
	}
}
