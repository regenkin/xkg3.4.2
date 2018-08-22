using System;

namespace Hidistro.Entities.VShop
{
	public class UserSign
	{
		public int ID
		{
			get;
			set;
		}

		public int UserID
		{
			get;
			set;
		}

		public System.DateTime SignDay
		{
			get;
			set;
		}

		public int Continued
		{
			get;
			set;
		}

		public int Stage
		{
			get;
			set;
		}

		public UserSign()
		{
			this.SignDay = System.DateTime.Now;
		}
	}
}
