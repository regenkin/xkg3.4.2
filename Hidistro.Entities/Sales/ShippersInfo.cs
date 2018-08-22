using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Sales
{
	public class ShippersInfo
	{
		public int ShipperId
		{
			get;
			set;
		}

		public int DistributorUserId
		{
			get;
			set;
		}

		public bool IsDefault
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(1, 30, Ruleset = "Valshipper", MessageTemplate = "发货点不能为空，长度限制在30个字符以内")]
		public string ShipperTag
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(2, 20, Ruleset = "Valshipper", MessageTemplate = "发货人姓名不能为空，长度在2-20个字符之间")]
		public string ShipperName
		{
			get;
			set;
		}

		public int RegionId
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(1, 300, Ruleset = "Valshipper", MessageTemplate = "详细地址不能为空，长度限制在300个字符以内")]
		public string Address
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(0, 20, Ruleset = "Valshipper", MessageTemplate = "电话号码的长度限制在20个字符以内")]
		public string TelPhone
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(0, 20, Ruleset = "Valshipper", MessageTemplate = "手机号码的长度限制在20个字符以内")]
		public string CellPhone
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(0, 20, Ruleset = "Valshipper", MessageTemplate = "邮编的长度限制在20个字符以内")]
		public string Zipcode
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "Valshipper", MessageTemplate = "备注的长度限制在300个字符以内")]
		public string Remark
		{
			get;
			set;
		}
	}
}
