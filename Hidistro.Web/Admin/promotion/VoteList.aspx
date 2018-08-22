<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VoteList.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.VoteList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            setDisplay();

            var status = getUrlParam("status");
            var indx = parseInt(status) - 1;
            $('#nav li').eq(indx).siblings().removeClass('active').end().addClass('active');

            $('.content-table table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
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
       
        //获取url中的参数
        function getUrlParam(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
            var r = window.location.search.substr(1).match(reg);  //匹配目标参数
            if (r != null) return unescape(r[2]); return null; //返回参数值
        }

        function selAll(obj) {
            if (obj == null) {
                obj = $('#selectAll').prop('checked');
            }

            $('td[stitle="chk"]').find('input[type="checkbox"]').each(function () {
                $(this).prop('checked', obj);
            });
            $('#selectAll').prop('checked', obj);
        }

        function setDisplay() {
            //查看
            $('span[stitle="view"]').each(function () {
                var status = $(this).prev().val();
                if (status != "未开始") {
                    $(this).css('display', '');
                }
                else {
                    $(this).css('display', 'none');
                }
            });

            //开启
            $('span[stitle="start"]').each(function () {
                var status = $(this).prev().prev().val();
                if (status == "未开始") {
                    $(this).css('display', '');
                }
                else {
                    $(this).css('display', 'none');
                }
            });

            //编辑
            $('span[stitle="modify"]').each(function () {                
                var status = $(this).prev().prev().prev().val();
                if (status == "已结束") {
                    $(this).css('display', 'none');
                }
                else
                {
                    $(this).css('display', '');
                }
            });
            
            

            //结束
            $('span[stitle="stop"]').each(function () {
                var status = $(this).prev().prev().prev().prev().val();
                if (status == "进行中") {
                    $(this).css('display', '');
                }
                else {
                    $(this).css('display', 'none');
                }
            });

            //删除
            $('span[stitle="delete"]').each(function () {
                var status = $(this).prev().prev().prev().prev().prev().val();
                if (status == "进行中") {
                    $(this).css('display', 'none');
                }
                else {
                    $(this).css('display', '');
                }
            });
        }

        function del(obj, action) {
            if (action == 1) {
                if (confirm("确定要执行该删除操作吗？删除后将不可以恢复！"))
                {
                    $('#<%=txt_Ids.ClientID%>').val(obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else {
                $('#<%=txt_Ids.ClientID%>').val(obj);
                return true;
            }
        }
        
        function dels()
        {
            var ids = "";
            $('td[stitle="chk"]').find('input[type="checkbox"]').each(function () {
                if ($(this).prop('checked')) {
                    ids += "," + $(this).val();
                }
            });
            $('#<%=txt_Ids.ClientID%>').val(ids);
            $('#<%=DelBtn.ClientID%>').click();
        }
        
        function showModel(id) {
            $.ajax({
                type: "post",
                url: "GetVoteItemsHandler.ashx?id=" + id,
                data: {},
                dataType: "json",
                success: function (result) {
                    if (result.type == "success") {
                        $('#modaltitle').text(result.VoteName);
                        $('#viewDetails tbody').empty();
                        var html = [];
                        if(result.data.length > 0)
                        {
                            $(result.data).each(function (i, item) { 
                                html.push('<tr>');
                                html.push('<td>' + item.VoteItemName + '</td>');
                                html.push('<td>');
                                html.push('<div class="progress" style="margin-bottom:0px;">');
                                html.push('<div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + item.Percentage + '%;"></div>');
                                html.push('</div>');
                                html.push('</td>');
                                html.push('<td>' + item.Percentage + '%</td>');
                                html.push('<td>' + item.ItemCount + '</td>');
                                html.push('</tr>');
                            });
                        }
                        $('#viewDetails tbody').append(html.join(''));
                    }
                }
            });

            $('#previewshow').modal('toggle').children().css({
                width: '700px',
                top: '200px'
            });
        }
    </script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>投票调查</h2>            
        </div>
        <div class="blank" style="text-align: left;">
            <a href="AddVote.aspx" class="btn btn-primary">新建投票调查</a>
        </div>

        <div class="play-tabs">
            <div class="table-page">
                <ul class="nav nav-tabs" role="tablist" id="nav">
                    <li role="presentation" class="active">
                        <a href="VoteList.aspx?status=1">进行中(<asp:Label runat="server" ID="lblIn" Text="0"></asp:Label>)</a>
                    </li>
                    <li role="presentation">
                        <a href="VoteList.aspx?status=2">已结束(<asp:Label runat="server" ID="lblEnd" Text="0"></asp:Label>)</a>
                    </li>
                    <li role="presentation">
                        <a href="VoteList.aspx?status=3">未开始(<asp:Label runat="server" ID="lblUnBegin" Text="0"></asp:Label>)</a>
                    </li>                    
                </ul>

                <div class="page-box" style="margin-right: 15px;">
                <div class="page fr">
                    <div class="form-group">
                        <label for="">每页显示数量：</label>
                        <UI:PageSize runat="server" ID="hrefPageSize" />
                    </div>
                </div>
            </div>
            </div>
        </div>

        <div class="set-switch">
        <div class="form-inline" style="margin-bottom: 5px; margin-top: 5px;">
            <label>投票标题:</label>
            <asp:TextBox runat="server" CssClass="form-control resetSize mr20" ID="txt_name" placeholder="投票标题" Width="200px"></asp:TextBox>

            <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server" Text="搜索" OnClick="btnSeach_Click" />
        </div>
        </div>

        <div style="margin-bottom: 10px; margin-top:10px;display:none;">
            <input type="checkbox" id="selectAll" onclick="selAll()" /> 全选
            <button type="button" class="btn btn-success resetSize" onclick="selAll(false)" style="margin-left:20px;">
                 取消
            </button>
            <button type="button" class="btn btn-danger resetSize" onclick="dels();" style="margin-left:10px;">
                批量删除
            </button>
            <div style="display: none;">
                <asp:Button runat="server" ID="DelBtn"  OnClick="DelBtn_Click"/>
            </div>
        </div>
        
<%--        <div class="select-page clearfix" style="margin-top: 20px;">
        </div>--%>
        <div>

        <div style="display:none">
            <asp:TextBox ID="txt_Ids" runat="server"></asp:TextBox>
        </div>       
            <div class="sell-table">
                <div class="title-table">
                    <table class="table">
                        <thead>
                            <tr>
                                <th width="1%"></th>
                                <th style="vertical-align:middle;text-align:left;">投票标题</th>
                                <th width="13%">参与方式</th>
                                <th width="20%">有效期限</th>
                                <th width="7%">参与人数</th>
                                <th style="text-align: center; width: 30%;">操作</th>
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
                                        <td width="1%" stitle="chk" style="vertical-align:middle;">
                                            <%--<input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("VoteId") %>' />--%>
                                        </td>
                                        <td style="vertical-align:middle;text-align:left;">                                            
                                            <%#Eval("VoteName") %>
                                        </td>
                                        <td width="13%">
                                            <%#Eval("sType") %>
                                        </td>
                                        <td width="20%">
                                            <%# Eval("StartDate")%> <br />
                                            至 <br />
                                            <%# Eval("EndDate")%>
                                        </td>
                                        <td width="7%">
                                            <%#Eval("sAttend") %>
                                        </td>

                                        <td style="text-align: center; width: 30%;">                                           
                                            <span class="qr"><img src="../images/qrcode.png" style="height:26px; cursor:pointer;" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/BeginVote.aspx?voteId="+Eval("VoteId")%>');" /></span>
                                            <span class="qr">
                                                <input type="text" id='urldata<%# Eval("VoteId") %>' placeholder="" name='urldata<%# Eval("VoteId") %>' value='<%#"http://"+Globals.DomainName+"/BeginVote.aspx?voteId="+Eval("VoteId")%>' disabled="" style="display: none">
                                                <img src="../images/copylink.png" class="fz" style="height:26px; cursor:pointer;" data-clipboard-target='urldata<%# Eval("VoteId") %>' id='url<%# Eval("VoteId") %>' onclick="copyurl(this.id);" />
                                            </span>
                                            <span class="submit_jiage" style="display:none;"><a href="javascript:void(0)" onclick="ShowShareLink('../vshop/showactivityurl.aspx?url=<%#Server.UrlEncode(GetUrl(Eval("VoteId"))) %>')">活动链接</a></span>
                                            <input type="hidden" name="ball" value="<%# Eval("sStatus") %>" />
                                            <span stitle="view">
                                                <a href="javascript:void(0);" onclick="showModel('<%#Eval("VoteId")%>')" class="btn btn-info resetSize" title="">查看投票</a>
                                            </span>

                                            <span stitle="start">
                                                <asp:LinkButton runat="server" ID="lkStart" CommandName="Start" IsShow="true" Text="立即开始" OnClick="lkStart_Click" OnClientClick='<%#string.Format("return del({0},0);",Eval("VoteId") )%>' CssClass="btn btn-warning resetSize" />
                                            </span>

                                            <span stitle="modify">
                                                <a href='<%# Globals.GetAdminAbsolutePath(string.Format("/promotion/AddVote.aspx?id={0}", Eval("VoteId")))%>')" class="btn btn-primary resetSize" title="">编辑</a>
                                            </span>
                                           
                                            <span stitle="stop">
                                                <asp:LinkButton runat="server" ID="lkStop" CommandName="Stop" IsShow="true" Text="结束" OnClick="lkStop_Click" OnClientClick='<%#string.Format("return del({0},0);",Eval("VoteId") )%>' CssClass="btn btn-warning resetSize" />
                                            </span> 
                      
                                            <span stitle="delete">
                                               <%-- <asp:LinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true" Text="删除" OnClick="lkDelete_Click" OnClientClick='<%#string.Format("return del({0},1);",Eval("VoteId") )%>' CssClass="btn btn-danger resetSize" />--%>
                                                  <asp:Button ID="lkDelete" runat="server" Text="删除" CommandName="Delete" CommandArgument='<%# Eval("VoteId") %>' CssClass="btn btn-danger resetSize"   OnClientClick="return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" ToolTip="" /> 
                                            </span>    
                                        </td>
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
                            <UI:Pager runat="server" ShowTotalPages="true" ID="pager" />
                    </div>
                </div>
            </div>   
        </div>

        <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content form-horizontal" id="hform">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title" id="modaltitle" style="text-align: left">投票标题</h4>
                    </div>
                    <div class="modal-body">                      
                        <table class="table" id="viewDetails">
                            <thead>
                                <tr>
                                    <th>选项值</th>
                                    <th>比例示意图</th>
                                    <th>百分比</th>
                                    <th>票数</th>
                                </tr>                                
                            </thead>
                            <tbody>
                                <%--<tr>
                                    <td>aaa</td>
                                    <td>
                                        <div class="progress">
								   	        <div class="progress-bar progress-bar-danger" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 40%;">
								   	        </div>								   	
								        </div>
                                    </td>
                                    <td><span class="complete">40%</span></td>
                                    <td>1</td>
                                </tr>--%>
                            </tbody>
                        </table>
                    </div>
                    <div class="modal-footer">
                        
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal-dialog -->
        </div>

        <%-- 商品二维码--%>
        <div class="modal fade" id="divqrcode">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">投票调查二维码</h4>
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
</asp:content>
