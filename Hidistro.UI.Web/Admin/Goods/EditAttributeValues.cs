using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class EditAttributeValues : AdminPage
	{
		private int attributeId;

		private int typeId;

		protected Grid grdAttributeValues;

		protected System.Web.UI.WebControls.TextBox txtValue;

		protected System.Web.UI.WebControls.Button btnCreate;

		protected System.Web.UI.WebControls.TextBox txtOldValue;

		protected System.Web.UI.WebControls.Button btnUpdate;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidvalueId;

		protected System.Web.UI.HtmlControls.HtmlInputHidden hidvalue;

		protected EditAttributeValues() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
			this.btnCreate.Click += new System.EventHandler(this.btnAdd_Click);
			this.grdAttributeValues.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdAttributeValues_RowDeleting);
			this.grdAttributeValues.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdAttributeValues_RowCommand);
			if (!int.TryParse(this.Page.Request.QueryString["AttributeId"], out this.attributeId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!int.TryParse(this.Page.Request.QueryString["TypeId"], out this.typeId))
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

		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			if (this.txtOldValue.Text.Trim().Length > 15 || this.txtOldValue.Text.Trim().Length == 0)
			{
				this.ShowMsg("属性值必须小于15个字符，不能为空", false);
				return;
			}
			AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(System.Convert.ToInt32(this.hidvalueId.Value));
			attributeValueInfo.ValueStr = this.txtOldValue.Text.Trim().Replace("+", "");
			if (ProductTypeHelper.UpdateAttributeValue(attributeValueInfo))
			{
				this.BindData();
				this.ShowMsg("修改成功", true);
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			if (this.txtValue.Text.Trim().Length > 15 || this.txtValue.Text.Trim().Length == 0)
			{
				this.ShowMsg("属性值必须小于15个字符，不能为空", false);
				return;
			}
			attributeValueInfo.ValueStr = this.txtValue.Text.Trim().Replace("+", "");
			attributeValueInfo.AttributeId = this.attributeId;
			if (ProductTypeHelper.AddAttributeValue(attributeValueInfo) > 0)
			{
				this.BindData();
				this.ShowMsg("添加成功", true);
			}
		}

		private void grdAttributeValues_RowDeleting(object source, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int attributeValueId = (int)this.grdAttributeValues.DataKeys[e.RowIndex].Value;
			if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
			{
				this.BindData();
			}
		}

		private void grdAttributeValues_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int attributeValueId = (int)this.grdAttributeValues.DataKeys[rowIndex].Value;
			int displaySequence = int.Parse((this.grdAttributeValues.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
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
			if (num > 0)
			{
				ProductTypeHelper.SwapAttributeValueSequence(attributeValueId, num, displaySequence, replaceDisplaySequence);
				this.BindData();
			}
		}
	}
}
