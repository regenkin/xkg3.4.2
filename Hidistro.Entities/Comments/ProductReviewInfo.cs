using Hidistro.Core;
using Hishop.Components.Validation.Validators;
using System;

namespace Hidistro.Entities.Comments
{
	public class ProductReviewInfo
	{
		public long ReviewId
		{
			get;
			set;
		}

		public int ProductId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string OrderId
		{
			get;
			set;
		}

		public string SkuId
		{
			get;
			set;
		}

		public int OrderItemID
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(1, 300, Ruleset = "Refer", MessageTemplate = "评论内容为必填项，长度限制在300字符以内")]
		public string ReviewText
		{
			get;
			set;
		}

		[HtmlCoding, StringLengthValidator(1, 30, Ruleset = "Refer", MessageTemplate = "用户昵称为必填项，长度限制在30字符以内")]
		public string UserName
		{
			get;
			set;
		}

		[RegexValidator("^[a-zA-Z\\.0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$", Ruleset = "Refer", MessageTemplate = "邮箱地址必须为有效格式"), StringLengthValidator(1, 256, Ruleset = "Refer", MessageTemplate = "邮箱不能为空，长度限制在256字符以内")]
		public string UserEmail
		{
			get;
			set;
		}

		public System.DateTime ReviewDate
		{
			get;
			set;
		}
	}
}
