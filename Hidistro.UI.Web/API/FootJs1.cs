using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Vshop;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Hidistro.UI.Web.API
{
	public class FootJs1 : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected void Page_Load(object sender, System.EventArgs e)
		{
			MemberInfo currentMember = MemberProcessor.GetCurrentMember();
			if (currentMember == null)
			{
				base.Response.Write(";");
			}
			else
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				int userId = currentMember.UserId;
				NoticeQuery noticeQuery = new NoticeQuery();
				noticeQuery.SendType = 0;
				noticeQuery.UserId = new int?(userId);
				bool value = DistributorsBrower.GetDistributorInfo(userId) != null;
				noticeQuery.IsDistributor = new bool?(value);
				noticeQuery.IsDel = new int?(0);
				System.Data.DataTable noticeNotReadDt = NoticeBrowser.GetNoticeNotReadDt(noticeQuery);
				if (noticeNotReadDt != null && noticeNotReadDt.Rows.Count > 0)
				{
					System.Data.DataRow[] array = noticeNotReadDt.Select("SendType='0'");
					if (array != null)
					{
						stringBuilder.Append("$('.my-message').html('<i></i>');");
					}
					else
					{
						stringBuilder.Append("$('.my-message').html('<i></i>').attr('href','notice.aspx?type=1');");
					}
					for (int i = 0; i < noticeNotReadDt.Rows.Count; i++)
					{
						if (System.Convert.ToInt32(noticeNotReadDt.Rows[i]["SendType"]) == 0)
						{
							stringBuilder.Append(string.Concat(new object[]
							{
								"$('.new_message ul').append('<li><a href=\"NoticeDetail.aspx?Id=",
								noticeNotReadDt.Rows[i]["Id"],
								"\">",
								noticeNotReadDt.Rows[i]["Title"],
								"</a></li>');"
							}));
						}
						else
						{
							stringBuilder.Append(string.Concat(new object[]
							{
								"$('.new_message ul').append('<li><a  href=\"NoticeDetail.aspx?Id=",
								noticeNotReadDt.Rows[i]["Id"],
								"\">",
								noticeNotReadDt.Rows[i]["Title"],
								"</a></li>');"
							}));
						}
					}
				}
				base.Response.Write(stringBuilder.ToString());
			}
			base.Response.End();
		}
	}
}
