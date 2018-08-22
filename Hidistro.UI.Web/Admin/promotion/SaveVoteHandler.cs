using ControlPanel.Promotions;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;
using System.Web;

namespace Hidistro.UI.Web.Admin.promotion
{
	public class SaveVoteHandler : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public void ProcessRequest(System.Web.HttpContext context)
		{
			context.Response.ContentType = "text/plain";
			try
			{
				int num = int.Parse(context.Request["id"].ToString());
				string text = context.Request["name"].ToString();
				string val = context.Request["begin"].ToString();
				string val2 = context.Request["end"].ToString();
				string memberGrades = context.Request["memberlvl"].ToString();
				string defualtGroup = context.Request["defualtgroup"].ToString();
				string customGroup = context.Request["customgroup"].ToString();
				string text2 = context.Request["img"].ToString();
				string text3 = context.Request["des"].ToString();
				bool isMultiCheck = bool.Parse(context.Request["ismulti"].ToString());
				int maxCheck = int.Parse(context.Request["maxcheck"].ToString());
				string text4 = context.Request["items"].ToString();
				System.DateTime now = System.DateTime.Now;
				System.DateTime now2 = System.DateTime.Now;
				if (string.IsNullOrEmpty(text))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"投票标题不能为空\"}");
				}
				else if (text.Length > 60)
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"投票标题不能超过60个字符\"}");
				}
				else if (string.IsNullOrEmpty(text4))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"投票选项不能为空\"}");
				}
				else if (string.IsNullOrEmpty(text2))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"没有上传活动封面\"}");
				}
				else if (!val.bDate(ref now))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的开始时间\"}");
				}
				else if (!val2.bDate(ref now2))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"请输入正确的结束时间\"}");
				}
				else if (now2 <= now)
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"结束时间要大于开始时间\"}");
				}
				else if (string.IsNullOrEmpty(text3))
				{
					context.Response.Write("{\"type\":\"error\",\"data\":\"活动说明不能为空\"}");
				}
				else
				{
					VoteInfo voteInfo = new VoteInfo();
					if (num != 0)
					{
						voteInfo = VoteHelper.GetVote((long)num);
						if (voteInfo == null)
						{
							context.Response.Write("{\"type\":\"error\",\"data\":\"没有找到这个调查\"}");
							return;
						}
					}
					System.Collections.Generic.List<VoteItemInfo> list = new System.Collections.Generic.List<VoteItemInfo>();
					if (!string.IsNullOrEmpty(text4))
					{
						string[] array = text4.Split(new char[]
						{
							','
						});
						if (array.Length > 0)
						{
							for (int i = 0; i < array.Length; i++)
							{
								VoteItemInfo voteItemInfo = new VoteItemInfo();
								if (num > 0)
								{
									voteItemInfo.VoteId = (long)num;
								}
								voteItemInfo.ItemCount = 0;
								voteItemInfo.VoteItemName = array[i];
								list.Add(voteItemInfo);
							}
						}
					}
					voteInfo.VoteName = text;
					voteInfo.EndDate = now2;
					voteInfo.StartDate = now;
					voteInfo.MemberGrades = memberGrades;
					voteInfo.DefualtGroup = defualtGroup;
					voteInfo.CustomGroup = customGroup;
					voteInfo.Description = text3;
					voteInfo.ImageUrl = text2;
					voteInfo.IsMultiCheck = isMultiCheck;
					voteInfo.MaxCheck = maxCheck;
					voteInfo.VoteItems = list;
					long num2;
					if (num == 0)
					{
						num2 = VoteHelper.Create(voteInfo);
					}
					else
					{
						num2 = voteInfo.VoteId;
						if (!VoteHelper.Update(voteInfo, true))
						{
							num2 = 0L;
						}
					}
					if (num2 > 0L)
					{
						context.Response.Write("{\"type\":\"success\",\"data\":\"" + num2.ToString() + "\"}");
					}
					else
					{
						context.Response.Write("{\"type\":\"error\",\"data\":\"写数据库出错\"}");
					}
				}
			}
			catch (System.Exception ex)
			{
				context.Response.Write("{\"type\":\"error\",\"data\":\"" + ex.Message + "\"}");
			}
		}

		private string IntToChar(int i)
		{
			return ((char)(i + 67)).ToString();
		}
	}
}
