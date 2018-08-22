using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Settings
{
	public class FreeShipping
	{
		private System.Collections.Generic.IList<FreeShippingRegion> freeShippingRegions;

		public System.Collections.Generic.IList<FreeShippingRegion> FreeShippingRegions
		{
			get
			{
				return this.freeShippingRegions;
			}
			set
			{
				this.freeShippingRegions = value;
			}
		}

		public int FreeId
		{
			get;
			set;
		}

		public int TemplateId
		{
			get;
			set;
		}

		[RangeValidator(1, RangeBoundaryType.Inclusive, 10, RangeBoundaryType.Inclusive, Ruleset = "ValFree", MessageTemplate = "运输方式参数不正确！")]
		public int ModeId
		{
			get;
			set;
		}

		[RegexValidator("(^\\d+(\\$\\d+)?$)", Ruleset = "ValFree", MessageTemplate = "包邮条件未设置！")]
		public string ConditionNumber
		{
			get;
			set;
		}

		public int ConditionType
		{
			get;
			set;
		}

		public string RegionIds
		{
			get;
			set;
		}
	}
}
