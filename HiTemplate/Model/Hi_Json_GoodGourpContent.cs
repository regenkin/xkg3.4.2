using System;
using System.Collections.Generic;

namespace HiTemplate.Model
{
	public class Hi_Json_GoodGourpContent
	{
		public int layout
		{
			get;
			set;
		}

		public bool showPrice
		{
			get;
			set;
		}

		public bool showIco
		{
			get;
			set;
		}

		public bool showName
		{
			get;
			set;
		}

		public int goodsize
		{
			get;
			set;
		}

		public GoodGourp group
		{
			get;
			set;
		}

		public IList<HiShop_Model_Good> goodslist
		{
			get;
			set;
		}
	}
}
