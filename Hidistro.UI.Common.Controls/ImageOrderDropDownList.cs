using System;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class ImageOrderDropDownList : DropDownList
	{
		public new int? SelectedValue
		{
			get
			{
				int value = 0;
				int.TryParse(base.SelectedValue, out value);
				return new int?(value);
			}
			set
			{
				if (value.HasValue)
				{
					base.SelectedValue = value.Value.ToString();
				}
			}
		}

		public override void DataBind()
		{
			this.Items.Clear();
			this.Items.Add(new ListItem("按上传时间从晚到早", "0"));
			this.Items.Add(new ListItem("按上传时间从早到晚", "1"));
			this.Items.Add(new ListItem("按图片名升序", "2"));
			this.Items.Add(new ListItem("按图片名降序", "3"));
			this.Items.Add(new ListItem("按修改时间从晚到早", "4"));
			this.Items.Add(new ListItem("按修改时间从早到晚", "5"));
			this.Items.Add(new ListItem("按图片从大到小", "6"));
			this.Items.Add(new ListItem("按图片从小到大", "7"));
		}
	}
}
