using Hishop.Components.Validation;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class ShippingModeInfo
	{
		private System.Collections.Generic.IList<ShippingModeGroupInfo> modeGroup;

		private System.Collections.Generic.IList<string> expressCompany;

		public int TemplateId
		{
			get;
			set;
		}

		public int ModeId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 60, Ruleset = "ValShippingModeInfo", MessageTemplate = "配送方式名称不能为空，长度限制在60字符以内")]
		public string Name
		{
			get;
			set;
		}

		public string TemplateName
		{
			get;
			set;
		}

		[RangeValidator(0, RangeBoundaryType.Inclusive, 100000, RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo", MessageTemplate = "起步重量不能为空,限制在100千克以内")]
		public decimal Weight
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValShippingModeInfo"), RangeValidator(0, RangeBoundaryType.Inclusive, 100000, RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo"), ValidatorComposition(CompositionType.Or, Ruleset = "ValShippingModeInfo", MessageTemplate = "加价重量必须限制在100千克以内")]
		public decimal? AddWeight
		{
			get;
			set;
		}

		[RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo", MessageTemplate = "默认起步价不能为空,限制在1000万以内")]
		public decimal Price
		{
			get;
			set;
		}

		[NotNullValidator(Negated = true, Ruleset = "ValShippingModeInfo"), RangeValidator(typeof(decimal), "0.00", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValShippingModeInfo"), ValidatorComposition(CompositionType.Or, Ruleset = "ValShippingModeInfo", MessageTemplate = "默认加价必须限制在1000万以内")]
		public decimal? AddPrice
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<ShippingModeGroupInfo> ModeGroup
		{
			get
			{
				if (this.modeGroup == null)
				{
					this.modeGroup = new System.Collections.Generic.List<ShippingModeGroupInfo>();
				}
				return this.modeGroup;
			}
			set
			{
				this.modeGroup = value;
			}
		}

		public System.Collections.Generic.IList<string> ExpressCompany
		{
			get
			{
				if (this.expressCompany == null)
				{
					this.expressCompany = new System.Collections.Generic.List<string>();
				}
				return this.expressCompany;
			}
			set
			{
				this.expressCompany = value;
			}
		}

		public int DisplaySequence
		{
			get;
			set;
		}
	}
}
