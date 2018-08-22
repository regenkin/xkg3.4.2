<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProductToActivity.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.EditProductToActivity" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectBtnCss{width:200px;height:40px;}
    </style>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $('input[name="CheckBoxGroup"]').each(function () {
                    $(this).prop('checked', check);        
                });
            });

            $('input[name="allBtn"]').each(function () {
                $(this).click(function () {
                    window.location.href = "ActivityList.aspx";
                });
            });

            $('.content-table table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            });
            var tableTitle = $('.title-table').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.title-table').css({
                        position: 'fixed',
                        top: '58px'
                    })
                }
                if ($(document).scrollTop() + $('.title-table').height() + 58 <= tableTitle) {
                    $('.title-table').removeAttr('style');
                }
            });
        });

        function winqrcode(url) {
            $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + url);
            $('#divqrcode').modal('toggle').children().css({
                width: '300px',
                height: '300px'
            });
            $("#divqrcode").modal({ show: true });
        }
        function closeModal(obj) {
            $("#" + obj).modal('hide');
            location.reload();
        }

        function BatchStop() {
            var id = $('#txt_id').val();
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                var data = {
                    actType: '1',
                    id: id,
                    products: ids.join(','),
                    type: 0
                };

                $.ajax({
                    type: "post",
                    url: "EditProductHandler.ashx",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            window.location.reload();
                        }
                        else {
                            ShowMsg("暂停商品失败（" + data.data + ")");
                        }
                    }
                });
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function BatchOpen() {
            var id = $('#txt_id').val();
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                var data = {
                    actType: '1',
                    id: id,
                    products: ids.join(','),
                    type: 1
                };

                $.ajax({
                    type: "post",
                    url: "EditProductHandler.ashx",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            window.location.reload();
                        }
                        else {
                            ShowMsg("恢复商品失败（" + data.data + ")");
                        }
                    }
                });
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function BatchRemove() {
            var id = $('#txt_id').val();
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                var data = {
                    actType: '1',
                    id: id,
                    products: ids.join(','),
                    type: 2
                };

                $.ajax({
                    type: "post",
                    url: "EditProductHandler.ashx",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            window.location.reload();
                        }
                        else {
                            ShowMsg("移除商品失败（" + data.data + ")");
                        }
                    }
                });
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function saveAll(obj) {
            var id = $('#txt_id').val();
            var ids = "";

            $('input[name="CheckBoxGroup"]').each(function () {
                if($(this).prop('checked'))
                {
                    ids += ','+$(this).val();
                }
            });

            if (ids != "") {
                ids = ids.substring(1);               
            }

            if (ids.length > 0)
            {
                var type;
                var btnName = $(obj).val();
                if (btnName == "暂停") {
                    type = 0;
                }
                else if (btnName == "恢复") {
                    type = 1;
                }
                else if (btnName == "移除") {
                    type = 2;
                }


                var data = {
                    actType: '1',
                    id: id,
                    products: ids,
                    type: type
                };

                $.ajax({
                    type: "post",
                    url: "EditProductHandler.ashx",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            window.location.reload();
                        }
                        else {
                            ShowMsg("修改商品失败（" + data.data + ")");
                        }
                    }
                });

            }          
            window.location.href = "ActivityList.aspx";
        }


        function setStatusSingle(obj, productId,btnName) {
           
            var type;
            var id = $('#txt_id').val();
            if (btnName == "暂停") {
                type = 0;
            }
            else if (btnName == "恢复") {
                type = 1;
            }
            else if (btnName == "移除") {
                type = 2;
            }
       
                       
            var data = {
                actType:'1',
                id: id,
                products: productId,
                type:type
            };

            $.ajax({
                type: "post",
                url: "EditProductHandler.ashx",
                data: data,
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {                            
                        window.location.reload();
                    }
                    else
                    {
                        ShowMsg("修改商品失败（" + data.data + ")");
                    }
                }
            });
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>减满活动商品选择</h2>
        </div>
        <input type="hidden" id="txt_id" value="<%=_id%>" />
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist">
                <li id="tabHeader_coupondsjoin" role="presentation" class="active">
                    <a href="EditProductToActivity.aspx?id=<%=_id%>">已加入(<asp:Label runat="server" ID="lblJoin" Text="0"></asp:Label>)</a>
                </li>
                <li id="tabHeader_couponds" role="presentation">
                    <a href="AddProductToActivity.aspx?id=<%=_id%>">出售中(<asp:Label runat="server" ID="lbsaleNumber" Text="0"></asp:Label>)</a>
                </li>

                <li id="tabHeader_memberCouponds" role="presentation" >
                    <a href="AddProductToCoupon_stock.aspx?id=<%=_id%>">仓库中(<asp:Label runat="server" ID="lbwareNumber" Text="0"></asp:Label>)</a>
                </li>
            </ul>
            <div class="page-box" style="margin-right: 15px;">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
        </div>

        <div class="set-switch" style="margin-top:10px;">
        <div class="form-inline">
            <label>商品名称:</label>
            <asp:TextBox ID="txt_name" Width="100" runat="server" CssClass="form-control resetSize mr20" />

            <label>现价格区间:</label>
            <asp:TextBox ID="txt_minPrice" Width="100" runat="server" CssClass="form-control resetSize" />
            <label>至</label>
            <asp:TextBox ID="txt_maxPrice" Width="100" runat="server" CssClass="form-control resetSize mr20" />
            <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn btn-primary resetSize" />
        </div>
        </div>

        <div class="form-inline" style="margin-top:-10px; margin-bottom: 10px;">
            <table style="width:100%;margin-top:5px;">
                <tr>
                    <td width="77%">
                        <ul>
                            <li class="batchHandleButton">
                                <span class="signicon"></span>

                                <input type="checkbox" name="selectAll" id="selectAll" /> 全选
                                <button type="button" class="btn btn-warning resetSize" onclick="BatchStop()" style="margin-left:20px;">
                                    批量暂停
                                </button>

                                <button type="button" class="btn btn-success resetSize" onclick="BatchOpen()" style="margin-left:10px;">
                                    批量恢复
                                </button>
                                                                                              
                                <button type="button" class="btn btn-danger resetSize" onclick="BatchRemove()" style="margin-left:10px;">
                                    批量移除
                                </button>
                                
                            </li>
                        </ul>
                    </td>
                    
                </tr>
            </table>
        </div>

        <div class="sell-table">
            <div class="title-table">
                <table class="table">
                    <thead>
                        <tr>
                            <th width="50%">商品名称</th>
                            <th width="10%" style="text-align:left;">价格</th>
                            <th width="40%"></th>
                         </tr>
                    </thead>
                </table>
            </div>
            <div class="content-table">
                <table class="table">
                    <tbody>
                        <asp:Repeater ID="grdProducts" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td width="50%">
                                        
                                        <input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("ProductId") %>' />

                                        <div class="img fl mr10">
                                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60" Width="60" Height="60" />
                                        </div>
                                        <div class="shop-info">
                                            <p class="mb5"><%# Eval("ProductName") %></p>
                                            <a class="er" href="javascript:void(0)" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("ProductId")%>');"></a>
                                            <input type="text" id='urldata<%# Eval("ProductId") %>' placeholder="" name='urldata<%# Eval("ProductId") %>' value='<%#"http://"+Globals.DomainName+"/ProductDetails.aspx?productId="+Eval("ProductId")%>' disabled="" style="display: none">
                                            <a class="fz" href="javascript:void(0)" data-clipboard-target='urldata<%# Eval("ProductId") %>' id='url<%# Eval("ProductId") %>' onclick="copyurl(this.id);"></a>
                                        </div>
                                    </td>
                                    <td width="10%" style="text-align:left;">
                                        <p>原价：<span><%#Eval("MarketPrice", "{0:f2}")%></span></p>
                                        <p>现价：<span style="color:#F60;"><%# Eval("SalePrice", "{0:f2}")%></span></p>
                                    </td>                                  
                                    <td style="text-align:center; width:40%;">
                                        <button type="button" onclick='<%#string.Format("setStatusSingle(this,{0},\"{1}\");", Eval("ProductId") , Eval("setStatus"))%>'
                                            class="btn btn-info resetSize" name="selectBtn" >                                            
                                            <%# Eval("setStatus")%>                                       
                                        </button>

                                        <button type="button" onclick='<%#string.Format("setStatusSingle(this,{0},\"移除\");", Eval("ProductId"))%>'
                                            class="btn btn-danger resetSize" id="removeBtn" style="margin-left:20px;">                                            
                                            移除                                            
                                        </button>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
            </div>
        </div>
        <div class="page">
            <div class="bottomPageNumber clearfix">
                <div class="pageNumber">
                    <div class="pagination">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>
        </div>
        <div style="height:50px;"></div>

        <div class="footer-btn navbar-fixed-bottom autow">
            <div style="text-align:center;margin-right:100px;">
                <input type="button" id="saveAllBtn" class="btn btn-primary" value="完 成" />
            </div>
        </div>

        <%-- 商品二维码--%>
        <div class="modal fade" id="divqrcode">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">商品二维码</h4>
                    </div>
                    <div class="modal-body" style="text-align: center">
                        <image id="imagecode" src=""></image>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
    </form>
</asp:Content>
