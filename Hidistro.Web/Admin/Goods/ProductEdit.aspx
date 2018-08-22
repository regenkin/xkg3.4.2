<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ProductEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ProductEdit" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <Hi:Script ID="Script2" runat="server" Src="/utility/jquery_hashtable.js" />
    <Hi:Script ID="Script1" runat="server" Src="/utility/jquery-powerFloat-min.js" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/bootstrapSwitch.js" />
    <link href="/utility/flashupload/flashupload.css" rel="stylesheet" type="text/css" />
    <link href="/admin/css/bootstrapSwitch.css" rel="stylesheet" type="text/css" />
    <Hi:Script ID="Script3" runat="server" Src="/utility/flashupload/flashupload.js" />
    <style>
        .formitemtitle5 {
            padding-right: 5px;
        }

        .valspan {
            margin-right: 5px;
        }

            .valspan input {
                margin: 0px 2px 3px 0px;
                vertical-align: middle;
            }

        .skuItemList ul li span {
            display: inline;
        }

        .skuItemList li input {
            margin-right: 5px;
        }

        #skuItems .formitemtitle4 {
            display: block;
            font-weight: bold;
            width: 100%;
            border-bottom: 1px dashed #ccc;
            margin: 5px 0;
        }

        .skuItem_Qty {
            text-align: right;
        }

        .SpecificationTh td {
            background: #F2F2F2;
        }


        /*style.css*/
        .specdiv {
            border: 1px solid #9CF;
            cursor: pointer;
            line-height: 20px;
            background-color: #FFC;
        }

        .specsna {
            border: 1px solid #9CF;
            cursor: not-allowed;
            line-height: 20px;
            color: #CCC;
            background-color: #FFF;
            display: block;
            float: left;
            margin-right: 3px;
            margin-top: 3px;
            white-space: nowrap;
        }

        .skuItemList li input {
            float: left;
        }

        .skuItemList ul li span {
            display: inline;
        }

        .skuItemList {
            float: left;
        }

            .skuItemList li {
                margin-right: 8px;
                float: left;
            }

        .target_box {
            width: 300px;
            padding: 3px;
            border: 1px solid #AAA;
            background-color: #FFF;
        }

        .specspan {
            border: 1px solid #9CF;
            cursor: pointer;
            line-height: 20px;
            background-color: #FFC;
            display: block;
            float: left;
            margin-right: 3px;
            margin-top: 3px;
            white-space: nowrap;
        }

        .specdefault {
            border: 1px dotted;
            cursor: pointer;
            line-height: 20px;
        }

        #attributeContent li:after {
            display: block;
            height: 0;
            content: "";
            clear: both;
        }

        #attributeContent li {
            clear: both;
            display: block;
            margin-bottom: 5px;
        }

            #attributeContent li select {
                margin-right: 5px;
            }
        /*#attributeContent span.formitemtitle5{display:inline-block;width:200px;text-align:right;}*/
        #div_tags label input[type="checkbox"] {
            vertical-align: middle;
            margin-top: -2px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2><%=operatorName %>商品信息</h2>
        </div>
        <div class="play-tabs">
            <ul class="nav nav-tabs speedOfProgress" role="tablist" id="myTab">
                <li role="presentation" class="active complete"><a href="selectcategory.aspx?categoryId=<%=categoryid %><%if (productId > 0)
                                                                                                                          {%>&productId=<%=productId %><% } %>&reurl=<%=Server.UrlEncode(reurl) %>">1.选择商品分类</a></li>
                <li role="presentation" <%if (isnext != 1)
                                          { %>class="active complete"
                    <%} %>><a href="#exitshopinfo" aria-controls="exitshopinfo" role="tab" data-toggle="tab">2.编辑商品信息</a></li>
                <li role="presentation" <%if (isnext == 1)
                                          { %>class="active complete"
                    <%} %>><a <%if (productId > 0)
                                { %>
                        href="#exitshopdate" aria-controls="exitshopdate" role="tab" data-toggle="tab" <%} %>>3.编辑商品详情</a></li>
            </ul>
            <div class="tab-content">
                <%--                        <div role="tabpanel" class="tab-pane active" id="shopclass">选择商品分类</div>--%>
                <div role="tabpanel" class="tab-pane <%if (isnext != 1)
                                                       { %>active<%} %>"
                    id="exitshopinfo">
                    <div class="exitshopinfo">
                        <div class="form-horizontal">
                            <h3>基本信息</h3>
                            <div class="form-group">
                                <label class="col-xs-2 control-label">所属分类：</label>
                                <div class="col-xs-4">
                                    <div class="exitpa">
                                        <asp:Literal runat="server" ID="litCategoryName"></asp:Literal>
                                        <asp:HyperLink runat="server" ID="lnkEditCategory" CssClass="a" Text="[编辑]"></asp:HyperLink>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="brand" class="col-xs-2 control-label">商品类型：</label>
                                <div class="col-xs-6">
                                    <Hi:ProductTypeDownList runat="server" CssClass="form-control inputw300 productType" ID="dropProductTypes" NullToDisplay="--请选择--" />
                                    <small>如果商品没有多种规格，且常规参数能满足商品展示，则不用选 <a target="_blank" href="producttypes.aspx">[商品类型管理]</a></small>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="brand" class="col-xs-2 control-label">品牌：</label>
                                <div class="col-xs-4">
                                    <Hi:BrandCategoriesDropDownList runat="server" ID="dropBrandCategories" NullToDisplay="--请选择--" CssClass="form-control inputw300" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="shopname" class="col-xs-2 control-label"><em>*</em>商品名称：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtProductName" MaxLength="50" />
                                </div>
                            </div>
                            <div class="form-group" style="display: none">
                                <label for="shopname" class="col-xs-2 control-label"><em></em>商品简称：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtProductShortName" MaxLength="20" />
                                </div>
                            </div>
                            <div class="form-group" style="display: <%=(productId>0?"block":"none")%>">
                                <label for="shopname" class="col-xs-2 control-label"><em>*</em>排序：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtDisplaySequence" Text="1" />
                                    <small>商品显示顺序，越大排在越前</small>
                                </div>
                            </div>

                            <div class="form-group" id="l_tags" runat="server">
                                <label class="col-xs-2 control-label">商品标签定义：</label>
                                <div class="col-xs-7">
                                    <div class="exitpa">
                                        <a id="a_addtag" href="javascript:void(0)" onclick="javascript:AddTags()" class="add btn btn-primary btn-xs">添加</a>
                                    </div>
                                    <div id="div_tags">
                                        <Hi:ProductTagsLiteral ID="litralProductTag" runat="server"></Hi:ProductTagsLiteral>
                                    </div>
                                    <div id="div_addtag" style="display: none">
                                        <input type="text" id="txtaddtag" maxlength="8" onkeyup="$(this).val(getStrbylen($(this).val(),8))" /> <input type="button" value="保存" onclick="return AddAjaxTags()" class="btn btn-success btn-xs" />
                                    </div>
                                    <Hi:TrimTextBox runat="server" ID="txtProductTag" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                                </div>
                            </div>



                            <div class="form-group">
                                <label for="shopinfo" class="col-xs-2 control-label">商品简介：</label>
                                <div class="col-xs-6">
                                    <Hi:TrimTextBox runat="server" Rows="4" Columns="50" ID="txtShortDescription" TextMode="MultiLine" />
                                    <small>微信分享给好友时会显示这里的文案，如果没有填写，则显示商品名称</small>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-2 control-label">商品图片：</label>
                                <div class="col-xs-7 clearbor">
                                    <div class="exitpa">
                                        <div class="clearfix FlashPanel" style="height: 122px;">
                                            <Hi:ProductFlashUpload ID="ucFlashUpload1" runat="server" MaxNum="5" />
                                        </div>
                                        <small>建议尺寸640×640像素</small>
                                        <small>支持批量上传，每个商品最多5张图，每张小于300KB，支持jpg、gif、png格式</small>
                                    </div>
                                </div>
                            </div>

                            <div id="attributeRow" style="display: none;" class="form-group">
                                <label class="col-xs-2 control-label">商品属性：</label>
                                <div id="attributeContent" class="col-xs-6 pt5"></div>
                                <Hi:TrimTextBox runat="server" ID="txtAttributes" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                            </div>
                        </div>
                    </div>
                    <%--                            <div class="exitshopinfo">
                                <div class="form-horizontal">
                                    <h3>商品信息</h3>
                                </div>
                            </div>--%>
                    <div class="exitshopinfo resize">
                        <div class="form-horizontal">
                            <h3>库存/规格</h3>
                            <div class="form-group" style="display: none;" id="enableSkuRow">
                                <label class="col-xs-2 control-label" style="display: none;">商品规格：</label>
                                <div class="col-xs-4">
                                    <input id="btnEnableSku" type="button" class="onshopmo" value="开启多商品规格" />
                                </div>
                            </div>
                            <span id="skuTitle"></span>
                            <div id="skuRow" style="display: none;" class="<%--form-group--%>">
                                <p id="skuContent" class="pb10">
                                    <input type="button" id="btnshowSkuValue" value="生成部分规格" />
                                    <input type="button" id="btnAddItem" value="增加一个规格" />
                                    <input type="button" id="btnCloseSku" value="关闭规格" />
                                    <input type="button" id="btnGenerateAll" value="生成所有规格" />
                                </p>
                                <p id="skuFieldHolder" style="margin: 0px auto; display: none;"></p>
                                <div id="skuTableHolder" style="clear: both; padding-top: 5px;">
                                </div>
                                <Hi:TrimTextBox runat="server" ID="txtSkus" TextMode="MultiLine" Style="display: none" MaxLength="20"></Hi:TrimTextBox>
                                <asp:CheckBox runat="server" ID="chkSkuEnabled" Style="display: none;" />
                            </div>






                            <div class="form-group">
                                <label for="input1" class="col-xs-2 control-label"><em>*</em>原价：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtMarketPrice"  Text="0"  />&nbsp;&nbsp;元
                                </div>
                            </div>
                            <div class="form-group" id="salePriceRow">
                                <label for="input2" class="col-xs-2 control-label"><em>*</em>现价：</label>
                                <div class="col-xs-2">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtSalePrice" Text="0" />&nbsp;&nbsp;元
                                </div>
                                <div  style="margin-left: -100px">
                                     <Hi:TrimTextBox runat="server" ID="txtMemberPrices" TextMode="MultiLine" Style="display: none;"></Hi:TrimTextBox>
                                    <input type="button" class="setma" onclick="editProductMemberPrice();" value="设置会员价" />
                                </div>
                            </div>
                            <div class="form-group" id="qtyRow">
                                <label for="input3" class="col-xs-2 control-label"><em>*</em>总库存：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtStock" MaxLength="9" Text="0" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="input4" class="col-xs-2 control-label">商家编码：</label>
                                <div class="col-xs-4">
                                    <asp:HiddenField ID="hdfSKUPrefix" runat="server" />
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtProductCode" />
                                </div>
                            </div>
                            <div class="form-group" id="skuCodeRow">
                                <label for="input4" class="col-xs-2 control-label"></label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtCostPrice" Visible="false" />
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtUnit" Visible="false" />

                                    <span style="display: none">
                                        <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtSku" /></span>
                                </div>
                            </div>

                            <div class="form-group" style="display: <%=(productId>0?"none":"block")%>">
                                <label for="input5" class="col-xs-2 control-label">基础销量：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtShowSaleCounts" MaxLength="9" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="exitshopinfo resize bg">
                        <div class="form-horizontal">
                            <h3>分销佣金设置</h3>
                            <div class="form-group">
                                <label class="col-xs-2 control-label">使用分类佣金设置：</label>
                                <div class="col-xs-4">
                                    <div class="switch" id="mySwitch">
                                        <asp:CheckBox ID="cbIsSetCommission" runat="server" Checked="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="input6" class="col-xs-2 control-label"><em></em>上二级佣金比例：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtThirdCommission" Enabled="false" />&nbsp;&nbsp;%
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="input7" class="col-xs-2 control-label"><em></em>上一级佣金比例：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtSecondCommission" Enabled="false" />&nbsp;&nbsp;%
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="input8" class="col-xs-2 control-label"><em></em>成交店铺佣金比例：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtFirstCommission" Enabled="false" />&nbsp;&nbsp;%
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="exitshopinfo resize">
                        <div class="form-horizontal">
                            <h3 class="resize">物流及其他</h3>
                            <div class="form-group">
                                <label for="input9" class="col-xs-2 control-label">物流体积：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtCubicMeter" Text="0.00" />&nbsp;&nbsp;立方米
                                </div>
                            </div>
                            <div class="form-group" <%-- id="weightRow"--%>>
                                <label for="input10" class="col-xs-2 control-label">物流重量：</label>
                                <div class="col-xs-4">
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtWeight" Visible="false" />
                                    <Hi:TrimTextBox runat="server" CssClass="form-control" ID="txtFreightWeight" Text="0.00" />&nbsp;&nbsp;千克
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-2 control-label"><em>*</em>运费设置：</label>
                                <div class="col-xs-4">
                                    <div class="setradio">
                                        <label>
                                            <asp:RadioButton ID="ChkisfreeShipping" runat="server" GroupName="freight" />
                                            包邮</label>
                                    </div>
                                    <div class="setradio">
                                        <label>
                                            <asp:RadioButton ID="rbtIsSetTemplate" runat="server" GroupName="freight" />
                                            运费模板</label>
                                        <Hi:FreightTemplateDownList ID="FreightTemplateDownList1" CssClass="form-control inputw200" runat="server" NullToDisplay="--请选择运费模板--" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-2 control-label"><em>*</em>是否上架：</label>
                                <div class="col-xs-4">
                                    <div class="setradio">
                                        <label>
                                            <asp:RadioButton runat="server" ID="radOnSales" GroupName="SaleStatus"></asp:RadioButton>立刻上架</label>
                                    </div>
                                    <div class="setradio">
                                        <label>
                                            <asp:RadioButton runat="server" ID="radInStock" GroupName="SaleStatus"></asp:RadioButton>放入仓库</label>
                                        <asp:RadioButton runat="server" ID="radUnSales" GroupName="SaleStatus" Visible="false" Text="下架区"></asp:RadioButton>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="nextbtn">
                        <asp:Button runat="server" ID="btnNext" Text="下一步，编辑商品详情" OnClientClick="return doSubmit();" CssClass="btn btn-success btn-sm" />
                        <%-- <button type="button" class="btn btn-success btn-sm" onclick="$('#myTab li:eq(2) a').tab('show');">下一步，编辑商品详情</button>--%>
                    </div>
                </div>
                <div role="tabpanel" class="tab-pane <%if (isnext == 1)
                                                       { %>active<%} %>"
                    id="exitshopdate">
                    <div class="edit-text clearfix">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">店铺主页</div>
                                    </div>
                                    <div class="upshop-view">
                                        <div class="img-info">
                                            <p>基本信息区</p>
                                            <p>固定样式，显示商品主图、价格等信息</p>
                                        </div>
                                        <div class="exit-shop-info">
                                            内容区
                                        </div>
                                    </div>
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                        <div class="edit-text-right">
                            <div class="edit-inner">
                                <Kindeditor:KindeditorControl ID="fckDescription" runat="server" Height="300" Width="570" />
                            </div>
                            <div class="exit-bottom">
                                <p>
                                    <asp:CheckBox runat="server" ID="ckbIsDownPic" Text="下载站外图片" />
                                </p>
                                <p>勾选以后，如果商品详情中包含有站外图片，则会将图片下载保存到您店铺的图片库中，需要下载的图片越多，需要的时间越长，请慎重选择。</p>
                            </div>
                        </div>
                    </div>
                    <div class="footer-btn navbar-fixed-bottom">
                        <button id="prevBtn" type="button" class="btn btn-primary" onclick="$('#myTab li:eq(1) a').tab('show');">上一步</button>
                        <asp:Button runat="server" ID="btnSave" Text="保存" OnClientClick="return doSubmit();" CssClass="btn btn-success inputw100" />
                        <button type="button" class="btn btn-success" id="preview" onclick="return SingleArticleShow()">保存并预览</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- 模态框（Modal） -->
        <div class="modal fade" id="priceBox" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="popTitle">编辑会员价</h4>
                    </div>
                    <div class="modal-body" id="priceContent">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                        <button type="button" class="btn btn-primary" onclick="doneEditPrice('priceBox');">
                            提交
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>



        <!-- 模态框（Modal） -->
        <div class="modal fade" id="skuValueBox" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title">选择要生成的规格</h4>
                    </div>
                    <div class="modal-body" id="skuItems">
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>
                        <button type="button" class="btn btn-primary" id="btnGenerate">
                            提交
                        </button>
                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div><span id="spanJs" runat="server"></span>

        <script type="text/javascript">

            $(document).ready(function () {
                $('#mySwitch').on('switch-change', function (e, data) {
                
                    var cid=<%=categoryid%>;
                    if(cid>0){
                        if (data.value) {
                            var data = "gettype=getcategorycommission&categoryid="+cid;                        
                            $.ajax({
                                url: "productedit.aspx",
                                type: "post",
                                data: data,
                                datatype: "json",
                                success: function (json) {
                                    if (json.type == "1") {
                                        $("#<%=txtThirdCommission.ClientID%>").val(json.t).attr("disabled", "disabled");
                                        $("#<%=txtSecondCommission.ClientID%>").val(json.s).attr("disabled", "disabled");
                                        $("#<%=txtFirstCommission.ClientID%>").val(json.f).attr("disabled", "disabled");
                                    }
                                }
                            });
                        }else{
                            $("#<%=txtThirdCommission.ClientID%>").removeAttr("disabled");
                            $("#<%=txtSecondCommission.ClientID%>").removeAttr("disabled");
                            $("#<%=txtFirstCommission.ClientID%>").removeAttr("disabled");
                        }
                    }else{
                        HiTipsShow("请先选择商品分类", "error");
                    }
                });




                <%if (productId > 0)
                  { %>
                $('.speedOfProgress li').click(function () {
                    var index = $(this).index();
                    $('.speedOfProgress li').removeClass('complete');
                    $('.speedOfProgress li').each(function (i) {
                        if (i > index) {
                            return false;
                        } else {
                            $(this).addClass('complete');
                        }
                    })
                })
                <%}%>
                <%if (isnext == 1)
                  {%>
                $(".speedOfProgress li").addClass('complete');
                <%}%>
                $('#aspnetForm').formvalidation({
                    'ctl00$ContentPlaceHolder1$txtSalePrice': {
                        validators: {
                            notEmpty: {
                                message: '现价不能为空'
                            },
                            regexp: {
                                regexp: /^[0-9]+(\.[0-9]+)?$/,
                                message: '价格只能输入整数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtProductName': {
                        validators: {
                            notEmpty: {
                                message: '商品名称不能为空'
                            },
                            stringLength: {
                                min: 1,
                                max: 60,
                                message: '商品名称需小于60字符'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtMarketPrice': {
                        validators: {
                            notEmpty: {
                                message: '原价不能为空'
                            },
                            regexp: {
                                regexp: /^[0-9]+(\.[0-9]+)?$/,
                                message: '价格只能输入整数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtStock': {
                        validators: {
                            notEmpty: {
                                message: '库存不能为空'
                            },
                            regexp: {
                                regexp: /^(0|[1-9]+?[0-9]*)$/,
                                message: '库存只能输入实数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$freight': {
                        validators: {
                            notEmpty: {
                                message: '请选择运费设置方式'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$SaleStatus': {
                        validators: {
                            notEmpty: {
                                message: '请选择是否上架'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtThirdCommission': {
                        validators: {
                            //notEmpty: {
                            //    message: '上二级佣金不能为空'
                            //},
                            regexp: {
                                regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                                message: '价格只能输入实数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtSecondCommission': {
                        validators: {
                            //notEmpty: {
                            //    message: '上一级佣金不能为空'
                            //},
                            regexp: {
                                regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                                message: '价格只能输入实数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtFirstCommission': {
                        validators: {
                            //notEmpty: {
                            //    message: '成交店铺佣金不能为空'
                            //},
                            regexp: {
                                regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                                message: '价格只能输入实数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtCubicMeter': {
                        validators: {
                            regexp: {
                                regexp:/^[0-9]+(\.[0-9]+)?$/,
                                message: '物流体积只能输入实数型数值'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtFreightWeight': {
                        validators: {
                            //notEmpty: {
                            //    message: '上一级佣金不能为空'
                            //},
                            regexp: {
                                regexp: /^[0-9]+(\.[0-9]+)?$/,
                                message: '物流重量只能输入实数型数值'
                            }
                        }
                    },
                    //'ctl00$ContentPlaceHolder1$txtthird': {
                    //    validators: {
                    //        notEmpty: {
                    //            message: '上二级佣金不能为空'
                    //        },
                    //        regexp: {
                    //            regexp: /^(0|(0+(\.[0-9]{1,2}))|[1-9]([0-9]?)(\.\d{1,2})?)$/,
                    //            message: '数据类型错误，只能输入实数型数值'
                    //        }

                    //    }
                    //},
                    //'ctl00$ContentPlaceHolder1$txtPageDesc': {
                    //    validators: {
                    //        stringLength: {
                    //            min: 0,
                    //            max: 100,
                    //            message: '告诉搜索引擎此分类浏览页面的主要内容，长度限制在100个字符以内'
                    //        }
                    //    }
                    //}
                });
                $('*').on('show.bs.modal', function () {
                    $(".FlashPanel").hide();
                })
                $('*').on('hidden.bs.modal', function () {
                    $(".FlashPanel").show();
                });
                checkIsSetTemplate();
                $("input[name='ctl00$ContentPlaceHolder1$freight']").click(function () { checkIsSetTemplate(); })
                /*编辑器监听事件*/
                um.addListener('ready', function (editor) {
                    $(".exit-shop-info").html(um.getContent());
                });
                um.addListener('selectionchange', function () {
                    $(".exit-shop-info").html(um.getContent());
                });
            });
            function checkIsSetTemplate() {
                if ($("#ctl00_ContentPlaceHolder1_rbtIsSetTemplate").is(":checked")) {
                    $("#ctl00_ContentPlaceHolder1_FreightTemplateDownList1").show();
                } else {
                    $("#ctl00_ContentPlaceHolder1_FreightTemplateDownList1").hide();
                }
            }

            function SingleArticleShow() {
                var memo = UE.getEditor('ctl00_ContentPlaceHolder1_fckDescription_txtMemo').getContent();
                $.ajax({
                    url: "productedit.aspx?productId=<%=productId%>",
                    type: "post",
                    data: "posttype=updatecontent&memo=" + encodeURIComponent(memo),
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            $('#ctl00_ContentPlaceHolder1_btnSave,#preview,#prevBtn').attr('disabled', 'true');setTimeout(function () { $('#ctl00_ContentPlaceHolder1_btnSave,#preview,#prevBtn').removeAttr('disabled'); }, 5000);
                            HiTipsShow(json.tips + "正在生成预览...", "success", function () {
                                OneProductView(<%=productId%>);
                                $('#previewshow').on('hidden.bs.modal', function () {
                                    /*模态框关闭的时候页面跳转到列表页*/
                                    window.location.href = "<%=reurl%>";
                                })
                            });
                        } else {
                            HiTipsShow(json.tips, "error");
                        }
                    }
                })
                return false;
            }
        </script>
    </form>
    <script type="text/javascript" src="attributes.helper.js?20160112"></script>
    <script type="text/javascript" src="grade.price.helper.js"></script>
    <script type="text/javascript" src="producttag.helper.js?20151027"></script>
</asp:Content>
