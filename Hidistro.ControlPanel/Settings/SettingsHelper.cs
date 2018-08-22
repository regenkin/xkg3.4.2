using Hidistro.Entities.Settings;
using Hidistro.SqlDal.Sales;
using Hidistro.SqlDal.Settings;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.ControlPanel.Settings
{
	public sealed class SettingsHelper
	{
		public static string Error = "";

		public static bool CreateShippingTemplate(FreightTemplate freightTemplate)
		{
			ShippingModeDao shippingModeDao = new ShippingModeDao();
			bool result = shippingModeDao.CreateShippingTemplate(freightTemplate);
			SettingsHelper.Error = shippingModeDao.Error;
			return result;
		}

		public static bool UpdateShippingTemplate(FreightTemplate freightTemplate)
		{
			ShippingModeDao shippingModeDao = new ShippingModeDao();
			bool result = shippingModeDao.UpdateShippingTemplate(freightTemplate);
			SettingsHelper.Error = shippingModeDao.Error;
			return result;
		}

		public static string getShippingTypeByModeId(int ModeId)
		{
			string result = "未知";
			switch (ModeId)
			{
			case 1:
				result = "快递";
				break;
			case 2:
				result = "EMS";
				break;
			case 3:
				result = "顺丰";
				break;
			case 4:
				result = "平邮";
				break;
			}
			return result;
		}

		public static string getFreeShipText(bool FreeShip)
		{
			string result = "卖家承担";
			if (FreeShip)
			{
				result = "包邮";
			}
			return result;
		}

		public static string getDefaultShipText(bool IsDefault)
		{
			string result = "";
			if (IsDefault)
			{
				result = "全国";
			}
			return result;
		}

		public static string getMUnitText(int MUnit)
		{
			string result = "件";
			switch (MUnit)
			{
			case 1:
				result = "件";
				break;
			case 2:
				result = "KG";
				break;
			case 3:
				result = "立方";
				break;
			}
			return result;
		}

		public static bool DeleteShippingTemplate(int templateId)
		{
			return new ShippingModeDao().DeleteShippingTemplate(templateId);
		}

		public static int DeleteShippingTemplates(string templateIds)
		{
			return new ShippingModeDao().DeleteShippingTemplates(templateIds);
		}

		public static string GetShippingTemplateLinkProduct(int[] templateIds)
		{
			return new ShippingModeDao().GetShippingTemplateLinkProduct(templateIds);
		}

		public static bool SetExpressIsDefault(int expressId)
		{
			return new ExpressTemplateDao().SetExpressDefault(expressId);
		}

		public static int DeleteExpressTemplates(string expressIds)
		{
			return new ExpressTemplateDao().DeleteExpressTemplates(expressIds);
		}

		public static IList<FreightTemplate> GetFreightTemplates()
		{
			return new ExpressTemplateDao().GetFreightTemplates();
		}

		public static FreightTemplate GetFreightTemplate(int templateId, bool includeDetail)
		{
			return new ShippingModeDao().GetFreightTemplate(templateId, includeDetail);
		}

		public static IList<SpecifyRegionGroup> GetSpecifyRegionGroups(int templateId)
		{
			return new ShippingModeDao().GetSpecifyRegionGroups(templateId);
		}

		public static System.Data.DataTable GetAllFreightItems()
		{
			return new ShippingModeDao().GetAllFreightItems();
		}

		public static System.Data.DataTable GetSpecifyRegionGroupsModeId(string TemplateIds, string RegionId)
		{
			return new ShippingModeDao().GetSpecifyRegionGroupsModeId(TemplateIds, RegionId);
		}

		public static FreightTemplate GetTemplateMessage(int TemplateId)
		{
			return new ShippingModeDao().GetTemplateMessage(TemplateId);
		}

		public static System.Data.DataTable GetFreeTemplateShipping(string RegionId, int TemplateId, int ModeId)
		{
			return new ShippingModeDao().GetFreeTemplateShipping(RegionId, TemplateId, ModeId);
		}
	}
}
