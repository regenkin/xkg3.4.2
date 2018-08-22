using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public interface IButton : IText
	{
		event EventHandler Click;

		event CommandEventHandler Command;

		AttributeCollection Attributes
		{
			get;
		}

		string CommandArgument
		{
			get;
			set;
		}

		string CommandName
		{
			get;
			set;
		}

		bool CausesValidation
		{
			get;
			set;
		}
	}
}
