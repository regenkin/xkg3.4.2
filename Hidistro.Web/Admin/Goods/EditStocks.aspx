<%@ Page Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="EditStocks.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.EditStocks" Title="无标题页" %>
<%@ Import Namespace="Hidistro.Core"%>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server" style="width: 650px;">
       <div class="page-header">
            <h2>批量修改商品库存</h2>
            <small>您可以对已选的这些商品的库存直接改成某个值，或增加/减少某个值，也可以手工输入您想要的库存后在页底处保存设置</small>
        </div>
    <div class="set-switch">
            <div class="form-inline mb10">
                <div class="form-group mr20">
                    <label for="sellshop1">将原库存改为:</label>
                    <asp:TextBox ID="txtTagetStock" runat="server" Width="80px" MaxLength="20" CssClass="form-control" />
                                 
                </div>

                <div class="form-group">
                    <label for="sellshop3"></label>
                    <asp:Button ID="btnTargetOK" runat="server" Text="确定" CssClass="btn btn-success" />
                </div>
            </div>
            <div class="form-inline">
                <div class="form-group mr20">
                    <label for="sellshop4">将原库存增加(输入负数为减少)：</label>
                   
                    <asp:TextBox ID="txtAddStock" CssClass="form-control" runat="server" Width="80px" />
                </div>

                <div class="form-group">
                    <label for="sellshop3"></label>
                    <asp:Button ID="btnOperationOK" runat="server" Text="确定" CssClass="btn btn-success" />
                </div>
            </div>

        </div>
      <UI:Grid ID="grdSelectedProducts"  CssClass="table table-bordered table-hover" DataKeyNames="SkuId" runat="server" ShowHeader="true" AutoGenerateColumns="false" HeaderStyle-CssClass="table_title"   GridLines="None" Width="100%">
                    <Columns> 
                    <asp:TemplateField HeaderText="货号" ItemStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            &nbsp;<%#Eval("SKU") %>                                             
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="商品" ItemStyle-Width="50%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                           <a href='<%#"/ProductDetails.aspx?productId="+Eval("ProductId")%>' target="_blank">
                                        <Hi:ListImage ID="ListImage1"  runat="server" DataField="ThumbnailUrl40"/>      
                                 </a> 
                            <%#Eval("ProductName") %> <%#Eval("SKUContent")%>                                                  
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="库存" HeaderStyle-Width="20%" HeaderStyle-CssClass="td_right td_left">
                        <ItemTemplate>
                            <asp:TextBox ID="txtStock" runat="server" Text = '<%#Eval("Stock") %>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>        
                    </Columns>
                </UI:Grid>
       <div class="form-group">
            <div class="col-xs-offset-4 col-xs-10">
                <button type="button" class="btn btn-default" onclick="CloseModal();">关闭</button>&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSaveStock" runat="server" Text="保存设置" CssClass="btn btn-success" />

            </div>
        </div>
        </form>
 <script>

        function CloseModal() {
            window.parent.closeModal("divEditBaseInfo");
        }

    </script>
</asp:Content>
