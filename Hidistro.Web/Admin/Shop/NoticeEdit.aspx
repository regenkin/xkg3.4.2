<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="NoticeEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.NoticeEdit" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>创建<%=menuTitle %></h2>
    </div>
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="shop-navigation clearfix">

            <div class="fl">
                <div class="mobile-border">
                    <div class="mobile-d">
                        <div class="mobile-header">
                            <i></i>
                            <div class="mobile-title">标题</div>
                        </div>
                        <div class="upshop-view">
                            <div class="img-info">
                                <p>基本信息区</p>
                            </div>
                            <div class="exit-shop-info" id="fckDescriptionShow">
                                内容区
                            </div>
                        </div>
                        <div class="mobile-footer"></div>
                    </div>
                </div>

            </div>



            <div class="fl frwidth">
                <div class="set-switch resetBorder">
                    <p class="mb10"><strong>标题：</strong></p>
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="50" CssClass="form-control mb5"></asp:TextBox>
                </div>
                <div>
                    <p class="mb10"><strong>正文内容：</strong><small style="display: inline">&nbsp;</small></p>

                    <div class="edit-inner">
                        <Kindeditor:KindeditorControl ID="txtMemo" runat="server" Height="220" Width="635" />
                    </div>

                </div>

                <div class="set-switch resetBorder mt10" style="background-color:#fff;">
                    <p class="mb10"><strong><%=(sendType==1?"消息发送":"公告") %>对象：</strong></p>
                    <p><asp:RadioButtonList ID="rbSendTolist" runat="server" RepeatDirection="Horizontal" Width="300">
                    <asp:ListItem Text="所有用户" Value="0"></asp:ListItem>
                    <asp:ListItem Text="分销商" Value="1"></asp:ListItem>
                    <asp:ListItem Text="指定用户" Value="2"></asp:ListItem>
                    </asp:RadioButtonList><div id="divUserList"></div>
                </p>
                </div>


                <p class="mb10"><strong></strong><small style="display: inline">&nbsp;</small></p>
                
                <div class="footer-btn navbar-fixed-bottom">
                    <button type="button" class="btn btn-success" onclick="return Save()"><%=(sendType==1?"发送":"保存") %></button>
                    <button type="button" class="btn btn-success" onclick="SaveAndView()"><%=(sendType==1?"发送":"保存") %>并预览</button>
                </div>
                <div style="margin-top: 200px;">&nbsp;</div>
            </div>


        </div>

    </form>
    <script type="text/javascript">
        /*编辑器监听事件*/
        um.addListener('ready', function (editor) {
            $("#fckDescriptionShow").html(um.getContent());
        });
        um.addListener('selectionchange', function () {
            $("#fckDescriptionShow").html(um.getContent());
        });
        function Save() {
            SaveData(0)
        }
        function SaveAndView() {
            SaveData(1)
        }
        function SaveData(savetype) {
            var title = $.trim($("#ctl00_ContentPlaceHolder1_txtTitle").val());
            var memo = UE.getEditor('ctl00_ContentPlaceHolder1_txtMemo_txtMemo').getContent();
            if (title == "") {
                ShowMsg("请输入标题！");
                return;
            }
            if (memo == "") {
                ShowMsg("请输入正文内容！");
                return;
            }
            var sendto = $("input[type='radio']:checked").val();
            if (sendto == undefined) {
                ShowMsg("请选择<%=(sendType==1?"消息发送":"公告") %>对象！");
                return;
            }
            if (sendto == 2) {
                var u = $.trim($("#divUserList").html());
                if (u == "") {
                    ShowMsg("请选择消息发送用户！");
                    return;
                }
            }
            var data = "posttype=save&id=<%=Id%>&type=<%=sendType%>&sendto=" + sendto + "&title=" + encodeURIComponent(title) + "&memo=" + encodeURIComponent(memo);
            $.ajax({
                url: "noticeedit.aspx",
                type: "post",
                data: data,
                datatype: "json",
                success: function (json) {
                    if (json.success=="1") {
                        if (savetype == 0) {
                            window.location.href = "<%=reUrl%>";
                        } else {
                            NoticeView(json.id);
                            $('#previewshow').on('hidden.bs.modal', function () {
                                /*模态框关闭的时候页面跳转到列表页*/
                                window.location.href = "<%=reUrl%>";
                            })
                        }
                    } else {
                        ShowMsg(json.tips, false);
                    }
                }
            })
            //alert(title+"}}"+memo)
        }
        function NoticeView(id) {
            var content = '<iframe src="/vshop/NoticeDetail.aspx?type=view&Id=' + id + '" id="ifmMobile" width="100%" scrolling="no" frameborder="0"></iframe>';
            MobileContentShow('<%=(sendType==1?"消息发送":"公告") %>展示页', content);
        }
        function ShowUserList() {
            var admin = "<%=adminName%>";
            DialogFrame("../shop/usersselect.aspx?admin=" + encodeURIComponent(admin), "选择用户", 580, 390, GetSelectedUserList);
        }
        function CancelShowUserList() {
            $("#divUserList").html("");
        }
        function GetSelectedUserList() {
            var data = "posttype=getselecteduser&t=" + (new Date()).getTime();
            $.ajax({
                url: "noticeedit.aspx",
                type: "post",
                data: data,
                datatype: "json",
                success: function (json) {
                    if (json.success == "1") {
                        //json.icount
                        var dataLength = json.userlist.length;
                        if (dataLength > 0) {
                            $("#divUserList").html("<table onclick='ShowUserList()' class='mt10 table table-bordered table-hover'><thead><tr><th>昵称</th><th>手机</th><th>用户名</th></tr></thead><tbody id='tbUserList'></tbody></table>");
                            for (var i = 0; i < dataLength ; i++) {
                                var html = "<tr><td>" + json.userlist[i].name + "</td><td>" + json.userlist[i].tel + "</td><td>" + json.userlist[i].bindname + "</td></tr>";
                                $("#tbUserList").append(html);
                            }
                            var html = "<tr><td colspan='3'>总共选择了<span class='red'>" + json.icount + "</span>个用户</td></tr>";
                            $("#tbUserList").append(html);
                        }
                    } else {
                        var html = "<tr><td colspan='3'>未选择用户</td></tr>";
                        $("#tbUserList").append(html);
                    }
                }
            });
        }
    </script>

</asp:Content>
