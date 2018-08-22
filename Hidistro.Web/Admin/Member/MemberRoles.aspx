<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemberRoles.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Member.MemberRoles" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl"
    TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            um.addListener('selectionchange', function() {
                var content = um.getContent();
                $('#contentdiv').empty();
                $('#contentdiv').append(content);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>会员权益</h2>
            <small></small>
        </div>
        <table style="width: 100%;" class="table table-hover mar table-bordered">
            <tr>
                <td style="width:30%; vertical-align:top;">
                    <div class="edit-text-left">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                <div class="mobile-header">
                                    <i></i>
                                    <div class="mobile-title">会员权益</div>
                                </div>
                                <div class="mobile mate-list">
                                    <div id="contentdiv" style="height:450px; overflow-y:auto;">
                                        这里是预览内容
                                    </div>
                                </div>
                                <div class="mobile-footer"></div>
                            </div>
                          </div>
                        </div>
     
                    </div>
                </td>
                <td style="vertical-align:top;">
                    <Kindeditor:KindeditorControl ID="fkContent" runat="server" Width="558" Height="200"  />
                                            <%--<span id="content" style="display: inline-block;">                                           
                                            <link rel="stylesheet" src="/kindeditor/themes/default/default.css" />
                                            <script charset="utf-8" src="/kindeditor/kindeditor.js"></script>
                                            <script charset="utf-8" src="/kindeditor/lang/zh_CN.js"></script>
                                            <script type="text/javascript">var auth = "";  </script>
                                            <script type="text/javascript">
                                                var editor;
                                                KindEditor.ready(function (K) {
                                                    editor = K.create('#ctl00_ContentPlaceHolder1_txt_content', {
                                                        resizeType: 2,
                                                        allowFileManager: true,
                                                        allowFlashUpload: false,
                                                        allowMediaUpload: false,
                                                        IsAdvPositions: false,
                                                        fileManagerJson: '/Admin/FileManagerJson.aspx',
                                                        uploadJson: '/Admin/UploadFileJson.aspx',
                                                        fileCategoryJson: '/Admin/FileCategoryJson.aspx',
                                                        afterChange: function () {
                                                            this.sync();
                                                            var content = this.html();
                                                            $('#id').val(content);
                                                            $('#contentdiv').empty();
                                                            $('#contentdiv').append(content);
                                                           
                                                        }
                                                    });
                                                });
                                            </script>
                                            <textarea runat="server" id="txt_content" name="txt_content" 
                                                style="width: 98%; height: 300px; visibility: hidden; display: none;" >                                               
                                            </textarea>

                                        </span>--%>
                                        <div class="btn_bottom" style="margin-top: 5px; text-align: center;">
                                            <asp:Button ID="btnSaveImageFtp" runat="server" Text="保存" CssClass="btn btn-success inputw100" />
                                        </div>
                </td>
            </tr>
        </table>
    </form>
    <script type="text/javascript">
        
        //$(document).ready(function () {
        //    editor.afterChange()
        //    {
        //        edit();
        //    }
        //})


        function edit() {
            var content = editor.html();
            $('#contentdiv').remove();
            $('#contentdiv').append(content);
        }
    </script>
</asp:Content>