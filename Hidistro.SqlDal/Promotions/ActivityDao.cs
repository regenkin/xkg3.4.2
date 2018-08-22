using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class ActivityDao
	{
		private Database database;

		public ActivityDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int Create(ActivityInfo act, ref string msg)
		{
			msg = "未知错误";
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE ActivitiesName=@Name");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, act.ActivitiesName);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					msg = "减免活动重名";
					result = 0;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE isAllProduct=1 and  EndTime>@NowTime");
					this.database.AddInParameter(sqlStringCommand, "NowTime", System.Data.DbType.DateTime, DateTime.Now);
					if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
					{
						msg = "已有全部商品满减活动未结束，不能再添加新活动！";
						result = 0;
					}
					else
					{
						sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Activities] ([ActivitiesName] ,[ActivitiesType],[StartTime],[EndTime],[ActivitiesDescription],[TakeEffect],[Type],[MemberGrades],[DefualtGroup],[CustomGroup],[attendTime],[attendType],[isAllProduct],[MeetMoney],[ReductionMoney],[MeetType]) VALUES (@ActivitiesName,@ActivitiesType,@StartTime,@EndTime,@ActivitiesDescription,@TakeEffect,@Type,@MemberGrades,@DefualtGroup,@CustomGroup,@attendTime,@attendType,@isAllProduct,@MeetMoney,@ReductionMoney ,@MeetType); SELECT CAST(scope_identity() AS int);");
						this.database.AddInParameter(sqlStringCommand, "ActivitiesName", System.Data.DbType.String, act.ActivitiesName);
						this.database.AddInParameter(sqlStringCommand, "ActivitiesType", System.Data.DbType.Int32, act.ActivitiesType);
						this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, act.StartTime);
						this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, act.EndTime);
						this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", System.Data.DbType.String, act.ActivitiesDescription);
						this.database.AddInParameter(sqlStringCommand, "TakeEffect", System.Data.DbType.Int32, act.TakeEffect);
						this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, act.Type);
						this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, act.MemberGrades);
						this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, act.DefualtGroup);
						this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, act.CustomGroup);
						this.database.AddInParameter(sqlStringCommand, "attendTime", System.Data.DbType.Int32, act.attendTime);
						this.database.AddInParameter(sqlStringCommand, "attendType", System.Data.DbType.Int32, act.attendType);
						this.database.AddInParameter(sqlStringCommand, "isAllProduct", System.Data.DbType.Boolean, act.isAllProduct);
						this.database.AddInParameter(sqlStringCommand, "MeetMoney", System.Data.DbType.Decimal, act.MeetMoney);
						this.database.AddInParameter(sqlStringCommand, "ReductionMoney", System.Data.DbType.Decimal, act.ReductionMoney);
						this.database.AddInParameter(sqlStringCommand, "MeetType", System.Data.DbType.Int32, act.MeetType);
						int num = (int)this.database.ExecuteScalar(sqlStringCommand);
						act.ActivitiesId = num;
						if (act.Details != null)
						{
							if (act.Details.Count > 0)
							{
								string query = "INSERT INTO [Hishop_Activities_Detail]([ActivitiesId],[MeetMoney],[ReductionMoney],[bFreeShipping],[Integral],[CouponId],[MeetNumber])VALUES(@ActivitiesId,@MeetMoney,@ReductionMoney,@bFreeShipping,@Integral,@CouponId ,@MeetNumber)";
								foreach (ActivityDetailInfo current in act.Details)
								{
									sqlStringCommand = this.database.GetSqlStringCommand(query);
									this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, act.ActivitiesId);
									this.database.AddInParameter(sqlStringCommand, "MeetMoney", System.Data.DbType.Decimal, current.MeetMoney);
									this.database.AddInParameter(sqlStringCommand, "ReductionMoney", System.Data.DbType.Decimal, current.ReductionMoney);
									this.database.AddInParameter(sqlStringCommand, "bFreeShipping", System.Data.DbType.Boolean, current.bFreeShipping);
									this.database.AddInParameter(sqlStringCommand, "Integral", System.Data.DbType.Int32, current.Integral);
									this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, current.CouponId);
									this.database.AddInParameter(sqlStringCommand, "MeetNumber", System.Data.DbType.Int32, current.MeetNumber);
									this.database.ExecuteScalar(sqlStringCommand);
								}
							}
						}
						msg = "";
						result = num;
					}
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = 0;
			}
			return result;
		}

		public bool EndAct(int Aid)
		{
			string query = "update Hishop_Activities set EndTime=@EndTime where ActivitiesId=@ActivitiesId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, Aid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Update(ActivityInfo act, ref string msg)
		{
			msg = "未知错误";
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE ActivitiesName=@Name and ActivitiesId<>@ID");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, act.ActivitiesName);
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, act.ActivitiesId);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					msg = "满减活动重名";
					result = false;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT ActivitiesId  FROM Hishop_Activities WHERE isAllProduct=1 and  EndTime>@NowTime and ActivitiesId<>@ActivitiesId");
					this.database.AddInParameter(sqlStringCommand, "NowTime", System.Data.DbType.DateTime, DateTime.Now);
					this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.String, act.ActivitiesId);
					if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
					{
						msg = "已有全部商品满减活动正在进行中，不能再添加新活动！";
						result = false;
					}
					else
					{
						sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [dbo].[Hishop_Activities] SET [ActivitiesName] = @ActivitiesName,  [ActivitiesType] = @ActivitiesType  ,[StartTime] = @StartTime   ,[EndTime] = @EndTime  ,[ActivitiesDescription] = @ActivitiesDescription ,[Type] = @Type ,[MemberGrades] = @MemberGrades  ,[DefualtGroup]=@DefualtGroup  ,[CustomGroup]=@CustomGroup  ,[attendTime] = @attendTime ,[attendType] = @attendType  ,[isAllProduct] = @isAllProduct  ,[MeetMoney] = @MeetMoney  ,[MeetType] = @MeetType  ,[ReductionMoney] = @ReductionMoney where ActivitiesId=@ID");
						this.database.AddInParameter(sqlStringCommand, "ActivitiesName", System.Data.DbType.String, act.ActivitiesName);
						this.database.AddInParameter(sqlStringCommand, "ActivitiesType", System.Data.DbType.Int32, act.ActivitiesType);
						this.database.AddInParameter(sqlStringCommand, "ActivitiesDescription", System.Data.DbType.String, act.ActivitiesDescription);
						this.database.AddInParameter(sqlStringCommand, "Type", System.Data.DbType.Int32, act.Type);
						this.database.AddInParameter(sqlStringCommand, "attendTime", System.Data.DbType.Int32, act.attendTime);
						this.database.AddInParameter(sqlStringCommand, "attendType", System.Data.DbType.Int32, act.attendType);
						this.database.AddInParameter(sqlStringCommand, "MeetMoney", System.Data.DbType.Decimal, act.MeetMoney);
						this.database.AddInParameter(sqlStringCommand, "ReductionMoney", System.Data.DbType.Decimal, act.ReductionMoney);
						this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, act.MemberGrades);
						this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, act.DefualtGroup);
						this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, act.CustomGroup);
						this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, act.StartTime);
						this.database.AddInParameter(sqlStringCommand, "EndTime", System.Data.DbType.DateTime, act.EndTime);
						this.database.AddInParameter(sqlStringCommand, "isAllProduct", System.Data.DbType.Boolean, act.isAllProduct);
						this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.String, act.ActivitiesId);
						this.database.AddInParameter(sqlStringCommand, "MeetType", System.Data.DbType.Int32, act.MeetType);
						object obj = this.database.ExecuteScalar(sqlStringCommand);
						if (act.Details != null)
						{
							if (act.Details.Count > 0)
							{
								string query = string.Format("delete from Hishop_Activities_Detail where ActivitiesId={0};", act.ActivitiesId);
								sqlStringCommand = this.database.GetSqlStringCommand(query);
								this.database.ExecuteScalar(sqlStringCommand);
								string query2 = "INSERT INTO [Hishop_Activities_Detail]([ActivitiesId],[MeetMoney],[ReductionMoney],[bFreeShipping],[Integral],[CouponId],[MeetNumber])VALUES(@ActivitiesId,@MeetMoney,@ReductionMoney,@bFreeShipping,@Integral,@CouponId,@MeetNumber)";
								foreach (ActivityDetailInfo current in act.Details)
								{
									sqlStringCommand = this.database.GetSqlStringCommand(query2);
									this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, act.ActivitiesId);
									this.database.AddInParameter(sqlStringCommand, "MeetMoney", System.Data.DbType.Decimal, current.MeetMoney);
									this.database.AddInParameter(sqlStringCommand, "ReductionMoney", System.Data.DbType.Decimal, current.ReductionMoney);
									this.database.AddInParameter(sqlStringCommand, "bFreeShipping", System.Data.DbType.Boolean, current.bFreeShipping);
									this.database.AddInParameter(sqlStringCommand, "Integral", System.Data.DbType.Int32, current.Integral);
									this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, current.CouponId);
									this.database.AddInParameter(sqlStringCommand, "MeetNumber", System.Data.DbType.Int32, current.MeetNumber);
									this.database.ExecuteScalar(sqlStringCommand);
								}
							}
						}
						msg = "";
						result = true;
					}
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = false;
			}
			return result;
		}

		public ActivityInfo GetAct(int Id)
		{
			ActivityInfo activityInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities WHERE ActivitiesId = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				activityInfo = ReaderConvert.ReaderToModel<ActivityInfo>(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities_Detail WHERE ActivitiesId = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				IList<ActivityDetailInfo> details = ReaderConvert.ReaderToList<ActivityDetailInfo>(dataReader);
				activityInfo.Details = details;
			}
			return activityInfo;
		}

		public ActivityInfo GetAct(string name)
		{
			ActivityInfo activityInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities WHERE ActivitiesName= @Name");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.Int32, name);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				activityInfo = ReaderConvert.ReaderToModel<ActivityInfo>(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Activities_Detail WHERE ActivitiesId = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, activityInfo.ActivitiesId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				IList<ActivityDetailInfo> details = ReaderConvert.ReaderToList<ActivityDetailInfo>(dataReader);
				activityInfo.Details = details;
			}
			return activityInfo;
		}

		public bool HasPartProductAct()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_Activities WHERE isAllProduct=0 and EndTime >= getdate()");
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) > 0;
		}

		public bool Delete(int Id)
		{
			bool result = false;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Activities WHERE ActivitiesId = @Id");
				this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
				System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DELETE FROM Hishop_Activities_Detail WHERE ActivitiesId = @Id");
				this.database.AddInParameter(sqlStringCommand2, "Id", System.Data.DbType.Int32, Id);
				System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("DELETE FROM Hishop_Activities_Product WHERE ActivitiesId = @Id");
				this.database.AddInParameter(sqlStringCommand3, "Id", System.Data.DbType.Int32, Id);
				try
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
					this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction);
					this.database.ExecuteNonQuery(sqlStringCommand3, dbTransaction);
					dbTransaction.Commit();
					result = true;
				}
				catch
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					if (dbTransaction.Connection != null)
					{
						dbConnection.Close();
					}
				}
			}
			return result;
		}

		public DbQueryResult Query(ActivitySearch query)
		{
			StringBuilder stringBuilder = new StringBuilder("1=1 ");
			if (query.status != ActivityStatus.All)
			{
				if (query.status == ActivityStatus.In)
				{
					stringBuilder.AppendFormat("and [StartTime] <= '{0}' and  [EndTime] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == ActivityStatus.End)
				{
					stringBuilder.AppendFormat("and [EndTime] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				else if (query.status == ActivityStatus.unBegin)
				{
					stringBuilder.AppendFormat("and [StartTime] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
			}
			if (query.begin.HasValue)
			{
				stringBuilder.AppendFormat("and [StartTime] >= '{0}'", query.begin.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (query.end.HasValue)
			{
				stringBuilder.AppendFormat("and [EndTime] <= '{0}'", query.end.Value.ToString("yyyy-MM-dd HH:mm:ss"));
			}
			if (!string.IsNullOrEmpty(query.Name))
			{
				stringBuilder.AppendFormat("and ActivitiesName like '%{0}%'  ", query.Name);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Activities", "ActivitiesId", stringBuilder.ToString(), "*");
		}

		public System.Data.DataTable QueryProducts(int actid)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select * from  Hishop_Activities_Product where ActivitiesId = {0} ", actid);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = dataTable;
			}
			return result;
		}

		public int GetHishop_Activities(int Activities_DetailID)
		{
			string query = "select ActivitiesId from Hishop_Activities_Detail where id=" + Activities_DetailID;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj == null || obj is DBNull)
			{
				result = 0;
			}
			else
			{
				result = int.Parse(obj.ToString());
			}
			return result;
		}

		public System.Data.DataTable QueryProducts()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("select * from  Hishop_Activities_Product", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = dataTable;
			}
			return result;
		}

		public System.Data.DataTable GetActProducts(int actId)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ActivitiesId, a.ProductId , b.ProductName , a.status from Hishop_Activities_Product a\tjoin Hishop_Products b on a.ProductId=b.ProductId where a.ActivitiesId=@ActivitiesId");
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetActivitiesProducts(int actId, int ProductID)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Activities_Product  where status=0 and ActivitiesId=@ActivitiesId and ProductID=@ProductID");
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
			this.database.AddInParameter(sqlStringCommand, "ProductID", System.Data.DbType.Int32, ProductID);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetActivities_Detail(int actId)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from  vw_Hishop_Activities_Detail  WHERE  StartTime<=getdate() and getdate()<=EndTIme and ActivitiesId=@ActivitiesId order by MeetMoney asc ");
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetActivities()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from  Hishop_Activities  WHERE  StartTime<=getdate() and getdate()<=EndTIme  ");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public int GetActivitiesMember(int Userid, int ActivitiesId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(id) from Hishop_ActivitiesMember where Userid=@Userid and ActivitiesId=@ActivitiesId  ");
			this.database.AddInParameter(sqlStringCommand, "Userid", System.Data.DbType.Int32, Userid);
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, ActivitiesId);
			int result = 0;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = int.Parse(dataTable.Rows[0][0].ToString());
			}
			return result;
		}

		public bool AddActivitiesMember(int ActivitiesId, int Userid, System.Data.Common.DbTransaction dbTran = null)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ActivitiesMember  (ActivitiesId,Userid)VALUES(@ActivitiesId,@Userid)");
			this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, ActivitiesId);
			this.database.AddInParameter(sqlStringCommand, "Userid", System.Data.DbType.Int32, Userid);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}

		public bool DeleteProducts(int actId, string ProductIds)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Activities_Product WHERE ActivitiesId ={0} and ProductId in ( {1} )", actId, ProductIds.ReplaceSingleQuoteMark()));
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool SetProductsStatus(int actId, int status, string ProductIds)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_Activities_Product set status={0}   WHERE ActivitiesId ={1} and ProductId in ({2})", status, actId, ProductIds.ReplaceSingleQuoteMark()));
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool AddProducts(int actId, bool IsAllProduct, IList<string> productIDs)
		{
			bool result;
			try
			{
				ActivityInfo act = this.GetAct(actId);
				if (act != null)
				{
					if (IsAllProduct)
					{
						act.isAllProduct = true;
					}
					else
					{
						act.isAllProduct = false;
					}
					if (IsAllProduct)
					{
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Activities set IsAllProduct=@IsAllProduct  WHERE ActivitiesId = @ActivitiesId");
						this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
						this.database.AddInParameter(sqlStringCommand, "IsAllProduct", System.Data.DbType.Boolean, act.isAllProduct);
						this.database.ExecuteNonQuery(sqlStringCommand);
						sqlStringCommand = this.database.GetSqlStringCommand("Delete from  Hishop_Activities_Product WHERE ActivitiesId = @ActivitiesId");
						this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
						this.database.ExecuteNonQuery(sqlStringCommand);
						result = true;
					}
					else
					{
						string text = "";
						if (productIDs.Count > 0)
						{
							foreach (string current in productIDs)
							{
								text = text + "," + current;
							}
						}
						text = text.Substring(1);
						System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from  Hishop_Activities_Product WHERE ActivitiesId ={0} and ProductId in ( {1} )", actId, text));
						this.database.ExecuteNonQuery(sqlStringCommand);
						StringBuilder stringBuilder = new StringBuilder();
						foreach (string current2 in productIDs)
						{
							stringBuilder.AppendFormat(" insert into Hishop_Activities_Product(ActivitiesId , ProductId) values({0} , {1}) ", act.ActivitiesId, current2);
						}
						sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
						this.database.ExecuteNonQuery(sqlStringCommand);
						sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Activities set IsAllProduct=@IsAllProduct WHERE ActivitiesId = @ActivitiesId");
						this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
						this.database.AddInParameter(sqlStringCommand, "IsAllProduct", System.Data.DbType.Boolean, act.isAllProduct);
						this.database.ExecuteNonQuery(sqlStringCommand);
						result = true;
					}
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public bool AddProducts(int actId, int productID)
		{
			bool result;
			try
			{
				ActivityInfo act = this.GetAct(actId);
				if (act != null)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat(" insert into Hishop_Activities_Product(ActivitiesId , ProductId) values({0} , {1}) ", act.ActivitiesId, productID);
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.ExecuteNonQuery(sqlStringCommand);
					sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Activities set IsAllProduct=0  WHERE ActivitiesId = @ActivitiesId");
					this.database.AddInParameter(sqlStringCommand, "ActivitiesId", System.Data.DbType.Int32, actId);
					this.database.ExecuteNonQuery(sqlStringCommand);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public System.Data.DataTable GetActivityTopics(string types = "0")
		{
			System.Data.DataTable result = null;
			string text = "SELECT ROW_NUMBER() OVER(ORDER BY BeginDate) num,*  FROM ((select Id,Name,MemberGrades,DefualtGroup,CustomGroup,BeginDate,EndDate,isnull(ImgUrl,'') as 'ImgUrl',1 as 'ActivityType' from Hishop_PointExChange_PointExChanges where BeginDate <= getdate() and EndDate >= getdate())  union (select ActivitiesId as 'Id',ActivitiesName as 'Name',MemberGrades,DefualtGroup,CustomGroup,StartTime as 'BeginDate',EndTime as 'EndDate', '' as 'ImgUrl', 2 as 'ActivityType' from Hishop_Activities where StartTime <= getdate() and EndTime >= getdate()) union (select CouponId as 'Id',CouponName as 'Name',MemberGrades,DefualtGroup,CustomGroup,BeginDate,EndDate,isnull(ImgUrl,'') as 'ImgUrl',3 as 'ActivityType' from Hishop_Coupon_Coupons where BeginDate <= getdate() and EndDate >= getdate() and Finished = 0 and CouponTypes like '%" + 2.ToString() + "%') union (select VoteId as 'Id',VoteName as 'Name','0' as 'MemberGrades',DefualtGroup,CustomGroup,StartDate as 'BeginDate',EndDate,isnull(ImageUrl,'') as 'ImgUrl',4 as 'ActivityType' from Hishop_Votes where StartDate <= getdate() and EndDate >= getdate()) union (select GameId as 'Id',GameTitle as 'Name',ApplyMembers as 'MemberGrades',DefualtGroup,CustomGroup,BeginTime as 'BeginDate',EndTime as 'EndDate', '' as 'ImgUrl', 5 as 'ActivityType' from Hishop_PromotionGame where BeginTime <= getdate() and EndTime >= getdate())) tb ";
			if (types != "0")
			{
				text = text + " where ActivityType in (" + types + ")";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}

		public int GetActivityTopicsNum(string types = "0")
		{
			string text = "SELECT count(id) FROM ((select Id,1 as 'ActivityType' from Hishop_PointExChange_PointExChanges where BeginDate <= getdate() and EndDate >= getdate()) union (select ActivitiesId as 'Id',2 as 'ActivityType' from Hishop_Activities where StartTime <= getdate() and EndTime >= getdate()) union (select CouponId as 'Id',3 as 'ActivityType' from Hishop_Coupon_Coupons where BeginDate <= getdate() and EndDate >= getdate() and CouponTypes like '%" + 2.ToString() + "%') union (select VoteId as 'Id',4 as 'ActivityType' from Hishop_Votes where StartDate <= getdate() and EndDate >= getdate()) union (select GameId as 'Id', 5 as 'ActivityType' from Hishop_PromotionGame where BeginTime <= getdate() and EndTime >= getdate())) tb ";
			if (types != "0")
			{
				text = text + " where ActivityType in (" + types + ")";
			}
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				result = int.Parse(dataTable.Rows[0][0].ToString());
			}
			return result;
		}
	}
}
