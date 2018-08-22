using Microsoft.Practices.EnterpriseLibrary.Data;
using Quartz;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.Jobs
{
	public class CouponJob : IJob
	{
		public void Execute(IJobExecutionContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Coupon_Coupons SET Finished = 1 WHERE Finished = 0 AND EndDate <= @CurrentTime;");
			Database database = DatabaseFactory.CreateDatabase();
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(stringBuilder.ToString());
			database.AddInParameter(sqlStringCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now);
			database.ExecuteNonQuery(sqlStringCommand);
		}
	}
}
