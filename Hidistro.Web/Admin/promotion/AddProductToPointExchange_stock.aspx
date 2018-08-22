<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddProductToPointExchange_stock.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.AddProductToPointExchange_stock" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .selectBtnCss{width:200px;height:40px;}
        .inputCss{width:54px;height:26px;}
    </style>
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $('input[name="CheckBoxGroup"]').each(function () {
                    if ($(this).prop('disabled') == false)
                        $(this).prop('checked', check);
                });
            });

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

        function setEdit() {
            $('p[stitle="canEdit"]').each(function () {
                $(this).click(function () {
                    var edit = $(this).next();
                    $(this).css('display', 'none');
                    $(edit).css('display', '');
                    $(edit).focus();
                });
            });


            $('input.inputCss').blur(function () {
                var val = $(this).val();
                if (!checkInt(val)) {
                    ShowMsg('请输入整数！', false);
                    return;
                }
                if (Number(val) <= 0) {
                    if ($(this).attr('name') == "point" || $(this).attr('name') == "pNumber") {
                        ShowMsg('请输入大于0的整数！', false);
                        return;
                    }
                    else {
                        val = "不限";
                    }
                }
                var p = $(this).prev();
                $(p).text(val);
                $(p).css('display', '');
                $(this).css('display', 'none');
            });
        }

        function setGrades(id, obj) {
            var pNumber = "";
            var point = "";
            var eachNumber = "";
            var flag = true;
            if (id == null) {
                id = "";
                $('input[name="CheckBoxGroup"]').each(function () {
                    if (!flag) return;
                    if ($(this).prop('checked')) {
                        id += "," + $(this).val();
                        var n1 = 0;
                        var n2 = 0;
                        var n3 = 0;
                        $(this).parent().next().next().find('input.inputCss').each(function () {
                            if (!checkInt($(this).val())) {
                                ShowMsg('请输入正确的发放总量！', false);
                                flag = false;
                                return;
                            }                          
                            pNumber += "," + $(this).val();
                            n1 = Number($(this).val());
                        });
                        if (!flag) return;
                        $(this).parent().next().next().next().find('input.inputCss').each(function () {
                            if (!checkInt($(this).val())) {
                                ShowMsg('请输入正确的兑换积分！', false);
                                flag = false;
                                return;
                            }
                            if (Number($(this).val()) <= 0) {
                                ShowMsg('请输入正确的兑换积分！', false);
                                flag = false;
                                return;
                            }
                            point += "," + $(this).val();
                            n2 = Number($(this).val());
                        });
                        if (!flag) return;
                        $(this).parent().next().next().next().next().prev().find('input.inputCss').each(function () {

                            if (!checkInt($(this).val())) {
                                ShowMsg('请输入正确的每人限兑数量！', false);
                                flag = false;
                                return;
                            }
                            eachNumber += "," + $(this).val();
                            n3 = Number($(this).val());
                        });
                        if (n3 > n1 && n1!=0) {
                            ShowMsg('每人限兑数量不能大于发放总量！', false);
                            flag = false;
                            return;
                        }
                    }
                });
                if (id.length > 1) {
                    id = id.substr(1);
                    pNumber = pNumber.substr(1);
                    point = point.substr(1);
                    eachNumber = eachNumber.substr(1);
                }
                else {
                    ShowMsg('请选择商品！', false);
                    flag = false;
                    return;
                }
            }
            else {
                $(obj).parent().prev().find('input.inputCss').each(function () {
                    eachNumber = $(this).val();
                    if (!checkInt(eachNumber)) {
                        ShowMsg('请输入正确的每人限兑数量！', false);
                        flag = false;
                        return;
                    }
                });
                $(obj).parent().prev().prev().find('input.inputCss').each(function () {
                    point = $(this).val();
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
                });
                $(obj).parent().prev().prev().prev().find('input.inputCss').each(function () {
                    pNumber = $(this).val();
                    if (!checkInt(pNumber)) {
                        ShowMsg('请输入正确的发放总量！', false);
                        flag = false;
                        return;
                    }
                });
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
                    products: id,
                    pNumbers: pNumber,
                    points: point,
                    eachNumbers: eachNumber
                };
                $.ajax({
                    type: "post",
                    url: "SelectProductToExchangeHandler.ashx",
                    data: data,
                    dataType: "json",
                    success: function (data) {
                        if (data.type == "success") {
                            window.location.reload();
                        }
                        else {
                            ShowMsg("添加商品失败（" + data.data + ")");
                        }
                    }
                });
            }
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>积分兑换商品选择</h2>
        </div>
        <input type="hidden" id="txt_id" value="<%=eId%>" />
        <div class="table-page">
            <ul class="nav nav-tabs" role="tablist">
                <li id="tabHeader_1" role="presentation">
                    <a href="EditProductToExchange.aspx?id=<%=eId%>">已加入(<asp:Label runat="server" ID="lbSelectNumber" Text="0"></asp:Label>)</a>
                </li>

                <li id="tabHeader_2" role="presentation" >
                    <a href="AddProductToPointExchange.aspx?id=<%=eId%>">出售中(<asp:Label runat="server" ID="lbsaleNumber" Text="0"></asp:Label>)</a>
                </li>

                <li id="tabHeader_3" role="presentation" class="active">
                    <a href="AddProductToPointExchange_stock.aspx?id=<%=eId%>">仓库中(<asp:Label runat="server" ID="lbwareNumber" Text="0"></asp:Label>)</a>
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
           <button type="button" class="btn btn-primary resetSize" onclick="setGrades()" style="margin-left:20px;">
               批量加入
           </button>
        </div>

        <div class="sell-table">
            <div class="title-table">
                <table class="table">
                    <thead>
                        <tr>
                            <th width="50%">商品名称</th>
                            <th width="10%" style="text-align:left;">价格</th>
                            <th width="10%">发放总量</th>
                            <th width="10%">兑换积分</th>
                            <th width="10%">每人限量</th>
                            <th width="10%"></th>
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
                                        <input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("ProductId") %>' <%#string.Format("{0}", Eval("canChkStatus"))%> />

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
                                    <td width="10%">
                                        <div <%#string.Format("style=\" display :{0}\"", Eval("canSelStatus"))%>>
                                            <p stitle="canEdit" class="btn btn-default resetSize">
                                                <%#Eval("ProductNumber")%>
                                            </p>
                                            <input type="text" name="pNumber" class="inputCss" style="display:none;" value="<%#Eval("ProductNumber")%>" />
                                        </div>
                                        <p <%#string.Format("style=\" display :{0}\"", Eval("seledStatus"))%>><%#Eval("ProductNumber")%></p>                                      
                                    </td>    
                                    <td width="10%">
                                        <div <%#string.Format("style=\" display :{0}\"", Eval("canSelStatus"))%>>
                                            <p stitle="canEdit" class="btn btn-default resetSize">
                                            <%#Eval("PointNumber")%>
                                            </p>
                                            <input type="text" name="point" class="inputCss" style="display: none;"  value="<%#Eval("PointNumber")%>" />
                                        </div>
                                        <p <%#string.Format("style=\" display :{0}\"", Eval("seledStatus"))%>><%#Eval("PointNumber")%></p>
                                    </td>
  
                                    <td width="10%">
                                        <div <%#string.Format("style=\" display :{0}\"", Eval("canSelStatus"))%>>
                                            <p stitle="canEdit" class="btn btn-default resetSize">
                                                <%#int.Parse(Eval("eachMaxNumber").ToString())==0?"不限":Eval("eachMaxNumber").ToString()%>
                                            </p>
                                            <input type="text" name="eachNumber" class="inputCss" style="display: none;" value="<%#Eval("eachMaxNumber")%>" />
                                        </div>
                                        <p <%#string.Format("style=\" display :{0}\"", Eval("seledStatus"))%>><%#int.Parse(Eval("eachMaxNumber").ToString())==0?"不限":Eval("eachMaxNumber").ToString()%></p>
                                    </td>

                                    <td style="text-align: center; width: 10%;">
                                        <button type="button" onclick='<%#string.Format("setGrades({0},this);", Eval("ProductId"))%>' class="btn btn-info resetSize" name="selectBtn" <%#string.Format("style=\" display :{0}\"", Eval("canSelStatus"))%>>
                                            选取加入                                            
                                        </button>

                                        <button type="button" class="btn btn-success resetSize" <%#string.Format("style=\" display :{0}\"", Eval("seledStatus"))%>>
                                            已经加入
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
</asp:Content>
