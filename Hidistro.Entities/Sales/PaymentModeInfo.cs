using Hidistro.Core;
using System;

namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class PaymentModeInfo
	{
		public int ModeId
		{
			get;
			set;
		}

		[HtmlCoding]
		public string Name
		{
			get;
			set;
		}

		public string Settings
		{
			get;
			set;
		}

		public string Description
		{
			get;
			set;
		}

		public string Gateway
		{
			get;
			set;
		}

		public int DisplaySequence
		{
			get;
			set;
		}

		public bool IsUseInpour
		{
			get;
			set;
		}

		public bool IsUseInDistributor
		{
			get;
			set;
		}

		public decimal Charge
		{
			get;
			set;
		}

		public bool IsPercent
		{
			get;
			set;
		}

		public PayApplicationType ApplicationType
		{
			get;
			set;
		}

		public decimal CalcPayCharge(decimal cartMoney)
		{
			decimal result;
			if (!this.IsPercent)
			{
				result = this.Charge;
			}
			else
			{
				result = cartMoney * (this.Charge / 100m);
			}
			return result;
		}
	}
}
