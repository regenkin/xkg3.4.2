$(document).ready(function () { ShowKey(); $("#chkKeys").bind("click", function () { ShowKey() }); });
function ShowKey() {
    if ($("#chkKeys:checked").length > 0) {
        $(".likey").show();
    }
    else {
        $(".likey").hide();
    }
}
function CheckKey() {
    if ($("#chkKeys:checked").length > 0) {
        if ($("#txtKeys").val() == "") {
            alert("你选择了，关键字回复，请填写关键字！");
            return false;
        }
    }
    var content=$("#fcContent");
    if (content.length>0&&content.val().length == 0) {
        alert("回复内容不能为空！");
        return false;
    }
    return true;
}