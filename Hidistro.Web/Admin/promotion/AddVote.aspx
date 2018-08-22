<%@ Page Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true"
    CodeBehind="AddVote.aspx.cs" Inherits="Hidistro.UI.Web.Admin.promotion.AddVote" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/SetMemberRange.ascx" TagName="SetMemberRange" TagPrefix="Hi" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
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
            var chk="<%=vote.IsMultiCheck%>";
            if(chk == "True")
            {
                $('#rd_multi').attr("checked", "checked");
                $('#txt_maxCheck').val(<%=vote.MaxCheck%>);
            }
            else
            {
                $('#rd_single').attr("checked","checked");
            }

            var multi = $("input[name='maxcheck']:checked").val();
            if (multi == "0")
            {
                $('#txt_maxCheck').attr("disabled", "disabled");
            }

            $("input[name='maxcheck']").change(function () {
                if($(this).val() == "0")
                {
                    $('#txt_maxCheck').attr("disabled", "disabled");
                }
                else
                {
                    $('#txt_maxCheck').removeAttr("disabled");
                }
            });
            
            //readMemberGrades();

            $('#saveBtn').click(function () {
                saveData();
            });
            $('#CancelBtn').click(function () {
                cancel();
            });
            

            $('input[type="text"]').change(function () {
                testInput(this);
            });
            
            uploadImg();

            if ('<%=id%>' != '0') {
                //setVote();
            }
           
        });
        
        function IntToChar(n) {
            return String.fromCharCode(n);
        }
        function CharToInt(c) {
            return c.charCodeAt();
        }

        function isDate(dateString) {
            if (dateString.trim() == "") return false;
            var r = dateString.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
            if (r == null) {
                return false;
            }
            var d = new Date(r[1], r[3] - 1, r[4]);
            var num = (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
            if (num == 0) {
                return false;
            }
            return (num != 0);
        }

        function testInput(obj) {
            var id = $(obj).attr("id");
            var content = $(obj).val();
            var regex;
            var parent;
            var btn;
            var minLen = 0;
            var maxlen = 0;

            var flag = false;
            var bError = false;
            if (!$(obj).prop('display')) {
                return false;
            }


            if (id == $('#txt_name').attr('id')) {
                if ($(obj).val() == "") {
                    bError = true;
                }
                minLen = 0;
                maxlen = 60;
            }

            if (id == $('#txt_maxCheck').attr('id')) {
                return false;
            }

            if (id == $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').attr('id')) {
                 if ($('#' + id).val() == "") {
                     bError = true;
                 }
                 if (!isDate($('#' + id).val())) {
                     bError = true;
                 }
            }
         

             if (id == $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').attr('id')) {
                 if ($('#' + id).val() == "") {
                     bError = true;
                 }
                 if (!isDate($('#' + id).val())) {
                     bError = true;
                 }
             }
            

            var len = $(obj).val().length;
           

            if (len <= minLen)
            {
                bError = true;
            }
            if (maxlen > 0)
            {
                if (len > maxlen) {
                    bError = true;
                }
            }


             if (bError) {
                 $(obj).removeClass();
                 $(obj).addClass("form-control errorInput");
             }
             else {
                 $(obj).removeClass();
                 $(obj).addClass("form-control");
             }         
             setBtnEnable();

             return !bError;
         }

        function setBtnEnable() {
            var error = $(".errorInput");
            if (error.length > 0) {
                $('#saveBtn').removeAttr('disabled');
                $('#saveBtn').prop('disabled', 'disabled');
            }
            else {
                $('#saveBtn').removeAttr('disabled');
            }
        }


        function beforeSave() {
            if ($('#txt_name').val() == "") {
                ShowMsg('请输入投票标题！', false);
                $('#txt_name').focus();
                return false;
            }
            if ($('#txt_name').val().length > 60) {
                ShowMsg('投票标题的长度不能超过60个字符！', false);
                $('#txt_name').focus();
                return false;
            }

            var val = $('#txt_items').val();
            var ss = val.split('\n');
            var items = [];
            for (var i = 0; i < ss.length; i++) {
                if ($.trim(ss[i]) != "") {
                    items.push(ss[i]);
                }
            }
            if (items.length == 0) {
                ShowMsg('请输入投票选项！', false);
                $('#txt_items').focus();
                return false;
            }

            if ($('#imgUrl').val() == "") {
                ShowMsg('请上传活动封面！', false);
                return false;
            }

            var startTime = $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val();
            var endTime = $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val();
            if (startTime == "") {
                ShowMsg('请输入开始时间！', false);
                return false;
            }

            if (endTime == "") {
                ShowMsg('请输入结束时间！', false);
                return false;
            }

            var sDate = new Date(startTime.replace("-", "/"));
            var eDate = new Date(endTime.replace("-", "/"));
            if (sDate >= eDate) {
                ShowMsg('有效期限结束时间要大于开始时间！', false);
                return false;
            }
            if ($('#memberdiv input:checked').length == 0) {
                ShowMsg("请选择适用会员！")
                return false;
            }

            if (um.getContent() == "") {
                ShowMsg('请输入活动说明！', false);
                return false;
            }

            var multi = $("input[name='maxcheck']:checked").val();
            if (multi == "1") {
                var maxCount = $('#txt_maxCheck').val();
                if (maxCount == "") {
                    ShowMsg('请输入最多可选项数量！', false);
                    return false;
                }
                if (parseInt(maxCount) < 2) {
                    ShowMsg('最多可选项数量要大于1！', false);
                    return false;
                }
            }

            return true;
        }


        function saveData() {
            if (!beforeSave()) return;
            var name = $('#txt_name').val();
            var begin = $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').val();
            var end = $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').val();
            var img = $('#imgUrl').val();
            var des = um.getContent();
            var memberlvl = $('#ctl00_ContentPlaceHolder1_SetMemberRange_txt_Grades').val();
            var defualtgroup = $('#ctl00_ContentPlaceHolder1_SetMemberRange_txt_DefualtGroup').val();
            var customgroup = $('#ctl00_ContentPlaceHolder1_SetMemberRange_txt_CustomGroup').val();
            var isAllmember = false;
            var type;

            var val = $('#txt_items').val();
            var ss = val.split('\n');
            var items = [];
            for (var i = 0; i < ss.length; i++) {
                if ($.trim(ss[i]) != "") {
                    items.push(ss[i]);
                }
            }

            var maxcheck = 1;
            var ismulti = false;
            var multi = $("input[name='maxcheck']:checked").val();
            if (multi == "1") {
                ismulti = true;
                maxcheck = $('#txt_maxCheck').val();
            }

            var vote = {
                id: '<%=id%>',
                name: name,
                begin: begin,
                end: end,
                des: des,
                memberlvl: memberlvl,
                defualtgroup: defualtgroup,
                customgroup: customgroup,
                img: img,
                ismulti: ismulti,
                maxcheck: maxcheck,
                items: items.join(',')
            };

            $.ajax({
                type: "post",
                url: "SaveVoteHandler.ashx",
                data: vote,
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        ShowMsg("创建投票调查成功！", true);
                        document.location.href = "VoteList.aspx";
                    }
                    else {
                        ShowMsg("创建投票调查失败！（" + data.data + ")", false);
                    }
                }
            });
        }

        function cancel() {
            document.location.href = "VoteList.aspx";
        }

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
                    $('#imgUrl').val(serverData.split("|")[1]);
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
            var oldurl = $('#imgUrl').val();
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
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>新建投票调查</h2>
        </div>
        <div>
            <asp:HiddenField ID="hidpic" runat="server" />
            <asp:HiddenField ID="hidpicdel" runat="server" />
            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>投票标题：</label>
                <div class="col-xs-4">                    
                    <input type="text" class="form-control" ID="txt_name" style="width:320px;" value="<%=vote.VoteName%>"/>
                    <%--<small>长度限制在60个字符以内</small>--%>
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>投票选项：</label>
                <div class="col-xs-4">
                    <textarea class="form-control" id="txt_items" cols="24" rows="6"><%=items %></textarea>
                    <small>在输入框中用回车换行区分多个选项值，一行一个选项</small>
                </div>
            </div>
            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>活动封面：</label>
                <div class="col-xs-4">
                    <div class="shop-logo" style="position: relative">
                        <div id="divFileProgressContainer1" style="float: left; border: 1px solid #a19c9c;
                            padding: 2px">
                            <p id="imgall" style="height: 70px; width: 70px; overflow: hidden;"></p>
                        </div>
                        <span id="divFileProgressContainer" style="float: left"></span>
                        <span id="spanButtonPlaceholder">修改</span>
                        <div style="clear: both; color: #808080; font-size: 12px; padding-top: 10px">
                            建议尺寸：640 x 400 像素，小于300KB，支持jpg、gif、png格式
                        </div>
                    </div>
                    <input type="hidden" id="imgUrl" value="<%=vote.ImageUrl%>" />
                 </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>有效期限：</label>
                <div class="form-inline">
                    <Hi:DateTimePicker runat="server" Style="margin-left: 15px;" CssClass="form-control" ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss" />
                    <label>至</label>
                    <Hi:DateTimePicker runat="server" CssClass="form-control" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" IsEnd="true" />
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>适用会员：</label>
                <div class="col-xs-4">
                    <Hi:SetMemberRange runat="server" ID="SetMemberRange" />
                </div>
            </div>

            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>活动说明：</label>
                <div class="col-xs-4">
                    <kindeditor:kindeditorcontrol id="fkContent" runat="server" width="670" height="200" />                   
                </div>
            </div>          

            <div class="form-group">
                <label class="col-xs-2 control-label"><em>*</em>参与方式：</label>
                <div class="form-inline">
                    <input type="radio" name="maxcheck" id="rd_single" style="margin-left: 15px;" value="0" />投票只能选择一项（单选）                   
                </div>
                <div class="form-inline">
                    <input type="radio" name="maxcheck" id="rd_multi" style="margin-left: 15px;" value="1" />投票允许多选，最多可选 
                    <input type="text" class="form-control" id="txt_maxCheck" style="width: 50px;" onKeyUp="this.value=this.value.replace(/\D/g,'')" value="" /> 项
                </div>
            </div>            

            <div class="form-group" style="text-align: center; width: 60%; margin-top:5px;">
                <div class="col-xs-offset-2 marginl">
                    <input id="saveBtn"  class="btn btn-success inputw100" value="保存" />
                    <input id="CancelBtn" class="btn btn-success inputw100" value="取消" />
                </div>
            </div>

        </div>
    </form>
</asp:content>