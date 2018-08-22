<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ArticleShow.aspx.cs" Inherits="Hidistro.UI.Web.Vshop.ArticleShow" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<meta charset="utf-8" /><meta name="viewport" content="width=device-width, initial-scale=1" /><meta name="renderer" content="webkit" /><link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css" />
<script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
<script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
   <link rel="stylesheet" href="/admin/css/common.css" />
    <style type="text/css">
        .mate-img.settitle .title {
            position: absolute;left: 0;bottom: 0;width: 100%;height: 40px;line-height: 40px;background-color: rgba(0,0,0,0.5);filter: progid:DXImageTransform.Microsoft.gradient(startColorstr=#55000000, endColorstr=#55000000);
padding-left: 10px;color: #fff;
        }
        .mate-inner.top .mate-img{
            position:relative;
            margin-bottom:15px;
        }
        .mate-inner {
            border-top: 1px solid #DDD;
            padding: 15px;
        }
    </style>
</head>
<body>
    <%if(ArticleType=="news"){ %>
   <div class="mobile mate-list" style="width:auto;max-width: 640px; margin: 0px auto;">
       <div class="mate-inner" onclick="location.href='<%=htmlUrl %>'" style="cursor:pointer;">
           <span><%=htmlPubTime %></span>
               <h3 id="singelTitle"><%=htmlTitle %></h3>
               <div class="mate-img"<%-- style="width:320px;height:145px;overflow:hidden"--%>>
                           <img id="img1" src="<%=htmlImageUrl %>" class="img-responsive">

               </div>
               <div class="mate-info" id="Lbmsgdesc"><%=htmlMemo %></div>
               <div class="red-all clearfix">
                           <strong class="fl">阅读原文</strong>
                           <em class="fr">&gt;</em>

               </div></div>
   </div>
    <%}else{ %>
    <div style="max-width: 640px; margin: 0px auto;">
    <div class="mate-inner top" onclick="location.href='<%=htmlUrl %>'" style="cursor:pointer;">
    <div class="mate-img settitle">
        <img src="<%=htmlImageUrl %>" class="img-responsive">
        <div class="title"><%=htmlTitle %></div>
    </div>
</div>
        <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
<div class="mate-inner">
    <div class="child-mate" onclick="location.href='<%#Eval("url") %>'" style="cursor:pointer;">
        <div class="child-mate-title clearfix">
            <div class="title" style="width: 70%;">
                <h4><%#Eval("Title") %></h4>
            </div>
            <div class="img">
                <img src="<%#Eval("ImageUrl") %>" class="img-responsive">
            </div>
        </div>
    </div>
</div></ItemTemplate>
        </asp:Repeater>
        </div>
    <%} %>
    
    <script type="text/javascript">
        $(document).ready(function () {
            setTimeout(function () {
                if (parent) {
                    if (parent.CallBack_MobileFramMain) {
                        var h = $(document.body).outerHeight(true);
                        if (h < 100) {
                            h = $(document.body).height();
                        }
                        if (h < 100) {
                            h = 500;
                        }
                        parent.CallBack_MobileFramMain(h)
                    }
                }
            }, 500);
            
        })
        </script>
</body>
</html>
