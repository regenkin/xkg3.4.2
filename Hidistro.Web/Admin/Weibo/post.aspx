<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Post" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="/admin/css/weibo.css">
       <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">

    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">
  
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">
 
    <script>

        jQuery(function () {
           
            jQuery("#Homepage").click(function () {
                jQuery("#txtContent").val($("#txtContent").val() + (jQuery(this).attr("gotourl"))).keyup();
            });
            jQuery('#GoodsAndType').click(function () {
                var Rand = Math.random();
                jQuery("#MyGoodsAndTypeIframe").attr("src", "Goods.aspx?Rand=" + Rand);
                jQuery('#myIframeModal').modal('toggle').children().css({
                    width: '800px',
                    height: '700px'
                })
                jQuery("#myIframeModal").modal({ show: true });
            });
          
        })
        function statusesupdate() {
            if (jQuery.trim(jQuery("#txtContent").val()) == "") {
                HiTipsShow("说点什么吧！", 'warning');
                return;
            }
            var imgvalue = $("#imgvalue").attr('src');
            if (typeof (imgvalue) == "undefined") {
                imgvalue = "";
            }
            jQuery("#send").button('loading');
           
            var url = "&status=" + encodeURI($("#txtContent").val()) + "&img=" + imgvalue;
       
            jQuery.getJSON("../../API/WeiBoAPI.ashx?action=statusesupdate" + url).done(function (d) {
                jQuery("#send").button('reset');
                if (jQuery.trim(d.created_at) != "") {
                    
                    HiTipsShow("发布成功！", 'success');
                    jQuery("#picback").html('');
                    $("#txtContent").val('');
                    InitTextCounter(140, "#txtContent", "#iLeftWords");
                }
                else {
                    HiTipsShow("发布失败！", 'fail');
                    
                }
                
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     
            
        <div class="page-header">
                    <h2>发布微博</h2>
                    <small></small>
    </div>
    
                             <div class="app-init-container">
        <div class="sinaweibo-letter-wrap">

            
            <div class="wb-sender">
                <div class="wb-sender__inner">
                    <div class="wb-sender__input js-editor-wrap">
                        <div class="misc top clearfix">
                            <div class="content-actions clearfix">

                                <div class="editor-module insert-emotion">
                                    <a class="js-open-emotion" style="" data-action-type="emotion" href="javascript:;">表情</a>
                                    <div class="emotion-wrapper">
                                        <ul class="emotion-container clearfix">
                                        </ul>
                                    </div>
                                </div>
                                <div class="editor-module insert-article">
                                    <a class="js-open-articles"  id="Picture" href="javascript:;"   
                                      >选择图片</a>
                                </div>
                                <div class="editor-module insert-shortlink" style ="display:none;">
                                    <div class="js-reply-menu dropup hover dropdown--right add-reply-menu">
                                        <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown">
                                            <span class="dropdown-current">插入链接</span>
                                            <i class="caret"></i>
                                        </a>
                                        <ul class="dropdown-menu">
                                            <li><a data-link-type="Goods" id="GoodsAndType" class="js-open-shortlink" href="javascript:;">商品及分类</a></li>
                                            <li><a data-link-type="Homepage" id="Homepage" class="js-open-shortlink" href="javascript:;" gotourl=' http://<%= Request.Url.Host+"/Default.aspx" %> '>店铺主页</a></li>
                                        </ul>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="content-wrapper">
                            <textarea id="txtContent" class="js-txta" cols="50" rows="4"></textarea>
                            <div class="js-picture-container picture-container"></div>
                            <div class="complex-backdrop"  >
                                <div class="js-complex-content complex-content" id="picback"></div>
                            </div>
                        </div>

                        <div class="misc clearfix">
                            <div class="content-actions clearfix">
                                <div class="word-counter pull-right">还能输入 <i id="iLeftWords">300</i> 个字</div>
                                <button class="btn btn-primary" onclick="statusesupdate();" data-loading-text='正在提交...'  id="send">发送</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
        
   
    <div class="notify-bar js-notify animated hinge hide">
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
                    <h4 class="modal-title" id="myModalLabel">图片库
                    </h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyIframe" width="1040" height="650" scrolling="no"  ></iframe>
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
    <div class="modal fade" id="myIframeModal">
    <div class="modal-dialog"   >
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
        </div><!-- /.modal-content -->
    </div><!-- /.modal-dialog -->
</div>

    <!-- end tpl_albums_imgs -->
     <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>
    <script src="/Admin/shop/Public/plugins/zclip/jquery.cookie.js"></script>
    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
    <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Admin/shop/Public/js/config.js"></script>
       
    <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>

     <script src="/Admin/js/HiShopComPlugin.js"></script>
    <script src="/Admin/shop/Public/js/dist/componentadmin-min.js?v1023"></script>
     <script src="/Admin/js/weiboHelper.js"></script>
    <style type="text/css">
        #dropdow-menu-link{float:left}
        .subtitlespan {
            display: inline-block;
            width: 220px;
            overflow: hidden;
        }
        .jbox-container {
            overflow-x: hidden;
        }
        .gagp-goodslist .twoclass ul li {
            padding: 0;
            border-bottom: none;
        }
        /*.gagp-goodslist{
            min-height:300px !important;
        }*/
        .modalshopclasslist .oneclass p:first-child i.down {
            background: url("../images/iconfont-14052230.png");
        }</style>
    <script type="text/javascript">
        $(document).ready(function () {
      
            InitTextCounter(140, "#txtContent", "#iLeftWords");
            CreateDropdown($("#txtContent"), $(".content-actions").eq(0), { createType: 1, showTitle: false, txtContinuity: true, reWriteSpan: false, iscallback: false, style: "padding:0px;" });

            $("#dropdow-menu-link  li[data-val='20']").remove();
            $('#Picture').click(function () {
                //var Rand = Math.random();
                //$("#MyIframe").attr("src", "PublicPictures.aspx?Rand=" + Rand);
                //$('#myModal').modal('toggle').children().css({
                //    width: '1100px',
                //    height:'650px'
                //})
                //$("#myModal").modal({ show: true });
                HiShop.popbox.ImgPicker(function (obj) {
             
                   closeModalPic("myModal", obj);
                    //gotovalue(a);
                });
                //HiShop.popbox.GoodsAndGroupPicker("goods", function (list) {
                    
                //});
            });
          
        })
    </script>



</asp:Content>
