using System;
using System.Globalization;

namespace Hidistro.UI.Common.Controls
{
	public class SkinNotFoundException : Exception
	{
		private string message;

		public override string Message
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "没有找到指定的样式文件：{0}", new object[]
				{
					this.message
				});
			}
		}

		public SkinNotFoundException()
		{
		}

		public SkinNotFoundException(string message) : base(message)
		{
			this.message = message;
		}
	}
}
