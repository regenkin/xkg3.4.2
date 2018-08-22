<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master"
    AutoEventWireup="true" CodeBehind="WXConfigChangeBind.aspx.cs"
    Inherits="Hidistro.UI.Web.Admin.WeiXin.WXConfigChangeBind" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
    <script>
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_grdMemberList img").each(function () {
                if ($(this).attr("src") == "") {
                    $(this).attr("src", "/Utility/pics/imgnopic.jpg");
                }
            });
            $('#selectAll').click(function () {
                var check = $(this).prop('checked');
                $("input[type='checkbox']").each(function () {
                    $(this).prop('checked', check);
                });
            });
            $("#aclear").click(function () {
                $("#<%=txtUserName.ClientID%>").val('');
                $("#<%=txtPhone.ClientID%>").val('');
            });
            $('#btnphone').click(function () {
                $("#selectType").val("0");
                var checklists = $('#datalist input[type="checkbox"]:checked').size();
                if (checklists > 0) {
                    $("#h5MsgTitle").html("手机号生成用户名");
                    $("#lbType").html("手机号");
                    $("#spanCount").html(checklists);
                    checklists = undefined;
                    $('#myModal1').modal('toggle').children().css({
                        width: '530px'
                    });
                } else {
                    ShowMsg('请选择要生成用户名的会员！', false);
                }
            });
            $("#btnnichen").click(function () {
                $("#selectType").val("1");
                var checklists = $('#datalist input[type="checkbox"]:checked').size();
                if (checklists > 0) {
                    $("#h5MsgTitle").html("昵称生成用户名");
                    $("#lbType").html("昵称");
                    $("#spanCount").html(checklists);
                    checklists = undefined;
                    $('#myModal1').modal('toggle').children().css({
                        width: '530px'
                    });
                } else {
                    ShowMsg('请选择要生成用户名的会员！', false);
                }
            });
            $('#myModal').modal('toggle').children().css({
                width: '530px'
            })

        });

        function ModifyMemo(isOk) {
            if (isOk) {
                $('#datalist input[type="checkbox"]:checked').each(function () {
                    var parent = $(this).parents('tr');
                    if ($("#selectType").val() == '0') {
                        parent.find('td .txtUserName').val(parent.find('td .userPhone').text().replace(' ',''));
                    } else {
                        parent.find('td .txtUserName').val(parent.find('td .userNichen').text().replace(' ', ''));
                    }

                });
            }
        }

        function ModifyMemo1() {
            $.ajax({
                type: 'get',
                dataType: 'json',
                url: 'GetWeixinProcessor.ashx?action=getcanchangebind',
                success: function (data) {
                    console.log(data);
                    if (data.status == '1')//不能换
                    {

                    } else if (data.status == '2')//可以换绑
                    {
                        location.href = "WXConfigBindOk.aspx";
                    } else {
                        ShowMsg(data.msg, false);
                    }
                },
                error: function () {
                    ShowMsg("请求出错了,请与管理员联系！", false);
                }
            });
        }
    </script>
    <style type="text/css">
        /*.selectthis {border-color:red; color:red; border:1px solid;}*/
        .tdClass {
            text-align: center;
        }

        .labelClass {
            margin-right: 10px;
        }

        .thCss {
            text-align: center;
        }

        .selectthis {
            border: 1px solid;
            border-color: #999999;
            color: #c93027;
            margin-right: 2px;
        }

            .selectthis:hover {
                border: 1px solid;
                border-color: #999999;
                color: #c93027;
                margin-right: 2px;
            }

        .aClass {
            border: 1px solid;
            border-color: #999999;
            color: #999999;
            margin-right: 2px;
        }

            .aClass:hover {
                border: 1px solid;
                border-color: #999999;
                color: #999999;
                margin-right: 2px;
            }

        #datalist td {
            word-break: break-all;
        }

        #ctl00_ContentPlaceHolder1_grdMemberList th {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            background-color: #F7F7F7;
            text-align: center;
            vertical-align: middle;
        }

        #ctl00_ContentPlaceHolder1_grdMemberList td {
            margin: 0px;
            border-left: 0px;
            border-right: 0px;
            text-align: center;
            vertical-align: middle;
        }

        .table-bordered > thead > tr > th {
            border: none;
        }

        .modalcontext div {
            line-height: 25px;
            width: 490px;
        }

        .modalcontext p {
            margin-top: 20px;
            line-height: 25px;
            width: 100%;
            text-align: center;
        }

        .modalcontext span {
            color: red;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="form1" runat="server">
        <div class="page-header">
            <h2>更换微信公众号</h2>
        </div>
        <div class="set-switch">
            <div class="iconimg"><i class="glyphicon glyphicon-info-sign"></i></div>
            <div class="info">
                <strong>更换绑定微信公众号之前，您还需要完成以下操作</strong>
                <p>
                    店铺当前还有&nbsp;<a href="javascript:void(0)"><b><%=BindOpenIDAndNoUserNameCount%></b></a>&nbsp;个微信授权登录的会员没有关联绑定系统用户名；
                </p>
                <p>为避免会员数据丢失，请先为这些会员关联绑定系统用户名以后再进行换绑操作。</p>
            </div>
        </div>


        <!--数据列表区域-->
        <div>

            <div class="form-inline mb10">
                <div class="set-switch">
                    <div class="form-inline  mb10">
                        <div class="form-group mr20" style="margin-left: 0px;">
                            <label for="sellshop1" class="ml10">昵称：</label>
                            <asp:TextBox ID="txtUserName" CssClass="form-control resetSize" runat="server" />
                        </div>
                        <div class="form-group mr20" style="margin-left: 30px;">
                            <label for="sellshop1">手机号码：</label>
                            <asp:TextBox ID="txtPhone" CssClass="form-control resetSize" runat="server" />
                        </div>
                        <div class="form-group mr20" style="margin-left: 30px;">
                            <asp:Button ID="btnSearchButton" runat="server" Text="搜索" CssClass="btn resetSize btn-primary mr10" />
                            <a href="javascript:void(0)" id="aclear">清除条件</a>
                        </div>

                    </div>

                </div>
            </div>


            <div class="title-table">
                <div style="margin-bottom: 5px; margin-top: 10px;">
                    <div class="form-inline" id="pagesizeDiv" style="float: left; width: 100%; margin-bottom: 5px;">
                    </div>
                    <div class="page-box">
                        <div class="page fr">
                            <div class="form-group" style="margin-right: 0px; margin-left: 0px; background: #fff;">
                                <label for="exampleInputName2">每页显示数量：</label>
                                <UI:PageSize runat="server" ID="hrefPageSize" />
                            </div>
                        </div>
                    </div>
                    <div class="pageNumber" style="float: right; height: 29px; margin-bottom: 5px; display: none;">
                        <label>每页显示数量：</label>
                        <div class="pagination" style="display: none;">
                            <UI:Pager runat="server" ShowTotalPages="false" ID="pager" />
                        </div>
                    </div>

                    <div class="form-inline" style="text-align: left; margin-top: 5px; background: #fff;">
                        <label>
                            <input type="checkbox" id="selectAll" style="margin: 0px 0px 0px 17px" />
                            全选</label>
                        <input type="button" id="btnphone" class="btn resetSize btn-primary" value="手机号生成用户名" />
                        <input type="button" id="btnnichen" class="btn resetSize btn-primary" value="昵称生成用户名" />
                        <asp:Button runat="server" ID="btnBatchSave" CssClass="btn resetSize btn-primary"
                            Text="批量保存" />


                    </div>
                    <!--结束-->
                </div>
                <table class="table table-hover mar table-bordered" style="border-bottom: none;">
                    <thead>
                        <tr>
                            <th style="width: 5%; text-align: center;">
                                <span id="ctl00_ContentPlaceHolder1_grdMemberList_ctl01_label">选择</span></th>
                            <th style="text-align: center; width: 10%">微信头像</th>
                            <th style="text-align: center; width: 15%">昵称/手机</th>
                            <th style="text-align: center; width: 25%">微信OpenID</th>
                            <th style="text-align: center; width: 20%">用户名</th>
                            <th style="text-align: center; width: 15%">注册时间</th>
                            <th style="text-align: center; width: 10%">操作</th>
                        </tr>
                    </thead>
                </table>
            </div>
            <div id="datalist">

                <UI:Grid ID="grdMemberList" runat="server" ShowHeader="true" AutoGenerateColumns="false"
                    DataKeyNames="UserId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
                    GridLines="None" Width="100%">
                    <Columns>
                        <UI:CheckBoxColumn CellWidth="51" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="微信头像" ItemStyle-Width="10%" SortExpression="UserName"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <img alt="头像" src="<%# Eval("UserHead") %>" style="height: 60px; width: 60px; border-width: 0px;" />
                                <input type="text" value="<%# Eval("UserId") %>" style="display: none;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="昵称/手机" ItemStyle-Width="15%" SortExpression="UserName"
                            HeaderStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <p class="userNichen"><%# Eval("UserName").ToString()==""?"未设置":Eval("UserName") %></p>
                                <p><%# Eval("CellPhone").ToString()==""?"未绑定":Eval("CellPhone") %></p>
                                <span class="userPhone" style="display: none"><%# Eval("CellPhone") %></span>

                                <asp:HiddenField Value='<%# Eval("CellPhone")%>' runat="server" ID="hidCellPhone" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="微信OpenID" HeaderStyle-HorizontalAlign="Center" ShowHeader="true"
                            ItemStyle-Width="25%">
                            <ItemTemplate>
                                <%# Eval("OpenID").ToString()==""?"未绑定":Eval("OpenID")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="用户名" ShowHeader="true" ItemStyle-Width="20%">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="txtUserName" Text='<%# Eval("UserBindName")%>' placeholder="自定义/手机/昵称"
                                    Style="width: 95%; text-indent:0px;" CssClass="form-control resetSize txtUserName" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="注册时间" HeaderStyle-HorizontalAlign="Center" SortExpression="GradeName"
                            ItemStyle-Width="15%">
                            <ItemTemplate>
                                <itemtemplate><%# Eval("CreateDate","{0:yyyy-MM-dd<br>HH:mm:ss}")%></itemtemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">
                            <ItemStyle CssClass=" " />
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton1" runat="server" CommandName="Update">保存</asp:LinkButton>
                                <input id="hdUserId" type="hidden" value="<%# Eval("UserID") %>" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </UI:Grid>

            </div>
        </div>
        <!--数据列表底部功能区域-->

        <input type="hidden" id="hdUserId" runat="server" value="" />
        <asp:Button ID="huifuUser" Text="huifu" runat="server" Style="display: none" />
        <asp:Button ID="BatchHuifu" Text="huifu" runat="server" Style="display: none" />

        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>


        <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal1">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="w-modalbox">
                        <h5 id="h5MsgTitle"></h5>
                        <div class="titileBorderBox borderSolidB">
                            <div class="contentBox pl20 modalcontext">
                                <div>
                                    生成会员的用户名后，会员可以通过用户名登录店铺，避免店铺更换绑定微信众账号后，会员的个人信息丢失。
                                </div>
                                <p>您已选择将<span id="spanCount"></span>位会员的<label id="lbType"></label>设置为会员用户名。</p>
                                <input type="hidden" id="selectType" />
                            </div>
                        </div>
                        <div class="y-ikown pt10 pb10">

                            <input type="submit" value="暂不生成" onclick="return ModifyMemo(false);"
                                class="btn btn-primary inputw100" data-dismiss="modal">

                            <input type="submit" value="开始生成" onclick="return ModifyMemo(true);"
                                class="btn btn-success inputw100" data-dismiss="modal">
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
</asp:Content>
