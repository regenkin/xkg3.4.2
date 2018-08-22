using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class HiLiteral : Literal, IText
	{
		public Control Control
		{
			get
			{
				return this;
			}
		}
	}
}
