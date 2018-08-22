<%@ Page Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="ImageReplace.aspx.cs" Inherits="Hidistro.UI.Web.Admin.ImageReplace" Title="无标题页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .frame-content .frame-span {
            float: left;
            clear: both;
            text-align: right;
        }

        .frame-input90 {
            width: 90px;
            margin: 0px 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <%--添加规格值--%>

        <div class="form-horizontal">
            <div class="form-group" style="height:70px">

                <asp:HiddenField ID="RePlaceImg" Value='' runat="server" />
                <asp:HiddenField ID="RePlaceId" Value='' runat="server" />
                <label for="inputPassword3" class="col-xs-4 control-label">
                    <span class="frame-span frame-input90">上传图片：<em> </em></span>
                </label>
                <asp:FileUpload ID="FileUpload1" Width="200" runat="server" onchange="FileExtChecking(this)" />
            </div>
        </div>
        <div class="modal-footer">
            <asp:Button ID="btnSaveImageData" runat="server" Text="确 定" CssClass="btn btn-success" OnClientClick="return isFlagValue()" />
            <input id="btnCancel" type="button" value="取 消" class="btn btn-default" />
        </div>
    </form>
    <script>
        function isFlagValue() {
            imgsrc = $("#ctl00_ContentPlaceHolder1_RePlaceImg").val();
            imgid = $("#ctl00_ContentPlaceHolder1_RePlaceId").val();
            if (imgsrc.length <= 0 || imgid.length <= 0) {
                alert("请选择要替换的图片或图片名称不允许为空！");
                return false;
            }
            return true;
        }
        $(document).ready(function () {
            $("#btnCancel").click(function () {
                parent.$('#divmyIframeModal').modal('hide')
            })
        })
    </script>
</asp:Content>

