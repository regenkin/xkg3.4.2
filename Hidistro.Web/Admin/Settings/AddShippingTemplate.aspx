<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="AddShippingTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.AddShippingTemplate" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>

<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="../js/Region.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
                <div class="page-header">
                    <h2>新建运费模板</h2>
                </div>
                <div class="freight-template">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label">模板名称：</label>
                            <div class="col-xs-3">
                                <input type="Text" id="templateName" class="form-control">
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label">计价方式：</label>
                            <div class="col-xs-3 setradiowidth" id="valuationmethod">
                                <span>
                                    <input type="radio" value="1" name="pric"  id="according" checked="checked">
                                    <label for="according">按件数</label>
                                </span>
                                <span>
                                    <input type="radio" value="2" name="pric" id="weigth">
                                    <label for="weigth">按重量</label>
                                </span>
                                <span>
                                    <input type="radio" value="3" name="pric" id="volume">
                                    <label for="volume">按体积</label>
                                </span>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="inputEmail3" class="col-xs-2 control-label">是否包邮：</label>
                            <div class="col-xs-3 setradiowidth" id="whofreight">
                                <span>
                                    <input type="radio" value="false" name="free" id="myfree" checked="checked">
                                    <label for="myfree">自定义运费</label>
                                </span>
                                <span>
                                    <input type="radio" value="true" name="free" id="hefree">
                                    <label for="hefree">卖家承担运费</label>
                                </span>
                            </div>
                        </div>
                        <div class="form-group clearm" id="shippertypeid">
                            <label for="inputEmail3" class="col-xs-2 control-label">运送方式：</label>
                            <div class="col-xs-10 setexit">
                                <p>除指定地区外，其余地区的运费采用“默认运费”</p>
                                <div class="select">
                                    <p>
                                        <input type="checkbox" name="ShippingType" key="快递" value="1" id="express">
                                        <label for="express">快递</label>
                                    </p>
                                    <div class="freight-editor hidde"></div>
                                </div>
                                <div class="select">
                                    <p>
                                        <input type="checkbox" name="ShippingType"  key="EMS" value="2" id="ems">
                                        <label for="ems">EMS</label>
                                    </p>
                                    <div class="freight-editor hidde"></div>
                                </div>
                                <div class="select">
                                    <p>
                                        <input type="checkbox" name="ShippingType"  key="顺丰" value="3" id="sf">
                                        <label for="sf">顺丰</label>
                                    </p>
                                    <div class="freight-editor hidde"></div>
                                </div>
                                <div class="select">
                                    <p>
                                        <input type="checkbox" name="ShippingType"  key="平邮" value="4" id="mail">
                                        <label for="mail">平邮</label>
                                    </p>
                                    <div class="freight-editor hidde"></div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group" id="freetypeid">
                            <label class="col-xs-2 control-label"></label>
                            <div class="col-xs-10">
                                <div class="specified-condition">
                                    <p>
                                        <input type="checkbox" name="" value="快递" id="HasFree">
                                        <label for="HasFree">是否指定包邮（选填）</label>
                                    </p>
                                    <div class="specified-dis"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

    <div class="footer-btn navbar-fixed-bottom">
        <button type="button" class="btn btn-primary" onclick="saveTemplates()">保存模板信息</button>
        <%--<button type="button" class="btn btn-primary" id="preview">保存并预览</button>--%>
    </div>

    <script type="text/javascript">

        var postData = null;


        // 验证重复元素，有重复返回true；否则返回false
        function isRepeat(str) {
            arr = str.split(",");
            var hash = {};
            for (var i in arr) {

                if (hash[arr[i]]) {
                    return true;
                }
                hash[arr[i]] = true;
            }

            return false;
        }

        //构造POST参数,验证相关参数的准确性
        function saveTemplates() {

            var AllRegions = "0,"; //用来判断是否Regions重复
            var err = 0;
            postData = { TemplateId: 0, task: "add", Name: "", MUnit: 1, FreeShip: 0, HasFree: 0, shipperSelect: [], freeShippings: [] }; //要POST的数据

            postData.Name = $("#templateName").val();

            if (postData.Name.trim().length < 2 || postData.Name.trim().length > 20) {
                HiTipsShow("模板名称2-20个字符！", 'error')
                err = 1;
                return;
            }

            postData.MUnit = $("input[name=pric]:checked").val();

            postData.FreeShip = $("input[name=free]:checked").val();

            if (postData.FreeShip == "false") {
                postData.FreeShip = 0;
            } else {
                postData.FreeShip = 1;
            }

            if (postData.FreeShip == 0) {

                //判断是否有运送方式被选择
                if ($("input[name='ShippingType']:checked").length < 1) {
                    if (err == 0) {
                        HiTipsShow("至少选择一种运送方式，否则无法保存！", 'error');
                        err = 1;
                    }
                    return;
                };

                $("input[name='ShippingType']:checked").each(function () {
                    var keyValue = $(this).attr("key");
                    var ModelId = $(this).val()
                    var SpecifyItem = { ModelId: '', FristNumber: 0, FristPrice: 0, AddNumber: 0, AddPrice: 0, IsDefault: 0, SpecifyRegions: "" };
                    SpecifyItem.ModelId = ModelId;

                    var $thisNext = $(this).parent().next(".freight-editor");
                    var $default = $thisNext.find(".default");

                    //找不到默认运费时，提示错误
                    if ($default.length < 1) {

                        if (err == 0) {
                            HiTipsShow("默认运费未找到，系统错误！", 'error');
                            err = 1;
                        }
                        return;
                    }

                    var $default_input = $default.find("input"); //默认运价input
                    SpecifyItem.FristNumber = $default_input.eq(0).val();
                    SpecifyItem.FristPrice = $default_input.eq(1).val();
                    SpecifyItem.AddNumber = $default_input.eq(2).val();
                    SpecifyItem.AddPrice = $default_input.eq(3).val();
                    SpecifyItem.IsDefault = 1;


                    if (postData.MUnit == 1) {
                        if (! /(^\d+$)/.test(SpecifyItem.FristNumber) || ! /(^\d+$)/.test(SpecifyItem.AddNumber)) {

                            if (err == 0) {
                                HiTipsShow("[" + keyValue + "]：默认运费单位,请填写整数值！", 'error');
                                err = 1;
                            }
                            return;
                        };
                    }
                    else {
                        if (! /(^\d+(\.\d{1,2})?$)/.test(SpecifyItem.FristNumber) || ! /(^\d+(\.\d{1,2})?$)/.test(SpecifyItem.AddNumber)) {

                            if (err == 0) {
                                HiTipsShow("[" + keyValue + "]：默认运费单位,请填写数值,最多保留两位小数！", 'error');
                                err = 1;
                            }
                            return;
                        };
                    }


                    if (! /^\d+(\.\d{1,2})?$/.test(SpecifyItem.FristPrice) || ! /(^\d+(\.\d{1,2})?$)/.test(SpecifyItem.AddPrice)) {

                        if (err == 0) {
                            HiTipsShow("[" + keyValue + "]默认运费价格请填写数值，保留两位小数！", 'error')
                            err = 1;
                        }
                        return;
                    };

                    postData.shipperSelect.push(SpecifyItem); //默认运价加入数组


                    //匹配分区运价信息,
                    var $Region = $thisNext.find(".tbl-except");


                    if ($Region.length > 0) {
                        //HiTipsShow("运输方式："+keyValue+"有一项运费信息未指定地区", 'error')
                        //return;

                        var $RegionItems = $Region.find(".RegionItem");
                        if ($RegionItems.length > 0) {

                            AllRegions = "o,";

                            $RegionItems.each(function () {
                                var SpecifyItem_Regions = { ModelId: ModelId, FristNumber: 0, FristPrice: 0, AddNumber: 0, AddPrice: 0, IsDefault: 0, SpecifyRegions: "" }

                                var $RegionsValue = $(this).find("p").attr("data-storage");

                                if ($RegionsValue == null || $RegionsValue == "") {

                                    if (err == 0) {
                                        HiTipsShow("运输方式[" + keyValue + "]：有一项运费信息未指定具体地区！", 'error');
                                        err = 1;
                                    }
                                    return false;
                                };

                                var $RegionItem_input = $(this).find("input[type=text]");
                                //获取对应数值
                                SpecifyItem_Regions.FristNumber = $RegionItem_input.eq(0).val();
                                SpecifyItem_Regions.FristPrice = $RegionItem_input.eq(1).val();
                                SpecifyItem_Regions.AddNumber = $RegionItem_input.eq(2).val();
                                SpecifyItem_Regions.AddPrice = $RegionItem_input.eq(3).val();
                                SpecifyItem_Regions.SpecifyRegions = $RegionsValue;
                                postData.shipperSelect.push(SpecifyItem_Regions);

                                AllRegions += $RegionsValue;

                                if (isRepeat(AllRegions)) {
                                    if (err == 0) {
                                        HiTipsShow("[" + keyValue + "]指定地区运费信息有部份区域重复！", 'error')
                                        err = 1;
                                    }
                                    return;
                                };


                               
                                if (postData.MUnit == 1) {
                                    if (! /^\d+$/.test(SpecifyItem_Regions.FristNumber) || ! /^\d+$/.test(SpecifyItem_Regions.AddNumber)) {

                                        if (err == 0) {
                                            HiTipsShow("[" + keyValue + "]指定地区运费单位请填写整数值！", 'error')
                                            err = 1;
                                        }
                                        return;
                                    };
                                }
                                else {
                                    if (! /^\d+(\.\d{1,2})?$/.test(SpecifyItem_Regions.FristNumber) || ! /^\d+(\.\d{1,2})?$/.test(SpecifyItem_Regions.AddNumber)) {

                                        if (err == 0) {
                                            HiTipsShow("[" + keyValue + "]指定地区运费单位请填写数值，保留2位小数！", 'error')
                                            err = 1;
                                        }
                                        return;
                                    };

                                }

                                if (! /^\d+(\.\d{1,2})?$/.test(SpecifyItem_Regions.FristPrice) ||  ! /^\d+(\.\d{1,2})?$/.test(SpecifyItem_Regions.AddPrice)) {

                                    if (err == 0) {
                                        HiTipsShow("[" + keyValue + "]指定地区运费价格请填写数值，保留两位小数！", 'error')
                                        err = 1;
                                    }
                                    return;
                                };



                            });

                        };
                    }

                });


                if (err == 1) {
                    return;
                }



                //读取包邮条件
                if ($("#HasFree").prop("checked")) {
                    postData.HasFree = 1;
                    var $FreeRegions = $(".FreeRegion");
                    if ($FreeRegions.length < 1) {
                        if (err == 0) {
                            HiTipsShow("包邮条件未设置！", 'error');
                            err = 1;
                        }
                        return;
                    }


                    var FreeAllRegions = {}; //包邮地区临时数据，方便去重复，key值为ModelId

                    $FreeRegions.each(function (rows) {
                        var $RegionsStr = $(this).find("p").attr("data-storage");
                        var FreeShippingRegion = { ModelId: 0, ConditionType: 0, ConditionNumber: "", FreeRegions: "" };
                        if ($RegionsStr == null || $RegionsStr == "") {

                            if (err == 0) {
                                HiTipsShow("指定包邮第" + (rows * 1 + 1) + "项未指定地区！", 'error');
                                err = 1;
                            }
                            return;
                        };

                        FreeShippingRegion.FreeRegions = $RegionsStr;
                        FreeShippingRegion.ModelId = $(this).find(".select-express").val();
                        FreeShippingRegion.ConditionType = $(this).find(".setfreeshipping").val();
                        var Conditions = $(this).find("input")
                        FreeShippingRegion.ConditionNumber = Conditions.eq(0).val();
                        if (Conditions.length > 1) FreeShippingRegion.ConditionNumber += "$" + Conditions.eq(1).val();


                        if (FreeAllRegions[FreeShippingRegion.ModelId] == null) {
                            FreeAllRegions[FreeShippingRegion.ModelId] = "o," + $RegionsStr;
                        } else {
                            FreeAllRegions[FreeShippingRegion.ModelId] += $RegionsStr;
                        }

                        if (isRepeat(FreeAllRegions[FreeShippingRegion.ModelId])) {


                            if (err == 0) {
                                HiTipsShow("指定包邮第" + (rows * 1 + 1) + "项,指定区域有重复，请查检！", 'error');
                                err = 1;
                            }

                            return;
                        }

                        if (FreeShippingRegion.ConditionNumber == "") {

                            if (err == 0) {
                                HiTipsShow("指定包邮第" + (rows * 1 + 1) + "项未填数值！", 'error');
                                err = 1;
                            }
                            return;
                        } else {
                            if (! /(^\d+(\$\d+)?$)/.test(FreeShippingRegion.ConditionNumber)) {

                                if (err == 0) {
                                    HiTipsShow("指定包邮第" + (rows * 1 + 1) + "项数值填写错误！", 'error');
                                    err = 1;
                                }
                                return;
                            }
                        }

                        postData.freeShippings.push(FreeShippingRegion);
                    });
                };

                if (err == 1) {
                    return;
                }

            }


            //alert(JSON.stringify(postData));

            //使用AJAXA保存数据
            $.post("AddShippingTemplate.aspx", postData, function (msg) {
                if (msg != "") {

                    try {
                        var result = eval("(" + msg + ")");
                        if (result.state == "success") {
                            HiTipsShow("添加模板成功,2秒后自动转到模板管理界面！", 'success', function () {
                                // return;
                                window.location = "ManageShippingTemplates.aspx";
                            });
                        } else {
                            HiTipsShow("出错了：" + result.msg, 'error');
                        }

                    } catch (e) {
                        HiTipsShow("服务器异常！", 'error');
                    }

                } else {
                    HiTipsShow("连接服务器出错！", 'error');
                }
            });

        }




        //////////////////////////////////////////////////////////////////////////////////////////////

        //保存区域选择的临时变量对象

        var SelData = {
            Group1: { Regions: [{ RegionIds: "" }] },
            Group2: { Regions: [{ RegionIds: "" }] },
            Group3: { Regions: [{ RegionIds: "" }] },
            Group4: { Regions: [{ RegionIds: "" }] },
            FreeShippings: [
            //{ ModeId: 1, ConditionNumber: 2, ConditionType: 1, RegionIds: "" }
            ],
        };


        $(function () {

            var temlElement = $('.freight-editor');
            str = '件',
            strWords='件',
            options = '',
            inputElement = '',
            remove = '<div class="batch"><input type="checkbox" id="allselect">&nbsp;<label>全选</label><a href="javascript:void(0)">批量设置</a><a href="javascript:void(0)" id="removeradio">批量删除</a></div>',
            tableElement = '<div class="tbl-except"><table class="table table-hover table-bordered"><thead><tr><th width="260">运送到</th><th>首件(件)</th><th>首费(元)</th><th>续件(件)</th><th>续费(元)</th><th>操作</th></tr></thead><tbody><tr class="RegionItem"><td><a href="javascript:void(0)" class="exit-area">编辑</a><div class="area-group"><p>未添加地区</p></div></td><td><input type="text" value="" name="" data-field="start"></td><td><input type="text" value="" name="" data-field="postage"></td><td><input type="text" value="" name="" data-field="plus"></td><td><input type="text" value="" name="" data-field="postageplus"></td><td><a class="delete" href="javascript:void(0)">删除</a></td></tr></tbody></table></div>';

            //计价方式事件
            $('#valuationmethod input').click(function () {
                if (confirm('切换计价方式后，所设置当前模板的运输信息将被清空，确定继续么？')) {
                    switch ($('#valuationmethod input:checked').val()) {
                        case '1':
                            str = '件';
                            strWords = '件';
                            break;
                        case '2':
                            str = 'kg';
                            strWords = '重';
                            break;
                        case '3':
                            str = 'm<sup>3</sup>';
                            strWords = '体积';
                            break;
                    }
                    tableElement = '<div class="tbl-except"><table class="table table-hover table-bordered"><thead><tr><th width="260">运送到</th><th>首' + strWords + '(' + str + ')</th><th>首费(元)</th><th>续' + strWords + '(' + str + ')</th><th>续费(元)</th><th>操作</th></tr></thead><tbody><tr class="RegionItem"><td><a href="javascript:void(0)" class="exit-area">编辑</a><div class="area-group"><p>未添加地区</p></div></td><td><input type="text" value="" name="" data-field="start"></td><td><input type="text" value="" name="" data-field="postage"></td><td><input type="text" value="" name="" data-field="plus"></td><td><input type="text" value="" name="" data-field="postageplus"></td><td><a class="delete" href="javascript:void(0)">删除</a></td></tr></tbody></table></div>';

                    $('.freight-editor').addClass('hidde').children().remove();
                    $('.select p input').attr('checked', false);
                } else {
                    return false;
                }
            });


            //调整运费方式事件
            $('#whofreight input').click(function () {
                if ($(this).val() == 'false') {
                    HiTipsShow('您的运费设置将变为未设置状态，请设置运费', 'success');
                    $('#shippertypeid').show();
                    $('#freetypeid').show();
                } else {
                    HiTipsShow('选择“卖家承担运费”后所有区域的运费将设置为0且原运费设置无法恢复，请保存原有运费设置。', 'success');
                    $('#shippertypeid').hide();
                    $('#freetypeid').hide();
                }
                $('.freight-editor').addClass('hidde').children().remove();
                $('.select p input').attr('checked', false);
            })
            //复选框事件
            temlElement.on('click', '.delete', function () {
                if (confirm('您确定要删除当前地区的设置吗？')) {
                    var row = $(this).parents(".RegionItem").index();
                    var $Group = $(this).parents('.freight-editor').prev().find("input").val();
                    $(this).parent().parent().remove();
                    SelData["Group" + $Group].Regions.splice(row, 1)
                } else {
                    return;
                }
            });


            //删除全部选中的tr
            temlElement.on('click', '#removeradio', function () {
                if ($(this).parent().prev().find('input:checked').get(0).checked) {
                    $(this).parent().prevAll('.tbl-except').find('.area-group input').each(function () {
                        if (this.checked) {

                            var row = $(this).parents(".RegionItem").index();
                            var $Group = $(this).parents('.freight-editor').prev().find("input").val();
                            $(this).parents('tr').remove();
                            SelData["Group" + $Group].Regions.splice(row, 1)


                        }
                    })
                    $(this).parent().nextAll('.tbl-attach').find('.batch-operation').remove();
                    $(this).parent().remove();
                    inputElement = "";
                } else {
                    alert('请选择批量选择的地区');
                }
            });

            //全选按钮事件
            temlElement.on('change', '#allselect', function () {
                if (this.checked) {
                    $(this).parent().prevAll('.tbl-except').find('.area-group input').prop('checked', true);
                } else {
                    $(this).parent().prevAll('.tbl-except').find('.area-group input').prop('checked', false);
                }
            });

            //批量操作按钮事件
            temlElement.on('click', '.batch-operation', function () {
                if (!$(this).parent().prevAll().hasClass('batch')) {
                    $(this).parent().before('<div class="batch clearfix"><input type="checkbox" id="allselect">&nbsp;<label  class="mr5">全选</label><a href="javascript:void(0)" class="mr5"><span id="template-y">批量设置</span><div class="downup"><div class="form-inline"><div class="form-group"><label for="defaultship">首件：</label><input type="text" class="form-control" data-field="start"><label>，' + str + '内</label></div><div class="form-group"><input type="txet" class="form-control" data-field="postage"><label>&nbsp;元</label></div><div class="form-group"><label>&nbsp;续件&nbsp;</label><input type="txet" class="form-control" data-field="plus"><label>&nbsp;' + str + '</label></div><div class="form-group"><label>&nbsp;续费&nbsp;</label><input type="txet" class="form-control" data-field="postageplus"><label>&nbsp;元</label></div></div><div class="temp-button"><button type="button" class="btn btn-success btn-sm modifytmpl sucmodify">确定</button><button type="button" class="btn btn-success btn-sm modifytmpl">取消</button></div></div></a><a href="javascript:void(0)" id="removeradio">批量删除</a></div>');
                    if (!$(this).parent().prevAll('.tbl-except').find('.area-group input').length) {
                        $(this).parent().prevAll('.tbl-except').find('.area-group').prepend('<input type="checkbox" name="">');
                    }
                    inputElement = '<input type="checkbox" name="">';
                    $(this).text('取消批量');
                } else {
                    $(this).text('批量操作');
                    inputElement = "";
                    $(this).parent().prev('.batch').remove();
                    $(this).parent().prevAll('.tbl-except').find('.area-group input').remove();
                }
            });

            temlElement.on('click', '.sucmodify', function () {
                var inputVal = {
                    start: $(this).parents('.downup').find('input[data-field="start"]').val(),
                    postage: $(this).parents('.downup').find('input[data-field="postage"]').val(),
                    plus: $(this).parents('.downup').find('input[data-field="plus"]').val(),
                    postageplus: $(this).parents('.downup').find('input[data-field="postageplus"]').val()
                };
                $(this).parents('.batch').prevAll('.tbl-except').find('input[type="text"]').each(function () {
                    switch ($(this).attr('data-field')) {
                        case 'start':
                            $(this).val(inputVal.start);
                            break;
                        case 'postage':
                            $(this).val(inputVal.postage);
                            break;
                        case 'plus':
                            $(this).val(inputVal.plus);
                            break;
                        case 'postageplus':
                            $(this).val(inputVal.postageplus);
                            break;
                    }
                });
            });
            //批量设置按钮事件
            temlElement.on('click', '.modifytmpl', function () {
                $(this).parents('.downup').hide();
            });
            //批量设置事件
            temlElement.on('click', '#template-y', function () {
                $(this).next().show();
            });
            //添加表格与行事件
            temlElement.on('click', '.designated-areas', function () {
                if (!$(this).parent().prevAll().hasClass('tbl-except')) {
                    $(this).parent().before(tableElement);
                } else {
                    $(this).parent().prevAll('.tbl-except').find('table tbody').append('<tr class="RegionItem"><td><a href="javascript:void(0)" class="exit-area">编辑</a><div class="area-group">' + inputElement + '<p>未添加地区</p></div></td><td><input type="text" value="" data-field="start"></td><td><input type="text" value="" name="" data-field="postage"></td><td><input type="text" value="" name="" data-field="plus"></td><td><input type="text" value="" name="" data-field="postageplus"></td><td><a class="delete" href="javascript:void(0)">删除</a></td></tr>');
                }

                var row = $(this).parent().prev().find(".RegionItem:last").index();
                var $Group = $(this).parents('.freight-editor').prev().find("input").val();

                if (!SelData["Group" + $Group].Regions[row]) {
                    var newRegion = { RegionIds: "" };
                    SelData["Group" + $Group].Regions.push(newRegion);
                }

                if (!$(this).next().get(0)) {
                    $(this).parent().append('&nbsp;&nbsp;<a href="javascript:void(0)" class="batch-operation">批量操作</a>');
                };

            });

            //快递等4个按钮的选中事件
            $('.select p input').change(function () {
                inputElement = "";
                $('.select-express').empty();
                $('.select p input').each(function () {
                    if ($(this)[0].checked) {
                        $('.select-express').append('<option  value="' + $(this).val() + '">' + $(this).attr("key") + '</option>');
                    }
                });
                if ($('#whofreight input:checked').val() == 'false') {
                    if ($(this)[0].checked) {
                        $(this).parent().next().removeClass('hidde');
                        if (!$(this).parent().next().children().length) {
                            $(this).parent().next().append('<div class="entity"><div class="default"><div class="form-inline"><div class="form-group"><label>默认运费：</label><input type="text" class="form-control"><label>&nbsp;' + str + '内，</label></div><div class="form-group"><input type="text" class="form-control"><label>&nbsp;元；</label></div><div class="form-group"><label>&nbsp;每增加</label><input type="text" class="form-control"><label>&nbsp;' + str + '&nbsp;</label></div><div class="form-group"><label>&nbsp;增加运费</label><input type="text" class="form-control"><label>&nbsp;元</label></div></div></div><div class="tbl-attach"><a href="javascript:void(0)" class="designated-areas">为指定地区城市设置运费</a></div></div>');
                        }
                    } else {
                        $(this).parent().next().addClass('hidde');
                    }
                }
            });
            function setStrOption() {
                options = "";
                $('.select p input').each(function () {
                    if ($(this)[0].checked) {
                        options += '<option value="' + $(this).val() + '">' + $(this).attr("key") + '</option>';
                    }
                })
            }
            //是否包邮事件
            $('.specified-condition > p input').change(function () {


                if ($(this)[0].checked) {
                    if ($("input[name='ShippingType']:checked").length < 1) {
                        $(this).prop("checked", false);
                        HiTipsShow('至少选择一个运送方式，才能启用包邮设置 ', 'error');
                        return;
                    }

                    setStrOption();
                    if (!$(this).parent().next().children().length) {
                        $(this).parent().next().append('<table class="table table-hover table-bordered"><thead><tr><th width="30%">选择地区</th><th>选择运送方式</th><th width="50%">设置包邮条件</th><th>操作</th></tr></thead><tbody><tr class="FreeRegion"><td><a href="javascript:void(0)" class="exit-area">编辑</a><div class="area-group"><p>未添加地区</p></div></td><td><select class="select-express">' + options + '</select></td><td><select class="setfreeshipping"><option value="1">件数</option><option  value="2">金额</option><option  value="3">件数+金额</option></select>　<span class="free-contion">满 <input type="text" value="" class="input-text " name="preferentialStandard"> 件包邮</span></td><td><p class="oper"><a href="javascript:void(0)" class="add">＋</a><a href="javascript:void(0)" class="remove">×</a></p></td></tr></tbody></table>');
                    }

                    var newRegion = { ModeId: $(this).parent().next().find(".select-express").val(), RegionIds: "" };
                    SelData.FreeShippings.push(newRegion);

                } else {
                    $(this).parent().next().empty();
                    SelData.FreeShippings = [];
                }
            });
            //是否包邮表格添加行事件
            $('.specified-dis').on('click', '.add', function () {
                setStrOption();
                $('.specified-dis table tbody').append('<tr class="FreeRegion"><td><a href="javascript:void(0)" class="exit-area">编辑</a><div class="area-group"><p>未添加地区</p></div></td><td><select class="select-express">' + options + '</select></td><td><select class="setfreeshipping"><option value="1">件数</option><option value="2">金额</option><option value="3">件数+金额</option></select>　<span class="free-contion">满 <input type="text" value="" class="input-text " name="preferentialStandard"> 件包邮</span></td><td><p class="oper"><a href="javascript:void(0)" class="add">＋</a><a href="javascript:void(0)" class="remove">×</a></p></td></tr>');
                var row = $('.FreeRegion').length - 1;
                if (!SelData.FreeShippings[row]) {
                    var newRegion = { ModeId: $(this).parents('tr').next().find(".select-express").val(), RegionIds: "" };
                    SelData.FreeShippings.push(newRegion);
                }
                $("body").scrollTop($("body")[0].offsetHeight);
            })
            //是否包邮表格删除行事件
            $('.specified-dis').on('click', '.remove', function () {
                var row = $(this).parents('tr').index();
                $(this).parents('tr').remove();
                SelData.FreeShippings.splice(row, 1);
            });

            //包邮运输方式变动
            $('.specified-dis').on('change', '.select-express', function () {
                var row = $(this).parents('tr').index();
                SelData.FreeShippings[row].ModeId = $(this).val();
                SelData.FreeShippings[row].RegionIds = "";
                $(this).parents('tr').find(".area-group p").text("未添加地区");
            });

            //包邮选择框事件
            $('.specified-dis').on('change', '.setfreeshipping', function () {
                $(this).next().empty();
                switch ($(this).val()) {
                    case '1':
                        $(this).next().append('满 <input type="text" value=""> 件包邮');
                        break;
                    case '2':
                        $(this).next().append('满 <input type="text" value="">元包邮');
                        break;
                    case '3':
                        $(this).next().append('满 <input type="text" value=""> 件,<input type="text" value="">元以上包邮');
                        break;
                    default:
                        return;
                }
            });


            //编辑地区
            $('.freight-template').on('click', '.exit-area', function () {

                $(this).next().find('p').attr('data-area', true);
                if ($('#area').length > 0) $('#area').remove();//每次重新生成，清除不必需的标签
                createArea();

                var obj = this;

                var checkedData = ""; //已先值
                var brothersData = ""; //其它待剔除值

                var $Group = $(obj).parents('.freight-editor').prev().find("input").val();

                //alert($Group);

                if ($Group) {
                    //运送方式处弹出

                    $Group = "Group" + $Group;
                    var tempg = $Group;

                    $Group = SelData[$Group].Regions;//Regions=[{ FristNumber: 0, FristPrice: 0, AddNumber: 0, AddPrice: 0, RegionIds: "" }];

                    var row = $(this).parents(".RegionItem").index(); //获取当前行index;


                    // alert(row+JSON.stringify($Group));

                    for (var n = 0; n < $Group.length; n++) {
                        //取出
                        if (n != row && $Group[n].RegionIds != "") {
                            brothersData = brothersData + $Group[n].RegionIds;
                        } else if (n == row) {
                            checkedData = $Group[n].RegionIds;
                        }
                    }



                } else {

                    //包邮地区处弹出
                    var row = $(this).parents(".FreeRegion").index(); //获取当前行index;
                    var $Mode = $(this).parents(".FreeRegion").find(".select-express").val();

                    //SelData.FreeShippings[];//{ ModeId: 1, ConditionNumber: 2, ConditionType: 1, RegionIds: "" };

                    for (var n = 0; n < SelData.FreeShippings.length; n++) {
                        //取出
                        if (SelData.FreeShippings[n].ModeId == $Mode) {
                            if (n != row && SelData.FreeShippings[n].RegionIds != "") {
                                brothersData = brothersData + SelData.FreeShippings[n].RegionIds;
                            } else {
                                checkedData = SelData.FreeShippings[n].RegionIds;
                            }
                        }
                    }
                }


                //初始化已选值

                // alert(checkedData);

                if (checkedData != "") {

                    var data = checkedData.split(',');
                    data.pop();
                    for (var i = 0; i < data.length; i++) {
                        $('#city_' + data[i]).prop('checked', true);
                        //$('#city_' + data[i]).prop('disabled', true);
                    };

                    $('#area .city-box').each(function () {
                        var checkedLength = $(this).find('input:checked').length;
                        if (checkedLength == $(this).find('input').length) {
                            $(this).prevAll('label').find('input').prop('checked', true);
                        }
                        if (checkedLength) {
                            $(this).prevAll('.select-number').text('(' + checkedLength + ')');
                        }
                    });

                }

                //禁用重复选值
                if (brothersData != "") {
                    var data = brothersData.split(',');
                    data.pop();
                    for (var i = 0; i < data.length; i++) {
                        $('#city_' + data[i]).prop('checked', false);
                        $('#city_' + data[i]).prop('disabled', true);//禁用
                    };

                    //禁用省
                    $('#area .city-box').each(function () {
                        var checkedLength = $(this).find('input:disabled').length;
                        if (checkedLength == $(this).find('input').length) {
                            $(this).prevAll('label').find('input').prop('disabled', true);
                        }
                    });
                }

            });


            var createArea = function () {

                var html = "",
                    proName = "",
                    i = 0;
                for (i; i < province.length; i++) {
                    var cityHtml = "";
                    var j = 0;
                    for (j; j < province[i]["city"].length; j++) {
                        cityHtml += '<li><label><input type="checkbox" id="city_' + province[i]["city"][j]["id"] + '" class="citycheckbox" value="' + province[i]["city"][j]["name"] + '" data-city="' + province[i]["city"][j]["id"] + '">' + province[i]["city"][j]["name"] + '</label></li>';
                    }
                    html += '<li><label><input type="checkbox" data-province="' + province[i]["name"] + '" id="province_' + province[i]["id"] + '" class="pro-check">' + province[i]["name"] + '</label><span class="select-number"></span><b class="glyphicon glyphicon-menu-down citydown"></b><div class="city-box"><ul class="clearfix">' + cityHtml + '</ul><i class="colse">×</i></div></li>';
                }

                $('body').append('<div id="area"><div class="area-title"><h3>选择区域</h3><a class="aui_close" href="javascript:void(0)">×</a></div><div class="setinside"><ul class="clearfix">' + html + '</ul><button class="btn btn-success area-ok">确定</button></div></div>');

            };

            $('body').on('click', '.citydown', function () {
                $(this).parents('.setinside').find('.city-box').hide();
                $(this).next().show();
            });

            $('body').on('click', '.colse', function () {
                $(this).parent().hide();
            });

            //省级选择框事件
            $('body').on('change', '.pro-check', function () {
                var cityInput = $(this).parent().nextAll('.city-box');
                if (!$(this)[0].checked) {
                    cityInput.find('input').prop('checked', false);
                } else {
                    cityInput.find('input').prop('checked', true);
                    cityInput.find('input[disabled]').prop('checked', false).attr('disab', false);
                }
                $(this).parent().next().text('(' + cityInput.find('input:checked').length + ')');
            });

            //市级选择框事件
            $('body').on('click', '.citycheckbox', function () {
                var downCity = $(this).parents('.city-box');
                downCity.prevAll('.select-number').text('(' + downCity.find('input:checked').length + ')');
            })
            $('body').on('click', '.aui_close', function () {
                $(this).parents('#area').hide();
                $('.area-group p').removeAttr('data-area');
            })
            //选择地区确定事件
            $('body').on('click', '.area-ok', function () {
                $(this).parents('#area').hide();
                var str = data = "";

                $('#area .city-box input:checked').each(function () {
                    data += $(this).attr('data-city') + ',';
                });


                $('#area .city-box').each(function () {
                    var checkedLength = $(this).find('input:checked').length;
                    if (checkedLength) {
                        if (checkedLength == $(this).find('input').length) {
                            str += $(this).prevAll('label').find('input').attr('data-province') + '、';
                        } else {
                            $(this).find('input').each(function () {
                                if ($(this)[0].checked) {
                                    str += $(this).val() + '、';
                                }
                            });
                        }
                    }
                })

                var $Group = $('.area-group p[data-area]').parents('.freight-editor').prev().find("input").val();

                if ($Group) {

                    $Group = "Group" + $Group;
                    var tempg = $Group;
                    $Group = SelData[$Group].Regions;//Regions=[{ FristNumber: 0, FristPrice: 0, AddNumber: 0, AddPrice: 0, RegionIds: "" }];
                    var row = $('.area-group p[data-area]').parents(".RegionItem").index(); //获取当前行index;
                    $Group[row].RegionIds = data;

                } else {
                    var row = $('.area-group p[data-area]').parents(".FreeRegion").index(); //获取当前行index;
                    var $Mode = $('.area-group p[data-area]').parents(".FreeRegion").find(".select-express").val();
                    SelData.FreeShippings[row].RegionIds = data;
                }

                if (str == "") {
                    str = "未添加地区";
                } else {
                    str = str.substring(0, str.length - 1);
                }

                $('.area-group p[data-area]').attr('data-storage', data);
                $('.area-group p[data-area]').text(str).attr('title', str);
                $('.area-group p').removeAttr('data-area');

            })
            $('body').on('mousedown', '.area-title h3', function (evt) {
                var downX = evt.clientX,
                    downY = evt.clientY,
                    topN = downY - parseInt($('#area').css('top')),
                    leftN = downX - parseInt($('#area').css('left'));
                winH = $(window).height(),
                elemH = $('#area').height(),
                winW = $(window).width(),
                elemW = $('#area').width();
                $(document).on('mousemove', function (evt) {
                    var moveX = evt.clientX - leftN,
                        moveY = evt.clientY - topN;
                    if (moveY < 0) moveY = 0;
                    if (moveY > winH - elemH) moveY = winH - elemH;
                    if (moveX < 200) moveX = 200;
                    if (moveX > winW - elemW + 200) moveX = winW - elemW + 200;
                    $('#area').css({
                        'left': moveX,
                        'top': moveY
                    })
                })
                $(document).on('mouseup', function () {
                    $(document).off('mousemove');
                    $(document).off('mouseup');
                })
            })
        })


</script>
</asp:Content>
