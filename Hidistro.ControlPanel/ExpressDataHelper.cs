using Hidistro.Entities.Sales;
using Hidistro.SqlDal;
using System;

namespace Hidistro.ControlPanel
{
	public class ExpressDataHelper
	{
		public bool AddExpressData(ExpressDataInfo model)
		{
			return new ExpressDataDao().AddExpressData(model);
		}

		public string GetExpressDataList(string computer, string expressNo)
		{
			return new ExpressDataDao().GetExpressDataList(computer, expressNo);
		}
	}
}
