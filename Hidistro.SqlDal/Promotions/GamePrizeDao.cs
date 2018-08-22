using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace Hidistro.SqlDal.Promotions
{
	public class GamePrizeDao
	{
		private Database _db = null;

		public GamePrizeDao()
		{
			this._db = DatabaseFactory.CreateDatabase();
		}

		public bool Create(GamePrizeInfo model)
		{
			string query = "INSERT INTO [Hishop_PromotionGamePrizes]\r\n                    ([GameId]\r\n                    ,[PrizeGrade]\r\n                    ,[PrizeType]\r\n                    ,[GivePoint]\r\n                    ,[GiveCouponId]\r\n                    ,[GiveShopBookId]\r\n                    ,[PrizeCount]\r\n                    ,[PrizeRate]\r\n                    ,[GriveShopBookPicUrl]\r\n                    ,[PrizeName]\r\n                    ,[Prize]\r\n                    ,IsLogistics\r\n                    ,PrizeImage\r\n                   )\r\n                  VALUES\r\n                    ( @GameId\r\n                    ,@PrizeGrade\r\n                    ,@PrizeType\r\n                    ,@GivePoint\r\n                    ,@GiveCouponId\r\n                    ,@GiveShopBookId\r\n                    ,@PrizeCount\r\n                    ,@PrizeRate\r\n                    ,@GriveShopBookPicUrl\r\n                    ,@PrizeName\r\n                    ,@Prize\r\n                    ,@IsLogistics\r\n                    ,@PrizeImage\r\n                    );";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, model.GameId);
			this._db.AddInParameter(sqlStringCommand, "@PrizeGrade", System.Data.DbType.Int32, (int)model.PrizeGrade);
			this._db.AddInParameter(sqlStringCommand, "@PrizeType", System.Data.DbType.Int32, (int)model.PrizeType);
			this._db.AddInParameter(sqlStringCommand, "@GivePoint", System.Data.DbType.Decimal, model.GivePoint);
			this._db.AddInParameter(sqlStringCommand, "@GiveCouponId", System.Data.DbType.String, model.GiveCouponId);
			this._db.AddInParameter(sqlStringCommand, "@GiveShopBookId", System.Data.DbType.String, model.GiveShopBookId);
			this._db.AddInParameter(sqlStringCommand, "@PrizeCount", System.Data.DbType.Int32, model.PrizeCount);
			this._db.AddInParameter(sqlStringCommand, "@PrizeRate", System.Data.DbType.Int32, model.PrizeRate);
			this._db.AddInParameter(sqlStringCommand, "@GriveShopBookPicUrl", System.Data.DbType.String, model.GriveShopBookPicUrl);
			this._db.AddInParameter(sqlStringCommand, "@PrizeName", System.Data.DbType.String, model.PrizeName);
			this._db.AddInParameter(sqlStringCommand, "@Prize", System.Data.DbType.String, model.Prize);
			this._db.AddInParameter(sqlStringCommand, "@IsLogistics", System.Data.DbType.Int32, model.IsLogistics);
			this._db.AddInParameter(sqlStringCommand, "@PrizeImage", System.Data.DbType.String, string.IsNullOrEmpty(model.PrizeImage) ? "/utility/pics/lipin60.png" : model.PrizeImage);
			int num = this._db.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool DeleteWinningPools(int GameId)
		{
			string query = "delete [Hishop_PromotionWinningPool] \r\n                    Where GameId=@GameId";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, GameId);
			int num = this._db.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool CreateWinningPool(GameWinningPoolInfo model)
		{
			string query = "INSERT INTO [Hishop_PromotionWinningPool]\r\n                    ([GameId]\r\n                    ,[Number]\r\n                    ,[GamePrizeId]\r\n                    ,[IsReceive])\r\n                  VALUES\r\n                    (@GameId\r\n                    ,@Number\r\n                    ,@GamePrizeId\r\n                    ,@IsReceive);";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, model.GameId);
			this._db.AddInParameter(sqlStringCommand, "@Number", System.Data.DbType.Int32, model.Number);
			this._db.AddInParameter(sqlStringCommand, "@GamePrizeId", System.Data.DbType.Int32, model.GamePrizeId);
			this._db.AddInParameter(sqlStringCommand, "@IsReceive", System.Data.DbType.Decimal, model.IsReceive);
			int num = this._db.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool Update(GamePrizeInfo model)
		{
			string query = "Update [Hishop_PromotionGamePrizes] set [PrizeType]=@PrizeType\r\n                    ,[GivePoint]=@GivePoint\r\n                    ,[GiveCouponId]=@GiveCouponId\r\n                    ,[GiveShopBookId]=@GiveShopBookId\r\n                    ,[PrizeCount]=@PrizeCount\r\n                    ,[GriveShopBookPicUrl]=@GriveShopBookPicUrl\r\n                    ,[IsLogistics]=@IsLogistics\r\n                    ,[PrizeImage]=@PrizeImage\r\n                    Where PrizeId=@PrizeId ;";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@PrizeType", System.Data.DbType.Int32, (int)model.PrizeType);
			this._db.AddInParameter(sqlStringCommand, "@GivePoint", System.Data.DbType.Decimal, model.GivePoint);
			this._db.AddInParameter(sqlStringCommand, "@GiveCouponId", System.Data.DbType.String, model.GiveCouponId);
			this._db.AddInParameter(sqlStringCommand, "@GiveShopBookId", System.Data.DbType.String, model.GiveShopBookId);
			this._db.AddInParameter(sqlStringCommand, "@PrizeCount", System.Data.DbType.Int32, model.PrizeCount);
			this._db.AddInParameter(sqlStringCommand, "@GriveShopBookPicUrl", System.Data.DbType.String, model.GriveShopBookPicUrl);
			this._db.AddInParameter(sqlStringCommand, "@IsLogistics", System.Data.DbType.Int32, model.IsLogistics);
			this._db.AddInParameter(sqlStringCommand, "@PrizeImage", System.Data.DbType.String, string.IsNullOrEmpty(model.PrizeImage) ? "/utility/pics/lipin60.png" : model.PrizeImage);
			this._db.AddInParameter(sqlStringCommand, "@PrizeId", System.Data.DbType.Int32, model.PrizeId);
			int num = this._db.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public bool Delete(GamePrizeInfo model)
		{
			string query = "delete [Hishop_PromotionGamePrizes] \r\n                    Where GameId=@GameId and PrizeId=@PrizeId and GameId in(select GameId from Hishop_PromotionGame where getdate()< BeginTime);";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, model.GameId);
			this._db.AddInParameter(sqlStringCommand, "@PrizeId", System.Data.DbType.Int32, model.PrizeId);
			int num = this._db.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public GamePrizeInfo GetModelByPrizeGradeAndGameId(PrizeGrade grade, int gameId)
		{
			string query = "Select * From [Hishop_PromotionGamePrizes] where PrizeGrade=@PrizeGrade and GameId=@GameId ";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@PrizeGrade", System.Data.DbType.Int32, (int)grade);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			GamePrizeInfo result;
			using (System.Data.IDataReader dataReader = this._db.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GamePrizeInfo>(dataReader);
			}
			return result;
		}

		public IList<GamePrizeInfo> GetGamePrizeListsByGameId(int gameId)
		{
			string query = "Select * From [Hishop_PromotionGamePrizes] where GameId=@GameId ";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			IList<GamePrizeInfo> result;
			using (System.Data.IDataReader dataReader = this._db.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<GamePrizeInfo>(dataReader);
			}
			return result;
		}

		public int GetOppNumberByToday(int userId, int gameId)
		{
			string query = "select COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId And PlayTime>=CONVERT(varchar(100), GETDATE(), 23)";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@UserId", System.Data.DbType.Int32, userId);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			return (int)this._db.ExecuteScalar(sqlStringCommand);
		}

		public int GetOppNumber(int userId, int gameId)
		{
			string query = "select COUNT(*) from Hishop_PromotionGameResultMembersLog where UserId=@UserId AND GameId=@GameId ";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@UserId", System.Data.DbType.Int32, userId);
			this._db.AddInParameter(sqlStringCommand, "@GameId", System.Data.DbType.Int32, gameId);
			return (int)this._db.ExecuteScalar(sqlStringCommand);
		}

		public GamePrizeInfo GetGamePrizeInfoById(int id)
		{
			string query = "Select * From [Hishop_PromotionGamePrizes] where PrizeId=@PrizeId ";
			System.Data.Common.DbCommand sqlStringCommand = this._db.GetSqlStringCommand(query);
			this._db.AddInParameter(sqlStringCommand, "@PrizeId", System.Data.DbType.Int32, id);
			GamePrizeInfo result;
			using (System.Data.IDataReader dataReader = this._db.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<GamePrizeInfo>(dataReader);
			}
			return result;
		}
	}
}
