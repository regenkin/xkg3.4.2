<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Admin/AdminNew.Master"
    CodeBehind="ImageData.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.ImageData" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <Hi:Script ID="Script2" runat="server" Src="/Utility/swfupload/handlers.js"></Hi:Script>
    <Hi:Script ID="Script7" runat="server" Src="/utility/iframeTools.js" />
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <Hi:Script ID="Script1" runat="server" Src="/Utility/swfupload/swfupload.js"></Hi:Script>

    <%--<link rel="stylesheet" href="/admin/css/css.css" type="text/css" media="screen" />--%>
    <link rel="stylesheet" href="/utility/skins/blue.css" type="text/css" media="screen" />

    <script type="text/javascript" src="/Utility/swfupload/swfobject.js"></script>
    <style type="text/css">
        .txtCss {
            color: #ffad34;
            font-size: 14px;
            font-style: normal;
        }

        .verticalalignTop {
            vertical-align: top;
        }

        .numCss {
            color: #ff6600;
            font-size: 12px;
        }

        .aCss {
            font-size: 12px;
        }

        .typNameCss {
            color: #0b5ba5;
        }

        .liCss {
            margin-bottom: 3px;
        }

        .table td {
            width: 217px;
            height: 217px;
        }

        .uploadify-queue {
            margin-bottom: 1em;
            position: absolute;
            width: 360px !important;
            right: 10px !important;
            top: 50px !important;
            left: auto !important;
            z-index: 999;
        }

        .uploadify-queue-item {
            background-color: #F5F5F5;
            border-radius: 3px;
            font: 11px Verdana,Geneva,sans-serif;
            margin-top: 5px;
            max-width: 350px;
            padding: 10px;
            box-shadow: 0px 0px 1px 2px rgba(0, 0, 0, 0.3);
        }

        .uploadify-progress {
            background-color: #E5E5E5;
            margin-top: 10px;
            width: 100%;
        }

        .uploadify-progress-bar {
            background-color: #09F;
            height: 3px;
            width: 1px;
        }

        .uploadify-button {
            background: #5CB85C none repeat scroll 0% 0% !important;
            border: 1px solid #4CAE4C !important;
            border-radius: 2px !important;
            color: #FFF;
            font: bold 12px Arial,Helvetica,sans-serif;
            text-align: center;
            text-shadow: 0px -1px 0px rgba(0, 0, 0, 0.25);
            padding: 0px !important;
        }

        #j-addImg {
            float: left;
            margin-top:1px;
        }
    </style>

    <script type="text/javascript">
        //$(document).ready(function () {
        //    $('#typeDiv').find('span').each(function () {
        //        if ($(this).attr('id') != 'ctl00_ContentPlaceHolder1_ImageTypeID') {
        //            $(this).removeClass();
        //            $(this).addClass('numCss');
        //            $(this).css('margin-left', '2px');
        //        }
        //    });
        //    $('#typeDiv').find('a').each(function () {
        //        $(this).removeClass();
        //        $(this).addClass('aCss');
        //        var url = $(this).attr('href');
        //        //url = url.replace('/store/ImageData.aspx', '/Shop/ImageData.aspx');
        //        $(this).attr('href', url);
        //    });
        //    $('#typeDiv').find('li').each(function () {
        //        $(this).removeClass();
        //        $(this).addClass('liCss');
        //    });
        //});


        //function copySuccess() {
        //    ShowMsg("图片地址已经复制，你可以使用Ctrl+V 粘贴！", true);
        //}
        //var myHerf = window.location.host;
        //var myproto = window.location.protocol;
        //applicationPath = "";
        //function bindFlashCopyButton(value, containerID) {
        //    var flashvars = {
        //        content: encodeURIComponent(myproto + "//" + myHerf + applicationPath + value),
        //        uri: '/Utility/swfupload/flash_copy_btn.png'
        //    };
        //    var params = {
        //        wmode: "transparent",
        //        allowScriptAccess: "always"
        //    };
        //    swfobject.embedSWF("/Utility/swfupload/clipboard.swf", containerID, "23", "12", "9.0.0", null, flashvars, params);
        //}

        //function setdisplay(obj, val) {
        //    var a = $(obj).children("em")
        //    for (i = 0; i < a.length; i++) {
        //        a[0].style.display = val == 0 ? 'none' : '';
        //    }
        //}



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>图片库</h2>
        </div>
        <table style="width: 100%; margin-top: 5px;">
            <tr>
                <td style="width: 77%;">
                    <ul>
                        <li class="batchHandleButton">
                            <input type="file" name="imgpicker_upload_input" id="j-addImg">
                            <span class="signicon"></span>
                            <button type="button" class="btn btn-success btn-xs ml10" onclick="CheckClickAll()">
                                全选<span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                            </button>
                            <button type="button" class="btn btn-success btn-xs ml5" onclick="CheckReverse()">
                                反选
                                    <span class="glyphicon glyphicon-fullscreen" aria-hidden="true"></span>
                            </button>
                            <span style="padding: 0px 5px 0px 5px;">
                                <img src="../images/u5_line.png"></span>
                            <button type="button" class="btn btn-primary btn-xs" onclick="MoveImg()">
                                移动到
                            </button>
                            <asp:Button ID="btnHiddenDel" runat="server" CssClass="hide" />
                            <button type="button" id="btnDelete1" runat="server" class="btn btn-danger btn-xs ml5" onclick="return delSel(this)">
                                删除
                                    <span class="glyphicon glyphicon-trash" aria-hidden="true"></span>
                            </button>
                            <%--                            <span style="padding: 0px 5px 0px 5px;">
                                <img src="../images/u5_line.png"></span>--%>
                        </li>
                    </ul>
                </td>
                <td width="20%" nowrap="nowrap">
                    <div class="form-inline">
                        <%--<label>请选择上传图片的分类：</label>
                            <Hi:ImageDataGradeDropDownList ID="dropImageFtp2" CssClass="form-control resetSize"
                                runat="server" NullToDisplay="请选择上传图片的路径" AutoPostBack="true" />--%>
                        <label style="font-weight: normal">显示顺序:</label>
                        <Hi:ImageOrderDropDownList CssClass="form-control resetSize" runat="server"
                            ID="ImageOrder" />
                        <span>
                            <asp:TextBox ID="txtWordName" CssClass="form-control resetSize" Width="110" runat="server" placeholder="图片名" />
                        </span>
                        <span>
                            <asp:Button CssClass="btn btn-primary resetSize" ID="btnImagetSearch" runat="server" Text="搜索" />
                        </span>
                    </div>
                </td>
            </tr>
        </table>







        <div class="picture-library clearfix">
            <div class="picture-library-left fl">
                <h4>图片分组</h4>
                <Hi:ImageTypeLabel runat="server" ID="ImageTypeID" TypeId="0" />
                <a class="btn btn-primary resetSize" href="javascript:void(0)" onclick="location.href = 'ImageType.aspx'">分组管理</a>
            </div>
            <div class="picture-library-right fl ml15">
                <div class="selectallimg">
                    共
                    <asp:Label ID="lblImageData" runat="server" Text="0" CssClass="red"></asp:Label>
                    张图片
                </div>
                <div class="picture-library-imglist">
                    <asp:Repeater ID="rptList" runat="server">
                        <HeaderTemplate>
                            <ul class="clearfix">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <li photoid="<%#Eval("PhotoId") %>">
                                <img src="<%# Eval("PhotoPath")%>">
                                <div class="shopimgexit">
                                    <p class="y3-shopimgname"><%# Eval("PhotoName")%></p>
                                    <div class="exitimgnameinput clearfix">
                                        <input type="text" maxlength="50">
                                        <input type="button" photoid="<%#Eval("PhotoId") %>" class="btn btn-primary btn-xs btnconfirm" value="确定" />
                                    </div>
                                    <div class="y3-exit-btn">
                                        <a href="<%# Eval("PhotoPath")%>" target="_blank" title="查看大图"><span class="glyphicon glyphicon-picture"></span></a>
                                        <span class="glyphicon glyphicon-pencil exitimgname" title="修改名称"></span>
                                        <input id="btn<%#Eval("PhotoId") %>" type="button" class="delphoto hide" title="删除图片" photoid="<%#Eval("PhotoId") %>" />
                                        <span class="glyphicon glyphicon-trash" title="删除图片"></span>
                                    </div>
                                </div>
                                <div class="imgselectMask"><i class="glyphicon glyphicon-ok"></i></div>
                            </li>
                        </ItemTemplate>
                        <FooterTemplate></ul></FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>






















        <div class="pageNumber">
            <div class="pagination">
                <UI:Pager runat="server" ShowTotalPages="true" ID="pager" DefaultPageSize="18" />
            </div>
        </div>

        <!--更改图片名称-->



        <div class="modal fade" id="ImageDataWindowName">
            <div class="modal-dialog ">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">文件名称更改</h4>
                    </div>
                    <div class="modal-body">
                        <asp:HiddenField ID="ReImageDataNameId" Value='' runat="server" />

                        <div class="form-group">
                            <label class="col-xs-2 control-label">图片名称：</label>
                            <div class="col-xs-8">
                                <asp:TextBox ID="ReImageDataName" runat="server" CssClass="form-control" Width="300px"></asp:TextBox>
                                <small class="help-block">图片名称不能为空,长度限制在30个字符以内</small>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnSaveImageDataName" runat="server" Text="确认" CssClass="btn btn-success" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->
            </div>
        </div>



        <!--图片路径替换-->
        <div id="ImageDataWindowFtp" style="display: none">
            <div class="frame-content">
                <asp:HiddenField ID="RePlaceImg" Value='' runat="server" />
                <asp:HiddenField ID="RePlaceId" Value='' runat="server" />
                <p>
                    <span class="frame-span frame-input90">上传图片：<em>*</em></span>
                    <asp:FileUpload ID="FileUpload" runat="server" onchange="FileExtChecking(this)" />
                </p>
            </div>
        </div>
        <!--文件移动-->



        <div class="modal fade" id="ImageDataWindowMove">
            <div class="modal-dialog ">
                <div class="modal-content form-horizontal">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <h4 class="modal-title">移动图片管理</h4>
                    </div>
                    <div class="modal-body ">
                        <div class="form-group">
                            <label class="col-xs-2 control-label">选择分组：</label>
                            <div class="col-xs-8">
                                <Hi:ImageDataGradeDropDownList ID="dropImageFtp" CssClass="form-control" runat="server" TypeId="1" />
                            </div>
                        </div>


                    </div>
                    <div class="modal-footer">
                        <asp:HiddenField ID="hdfSelIDList" runat="server" />
                        <asp:Button ID="btnMoveImageData" runat="server" Text="文件移动" CssClass="btn btn-success" />
                        <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                    </div>
                </div>
                <!-- /.modal-content -->

            </div>
        </div>

    </form>

    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ctl00_ContentPlaceHolder1_ImageOrder").change(function(){
                location.href="imagedata.aspx?orderby="+$(this).val();
            })


            $("#j-addImg").uploadify({
                debug: !1,
                auto: !0,
                width: 86,
                height: 22,
                multi: !0,
                swf: "/Admin/shop/Public/plugins/uploadify/uploadify.swf",
                uploader: "/hieditor/ueditor/net/controller.ashx?action=uploadtemplateimage",
                buttonText: "上传图片",
                fileSizeLimit: "2MB",
                fileTypeExts: "*.jpg; *.jpeg; *.png; *.gif; *.bmp",
                onUploadStart: function () {
                    $("#j-addImg").uploadify("settings", "formData", {
                        cate_id: <%=Globals.RequestQueryNum("ImageTypeId")%>,
                        //ASPSESSID: a.cookie("ASPSESSID")
                    })
                },
                onSelectError: function (a, b, c) {
                    switch (b) {
                        case -100: ShowMsg("对不起，系统只允许您一次最多上传10个文件",false);
                            break;
                        case -110: ShowMsg("对不起，文件 [" + a.name + "] 大小超出2MB！",false);//系统已经提示，这里加了就会重复提示
                            break;
                        case -120: ShowMsg("文件 [" + a.name + "] 大小异常！",false);
                            break;
                        case -130: ShowMsg("文件 [" + a.name + "] 类型不正确！",false);
                    }
                },
                onFallback: function () {
                    ShowMsg("您未安装FLASH控件，无法上传图片！请安装FLASH控件后再试。",false);
                },
                onQueueComplete: function (a) {
                    ShowMsgAndReUrl("上传成功！", true, localUrl);
                },
                onUploadError: function (a, b, c, d) {
                    ShowMsg("对不起：" + a.name + "上传失败：" + d,false);
                }
            })
        });



        var localUrl = "<%=localUrl%>";
    $(function () {
        $('.picture-library-imglist ul li').click(function () {
            $(this).toggleClass('selected');
            $(this).find('.exitimgnameinput').hide();
            $(this).find('.y3-shopimgname').removeAttr('style');
        });
        $('.y3-exit-btn span,.exitimgnameinput').click(function (evt) {
            evt.stopPropagation();
            if ($(this).hasClass('exitimgname')) {
                $(this).parent().prev().show();
                $(this).parent().prev().find('input').eq(0).val($(this).parent().prevAll('.y3-shopimgname').text());
                $(this).parent().prevAll('.y3-shopimgname').hide();
            }
        })
        //$(".exitimgnameinput input[type='text']").dblclick(function (evt) {
        //    evt.stopPropagation();
        //})
        $(".y3-exit-btn .glyphicon-trash").click(function (evt) {
            evt.stopPropagation();
            $(this).prev().click();
        })
        $(".y3-exit-btn .delphoto").click(function (evt) {
            var obj = this;
            evt.stopPropagation();
            var pid = $(this).attr("photoid");
            if (HiConform('<strong>确定要执行删除操作吗？</strong><p>删除后所有引用过该图片的地方都将找不到图片！</p>', obj)) {
                $.ajax({
                    url: "/Admin/shop/api/Hi_Ajax_DelImg.ashx",
                    type: "post",
                    dataType: "json",
                    data: {
                        "file_id[]": pid,
                    },
                    success: function (a) {
                        if (a.status) {
                            ShowMsgAndReUrl("图片删除成功！", true, localUrl);
                        } else {
                            ShowMsg(a.msg, false);
                        }
                    }
                })
            }

        })
        $(".btnconfirm").click(function (evt) {
            evt.stopPropagation();
            var obj = this;
            var pid = $(this).attr("photoid");
            var name = $(this).prev().val();
            $.ajax({
                url: "/Admin/shop/api/Hi_Ajax_RenameImg.ashx",
                type: "post",
                dataType: "json",
                data: {
                    file_id: pid,
                    file_name: name,
                    type: 0
                },
                success: function (a) {
                    if (a.status) {
                        $(obj).parent().parent().find(".y3-shopimgname").html(name).removeAttr('style');
                        $(obj).parent().hide();
                        ShowMsg("名称修改成功", true)
                        //ShowMsgAndReUrl("名称修改成功！", true, localUrl);
                    }
                }
            })
        })
    });
    </script>
    <script type="text/javascript">
        //var formtype = "change";
        //function validatorForm() {
        //    var imgsrc = "", imgid = "";
        //    switch (formtype) {
        //        case "change":
        //            imgsrc = $("#ctl00_contentHolder_ReImageDataName").val().replace(/\s/g, "");
        //            if (imgsrc.length <= 0) {
        //                ShowMsg("图片名称不允许为空！", false);
        //                return false;
        //            }
        //            break;
        //        case "remove":
        //            if (!confirm("您确定要移动选中的图片吗？")) {
        //                return false;
        //            }
        //            break;
        //    };
        //    return true;
        //}
        function delSel(o) {
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val("");
            var obj = $(".picture-library-imglist li[class='selected']");
            var v = "";
            obj.each(function () {
                v += $(this).attr("photoid") + ",";
            })
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val(v);
            if (obj.length > 0) {
                if (HiConform('<strong>确定要执行删除操作吗？</strong><p>删除后所有引用过该图片的地方都将找不到图片！</p>', o)) {
                    $("#ctl00_ContentPlaceHolder1_btnHiddenDel").click();
                }
            } else {
                ShowMsg("请先选择要删除的图片！")
            }
        }

        //替换
        function RePlaceImg(imgSrc, imgId) {
            DialogFrame("ImageReplace.aspx?imgsrc=" + imgSrc + "&imgId=" + imgId + "&reurl=" + encodeURIComponent(location.href), '图片替换', 335, 140);
        }

        //反选
        function CheckReverse() {
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val("");
            var temp = $(".picture-library-imglist li[class!='selected']");
            $(".picture-library-imglist li[class='selected']").removeClass("selected");
            temp.addClass("selected");
            var obj = $(".picture-library-imglist li[class='selected']");
            var v = "";
            obj.each(function () {
                v += $(this).attr("photoid") + ",";
            })
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val(v);
            //var frm = document.aspnetForm;
            //for (i = 0; i < frm.length; i++) {
            //    e = frm.elements[i];
            //    if (e.type == 'checkbox' && e.name.indexOf('checkboxCol') != -1) {
            //        if (e.checked == false)
            //            e.checked = true;
            //        else
            //            e.checked = false;
            //    }
            //}
        }

        //全选
        function CheckClickAll() {
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val("");
            $(".picture-library-imglist li").removeClass("selected").addClass("selected");
            var obj = $(".picture-library-imglist li[class='selected']");
            var v = "";
            obj.each(function () {
                v += $(this).attr("photoid") + ",";
            })
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val(v);
        }

     <%--   var queueErrorArray;
        var swfu;
        $(function () {

            function loader(thing) {
                var settings = {
                    // Backend Settings
                    upload_url: "ImageData.aspx",
                    use_query_string: false,
                    post_params: {
                        iscallback: "true",
                        typeId: thing,
                        "ASPSESSID": "<%=Session.SessionID %>"
                    },
                    // File Upload Settings
                    file_size_limit: "501",
                    file_types: "*.jpg;*.gif;*.png;*.jpeg",
                    file_types_description: "JPG Images",
                    file_upload_limit: "0",    // Zero means unlimited

                    // Event Handler Settings - these functions as defined in Handlers.js
                    // The handlers are not part of SWFUpload but are part of my website and control how
                    // my website reacts to the SWFUpload events.
                    file_queue_error_handler: fileQueueError,
                    file_dialog_complete_handler: fileDialogComplete,
                    upload_progress_handler: uploadProgress,
                    upload_error_handler: uploadError,
                    upload_success_handler: uploadSuccess,
                    upload_complete_handler: uploadComplete,
                    // Button settings
                    button_image_url: "/DialogTemplates/images/swfupload_uploadBtn2.png",
                    button_placeholder_id: "spanButtonPlaceholder",
                    button_width: 63,
                    button_height: 22,
                    default_preview: "/DialogTemplates/images/07.png",

                    // Flash Settings
                    flash_url: "/DialogTemplates/swfupload/swfupload.swf", // Relative to this file
                    custom_settings: { upload_target: "divFileProgressContainer" }
                };
                //swfu = new SWFUpload(settings);
            };
            $("#ctl00_contentHolder_dropImageFtp2").change(function () {
                swfu.setPostParams({ "typeId": this.value, "iscallback": "true" });
            });
            loader(0);
        });--%>

        //改名
        //function ReImgName(imgName, imgId) {
        //    vilidsetings = {
        //        'ctl00$ContentPlaceHolder1$ImageDataWindowName': {
        //            validators: {
        //                notEmpty: {
        //                    message: '填写文件名称'
        //                },
        //                stringLength: {
        //                    min: 1,
        //                    max: 30,
        //                    message: '长度不能超过30个字符'
        //                }
        //            }
        //        }
        //    };
        //    arrytext = null;
        //    formtype = "change";
        //    $("#ctl00_ContentPlaceHolder1_ReImageDataName").val(imgName);
        //    $("#ctl00_ContentPlaceHolder1_ReImageDataNameId").val(imgId);
        //    //DialogShowNew('文件名称更改', 'imagecmp', 'ImageDataWindowName', 'ctl00_ContentPlaceHolder1_btnSaveImageDataName');
        //    $('#ImageDataWindowName').modal('toggle').children().css({
        //        width: '520px',
        //        height: '260px'
        //    })
        //    $("#ImageDataWindowName").modal({ show: true });
        //}

        function MoveImg() {
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val("");
            var obj = $(".picture-library-imglist li[class='selected']");
            var v = "";
            obj.each(function () {
                v += $(this).attr("photoid") + ",";
            })
            $("#ctl00_ContentPlaceHolder1_hdfSelIDList").val(v);
            if (obj.length > 0) {
                $('#ImageDataWindowMove').modal('toggle').children().css({
                    width: '600px',
                    height: '260px'
                })
                $("#ImageDataWindowMove").modal({ show: true });
            }
            else
                ShowMsg("请选择需要移动的图片！", false);
        }

        //function validatorForm() {
        //    $("#hform").find(":input").trigger("blur"); //触发验证
        //    var numError = $("#hform").find('.has-error').length;
        //    if (numError) return false; //验证未通过
        //    return true;
        //}


        //function DialogShowNew(hishop_titile, hishop_id, hishop_div, btnId) {

        //    var tform = $("<form id='hform'><form>"); //构造form,方便绑定验证方法
        //    tform.append($("#" + hishop_div).html());

        //    dialog = art.dialog({
        //        id: hishop_id,
        //        title: hishop_titile,
        //        content: tform[0],
        //        init: function () {
        //            if (arrytext != null) {
        //                getArryText(arrytext);
        //            }
        //        },
        //        resize: true,
        //        fixed: true,
        //        close: function () {
        //            arrytext = null;
        //        },
        //        button: [{
        //            name: '确 认', callback: function () {
        //                var istag = validatorForm();
        //                if (istag) {
        //                    var temparrytext = arrytext;
        //                    if (temparrytext != null) {
        //                        setShowText(temparrytext);
        //                        this.close();
        //                        getArryText(temparrytext);

        //                        $("#" + btnId).trigger("click");

        //                    }
        //                    //else if(btnId='ctl00_ContentPlaceHolder1_btnMoveImageData')
        //                    //{
        //                    //    setShowText(temparrytext);
        //                    //    this.close();
        //                    //    getArryText(temparrytext);
        //                    //    $("#" + btnId).trigger("click");
        //                    //}
        //                } else {
        //                    return false;
        //                }
        //            }, focus: true
        //        },
        //                { name: '取 消' }
        //        ]
        //    });


        //    $('#hform').formvalidation(vilidsetings);//绑定验证方法

        //}
    </script>
</asp:Content>


