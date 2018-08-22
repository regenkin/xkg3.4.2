using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class OrderReturn : PaymentTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litMessage;

		public OrderReturn() : base(false)
		{
		}

		protected override void OnInit(System.EventArgs e)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-PaymentReturn.html";
			}
			base.OnInit(e);
		}

		protected override void AttachChildControls()
		{
			this.litMessage = (System.Web.UI.WebControls.Literal)this.FindControl("litMessage");
		}

		protected override void DisplayMessage(string status)
		{
			switch (status)
			{
			case "ordernotfound":
				this.litMessage.Text = string.Format("没有找到对应的订单信息，订单号：{0}", this.OrderId);
				return;
			case "gatewaynotfound":
				this.litMessage.Text = "没有找到与此订单对应的支付方式，系统无法自动完成操作，请联系管理员";
				return;
			case "verifyfaild":
				this.litMessage.Text = "支付返回验证失败，操作已停止";
				return;
			case "success":
				this.litMessage.Text = string.Format("恭喜您，订单已成功完成支付：{0}</br>支付金额：{1}", this.OrderId, this.Amount.ToString("F"));
				return;
			case "exceedordermax":
				this.litMessage.Text = "订单为团购订单，订购数量超过订购总数，支付失败";
				return;
			case "groupbuyalreadyfinished":
				this.litMessage.Text = "订单为团购订单，团购活动已结束，支付失败";
				return;
			case "fail":
				this.litMessage.Text = string.Format("订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", this.Amount.ToString("F"));
				return;
			}
			this.litMessage.Text = "未知错误，操作已停止";
		}
	}
}
