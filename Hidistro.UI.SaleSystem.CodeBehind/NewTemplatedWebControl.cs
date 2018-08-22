using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class NewTemplatedWebControl : System.Web.UI.WebControls.WebControl, System.Web.UI.INamingContainer
	{
		private System.Web.UI.ITemplate _skinTemplate;

		private string skinName;

		public override System.Web.UI.ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		[System.ComponentModel.Browsable(false), System.ComponentModel.DefaultValue(null), System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty)]
		public System.Web.UI.ITemplate SkinTemplate
		{
			get
			{
				return this._skinTemplate;
			}
			set
			{
				this._skinTemplate = value;
				base.ChildControlsCreated = false;
			}
		}

		public override System.Web.UI.Page Page
		{
			get
			{
				if (base.Page == null)
				{
					base.Page = (System.Web.HttpContext.Current.Handler as System.Web.UI.Page);
				}
				return base.Page;
			}
			set
			{
				base.Page = value;
			}
		}

		protected virtual string SkinPath
		{
			get
			{
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				string vTheme = masterSettings.VTheme;
				string result;
				if (this.SkinName.StartsWith(vTheme))
				{
					result = this.SkinName;
				}
				else if (this.SkinName.StartsWith("/"))
				{
					result = vTheme + this.SkinName;
				}
				else
				{
					result = string.Concat(new string[]
					{
						Globals.ApplicationPath,
						"/Templates/vshop/",
						vTheme,
						"/",
						this.SkinName
					});
				}
				return result;
			}
		}

		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					value = value.ToLower(System.Globalization.CultureInfo.InvariantCulture);
					if (value.EndsWith(".html"))
					{
						this.skinName = value;
					}
				}
			}
		}

		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
		{
		}

		public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer)
		{
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			this.LoadHtmlThemedControl();
		}

		protected bool LoadHtmlThemedControl()
		{
			string text = System.IO.File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), System.Text.Encoding.UTF8);
			bool result;
			if (!string.IsNullOrEmpty(text))
			{
				System.Web.UI.Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
	}
}
