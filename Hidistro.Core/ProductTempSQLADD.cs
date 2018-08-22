using System;

namespace Hidistro.Core
{
	public static class ProductTempSQLADD
	{
		public static string ReturnShowOrder(ProductShowOrderPriority show)
		{
			string result;
			if (show == ProductShowOrderPriority.IDDESC)
			{
				result = " DisplaySequence DESC";
			}
			else if (show == ProductShowOrderPriority.AddedDateDESC)
			{
				result = " AddedDate DESC";
			}
			else if (show == ProductShowOrderPriority.AddedDateASC)
			{
				result = " AddedDate ASC";
			}
			else if (show == ProductShowOrderPriority.ShowSaleCountsDESC)
			{
				result = " ShowSaleCounts DESC";
			}
			else
			{
				result = "";
			}
			return result;
		}
	}
}
