using ASPNET.WebControls;
using Hidistro.ControlPanel.Settings;
using Hidistro.Entities;
using Hidistro.Entities.Settings;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.Admin.Settings
{
	public class ManageShippingTemplates : AdminPage
	{
		private System.Collections.Generic.Dictionary<int, string> AllRegionIds;

		protected System.Web.UI.HtmlControls.HtmlForm thisForm;

		protected System.Web.UI.WebControls.Button lkbDeleteCheck;

		protected System.Web.UI.WebControls.Repeater ListTemplates;

		protected System.Web.UI.HtmlControls.HtmlGenericControl TablefFooter;

		protected Pager pager1;

		protected ManageShippingTemplates() : base("m09", "szp06")
		{
		}

		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.lkbDeleteCheck.Click += new System.EventHandler(this.lkbDeleteCheck_Click);
			if (!this.Page.IsPostBack)
			{
				this.TablefFooter.Visible = true;
				this.BindTemplates();
			}
		}

		private void lkbDeleteCheck_Click(object sender, System.EventArgs e)
		{
			this.DeleteCheck();
		}

		private void DeleteCheck()
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的运费模板", false);
				return;
			}
			int num = SettingsHelper.DeleteShippingTemplates(text);
			this.ShowMsg(string.Format("成功删除了{0}条模板关联数据", num), true);
			this.BindTemplates();
		}

		protected void DeleteShiper(object sender, System.EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(((System.Web.UI.WebControls.Button)sender).CommandArgument, out num))
			{
				this.ShowMsg("非正常删除！", true);
				return;
			}
			string shippingTemplateLinkProduct = SettingsHelper.GetShippingTemplateLinkProduct(new int[]
			{
				num
			});
			if (shippingTemplateLinkProduct.StartsWith(num.ToString() + "|"))
			{
				this.ShowMsg("您选择的模板已有商品使用，不能删除！", false);
				return;
			}
			if (SettingsHelper.DeleteShippingTemplate(num))
			{
				this.BindTemplates();
				this.ShowMsg("已经成功删除选择的模板信息", true);
				return;
			}
			this.ShowMsg("非正常删除！", true);
		}

		public void BindTemplates()
		{
			if (this.AllRegionIds == null)
			{
				this.AllRegionIds = RegionHelper.GetAllCitys();
			}
			System.Collections.Generic.IList<FreightTemplate> freightTemplates = SettingsHelper.GetFreightTemplates();
			this.ListTemplates.DataSource = freightTemplates;
			this.ListTemplates.DataBind();
			if (freightTemplates.Count < 1)
			{
				this.TablefFooter.Visible = false;
			}
		}

		private string getRegionNameById(int RegionId)
		{
			string result = "未知";
			if (this.AllRegionIds.ContainsKey(RegionId))
			{
				result = this.AllRegionIds[RegionId];
			}
			return result;
		}

		public string getRegionNamesByIds(string RegionIds)
		{
			string text = "";
			if (!string.IsNullOrEmpty(RegionIds))
			{
				string[] array = RegionIds.Split(new char[]
				{
					','
				});
				text = "";
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text2 = array2[i];
					int num = 0;
					if (int.TryParse(text2.Trim(), out num) && num > 0)
					{
						text = text + this.getRegionNameById(num) + "，";
					}
				}
			}
			return text;
		}

		protected void rptypelist_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Repeater repeater = e.Item.FindControl("ListShipper") as System.Web.UI.WebControls.Repeater;
				FreightTemplate freightTemplate = (FreightTemplate)e.Item.DataItem;
				int templateId = System.Convert.ToInt32(freightTemplate.TemplateId);
				System.Collections.Generic.IList<SpecifyRegionGroup> specifyRegionGroups = SettingsHelper.GetSpecifyRegionGroups(templateId);
				if (specifyRegionGroups.Count < 1)
				{
					specifyRegionGroups.Add(new SpecifyRegionGroup
					{
						RegionIds = "",
						ModeId = 1,
						FristNumber = 1m,
						FristPrice = 0m,
						AddNumber = 1m,
						AddPrice = 0m,
						IsDefault = true
					});
				}
				repeater.DataSource = specifyRegionGroups;
				repeater.DataBind();
			}
		}
	}
}
