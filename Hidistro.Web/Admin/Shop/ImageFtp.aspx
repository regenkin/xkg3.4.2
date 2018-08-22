<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImageFtp.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.Shop.ImageFtp" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link id="cssLink" rel="stylesheet" href="../css/style.css" type="text/css" media="screen" />
    <style type="text/css">
        .numCss{color:#ff6600; font-size:12px;}  
        .aCss{font-size:12px;}
        .typNameCss{color:#0b5ba5;}
        .liCss{margin-bottom:3px;}
        table td{border:1px solid #ddd;text-align:center;} 
        input[type="file"] {
    width:591px;
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>上传图片</h2>
            <%--<small>上传图片至服务器</small>--%>
        </div>
        <div style="margin-left:-130px;">
            <div class="form-group">
                <label for="inputEmail2" class="col-xs-2 control-label resetSize">上传到：</label>
                <div class="col-xs-4">
                    <Hi:ImageDataGradeDropDownList ID="dropImageFtp" TypeId="0" CssClass="form-control resetSize" runat="server" />
                </div>
            </div>
        </div>

        <div style="display:inline;">
            <div style="width:70%;float:left;">
            <table class="table table-hover mar table-bordered ImagesFtp" id="ImagesFtp">
                <tr>
<%--                    <td>
                        <table cellpadding="0" cellspacing="5" id="ImagesFtp" class="ImagesFtp">
                            <tr>--%>
                                <td width="100%">
                                    <!--<input type="file" onchange="FileExtChecking(this)" name="file" />-->
                                    <asp:FileUpload ID="FileUpload" runat="server"  onchange="FileExtChecking(this)" accept="image/gif, image/x-png, image/jpeg" />
                                </td>
                                <td width="60" nowrap="nowrap"><a onclick="AddAttachment()" href="javascript:void(0)" class="add glyphicon glyphicon-plus">添加</a></td>
                            <%--</tr>--%>
                        <%--</table>
                        
                    </td>

                </tr>--%>
            </table>
                <div class="btn_bottom" style="margin-top: 70px; text-align:center; ">
                    <asp:Button ID="btnSaveImageFtp" runat="server" Text="确定上传" CssClass="btn btn-primary bigsize" />
                </div> 
            </div>        
            <div class="ImagesMsg" style="background-color: #F7F7F7;width:20%;float: left;margin-left:35px;">
                <div style="text-align:center;padding-top:10px;">
                    <label>提示</label>
                </div>
                <div style="padding:20px 5px 20px 5px;">
                <small class="help-block" style="padding-bottom:10px;">1、您一次最多可以上传10张图片</small>
                <small class="help-block" style="padding-bottom:10px;">2、请勿重复选择同一个图片文件</small>
                <small class="help-block">3、图片文件的大小建议控制在500KB以内，图片太大会影响网站打开速度</small>     
                </div>                                    
            </div>
        </div>
    </form>
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {
            $('#typeDiv').find('span').each(function () {
                if ($(this).attr('id') != 'ctl00_ContentPlaceHolder1_ImageTypeID') {
                    $(this).removeClass();
                    $(this).addClass('numCss');
                    $(this).css('margin-left', '2px');
                }
            });
            $('#typeDiv').find('a').each(function () {
                $(this).removeClass();
                $(this).addClass('aCss');
                var url = $(this).attr('href');
                url = url.replace('/store/ImageData.aspx', '/Shop/ImageData.aspx');
                $(this).attr('href', url);
            });
            $('#typeDiv').find('li').each(function () {
                $(this).removeClass();
                $(this).addClass('liCss');
            });
        });

        function AddAttachment() {
            var objTable = $("#ImagesFtp");
            var intCount = $("#ImagesFtp tr").children().size() / 2;
            if (intCount >= 10)
            { ShowMsg("附件不能超过10个",false); return; }
            objTable.append("<tr><td width=\"100%\"><input type='file' name='fileFtp' onchange='FileExtChecking(this)'  accept='image/gif, image/x-png, image/jpeg'/></td><td nowrap=\"nowrap\"><a href='javascript:void(0);' onclick='DisposeTr(this)' class='del glyphicon glyphicon-minus'>移除</a></td></tr>");
        }
        function DisposeTr(arg_obj_item) {
            var objTr = $(arg_obj_item).parent().parent();
            objTr.remove();
        }

        function DisposeTr(arg_obj_item) {
            var objTr = $(arg_obj_item).parent().parent();
            objTr.remove();
        }

        function FileExtChecking(obj) {
            var ErrMsg = "";
            var AllowExt = ".jpg|.gif|.png|.jpeg|"  //允许上传的文件类型 每个扩展名后边要加一个"|" 小写 .bmp|
            var FileExt = obj.value.substr(obj.value.lastIndexOf(".")).toLowerCase();
            if (AllowExt != 0 && AllowExt.indexOf(FileExt + "|") == -1)  //判断文件类型是否允许上传
            {
                ErrMsg = "\n该类型不允许上传！</br>请上传:" + AllowExt + "类型的文件!";
                obj.value = "";
                ShowMsg(ErrMsg, false);
                return false;
            }
        }
    </script>
</asp:Content>
