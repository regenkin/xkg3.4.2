using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;

namespace Hidistro.UI.Common.Controls
{
	[ParseChildren(true), PersistChildren(false)]
	public abstract class SimpleTemplatedWebControl : TemplatedWebControl
	{
		protected int referralId;

		private string skinName;

		protected virtual string SkinPath
		{
			get
			{
				return Globals.ApplicationPath + "/Templates/common/" + this.skinName;
			}
		}

		public virtual string SkinName
		{
			get
			{
				return this.skinName;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					return;
				}
				value = value.ToLower(CultureInfo.InvariantCulture);
				if (!value.EndsWith(".html"))
				{
					return;
				}
				this.skinName = value;
			}
		}

		private bool SkinFileExists
		{
			get
			{
				return !string.IsNullOrEmpty(this.SkinName);
			}
		}

		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (this.LoadHtmlThemedControl())
			{
				this.AttachChildControls();
				return;
			}
			throw new SkinNotFoundException(this.SkinPath);
		}

		protected bool LoadHtmlThemedControl()
		{
			string text = this.ControlText();
			if (!string.IsNullOrEmpty(text))
			{
				Control control = this.Page.ParseControl(text);
				control.ID = "_";
				this.Controls.Add(control);
				return true;
			}
			return false;
		}

		private string ControlText()
		{
			if (!this.SkinFileExists)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(File.ReadAllText(this.Page.Request.MapPath(this.SkinPath), Encoding.UTF8));
			if (stringBuilder.Length == 0)
			{
				return null;
			}
			stringBuilder.Replace("<%", "").Replace("%>", "");
			string vshopSkinPath = Globals.GetVshopSkinPath(null);
			stringBuilder.Replace("/images/", vshopSkinPath + "/images/");
			stringBuilder.Replace("/script/", vshopSkinPath + "/script/");
			stringBuilder.Replace("/style/", vshopSkinPath + "/style/");
			stringBuilder.Replace("/utility/", Globals.ApplicationPath + "/utility/");
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"UI\" Namespace=\"ASPNET.WebControls\" Assembly=\"ASPNET.WebControls\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.Common.Controls\" Assembly=\"Hidistro.UI.Common.Controls\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Register TagPrefix=\"Hi\" Namespace=\"Hidistro.UI.SaleSystem.Tags\" Assembly=\"Hidistro.UI.SaleSystem.Tags\" %>" + Environment.NewLine);
			stringBuilder.Insert(0, "<%@ Control Language=\"C#\" %>" + Environment.NewLine);
			MatchCollection matchCollection = Regex.Matches(stringBuilder.ToString(), "href(\\s+)?=(\\s+)?\"url:(?<UrlName>.*?)(\\((?<Param>.*?)\\))?\"", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			for (int i = matchCollection.Count - 1; i >= 0; i--)
			{
				int num = matchCollection[i].Groups["UrlName"].Index - 4;
				int num2 = matchCollection[i].Groups["UrlName"].Length + 4;
				if (matchCollection[i].Groups["Param"].Length > 0)
				{
					num2 += matchCollection[i].Groups["Param"].Length + 2;
				}
				stringBuilder.Remove(num, num2);
				stringBuilder.Insert(num, Globals.GetSiteUrls().UrlData.FormatUrl(matchCollection[i].Groups["UrlName"].Value.Trim(), new object[]
				{
					matchCollection[i].Groups["Param"].Value
				}));
			}
			return stringBuilder.ToString();
		}
	}
}
