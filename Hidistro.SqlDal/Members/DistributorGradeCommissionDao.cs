using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Members
{
	public class DistributorGradeCommissionDao
	{
		private Database database;

		public DistributorGradeCommissionDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public bool AddCommission(DistributorGradeCommissionInfo info)
		{
			string text = "UPDATE aspnet_Distributors set ReferralBlance=ReferralBlance+@Commission WHERE UserId=@UserID;";
			text += "INSERT INTO Hishop_DistributorGradeCommission(UserID,Commission,PubTime,OperAdmin,Memo,OrderID,OldCommissionTotal)VALUES(@UserID,@Commission,@PubTime,@OperAdmin,@Memo,@OrderID,@OldCommissionTotal);";
			text += "select @@identity;INSERT INTO Hishop_Commissions(UserId,ReferralUserId,OrderId,TradeTime,OrderTotal,CommTotal,CommType,State,CommRemark)values(@UserId,@ReferralUserId,@OrderID,@PubTime,0,@Commission,@CommType,1,@Memo);";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, info.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, info.ReferralUserId);
			this.database.AddInParameter(sqlStringCommand, "Commission", System.Data.DbType.Decimal, info.Commission);
			this.database.AddInParameter(sqlStringCommand, "PubTime", System.Data.DbType.DateTime, info.PubTime);
			this.database.AddInParameter(sqlStringCommand, "OperAdmin", System.Data.DbType.String, info.OperAdmin);
			this.database.AddInParameter(sqlStringCommand, "Memo", System.Data.DbType.String, info.Memo);
			this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, info.OrderID);
			this.database.AddInParameter(sqlStringCommand, "OldCommissionTotal", System.Data.DbType.Decimal, info.OldCommissionTotal);
			this.database.AddInParameter(sqlStringCommand, "CommType", System.Data.DbType.Int32, info.CommType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
