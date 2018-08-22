<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/SimplePage.Master" AutoEventWireup="true" CodeBehind="SetLogistics.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.SetLogistics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            /*获取物流公司选择*/
            $.ajax({
                url: "sendordergoods.aspx",
                type: "post",
                data: "posttype=getcompany&t=" + (new Date()).getTime(),
                datatype: "json",
                success: function (json) {
                    if (json[0].type == "1") {
                        var datalist = json[0].data;
                        var selectobj =
                        $(".getcompanyname").each(function () {
                            CreateSelectToObj(this, datalist);
                        })
                    }
                }
            });
            function CreateSelectToObj(obj, jsondata) {
                var selHtml = "";
                selHtml = "<select name='comp'><option value=''>请选择</option>";
                for (var i = 0; i < jsondata.length; i++) {
                    if (jsondata[i].code == $(obj).attr("title")) {
                        selHtml += "<option value='" + jsondata[i].code + "' selected='selected'>" + jsondata[i].name + "</option>";
                    } else {
                        selHtml += "<option value='" + jsondata[i].code + "'>" + jsondata[i].name + "</option>";
                    }
                }
                selHtml += "</select>";
                $(obj).html(selHtml);
            }
            $("#btnCancel").click(function () {
                parent.$('#divmyIframeModal').modal('hide')
            })
            $("#btnConfirm").click(function () {
                var selObj = $(".getcompanyname").find("select");
                var selid = $(selObj).val();
                var selname = $(selObj).find("option:selected").text();
                if ($.trim(selid).length==0) {
                    parent.ShowMsg("请指定物流公司！");
                    return false;
                }
                var data = "posttype=savelogistics&selid=" + encodeURIComponent(selid) + "&selname=" + encodeURIComponent(selname) + "&orderlist=<%=orderIds%>&t=" + (new Date()).getTime();

                $.ajax({
                    url: "setlogistics.aspx",
                    type: "post",
                    data: data,
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            parent.ShowMsgAndReUrl("批量指定物流成功！", true, "<%=Reurl%>", "parent");
                        } else {
                            parent.ShowMsg(json.tips);
                        }
                    }
                });
            })
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">    
    <div class="form-group">
        <label class="col-xs-4 control-label alignR" style="padding-top: 0px; line-height: 22px"><em></em>订单数：</label>
        <asp:Literal ID="litOrdersCount" runat="server" Text="2"></asp:Literal>
       
    </div>
    <div class="form-group">
        <label class="col-xs-4 control-label alignR">指定物流公司：</label>
        <span style="width: 132px; display: inline-block;" class="getcompanyname" title=""></span>
    </div>

    <div class="modal-footer">
        <input value="确定" id="btnConfirm" class="btn btn-success" type="button" />
        <button id="btnCancel" type="button" class="btn btn-default">关闭</button>
    </div>
</asp:Content>
