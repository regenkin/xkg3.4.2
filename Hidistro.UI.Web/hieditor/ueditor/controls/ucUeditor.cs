using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.hieditor.ueditor.controls
{
	public class ucUeditor : System.Web.UI.UserControl
	{
		private bool _isfristedit = true;

		private int _height = 200;

		private int _width = 600;

		private int _ShowType;

		protected System.Web.UI.WebControls.TextBox txtMemo;

		public string Text
		{
			get
			{
				return base.Request.Form[this.txtMemo.ClientID.Replace("_", "$")];
			}
			set
			{
				this.txtMemo.Text = value;
			}
		}

		public bool IsFirstEdit
		{
			get
			{
				return this._isfristedit;
			}
			set
			{
				this._isfristedit = value;
			}
		}

		public int Height
		{
			get
			{
				return this._height;
			}
			set
			{
				this._height = value;
			}
		}

		public int Width
		{
			get
			{
				return this._width;
			}
			set
			{
				this._width = value;
			}
		}

		public int ShowType
		{
			get
			{
				return this._ShowType;
			}
			set
			{
				this._ShowType = value;
			}
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.txtMemo.Width = this._width;
			this.txtMemo.Height = this._height;
		}
	}
}
