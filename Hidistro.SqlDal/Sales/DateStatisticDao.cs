using Hidistro.Core;
using Hidistro.Entities.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Hidistro.SqlDal.Sales
{
	public class DateStatisticDao
	{
		private Database database;

		public DateStatisticDao()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}

		public System.Data.DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = DateTime.Now.AddDays(-6.0);
				DateTime dateTime2 = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
				DateTime now = DateTime.Now;
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime2);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, now);
				decimal allSalesTotal = 0m;
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				if (obj != null)
				{
					allSalesTotal = Convert.ToDecimal(obj);
				}
				System.Data.DataTable dataTable = this.CreateTable();
				for (int i = 0; i < 7; i++)
				{
					DateTime dateTime3 = DateTime.Now.AddDays((double)(-(double)i));
					decimal daySaleTotal = this.GetDaySaleTotal(dateTime3.Year, dateTime3.Month, dateTime3.Day, saleStatisticsType);
					this.InsertToTable(dataTable, dateTime3.Day, daySaleTotal, allSalesTotal);
				}
				result = dataTable;
			}
			return result;
		}

		public decimal GetDaySaleTotal(int year, int month, int day, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, month, day);
				DateTime dateTime2 = dateTime.AddDays(1.0);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}

		public decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, month, 1);
				DateTime dateTime2 = dateTime.AddMonths(1);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}

		public System.Data.DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
				System.Data.DataTable dataTable = this.CreateTable();
				decimal monthSaleTotal = this.GetMonthSaleTotal(year, month, saleStatisticsType);
				int dayCount = this.GetDayCount(year, month);
				int num = (year == DateTime.Now.Year && month == DateTime.Now.Month) ? DateTime.Now.Day : dayCount;
				for (int i = 1; i <= num; i++)
				{
					DateTime dateTime = new DateTime(year, month, i);
					DateTime dateTime2 = dateTime.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", dateTime);
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", dateTime2);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					decimal salesTotal = (obj == null) ? 0m : Convert.ToDecimal(obj);
					this.InsertToTable(dataTable, i, salesTotal, monthSaleTotal);
				}
				result = dataTable;
			}
			return result;
		}

		private int GetDayCount(int year, int month)
		{
			int result;
			if (month == 2)
			{
				if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
				{
					result = 29;
				}
				else
				{
					result = 28;
				}
			}
			else if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
			{
				result = 31;
			}
			else
			{
				result = 30;
			}
			return result;
		}

		public decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, 1, 1);
				DateTime dateTime2 = dateTime.AddYears(1);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}

		public System.Data.DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
				System.Data.DataTable dataTable = this.CreateTable();
				int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
				for (int i = 1; i <= num; i++)
				{
					DateTime dateTime = new DateTime(year, i, 1);
					DateTime dateTime2 = dateTime.AddMonths(1);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", dateTime);
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", dateTime2);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					decimal salesTotal = (obj == null) ? 0m : Convert.ToDecimal(obj);
					decimal yearSaleTotal = this.GetYearSaleTotal(year, saleStatisticsType);
					this.InsertToTable(dataTable, i, salesTotal, yearSaleTotal);
				}
				result = dataTable;
			}
			return result;
		}

		private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (saleStatisticsType)
			{
			case SaleStatisticsType.SaleCounts:
				stringBuilder.Append("SELECT COUNT(OrderId) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				break;
			case SaleStatisticsType.SaleTotal:
				stringBuilder.Append("SELECT Isnull(SUM(OrderTotal),0)");
				stringBuilder.Append(" FROM Hishop_orders WHERE  (OrderDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				break;
			case SaleStatisticsType.Profits:
				stringBuilder.Append("SELECT IsNull(SUM(OrderProfit),0) FROM Hishop_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
				break;
			}
			return stringBuilder.ToString();
		}

		private void InsertToTable(System.Data.DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
		{
			System.Data.DataRow dataRow = table.NewRow();
			dataRow["Date"] = date;
			dataRow["SaleTotal"] = salesTotal;
			if (allSalesTotal != 0m)
			{
				dataRow["Percentage"] = salesTotal / allSalesTotal * 100m;
			}
			else
			{
				dataRow["Percentage"] = 0;
			}
			dataRow["Lenth"] = (decimal)dataRow["Percentage"] * 4m;
			table.Rows.Add(dataRow);
		}

		private System.Data.DataTable CreateTable()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.DataColumn column = new System.Data.DataColumn("Date", typeof(int));
			System.Data.DataColumn column2 = new System.Data.DataColumn("SaleTotal", typeof(decimal));
			System.Data.DataColumn column3 = new System.Data.DataColumn("Percentage", typeof(decimal));
			System.Data.DataColumn column4 = new System.Data.DataColumn("Lenth", typeof(decimal));
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			return dataTable;
		}

		public IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
		{
			IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate) AS UserAdd ");
			this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
			this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
			DateTime date = default(DateTime);
			DateTime dateTime = default(DateTime);
			if (days.HasValue)
			{
				date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double)(-(double)days.Value));
			}
			else if (year.HasValue && month.HasValue)
			{
				date = new DateTime(year.Value, month.Value, 1);
			}
			else if (year.HasValue && !month.HasValue)
			{
				date = new DateTime(year.Value, 1, 1);
			}
			if (days.HasValue)
			{
				for (int i = 1; i <= days; i++)
				{
					UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
					if (i > 1)
					{
						date = dateTime;
					}
					dateTime = date.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
					userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
					userStatisticsForDate.TimePoint = date.Day;
					list.Add(userStatisticsForDate);
				}
			}
			else if (year.HasValue && month.HasValue)
			{
				int num = DateTime.DaysInMonth(year.Value, month.Value);
				for (int i = 1; i <= num; i++)
				{
					UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
					if (i > 1)
					{
						date = dateTime;
					}
					dateTime = date.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
					userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
					userStatisticsForDate.TimePoint = i;
					list.Add(userStatisticsForDate);
				}
			}
			else if (year.HasValue && !month.HasValue)
			{
				int num2 = 12;
				for (int i = 1; i <= num2; i++)
				{
					UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
					if (i > 1)
					{
						date = dateTime;
					}
					dateTime = date.AddMonths(1);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
					userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
					userStatisticsForDate.TimePoint = i;
					list.Add(userStatisticsForDate);
				}
			}
			return list;
		}

		public decimal GetAddUserTotal(int year)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM aspnet_Members WHERE CreateDate BETWEEN @StartDate AND @EndDate)  AS UserAdd");
			DateTime dateTime = new DateTime(year, 1, 1);
			DateTime dateTime2 = dateTime.AddYears(1);
			this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
			this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return (obj == null) ? 0m : Convert.ToDecimal(obj);
		}
	}
}
