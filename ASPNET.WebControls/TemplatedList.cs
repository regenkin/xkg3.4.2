using System;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	public class TemplatedList : DataList
	{
		private string skinName = string.Empty;

		public virtual string TemplateFile
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
				this.skinName = value;
			}
		}

		protected override void CreateChildControls()
		{
			if (this.ItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				this.ItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
			if (this.AlternatingItemStyle != null && this.AlternatingItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				this.AlternatingItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
			if (this.SelectedItemStyle != null && this.SelectedItemTemplate == null && !string.IsNullOrEmpty(this.TemplateFile))
			{
				this.SelectedItemTemplate = this.Page.LoadTemplate(this.TemplateFile);
			}
		}

		protected override void OnItemDataBound(DataListItemEventArgs e)
		{
			base.OnItemDataBound(e);
			if (e.Item.ItemType == ListItemType.Pager || e.Item.ItemType == ListItemType.Footer || e.Item.ItemType == ListItemType.Header)
			{
				e.Item.Visible = false;
			}
		}
	}
}
