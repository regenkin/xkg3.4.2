using Hidistro.Core;
using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Comments
{
	public class ProductConsultationAndReplyQuery : Pagination
	{
		public int ConsultationId
		{
			get;
			set;
		}

		public virtual int ProductId
		{
			get;
			set;
		}

		public ConsultationReplyType Type
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		[HtmlCoding]
		public string Keywords
		{
			get;
			set;
		}

		[HtmlCoding]
		public string ProductCode
		{
			get;
			set;
		}

		public int? CategoryId
		{
			get;
			set;
		}

		public bool? HasReplied
		{
			get;
			set;
		}
	}
}
