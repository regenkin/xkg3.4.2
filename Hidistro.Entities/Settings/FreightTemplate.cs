using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Settings
{
	public class FreightTemplate
	{
		private System.Collections.Generic.IList<FreeShipping> freeShippings;

		private System.Collections.Generic.IList<SpecifyRegionGroup> specifyRegionGroups;

		public System.Collections.Generic.IList<SpecifyRegionGroup> SpecifyRegionGroups
		{
			get
			{
				return this.specifyRegionGroups;
			}
			set
			{
				this.specifyRegionGroups = value;
			}
		}

		public System.Collections.Generic.IList<FreeShipping> FreeShippings
		{
			get
			{
				return this.freeShippings;
			}
			set
			{
				this.freeShippings = value;
			}
		}

		public int TemplateId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 20, Ruleset = "ValFreight", MessageTemplate = "请填写模板名称，2-20个字符！")]
		public string Name
		{
			get;
			set;
		}

		public bool FreeShip
		{
			get;
			set;
		}

		public int MUnit
		{
			get;
			set;
		}

		public bool HasFree
		{
			get;
			set;
		}
	}
}
