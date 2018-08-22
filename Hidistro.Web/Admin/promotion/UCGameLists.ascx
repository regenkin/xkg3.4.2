<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UCGameLists.ascx.cs"
    Inherits="Hidistro.UI.Web.Admin.promotion.UCGameLists" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagPrefix="uc1" TagName="ucDateTimePicker" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<script src="../js/ZeroClipboard.min.js"></script>
<script>
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

    $(function () {
        $(".imgCopy").each(function () {
            var clip = new ZeroClipboard($(this), {
                moviePath: "../js/ZeroClipboard.swf"
            });
            clip.on('complete', function (client, args) {
                HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
            });
        });
    });
    function winqrcode(url) {
        ///onsole.log(url);
        $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + encodeURIComponent(url).replace("&", "%26"));
        $('#divqrcode').modal('toggle').children().css({
            width: '300px',
            height: '300px'
        });
        $("#divqrcode").modal({ show: true });
    }
    function CopyUrl(obj) {
        var clip = new ZeroClipboard(obj, {
            moviePath: "../js/ZeroClipboard.swf"
        });
    }
    function CheckAll() {
        var check = $("#selectAll").prop('checked');
        $("input[type='checkbox']").each(function () {
            $(this).prop('checked', check);
        });

    }
    function CheckAllDel() {
        var length = $("input[type='checkbox']:checked").length;
        if (length <= 0) {
            ShowMsg("请至少选择一条要删除的数据！");
            return false;
        }
        return HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>', this);
    }
</script>
<style type="text/css">
    #ctl00_ContentPlaceHolder1_UCGameLists1_grdGameLists th {
        margin: 0px;
        border-left: 0px;
        border-right: 0px;
        background-color: #F7F7F7;
        text-align: center;
        vertical-align: middle;
    }

    #ctl00_ContentPlaceHolder1_UCGameLists1_grdGameLists td {
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

<div class="tab-pane active">
    <div style="margin-top: 10px;">
        <div class="activediv" style="background-color: #fff">
            <div class="table-page">
                <ul class="nav nav-tabs" role="tablist">
                    <li id="tabHeader_active" role="presentation" class="active">
                        <a href="?isFinished=0">活动中</a>
                    </li>
                    <li id="tabHeader_over" role="presentation">
                        <a href="?isFinished=1">已结束</a>
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
        <div id="search">
            <div class="form-inline" style="margin-left: 15px; margin-bottom: 10px; margin-top: 10px">
                活动时间：
                <uc1:ucDateTimePicker runat="server" ID="calendarStartDate" CssClass="form-control resetSize"
                    Width="130" />
                至
                <uc1:ucDateTimePicker runat="server" ID="calendarEndDate" CssClass="form-control resetSize"
                    Width="130" />

                <asp:Button CssClass="btn btn-primary resetSize" ID="btnSeach" runat="server"
                    Text="查询" OnClick="btnSeach_Click" />
            </div>
            <div class="form-inline" style="margin-bottom: 10px; margin-top: 10px; margin-left: 17px"
                id="operateDiv">
                <label onclick="CheckAll()" style="font-weight: normal;">
                    <input type="checkbox" id="selectAll" />
                    全选</label>
                <asp:Button ID="btnDel" runat="server" Text="批量删除" CssClass="btn btn-danger resetSize"
                    Style="margin-left: 20px;" OnClick="btnDel_Click" OnClientClick="return  HiConform('<strong>确定要删除选择的活动吗？</strong><p>删除活动不可恢复！</p>',this)" />
            </div>
        </div>
        <UI:Grid ID="grdGameLists" runat="server" ShowHeader="true" AutoGenerateColumns="false"
            DataKeyNames="GameId" HeaderStyle-CssClass="table_title" CssClass="table table-hover mar table-bordered"
            GridLines="None" Width="100%"
            OnRowDeleting="grdGameLists_RowDeleting"
            OnRowUpdating="grdGameLists_RowUpdating"
            OnRowDataBound="grdGameLists_RowDataBound">
            <Columns>
                <UI:CheckBoxColumn ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" CellWidth="50" />
                <asp:TemplateField HeaderText="活动名称" SortExpression="GameType">
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <a href="<%# Eval("GameUrl") %>" target="_blank"><%# Eval("GameTitle") %></a>
                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%# Eval("Status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="每人参与次数" ShowHeader="true">
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%-- <%# ((Hidistro.Entities.Promotions.PlayType)int.Parse(Eval("PlayType").ToString())).ToString() %>--%>
                        <%# GetLimit(Eval("LimitEveryDay"),Eval("MaximumDailyLimit")) %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="有效期" ShowHeader="true" HeaderStyle-Width="200">
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("BeginTime","{0:yyyy-MM-dd HH:mm:ss}")%> 至<br />
                        <%# Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}")%>
                        <asp:HiddenField ID="hfBeginTime" runat="server" Value='<%# Eval("BeginTime","{0:yyyy-MM-dd HH:mm:ss}") %>' />
                        <asp:HiddenField ID="hfEndTime" runat="server" Value='<%# Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="参与人数" ShowHeader="true">
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("TotalCount")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="中奖人数" ShowHeader="true">
                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%# Eval("PrizeCount")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="border_top border_bottom">
                    <ItemStyle CssClass="spanD spanN" VerticalAlign="Middle" HorizontalAlign="Left" Width="340" />
                    <ItemTemplate>
                        <span>
                            <img src="../images/qrcode.png" style="height: 26px; cursor: pointer;" onclick='winqrcode("<%# Eval("GameUrl") %>")' /></span>
                        <span>
                            <img src="../images/copylink.png" style="height: 26px; cursor: pointer;" class="imgCopy"
                                onclick="CopyUrl(this)" data-clipboard-target='urldata<%# Eval("GameId") %>' />
                            <input type="text" id='urldata<%# Eval("GameId") %>' placeholder="" name='urldata<%# Eval("GameId") %>'
                                value='<%# Eval("GameUrl") %>'
                                disabled="" style="display: none">
                        </span>
                        &nbsp;<span class="submit_jiage"><a href="<%# GetPrizeListsUrl(Eval("GameId").ToString()) %>"
                            class="btn btn-primary resetSize">中奖记录</a></span>
                        <span class="submit_jiage"><%# GetEditUrl(Eval("GameId").ToString(),Eval("BeginTime").ToString()) %></span>
                        <span title="FinishSpan" class="submit_jiage">
                            <asp:Button runat="server" ID="FinishBtn"
                                CssClass="btn btn-warning resetSize" CommandName="Update" Text="结束" OnClientClick="return HiConform('<strong>确定要结束选择的游戏吗？</strong><p>结束游戏不可恢复！</p>',this)"></asp:Button>
                        </span>
                        <span title="DeleteSpan" class="submit_shanchu">
                            <%--<Hi:ImageLinkButton runat="server" ID="lkDelete" CommandName="Delete" IsShow="true"
                                Text="删除" CssClass="btn btn-danger resetSize" />--%>

                            <asp:Button ID="lkDelete" runat="server" Text="删除" CommandName="Delete" CssClass="btn btn-danger resetSize" OnClientClick="return HiConform('<strong>确定要删除选择的抽奖游戏吗？</strong><p class=red>删除后，用户的参与记录也将同时删除！</p>',this)" ToolTip="" />
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </UI:Grid>
        <div class="bottomPageNumber clearfix">
            <div class="pageNumber">
                <div class="pagination" style="width: auto; float: right;">
                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                </div>
            </div>
        </div>
    </div>
</div>
<div class="modal fade" id="divqrcode">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title">活动二维码</h4>
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


