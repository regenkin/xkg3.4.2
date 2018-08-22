<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="DistributorFrozenList.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.DistributorFrozenList" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        em{color:#FF6600}
    </style>
    <script>
        function resetform() {
            document.getElementById("aspnetForm").reset();
        }

        $(function () {

            $('.allselect').change(function () {
                $('.content-table input[type="checkbox"]').prop('checked', $(this)[0].checked);
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



            $("#PasswordShow").formvalidation({
                'submit':'#ctl00_ContentPlaceHolder1_PassCheck',
                'ctl00$ContentPlaceHolder1$txtPassword': {
                    validators: {
                        notEmpty: {
                            message: '密码不能为空，6-20个字符！'
                        },
                        stringLength: {
                            min: 6,
                            max: 20,
                            message: '密码不能为空，6-20个字符！'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtConformPassword': {
                    validators: {
                        notEmpty: {
                            message: '确认密码不能为空'
                        },
                        repeatPass: {
                            message: '密码与上次输入不符'
                        }
                    }
                }
            });

            $("#GradeShow").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_GradeCheck',
                'ctl00$ContentPlaceHolder1$GradeCheckList': {
                    validators: {
                        notEmpty: {
                            message: '请选择分销商等级'
                        }
                    }
                }
            });


        })


     
        function ShowGrade(obj) {
            if ($('.content-table input[type="checkbox"]:checked').length < 1) {
                //alert("请先选择要修改用户等级的分销商！");
                HiConform('<strong>请先选择要修改用户等级的分销商</strong>', obj);
                return;
            }


            $('#GradeShow').modal('toggle').children().css({
                width: '400px',top:"170px"
            });
        
        }

        function ShowPassword(obj) {

            if ($('.content-table input[type="checkbox"]:checked').length < 1) {
                //alert("请先选择要修改密码的分销商！");
                HiConform('<strong>请先选择要修改密码的分销商！</strong>', obj);
                return;
            }

            $('#PasswordShow').modal('toggle').children().css({
                width: '400px', top: "170px"
            });
           
        }

        function ShowEditDistributorInfos(userid, obj) {
            var temp = $(obj).attr("data");
            // CurrentUser
            if ($(obj).attr("data")) {
                var datas = temp.split("#");
                if (datas.length == 4) {

                    $("#ctl00_ContentPlaceHolder1_EditUserID").val(userid);
                    $("#ctl00_ContentPlaceHolder1_EdittxtRealname").val(datas[1].trim());
                    $("#ctl00_ContentPlaceHolder1_EdittxtCellPhone").val(datas[2].trim());
                    $("#ctl00_ContentPlaceHolder1_EdittxtQQNum").val(datas[3].trim());
                    $("#CurrentUser").html(datas[0]);

                    $('#EditDistributorInfos').modal('toggle').children().css({
                        width: '600px', top: "130px"
                    });
                }



            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
            <h2>分销商管理</h2>
   </div>
     <form runat="server">
       <div id="mytabl">
            <!-- Nav tabs -->
            <div class="table-page">
                <ul class="nav nav-tabs">
                    <li ><a href="DistributorList.aspx"><asp:Literal ID="ListActive" Text="分销商列表(10)" runat="server"></asp:Literal></a></li>
                    <li class="active"><a><asp:Literal ID="Listfrozen" Text="已冻结(0)"  runat="server"></asp:Literal></a></li>
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
            <div class="tab-pane"></div>
                <div class="tab-pane active">

                    <div class="set-switch">
                        <div class="form-inline mb10">
                            <div class="form-group mr20">
                                <label for="sellshop1">店铺名：</label>
                                <asp:TextBox ID="txtStoreName" CssClass="form-control resetSize inputw150" runat="server" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop2">　联系人：</label>
                                <asp:TextBox ID="txtRealName"  CssClass="form-control resetSize" runat="server" 
                                    Width="150" />
                            </div>
                            
                        </div>
                        <div class="form-inline">
                            <div class="form-group mr20">
                                <label for="sellshop4">用户名：</label>
                                <asp:TextBox ID="txtMicroSignal" CssClass="form-control resetSize inputw150" runat="server" />
                            </div>
                            <div class="form-group mr20">
                                <label for="sellshop5">手机号码：</label>
                                 <asp:TextBox ID="txtCellPhone" CssClass="form-control resetSize inputw150" runat="server" />
                            </div>
                            <div class="form-group">
                                <label for="sellshop6">分销商等级：</label>
                                <Hi:DistributorGradeDropDownList ID="DrGrade"  CssClass="form-control resetSize inputw120" runat="server" AllowNull="true" NullToDisplay="全部" />
                            </div>
                        </div>
                        <div class="reset-search" style="margin-left:10px;margin-top:14px">
                            <a class="bl mb5" onclick="resetform();" style="cursor: pointer">清除条件</a>
                            <asp:Button ID="btnSearchButton" runat="server" Text="查询" CssClass="btn resetSize btn-primary" />
                        </div>
                    </div>


                    <div class="select-page clearfix" style="margin-top: 20px;">
                    </div>
                    <div class="sell-table">
                        <div class="title-table">
                            <table class="table">
                                <thead>
                                    <tr>
                                        <th width="10%">微信头像</th>
                                        <th width="15%">微信昵称/手机</th>
                                        <th width="15%">用户名</th>
                                        <th width="10%">店名/联系人</th>
                                        <th width="10%">分销商等级</th>
                                        <th width="10%">总销售额</th>
                                        <th width="10%">佣金总额</th>
                                        <th width="8%">申请时间</th>
                                        <th width="10%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td colspan="8">
                                            <div class="mb10 table-operation">
                                                <input type="checkbox" id="sells1" class="allselect">
                                                <label for="sells1">全选</label>
                                                  <asp:Button ID="UnFrozenCheck" runat="server" Text="批量解冻" CssClass="btn resetSize btn-success" IsShow="true" OnClientClick="return HiConform('<strong>确定要解冻分销商吗？</strong><p>解冻之后，分销商可获取到下级贡献的佣金！</p>', this);" />
                           <%--  <Hi:ImageLinkButton ID="UnFrozenCheck" class="btn resetSize btn-primary" runat="server" Text="批量解冻" IsShow="true"  DeleteMsg="确定解冻所选择的分销商吗？"/>--%>
                                                <button type="button" class="btn resetSize btn-primary" onclick="ShowGrade(this)">批量设置等级</button>
                                                &nbsp;&nbsp;︱
                                                <button type="button" class="btn resetSize btn-primary" onclick="ShowPassword(this)">设置密码</button>
                                                 <asp:Button ID="CancleCheck" runat="server" Text="取消分销资质" CssClass="btn resetSize btn-primary" IsShow="true" OnClientClick="return HiConform('<strong class=red>取消分销商资质以后不可恢复也不能重新申请。</strong><p>是否继续？</p>', this);" />
                                       <%--         <Hi:ImageLinkButton ID="CancleCheck" class="btn resetSize btn-primary" runat="server" Text="取消分销资质" IsShow="true"  DeleteMsg="取消后不可恢复！确定要取消所选的分销商资质吗？"/>--%>
  &nbsp;&nbsp;
                                                <asp:HyperLink Target="_blank" Visible="false" runat="server" ID="btnDownTaobao" Text="下载淘宝商品" />
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="content-table">
                            <table class="table">
                                <tbody>
                                    <asp:Repeater ID="reDistributor" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td width="10%">
                                                    <input name="CheckBoxGroup" class="fl" type="checkbox" value='<%#Eval("UserId") %>' />
                                                    <%--<%# Eval("UserHead").ToString()!=""?"<img  src='"+Eval("UserHead")+"' width=\"50\" height=\"50\"/>":""%>--%>
                                                    <div class="img fl mr10">
                                                        <Hi:ListImage ID="ListImage1" runat="server" DataField="UserHead"  Width="60" Height="60"/>
                                                    </div>
                                                </td>
                                                <td width="15%" style="text-align:left">
                                                    <p><span><%# Eval("UserName")%></span></p>
                                                    <p><span><%# Eval("CellPhone")%></span></p>
                                                </td>
                                                <td width="15%"><%# Eval("UserName")%></td>
                                                <td width="10%"  style="text-align:left">
                                                   <p><%# Eval("StoreName")%></p>
                                                    <p><%# Eval("RealName")%></p>
                                                </td>
                                                <td width="10%"><%#Eval("Name") %></td>
                                                <td width="10%"><em>￥<%# Math.Round((decimal)Eval("OrdersTotal"),2) %></em></td>
                                                <td width="10%"><em>￥<%# Math.Round((decimal)Eval("ReferralBlance"),2) %></em></td>
                                                <td width="8%"><%#Eval("CreateTime","{0:yyyy-MM-dd<br>HH:mm:ss}") %></td>
                                               <td width="10%">
                                                    <p>
                                                        <a href="DistributorDetails.aspx?UserId=<%#Eval("UserId") %>" >详情</a>︱ 
                                                        <a class="table-icon edit" href="#" onclick="ShowEditDistributorInfos(<%#Eval("UserId") %>,this);"  data="<%#  Eval("UserName")%># <%#  Eval("RealName") %># <%#  Eval("CellPhone")%># <%#  Eval("QQ")%>" >编辑</a>
                                                    </p>
                                                    <p>
                                                         <a href="CommissionsList.aspx?UserId=<%#Eval("UserId") %>" target="_blank" >佣金</a>︱
                                                       <%-- <Hi:ImageLinkButton ID="btnFrozen" CommandName="Thaw" CommandArgument='<%# Eval("UserId")%>' runat="server" Text="解冻" IsShow="true"
                                    DeleteMsg="确定解冻所选择的分销商吗？"  title="冻结以后，分销商不能进入分销管理中心，期间也不会获得佣金。" />--%>
                                                         <asp:Button ID="btnFrozen" runat="server" Text="解冻"  CommandName="Thaw" CommandArgument='<%# Eval("UserId")%>' class="btnLink pad" IsShow="true" OnClientClick="return HiConform('<strong>解冻所选择的分销商</strong><p>确定要解冻分销商吗？</p>', this);" />
                                                    </p>
                                                   <p>
                                                     <%--  <Hi:ImageLinkButton ID="ImageLinkButton1" CommandName="Forbidden" CommandArgument='<%# Eval("UserId")%>' runat="server" Text="取消分销商资质" IsShow="true"
                                    DeleteMsg="取消分销商资质以后不可恢复也不能重新申请，是否继续？" />--%>
                                                        <asp:Button ID="ImageLinkButton1" CommandName="Forbidden" CommandArgument='<%# Eval("UserId")%>' class="btnLink pad"  runat="server" Text="取消分销商资质"   IsShow="true" OnClientClick="return HiConform('<strong>取消后不可恢复！</strong><p>确定要取消该分销商资质吗？</p>', this);" />
                                                   </p>
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
                
              
                </div>
            </div>
        </div>
  
   


      <!--批量等级设置-->
    <div class="modal fade" id="GradeShow">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" style="text-align:left" >批量设置等级</h4>
      </div>
        <div class="modal-body form-horizontal" >
            <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>请选择等级：</label>
                        <div class="col-xs-6">
                          <Hi:DistributorGradeDropDownList ID="GradeCheckList"  CssClass="form-control inputw120" runat="server" AllowNull="false" />
                        </div>
                </div>
        </div>
      <div class="modal-footer">
          <asp:Button ID="GradeCheck"  class="btn btn-primary" Text="确定修改"  runat="server"  />
        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->

     <!--批星重置密码-->
   <div class="modal fade" id="PasswordShow">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" style="text-align:left" >批量重置密码</h4>
      </div>
        <div class="modal-body form-horizontal" >
            <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>新密码：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="txtPassword" TextMode="Password" CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        </div>
                </div>

            <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>确认密码：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="txtConformPassword" TextMode="Password" CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        </div>
                </div>

          
           
        </div>
      <div class="modal-footer">
          <asp:Button ID="PassCheck"  class="btn btn-primary" Text="确定修改" runat="server" />
        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->



            <!--编辑用户信息-->
   <div class="modal fade" id="EditDistributorInfos">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
        <h4 class="modal-title" style="text-align:left" >修改分销商信息</h4>
      </div>
        <div class="form-horizontal" style="overflow-x:hidden" >

            <asp:HiddenField ID="EditUserID" Value="" runat="server" />

                <div class="form-group set-switch" >
                        <label for="inputEmail3" class="col-xs-4 control-label">当前修改用户：</label>
                        <div class="col-xs-6">
                         <span class="setControl" id="CurrentUser">ssss</span>
                        </div>
                </div>
                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label">联系人：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="EdittxtRealname"  CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        </div>
                </div>

                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>手机号：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="EdittxtCellPhone"  CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        </div>
                </div>

                <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>QQ号码：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="EdittxtQQNum" CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        </div>
                </div>


            <div class="form-group">
                        <label for="inputEmail3" class="col-xs-4 control-label"><em>*</em>用户密码：</label>
                        <div class="col-xs-6">
                          <asp:TextBox  ID="EdittxtPassword" TextMode="Password" CssClass="form-control  inputw120"   runat="server" ></asp:TextBox>
                        <small>密码为空表示不修改</small>
                        </div>
                </div>

          
           
        </div>
      <div class="modal-footer">
          <asp:Button ID="EditSave"  class="btn btn-primary" Text="确定修改" runat="server" />
        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
      </div>
    </div><!-- /.modal-content -->
  </div><!-- /.modal-dialog -->
</div><!-- /.modal -->
          
</form>
</asp:Content>
