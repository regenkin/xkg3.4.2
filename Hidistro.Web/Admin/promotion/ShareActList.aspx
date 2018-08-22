<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShareActList.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.ShareActList" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
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
            var status = getUrlParam("status");
            $('#nav li').eq(status).siblings().removeClass('active').end().addClass('active');
        });

        function setEnable(obj) {
            var type = "1";
            var ob = $("#" + obj.id);
            var cls = ob.attr("class");
            var enable = "false";
            if (cls == "switch-btn") {

                ob.empty();
                ob.append("已关闭 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn off");
                enable = "false";

            }
            else {
                ob.empty();
                ob.append("已开启 <i></i>")
                ob.removeClass();
                ob.addClass("switch-btn");
                enable = "true";
            }

            $.ajax({
                type: "post",
                url: "ConfigHandler.ashx",
                data: { type: type, enable: enable },
                dataType: "text",
                success: function (data) {
                    if (enable == 'true') {
                        ShowMsg('分享助力已开启！', true);
                        $('#mainDiv').css('display', '');
                    }
                    else {
                         ShowMsg('分享助力已关闭！',true);
                         $('#mainDiv').css('display', 'none');
                    }
                }
            });
        }


        function selAll(obj) {
            if (obj == null) {
                obj = $('#selectAll').prop('checked');
            }
            $('td[title="chk"]').find('input[type="checkbox"]').each(function () {
                $(this).prop('checked', obj);
            });
            $('#selectAll').prop('checked', obj);
        }

        function del(obj) {
            if (!confirm('确定要执行该删除操作吗？删除后将不可以恢复！')) {
                return false;
            }
            else {
                $('#<%=txt_Ids.ClientID%>').val(obj);
                return true;
            }
        }

        function dels() {
            if (!confirm('确定要执行该删除操作吗？删除后将不可以恢复！')) {
                return false;
            }
            else {
                var ids = [];
                $('td[title="chk"]').find('input[type="checkbox"]').each(function () {
                    if ($(this).prop('checked')) {
                        ids.push($(this).val());
                    }
                });
                if (ids.length == 0) {
                    ShowMsg('请选择活动！', false);
                    return;
                }
                else {
                    $('#<%=txt_Ids.ClientID%>').val(ids.join(','));
                    $('#<%=DelBtn.ClientID%>').click();
                }
            }
        }
      
        //获取url中的参数
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>分享助力</h2>          
            <%--<small>活动时间内，会员单笔订单金额达到您设置的条件以后，就可以在自己的朋友圈分享优惠券给好友。</small> 
            <small>多个活动同时进行时，按满足金额倒序排，取第一个满足条件的活动。</small> --%>  
        </div>                

            <div id="mainDiv">
                <div class="blank" style="text-align: left;">
                    <a href="AddShareAct.aspx" class="btn btn-primary">添加分享助力活动</a>
                </div>

                <div class="activediv"  style="background-color:#fff">

           
                <div class="set-switch">
                <div class="form-inline">
                    <label>优惠券名称:</label>
                    <asp:TextBox runat="server" CssClass="form-control resetSize mr20" ID="txt_name" placeholder="优惠券名称" Width="110px"></asp:TextBox>                   
                    <asp:Button CssClass="btn btn-primary resetSize"  ID="btnSeach" runat="server" Text="搜索" />
                </div>
                </div>

                <div class="play-tabs">
                    <div class="table-page">
                        <ul class="nav nav-tabs" role="tablist" id="nav">
                            <li role="presentation" class="active">
                                <a href="ShareActList.aspx?status=0">所有活动(<asp:Label runat="server" ID="lblAll" Text="0"></asp:Label>)</a>
                            </li>
                            <li role="presentation" >
                                <a href="ShareActList.aspx?status=1">进行中(<asp:Label runat="server" ID="lblIn" Text="0"></asp:Label>)</a>
                            </li>
                            <li role="presentation">
                                <a href="ShareActList.aspx?status=2">已结束(<asp:Label runat="server" ID="lblEnd" Text="0"></asp:Label>)</a>
                            </li>
                            <li role="presentation">
                                <a href="ShareActList.aspx?status=3">未开始(<asp:Label runat="server" ID="lblUnBegin" Text="0"></asp:Label>)</a>
                            </li>                    
                        </ul>

                        <div class="page-box" style="margin-right: 15px;">
                        <div class="page fr">
                            <div class="form-group">
                                <label for="exampleInputName2">每页显示数量：</label>
                                <UI:PageSize runat="server" ID="PageSize1" />
                            </div>
                        </div>
                    </div>
                    </div>
                </div>

                <div class="form-inline" style="margin-top:10px;margin-bottom:10px;margin-left:5px;">
                    <input type="checkbox" id="selectAll" onclick="selAll()" /> 全选
                  <%--  <button type="button" class="btn btn-danger resetSize" onclick="dels();" style="margin-left:20px;">
                        批量删除
                    </button>--%>
                    <asp:Button ID="lkDelete" runat="server" Text="批量删除"  CssClass="btn btn-danger resetSize"    OnClientClick="return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" ToolTip="" /> 
                    <div style="display: none;">
                        <asp:Button runat="server" ID="DelBtn"/>
                        <asp:TextBox ID="txt_Ids" runat="server"></asp:TextBox>
                    </div>                          
                </div>
                     </div>
                <div class="sell-table">
                <div class="title-table">
                    <table class="table">
                        <thead>
                            <tr>
                                <th width="2%"></th>
                                <th width="15%" style="vertical-align:middle; text-align:left;">活动名称</th>
                                <th width="20%">发券需订单金额满</th>
                                <th width="15%">发券满额送券</th>
                                <th width="28%">有效期</th>  
                                <th width="20%">操作</th>
                            </tr>
                        </thead>
                    </table>
                </div>


                <div class="content-table">
                    <table class="table">
                        <tbody>
                            <asp:Repeater ID="grdDate" runat="server">
                                <ItemTemplate>
                                    <tr>
                                        <td width="2%" title="chk" style="vertical-align:central;">
                                            <input name="CheckBoxGroup" type="checkbox" value='<%#Eval("Id") %>' />
                                        </td>
                                        <td width="15%" style="vertical-align:middle; text-align:left;">                                         
                                            <%#Eval("ActivityName") %>
                                        </td>
                                        <td width="20%">
                                            <%#Eval("MeetValue") %>
                                        </td>
                                        <td width="15%">
                                            <%# Eval("CouponNumber")%>
                                        </td>
                                        <td width="28%">
                                            <%# Eval("BeginDate")%> 至 <%# Eval("EndDate")%>
                                        </td>             

                                        <td  title="controls" style="text-align: center; width: 20%;">
                                            <span class="submit_jiage mr20">
                                                <a class="btn btn-warning resetSize" href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddShareAct.aspx?id={0}", Eval("Id")))%>')">编辑</a>
                                            </span>
                      
                                            <%--<span title="DeleteSpan" class="submit_shanchu">
                                                <Hi:ImageLinkButton runat="server" ID="lkDelete" CssClass="btn btn-warning btn-sm" CommandName="Delete" IsShow="true" Text="删除"  OnClick="lkDelete_Click" />
                                            </span>--%>
                                            <asp:Button ID="lkDelete" runat="server" Text="删除" CommandName="Delete" CssClass="btn btn-danger resetSize"  CommandArgument='<%# Eval("Id")%>'  OnClientClick="return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" ToolTip="" /> 
                                        </td>
                                    </tr>
                                    <tr style="display: none;">

                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>

                        </tbody>
                    </table>
                </div>
            </div>


                <div class="bottomPageNumber clearfix">
                    <div class="pageNumber">
                        <div class="pagination" style="width: auto">
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                        </div>
                    </div>
                </div>
   

            </div>
    </form>
</asp:content>




