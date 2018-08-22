


function CheckPrzie(prizeType) {
    var prizeName = '';
    switch (prizeType) {
        case 0:
            {
                prizeName = '一等奖';
                break;
            }
        case 1:
            {
                prizeName = '二等奖';
                break;
            }
        case 2:
            {
                prizeName = '三等奖';
                break;
            }
        case 3:
            {
                prizeName = '四等奖';
                break;
            }
        default:
            break;
    }


    //综合中奖率
    var prizeRate = $("#ctl00_ContentPlaceHolder1_UCGamePrizeInfo_txtPrizeRate").val();
    var r = /^(\d{1,2}(\.\d{1,2})?|100)$/;
    if (!r.test(prizeRate) || prizeRate == 0) {
        ShowMsg("综合中奖率为0-100之间的实数，精确到小数点后两位！");
        return false;
    };

    //奖品名称
    var prize = $("#txtPrize" + prizeType).val();
    if (!CheckValue(prize)) {
        ShowMsg('请输入' + prizeName + '奖品名称!');
        return false;
    }
    else {
        if (!checklength(prize, prizeName))
        {
            ShowMsg(prizeName + "奖品名称不能超过10个中文长度!");
            return false;
        }
    }

    if ($("#rd" + prizeType + "_0")[0].checked) {
        var point0 = $("#txtGivePoint" + prizeType).val();
        if (!CheckValue(point0)) {
            ShowMsg('请输入' + prizeName + '赠送积分!');
            return false;
        }
        if (!IsNum(point0)) {
            ShowMsg(prizeName + '赠送积分格式不对!');
            return false;
        }
    }
    if ($("#rd" + prizeType + "_1")[0].checked) {
        if (!CheckValue($("#seletCouponId" + prizeType).val())) {
            ShowMsg('请选择' + prizeName + '赠送优惠券!');
            return false;
        }
    }
    if ($("#rd" + prizeType + "_2")[0].checked) {
        if (!CheckValue($("#txtShopbookId" + prizeType).val())) {
            ShowMsg('请选择' + prizeName + '赠送商品!');
            return false;
        }
    }

    var prizeCount0 = $("#txtPrizeCount" + prizeType).val();
    if (!CheckValue(prizeCount0)) {
        ShowMsg('请输入' + prizeName + '奖品数量!');
        return false;
    }
    if (!IsNum(prizeCount0)) {
        ShowMsg(prizeName + '奖品数量格式不对!');
        return false;
    }
    var prizeRate0 = $("#txtPrizeRate" + prizeType).val();
    return true;
}
function IsNum(str) {
    var re = /^[0-9]*]*$/
    return re.test(str);
}
function checklength(value, name) {
    var less = getByteLen(value);
    if (less <= 20) {
        return true;
    }
    else {
        return false;
    } 
}
function getByteLen(val) {
    var len = 0;
    for (var i = 0; i < val.length; i++) {
        if (val[i].match(/[^x00-xff]/ig) != null) //全角
            len += 2;
        else
            len += 1;
    }
    return len;
}
function CheckPrizeInfo() {
    var index = $("#tabGameMenu li").length;
    $("#ctl00_ContentPlaceHolder1_UCGamePrizeInfo_hfIndex").val(index);
    var rate = $("#ctl00_ContentPlaceHolder1_UCGamePrizeInfo_txtPrizeRate").val();
    var r = /^(\d{1,2}(\.\d{1,2})?|100|100.00)$/;
    if (!r.test(rate) || rate == 0) {
        ShowMsg("综合中奖率为0-100之间的实数，精确到小数点后两位！");
        return false;
    };
    var index = $("#tabGameMenu li").length;
    //addEventListener
    switch (parseInt(index)) {
        case 1:
            if (!CheckPrzie(0)) return false;
            break;
        case 2:
            if (!CheckPrzie(0)) return false;
            if (!CheckPrzie(1)) return false;
            break;
        case 3:
            if (!CheckPrzie(0)) return false;
            if (!CheckPrzie(1)) return false;
            if (!CheckPrzie(2)) return false;
            break;
        case 4:
            if (!CheckPrzie(0)) return false;
            if (!CheckPrzie(1)) return false;
            if (!CheckPrzie(2)) return false;
            if (!CheckPrzie(3)) return false;
            break;
        default:
    }
    return true;
}

function CheckValue(value) {
    if (value == '' || value == null) {
        return false;
    }
    return true;
}

function ShowSuccess() {
    ShowStep2();
}

function ShowStep2() {
    $("#step1").hide();
    $("#step2").hide();
    $("#btnSubmit").show();
    $("#step3").show();
    //显示二维码
    winqrcode();
}

