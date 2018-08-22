var gameData;

function InitInfo() {
    var gameId = GetGameId();
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'text',
        data: { "action": "getprizelists", "gameId": gameId },
        async: false,
        success: function (data) {
            data = eval('(' + data + ')');
            if (data.status == '0') {
                alert_h(data.Desciption);
            } else {
                gameData = data;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            //onsole.log("请求出错了");
        }
    });
}

//判断是否还能玩
function IsCanPlay()
{
    var result = false;
    var gameId = GetGameId();
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "checkusercanplay", "gameId": gameId },
        async: false,
        success: function (data)
        {
            if (data.status == 1) {
                result = true;
            }
        }
    });
    return result;
}
//抽奖
function GetPrize() {
    var gameId = GetGameId();
    var message ="";
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "getprizeinfo", "gameId": gameId },
        async: false,
        success: function (data) {
            message = data;
        }
    });
    return message;
}


//分享

//抽奖
function GetOpportunity() {
    var gameId = GetGameId();
    var oppNumber = 0;
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "GetOpportunity", "gameId": gameId, "LimitEveryDay": gameData.LimitEveryDay, "MaximumDailyLimit": gameData.MaximumDailyLimit },
        async: false,
        success: function (data) {
            if (data.status == "ok") {
                oppNumber = data.opportunitynumber;
            }
        }
    });
    return oppNumber;
}

var pageIndex = 1;
var pageSize = 7;
function UserPrizeLists() {
    var gameId = GetGameId();
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "getuserprizelists", "gameId": gameId,"pageIndex":pageIndex,"pageSize":pageSize },
        async: false,
        success: function (data) {
            var emlement = "";
            $.each(data.lists, function (index, item) {
                emlement += " <ul><li>" + item.UserName + "</li><li style='width:30%'>" + item.Prize + "</li><li style='width:20%'>" + item.PrizeGrade + "</li><li>" + item.DateTime + "</li></ul>";
            });
            $("#userPrizeLists").append(emlement);
        }
    });
    pageIndex++;
    nDivHight = $(".nameList").outerHeight();
}



function GetGameId() {
    var gameId = window.location.search.substr(window.location.search.indexOf("=") + 1);
    if (gameId.indexOf("&") > 0)
        gameId = gameId.substr(0, gameId.indexOf("&"));
    return gameId;
}

//验证手机号码
function CheckMemberPhone() {
    //0 不需要验证  1 需要验证
    var checkMember = gameData.MemberCheck;
    if (parseInt(checkMember) > 0) {
        //弹窗提示
        if (parseInt(gameData.HasPhone) == 0) {
            $("body").append('<div class="mask"><div class="phone-box"><div class="form-phone"><h5>请填写您的手机号码</h5><div class="form-input"><label>手机号：</label><input type="tel" id="txtCellPhone"></div><p class="showmessage" style="color:red"></p><button class="btn-phone" type="button" id="btnSavePhone">确认手机号码</button><span class="btn-close"><a href="/default.aspx">×</a></span></div></div></div>');
            $('.mask').fadeIn(300);
        }
    }
}

///修改手机号码
function UpdateCellPhone(phone) {
    $.ajax({
        url: "/API/Hi_Ajax_Game.ashx",
        type: 'post', dataType: 'json',
        data: { "action": "UpdateCellPhone", "CellPhone": phone },
        async: false,
        success: function () {
            returnFn();
        }
    });
}

//保存手机号码
function SavePhone() {
   
    $(document).on('click', '#btnSavePhone', function () {
        var phone = $("#txtCellPhone").val();
        var phoneReg = /^0?1[3|4|5|6|7|8|9][0-9]\d{8}$/;
        if (phone == "" || !phoneReg.test(phone)) {
            $(".showmessage").html("输入的手机号码不合法");
            return;
        }
        else {
            UpdateCellPhone(phone);
        }
    });
}

var nDivHight;
$(function () {
    var nScrollHight = 0; //滚动距离总长(注意不是滚动条的长度)
    var nScrollTop = 0;   //滚动到的当前位置
    $(".nameList").scroll(function () {
        nScrollHight = $(this)[0].scrollHeight;
        nScrollTop = $(this)[0].scrollTop;
        if (nScrollTop + nDivHight >= nScrollHight)
            UserPrizeLists();
    });
});
