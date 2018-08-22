using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hidistro.UI.Web.Admin.Goods.ascx;
using System;

namespace Hidistro.UI.Web.Admin.Goods
{
	[PrivilegeCheck(Privilege.AddProductType)]
	public class AddSpecification : AdminPage
	{
		protected SpecificationView specificationView;

		protected AddSpecification() : base("m02", "spp07")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			bool flag = !string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true";
			int attributeValueId;
			if (flag && int.TryParse(base.Request["ValueId"], out attributeValueId))
			{
				if (ProductTypeHelper.DeleteAttributeValue(attributeValueId))
				{
					if (!string.IsNullOrEmpty(base.Request["ImageUrl"]))
					{
						StoreHelper.DeleteImage(base.Request["ImageUrl"]);
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						base.Response.Write("{\"Status\":\"true\"}");
						base.Response.End();
					}
					else
					{
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						base.Response.Write("{\"Status\":\"true\"}");
						base.Response.End();
					}
				}
				else
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{\"Status\":\"false\"}");
					base.Response.End();
				}
			}
			if (!string.IsNullOrEmpty(base.Request["isAjax"]) && base.Request["isAjax"] == "true")
			{
				string text = base.Request["Mode"].ToString();
				string text2 = "false";
				string a;
				if ((a = text) != null)
				{
					int attributeId;
					string text3;
					if (a == "Add")
					{
						attributeId = 0;
						text3 = "参数缺少";
						if (int.TryParse(base.Request["AttributeId"], out attributeId))
						{
							text3 = "属性名称不允许为空！";
							if (!string.IsNullOrEmpty(base.Request["ValueName"].ToString()))
							{
								string valueStr = Globals.HtmlEncode(base.Request["ValueName"].ToString());
								AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
								attributeValueInfo.ValueStr = valueStr;
								attributeValueInfo.AttributeId = attributeId;
								text3 = "添加属性值失败";
								int num = ProductTypeHelper.AddAttributeValue(attributeValueInfo);
								if (num > 0)
								{
									text3 = num.ToString();
									text2 = "true";
								}
							}
						}
						base.Response.Clear();
						base.Response.ContentType = "application/json";
						base.Response.Write(string.Concat(new string[]
						{
							"{\"Status\":\"",
							text2,
							"\",\"msg\":\"",
							text3,
							"\"}"
						}));
						base.Response.End();
						return;
					}
					if (!(a == "AddSkuItemValue"))
					{
						return;
					}
					text3 = "参数缺少";
					if (int.TryParse(base.Request["AttributeId"], out attributeId))
					{
						text3 = "规格值名不允许为空！";
						if (!string.IsNullOrEmpty(base.Request["ValueName"].ToString()))
						{
							string text4 = Globals.HtmlEncode(base.Request["ValueName"].ToString().Replace("+", "").Replace(",", ""));
							text3 = "规格值名长度不允许超过50个字符";
							if (text4.Length < 50)
							{
								AttributeValueInfo attributeValueInfo2 = new AttributeValueInfo();
								attributeValueInfo2.ValueStr = text4;
								attributeValueInfo2.AttributeId = attributeId;
								text3 = "添加规格值失败";
								int num2 = ProductTypeHelper.AddAttributeValue(attributeValueInfo2);
								if (num2 > 0)
								{
									text3 = num2.ToString();
									text2 = "true";
								}
							}
						}
					}
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write(string.Concat(new string[]
					{
						"{\"Status\":\"",
						text2,
						"\",\"msg\":\"",
						text3,
						"\"}"
					}));
					base.Response.End();
				}
			}
		}

		private void btnFilish_Click(object server, System.EventArgs e)
		{
			base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ProductTypes.aspx"), true);
		}
	}
}
