using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class LimitedTimeDiscountHandler : System.Web.IHttpHandler
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
			string text = Globals.RequestFormStr("action");
			string key;
			switch (key = text.ToLower())
			{
			case "changestatus":
				this.ChangeStatus(context);
				return;
			case "changediscountproductstatus":
				this.ChangeDiscountProductStatus(context);
				return;
			case "savediscountproduct":
				this.SaveDiscountProduct(context);
				return;
			case "isdiscountproduct":
				this.IsDiscountProduct(context);
				return;
			case "savediscountproductinfo":
				this.SaveDiscountProductInfo(context);
				return;
			case "updatediscountproductlist":
				this.UpdateDiscountProductList(context);
				return;
			case "deletediscountproduct":
				this.DeleteDiscountProduct(context);
				break;

				return;
			}
		}

		private void DeleteDiscountProduct(System.Web.HttpContext context)
		{
			string text = Globals.RequestFormStr("limitedTimeDiscountProductIds");
			if (!string.IsNullOrEmpty(text))
			{
				bool flag = LimitedTimeDiscountHelper.DeleteDiscountProduct(text);
				if (flag)
				{
					context.Response.Write("{\"msg\":\"success\"}");
					return;
				}
				context.Response.Write("{\"msg\":\"fail\"}");
			}
		}

		private void UpdateDiscountProductList(System.Web.HttpContext context)
		{
			int id = Globals.RequestFormNum("LimitedTimeDiscountId");
			string text = Globals.RequestFormStr("discountProductList").Trim(new char[]
			{
				','
			});
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				string[] array3 = text2.Split(new char[]
				{
					'^'
				});
				for (int j = 0; j < array3.Length; j++)
				{
					if (!string.IsNullOrEmpty(array3[0]) && !string.IsNullOrEmpty(array3[1]) && !string.IsNullOrEmpty(array3[4]) && (!string.IsNullOrEmpty(array3[2]) || !string.IsNullOrEmpty(array3[3])))
					{
						LimitedTimeDiscountProductInfo limitedTimeDiscountProductInfo = new LimitedTimeDiscountProductInfo();
						int limitedTimeDiscountProductId = Globals.ToNum(array3[0]);
						LimitedTimeDiscountHelper.GetDiscountInfo(id);
						limitedTimeDiscountProductInfo.LimitedTimeDiscountProductId = limitedTimeDiscountProductId;
						limitedTimeDiscountProductInfo.Discount = ((string.IsNullOrEmpty(array3[2]) || array3[2] == "undefined") ? 0m : decimal.Parse(array3[2]));
						limitedTimeDiscountProductInfo.Minus = ((string.IsNullOrEmpty(array3[3]) || array3[2] == "undefined") ? 0m : decimal.Parse(array3[3]));
						limitedTimeDiscountProductInfo.FinalPrice = decimal.Parse(array3[4]);
						LimitedTimeDiscountHelper.UpdateLimitedTimeDiscountProductById(limitedTimeDiscountProductInfo);
					}
				}
			}
			context.Response.Write("{\"msg\":\"success\"}");
		}

		private void SaveDiscountProductInfo(System.Web.HttpContext context)
		{
			int productId = Globals.RequestFormNum("ProductId");
			int limitedTimeDiscountId = Globals.RequestFormNum("LimitedTimeDiscountId");
			decimal discount;
			decimal.TryParse(Globals.RequestFormStr("Discount"), out discount);
			decimal minus;
			decimal.TryParse(Globals.RequestFormStr("Minus"), out minus);
			decimal finalPrice;
			decimal.TryParse(Globals.RequestFormStr("FinalPrice"), out finalPrice);
			if (LimitedTimeDiscountHelper.UpdateLimitedTimeDiscountProduct(new LimitedTimeDiscountProductInfo
			{
				ProductId = productId,
				LimitedTimeDiscountId = limitedTimeDiscountId,
				Discount = discount,
				Minus = minus,
				FinalPrice = finalPrice
			}))
			{
				context.Response.Write("{\"msg\":\"success\"}");
				return;
			}
			context.Response.Write("{\"msg\":\"fial\"}");
		}

		private void IsDiscountProduct(System.Web.HttpContext context)
		{
			int num = Globals.RequestFormNum("productId");
			if (num > 0)
			{
				LimitedTimeDiscountProductInfo discountProductInfoByProductId = LimitedTimeDiscountHelper.GetDiscountProductInfoByProductId(num);
				if (discountProductInfoByProductId != null)
				{
					LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(discountProductInfoByProductId.LimitedTimeDiscountId);
					MemberInfo currentMember = MemberProcessor.GetCurrentMember();
					int num2 = 0;
					int num3 = discountInfo.LimitNumber;
					if (discountInfo != null)
					{
						if (currentMember != null)
						{
							if (MemberHelper.CheckCurrentMemberIsInRange(discountInfo.ApplyMembers, discountInfo.DefualtGroup, discountInfo.CustomGroup, currentMember.UserId))
							{
								int limitedTimeDiscountUsedNum = ShoppingCartProcessor.GetLimitedTimeDiscountUsedNum(discountProductInfoByProductId.LimitedTimeDiscountId, null, num, currentMember.UserId, false);
								if (discountInfo.LimitNumber == 0)
								{
									num2 = discountInfo.LimitedTimeDiscountId;
								}
								else if (discountInfo.LimitNumber - limitedTimeDiscountUsedNum > 0)
								{
									num2 = discountInfo.LimitedTimeDiscountId;
									num3 = discountInfo.LimitNumber - limitedTimeDiscountUsedNum;
								}
								else
								{
									num3 = 0;
								}
							}
						}
						else
						{
							num2 = discountInfo.LimitedTimeDiscountId;
						}
					}
					if (discountInfo != null)
					{
						context.Response.Write(string.Concat(new object[]
						{
							"{\"msg\":\"success\",\"ActivityName\":\"",
							Globals.String2Json(discountInfo.ActivityName),
							"\",\"FinalPrice\":\"",
							discountProductInfoByProductId.FinalPrice.ToString("f2"),
							"\",\"LimitedTimeDiscountId\":\"",
							num2,
							"\",\"LimitNumber\":\"",
							discountInfo.LimitNumber,
							"\",\"RemainNumber\":\"",
							num3,
							"\"}"
						}));
					}
				}
			}
		}

		private void SaveDiscountProduct(System.Web.HttpContext context)
		{
			Globals.RequestFormNum("id");
			string text = Globals.RequestFormStr("discountProductList").Trim(new char[]
			{
				','
			});
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				string[] array3 = text2.Split(new char[]
				{
					'^'
				});
				LimitedTimeDiscountProductInfo limitedTimeDiscountProductInfo = new LimitedTimeDiscountProductInfo();
				if (!string.IsNullOrEmpty(array3[0]) && !string.IsNullOrEmpty(array3[1]) && !string.IsNullOrEmpty(array3[4]) && (!string.IsNullOrEmpty(array3[2]) || !string.IsNullOrEmpty(array3[3])))
				{
					int num = Globals.ToNum(array3[0]);
					LimitedTimeDiscountInfo discountInfo = LimitedTimeDiscountHelper.GetDiscountInfo(num);
					limitedTimeDiscountProductInfo.LimitedTimeDiscountId = num;
					limitedTimeDiscountProductInfo.ProductId = Globals.ToNum(array3[1]);
					limitedTimeDiscountProductInfo.Discount = ((string.IsNullOrEmpty(array3[2]) || array3[2] == "undefined") ? 0m : decimal.Parse(array3[2]));
					limitedTimeDiscountProductInfo.Minus = ((string.IsNullOrEmpty(array3[3]) || array3[2] == "undefined") ? 0m : decimal.Parse(array3[3]));
					limitedTimeDiscountProductInfo.FinalPrice = decimal.Parse(array3[4]);
					if (discountInfo != null)
					{
						limitedTimeDiscountProductInfo.BeginTime = discountInfo.BeginTime;
						limitedTimeDiscountProductInfo.EndTime = discountInfo.EndTime;
					}
					limitedTimeDiscountProductInfo.CreateTime = System.DateTime.Now;
					limitedTimeDiscountProductInfo.Status = 1;
					LimitedTimeDiscountHelper.AddLimitedTimeDiscountProduct(limitedTimeDiscountProductInfo);
				}
			}
			context.Response.Write("{\"msg\":\"success\"}");
		}

		private void ChangeDiscountProductStatus(System.Web.HttpContext context)
		{
			string ids = Globals.RequestFormStr("id");
			int status = Globals.RequestFormNum("status");
			bool flag = LimitedTimeDiscountHelper.ChangeDiscountProductStatus(ids, status);
			if (flag)
			{
				context.Response.Write("{\"msg\":\"success\"}");
				return;
			}
			context.Response.Write("{\"msg\":\"fial\"}");
		}

		private void ChangeStatus(System.Web.HttpContext context)
		{
			int id = Globals.RequestFormNum("id");
			int num = Globals.RequestFormNum("status");
			bool flag = LimitedTimeDiscountHelper.UpdateDiscountStatus(id, (num == 3) ? DiscountStatus.Normal : DiscountStatus.Stop);
			if (flag)
			{
				context.Response.Write("{\"msg\":\"success\"}");
				return;
			}
			context.Response.Write("{\"msg\":\"fial\"}");
		}
	}
}
