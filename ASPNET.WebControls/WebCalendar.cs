using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class WebCalendar : TextBox
	{
		public DateTime? SelectedDate
		{
			get
			{
				if (string.IsNullOrEmpty(this.Text))
				{
					return null;
				}
				string text = this.Text;
				if (this.CalendarType == CalendarType.StartDate)
				{
					text += " 00:00:00";
				}
				else if (this.CalendarType == CalendarType.EndDate)
				{
					text += " 23:59:59";
				}
				DateTime value;
				if (DateTime.TryParse(text, out value))
				{
					return new DateTime?(value);
				}
				return null;
			}
			set
			{
				if (value.HasValue)
				{
					this.Text = value.Value.ToString("yyyy-MM-dd");
				}
			}
		}

		public CalendarType CalendarType
		{
			get;
			set;
		}

		public int BeginYear
		{
			get;
			set;
		}

		public int EndYear
		{
			get;
			set;
		}

		public CalendarLanguage CalendarLanguage
		{
			get;
			set;
		}

		public WebCalendar()
		{
			this.CalendarType = CalendarType.Default;
			this.BeginYear = 1990;
			this.EndYear = 2020;
			this.CalendarLanguage = CalendarLanguage.zh_CN;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Attributes.Add("readonly", "readonly");
			base.Attributes.Add("onclick", string.Format("new Calendar({0}, {1}, {2}).show(this);", this.BeginYear, this.EndYear, (int)this.CalendarLanguage));
			string script = string.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Calendar.calendar.js"));
			if (!this.Page.ClientScript.IsStartupScriptRegistered(base.GetType(), "WebCalendarScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "WebCalendarScript", script, false);
			}
			base.Render(writer);
		}
	}
}
