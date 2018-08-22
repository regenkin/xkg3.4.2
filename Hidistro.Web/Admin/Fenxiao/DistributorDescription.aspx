<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorDescription.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorDescription" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
                    <h2>分销设置</h2>
    </div>


      <form id="thisForm" runat="server" class="form-horizontal" >
        <div class="mate-tabl" id="mytab">
                    <ul class="nav nav-tabs" role="tablist" >
                        <li role="presentation" class="active"><a href="DistributorApplySet.aspx?tabnum=0" >基本设置</a></li>
                        <li role="presentation" class=""><a  href="DistributorApplySet.aspx?tabnum=1" >招募细则</a></li>
                         <li role="presentation" class=""><a href="#fkContent" aria-controls="fkContent"  role="tab" data-toggle="tab"  >分销说明</a></li>
                    </ul>
                    <div class="tab-content">
                        
                        <div role="tabpanel" class="tab-pane active" id="profile">-</div>
                        <div role="tabpanel" class="tab-pane " id="messages">--</div>

                        <div role="tabpanel" class="tab-pane active" id="fkContent">
                           
                              <div class="edit-text clearfix">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">分销说明</div>
                                    </div>
                                    <div class="upshop-view">
                                        <div class="img-info">
                                            <p>基本信息区</p>
                                            <p>固定样式，显示商品主图、价格等信息</p>
                                        </div>
                                        <div class="exit-shop-info" id="fkContentShow">
                                            内容区
                                        </div>
                                    </div>
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                        <div class="edit-text-right">
                            <div class="edit-inner">
                                <Kindeditor:KindeditorControl  ID="htmlfkContent"  runat="server" Height="300" Width="570" />
                            </div>
                          
                        </div>
                    </div>



                         <div class="footer-btn navbar-fixed-bottom">
                         <asp:Button runat="server" ID="Button2" Text="保存" OnClientClick="return checkfkContent();"  OnClick="btnSave_fkContent" CssClass="btn btn-success inputw100" />
                          </div>

                        </div>

     </div>
  </div>

    </form>






    <script>

        $(function () {
           
            /*编辑器监听事件*/
            um.addListener('ready', function (editor) {
                $("#fkContentShow").html(um.getContent());
            });
            um.addListener('selectionchange', function () {
                $("#fkContentShow").html(um.getContent());
            });


            var tabnum =2;
            $("#mytab li a").eq(tabnum).tab("show");

        });

        function checkfkContent() {

            if (um.getContent() == "") {
                HiTipsShow("分销说明内容不能为空", 'error');
                return false;
            };

        }

       </script>

</asp:Content>
