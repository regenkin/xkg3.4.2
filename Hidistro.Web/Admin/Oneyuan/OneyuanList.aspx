<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master" CodeBehind="OneyuanList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Oneyuan.OneyuanList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%@ Import Namespace="Hidistro.SaleSystem.Vshop" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .tableRow{border:1px solid #ccc}
        .tableRow tr td{padding-left:5px!important}
        .tableRow tr:hover{background:#def3e1}
        .table th{text-align:left!important;padding-left:5px!important}
        .success{color:#13b743}
        .green{color:#4cff00}
        .normal{color:#888}
        .errcss{color:#ff6a00}
       .red{border:1px solid #ff6a00;padding:5px;margin-top:5px;display:block;border-radius:4px}
       body{padding-right:0px!important}
    </style>
    <script>

        function AView(Aid) {
            window.location.href = "AddOneyuanInfo.aspx?vaid=" + Aid;

        }
        function AEdit(Aid) {
            window.location.href = "AddOneyuanInfo.aspx?aid=" + Aid;
        }
        function AStart(Aid) {
            var DataJson = { action: "Start", Aid: Aid };

            HiTipsShow("确定开启活动？", "confirmII", function () {

                AjaxPost(DataJson);
            });
           
        }


        function AEnd(Aid,Fnum,Rtype,Rnum) {
            var DataJson = { action: "End", Aid: Aid };

            var msg = "确定提前结束活动？";
          
            if (Fnum == 0)
            {
                //如果没有人员参与，直接结束，并清除未付款参与者信息
                HiTipsShow(msg, "confirmII", function () {
                    AjaxPost(DataJson);
                });
            }
            else
            {
                //如果有人参与，判断开奖类型
                var CanAward=false;
                if ((Rtype != 2 && Fnum >= Rnum) || Rtype == 2) {
                    CanAward=true;
                }
                
                if (CanAward)
                {
                    msg += "<br/><span class='red'>当前活动已有" + Fnum + "人参加并付款,已达设置开奖条件！</span>"

                }
                else{
                    msg += "<br/><span  class='red'>当前活动已有" + Fnum + "人参加并付款,尚未达到开奖条件！</span>"
                }


                $("#EndBody").html(msg);
                $("#ActivityEnd").modal("show").children().css({
                    width: '400px', top: "100px"
                });;

                // DrawEnd
                // RefundEnd
                DataJson.action = "EndII";
                $("#DrawEnd").unbind("click").click(function () {
                    DataJson.CanDraw = 1; //立即开奖
                    $("#ActivityEnd").modal("toggle");
                    AjaxPost(DataJson);
                });

                $("#RefundEnd").unbind("click").click(function () {
                    DataJson.CanDraw = 0; //退款
                    $("#ActivityEnd").modal("toggle");
                    AjaxPost(DataJson, null, function (Data) {

                        if (Data.state) {
                            BatchRefund(Aid); //回调成功后，再调用批量退款功能
                        }
                        else {
                            HiTipsShow(Data.msg, "warning");
                        }
                     
                    });
                });
              

            } 
        }


        function ADel(Aid) {
            var DataJson = { action: "Del", Aid: Aid };
            HiTipsShow("确定删除活动？", "confirmII", function () {
                AjaxPost(DataJson);
            });
        }
       
        function BatchRefund(Aid) {
            var DataJson = { action: "BatchRefund", vaid: Aid };
            HiTipsShow("确定批量退款？确定后微信支付将直接在后台处理退款，支付宝将转向支付宝服务器处理退款！", "confirmII", function () {
                AjaxPost(DataJson, "RefoundList.aspx", function (data) {
                    if (data.state) {
                        HiTipsShow(data.msg, "success", function () {
                          //  window.location.reload(); //重新加载当前页
                            if (data.alipay) {
                                window.open("../OutPay/OneyungRefund.aspx?vaid=" + Aid); //转到支付宝退款
                            }
                        });
                    } else {
                        HiTipsShow(data.msg, "error", function () {
                            window.location.reload();
                        });
                    };

                }); //在REFUND页面处理
            });
        }

        //批量删除
        function BatchDel()
        {
            if ($("input[name=CheckBoxGroup]:checked").length < 1) {
                HiTipsShow("没有活动被选择！请先选择需要删除的活动", "warning");
                return;
            }

            var Aids = [];

            $("input[name=CheckBoxGroup]:checked").each(function () {
                var candel = $(this).attr("candel");

                if (candel == "1") {
                    Aids.push($(this).val());
                }
                else {
                    $(this).prop("checked", false);
                }
            });

            if (Aids.length == 0) {
                HiTipsShow("没有符合删除条件的活动被选择！", "warning");
                return;
            }

            var idstr = Aids.join(","); //可删除的AID
            var DataJson = { action: "BatchDel", Aids: idstr };

            HiTipsShow("确定批量删除活动？", "confirmII", function () {

                AjaxPost(DataJson);
            });

           
        }

        ///批量开始
        function BatchStart()
        {
            if ($("input[name=CheckBoxGroup]:checked").length < 1) {
                HiTipsShow("没有活动被选择！请先选择需要开始的活动", "warning");
                return;
            }

            var Aids=[];

            $("input[name=CheckBoxGroup]:checked").each(function () {
                var state = $(this).attr("state");

                if (state == "未开始") {
                    Aids.push($(this).val());
                }
                else {
                    $(this).prop("checked", false);
                }
            });


            if (Aids.length == 0) {
                HiTipsShow("没有符合开始条件的活动被选择！", "warning");
                return;
            }

          var idstr= Aids.join(","); //可删除的AID
          var DataJson = { action: "BatchStart", Aids: idstr };

          HiTipsShow("确定批量开始活动，会重新修改活动开始时间？", "confirmII", function () {

              AjaxPost(DataJson);
          });

        }


        function AjaxPost(PostData,postUrl,callback)
        {
            var url = "OneyuanList.aspx";
            if (postUrl != null) {
                url = postUrl;
            }
            
            $.ajax({
                type: "post",
                url: url,
                data: PostData,
                async: false,
                dataType: "json",
                success: function (data) {

                    if (callback) {
                        callback(data); //如果有回调，则调用回调
                    }
                    else {
                        if (data.state) {
                            HiTipsShow(data.msg, "success", function () {
                                window.location.reload(); //重新加载当前页
                            });

                        } else {
                            HiTipsShow(data.msg, "error");
                        };
                    }
                    
                },
                error: function () {
                    HiTipsShow("访问服务器异常！", "error");
                }
            });




        }



        $(function () {

            var tabNum = getParam("state");
            if (tabNum != "")
            {
                var $tab = $(".nav-tabs li");
                $tab.removeClass("active");
                $tab.eq(tabNum).addClass("active");
            }
         

            $("#sells1").click(function () {
                $("input[name=CheckBoxGroup]").prop("checked", $(this).prop("checked"));
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
            <h2>一元夺宝
                <small style="float:right;margin-top:10px;margin-left:10px;display:inline-block;color:red">系统每半小时会自动检查一次需要开奖的活动，如果满足条件自动开奖！</small>
            </h2>
   </div>
     <a class="btn btn-success" href="AddOneyuanInfo.aspx">新建一元夺宝活动</a>
     <form runat="server">

             <div id="mytabl" style="margin-top:10px">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li class="active"><a href="OneyuanList.aspx?state=0"><asp:Literal ID="ListTotal" Text="所有夺宝(10)" runat="server"></asp:Literal></a></li>
                    <li><a href="OneyuanList.aspx?state=1"><asp:Literal ID="ListStart" Text="进行中(0)"  runat="server"></asp:Literal></a></li>
                    <li><a href="OneyuanList.aspx?state=2"><asp:Literal ID="ListWait" Text="未开始(0)"  runat="server"></asp:Literal></a></li>
                    <li><a href="OneyuanList.aspx?state=3"><asp:Literal ID="Listend" Text="已结束(0)"  runat="server"></asp:Literal></a></li>
                </ul>
                <div class="page-box">
                    <div class="page fr">
                        <div class="form-group">
                            <label for="exampleInputName2">每页显示数量：</label>
                            <UI:PageSize runat="server" ID="hrefPageSize" />
                        </div>
                    </div>
                </div>
            </div>
            <!-- Tab panes -->
            <div class="tab-content">
               <div class="tab-pane active">

                    <div class="set-switch">
                        
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="sellshop4">活动标题：</label>
                                <asp:TextBox ID="txtTitle" CssClass="form-control  resetSize  inputw150" runat="server" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop5">揭晓方式：</label>
                            <asp:DropDownList ID="txtReachType" CssClass="form-control  resetSize " runat="server" >
                                    <asp:ListItem Value="0">全部</asp:ListItem>
                                    <asp:ListItem Value="1">满份开奖</asp:ListItem>
                                    <asp:ListItem Value="2">到期开奖</asp:ListItem>
                                    <asp:ListItem Value="3">到期满份开奖</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="form-group">
                                <label for="sellshop6">状态：</label>
                                <asp:DropDownList ID="txtState" CssClass="form-control  resetSize " runat="server" >
                                    <asp:ListItem Value="0">全部</asp:ListItem>
                                    <asp:ListItem Value="1">进行中</asp:ListItem>
                                    <asp:ListItem Value="2">未开始</asp:ListItem>
                                    <asp:ListItem Value="3">已结束</asp:ListItem>
                                </asp:DropDownList>
                                </div>
                             <div class="form-group" style="margin-left :60px; margin-top:2px">
                            <asp:Button ID="btnSearchButton" runat="server" Text="查询"  CssClass="btn btn-primary  resetSize " /> 
                              </div> <div class="form-group mr20" style ="margin-left :30px;margin-top:10px"><a class="bl mb5" onclick="resetform();" style="cursor: pointer;">清除条件</a>
                            </div>
                        </div>
                    </div>

               </div>

            </div>
        </div>
 

         <div class="mb10 table-operation" style="padding-left:6px">
                                                <input type="checkbox" id="sells1" class="allselect">
                                                <label for="sells1">全选</label>
                                                 <a  class="btn resetSize btn-success" onclick="BatchStart()">批量开始</a>
                                                 <a  class="btn resetSize btn-danger"  onclick="BatchDel()">批量删除</a>
                                                  </div>
         <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th width="2%"></th>
                                        <th width="10%">活动封面</th>
                                        <th width="13%">活动标题</th>
                                        <th width="15%">起止时间</th>
                                        <th width="8%">揭晓方式</th>
                                        <th width="12%">满足份数</th>
                                        <th width="6%">状态</th>
                                         <th width="8%">开奖状态</th>
                                        <th width="20%" style="text-align:center">操作</th>
                                    </tr>
                                </thead>
                                <tbody  class="tableRow">
                                   
                                    <asp:Repeater ID="Datalist" runat="server">
                                        <ItemTemplate>

                                            <tr>
                                       <td style="vertical-align:middle"><input name="CheckBoxGroup" class="fl" type="checkbox" value='<%# Eval("ActivityId") %>' State='<%# Eval("ASate") %>' CanDel='<%# Eval("CanDel") %>' /></td>
                                        <td >
                                             <Hi:ListImage ID="ListImage1" runat="server" DataField="HeadImgage"  Width="128" Height="40"/>
                                        </td>
                                        <td><a target="_blank" title="点击打开活动页面" href="/Vshop/ViewOneTao.aspx?vaid=<%#Eval("ActivityId") %>"><%# Eval("Title") %></a></td>
                                        <td >自<%# Eval("StartTime","{0:yyyy-MM-dd HH:mm:ss}") %><br />
                                            至<%# Eval("EndTime","{0:yyyy-MM-dd HH:mm:ss}") %>
                                        </td>
                                        <td><%# OneyuanTaoHelp.getReachTypeStr((int)Eval("ReachType")) %></td>
                                        <td ><%# Eval("ReachNum") %><br />
                                           <span style="color:#aaa">已售</span>:<span style="color:red"> <%# Eval("FinishedNum") %></span>
                                        </td>
                                        <td><%# Eval("ASate") %></td>
                                        <td ><%# Eval("PrizeState") %></td>
                                         <td >
                                            <%# Eval("ActionBtn") %>
                                        </td>
                                     </tr>

                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
</div>
          <div class="page">
                        <div class="bottomPageNumber clearfix">
                            <div class="pageNumber">
                                <div class="pagination">
                                    <UI:Pager runat="server" ShowTotalPages="true" ID="pager"  />
                                </div>
                            </div>
                        </div>
                    </div>

     </form>  


     <!--活动提前结束-->
   <div class="modal fade" id="ActivityEnd">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header" style="background:#eee;border-top-left-radius:10px;border-top-right-radius:10px">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" style="text-align:left;color:#808080" >活动提前结束提醒</h4>
      </div>
        <div class="modal-body form-horizontal" id="EndBody" >
          当前活动已有人参与，请确定是开奖还是退款！
        </div>
      <div class="modal-footer">
          <button id="DrawEnd"  class="btn btn-success resetSize" Text="结束并开奖" />结束并开奖</button>
          <button id="RefundEnd"  class="btn btn-primary  resetSize" Text="结束并退款" />结束并退款</button>
        <button type="button" class="btn btn-default  resetSize" data-dismiss="modal">关闭</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
</asp:Content>