using Hidistro.Core;
using Quartz;
using System;

namespace Hidistro.Jobs
{
	public class ShiftNotify_Clock : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			try
			{
				Globals.Debuglog("打卡定时器定时打卡成功...", "_TonjiClock.txt");
			}
			catch (Exception ex)
			{
				Globals.Debuglog("打卡定时器定时打卡出错：" + ex.Message, "_TonjiClock.txt");
			}
		}
	}
}
