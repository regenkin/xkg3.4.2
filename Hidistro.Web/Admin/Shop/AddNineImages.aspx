<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="AddNineImages.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.AddNineImages" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
     <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">
    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">

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

    <script src="../js/jquery.gridly.js" type="text/javascript"></script>
    <script type="text/javascript">
        var share_id ="<%=nid%>";
        $(function () {
            
            $('.gridly').gridly({
                base: 70, // px 
                gutter: 10, // px
                columns: 6
            });

            $("#ShareTitle").change(function () {
                var sharetitle = $(this).val();
                if (sharetitle.length > 100) {
                    sharetitle = sharetitle.substring(0,100)
                }
                $(this).val(sharetitle);
                $("#ShareTitleShow1").text(sharetitle);

            });
            var pX, pY;
            $('.gridly').on("mousedown", ".waitfile", function (e) {
                pX = e.pageX;
                pY = e.pageY;
            });
            $('.gridly').on("mouseup", ".waitfile", function (e) {
                if (pX == e.pageX && pY == e.pageY) {
                    $selLi = $(this);
                    editType = "add";
                    showSingle();
                }
            });
            

            $('.gridly li').on("mouseup", "a", function (e) {
                $selLi = $(this).parents("li");
                editIndex = -1;
                editIndex = $(".fileok").index($selLi);

                if ($(this).text() == "编辑") {
                    editType = "edit";
                    showSingle();
                } else if ($(this).text() == "删除") {
                    $selLi.html("<a>+<a>").addClass("waitfile").removeClass("fileok");
                    $('.y-imglist li.img').eq(editIndex).html("<p>图片" + (editIndex * 1 + 1 * 1) + "</p>").addClass("wait").removeClass("img");
                    reSetimglist();
                };
                
            });


            $("#saveBtn").click(function () {
                
                var PostData = { task: "", ID: "0", ShareDesc: "", image1: "", image2: "", image3: "", image4: "", image5: "", image6: "", image7: "", image8: "", image9: "" };
                var imgCount = 0;
                $(".fileok").each(function (i) {
                    var img = "image" + (1 * 1 + i * 1);
                    PostData[img] = $(this).find("img").attr("src");
                    imgCount = imgCount + 1;
                });
                var shareDesc=$("#ShareTitle").val()
                PostData.ShareDesc = shareDesc;

                if ($.trim(shareDesc) == "") {
                    HiTipsShow("请填写标题！", "error");
                    return false;
                }
                 
                if (imgCount < 1) {
                    HiTipsShow("请选择图片！", "error");
                    return false;
                }
                
                PostData.ID = share_id;
                if (PostData.ID > 0) {
                    PostData.task = "edit";
                } else {
                    PostData.task = "add";
                }
                //alert("task:" + PostData.task + ",ID:" + PostData.ID + ",ShareDesc:" + PostData.ShareDesc + ",image1:" + PostData.image1 + ",image2:" + PostData.image2 + ",image3:" + PostData.image3);
                $.ajax({
                    url: "AddNineImages.aspx",
                    type: 'post',
                    data: PostData,
                    success: function (data) {
                        if (data == "success") {
                            HiTipsShow("保存成功", "success", function () {
                               window.location.href = "ManageNineImages.aspx";
                            });
                           
                        } else {
                            HiTipsShow("保存失败,请重试！", "error");
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        // alert(XMLHttpRequest.status);
                        // alert(XMLHttpRequest.readyState);
                        // alert(textStatus);
                        HiTipsShow("访问服务器出错，请重试！", "warning");
                    }
                })

            });

            $("#BatchClear").click(function () {
                var $fileok = $(".fileok");

                $fileok.each(function (i) {

                    $(this).html("<a>+<a>").addClass("waitfile").removeClass("fileok");
                    $('.y-imglist li.img').eq(0).html("<p>图片" + (editIndex * 1 + 1 * 1) + "</p>").addClass("wait").removeClass("img");
                   
                });

                reSetimglist();

            });

            $("#BatchUp").click(function () {

                if ($(".waitfile").length < 1) {
                    HiTipsShow("九宫格图片已满，请空后再选择！", "warning");
                    return;
                }

                HiShop.popbox.ImgPicker(function (obj) {
                   
                    var imgs = obj;

  
                    if (imgs.length > 0) {
                        var $waitfile = $(".waitfile");
                        var licount = $waitfile.length;

                       

                        $.each(imgs,function (i) {
                            if (i < licount) {
                                var $nli = $waitfile.eq(i);
                                $nli.html("<img src='" + imgs[i] + "'> <div class='exitremove'> <a>编辑</a><a>删除</a></div>");
                                if (!$nli.hasClass("fileok")) {
                                    $nli.addClass("fileok");
                                    $nli.removeClass("waitfile");
                                };

                                $('.y-imglist li.wait').eq(0).removeClass('wait').addClass('img').html("<img src='" + imgs[i] + "'>");


                            }
                        });
                        reSetimglist();
                    };



                });
            });

           



            if (share_id > 0) {

                $.ajax({
                    url: "AddNineImages.aspx",
                    type: 'post',
                    data: {ID:share_id,task:"read"},
                    success: function (data) {
                        if (data.indexOf("falid：") == 0) {
                            share_id = 0;
                            HiTipsShow(data, "warning");
                            return;
                        };
                        var jsondata = JSON.parse(data);

                        if (jsondata != null) {

                            $("#ShareTitle").val(jsondata.ShareDesc); //标题内容
                            $("#ShareTitle").trigger("change");

                            var imgs = [1,2,3,4,5,6,7,8,9];
                            $.each(imgs, function (i) {
                                imgs[i] = jsondata["image" + (i * 1 + 1 * 1)];
                            });


                            ///清空已有数据
                            $(".fileok").html("<a>+<a>").addClass("waitfile").removeClass("fileok");
                            $('.y-imglist li.img').html("<p>图片1</p>").addClass("wait").removeClass("img");




                            var $waitfile = $(".waitfile");
                            var licount = $waitfile.length;

                            $.each(imgs, function (i) {
                                if (i < licount) {
                                    var $nli = $waitfile.eq(i);
                                    if (imgs[i] != "") {

                                    $nli.html("<img src='" + imgs[i] + "'> <div class='exitremove'> <a>编辑</a><a>删除</a></div>");
                                    if (!$nli.hasClass("fileok")) {
                                        $nli.addClass("fileok");
                                        $nli.removeClass("waitfile");
                                    };

                                    $('.y-imglist li.wait').eq(0).removeClass('wait').addClass('img').html("<img src='" + imgs[i] + "'>");

                                    }

                                }
                            });
                            reSetimglist();


                        } else {
                            share_id = 0;
                            HiTipsShow(data, "warning");
                            return;
                        }

                    },
                    error: function () {
                        HiTipsShow("访问服务器出错", "warning");
                        share_id = 0;
                    }
                })

            }
            
        })




        function showSingle() {
            //var Rand = Math.random();
            //$("#MyIframe").attr("src", "../weibo/Pictures.aspx?Rand=" + Rand);
            //$('#myModal').modal('toggle').children().css({
            //    width: '800px',
            //    height: '700px'
            //})
            //$("#myModal").modal({ show: true });

            HiShop.popbox.ImgPicker(function (obj) {
                BindPicData(obj);
            });
        }

        var $selLi = null;
        var editType = "add";
        var editIndex = -1;

        function BindPicData(Imgvalue) {
            var imgSrc = Imgvalue[0];

            $selLi.html("<img src='" + imgSrc + "'> <div class='exitremove'> <a>编辑</a><a>删除</a></div>");
            if (!$selLi.hasClass("fileok")) {
                $selLi.addClass("fileok");
                $selLi.removeClass("waitfile");
            };

            if (editType == "add") {
                $('.y-imglist li.wait').eq(0).removeClass('wait').addClass('img').html("<img src='" + imgSrc + "'>");
            }

            if (editType == "edit") {
                $('.y-imglist li.img').eq(editIndex).removeClass('wait').addClass('img').html("<img src='" + imgSrc + "'>");
            }

            reSetimglist();
        }
    </script>
    <style>
        .mt120{margin-top:50px}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="page-header">
                    <h2><asp:Literal ID="EditType" Text="新增" runat="server" ClientIDMode="Static"></asp:Literal>九图一文素材</h2>
                </div>
                <div class="edit-text clearfix">
                    <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">店铺主页</div>
                                    </div>
                                    <div class="ninefigureone">
                                        <div class="ninefigureonecontent">
                                            <div class="userimg">
                                                <img src="/Utility/pics/headlogo.jpg">
                                            </div>
                                            <div class="ninefigureoneinfoimg">
                                                <h3 class="mb5">您的昵称</h3>
                                                <p class="info mb5" id="ShareTitleShow1">分享标题内容</p>

                                                <ul class="clearfix y-imglist">
                                                    <li class="img">
                                                        <img src="/Utility/pics/headlogo.jpg">
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片2</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片3</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片4</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片5</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片6</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片7</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片8</p>
                                                    </li>
                                                    <li class="wait">
                                                        <p>图片9</p>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="clear-line">
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                    <div class="edit-text-right mt120">
                        <div class="edit-inner">
                            <div class="form-group">
                                <label><em>*</em>标题：</label>
                                <textarea rows="5" id="ShareTitle" style="padding:5px">分享标题内容</textarea>
                            </div>
                            <div class="form-group">
                                <label><em>*</em>九宫格图片：</label><br />
                               <button class="btn btn-primary bl mt5" id="BatchUp">选择图库图片</button>　<button class="btn btn-danger bl mt5" id="BatchClear">一键清空</button>
                            </div>
                          
                            <div class="y-fileimglist" style="margin-left:10px">
                                <ul class="clearfix gridly">
                                    <li class="fileok">
                                        <img src="/Utility/pics/headlogo.jpg">
                                        <div class="exitremove"> <a>编辑</a><a>删除</a></div>
                                    </li>
                                    <li class="waitfile">
                                         <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                         <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                        <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                        <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                        <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                        <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                        <a>+</a>
                                    </li>
                                    <li class="waitfile">
                                        <a>+</a>
                                    </li>
                                </ul>
                            </div>
                            <small>上传1-9张图片，建议上传小于200K的.jpg、.gif、.png格式图片；</small>
                            <small>左键按住图片，可拖拽图片进行排序</small>
                        </div>
                    </div>
                </div>


    <div class="footer-btn navbar-fixed-bottom">
        <button type="button" class="btn btn-primary" id="saveBtn">保存</button>
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
</asp:Content>
