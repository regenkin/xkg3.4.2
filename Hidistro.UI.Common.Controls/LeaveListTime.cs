using System;
using System.Text;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public class LeaveListTime : Control
	{
		private string outStr = string.Empty;

		private string auto = "productId";

		private string bindData = "EndDate";

		private string startData = "StartDate";

		public string Auto
		{
			get
			{
				return this.auto;
			}
			set
			{
				this.auto = value;
			}
		}

		public string BindData
		{
			get
			{
				return this.bindData;
			}
			set
			{
				this.bindData = value;
			}
		}

		public string StartData
		{
			get
			{
				return this.startData;
			}
			set
			{
				this.startData = value;
			}
		}

		public override void DataBind()
		{
			int num = 1;
			int num2 = (int)DataBinder.Eval(this.Page.GetDataItem(), this.Auto);
			DateTime t = (DateTime)DataBinder.Eval(this.Page.GetDataItem(), this.BindData);
			DateTime dateTime = (DateTime)DataBinder.Eval(this.Page.GetDataItem(), this.StartData);
			if (t < DateTime.Now)
			{
				num = 0;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" <script type=\"text/javascript\"> ");
			stringBuilder.AppendFormat(" function LimitTimeBuyTimeShow_{0}()", num2.ToString());
			stringBuilder.Append(" { ");
			stringBuilder.AppendFormat(" showTimeList(\"{0}\",\"htmlspan{1}\",\"{2}\",\"{3}\");", new object[]
			{
				t.ToString("yyyy-MM-dd HH:mm:ss"),
				num2.ToString(),
				num,
				dateTime.ToString("yyyy-MM-dd HH:mm:ss")
			});
			stringBuilder.AppendFormat(" setTimeout(\"LimitTimeBuyTimeShow_{0}()\", 1000);", num2.ToString());
			stringBuilder.Append(" }");
			stringBuilder.AppendFormat(" LimitTimeBuyTimeShow_{0}(); ", num2.ToString());
			stringBuilder.Append(" </script>");
			this.outStr = stringBuilder.ToString();
			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.outStr.ToString());
		}
	}
}
