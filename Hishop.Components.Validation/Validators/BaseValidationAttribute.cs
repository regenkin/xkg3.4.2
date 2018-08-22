using Hishop.Components.Validation.Properties;
using System;
using System.Web;

namespace Hishop.Components.Validation.Validators
{
	public abstract class BaseValidationAttribute : Attribute
	{
		private string ruleset;

		private string messageTemplate;

		private string messageTemplateResourceName;

		private Type messageTemplateResourceType;

		private string messageTemplateClassKey;

		private string messageTemplateResourceKey;

		private string tag;

		public string MessageTemplateClassKey
		{
			get
			{
				return this.messageTemplateClassKey;
			}
			set
			{
				if (this.messageTemplate != null || (this.messageTemplateResourceName != null && this.messageTemplateResourceType != null))
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceBasedMessageTemplatesIfTemplateIsSet);
				}
				this.messageTemplateClassKey = value;
			}
		}

		public string MessageTemplateResourceKey
		{
			get
			{
				return this.messageTemplateResourceKey;
			}
			set
			{
				if (this.messageTemplate != null || (this.messageTemplateResourceName != null && this.messageTemplateResourceType != null))
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceBasedMessageTemplatesIfTemplateIsSet);
				}
				this.messageTemplateResourceKey = value;
			}
		}

		public string Ruleset
		{
			get
			{
				if (this.ruleset == null)
				{
					return string.Empty;
				}
				return this.ruleset;
			}
			set
			{
				this.ruleset = value;
			}
		}

		public string MessageTemplate
		{
			get
			{
				return this.messageTemplate;
			}
			set
			{
				if (this.messageTemplateResourceName != null)
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceMessageTemplatesIfResourceTemplateIsSet);
				}
				if (this.messageTemplateResourceType != null)
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceMessageTemplatesIfResourceTemplateIsSet);
				}
				if (this.messageTemplateClassKey != null)
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceMessageTemplatesIfResourceTemplateIsSet);
				}
				if (this.messageTemplateResourceKey != null)
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceMessageTemplatesIfResourceTemplateIsSet);
				}
				this.messageTemplate = value;
			}
		}

		public string MessageTemplateResourceName
		{
			get
			{
				return this.messageTemplateResourceName;
			}
			set
			{
				if (this.messageTemplate != null || (this.messageTemplateClassKey != null && this.messageTemplateResourceKey != null))
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceBasedMessageTemplatesIfTemplateIsSet);
				}
				this.messageTemplateResourceName = value;
			}
		}

		public Type MessageTemplateResourceType
		{
			get
			{
				return this.messageTemplateResourceType;
			}
			set
			{
				if (this.messageTemplate != null || (this.messageTemplateClassKey != null && this.messageTemplateResourceKey != null))
				{
					throw new InvalidOperationException(Resources.ExceptionCannotSetResourceBasedMessageTemplatesIfTemplateIsSet);
				}
				this.messageTemplateResourceType = value;
			}
		}

		public string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
			}
		}

		protected internal string GetMessageTemplate()
		{
			if (this.messageTemplate != null)
			{
				return this.messageTemplate;
			}
			if (this.messageTemplateClassKey != null && this.messageTemplateResourceKey != null)
			{
				return (string)HttpContext.GetGlobalResourceObject(this.messageTemplateClassKey, this.messageTemplateResourceKey);
			}
			if (this.messageTemplateResourceName != null && this.messageTemplateResourceType != null)
			{
				return ResourceLoader.LoadString(this.messageTemplateResourceType.FullName, this.messageTemplateResourceName, this.messageTemplateResourceType.Assembly);
			}
			if (this.messageTemplateResourceName != null || this.messageTemplateResourceType != null)
			{
				throw new InvalidOperationException(Resources.ExceptionPartiallyDefinedResourceForMessageTemplate);
			}
			return null;
		}
	}
}
