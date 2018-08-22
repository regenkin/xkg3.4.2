using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;

namespace Hidistro.UI.Common.Controls
{
	public class CustomChart
	{
		public class ChartItem
		{
			private Color _color;

			private string _description;

			private string _label;

			private float _startPos;

			private float _sweepSize;

			private float _value;

			public string Description
			{
				get
				{
					return this._description;
				}
				set
				{
					this._description = value;
				}
			}

			public Color ItemColor
			{
				get
				{
					return this._color;
				}
				set
				{
					this._color = value;
				}
			}

			public string Label
			{
				get
				{
					return this._label;
				}
				set
				{
					this._label = value;
				}
			}

			public float StartPos
			{
				get
				{
					return this._startPos;
				}
				set
				{
					this._startPos = value;
				}
			}

			public float SweepSize
			{
				get
				{
					return this._sweepSize;
				}
				set
				{
					this._sweepSize = value;
				}
			}

			public float Value
			{
				get
				{
					return this._value;
				}
				set
				{
					this._value = value;
				}
			}

			private ChartItem()
			{
			}

			public ChartItem(string label, string desc, float data, float start, float sweep, Color clr)
			{
				this._label = label;
				this._description = desc;
				this._value = data;
				this._startPos = start;
				this._sweepSize = sweep;
				this._color = clr;
			}
		}

		public class ChartItemsCollection : CollectionBase
		{
			public CustomChart.ChartItem this[int index]
			{
				get
				{
					return (CustomChart.ChartItem)base.List[index];
				}
				set
				{
					base.List[index] = value;
				}
			}

			public int Add(CustomChart.ChartItem value)
			{
				return base.List.Add(value);
			}

			public bool Contains(CustomChart.ChartItem value)
			{
				return base.List.Contains(value);
			}

			public int IndexOf(CustomChart.ChartItem value)
			{
				return base.List.IndexOf(value);
			}

			public void Remove(CustomChart.ChartItem value)
			{
				base.List.Remove(value);
			}
		}

		public abstract class Chart
		{
			private Color[] _color = new Color[]
			{
				Color.Chocolate,
				Color.YellowGreen,
				Color.Olive,
				Color.DarkKhaki,
				Color.Sienna,
				Color.PaleGoldenrod,
				Color.Peru,
				Color.Tan,
				Color.Khaki,
				Color.DarkGoldenrod,
				Color.Maroon,
				Color.OliveDrab
			};

			private CustomChart.ChartItemsCollection _dataPoints = new CustomChart.ChartItemsCollection();

			public CustomChart.ChartItemsCollection DataPoints
			{
				get
				{
					return this._dataPoints;
				}
				set
				{
					this._dataPoints = value;
				}
			}

			public abstract Bitmap Draw();

			public Color GetColor(int index)
			{
				if (index == 11)
				{
					return this._color[index];
				}
				return this._color[index % 12];
			}

			public void SetColor(int index, Color NewColor)
			{
				if (index == 11)
				{
					this._color[index] = NewColor;
					return;
				}
				this._color[index % 12] = NewColor;
			}
		}

		public class PieChart : CustomChart.Chart
		{
			private const int _bufferSpace = 125;

			private Color _backgroundColor;

			private Color _borderColor;

			private ArrayList _chartItems;

			private int _legendFontHeight;

			private float _legendFontSize;

			private string _legendFontStyle;

			private int _legendHeight;

			private int _legendWidth;

			private int _perimeter;

			private float _total;

			public PieChart()
			{
				this._chartItems = new ArrayList();
				this._perimeter = 250;
				this._backgroundColor = Color.White;
				this._borderColor = Color.FromArgb(63, 63, 63);
				this._legendFontSize = 8f;
				this._legendFontStyle = "Verdana";
			}

			public PieChart(Color bgColor)
			{
				this._chartItems = new ArrayList();
				this._perimeter = 250;
				this._backgroundColor = bgColor;
				this._borderColor = Color.FromArgb(63, 63, 63);
				this._legendFontSize = 8f;
				this._legendFontStyle = "Verdana";
			}

			private void CalculateLegendWidthHeight()
			{
				Font font = new Font(this._legendFontStyle, this._legendFontSize);
				this._legendFontHeight = font.Height + 5;
				this._legendHeight = font.Height * (this._chartItems.Count + 1);
				if (this._legendHeight > this._perimeter)
				{
					this._perimeter = this._legendHeight;
				}
				this._legendWidth = this._perimeter + 125;
			}

			public void CollectDataPoints(string[] xValues, string[] yValues)
			{
				this._total = 0f;
				for (int i = 0; i < xValues.Length; i++)
				{
					float num = Convert.ToSingle(yValues[i]);
					this._chartItems.Add(new CustomChart.ChartItem(xValues[i], xValues.ToString(), num, 0f, 0f, Color.AliceBlue));
					this._total += num;
				}
				float startPos = 0f;
				int num2 = 0;
				foreach (CustomChart.ChartItem chartItem in this._chartItems)
				{
					chartItem.StartPos = startPos;
					chartItem.SweepSize = chartItem.Value / this._total * 360f;
					startPos = chartItem.StartPos + chartItem.SweepSize;
					chartItem.ItemColor = base.GetColor(num2++);
				}
				this.CalculateLegendWidthHeight();
			}

			public override Bitmap Draw()
			{
				int perimeter = this._perimeter;
				Rectangle rect = new Rectangle(0, 0, perimeter, perimeter - 1);
				Bitmap bitmap = new Bitmap(perimeter + this._legendWidth, perimeter);
				Graphics graphics = null;
				StringFormat stringFormat = null;
				try
				{
					graphics = Graphics.FromImage(bitmap);
					stringFormat = new StringFormat();
					graphics.FillRectangle(new SolidBrush(this._backgroundColor), 0, 0, perimeter + this._legendWidth, perimeter);
					stringFormat.Alignment = StringAlignment.Far;
					for (int i = 0; i < this._chartItems.Count; i++)
					{
						CustomChart.ChartItem chartItem = (CustomChart.ChartItem)this._chartItems[i];
						using (null)
						{
							SolidBrush brush = new SolidBrush(chartItem.ItemColor);
							graphics.FillPie(brush, rect, chartItem.StartPos, chartItem.SweepSize);
							graphics.FillRectangle(brush, perimeter + 125, i * this._legendFontHeight + 15, 10, 10);
							graphics.DrawString(chartItem.Label, new Font(this._legendFontStyle, this._legendFontSize), new SolidBrush(Color.Black), (float)(perimeter + 125 + 20), (float)(i * this._legendFontHeight + 13));
							graphics.DrawString(chartItem.Value.ToString("C"), new Font(this._legendFontStyle, this._legendFontSize), new SolidBrush(Color.Black), (float)(perimeter + 125 + 200), (float)(i * this._legendFontHeight + 13), stringFormat);
						}
					}
					graphics.DrawEllipse(new Pen(this._borderColor, 2f), rect);
					graphics.DrawRectangle(new Pen(this._borderColor, 1f), perimeter + 125 - 10, 10, 220, this._chartItems.Count * this._legendFontHeight + 25);
					graphics.DrawString("Total", new Font(this._legendFontStyle, this._legendFontSize, FontStyle.Bold), new SolidBrush(Color.Black), (float)(perimeter + 125 + 30), (float)((this._chartItems.Count + 1) * this._legendFontHeight), stringFormat);
					graphics.DrawString(this._total.ToString("C"), new Font(this._legendFontStyle, this._legendFontSize, FontStyle.Bold), new SolidBrush(Color.Black), (float)(perimeter + 125 + 200), (float)((this._chartItems.Count + 1) * this._legendFontHeight), stringFormat);
					graphics.SmoothingMode = SmoothingMode.AntiAlias;
				}
				finally
				{
					if (stringFormat != null)
					{
						stringFormat.Dispose();
					}
					if (graphics != null)
					{
						graphics.Dispose();
					}
				}
				return bitmap;
			}
		}

		public class BarGraph : CustomChart.Chart
		{
			private const float _graphLegendSpacer = 15f;

			private const int _labelFontSize = 7;

			private const int _legendFontSize = 9;

			private const float _legendRectangleSize = 10f;

			private const float _spacer = 5f;

			private Color _backColor;

			private float _barWidth;

			private float _bottomBuffer;

			private bool _displayBarData;

			private bool _displayLegend;

			private Color _fontColor;

			private string _fontFamily;

			private float _graphHeight;

			private float _graphWidth;

			private float _legendWidth;

			private string _longestLabel;

			private string _longestTickValue;

			private float _maxLabelWidth;

			private float _maxTickValueWidth;

			private float _maxValue;

			private float _scaleFactor;

			private float _spaceBtwBars;

			private float _topBuffer;

			private float _totalHeight;

			private float _totalWidth;

			private float _xOrigin;

			private string _yLabel;

			private float _yOrigin;

			private int _yTickCount;

			private float _yTickValue;

			public Color BackgroundColor
			{
				set
				{
					this._backColor = value;
				}
			}

			public int BottomBuffer
			{
				set
				{
					this._bottomBuffer = Convert.ToSingle(value);
				}
			}

			public Color FontColor
			{
				set
				{
					this._fontColor = value;
				}
			}

			public string FontFamily
			{
				get
				{
					return this._fontFamily;
				}
				set
				{
					this._fontFamily = value;
				}
			}

			public int Height
			{
				get
				{
					return Convert.ToInt32(this._totalHeight);
				}
				set
				{
					this._totalHeight = Convert.ToSingle(value);
				}
			}

			public bool ShowData
			{
				get
				{
					return this._displayBarData;
				}
				set
				{
					this._displayBarData = value;
				}
			}

			public bool ShowLegend
			{
				get
				{
					return this._displayLegend;
				}
				set
				{
					this._displayLegend = value;
				}
			}

			public int TopBuffer
			{
				set
				{
					this._topBuffer = Convert.ToSingle(value);
				}
			}

			public string VerticalLabel
			{
				get
				{
					return this._yLabel;
				}
				set
				{
					this._yLabel = value;
				}
			}

			public int VerticalTickCount
			{
				get
				{
					return this._yTickCount;
				}
				set
				{
					this._yTickCount = value;
				}
			}

			public int Width
			{
				get
				{
					return Convert.ToInt32(this._totalWidth);
				}
				set
				{
					this._totalWidth = Convert.ToSingle(value);
				}
			}

			public BarGraph()
			{
				this._longestTickValue = string.Empty;
				this._maxValue = 0f;
				this._longestLabel = string.Empty;
				this._maxLabelWidth = 0f;
				this.AssignDefaultSettings();
			}

			public BarGraph(Color bgColor)
			{
				this._longestTickValue = string.Empty;
				this._maxValue = 0f;
				this._longestLabel = string.Empty;
				this._maxLabelWidth = 0f;
				this.AssignDefaultSettings();
				this.BackgroundColor = bgColor;
			}

			private void AssignDefaultSettings()
			{
				this._totalWidth = 800f;
				this._totalHeight = 350f;
				this._fontFamily = "Verdana";
				this._backColor = Color.White;
				this._fontColor = Color.Black;
				this._topBuffer = 30f;
				this._bottomBuffer = 30f;
				this._yTickCount = 2;
				this._displayLegend = false;
				this._displayBarData = false;
			}

			private void CalculateBarWidth(int dataCount, float barGraphWidth)
			{
				this._barWidth = barGraphWidth / (float)(dataCount * 2);
				this._spaceBtwBars = barGraphWidth / (float)(dataCount * 2);
			}

			private void CalculateGraphDimension()
			{
				this.FindLongestTickValue();
				this._longestTickValue += "0";
				this._maxTickValueWidth = this.CalculateImgFontWidth(this._longestTickValue, 7, this.FontFamily);
				float num = 5f + this._maxTickValueWidth;
				float num2;
				if (this._displayLegend)
				{
					this._legendWidth = 20f + this._maxLabelWidth + 5f;
					num2 = 15f + this._legendWidth + 5f;
				}
				else
				{
					num2 = 5f;
				}
				this._graphHeight = this._totalHeight - this._topBuffer - this._bottomBuffer;
				this._graphWidth = this._totalWidth - num - num2;
				this._xOrigin = num;
				this._yOrigin = this._topBuffer;
				this._scaleFactor = this._maxValue / this._graphHeight;
			}

			private float CalculateImgFontWidth(string text, int size, string family)
			{
				Bitmap bitmap = null;
				Graphics graphics = null;
				Font font = null;
				float width;
				try
				{
					font = new Font(family, (float)size);
					bitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
					graphics = Graphics.FromImage(bitmap);
					width = graphics.MeasureString(text, font).Width;
				}
				finally
				{
					if (graphics != null)
					{
						graphics.Dispose();
					}
					if (bitmap != null)
					{
						bitmap.Dispose();
					}
					if (font != null)
					{
						font.Dispose();
					}
				}
				return width;
			}

			private void CalculateSweepValues()
			{
				int num = 0;
				foreach (CustomChart.ChartItem chartItem in base.DataPoints)
				{
					if (chartItem.Value >= 0f)
					{
						chartItem.SweepSize = chartItem.Value / this._scaleFactor;
					}
					chartItem.StartPos = this._spaceBtwBars / 2f + (float)num * (this._barWidth + this._spaceBtwBars);
					num++;
				}
			}

			private void CalculateTickAndMax()
			{
				this._maxValue *= 1.1f;
				float num;
				if (this._maxValue != 0f)
				{
					double y = Convert.ToDouble(Math.Floor(Math.Log10((double)this._maxValue)));
					num = Convert.ToSingle(Math.Ceiling((double)this._maxValue / Math.Pow(10.0, y)) * Math.Pow(10.0, y));
				}
				else
				{
					num = 1f;
				}
				this._yTickValue = num / (float)this._yTickCount;
				double y2 = Convert.ToDouble(Math.Floor(Math.Log10((double)this._yTickValue)));
				this._yTickValue = Convert.ToSingle(Math.Ceiling((double)this._yTickValue / Math.Pow(10.0, y2)) * Math.Pow(10.0, y2));
				this._maxValue = this._yTickValue * (float)this._yTickCount;
			}

			public void CollectDataPoints(string[] values)
			{
				this.CollectDataPoints(values, values);
			}

			public void CollectDataPoints(string[] labels, string[] values)
			{
				if (labels.Length != values.Length)
				{
					throw new Exception("X data count is different from Y data count");
				}
				for (int i = 0; i < labels.Length; i++)
				{
					float num = Convert.ToSingle(values[i]);
					string text = this.MakeShortLabel(labels[i]);
					base.DataPoints.Add(new CustomChart.ChartItem(text, labels[i], num, 0f, 0f, base.GetColor(i)));
					if (this._maxValue < num)
					{
						this._maxValue = num;
					}
					if (this._displayLegend)
					{
						string text2 = labels[i] + " (" + text + ")";
						float num2 = this.CalculateImgFontWidth(text2, 9, this.FontFamily);
						if (this._maxLabelWidth < num2)
						{
							this._longestLabel = text2;
							this._maxLabelWidth = num2;
						}
					}
				}
				this.CalculateTickAndMax();
				this.CalculateGraphDimension();
				this.CalculateBarWidth(base.DataPoints.Count, this._graphWidth);
				this.CalculateSweepValues();
			}

			public override Bitmap Draw()
			{
				int height = Convert.ToInt32(this._totalHeight);
				Bitmap bitmap = new Bitmap(Convert.ToInt32(this._totalWidth), height);
				using (Graphics graphics = Graphics.FromImage(bitmap))
				{
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.SmoothingMode = SmoothingMode.AntiAlias;
					Rectangle rect = new Rectangle(-1, -1, bitmap.Width + 1, bitmap.Height + 1);
					LinearGradientBrush brush = new LinearGradientBrush(rect, Color.FromArgb(246, 247, 251), Color.FromArgb(219, 228, 235), LinearGradientMode.Vertical);
					graphics.FillRectangle(brush, rect);
					this.DrawVerticalLabelArea(graphics);
					this.DrawBars(graphics);
					this.DrawXLabelArea(graphics);
					if (this._displayLegend)
					{
						this.DrawLegend(graphics);
					}
				}
				return bitmap;
			}

			private void DrawBars(Graphics graph)
			{
				SolidBrush solidBrush = null;
				Font font = null;
				StringFormat stringFormat = null;
				try
				{
					solidBrush = new SolidBrush(this._fontColor);
					font = new Font(this._fontFamily, 7f);
					stringFormat = new StringFormat();
					stringFormat.Alignment = StringAlignment.Center;
					int num = 0;
					foreach (CustomChart.ChartItem chartItem in base.DataPoints)
					{
						using (SolidBrush solidBrush2 = new SolidBrush(chartItem.ItemColor))
						{
							float num2 = (chartItem.SweepSize == 0f) ? 2f : chartItem.SweepSize;
							float num3 = this._yOrigin + this._graphHeight - num2;
							graph.FillRectangle(solidBrush2, this._xOrigin + chartItem.StartPos + (this._barWidth / 2f - 8f), num3, 16f, num2);
							if (this._displayBarData)
							{
								float x = this._xOrigin + (float)num * (this._barWidth + this._spaceBtwBars);
								float y = num3 - 2f - (float)font.Height;
								RectangleF layoutRectangle = new RectangleF(x, y, this._barWidth + this._spaceBtwBars, (float)font.Height);
								graph.DrawString((chartItem.Value.ToString(CultureInfo.InvariantCulture) == "0.0") ? "0" : chartItem.Value.ToString(CultureInfo.InvariantCulture), font, solidBrush, layoutRectangle, stringFormat);
							}
							num++;
						}
					}
				}
				finally
				{
					if (solidBrush != null)
					{
						solidBrush.Dispose();
					}
					if (font != null)
					{
						font.Dispose();
					}
					if (stringFormat != null)
					{
						stringFormat.Dispose();
					}
				}
			}

			private void DrawLegend(Graphics graph)
			{
				Font font = null;
				SolidBrush solidBrush = null;
				StringFormat stringFormat = null;
				Pen pen = null;
				try
				{
					font = new Font(this._fontFamily, 9f);
					solidBrush = new SolidBrush(this._fontColor);
					stringFormat = new StringFormat();
					pen = new Pen(this._fontColor);
					stringFormat.Alignment = StringAlignment.Near;
					float num = this._xOrigin + this._graphWidth + 6f;
					float yOrigin = this._yOrigin;
					float num2 = num + 5f;
					float x = num2 + 10f + 5f;
					float num3 = 0f;
					int num4 = 0;
					for (int i = 0; i < base.DataPoints.Count; i++)
					{
						CustomChart.ChartItem chartItem = base.DataPoints[i];
						string s = chartItem.Description + "(" + chartItem.Label + ")";
						num3 += (float)font.Height + 5f;
						float num5 = yOrigin + 5f + (float)(i - num4) * ((float)font.Height + 5f);
						graph.DrawString(s, font, solidBrush, x, num5, stringFormat);
						graph.FillRectangle(new SolidBrush(base.DataPoints[i].ItemColor), num2, num5 + 3f, 10f, 10f);
					}
					graph.DrawRectangle(pen, num, yOrigin, this._legendWidth, num3 + 5f);
				}
				finally
				{
					if (font != null)
					{
						font.Dispose();
					}
					if (solidBrush != null)
					{
						solidBrush.Dispose();
					}
					if (stringFormat != null)
					{
						stringFormat.Dispose();
					}
					if (pen != null)
					{
						pen.Dispose();
					}
				}
			}

			private void DrawVerticalLabelArea(Graphics graph)
			{
				Font font = null;
				SolidBrush solidBrush = null;
				StringFormat stringFormat = null;
				Pen pen = null;
				StringFormat stringFormat2 = null;
				try
				{
					font = new Font(this._fontFamily, 7f);
					solidBrush = new SolidBrush(this._fontColor);
					stringFormat = new StringFormat();
					pen = new Pen(this._fontColor);
					stringFormat2 = new StringFormat();
					stringFormat.Alignment = StringAlignment.Near;
					RectangleF layoutRectangle = new RectangleF(0f, this._yOrigin - 10f - (float)font.Height, this._xOrigin * 2f, (float)font.Height);
					stringFormat2.Alignment = StringAlignment.Center;
					graph.DrawString(this._yLabel, font, solidBrush, layoutRectangle, stringFormat2);
					for (int i = 0; i < this._yTickCount; i++)
					{
						float num = this._topBuffer + (float)i * this._yTickValue / this._scaleFactor;
						float y = num - (float)(font.Height / 2);
						RectangleF layoutRectangle2 = new RectangleF(5f, y, this._maxTickValueWidth, (float)font.Height);
						graph.DrawString((this._maxValue - (float)i * this._yTickValue).ToString("#,###.##"), font, solidBrush, layoutRectangle2, stringFormat);
						graph.DrawLine(pen, this._xOrigin, num, this._xOrigin - 4f, num);
					}
					graph.DrawLine(pen, this._xOrigin, this._yOrigin, this._xOrigin, this._yOrigin + this._graphHeight);
				}
				finally
				{
					if (font != null)
					{
						font.Dispose();
					}
					if (solidBrush != null)
					{
						solidBrush.Dispose();
					}
					if (stringFormat != null)
					{
						stringFormat.Dispose();
					}
					if (pen != null)
					{
						pen.Dispose();
					}
					if (stringFormat2 != null)
					{
						stringFormat2.Dispose();
					}
				}
			}

			private void DrawXLabelArea(Graphics graph)
			{
				Font font = null;
				SolidBrush solidBrush = null;
				StringFormat stringFormat = null;
				Pen pen = null;
				try
				{
					font = new Font(this._fontFamily, 7f);
					solidBrush = new SolidBrush(this._fontColor);
					stringFormat = new StringFormat();
					pen = new Pen(this._fontColor);
					stringFormat.Alignment = StringAlignment.Center;
					graph.DrawLine(pen, this._xOrigin, this._yOrigin + this._graphHeight, this._xOrigin + this._graphWidth, this._yOrigin + this._graphHeight);
					float y = this._yOrigin + this._graphHeight + 2f;
					float num = this._barWidth + this._spaceBtwBars;
					int num2 = 0;
					foreach (CustomChart.ChartItem chartItem in base.DataPoints)
					{
						float x = this._xOrigin + (float)num2 * num;
						RectangleF layoutRectangle = new RectangleF(x, y, num, (float)font.Height);
						string s = this._displayLegend ? chartItem.Label : chartItem.Description;
						graph.DrawString(s, font, solidBrush, layoutRectangle, stringFormat);
						num2++;
					}
				}
				finally
				{
					if (font != null)
					{
						font.Dispose();
					}
					if (solidBrush != null)
					{
						solidBrush.Dispose();
					}
					if (stringFormat != null)
					{
						stringFormat.Dispose();
					}
					if (pen != null)
					{
						pen.Dispose();
					}
				}
			}

			private void FindLongestTickValue()
			{
				for (int i = 0; i < this._yTickCount; i++)
				{
					string text = (this._maxValue - (float)i * this._yTickValue).ToString("#,###.##");
					if (this._longestTickValue.Length < text.Length)
					{
						this._longestTickValue = text;
					}
				}
			}

			private string MakeShortLabel(string text)
			{
				string result = text;
				if (text.Length > 2)
				{
					int startIndex = Convert.ToInt32(Math.Floor((double)(text.Length / 2)));
					result = text.Substring(0, 1) + text.Substring(startIndex, 1) + text.Substring(text.Length - 1, 1);
				}
				return result;
			}
		}
	}
}
