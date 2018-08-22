using Hidistro.Core;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Ascx
{
	public class ucDateTimePicker : System.Web.UI.UserControl
	{
		private bool _IsAdmin = true;

		private bool _Enabled = true;

		private string _DateFormat = "yyyy-MM-dd";

		private string _CssClass = "";

		private int _Width;

		private string _PlaceHolder = "";

		private string _Style = "";

		private int _minView = 2;

		private bool _isEnd;

		protected System.Web.UI.WebControls.TextBox txtDateTimePicker;

		protected System.Web.UI.WebControls.Literal ltScript;

		public bool IsAdmin
		{
			get
			{
				return this._IsAdmin;
			}
			set
			{
				this._IsAdmin = value;
			}
		}

		public bool Enabled
		{
			get
			{
				return this._Enabled;
			}
			set
			{
				this._Enabled = value;
			}
		}

		public string DateFormat
		{
			set
			{
				if (value != null)
				{
					if (value == "yyyy-MM-dd")
					{
						this._DateFormat = value;
						return;
					}
					if (value == "yyyy-MM-dd HH" || value == "yyyy-MM-dd HH:mm" || value == "yyyy-MM-dd HH:mm:ss")
					{
						this._minView = 0;
						this._DateFormat = value;
						return;
					}
				}
				this._DateFormat = "yyyy-MM-dd";
			}
		}

		public string CssClass
		{
			get
			{
				return this._CssClass;
			}
			set
			{
				this._CssClass = value;
			}
		}

		public int Width
		{
			get
			{
				return this._Width;
			}
			set
			{
				this._Width = value;
			}
		}

		public string PlaceHolder
		{
			get
			{
				return this._PlaceHolder;
			}
			set
			{
				this._PlaceHolder = value;
			}
		}

		public string Style
		{
			get
			{
				return this._Style;
			}
			set
			{
				this._Style = value;
			}
		}

		public bool IsEnd
		{
			get
			{
				return this._isEnd;
			}
			set
			{
				this._isEnd = value;
			}
		}

		public System.DateTime? SelectedDate
		{
			get
			{
				System.DateTime? result = null;
				string text = Globals.RequestFormStr(this.txtDateTimePicker.ClientID.Replace("_", "$"));
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						result = new System.DateTime?(System.DateTime.Parse(text));
					}
					catch (System.Exception)
					{
						return new System.DateTime?(System.DateTime.Now);
					}
					return result;
				}
				return result;
			}
			set
			{
				if (!value.HasValue)
				{
					this.txtDateTimePicker.Text = "";
					return;
				}
				System.DateTime dateTime = System.DateTime.Parse(value.ToString());
				this.txtDateTimePicker.Text = dateTime.ToString(this._DateFormat);
			}
		}

		public string Text
		{
			get
			{
				return this.txtDateTimePicker.Text;
			}
			set
			{
				this.txtDateTimePicker.Text = value;
			}
		}

		public System.DateTime? TextToDate
		{
			get
			{
				System.DateTime? result = null;
				string text = this.txtDateTimePicker.Text.Trim();
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						result = new System.DateTime?(System.DateTime.Parse(text));
					}
					catch (System.Exception)
					{
						return new System.DateTime?(System.DateTime.Now);
					}
					return result;
				}
				return result;
			}
			set
			{
				if (!value.HasValue)
				{
					this.txtDateTimePicker.Text = "";
					return;
				}
				System.DateTime dateTime = System.DateTime.Parse(value.ToString());
				this.txtDateTimePicker.Text = dateTime.ToString(this._DateFormat);
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this._CssClass))
			{
				this.txtDateTimePicker.CssClass = this._CssClass;
			}
			if (this._Width > 0)
			{
				this.txtDateTimePicker.Width = this._Width;
			}
			if (!this._Enabled)
			{
				this.txtDateTimePicker.Enabled = false;
			}
			if (!string.IsNullOrEmpty(this._PlaceHolder))
			{
				this.txtDateTimePicker.Attributes.Add("placeholder", this._PlaceHolder);
			}
			if (!string.IsNullOrEmpty(this._Style))
			{
				this.txtDateTimePicker.Attributes.Add("style", this._Style);
			}
			this.ltScript.Text = string.Concat(new object[]
			{
				"<script>var ",
				this.txtDateTimePicker.ClientID,
				"_obj=$(\"#",
				this.txtDateTimePicker.ClientID,
				"\").datetimepicker({ format: '",
				this._DateFormat.ToString().Replace("mm", "ii").Replace("MM", "mm").Replace("HH", "hh"),
				"', minView: ",
				this._minView,
				" ,isadmin:",
				this._IsAdmin.ToString().ToLower(),
				",isEnd:",
				this.IsEnd ? "1" : "0",
				"});</script>"
			});
		}
	}
}
