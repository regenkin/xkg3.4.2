using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.VShop;
using Hidistro.SqlDal.VShop;
using System;
using System.Data;

namespace Hidistro.ControlPanel.Store
{
	public static class UserSignHelper
	{
		public static int InsertUserSign(UserSign us)
		{
			return new UserSignDao().InsertUserSign(us);
		}

		public static System.Data.DataTable SignInfoByUser(int userID)
		{
			return new UserSignDao().SignInfoByUser(userID);
		}

		public static int UpdateUserSign(UserSign us)
		{
			return new UserSignDao().UpdateUserSign(us);
		}

		public static int MaxContinued(DateTime t1, DateTime t2)
		{
			int num = (t2 - t1).Days;
			if (num == 0)
			{
				num = t2.Day - t1.Day;
			}
			return num;
		}

		public static bool IsSign(int userID)
		{
			System.Data.DataTable dataTable = UserSignHelper.SignInfoByUser(userID);
			bool result;
			if (dataTable.Rows.Count < 1)
			{
				result = true;
			}
			else
			{
				DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0]["SignDay"]);
				result = !(DateTime.Now.ToString("yyyyMMdd") == dateTime.ToString("yyyyMMdd"));
			}
			return result;
		}

		public static int USign(int userID)
		{
			System.Data.DataTable dataTable = UserSignHelper.SignInfoByUser(userID);
			UserSign userSign = new UserSign();
			int result;
			if (dataTable.Rows.Count < 1)
			{
				userSign.UserID = userID;
				userSign.Continued = 1;
				UserSignHelper.InsertUserSign(userSign);
			}
			else
			{
				userSign.ID = Convert.ToInt32(dataTable.Rows[0]["ID"]);
				userSign.SignDay = DateTime.Now;
				userSign.UserID = Convert.ToInt32(dataTable.Rows[0]["UserID"]);
				userSign.Continued = Convert.ToInt32(dataTable.Rows[0]["Continued"]);
				if (UserSignHelper.MaxContinued(Convert.ToDateTime(dataTable.Rows[0]["SignDay"]), userSign.SignDay) == 1)
				{
					userSign.Continued++;
				}
				else if (UserSignHelper.MaxContinued(Convert.ToDateTime(dataTable.Rows[0]["SignDay"]), userSign.SignDay) == 0)
				{
					result = -1;
					return result;
				}
			}
			int num = UserSignHelper.AddPoint(userSign);
			UserSignHelper.UpdateUserSign(userSign);
			result = num;
			return result;
		}

		public static int AddPoint(UserSign us)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			int result;
			if (!masterSettings.sign_score_Enable)
			{
				result = 0;
			}
			else
			{
				IntegralDetailInfo integralDetailInfo = new IntegralDetailInfo();
				integralDetailInfo.IntegralSourceType = 1;
				integralDetailInfo.IntegralSource = "签到";
				integralDetailInfo.Userid = us.UserID;
				integralDetailInfo.IntegralChange = masterSettings.SignPoint;
				integralDetailInfo.IntegralStatus = Convert.ToInt32(IntegralDetailStatus.SignToIntegral);
				if (masterSettings.sign_score_Enable)
				{
					if (us.Continued > masterSettings.SignWhere || us.Continued == masterSettings.SignWhere)
					{
						integralDetailInfo.IntegralChange += masterSettings.SignWherePoint;
						us.Continued = 0;
					}
				}
				IntegralDetailHelp.AddIntegralDetail(integralDetailInfo, null);
				result = Convert.ToInt32(integralDetailInfo.IntegralChange);
			}
			return result;
		}
	}
}
