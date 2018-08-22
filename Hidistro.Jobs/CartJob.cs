using Microsoft.Practices.EnterpriseLibrary.Data;
using Quartz;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.Jobs
{
	public class CartJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			int num = 5;
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE AddTime <= @CurrentTime");
			database.AddInParameter(sqlStringCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now.AddDays((double)(-(double)num)));
			database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
