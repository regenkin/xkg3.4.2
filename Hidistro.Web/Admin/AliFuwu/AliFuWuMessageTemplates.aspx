<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" Debug="true" CodeBehind="AliFuWuMessageTemplates.aspx.cs" 
    Inherits="Hidistro.UI.Web.Admin.AliFuwu.AliFuWuMessageTemplates" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />
    <Hi:Script ID="Script5" runat="server" Src="/utility/jquery.artDialog.js" />
    <Hi:Script ID="Script6" runat="server" Src="/utility/Window.js" />
<script>
    $(function () {
        $('[MessageType="会员注册时"]').hide();
    });
</script>	
    <style type="text/css">
        .auto-style1 {
            height: 26px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>消息模板设置</h2>
        </div>
        <div class="mate-tabl">
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#home" aria-controls="home" data-toggle="tab">基本设置</a></li>
                <li role="presentation"><a href="#profile" aria-controls="profile" data-toggle="tab" id="aMessageSet">消息设置</a></li>
            </ul>
             <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="home">
                            <p class="y-textborderleft">您可以根据运营者角色绑定支付宝服务窗用于接收消息提醒</p>
                            <div class="y-tabinerboxsame mt20 mb20 pl20"  >
                                <a class="btn btn-success bindingmicrochannel">绑定运营者支付宝服务窗</a>
                                <div class="y-charttable mt10">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th width="10%">姓名</th>
                                                <th width="15%">运营者角色</th>
                                                <th width="50%">接收消息类型</th>
                                                <th width="*" style="text-align:center;">操作</th>
                                            </tr> 
                                        </thead>
                                        <tbody>
                                            <asp:Repeater runat="server" ID="rptAdminUserList" OnItemCommand="rptAdminUserList_ItemCommand" OnItemDataBound="rptAdminUserList_ItemDataBound" >
                                                <ItemTemplate>
                                                    <tr>
                                                        <td><%#Eval("RealName") %><asp:HiddenField ID="hdfAutoID" runat="server" /></td>
                                                       <td><%#Eval("RoleName") %></td>
                                                        <td>
                                                            <ul class="clearfix">
                                                             
                                                                <li <%#Eval("Msg1").ToString()!="1"? "style=display:none;" :"" %> >新订单提醒</li>
                                                                <li <%#Eval("Msg2").ToString()!="1"? "style=\"display:none;\"" :"" %> >订单付款提醒</li>
                                                                <li <%#Eval("Msg3").ToString()!="1"? "style=\"display:none;\"" :"" %> >退款申请</li>

                                                                <li <%#Eval("Msg5").ToString()!="1"? "style=\"display:none;\"" :"" %> >用户咨询提醒</li>
                                                                <li <%#Eval("Msg4").ToString()!="1"? "style=\"display:none;\"" :"" %> >提现申请提醒</li>
                                                                <li <%#Eval("Msg6").ToString()!="1"? "style=\"display:none;\"" :"" %> >分销商申请成功提醒</li>

                                                             
                                                            </ul>
                                                             <span style="color:blue;display:none;">
                                                                <asp:Literal runat="server" ID="lbMsgList" Text='<%#Eval("MsgDesc1") + "&nbsp;&nbsp;&nbsp;&nbsp;" 
                                                                + Eval("MsgDesc2")+ "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                    + Eval("MsgDesc3")+ "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                    + Eval("MsgDesc4")+ "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                    + Eval("MsgDesc5")+ "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                    + Eval("MsgDesc6")+ "&nbsp;&nbsp;&nbsp;&nbsp;"
                                                                %>' />
                                                             </span>
                                                        </td>
                                                        <td>
                                                            <div class="y-messagesetting clearfix">
                                                                <div class="fl mr5">
                                                                    <span class="rela">更改消息类型<i class="line"></i></span>
                                                                    <div class="absol change">
                                                                         <asp:TextBox  runat="server" ID="txtUserOpenId" Text='<%#Eval("UserOpenId") %>' Visible="false"  /> 
                                                                         <asp:TextBox  runat="server" ID="txtRealName" Text='<%#Eval("RealName") %>'  Visible="false" /><p></p>
                                   
                                                                        <p class="mb10 admin">&nbsp;&nbsp;&nbsp;运营者角色：
                                                                            <asp:TextBox  runat="server" ID="txtRoleName" Text='<%#Eval("RoleName") %>' /></p>
                                                                        <div class="clearfix">
                                                                           
                                                                                <label class="middle">
                                                                                    <asp:CheckBox runat="server" ID="cbMsg1" />新订单提醒
                                                                                </label>
                                                                                <label class="middle">
                                                                                    <asp:CheckBox runat="server" ID="cbMsg2" />订单付款提醒
                                                                                </label>
                                                                                <label class="middle">
                                                                                    <asp:CheckBox runat="server" ID="cbMsg3" />退货申请提醒
                                                                                </label>
                                                                                <label class="middle">
                                                                                    <asp:CheckBox runat="server" ID="cbMsg5" />用户咨询提醒
                                                                                </label>
                                                                                <label class="middle">
                                                                                    <asp:CheckBox runat="server" ID="cbMsg4" />提现申请提醒
                                                                                </label>
                                                                                <label class="middle">
                                                                                    <asp:CheckBox runat="server" ID="cbMsg6" />分销商申请成功
                                                                                </label>

 
                                                                    
                                                                        </div>
                                                                        <div class="btn" style="text-align: center;">
                                                                            
                                                                            <asp:Button runat="server" ID="btnSaveRoleRow" Text="保存" CommandName="Save" CssClass="btn btn-primary btn-sm inputw100" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                <span class="fl">|</span>
                                                                <div class="fl ml5">
                                                                    <span class="rela">取消绑定<i class="line"></i></span>
                                                                    <div class="cancel" style="text-align:center;">
                                                                        <button class="btn btn-primary btn-sm y-setw">放弃</button>
                                                                   
                                                                        <asp:Button runat="server" ID="btnDelete" Text="取消绑定" CommandName="Delete" CssClass="btn btn-success btn-sm y-setw" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <p class="y-textborderleft">支付宝服务窗消息模板ID设置<a href="../AliFuwu/AliFuWuSettings.html" target="_blank">(如何获得支付宝服务窗消息模板ID？)</a></p>
                            <div class="mt20 y-charttable pb100 pl20">
                                <table class="table" id="tbTemplatesList">
                                    <thead>
                                        <tr>
                                            <th width="25%">消息标题</th>
                                            <th width="25%">模板编号</th>
                                            <th width="50%">支付宝服务窗模板ID</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                         <asp:Repeater runat="server" ID="rptAliFuWuMessageTemplateList">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("Name") %><asp:HiddenField ID="hdfMessageType" runat="server" Value='<%#Eval("MessageType") %>'  /> </td>
                                                    <td><%#Eval("WXOpenTM") %></td>
                                                    <td><asp:TextBox runat="server"  ID="txtTemplateId" Text='<%#Eval("WeixinTemplateId") %>'  CssClass="inputw350" /></td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                 
                                    </tbody>
                                </table>

                                                    
                            </div>
                            <div class="footer-btn navbar-fixed-bottom">
                                <%--<button type="button" class="btn btn-success" id="btn-save2">保存设置</button>--%>
                                <asp:Button ID="btnSaveTemplatesList" runat="server" Text="保存模板列表" CssClass="btn btn-success"  OnClick="btnSaveTemplatesList_Click" />
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="profile">
                            <div class="tablemessagesetting">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th align="center">消息接收对象</th>
                                            <th width="85%">消息类型</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td align="center">分销商</td>
                                            <td>
                                                <asp:CheckBoxList  runat="server" ID="cbPowerListDistributors"  DataTextField="DetailName" DataValueField="DetailType" RepeatColumns="5"  
                                                     CssClass="middle"
                                                     />

                                           <%--  <%#Eval("RoleName") %>--%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">会员</td>
                                            <td>
                                                <%-- <li style='display:none;' >新订单提醒1</li>
                                                                <li 'style="display:none;"' >退货申请提醒2</li>
                                                                <li 'style="display:none;"' >提现申请提醒3</li>--%>
                                                <asp:CheckBoxList  runat="server" ID="cbPowerListMember"  DataTextField="DetailName" DataValueField="DetailType" RepeatColumns="5"  
                                                     CssClass="middle" BorderStyle="None" 
                                                     />
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="footer-btn navbar-fixed-bottom">
                                <asp:Button ID="btnSaveUserMsgDetail" runat="server" Text="保存消息设置" CssClass="btn btn-success"  OnClick="btnSaveUserMsgDetail_Click"/>
                            </div>
                        </div>
                    </div>
        </div>

        <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal"  onclick="CloseScan();">
        <div class="modal-dialog modal-sm" style="width:500px;">
            <div class="modal-content" id="divMiddle">
                <div class="w-modalbox">
                    <h5 >绑定运营者支付宝服务窗</h5>
                    <div class="y-wechatstep">
                        <ul class="clearfix">
                            <li class="triangle active clearfix"  id="liStep1">1&nbsp;绑定支付宝服务窗</li>
                            <li class=""   id="liStep2">2&nbsp;选中接收消息类型</li>
                        </ul>
                    </div>
                    <div class="wechatstepcontentbox">
                        <div class="wechatstepcontent active">
                            <h6 class="mb5">扫描二维码后，将自动获取绑定授权。成功获取授权后，可点击下一步继续</h6>
                            <div class="y-qrcode" id="divQRCode">
                                <asp:Image ID="imgQRCode" runat="server"  Width="170px" Height="170px" />
                            </div>

                            <div class="y-qrcode" id="divHeadImage" style="display:none;">
                                <asp:Image ID="imgHeadImage" runat="server"  Width="170px" Height="170px"/>
                            </div>

                            <label>
                                运营者支付宝服务窗OpenID：
                                <%--<label id="lbUserOpenID" /><br />--%>
                               <%-- <input id="lbUserOpenID" class="inputw200" type="text" placeholder="扫描上方二维码或手动输入">--%>
                                <asp:TextBox runat="server"  class="inputw300" ID="txtScanOpenID" placeholder="扫描上方二维码或手动输入"  /><br />
                                  <input type="hidden" id="hfIsSearching" value="0" />
                                 <asp:HiddenField ID="hiddSceneId" runat="server" />
                                <%--昵称：--%><%--<label id="lbUserNickName"></label>--%>
                            </label>
                                     <label  id="lbScanInfo"  style="color:red; margin-left:20px;"></label>
                                     <asp:HiddenField ID="hfWeiXinAccessToken" runat="server" Value="" />
                                     <%--<asp:HiddenField ID="hfCurrOpenID" runat="server" Value="" />--%>
                                     <asp:HiddenField ID="hfAppID" runat="server" />
                            
                            <p class="mt10 mb5" style="color:#999999;font-size:12px;">openid号可以在<a href="../member/managemembers.aspx" target="memberList" >会员列表</a>中指定的会员详情中查看并复制；</p>
                            <p style="color:#999999;font-size:12px;">用运营者扫描二维码，也可用手机拍照后，将二维码发送给运营者扫描</p>
                        </div>

                        <div class="wechatstepcontent">
                            <h6 class="mb20">为运营者指定接收消息的类型，可多选</h6>
                            <div class="form-horizontal mb10">
                                <div class="form-group">
                                    <label class="col-xs-3 pad resetSize control-label" for="pausername">运营者姓名：</label>
                                    <div class="col-xs-9">
                                        <asp:TextBox ID="txtAdminName" CssClass="form-control resetSize inputw150" runat="server"/>
<%--                                        <asp:TextBox ID="txtManageOpenID" CssClass="form-control inputw300" runat="server"  Visible="false" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal mb10">
                                <div class="form-group">
                                    <label class="col-xs-3 pad resetSize control-label" for="pausername">运营者角色：</label>
                                    <div class="col-xs-9">
                                     
                                        <asp:TextBox ID="txtAdminRole" CssClass="form-control resetSize inputw150" runat="server"/>
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal mb10">
                                <div class="form-group">
                                    <label class="col-xs-3 pad resetSize control-label" for="pausername">选择消息类型：</label>
                                    <div class="col-xs-9">
                                        <label class="middle mb5 mr10">
                                            <asp:CheckBox runat="server" ID="cbMsg1" Text="新订单提醒" />
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <asp:CheckBox runat="server" ID="cbMsg2" Text="订单付款提醒" />
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <asp:CheckBox runat="server" ID="cbMsg3" Text="退货申请提醒" />
                                        </label> 
                                        <label class="middle mb5 mr10">
                                            <asp:CheckBox runat="server" ID="cbMsg5" Text="用户咨询提醒" />
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <asp:CheckBox runat="server" ID="cbMsg4" Text="提现申请提醒" />
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <asp:CheckBox runat="server" ID="cbMsg6" Text="分销商申请成功提醒" />
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="y-ikown y-wxbdbtn active pt10 pb10">
                        <%--<a href="javascript:CloseWin();" class="btn btn-primary inputw100" data-dismiss="modal" 
                            aria-label="Close"

                            >暂不绑定22</a>--%>

                        <%--<input type="submit" value="暂不绑定" class="btn btn-primary inputw100" data-dismiss="modal">--%>
                        <a href="javascript:void(0)"  data-id="close" class="btn btn-primary inputw100" data-dismiss="modal">暂不绑定</a>
                        <%--<input type="submit" value="保存至下一步" class="btn btn-success inputw100 y-nextstep">--%>
                        <a    class="btn btn-success inputw100 y-nextstep" onclick="return CheckStep(1);">保存至下一步</a>
                    </div>
                    <div class="y-ikown y-wxbdbtn pt10 pb10">
                        <%--<input type="submit" value="上一步" class="btn btn-primary inputw100 y-pverstep">--%>

                         <%--<a    class="btn btn-primary inputw100 y-pverstep" style="display:none;">上一步</a>--%>
                        <a href="javascript:void(0)"  data-id="close" class="btn btn-primary inputw100" data-dismiss="modal">暂不绑定</a>

                        <%--<input type="submit" value="保存" class="btn btn-success inputw100" data-dismiss="modal">--%>
                        <asp:Button ID="btnSaveRole" runat="server"  OnClientClick="return CheckStep(2);"  Text="保存" CssClass="btn btn-success inputw100"  OnClick="btnSaveRole_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
    </form>

    
<script type="text/javascript">

    function CloseWin() {
        $("#myModal").css("display", "none");
        return true;
    }
    function CheckStep(FuncID) {
        //alert(11);
        //if (FuncID == 1)  //检测是否扫描或输入了OpenId
        //{
        //    alert(12);
        //    if ($("#ctl00_ContentPlaceHolder1_txtScanOpenID").val() == "")
        //    {
        //        ShowMsg("请扫描或输入OpenId！", false);

        //        return false;
        //    }
        //}
        //else 
        if (FuncID == 2)  //
        {
            if ($("#ctl00_ContentPlaceHolder1_txtAdminName").val() == ""
                || $("#ctl00_ContentPlaceHolder1_txtAdminRole").val() == "") {
                ShowMsg("运营者姓名和角色都不可为空！", false);
                return false;
            }


        }

        return true;
    }
</script>

<script type="text/javascript">
    $(function () {
        $('.bindingmicrochannel').click(function () {
            $('#myModal').modal('toggle').children().css({
                width: '530px'
            });

            ShowScan();
        });
        $('.y-nextstep').click(function () {


            if ($("#ctl00_ContentPlaceHolder1_txtScanOpenID").val() == "") {
                ShowMsg("请扫描或输入OpenId！", false);

                return false;
            }

            $('.y-wechatstep ul li').removeClass('active').last().addClass('active');
            $('.wechatstepcontentbox .wechatstepcontent').removeClass('active').last().addClass('active');
            $('.y-wxbdbtn').removeClass('active').last().addClass('active');
        })
        $('.y-pverstep').click(function () {
            $('.y-wechatstep ul li').removeClass('active').first().addClass('active');
            $('.wechatstepcontentbox .wechatstepcontent').removeClass('active').first().addClass('active');
            $('.y-wxbdbtn').removeClass('active').first().addClass('active');
        })
    })
</script>

<script type="text/javascript">
    function CloseScan() {
        $("#hfIsSearching").val("9");
    }

    function ClearScanInfo() {
        $("#hfIsSearching").val("0");
        //$("#ctl00_ContentPlaceHolder1_txtScanOpenID").val("");
        //$("#txtScanOpenID").val("aaaaa");
        var txtOpenID = "<%=txtScanOpenID.ClientID%>";  // txtManageOpenID
        $("#" + txtOpenID).val("");


        $("#divQRCode").css("display", "block");  //显示二维码
        $("#divHeadImage").css("display", "none");  //隐藏头像


        var AppIDControl = "<%=hfAppID.ClientID%>";
        var AppID = $("#" + AppIDControl).val();

        $.ajax({
            url: "/api/VshopProcess.ashx",
            type: "post",
            data: "action=clearqrcodescaninfo&AppID=" + AppID,
            datatype: "json",
            success: function (json) {
                //
            },
            error: function () {
                //
            }
        });
        //
    };
    $('#divMiddle').click(function (e) {
        //alert($(e.target).attr('data-id'));
        if ($(e.target).attr('data-id') != 'close') {
            e.stopPropagation();
        }
    })

    $('#aMessageSet').click(function (e) {

        var NotSetCount = 0; //未设置的模板数
        $("#tbTemplatesList").find("input[type='text']").each(function () {
            if ($(this).val().trim() == "") {
                NotSetCount++;
                ShowMsg("消息模板ID设置未完成，无法进行当前操作。", false);

                e.stopPropagation();
                return false;
            }
        });

        return true;


    })

    function SearchOpenID() {

        //alert("SearchOpenID()...");

        $("#hfIsSearching").val("1");  //正在查询后台本次扫描信息


        //alert("0.1...");

        var myDate = new Date();
        var mytime = myDate.toLocaleTimeString();     //获取当前时间


        //alert("0.2...");


        var FirstInfo = ""; // "请扫描..." + getNowFormatDate();


        //alert("0.3...");
        $("#lbScanInfo").text(FirstInfo);


        //alert("aaa");

        $("#txtScanOpenID").val("");

        //alert("bbb");
        //$("#lbUserNickName").text("");


        //alert("ccc");

        var AppIDControl = "<%=hfAppID.ClientID%>";
        var AppID = $("#" + AppIDControl).val();
        var scene = "<%=hiddSceneId.ClientID%>"
        var sceneId = $("#" + scene).val();
        // alert("ddd AppID=" + AppID );
        $.ajax({
            url: "/api/VshopProcess.ashx",
            type: "post",
            data: "action=getalifuwuqrcodescaninfo&sceneId=" + sceneId,
            datatype: "json",
            async: false,//ty
            success: function (json) {
                //alert("成功返回了扫描信息。。。");
                if (json.Status == 1) {
                    //alert("Status=" + json.Status);

                    var ScanInfo = "扫描成功！";


                    $("#lbScanInfo").text(ScanInfo);

                    //alert("it=" + json.OpenID);

                     var txtOpenID = "<%=txtScanOpenID.ClientID%>";  // txtManageOpenID

                    $("#" + txtOpenID).val(json.SceneId);

                    $("#hfIsSearching").val("9");  //9表示成功扫描

                    $("#divQRCode").css("display", "none");  //隐藏二维码
                    $("#divHeadImage").css("display", "block");  //显示头像


                    //var OpenID = "", NickName = "";
                    //var UserHead = "";
                    //if (json.UserHead != "") {
                    //    UserHead = json.UserHead;
                    //    //ScanInfo = ScanInfo + "&nbsp;UserHead：" + json.UserHead;
                    //}


                    <%--if (true) {
                        OpenID = json.OpenID;
                        NickName = json.NickName;

                        var strToken = "<%= hfWeiXinAccessToken.ClientID %>";
                        var ACCESS_TOKEN = $("#" + strToken).val();

                        //$("#lbUserOpenID").text(OpenID);
                        //$("#lbUserOpenID").val(OpenID);
                        $("#txtScanOpenID").val(OpenID);

                        //$("#lbUserNickName").text(NickName);  //昵称

                        //显示头像信息等
                        $("#divHeadImage").css("display", "block");
                        var strimgHeadImage = "<%=imgHeadImage.ClientID %>";

                        $("#" + strimgHeadImage).attr("src", UserHead);
                    }
                    else {
                        //
                    }--%>


                }
                else {
                    $("#hfIsSearching").val("0");
                }
                //$(".nav.nav-tabs").show();
            },
            error: function () {
                //alert("error");
                $("#hfIsSearching").val("0");
            }
        });
        //$("[data-toggle='tooltip']").tooltip({ html: false });
    };

    function ShowScan() {
        $('.y-wechatstep ul li').removeClass('active').first().addClass('active');
        $('.wechatstepcontentbox .wechatstepcontent').removeClass('active').first().addClass('active');
        $('.y-wxbdbtn').removeClass('active').first().addClass('active');

        ClearScanInfo();  //清除以往扫描记录

        $("#lbScanInfo").text("等待扫描中1...");


        var txtOpenID = "<%=txtScanOpenID.ClientID%>";  // txtManageOpenID
        $("#" + txtOpenID).val("");

        $("#divQRCode").css("display", "block");
        $("#divHeadImage").css("display", "none");

        $("#hfIsSearching").val("0");


        $("#lbScanInfo").text("等待扫描中2...");


        //alert("ShowScan...2");

        startSearch();
    }

    function getNowFormatDate() {
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }


        //分
        var nMin = date.getMinutes();
        var strMin = nMin;
        if (nMin >= 0 && nMin <= 9) {
            strMin = "0" + nMin;
        }


        //秒
        var nSec = date.getSeconds();
        var strSec = nSec;
        if (nSec >= 0 && nSec <= 9) {
            strSec = "0" + nSec;
        }

        //var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
        //        + " " + date.getHours() + seperator2 + date.getMinutes()
        //        + seperator2 + date.getSeconds();

        var currentdate = date.getHours() + seperator2 + strMin + seperator2 + strSec;
        return currentdate;
    }

    //使用setTimeout超时调用 
    function startSearch() {
        var myDate = new Date();
        var mytime = myDate.toLocaleTimeString();     //获取当前时间

        mytime = getNowFormatDate();
        $("#lbScanInfo").text("等待扫描中..." + mytime);

        //$("#currentTime").text(new Date().toLocaleString());

        if ($("#hfIsSearching").val() != "1" && $("#hfIsSearching").val() != "9") {
            SearchOpenID();
        }

        if ($("#hfIsSearching").val() == "9") {
            $("#lbScanInfo").text("扫描已完成。");

            return;
        }
        setTimeout('startSearch()', 2000);
    }

</script>

</asp:Content>
