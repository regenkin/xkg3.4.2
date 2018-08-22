using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hishop.Components.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods.ascx
{
	public class AttributeView : System.Web.UI.UserControl
	{
		private int typeId;

		protected Grid grdAttribute;

		protected System.Web.UI.WebControls.TextBox txtName;

		protected System.Web.UI.WebControls.CheckBox chkMulti_copy;

		protected System.Web.UI.WebControls.TextBox txtValues;

		protected System.Web.UI.WebControls.Button btnCreate;

		protected System.Web.UI.WebControls.TextBox txtValueStr;

		protected System.Web.UI.WebControls.Button btnCreateValue;

		protected System.Web.UI.WebControls.CheckBox chkMulti;

		protected System.Web.UI.HtmlControls.HtmlInputHidden currentAttributeId;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["typeId"]))
			{
				int.TryParse(this.Page.Request.QueryString["typeId"], out this.typeId);
			}
			this.grdAttribute.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdAttribute_RowDeleting);
			this.grdAttribute.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdAttribute_RowCommand);
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			this.btnCreateValue.Click += new System.EventHandler(this.btnCreateValueAdd_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindAttribute();
			}
		}

		private void grdAttribute_RowDeleting(object source, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int attriubteId = (int)this.grdAttribute.DataKeys[e.RowIndex].Value;
			if (ProductTypeHelper.DeleteAttribute(attriubteId))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
				return;
			}
			this.BindAttribute();
		}

		private void grdAttribute_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int attributeId = (int)this.grdAttribute.DataKeys[rowIndex].Value;
			int displaySequence = int.Parse((this.grdAttribute.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
			int num = 0;
			int replaceDisplaySequence = 0;
			if (e.CommandName == "saveAttributeName")
			{
				System.Web.UI.WebControls.TextBox textBox = this.grdAttribute.Rows[rowIndex].FindControl("txtAttributeName") as System.Web.UI.WebControls.TextBox;
				AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeId);
				if (string.IsNullOrEmpty(textBox.Text.Trim()) || textBox.Text.Trim().Length > 15)
				{
					string str = string.Format("ShowMsg(\"{0}\", {1});", "属性名称限制在1-15个字符以内", "false");
					this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
					return;
				}
				attribute.AttributeName = Globals.HtmlEncode(textBox.Text);
				ProductTypeHelper.UpdateAttributeName(attribute);
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
			if (e.CommandName == "SetYesOrNo")
			{
				AttributeInfo attribute2 = ProductTypeHelper.GetAttribute(attributeId);
				if (attribute2.IsMultiView)
				{
					attribute2.UsageMode = AttributeUseageMode.View;
				}
				else
				{
					attribute2.UsageMode = AttributeUseageMode.MultiView;
				}
				ProductTypeHelper.UpdateAttributeName(attribute2);
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
			if (e.CommandName == "Fall")
			{
				if (rowIndex < this.grdAttribute.Rows.Count - 1)
				{
					num = (int)this.grdAttribute.DataKeys[rowIndex + 1].Value;
					replaceDisplaySequence = int.Parse((this.grdAttribute.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
				}
			}
			else if (e.CommandName == "Rise" && rowIndex > 0)
			{
				num = (int)this.grdAttribute.DataKeys[rowIndex - 1].Value;
				replaceDisplaySequence = int.Parse((this.grdAttribute.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text, System.Globalization.NumberStyles.None);
			}
			if (num > 0)
			{
				ProductTypeHelper.SwapAttributeSequence(attributeId, num, displaySequence, replaceDisplaySequence);
				this.BindAttribute();
			}
		}

		private void btnCreateValueAdd_Click(object sender, System.EventArgs e)
		{
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			System.Collections.Generic.IList<AttributeValueInfo> list = new System.Collections.Generic.List<AttributeValueInfo>();
			int attributeId = int.Parse(this.currentAttributeId.Value);
			attributeValueInfo.AttributeId = attributeId;
			if (!string.IsNullOrEmpty(this.txtValueStr.Text.Trim()))
			{
				string text = this.txtValueStr.Text.Trim().Replace("，", ",");
				string[] array = text.Split(new char[]
				{
					','
				});
				int num = 0;
				while (num < array.Length && array[num].Trim().Length <= 100)
				{
					AttributeValueInfo attributeValueInfo2 = new AttributeValueInfo();
					if (array[num].Trim().Length > 50)
					{
						string str = string.Format("ShowMsg(\"{0}\", {1});", "属性值限制在50个字符以内", "false");
						this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
						return;
					}
					attributeValueInfo2.ValueStr = Globals.HtmlEncode(array[num].Trim()).Replace("+", "");
					attributeValueInfo2.AttributeId = attributeId;
					list.Add(attributeValueInfo2);
					num++;
				}
				foreach (AttributeValueInfo current in list)
				{
					ProductTypeHelper.AddAttributeValue(current);
				}
				this.txtValueStr.Text = string.Empty;
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			if (this.txtName.Text.Trim().Length > 15)
			{
				string str = string.Format("ShowMsg(\"{0}\", {1});", "属性名称限制在15个字符以内", "false");
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
				return;
			}
			AttributeInfo attributeInfo = new AttributeInfo();
			attributeInfo.TypeId = this.typeId;
			attributeInfo.AttributeName = Globals.HtmlEncode(this.txtName.Text.Trim());
			if (this.chkMulti.Checked)
			{
				attributeInfo.UsageMode = AttributeUseageMode.MultiView;
			}
			else
			{
				attributeInfo.UsageMode = AttributeUseageMode.View;
			}
			if (!string.IsNullOrEmpty(this.txtValues.Text.Trim()))
			{
				string text = this.txtValues.Text.Trim().Replace("，", ",");
				string[] array = text.Split(new char[]
				{
					','
				});
				int num = 0;
				while (num < array.Length && array[num].Length <= 100)
				{
					AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
					if (array[num].Length > 15)
					{
						string str2 = string.Format("ShowMsg(\"{0}\", {1});", "扩展属性的值，每个值的字符数最多15个字符", "false");
						this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript2", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str2 + "},300);</script>");
						return;
					}
					attributeValueInfo.ValueStr = Globals.HtmlEncode(array[num]);
					attributeInfo.AttributeValues.Add(attributeValueInfo);
					num++;
				}
			}
			ValidationResults validationResults = Validation.Validate<AttributeInfo>(attributeInfo, new string[]
			{
				"ValAttribute"
			});
			string str3 = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults))
				{
					str3 += Formatter.FormatErrorMessage(current.Message);
				}
				return;
			}
			if (ProductTypeHelper.AddAttribute(attributeInfo))
			{
				this.txtName.Text = string.Empty;
				this.txtValues.Text = string.Empty;
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}

		private void BindAttribute()
		{
			this.grdAttribute.DataSource = ProductTypeHelper.GetAttributes(this.typeId, AttributeUseageMode.View);
			this.grdAttribute.DataBind();
		}
	}
}
