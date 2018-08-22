<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActivityList.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.ActivityList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
            setDisplay();

            var status = getUrlParam("status");
            $('#nav li').eq(status).siblings().removeClass('active').end().addClass('active');
        });


        function setDisplay() {
            $('span[stitle="modifyProductSpan"]').each(function() {               
                var obj = $(this).prev().val().toLowerCase();

                if (obj == "true") {
                    $(this).css('display', 'none');
                }
                else {
                    $(this).css('display', '');
                }
            });
        }


        function selAll(obj) {
            if (obj == null) {
                obj = $('#selectAll').prop('checked');
            }

            $('#ctl00_ContentPlaceHolder1_grdCoupondsList').find('input[type="checkbox"]').each(function () {
                $(this).prop('checked', obj);
            });
            $('#selectAll').prop('checked',obj);

        }

        function EndAct(Aid)
        {
            HiTipsShow("确定结束活动？结束后不可重新恢复！", "confirmII", function () {
                
                $.ajax({
                    type: "post",
                    url: "SaveActivityHandler.ashx",
                    data: {delId:Aid,action:"End"},
                    dataType: "json",
                    success: function (data) {
                        if (data.state) {
                            HiTipsShow(data.msg, "success", function () {
                                document.location.href = document.location.href;
                            });
                        }
                        else {
                            HiTipsShow(data.msg, "error");
                        }
                       
                    },
                    error: function () {
                        HiTipsShow("访问服务器出错!", "error");
                    }
                });



            }, "操作提示");
        }

        function dels() {
            var flag = true;
            if (HiConform("确定要执行该删除操作吗？删除后将不可以恢复！", null)) {
                flag = true;
            }
            else {
                flag = false;
            }
            if (flag) {
                $('#<%=DelBtn.ClientID%>').click();
            }
        }

        //获取url中的参数
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }
    </script>
    <style type="text/css">
        #ctl00_ContentPlaceHolder1_grdCoupondsList th {margin:0px;border-left:0px;border-right:0px;background-color:#F7F7F7;text-align:center; vertical-align:middle;}
        #ctl00_ContentPlaceHolder1_grdCoupondsList td {margin:0px;border-left:0px;border-right:0px;vertical-align:middle;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>满减活动</h2>
            <%--<small>满减满送活动是给商家提供的一个店铺营销工具，通过这个营销工具可以让商家的店铺促销活动更加丰富。</small>--%>
        </div>
        <div class="blank" style="text-align: left;">
            <a href="AddActivity.aspx" class="btn btn-primary">添加满减活动</a>
        </div>
        <div class="activediv"  style="background-color:#fff">

       
        <div class="play-tabs">
            <div class="table-page">
                <ul class="nav nav-tabs" role="tablist" id="nav">
                    <li  role="presentation" class="active">
                        <a href="ActivityList.aspx?status=0">所有活动(<asp:Label runat="server" ID="lblAll" Text="0"></asp:Label>)</a>
                    </li>

                    <li  role="presentation">
                        <a href="ActivityList.aspx?status=1">进行中(<asp:Label runat="server" ID="lblIn" Text="0"></asp:Label>)</a>
                    </li>
                    <li  role="presentation">
                        <a href="ActivityList.aspx?status=2">已结束(<asp:Label runat="server" ID="lblEnd" Text="0"></asp:Label>)</a>
                    </li>
                    <li  role="presentation">
                        <a href="ActivityList.aspx?status=3">未开始(<asp:Label runat="server" ID="lblUnBegin" Text="0"></asp:Label>)</a>
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
           
        </div>
 
        <div class="set-switch" style="margin-top:10px;">
        <div class="form-inline">
            <asp:TextBox runat="server" CssClass="form-control resetSize mr20" ID="txt_name" placeholder="活动名称" Width="110px"></asp:TextBox>
            <HI:DateTimePicker runat="server" ReadOnly="false" CssClass="form-control resetSize" ID="calendarStartDate" placeholder="有效期" Width="110" />
            至
            <Hi:DateTimePicker runat="server" CssClass="form-control resetSize mr20" ID="calendarEndDate" placeholder="有效期" Width="110" />
            <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="查询" />
        </div>
        </div>

        <div class="form-inline" style="margin-bottom: 10px; margin-top:-10px; margin-left:10px;">
            <input type="checkbox" id="selectAll" onclick="selAll()" /> 全选
            <%--<button type="button" class="btn btn-success resetSize" onclick="selAll(false)" style="margin-left:20px;">
                取消
            </button>--%>
           <%-- <button type="button" class="btn btn-danger resetSize"  onclick="dels();" style="margin-left:20px;">
                批量删除
            </button>--%>
                        <asp:Button ID="btnDelete" runat="server"  style="margin-left:20px;" Text="批量删除" CssClass="btn resetSize btn-danger" OnClientClick="return HiConform('<strong>确定要执行删除操作吗？<p>删除后不可恢复！</p></strong>', this);" />
            <div style="display:none;">
                <asp:Button runat="server" ID="DelBtn" OnClick="Unnamed_Click" />
            </div>
        </div>

             </div>
        <div style="margin-top:5px;">
             <UI:Grid ID="grdCoupondsList"  runat="server" ShowHeader="true" AutoGenerateColumns="false"
                 DataKeyNames="ActivitiesId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered" GridLines="None" Width="100%">
                <Columns>
                    <UI:CheckBoxColumn  CellWidth="50" />
                    
                    <asp:TemplateField HeaderText="活动名称" SortExpression="ActivitiesName" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Eval("ActivitiesName")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="有效期" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="280" />
                        <ItemTemplate>
                            自<%# Eval("StartTime","{0:yyyy-MM-dd HH:mm:ss}")%> <br /> 至<%# Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}")%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="优惠方式" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="100" />
                        <ItemTemplate>
                            <%# Eval("attendType").ToString()=="0"?"普通优惠":"多级优惠"%>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="活动状态" ShowHeader="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="100" />
                        <ItemTemplate>
                            <%# Eval("sStatus")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                                                                                                

                    <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                        <ItemStyle Width="230" />
                        <ItemTemplate>
                            <input type="hidden" name="ball" value="<%# Eval("IsAllProduct") %>" />
                            <span stitle="modifyProductSpan">
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/EditProductToActivity.aspx?id={0}", Eval("ActivitiesId")))%>')" class="btn btn-info resetSize">修改宝贝</a>
                            </span>
                            <span style="display: <%# Eval("sStatus").ToString()=="未开始"?"":"none"%> ">
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddActivity.aspx?id={0}", Eval("ActivitiesId")))%>')" class="btn btn-warning resetSize">编辑</a>
                            </span>
                            <span>
                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddActivity.aspx?View=1&id={0}", Eval("ActivitiesId")))%>')" class="btn btn-info resetSize">查看</a>
                            </span>
                            <span style="display: <%# Eval("sStatus").ToString()=="进行中"?"":"none"%> ">
                                <a href="javascript:EndAct('<%#Eval("ActivitiesId")%>')" class="btn-danger btn resetSize">结束</a>
                            </span>                       
                            <span style="display: <%# Eval("sStatus").ToString()!="进行中"?"":"none"%> ">
                                 <asp:Button ID="lkDelete" runat="server" Text="删除" CommandName="Delete"  CssClass="btn btn-danger resetSize"  OnClientClick="return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" ToolTip="" /> 
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
</asp:Content>

