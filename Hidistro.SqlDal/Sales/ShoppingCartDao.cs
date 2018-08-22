using Hidistro.Core;
using Hidistro.Entities.Bargain;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Bargain;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class ShoppingCartDao
	{
		private Database database;

		public ShoppingCartDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public ShoppingCartInfo GetShoppingCart(MemberInfo member)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, (string)dataReader["SkuId"], (int)dataReader["Quantity"], 0, 0, 0);
					if (cartItemInfo != null)
					{
						shoppingCartInfo.LineItems.Add(cartItemInfo);
					}
				}
			}
			return shoppingCartInfo;
		}

		public ShoppingCartInfo GetShoppingCart(MemberInfo member, int Templateid)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId and Templateid=@Templateid");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "Templateid", System.Data.DbType.Int32, Templateid);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, (string)dataReader["SkuId"], (int)dataReader["Quantity"], 0, 0, 0);
					if (cartItemInfo != null)
					{
						shoppingCartInfo.LineItems.Add(cartItemInfo);
					}
				}
			}
			return shoppingCartInfo;
		}

		public List<ShoppingCartInfo> GetShoppingCartAviti(MemberInfo member, int type)
		{
			List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
			System.Data.DataTable shoppingCategoryId = this.GetShoppingCategoryId(member, type);
			System.Data.DataTable dataTable = new System.Data.DataTable();
			for (int i = 0; i < shoppingCategoryId.Rows.Count; i++)
			{
				ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
				shoppingCartInfo.CategoryId = int.Parse(shoppingCategoryId.Rows[i]["CategoryId"].ToString());
				dataTable = this.GetShopping(shoppingCartInfo.CategoryId.ToString(), member, type);
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, dataTable.Rows[j]["SkuId"].ToString(), int.Parse(dataTable.Rows[j]["Quantity"].ToString()), type, 0, Globals.ToNum(dataTable.Rows[j]["limitedTimeDiscountId"].ToString()));
					if (cartItemInfo != null)
					{
						shoppingCartInfo.LineItems.Add(cartItemInfo);
					}
				}
				list.Add(shoppingCartInfo);
			}
			return list;
		}

		public List<ShoppingCartInfo> GetOrderSummitCart(MemberInfo member)
		{
			List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
			System.Data.DataTable template = this.GetTemplate(member);
			System.Data.DataTable dataTable = new System.Data.DataTable();
			for (int i = 0; i < template.Rows.Count; i++)
			{
				decimal num = 0m;
				int num2 = 0;
				ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
				shoppingCartInfo.TemplateId = template.Rows[i]["TemplateId"].ToString();
				dataTable = this.GetShoppingTemplateid(shoppingCartInfo.TemplateId, member);
				for (int j = 0; j < dataTable.Rows.Count; j++)
				{
					ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, dataTable.Rows[j]["SkuId"].ToString(), int.Parse(dataTable.Rows[j]["Quantity"].ToString()), int.Parse(dataTable.Rows[j]["Type"].ToString()), 0, Globals.ToNum(dataTable.Rows[j]["LimitedTimeDiscountId"].ToString()));
					if (cartItemInfo != null)
					{
						if (cartItemInfo.Type == 0)
						{
							shoppingCartInfo.Amount = cartItemInfo.SubTotal;
							num += shoppingCartInfo.Amount;
							shoppingCartInfo.Exemption = 0m;
							shoppingCartInfo.ShipCost = 0m;
							cartItemInfo.ExchangeId = 0;
						}
						else
						{
							num2 += cartItemInfo.PointNumber * cartItemInfo.Quantity;
							cartItemInfo.ExchangeId = int.Parse(dataTable.Rows[j]["ExchangeId"].ToString());
						}
						shoppingCartInfo.LineItems.Add(cartItemInfo);
					}
				}
				shoppingCartInfo.Total = num;
				shoppingCartInfo.GetPointNumber = num2;
				shoppingCartInfo.MemberPointNumber = member.Points;
				list.Add(shoppingCartInfo);
			}
			return list;
		}

		public System.Data.DataTable GetTemplate(MemberInfo member)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select distinct TemplateId from Hishop_ShoppingCarts where userid=" + member.UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetShoppingTemplateid(string TemplateId, MemberInfo member)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new object[]
			{
				"select * from Hishop_ShoppingCarts where TemplateId=",
				TemplateId,
				" and UserId = ",
				member.UserId,
				" order by Type"
			}));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetShoppingCategoryId(MemberInfo member, int type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new object[]
			{
				"select distinct CategoryId from Hishop_ShoppingCarts where userid=",
				member.UserId,
				" and type=",
				type
			}));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetShopping(string CategoryId, MemberInfo member, int type)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(string.Concat(new object[]
			{
				"select * from Hishop_ShoppingCarts where CategoryId=",
				CategoryId,
				" and UserId = ",
				member.UserId,
				" and [Type]=",
				type
			}));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public System.Data.DataTable GetAllFull(int ActivitiesType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select ReductionMoney,ActivitiesId,ActivitiesName,MeetMoney,ActivitiesType from Hishop_Activities where datediff(dd,GETDATE(),StartTime)<=0 and datediff(dd,GETDATE(),EndTIme)>=0 and (ActivitiesType=0 or ActivitiesType=" + ActivitiesType + ")  order by MeetMoney asc");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public ShoppingCartItemInfo GetCartItemInfo(MemberInfo member, string skuId, int quantity, int type = 0, int bargainDetialId = 0, int limitedTimeDiscountId = 0)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
			this.database.AddInParameter(storedProcCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, (member != null) ? member.UserId : 0);
			this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(storedProcCommand, "GradeId", System.Data.DbType.Int32, (member != null) ? member.GradeId : 0);
			this.database.AddInParameter(storedProcCommand, "Type", System.Data.DbType.Int32, type);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					ShoppingCartItemInfo arg_E7_0 = shoppingCartItemInfo;
					shoppingCartItemInfo.ShippQuantity = quantity;
					arg_E7_0.Quantity = quantity;
					shoppingCartItemInfo.MainCategoryPath = dataReader["MainCategoryPath"].ToString();
					shoppingCartItemInfo.ProductId = (int)dataReader["ProductId"];
					if (DBNull.Value != dataReader["CubicMeter"])
					{
						shoppingCartItemInfo.CubicMeter = (decimal)dataReader["CubicMeter"];
					}
					if (DBNull.Value != dataReader["FreightWeight"])
					{
						shoppingCartItemInfo.FreightWeight = (decimal)dataReader["FreightWeight"];
					}
					if (dataReader["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)dataReader["SKU"];
					}
					shoppingCartItemInfo.Name = (string)dataReader["ProductName"];
					if (DBNull.Value != dataReader["Weight"])
					{
						shoppingCartItemInfo.Weight = (int)dataReader["Weight"];
					}
					if (DBNull.Value != dataReader["FreightTemplateId"])
					{
						shoppingCartItemInfo.FreightTemplateId = (int)dataReader["FreightTemplateId"];
					}
					else
					{
						shoppingCartItemInfo.FreightTemplateId = 0;
					}
					if (DBNull.Value != dataReader["ThirdCommission"])
					{
						shoppingCartItemInfo.ThirdCommission = (decimal)dataReader["ThirdCommission"];
					}
					else
					{
						shoppingCartItemInfo.ThirdCommission = 0m;
					}
					if (DBNull.Value != dataReader["SecondCommission"])
					{
						shoppingCartItemInfo.SecondCommission = (decimal)dataReader["SecondCommission"];
					}
					else
					{
						shoppingCartItemInfo.SecondCommission = 0m;
					}
					if (DBNull.Value != dataReader["FirstCommission"])
					{
						shoppingCartItemInfo.FirstCommission = (decimal)dataReader["FirstCommission"];
					}
					else
					{
						shoppingCartItemInfo.FirstCommission = 0m;
					}
					if (DBNull.Value != dataReader["IsSetCommission"])
					{
						shoppingCartItemInfo.IsSetCommission = (bool)dataReader["IsSetCommission"];
					}
					else
					{
						shoppingCartItemInfo.IsSetCommission = false;
					}
					BargainDetialInfo bargainDetialInfo = null;
					if (bargainDetialId > 0)
					{
						bargainDetialInfo = new BargainDao().GetBargainDetialInfo(bargainDetialId);
					}
					if (bargainDetialId > 0 && bargainDetialInfo != null)
					{
						shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = bargainDetialInfo.Price);
					}
					else
					{
						shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = (decimal)dataReader["SalePrice"]);
					}
					if (limitedTimeDiscountId > 0)
					{
						LimitedTimeDiscountProductInfo limitedTimeDiscountProductByLimitIdAndProductIdAndUserId = new LimitedTimeDiscountDao().GetLimitedTimeDiscountProductByLimitIdAndProductIdAndUserId(limitedTimeDiscountId, shoppingCartItemInfo.ProductId, (member != null) ? member.UserId : 0);
						if (limitedTimeDiscountProductByLimitIdAndProductIdAndUserId != null && limitedTimeDiscountProductByLimitIdAndProductIdAndUserId.BeginTime <= DateTime.Now && DateTime.Now < limitedTimeDiscountProductByLimitIdAndProductIdAndUserId.EndTime)
						{
							shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = limitedTimeDiscountProductByLimitIdAndProductIdAndUserId.FinalPrice);
							shoppingCartItemInfo.LimitedTimeDiscountId = limitedTimeDiscountId;
						}
						else
						{
							shoppingCartItemInfo.LimitedTimeDiscountId = 0;
						}
					}
					else
					{
						shoppingCartItemInfo.LimitedTimeDiscountId = 0;
					}
					if (DBNull.Value != dataReader["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = dataReader["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != dataReader["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = dataReader["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != dataReader["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = dataReader["ThumbnailUrl100"].ToString();
					}
					if (DBNull.Value != dataReader["IsfreeShipping"])
					{
						shoppingCartItemInfo.IsfreeShipping = Convert.ToBoolean(dataReader["IsfreeShipping"]);
					}
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (dataReader["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["AttributeName"]) && dataReader["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["ValueStr"]))
							{
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									dataReader["AttributeName"],
									"：",
									dataReader["ValueStr"],
									"; "
								});
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							shoppingCartItemInfo.Type = 1;
							if (DBNull.Value != dataReader["ProductNumber"])
							{
								shoppingCartItemInfo.ProductNumber = Convert.ToInt32(dataReader["ProductNumber"]);
							}
							if (DBNull.Value != dataReader["PointNumber"])
							{
								shoppingCartItemInfo.PointNumber = Convert.ToInt32(dataReader["PointNumber"]);
							}
							if (DBNull.Value != dataReader["status"])
							{
								shoppingCartItemInfo.Status = Convert.ToInt32(dataReader["status"]);
							}
							if (DBNull.Value != dataReader["exChangeId"])
							{
								shoppingCartItemInfo.ExchangeId = Convert.ToInt32(dataReader["exChangeId"]);
							}
						}
					}
					else
					{
						shoppingCartItemInfo.Type = 0;
					}
				}
			}
			return shoppingCartItemInfo;
		}

		public int GetLimitedTimeDiscountUsedNum(int limitedTimeDiscountId, string skuId, int productId, int userid, bool isContainsShippingCart)
		{
			int num = 0;
			if (userid == -1)
			{
				userid = Globals.GetCurrentMemberUserId();
			}
			if (productId <= 0)
			{
				SKUItem skuItem = new SkuDao().GetSkuItem(skuId);
				if (skuItem != null)
				{
					productId = skuItem.ProductId;
				}
			}
			if (productId > 0 && limitedTimeDiscountId > 0 && userid > 0)
			{
				string query = string.Concat(new object[]
				{
					"select isnull(sum(Quantity),0) from Hishop_OrderItems a inner join Hishop_Orders b on a.OrderId=b.OrderId where a.LimitedTimeDiscountId=",
					limitedTimeDiscountId,
					" and a.ProductId=",
					productId,
					" and b.UserID=",
					userid,
					" and a.OrderItemsStatus<>4"
				});
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				num = Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
				if (isContainsShippingCart)
				{
					query = string.Concat(new object[]
					{
						"select isnull(sum(Quantity),0) from Hishop_ShoppingCarts where UserId=",
						userid,
						" and SkuId in(select SkuId from Hishop_SKUs where ProductId=",
						productId,
						") and LimitedTimeDiscountId=",
						limitedTimeDiscountId
					});
					sqlStringCommand = this.database.GetSqlStringCommand(query);
					this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
					num += Globals.ToNum(this.database.ExecuteScalar(sqlStringCommand));
				}
			}
			return num;
		}

		public void AddLineItem(MemberInfo member, string skuId, int quantity, int categoryid, int Templateid, int type, int exchangeId, int limitedTimeDiscountId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(storedProcCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, categoryid);
			this.database.AddInParameter(storedProcCommand, "Templateid", System.Data.DbType.Int32, Templateid);
			this.database.AddInParameter(storedProcCommand, "Type", System.Data.DbType.Int32, type);
			this.database.AddInParameter(storedProcCommand, "ExchangeId", System.Data.DbType.Int32, exchangeId);
			this.database.AddInParameter(storedProcCommand, "LimitedTimeDiscountId", System.Data.DbType.Int32, limitedTimeDiscountId);
			this.database.ExecuteNonQuery(storedProcCommand);
		}

		public void ClearShoppingCart(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void RemoveLineItem(int userId, string skuId, int type, int limitedTimeDiscountId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId And [Type]=@Type And LimitedTimeDiscountId=" + limitedTimeDiscountId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, type);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public void UpdateLineItemQuantity(MemberInfo member, string skuId, int quantity, int type, int limitedTimeDiscountId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId And [Type]=@Type AND LimitedTimeDiscountId=" + limitedTimeDiscountId);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, type);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
