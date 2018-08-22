using System;

namespace Hidistro.Entities.Commodities
{
	[System.Serializable]
	public class AttributeValueInfo
	{
		public int ValueId
		{
			get;
			set;
		}

		public int AttributeId
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}

		public string ValueStr
		{
			get;
			set;
		}

		public string ImageUrl
		{
			get;
			set;
		}
	}
}
