using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Promotions
{
	public class GameActDao
	{
		private Database database;

		public GameActDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public int Create(GameActInfo game, ref string msg)
		{
			msg = "未知错误";
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Game]([GameType],[GameName],[BeginDate],[EndDate],[Decription],[MemberGrades], [usePoint],[GivePoint],[bOnlyNotWinner],[attendTimes],[ImgUrl],[status],[unWinDecrip],[CreateStep]) VALUES (@GameType,@GameName,@BeginDate,@EndDate,@Decription,@MemberGrades,@usePoint,@GivePoint,@bOnlyNotWinner, @attendTimes,@ImgUrl,@status,@unWinDecrip,@CreateStep) SELECT CAST(scope_identity() AS int)");
				this.database.AddInParameter(sqlStringCommand, "GameType", System.Data.DbType.Int32, (int)game.GameType);
				this.database.AddInParameter(sqlStringCommand, "GameName", System.Data.DbType.String, game.GameName);
				this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, game.BeginDate);
				this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, game.EndDate);
				this.database.AddInParameter(sqlStringCommand, "Decription", System.Data.DbType.String, game.Decription);
				this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, game.MemberGrades);
				this.database.AddInParameter(sqlStringCommand, "usePoint", System.Data.DbType.Int32, game.usePoint);
				this.database.AddInParameter(sqlStringCommand, "GivePoint", System.Data.DbType.Int32, game.GivePoint);
				this.database.AddInParameter(sqlStringCommand, "bOnlyNotWinner", System.Data.DbType.Boolean, game.bOnlyNotWinner);
				this.database.AddInParameter(sqlStringCommand, "attendTimes", System.Data.DbType.Int32, game.attendTimes);
				this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, game.ImgUrl);
				this.database.AddInParameter(sqlStringCommand, "status", System.Data.DbType.Int32, game.status);
				this.database.AddInParameter(sqlStringCommand, "unWinDecrip", System.Data.DbType.String, game.unWinDecrip);
				this.database.AddInParameter(sqlStringCommand, "CreateStep", System.Data.DbType.Int32, game.CreateStep);
				int num = (int)this.database.ExecuteScalar(sqlStringCommand);
				msg = "";
				result = num;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = 0;
			}
			return result;
		}

		public bool Update(GameActInfo game, ref string msg)
		{
			msg = "未知错误";
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE[Hishop_Game]   SET [GameType] = @GameType , [GameName] = @GameName  , [BeginDate] = @BeginDate , [EndDate] = @EndDate , [Decription] = @Decription , [MemberGrades] = @MemberGrades , [usePoint] = @usePoint , [GivePoint] = @GivePoint , [bOnlyNotWinner] = @bOnlyNotWinner , [attendTimes] = @attendTimes , [ImgUrl] = @ImgUrl , [status] = @status , [unWinDecrip] = @unWinDecrip , [CreateStep] = @CreateStep  where GameId=@ID");
				this.database.AddInParameter(sqlStringCommand, "GameType", System.Data.DbType.Int32, (int)game.GameType);
				this.database.AddInParameter(sqlStringCommand, "GameName", System.Data.DbType.String, game.GameName);
				this.database.AddInParameter(sqlStringCommand, "BeginDate", System.Data.DbType.DateTime, game.BeginDate);
				this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, game.EndDate);
				this.database.AddInParameter(sqlStringCommand, "Decription", System.Data.DbType.String, game.Decription);
				this.database.AddInParameter(sqlStringCommand, "MemberGrades", System.Data.DbType.String, game.MemberGrades);
				this.database.AddInParameter(sqlStringCommand, "usePoint", System.Data.DbType.Int32, game.usePoint);
				this.database.AddInParameter(sqlStringCommand, "GivePoint", System.Data.DbType.Int32, game.GivePoint);
				this.database.AddInParameter(sqlStringCommand, "bOnlyNotWinner", System.Data.DbType.Boolean, game.bOnlyNotWinner);
				this.database.AddInParameter(sqlStringCommand, "attendTimes", System.Data.DbType.Int32, game.attendTimes);
				this.database.AddInParameter(sqlStringCommand, "ImgUrl", System.Data.DbType.String, game.ImgUrl);
				this.database.AddInParameter(sqlStringCommand, "status", System.Data.DbType.Int32, game.status);
				this.database.AddInParameter(sqlStringCommand, "unWinDecrip", System.Data.DbType.String, game.unWinDecrip);
				this.database.AddInParameter(sqlStringCommand, "CreateStep", System.Data.DbType.Int32, game.CreateStep);
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, game.GameId);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				result = true;
			}
			catch (Exception ex)
			{
				msg = ex.Message;
				result = false;
			}
			return result;
		}

		public GameActInfo GetGame(int Id)
		{
			GameActInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Game WHERE GameId = @ID");
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GameActInfo>(dataReader);
			}
			return result;
		}

		public bool Delete(int Id)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM  Hishop_Game WHERE GameId = @ID");
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Game_Prize WHERE GameId = @ID");
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int InsertPrize(GameActPrizeInfo prize)
		{
			int result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Game_Prize]([GameId],[PrizeName],[sort],[PrizeType],[GrivePoint],[PointNumber],[PointRate],[GiveCouponId],[CouponNumber],[CouponRate],[GiveProductId],[ProductNumber],[ProductRate])VALUES(@GameId,@PrizeName,@sort,@PrizeType,@GrivePoint,@PointNumber ,@PointRate,@GiveCouponId,@CouponNumber,@CouponRate,@GiveProductId,@ProductNumber,@ProductRate)  SELECT CAST(scope_identity() AS int)");
				this.database.AddInParameter(sqlStringCommand, "GameId", System.Data.DbType.Int32, prize.GameId);
				this.database.AddInParameter(sqlStringCommand, "PrizeName", System.Data.DbType.String, prize.PrizeName);
				this.database.AddInParameter(sqlStringCommand, "sort", System.Data.DbType.Int32, prize.sort);
				this.database.AddInParameter(sqlStringCommand, "PrizeType", System.Data.DbType.Int64, (long)prize.PrizeType);
				this.database.AddInParameter(sqlStringCommand, "GrivePoint", System.Data.DbType.Int32, prize.GrivePoint);
				this.database.AddInParameter(sqlStringCommand, "PointNumber", System.Data.DbType.Int32, prize.PointNumber);
				this.database.AddInParameter(sqlStringCommand, "PointRate", System.Data.DbType.Decimal, prize.PointRate);
				this.database.AddInParameter(sqlStringCommand, "GiveCouponId", System.Data.DbType.Int32, prize.GiveCouponId);
				this.database.AddInParameter(sqlStringCommand, "CouponNumber", System.Data.DbType.Int32, prize.CouponNumber);
				this.database.AddInParameter(sqlStringCommand, "CouponRate", System.Data.DbType.Decimal, prize.CouponRate);
				this.database.AddInParameter(sqlStringCommand, "GiveProductId", System.Data.DbType.Int32, prize.GiveProductId);
				this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, prize.ProductNumber);
				this.database.AddInParameter(sqlStringCommand, "ProductRate", System.Data.DbType.Decimal, prize.ProductRate);
				result = (int)this.database.ExecuteScalar(sqlStringCommand);
			}
			catch (Exception var_1_1B3)
			{
				result = 0;
			}
			return result;
		}

		public bool UpdatePrize(GameActPrizeInfo prize)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE [Hishop_Game_Prize]   SET [GameId] = @GameId , [PrizeName] = @PrizeName , [sort] = @sort , [PrizeType] = @PrizeType , [GrivePoint] = @GrivePoint , [PointNumber] = @PointNumber , [PointRate] = @PointRate , [GiveCouponId] = @GiveCouponId , [CouponNumber] = @CouponNumber , [CouponRate] = @CouponRate , [GiveProductId] = @GiveProductId , [ProductNumber] = @ProductNumber , [ProductRate] = @ProductRate  where [GameId]= @GameId and [Id] = @Id");
			this.database.AddInParameter(sqlStringCommand, "GameId", System.Data.DbType.Int32, prize.GameId);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, prize.Id);
			this.database.AddInParameter(sqlStringCommand, "PrizeName", System.Data.DbType.String, prize.PrizeName);
			this.database.AddInParameter(sqlStringCommand, "sort", System.Data.DbType.Int32, prize.sort);
			this.database.AddInParameter(sqlStringCommand, "PrizeType", System.Data.DbType.Int64, (long)prize.PrizeType);
			this.database.AddInParameter(sqlStringCommand, "GrivePoint", System.Data.DbType.Int32, prize.GrivePoint);
			this.database.AddInParameter(sqlStringCommand, "PointNumber", System.Data.DbType.Int32, prize.PointNumber);
			this.database.AddInParameter(sqlStringCommand, "PointRate", System.Data.DbType.Decimal, prize.PointRate);
			this.database.AddInParameter(sqlStringCommand, "GiveCouponId", System.Data.DbType.Int32, prize.GiveCouponId);
			this.database.AddInParameter(sqlStringCommand, "CouponNumber", System.Data.DbType.Int32, prize.CouponNumber);
			this.database.AddInParameter(sqlStringCommand, "CouponRate", System.Data.DbType.Decimal, prize.CouponRate);
			this.database.AddInParameter(sqlStringCommand, "GiveProductId", System.Data.DbType.Int32, prize.GiveProductId);
			this.database.AddInParameter(sqlStringCommand, "ProductNumber", System.Data.DbType.Int32, prize.ProductNumber);
			this.database.AddInParameter(sqlStringCommand, "ProductRate", System.Data.DbType.Decimal, prize.ProductRate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public System.Data.DataTable GetPrizes(int gameId)
		{
			System.Data.DataTable result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select * from Hishop_Game_Prize where [GameId]= @GameId order by sort ");
				this.database.AddInParameter(sqlStringCommand, "GameId", System.Data.DbType.Int32, gameId);
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

		public bool DeletePrize(int gameid, int prizeId)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_Game_Prize where [GameId]= @GameId and [ID] = @ID ");
				this.database.AddInParameter(sqlStringCommand, "GameId", System.Data.DbType.Int32, gameid);
				this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, prizeId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch (Exception var_1_59)
			{
				result = false;
			}
			return result;
		}

		public bool DeletePrize(int gameid)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from Hishop_Game_Prize where [GameId]= @GameId ");
				this.database.AddInParameter(sqlStringCommand, "GameId", System.Data.DbType.Int32, gameid);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch (Exception var_1_3F)
			{
				result = false;
			}
			return result;
		}

		public GameActPrizeInfo GetPrize(int gameId, int prizeId)
		{
			GameActPrizeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Game_Prize  where [GameID]= @GameID and [ID] = @ID");
			this.database.AddInParameter(sqlStringCommand, "GameID", System.Data.DbType.Int32, gameId);
			this.database.AddInParameter(sqlStringCommand, "ID", System.Data.DbType.Int32, prizeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GameActPrizeInfo>(dataReader);
			}
			return result;
		}

		public IList<GameActPrizeInfo> GetPrizesModel(int gameId)
		{
			IList<GameActPrizeInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Game_Prize  where [GameID] = @GameID order by sort");
			this.database.AddInParameter(sqlStringCommand, "GameID", System.Data.DbType.Int32, gameId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<GameActPrizeInfo>(dataReader);
			}
			return result;
		}
	}
}
