using System;

namespace Hidistro.Entities.VShop
{
	public class ActivitySignUpInfo
	{
		public int ActivitySignUpId
		{
			get;
			set;
		}

		public int ActivityId
		{
			get;
			set;
		}

		public int UserId
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}

		public string RealName
		{
			get;
			set;
		}

		public System.DateTime SignUpDate
		{
			get;
			set;
		}

		public string Item1
		{
			get;
			set;
		}

		public string Item2
		{
			get;
			set;
		}

		public string Item3
		{
			get;
			set;
		}

		public string Item4
		{
			get;
			set;
		}

		public string Item5
		{
			get;
			set;
		}

		public ActivitySignUpInfo()
		{
			this.SignUpDate = System.DateTime.Now;
		}
	}
}
