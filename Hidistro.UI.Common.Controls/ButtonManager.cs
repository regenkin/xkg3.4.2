using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public sealed class ButtonManager
	{
		private ButtonManager()
		{
		}

		public static IButton CreateButton(Button button)
		{
			if (button == null)
			{
				throw new ArgumentNullException("button", "The button parameter can not be null");
			}
			return new ButtonWrapper(button);
		}

		public static IButton CreateLinkButton(LinkButton button)
		{
			if (button == null)
			{
				throw new ArgumentNullException("button", "The button parameter can not be null");
			}
			return new LinkButtonWrapper(button);
		}

		public static IButton CreateImageLinkButton(ImageLinkButton button)
		{
			if (button == null)
			{
				throw new ArgumentNullException("button", "The button parameter can not be null");
			}
			return new ImageLinkButtonWrapper(button);
		}

		public static IButton Create(Control cntrl)
		{
			if (cntrl == null)
			{
				return null;
			}
			IButton button = cntrl as IButton;
			if (button == null)
			{
				if (cntrl is Button)
				{
					button = ButtonManager.CreateButton(cntrl as Button);
				}
				else if (cntrl is ImageLinkButton)
				{
					button = ButtonManager.CreateImageLinkButton(cntrl as ImageLinkButton);
				}
				else if (cntrl is LinkButton)
				{
					button = ButtonManager.CreateLinkButton(cntrl as LinkButton);
				}
			}
			return button;
		}
	}
}
