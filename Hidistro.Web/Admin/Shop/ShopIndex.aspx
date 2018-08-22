<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShopIndex.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.Shop.ShopIndex" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <form id="thisForm" runat="server" class="form-horizontal">
                <div class="page-header">
                    <h2>店铺主页</h2>
                </div>
                <h3 class="templateTitle">当前使用的模板</h3>
                <div class="set-switch clearfix">
                    <div class="usertemplate">
                        <img src="<%= GetImgName(tempLatePath) %>">
                    </div>
                    <div class="usertemplateInfo">
                        <div class="qrCode">
                            <img src="http://s.jiathis.com/qrcode.php?url=<%= showUrl %>">
                        </div>
                        <p class="mb20">手机扫描此二维码，可直接在手机上访问店铺</p>
                        <p class="mb20">店铺网址：<br><%=showUrl %></p>
                        <p class="mb20">模板名称：<%=templateCuName %></p>
                        <input type ="button" class="btn btn-primary mt20" value ="编辑模板" dataID="<%= GetTempUrl(tempLatePath)%>" id ="btn_edit"/>
                      
                        <div class="linkCopy">
                             <input type ="button" class="btn btn-primary inputw100 mb10" style="display: block;" id="btn_show" value ="预览" />
                             <input type ="button" class="btn btn-primary inputw100" id="btn_copy" value ="复制网址" />
                          
                        </div>
                    </div>
                </div>
                <h3 class="templateTitle">可选用的模板</h3>
                <div class="templateList">
                    <ul class="clearfix">
                         <asp:Repeater ID="Repeater1" runat="server">
                  <ItemTemplate> 
                        <li class="<%# Eval("ThemeName").ToString ()==tempLatePath ?  "active" :""  %>">
                           <div class="img">
                                <div>
                                    <img src="<%# GetImgName(  Eval("ThemeName").ToString ()) %>">
                                </div>
                           </div>
                           <div class="lightBtn">
                                <div style="display: none;" class="enableExit">
                                    <input type ="button" class="btn btn-sm btn-success"  value ="启用" dataID="<%# Eval("ThemeName") %>" >
                                     <input type ="button" class="btn btn-sm btn-primary" value ="编辑" dataID="<%#   GetTempUrl(Eval("ThemeName").ToString ()) %>" >
                                    
                                </div>
                           </div>
                           <p class="templateUser"><%#  Eval("Name").ToString () %></p>
                        </li>
                
                         </ItemTemplate> 
                         </asp:Repeater>
                    </ul>
                </div>
                </form>
    <script type="text/javascript" src="/admin/js/ZeroClipboard.min.js"></script>
     <script type="text/javascript">
         $(function () {
             
             init();
        

             $(".btn-success").click(function () {          
                 if (ShopIndex.EnableTemp($(this).attr("dataID")).value) {
                     ShowMsgAndReUrl("设置成功", true, "/admin/shop/ShopIndex.aspx", null);
                 }
             });

             $("#btn_show").click(function () {
                 window.open ("/Default.aspx");
             });
             $("#btn_edit").click(function () {
                 window.location = $(this).attr("dataID");
             });

             $(".lightBtn .btn-primary").click(function () {
                 window.location = $(this).attr("dataID");
             });

             $('.templateList ul li').hover(function () {
                 $(this).find('.enableExit').show();
             }, function () {
                 $(this).find('.enableExit').hide();
             });

             
         });

         var copy;
         function init() {
              copy = new ZeroClipboard(document.getElementById("btn_copy"), {
                 moviePath: "/admin/js/ZeroClipboard.swf"
             });
             copy.setHandCursor(true); //设置手型  
             copy.addEventListener('mouseDown', function (client) {  //创建监听  
                 copyUrl(); //设置需要复制的代码  
             });
             copy.on('complete', function (client, args) {
                 HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
             });
         }

         function copyUrl() {
             copy.setText("http://"+window.location.host+"/default.aspx");
         }
    </script>
</asp:Content>
