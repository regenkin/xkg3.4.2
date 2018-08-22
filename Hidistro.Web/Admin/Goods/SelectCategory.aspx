<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="SelectCategory.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.SelectCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/selectcategory.css" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>上架新商品</h2>
    </div>
    <div class="play-tabs">
        <ul class="nav nav-tabs speedOfProgress"<%-- role="tablist"--%>>
            <li role="presentation" class="active complete"><a aria-expanded="true" href="#shopclass" aria-controls="shopclass" role="tab" data-toggle="tab">1.选择商品分类</a></li>
            <li class="" role="presentation"><a <%if (productId > 0) {%>href="productedit.aspx?categoryId=<%=categoryid %>&productId=<%=productId %>&reurl=<%=Server.UrlEncode(reurl) %>"<%} %>>2.编辑商品信息</a></li>
            <li class="" role="presentation"><a <%if (productId > 0) {%>href="productedit.aspx?categoryId=<%=categoryid %>&productId=<%=productId %>&isnext=1&reurl=<%=Server.UrlEncode(reurl) %>"<%} %>>3.编辑商品详情</a></li>
        </ul>
        <div class="tab-content">
            <div class="dataarea mainwidth td_top_ccc">
                <div class="results">
                    <div class="results_main">
                         <div class="results_left">
                <label>
                    <input type="button" name="button2" value="" class="search_left" />
                </label>
            </div>
                        <div class="results_pos">
                            <ol class="results_ol">
                            </ol>
                        </div>
                          <div class="results_right">
                <label>
                    <input type="button" name="button2" value="" class="search_right" />
                </label>
            </div>
                    </div>
                </div>
                <%--   <div class="results_img"></div>--%>


                <div class="panel panel-info mt5">
                    <div class="panel-heading">
                        <h3 class="panel-title">你当前选择的是：<span id="fullName"></span></h3>
                    </div>
                </div>
            </div>
            <%-- <div class="databottom"></div>--%>
            <div class="alignc">
                <button id="btnNext" class="btn btn-success btn-sm">已选好分类，进入下一步 »</button>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="publish.helper.js"></script>
</asp:Content>
