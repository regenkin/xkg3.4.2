using Hidistro.Core;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Xml;

namespace Hidistro.Jobs
{
	public static class JobsHelp
	{
		private class jobinfo
		{
			public string name
			{
				get;
				set;
			}

			public string type
			{
				get;
				set;
			}

			public string CronExpression
			{
				get;
				set;
			}

			public bool enabled
			{
				get;
				set;
			}
		}

		private static IScheduler sched = null;

		private static string ConfigFile = "";

		public static void start(string _ConfigFile)
		{
			JobsHelp.ConfigFile = _ConfigFile;
			List<JobsHelp.jobinfo> list = new List<JobsHelp.jobinfo>();
			try
			{
				if (JobsHelp.sched != null)
				{
					JobsHelp.stop();
					JobsHelp.sched = null;
				}
				JobsHelp.sched = new StdSchedulerFactory().GetScheduler();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(JobsHelp.ConfigFile);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("Jobs");
				if (xmlNode.ChildNodes.Count > 0)
				{
					foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
					{
						JobsHelp.jobinfo jobinfo = new JobsHelp.jobinfo
						{
							name = xmlNode2.Attributes["name"].Value,
							type = xmlNode2.Attributes["type"].Value,
							CronExpression = xmlNode2.Attributes["CronExpression"].Value,
							enabled = bool.Parse(xmlNode2.Attributes["enabled"].Value)
						};
						if (jobinfo.enabled)
						{
							list.Add(jobinfo);
							IJobDetail jobDetail = JobBuilder.Create(Type.GetType(jobinfo.type)).WithIdentity(jobinfo.name, jobinfo.name + "Group").Build();
							ITrigger trigger = TriggerBuilder.Create().WithIdentity(jobinfo.name, jobinfo.name + "Group").WithCronSchedule(jobinfo.CronExpression).Build();
							JobsHelp.sched.ScheduleJob(jobDetail, trigger);
						}
					}
					if (list.Count > 0)
					{
						JobsHelp.sched.Start();
					}
					else
					{
						Globals.Debuglog("暂未有计划任务开启1", "_Debuglog.txt");
					}
				}
				else
				{
					Globals.Debuglog("暂未有计划任务开启", "_Debuglog.txt");
				}
			}
			catch (Exception ex)
			{
				Globals.Debuglog(JsonConvert.SerializeObject(list), "_Debuglog.txt");
				Globals.Debuglog("启动计划任务失败：" + ex.Message, "_Debuglog.txt");
			}
		}

		public static void stop()
		{
			try
			{
				if (JobsHelp.sched != null)
				{
					JobsHelp.sched.Shutdown(false);
					JobsHelp.sched.Clear();
				}
			}
			catch (Exception ex)
			{
				Globals.Debuglog("关闭计划任务失败：" + ex.Message, "_Debuglog.txt");
			}
		}
	}
}
