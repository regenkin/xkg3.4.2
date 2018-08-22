using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class ShareActDao
	{
		private Database database;

		public ShareActDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int Create(ShareActivityInfo share, ref string msg)
		{
			msg = "未知错误";
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_ShareActivity]([CouponId],[BeginDate],[EndDate],[MeetValue],[CouponNumber],[CouponName],[ActivityName],[ImgUrl],[ShareTitle],[Description])VALUES (@CouponId,@BeginDate,@EndDate,@MeetValue,@CouponNumber,@CouponName,@ActivityName,@ImgUrl,@ShareTitle,@Description); SELECT CAST(scope_identity() AS int);");
				this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, share.CouponId);
				this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, share.BeginDate);
				this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, share.EndDate);
				this.database.AddInParameter(sqlStringCommand, "MeetValue", System.Data.DbType.Decimal, share.MeetValue);
				this.database.AddInParameter(sqlStringCommand, "CouponNumber", System.Data.DbType.Int32, share.CouponNumber);
				this.database.AddInParameter(sqlStringCommand, "CouponName", System.Data.DbType.String, share.CouponName);
				this.database.AddInParameter(sqlStringCommand, "ActivityName", System.Data.DbType.String, share.ActivityName);
				this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, share.ImgUrl);
				this.database.AddInParameter(sqlStringCommand, "ShareTitle", System.Data.DbType.String, share.ShareTitle);
				this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, share.Description);
				int num = (int)this.database.ExecuteScalar(sqlStringCommand);
				result = num;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = 0;
			}
			return result;
		}

		public bool Update(ShareActivityInfo share, ref string msg)
		{
			msg = "未知错误";
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_ShareActivity] SET [CouponId] = @CouponId ,[BeginDate] = @BeginDate ,[EndDate] = @EndDate ,[MeetValue] = @MeetValue ,[CouponName] = @CouponName ,[CouponNumber] = @CouponNumber ,[ActivityName] = @ActivityName,[ImgUrl] = @ImgUrl,[ShareTitle] = @ShareTitle,[Description]= @Description where ID=@ID");
				this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, share.CouponId);
				this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, share.BeginDate);
				this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, share.EndDate);
				this.database.AddInParameter(sqlStringCommand, "MeetValue", System.Data.DbType.Decimal, share.MeetValue);
				this.database.AddInParameter(sqlStringCommand, "CouponName", System.Data.DbType.String, share.CouponName);
				this.database.AddInParameter(sqlStringCommand, "CouponNumber", System.Data.DbType.Int32, share.CouponNumber);
				this.database.AddInParameter(sqlStringCommand, "ActivityName", System.Data.DbType.String, share.ActivityName);
				this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, share.ImgUrl);
				this.database.AddInParameter(sqlStringCommand, "ShareTitle", System.Data.DbType.String, share.ShareTitle);
				this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, share.Description);
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.String, share.Id);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				msg = "";
				result = true;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = false;
			}
			return result;
		}

		public ShareActivityInfo GetAct(int Id)
		{
			ShareActivityInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShareActivity WHERE id = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ShareActivityInfo>(dataReader);
			}
			return result;
		}

		public bool Delete(int Id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShareActivity WHERE ID = @Id");
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public DbQueryResult Query(ShareActivitySearch query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (!string.IsNullOrEmpty(query.CouponName))
			{
				stringBuilder.AppendFormat(" and CouponName like '%{0}%' ", query.CouponName);
			}
			if (query.status != ShareActivityStatus.All)
			{
				if (query.status == ShareActivityStatus.In)
				{
					stringBuilder.AppendFormat("and [BeginDate] <= '{0}' and [EndDate] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == ShareActivityStatus.End)
				{
					stringBuilder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == ShareActivityStatus.unBegin)
				{
					stringBuilder.AppendFormat("and [BeginDate] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ShareActivity", "id", stringBuilder.ToString(), "*");
		}

		public bool AddRecord(ShareActivityRecordInfo record)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_ShareActivity_Record]([shareId],[shareUser],[attendUser]) VALUES (@shareId,@shareUser,@attendUser);");
			this.database.AddInParameter(sqlStringCommand, "shareId", System.Data.DbType.Int32, record.shareId);
			this.database.AddInParameter(sqlStringCommand, "shareUser", System.Data.DbType.Int32, record.shareUser);
			this.database.AddInParameter(sqlStringCommand, "attendUser", System.Data.DbType.Int32, record.attendUser);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int GeTAttendCount(int actId, int shareUser)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_ShareActivity_Record WHERE shareId = @shareId and shareUser=@shareUser");
			this.database.AddInParameter(sqlStringCommand, "shareId", System.Data.DbType.Int32, actId);
			this.database.AddInParameter(sqlStringCommand, "shareUser", System.Data.DbType.Int32, shareUser);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}

		public bool HasAttend(int actId, int attendUser)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_ShareActivity_Record WHERE shareId = @shareId and attendUser=@attendUser");
			this.database.AddInParameter(sqlStringCommand, "shareId", System.Data.DbType.Int32, actId);
			this.database.AddInParameter(sqlStringCommand, "attendUser", System.Data.DbType.Int32, attendUser);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetShareActivity()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from  Hishop_ShareActivity  WHERE  BeginDate<=getdate() and getdate()<=EndDate  order by MeetValue asc ");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetOrderRedPager(string OrderID, int UserID)
		{
			System.Data.DataTable result = null;
			string text = string.Empty;
			if (UserID > 0)
			{
				text = "SELECT * from  vshop_OrderRedPager WHERE  OrderID=@OrderID and UserID=@UserID ";
			}
			else if (UserID == -100)
			{
				text = "SELECT * from  vshop_OrderRedPager WHERE  OrderID=@OrderID ";
			}
			if (!string.IsNullOrEmpty(text))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "OrderID", System.Data.DbType.String, OrderID);
				this.database.AddInParameter(sqlStringCommand, "UserID", System.Data.DbType.Int32, UserID);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			return result;
		}

		public ShareActivityInfo GetShareActivity(int CouponId)
		{
			ShareActivityInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShareActivity WHERE CouponId = @CouponId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, CouponId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<ShareActivityInfo>(dataReader);
			}
			return result;
		}
	}
}
