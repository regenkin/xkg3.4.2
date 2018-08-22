using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Hidistro.ControlPanel.Members
{
	public static class MemberHelper
	{
		public static System.Data.DataTable GetTop50NotTopRegionIdBind()
		{
			return new MemberDao().GetTop50NotTopRegionIdBind();
		}

		public static int SetOrderDate(int userID, int orderType)
		{
			return new MemberDao().SetOrderDate(userID, orderType);
		}

		public static int GetActiveDay()
		{
			return new MemberDao().GetActiveDay();
		}

		public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			return new MemberGradeDao().HasSamePointMemberGrade(memberGrade);
		}

		public static bool HasSameMemberGrade(MemberGradeInfo memberGrade)
		{
			return new MemberGradeDao().HasSameMemberGrade(memberGrade);
		}

		public static bool CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			bool result;
			if (null == memberGrade)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(memberGrade, true);
				bool flag = MemberHelper.IsCanSetThisGrade(memberGrade);
				if (!flag)
				{
					throw new Exception("交易次数的上下级别，与交易额的上下级别不是同一个！");
				}
				flag = new MemberGradeDao().CreateMemberGrade(memberGrade);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.AddMemberGrade, string.Format(CultureInfo.InvariantCulture, "添加了名为 “{0}” 的会员等级", new object[]
					{
						memberGrade.Name
					}));
				}
				result = flag;
			}
			return result;
		}

		private static bool IsCanSetThisGrade(MemberGradeInfo memberGrade)
		{
			List<MemberGradeInfo> list = MemberHelper.GetMemberGrades().ToList<MemberGradeInfo>();
			int num = 0;
			int num2 = 0;
			foreach (MemberGradeInfo current in list)
			{
				if (current.GradeId != memberGrade.GradeId)
				{
					if (current.TranVol.HasValue && memberGrade.TranVol.HasValue)
					{
						if (current.TranVol.Value > memberGrade.TranVol.Value)
						{
							num++;
						}
					}
					if (current.TranTimes.HasValue && memberGrade.TranTimes.HasValue)
					{
						if (current.TranTimes.Value > memberGrade.TranTimes.Value)
						{
							num2++;
						}
					}
				}
			}
			return num == num2;
		}

		public static bool UpdateMemberGrade(MemberGradeInfo memberGrade)
		{
			bool result;
			if (null == memberGrade)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(memberGrade, true);
				bool flag = MemberHelper.IsCanSetThisGrade(memberGrade);
				if (!flag)
				{
					throw new Exception("交易次数的上下级别，与交易额的上下级别不是同一个！");
				}
				flag = new MemberGradeDao().UpdateMemberGrade(memberGrade);
				if (flag)
				{
					EventLogs.WriteOperationLog(Privilege.EditMemberGrade, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员等级", new object[]
					{
						memberGrade.GradeId
					}));
				}
				result = flag;
			}
			return result;
		}

		public static void SetDefalutMemberGrade(int gradeId)
		{
			new MemberGradeDao().SetDefalutMemberGrade(gradeId);
		}

		public static int SelectUserCountGrades(int gid)
		{
			return new MemberGradeDao().SelectUserCountGrades(gid);
		}

		public static bool DeleteMemberGrade(int gradeId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMemberGrade);
			bool flag = new MemberGradeDao().DeleteMemberGrade(gradeId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteMemberGrade, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员等级", new object[]
				{
					gradeId
				}));
			}
			return flag;
		}

		public static IList<MemberGradeInfo> GetMemberGrades()
		{
			return new MemberGradeDao().GetMemberGrades("");
		}

		public static IList<MemberGradeInfo> GetMemberGrades(string GradeIds = "")
		{
			return new MemberGradeDao().GetMemberGrades(GradeIds);
		}

		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return new MemberGradeDao().GetMemberGrade(gradeId);
		}

		public static bool IsExist(string name)
		{
			return new MemberGradeDao().IsExist(name);
		}

		public static DbQueryResult GetMembers(MemberQuery query, bool isNotBindUserName = false)
		{
			return new MemberDao().GetMembers(query, isNotBindUserName);
		}

		public static IList<MemberInfo> GetMembersByRank(int? gradeId)
		{
			return new MemberDao().GetMembersByRank(gradeId);
		}

		public static IList<MemberInfo> GetMemdersByCardNumbers(string cards)
		{
			return new MemberDao().GetMemdersByCardNumbers(cards);
		}

		public static System.Data.DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			return new MemberDao().GetMembersNopage(query, fields);
		}

		public static MemberInfo GetMember(int userId)
		{
			return new MemberDao().GetMember(userId);
		}

		public static int GetMemberIdByUserNameOrNiChen(string username = "", string nich = "")
		{
			return new MemberDao().GetMemberIdByUserNameOrNiChen(username, nich);
		}

		public static bool Delete(int userId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			bool flag = new MemberDao().Delete(userId);
			if (flag)
			{
				HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
				EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的会员", new object[]
				{
					userId
				}));
			}
			return flag;
		}

		public static bool huifu(int userId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			bool flag = new MemberDao().Huifu(userId);
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "恢复了编号为 “{0}” 的会员", new object[]
				{
					userId
				}));
			}
			return flag;
		}

		public static bool BacthHuifu(string userId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			return new MemberDao().BatchHuifu(userId);
		}

		public static bool Delete2(int userId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			bool flag = new MemberDao().Delete2(userId);
			if (flag)
			{
				HiCache.Remove(string.Format("DataCache-Member-{0}", userId));
				EventLogs.WriteOperationLog(Privilege.DeleteMember, string.Format(CultureInfo.InvariantCulture, "逻辑删除了编号为 “{0}” 的会员", new object[]
				{
					userId
				}));
			}
			return flag;
		}

		public static bool Deletes(string userId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteMember);
			return new MemberDao().Deletes(userId);
		}

		public static int SetUsersGradeId(string userId, int gradeId)
		{
			return new MemberDao().SetUsersGradeId(userId, gradeId);
		}

		public static bool SetUserHeadAndUserName(string OpenId, string HUserHead, string UserName)
		{
			return new MemberDao().SetUserHeadAndUserName(OpenId, HUserHead, UserName);
		}

		public static bool Update(MemberInfo member)
		{
			bool flag = new MemberDao().Update(member);
			if (flag)
			{
				HiCache.Remove(string.Format("DataCache-Member-{0}", member.UserId));
				EventLogs.WriteOperationLog(Privilege.EditMember, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的会员", new object[]
				{
					member.UserId
				}));
			}
			return flag;
		}

		public static int IsExiteDistributorNames(string distributorname)
		{
			return new DistributorsDao().IsExiteDistributorsByStoreName(distributorname);
		}

		public static bool CreateDistributorByUserIds(string userids, ref string msg)
		{
			bool flag = false;
			userids = userids.Trim(new char[]
			{
				','
			});
			string[] array = userids.Split(new char[]
			{
				','
			});
			bool result;
			if (array.Length == 0)
			{
				msg = "没有会员被选择！";
				result = flag;
			}
			else
			{
				DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorGradeDao().GetIsDefaultDistributorGradeInfo();
				if (isDefaultDistributorGradeInfo == null)
				{
					msg = "默认分销商等级未设置,无法生成分销商!";
					result = flag;
				}
				else
				{
					Dictionary<int, bool> existDistributorList = new DistributorsDao().GetExistDistributorList(userids);
					List<int> list = new List<int>();
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string s = array2[i];
						int num = 0;
						if (int.TryParse(s, out num) && !existDistributorList.ContainsKey(num))
						{
							list.Add(num);
						}
					}
					if (list.Count == 0)
					{
						msg = "选择的会员已经是分销商，操作终止！";
						result = flag;
					}
					else
					{
						int num2 = 0;
						foreach (int current in list)
						{
							int userId = current;
							MemberInfo member = MemberHelper.GetMember(userId);
							int referralUserId = member.ReferralUserId;
							string referralPath = string.Empty;
							DistributorsInfo distributorsInfo = new DistributorsInfo();
							distributorsInfo.DistributorGradeId = DistributorGrade.OneDistributor;
							if (referralUserId > 0)
							{
								DistributorsInfo distributorInfo = new DistributorsDao().GetDistributorInfo(referralUserId);
								if (distributorInfo != null)
								{
									if (!string.IsNullOrEmpty(distributorInfo.ReferralPath) && !distributorInfo.ReferralPath.Contains("|"))
									{
										referralPath = distributorInfo.ReferralPath + "|" + distributorInfo.UserId.ToString();
									}
									else if (!string.IsNullOrEmpty(distributorInfo.ReferralPath) && distributorInfo.ReferralPath.Contains("|"))
									{
										referralPath = distributorInfo.ReferralPath.Split(new char[]
										{
											'|'
										})[1] + "|" + distributorInfo.UserId.ToString();
									}
									else
									{
										referralPath = distributorInfo.UserId.ToString();
									}
									if (!string.IsNullOrEmpty(distributorInfo.Logo))
									{
										distributorsInfo.Logo = distributorInfo.Logo;
									}
									if (distributorInfo.DistributorGradeId == DistributorGrade.OneDistributor)
									{
										distributorsInfo.DistributorGradeId = DistributorGrade.TowDistributor;
									}
									else if (distributorInfo.DistributorGradeId == DistributorGrade.TowDistributor)
									{
										distributorsInfo.DistributorGradeId = DistributorGrade.ThreeDistributor;
									}
									else
									{
										distributorsInfo.DistributorGradeId = DistributorGrade.ThreeDistributor;
									}
								}
							}
							if (string.IsNullOrEmpty(distributorsInfo.Logo))
							{
								SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
								distributorsInfo.Logo = masterSettings.DistributorLogoPic;
							}
							distributorsInfo.UserId = member.UserId;
							distributorsInfo.RequestAccount = "";
							distributorsInfo.StoreName = "微小店_" + DateTime.Now.ToString("HHmmss") + "_" + member.UserName;
							distributorsInfo.StoreDescription = "系统生成的微小店，请修改描述内容";
							distributorsInfo.BackImage = "";
							distributorsInfo.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
							distributorsInfo.ReferralPath = referralPath;
							distributorsInfo.ParentUserId = new int?(Convert.ToInt32(referralUserId));
							if (new DistributorsDao().CreateDistributor(distributorsInfo))
							{
								num2++;
							}
						}
						if (num2 > 0)
						{
							msg = "成功生成" + num2.ToString() + "位分销商，请检查！";
							flag = true;
						}
						else
						{
							msg = "生成分销商失败！";
							flag = false;
						}
						result = flag;
					}
				}
			}
			return result;
		}

		public static IList<string> BatchCreateMembers(IList<string> distributornames, int referruserId, string createtype = "1")
		{
			string text = string.Empty;
			string text2 = string.Empty;
			IList<string> list = new List<string>();
			DistributorGrade distributorGradeId = DistributorGrade.ThreeDistributor;
			if (referruserId > 0)
			{
				text2 = new DistributorsDao().GetDistributorInfo(referruserId).ReferralPath;
				if (string.IsNullOrEmpty(text2))
				{
					text2 = referruserId.ToString();
					distributorGradeId = DistributorGrade.TowDistributor;
				}
				else if (text2.Contains("|"))
				{
					text2 = text2.Split(new char[]
					{
						'|'
					})[1] + "|" + referruserId.ToString();
				}
				else
				{
					text2 = text2 + "|" + referruserId.ToString();
				}
			}
			foreach (string current in distributornames)
			{
				MemberInfo memberInfo = new MemberInfo();
				string generateId = Globals.GetGenerateId();
				memberInfo.GradeId = new MemberGradeDao().GetDefaultMemberGrade();
				memberInfo.UserName = current;
				memberInfo.CreateDate = DateTime.Now;
				memberInfo.UserBindName = current;
				memberInfo.SessionId = generateId;
				memberInfo.ReferralUserId = Convert.ToInt32(referruserId);
				memberInfo.SessionEndTime = DateTime.Now.AddYears(10);
				memberInfo.Password = HiCryptographer.Md5Encrypt("888888");
				memberInfo.UserHead = "/templates/common/images/user.png";
				if (new MemberDao().GetusernameMember(current) == null && new MemberDao().CreateMember(memberInfo))
				{
					DistributorsInfo distributorsInfo = new DistributorsInfo();
					distributorsInfo.UserId = new MemberDao().GetusernameMember(current).UserId;
					distributorsInfo.RequestAccount = "";
					distributorsInfo.StoreName = current;
					distributorsInfo.StoreDescription = "";
					distributorsInfo.BackImage = "";
					distributorsInfo.Logo = "";
					distributorsInfo.DistributorGradeId = distributorGradeId;
					text = distributorsInfo.UserId.ToString();
					distributorsInfo.ReferralPath = text2;
					distributorsInfo.ParentUserId = new int?(Convert.ToInt32(referruserId));
					DistributorGradeInfo isDefaultDistributorGradeInfo = new DistributorsDao().GetIsDefaultDistributorGradeInfo();
					distributorsInfo.DistriGradeId = isDefaultDistributorGradeInfo.GradeId;
					if (new DistributorsDao().CreateDistributor(distributorsInfo) && createtype == "1")
					{
						list.Add(current);
					}
				}
				else if (createtype == "2")
				{
					list.Add(current);
				}
			}
			return list;
		}

		public static string GetAllDistributorsName(string keysearch)
		{
			string text = "";
			System.Data.DataTable allDistributorsName = new DistributorsDao().GetAllDistributorsName(keysearch);
			foreach (System.Data.DataRow dataRow in allDistributorsName.Rows)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"{\"title\":\"",
					Globals.HtmlEncode(dataRow[0].ToString()),
					"\",\"result\":\"",
					dataRow[1].ToString(),
					"\"},"
				});
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.Length - 1);
			}
			return text;
		}

		public static int GetSystemDistributorsCount()
		{
			return new DistributorsDao().GetSystemDistributorsCount();
		}

		public static int CanChangeBindWeixin()
		{
			int result;
			if (MemberHelper.GetBindOpenIDAndNoUserNameCount() > 0)
			{
				result = 1;
			}
			else if (MemberHelper.GetBindOpenIDCount() > 0)
			{
				result = 3;
			}
			else
			{
				result = 2;
			}
			return result;
		}

		public static int GetBindOpenIDAndNoUserNameCount()
		{
			return new MemberDao().GetBindOpenIDAndNoUserNameCount();
		}

		public static int GetBindOpenIDCount()
		{
			return new MemberDao().GetBindOpenIDCount();
		}

		public static bool InsertClientSet(Dictionary<int, MemberClientSet> clientset)
		{
			return new MemberDao().InsertClientSet(clientset);
		}

		public static Dictionary<int, MemberClientSet> GetMemberClientSet()
		{
			return new MemberDao().GetMemberClientSet();
		}

		public static int SetRegion(string userID, int regionId)
		{
			return new MemberDao().SetRegion(userID, regionId);
		}

		public static int SetRegions(string userIDs, int regionId)
		{
			return new MemberDao().SetRegions(userIDs, regionId);
		}

		public static int SetUserGroup(int day)
		{
			return new MemberGradeDao().SetUserGroup(day);
		}

		public static int SelectUserGroupSet()
		{
			return new MemberGradeDao().SelectUserGroupSet();
		}

		public static DbQueryResult GetIntegralDetail(IntegralDetailQuery query)
		{
			return new IntegralDetailDao().GetIntegralDetail(query);
		}

		public static bool BindUserName(int UserId, string UserBindName, string Password)
		{
			MemberDao memberDao = new MemberDao();
			return memberDao.BindUserName(UserId, UserBindName, Password);
		}

		public static string GetUserOpenIdByUserId(int UserId)
		{
			MemberDao memberDao = new MemberDao();
			return memberDao.GetOpenIDByUserId(UserId);
		}

		public static string GetAliUserOpenIdByUserId(int UserId)
		{
			MemberDao memberDao = new MemberDao();
			return memberDao.GetAliOpenIDByUserId(UserId);
		}

		public static bool IsExistUserBindName(string userBindName)
		{
			return new MemberDao().GetMemberIdByUserNameOrNiChen(userBindName, null) > 0;
		}

		public static bool ClearAllOpenId()
		{
			return new MemberDao().ClearAllOpenId("wx");
		}

		public static bool ClearAllAlipayopenId()
		{
			return new MemberDao().ClearAllOpenId("fuwu");
		}

		public static bool CheckCurrentMemberIsInRange(string Grades, string DefualtGroup, string CustomGroup, int userid)
		{
			return new MemberDao().CheckCurrentMemberIsInRange(Grades, DefualtGroup, CustomGroup, userid);
		}
	}
}
