using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Promotions;
using System;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SaveCouponDataHandler : System.Web.IHttpHandler
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
			try
			{
				string text = context.Request["name"].ToString().Trim();
				string val = context.Request["val"].ToString().Trim();
				string val2 = context.Request["condition"].ToString().Trim();
				string val3 = context.Request["begin"].ToString().Trim();
				string val4 = context.Request["end"].ToString().Trim();
				string val5 = context.Request["total"].ToString().Trim();
				string value = context.Request["isAllMember"].ToString().Trim();
				string text2 = context.Request["memberlvl"].ToString().Trim();
				string text3 = context.Request["defualtGroup"].ToString().Trim();
				string text4 = context.Request["customGroup"].ToString().Trim();
				string val6 = context.Request["maxNum"].ToString().Trim();
				string val7 = context.Request["type"].ToString().Trim();
				string couponTypes = context.Request["couponType"].ToString().Trim();
				decimal num = 0m;
				decimal num2 = 0m;
				System.DateTime now = System.DateTime.Now;
				System.DateTime now2 = System.DateTime.Now;
				int stockNum = 0;
				string memberGrades = "";
				int maxReceivNum = 1;
				int num3 = 0;
				if (string.IsNullOrEmpty(text) || text.Length > 20)
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券名称\"}");
				}
				else
				{
					string couponName = text;
					if (!this.bDecimal(val, ref num))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券面值\"}");
					}
					else if (!this.bDecimal(val2, ref num2))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券适用最大满足额\"}");
					}
					else if (!this.bDate(val3, true, ref now))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的生效日期\"}");
					}
					else if (!this.bDate(val4, false, ref now2))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的失效日期\"}");
					}
					else if (!this.bInt(val5, ref stockNum))
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的优惠券发放量\"}");
					}
					else
					{
						bool flag = bool.Parse(value);
						if (!flag)
						{
							if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text4))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"请选择正确的会员等级\"}");
								return;
							}
							memberGrades = text2;
						}
						if (!this.bInt(val6, ref maxReceivNum))
						{
							context.Response.Write("{\"type\":\"error\",\"data\":\"请选择正确的优惠券最大领取张数\"}");
						}
						else
						{
							this.bInt(val7, ref num3);
							if (num2 < num && num2 != 0m)
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"优惠券面值不能大于满足金额\"}");
							}
							else if (now2 < now)
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"优惠券失效日期不能早于生效日期\"}");
							}
							else
							{
								CouponInfo couponInfo = new CouponInfo();
								couponInfo.CouponName = couponName;
								couponInfo.CouponValue = num;
								couponInfo.ConditionValue = num2;
								couponInfo.BeginDate = now;
								couponInfo.EndDate = now2;
								couponInfo.StockNum = stockNum;
								couponInfo.IsAllProduct = (num3 == 0);
								if (flag)
								{
									couponInfo.MemberGrades = "0";
									couponInfo.DefualtGroup = "0";
									couponInfo.CustomGroup = "0";
								}
								else
								{
									couponInfo.MemberGrades = memberGrades;
									couponInfo.DefualtGroup = text3;
									couponInfo.CustomGroup = text4;
								}
								couponInfo.maxReceivNum = maxReceivNum;
								couponInfo.CouponTypes = couponTypes;
								CouponActionStatus couponActionStatus = CouponHelper.CreateCoupon(couponInfo);
								if (couponActionStatus == CouponActionStatus.Success)
								{
									couponInfo = CouponHelper.GetCoupon(couponName);
									if (couponInfo != null)
									{
										context.Response.Write("{\"type\":\"success\",\"data\":\"" + couponInfo.CouponId.ToString() + "\"}");
									}
									else
									{
										context.Response.Write("{\"type\":\"error\",\"data\":\"写数据库异常\"}");
									}
								}
								else
								{
									context.Response.Write("{\"type\":\"error\",\"data\":\"" + this.GetErrMsg(couponActionStatus.ToString()) + "\"}");
								}
							}
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}

		private bool bInt(string val, ref int i)
		{
			if (string.IsNullOrEmpty(val))
			{
				return false;
			}
			i = 0;
			return !val.Contains(".") && !val.Contains("-") && int.TryParse(val, out i);
		}

		private bool bDecimal(string val, ref decimal i)
		{
			if (string.IsNullOrEmpty(val))
			{
				return false;
			}
			i = 0m;
			return !val.Contains("-") && decimal.TryParse(val, out i);
		}

		private bool bDate(string val, bool flag, ref System.DateTime i)
		{
			return !string.IsNullOrEmpty(val) && System.DateTime.TryParse(val, out i);
		}

		private string GetErrMsg(string msg)
		{
			string result = string.Empty;
			if (msg != null && msg == "DuplicateName")
			{
				result = "优惠券名称重复";
			}
			else
			{
				result = "写数据库异常";
			}
			return result;
		}
	}
}
