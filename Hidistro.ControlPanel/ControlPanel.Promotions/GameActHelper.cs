using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;
using System.Collections.Generic;
using System.Data;

namespace ControlPanel.Promotions
{
	public class GameActHelper
	{
		private static GameActDao _game = new GameActDao();

		public static System.Data.DataTable GetPrizes(int gameId)
		{
			return GameActHelper._game.GetPrizes(gameId);
		}

		public static IList<GameActPrizeInfo> GetPrizesModel(int gameId)
		{
			return GameActHelper._game.GetPrizesModel(gameId);
		}

		public static bool DeletePrize(int gameId, int prizeId)
		{
			return GameActHelper._game.DeletePrize(gameId, prizeId);
		}

		public static bool DeletePrize(int gameId)
		{
			return GameActHelper._game.DeletePrize(gameId);
		}

		public static GameActPrizeInfo GetPrize(int gameId, int prizeId)
		{
			return GameActHelper._game.GetPrize(gameId, prizeId);
		}

		public static int InsertPrize(GameActPrizeInfo prize)
		{
			return GameActHelper._game.InsertPrize(prize);
		}

		public static bool UpdatePrize(GameActPrizeInfo prize)
		{
			return GameActHelper._game.UpdatePrize(prize);
		}

		public static int Create(GameActInfo game, ref string msg)
		{
			return GameActHelper._game.Create(game, ref msg);
		}

		public static bool Update(GameActInfo game, ref string msg)
		{
			return GameActHelper._game.Update(game, ref msg);
		}

		public static GameActInfo Get(int Id)
		{
			return GameActHelper._game.GetGame(Id);
		}

		public static bool Delete(int Id)
		{
			return GameActHelper._game.Delete(Id);
		}
	}
}
