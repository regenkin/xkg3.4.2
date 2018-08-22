using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class TemplatedWebControl : WebControl, INamingContainer
	{
		private SmallStatusMessage smallStatus;

		private ITemplate _skinTemplate;

		public override ControlCollection Controls
		{
			get
			{
				this.EnsureChildControls();
				return base.Controls;
			}
		}

		[Browsable(false), DefaultValue(null), PersistenceMode(PersistenceMode.InnerProperty)]
		public ITemplate SkinTemplate
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

		public override Page Page
		{
			get
			{
				if (base.Page == null)
				{
					base.Page = (HttpContext.Current.Handler as Page);
				}
				return base.Page;
			}
			set
			{
				base.Page = value;
			}
		}

		protected virtual void ShowMessage(string msg, bool success)
		{
			this.smallStatus = (SmallStatusMessage)this.FindControl("Status");
			if (this.smallStatus != null)
			{
				this.smallStatus.Success = success;
				this.smallStatus.Text = msg;
				this.smallStatus.Visible = true;
			}
		}

		public override void DataBind()
		{
			this.EnsureChildControls();
		}

		public override void RenderBeginTag(HtmlTextWriter writer)
		{
		}

		public override void RenderEndTag(HtmlTextWriter writer)
		{
		}

		public override Control FindControl(string id)
		{
			Control control = base.FindControl(id);
			if (control == null && this.Controls.Count == 1)
			{
				control = this.Controls[0].FindControl(id);
			}
			return control;
		}

		protected virtual bool LoadSkinTemplate()
		{
			if (this.SkinTemplate != null)
			{
				this.SkinTemplate.InstantiateIn(this);
				return true;
			}
			return false;
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadSkinTemplate())
			{
				this.AttachChildControls();
			}
		}

		protected abstract void AttachChildControls();
	}
}
