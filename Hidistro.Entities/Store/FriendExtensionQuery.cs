using Hidistro.Core;
using Hidistro.Core.Entities;
using System;

namespace Hidistro.Entities.Store
{
	public class FriendExtensionQuery : Pagination
	{
		[HtmlCoding]
		public string ExensionImg
		{
			get;
			set;
		}

		public string ExensiontRemark
		{
			get;
			set;
		}

		public int ExtensionId
		{
			get;
			set;
		}
	}
}
