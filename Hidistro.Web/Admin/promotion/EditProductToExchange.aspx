<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EditProductToExchange.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.EditProductToExchange" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectBtnCss {width:200px;height:40px;}
        .inputCss {width:54px;height:26px;}
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
            setDisplay();
            setEdit();

            $('#saveAllBtn').click(function () {
                window.location.href = "ExChangeList.aspx";
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

        function setDisplay() {
            $('input[name="status"]').each(function () {
                var status = $(this).val();
                var renew = $(this).next();
                var pause = $(this).next().next();
                if (status == "0") {
                    $(renew).css('display', 'none');
                    $(pause).css('display', '');
                }
                else
                {
                    $(renew).css('display', '');
                    $(pause).css('display', 'none');
                }
            });
        }

        function setEdit() {
            $('p[stitle="canEdit"]').each(function () {
                $(this).click(function () {
                    var edit = $(this).next();
                    $(this).css('display', 'none');
                    $(edit).css('display', '');
                    $(edit).focus();
                });
            });


            //$('input.inputCss').blur(function () {
            //    var val = $(this).val();
            //    if (!checkInt(val)) {
            //        ShowMsg('请输入整数！', false);
            //        return;
            //    }
            //    if (Number(val) <= 0) {
            //        if ($(this).attr('name') == "point") {
            //            ShowMsg('请输入大于0的整数！', false);
            //            return;
            //        }
            //        else {
            //            if ($(this).attr('name') == "pNumber") {
            //                val = "0";
            //            }
            //            else {
            //                val = "不限";
            //            }
            //        }
            //    }
            //    var p = $(this).prev();
            //    $(p).text(val);
            //    $(p).css('display', '');
            //    $(this).css('display', 'none');
            //});
        }

        function checkInt(str) {
            var type = /^[0-9]*$/;
            var re = new RegExp(type);
            if (str.match(re) == null) {
                return false;
            }
            else {
                return true;
            }
        }

        function setGrades(productId, obj) {
            var val = $(obj).val();
            if (!checkInt(val)) {
                ShowMsg('请输入整数！', false);
                return;
            }
            if (Number(val) <= 0) {
                if ($(obj).attr('name') == "point" || $(obj).attr('name') == "pNumber") {
                    ShowMsg('请输入大于0的整数！', false);
                    return;
                }
                else {
                    val = "不限";
                }
            }
            var p = $(obj).prev();
            $(p).text(val);
            $(p).css('display', '');
            $(obj).css('display', 'none');

            var pNumber = "";
            var point = "";
            var eachNumber = "";
            var flag = true;
            if (productId == null) {
                ShowMsg('请选择商品！', false);
                flag = false;
                return;
            }
            else {
                eachNumber = $(obj).parent().parent().find('input.inputCss').eq(2).val();
                if (!checkInt(eachNumber)) {
                    ShowMsg('请输入正确的每人限兑数量！', false);
                    flag = false;
                    return;
                }

                point = $(obj).parent().parent().find('input.inputCss').eq(1).val();
                if (!checkInt(point)) {
                    ShowMsg('请输入正确的兑换积分！', false);
                    flag = false;
                    return;
                }
                if (Number(point) <= 0) {
                    ShowMsg('请输入正确的兑换积分！', false);
                    flag = false;
                    return;
                }

                pNumber = $(obj).parent().parent().find('input.inputCss').eq(0).val();
                if (!checkInt(pNumber)) {
                    ShowMsg('请输入正确的发放总量！', false);
                    flag = false;
                    return;
                }
                if (Number(pNumber) <= 0) {
                    ShowMsg('请输入正确的发放总量！', false);
                    flag = false;
                    return;
                }

                if (Number(eachNumber) > Number(pNumber) && Number(pNumber) != 0) {
                    ShowMsg('每人限兑数量不能大于发放总量！', false);
                    flag = false;
                    return;
                }
            }
            if (flag) {
                var exchangeId = $('#txt_id').val();
                var data = {
                    id: exchangeId,
                    products: productId,
                    pNumbers: pNumber,
                    points: point,
                    eachNumbers: eachNumber
                };
                $.ajax({
                    type: "post",
                    url: "EditExchangeProducts.ashx",
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
        }

        function setPause(productId) {
            var exchangeId = $('#txt_id').val();
            var data = {
                actType: '2',
                id: exchangeId,
                products: productId,
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

        function setDel(productId) {
            var exchangeId = $('#txt_id').val();
            var data = {
                actType: '2',
                id: exchangeId,
                products: productId,
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

        function setRenew(productId) {
            var exchangeId = $('#txt_id').val();
            var data = {
                actType: '2',
                id: exchangeId,
                products: productId,
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

        function BatchStop(obj) {
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                setPause(ids.join(','));
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function BatchOpen() {
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                setRenew(ids.join(','));
            }
            else {
                ShowMsg('请选择商品');
            }
        }

        function BatchRemove() {
            var ids = [];
            $('input[name="CheckBoxGroup"]').each(function () {
                if ($(this).prop('checked')) {
                    ids.push($(this).val());
                }
            });
            if (ids.length > 0) {
                setDel(ids.join(','));
            }
            else {
                ShowMsg('请选择商品');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
       
        <div class="page-header">
            <h2>积分兑换商品选择</h2>   
        </div>
        <input type="hidden" id="txt_id" value="<%=exchangeId%>" />
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist">
                <li id="tabHeader_1" role="presentation" class="active">
                    <a href="EditProductToExchange.aspx?id=<%=exchangeId%>">已加入(<asp:Label runat="server"
                        ID="lbSelectNumber" Text="0"></asp:Label>)</a>
                </li>

                <li id="tabHeader_2" role="presentation" >
                    <a href="AddProductToPointExchange.aspx?id=<%=exchangeId%>">出售中(<asp:Label runat="server"
                        ID="lbsaleNumber" Text="0"></asp:Label>)</a>
                </li>

                <li id="tabHeader_3" role="presentation">
                    <a href="AddProductToPointExchange_stock.aspx?id=<%=exchangeId%>">仓库中(<asp:Label runat="server"
                        ID="lbwareNumber" Text="0"></asp:Label>)</a>
                </li>
            </ul>
            <div class="page-box" style="margin-right: 15px; text-align: right;">
                <div class="page fr">
                    <div class="form-group">
                        <label for="exampleInputName2">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
        </div>
        
        <div class="set-switch" style="margin-top:10px;">
            <div class="form-inline" style="margin-top: 5px; margin-bottom: 5px; vertical-align: central;">
                <label>商品名称:</label>
                <asp:TextBox ID="txt_name" Width="100" runat="server" CssClass="form-control resetSize mr20" />

                <label>现价格区间:</label>
                <asp:TextBox ID="txt_minPrice" Width="100" runat="server" CssClass="form-control resetSize" />
                <label>至</label>
                <asp:TextBox ID="txt_maxPrice" Width="100" runat="server" CssClass="form-control resetSize mr20" />
                <asp:Button ID="btnQuery" runat="server" Text="查询" CssClass="btn btn-primary resetSize" />
            </div>
        </div>
        

        <div style="margin-bottom: 10px; margin-top:-10px;">
            <input type="checkbox" id="selectAll" /> 全选

       <%--    <button type="button" class="btn btn-warning resetSize" onclick="BatchStop(this);" style="margin-left:20px;">
               批量暂停</button>--%>
                 <asp:Button ID="btnBatchStop" runat="server" Text="批量暂停" IsShow="true" OnClick="btnBatchStop_Click" CommandName="Update" CssClass="btn btn-danger resetSize" style="margin-left:20px;"  OnClientClick="return HiConform('<strong>暂停选择的商品</strong><p>确定要暂停选择的商品吗？</p>',this)" ToolTip="" /> 
           
           <%-- <button type="button" class="btn btn-success resetSize" onclick="BatchOpen();" style="margin-left:10px;">
               批量恢复
            </button--%> 
              <asp:Button ID="btnBatchOpen" runat="server" Text="批量恢复" IsShow="true"  style="margin-left:10px;" OnClick="btnBatchOpen_Click" CssClass="btn btn-success resetSize"   OnClientClick="return HiConform('<strong>批量恢复选择的商品</strong><p>确定要恢复选择的商品吗？</p>',this)" ToolTip="" /> 
           
           <%-- <button type="button" class="btn btn-danger resetSize" onclick="BatchRemove();" style="margin-left:10px;">
               批量移除
            </button>--%>
             <asp:Button ID="btnBatchRemove" runat="server" Text="批量移除" IsShow="true"  style="margin-left:10px;" OnClick="btnBatchRemove_Click" CssClass="btn btn-danger resetSize"   OnClientClick="return HiConform('<strong>批量移除选择的商品</strong><p>确定要移除选择的商品吗？</p>',this)" ToolTip="" /> 
           
        </div>

        <div class="sell-table">
            <div class="title-table">
                <table class="table">
                    <thead>
                        <tr>
                            <th width="40%">商品名称</th>
                            <th width="10%" style="text-align:left;">价格</th>
                            <th width="10%">发放总量</th>
                            <th width="10%">兑换积分</th>
                            <th width="10%">每人限量</th>
                            <th width="20%"></th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div class="content-table">
                <table class="table">
                    <tbody>
                        <asp:Repeater ID="grdProducts" runat="server" OnItemCommand="grdProducts_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td width="40%">

                                        <input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("ProductId") %>' />

                                        <div class="img fl mr10">
                                            <Hi:ListImage ID="ListImage1" runat="server" DataField="ThumbnailUrl60" Width="60"
                                                Height="60" />
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
                                    <td width="10%">
                                        <p stitle="canEdit" class="btn btn-default resetSize">
                                            <%#Eval("ProductNumber")%>
                                        </p>
                                        <input type="text" name="pNumber" class="inputCss" style="display: none;" value="<%#Eval("ProductNumber")%>" onblur='<%#string.Format("setGrades({0},this);", Eval("ProductId"))%>' />

                                    </td>
                                    <td width="10%">
                                        <p stitle="canEdit" class="btn btn-default resetSize">
                                            <%#Eval("PointNumber")%>
                                        </p>
                                        <input type="text" name="point" class="inputCss" style="display: none;" value="<%#Eval("PointNumber")%>" onblur='<%#string.Format("setGrades({0},this);", Eval("ProductId"))%>' />
                                    </td>

                                    <td width="10%">
                                        <p stitle="canEdit" class="btn btn-default resetSize">
                                            <%#int.Parse(Eval("eachMaxNumber").ToString())==0?"不限":Eval("eachMaxNumber").ToString()%>
                                        </p>
                                        <input type="text" name="eachNumber" class="inputCss" style="display: none;" value="<%#Eval("eachMaxNumber")%>" onblur='<%#string.Format("setGrades({0},this);", Eval("ProductId"))%>' />
                                    </td>

                                    <td style="text-align: center; width: 20%;">
                                        <input type="hidden" value="<%#Eval("status")%>" name="status" />

                                       <%-- <button type="button" name="renew" onclick='<%#string.Format("setRenew({0});", Eval("ProductId"))%>' class="btn btn-success resetSize">
                                            恢复
                                        </button>--%>
                                         <asp:Button ID="btnRenew" runat="server" Text="恢复" IsShow="true"  name="renew" CommandName="Renew" CommandArgument='<%#Eval("ProductId")%>'  class="btn btn-success resetSize"   OnClientClick="return HiConform('<strong>恢复选择的商品</strong><p>确定要恢复选择的商品吗？</p>',this)" ToolTip="" /> 
                                        <%--<button type="button" onclick='<%#string.Format("setPause({0});", Eval("ProductId"))%>' class="btn btn-warning resetSize" name="pause">
                                            暂停                                            
                                        </button>--%>
                                          <asp:Button ID="btnPause" runat="server" Text="暂停" IsShow="true"  name="pause"  CommandName="Pause" CommandArgument='<%#Eval("ProductId")%>'  class="btn btn-success resetSize"   OnClientClick="return HiConform('<strong>暂停选择的商品</strong><p>确定要暂停选择的商品吗？</p>',this)" ToolTip="" /> 

                                    <%--    <button type="button" onclick='<%#string.Format("setDel({0});", Eval("ProductId"))%>' class="btn btn-danger resetSize" name="del">
                                            移除                    
                                        </button>--%>
                                         <asp:Button ID="btnDel" runat="server" Text="移除" IsShow="true"  name="del"  CommandName="Delete" CommandArgument='<%#Eval("ProductId")%>'  class="btn btn-danger resetSize"   OnClientClick="return HiConform('<strong>移除选择的商品</strong><p>确定要移除选择的商品吗？</p>',this)" ToolTip="" />  
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
        <div style="height: 50px;"></div>

        <div class="footer-btn navbar-fixed-bottom autow">
            <div style="text-align: center; margin-right: 100px;">
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
       <script>

           function SelectAllNew(obj) {
               $("[name=CheckBoxGroup]").prop("checked", $(obj).get(0).checked);
               //.attr("checked", $(obj).get(0).checked);//这种方式会有异常，只能执行一次，
           }
   </script>
</asp:Content>
