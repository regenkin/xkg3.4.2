using Hidistro.Entities.StatisticsReport;
using System;

namespace Hidistro.ControlPanel.VShop
{
	public class StatisticNotifier
	{
		public delegate void DataUpdatedEventHandler(object sender, StatisticNotifier.DataUpdatedEventArgs e);

		public class DataUpdatedEventArgs : EventArgs
		{
			public readonly int temperature;
		}

		public string actionDesc = "";

		public UpdateAction updateAction;

		public DateTime RecDateUpdate;

		public event StatisticNotifier.DataUpdatedEventHandler DataUpdated;

		public virtual void OnDataUpdated(StatisticNotifier.DataUpdatedEventArgs e)
		{
			if (this.DataUpdated != null)
			{
				this.DataUpdated(this, e);
			}
		}

		public void UpdateDB()
		{
			StatisticNotifier.DataUpdatedEventArgs e = new StatisticNotifier.DataUpdatedEventArgs();
			this.OnDataUpdated(e);
		}
	}
}
