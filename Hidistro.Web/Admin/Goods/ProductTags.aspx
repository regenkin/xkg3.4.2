<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ProductTags.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ProductTags" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>商品标签管理</h2>
        <small>定义商品所属的各个标签，如果在上架商品时给商品指定了某个标签，则商品详细页会显示该标签</small>
    </div>

    <div class="clearfix">
        <div class="fl">
            <a href="javascript:void(0)" onclick="ShowTags('add',null,this)" class="btn btn-success resetSize">添加标签</a>
        </div>
    </div>
    <form runat="server">
        <div class="datalist mt5">
            <asp:Repeater ID="rp_prducttag" runat="server">
                <HeaderTemplate>
                    <table class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th style="width: 80%;">标签名称</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("TagName") %></td>
                        <td>
                            <span class="submit_shanchu">
                                <a href="javascript:void(0)" onclick="ShowTags('update',<%# Eval("TagID") %>,this)">编辑</a>
                                <asp:Button ID="btnDel" runat="server" CommandName="delete" CommandArgument='<%# Eval("TagID") %>' OnClientClick="return HiConform('<strong>确定要删除该商品标签吗？</strong><p>删除后不可恢复！</p>',this);" Text="删除" CssClass="btnLink" />
                            </span>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
	</table>
                </FooterTemplate>
            </asp:Repeater>
        </div>


        <%--修改商品标签名--%>
        <div class="modal fade" id="updatetag_div">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">编辑商品标签</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label"><em>*</em>标签名称：</label><asp:TextBox ID="txttagname" CssClass="form-control inputw200" runat="server" MaxLength="20" onkeyup="$(this).val(getStrbylen($(this).val(),8))" />
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="hidden" id="hdtagId" runat="server" />
                        <asp:Button ID="btnupdatetag" runat="server" Text="修改商品标签" CssClass="btn btn-success" OnClientClick="return checkform('ctl00_ContentPlaceHolder1_txttagname')" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>

        <%--添加商品标签名--%>
        <div id="d" style="display: none">
            <div class="frame-content">
                <p><span class="frame-span frame-input90"><em>*</em>&nbsp;标签名称：</span>   </p>
            </div>
        </div>

        <div class="modal fade" id="addtag_div">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">添加商品标签</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group isordersbatch">
                            <label class="col-xs-3 control-label"><em>*</em>标签名称：</label><asp:TextBox ID="txtaddtagname" CssClass="form-control inputw200" runat="server" MaxLength="20" onkeyup="$(this).val(getStrbylen($(this).val(),8))" />
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnaddtag" runat="server" Text="添加商品标签" CssClass="btn btn-success"  OnClientClick="return checkform('ctl00_ContentPlaceHolder1_txtaddtagname')"/>
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        var formtype = "";
        function ShowTags(oper, tagId, link_obj) {
            arrytext = null;
            if (oper == "add") {
                formtype = "add";
                $("#ctl00_ContentPlaceHolder1_txtaddtagname").val("");

                $('#addtag_div').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
                //DialogShow("添加商品标签名称", "addtag", "addtag_div", "ctl00_ContentPlaceHolder1_btnaddtag");
            } else {
                formtype = "edite";
                var tagName = $(link_obj).parents("tr").find("td").eq(0).text();
                $("#ctl00_ContentPlaceHolder1_hdtagId").val(tagId);
                $("#ctl00_ContentPlaceHolder1_txttagname").val(tagName);
                $('#updatetag_div').modal('toggle').children().css({
                    width: '520px',
                    height: '180px'
                })
                //DialogShow("编辑商品标签名称", "editetag", "updatetag_div", "ctl00_ContentPlaceHolder1_btnupdatetag");
            }
        }
        function checkform(objId) {
            var info = $.trim($('#'+objId).val());
            if (info == "") {
                ShowMsg("请输入标签名称",false);
                return false;
            }
        }        
    </script>
</asp:Content>
