using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Messages;
using Hidistro.SqlDal.Promotions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ControlPanel.Promotions
{
	public static class GameHelper
	{
		private static Random Rnd = new Random();

		public static bool Create(GameInfo model, out int gameId)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model参数不能为null");
			}
			Globals.EntityCoding(model, true);
			bool flag = new GameDao().Create(model, out gameId);
			if (flag)
			{
			}
			return flag;
		}

		public static bool Update(GameInfo model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model参数不能为null");
			}
			Globals.EntityCoding(model, true);
			bool flag = new GameDao().Update(model, true);
			if (flag)
			{
			}
			return flag;
		}

		public static bool Delete(params int[] gameIds)
		{
			if (gameIds == null || gameIds.Count<int>() <= 0)
			{
				throw new ArgumentNullException("参数gameIds不能为空！");
			}
			return new GameDao().Delete(gameIds);
		}

		public static bool UpdateStatus(int gameId, GameStatus status)
		{
			bool flag = new GameDao().UpdateStatus(gameId, status);
			if (flag)
			{
			}
			return flag;
		}

		public static bool UpdateOutOfDateStatus()
		{
			return new GameDao().UpdateOutOfDateStatus();
		}

		public static GameInfo GetModelByGameId(int gameId)
		{
			GameInfo modelByGameId = new GameDao().GetModelByGameId(gameId);
			Globals.EntityCoding(modelByGameId, false);
			return modelByGameId;
		}

		public static GameInfo GetModelByGameId(string keyWord)
		{
			GameInfo modelByGameId = new GameDao().GetModelByGameId(keyWord);
			Globals.EntityCoding(modelByGameId, false);
			return modelByGameId;
		}

		public static IEnumerable<GameInfo> GetLists(GameSearch search)
		{
			return new GameDao().GetLists(search);
		}

		public static GameInfo GetGameInfoById(int gameId)
		{
			return new GameDao().GetGameInfoById(gameId);
		}

		public static DbQueryResult GetGameList(GameSearch search)
		{
			return new GameDao().GetGameList(search);
		}

		public static DbQueryResult GetGameListByView(GameSearch search)
		{
			return new GameDao().GetGameListByView(search);
		}

		public static bool CreatePrize(GamePrizeInfo model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model参数不能为null");
			}
			bool flag = new GamePrizeDao().Create(model);
			if (flag)
			{
			}
			return flag;
		}

		public static GamePrizeInfo GetGamePrizeInfoById(int prizeId)
		{
			if (prizeId < 0)
			{
				throw new ArgumentNullException("参数错误");
			}
			return new GamePrizeDao().GetGamePrizeInfoById(prizeId);
		}

		public static bool CreateWinningPool(GameWinningPoolInfo model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model参数不能为null");
			}
			bool flag = new GamePrizeDao().CreateWinningPool(model);
			if (flag)
			{
			}
			return flag;
		}

		private static List<int> GetPool(List<GamePrizeInfo> list)
		{
			List<int> list2 = new List<int>();
			if (list != null && list.Count > 0)
			{
				foreach (GamePrizeInfo current in list)
				{
					if (current != null && current.PrizeCount > 0)
					{
						for (int i = 0; i < current.PrizeCount; i++)
						{
							list2.Add(current.PrizeId);
						}
					}
				}
			}
			return list2;
		}

		public static bool DeleteWinningPools(int gameId)
		{
			return new GamePrizeDao().DeleteWinningPools(gameId);
		}

		public static void CreateWinningPools(float PrizeRate, int prizeCount, int gameId)
		{
			IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(gameId);
			List<int> pool = GameHelper.GetPool(gamePrizeListsByGameId.ToList<GamePrizeInfo>());
			int num = (int)((float)prizeCount / (PrizeRate / 100f));
			float num2 = (float)prizeCount % (PrizeRate / 100f);
			for (int i = 1; i < num + 1; i++)
			{
				GameHelper.CreateWinningPool(new GameWinningPoolInfo
				{
					GameId = gameId,
					Number = i,
					GamePrizeId = (i < prizeCount + 1) ? pool[i - 1] : 0,
					IsReceive = 0
				});
			}
			if (num2 > 0f)
			{
				GameHelper.CreateWinningPool(new GameWinningPoolInfo
				{
					GameId = gameId,
					Number = num + 1,
					GamePrizeId = 0,
					IsReceive = 0
				});
			}
		}

		public static List<GameWinningPool> GetWinningPoolList(int gameId)
		{
			return new GameDao().GetWinningPoolList(gameId);
		}

		public static bool UpdatePrize(GamePrizeInfo model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model参数不能为null");
			}
			bool flag = new GamePrizeDao().Update(model);
			if (flag)
			{
			}
			return flag;
		}

		public static bool DeletePromotionGamePrize(GamePrizeInfo model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model参数不能为null");
			}
			bool flag = new GamePrizeDao().Delete(model);
			if (flag)
			{
			}
			return flag;
		}

		public static GamePrizeInfo GetModelByPrizeGradeAndGameId(PrizeGrade grade, int gameId)
		{
			return new GamePrizeDao().GetModelByPrizeGradeAndGameId(grade, gameId);
		}

		public static IList<GamePrizeInfo> GetGamePrizeListsByGameId(int gameId)
		{
			IList<GamePrizeInfo> result;
			if (gameId <= 0)
			{
				result = null;
			}
			else
			{
				result = new GamePrizeDao().GetGamePrizeListsByGameId(gameId);
			}
			return result;
		}

		public static bool IsCanPrize(int gameId, int userid)
		{
			return new PrizeResultDao().IsCanPrize(gameId, userid);
		}

		public static bool AddPrizeLog(PrizeResultInfo model)
		{
			if (model == null)
			{
				throw new ArgumentNullException("参数model不能不null");
			}
			return new PrizeResultDao().AddPrizeLog(model);
		}

		public static bool UpdatePrizeLogStatus(int logId)
		{
			return new PrizeResultDao().UpdatePrizeLogStatus(logId);
		}

		public static IList<PrizeResultViewInfo> GetPrizeLogLists(int gameId, int pageIndex, int pageSize)
		{
			return new PrizeResultDao().GetPrizeLogLists(gameId, pageIndex, pageSize);
		}

		public static int GetOppNumberByToday(int userId, int gameId)
		{
			return new GamePrizeDao().GetOppNumberByToday(userId, gameId);
		}

		public static int GetOppNumber(int userId, int gameId)
		{
			return new GamePrizeDao().GetOppNumber(userId, gameId);
		}

		public static DbQueryResult GetPrizeLogLists(PrizesDeliveQuery query)
		{
			return new PrizeResultDao().GetPrizeLogLists(query);
		}

		public static string GetPrizeFullName(PrizeResultViewInfo item)
		{
			string result;
			switch (item.PrizeType)
			{
			case PrizeType.赠送积分:
				result = string.Format("{0} 积分", item.GivePoint);
				break;
			case PrizeType.赠送优惠券:
			{
				CouponInfo coupon = CouponHelper.GetCoupon(int.Parse(item.GiveCouponId));
				if (coupon != null)
				{
					result = coupon.CouponName;
				}
				else
				{
					result = "优惠券" + item.GiveCouponId + "[已删除]";
				}
				break;
			}
			case PrizeType.赠送商品:
			{
				ProductInfo productBaseInfo = ProductHelper.GetProductBaseInfo(int.Parse(item.GiveShopBookId));
				if (productBaseInfo != null)
				{
					result = productBaseInfo.ProductName;
				}
				else
				{
					result = "赠送商品[已删除]";
				}
				break;
			}
			default:
				result = "";
				break;
			}
			return result;
		}

		public static string GetPrizeName(PrizeType PrizeType, string FullName)
		{
			string result;
			switch (PrizeType)
			{
			case PrizeType.赠送优惠券:
			case PrizeType.赠送商品:
				result = Globals.SubStr(FullName, 12, "..");
				break;
			default:
				result = FullName;
				break;
			}
			return result;
		}

		public static GamePrizeInfo UserPrize(int gameId, int useId)
		{
			int[] expr_07 = new int[]
			{
				67,
				112,
				202,
				292,
				337
			};
			IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(gameId);
			int num = gamePrizeListsByGameId.Max((GamePrizeInfo p) => p.PrizeRate);
			GamePrizeInfo gamePrizeInfo = new GamePrizeInfo
			{
				PrizeId = 0,
				PrizeRate = (num >= 100) ? 0 : 100,
				PrizeGrade = PrizeGrade.未中奖
			};
			gamePrizeListsByGameId.Add(gamePrizeInfo);
			GamePrizeInfo gamePrizeInfo2 = GameHelper.ChouJiang(gamePrizeListsByGameId);
			if (gamePrizeInfo2.PrizeId != 0 && gamePrizeInfo2.PrizeCount <= 0)
			{
				gamePrizeInfo2 = gamePrizeInfo;
			}
			if (gamePrizeInfo2.PrizeId != 0 && gamePrizeInfo2.PrizeType == PrizeType.赠送优惠券)
			{
				SendCouponResult sendCouponResult = CouponHelper.IsCanSendCouponToMember(int.Parse(gamePrizeInfo2.GiveCouponId), useId);
				if (sendCouponResult != SendCouponResult.正常领取)
				{
					gamePrizeInfo2 = gamePrizeInfo;
				}
			}
			new PrizeResultDao().AddPrizeLog(new PrizeResultInfo
			{
				GameId = gameId,
				PrizeId = gamePrizeInfo2.PrizeId,
				UserId = useId
			});
			return gamePrizeInfo2;
		}

		private static GamePrizeInfo ChouJiang(IList<GamePrizeInfo> prizeLists)
		{
			return (from x in Enumerable.Range(0, 10000)
			let prizeInfo0 = prizeLists[GameHelper.Rnd.Next(prizeLists.Count<GamePrizeInfo>())]
			let rnd = GameHelper.Rnd.Next(0, 100)
			where rnd < prizeInfo0.PrizeRate
			select prizeInfo0).First<GamePrizeInfo>();
		}

		public static DbQueryResult GetPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
		{
			return new GameDao().GetPrizesDeliveryList(query, ExtendLimits, selectFields);
		}

		public static DbQueryResult GetAllPrizesDeliveryList(PrizesDeliveQuery query, string ExtendLimits = "", string selectFields = "*")
		{
			return new GameDao().GetAllPrizesDeliveryList(query, ExtendLimits, selectFields);
		}

		public static bool UpdatePrizesDelivery(PrizesDeliveQuery query)
		{
			bool flag = new GameDao().UpdatePrizesDelivery(query);
			if (flag)
			{
				string gameTitle = "";
				int userId = 0;
				new GameDao().GetPrizesDeliveInfo(query, out gameTitle, out userId);
				try
				{
					MemberInfo member = MemberHelper.GetMember(userId);
					if (member != null)
					{
						Messenger.SendWeiXinMsg_PrizeRelease(member, gameTitle);
					}
				}
				catch (Exception var_4_4F)
				{
				}
			}
			return flag;
		}

		public static bool UpdateOneyuanDelivery(PrizesDeliveQuery query)
		{
			return new GameDao().UpdatePrizesDelivery(query);
		}

		public static bool DeletePrizesDelivery(int[] ids)
		{
			return new GameDao().DeletePrizesDelivery(ids);
		}

		public static System.Data.DataTable GetPrizesDeliveryNum()
		{
			return new GameDao().GetPrizesDeliveryNum();
		}

		public static string GetPrizesDeliveStatus(string status, string isLogistics, string type, string gametype)
		{
			string result = "未定义";
			if (status != null)
			{
				if (!(status == "0"))
				{
					if (!(status == "1"))
					{
						if (!(status == "2"))
						{
							if (!(status == "3"))
							{
								if (status == "4")
								{
									result = "已收货";
								}
							}
							else
							{
								result = "已收货";
							}
						}
						else
						{
							result = "已发货";
						}
					}
					else
					{
						result = "待发货";
					}
				}
				else if (type == "2" || gametype == "5")
				{
					result = "待填写收货地址";
				}
				else if (isLogistics == "0")
				{
					result = "已收货";
				}
				else
				{
					result = "待填写收货地址";
				}
			}
			return result;
		}

		public static string GetPrizeGradeName(string PrizeGrade)
		{
			return Enum.GetName(typeof(PrizeGrade), int.Parse(PrizeGrade));
		}

		public static string GetGameTypeName(string GameType)
		{
			return Enum.GetName(typeof(GameType), int.Parse(GameType));
		}

		public static bool UpdateWinningPoolIsReceive(int winningPoolId)
		{
			return new GameDao().UpdateWinningPoolIsReceive(winningPoolId);
		}
	}
}
