using Hidistro.ControlPanel.OutPay;
using Hidistro.Entities.VShop;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.Admin.OutPay
{
	public class OneyungRefund : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = "";
			string text2 = base.Request.QueryString["vaid"];
			if (!string.IsNullOrEmpty(text2))
			{
				System.Collections.Generic.List<string> participantPids = OneyuanTaoHelp.GetParticipantPids(text2, true, false, "alipay");
				if (participantPids.Count == 0)
				{
					base.Response.Write("没有适合支付宝退款的活动参与记录！");
					return;
				}
				text = string.Join(",", participantPids);
			}
			if (text == "")
			{
				text = base.Request.QueryString["pids"];
			}
			if (string.IsNullOrEmpty(text))
			{
				base.Response.Write("非正常访问！");
				return;
			}
			text = text.Replace("\u3000", "").Replace(" ", "").Trim();
			string[] pIds = text.Split(new char[]
			{
				','
			});
			System.Collections.Generic.IList<OneyuanTaoParticipantInfo> refundParticipantList = OneyuanTaoHelp.GetRefundParticipantList(pIds);
			if (refundParticipantList == null)
			{
				base.Response.Write("获取夺宝信息失败,可能信息已删除！");
				return;
			}
			System.Collections.Generic.List<alipayReturnInfo> list = new System.Collections.Generic.List<alipayReturnInfo>();
			foreach (OneyuanTaoParticipantInfo current in refundParticipantList)
			{
				if (current.IsPay && !current.IsRefund && !current.IsWin && !string.IsNullOrEmpty(current.PayNum) && !string.IsNullOrEmpty(current.PayWay) && !(current.PayWay == "weixin"))
				{
					alipayReturnInfo item = new alipayReturnInfo
					{
						alipaynum = current.PayNum,
						refundNum = current.TotalPrice,
						Remark = "一元夺宝退款,对应活动编码：" + current.ActivityId
					};
					list.Add(item);
				}
			}
			if (list.Count == 0)
			{
				base.Response.Write("当前选择的退款记录不符号退款条件，为非支付宝付款记录！");
				return;
			}
			string notify_url = string.Format("http://{0}/Admin/OutPay/OneyuanAlipayRefundNotify.aspx", base.Request.Url.Host);
			string s = RefundHelper.AlipayRefundRequest(notify_url, list);
			base.Response.Write(s);
			base.Response.End();
		}
	}
}
