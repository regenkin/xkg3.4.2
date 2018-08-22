using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class ShippingModeGroupInfo
	{
		private System.Collections.Generic.IList<ShippingRegionInfo> modeRegions = new System.Collections.Generic.List<ShippingRegionInfo>();

		public int GroupId
		{
			get;
			set;
		}

		public int TemplateId
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public decimal AddPrice
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<ShippingRegionInfo> ModeRegions
		{
			get
			{
				return this.modeRegions;
			}
			set
			{
				this.modeRegions = value;
			}
		}
	}
}
