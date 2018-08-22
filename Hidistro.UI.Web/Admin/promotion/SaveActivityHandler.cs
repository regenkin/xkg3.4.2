using ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SaveActivityHandler : System.Web.IHttpHandler
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
			string text = Globals.RequestFormStr("action");
			if (!string.IsNullOrEmpty(text))
			{
				if (text == "End")
				{
					int num = Globals.RequestFormNum("delId");
					if (num > 0)
					{
						if (ActivityHelper.EndAct(num))
						{
							context.Response.Write("{\"state\":\"true\",\"msg\":\"活动成功结束\"}");
							return;
						}
						context.Response.Write("{\"state\":\"false\",\"msg\":\"活动结束失败\"}");
						return;
					}
					else
					{
						context.Response.Write("{\"state\":\"false\",\"msg\":\"参数不正确\"}");
					}
				}
				return;
			}
			try
			{
				int num2 = int.Parse(context.Request["id"].ToString());
				string text2 = context.Request["name"].ToString();
				string val = context.Request["begin"].ToString();
				string val2 = context.Request["end"].ToString();
				string text3 = context.Request["memberlvl"].ToString();
				string text4 = context.Request["defualtGroup"].ToString();
				string text5 = context.Request["customGroup"].ToString();
				string val3 = context.Request["maxNum"].ToString();
				string val4 = context.Request["type"].ToString();
				string val5 = context.Request["attendType"].ToString();
				string val6 = context.Request["meetType"].ToString();
				int attendType = 0;
				int meetType = 0;
				System.DateTime now = System.DateTime.Now;
				System.DateTime now2 = System.DateTime.Now;
				int attendTime = 0;
				int num3 = 0;
				if (text2.Length > 30)
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"活动名称不能超过30个字符\"}");
				}
				else if (!val.bDate(ref now))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的开始时间\"}");
				}
				else if (!val2.bDate(ref now2))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的结束时间\"}");
				}
				else if (now2 < now)
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"结束时间不能早于开始时间\"}");
				}
				else if (string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text4) && string.IsNullOrEmpty(text5))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请选择适用会员等级\"}");
				}
				else if (!val3.bInt(ref attendTime))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请选择参与次数\"}");
				}
				else if (!val4.bInt(ref num3))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请选择参与商品类型\"}");
				}
				else if (!val5.bInt(ref attendType))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请选择优惠类型\"}");
				}
				else if (!val6.bInt(ref meetType))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请选择优惠条件\"}");
				}
				else
				{
					System.Collections.Generic.List<ActivityDetailInfo> list = new System.Collections.Generic.List<ActivityDetailInfo>();
					JArray jArray = (JArray)JsonConvert.DeserializeObject(context.Request.Form["stk"].ToString());
					if (jArray.Count > 1)
					{
						for (int i = 0; i < jArray.Count - 1; i++)
						{
							JToken jToken = jArray[i]["meet"];
							for (int j = i + 1; j < jArray.Count; j++)
							{
								if (jArray[j]["meet"] == jToken)
								{
									context.Response.Write("{\"type\":\"error\",\"data\":\"多级优惠的各级满足金额不能相同\"}");
									return;
								}
							}
						}
					}
					if (jArray.Count > 0)
					{
						for (int k = 0; k < jArray.Count; k++)
						{
							ActivityDetailInfo activityDetailInfo = new ActivityDetailInfo();
							string val7 = jArray[k]["meet"].ToString();
							string val8 = jArray[k]["meetNumber"].ToString();
							string val9 = jArray[k]["redus"].ToString();
							string val10 = jArray[k]["free"].ToString();
							string val11 = jArray[k]["point"].ToString();
							string val12 = jArray[k]["coupon"].ToString();
							decimal num4 = 0m;
							int num5 = 0;
							decimal num6 = 0m;
							int integral = 0;
							int couponId = 0;
							int num7 = 0;
							if (!val8.bInt(ref num5))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠满足次数输入错误！\"}");
								return;
							}
							if (!val7.bDecimal(ref num4))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠满足金额输入错误！\"}");
								return;
							}
							if (!val9.bDecimal(ref num6))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠减免金额输入错误！\"}");
								return;
							}
							if (!val10.bInt(ref num7))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠免邮选择错误！\"}");
								return;
							}
							bool bFreeShipping = num7 != 0;
							if (!val11.bInt(ref integral))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠积分输入错误！\"}");
								return;
							}
							if (!val11.bInt(ref integral))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠积分输入错误！\"}");
								return;
							}
							if (!val12.bInt(ref couponId))
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠优惠券选择错误！\"}");
								return;
							}
							if (num5 == 0 && num6 > num4)
							{
								context.Response.Write("{\"type\":\"error\",\"data\":\"第" + (k + 1).ToString() + "级优惠减免金额不能大于满足金额！\"}");
								return;
							}
							activityDetailInfo.ActivitiesId = 0;
							activityDetailInfo.bFreeShipping = bFreeShipping;
							activityDetailInfo.CouponId = couponId;
							activityDetailInfo.MeetMoney = num4;
							activityDetailInfo.MeetNumber = num5;
							activityDetailInfo.ReductionMoney = num6;
							activityDetailInfo.Integral = integral;
							list.Add(activityDetailInfo);
						}
					}
					ActivityInfo activityInfo = new ActivityInfo();
					if (num2 != 0)
					{
						activityInfo = ActivityHelper.GetAct(num2);
						if (activityInfo == null)
						{
							context.Response.Write("{\"type\":\"error\",\"data\":\"没有找到这个活动\"}");
							return;
						}
					}
					activityInfo.ActivitiesName = text2;
					activityInfo.EndTime = now2.Date.AddDays(1.0).AddSeconds(-1.0);
					activityInfo.StartTime = now.Date;
					activityInfo.attendTime = attendTime;
					activityInfo.attendType = attendType;
					activityInfo.isAllProduct = (num3 == 0);
					activityInfo.MemberGrades = text3;
					activityInfo.DefualtGroup = text4;
					activityInfo.CustomGroup = text5;
					activityInfo.Type = new int?(0);
					activityInfo.MeetMoney = 0m;
					activityInfo.ReductionMoney = 0m;
					activityInfo.Details = list;
					activityInfo.MeetType = meetType;
					string str = "";
					int num8;
					if (num2 == 0)
					{
						num8 = ActivityHelper.Create(activityInfo, ref str);
					}
					else
					{
						num8 = activityInfo.ActivitiesId;
						if (!ActivityHelper.Update(activityInfo, ref str))
						{
							num8 = 0;
						}
					}
					if (num8 > 0)
					{
						context.Response.Write("{\"type\":\"success\",\"data\":\"" + num8.ToString() + "\"}");
					}
					else
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"" + str + "\"}");
					}
				}
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}
	}
}
