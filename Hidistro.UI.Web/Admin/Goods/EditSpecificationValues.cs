using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class EditSpecificationValues : AdminPage
	{
		private int attributeId;

		protected Grid grdAttributeValues;

		protected System.Web.UI.HtmlControls.HtmlInputHidden currentAttributeId;

		protected EditSpecificationValues() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdAttributeValues.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdAttributeValues_RowCommand);
			if (!int.TryParse(this.Page.Request.QueryString["AttributeId"], out this.attributeId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				this.BindData();
			}
		}

		private void BindData()
		{
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(this.attributeId);
			this.grdAttributeValues.DataSource = attribute.AttributeValues;
			this.grdAttributeValues.DataBind();
		}

		private void grdAttributeValues_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int attributeValueId = (int)this.grdAttributeValues.DataKeys[rowIndex].Value;
			int displaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
			string imageUrl = e.CommandArgument.ToString();
			int num = 0;
			int replaceDisplaySequence = 0;
			if (e.CommandName == "Fall")
			{
				if (rowIndex < this.grdAttributeValues.Rows.Count - 1)
				{
					num = (int)this.grdAttributeValues.DataKeys[rowIndex + 1].Value;
					replaceDisplaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
				}
			}
			else if (e.CommandName == "Rise" && rowIndex > 0)
			{
				num = (int)this.grdAttributeValues.DataKeys[rowIndex - 1].Value;
				replaceDisplaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
			}
			if (e.CommandName == "dele")
			{
				if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
				{
					StoreHelper.DeleteImage(imageUrl);
				}
				else
				{
					this.ShowMsg("该规格下存在商品", false);
				}
			}
			if (num > 0)
			{
				ProductTypeHelper.SwapAttributeValueSequence(attributeValueId, num, displaySequence, replaceDisplaySequence);
			}
			this.BindData();
		}
	}
}
