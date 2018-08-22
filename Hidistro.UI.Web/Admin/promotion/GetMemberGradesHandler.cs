using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class GetMemberGradesHandler : System.Web.IHttpHandler
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
			string text = context.Request.QueryString["action"];
			if (string.IsNullOrEmpty(text))
			{
				this.GetMemberGrade(context);
				return;
			}
			string key;
			switch (key = text.ToLower())
			{
			case "getmembergrade":
				this.GetMemberGrade(context);
				return;
			case "getcoupontype":
				this.GetCouponType(context);
				return;
			case "getcouponinfo":
				this.GetCouponInfo(context);
				return;
			case "setregisternosendcoupon":
				this.SetRegisterNoSendCoupon(context);
				return;
			case "getstroeidbystorename":
				this.GetStoreIdByStoreName(context);
				return;
			case "getsearchusercount":
				this.GetSearchUserCount(context);
				return;
			case "sendcoupontosearchuser":
				this.SendCouponToSearchUser(context);
				return;
			case "getuseridbynich":
				this.GetUserIdByNiCh(context);
				return;
			case "getuseridbyusername":
				this.GetUserIdByUserName(context);
				return;
			case "sendcoupontousers":
				this.SendCouponToUser(context);
				return;
			case "getusercustomgroup":
				this.GetUserCustomGroup(context);
				return;
			case "getusercustomgroupandgrade":
				this.GetUserCustomGroupAndGrade(context);
				return;
			case "getmembergroup":
				this.GetMemberGroup(context);
				break;

				return;
			}
		}

		private void GetMemberGrade(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			try
			{
				new System.Text.StringBuilder();
				System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
				System.Collections.Generic.List<SimpleGradeClass> list = new System.Collections.Generic.List<SimpleGradeClass>();
				if (memberGrades.Count > 0)
				{
					foreach (MemberGradeInfo current in memberGrades)
					{
						list.Add(new SimpleGradeClass
						{
							GradeId = current.GradeId,
							Name = current.Name
						});
					}
				}
				var value = new
				{
					type = "success",
					data = list
				};
				string s = JsonConvert.SerializeObject(value);
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",data:\"" + ex.Message + "\"}");
			}
		}

		private void GetCouponType(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Collections.Generic.IList<CouponTypes> list = new System.Collections.Generic.List<CouponTypes>();
			foreach (int num in System.Enum.GetValues(typeof(CouponType)))
			{
				list.Add(new CouponTypes
				{
					id = num,
					Name = ((CouponType)num).ToString()
				});
			}
			var value = new
			{
				type = "success",
				data = list
			};
			string s = JsonConvert.SerializeObject(value);
			context.Response.Write(s);
		}

		private void GetCouponInfo(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			int couponId = 0;
			if (!int.TryParse(context.Request["id"], out couponId))
			{
				stringBuilder.Append("\"Status\":\"0\"}");
				context.Response.Write(stringBuilder.ToString());
			}
			CouponInfo coupon = CouponHelper.GetCoupon(couponId);
			if (coupon == null)
			{
				stringBuilder.Append("\"Status\":\"1\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			var value = new
			{
				Status = 2,
				Count = coupon.StockNum - coupon.ReceiveNum,
				BeginTime = coupon.BeginDate,
				EndTime = coupon.EndDate
			};
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			context.Response.Write(JsonConvert.SerializeObject(value, Formatting.Indented, new JsonConverter[]
			{
				isoDateTimeConverter
			}));
		}

		private void SetRegisterNoSendCoupon(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			masterSettings.IsRegisterSendCoupon = false;
			try
			{
				SettingsManager.Save(masterSettings);
				stringBuilder.Append("\"Status\":\"ok\"}");
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"Status\":\"err\"}");
			}
			context.Response.Write(stringBuilder);
		}

		private void GetStoreIdByStoreName(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["storeName"];
			if (string.IsNullOrWhiteSpace(text))
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					description = "参数错误!"
				}));
				return;
			}
			int num = MemberHelper.IsExiteDistributorNames(text);
			if (num > 0)
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "ok",
					storeId = num
				}));
				return;
			}
			context.Response.Write(JsonConvert.SerializeObject(new
			{
				status = "err",
				description = "上级店铺名称不存在!"
			}));
		}

		private void GetSearchUserCount(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			var value = new
			{
				status = "err",
				msg = "参数错误！"
			};
			string text = context.Request["gId"];
			if (string.IsNullOrWhiteSpace(text))
			{
				context.Response.Write(JsonConvert.SerializeObject(value));
				return;
			}
			string text2 = context.Request["rId"];
			if (!string.IsNullOrWhiteSpace(text2))
			{
				try
				{
					int.Parse(text2);
				}
				catch (System.Exception)
				{
					context.Response.Write(JsonConvert.SerializeObject(value));
					return;
				}
			}
			string text3 = context.Request["bDate"];
			if (!string.IsNullOrWhiteSpace(text3))
			{
				try
				{
					System.DateTime.Parse(text3);
				}
				catch (System.Exception)
				{
					context.Response.Write(JsonConvert.SerializeObject(value));
					return;
				}
			}
			string text4 = context.Request["eDate"];
			if (!string.IsNullOrWhiteSpace(text4))
			{
				try
				{
					System.DateTime.Parse(text4);
				}
				catch (System.Exception)
				{
					context.Response.Write(JsonConvert.SerializeObject(value));
					return;
				}
			}
			string text5 = context.Request["uType"];
			if (string.IsNullOrWhiteSpace(text5))
			{
				context.Response.Write(JsonConvert.SerializeObject(value));
			}
			else
			{
				string text6 = context.Request["cGroup"];
				if (string.IsNullOrWhiteSpace(text6))
				{
					context.Response.Write(JsonConvert.SerializeObject(value));
					return;
				}
				try
				{
					int.Parse(text5);
				}
				catch (System.Exception)
				{
					context.Response.Write(JsonConvert.SerializeObject(value));
					return;
				}
				try
				{
					int memeberNumBySearch = CouponHelper.GetMemeberNumBySearch(text, text2, text3, text4, int.Parse(text5), text6);
					var value2 = new
					{
						status = "ok",
						msg = "",
						count = memeberNumBySearch
					};
					context.Response.Write(JsonConvert.SerializeObject(value2));
				}
				catch (System.Exception)
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "err",
						msg = "获取会员数出错！"
					}));
				}
			}
		}

		private void SendCouponToSearchUser(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int couponId = 0;
			if (!int.TryParse(context.Request["couponId"], out couponId))
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "参数错误！"
				}));
				return;
			}
			try
			{
				bool flag = CouponHelper.SendCouponToMemebers(couponId);
				if (flag)
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "ok",
						msg = "发送成功！"
					}));
				}
				else
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "err",
						msg = "发送失败！"
					}));
				}
			}
			catch (System.Exception)
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "发送出错！"
				}));
			}
		}

		private void GetUserIdByNiCh(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["nich"];
			if (string.IsNullOrWhiteSpace(text))
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "参数错误！"
				}));
				return;
			}
			try
			{
				string[] array = text.Split(new char[]
				{
					'_'
				});
				System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					int memberIdByUserNameOrNiChen = MemberHelper.GetMemberIdByUserNameOrNiChen(null, text2);
					if (memberIdByUserNameOrNiChen > 0)
					{
						list.Add(memberIdByUserNameOrNiChen);
					}
					else if (stringBuilder.Length <= 0)
					{
						stringBuilder.AppendFormat("{0}", text2);
					}
					else
					{
						stringBuilder.AppendFormat(",{0}", text2);
					}
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" 无效！");
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "err",
						msg = "微信昵称 " + stringBuilder.ToString()
					}));
				}
				else
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "ok",
						msg = "",
						ids = string.Join<int>("_", list),
						count = list.Count
					}));
				}
			}
			catch (System.Exception)
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "程序出错！"
				}));
			}
		}

		private void GetUserIdByUserName(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["uname"];
			if (string.IsNullOrWhiteSpace(text))
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "参数错误！"
				}));
				return;
			}
			try
			{
				string[] array = text.Split(new char[]
				{
					'_'
				});
				System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					int memberIdByUserNameOrNiChen = MemberHelper.GetMemberIdByUserNameOrNiChen(text2, null);
					if (memberIdByUserNameOrNiChen > 0)
					{
						list.Add(memberIdByUserNameOrNiChen);
					}
					else if (stringBuilder.Length <= 0)
					{
						stringBuilder.AppendFormat("{0}", text2);
					}
					else
					{
						stringBuilder.AppendFormat(",{0}", text2);
					}
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" 无效！");
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "err",
						msg = "用户名 " + stringBuilder.ToString()
					}));
				}
				else
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "ok",
						msg = "",
						ids = string.Join<int>("_", list),
						count = list.Count
					}));
				}
			}
			catch (System.Exception)
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "程序出错！"
				}));
			}
		}

		private void SendCouponToUser(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string text = context.Request["ids"];
			if (string.IsNullOrWhiteSpace(text))
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "参数错误！"
				}));
				return;
			}
			int couponId = 0;
			if (!int.TryParse(context.Request["cId"], out couponId))
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "参数错误！"
				}));
				return;
			}
			try
			{
				bool flag = CouponHelper.SendCouponToMemebers(couponId, text);
				if (flag)
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "ok",
						msg = "发送成功！"
					}));
				}
				else
				{
					context.Response.Write(JsonConvert.SerializeObject(new
					{
						status = "err",
						msg = "发送失败！"
					}));
				}
			}
			catch (System.Exception)
			{
				context.Response.Write(JsonConvert.SerializeObject(new
				{
					status = "err",
					msg = "程序出错！"
				}));
			}
		}

		private void GetUserCustomGroupAndGrade(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			try
			{
				new System.Text.StringBuilder();
				System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
				System.Collections.Generic.List<CustomGroup> list = new System.Collections.Generic.List<CustomGroup>();
				if (customGroupingList.Count > 0)
				{
					foreach (CustomGroupingInfo current in customGroupingList)
					{
						list.Add(new CustomGroup
						{
							id = current.Id,
							Name = current.GroupName
						});
					}
				}
				new System.Text.StringBuilder();
				System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
				System.Collections.Generic.List<SimpleGradeClass> list2 = new System.Collections.Generic.List<SimpleGradeClass>();
				if (memberGrades.Count > 0)
				{
					foreach (MemberGradeInfo current2 in memberGrades)
					{
						list2.Add(new SimpleGradeClass
						{
							GradeId = current2.GradeId,
							Name = current2.Name
						});
					}
				}
				var value = new
				{
					type = "success",
					data = list,
					gradedata = list2
				};
				string s = JsonConvert.SerializeObject(value);
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",data:\"" + ex.Message + "\"}");
			}
		}

		private void GetUserCustomGroup(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			try
			{
				new System.Text.StringBuilder();
				System.Collections.Generic.IList<CustomGroupingInfo> customGroupingList = CustomGroupingHelper.GetCustomGroupingList();
				System.Collections.Generic.List<CustomGroup> list = new System.Collections.Generic.List<CustomGroup>();
				if (customGroupingList.Count > 0)
				{
					foreach (CustomGroupingInfo current in customGroupingList)
					{
						list.Add(new CustomGroup
						{
							id = current.Id,
							Name = current.GroupName
						});
					}
				}
				var value = new
				{
					type = "success",
					data = list
				};
				string s = JsonConvert.SerializeObject(value);
				context.Response.Write(s);
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",data:\"" + ex.Message + "\"}");
			}
		}

		private void GetMemberGroup(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			int userId = 0;
			if (!int.TryParse(context.Request["userId"], out userId))
			{
				stringBuilder.Append("\"Status\":\"ok\"}");
			}
			else
			{
				string memberGroupList = CustomGroupingHelper.GetMemberGroupList(userId);
				stringBuilder.Append("\"Status\":\"" + memberGroupList + "\"}");
			}
			context.Response.Write(stringBuilder);
		}
	}
}
