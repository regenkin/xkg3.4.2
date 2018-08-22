
function AddMenu(settingjson)
{
    $("#menuIn").empty();
    CreateHome("#menuIn");
    CreateTel(settingjson.Phone, "#menuIn");
    CreateCart("#menuIn");
    CreateMember("#menuIn");
    CreateShare("#menuIn");
    CreateCollect(settingjson.GuidePage, "#menuIn");
    CreateSearch("#menuIn");
}

//首页
function CreateHome(ele)
{
    $(ele).append("<a class=\"a-home\" href=\"/Default.aspx\" title=\"首页\" style=\"display: block;\"></a>");
}
//联系店主
function CreateTel(tel,ele) {
    $(ele).append(" <a class=\"a-tel\" href=\"tel:" + tel + "\" title=\"联系店主\" style=\"display: inline;\"></a>");
}
//购物车
function CreateCart(ele) {
    $(ele).append("<a class=\"a-member\" href=\"/Vshop/ShoppingCart.aspx\" title=\"购物车\" style=\"display: block;\"></a>");
}
//会员中心
function CreateMember(ele) {
    $(ele).append(" <a class=\"a-search\" href=\"/Vshop/MemberCenter.aspx\" title=\"会员\" style=\"display: block;\"></a>");
}
//分享
function CreateShare(ele) {
    $(ele).append("  <a class=\"a-cart\" href=\"javascript:;\" id=\"share-link\" title=\"分享\" style=\"display: block;\"></a>");
    // 分享
    $('#share-link').click(function (event) {
        $('.menu').removeClass('show');
        $('.mask_menu').hide();
        $('.menu-c-inner').removeClass('in').addClass('outer')
        $('.sharebg').show();
    });
}
//关注
function CreateCollect(GuidePage, ele) {
    $(ele).append("<a class=\"a-collect\" href=\"" + GuidePage + "\" id=\"collect-link\" title=\"关注\" style=\"display: block;\"></a>");
}
//搜索
function CreateSearch(ele) {
    $(ele).append("<a class=\"a-server\" href=\"/ProductList.aspx\" title=\"搜索\" style=\"display: block;\"></a>");
}