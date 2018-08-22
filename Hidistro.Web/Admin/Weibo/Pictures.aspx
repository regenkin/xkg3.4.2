<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pictures.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Pictures" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel="icon" href="../../images/hi.ico" />
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css" />
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="/admin/js/jquery.nicescroll.min.js"></script>
    <script src="../../Utility/globals.js"></script>
    <link rel="stylesheet" href="../css/common.css" />
    <script>
        $(function () {

            $('#mytabl > ul li').click(function () {
                $('#mytabl > ul li').removeClass('active');
                $(this).addClass('active');
                $(this).parent().next().children().removeClass('active');
                $(this).parent().next().children().eq($(this).index()).addClass('active');
            })
        })
        var applicationPath = "";
        function gotovalue(obj) {

            window.parent.closeModalPic("myModal", obj);
        }
        function gotoupvalue() {            
            var obj = $("#uploader1_uploadedImageUrl").attr('value');
            if (obj != "") {
                //保存图片到图片库
                var data = "posttype=togallery&photourl="+obj+"&t="+(new Date()).getTime();
                $.ajax({
                    url: "pictures.aspx",
                    type: "post",
                    data: data,
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            window.parent.closeModalPic("myModal", obj);
                        } else {
                            HiTipsShow(json.tips, "error");
                        }
                    }
                })
                
            }
            else
                alert("请先上传图片！");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div id="mytabl">
                <!-- Nav tabs -->
                <ul class="nav nav-tabs">
                    <li class="active"><a href="#home">图片库</a></li>
                    <li><a href="#profile">新图片</a></li>

                </ul>
                <!-- Tab panes -->

                <div class="tab-content">
                    <div class="tab-pane active">
                        <div class="form-inline">
                            <div class="form-group">

                                <label for="exampleInputName2">图片名称</label>
                                <asp:TextBox ID="txtWordName" runat="server" CssClass="form-control" placeholder="输入图片名称" Width="200" />
                                <asp:Button ID="btnImagetSearch" runat="server" Text="查询" CssClass="btn btn-primary btn-sm" />
                            </div>

                        </div>

                        <asp:DataList ID="photoDataList" runat="server" RepeatColumns="4" ShowFooter="False"
                            ShowHeader="False" DataKeyField="PhotoId" CellPadding="0" RepeatDirection="Horizontal">
                            <ItemTemplate>
                                <div class="imageItem imageLink">
                                    <dl>
                                        <dd>
                                            <div class="imgwh">
                                                <img style="cursor:pointer" class="img-responsive" src='<%=GlobalsPath%>/Admin/PicRar.aspx?P=<%# Eval("PhotoPath")%>&W=140&H=110' alt='<%#Eval("PhotoPath") %>' onclick='gotovalue(this.alt);' />

                                            </div>
                                            <%# TruncStr(DataBinder.Eval(Container.DataItem, "PhotoName").ToString(), 20)%>
                                        </dd>
                                    </dl>

                                </div>
                            </ItemTemplate>
                        </asp:DataList>
                        <div class="page">
                            <div class="bottomPageNumber clearfix">
                                <div class="pageNumber">
                                    <div class="pagination">
                                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="20" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane">
                        <table class="table table-bordered table-hover"  >

                            <tbody>
                                <tr>

                                    <td style="width:100px; text-align:center; vertical-align: middle">上传图片</td>
                                    <td>
                                        <div class="uploadimages" style="  text-align:center; vertical-align: middle">
                                            <Hi:UpImg runat="server" ID="uploader1" IsNeedThumbnail="false" UploadType="Weibo" />
                                        </div>
                                    </td>
                                    <td style="width:100px; text-align:center; vertical-align: middle">
                                        <button type="button" id="btnupimg" class="btn btn-default"  onclick='gotoupvalue();'>选取</button>

                                    </td>
                                </tr>

                            </tbody>
                        </table>

                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>