using Hidistro.ControlPanel.VShop;
using System;

namespace Hidistro.Jobs
{
	public class AsyncWorkDelegate_TongJi
	{
		public void CalData(string AppPath, out bool result)
		{
			string text = "";
			ShopStatisticHelper.AutoStatisticsOrdersV2(AppPath, out text);
			result = true;
		}
	}
}
