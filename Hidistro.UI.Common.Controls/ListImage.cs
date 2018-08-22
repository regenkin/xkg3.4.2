using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ListImage : Image
	{
		public string DataField
		{
			get;
			set;
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (!string.IsNullOrEmpty(base.ImageUrl))
			{
				if (!string.IsNullOrEmpty(base.ImageUrl) && !Utils.IsUrlAbsolute(base.ImageUrl.ToLower()) && Utils.ApplicationPath.Length > 0 && !base.ImageUrl.StartsWith(Utils.ApplicationPath))
				{
					base.ImageUrl = Utils.ApplicationPath + base.ImageUrl;
				}
				base.Render(writer);
			}
		}

		protected override void OnDataBinding(EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.DataField))
			{
				object obj = DataBinder.Eval(this.Page.GetDataItem(), this.DataField);
				if (obj != null && obj != DBNull.Value && !string.IsNullOrEmpty(obj.ToString()))
				{
					base.ImageUrl = (string)obj;
					return;
				}
				SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
				if (this.DataField.Equals("ThumbnailUrl40"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail1;
					return;
				}
				if (this.DataField.Equals("ThumbnailUrl60"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail2;
					return;
				}
				if (this.DataField.Equals("ThumbnailUrl100"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail3;
					return;
				}
				if (this.DataField.Equals("ThumbnailUrl160"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail4;
					return;
				}
				if (this.DataField.Equals("ThumbnailUrl180"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail5;
					return;
				}
				if (this.DataField.Equals("ThumbnailUrl220"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail6;
					return;
				}
				if (this.DataField.Equals("ThumbnailUrl310"))
				{
					base.ImageUrl = masterSettings.DefaultProductThumbnail7;
					return;
				}
				base.ImageUrl = masterSettings.DefaultProductThumbnail8;
			}
		}
	}
}
