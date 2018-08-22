using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Goods
{
	public class SkuValue : AdminPage
	{
		private int attributeId;

		private int valueId;

		protected System.Web.UI.HtmlControls.HtmlGenericControl valueStr;

		protected System.Web.UI.WebControls.TextBox txtValueStr;

		protected System.Web.UI.HtmlControls.HtmlGenericControl valueImage;

		protected System.Web.UI.WebControls.FileUpload fileUpload;

		protected System.Web.UI.WebControls.TextBox txtValueDec;

		protected System.Web.UI.WebControls.Button btnCreateValue;

		protected System.Web.UI.HtmlControls.HtmlInputHidden currentAttributeId;

		protected SkuValue() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"].ToString().Trim()))
				{
					base.GotoResourceNotFound();
					return;
				}
				string a = this.Page.Request.QueryString["action"].ToString().Trim();
				if (a == "add")
				{
					if (!int.TryParse(this.Page.Request.QueryString["attributeId"], out this.attributeId))
					{
						base.GotoResourceNotFound();
						return;
					}
				}
				else
				{
					if (!int.TryParse(this.Page.Request.QueryString["valueId"], out this.valueId))
					{
						base.GotoResourceNotFound();
						return;
					}
					AttributeValueInfo attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(this.valueId);
					this.attributeId = attributeValueInfo.AttributeId;
					this.txtValueDec.Text = (this.txtValueStr.Text = Globals.HtmlDecode(attributeValueInfo.ValueStr));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["useImg"].ToString()) && this.Page.Request.QueryString["useImg"].ToString().Equals("True"))
				{
					this.txtValueStr.Text = "";
					this.valueStr.Visible = false;
					this.valueImage.Visible = true;
				}
				this.currentAttributeId.Value = this.attributeId.ToString();
			}
			this.btnCreateValue.Click += new System.EventHandler(this.btnCreateValue_Click);
		}

		protected void btnCreateValue_Click(object sender, System.EventArgs e)
		{
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			System.Collections.Generic.IList<AttributeValueInfo> list = new System.Collections.Generic.List<AttributeValueInfo>();
			int num = int.Parse(this.currentAttributeId.Value);
			attributeValueInfo.AttributeId = num;
			string a = this.Page.Request.QueryString["action"].ToString().Trim();
			if (a == "add")
			{
				if (!string.IsNullOrEmpty(this.txtValueStr.Text.Trim()))
				{
					string text = this.txtValueStr.Text.Trim();
					string[] array = text.Split(new char[]
					{
						','
					});
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i].Trim().Length > 100)
						{
							this.ShowMsgToTarget("[" + array[i].Trim() + "]属性值超出了50个字符", false, "parent");
							return;
						}
						AttributeValueInfo attributeValueInfo2 = new AttributeValueInfo();
						if (array[i].Trim().Length > 50)
						{
							this.ShowMsgToTarget("属性值限制在50个字符以内", false, "parent");
							return;
						}
						attributeValueInfo2.ValueStr = Globals.HtmlEncode(array[i].Trim());
						attributeValueInfo2.AttributeId = num;
						list.Add(attributeValueInfo2);
					}
					foreach (AttributeValueInfo current in list)
					{
						ProductTypeHelper.AddAttributeValue(current);
					}
					base.ClientScript.RegisterStartupScript(base.ClientScript.GetType(), "myscript", "<script>window.parent.closeModal(getParam('action'));</script>");
					this.txtValueStr.Text = "";
				}
				if (this.fileUpload.HasFile)
				{
					try
					{
						attributeValueInfo.ImageUrl = ProductTypeHelper.UploadSKUImage(this.fileUpload.PostedFile);
						attributeValueInfo.ValueStr = Globals.HtmlEncode(this.txtValueDec.Text);
					}
					catch
					{
					}
					if (ProductTypeHelper.AddAttributeValue(attributeValueInfo) > 0)
					{
						return;
					}
				}
			}
			else
			{
				this.valueId = int.Parse(this.Page.Request.QueryString["valueId"]);
				attributeValueInfo = ProductTypeHelper.GetAttributeValueInfo(this.valueId);
				AttributeInfo attribute = ProductTypeHelper.GetAttribute(attributeValueInfo.AttributeId);
				if (attribute.UseAttributeImage)
				{
					if (!string.IsNullOrEmpty(this.txtValueDec.Text))
					{
						attributeValueInfo.ValueStr = Globals.HtmlEncode(this.txtValueDec.Text);
					}
				}
				else if (!string.IsNullOrEmpty(this.txtValueStr.Text))
				{
					attributeValueInfo.ValueStr = Globals.HtmlEncode(this.txtValueStr.Text);
				}
				if (this.fileUpload.HasFile)
				{
					try
					{
						StoreHelper.DeleteImage(attributeValueInfo.ImageUrl);
						attributeValueInfo.ImageUrl = ProductTypeHelper.UploadSKUImage(this.fileUpload.PostedFile);
					}
					catch
					{
					}
				}
				ProductTypeHelper.UpdateAttributeValue(attributeValueInfo);
				base.ClientScript.RegisterStartupScript(base.ClientScript.GetType(), "myscript", "<script>window.parent.closeModal(getParam('action'));</script>");
			}
		}
	}
}
