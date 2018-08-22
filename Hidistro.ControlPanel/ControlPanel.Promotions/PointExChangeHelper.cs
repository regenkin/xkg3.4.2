using Hidistro.Entities.Promotions;
using Hidistro.SqlDal.Promotions;
using System;
using System.Data;

namespace ControlPanel.Promotions
{
	public class PointExChangeHelper
	{
		private static PointExChangeDao _exchange = new PointExChangeDao();

		public static System.Data.DataTable GetProducts(int exchangeId)
		{
			return PointExChangeHelper._exchange.GetProducts(exchangeId);
		}

		public static System.Data.DataTable GetProducts(int exchangeId, int pageNumber, int maxNum, out int total, string sort, string order)
		{
			return PointExChangeHelper._exchange.GetProducts(exchangeId, pageNumber, maxNum, out total, sort, order == "asc");
		}

		public static bool DeleteProduct(int exchangeId, int productId)
		{
			return PointExChangeHelper._exchange.DeleteProduct(exchangeId, productId);
		}

		public static PointExchangeProductInfo GetProductInfo(int exchangeId, int productId)
		{
			return PointExChangeHelper._exchange.GetProductInfo(exchangeId, productId);
		}

		public static bool InsertProduct(PointExchangeProductInfo product)
		{
			return PointExChangeHelper._exchange.InsertProduct(product);
		}

		public static bool UpdateProduct(PointExchangeProductInfo product)
		{
			return PointExChangeHelper._exchange.UpdateProduct(product);
		}

		public static bool UpdateProduct(int exchangeId, int productId)
		{
			return PointExChangeHelper._exchange.DeleteProduct(exchangeId, productId);
		}

		public static int Create(PointExChangeInfo exchange, ref string msg)
		{
			return PointExChangeHelper._exchange.Create(exchange, ref msg);
		}

		public static bool Update(PointExChangeInfo exchange, ref string msg)
		{
			return PointExChangeHelper._exchange.Update(exchange, ref msg);
		}

		public static PointExChangeInfo Get(int Id)
		{
			return PointExChangeHelper._exchange.GetExChange(Id);
		}

		public static bool Delete(int Id)
		{
			return PointExChangeHelper._exchange.Delete(Id);
		}

		public static System.Data.DataTable Query(ExChangeSearch search, ref int total)
		{
			return PointExChangeHelper._exchange.Query(search, ref total);
		}

		public static bool SetProductsStatus(int exchangeId, int status, string productIds)
		{
			return PointExChangeHelper._exchange.SetProductsStatus(exchangeId, status, productIds);
		}

		public static bool DeleteProducts(int exchangeId, string productIds)
		{
			return PointExChangeHelper._exchange.DeleteProducts(exchangeId, productIds);
		}

		public static bool EditProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
		{
			return PointExChangeHelper._exchange.EditProducts(exchangeId, productIds, pNumbers, points, eachNumbers);
		}

		public static bool AddProducts(int exchangeId, string productIds, string pNumbers, string points, string eachNumbers)
		{
			return PointExChangeHelper._exchange.AddProducts(exchangeId, productIds, pNumbers, points, eachNumbers);
		}

		public static int GetProductExchangedCount(int exchangeId, int productId)
		{
			return PointExChangeHelper._exchange.GetProductExchangedCount(exchangeId, productId);
		}

		public static int GetUserProductExchangedCount(int exchangeId, int productId, int userId)
		{
			return PointExChangeHelper._exchange.GetUserProductExchangedCount(exchangeId, productId, userId);
		}
	}
}
