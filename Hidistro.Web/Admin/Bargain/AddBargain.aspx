<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddBargain.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Bargain.AddBargain" MasterPageFile="~/Admin/AdminNew.Master" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register Src="~/Admin/Ascx/ucDateTimePicker.ascx" TagName="DateTimePicker" TagPrefix="Hi" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #SWFUpload_1 {
            position: absolute;
            left: 0;
            top: 10px;
        }

        #SWFUpload_0 {
            position: absolute;
            left: 80px;
            top: 70px;
        }
        /*small{
            display:inline-block !important;
        }*/
    </style>
    <script type="text/javascript">
        function ShowProduct() {
            $DialogFrame_ReturnValue = "";// 返回值，IsMultil=1可多选
            DialogFrame("/Admin/Oneyuan/ProductSelect.aspx?IsMultil=0", "选择商品", 680, 500, function (rs) {
                if (rs != "") {
                    var rsArray = rs.split("^");
                    if (rsArray.length == 7) {
                        //获取到正确的数值44^男鞋^8.00^8.00^/Storage/master/product/thumbs60/60_b1d772be0a2b4f469cca08044c1c4c48.jpg^3000^0
                        var imageUrl = rsArray[4];
                        if (rsArray[4] == "")
                            imageUrl = "/utility/pics/none.gif";

                        var phtml = '  <div class="shop-img fl">' +
                        '  <img src="' + imageUrl + '" width="60" style="height:60px!important" /></div>' +
                        '  <div class="shop-username fl ml10">' +
                        '   <p style="color:#222">' + rsArray[1] + '</p></div>' +
                        '  <p class="fl ml20">现价：￥' + rsArray[2] + '</p>' +
                        '  <p class="fl ml20">库存：' + rsArray[5] + '</p>';
                        $("#productInfo").html(phtml);
                        ///赋值
                        $("#ctl00_ContentPlaceHolder1_hiddProductId").val(rsArray[0]);
                        $("#ctl00_ContentPlaceHolder1_productImage").attr("src", imageUrl)
                        $("#ctl00_ContentPlaceHolder1_lbProductName").html(rsArray[1]);
                    }
                }
            });
        }



        $(function () {
            //同步显示活动标题
            $("#ctl00_ContentPlaceHolder1_txtTitle").keyup(function () {
                var title = $(this).val();
                $(".f-title").html(title);
            });

            //数据验证
            $(".container").formvalidation({
                'submit': '#ctl00_ContentPlaceHolder1_btnSave',
                'ctl00$ContentPlaceHolder1$txtTitle': {
                    validators: {
                        notEmpty: {
                            message: '分享标题不能为空'
                        },
                        stringLength: {
                            min: 1,
                            max: 20,
                            message: '分享标题1-20个字符'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtActivityStock': {
                    validators: {
                        notEmpty: {
                            message: '活动库存不能为空'
                        },
                        regexp: {
                            regexp: /^\d{1,4}$/,
                            message: '请填写正确的活动库存！'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtPurchaseNumber': {
                    validators: {
                        notEmpty: {
                            message: '限购数量不能为空'
                        },
                        regexp: {
                            regexp: /^\d{1,4}$/,
                            message: '请填写正确的限购数量！'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtInitialPrice': {
                    validators: {
                        notEmpty: {
                            message: '初始价格不能为空'
                        },
                        regexp: {
                            regexp: /^[0-9]+(\.[0-9]+)?$/,
                            message: '初始价格只能输入整数型数值'
                        }
                    }
                },
                'ctl00$ContentPlaceHolder1$txtFloorPrice': {
                    validators: {
                        notEmpty: {
                            message: '活动底价不能为空'
                        },
                        regexp: {
                            regexp: /^[0-9]+(\.[0-9]+)?$/,
                            message: '活动底价只能输入整数型数值'
                        }
                    }
                }
                //时间验证
                //'ctl00$ContentPlaceHolder1$bankPayDate': {
                //validators: {
                //        notEmpty: {
                //            message: '支付日期不能为空'
                //        },
                //    regexp: {
                //        regexp: /^(\d{4})\-(\d{2})\-(\d{2}) (\d{2}):(\d{2})$/,
                //        message: '请填写正确的日期格式'
                //    }
                //}
                //}
            });
            //上传图片
            uploadImg();
        })
    </script>
    <script type="text/javascript">
        ///上传图片
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
                        HiTipsShow("上传图片成功！", "success");
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
                    upload_url: "/Admin/promotion/ImgUpload.aspx",
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
                    button_text: "上传图片",
                    button_text_style: "color: #555555; font-size: 16pt;cursor:pointer",
                    button_placeholder_id: "spanButtonPlaceholder",
                    button_action: SWFUpload.BUTTON_ACTION.SELECT_FILE,
                    button_width: 60,
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
    <script src="../../Utility/swfupload/swfupload.js" type="text/javascript" charset="gbk"></script>
    <script src="../../Utility/swfupload/DisLogoupload.js" type="text/javascript" charset="gbk"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="thisForm" runat="server" class="form-horizontal">
        <div>
            <div class="page-header">
                <h2>好友砍价<small style="display: inline; margin-left: 20px; color: red">好友砍价商品不同时享受满减活动,不能使用优惠劵,不能使用积分抵扣订单金额 </small></h2>
            </div>
            <div class="blank">
                <a href="ManagerBargain.aspx?Type=0" class="btn btn-primary btn-sm inputw100">&lt;&lt; 返回</a>
            </div>
            <div class="shop-navigation pb100 clearfix">
                <div class="fl">
                    <div class="mobile-border">
                        <div class="mobile-d">
                            <div class="mobile-header">
                                <i></i>
                                <div class="mobile-title">好友砍价</div>
                            </div>
                            <div class="set-overflow">
                                <div style="min-height: 350px;">
                                    <div class="y3-shared-title">
                                        <p class="f-title">
                                            <asp:Label runat="server" ID="lbtitle" Text="分享标题"></asp:Label>
                                        </p>
                                        <div class="y3-shopimgname">
                                            <asp:Image ImageUrl="/utility/pics/none.gif" runat="server" ID="productImage" />
                                            <asp:Label ID="lbProductName" runat="server" Text="商品名称"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="clear-line">
                            <div class="mobile-footer"></div>
                        </div>
                    </div>
                </div>
                <div class="fl frwidth">
                    <div class="set-switch resetBorder">
                        <p class="mb10 borderSolidB pb5"><strong>活动设置：</strong></p>
                        <div class="form-horizontal clearfix">
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;分享标题：</label>
                                <div class="form-inline col-xs-9">
                                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="20" CssClass="form-control resetSize inputw300" placeholder="该活动被分享的时候显示的默认标题"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动时间：</label>
                                <div class="form-inline journal-query col-xs-9">
                                    <div class="form-group">
                                        <Hi:DateTimePicker runat="server" CssClass="form-control resetSize" ID="calendarStartDate" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="开始时间" />
                                        &nbsp;&nbsp;至&nbsp;
                                        <Hi:DateTimePicker runat="server" CssClass="form-control resetSize" ID="calendarEndDate" DateFormat="yyyy-MM-dd HH:mm:ss" PlaceHolder="结束时间" IsEnd="true" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate"><em>*</em>&nbsp;&nbsp;活动封面：</label>
                                <asp:HiddenField ID="hidpic" runat="server" />
                                <asp:HiddenField ID="hidpicdel" runat="server" />
                                <div class="col-xs-9">
                                    <div class="pt10 clearfix" style="position: relative">
                                        <div id="divFileProgressContainer1" style="float: left; border: 1px solid #a19c9c; padding: 2px">
                                            <p id="imgall" style="height: 70px; width: 70px; overflow: hidden;"></p>
                                        </div>
                                        <span id="divFileProgressContainer" style="float: left"></span>
                                        <span id="spanButtonPlaceholder" style="">上传图片</span>
                                        <div style="clear: both; color: #808080; font-size: 12px; padding-top: 10px">
                                            建议尺寸：600 x 200 像素，小于300KB，支持jpg、gif、png格式
                                        </div>
                                    </div>
                                </div>
                                <div style="display: none">
                                    <asp:TextBox runat="server" ID="txt_img"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-xs-3 pad resetSize control-label" for="setdate">活动说明：</label>
                                <div class="form-inline journal-query col-xs-9">
                                    <div class="form-group">
                                        <asp:TextBox runat="server" ID="txtRemarks" MaxLength="200" CssClass="form-control inputtext" TextMode="MultiLine"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="set-switch resetBorder">
                        <p class="mb10 borderSolidB pb5"><strong>商品设置：</strong></p>
                        <div class="form-horizontal clearfix">
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;选择商品：</label>
                                <div class="form-inline col-xs-9 pt3">
                                    <a href="javascript:ShowProduct()">点击选择</a>&nbsp;&nbsp;&nbsp;&nbsp;
                                      &nbsp; &nbsp; 选择的多规格商品每个规格的价格必须相同 
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername">&nbsp;&nbsp;</label>
                                <div class="form-inline col-xs-9">
                                    <asp:HiddenField runat="server" ID="hiddProductId" />
                                    <div class="y3-prize-info clearfix" id="productInfo">
                                        <%= productInfoHtml %>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;活动库存：</label>
                                <div class="form-inline col-xs-9">
                                    <asp:TextBox ID="txtActivityStock" runat="server" MaxLength="4" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    &nbsp; &nbsp;<p class="colorc">该商品参加砍价活动最大可销售数量</p> 
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;限购数量：</label>
                                <div class="form-inline col-xs-9">
                                    <asp:TextBox ID="txtPurchaseNumber" runat="server" MaxLength="4" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    &nbsp; &nbsp;<p class="colorc"> 同一个会员最多购买数量 </p> 
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="txtInitialPrice"><em>*</em>&nbsp;&nbsp;初始价格：</label>
                                <div class="form-inline col-xs-9">
                                    <asp:TextBox ID="txtInitialPrice" runat="server" MaxLength="10" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    &nbsp;元
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;活动底价：</label>
                                <div class="form-inline col-xs-9">
                                    <asp:TextBox ID="txtFloorPrice" runat="server" MaxLength="10" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                    &nbsp;元 &nbsp; &nbsp;<p class="colorc"> 最终成交价格不会低于底价 </p> 
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;是否分佣：</label>
                                <div class="form-inline col-xs-9">
                                    <asp:CheckBox runat="server" ID="ckIsCommission" Text="分佣" Checked="true" />
                                </div>
                            </div>
                            <div class="form-group setmargin">
                                <label class="col-xs-3 pad resetSize control-label" for="pausername"><em>*</em>&nbsp;&nbsp;砍价方式：</label>
                                <div class="form-inline col-xs-9">
                                    <div class="mb10">
                                        <label class="mr10 middle fl pt3">
                                            <asp:RadioButton ID="rbtBargainTypeOne" GroupName="BargainType" Checked="true" runat="server" Text="每次砍掉" /></label>
                                        <asp:TextBox ID="txtBargainTypeOneValue" MaxLength="8" runat="server" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                        &nbsp;元
                                    </div>
                                    <div>
                                        <label class="mr10 middle fl pt3">
                                            <asp:RadioButton ID="rbtBargainTypeTwo" GroupName="BargainType" runat="server" Text="随机砍掉" />
                                        </label>
                                        <asp:TextBox ID="txtBargainTypeTwoValue1" MaxLength="8" runat="server" placeholder="最小值" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                        &nbsp;~&nbsp;
                                        <asp:TextBox ID="txtBargainTypeTwoValue2" MaxLength="8" runat="server" placeholder="最大值" CssClass="form-control resetSize inputw100"></asp:TextBox>
                                        &nbsp;元
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="footer-btn navbar-fixed-bottom">
            <asp:Button runat="server" ID="btnSave" CssClass="btn btn-success" Text="保存" />
            <%--  <asp:Button runat="server" ID="Button1"  CssClass="btn btn-success" Text="保存" OnClientClick="return CheckData();"/>--%>
        </div>
    </form>
</asp:Content>

