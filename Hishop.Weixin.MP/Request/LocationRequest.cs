using System;

namespace Hishop.Weixin.MP.Request
{
	public class LocationRequest : AbstractRequest
	{
		public float Location_X
		{
			get;
			set;
		}

		public float Location_Y
		{
			get;
			set;
		}

		public int Scale
		{
			get;
			set;
		}

		public string Label
		{
			get;
			set;
		}
	}
}
