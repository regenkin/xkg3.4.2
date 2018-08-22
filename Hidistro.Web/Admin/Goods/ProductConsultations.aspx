<%@ Page Language="C#" AutoEventWireup="true" Inherits="Hidistro.UI.Web.Admin.Goods.ProductConsultations"
    MasterPageFile="~/Admin/AdminNew.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        /*.table_title{background:#f2f2f2}
        .table td,th{text-align:center}*/
        #ctl00_ContentPlaceHolder1_grdConsultation th {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            background-color: #F7F7F7;
            text-align: left;
            vertical-align: middle;
        }

        #ctl00_ContentPlaceHolder1_grdConsultation td {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            /*vertical-align: middle;*/
        }

        .username {
            margin-left: 10px;
        }

        td {
            word-break: break-all;
        }
  
        .midclass.table tbody tr td.NameMid{vertical-align:middle;}
    </style>
    <script type="text/javascript">
        function showModel(id) {
            if (parseInt(id) > 0) {
                $('#hdCid').val(id);
            }
            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '200px'
            });
        }
        function errAlert(msg) {
            HiTipsShow(msg, 'error');
        }
        function Reply() {
            var id = $('#hdCid').val();
            var content = $('#<%=txt_content.ClientID%>').val();
            $.ajax({
                type: "post",
                url: "ReplyProductConsultation.ashx",
                data: { id: id, content: content },
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        $('#<%=txt_content.ClientID%>').val("");
                        $('#hdCid').val(0);
                        window.location.reload();
                    }
                    else {
                        errAlert(data.data);
                    }
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>客户咨询</h2>
            <%--<small>管理店铺的所有商品咨询，您可以查询或删除商品咨询</small>--%>
        </div>
        <div id="mytabl">
            <div class="table-page">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs">
                    <li class="active"><a href="#home">未回复咨询</a></li>
                    <li><a href="ProductConsultationsReplyed.aspx">已回复咨询</a></li>
                </ul>
                <div class="page-box">
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Tab panes -->
            <div class="tab-content">
                <div class="tab-pane active">
                    <div class="set-switch">
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="">商品名称</label>
                                <asp:TextBox ID="txtSearchText" runat="server" CssClass="form-control resetSize"
                                    placeholder="" />
                            </div>
                            <div class="form-group mr20">
                                <label for="">商品分类</label>
                                <Hi:ProductCategoriesDropDownList ID="dropCategories" runat="server" CssClass="form-control resetSize" />
                            </div>
                            <div class="form-group mr20">
                                <label for="">商家编码</label>
                                <asp:TextBox ID="txtSKU" runat="server" CssClass="form-control resetSize" placeholder="" />
                            </div>
                            <div class="form-group">
                                <asp:Button ID="btnSearch" runat="server" Text="查询" class="btn resetSize btn-primary" />
                            </div>
                        </div>
                    </div>
                 <%--   <div class="select-page clearfix" style="margin-top: 20px;">
                    </div>--%>
                    <UI:Grid ID="grdConsultation" runat="server" ShowHeader="true" CssClass="table mar table-bordered midclass"
                        AutoGenerateColumns="false" DataKeyNames="ConsultationId" HeaderStyle-CssClass="table_title"
                        GridLines="None" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="商品" ItemStyle-Width="30%" HeaderStyle-CssClass="td_left td_right_fff">
                                <ItemStyle VerticalAlign="Top" />
                                <ItemTemplate >                               
                                    <div style="float: left;"><div class="img fl mr10">
                                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60"  Width="60" Height="60"/></div>
                                        <a href='<%#string.Format("/ProductDetails.aspx?productId={0}",Eval("productId"))%>' target="_blank">
                                            <asp:Literal ID="lblProductName" runat="server" Text='<%# Eval("ProductName") %>' />
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="客户咨询" HeaderStyle-CssClass="td_left td_right_fff">
                                <ItemTemplate>
                                    <div>
                                        <asp:Label ID="lblConsultationText" runat="server" Text='<%# Eval("ConsultationText") %>'
                                            CssClass="line"></asp:Label>
                                    </div>
                                    <br />
                                    <span style="color: #999;">
                                        <Hi:FormatedTimeLabel ID="ConsultationDateTime" Time='<%# Eval("ConsultationDate") %>'
                                            runat="server"></Hi:FormatedTimeLabel>
                                        <asp:Label ID="lblUserName" runat="server" Text='<%#Eval("UserName").ToString() %>'
                                            CssClass="username"></asp:Label>
                                    </span>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="操作" ItemStyle-Width="60" HeaderStyle-CssClass="td_left td_right_fff" >
                                <ItemTemplate>
                                  
                                    <span class="submit_fuihu"><a href="javascript:void(0);" onclick="showModel('<%#Eval("ConsultationId")%>')"
                                        class="btn btn-success resetSize mb5">回复</a></span>
                                    <span class="submit_shanchu">
                                        <asp:Button ID="btnDel" CssClass="btn btn-danger resetSize" runat="server" Text="删除" CommandName="Delete" CommandArgument='<%#Eval("ConsultationId") %>' OnClientClick="return HiConform('<strong>确定要删除所选的客户咨询吗？</strong><p>删除后不可恢复！</p>',this)" />
                                    </span>
                                 
                                </ItemTemplate>
                                <ItemStyle CssClass="NameMid" />
                            </asp:TemplateField>
                        </Columns>
                    </UI:Grid>
                    <div class="page">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal" id="hform">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span
                            aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title" id="modaltitle" style="text-align: left">回复客户咨询</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-xs-3 control-label">回复内容：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" class="form-control" TextMode="MultiLine" Width="310px"
                                    Height="100px" Style="margin-left: 15px;" ID="txt_content"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input id="hdCid" type="hidden" value="0" />
                        <button type="button" class="btn btn-success" data-dismiss="modal" onclick="Reply();">
                            确 定</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">取 消</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </form>
</asp:Content>

