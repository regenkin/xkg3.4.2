<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CouponsPage.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.CouponsPage" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>
    </title>
    <link rel="stylesheet" href="/admin/css/common.css" />
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css">
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <%--    <style type="text/css">
    	html,body{width: 100%;height: 100%;background-color: #F1F1F1; position: relative;}
    	.dislogin{width: 340px;padding: 100px 20px 20px; position: absolute;left: 50%;margin-left: -170px; background:url(../images/login-title.png) no-repeat 25px 35px #fff;box-shadow: 0 0 300px #fff;}
    	.dislogin .form-group{margin-bottom: 10px;position: relative;}
    	.dislogin .form-group label{position: absolute;left: 10px;top: 0;height: 40px;line-height: 40px;color: #B3B3B3;z-index: 10;}
    	.dislogin-input .form-control{height: 30px;padding: 6px 12px 6px 35px;}
    	.dislogin .vercode{position: relative;}
    	.dislogin .vercode .form-control{border-right: none;width: 190px;}
    	.dislogin .vercode .imgcode{position: absolute;top: 0;right: 0; width: 112px;height: 40px;overflow: hidden;}
    	.dislogin-btn .btn{display: block;width: 100%;height: 40px;}
    	form{margin-bottom: 30px;}
    	.wechat-code{position: relative;height: 95px;}
    	.wechat-code .code-img{width: 95px;height: 95px;position: absolute;left: 0;top: 0;}
    	.wechat-code p{margin-left: 105px;color: #464646;text-shadow:0 0 0 #A5A5A5;line-height: 25px;}
    	.wechatimg,.shoppingimg,.modimg,.userimg{position: absolute;width: 60px;height: 60px;background-image: url(../images/loginbg2.png);background-repeat: no-repeat;}
    	.wechatimg{left: -100px;top: 100px;background-position: 0 1px;}
    	.shoppingimg{right: -100px;top: 100px;background-position: 0 -62px;}
    	.modimg{left: -100px;bottom: 120px;background-position:15px -133px;}
    	.userimg{right: -100px;bottom: 120px;background-position:11px -199px;}
    </style>--%>
    <script type="text/javascript">
        $(document).ready(function () {
            setHeader();
            setTabContent();
        });

        function setHeader()
        { 
            $('th').each(function () {
                $(this).css('text-align', 'center');
            });
        }

        function setTabContent() {
            var bfinish = $('#bFinishedHidden').val() == "true" ? true : false;
            var ballProduct = $('#bAllProductHidden').val() == "true" ? true : false;

            if (!ballProduct)
            {
                //FinishSpan  unFinishSpan modifyProductSpan DeleteSpan
                if(!bfinish)
                {
                    $('span[title="FinishSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                    $('span[title="unFinishSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="modifyProductSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="DeleteSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                }
                else
                {
                    $('span[title="FinishSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="unFinishSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                    $('span[title="modifyProductSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="DeleteSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                }
            }
            else
            {
                if (!bfinish) {
                    $('span[title="FinishSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                    $('span[title="unFinishSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="modifyProductSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                    $('span[title="DeleteSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                }
                else {
                    $('span[title="FinishSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="unFinishSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                    $('span[title="modifyProductSpan"]').each(function () {
                        $(this).css('display', 'none');
                    });
                    $('span[title="DeleteSpan"]').each(function () {
                        $(this).css('display', '');
                    });
                }
            }
        }


        //function setWindowSize() { //iframe自动本窗口高度
        //    try {
        //        var thiswin = window.parent.document.getElementById(window.name);
        //        if (window.document.body.scrollWidth - thiswin.offsetWidth > 6) {
        //            document.body.style.overflowX = "auto";
        //            thiswin.height = window.document.body.scrollHeight + 20;
        //            thiswin.width = window.document.body.scrollWidth + 20;
        //        } else {
        //            document.body.style.overflowX = "hidden";
        //            thiswin.height = window.document.body.scrollHeight;
        //            thiswin.width = window.document.body.scrollWidth
        //        }
        //    } catch (e) { }
        //}
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <input type="hidden" id="bFinishedHidden" value="<%=bFininshed%>" />
        <input type="hidden" id="bAllProductHidden" value="<%=bAllProduct%>" />
        <div class="form-inline" style="margin-bottom:5px;">
            
           <asp:TextBox runat="server" CssClass="form-control" ID="txt_name" placeholder="优惠券名称" Width="110px"></asp:TextBox>   
        
            <asp:TextBox runat="server" CssClass="form-control" ID="txt_minVal" placeholder="面值"
                Width="110px"></asp:TextBox>
            至
            <asp:TextBox runat="server" CssClass="form-control" ID="txt_maxVal" placeholder="面值"
                Width="110px"></asp:TextBox>
      
            <UI:WebCalendar runat="server" ReadOnly="false" CssClass="form-control"
                ID="calendarStartDate" placeholder="有效期" Width="110px" />
            至
            <UI:WebCalendar runat="server" CssClass="form-control" ID="calendarEndDate" placeholder="有效期"
                Width="110px" />
            <asp:Button CssClass="btn btn-primary" ID="btnSeach" runat="server" Text="查询" OnClick="btnImagetSearch_Click" />

        </div>
        <UI:Grid ID="grdCoupondsList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
            DataKeyNames="CouponId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
            GridLines="None" Width="100%">
                            <Columns>
                                <UI:CheckBoxColumn CellWidth="50" ItemStyle-HorizontalAlign="Center" />

                                <asp:TemplateField HeaderText="优惠券名称" SortExpression="CouponName" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <%# Eval("CouponName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="面值" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                       ￥<%# Eval("CouponValue")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="使用条件" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <%# Eval("useConditon")%>
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="使用期限" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <%# Eval("BeginDate")%>至
                                        <%# Eval("EndDate")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="领取限制" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <%# Eval("ReceivNum")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="库存" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <%# Eval("StockNum")%>张
                                    </ItemTemplate>
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="已领取" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <%# Eval("ReceiveNum")%>张
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="已使用" HeaderStyle-HorizontalAlign="Center" ShowHeader="true">
                                    <ItemTemplate>
                                        <%# Eval("UsedNum")%>张
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="操作" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="border_top border_bottom"
                                    HeaderStyle-Width="95">
                                    <ItemStyle CssClass="spanD spanN" />
                                    <ItemTemplate>

                                        <span class="submit_jiage"><a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/EditCoupond.aspx?id={0}", Eval("CouponId")))%>'>
                                            编辑</a></span>

                                        <span  title="FinishSpan" class="submit_jiage">
                                            <a href='#' onclick="FinishCoupond(<%# Eval("CouponId") %>,0)">
                                            作废</a></span>

                                        <span title="unFinishSpan" class="submit_jiage">
                                            <a href='#' onclick="FinishCoupond(<%# Eval("CouponId") %>,1)">启用</a></span>

                                        <span title="modifyProductSpan" class="submit_jiage">
                                            <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/EditProducts.aspx?id={0}", Eval("CouponId")))%>')">修改宝贝</a></span>

                                        <span title="DeleteSpan" class="submit_shanchu">
                                            <Hi:ImageLinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true" Text="删除" /></span>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </ui:grid>
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
