using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public sealed class TextControlHelper
	{
		private TextControlHelper()
		{
		}

		public static IText CreateLiteral(Literal lit)
		{
			return new LiteralWrapper(lit);
		}

		public static IText CreateLabel(Label label)
		{
			return new LabelWrapper(label);
		}

		public static IText Create(Control cntrl)
		{
			if (cntrl == null)
			{
				return null;
			}
			IText text = cntrl as IText;
			if (text == null)
			{
				if (cntrl is Literal)
				{
					text = new LiteralWrapper(cntrl as Literal);
				}
				else if (cntrl is Label)
				{
					text = new LabelWrapper(cntrl as Label);
				}
			}
			return text;
		}
	}
}
