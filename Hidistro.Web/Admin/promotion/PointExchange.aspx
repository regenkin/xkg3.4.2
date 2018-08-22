<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PointExchange.aspx.cs"
    MasterPageFile="~/Admin/AdminNew.Master" Inherits="Hidistro.UI.Web.Admin.promotion.PointExchange" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagName="SetMemberRange" TagPrefix="Hi" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../../Utility/swfupload/swfupload.js" type="text/javascript" charset="gbk"></script>
    <script src="../../Utility/swfupload/DisLogoupload.js" type="text/javascript" charset="gbk"></script>
    <Hi:Script ID="Script4" runat="server" Src="/admin/js/jquery.formvalidation.js" />
    <style type="text/css">
       .lbCss{font-weight:bold; font-size:12px;}
       .errorInput{border:1px,solid;border-color:#a94442;}
       span,small {font-size:12px}
       .swfupload{float:left;}
       #SWFUpload_0 { margin-top:55px ;margin-left:5px;border:0px solid red;background:none}
       #SWFUpload_1{position:absolute;top:0px;left:0px}
       .errorFocus {height: 34px;padding-left:15px}
       .msgError{color:red;background:url(../images/false.gif) left center no-repeat;border:none;padding-left:24px}
       .gray{color:#a19c9c;padding-left:10px;display:none;}
       .wtx{clear:both;margin-left:190px;font-size:12px;line-height:20px;color:#a19c9c;}
       .dialog-content td{vertical-align:middle}
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            InitValidators();
            setBtnDisplay();
            uploadImg();
            //$("#allMember").on("change", function () {
            //    var allRadio = $(this).parents('.resetradio').next().find('input');
            //    if (this.checked) {
            //        allRadio.prop('checked', true).attr('disabled', true);
            //    } else {
            //        allRadio.prop('checked', false).attr('disabled', false);
            //    }
            //});
            //$('.allradio input').change(function () {
            //    if ($('.allradio input:checked').length == 3) {
            //        $("#allMember").prop('checked', true);
            //    } else {
            //        $("#allMember").prop('checked', false);
            //    }
            //})
        });

        function setBtnDisplay() {
            if ('<%=eId%>' == '0') {
                $('#backBtn').css('display', 'none');
            }
            else {
                $('#backBtn').css('display', '');
            }

            if ('<%=bFinished%>' == "False") {
                $('#<%=saveBtn.ClientID%>').css('display', '');
            }
            else {
                $('#<%=saveBtn.ClientID%>').css('display', 'none');
            }

        }
        
        function beforeSubmit() {
            var name=$('#<%=txt_name.ClientID%>').val();
            if (name == "" || name.length > 30) {
                //ShowMsg("请输入活动名称，长度不能超过30个字符！",false);
                $('#<%=txt_name.ClientID%>').focus();
                return false;
            }
            var imgUrl = $('#ctl00_ContentPlaceHolder1_hidpic').val();
            if (imgUrl == "") {
                ShowMsg("请选择活动封面！", false);
                return false;
            }
            if ($('#memberdiv input:checked').length == 0) {
                ShowMsg("请选择适用会员！")
                return false;
            }
            var startDate = $('#ctl00_ContentPlaceHolder1_calendarStartDate_txtDateTimePicker').val();
            if (startDate == "") {
                ShowMsg("请输入活动开始时间！", false);
                return false;
            }

            var endDate = $('#ctl00_ContentPlaceHolder1_calendarEndDate_txtDateTimePicker').val();
            if ( endDate == "") {
                ShowMsg("请输入活动结束时间！", false);
                return false;
            }

            return true;
        }

        function InitValidators() {
            $('#aspnetForm').formvalidation({
                'ctl00$ContentPlaceHolder1$txt_name': {
                    validators: {
                        notEmpty: {
                            message: '请输入活动名称！'
                        },
                        stringLength: {
                            min: 1,
                            max: 30,
                            message: '活动名称的长度不能超过30个字符'
                        }
                    }
                }
            });
        }

        function fileQueueErrorNew(file, errorCode, message) {
            try {
                switch (errorCode) {
                    case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE:
                        setTimeout(function () { HiTipsShow("文件为空！", 'error') }, 300);
                        break;
                    case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT:
                        setTimeout(function () { HiTipsShow("最大上传文件300KB！", 'error') }, 300);
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
                    $('#<%=txt_img.ClientID%>').val(serverData.split("|")[1]);
                    swfu.setPostParams({ "imgurl": 0 });

                    setTimeout(function () { HiTipsShow("活动封面修改成功！", "success") }, 300);
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
            var oldurl = $('<%=txt_img.ClientID%>').val();
            if ($("#span0").length > 0) {
                var obj = $("#span0 img").attr("src") + "|";
                var alldelpic = $("#ctl00_ContentPlaceHolder1_hidpicdel").val();
                alldelpic += obj;
                $("#ctl00_ContentPlaceHolder1_hidpicdel").val(alldelpic);
                $("#ctl00_ContentPlaceHolder1_hidpic").val(""); //清空
                $("#span0").remove();
                swfu.setPostParams({ "imgurl": 0, "oldurl": oldurl }); //动态参数
            };
        }

        function uploadImg() {

            function loader(url) {

                var oldurl = "";
                var settings = {
                    upload_url: "ImgUpload.aspx",
                    post_params: {
                        imgurl: url,
                        oldurl: oldurl
                    },
                    use_query_string: true,
                    file_size_limit: "300 KB",
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
        }
    </script>



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2><%=Globals.RequestQueryNum("id")>0?"编辑":"添加" %>积分兑换活动</h2>            
        </div>
        <asp:HiddenField ID="hidpic" runat="server" />
        <asp:HiddenField ID="hidpicdel" runat="server" />
        <div class="form-group">
            <label class="col-xs-3 control-label"><em>*</em>活动名称：</label>
            <div class="col-xs-4">
                <asp:TextBox runat="server" class="form-control" ID="txt_name" Width="200px"></asp:TextBox>
            </div>
        </div>

        <div class="form-group">
            <label class="col-xs-3 control-label"><em>*</em>活动封面：</label>
            <div class="col-xs-5">
                <div class="shop-logo" style="position: relative">
                    <div id="divFileProgressContainer1" style="float: left; border: 1px solid #a19c9c;
                        padding: 2px">
                        <p id="imgall" style="height: 70px; width: 70px; overflow: hidden;"></p>
                    </div>
                    <span id="divFileProgressContainer" style="float: left"></span>
                    <span id="spanButtonPlaceholder">修改</span>
                    <div style="clear: both; color: #808080; font-size: 12px; padding-top: 10px">
                        建议尺寸：640 x 220 像素，小于300KB，支持jpg、gif、png格式
                    </div>
                </div>
            </div>
        </div>

        <div style="display:none">
            <asp:TextBox runat="server" ID="txt_img" ></asp:TextBox>
        </div>


        <div class="form-group">
            <label class="col-xs-3 control-label"><em>*</em>适用会员：</label>
            <div class="col-xs-7">
                <Hi:SetMemberRange runat="server" ID="SetMemberRange" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-xs-3 control-label"><em>*</em>有效期限：</label>
            <div class="form-inline">
                <Hi:DateTimePicker runat="server" Style="margin-left: 15px;" CssClass="form-control" ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss"  />
                <label>至</label>
                <Hi:DateTimePicker runat="server" CssClass="form-control" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss"
                    IsEnd="true" />
            </div>
        </div>

        <div class="form-group">
            <label class="col-xs-3 control-label"></label>
            <div class="form-inline">
                <asp:Button runat="server" ID="saveBtn" class="btn btn-success bigsize" Style="margin-left: 15px;"
                    Text="下一步，选择宝贝" OnClientClick="return beforeSubmit();"/>
                <input type="button" style="display: none" class="btn btn-success bigsize" id="backBtn" onclick="window.location.href='ExChangeList.aspx'"
                    value="返回" />

            </div>
        </div>
    </form>
</asp:Content>
