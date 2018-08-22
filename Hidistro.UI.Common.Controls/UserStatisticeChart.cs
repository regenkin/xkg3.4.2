using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;

namespace Hidistro.UI.Common.Controls
{
	public class UserStatisticeChart : IHttpHandler
	{
		public MemoryStream ShowChart_MS = new MemoryStream();

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(HttpContext context)
		{
			try
			{
				string chartType = string.Empty;
				string xValues = string.Empty;
				string yValues = string.Empty;
				int? width = null;
				if (!string.IsNullOrEmpty(context.Request.Params["ChartType"]) && !string.IsNullOrEmpty(context.Request.Params["XValues"]) && !string.IsNullOrEmpty(context.Request.Params["YValues"]))
				{
					chartType = context.Request.QueryString["ChartType"];
					xValues = context.Request.QueryString["XValues"];
					yValues = context.Request.QueryString["YValues"];
				}
				if (!string.IsNullOrEmpty(context.Request.Params["Width"]))
				{
					width = new int?(int.Parse(context.Request.Params["Width"]));
				}
				this.ShowChar(chartType, xValues, yValues, width);
				context.Response.ClearContent();
				context.Response.ContentType = "image/png";
				context.Response.BinaryWrite(this.ShowChart_MS.ToArray());
			}
			catch
			{
			}
		}

		public void ShowChar(string ChartType, string XValues, string YValues, int? width)
		{
			if (ChartType != null)
			{
				string text = "false";
				bool flag;
				if (text == null)
				{
					flag = false;
				}
				else
				{
					try
					{
						flag = Convert.ToBoolean(text);
					}
					catch
					{
						flag = false;
					}
				}
				if (XValues != null && YValues != null)
				{
					string[] num = YValues.Split("|".ToCharArray());
					int verticalTickCount = 6;
					int num2 = this.FunMaxNum(num);
					if (num2 < 5)
					{
						verticalTickCount = num2 + 1;
					}
					Color bgColor;
					if (flag)
					{
						bgColor = Color.White;
					}
					else
					{
						bgColor = Color.FromArgb(255, 253, 244);
					}
					Bitmap bitmap = null;
					if (ChartType != null && ChartType == "bar")
					{
						CustomChart.BarGraph barGraph = new CustomChart.BarGraph(bgColor);
						barGraph.VerticalLabel = "";
						barGraph.VerticalTickCount = verticalTickCount;
						barGraph.ShowLegend = false;
						barGraph.ShowData = true;
						barGraph.Height = 200;
						barGraph.Width = (width.HasValue ? width.Value : 600);
						barGraph.CollectDataPoints(XValues.Split("|".ToCharArray()), YValues.Split("|".ToCharArray()));
						bitmap = barGraph.Draw();
					}
					bitmap.Save(this.ShowChart_MS, ImageFormat.Png);
				}
			}
		}

		public int FunMaxNum(string[] num)
		{
			decimal num2 = Convert.ToDecimal(num[0]);
			for (int i = 1; i < num.Length; i++)
			{
				if (Convert.ToDecimal(num[i]) > num2)
				{
					num2 = Convert.ToDecimal(num[i]);
				}
			}
			return Convert.ToInt32(num2);
		}
	}
}
