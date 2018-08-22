<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponsList.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.CouponsList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {
            var tableTitle = $('.activediv').offset().top - 58;
            $(window).scroll(function () {
                if ($(document).scrollTop() >= tableTitle) {
                    $('.activediv').css({
                        position: 'fixed',
                        top: '58px',
                        borderBottom: '1px solid #ccc',
                        boxShadow: '0 1px 3px #ccc',
                        width: '1020px',
                    })
                }
                //if ($(document).scrollTop() + $('.activediv').height() + 58 <= tableTitle) {
                if ($(document).scrollTop() + 58 <= tableTitle) {
                    $('.activediv').attr("style", "background-color: rgb(255, 255, 255);");
                }
            });
        })

        $(document).ready(function () {
            var bfinished = $('#bFinishedHidden').val();
            if (bfinished == "True") {
                $('#operateDiv').show();
                $('.select-page').hide();
            }
            else {
                $('#operateDiv').hide();
                $('.select-page').show();
            }
            setHeader();
            setTabContent();
            $('#<%=txt_totalNum.ClientID%>').blur(function () {
                testInput(this);
            });

            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $("input[type='checkbox'][disabled!='disabled']").each(function () {
                    $(this).prop('checked', check);
                });
            });

            $('.table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            });
        });

        function GetProductId() {
            var v_str = "";
            $("input[type='checkbox'][name='CheckBoxGroup']:checked").each(function (rowIndex, rowItem) {
                v_str += $(rowItem).attr("value") + ",";
            });
            if (v_str.length == 0) {
                HiTipsShow("请选择优惠券", "warning")
                return "";
            } 
            return v_str.substring(0, v_str.length - 1);
        }

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

        function setHeader() {
            $('th').each(function () {
                $(this).css('text-align', 'center');
            });
        }

        function setActive(obj) {
            if (obj == false) {
                $('#tabHeader_couponds').removeClass();
                $('#tabHeader_Finished').removeClass();
                $('#tabHeader_couponds').addClass('active');
            }
            else {
                $('#tabHeader_couponds').removeClass();
                $('#tabHeader_Finished').removeClass();
                $('#tabHeader_Finished').addClass('active');
            }
        }

        function setTabContent() {
            var bfinish = $('#bFinishedHidden').val().toLowerCase() == "true" ? true : false;
            setActive(bfinish);
            if (!bfinish) {
                $('span[stitle="qrc"]').each(function () {
                    $(this).css('display', '');
                });
                $('span[stitle="FinishSpan"]').each(function () {
                    $(this).css('display', '');
                });
                $('span[stitle="unFinishSpan"]').each(function () {
                    $(this).css('display', 'none');
                });

                $('span[stitle="DeleteSpan"]').each(function () {
                    $(this).css('display', 'none');
                });

                $('span[stitle="modifyProductSpan"]').each(function () {
                    $(this).css('display', 'none');
                    var obj = $(this).prev().prev().prev().prev().prev().prev().val().toLowerCase();
                    if (obj == "true") {
                        $(this).css('display', 'none');
                    }
                    else {
                        $(this).css('display', 'block');
                    }
                });
            }
            else {
                $('span[stitle="qrc"]').each(function () {
                    $(this).css('display', 'none');
                });
                $('span[stitle="FinishSpan"]').each(function () {
                    $(this).css('display', 'none');
                });
                $('span[stitle="unFinishSpan"]').each(function () {
                    $(this).css('display', '');
                });
                $('span[stitle="modifyProductSpan"]').each(function () {
                    $(this).css('display', 'none');
                });
                $('span[stitle="DeleteSpan"]').each(function () {
                    $(this).css('display', '');
                });
            }
        }

        function confirmFinished(obj) {
            if (HiConform("结束优惠券<p>结束优惠券，是否继续？</p>", obj)) {
                return true;
            }
            else {
                return false;
            }
        }

        function toDate(str) {
            str = str.replace(/-/g, "/");
            var date = new Date(str);
            return date;
        }

        function useCoupon(obj, couponId, flag) {
            if (flag.toLowerCase() == "false") {
                if (HiConform("<strong>启用优惠券<p>启用优惠券，是否继续？</p></strong>", obj)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if (HiConform("<strong>1、启用优惠券，是否继续？<p>\n 2、优惠券已过期，重新设定有效期？</p> </strong>", obj)) {
                    //弹出修改窗体                    
                    $('#<%=txt_used.ClientID%>').val("true");
                    showModel(couponId);
                    return false;
                }
                else {
                    return false;
                }
            }
        }

        function showModel(id) {

            $.ajax({
                type: "post",
                url: "GetCouponDataHandler.ashx?id=" + id,
                data: {},
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        var total = Number(data.data.StockNum);// + Number(data.data.ReceiveNum);
                        $('#<%=txt_totalNum.ClientID%>').val(total);
                        $('#<%=ddl_maxNum.ClientID%>').val(data.data.maxReceivNum);
                        $('#<%=calendarStartDate2.ClientID%>_txtDateTimePicker').val(data.data.BeginDate.replace("T", " "));
                        $('#<%=calendarEndDate2.ClientID%>_txtDateTimePicker').val(data.data.EndDate.replace("T", " "));
                    }
                }
            });

            $('#<%=txt_id.ClientID%>').val(id);

            $('#previewshow').modal('toggle').children().css({
                width: '500px',
                top: '200px'
            });

        }


        function testInput(obj) {
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;

            var flag = false;

            if (id == $('#<%=txt_totalNum.ClientID%>').attr('id')) {
                regex = /^[0-9]*$/;
                parent = $(obj).parent().parent();
                flag = true;
            }

            if (flag) {
                if (testRegex(regex, content) && content != "") {
                    $(parent).removeClass();
                    $(parent).addClass("form-group");
                    $('#<%=btnSubmit.ClientID%>').removeAttr('disabled');
                }
                else {
                    $(parent).removeClass();
                    $(parent).addClass("form-group has-error");
                    $('#<%=btnSubmit.ClientID%>').removeAttr('disabled');
                    $('#<%=btnSubmit.ClientID%>').attr('disabled', 'disabled');
                }
            }

        }

        function testRegex(rgx, str) {
            if (str == "") return true;
            return result = rgx.test(str);
        }

        function setDel(id, obj) {
            var flag = true;
            if (id == null) {
                id = "";
                $("#ctl00_ContentPlaceHolder1_grdCoupondsList input:checkbox").each(function () {
                    if (!flag) return;
                    if ($(this).prop('checked')) {
                        id += "," + $(this).parent().parent().find("#hdCouponId").val();
                    }
                });
                if (id.length > 1) {
                    id = id.substr(1);
                }
                else {
                    ShowMsg('请选择优惠券！', false);
                    flag = false;
                    return;
                }
            }
            if (HiConform("删除优惠券，是否继续？", obj)) {
                flag = true;
            }
            else {
                flag = false;
            }
            if (flag) {
                $('#<%=txt_ids.ClientID%>').val(id);
                $('#<%=DelBtn.ClientID%>').click();
            }

        }
    </script>
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_grdCoupondsList th {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            background-color: #F7F7F7;
            text-align: center;
            vertical-align: middle;
        }

        #ctl00_ContentPlaceHolder1_grdCoupondsList td {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            vertical-align: middle;
        }

        #searchDiv input {
            margin-right: 20px;
        }

        .ml20 {
            margin-left: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>优惠券</h2>
            <%--<small>优惠券保存后，只能对发放总量和领券限制做编辑修改，请谨慎操作。</small>--%>
        </div>
        <div class="blank">
            <a href="NewCoupon.aspx" class="btn btn-primary">新增优惠券</a>
        </div>
        
        <div id="allProductsDiv">
            <div class="play-tabs">
               
               
                <div class="tab-pane active">
                     <div style="margin-top:10px;background-color:#fff">
                        <input type="hidden" id="bFinishedHidden" value="<%=bFininshed%>" />
                         <div class="activediv" style="background-color:#fff">
                              <div class="table-page">
                <ul class="nav nav-tabs" role="tablist">
                    <li id="tabHeader_couponds" role="presentation" class="active">
                        <a href="CouponsList.aspx?bFininshed=false">优惠券</a>
                    </li>
                    <li id="tabHeader_Finished" role="presentation">
                        <a href="CouponsList.aspx?bFininshed=true">已结束优惠券</a>
                    </li> 
                    <li id="tabHeader_memberCouponds" role="presentation">
                        <a href="MemberCouponList.aspx">领用/使用记录</a>
                    </li>                                       
                </ul>

                 <div class="page-box" style="margin-right:15px;">
                        <div class="page fr">
                            <div class="form-group">
                                <label for="exampleInputName2">每页显示数量：</label>
                                <UI:PageSize runat="server" ID="hrefPageSize" />
                            </div>
                        </div>
                    </div>
                </div>
                        <div class="set-switch">
                        <div class="form-inline" id="searchDiv">
            
                           <asp:TextBox runat="server" CssClass="form-control resetSize" ID="txt_name" placeholder="优惠券名称" Width="110px"></asp:TextBox>           
                            <asp:TextBox runat="server" CssClass="form-control resetSize" ID="txt_minVal" placeholder="面值" Width="110px"></asp:TextBox>至
                            <asp:TextBox runat="server" CssClass="form-control resetSize ml20" ID="txt_maxVal" placeholder="面值" Width="110px"></asp:TextBox>
                            <uc1:ucDateTimePicker runat="server" ID="calendarStartDate" CssClass="form-control resetSize"
                                Style="width: 110px; margin:0px;" PlaceHolder="有效期" />至
                            <uc1:ucDateTimePicker runat="server" ID="calendarEndDate" CssClass="form-control resetSize"
                                Style="width: 110px;" PlaceHolder="有效期" />
                            <asp:DropDownList ID="ddlCouponType" runat="server" Width="120" CssClass="form-control resetSize"></asp:DropDownList>
                            <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="查询"/>

                        </div>
                        </div>

                         <div class="form-inline" style="margin-bottom: 10px; margin-top:-10px;margin-left:19px; display:none;" id="operateDiv">
                            <div class="checkbox"><input type="checkbox" id="selectAll" class="allselect" /> <label for="selectAll">全选</label></div>

                          <%--  <button type="button" class="btn btn-danger resetSize" onclick="setDel();" style="margin-left:20px;">
                                批量删除
                            </button>--%>
                              <asp:Button ID="btnDelete" runat="server"  style="margin-left:20px;" Text="批量删除" CssClass="btn resetSize btn-danger" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？</strong><p>删除优惠券不可恢复！</p>', this);" />

                            <div style="display: none;">
                                <asp:Button runat="server" ID="DelBtn"/>
                                <asp:TextBox runat="server" ID="txt_ids"></asp:TextBox>
                            </div>
                        </div>
                         </div>
<%--                        <div class="select-page clearfix" style="margin-top: 20px;">
                        </div>--%>
                        <UI:Grid ID="grdCoupondsList" runat="server" ShowHeader="true" OnRowDataBound="grdCoupondsList_RowDataBound" AutoGenerateColumns="false"
                            DataKeyNames="CouponId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered" GridLines="None" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="选择" >
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbId" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="优惠券名称" SortExpression="CouponName">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("CouponName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="面值" ShowHeader="true">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        ￥<%# Eval("CouponValue")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="使用条件" ShowHeader="true" >
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="75px" />
                                    <ItemTemplate>
                                        <%# Eval("useConditon")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="使用期限" ShowHeader="true" HeaderStyle-Width="170" ItemStyle-Width="150">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("BeginDate","{0:yyyy/MM/dd HH:mm:ss}")%> 至 
                                        <%# Eval("EndDate","{0:yyyy/MM/dd HH:mm:ss}")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="领取限制" ShowHeader="true" HeaderStyle-Width="75px">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="75px" />
                                    <ItemTemplate>
                                        <%# Eval("ReceivNum")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="发放总量" ShowHeader="true">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("StockNum")%>张
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="已领取" ShowHeader="true">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("ReceiveNum")%>张
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="已使用" ShowHeader="true">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Eval("UsedNum")%>张
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom" HeaderStyle-Width="200">
                                    <ItemStyle CssClass="spanD spanN" VerticalAlign="Middle" HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <input type="hidden" name="ball" value="<%# Eval("IsAllProduct") %>" />
                                        <span stitle="qrc" style="display:none;">
                                            <img src="../images/qrcode.png" style="height:26px; cursor:pointer;" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/VShop/CouponDetails.aspx?CouponId="+Eval("CouponId")%>');" />
                                        </span>
                                        <%if(!isFinished){ %>
                                        <span stitle="qrc" style="display:none;">
                                            <input type="text" id='urldata<%# Eval("CouponId") %>' placeholder="" name='urldata<%# Eval("CouponId") %>' value='<%#"http://"+Globals.DomainName+"/VShop/CouponDetails.aspx?CouponId="+Eval("CouponId")%>' disabled="" style="display: none">
                                            <img src="../images/copylink.png" class="fz" style="height:26px; cursor:pointer;" data-clipboard-target='urldata<%# Eval("CouponId") %>' id='url<%# Eval("CouponId") %>' onclick="copyurl(this.id);" />
                                        </span>
                                        <span class="submit_jiage"><a onclick="showModel('<%#Eval("CouponId")%>')" class="btn btn-warning resetSize">编辑</a></span>

                                        <span stitle="FinishSpan" class="submit_jiage"  style="display:none;">
<%--                                            <asp:LinkButton runat="server" ID="FinishBtn" OnClientClick="return confirmFinished(this);" CssClass="btn btn-danger resetSize" CommandName="Update" Text="结束"></asp:LinkButton>--%>
                                              <asp:Button ID="FinishBtn"  IsShow="true" runat="server" Text="结束" CommandName="Update" CssClass="btn btn-danger resetSize" OnClientClick="return HiConform('<strong>结束优惠券<p>确定要结束优惠券，是否继续？</p></strong>',this)" />
                                        </span>

                                        <span stitle="unFinishSpan" class="submit_jiage" style="display:none;"> 
                                            <asp:Button runat="server" ID="unFinishBtn" OnClientClick='<%#string.Format("return useCoupon(this, \"{0}\",\"{1}\")", Eval("CouponId"),Eval("expire"))%>' CssClass="btn btn-success resetSize" CommandName="Update" Text="启用"></asp:Button>
                                        </span>

                                        <span stitle="modifyProductSpan" class="submit_jiage" style="display:none;">
                                            <a href='EditProductToCoupon.aspx?id=<%#Eval("CouponId")%>'>修改商品</a>
                                        </span>
                                        <%} %>
                                        <span stitle="DeleteSpan" class="submit_shanchu" style="display:none;">
                                         <%--   <Hi:ImageLinkButton runat="server" ID="lkDelete"  CommandName="Delete" IsShow="true" Text="删除" CssClass="btn btn-danger resetSize" />--%>
                                            <asp:Button ID="lkDelete"    IsShow="true" runat="server" Text="删除" CommandName="Delete" CssClass="btn btn-danger resetSize"  OnClientClick="return HiConform('<strong>确定要删除选择的优惠券吗？</strong><p>删除优惠券不可恢复！</p>',this)" />
                                        </span>
                                        <input id="hdCouponId" type="hidden" value="<%# Eval("CouponId") %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </ui:grid>
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination" style="width: auto">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1"  />
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
                        <h4 class="modal-title" id="modaltitle" style="text-align: left">编辑优惠券</h4>
                    </div>
                    <div class="modal-body">
                        <input type="hidden" id="htxtRoleId" runat="server" />                       

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>发放总量：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" class="form-control" Width="200px" Style="margin-left: 15px;"
                                    ID="txt_totalNum"></asp:TextBox>
                                <label style="margin-left: 3px;">张</label>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label">领券限制：</label>
                            <div style="position: relative; width: 200px; float: left">
                                <asp:DropDownList runat="server" ID="ddl_maxNum" Style="margin-left: 15px;" CssClass="form-control">
                                </asp:DropDownList>
                                <small class="help-block" style="margin-left: 15px;">每个用户自助领券上限，如不选，则默认为1张</small>
                            </div>
                        </div>
                        <div class="form-group" style="display:none">
                            <label class="col-xs-3 control-label"><em>*</em>生效时间：</label>
                            <div class="form-inline">
                                <uc1:ucDateTimePicker runat="server" ID="calendarStartDate2" CssClass="form-control"
                                    Style="margin-left: 15px;" PlaceHolder="生效时间" />                        
                            </div>
                        </div>

                        <div class="form-group" style="display:none">
                            <label class="col-xs-3 control-label"><em>*</em>过期时间：</label>
                            <div class="form-inline">
                                <uc1:ucDateTimePicker runat="server" ID="calendarEndDate2" CssClass="form-control"
                                    Style="margin-left: 15px;" PlaceHolder="过期时间" />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSubmit" runat="server" Text="确 定"  CssClass="btn  btn-success" />
                        <asp:TextBox ID="txt_id" runat="server"  style="display:none" ></asp:TextBox>
                        <asp:TextBox ID="txt_used" runat="server" Style="display: none"></asp:TextBox>
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>
        <!-- /.modal -->


        <%-- 商品二维码--%>
        <div class="modal fade" id="divqrcode">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">优惠券二维码</h4>
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
