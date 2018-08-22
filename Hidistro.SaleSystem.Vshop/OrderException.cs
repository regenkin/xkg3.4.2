using Hidistro.Core.Exceptions;
using System;

namespace Hidistro.SaleSystem.Vshop
{
	public class OrderException : HiException
	{
		public OrderException()
		{
		}

		public OrderException(string message) : base(message)
		{
		}
	}
}
