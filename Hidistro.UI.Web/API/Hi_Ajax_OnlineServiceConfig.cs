using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Orders;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Data;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_OnlineServiceConfig : System.Web.IHttpHandler
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
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember != null)
			{
				MemberGradeInfo memberGrade = MemberProcessor.GetMemberGrade(currentMember.GradeId);
				OrderInfo userLastOrder = MemberProcessor.GetUserLastOrder(currentMember.UserId);
				string arg = (!string.IsNullOrEmpty(currentMember.UserBindName)) ? currentMember.UserBindName : currentMember.UserName;
				string arg2 = (!string.IsNullOrEmpty(currentMember.OpenId)) ? currentMember.UserName : string.Empty;
				int port = context.Request.Url.Port;
				string text = (port == 80) ? "" : (":" + port.ToString());
				string.Concat(new string[]
				{
					"http://",
					context.Request.Url.Host,
					text,
					Globals.ApplicationPath,
					"/Admin/member/managemembers.aspx?Username=",
					currentMember.UserName,
					"&pageSize=10"
				});
				string arg3 = currentMember.UserBindName + "【" + ((memberGrade != null) ? memberGrade.Name : "普通会员") + "】";
				string arg4 = currentMember.OrderNumber.ToString() + "单（￥" + currentMember.Expenditure.ToString("F2") + "）";
				string arg5 = (userLastOrder != null) ? userLastOrder.OrderDate.ToString("yyyy-MM-dd HH:mm:ss") : string.Empty;
				string arg6 = string.Empty;
				string arg7 = string.Empty;
				string arg8 = string.Empty;
				string arg9 = string.Empty;
				int currentMemberUserId = Globals.GetCurrentMemberUserId();
				DistributorsInfo userIdDistributors = DistributorsBrower.GetUserIdDistributors(currentMemberUserId);
				if (userIdDistributors != null)
				{
					DistributorGradeInfo distributorGradeInfo = DistributorGradeBrower.GetDistributorGradeInfo(userIdDistributors.DistriGradeId);
					string text2 = "0.00";
					DistributorsInfo distributorInfo = DistributorsBrower.GetDistributorInfo(userIdDistributors.UserId);
					if (distributorInfo != null)
					{
						text2 = distributorInfo.ReferralBlance.ToString("F2");
					}
					string str = "0";
					string str2 = "0";
					System.Data.DataTable distributorsSubStoreNum = VShopHelper.GetDistributorsSubStoreNum(userIdDistributors.UserId);
					if (distributorsSubStoreNum != null || distributorsSubStoreNum.Rows.Count > 0)
					{
						str = distributorsSubStoreNum.Rows[0]["firstV"].ToString();
						str2 = distributorsSubStoreNum.Rows[0]["secondV"].ToString();
					}
					string.Concat(new string[]
					{
						"http://",
						context.Request.Url.Host,
						text,
						Globals.ApplicationPath,
						"/Admin/Fenxiao/distributorlist.aspx?MicroSignal=",
						currentMember.UserName,
						"&Status=0&pageSize=10"
					});
					arg6 = userIdDistributors.StoreName + "【" + distributorGradeInfo.Name + "】";
					arg7 = "￥" + userIdDistributors.OrdersTotal.ToString("F2");
					arg8 = string.Concat(new string[]
					{
						"￥",
						text2,
						"（待提现￥",
						userIdDistributors.ReferralBlance.ToString("F2"),
						"，已提现￥",
						userIdDistributors.ReferralRequestBalance.ToString("F2"),
						"）"
					});
					arg9 = "一级分店数" + str + "，二级分店数" + str2;
				}
				stringBuilder.Append("<script>");
				stringBuilder.Append("var mechatMetadataInter = setInterval(function() {");
				stringBuilder.Append("if (window.mechatMetadata) {");
				stringBuilder.Append("clearInterval(mechatMetadataInter);");
				stringBuilder.Append("window.mechatMetadata({");
				stringBuilder.AppendFormat("appUserName: '{0}',", arg);
				stringBuilder.AppendFormat("appNickName: '{0}',", currentMember.UserName);
				stringBuilder.AppendFormat("realName: '{0}',", currentMember.RealName);
				stringBuilder.AppendFormat("avatar: '{0}',", currentMember.UserHead);
				stringBuilder.AppendFormat("tel: '{0}',", currentMember.CellPhone);
				stringBuilder.AppendFormat("email: '{0}',", currentMember.Email);
				stringBuilder.AppendFormat("QQ: '{0}',", currentMember.QQ);
				stringBuilder.AppendFormat("weibo: '',", new object[0]);
				stringBuilder.AppendFormat("weixin: '{0}',", arg2);
				stringBuilder.AppendFormat("address: '{0}',", currentMember.Address);
				stringBuilder.Append("extraParams: {");
				stringBuilder.AppendFormat("'会员帐号': '{0}',", arg3);
				stringBuilder.AppendFormat("'会员订单': '{0}',", arg4);
				stringBuilder.AppendFormat("'会员积分': '{0}',", currentMember.Points.ToString());
				stringBuilder.AppendFormat("'最近购买': '{0}',", arg5);
				stringBuilder.AppendFormat("'店铺名称': '{0}',", arg6);
				stringBuilder.AppendFormat("'销售额': '{0}',", arg7);
				stringBuilder.AppendFormat("'佣金信息': '{0}',", arg8);
				stringBuilder.AppendFormat("'分店数量': '{0}'", arg9);
				stringBuilder.Append("}");
				stringBuilder.Append("});");
				stringBuilder.Append("}");
				stringBuilder.Append("}, 500);");
				stringBuilder.Append("</script>");
			}
			context.Response.Write(stringBuilder.ToString() + " ");
		}
	}
}
