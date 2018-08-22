using System;

namespace Hidistro.ControlPanel.VShop
{
	public class UpdateStatistics
	{
		public void Update(object sender, StatisticNotifier.DataUpdatedEventArgs e)
		{
			StatisticNotifier statisticNotifier = (StatisticNotifier)sender;
			string text = "";
			try
			{
				ShopStatisticHelper.StatisticsOrdersByNotify(statisticNotifier.RecDateUpdate, statisticNotifier.updateAction, statisticNotifier.actionDesc, out text);
			}
			catch (Exception var_2_2C)
			{
			}
		}
	}
}
