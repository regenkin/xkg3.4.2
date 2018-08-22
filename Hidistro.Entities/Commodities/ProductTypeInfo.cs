using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	public class ProductTypeInfo
	{
		private System.Collections.Generic.IList<int> brands;

		public int TypeId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "ValProductType", MessageTemplate = "商品类型名称不能为空，长度限制在1-30个字符之间")]
		public string TypeName
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<int> Brands
		{
			get
			{
				if (this.brands == null)
				{
					this.brands = new System.Collections.Generic.List<int>();
				}
				return this.brands;
			}
			set
			{
				this.brands = value;
			}
		}

		[HtmlCoding, StringLengthValidator(0, 100, Ruleset = "ValProductType", MessageTemplate = "备注的长度限制在0-100个字符之间")]
		public string Remark
		{
			get;
			set;
		}
	}
}
