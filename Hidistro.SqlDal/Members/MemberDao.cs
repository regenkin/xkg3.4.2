using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Hidistro.SqlDal.Members
{
	public class MemberDao
	{
		private Database database;

		public MemberDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetTop50NotTopRegionIdBind()
		{
			string query = "select UserID,regionId FROM [aspnet_Members]  where TopRegionId=0 and regionId>0";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			return this.database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}

		public int GetActiveDay()
		{
			string commandText = string.Format("select top 1   isnull(ActiveDay,1)  from Hishop_UserGroupSet", new object[0]);
			int result = 1;
			try
			{
				result = Convert.ToInt32(this.database.ExecuteScalar(System.Data.CommandType.Text, commandText));
			}
			catch
			{
			}
			return result;
		}

		public DbQueryResult GetMembers(MemberQuery query, bool isNotBindUserName = false)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.HasVipCard.HasValue)
			{
				if (query.HasVipCard.Value)
				{
					stringBuilder.Append("VipCardNumber is not null");
				}
				else
				{
					stringBuilder.Append("VipCardNumber is null");
				}
			}
			if (query.GradeId.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("GradeId = {0}", query.GradeId.Value);
			}
			if (query.IsApproved.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("IsApproved = '{0}'", query.IsApproved.Value);
			}
			if (!string.IsNullOrEmpty(query.CellPhone))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("CellPhone = '{0}'", query.CellPhone);
			}
			if (query.Stutas.HasValue)
			{
				if (Convert.ToInt32(query.Stutas) > 0)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" AND ");
					}
					stringBuilder.AppendFormat("Status = {0}", Convert.ToInt32(query.Stutas));
				}
			}
			if (!string.IsNullOrEmpty(query.Username))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			}
			if (!string.IsNullOrEmpty(query.Realname))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ", new object[0]);
				}
				stringBuilder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
			}
			if (!string.IsNullOrEmpty(query.UserBindName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ", new object[0]);
				}
				stringBuilder.AppendFormat("UserBindName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserBindName));
			}
			if (!string.IsNullOrEmpty(query.StoreName))
			{
				if (query.StoreName == "主店")
				{
					stringBuilder.AppendFormat(" AND ReferralUserId not in  (select userid from  dbo.aspnet_Distributors where  UserId=m.ReferralUserId)", new object[0]);
				}
				else
				{
					stringBuilder.AppendFormat(" AND ReferralUserId in  (SELECT UserId  FROM aspnet_Distributors where storename='{0}' )", query.StoreName);
				}
			}
			if (isNotBindUserName)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ", new object[0]);
				}
				stringBuilder.AppendFormat(" (UserBindName is null or UserBindName='')", new object[0]);
			}
			int activeDay = this.GetActiveDay();
			if (!string.IsNullOrEmpty(query.ClientType))
			{
				string clientType = query.ClientType;
				if (clientType != null)
				{
					if (clientType == "new")
					{
						stringBuilder.AppendFormat(" AND LastOrderDate is null", new object[0]);
						goto IL_454;
					}
					if (clientType == "activy")
					{
						int activeDay2 = new MemberDao().GetActiveDay();
						stringBuilder.AppendFormat("  and PayOrderDate is not null and PayOrderDate >='" + DateTime.Now.AddDays((double)(-(double)activeDay2)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", new object[0]);
						goto IL_454;
					}
					if (clientType == "sleep")
					{
						int activeDay2 = new MemberDao().GetActiveDay();
						stringBuilder.AppendFormat("  and  (PayOrderDate is null or PayOrderDate <'" + DateTime.Now.AddDays((double)(-(double)activeDay2)).ToString("yyyy-MM-dd HH:mm:ss") + "') ", new object[0]);
						goto IL_454;
					}
				}
				stringBuilder.AppendFormat(" AND LastOrderDate is null", new object[0]);
				IL_454:;
			}
			if (query.GroupId.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserId in (select UserId from Vshop_CustomGroupingUser where GroupId={0})", query.GroupId.Value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "aspnet_Members m", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*, (SELECT Name FROM aspnet_MemberGrades WHERE GradeId = m.GradeId) AS GradeName ,(select COUNT(*) from  dbo.vw_VShop_FinishOrder_Main where UserId=m.UserId) as OrderCount ,(select SUM(ValidOrderTotal) from  dbo.vw_VShop_FinishOrder_Main where UserId=m.UserId) as OrderTotal,(select StoreName from  dbo.aspnet_Distributors where UserId=m.ReferralUserId ) as StoreName");
		}

		public IList<MemberInfo> GetMembersByRank(int? gradeId)
		{
			IList<MemberInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members");
			if (gradeId.HasValue && gradeId.Value > 0)
			{
				System.Data.Common.DbCommand expr_36 = sqlStringCommand;
				expr_36.CommandText += string.Format(" WHERE GradeId={0} AND Status={1}", gradeId.Value, Convert.ToInt32(UserStatus.Normal));
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MemberInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetMembersByUserId(int referralUserId, int pageIndex, int pageSize, out int total)
		{
			total = 0;
			string query = string.Format(" SELECT count(*) FROM aspnet_Members s left join aspnet_Distributors d on s.userid=d.userid  where  s.ReferralUserId={0}  and Status=1 and d.StoreName is null ", referralUserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				total = (int)obj;
			}
			int num = (pageIndex - 1) * pageSize + 1;
			int num2 = num + pageSize - 1;
			string query2 = string.Format(" SELECT * FROM (select ROW_NUMBER()  OVER (ORDER BY s.userid) as [RowIndex] , s.userid,username,(SELECT Name FROM aspnet_MemberGrades WHERE GradeId = s.GradeId) AS GradeName,UserHead,CreateDate,s.ReferralUserId,d.StoreName, (select count(*) from Hishop_Orders where UserId=s.UserId) as OrderMumber, (select top 1 OrderDate from Hishop_Orders where UserId=s.UserId order by OrderDate desc) as OrderDate, (select SUM(OrderTotal) from Hishop_Orders where UserId=s.UserId )as OrdersTotal  from aspnet_Members s left join aspnet_Distributors d on s.userid=d.userid  where  s.ReferralUserId={0} and Status=1 and d.StoreName is null ) AS W WHERE  RowIndex between {1} and {2}", referralUserId, num, num2);
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand(query2);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand2))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}

		public bool SaveMemberInfoByAddress(ShippingAddressInfo info)
		{
			MemberInfo memberInfo = null;
			if (info != null)
			{
				memberInfo = this.GetMember(info.UserId);
			}
			bool result;
			if (memberInfo != null)
			{
				if (string.IsNullOrEmpty(memberInfo.RealName) && !string.IsNullOrEmpty(info.ShipTo))
				{
					memberInfo.RealName = info.ShipTo;
					memberInfo.CellPhone = info.CellPhone;
					memberInfo.Address = info.Address;
					memberInfo.RegionId = info.RegionId;
					memberInfo.TopRegionId = RegionHelper.GetTopRegionId(info.RegionId);
				}
				result = this.Update(memberInfo);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public IList<MemberInfo> GetMemdersByCardNumbers(string cards)
		{
			IList<MemberInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Members WHERE VipCardNumber IN ({0}) AND Status={1} ", cards, Convert.ToInt32(UserStatus.Normal)));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MemberInfo>(dataReader);
			}
			return result;
		}

		public IList<MemberInfo> GetMemdersByOpenIds(string openids)
		{
			IList<MemberInfo> result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM aspnet_Members where openid IN ({0}) AND Status={1}", openids, Convert.ToInt32(UserStatus.Normal)));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MemberInfo>(dataReader);
			}
			return result;
		}

		public System.Data.DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			System.Data.DataTable result;
			if (fields.Count == 0)
			{
				result = null;
			}
			else
			{
				System.Data.DataTable dataTable = null;
				string text = string.Empty;
				foreach (string current in fields)
				{
					text = text + current + ",";
				}
				text = text.Substring(0, text.Length - 1);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("SELECT {0} FROM aspnet_Members WHERE   Status={1} ", text, Convert.ToInt32(UserStatus.Normal));
				if (!string.IsNullOrEmpty(query.Username))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
				}
				if (query.GradeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GradeId={0}", query.GradeId);
				}
				if (query.HasVipCard.HasValue)
				{
					if (query.HasVipCard.Value)
					{
						stringBuilder.Append(" AND VipCardNumber is not null");
					}
					else
					{
						stringBuilder.Append(" AND VipCardNumber is null");
					}
				}
				if (!string.IsNullOrEmpty(query.Realname))
				{
					stringBuilder.AppendFormat(" AND Realname LIKE '%{0}%'", query.Realname);
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					dataReader.Close();
				}
				result = dataTable;
			}
			return result;
		}

		public MemberInfo GetMember(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			MemberInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberInfo>(dataReader);
			}
			return result;
		}

		public DistributorInfo GetDistributor(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.StoreName, a.CreateTime as  CreateTime_Distributor, a.DistributorGradeId,\r\n                b.* FROM aspnet_Distributors a\r\n                left join aspnet_Members b  on a.UserId=b.UserId\r\n                WHERE a.UserId = @UserId UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			DistributorInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<DistributorInfo>(dataReader);
			}
			return result;
		}

		public MemberInfo GetusernameMember(string username)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserBindName = @UserBindName ");
			this.database.AddInParameter(sqlStringCommand, "UserBindName", System.Data.DbType.String, username);
			MemberInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberInfo>(dataReader);
			}
			return result;
		}

		public int GetMemberIdByUserNameOrNiChen(string username = "", string nich = "")
		{
			int result;
			if (!string.IsNullOrWhiteSpace(username))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Members WHERE UserBindName = @UserBindName AND Status=@Status");
				this.database.AddInParameter(sqlStringCommand, "UserBindName", System.Data.DbType.String, username);
				this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Convert.ToInt32(UserStatus.Normal));
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				if (obj != null)
				{
					result = (int)obj;
					return result;
				}
			}
			else if (!string.IsNullOrWhiteSpace(nich))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Members WHERE UserName = @UserName AND Status=@Status");
				this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, nich);
				this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Convert.ToInt32(UserStatus.Normal));
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				if (obj != null)
				{
					result = (int)obj;
					return result;
				}
			}
			result = 0;
			return result;
		}

		public MemberInfo GetBindusernameMember(string UserBindName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserBindName = @UserBindName");
			this.database.AddInParameter(sqlStringCommand, "UserBindName", System.Data.DbType.String, UserBindName);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Convert.ToInt32(UserStatus.Normal));
			MemberInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberInfo>(dataReader);
			}
			return result;
		}

		public MemberInfo GetMember(string sessionId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE SessionId = @SessionId AND Status=@Status");
			this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, sessionId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Convert.ToInt32(UserStatus.Normal));
			MemberInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberInfo>(dataReader);
			}
			return result;
		}

		public bool CreateMember(MemberInfo member)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_Members(GradeId,ReferralUserId,UserName,CreateDate,OrderNumber,Expenditure,Points,TopRegionId, RegionId,OpenId, SessionId, SessionEndTime,Password,UserHead,UserBindName,Status,AlipayUserId,AlipayUsername,AlipayOpenid,AlipayLoginId,AlipayAvatar) VALUES(@GradeId,@ReferralUserId,@UserName,@CreateDate,0,0,0,0,0,@OpenId, @SessionId, @SessionEndTime,@Password,@UserHead,@UserBindName,@Status,@AlipayUserId,@AlipayUsername,@AlipayOpenid,@AlipayLoginId,@AlipayAvatar)");
			if (member.AlipayOpenid == null)
			{
				member.AlipayOpenid = "";
			}
			if (member.AlipayLoginId == null)
			{
				member.AlipayLoginId = "";
			}
			if (member.AlipayUserId == null)
			{
				member.AlipayUserId = "";
			}
			if (member.AlipayUsername == null)
			{
				member.AlipayUsername = "";
			}
			if (member.AlipayAvatar == null)
			{
				member.AlipayAvatar = "";
			}
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, member.ReferralUserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, member.UserName);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, member.CreateDate);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, member.OpenId);
			this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, member.SessionId);
			this.database.AddInParameter(sqlStringCommand, "SessionEndTime", System.Data.DbType.DateTime, member.SessionEndTime);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, member.Password);
			this.database.AddInParameter(sqlStringCommand, "UserHead", System.Data.DbType.String, member.UserHead);
			this.database.AddInParameter(sqlStringCommand, "UserBindName", System.Data.DbType.String, member.UserBindName);
			this.database.AddInParameter(sqlStringCommand, "AlipayAvatar", System.Data.DbType.String, member.AlipayAvatar);
			this.database.AddInParameter(sqlStringCommand, "AlipayUsername", System.Data.DbType.String, member.AlipayUsername);
			this.database.AddInParameter(sqlStringCommand, "AlipayLoginId", System.Data.DbType.String, member.AlipayLoginId);
			this.database.AddInParameter(sqlStringCommand, "AlipayOpenid", System.Data.DbType.String, member.AlipayOpenid);
			this.database.AddInParameter(sqlStringCommand, "AlipayUserId", System.Data.DbType.String, member.AlipayUserId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, Convert.ToInt32(UserStatus.Normal));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public string GetCurrentParentUserId(int? userId)
		{
			string result = "";
			string query = "SELECT ReferralPath FROM aspnet_Distributors WHERE UserId=@UserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int64, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = userId.ToString();
				if (dataReader["ReferralUserId"].ToString() != "0")
				{
					result = dataReader["ReferralUserId"].ToString() + "|" + userId.ToString();
				}
			}
			return result;
		}

		public bool IsExitOpenId(string openId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(*) FROM aspnet_Members WHERE OpenId = @OpenId");
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}

		public MemberInfo GetOpenIdMember(string openId, string From = "wx")
		{
			System.Data.Common.DbCommand sqlStringCommand;
			if (From == "fuwu")
			{
				if (openId.Length > 16)
				{
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE AlipayOpenid = @openId ");
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE AlipayUserId = @openId ");
				}
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE openId = @openId");
			}
			this.database.AddInParameter(sqlStringCommand, "openId", System.Data.DbType.String, openId);
			MemberInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToModel<MemberInfo>(dataReader);
			}
			return result;
		}

		public List<MemberInfo> GetMemberInfoList(string userIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_Members WHERE UserId in (" + userIds + ")");
			List<MemberInfo> result = new List<MemberInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = ReaderConvert.ReaderToList<MemberInfo>(dataReader).ToList<MemberInfo>();
			}
			return result;
		}

		public bool BindUserName(int UserId, string UserBindName, string Password)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET UserBindName = @UserBindName, Password = @Password WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserBindName", System.Data.DbType.String, UserBindName);
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, Password);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetMemberSessionId(string sessionId, DateTime sessionEndTime, string openId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET SessionId = @SessionId, SessionEndTime = @SessionEndTime WHERE OpenId = @OpenId");
			this.database.AddInParameter(sqlStringCommand, "SessionId", System.Data.DbType.String, sessionId);
			this.database.AddInParameter(sqlStringCommand, "SessionEndTime", System.Data.DbType.DateTime, sessionEndTime);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool SetPwd(string userid, string pwd)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Password = @Password  WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, pwd);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool ReSetUserHead(string userid, string wxName, string wxHead, string Openid = "")
		{
			string str = "";
			if (!string.IsNullOrEmpty(Openid.Trim()))
			{
				str = ",OpenId=@OpenId ";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET UserName = @UserName,UserHead = @UserHead " + str + " WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, wxName);
			this.database.AddInParameter(sqlStringCommand, "UserHead", System.Data.DbType.String, wxHead);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userid);
			if (!string.IsNullOrEmpty(Openid.Trim()))
			{
				this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, Openid);
			}
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int SetMultiplePwd(string userids, string pwd)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET Password = @Password  WHERE UserId in(" + userids + ")");
			this.database.AddInParameter(sqlStringCommand, "Password", System.Data.DbType.String, pwd);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int SetAlipayInfos(MemberInfo user)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET AlipayLoginId = @AlipayLoginId,AlipayOpenid = @AlipayOpenid,AlipayUserId = @AlipayUserId,AlipayUsername = @AlipayUsername,AlipayAvatar = @AlipayAvatar  WHERE UserId=@UserId ");
			this.database.AddInParameter(sqlStringCommand, "AlipayUserId", System.Data.DbType.String, user.AlipayUserId);
			this.database.AddInParameter(sqlStringCommand, "AlipayOpenid", System.Data.DbType.String, user.AlipayOpenid);
			this.database.AddInParameter(sqlStringCommand, "AlipayLoginId", System.Data.DbType.String, user.AlipayLoginId);
			this.database.AddInParameter(sqlStringCommand, "AlipayUsername", System.Data.DbType.String, user.AlipayUsername);
			this.database.AddInParameter(sqlStringCommand, "AlipayAvatar", System.Data.DbType.String, user.AlipayAvatar);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, user.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int GetMemberNumOfTotal(int topUserId, out int topNum)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select (SELECT COUNT(UserId) \r\n  FROM aspnet_Members) as total,(SELECT COUNT(UserId) \r\n  FROM aspnet_Members where ReferralUserId=@ReferralUserId ) as topNum");
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, topUserId);
			System.Data.DataTable dataTable = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = (int)dataTable.Rows[0]["total"];
				topNum = (int)dataTable.Rows[0]["topNum"];
			}
			else
			{
				topNum = 0;
			}
			return result;
		}

		public int GetDistributorNumOfTotal(int topUserId, out int topNum)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select (SELECT COUNT(UserId) \r\n  FROM aspnet_Distributors) as total,(SELECT COUNT(UserId) \r\n  FROM aspnet_Distributors where ReferralUserId=@ReferralUserId ) as topNum");
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, topUserId);
			System.Data.DataTable dataTable = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				result = (int)dataTable.Rows[0]["total"];
				topNum = (int)dataTable.Rows[0]["topNum"];
			}
			else
			{
				topNum = 0;
			}
			return result;
		}

		public int SetRegion(string userID, int regionId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET ReferralUserId = @ReferralUserId  WHERE UserId =" + userID);
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, regionId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int SetOrderDate(int userID, int orderType)
		{
			string text = string.Empty;
			switch (orderType)
			{
			case 0:
				text = "LastOrderDate";
				break;
			case 1:
				text = "PayOrderDate";
				break;
			case 2:
				text = "FinishOrderDate";
				break;
			default:
				text = "LastOrderDate";
				break;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"UPDATE aspnet_Members SET ",
				text,
				" = getdate()  WHERE UserId =",
				userID
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public int SetRegions(string userIDs, int regionId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET ReferralUserId = @ReferralUserId  WHERE UserId in(" + userIDs + ")");
			this.database.AddInParameter(sqlStringCommand, "ReferralUserId", System.Data.DbType.Int32, regionId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool DelUserMessage(int userid, string openid, string userhead, int olduserid)
		{
			string text = "";
			text += "begin try  ";
			text += "  begin tran TranUpdate";
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM aspnet_Members WHERE UserId =",
				userid,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM Hishop_ShoppingCarts WHERE UserId =",
				userid,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM Hishop_UserShippingAddresses WHERE UserId =",
				userid,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM vshop_ActivitySignUp WHERE UserId =",
				userid,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" Update  aspnet_Members set OpenId='",
				openid,
				"',UserHead='",
				userhead,
				"' WHERE UserId =",
				olduserid,
				"; "
			});
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelUserMessage(MemberInfo newuser, int olduserid)
		{
			string text = "";
			text += "begin try  ";
			text += "  begin tran TranUpdate";
			object obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM aspnet_Members WHERE UserId =",
				newuser.UserId,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM Hishop_ShoppingCarts WHERE UserId =",
				newuser.UserId,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM Hishop_UserShippingAddresses WHERE UserId =",
				newuser.UserId,
				"; "
			});
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				" DELETE FROM vshop_ActivitySignUp WHERE UserId =",
				newuser.UserId,
				"; "
			});
			text += " Update  aspnet_Members ";
			string text2 = "";
			if (!string.IsNullOrEmpty(newuser.OpenId))
			{
				text2 = string.Concat(new string[]
				{
					"  set OpenId='",
					newuser.OpenId,
					"',UserHead='",
					newuser.UserHead,
					"'"
				});
			}
			if (!string.IsNullOrEmpty(newuser.AlipayOpenid))
			{
				if (text2 == "")
				{
					text2 = " set ";
				}
				else
				{
					text2 = " ,";
				}
				string text3 = text2;
				text2 = string.Concat(new string[]
				{
					text3,
					"AlipayUserId='",
					newuser.AlipayUserId,
					"',AlipayOpenid='",
					newuser.AlipayOpenid,
					"'"
				});
				text3 = text2;
				text2 = string.Concat(new string[]
				{
					text3,
					",AlipayLoginId='",
					newuser.AlipayLoginId,
					"',AlipayUsername='",
					newuser.AlipayUsername,
					"'"
				});
				text2 = text2 + ",AlipayAvatar='" + newuser.AlipayAvatar + "' ";
				if (string.IsNullOrEmpty(newuser.UserHead) && !string.IsNullOrEmpty(newuser.AlipayAvatar) && string.IsNullOrEmpty(newuser.OpenId))
				{
					text2 = text2 + ",UserHead='" + newuser.AlipayAvatar + "'";
				}
			}
			obj = text;
			text = string.Concat(new object[]
			{
				obj,
				text2,
				" WHERE UserId =",
				olduserid,
				" ;"
			});
			text += " COMMIT TRAN TranUpdate";
			text += "  end try \r\n                    begin catch \r\n                        ROLLBACK TRAN TranUpdate\r\n                    end catch ";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Delete(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_Members WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Huifu(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=1 WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool BatchHuifu(string userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=1 WHERE UserId in (" + userId + ")");
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Delete2(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=7 WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Deletes(string userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET Status=7 WHERE UserId in (" + userId + ")");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public int SetUsersGradeId(string userId, int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new object[]
			{
				"UPDATE  aspnet_Members SET GradeId=",
				gradeId,
				"  WHERE UserId in (",
				userId,
				")"
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}

		public bool SetUserHeadAndUserName(string OpenId, string UserHead, string UserName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE  aspnet_Members SET UserName = @UserName,UserHead=@UserHead,IsAuthorizeWeiXin=1 where OpenId = @OpenId ");
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, OpenId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, UserName);
			this.database.AddInParameter(sqlStringCommand, "UserHead", System.Data.DbType.String, UserHead);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool Update(MemberInfo member)
		{
			string str = string.Empty;
			if (string.IsNullOrEmpty(member.OpenId))
			{
				str = ",OpenId = @OpenId";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Members SET GradeId = @GradeId" + str + ",UserName = @UserName, RealName = @RealName, TopRegionId = @TopRegionId, RegionId = @RegionId,VipCardNumber = @VipCardNumber, VipCardDate = @VipCardDate, Email = @Email, CellPhone = @CellPhone, QQ = @QQ, Address = @Address, Expenditure = @Expenditure, OrderNumber = @OrderNumber,MicroSignal=@MicroSignal,UserHead=@UserHead,CardID=@CardID WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, member.OpenId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, member.UserName);
			this.database.AddInParameter(sqlStringCommand, "RealName", System.Data.DbType.String, member.RealName);
			this.database.AddInParameter(sqlStringCommand, "TopRegionId", System.Data.DbType.Int32, member.TopRegionId);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, member.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Email", System.Data.DbType.String, member.Email);
			this.database.AddInParameter(sqlStringCommand, "VipCardNumber", System.Data.DbType.String, member.VipCardNumber);
			this.database.AddInParameter(sqlStringCommand, "VipCardDate", System.Data.DbType.DateTime, member.VipCardDate);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, member.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "QQ", System.Data.DbType.String, member.QQ);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, member.Address);
			this.database.AddInParameter(sqlStringCommand, "Expenditure", System.Data.DbType.Currency, member.Expenditure);
			this.database.AddInParameter(sqlStringCommand, "OrderNumber", System.Data.DbType.Int32, member.OrderNumber);
			this.database.AddInParameter(sqlStringCommand, "MicroSignal", System.Data.DbType.String, member.MicroSignal);
			this.database.AddInParameter(sqlStringCommand, "UserHead", System.Data.DbType.String, member.UserHead);
			this.database.AddInParameter(sqlStringCommand, "CardID", System.Data.DbType.String, member.CardID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool InsertClientSet(Dictionary<int, MemberClientSet> clientsets)
		{
			StringBuilder stringBuilder = new StringBuilder("DELETE FROM  [Hishop_MemberClientSet];");
			foreach (KeyValuePair<int, MemberClientSet> current in clientsets)
			{
				string text = "";
				string text2 = "";
				if (current.Value.StartTime.HasValue)
				{
					text = current.Value.StartTime.Value.ToString("yyyy-MM-dd");
				}
				if (current.Value.EndTime.HasValue)
				{
					text2 = current.Value.EndTime.Value.ToString("yyyy-MM-dd");
				}
				stringBuilder.AppendFormat(string.Concat(new object[]
				{
					"INSERT INTO Hishop_MemberClientSet(ClientTypeId,StartTime,EndTime,LastDay,ClientChar,ClientValue) VALUES (",
					current.Key,
					",'",
					text,
					"','",
					text2,
					"',",
					current.Value.LastDay,
					",'",
					current.Value.ClientChar,
					"',",
					current.Value.ClientValue,
					");"
				}), new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public Dictionary<int, MemberClientSet> GetMemberClientSet()
		{
			Dictionary<int, MemberClientSet> dictionary = new Dictionary<int, MemberClientSet>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MemberClientSet");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					MemberClientSet memberClientSet = DataMapper.PopulateMemberClientSet(dataReader);
					dictionary.Add(memberClientSet.ClientTypeId, memberClientSet);
				}
			}
			return dictionary;
		}

		public int GetBindOpenIDAndNoUserNameCount()
		{
			string query = string.Format("Select COUNT(*) as SumRec from  aspnet_Members where (UserBindName is null or UserBindName='')  and Status={0};", 1);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public int GetBindOpenIDCount()
		{
			string query = string.Format("Select COUNT(*) as SumRec from  aspnet_Members where OpenId is not null and OpenId<>'' and Status={0};", 1);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public bool ClearAllOpenId(string from = "wx")
		{
			string query = "Update aspnet_Members set OpenId='',IsAuthorizeWeiXin=0";
			if (from == "fuwu")
			{
				query = "Update aspnet_Members set AlipayOpenid=''";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			int num = this.database.ExecuteNonQuery(sqlStringCommand);
			return num > 0;
		}

		public string GetOpenIDByUserId(int UserId)
		{
			string query = string.Format("Select top 1 OpenId  from  aspnet_Members where  UserId={0}  ", UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (string)obj;
			}
			else
			{
				result = "";
			}
			return result;
		}

		public string GetAliOpenIDByUserId(int UserId)
		{
			string query = string.Format("Select top 1 AlipayOpenid  from  aspnet_Members where  UserId={0}  ", UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (string)obj;
			}
			else
			{
				result = "";
			}
			return result;
		}

		public int GetMemeberNumBySearch(string gradeIds, string referralUserId, string beginCreateDate, string endCreateDate, int userType, string customGroup, int adminId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_SendCouponUser");
			this.database.AddInParameter(storedProcCommand, "@GradeIds", System.Data.DbType.String, gradeIds);
			this.database.AddInParameter(storedProcCommand, "@ReferralUserId", System.Data.DbType.String, referralUserId);
			this.database.AddInParameter(storedProcCommand, "@BeginCreateDate", System.Data.DbType.String, beginCreateDate);
			this.database.AddInParameter(storedProcCommand, "@EndCreateDate", System.Data.DbType.String, endCreateDate);
			this.database.AddInParameter(storedProcCommand, "@UserType", System.Data.DbType.Int32, userType);
			this.database.AddInParameter(storedProcCommand, "@CustomGroupIds", System.Data.DbType.String, customGroup);
			this.database.AddInParameter(storedProcCommand, "@AdminId", System.Data.DbType.Int32, adminId);
			this.database.AddOutParameter(storedProcCommand, "@Count", System.Data.DbType.Int32, 4);
			this.database.ExecuteNonQuery(storedProcCommand);
			object value = storedProcCommand.Parameters["@Count"].Value;
			int result;
			if (value != null && value != DBNull.Value)
			{
				result = (int)value;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public IList<int> GetGradeMemberList(string Grades)
		{
			IList<int> list = new List<int>();
			string query = "SELECT UserId FROM dbo.aspnet_Members WHERE GradeId in (" + Grades + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetCustomGroupMemberList(string CustomGroup)
		{
			IList<int> list = new List<int>();
			string query;
			if (!CustomGroup.Equals("0"))
			{
				query = "SELECT UserId FROM dbo.Vshop_CustomGroupingUser WHERE GroupId in (" + CustomGroup + ")";
			}
			else
			{
				query = "SELECT UserId FROM dbo.Vshop_CustomGroupingUser";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetDefualtGroupMemberList(string DefualtGroup)
		{
			IList<int> list = new List<int>();
			if (DefualtGroup.Contains("1"))
			{
				list = this.GetNewGroupMemberList();
			}
			if (DefualtGroup.Contains("2"))
			{
				if (list.Count == 0)
				{
					list = this.GetActivyGroupMemberList();
				}
				else
				{
					IList<int> activyGroupMemberList = this.GetActivyGroupMemberList();
					foreach (int current in activyGroupMemberList)
					{
						list.Add(current);
					}
				}
			}
			if (DefualtGroup.Contains("3"))
			{
				if (list.Count == 0)
				{
					list = this.GetSleepGroupMemberList();
				}
				else
				{
					IList<int> sleepGroupMemberList = this.GetSleepGroupMemberList();
					foreach (int current in sleepGroupMemberList)
					{
						list.Add(current);
					}
				}
			}
			return list;
		}

		public IList<int> GetNewGroupMemberList()
		{
			IList<int> list = new List<int>();
			string query = "SELECT UserId FROM aspnet_Members WHERE LastOrderDate IS NULL";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetActivyGroupMemberList()
		{
			IList<int> list = new List<int>();
			int activeDay = this.GetActiveDay();
			string query = string.Format("SELECT UserId FROM aspnet_Members WHERE PayOrderDate is not null and PayOrderDate >='" + DateTime.Now.AddDays((double)(-(double)activeDay)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetSleepGroupMemberList()
		{
			IList<int> list = new List<int>();
			int activeDay = this.GetActiveDay();
			string query = string.Format("SELECT UserId FROM aspnet_Members WHERE PayOrderDate is null or PayOrderDate <'" + DateTime.Now.AddDays((double)(-(double)activeDay)).ToString("yyyy-MM-dd HH:mm:ss") + "' ", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetStoreNameMemberList(string StoreName)
		{
			IList<int> list = new List<int>();
			string text = "SELECT UserId FROM dbo.aspnet_Members m WHERE 1=1 ";
			if (StoreName == "主店")
			{
				text += " AND ReferralUserId not in (select userid from dbo.aspnet_Distributors where UserId=m.ReferralUserId)";
			}
			else
			{
				text += " AND ReferralUserId in (SELECT UserId  FROM aspnet_Distributors where storename=@StoreName)";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			if (StoreName != "主店")
			{
				this.database.AddInParameter(sqlStringCommand, "StoreName", System.Data.DbType.String, StoreName);
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetTradeMoneyIntervalMemberList(decimal? start, decimal? end)
		{
			IList<int> list = new List<int>();
			string text = "SELECT m.UserId FROM aspnet_Members AS m\r\n                           LEFT JOIN \r\n                          (SELECT SUM(ValidOrderTotal) AS ValidOrderTotal,UserId FROM dbo.vw_VShop_FinishOrder_Main GROUP BY UserId) AS v \r\n                           ON m.UserId=v.UserId WHERE 1=1 ";
			if (start.HasValue)
			{
				text = text + " AND v.ValidOrderTotal >= " + start.Value;
			}
			if (end.HasValue)
			{
				text = text + " AND v.ValidOrderTotal <= " + end.Value;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetTradeNumIntervalMemberList(int? start, int? end)
		{
			IList<int> list = new List<int>();
			string text = "SELECT m.UserId FROM aspnet_Members AS m\r\n                           LEFT JOIN \r\n                          (SELECT SUM(1) AS OrderCount,UserId FROM dbo.vw_VShop_FinishOrder_Main GROUP BY UserId) AS v \r\n                           ON m.UserId=v.UserId WHERE 1=1 ";
			if (start.HasValue)
			{
				text = text + " AND v.OrderCount >= " + start.Value;
			}
			if (end.HasValue)
			{
				text = text + " AND v.OrderCount <= " + end.Value;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetCreateDateIntervalMemberList(DateTime? start, DateTime? end)
		{
			IList<int> list = new List<int>();
			string text = "SELECT UserId FROM dbo.aspnet_Members WHERE 1=1 ";
			if (start.HasValue)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,'",
					start.Value,
					"',CreateDate) >= 0"
				});
			}
			if (end.HasValue)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,'",
					end.Value,
					"',CreateDate) <= 0"
				});
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetPayDateIntervalMemberList(DateTime? start, DateTime? end)
		{
			IList<int> list = new List<int>();
			string text = "SELECT m.UserId FROM aspnet_Members AS m\r\n                           LEFT JOIN \r\n                          (SELECT MAX(PayDate) AS PayDate,UserId FROM dbo.vw_VShop_FinishOrder_Main GROUP BY UserId) AS v \r\n                           ON m.UserId=v.UserId WHERE 1=1 ";
			if (start.HasValue)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,'",
					start.Value,
					"',v.PayDate) >= 0"
				});
			}
			if (end.HasValue)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,'",
					end.Value,
					"',v.PayDate) <= 0"
				});
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public IList<int> GetAllMemberList()
		{
			IList<int> list = new List<int>();
			string query = "SELECT UserId FROM dbo.aspnet_Members where Status=1";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						int item = Convert.ToInt32(dataRow["UserId"]);
						list.Add(item);
					}
				}
			}
			return list;
		}

		public bool CheckCurrentMemberIsInRange(string Grades, string DefualtGroup, string CustomGroup, int tuserid = 0)
		{
			bool flag = false;
			MemberDao memberDao = new MemberDao();
			int item;
			if (tuserid <= 0)
			{
				item = Globals.GetCurrentMemberUserId();
			}
			else
			{
				item = tuserid;
			}
			bool result;
			if (!string.IsNullOrEmpty(Grades))
			{
				if (!Grades.Equals("-1"))
				{
					if (Grades.Equals("0"))
					{
						result = true;
						return result;
					}
					IList<int> list = memberDao.GetGradeMemberList(Grades);
					flag = list.Contains(item);
					if (flag)
					{
						result = true;
						return result;
					}
				}
			}
			if (!string.IsNullOrEmpty(DefualtGroup))
			{
				if (!DefualtGroup.Equals("-1"))
				{
					if (DefualtGroup.Equals("0"))
					{
						result = true;
						return result;
					}
					IList<int> list = memberDao.GetDefualtGroupMemberList(DefualtGroup);
					flag = list.Contains(item);
					if (flag)
					{
						result = true;
						return result;
					}
				}
			}
			if (!string.IsNullOrEmpty(CustomGroup))
			{
				IList<int> list = memberDao.GetCustomGroupMemberList(CustomGroup);
				flag = list.Contains(item);
			}
			result = flag;
			return result;
		}

		public bool CheckMemberIsBuyProds(int userId, string prodIds, DateTime? startTime, DateTime? endTime)
		{
			string text = "SELECT COUNT(o.OrderId) \r\n                             FROM Hishop_Orders AS o \r\n                        LEFT JOIN Hishop_OrderItems AS oi ON o.OrderId=oi.OrderId\r\n                        where 1=1 ";
			text = text + " AND UserId=" + userId;
			if (startTime.HasValue)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,'",
					startTime.Value,
					"',o.PayDate) >= 0"
				});
			}
			if (endTime.HasValue)
			{
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,'",
					endTime.Value.AddDays(1.0),
					"',o.PayDate) < 0"
				});
			}
			text = text + " AND oi.ProductId IN (" + prodIds + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			object value = this.database.ExecuteScalar(sqlStringCommand);
			bool result;
			try
			{
				result = (Convert.ToInt32(value) > 0);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public int GetUserFollowStateByUserId(int UserId, string type = "wx")
		{
			int result = 0;
			string query;
			if (type == "wx")
			{
				query = string.Format("Select top 1 IsFollowWeixin  from  aspnet_Members where  UserId={0}  ", UserId);
			}
			else
			{
				query = string.Format("Select top 1 IsFollowAlipay  from  aspnet_Members where  UserId={0}  ", UserId);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = (int)obj;
			}
			return result;
		}

		public bool UpdateUserFollowStateByUserId(int UserId, int state, string type = "wx")
		{
			string text;
			if (type == "wx")
			{
				text = string.Format(" set  IsFollowWeixin={0} ", state);
			}
			else
			{
				text = string.Format(" set  IsFollowAlipay={0} ", state);
			}
			text = "update  aspnet_Members " + text + string.Format(" where  UserId={0}  ", UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool addFuwuFollowUser(string openid)
		{
			string query = "delete from Vshop_FollowUsers where OpenId=@OpenId;insert into Vshop_FollowUsers(OpenId,FollowTime)values(@OpenId,@FollowTime);update aspnet_Members set IsFollowAlipay=1 where AlipayOpenid=@OpenId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openid);
			this.database.AddInParameter(sqlStringCommand, "FollowTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool DelFuwuFollowUser(string openid)
		{
			string query = "delete from Vshop_FollowUsers where OpenId=@OpenId;update aspnet_Members set IsFollowAlipay=0 where AlipayOpenid=@OpenId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openid);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}

		public bool IsFuwuFollowUser(string openid)
		{
			string query = "select top 1 openid from Vshop_FollowUsers where OpenId=@OpenId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(query);
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openid);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return obj != null;
		}
	}
}
