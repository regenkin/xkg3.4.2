using Hidistro.Entities.Members;
using Hidistro.SqlDal.Members;
using System;
using System.Collections.Generic;
using System.Data;

namespace Hidistro.ControlPanel.Members
{
	public static class CustomGroupingHelper
	{
		public static System.Data.DataTable GetCustomGroupingUser(int groupId)
		{
			return new CustomGroupingDao().GetCustomGroupingUser(groupId);
		}

		public static System.Data.DataTable GetCustomGroupingDataTable()
		{
			return new CustomGroupingDao().GetCustomGroupingTable();
		}

		public static IList<CustomGroupingInfo> GetCustomGroupingList()
		{
			return new CustomGroupingDao().GetCustomGroupingList();
		}

		public static IList<CustomGroupingInfo> GetCustomGroupingList(string customGroupIds = "")
		{
			return new CustomGroupingDao().GetCustomGroupingList(customGroupIds);
		}

		public static string AddCustomGrouping(CustomGroupingInfo customGroupingInfo)
		{
			return new CustomGroupingDao().AddCustomGrouping(customGroupingInfo);
		}

		public static bool DelGroup(int groupid)
		{
			return new CustomGroupingDao().DelGroup(groupid);
		}

		public static string UpdateCustomGrouping(CustomGroupingInfo customGroupingInfo)
		{
			return new CustomGroupingDao().UpdateCustomGrouping(customGroupingInfo);
		}

		public static CustomGroupingInfo GetGroupInfoById(int groupId)
		{
			return new CustomGroupingDao().GetGroupInfoById(groupId);
		}

		public static string GetMemberGroupList(int userId)
		{
			IList<int> memberGroupList = new CustomGroupingUserDao().GetMemberGroupList(userId);
			string result;
			if (memberGroupList != null && memberGroupList.Count > 0)
			{
				string text = "";
				foreach (int current in memberGroupList)
				{
					text = text + current.ToString() + ",";
				}
				text = text.Substring(0, text.Length - 1);
				result = text;
			}
			else
			{
				result = "-1";
			}
			return result;
		}

		public static string AddCustomGroupingUser(IList<int> UserIdList, int groupId)
		{
			string result = string.Empty;
			int num = 0;
			int num2 = 0;
			CustomGroupingUserDao customGroupingUserDao = new CustomGroupingUserDao();
			foreach (int current in UserIdList)
			{
				if (customGroupingUserDao.GetGroupIdByUserId(current, groupId) > 0)
				{
					num++;
				}
				else if (!customGroupingUserDao.AddCustomGroupingUser(current, groupId))
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				result = string.Concat(new object[]
				{
					"成功添加",
					UserIdList.Count - num2,
					"条，失败",
					num2,
					"条"
				});
			}
			else if (num > 0)
			{
				result = string.Concat(new object[]
				{
					"有效添加",
					UserIdList.Count - num,
					"条，其余",
					num,
					"条已经在该分组中"
				});
			}
			return result;
		}

		public static void SetUserCustomGroup(int userId, IList<int> GroupIdList)
		{
			CustomGroupingUserDao customGroupingUserDao = new CustomGroupingUserDao();
			IList<int> memberGroupList = customGroupingUserDao.GetMemberGroupList(userId);
			if (memberGroupList != null && memberGroupList.Count > 0)
			{
				foreach (int current in memberGroupList)
				{
					customGroupingUserDao.DelGroupUser(userId.ToString(), current);
				}
			}
			if (GroupIdList != null && GroupIdList.Count > 0)
			{
				foreach (int current in GroupIdList)
				{
					customGroupingUserDao.AddCustomGroupingUser(userId, current);
				}
			}
		}

		public static bool DelGroupUser(string UserId, int groupid)
		{
			return new CustomGroupingUserDao().DelGroupUser(UserId, groupid);
		}

		public static IList<int> GetMemberList(MemberQuery query)
		{
			IList<int> list = new List<int>();
			MemberDao memberDao = new MemberDao();
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				list = memberDao.GetStoreNameMemberList(query.StoreName);
			}
			else
			{
				list = memberDao.GetAllMemberList();
			}
			IList<int> list2 = new List<int>(list);
			IList<int> result;
			if (query.TradeMoneyStart.HasValue || query.TradeMoneyEnd.HasValue)
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetTradeMoneyIntervalMemberList(query.TradeMoneyStart, query.TradeMoneyEnd);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			if (query.TradeNumStart.HasValue || query.TradeNumEnd.HasValue)
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetTradeNumIntervalMemberList(query.TradeNumStart, query.TradeNumEnd);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(query.GradeIds) && !query.GradeIds.Equals("0"))
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetGradeMemberList(query.GradeIds);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(query.ClientType) && !query.ClientType.Equals("0"))
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetDefualtGroupMemberList(query.ClientType);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			if (!string.IsNullOrEmpty(query.GroupIds))
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetCustomGroupMemberList(query.GroupIds);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			if (query.RegisterStartTime.HasValue || query.RegisterEndTime.HasValue)
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetCreateDateIntervalMemberList(query.RegisterStartTime, query.RegisterEndTime);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			if (query.StartTime.HasValue || query.EndTime.HasValue)
			{
				if (list.Count > 0)
				{
					IList<int> list3 = memberDao.GetPayDateIntervalMemberList(query.StartTime, query.EndTime);
					if (list3 == null || list3.Count <= 0)
					{
						result = new List<int>();
						return result;
					}
					foreach (int current in list)
					{
						if (!list3.Contains(current))
						{
							list2.Remove(current);
						}
					}
				}
			}
			result = list2;
			return result;
		}
	}
}
