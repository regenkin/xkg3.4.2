using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class TaobaoProductHandler : System.Web.IHttpHandler
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
			GzipExtention.Gzip(context);
			string text = context.Request["action"];
			string a;
			if ((a = text) != null)
			{
				if (a == "TaobaoProductIsExit")
				{
					this.ProcessTaobaoProductIsExit(context);
					return;
				}
				if (a == "TaobaoProductDown")
				{
					this.ProcessTaobaoProductDown(context);
					return;
				}
			}
			context.Response.Write("error");
		}

		private void ProcessTaobaoProductIsExit(System.Web.HttpContext context)
		{
			long taobaoProductId = long.Parse(context.Request.Form["taobaoProductId"]);
			bool flag = ProductHelper.IsExitTaobaoProduct(taobaoProductId);
			context.Response.Write(flag.ToString());
		}

		private void ProcessTaobaoProductDown(System.Web.HttpContext context)
		{
			ProductInfo productInfo = new ProductInfo();
			productInfo.CategoryId = 0;
			productInfo.BrandId = new int?(0);
			productInfo.ProductName = System.Web.HttpUtility.UrlDecode(context.Request.Form["ProductName"]);
			productInfo.ProductCode = context.Request.Form["ProductCode"];
			productInfo.Description = System.Web.HttpUtility.UrlDecode(context.Request.Form["Description"]);
			if (context.Request.Form["SaleStatus"] == "onsale")
			{
				productInfo.SaleStatus = ProductSaleStatus.OnSale;
			}
			else
			{
				productInfo.SaleStatus = ProductSaleStatus.OnStock;
			}
			productInfo.AddedDate = System.DateTime.Parse(context.Request.Form["AddedDate"]);
			productInfo.TaobaoProductId = long.Parse(context.Request.Form["TaobaoProductId"]);
			string text = context.Request.Form["ImageUrls"];
			if (!string.IsNullOrEmpty(text))
			{
				this.DownloadImage(productInfo, text, context);
			}
			productInfo.TypeId = new int?(ProductTypeHelper.GetTypeId(context.Request.Form["TypeName"]));
			int value = int.Parse(context.Request.Form["Weight"]);
			System.Collections.Generic.Dictionary<string, SKUItem> skus = this.GetSkus(productInfo, value, context);
			ProductActionStatus productActionStatus = ProductHelper.AddProduct(productInfo, skus, null, null);
			if (productActionStatus == ProductActionStatus.Success)
			{
				TaobaoProductInfo taobaoProduct = this.GetTaobaoProduct(context);
				taobaoProduct.ProductId = productInfo.ProductId;
				taobaoProduct.ProTitle = productInfo.ProductName;
				taobaoProduct.Num = (long)productInfo.Stock;
				if (productInfo.Stock <= 0)
				{
					taobaoProduct.Num = long.Parse(context.Request.Form["Stock"]);
				}
				ProductHelper.UpdateToaobProduct(taobaoProduct);
			}
			context.Response.Write(productActionStatus.ToString());
		}

		private void DownloadImage(ProductInfo product, string imageUrls, System.Web.HttpContext context)
		{
			imageUrls = System.Web.HttpUtility.UrlDecode(imageUrls);
			string[] array = imageUrls.Split(new char[]
			{
				';'
			});
			int num = 1;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string text2 = string.Format("/Storage/master/product/images/{0}", System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture) + text.Substring(text.LastIndexOf('.')));
				string text3 = text2.Replace("/images/", "/thumbs40/40_");
				string text4 = text2.Replace("/images/", "/thumbs60/60_");
				string text5 = text2.Replace("/images/", "/thumbs100/100_");
				string text6 = text2.Replace("/images/", "/thumbs160/160_");
				string text7 = text2.Replace("/images/", "/thumbs180/180_");
				string text8 = text2.Replace("/images/", "/thumbs220/220_");
				string text9 = text2.Replace("/images/", "/thumbs310/310_");
				string text10 = text2.Replace("/images/", "/thumbs410/410_");
				string text11 = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + text2);
				System.Net.WebClient webClient = new System.Net.WebClient();
				try
				{
					webClient.DownloadFile(text, text11);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text3), 40, 40);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text4), 60, 60);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text5), 100, 100);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text6), 160, 160);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text7), 180, 180);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text8), 220, 220);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text9), 310, 310);
					ResourcesHelper.CreateThumbnail(text11, context.Request.MapPath(Globals.ApplicationPath + text10), 410, 410);
					switch (num)
					{
					case 1:
						product.ImageUrl1 = text2;
						product.ThumbnailUrl40 = text3;
						product.ThumbnailUrl60 = text4;
						product.ThumbnailUrl100 = text5;
						product.ThumbnailUrl160 = text6;
						product.ThumbnailUrl180 = text7;
						product.ThumbnailUrl220 = text8;
						product.ThumbnailUrl310 = text9;
						product.ThumbnailUrl410 = text10;
						break;
					case 2:
						product.ImageUrl2 = text2;
						break;
					case 3:
						product.ImageUrl3 = text2;
						break;
					case 4:
						product.ImageUrl4 = text2;
						break;
					case 5:
						product.ImageUrl5 = text2;
						break;
					}
					num++;
				}
				catch
				{
				}
			}
		}

		private System.Collections.Generic.Dictionary<string, SKUItem> GetSkus(ProductInfo product, decimal weight, System.Web.HttpContext context)
		{
			string text = context.Request.Form["SkuString"];
			System.Collections.Generic.Dictionary<string, SKUItem> dictionary;
			if (string.IsNullOrEmpty(text))
			{
				product.HasSKU = false;
				dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>
				{
					{
						"0",
						new SKUItem
						{
							SkuId = "0",
							SKU = product.ProductCode,
							SalePrice = decimal.Parse(context.Request.Form["SalePrice"]),
							CostPrice = 0m,
							Stock = int.Parse(context.Request.Form["Stock"]),
							Weight = weight
						}
					}
				};
			}
			else
			{
				product.HasSKU = true;
				dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>();
				text = System.Web.HttpUtility.UrlDecode(text);
				string[] array = text.Split(new char[]
				{
					'|'
				});
				for (int i = 0; i < array.Length; i++)
				{
					string text2 = array[i];
					string[] array2 = text2.Split(new char[]
					{
						','
					});
					SKUItem sKUItem = new SKUItem();
					sKUItem.SKU = array2[0];
					sKUItem.Weight = weight;
					sKUItem.Stock = int.Parse(array2[1]);
					sKUItem.SalePrice = decimal.Parse(array2[2]);
					string text3 = array2[3];
					string text4 = "";
					string[] array3 = text3.Split(new char[]
					{
						';'
					});
					for (int j = 0; j < array3.Length; j++)
					{
						string text5 = array3[j];
						string[] array4 = text5.Split(new char[]
						{
							':'
						});
						int specificationId = ProductTypeHelper.GetSpecificationId(product.TypeId.Value, array4[0]);
						int specificationValueId = ProductTypeHelper.GetSpecificationValueId(specificationId, array4[1].Replace("\\", "/"));
						text4 = text4 + specificationValueId + "_";
						sKUItem.SkuItems.Add(specificationId, specificationValueId);
					}
					sKUItem.SkuId = text4.Substring(0, text4.Length - 1);
					dictionary.Add(sKUItem.SkuId, sKUItem);
				}
			}
			return dictionary;
		}

		private TaobaoProductInfo GetTaobaoProduct(System.Web.HttpContext context)
		{
			TaobaoProductInfo taobaoProductInfo = new TaobaoProductInfo();
			taobaoProductInfo.Cid = long.Parse(context.Request.Form["Cid"]);
			taobaoProductInfo.StuffStatus = context.Request.Form["StuffStatus"];
			taobaoProductInfo.LocationState = context.Request.Form["LocationState"];
			taobaoProductInfo.LocationCity = context.Request.Form["LocationCity"];
			taobaoProductInfo.FreightPayer = context.Request.Form["FreightPayer"];
			if (!string.IsNullOrEmpty(context.Request.Form["PostFee"]))
			{
				taobaoProductInfo.PostFee = decimal.Parse(context.Request.Form["PostFee"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["ExpressFee"]))
			{
				taobaoProductInfo.ExpressFee = decimal.Parse(context.Request.Form["ExpressFee"]);
			}
			if (!string.IsNullOrEmpty(context.Request.Form["EMSFee"]))
			{
				taobaoProductInfo.EMSFee = decimal.Parse(context.Request.Form["EMSFee"]);
			}
			taobaoProductInfo.HasInvoice = bool.Parse(context.Request.Form["HasInvoice"]);
			taobaoProductInfo.HasWarranty = bool.Parse(context.Request.Form["HasWarranty"]);
			taobaoProductInfo.HasDiscount = false;
			taobaoProductInfo.ListTime = System.DateTime.Now;
			taobaoProductInfo.PropertyAlias = context.Request.Form["PropertyAlias"];
			taobaoProductInfo.InputPids = context.Request.Form["InputPids"];
			taobaoProductInfo.InputStr = context.Request.Form["InputStr"];
			taobaoProductInfo.SkuProperties = context.Request.Form["SkuProperties"];
			taobaoProductInfo.SkuQuantities = context.Request.Form["SkuQuantities"];
			taobaoProductInfo.SkuPrices = context.Request.Form["SkuPrices"];
			taobaoProductInfo.SkuOuterIds = context.Request.Form["SkuOuterIds"];
			return taobaoProductInfo;
		}

		private string LoadImage(string path)
		{
			byte[] array = null;
			using (System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
			{
				array = new byte[fileStream.Length];
				fileStream.Read(array, 0, (int)fileStream.Length);
			}
			return System.Convert.ToBase64String(array);
		}
	}
}
