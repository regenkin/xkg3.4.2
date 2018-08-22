<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ArticlesEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.ArticlesEdit" %>

<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">
    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">
    <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
    <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Admin/shop/Public/js/config.js"></script>
    <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>
    <script src="/Admin/js/HiShopComPlugin.js"></script>
    <script src="/Admin/shop/Public/js/dist/componentadmin-min.js?v1023"></script>
    <style type="text/css">
        .jbox-container {
            overflow-x: hidden;
        }
        .gagp-goodslist .twoclass ul li {
            padding: 0;
            border-bottom: none;
        }
        .gagp-goodslist{
            min-height:300px !important;
        }
        .modalshopclasslist .oneclass p:first-child i.down {
            background: url("../images/iconfont-14052230.png");
        }
    </style>
    <script type="text/javascript">
        var MaterialID=<%=MaterialID%>;
        var urlStart=window.location.protocol+"//"+window.location.host;
        function callbackData(){
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    //alert(item.link);
                    //alert(item.title);
                    var tempLink="";
                    switch(item.title)
                    {
                        case "查看原文":
                            tempLink="";
                            $('#urlData').hide(); 
                            $('#spLinkTitle').html(item.title);
                            $('#hidLinkType').val('1');
                            //$('#divContent').show();
                            break;
                        case "店铺主页":
                            tempLink=item.link;
                            $('#urlData').show();
                            $('#hidLinkType').val('4');
                            $('#spLinkTitle').html(item.title);
                            break;
                        default:
                            tempLink=item.link;
                            $('#urlData').show();
                            //$('#divContent').hide();
                            $('#hidLinkType').val('8');
                            $('#spLinkTitle').html("已选择链接");
                            break;
                    }
                    if(tempLink.length>1){
                        if(tempLink.indexOf("http")==-1){
                            $('#urlData').val(urlStart+tempLink);
                        }else{
                            $('#urlData').val(tempLink);
                        }
                    }
                }
            }); 
        }

        $(document).ready(function () {
            CreateDropdown($("#urlData"), $("#box_move .form-group").eq(1), {createType:3,showTitle:true,txtContinuity:false,reWriteSpan:true,callback:callbackData});
          
            $("#spLinkTitle").html("<%=htmlLinkTypeName%>");
            if($("#hidLinkType").val()=="1"){
                $('#urlData').hide();
            }
            $("#dropdow-menu-link  li[data-val='13']").remove();


            //$('#Picture').click(function () {
            //    var Rand = Math.random();
            //    $("#MyIframe").attr("src", "../weibo/Pictures.aspx?Rand=" + Rand);
            //    $('#myModal').modal('toggle').children().css({
            //        width: '800px',
            //        height: '700px'
            //    })
            //    $("#myModal").modal({ show: true });
            //});

            $('#Picture,#smallpic').click(function () {
                HiShop.popbox.ImgPicker(function (obj) {
                    BindPicData(obj);
                });
            });


            //$("#Homepage").click(function () {
            //    $('#urlData').val(($(this).attr("gotourl"))).hide();
            //    $('#title').val($(this).html()).keyup();
            //    $('#spLinkTitle').html($(this).html());
            //    $('#hidLinkType').val('4');
            //    $('#divContent').hide();
            //    $('#urlData').show();
            //});
            //$('#GoodsAndType').click(function () {
            //    var Rand = Math.random();
            //    $("#MyGoodsAndTypeIframe").attr("src", "../weibo/Goods.aspx?Rand=" + Rand);
            //    $('#myIframeModal').modal('toggle').children().css({
            //        width: '800px',
            //        height: '700px'
            //    })
            //    $("#myIframeModal").modal({ show: true });
            //});
            
            <%=htmlAddJs%>
            $("#btn-save").click(function () {
                var title = $('#title').val();
                var img = $("#fmSrc").val();
                var linkUrl = $('#urlData').val();
                var linkType=$("#hidLinkType").val();
                if(linkType==""){
                    linkType=1;
                }
                var memo = $('#digest').val();
                if ($.trim(title) == "") {
                    HiTipsShow("请填写标题！", "error");
                    return false;
                }
                if (img == "undefined"||img=="") {
                    HiTipsShow("请选择封面图片！", "error");
                    return false;
                }
                if (linkType!="1"&&$.trim(linkUrl)=="") {
                    HiTipsShow("请设置链接地址！", "error");
                    return false;
                }
                var content=UE.getEditor('ctl00_ContentPlaceHolder1_fkContent_txtMemo').getContent();//editor.html();
                if(linkType=="1"&&content.length<1){
                    HiTipsShow("请输入正文内容！", "error");
                    return false;                
                }


                var IsShare=$("#ctl00_ContentPlaceHolder1_IsShare").prop("checked");

               

                $.ajax({
                    url: "articlesedit.aspx?id=<%=MaterialID%>",
                    type: "post",
                    data: "posttype=addsinglearticle&linkType="+encodeURIComponent(linkType)+"&linkUrl=" + encodeURIComponent(linkUrl) + "&title=" + encodeURIComponent(title) + "&img=" + encodeURIComponent(img) + "&memo=" +encodeURIComponent(memo)+ "&content=" +encodeURIComponent(content)+"&IsShare="+IsShare,
                    datatype: "json",
                    success: function (json) {
                        if (json.type == "1") {
                            HiTipsShow(json.tips, "success", function () { location.href = '<%=ReUrl%>' });
                        }else{
                            HiTipsShow(json.tips, "error");
                        }
                    }
                })
            })
        })
        
        function SingleArticleShow(){
            var title = $('#title').val();
            var img = $("#fmSrc").val();
            var linkUrl = $('#urlData').val();
            var linkType=$("#hidLinkType").val();
            if(linkType==""){
                linkType=1;
            }
            var memo = $('#digest').val();
            if ($.trim(title) == "") {
                HiTipsShow("请填写标题！", "error");
                return false;
            }
            if (img == "undefined"||img=="") {
                HiTipsShow("请选择封面图片！", "error");
                return false;
            }
            if (linkType!="1"&&$.trim(linkUrl)=="") {
                HiTipsShow("请设置链接地址！", "error");
                return false;
            }
            var content=UE.getEditor('ctl00_ContentPlaceHolder1_fkContent_txtMemo').getContent();//editor.html();
            if(linkType=="1"&&content.length<1){
                HiTipsShow("请输入正文内容！", "error");
                return false;
            }
            var IsShare=$("#ctl00_ContentPlaceHolder1_IsShare").prop("checked");


            $.ajax({
                url: "articlesedit.aspx?id=<%=MaterialID%>",
                type: "post",
                data: "posttype=addsinglearticle&linkType="+encodeURIComponent(linkType)+"&linkUrl=" + encodeURIComponent(linkUrl) + "&title=" + encodeURIComponent(title) + "&img=" + encodeURIComponent(img) + "&memo=" +encodeURIComponent(memo)+ "&content=" +encodeURIComponent(content)+"&IsShare="+IsShare,
                datatype: "json",
                success: function (json) {
                    if (json.type == "1") {
                        HiTipsShow(json.tips+"正在生成预览...", "success", function () { 
                            ArticleView(json.id);
                            $('#previewshow').on('hidden.bs.modal', function () {
                                /*模态框关闭的时候页面跳转到列表页*/
                                window.location.href = "<%=ReUrl%>";
                            })
                        });
                    }else{
                        HiTipsShow(json.tips, "error");
                    }
                }
            })
        }
        //function closeModalPic(modalid, Imgvalue) {
        //    var imgSrc = Imgvalue;
        //    if (imgSrc.length > 5) {
        //        BindPicData(Imgvalue);
        //    }
        //    $('#myModal').modal('hide');
        //}
        function BindPicData(img){
            $("#fmSrc").val(img);
            var smallimg = $("<img width='100' height='100'>");
            $("#smallpic").empty();
            smallimg.attr("src", img);
            $("#img1").attr("src", img);
            $("#smallpic").append(smallimg);
            $("#smallpic").show();
        }
        function closeModal(modalid, txtContentid, value, text) {
            var linkurl = 'http://' + value;
            $('#urlData').val(linkurl).hide();
            $('#title').val(text).keyup();
            $('#spLinkTitle').html("商品及分类");
            $('#' + modalid).modal('hide');
            $('#hidLinkType').val('2');
            //$('#divContent').hide();
            $('#urlData').show();
        }
        function syncTitle(value) {
            $("#singelTitle").text(value);
        }
        function syncAbstract(value) {
            $("#Lbmsgdesc").text(value);
            $("#digest").val(getStrbylen(value,240));
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form runat="server">
        <div class="page-header">
            <h2><%=htmlOperName %>单条图文</h2>
        </div>
        <div class="blank">
            <a href="<%=ReUrl %>" class="btn btn-primary">&lt;&lt; 返回图文管理</a>
        </div>
        <div class="edit-text clearfix">
            <div class="edit-text-left">
                <div class="mobile-d">
                    <div class="mobile-header">
                        <i></i>
                        <div class="mobile-title">店铺主页</div>
                    </div>
                    <div class="mobile mate-list">
                        <div class="mate-inner">
                            <span><%=htmlDate %></span>
                            <h3 id="singelTitle"><%=htmlArticleTitle %></h3>
                            <div class="mate-img" <%--style="width:320px;height:145px;overflow:hidden"--%>>
                                <img id="img1" src="../images/320x145.gif" class="img-responsive">
                            </div>
                            <div class="mate-info" id="Lbmsgdesc"><%=htmlMemo %></div>
                            <div class="red-all clearfix">
                                <strong class="fl">阅读原文</strong>
                                <em class="fr">&gt;</em>
                            </div>
                        </div>
                    </div>
                    <div class="mobile-footer"></div>
                </div>
            </div>
            <div class="edit-text-right" id="box_move">
                <div class="edit-inner">
                    <div class="form-group">
                        <label for="exampleInputEmail1"><em>*</em>标题：</label>
                        <input type="hidden" value="<%=htmlLinkType %>" id="hidLinkType" /><input type="text" class="form-control" id="title" placeholder="单图文标题" onkeyup="syncTitle(this.value)" value="<%=(htmlArticleTitle == "单条图文标题"?"":htmlArticleTitle) %>" maxlength="50" />
                        <small class="help-block">建议不多于30个字</small>
                    </div>
                    <div class="form-group">
                        <label><em>*</em>封面：</label>
                        <div id="smallpic" style="display: none;"></div>
                        <input id="fmSrc" type="text" value="" style="display: none;" />
                        <button type="button" class="btn btn-primary form-control" id="Picture">选择图片</button>
                        <small class="help-block inline">建议尺寸：600×346像素，小于200KB，支持.jpg、.gif、.png格式</small>
                    </div>
                    <input type="text" class="form-control" id="urlData" placeholder="" value="<%=htmlUrl %>" style="margin-top: 5px;">
                    <div class="form-group">
                        <p>
                            <label><em>&nbsp;</em>摘要：</label></p>
                        <textarea rows="6" id="digest" name="digest" placeholder="请输入摘要内容" onkeyup="syncAbstract(this.value)"><%=(htmlMemo == "摘要"?"":htmlMemo) %></textarea>
                    </div>

                    <div class="form-group" id="divContent">
                        <p><label>正文：</label></p>
                        <Kindeditor:KindeditorControl ID="fkContent" runat="server" Width="558" Height="200" />
                    </div>
                    <div class="form-group">
                    <label>分销商分享素材:<input type="checkbox" id="IsShare" checked="checked" runat="server" style="margin:-3px 5px 0px 10px" />是</label>
                  </div>
                </div>
            </div>
        </div>
        <div style="height: 100px; width: 100%">&nbsp;</div>
        <div class="footer-btn navbar-fixed-bottom">
            <input type="hidden" id="viewJson" />
            <button type="button" class="btn btn-success" id="btn-save">保存</button>
            <button type="button" class="btn btn-success" onclick="SingleArticleShow()">保存并预览</button>
        </div>

        <div class="modal fade" id="myIframeModal">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">商品及分类</h4>
                    </div>
                    <div class="modal-body">
                        <iframe src="" id="MyGoodsAndTypeIframe" width="750" height="370"></iframe>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
        </div>

        <!-- 模态框（Modal） -->
        <div class="modal fade" id="myModal" tabindex="-1" role="dialog"
            aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close"
                            data-dismiss="modal" aria-hidden="true">
                            &times;
                        </button>
                        <h4 class="modal-title" id="myModalLabel">图片
                        </h4>
                    </div>
                    <div class="modal-body">
                        <iframe src="" id="MyIframe" width="750" height="370"></iframe>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default"
                            data-dismiss="modal">
                            关闭
                        </button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
            <!-- /.modal -->
        </div>
    </form>
</asp:Content>
