<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManageNineImages.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.ManageNineImages" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ninefigureoneinfoimg{
            width:210px;border:1px solid #bbb;border-radius:5px;padding:10px;float:left;margin-right:10px;margin-top:10px;position:relative
        }
        .y-imglist li{width:60px!important;height:60px!important}
        .exitremove{display:none;position: absolute;left: 0px;bottom:5px;height: 30px;width: 100%;background-color: rgba(0, 0, 0, 0.5);line-height: 30px;}
        .exitremove a{text-align:center;width:50%;display:inline-block;color:#fff;cursor:pointer;}
        .exitremove a:hover{color:#ffd800}
        .info{line-height:20px;height:40px;overflow:hidden}
        .search{right: -27px;
top: 5px;
color: #777;}
        .paddingl{padding-left:30px!important}
        .img{border:1px solid #ddd;padding:2px}
    </style>
   <script>

       function searchkey() {
           var key = encodeURIComponent($("#txtKey").val());
           var urlparms = "";
           if ($.trim(key) != "") {
               if (urlparms != "") {
                   urlparms = "" + urlparms + "&key=" + key;
               } else {
                   urlparms = "?key=" + key;
               }
           }
           var url = "ManageNineImages.aspx" + urlparms;
           window.location.href = url;
           return false;
       }

       function searchClick(event) { if (event.keyCode == 13) { searchkey(); return false; } }

       function delImg(id) {
           delId = id;
           HiTipsShow("确定删除该素材？", "confirm", "delSel");

       }

       var delId = 0;
       $(function () {

           $("#delSel").click(function () {
               $.ajax({
                   url: "AddNineImages.aspx",
                   type: 'post',
                   data: { ID: delId, task: "del" },
                   success: function (data) {
                       if (data.indexOf("falid：") == 0) {
                           delId = 0;
                           HiTipsShow(data, "warning");
                           return;
                       }
                       HiTipsShow("删除成功！", "success", function () {
                           window.location.href = window.location.href;
                       });
                   },
                   error: function () {
                       HiTipsShow("访问服务器出错", "warning");
                       share_id = 0;
                   }
               })
           });


           $(".ninefigureoneinfoimg").hover(function () {
               $(this).find(".exitremove").show();
           }, function () {
               $(this).find(".exitremove").hide();
           })
       });

   </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
     <div class="page-header">
 <h2>
九图一文素材管理　<span style="font-size:12px;color:#aaa;">管理九图一文格式的图文素材</span>
</h2> 
    </div>
    <div class="form-inline">
                        <div class="form-group">
                            <a class="btn btn-success resetSize" href="AddNineImages.aspx">新建九图一文素材</a>
                        </div>
                        <div class="form-group">
                            <i class="glyphicon glyphicon-search search"></i>
                            <input class="form-control resetSize paddingl" id="txtKey" placeholder="按回车键搜索" value="" onkeydown="return searchClick(event)" type="text">
                        </div>
  </div>
    <div style="clear:both;position:relative;margin-top:10px;">
        <div style="line-height:30px">九图一文素材列表：（共<asp:Literal ID="NineTotal" Text="0" runat="server"></asp:Literal>条）</div>
        
        <asp:Repeater ID="ShareRep" runat="server">
            <ItemTemplate>

         <div class="ninefigureoneinfoimg">
                                                
                                                <p class="info mb5"><%# Eval("ShareDesc") %></p>

                                                <ul class="clearfix y-imglist">
                                                    <li class="img">
                                                        <img src="<%# Eval("image1").ToString().Length>5?Eval("image1"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image2").ToString().Length>5?Eval("image2"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image3").ToString().Length>5?Eval("image3"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image4").ToString().Length>5?Eval("image4"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image5").ToString().Length>5?Eval("image5"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image6").ToString().Length>5?Eval("image6"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image7").ToString().Length>5?Eval("image7"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image8").ToString().Length>5?Eval("image8"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                    <li class="img">
                                                         <img src="<%# Eval("image9").ToString().Length>5?Eval("image9"):"/Utility/pics/none.gif" %>">
                                                    </li>
                                                </ul>
             <div class="exitremove"> <a href="AddNineImages.aspx?ID=<%# Eval("id")%>" >编辑</a><a href="javascript:delImg(<%# Eval("id")%>)">删除</a></div>
                                            </div>
    
            </ItemTemplate>
        </asp:Repeater>

    </div>
     <div class="page" style="clear:both">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" DefaultPageSize="12" ID="pager"/>
                                </div>
                            </div>
                        </div>
    </div>
    <div id="delSel" style="display:none"></div>
</asp:Content>
