using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.SqlDal.Store;
using System;
using System.Data;

namespace Hidistro.ControlPanel.Store
{
	public class NoticeHelper
	{
		public static System.Data.DataSet GetTempSelectedUser(string adminName)
		{
			return new NoticeUserDao().GetTempSelectedUser(adminName);
		}

		public static void AddUser(int userid, string adminname)
		{
			new NoticeUserDao().AddUser(userid, adminname);
		}

		public static void DelUser(int userid, string adminname)
		{
			new NoticeUserDao().DelUser(userid, adminname);
		}

		public static bool GetUserIsSel(int userid, string adminName)
		{
			return new NoticeDao().GetUserIsSel(userid, adminName);
		}

		public static System.Data.DataSet GetSelectedUser(string adminName)
		{
			return new NoticeDao().GetSelectedUser(adminName);
		}

		public static int GetSelectedUser(int noticeid)
		{
			return new NoticeDao().GetSelectedUser(noticeid);
		}

		public static int SaveNotice(NoticeInfo info)
		{
			return new NoticeDao().SaveNotice(info);
		}

		public static DbQueryResult GetNoticeRequest(NoticeQuery query)
		{
			return new NoticeDao().GetNoticeRequest(query);
		}

		public static NoticeInfo GetNoticeInfo(int id)
		{
			return new NoticeDao().GetNoticeInfo(id);
		}

		public static bool DelNotice(int noticeid)
		{
			return new NoticeDao().DelNotice(noticeid);
		}

		public static bool NoticePub(int noticeid)
		{
			return new NoticeDao().NoticePub(noticeid);
		}
	}
}
