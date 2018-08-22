using Hishop.Components.Validation.Validators;
using System;
using System.Collections.Generic;

namespace Hidistro.Entities.Commodities
{
	public class AttributeInfo
	{
		private System.Collections.Generic.IList<AttributeValueInfo> attributeValues;

		public int AttributeId
		{
			get;
			set;
		}

		[StringLengthValidator(1, 30, Ruleset = "ValAttribute", MessageTemplate = "扩展属性的名称，长度在1至30个字符之间")]
		public string AttributeName
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}

		public int TypeId
		{
			get;
			set;
		}

		public string TypeName
		{
			get;
			set;
		}

		public AttributeUseageMode UsageMode
		{
			get;
			set;
		}

		public bool UseAttributeImage
		{
			get;
			set;
		}

		public bool IsMultiView
		{
			get
			{
				return this.UsageMode == AttributeUseageMode.MultiView;
			}
		}

		public System.Collections.Generic.IList<AttributeValueInfo> AttributeValues
		{
			get
			{
				if (this.attributeValues == null)
				{
					this.attributeValues = new System.Collections.Generic.List<AttributeValueInfo>();
				}
				return this.attributeValues;
			}
			set
			{
				this.attributeValues = value;
			}
		}

		public string ValuesString
		{
			get
			{
				string text = string.Empty;
				foreach (AttributeValueInfo current in this.AttributeValues)
				{
					text = text + current.ValueStr + ",";
				}
				if (text.Length > 0)
				{
					text = text.Substring(0, text.Length - 1);
				}
				return text;
			}
		}
	}
}
