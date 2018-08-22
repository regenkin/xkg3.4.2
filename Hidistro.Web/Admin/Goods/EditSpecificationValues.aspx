<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditSpecificationValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditSpecificationValues" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table_title {
            background: #f2f2f2;
        }

        table td, th {
            text-align: center;
        }
    </style>
    <script type="text/javascript">
        //编辑规格值
        function UpdateAttributeValue(ValueId, ValueStr, ImageUrl, useAttributeImage) {
            var Rand = Math.random();
            var pathurl = "SkuValue.aspx?action=update&valueId=" + ValueId + "&useImg=" + useAttributeImage+"&Rand="+Rand;
            //var title = "修改规格值";
            //if (useAttributeImage == "True") {
            //    DialogFrame(pathurl, title, 420, 200);
            //} else {
            //    DialogFrame(pathurl, title, 440, 160);
            //}
            $("#MyIframe").attr("src", pathurl);
            $('#myShowAddSKUValueDivModal').modal('toggle').children().css({
                width: '650px',
                height: '150px'
            })
            $("#myShowAddSKUValueDivModal").modal({ show: true });
        }

        //添加新规格值
        function ShowAddSKUValueDiv(attributeId, useAttributeImage) {
            //var pathurl = "product/SkuValue.aspx?action=add&attributeId=" + attributeId + "&useImg=" + useAttributeImage;
            //var title = "添加规格值";
            //if (useAttributeImage == "True") {
            //    DialogFrame(pathurl, title, 420, 200);
            //} else {
            //    DialogFrame(pathurl, title, 450, 160);
            //}
            
            var Rand = Math.random();
            var pathurl = "SkuValue.aspx?action=add&attributeId=" + attributeId + "&useImg=" + useAttributeImage + "&Rand=" + Rand;
     
            $("#MyIframe").attr("src", pathurl);
            $('#myShowAddSKUValueDivModal').modal('toggle').children().css({
                width: '650px',
                height: '150px'
            })
            $("#myShowAddSKUValueDivModal").modal({ show: true });
        }
        function closeModal()
        {
            $("#myShowAddSKUValueDivModal").modal('hide');
            location.href = "EditSpecificationValues.aspx?typeId=" + getParam('typeId')+"&AttributeId="+getParam('AttributeId')+"&UseAttributeImage="+getParam('UseAttributeImage');
        }
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, "");//删除前后空格
        }
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>编辑规格值</h2>

        </div>
        <div style="margin-bottom: 10px; margin-top: 10px;">
            <input type="button" name="button" id="button1" value="添加新规格值" class="btn btn-success " onclick="javascript:ShowAddSKUValueDiv( '<%=Page.Request.QueryString["AttributeId"]%>    ','<%=Page.Request.QueryString["UseAttributeImage"]%>    ');" />

        </div>

        <UI:Grid ID="grdAttributeValues" CssClass="table table-hover mar table-bordered" runat="server" SortOrderBy="DisplaySequence" SortOrder="desc" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ValueId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="规格值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                    <ItemTemplate>
                        <Hi:SKUImage ID="SKUImage1" runat="server" CssClass="a_none" ImageUrl='<%# Eval("ImageUrl")%>' ValueStr='<%# Eval("ValueStr")%>' />
                        <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible="false"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <UI:SortImageColumn HeaderText="排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%" />
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="20%">
                    <ItemStyle CssClass="spanD spanN" />
                    <ItemTemplate>
                         <span><a href="javascript:UpdateAttributeValue('<%#Eval("ValueId") %>','<%#Eval("ValueStr") %>','<%# Eval("ImageUrl")%>','<%=Page.Request.QueryString["UseAttributeImage"]%>');">修改</a></span>
                        <span class="submit_shanchu">
                            <asp:LinkButton ID="btnAdd" Text="删除" runat="server" CommandName="dele" CommandArgument='<%#Eval("ImageUrl") %>' />
                        </span>
                       
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>


        <input runat="server" type="hidden" id="currentAttributeId" />
        <div class="modal fade" id="myShowAddSKUValueDivModal">

            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">添加规格值</h4>
                    </div>
                    <div class="modal-body">
                        <iframe src="" id="MyIframe" width="600" height="130" scrolling="no"></iframe>

                    </div>

                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

    </form>
    <script>


    </script>
</asp:Content>

