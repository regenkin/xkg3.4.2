<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="BranchAddDistributors.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Fenxiao.BranchAddDistributors" %>


<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.Entities.Sales" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<Hi:Script runat="server" Src="/utility/jquery.bigautocomplete.js"></Hi:Script>
<hi:style ID="Style1" runat="server" href="/utility/jquery.bigautocomplete.css" media="screen" />
    <script>
        function selecttype(typeselect) {
            if (typeselect == 1) {
                $("#NumberId").show();
                $("#NamesId").hide();
            } else {
                $("#NumberId").hide();
                $("#NamesId").show();
            }
        }

        $(function () {
            selecttype($("input[type='radio']:checked").val());
            var searchajax = "?action=SearchKey";
            $("#ctl00_ContentPlaceHolder1_txtslsdistributors").bigAutocomplete({ url: searchajax, width: 230 });

            $("#ctl00_ContentPlaceHolder1_txtslsdistributors").keypress(function (e) {
                if (e.which == 13) {
                    return false;
                }
            });
        });

        function vailidbatch() {



            if ($("input[type='radio']:checked").val() == "1") {//输入数量
                if ($("#ctl00_ContentPlaceHolder1_txtnumber").val().replace(/\s/g, "") == ""
                 || isNaN($("#ctl00_ContentPlaceHolder1_txtnumber").val().replace(/\s/g, ""))
                 || parseInt($("#ctl00_ContentPlaceHolder1_txtnumber").val().replace(/\s/g, "")) <= 0
                 || parseInt($("#ctl00_ContentPlaceHolder1_txtnumber").val().replace(/\s/g, "")) > 999) {
                    HiTipsShow("生成数量:请输入1~999的正整数！", 'error');
                    return false;
                }

            } else { //输入指定账号
              

                if ($("#ctl00_ContentPlaceHolder1_txtslsdistributors").val().replace(/\s/g, "") == "") {
                    HiTipsShow("请输入要生成的分销账号", 'error');
                    return false;
                }

                var tag = true;
                var regA = /[\w\d]/,
                    regB = /(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)/;
                var arr = $("#ctl00_ContentPlaceHolder1_txtdistributornames").val().split('\n');
                $.each(arr, function (i, e) {
                    var is = regA.test(e) || regB.test(e),
                        len = e.length;
                    var a = (len >= 6) && (len <= 50) ? true : false;
                    if (!is || !a) {
                        HiTipsShow("出现非法字符或长度不符合,请检查！", 'error');
                        tag = false;
                        return false;
                    }
                });
                return tag
            }
        }


</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="page-header">
            <h2 id="EditTitle" runat="server">批量生成分销商账号</h2>
          <small>自动生成用户默认密码：888888</small>
   </div>

    <form runat="server">

          <div class="form-horizontal" id="Distributorform">

             <div class="form-group has-feedback">
                        <label class="col-xs-2 control-label"><em>*</em>生成规则：</label>
                        <div class="col-xs-5">
		                    <div class="radio-inline">
		                        <label>
		                            <input type="radio" name="radioadd" value="1" onclick="selecttype(1)" checked="true"  id="radionumber" runat="server" /> 指定数量生成
		                        </label>
		                    </div>
		                    <div class="radio-inline">
		                        <label>
		                            <input type="radio" name="radioadd" value="2" onclick="selecttype(2)" id="radioaccount" runat="server"  /> 指定账号生成
		                        </label>
		                    </div>
		                   
                        </div>
                    </div>


               <div class="form-group">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>推荐分销商：</label>
                        <div class="col-xs-3">
                            <input type="text" id="txtslsdistributors" class="form-control  inputw120" name="txtslsdistributors" runat="server" autocomplete="off" />
                        </div>
                </div>

                <div class="form-group" id="NumberId">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>输入生成数量：</label>
                        <div class="col-xs-3">
                            <input type="text" class="form-control  inputw120" name="txtnumber" id="txtnumber" runat="server" />
                            <small >请输入1~999的正整数</small>
                        </div>
                </div>

                
               <div class="form-group" id="NamesId" style="display:none">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>输入帐号：</label>
                        <div class="col-xs-4">
                           
                            <textarea id="txtdistributornames" class="form-control" name="txtdistributornames"  rows="8" wrap="off" cols="70"  runat="server"></textarea>
                            <small>每个分销商账号在6~50个英文字符，一行一个</small>
                        </div>
                </div>

            

               <div class="form-group">
                        <div class="col-xs-offset-2 col-xs-10">
                             <asp:Button ID="batchCreate" runat="server" Text="生 成" OnClientClick="return vailidbatch()"  CssClass="btn btn-success" />　
                            <asp:Button runat="server" ID="btnExport" Visible="false" Text="导出分销商" class="btn btn-success"/>
                        </div>
                 </div>

       


      </div>














    </form>

</asp:Content>
