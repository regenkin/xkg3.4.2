using System;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	public interface IText
	{
		bool Visible
		{
			get;
			set;
		}

		string Text
		{
			get;
			set;
		}

		Control Control
		{
			get;
		}
	}
}
