using ControlPanel.Promotions;
using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.SaleSystem.Vshop;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Hidistro.UI.Web.API
{
	public class Hi_Ajax_Game : System.Web.IHttpHandler
	{
		public class GameData
		{
			public int status
			{
				get;
				set;
			}

			public System.DateTime BeginDate
			{
				get;
				set;
			}

			public System.DateTime EndDate
			{
				get;
				set;
			}

			public string Description
			{
				get;
				set;
			}

			public int LimitEveryDay
			{
				get;
				set;
			}

			public int MaximumDailyLimit
			{
				get;
				set;
			}

			public int MemberCheck
			{
				get;
				set;
			}

			public int HasPhone
			{
				get;
				set;
			}

			public System.Collections.Generic.IList<Hi_Ajax_Game.PrizeData> prizeLists
			{
				get;
				set;
			}
		}

		public class PrizeData
		{
			public string prizeType
			{
				get;
				set;
			}

			public string prizeName
			{
				get;
				set;
			}

			public string prize
			{
				get;
				set;
			}

			public string PrizeFullName
			{
				get;
				set;
			}

			public int prizeCount
			{
				get;
				set;
			}

			public int prizeId
			{
				get;
				set;
			}
		}

		private static object objLock = new object();

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = context.Request["action"];
			if (!string.IsNullOrEmpty(text))
			{
				text = text.ToLower();
			}
			string key;
			switch (key = text)
			{
			case "getprizelists":
				this.GetPrizeLists(context);
				return;
			case "checkusercanplay":
				this.CheckUserCanPlay(context);
				return;
			case "getprizeinfo":
				this.GetPrize(context);
				return;
			case "getuserprizelists":
				this.GetUserPrizeLists(context);
				return;
			case "checkcanvote":
				this.CheckCanVote(context);
				return;
			case "uservote":
				this.UserVote(context);
				return;
			case "getcoupon":
				this.GetCouponToMember(context);
				return;
			case "getopportunity":
				this.GetOpportunity(context);
				return;
			case "updatecellphone":
				this.UpdateCellPhone(context);
				break;

				return;
			}
		}

		private void UpdateCellPhone(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			string text = context.Request["CellPhone"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"fail\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			currentMember.CellPhone = text;
			if (MemberProcessor.UpdateMember(currentMember))
			{
				stringBuilder.Append("\"status\":\"ok\",\"message\":\"修改成功\"}");
			}
			context.Response.Write(stringBuilder.ToString());
		}

		private void GetOpportunity(System.Web.HttpContext context)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			string text = context.Request["gameid"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			GameInfo modelByGameId = GameHelper.GetModelByGameId(text);
			if (System.DateTime.Now < modelByGameId.BeginTime)
			{
				stringBuilder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			if (System.DateTime.Now > modelByGameId.EndTime || modelByGameId.Status == GameStatus.结束)
			{
				stringBuilder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			try
			{
				if (!MemberProcessor.CheckCurrentMemberIsInRange(modelByGameId.ApplyMembers, modelByGameId.DefualtGroup, modelByGameId.CustomGroup))
				{
					stringBuilder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				GameHelper.IsCanPrize(modelByGameId.GameId, currentMember.UserId);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"ok\",\"opportunitynumber\":\"0\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			int num = Globals.RequestFormNum("LimitEveryDay");
			int num2 = Globals.RequestFormNum("MaximumDailyLimit");
			if (num == 0 && num2 == 0)
			{
				stringBuilder.Append("\"status\":\"ok\",\"opportunitynumber\":\"-1\"}");
			}
			else
			{
				int oppNumberByToday = GameHelper.GetOppNumberByToday(currentMember.UserId, modelByGameId.GameId);
				int oppNumber = GameHelper.GetOppNumber(currentMember.UserId, modelByGameId.GameId);
				int num3 = num - oppNumberByToday;
				int num4 = num2 - oppNumber;
				int num5;
				if (num2 == 0)
				{
					num5 = num3;
				}
				else
				{
					num5 = ((num3 >= num4) ? num4 : num3);
				}
				if (num == 0)
				{
					num5 = num4;
				}
				if (num5 < 0)
				{
					num5 = 0;
				}
				stringBuilder.Append("\"status\":\"ok\",\"opportunitynumber\":\"" + num5 + "\"}");
			}
			context.Response.Write(stringBuilder.ToString());
		}

		private void GetPrizeLists(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			string text = context.Request["gameid"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			GameInfo modelByGameId = GameHelper.GetModelByGameId(text);
			if (modelByGameId == null)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"游戏不存在!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			Hi_Ajax_Game.GameData gameData = new Hi_Ajax_Game.GameData
			{
				status = 1,
				Description = modelByGameId.Description,
				BeginDate = modelByGameId.BeginTime,
				EndDate = modelByGameId.EndTime,
				LimitEveryDay = modelByGameId.LimitEveryDay,
				MaximumDailyLimit = modelByGameId.MaximumDailyLimit,
				MemberCheck = modelByGameId.MemberCheck,
				HasPhone = string.IsNullOrEmpty(currentMember.CellPhone) ? 0 : 1
			};
			System.Collections.Generic.IList<GamePrizeInfo> gamePrizeListsByGameId = GameHelper.GetGamePrizeListsByGameId(modelByGameId.GameId);
			System.Collections.Generic.List<Hi_Ajax_Game.PrizeData> list = new System.Collections.Generic.List<Hi_Ajax_Game.PrizeData>();
			GamePrizeInfo gamePrizeInfo = new GamePrizeInfo();
			GamePrizeInfo gamePrizeInfo2 = new GamePrizeInfo();
			GamePrizeInfo gamePrizeInfo3 = new GamePrizeInfo();
			GamePrizeInfo gamePrizeInfo4 = new GamePrizeInfo();
			switch (gamePrizeListsByGameId.Count<GamePrizeInfo>())
			{
			case 1:
			{
				gamePrizeInfo = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.一等奖);
				string prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo.PrizeType,
					GivePoint = gamePrizeInfo.GivePoint,
					GiveCouponId = gamePrizeInfo.GiveCouponId,
					GiveShopBookId = gamePrizeInfo.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo.PrizeId,
					prize = gamePrizeInfo.Prize,
					prizeType = gamePrizeInfo.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				break;
			}
			case 2:
			{
				gamePrizeInfo = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.一等奖);
				string prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo.PrizeType,
					GivePoint = gamePrizeInfo.GivePoint,
					GiveCouponId = gamePrizeInfo.GiveCouponId,
					GiveShopBookId = gamePrizeInfo.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo.PrizeId,
					prize = gamePrizeInfo.Prize,
					prizeType = gamePrizeInfo.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				gamePrizeInfo2 = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.二等奖);
				prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo2.PrizeType,
					GivePoint = gamePrizeInfo2.GivePoint,
					GiveCouponId = gamePrizeInfo2.GiveCouponId,
					GiveShopBookId = gamePrizeInfo2.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo2.PrizeId,
					prize = gamePrizeInfo2.Prize,
					prizeType = gamePrizeInfo2.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo2.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo2.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				break;
			}
			case 3:
			{
				gamePrizeInfo = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.一等奖);
				string prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo.PrizeType,
					GivePoint = gamePrizeInfo.GivePoint,
					GiveCouponId = gamePrizeInfo.GiveCouponId,
					GiveShopBookId = gamePrizeInfo.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo.PrizeId,
					prize = gamePrizeInfo.Prize,
					prizeType = gamePrizeInfo.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				gamePrizeInfo2 = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.二等奖);
				prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo2.PrizeType,
					GivePoint = gamePrizeInfo2.GivePoint,
					GiveCouponId = gamePrizeInfo2.GiveCouponId,
					GiveShopBookId = gamePrizeInfo2.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo2.PrizeId,
					prize = gamePrizeInfo2.Prize,
					prizeType = gamePrizeInfo2.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo2.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo2.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				gamePrizeInfo3 = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.三等奖);
				prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo3.PrizeType,
					GivePoint = gamePrizeInfo3.GivePoint,
					GiveCouponId = gamePrizeInfo3.GiveCouponId,
					GiveShopBookId = gamePrizeInfo3.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo3.PrizeId,
					prize = gamePrizeInfo3.Prize,
					prizeType = gamePrizeInfo3.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo3.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo3.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				break;
			}
			case 4:
			{
				gamePrizeInfo = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.一等奖);
				string prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo.PrizeType,
					GivePoint = gamePrizeInfo.GivePoint,
					GiveCouponId = gamePrizeInfo.GiveCouponId,
					GiveShopBookId = gamePrizeInfo.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo.PrizeId,
					prize = gamePrizeInfo.Prize,
					prizeType = gamePrizeInfo.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				gamePrizeInfo2 = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.二等奖);
				prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo2.PrizeType,
					GivePoint = gamePrizeInfo2.GivePoint,
					GiveCouponId = gamePrizeInfo2.GiveCouponId,
					GiveShopBookId = gamePrizeInfo2.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo2.PrizeId,
					prize = gamePrizeInfo2.Prize,
					prizeType = gamePrizeInfo2.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo2.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo2.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				gamePrizeInfo3 = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.三等奖);
				prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo3.PrizeType,
					GivePoint = gamePrizeInfo3.GivePoint,
					GiveCouponId = gamePrizeInfo3.GiveCouponId,
					GiveShopBookId = gamePrizeInfo3.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo3.PrizeId,
					prize = gamePrizeInfo3.Prize,
					prizeType = gamePrizeInfo3.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo3.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo3.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				gamePrizeInfo4 = gamePrizeListsByGameId.FirstOrDefault((GamePrizeInfo p) => p.PrizeGrade == PrizeGrade.四等奖);
				prizeFullName = GameHelper.GetPrizeFullName(new PrizeResultViewInfo
				{
					PrizeType = gamePrizeInfo4.PrizeType,
					GivePoint = gamePrizeInfo4.GivePoint,
					GiveCouponId = gamePrizeInfo4.GiveCouponId,
					GiveShopBookId = gamePrizeInfo4.GiveShopBookId
				});
				list.Add(new Hi_Ajax_Game.PrizeData
				{
					prizeId = gamePrizeInfo4.PrizeId,
					prize = gamePrizeInfo4.Prize,
					prizeType = gamePrizeInfo4.PrizeGrade.ToString(),
					prizeCount = gamePrizeInfo4.PrizeRate,
					prizeName = GameHelper.GetPrizeName(gamePrizeInfo4.PrizeType, prizeFullName),
					PrizeFullName = prizeFullName
				});
				break;
			}
			}
			gameData.prizeLists = list;
			IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter();
			isoDateTimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
			context.Response.Write(JsonConvert.SerializeObject(gameData, Formatting.Indented, new JsonConverter[]
			{
				isoDateTimeConverter
			}));
		}

		private void CheckUserCanPlay(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			string text = context.Request["gameid"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			GameInfo modelByGameId = GameHelper.GetModelByGameId(text);
			if (modelByGameId == null)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"游戏不存在!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			int userid = 0;
			try
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				userid = currentMember.UserId;
			}
			catch (System.Exception)
			{
				userid = 1;
			}
			try
			{
				if (MemberProcessor.CheckCurrentMemberIsInRange(modelByGameId.ApplyMembers, modelByGameId.DefualtGroup, modelByGameId.CustomGroup))
				{
					if (!GameHelper.IsCanPrize(modelByGameId.GameId, userid))
					{
						throw new System.Exception("不能再玩！");
					}
					stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"可以正常玩!\"}");
					context.Response.Write(stringBuilder.ToString());
				}
				else
				{
					stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"当前会员不在活动的适用会员范围内\"}");
					context.Response.Write(stringBuilder.ToString());
				}
			}
			catch (System.Exception ex)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"" + ex.Message + "!\"}");
				context.Response.Write(stringBuilder.ToString());
			}
		}

		private void GetPrize(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			string text = context.Request["gameid"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			GameInfo modelByGameId = GameHelper.GetModelByGameId(text);
			if (modelByGameId == null)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			if (System.DateTime.Now < modelByGameId.BeginTime)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"活动还没开始!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			if (System.DateTime.Now > modelByGameId.EndTime || modelByGameId.Status == GameStatus.结束)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"活动已结束!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			int num = 0;
			try
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				num = currentMember.UserId;
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"请先登录!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			try
			{
				if (!MemberProcessor.CheckCurrentMemberIsInRange(modelByGameId.ApplyMembers, modelByGameId.DefualtGroup, modelByGameId.CustomGroup))
				{
					stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"当前会员不在活动的适用会员范围内\"}");
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				GameHelper.IsCanPrize(modelByGameId.GameId, num);
			}
			catch (System.Exception ex)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"" + ex.Message + "!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			lock (Hi_Ajax_Game.objLock)
			{
				try
				{
					System.Collections.Generic.List<GameWinningPool> winningPoolList = GameHelper.GetWinningPoolList(Globals.ToNum(modelByGameId.GameId));
					int num2 = winningPoolList.Count<GameWinningPool>();
					string text2 = "";
					if (num2 > 0)
					{
						System.Random random = new System.Random();
						int index = random.Next(0, num2);
						GameWinningPool gameWinningPool = winningPoolList[index];
						if (gameWinningPool != null)
						{
							int prizeId = 0;
							if (gameWinningPool.GamePrizeId > 0)
							{
								GamePrizeInfo gamePrizeInfoById = GameHelper.GetGamePrizeInfoById(gameWinningPool.GamePrizeId);
								if (gamePrizeInfoById != null)
								{
									prizeId = gamePrizeInfoById.PrizeId;
									text2 = gamePrizeInfoById.PrizeName;
								}
								stringBuilder.Append(string.Concat(new object[]
								{
									"\"status\":\"1\",\"Desciption\":\"\",\"prizeName\":\"",
									text2,
									"\",\"prizeState\":\"ok\",\"prizeId\":\"",
									gameWinningPool.GamePrizeId,
									"\",\"prize\":\"",
									gamePrizeInfoById.Prize,
									"\",\"prizeGrade\":\"",
									this.GetPrizeName(gamePrizeInfoById.PrizeGrade),
									"\"}"
								}));
							}
							else
							{
								text2 = modelByGameId.NotPrzeDescription;
								stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"\",\"prizeName\":\"" + text2 + "\",\"prizeState\":\"fail\",\"prizeId\":\"0\",\"prizeGrade\":\"0\"}");
							}
							GameHelper.AddPrizeLog(new PrizeResultInfo
							{
								GameId = modelByGameId.GameId,
								PrizeId = prizeId,
								UserId = num
							});
							GameHelper.UpdateWinningPoolIsReceive(gameWinningPool.WinningPoolId);
						}
						context.Response.Write(stringBuilder.ToString());
					}
					else
					{
						text2 = (string.IsNullOrEmpty(modelByGameId.NotPrzeDescription) ? "谢谢参与！" : modelByGameId.NotPrzeDescription);
						stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"\",\"prizeName\":\"" + text2 + "\",\"prizeState\":\"fail\",\"prizeId\":\"0\",\"prizeGrade\":\"0\"}");
						GameHelper.AddPrizeLog(new PrizeResultInfo
						{
							GameId = modelByGameId.GameId,
							PrizeId = 0,
							UserId = num
						});
						context.Response.Write(stringBuilder.ToString());
					}
				}
				catch (System.Exception ex2)
				{
					Globals.Debuglog(ex2.ToString(), "_GameDebuglog.txt");
				}
			}
		}

		public string GetPrizeName(PrizeGrade prizeGrade)
		{
			if (prizeGrade == PrizeGrade.未中奖)
			{
				return "谢谢参与";
			}
			return prizeGrade.ToString();
		}

		private int GetPrizeIndex(PrizeGrade prizeGrade)
		{
			if (prizeGrade == PrizeGrade.未中奖)
			{
				return (int)prizeGrade;
			}
			System.Random random = new System.Random();
			if (random.Next(1, 10) % 2 == 0)
			{
				return (int)(prizeGrade + 5);
			}
			return (int)prizeGrade;
		}

		private void GetUserPrizeLists(System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			string text = context.Request["gameid"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			int pageIndex = 1;
			int pageSize = 7;
			try
			{
				pageIndex = int.Parse(context.Request["pageIndex"]);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			try
			{
				pageSize = int.Parse(context.Request["pageSize"]);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			System.Collections.Generic.IList<PrizeResultViewInfo> prizeLogLists = GameHelper.GetPrizeLogLists(GameHelper.GetModelByGameId(text).GameId, pageIndex, pageSize);
			stringBuilder.Append("\"lists\":[");
			int count = prizeLogLists.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					PrizeResultViewInfo prizeResultViewInfo = prizeLogLists[i];
					stringBuilder.Append(string.Concat(new string[]
					{
						"{\"PrizeGrade\":\"",
						prizeResultViewInfo.PrizeGrade.ToString(),
						"\",\"UserName\":\"",
						this.GetUserName(prizeResultViewInfo.UserName),
						"\",\"PrizeName\":\"",
						GameHelper.GetPrizeName(prizeResultViewInfo.PrizeType, GameHelper.GetPrizeFullName(prizeResultViewInfo)),
						"\",\"Prize\":\"",
						prizeResultViewInfo.PrizeName,
						"\",\"DateTime\":\"",
						prizeResultViewInfo.PlayTime.ToString("yyyy-MM-dd"),
						"\"}"
					}));
					if (i != count - 1)
					{
						stringBuilder.Append(",");
					}
				}
			}
			stringBuilder.Append("]}");
			context.Response.Write(stringBuilder);
		}

		private string GetUserName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return "";
			}
			int length = name.Length;
			string text = name.Substring(0, 1) + "**";
			if (length <= 3)
			{
				return text;
			}
			return text + name.Substring(length - 1);
		}

		private void CheckCanVote(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			int num = 0;
			try
			{
				num = int.Parse(context.Request["voteId"]);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			VoteInfo vote = VoteHelper.GetVote((long)num);
			if (vote == null)
			{
				stringBuilder.Append("\"status\":\"2\",\"Desciption\":\"不存在该投票!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			if (!MemberProcessor.CheckCurrentMemberIsInRange(vote.MemberGrades, vote.DefualtGroup, vote.CustomGroup))
			{
				stringBuilder.Append("\"status\":\"2\",\"Desciption\":\"该投票不适应您的会员，谢谢!\"}");
				context.Response.Write(stringBuilder.ToString());
			}
			else
			{
				if (!VoteHelper.IsVote(num))
				{
					stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"可以投票!\"}");
					context.Response.Write(stringBuilder.ToString());
					return;
				}
				stringBuilder.Append("\"status\":\"2\",\"Desciption\":\"已投过票!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
		}

		private void UserVote(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			int voteId = 0;
			try
			{
				voteId = int.Parse(context.Request["voteId"]);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			string text = context.Request["voteItem"];
			if (string.IsNullOrEmpty(text))
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
			}
			else
			{
				try
				{
					bool flag = VoteHelper.Vote(voteId, text);
					if (!flag)
					{
						throw new System.Exception("投票失败！");
					}
					stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"成功!\"}");
					context.Response.Write(stringBuilder.ToString());
				}
				catch (System.Exception ex)
				{
					stringBuilder.Append("\"status\":\"2\",\"Desciption\":\"" + ex.Message + "\"}");
					context.Response.Write(stringBuilder.ToString());
				}
			}
		}

		private void GetCouponToMember(System.Web.HttpContext context)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("{");
			int couponId = 0;
			try
			{
				couponId = int.Parse(context.Request["couponId"]);
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"0\",\"Desciption\":\"参数错误!\"}");
				context.Response.Write(stringBuilder.ToString());
				return;
			}
			int userId = 0;
			try
			{
				MemberInfo currentMember = MemberProcessor.GetCurrentMember();
				userId = currentMember.UserId;
			}
			catch (System.Exception)
			{
				userId = 1;
			}
			try
			{
				SendCouponResult sendCouponResult = CouponHelper.SendCouponToMember(couponId, userId);
				if (sendCouponResult == SendCouponResult.正常领取)
				{
					stringBuilder.Append("\"status\":\"1\",\"Desciption\":\"领取成功!\"}");
					context.Response.Write(stringBuilder.ToString());
				}
				else
				{
					if (sendCouponResult == SendCouponResult.其它错误)
					{
						throw new System.Exception();
					}
					stringBuilder.Append("\"status\":\"2\",\"Desciption\":\"" + sendCouponResult.ToString() + "!\"}");
					context.Response.Write(stringBuilder.ToString());
				}
			}
			catch (System.Exception)
			{
				stringBuilder.Append("\"status\":\"3\",\"Desciption\":\"领取失败!\"}");
				context.Response.Write(stringBuilder.ToString());
			}
		}
	}
}
