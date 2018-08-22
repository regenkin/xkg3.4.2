using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Admin
{
	public class VsiteHandler : System.Web.IHttpHandler
	{
		private class EnumJson
		{
			public string Name
			{
				get;
				set;
			}

			public string Value
			{
				get;
				set;
			}
		}

		private class SelectProduct
		{
			public string productid
			{
				get;
				set;
			}

			public string ProductName
			{
				get;
				set;
			}

			public string ThumbnailUrl60
			{
				get;
				set;
			}
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = context.Request.Form["actionName"];
			string text2 = string.Empty;
			string key;
			switch (key = text)
			{
			case "Vote":
			{
				System.Collections.Generic.IList<VoteInfo> voteList = StoreHelper.GetVoteList();
				text2 = JsonConvert.SerializeObject(voteList);
				break;
			}
			case "Category":
			{
				var value = from item in CatalogHelper.GetMainCategories()
				select new
				{
					CateId = item.CategoryId,
					CateName = item.Name
				};
				text2 = JsonConvert.SerializeObject(value);
				break;
			}
			case "Activity":
			{
				System.Array values = System.Enum.GetValues(typeof(LotteryActivityType));
				System.Collections.Generic.List<VsiteHandler.EnumJson> list = new System.Collections.Generic.List<VsiteHandler.EnumJson>();
				foreach (System.Enum @enum in values)
				{
					list.Add(new VsiteHandler.EnumJson
					{
						Name = @enum.ToShowText(),
						Value = @enum.ToString()
					});
				}
				text2 = JsonConvert.SerializeObject(list);
				break;
			}
			case "ActivityList":
			{
				string value2 = context.Request.Form["acttype"];
				LotteryActivityType lotteryActivityType = (LotteryActivityType)System.Enum.Parse(typeof(LotteryActivityType), value2);
				if (lotteryActivityType == LotteryActivityType.SignUp)
				{
					var value3 = from item in VShopHelper.GetAllActivity()
					select new
					{
						ActivityId = item.ActivityId,
						ActivityName = item.Name
					};
					text2 = JsonConvert.SerializeObject(value3);
				}
				break;
			}
			case "AccountTime":
			{
				text2 += "{";
				BalanceDrawRequestQuery balanceDrawRequestQuery = new BalanceDrawRequestQuery();
				balanceDrawRequestQuery.RequestTime = "";
				balanceDrawRequestQuery.CheckTime = "";
				balanceDrawRequestQuery.StoreName = "";
				balanceDrawRequestQuery.PageIndex = 1;
				balanceDrawRequestQuery.PageSize = 1;
				balanceDrawRequestQuery.SortOrder = SortAction.Desc;
				balanceDrawRequestQuery.SortBy = "RequestTime";
				balanceDrawRequestQuery.RequestEndTime = "";
				balanceDrawRequestQuery.RequestStartTime = "";
				balanceDrawRequestQuery.IsCheck = "1";
				balanceDrawRequestQuery.UserId = context.Request.Form["UserID"];
				Globals.EntityCoding(balanceDrawRequestQuery, true);
				DbQueryResult balanceDrawRequest = DistributorsBrower.GetBalanceDrawRequest(balanceDrawRequestQuery, null);
				System.Data.DataTable dataTable = (System.Data.DataTable)balanceDrawRequest.Data;
				if (dataTable.Rows.Count > 0)
				{
					if (dataTable.Rows[0]["MerchantCode"].ToString().Trim() != context.Request.Form["merchantcode"].Trim())
					{
						text2 = text2 + "\"Time\":\"" + dataTable.Rows[0]["RequestTime"].ToString() + "\"";
					}
					else
					{
						text2 += "\"Time\":\"\"";
					}
				}
				else
				{
					text2 += "\"Time\":\"\"";
				}
				text2 += "}";
				break;
			}
			case "ProdSelect":
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
				if (!string.IsNullOrEmpty(masterSettings.DistributorProducts))
				{
					System.Data.DataTable products = ProductHelper.GetProducts(masterSettings.DistributorProducts);
					if (products != null && products.Rows.Count > 0)
					{
						System.Collections.Generic.List<VsiteHandler.SelectProduct> list2 = new System.Collections.Generic.List<VsiteHandler.SelectProduct>();
						foreach (System.Data.DataRow dataRow in products.Rows)
						{
							list2.Add(new VsiteHandler.SelectProduct
							{
								productid = dataRow["productid"].ToString(),
								ProductName = dataRow["ProductName"].ToString(),
								ThumbnailUrl60 = dataRow["ThumbnailUrl60"].ToString()
							});
						}
						text2 = JsonConvert.SerializeObject(list2);
					}
				}
				break;
			}
			case "ProdDel":
			{
				text2 += "{";
				string value4 = context.Request.Form["productid"];
				SiteSettings masterSettings2 = SettingsManager.GetMasterSettings(false);
				if (!string.IsNullOrEmpty(masterSettings2.DistributorProducts) && masterSettings2.DistributorProducts.Contains(value4))
				{
					string text3 = "";
					string[] array = masterSettings2.DistributorProducts.Split(new char[]
					{
						','
					});
					for (int i = 0; i < array.Length; i++)
					{
						string text4 = array[i];
						if (!text4.Equals(value4))
						{
							text3 = text3 + text4 + ",";
						}
					}
					if (text3.Length > 0)
					{
						text3 = text3.Substring(0, text3.Length - 1);
					}
					masterSettings2.DistributorProducts = text3;
					SettingsManager.Save(masterSettings2);
				}
				text2 += "\"status\":\"ok\"";
				text2 += "}";
				break;
			}
			}
			context.Response.Write(text2);
		}
	}
}
