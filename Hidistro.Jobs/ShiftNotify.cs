using Hidistro.ControlPanel.VShop;
using Hidistro.Core;
using Quartz;
using System;

namespace Hidistro.Jobs
{
	public class ShiftNotify : IJob
	{
		public string AppPath = "";

		public void Execute(IJobExecutionContext context)
		{
			string logname = "_Tonji.txt";
			try
			{
				Globals.Debuglog("定时器正执行指定任务AutoStatisticsOrdersV2...", logname);
				string str = "";
				ShopStatisticHelper.AutoStatisticsOrdersV2("", out str);
				Globals.Debuglog("任务执行完毕。结果：" + str, logname);
			}
			catch (Exception ex)
			{
				Globals.Debuglog("任务执行出错：" + ex.Message, logname);
			}
		}
	}
}
