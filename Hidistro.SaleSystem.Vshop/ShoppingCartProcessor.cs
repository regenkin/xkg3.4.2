using Hidistro.Core;
using Hidistro.Entities.Bargain;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.SqlDal.Bargain;
using Hidistro.SqlDal.Commodities;
using Hidistro.SqlDal.Members;
using Hidistro.SqlDal.Promotions;
using Hidistro.SqlDal.Sales;
using System;
using System.Collections.Generic;

namespace Hidistro.SaleSystem.Vshop
{
	public static class ShoppingCartProcessor
	{
		public static int GetLimitedTimeDiscountUsedNum(int limitedTimeDiscountId, string skuId, int productId, int userid, bool isContainsShippingCart)
		{
			return new ShoppingCartDao().GetLimitedTimeDiscountUsedNum(limitedTimeDiscountId, skuId, productId, userid, isContainsShippingCart);
		}

		public static ShoppingCartInfo GetShoppingCart()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			ShoppingCartInfo result;
			if (currentMember == null)
			{
				result = null;
			}
			else
			{
				ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember);
				if (shoppingCart.LineItems.Count == 0)
				{
					result = null;
				}
				else
				{
					result = shoppingCart;
				}
			}
			return result;
		}

		public static ShoppingCartInfo GetShoppingCart(int Templateid)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			ShoppingCartInfo result;
			if (currentMember == null)
			{
				result = null;
			}
			else
			{
				ShoppingCartInfo shoppingCart = new ShoppingCartDao().GetShoppingCart(currentMember, Templateid);
				if (shoppingCart.LineItems.Count == 0)
				{
					result = null;
				}
				else
				{
					result = shoppingCart;
				}
			}
			return result;
		}

		public static List<ShoppingCartInfo> GetOrderSummitCart()
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			List<ShoppingCartInfo> result;
			if (currentMember == null)
			{
				result = null;
			}
			else
			{
				List<ShoppingCartInfo> orderSummitCart = new ShoppingCartDao().GetOrderSummitCart(currentMember);
				if (orderSummitCart.Count == 0)
				{
					result = null;
				}
				else
				{
					result = orderSummitCart;
				}
			}
			return result;
		}

		public static List<ShoppingCartInfo> GetShoppingCartAviti(int type = 0)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			List<ShoppingCartInfo> result;
			if (currentMember == null)
			{
				result = null;
			}
			else
			{
				List<ShoppingCartInfo> shoppingCartAviti = new ShoppingCartDao().GetShoppingCartAviti(currentMember, type);
				if (shoppingCartAviti.Count == 0)
				{
					result = null;
				}
				else
				{
					result = shoppingCartAviti;
				}
			}
			return result;
		}

		public static void AddLineItem(string skuId, int quantity, int categoryid, int Templateid, int type = 0, int exchangeId = 0, int limitedTimeDiscountId = 0)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (quantity <= 0)
			{
				quantity = 1;
			}
			int num = 1;
			int quantity2 = 0;
			if (limitedTimeDiscountId == 0)
			{
				SKUItem skuItem = new SkuDao().GetSkuItem(skuId);
				if (skuItem != null)
				{
					int productId = skuItem.ProductId;
					string limitedTimeDiscountIdByProductId = new LimitedTimeDiscountDao().GetLimitedTimeDiscountIdByProductId(currentMember.UserId, skuId, productId);
					int num2 = Globals.ToNum(limitedTimeDiscountIdByProductId);
					if (num2 > 0)
					{
						LimitedTimeDiscountInfo discountInfo = new LimitedTimeDiscountDao().GetDiscountInfo(num2);
						if (new MemberDao().CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, currentMember.UserId))
						{
							int limitedTimeDiscountUsedNum = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(num2, skuId, productId, currentMember.UserId, true);
							if (discountInfo.LimitNumber == 0)
							{
								limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
							}
							else if (discountInfo.LimitNumber - limitedTimeDiscountUsedNum >= quantity)
							{
								limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
							}
							else if (discountInfo.LimitNumber - limitedTimeDiscountUsedNum < quantity)
							{
								num = 2;
								limitedTimeDiscountId = discountInfo.LimitedTimeDiscountId;
								quantity2 = quantity - (discountInfo.LimitNumber - limitedTimeDiscountUsedNum);
								quantity = discountInfo.LimitNumber - limitedTimeDiscountUsedNum;
							}
						}
					}
				}
			}
			new ShoppingCartDao().AddLineItem(currentMember, skuId, quantity, categoryid, Templateid, type, exchangeId, limitedTimeDiscountId);
			if (num == 2)
			{
				new ShoppingCartDao().AddLineItem(currentMember, skuId, quantity2, categoryid, Templateid, type, exchangeId, 0);
			}
		}

		public static void RemoveLineItem(string skuId, int type, int limitedTimeDiscountId)
		{
			new ShoppingCartDao().RemoveLineItem(Globals.GetCurrentMemberUserId(), skuId, type, limitedTimeDiscountId);
		}

		public static void UpdateLineItemQuantity(string skuId, int quantity, int type, int limitedTimeDiscountId)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveLineItem(skuId, type, limitedTimeDiscountId);
			}
			new ShoppingCartDao().UpdateLineItemQuantity(currentMember, skuId, quantity, type, limitedTimeDiscountId);
		}

		public static void ClearShoppingCart()
		{
			new ShoppingCartDao().ClearShoppingCart(Globals.GetCurrentMemberUserId());
		}

		public static int GetSkuStock(string skuId, int type = 0, int exchangeId = 0)
		{
			int result = new SkuDao().GetSkuItem(skuId).Stock;
			if (type > 0)
			{
				int productId = int.Parse(skuId.Split(new char[]
				{
					'_'
				})[0]);
				PointExchangeProductInfo productInfo = new PointExChangeDao().GetProductInfo(exchangeId, productId);
				if (productInfo != null)
				{
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					int userProductExchangedCount = new PointExChangeDao().GetUserProductExchangedCount(exchangeId, productId, currentMember.UserId);
					int productExchangedCount = new PointExChangeDao().GetProductExchangedCount(exchangeId, productId);
					int num = (productInfo.ProductNumber - productExchangedCount >= 0) ? (productInfo.ProductNumber - productExchangedCount) : 0;
					int num2;
					if (productInfo.EachMaxNumber > 0)
					{
						if (userProductExchangedCount < productInfo.EachMaxNumber)
						{
							if (productInfo.EachMaxNumber <= num)
							{
								num2 = productInfo.EachMaxNumber;
							}
							else
							{
								num2 = num;
							}
						}
						else
						{
							num2 = 0;
						}
					}
					else
					{
						num2 = num;
					}
					if (num2 > 0)
					{
						result = num2;
					}
				}
			}
			return result;
		}

		public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0, 0, 0);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				shoppingCartInfo.LineItems.Add(cartItemInfo);
				result = shoppingCartInfo;
			}
			return result;
		}

		public static List<ShoppingCartInfo> GetListShoppingCart(string productSkuId, int buyAmount, int bargainDetialId = 0, int limitedTimeDiscountId = 0)
		{
			List<ShoppingCartInfo> list = new List<ShoppingCartInfo>();
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			List<ShoppingCartInfo> result;
			if (bargainDetialId > 0)
			{
				ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0, bargainDetialId, 0);
				if (cartItemInfo == null)
				{
					result = null;
					return result;
				}
				BargainDetialInfo bargainDetialInfo = new BargainDao().GetBargainDetialInfo(bargainDetialId);
				if (bargainDetialInfo == null)
				{
					result = null;
					return result;
				}
				shoppingCartInfo.TemplateId = cartItemInfo.FreightTemplateId.ToString();
				shoppingCartInfo.Amount = bargainDetialInfo.Number * bargainDetialInfo.Price;
				shoppingCartInfo.Total = shoppingCartInfo.Amount;
				shoppingCartInfo.Exemption = 0m;
				shoppingCartInfo.ShipCost = 0m;
				shoppingCartInfo.GetPointNumber = cartItemInfo.PointNumber * cartItemInfo.Quantity;
				shoppingCartInfo.MemberPointNumber = currentMember.Points;
				shoppingCartInfo.LineItems.Add(cartItemInfo);
				list.Add(shoppingCartInfo);
			}
			else
			{
				ShoppingCartItemInfo cartItemInfo = new ShoppingCartDao().GetCartItemInfo(currentMember, productSkuId, buyAmount, 0, 0, limitedTimeDiscountId);
				if (cartItemInfo == null)
				{
					result = null;
					return result;
				}
				shoppingCartInfo.TemplateId = cartItemInfo.FreightTemplateId.ToString();
				shoppingCartInfo.Amount = cartItemInfo.SubTotal;
				shoppingCartInfo.Total = shoppingCartInfo.Amount;
				shoppingCartInfo.Exemption = 0m;
				shoppingCartInfo.ShipCost = 0m;
				shoppingCartInfo.GetPointNumber = cartItemInfo.PointNumber * cartItemInfo.Quantity;
				shoppingCartInfo.MemberPointNumber = currentMember.Points;
				shoppingCartInfo.LineItems.Add(cartItemInfo);
				list.Add(shoppingCartInfo);
			}
			result = list;
			return result;
		}
	}
}
