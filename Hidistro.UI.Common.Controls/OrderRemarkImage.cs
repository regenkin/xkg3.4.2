using Hidistro.Entities.Orders;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OrderRemarkImage : Literal
	{
		private string imageFormat = "<span class=\"glyphicon {0} help\" style=\"color:{1}\"></span>";

		private int managerMarkValue;

		private string dataField;

		public int ManagerMarkValue
		{
			get
			{
				return this.managerMarkValue;
			}
			set
			{
				this.managerMarkValue = value;
			}
		}

		public string DataField
		{
			get
			{
				return this.dataField;
			}
			set
			{
				this.dataField = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.managerMarkValue > 0)
			{
				base.Text = this.GetImageSrc(this.managerMarkValue);
			}
			base.Render(writer);
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (this.managerMarkValue <= 0)
			{
				object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
				if (obj != null && obj != DBNull.Value)
				{
					base.Text = this.GetImageSrc(obj);
				}
				else
				{
					base.Text = string.Format(this.imageFormat, "glyphicon-flag", "#ababab");
				}
			}
			base.OnDataBinding(e);
		}

		protected string GetImageSrc(object managerMark)
		{
			string arg_05_0 = string.Empty;
			string arg = "glyphicon-flag";
			string arg2 = "#ababab";
			switch ((OrderMark)managerMark)
			{
			case OrderMark.Draw:
				arg = "glyphicon-ok";
				arg2 = "#309930";
				break;
			case OrderMark.ExclamationMark:
				arg = "glyphicon-exclamation-sign";
				arg2 = "#CB1E02";
				break;
			case OrderMark.Red:
				arg2 = "#CB1E02";
				break;
			case OrderMark.Green:
				arg2 = "#4E994E";
				break;
			case OrderMark.Yellow:
				arg2 = "#FFC500";
				break;
			case OrderMark.Gray:
				arg2 = "#ABABAB";
				break;
			}
			return string.Format(this.imageFormat, arg, arg2);
		}
	}
}
