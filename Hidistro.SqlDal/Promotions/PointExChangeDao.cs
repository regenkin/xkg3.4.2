using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Promotions
{
	public class PointExChangeDao
	{
		private Database database;

		public PointExChangeDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int Create(PointExChangeInfo exchange, ref string msg)
		{
			msg = "未知错误";
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Id  FROM Hishop_PointExChange_PointExChanges WHERE Name=@Name");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, exchange.Name);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					msg = "积分兑换活动重名";
					result = 0;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_PointExChange_PointExChanges]([Name] ,[MemberGrades],[DefualtGroup],[CustomGroup],[BeginDate],[EndDate],[ProductNumber],[ImgUrl]) VALUES(@Name  ,@MemberGrades  ,@DefualtGroup  ,@CustomGroup  ,@BeginDate  ,@EndDate  ,@ProductNumber,@ImgUrl); SELECT CAST(scope_identity() AS int);");
					this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, exchange.Name);
					this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, exchange.MemberGrades);
					this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, exchange.DefualtGroup);
					this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, exchange.CustomGroup);
					this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, exchange.BeginDate);
					this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, exchange.EndDate);
					this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, exchange.ProductNumber);
					this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, exchange.ImgUrl);
					int num = (int)this.database.ExecuteScalar(sqlStringCommand);
					msg = "";
					result = num;
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = 0;
			}
			return result;
		}

		public bool Update(PointExChangeInfo exchange, ref string msg)
		{
			msg = "未知错误";
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Id  FROM Hishop_PointExChange_PointExChanges WHERE Name=@Name and Id<>@ID");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, exchange.Name);
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, exchange.Id);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					msg = "积分兑换活动重名";
					result = false;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_PointExChange_PointExChanges] SET [Name] = @Name ,[MemberGrades] = @MemberGrades ,[DefualtGroup] = @DefualtGroup ,[CustomGroup] = @CustomGroup ,  [BeginDate] = @BeginDate ,[EndDate] = @EndDate, [ProductNumber] = @ProductNumber , [ImgUrl]=@ImgUrl where Id=@ID");
					this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, exchange.Name);
					this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, exchange.MemberGrades);
					this.database.AddInParameter(sqlStringCommand, "DefualtGroup", System.Data.DbType.String, exchange.DefualtGroup);
					this.database.AddInParameter(sqlStringCommand, "CustomGroup", System.Data.DbType.String, exchange.CustomGroup);
					this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, exchange.BeginDate);
					this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, exchange.EndDate);
					this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, exchange.ProductNumber);
					this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.String, exchange.Id);
					this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, exchange.ImgUrl);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					msg = "";
					result = true;
				}
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = false;
			}
			return result;
		}

		public PointExChangeInfo GetExChange(int Id)
		{
			PointExChangeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PointExChange_PointExChanges WHERE ID = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<PointExChangeInfo>(dataReader);
			}
			return result;
		}

		public PointExChangeInfo GetExChange(string Name)
		{
			PointExChangeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PointExChange_PointExChanges WHERE Name = @Name");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, Name);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<PointExChangeInfo>(dataReader);
			}
			return result;
		}

		public bool Delete(int Id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PointExChange_PointExChanges WHERE Id = @Id");
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable Query(ExChangeSearch search, ref int total)
		{
			System.Data.DataTable result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				total = 0;
				if (!string.IsNullOrEmpty(search.ProductName))
				{
					string value = string.Format(" and id in (select exchangeID from Hishop_PointExChange_Products a join [Hishop_Products] b on a.ProductId=b.ProductId where b.ProductName like '%{0}%' )", search.ProductName.ReplaceSingleQuoteMark());
					stringBuilder.Append(value);
				}
				if (search.status != ExchangeStatus.All)
				{
					if (search.status == ExchangeStatus.In)
					{
						stringBuilder.AppendFormat("and [BeginDate] <= '{0}' and [EndDate] >= '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					}
					else if (search.status == ExchangeStatus.End)
					{
						stringBuilder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					}
					else if (search.status == ExchangeStatus.unBegin)
					{
						stringBuilder.AppendFormat("and [BeginDate] > '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
					}
				}
				if (search.bFinished.HasValue)
				{
					stringBuilder.AppendFormat("and [EndDate] < '{0}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				}
				string query = "select count(id) as total from Hishop_PointExChange_PointExChanges where " + stringBuilder.ToString();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					total = int.Parse(dataTable.Rows[0][0].ToString());
				}
				if (total <= 0)
				{
					result = null;
				}
				else
				{
					int num = 0;
					int num2 = search.PageIndex * search.PageSize;
					if (search.PageIndex != 0 && search.PageSize != 0)
					{
						num = search.PageSize;
						if ((double)search.PageIndex >= Math.Ceiling((double)total / (double)search.PageSize))
						{
							search.PageIndex = int.Parse(Math.Ceiling((double)total / (double)search.PageSize).ToString());
						}
						int num3 = search.PageIndex * search.PageSize;
						if (num3 > total)
						{
							num = search.PageSize - (num3 - total);
						}
					}
					string arg = "(select sum(p.ProductNumber)  from Hishop_PointExChange_Products p where exChangeId=m.Id ) as TotalNumber";
					string arg2 = "(select count(c.ProductId)  from Hishop_PointExchange_Changed c where exChangeId=m.Id ) as ExChangedNumber";
					string str = "order by m.ID desc ";
					string text = "order by ID ";
					string text2 = "order by ID desc ";
					string str2 = string.Format("select top {0} m.*  , {1},{2} from Hishop_PointExChange_PointExChanges m where ", num2, arg, arg2);
					string query2 = string.Format("select * from ( select top {0} * from ( {1} ) as t1 {2} ) as t2 {3} ", new object[]
					{
						num,
						str2 + stringBuilder.ToString() + str,
						text,
						text2
					});
					sqlStringCommand = this.database.GetSqlStringCommand(query2);
					using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
					{
						System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
						result = dataTable;
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public bool InsertProduct(PointExchangeProductInfo product)
		{
			bool result;
			try
			{
				bool flag = false;
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_PointExChange_Products WHERE ProductId = @ProductId and exChangeId=@exChangeId");
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, product.ProductId);
				this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, product.exChangeId);
				int num = (int)this.database.ExecuteScalar(sqlStringCommand);
				if (num == 0)
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO [Hishop_PointExChange_Products]([exChangeId],[ProductId],[status],[ProductNumber],[PointNumber],[ChangedNumber],eachMaxNumber) VALUES (@exChangeId,@ProductId,@status,@ProductNumber,@PointNumber,@ChangedNumber,@eachMaxNumber) update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber+1 where Id=@exChangeId ");
					this.database.AddInParameter(sqlStringCommand2, "exChangeId", System.Data.DbType.Int32, product.exChangeId);
					this.database.AddInParameter(sqlStringCommand2, "ProductId", System.Data.DbType.Int32, product.ProductId);
					this.database.AddInParameter(sqlStringCommand2, "status", System.Data.DbType.Int32, product.status);
					this.database.AddInParameter(sqlStringCommand2, "ProductNumber", System.Data.DbType.Int32, product.ProductNumber);
					this.database.AddInParameter(sqlStringCommand2, "PointNumber", System.Data.DbType.Int32, product.PointNumber);
					this.database.AddInParameter(sqlStringCommand2, "ChangedNumber", System.Data.DbType.Int32, product.ChangedNumber);
					this.database.AddInParameter(sqlStringCommand2, "EachMaxNumber", System.Data.DbType.Int32, product.EachMaxNumber);
					flag = (this.database.ExecuteNonQuery(sqlStringCommand2) > 0);
				}
				result = flag;
			}
			catch (Exception var_4_178)
			{
				result = false;
			}
			return result;
		}

		public bool UpdateProduct(PointExchangeProductInfo product)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_PointExChange_Products] SET  [exChangeId] = @exChangeId  ,[ProductId] = @ProductId  ,[status] = @status  ,[ProductNumber] = @ProductNumber  ,[PointNumber] = @PointNumber  ,[ChangedNumber] = @ChangedNumber ,[eachMaxNumber]=@eachMaxNumber where [exChangeId]= @exChangeId and [ProductId] = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, product.exChangeId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, product.ProductId);
			this.database.AddInParameter(sqlStringCommand, "status", System.Data.DbType.Int32, product.status);
			this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, product.ProductNumber);
			this.database.AddInParameter(sqlStringCommand, "PointNumber", System.Data.DbType.Int32, product.PointNumber);
			this.database.AddInParameter(sqlStringCommand, "ChangedNumber", System.Data.DbType.Int32, product.ChangedNumber);
			this.database.AddInParameter(sqlStringCommand, "EachMaxNumber", System.Data.DbType.Int32, product.EachMaxNumber);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetProducts(int exchangeId)
		{
			System.Data.DataTable result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_PointExChange_Products where [exChangeId]=@exChangeId");
				this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					result = dataTable;
				}
			}
			catch (Exception var_3_59)
			{
				result = null;
			}
			return result;
		}

		public System.Data.DataTable GetProducts(int exchangeId, int pageNumber, int maxNum, out int total, string sort, bool order = false)
		{
			string filter = " status=0 and exChangeId=" + exchangeId;
			string selectFields = "ProductName,exChangeId,ProductId,status,ProductNumber,PointNumber,ChangedNumber,eachMaxNumber,ThumbnailUrl100,MemberGrades";
			DbQueryResult dbQueryResult = DataHelper.PagingByRownumber(pageNumber, maxNum, sort, order ? SortAction.Asc : SortAction.Desc, true, "vw_Hishop_PointExchange_Products", "ProductId", filter, selectFields);
			System.Data.DataTable result = (System.Data.DataTable)dbQueryResult.Data;
			total = dbQueryResult.TotalRecords;
			return result;
		}

		public bool DeleteProduct(int exchangeId, int productId)
		{
			bool result = false;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_PointExChange_Products where [exChangeId]= @exChangeId and [ProductId] = @ProductId ");
				this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
				System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber-1 where Id=@Id");
				this.database.AddInParameter(sqlStringCommand2, "Id", System.Data.DbType.Int32, exchangeId);
				try
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
					this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction);
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

		public PointExchangeProductInfo GetProductInfo(int exchangeId, int productId)
		{
			PointExchangeProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PointExChange_Products  where [exChangeId]= @exChangeId and [ProductId] = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<PointExchangeProductInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable QueryExChanged(ExChangedProductSearch search)
		{
			System.Data.DataTable result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder(" 1=1 ");
				int num = 0;
				if (search.exChangeId.HasValue)
				{
					stringBuilder.Append(string.Format(" and exChangeID={0} ", search.exChangeId.Value));
				}
				string query = "select count(ProductId) as total from Hishop_PointExchange_Changed where " + stringBuilder.ToString();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					num = int.Parse(dataTable.Rows[0][0].ToString());
				}
				if (num <= 0)
				{
					result = null;
				}
				else
				{
					int num2 = 0;
					int num3 = search.PageIndex * search.PageSize;
					if (search.PageIndex != 0 && search.PageSize != 0)
					{
						num2 = search.PageSize;
						if ((double)search.PageIndex >= Math.Ceiling((double)num / (double)search.PageSize))
						{
							search.PageIndex = int.Parse(Math.Ceiling((double)num / (double)search.PageSize).ToString());
						}
						int num4 = search.PageIndex * search.PageSize;
						if (num4 > num)
						{
							num2 = search.PageSize - (num4 - num);
						}
					}
					string str = "order by a.ExChangeID desc , a.Date desc ";
					string text = "order by ExChangeID , Date  ";
					string text2 = "order by ExChangeID desc , Date desc ";
					string str2 = string.Format("select top {0} a.* , b.ProductName,b.ThumbnailUrl40 from Hishop_PointExchange_Changed a left join [Hishop_Products] b on a.ProductID=b.ProductID  where ", num3);
					string query2 = string.Format("select * from ( select top {0} * from ( {1} ) as t1 {2} ) as t2 {3} ", new object[]
					{
						num2,
						str2 + stringBuilder.ToString() + str,
						text2,
						text
					});
					sqlStringCommand = this.database.GetSqlStringCommand(query2);
					using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
					{
						System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
						result = dataTable;
					}
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public bool SetProductsStatus(int actId, int status, string ProductIds)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Update Hishop_PointExChange_Products set status={0}  WHERE exChangeId ={1} and ProductId in ({2})", status, actId, ProductIds.ReplaceSingleQuoteMark()));
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool DeleteProducts(int actId, string ProductIds)
		{
			bool result = false;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("Delete from Hishop_PointExChange_Products WHERE exChangeId ={0} and ProductId in ( {1} )", actId, ProductIds.ReplaceSingleQuoteMark()));
				System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber-1 where Id=@Id");
				this.database.AddInParameter(sqlStringCommand2, "Id", System.Data.DbType.Int32, actId);
				try
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction);
					this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction);
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

		public bool EditProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
		{
			bool result;
			try
			{
				string[] array = productIds.Split(new char[]
				{
					','
				});
				string[] array2 = pNumbers.Split(new char[]
				{
					','
				});
				string[] array3 = points.Split(new char[]
				{
					','
				});
				string[] array4 = eachNumbers.Split(new char[]
				{
					','
				});
				int num = 0;
				for (int i = 0; i < array.Length; i++)
				{
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_PointExChange_Products] SET  [ProductNumber] = @ProductNumber  ,[PointNumber] = @PointNumber  ,[eachMaxNumber]=@eachMaxNumber where [exChangeId]= @exChangeId and [ProductId] = @ProductId");
					this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
					this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, int.Parse(array[i]));
					this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, int.Parse(array2[i]));
					this.database.AddInParameter(sqlStringCommand, "PointNumber", System.Data.DbType.Int32, int.Parse(array3[i]));
					this.database.AddInParameter(sqlStringCommand, "EachMaxNumber", System.Data.DbType.Int32, int.Parse(array4[i]));
					num += this.database.ExecuteNonQuery(sqlStringCommand);
				}
				result = (num > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool AddProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
		{
			bool result;
			try
			{
				int num = 0;
				string[] array = productIds.Split(new char[]
				{
					','
				});
				string[] array2 = pNumbers.Split(new char[]
				{
					','
				});
				string[] array3 = points.Split(new char[]
				{
					','
				});
				string[] array4 = eachNumbers.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(1) FROM Hishop_PointExChange_Products WHERE ProductId = @ProductId and exChangeId=@exChangeId");
					this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, int.Parse(array[i]));
					this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
					int num2 = (int)this.database.ExecuteScalar(sqlStringCommand);
					if (num2 == 0)
					{
						System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO [Hishop_PointExChange_Products]([exChangeId],[ProductId],[status],[ProductNumber],[PointNumber],[ChangedNumber],eachMaxNumber) VALUES (@exChangeId,@ProductId,@status,@ProductNumber,@PointNumber,@ChangedNumber,@eachMaxNumber) update Hishop_PointExChange_PointExChanges set ProductNumber=ProductNumber+1 where Id=@exChangeId ");
						this.database.AddInParameter(sqlStringCommand2, "exChangeId", System.Data.DbType.Int32, exchangeId);
						this.database.AddInParameter(sqlStringCommand2, "ProductId", System.Data.DbType.Int32, int.Parse(array[i]));
						this.database.AddInParameter(sqlStringCommand2, "status", System.Data.DbType.Int32, 0);
						this.database.AddInParameter(sqlStringCommand2, "ChangedNumber", System.Data.DbType.Int32, 0);
						this.database.AddInParameter(sqlStringCommand2, "ProductNumber", System.Data.DbType.Int32, int.Parse(array2[i]));
						this.database.AddInParameter(sqlStringCommand2, "PointNumber", System.Data.DbType.Int32, int.Parse(array3[i]));
						this.database.AddInParameter(sqlStringCommand2, "EachMaxNumber", System.Data.DbType.Int32, int.Parse(array4[i]));
						num += this.database.ExecuteNonQuery(sqlStringCommand2);
					}
				}
				result = (num > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public int GetProductExchangedCount(int exchangeId, int productId)
		{
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(id) FROM Hishop_PointExchange_Changed WHERE exChangeId = @exChangeId and ProductId=@ProductId");
				this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
				result = (int)this.database.ExecuteScalar(sqlStringCommand);
			}
			catch (Exception var_1_5B)
			{
				result = 0;
			}
			return result;
		}

		public int GetUserProductExchangedCount(int exchangeId, int productId, int userId)
		{
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(id) FROM Hishop_PointExchange_Changed WHERE exChangeId = @exChangeId and ProductId=@ProductId and MemberID=@MemberID");
				this.database.AddInParameter(sqlStringCommand, "exChangeId", System.Data.DbType.Int32, exchangeId);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
				this.database.AddInParameter(sqlStringCommand, "MemberID", System.Data.DbType.Int32, userId);
				result = (int)this.database.ExecuteScalar(sqlStringCommand);
			}
			catch (Exception var_1_75)
			{
				result = 0;
			}
			return result;
		}
	}
}
