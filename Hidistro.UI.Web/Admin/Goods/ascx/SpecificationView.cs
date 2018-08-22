using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods.ascx
{
	public class SpecificationView : System.Web.UI.UserControl
	{
		protected int typeId;

		protected Grid grdSKU;

		protected System.Web.UI.WebControls.TextBox txtName;

		protected UseAttributeImageRadioButtonList radIsImage;

		protected System.Web.UI.WebControls.Button btnCreate;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
			{
				int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
			}
			this.grdSKU.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdSKU_RowCommand);
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			this.grdSKU.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdSKU_RowDataBound);
			this.grdSKU.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdSKU_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				this.BindAttribute();
			}
		}

		private void grdSKU_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = e.Row.FindControl("litUseAttributeImage") as System.Web.UI.WebControls.Literal;
				if (literal.Text == "True")
				{
					literal.Text = "图";
					return;
				}
				literal.Text = "文";
			}
		}

		private void grdSKU_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int attributeId = System.Convert.ToInt32(this.grdSKU.DataKeys[rowIndex].Value);
			if (e.CommandName == "saveSKUName")
			{
				System.Web.UI.WebControls.TextBox textBox = this.grdSKU.Rows[rowIndex].Cells[2].FindControl("txtSKUName") as System.Web.UI.WebControls.TextBox;
				AttributeInfo attributeInfo = new AttributeInfo();
				attributeInfo.AttributeId = attributeId;
				if (string.IsNullOrEmpty(textBox.Text.Trim()) || textBox.Text.Trim().Length > 50)
				{
					string str = string.Format("ShowMsg(\"{0}\", {1});", "规格名称限制在1-50个字符以内", "false");
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
					return;
				}
				attributeInfo.AttributeName = Globals.HtmlEncode(textBox.Text);
				attributeInfo.UsageMode = AttributeUseageMode.Choose;
				ProductTypeHelper.UpdateAttributeName(attributeInfo);
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
			int displaySequence = int.Parse((this.grdSKU.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
			int num = 0;
			int replaceDisplaySequence = 0;
			if (e.CommandName == "Fall")
			{
				if (rowIndex < this.grdSKU.Rows.Count - 1)
				{
					num = (int)this.grdSKU.DataKeys[rowIndex + 1].Value;
					replaceDisplaySequence = int.Parse((this.grdSKU.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
				}
			}
			else if (e.CommandName == "Rise" && rowIndex > 0)
			{
				num = (int)this.grdSKU.DataKeys[rowIndex - 1].Value;
				replaceDisplaySequence = int.Parse((this.grdSKU.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
			}
			if (num > 0)
			{
				ProductTypeHelper.SwapAttributeSequence(attributeId, num, displaySequence, replaceDisplaySequence);
			}
			this.BindAttribute();
		}

		private void grdSKU_RowDeleting(object source, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int num = (int)this.grdSKU.DataKeys[e.RowIndex].Value;
			AttributeInfo attribute = ProductTypeHelper.GetAttribute(num);
			if (ProductTypeHelper.DeleteAttribute(num))
			{
				foreach (AttributeValueInfo current in attribute.AttributeValues)
				{
					StoreHelper.DeleteImage(current.ImageUrl);
				}
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
				return;
			}
			this.BindAttribute();
			string str = string.Format("ShowMsg(\"{0}\", {1});", "有商品在使用此规格，无法删除", "false");
			this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
		}

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			AttributeInfo attributeInfo = new AttributeInfo();
			attributeInfo.TypeId = this.typeId;
			attributeInfo.AttributeName = Globals.HtmlEncode(this.txtName.Text).Replace("，", ",");
			attributeInfo.UsageMode = AttributeUseageMode.Choose;
			attributeInfo.UseAttributeImage = this.radIsImage.SelectedValue;
			ValidationResults validationResults = Validation.Validate<AttributeInfo>(attributeInfo, new string[]
			{
				"ValAttribute"
			});
			string str = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					str += Formatter.FormatErrorMessage(current.Message);
				}
				return;
			}
			ProductTypeHelper.GetAttributes(this.typeId, AttributeUseageMode.Choose);
			if (ProductTypeHelper.AddAttributeName(attributeInfo))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}

		private void BindAttribute()
		{
			this.grdSKU.DataSource = ProductTypeHelper.GetAttributes(this.typeId, AttributeUseageMode.Choose);
			this.grdSKU.DataBind();
		}
	}
}
