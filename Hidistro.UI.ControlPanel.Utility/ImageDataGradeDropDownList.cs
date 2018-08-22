using Hidistro.ControlPanel.Store;
using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace Hidistro.UI.ControlPanel.Utility
{
	public class ImageDataGradeDropDownList : DropDownList
	{
		private bool allowNull = true;

		private string nullToDisplay = "";

		public bool AllowNull
		{
			get
			{
				return this.allowNull;
			}
			set
			{
				this.allowNull = value;
			}
		}

		public string NullToDisplay
		{
			get
			{
				return this.nullToDisplay;
			}
			set
			{
				this.nullToDisplay = value;
			}
		}

		public new int? SelectedValue
		{
			get
			{
				int? result;
				if (string.IsNullOrEmpty(base.SelectedValue))
				{
					result = null;
				}
				else
				{
					result = new int?(int.Parse(base.SelectedValue, CultureInfo.InvariantCulture));
				}
				return result;
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedIndex = base.Items.IndexOf(base.Items.FindByValue(value.Value.ToString(CultureInfo.InvariantCulture)));
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			base.Items.Add(new ListItem("默认分组", "0"));
			DataTable photoCategories = GalleryHelper.GetPhotoCategories(0);
			foreach (DataRow dataRow in photoCategories.Rows)
			{
				base.Items.Add(new ListItem(dataRow["CategoryName"].ToString(), dataRow["CategoryId"].ToString()));
			}
		}
	}
}
