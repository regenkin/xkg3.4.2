using System;
using System.Collections.Generic;

namespace Hishop.AlipayFuwu.Api.Model
{
	public class FWButton
	{
		public string name
		{
			get;
			set;
		}

		public string actionParam
		{
			get;
			set;
		}

		public string actionType
		{
			get;
			set;
		}

		public System.Collections.Generic.IEnumerable<FWButton> subButton
		{
			get;
			set;
		}
	}
}
