using ASPNET.WebControls;
using Hidistro.Core;
using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class VshopTemplatedRepeater : Repeater
	{
		private string skinName = string.Empty;

		public string TemplateFile
		{
			get
			{
				if (!string.IsNullOrEmpty(this.skinName) && !Utils.IsUrlAbsolute(this.skinName.ToLower()))
				{
					return Utils.ApplicationPath + this.skinName;
				}
				return this.skinName;
			}
			set
			{
				if (!string.IsNullOrEmpty(value))
				{
					if (value.StartsWith("/"))
					{
						this.skinName = Globals.GetVshopSkinPath(null) + value;
					}
					else
					{
						this.skinName = Globals.GetVshopSkinPath(null) + "/" + value;
					}
				}
				if (!this.skinName.StartsWith("/templates"))
				{
					this.skinName = this.skinName.Substring(this.skinName.IndexOf("/templates"));
				}
			}
		}

		protected override void CreateChildControls()
		{
			if (this.ItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				this.ItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
		}
	}
}
