<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Config" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Utility/swfupload/swfupload.js" type="text/javascript" charset="gbk"></script>
    <script src="../../Utility/swfupload/DisLogoupload.js" type="text/javascript" charset="gbk"></script>
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style>
        span, small {
            font-size: 12px;
        }

        .swfupload {
            float: left;
        }

        #SWFUpload_0 {
            margin-top: 55px;
            margin-left: 5px;
            border: 0px solid red;
            background: none;
        }

        #SWFUpload_1 {
            position: absolute;
            top: 0px;
            left: 0px;
        }

        .errorFocus {
            height: 34px;
            padding-left: 15px;
        }

        .msgError {
            color: red;
            background: url(../images/false.gif) left center no-repeat;
            border: none;
            padding-left: 24px;
        }

        .gray {
            color: #a19c9c;
            padding-left: 10px;
            display: none;
        }

        .wtx {
            clear: both;
            margin-left: 190px;
            font-size: 12px;
            line-height: 20px;
            color: #a19c9c;
        }

        .dialog-content td {
            vertical-align: middle;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="page-header">
        <h2>店铺信息</h2>
    </div>
    <form id="thisForm" runat="server" class="form-horizontal">
        <asp:HiddenField ID="hidpic" runat="server" />
        <asp:HiddenField ID="hidpicdel" runat="server" />
        <div class="form-group">
            <label for="inputEmail3" class="col-xs-2 control-label"><span style="color: red">*</span>店铺LOGO：</label>
            <div class="col-xs-3">
                <div class="shop-logo" style="position: relative">
                    <div id="divFileProgressContainer1" style="float: left; border: 1px solid #a19c9c; padding: 2px">
                        <p id="imgall" style="height: 70px; width: 70px; overflow: hidden;"></p>
                    </div>
                    <span id="divFileProgressContainer" style="float: left"></span>
                    <span id="spanButtonPlaceholder">修改</span>
                    <div style="clear: both; color: #808080; font-size: 12px; padding-top: 10px">
                        建议尺寸：200 x 200 像素<br />
                        小于120KB，支持.jpg、.gif、.png格式
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <label for="inputEmail3" class="col-xs-2 control-label"><span style="color: red">*</span>店铺名称：</label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtSiteName" CssClass="form-control" runat="server" />
            </div>
        </div>
        <div class="form-group">
            <label for="inputPassword3" class="col-xs-2 control-label">联系电话：</label>
            <div class="col-xs-3">
                <asp:TextBox ID="txtShopTel" CssClass="form-control" runat="server" />
                <small class="help-block">支持输入手机号码或座机号码</small>
            </div>

        </div>



        <div class="form-group">
            <label for="inputPassword3" class="col-xs-2 control-label">店铺介绍：</label>
            <div class="col-xs-4">
                <asp:TextBox ID="txtShopIntroduction" CssClass="form-control" Width="300px" Height="100px" TextMode="MultiLine" runat="server" />
                <small class="help-block">微信分享店铺给好友时会显示这里的文案</small>
            </div>
        </div>
        <div class="form-group">
            <div class="col-xs-offset-2 col-xs-10">
                <asp:Button ID="btnSave" runat="server" OnClientClick="" Text="保存" CssClass="btn inputw100 btn-success" />
            </div>
        </div>









        <script type="text/javascript">
            var swfu;
            var swfum;
            $(function () {


                function loader(url) {
                    var settings = {
                        upload_url: "DistributorLogoUpload.aspx",
                        post_params: {
                            imgurl: url
                        },
                        use_query_string: true,
                        file_size_limit: "120 KB",
                        file_types: "*.jpg;*.gif;*.png;*.jpeg",
                        file_types_description: "JPG Images",
                        file_upload_limit: "0",

                        file_queue_error_handler: fileQueueErrorNew,
                        file_dialog_complete_handler: fileDialogComplete,
                        upload_progress_handler: uploadProgress,
                        upload_start_handler: uploadStartNew,
                        upload_error_handler: fileQueueErrorNew,
                        upload_success_handler: uploadSuss,
                        upload_complete_handler: uploadComplete,
                        //button_image_url: "/Admin/images/swfupload_uploadBtn.png",
                        button_text: "修改",
                        button_text_style: "color: #555555; font-size: 16pt;cursor:pointer",
                        button_placeholder_id: "spanButtonPlaceholder",
                        button_action: SWFUpload.BUTTON_ACTION.SELECT_FILE,
                        button_width: 40,
                        button_cursor: SWFUpload.CURSOR.HAND,
                        button_window_mode: SWFUpload.WINDOW_MODE.TRANSPARENT,
                        button_height: 20,
                        button_text_top_padding: 0,
                        button_text_left_padding: 0,
                        flash_url: "/Utility/swfupload/swfupload.swf",
                        custom_settings: {
                            upload_target: "divFileProgressContainer1"
                        },
                        debug: false
                    };

                    swfu = new SWFUpload(settings);

                    settings.button_placeholder_id = "divFileProgressContainer";
                    settings.button_text = "";
                    settings.button_height = 70;
                    settings.button_width = 70;
                    settings.post_params.imgurl = 0;

                    swfum = new SWFUpload(settings)

                };



                if ($("#ctl00_ContentPlaceHolder1_hidpic").val() != "") {
                    $("#imgall").append("<span id='span0' style='margin-right:10px'><img src='" + $("#ctl00_ContentPlaceHolder1_hidpic").val() + "' width='70' height='70'/></span>");
                    loader(1);
                } else {
                    loader(0);
                }


            });


            //上传错误检查
            function fileQueueErrorNew(file, errorCode, message) {
                try {
                    switch (errorCode) {
                        case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                            setTimeout(function () { HiTipsShow("文件为空！", 'error') }, 300);
                            break;
                        case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                            setTimeout(function () { HiTipsShow("最大上传文件120KB！", 'error') }, 300);
                            break;
                        case SWFUpload.UPLOAD_ERROR.SECURITY_ERROR:
                            alert("您的登入信息已失效！");
                            window.location = "/admin/Login.aspx";
                            break;
                        case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                            setTimeout(function () { HiTipsShow("空文件，上传失败", 'error') }, 300);
                            break;
                        case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE:
                            setTimeout(function () { HiTipsShow("非图片文件，请使用JPG，PNG，BMP，GIF格式文件", 'error') }, 300);
                            break;
                        default:
                            alert("上传异常：错误代码" + errorCode + ",提示信息->" + message);
                            break;
                    }

                } catch (ex) {
                    this.debug(ex);
                }

            }


            //开始上传触发
            function uploadStartNew(file) {
                try {

                    EditDelImg();
                    var progress = new FileProgress(file, this.customSettings.progressTarget);
                    progress.setStatus("Uploading...");
                    progress.toggleCancel(true, this);
                }
                catch (ex) {
                }

                return true;

            }

            //上传成功触发事件
            function uploadSuss(file, serverData) {
                try {


                    if (serverData != "0") {
                        var imgurl = $("#ctl00_ContentPlaceHolder1_hidpic").val();

                        $("#ctl00_ContentPlaceHolder1_hidpic").val(imgurl);
                        $("#imgall").append("<span id='span0' style='margin-right:10px'><img src='" + serverData.split("|")[1] + "' width='70' height='70'/></span>");
                        $("#ctl00_ContentPlaceHolder1_hidpic").val(serverData.split("|")[1]);
                        swfu.setPostParams({ "imgurl": 0 });

                        setTimeout(function () { HiTipsShow("修改成功！", "success") }, 300);
                    }
                    else {
                        alert("图片上传最多1张！");
                    }
                    var progress = new FileProgress(file, this.customSettings.upload_target);
                    progress.toggleCancel(false);


                } catch (ex) {
                    this.debug(ex);
                }
            }

            function EditDelImg() {
                //删除图片，暂时未更新
                if ($("#span0").length > 0) {
                    var obj = $("#span0 img").attr("src") + "|";
                    var alldelpic = $("#ctl00_ContentPlaceHolder1_hidpicdel").val();
                    alldelpic += obj;
                    $("#ctl00_ContentPlaceHolder1_hidpicdel").val(alldelpic);
                    $("#ctl00_ContentPlaceHolder1_hidpic").val(""); //清空
                    $("#span0").remove();
                    swfu.setPostParams({ "imgurl": 0 }); //动态参数

                };
            }


            function InitValidators() {
                $('#aspnetForm').formvalidation({
                    'ctl00$ContentPlaceHolder1$txtSiteName': {
                        validators: {
                            notEmpty: {
                                message: '请填写您的店铺名称，长度在10个字以内'
                            },
                            stringLength: {
                                min: 1,
                                max: 10,
                                message: '店铺名称的长度不能超过10个字'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtShopTel': {
                        validators: {
                            tell: {
                                message: '请填写正确的手机或座机号码'
                            }
                        }
                    },
                    'ctl00$ContentPlaceHolder1$txtShopIntroduction': {
                        validators: {
                            stringLength: {
                                min: 0,
                                max: 60,
                                message: '店铺介绍的长度不能超过60个字'
                            }
                        }
                    }
                });
            }

            $(document).ready(function () {
                InitValidators();

            });
        </script>
    </form>
</asp:Content>
