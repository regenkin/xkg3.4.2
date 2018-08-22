using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPNET.WebControls
{
	[ToolboxData("<{0}:Grid runat=server></{0}:Grid>")]
	public class GetableGrid : GridView
	{
		private const string SortOrderKey = "sort";

		private const string SortByKey = "sortBy";

		private string currentSort;

		private string currentSortBy;

		private bool showOrderIcons = true;

		private string sortAscIcon;

		private string sortDescIcon;

		private string defaultSortIcon;

		public bool ShowOrderIcons
		{
			get
			{
				return this.showOrderIcons;
			}
			set
			{
				this.showOrderIcons = value;
			}
		}

		public string SortAscIcon
		{
			get
			{
				return this.sortAscIcon;
			}
			set
			{
				this.sortAscIcon = value;
			}
		}

		public string SortDescIcon
		{
			get
			{
				return this.sortDescIcon;
			}
			set
			{
				this.sortDescIcon = value;
			}
		}

		public string DefaultSortIcon
		{
			get
			{
				return this.defaultSortIcon;
			}
			set
			{
				this.defaultSortIcon = value;
			}
		}

		public override bool AllowPaging
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public override bool AllowSorting
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		public string SortOrder
		{
			get
			{
				return this.currentSort;
			}
		}

		public string SortOrderBy
		{
			get
			{
				return this.currentSortBy;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.currentSort = this.Context.Request.QueryString["sort"];
			this.currentSortBy = this.Context.Request.QueryString["sortBy"];
			this.SortAscIcon = this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.asc.gif");
			this.SortDescIcon = this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.desc.gif");
			this.DefaultSortIcon = this.Page.ClientScript.GetWebResourceUrl(base.GetType(), "ASPNET.WebControls.Grid.Images.por_arrow_13.gif");
			base.OnInit(e);
		}

		protected override void OnRowDataBound(GridViewRowEventArgs e)
		{
			base.OnRowDataBound(e);
			if (e.Row.RowType == DataControlRowType.Pager || e.Row.RowType == DataControlRowType.Footer)
			{
				e.Row.Visible = false;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (this.AllowSorting && this.Controls.Count >= 0 && this.Rows.Count > 0 && this.ShowOrderIcons)
			{
				int num = 0;
				string url = this.CreateUrl();
				foreach (DataControlField dataControlField in this.Columns)
				{
					if (!string.IsNullOrEmpty(dataControlField.SortExpression))
					{
						Image image = new Image();
						image.BorderWidth = Unit.Pixel(0);
						image.Width = Unit.Pixel(13);
						image.Height = Unit.Pixel(7);
						string sort;
						if (dataControlField.SortExpression == this.currentSortBy)
						{
							if (string.Compare("desc", this.currentSort, true, CultureInfo.InvariantCulture) == 0)
							{
								image.ImageUrl = this.SortDescIcon;
								sort = "ASC";
							}
							else
							{
								image.ImageUrl = this.SortAscIcon;
								sort = "DESC";
							}
						}
						else
						{
							image.ImageUrl = this.DefaultSortIcon;
							sort = "ASC";
						}
						this.Controls[0].Controls[0].Controls[num].Controls.Clear();
						this.Controls[0].Controls[0].Controls[num].Controls.Add(image);
						this.Controls[0].Controls[0].Controls[num].Controls.Add(new LiteralControl("&nbsp;"));
						this.Controls[0].Controls[0].Controls[num].Controls.Add(this.CreateHeader(dataControlField.HeaderText, sort, dataControlField.SortExpression, url));
					}
					num++;
				}
			}
			base.Render(writer);
		}

		private HyperLink CreateHeader(string title, string sort, string sortBy, string url)
		{
			HyperLink hyperLink = new HyperLink();
			hyperLink.Text = title;
			hyperLink.NavigateUrl = url + string.Format(CultureInfo.InvariantCulture, "{0}={1}&{2}={3}", new object[]
			{
				"sortBy",
				sortBy,
				"sort",
				sort
			});
			hyperLink.ControlStyle.CopyFrom(base.HeaderStyle);
			return hyperLink;
		}

		private string CreateUrl()
		{
			string text = this.Context.Request.Url.AbsolutePath + "?";
			if (this.Context.Request.QueryString.Count > 0)
			{
				for (int i = 0; i < this.Context.Request.QueryString.Count; i++)
				{
					string text2 = this.Context.Request.QueryString.Keys[i];
					if (string.Compare(text2, "sortBy", true) != 0 && string.Compare(text2, "sort", true) != 0)
					{
						string text3 = text;
						text = string.Concat(new string[]
						{
							text3,
							text2,
							"=",
							this.Page.Server.UrlEncode(this.Context.Request.QueryString[text2]),
							"&"
						});
					}
				}
			}
			return text;
		}
	}
}
