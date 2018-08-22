<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetArticles.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.Shop.GetArticles" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<div class="page-header">
                    <h2>图文素材管理</h2>
                </div>
                <div class="btn-search">
                    <div class="form-inline">
                        <div class="form-group">
                            <a class="btn btn-success resetSize" href="articlesedit.aspx">新建单条图文</a>
                        </div>
                        <div class="form-group">
                            <a class="btn btn-success resetSize" href="multiarticlesedit.aspx">新建多条图文</a>
                        </div>
                        <div class="form-group">
                            <i class="glyphicon glyphicon-search search"></i>
                            <input type="text" class="form-control resetSize paddingl" id="txtKey" placeholder="按回车键搜索" value="<%=ArticleTitle %>" onkeydown="return searchClick(event)">
                        </div>
                    </div>
                </div>
                <div class="mate-tabl" id="articleContainer">
                    <ul class="nav nav-tabs">
                        <li role="presentation" <%if(articletype==0){ %>class="active"<%} %>><a href="articles.aspx<%if(!string.IsNullOrEmpty(ArticleTitle)){%>?key=<%=Server.UrlEncode(ArticleTitle) %><%} %>">所有素材</a></li>
                        <li role="presentation" <%if(articletype==1){ %>class="active"<%} %>><a href="articles.aspx?articletype=1<%if(!string.IsNullOrEmpty(ArticleTitle)){%>&key=<%=Server.UrlEncode(ArticleTitle) %><%} %>">分销商分享素材</a></li>
                        <li role="presentation" <%if(articletype==2){ %>class="active"<%} %>><a href="articles.aspx?articletype=2<%if(!string.IsNullOrEmpty(ArticleTitle)){%>&key=<%=Server.UrlEncode(ArticleTitle) %><%} %>">单条图文</a></li>
                        <li role="presentation" <%if(articletype==4){ %>class="active"<%} %>><a href="articles.aspx?articletype=4<%if(!string.IsNullOrEmpty(ArticleTitle)){%>&key=<%=Server.UrlEncode(ArticleTitle) %><%} %>">多条图文</a></li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active" id="home">
                            <p class="mateleng">图文素材列表：(共<span><%=recordcount %></span>条)</p>
                            <div class="loading"></div>
                            <div class="mate-all clearfix">
                                <asp:Repeater ID="rptList" runat="server">
                                    <ItemTemplate><%#FormatArticleShow(Eval("ArticleID"),Eval("ArticleType"),Eval("Title"),Eval("PubTime"),Eval("ImageUrl"),Eval("Memo"),Eval("IsShare")) %>
                              </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </div>
                    </div>
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="false" ID="pager" DefaultPageSize="9" />
                </div>
            </div>
        </div>
                    </div>