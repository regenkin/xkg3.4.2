using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Entities.Promotions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_GetActivityTopics : System.Web.IHttpHandler
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
			int num = 0;
			int.TryParse(context.Request.Params["act"], out num);
			string types = "0";
			if (num > 0 && num < 4)
			{
				types = num.ToString();
			}
			else if (num == 4)
			{
				types = "4,5";
			}
			System.Data.DataTable activityTopics = ActivityHelper.GetActivityTopics(types);
			activityTopics.Columns.Add("hasImage");
			activityTopics.Columns.Add("NewMemberGrades");
			activityTopics.Columns.Add("Url");
			activityTopics.Columns.Add("Limit");
			activityTopics.Columns.Add("Discount");
			activityTopics.Columns.Add("Description");
			activityTopics.Columns.Add("Point");
			activityTopics.Columns.Add("CouponMoeny");
			activityTopics.Columns.Add("Product");
			foreach (System.Data.DataRow dataRow in activityTopics.Rows)
			{
				int num2 = int.Parse(dataRow["Id"].ToString());
				int num3 = int.Parse(dataRow["ActivityType"].ToString());
				string a = dataRow["MemberGrades"].ToString();
				string a2 = dataRow["DefualtGroup"].ToString();
				string a3 = dataRow["CustomGroup"].ToString();
				if (num3 == 2 || num3 == 4 || num3 == 5)
				{
					dataRow["hasImage"] = "none";
				}
				else
				{
					dataRow["hasImage"] = "''";
				}
				if (a == "0" || a2 == "0" || a3 == "0")
				{
					dataRow["NewMemberGrades"] = "全部会员";
				}
				else
				{
					dataRow["NewMemberGrades"] = "部分会员";
				}
				if (num3 == 1)
				{
					dataRow["Url"] = "/ExchangeList.aspx?id=" + num2;
				}
				else if (num3 == 2)
				{
					dataRow["Url"] = "";
					ActivityInfo act = ActivityHelper.GetAct(num2);
					if (act != null)
					{
						dataRow["Limit"] = "每人参与" + act.attendTime.ToString() + "次";
						int meetType = act.MeetType;
						System.Data.DataTable activities_Detail = ActivityHelper.GetActivities_Detail(num2);
						string text = string.Empty;
						string text2 = "";
						if (act.attendType == 0)
						{
							foreach (System.Data.DataRow dataRow2 in activities_Detail.Rows)
							{
								if (meetType == 1)
								{
									text2 = text2 + "满" + dataRow2["MeetNumber"].ToString() + "件";
								}
								else
								{
									text2 = text2 + "满" + dataRow2["MeetMoney"].ToString() + "元";
								}
								if (decimal.Parse(dataRow2["ReductionMoney"].ToString()) != 0m)
								{
									text2 = text2 + "，减" + dataRow2["ReductionMoney"].ToString() + "元";
								}
								if (bool.Parse(dataRow2["bFreeShipping"].ToString()))
								{
									text2 += "，免邮";
								}
								if (int.Parse(dataRow2["Integral"].ToString()) != 0)
								{
									text2 = text2 + "，送" + dataRow2["Integral"].ToString() + "积分";
								}
								if (int.Parse(dataRow2["CouponId"].ToString()) != 0)
								{
									text2 += "，送优惠券";
								}
							}
							text += text2;
						}
						else
						{
							text = "多级优惠（每层级优惠不累积叠加）<br/>";
							int num4 = 0;
							foreach (System.Data.DataRow dataRow3 in activities_Detail.Rows)
							{
								num4++;
								text2 = text2 + "层级" + num4.ToString() + "：";
								if (meetType == 1)
								{
									text2 = text2 + "满" + dataRow3["MeetNumber"].ToString() + "件";
								}
								else
								{
									text2 = text2 + "满" + dataRow3["MeetMoney"].ToString() + "元";
								}
								if (decimal.Parse(dataRow3["ReductionMoney"].ToString()) != 0m)
								{
									text2 = text2 + "，减" + dataRow3["ReductionMoney"].ToString() + "元";
								}
								if (bool.Parse(dataRow3["bFreeShipping"].ToString()))
								{
									text2 += "，免邮";
								}
								if (int.Parse(dataRow3["Integral"].ToString()) != 0)
								{
									text2 = text2 + "，送" + dataRow3["Integral"].ToString() + "积分";
								}
								if (int.Parse(dataRow3["CouponId"].ToString()) != 0)
								{
									text2 += "，送优惠券";
								}
								text2 += "<br/>";
							}
							text += text2;
						}
						dataRow["Discount"] = text;
						if (act.isAllProduct)
						{
							dataRow["Product"] = "全部商品";
						}
						else
						{
							string text3 = string.Empty;
							System.Data.DataTable dataTable = ActivityHelper.QueryProducts(num2);
							foreach (System.Data.DataRow dataRow4 in dataTable.Rows)
							{
								if (dataRow4["status"].ToString() == "0")
								{
									text3 = text3 + dataRow4["ProductID"].ToString() + "_";
								}
							}
							if (!string.IsNullOrEmpty(text3))
							{
								dataRow["Url"] = "/ProductList.aspx?pIds=" + text3.TrimEnd(new char[]
								{
									'_'
								});
							}
							dataRow["Product"] = "部分商品";
						}
					}
				}
				else if (num3 == 3)
				{
					dataRow["Url"] = "/VShop/CouponDetails.aspx?CouponId=" + num2;
					CouponInfo coupon = CouponHelper.GetCoupon(num2);
					if (coupon != null)
					{
						dataRow["CouponMoeny"] = coupon.CouponValue.ToString();
						if (coupon.ConditionValue > 0m)
						{
							dataRow["Limit"] = "满" + coupon.ConditionValue.ToString() + "元可用";
						}
						else
						{
							dataRow["Limit"] = "不限制";
						}
					}
				}
				else if (num3 == 4)
				{
					dataRow["Url"] = "/BeginVote.aspx?voteId=" + num2;
					VoteInfo vote = VoteHelper.GetVote((long)num2);
					if (vote != null)
					{
						dataRow["Description"] = vote.Description;
					}
				}
				else if (num3 == 5)
				{
					GameInfo modelByGameId = GameHelper.GetModelByGameId(num2);
					if (modelByGameId != null)
					{
						dataRow["Url"] = modelByGameId.GameUrl;
						dataRow["Limit"] = this.GetLimit(modelByGameId.LimitEveryDay, modelByGameId.MaximumDailyLimit);
						dataRow["Point"] = modelByGameId.NeedPoint.ToString();
						dataRow["Description"] = modelByGameId.Description;
					}
				}
			}
			string s = JsonConvert.SerializeObject(activityTopics, Formatting.Indented, new JsonConverter[]
			{
				new IsoDateTimeConverter
				{
					DateTimeFormat = "yyyy-MM-dd HH:mm"
				}
			});
			context.Response.Write(s);
		}

		protected string GetLimit(object limitEveryDay, object maximumDailyLimit)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int num = (int)limitEveryDay;
			int num2 = (int)maximumDailyLimit;
			if (num == 0 && num2 == 0)
			{
				stringBuilder.Append("不限次");
			}
			else
			{
				if (num != 0)
				{
					stringBuilder.AppendFormat("每天参与{0}次", num);
				}
				stringBuilder.Append("<br/>");
				if (num2 != 0)
				{
					stringBuilder.AppendFormat("参与上限{0}次", num2);
				}
			}
			return stringBuilder.ToString();
		}
	}
}
