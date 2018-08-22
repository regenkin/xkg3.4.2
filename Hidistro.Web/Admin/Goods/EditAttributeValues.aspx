<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditAttributeValues.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditAttributeValues" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .table_title {
            background: #f2f2f2;
        }

        table td, th {
            text-align: center;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <form runat="server">
        <div class="page-header">
            <h2>编辑扩展属性</h2>
            <small>可以添加或修改扩展属性值。</small>
        </div>
        <div style="margin-bottom:10px; margin-top:10px;">
        <input type="button" name="button" id="button1" value="添加属性值" class="btn btn-success " onclick="AddAttributeValue();" /></div>
        <UI:Grid ID="grdAttributeValues" runat="server" SortOrderBy="DisplaySequence" SortOrder="desc" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="ValueId"  CssClass="table table-hover mar table-bordered" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="属性值" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="40%">
                    <ItemTemplate>
                        <asp:Label ID="lblAttributeName" runat="server" Text='<%# Eval("ValueStr") %>'></asp:Label>
                        <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible="false"></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <UI:SortImageColumn HeaderText="排序" ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="15%" />
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_left td_right_fff" ItemStyle-Width="20%">
                    <ItemStyle CssClass="spanD spanN" />
                    <ItemTemplate>
                  
                        <span><a href="javascript:UpdateAttributeValue('<%#Eval("ValueId") %>','<%#Eval("ValueStr") %>');">修改</a></span>
                              <span class="submit_shanchu">
                            <Hi:ImageLinkButton ID="lkbDelete" CssClass="SmallCommonTextButton" runat="server" IsShow="true" CommandName="Delete" Text="删除" /></span>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </UI:Grid>



        <%--添加属性值--%>
        <div class="modal fade" id="myaddAttributeValueModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">添加属性值</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>属性值</label>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtValue" runat="server" Width="300" CssClass="form-control"></asp:TextBox>
                        </div>
                         <small class="help-block">扩展属性值不允许为空</small>
                    </div>

                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <asp:Button ID="btnCreate" runat="server" Text="添加属性值" CssClass="btn btn-primary" />

            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
        

        <%--修改属性值--%>
        <div class="modal fade" id="myEditAttributeValueModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">修改属性值</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>属性值</label>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtOldValue" runat="server" Width="300" CssClass="form-control"></asp:TextBox>
                        </div>
                         <small class="help-block">扩展属性值不允许为空</small>
                    </div>
                    
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <asp:Button ID="btnUpdate" runat="server" Text="修改属性值" CssClass="btn btn-primary" />

            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
       

        <div style="display: none">
            <input type="hidden" id="hidvalueId" runat="server" />
            <input type="hidden" id="hidvalue" runat="server" />
        </div>
    </form>
    <script>

        var formtype = "";
        function AddAttributeValue() {
            formtype = "add";
            $("#ctl00_ContentPlaceHolder1_txtValue").val("");
            $('#myaddAttributeValueModal').modal('toggle').children().css({
                width: '600px',
                height: '100px'
            })
            $("#myaddAttributeValueModal").modal({ show: true });
        }


        function UpdateAttributeValue(ValueId, ValueStr) {
            formtype = "edite";
            $("#ctl00_ContentPlaceHolder1_hidvalueId").val(ValueId);
            $("#ctl00_ContentPlaceHolder1_txtOldValue").val(ValueStr);
            $("#ctl00_ContentPlaceHolder1_hidvalue").val(ValueStr);
            $('#myEditAttributeValueModal').modal('toggle').children().css({
                width: '600px',
                height: '100px'
            })
            $("#myEditAttributeValueModal").modal({ show: true });

        }
 
    </script>
</asp:Content>
