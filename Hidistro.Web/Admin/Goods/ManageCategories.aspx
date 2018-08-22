<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManageCategories.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ManageCategories1" %>

<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ZeroClipboard.min.js"></script>
    <style type="text/css">
        #myIframeModal .modal-body input.form-control {
            display: inline-block;
        }

        .fz, .er {
            display: inline-block;
            width: 17px;
            height: 17px;
            margin-right: 5px;
            float: right;
        }
    </style>
    <style type="text/css">
        .outBox {
            width: 17px;
            height: 17px;
            margin: 200px auto;
        }

            .outBox a {
                background-color: red;
                color: #fff;
                font-size: 12px;
                display: block;
                width: 17px;
                height: 17px;
                line-height: 16px;
                text-align: center;
                position: relative;
            }

        .prompt {
            background-color: rgba(0,0,0,0.8);
            padding: 10px;
            position: absolute;
            right: 25px;
            top: -98px;
            text-align: center;
            display: none;
            width: 175px;
            height: 190px;
        }

        .arrow {
            color: #333;
            position: absolute;
            right: -6px;
            top: 98px;
            font-size: 20px;
        }

        .prompt p {
            margin: 0;
            color: #fff;
            font-size: 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2>商品分类管理</h2>
        </div>
        <div class="clearfix">
            <div class="fl">
                <a href="categoryedit.aspx" class="btn btn-primary btn-sm">添加商品分类<span class="glyphicon glyphicon-plus" aria-hidden="true"></span></a>
                <a id="openAll" class="btn btn-info btn-sm">全部展开<span class="glyphicon glyphicon-plus-sign" aria-hidden="true"></span></a>
                <a id="closeAll" class="btn btn-info btn-sm">全部收缩<span class="glyphicon glyphicon-minus-sign" aria-hidden="true"></span></a>
            </div>
            <div class="fr">
                <asp:LinkButton ID="btnOrder" runat="server" Text="保存排序<span class='glyphicon glyphicon-ok' aria-hidden='true'></span>" CssClass="btn btn-success btn-sm" />
            </div>
        </div>
        <!--数据列表区域-->
        <div class="datalist mt5">
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
                    <table class="table table-bordered table-hover">
                        <thead>
                            <tr>
                                <th style="width: 65%;">分类名称</th>
                                <th style="width: 80px;">排序</th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>


                            <span class="Name" parentid='<%# Eval("ParentCategoryId") %>' categoryid="<%#Eval("CategoryId")%>">
                                <ico id="spShowImage" runat="server" class="glyphicon glyphicon-minus-sign"></ico>
                                <asp:Literal ID="lblCategoryName" runat="server" /></span>

                            <input type="text" id='urldata<%# Eval("CategoryId") %>' placeholder="" name='urldata<%# Eval("CategoryId") %>' value='<%#"http://"+Globals.DomainName+"/ProductList.aspx?categoryId="+Eval("CategoryId")%>' disabled="" style="display: none">
                            <a class="fz" title="点击复制该类别网址" href="javascript:void(0)" data-clipboard-target='urldata<%# Eval("CategoryId") %>' id='url<%# Eval("CategoryId") %>' onclick="copyurl(this.id);"></a>

                            <a class="er" style="position: relative" href="javascript:void(0)" tsrc="http://s.jiathis.com/qrcode.php?url=http://<%#Globals.DomainName+"/ProductList.aspx%3FcategoryId="+Eval("CategoryId")%>" onclick="winqrcode('<%#"http://"+Globals.DomainName+"/ProductList.aspx?categoryId="+Eval("CategoryId")%>');"></a>



                        </td>
                        <td>
                            <asp:TextBox ID="txtSequence" runat="server" Text='<%# Eval("DisplaySequence") %>'
                                Width="80px" /><asp:HiddenField ID="hdfCategoryID" runat="server" />
                        </td>
                        <td>
                            <span class="Name icon Pg_10"><a href="javascript:ShowRemoveProduct(<%#Eval("CategoryId") %>)">转移商品</a></span> <span class="submit_bianji">
                                <a href="javascript:EditCategory(<%#Eval("CategoryId")%>)">编辑</a>
                            </span><span class="submit_shanchu">
                                <%-- <asp:LinkButton ID="lbtnDel" runat="server" Text="删除" CommandName="DeleteCagetory"  CommandArgument='<%#Eval("CategoryId") %>' OnClientClick="return HiConform('删除分类会级联删除其所有子分类，确定要删除选择的分类吗？',this)"></asp:LinkButton>--%>

                                <asp:Button ID="btnDel" CssClass="btnLink" runat="server" Text="删除" CommandName="DeleteCagetory" CommandArgument='<%#Eval("CategoryId") %>' OnClientClick="return HiConform('<strong>确定要删除选择的分类吗？</strong><p>删除分类会级联删除其所有子分类！</p>',this)" />
                            </span><span class="Name icon Pg_10">
                                <a href="javascript:void(0)" <%#FormatEditeCommission(Eval("ParentCategoryId"),Eval("CategoryId"),Eval("FirstCommission"),Eval("SecondCommission"),Eval("ThirdCommission")) %>>类目佣金</a>
                            </span></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </tbody>
	</table>
                </FooterTemplate>
            </asp:Repeater>

        </div>



        <div class="modal fade" id="myIframeModal">
            <div class="modal-dialog ">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">修改类目佣金</h4>
                    </div>
                    <div class="modal-body exitshopinfo ">
                        <div class="form-group">
                            <h2 class="pl5">上二级佣金比例 </h2>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-2 control-label">分佣设置</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control inputw200" id="txtthird" name="txtthird" runat="server" value="1" />&nbsp;&nbsp;%
                                <%--<span style="color: #888888; padding-left: 10px;">输入1-100的数</span>--%>
                            </div>

                        </div>
                        <div class="form-group">
                            <h2 class="pl5">上一级佣金比例</h2>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-2 control-label">分佣设置</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control inputw200" id="txtsecond" name="txtsecond" runat="server" value="1" />&nbsp;&nbsp;%
                                <%--<span style="color: #888888; padding-left: 10px;">输入1-100的数</span>--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <h2 class="pl5">成交店铺佣金比例</h2>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-2 control-label">分佣设置</label>
                            <div class="col-xs-8">
                                <input type="text" class="form-control inputw200" id="txtfirst" name="txtfirst" runat="server" />&nbsp;&nbsp;%
                                <%--<span style="color: #888888; padding-left: 10px;">输入1-100的数</span>--%>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input type="text" id="txtcategoryId" runat="server" class="hide" />
                        <asp:Button ID="btnSetCommissions" runat="server" Text="保存" CssClass="btn btn-success" OnClientClick="return validatorForm()" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
        </div>

    </form>


    <!-- 模态框（Modal） -->
    <div class="modal fade" id="myModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="myModalLabel">将指定分类下的商品转移到其他分类
                    </h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyIframe" width="550" height="170" scrolling="No"></iframe>
                </div>
            </div>
        </div>
        <!-- /.modal -->
    </div>
    <div style="display: none">
        <iframe id="ifPostDown" name="ifPostDown"></iframe>
        <form id="formDown" method="post" action="managecategories.aspx" target="ifPostDown">
            <input type="hidden" id="hdPicUrl" name="picurl" />
        </form>
    </div>
    <div class="modal fade" id="divqrcode">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">二维码</h4>
                </div>
                <div class="modal-body" style="text-align: center">
                    <img id="imagecode" src="" />
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success float downloadbtn">下载</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal-dialog -->
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //全部隐藏
            $(".Name b,.Name ico").css("cursor", "pointer");
            $(".downloadbtn").click(function () {
                var dimg = $('#imagecode').attr("src");
                $("#hdPicUrl").val(dimg);
                $("#formDown").submit();
            })
            $('a.er').hover(function () {
                var top = -98,
					elementB = $(window).height() + $(window).scrollTop() - $(this).offset().top,
                    elementT = $(this).offset().top - $(window).scrollTop() - $('header').height();
                if (elementB < 110) {
                    top = top - (95 - elementB);
                }
                
                if (elementT < 90) {
                    top = 0;
                }
                $(this).append('<div class="prompt" style="top:' + top + 'px"><span class="arrow" style="top:' + (-top) + 'px">◆</span><img alt="加载中..." src="' + $(this).attr("tsrc") + '" style="width:155px;height:155px"><p>点击右侧“码”图标下载二维码</p></div>');
                $("div.prompt").show();
            }, function () {
                if ($('.prompt')[0]) {
                    $('.prompt').remove();
                }
            })
            $("#closeAll").bind("click", function () {
                $(".datalist table tr").each(function (index, domEle) {
                    if (index != 0) {
                        var optionTag = $(this).html();
                        if (optionTag.indexOf("parentid=\"0\"") < 0) {
                            $(domEle).hide();
                            $(".datalist table tr td ico").attr("class", "glyphicon glyphicon-plus-sign");
                        }
                    }
                })
            });
            $(".Name b").click(function () { $(this).prev().click(); })
            //全部展开
            $("#openAll").bind("click", function () {
                $(".datalist table tr").each(function (index, domEle) {
                    if (index != 0) {
                        $(domEle).show();
                        $(".datalist table tr td span ico").attr("class", "glyphicon glyphicon-minus-sign");
                    }
                })
            });

            $('.datalist table tbody tr').each(function () {
                var id = $(this).eq(0).find(".fz").attr("id");
                var copy = new ZeroClipboard(document.getElementById(id), {
                    moviePath: "../js/ZeroClipboard.swf"
                });
                copy.on('complete', function (client, args) {
                    HiTipsShow("复制成功，复制内容为：" + args.text, 'success');
                });
            })
        });
        $(".datalist table tr td ico").each(function (index, imgObj) {
            //为第一级的时候添加点击事件效果
            $(imgObj).click(function () {
                if ($(imgObj).attr("class") == "glyphicon glyphicon-minus-sign") {
                    var currentTrNode = $(imgObj).parents("tr");
                    currentTrNode = currentTrNode.next();
                    var optionHTML;
                    while (true) {
                        optionHTML = currentTrNode.html();
                        if (typeof (optionHTML) != "string") { break; }
                        if (optionHTML.indexOf("parentid=\"0\"") < 0) {
                            currentTrNode.hide();
                            currentTrNode = currentTrNode.next();
                        }
                        else { break; }
                    }
                    //把img src设加可开打状态
                    $(imgObj).attr("class", "glyphicon glyphicon-plus-sign");
                }
                else {
                    var currentTrNode = $(imgObj).parents("tr");
                    currentTrNode = currentTrNode.next();
                    var optionHTML;
                    while (true) {
                        optionHTML = currentTrNode.html();
                        if (typeof (optionHTML) != "string") { break; }
                        if (optionHTML.indexOf("parentid=\"0\"") < 0) {
                            currentTrNode.show();
                            currentTrNode = currentTrNode.next();
                        }
                        else { break; }
                    }

                    $(imgObj).attr("class", "glyphicon glyphicon-minus-sign");
                }
            });
        })

        function copyurl(obj) {
            var copy = new ZeroClipboard(document.getElementById(obj), {
                moviePath: "../js/ZeroClipboard.swf"
            });
        }
        function winqrcode(url) {
            $("#imagecode").attr('src', "http://s.jiathis.com/qrcode.php?url=" + encodeURIComponent(url));
            $('#divqrcode').modal('toggle').children().css({
                width: '300px',
                height: '300px'
            });
            $("#divqrcode").modal({ show: true });
        }
        function EditCategory(id) {
            var reurl = location.href;
            location.href = "categoryedit.aspx?categoryId=" + id + "&reurl=" + encodeURIComponent(reurl);
        }
        function ShowRemoveProduct(categroyId) {
            if (categroyId != null && parseInt(categroyId) > 0) {
                $("#MyIframe").attr("src", "DisplaceCategory.aspx?CategoryId=" + categroyId);
                $('#myModal').modal('toggle').children().css({
                    width: '600px',
                    height: '260px'
                })
                $("#myModal").modal({ show: true });
                //DialogFrame("product/DisplaceCategory.aspx?CategoryId=" + categroyId, "转移商品", 530, 270);
            } else {
                alert("请选择要转移商品的商品分类！");
            }
        }
        function setArryText(objid, value) {
            $("#" + objid).val(value);
        }
        function EditeCommission(cid, strcommissionfirst, strcommissionsecond, strcommissionthird) {
            arrytext = null;
            setArryText('ctl00_ContentPlaceHolder1_txtcategoryId', cid);
            if (strcommissionfirst != "" && strcommissionsecond != "" && strcommissionthird != "") {
                setArryText('ctl00_ContentPlaceHolder1_txtfirst', strcommissionfirst);
                setArryText('ctl00_ContentPlaceHolder1_txtsecond', strcommissionsecond);
                setArryText('ctl00_ContentPlaceHolder1_txtthird', strcommissionthird);
            }
            else {
                setArryText('ctl00_ContentPlaceHolder1_txtfirst', "");
                setArryText('ctl00_ContentPlaceHolder1_txtsecond', "");
                setArryText('ctl00_ContentPlaceHolder1_txtthird', "");
            }
            //DialogShow('修改类目佣金', 'commission', 'divCommissions', 'ctl00_ContentPlaceHolder1_btnSetCommissions');
            $('#myIframeModal').modal('toggle').children().css({
                width: '600px',
                height: '260px'
            })
            $("#myIframeModal").modal({ show: true });

        }

        function validatorForm() {
            var categoryId = $("#ctl00_ContentPlaceHolder1_txtcategoryId").val().replace(/\s/g, "");
            var firstcommsion = $("#ctl00_ContentPlaceHolder1_txtfirst").val().replace(/\s/g, "");
            var secondcommsion = $("#ctl00_ContentPlaceHolder1_txtsecond").val().replace(/\s/g, "");
            var thirdcommsion = $("#ctl00_ContentPlaceHolder1_txtthird").val().replace(/\s/g, "");
            var validreg = /^\d+(?=\.{0,1}\d+$|$)/;
            if (isNaN(categoryId)) {
                ShowMsg('请先选择要编辑的类目佣金！', false);
                return false;
            }
            //if (!validreg.test(firstcommsion) ||!validreg.test(secondcommsion)|| !validreg.test(thirdcommsion)) {
            if ((!validreg.test(firstcommsion) && firstcommsion != "") || (!validreg.test(secondcommsion) && secondcommsion != "") || (!validreg.test(thirdcommsion) && thirdcommsion != "")) {
                ShowMsg('佣金请输入数字格式！', false);
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
