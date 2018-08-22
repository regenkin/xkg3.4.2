<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddShareAct.aspx.cs" MasterPageFile="~/Admin/AdminNew.Master"
    Inherits="Hidistro.UI.Web.Admin.promotion.AddShareAct" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<asp:content id="Content1" contentplaceholderid="head" runat="server">
    <style type="text/css">
        .graylabel{font-size:10px;color:#737373;}
        .share{width: 300px;height: 90px; background-color: #F7F7F7; margin-top: 15px;}
        .errorInput{border:1px,solid;border-color:#a94442;}
        .swfupload{float:left;}
        #SWFUpload_0 { margin-top:55px ;margin-left:5px;border:0px solid red;background:none}
        #SWFUpload_1{position:absolute;top:0px;left:0px}
    </style>
    <script src="../../Utility/swfupload/swfupload.js" type="text/javascript" charset="gbk"></script>
    <script src="../../Utility/swfupload/DisLogoupload.js" type="text/javascript" charset="gbk"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('input[type="text"]').change(function () {
                testInput(this);
            });

            $('#<%=cmb_CouponList.ClientID%>').change(function () {
                var couponid = $(this).val();
                if(couponid != "0")
                {
                    getCoupon(couponid);
                }
            });

            $('#<%=txt_title.ClientID%>').blur(function () {
                var title = $(this).val();
                if($.trim(title) != "")
                    $('#view_title').text(title);
            });

            $('#<%=txt_des.ClientID%>').blur(function () {
                var title = $(this).val();
                if ($.trim(title) != "")
                    $('#view_des').text(title);
            });

            uploadImg();

            if($('#<%=txt_title.ClientID%>').val() != "")
            {
                $('#view_title').text($('#<%=txt_title.ClientID%>').val());
            }

            if($('#<%=txt_des.ClientID%>').val() != "")
            {
                $('#view_des').text($('#<%=txt_des.ClientID%>').val());
            }

            if($('#<%=txt_img.ClientID%>').val() != "")
            {
                $('#view_img').attr("src", $('#<%=txt_img.ClientID%>').val());
            }
        });

        function getCoupon(couponid) {
            $.ajax({
                type: "post",
                url: "GetCouponDataHandler.ashx?id=" + couponid,
                data: {},
                dataType: "json",
                success: function (data) {
                    if (data.type == "success") {
                        $('#period').show();
                        $('#s_begin').text(data.data.BeginDate);
                        $('#s_end').text(data.data.EndDate);
                    }
                    else
                    {
                        $('#period').hide();
                    }
                }
            });
        }

        function testRegex(rgx, str, bflag) {
            if (str == "") {
                if (bflag)
                { return true; }
                else { return false; }
            }
            return result = rgx.test(str);
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

            var flag = false;
            var bError = false;
            if (id == $('#<%=txt_MeetValue.ClientID%>').attr('id')) {
                if ($('#' + id).val() == "") {
                    bError = true;
                }
                regex = /^\d+(\.\d{2})?$/;
                flag = true;
            }
            if (id == $('#<%=txt_Number.ClientID%>').attr('id')) {
                if ($('#' + id).val() == "") {
                    bError = true;
                }
                regex = /^[0-9]*$/;
                flag = true;
            }

            if (id == $('#<%=calendarStartDate.ClientID%>_txtDateTimePicker').attr('id')) {
                if ($('#' + id).val() == "") {
                     bError = true;
                 }
                 //if (!isDate($('#' + id).val())) {
                 //    bError = true;
                 //}
             }

             if (id == $('#<%=calendarEndDate.ClientID%>_txtDateTimePicker').attr('id')) {
                 if ($('#' + id).val() == "") {
                     bError = true;
                 }
                 //if (!isDate($('#' + id).val())) {
                 //    bError = true;
                 //}
             }           
            
             if (flag) {
                 if (testRegex(regex, content, false)) {
                     $(obj).removeClass();
                     $(obj).addClass("form-control");
                 }
                 else {
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
                $('#ctl00_ContentPlaceHolder1_saveBtn').prop('disabled', true);
            }
            else {
                $('#ctl00_ContentPlaceHolder1_saveBtn').prop('disabled', false);
            }
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

                    setTimeout(function () {
                        HiTipsShow("图片修改成功！", "success");
                        $('#view_img').attr("src", serverData.split("|")[1]);
                    }, 300);
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
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div class="page-header">
            <h2>添加分享助力活动</h2>
        </div>
        <div>
            <table style="width:100%;">
            <tr>
                <td style="width:360px;">
                    <div class="edit-text-left">
                        <div class="edit-text-left">
                            <div class="mobile-border">
                                <div class="mobile-d">
                                    <div class="mobile-header">
                                        <i></i>
                                        <div class="mobile-title">分享助力</div>
                                    </div>
                                    <div class="set-overflow" style="height:450px;">
                                        <div class="white-box">                                            
                                            <div class="share autol">
                                                <div id="view_title">
                                                    活动标题
                                                </div>
                                                <div class="fl">
                                                    <img src="http://fpoimg.com/60x60" style="width:60px; height:60px;" id="view_img"/>
                                                </div>
                                                <div class="fl" style="margin-left:10px;">
                                                    <p id="view_des">活动介绍</p>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="mobile-nav">
                                        <ul class="clearfix">
                                            <li>
                                                <span class="glyphicon glyphicon-home"></span>
                                                <p>店铺主页</p>
                                            </li>
                                            <li>
                                                <span class="glyphicon glyphicon-user"></span>
                                                <p>会员主页</p>
                                            </li>
                                            <li>
                                                <span class="glyphicon glyphicon-list"></span>
                                                <p>所有商品</p>
                                            </li>
                                            <li>
                                                <span class="glyphicon glyphicon-duplicate"></span>
                                                <p>申请分销</p>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="clear-line">
                                    <div class="mobile-footer"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </td>
                <td style="width:auto; vertical-align:top;">
                    <div class="set-switch" style="width: 95%;">
                        <label class="smallTitleCss">基本信息：</label>
                        <div class="graylineCss"></div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>活动名称：</label>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" class="form-control" ID="txt_name" Width="200px"></asp:TextBox>                          
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>选择优惠券：</label>
                            <div class="col-xs-4">
                                <asp:DropDownList runat="server" ID="cmb_CouponList"  CssClass="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group" id="period" style="display:none;">
                            <label class="col-xs-3 control-label"></label>
                            <div class="col-xs-9">
                                <label>优惠券有效期：<span id="s_begin"></span> ~ <span id="s_end"></span></label>
                            </div>
                        </div>
                    </div>
                    <div class="set-switch" style="width: 95%;">
                        <label class="smallTitleCss">活动信息：</label>
                        <div class="graylineCss"></div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>活动时间：</label>
                            <div class="form-inline">
                                <Hi:DateTimePicker runat="server" Style="margin-left: 15px;" CssClass="form-control"
                                    ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss" />
                                <label>至</label>
                                <Hi:DateTimePicker runat="server" CssClass="form-control" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" IsEnd="true" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>订单金额满：</label>
                            <div class="form-inline">
                                <asp:TextBox runat="server" ID="txt_MeetValue" CssClass="form-control" style="margin-left:15px;width:100px;"></asp:TextBox>
                                <label>元，可分享</label>
                                <asp:TextBox runat="server" ID="txt_Number" CssClass="form-control" Width="100" ></asp:TextBox>
                                <label>张优惠券</label>
                            </div>
                        </div>

                        <asp:HiddenField ID="hidpic" runat="server" />
                        <asp:HiddenField ID="hidpicdel" runat="server" />
                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>朋友圈显示图片：</label>
                            <div class="col-xs-9">
                                <div class="shop-logo" style="position: relative">
                                    <div id="divFileProgressContainer1" style="float: left; border: 1px solid #a19c9c;padding: 2px">
                                        <p id="imgall" style="height: 70px; width: 70px; overflow: hidden;"></p>
                                    </div>
                                    <span id="divFileProgressContainer" style="float: left"></span>
                                    <span id="spanButtonPlaceholder">修改</span>
                                    <div style="clear: both; color: #808080; font-size: 12px; padding-top: 10px">
                                        建议尺寸：200 x 200 像素，小于300KB，支持jpg、gif、png格式
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="display:none">
                            <asp:TextBox runat="server" ID="txt_img" ></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>朋友圈分享标题：</label>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" ID="txt_title" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"><em>*</em>活动介绍：</label>
                            <div class="col-xs-4">
                                <asp:TextBox runat="server" ID="txt_des" CssClass="form-control" TextMode="MultiLine" Width="250" Height="100"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label class="col-xs-3 control-label"></label>
                            <div class="col-xs-9">            
                                <label>微信昵称标签  <span style="color:#CC0000;">{{微信昵称}}</span></label><br />
                                <label>店铺名称标签  <span style="color:#CC0000;">{{店铺名称}}</span></label>
                                <label class="graylabel">在标题和活动介绍中插入微信昵称或店铺名标签，会员在分享到朋友圈</label>                                
                                <label class="graylabel">的时候可以在相应位置显示其微信昵称或店铺名。</label>
                            </div>
                        </div>                        
                    </div>
                </td>
            </tr>
        </table>
        <div style="height:50px;"></div>

        <div class="footer-btn navbar-fixed-bottom autow">
            <asp:Button runat="server" ID="saveBtn" class="btn btn-success bigsize" Style="margin-left: 15px;" Text="保存" />
            <asp:HiddenField ID="shareActId" runat="server" Value="0" />
        </div>

            <%--<div class="form-group">
                <div class="col-xs-offset-3 marginl">
                    
                </div>
            </div>--%>
        </div>
        
    </form>
</asp:content>


