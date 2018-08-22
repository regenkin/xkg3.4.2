$(function () {
    //添加一个模块
    $(".drag-module-lists li").click(function () {
        //alert("asdasd");
        var type = $(this).data("type"); //获取模块类型

        var  data = dataInit(parseInt(type));
        //添加手机内容
        if (data.type != 5) {
            insertDom(data);
        } else {
            HiShop.Goods.GoodsList(data);
        }
    });

    //初始化布局拖动事件
    $("#diy-phone .drag").sortable({
        revert: true,
        placeholder: "drag-highlight",
        stop: function (event, ui) {
            HiShop.DIY.repositionCtrl(ui.item, $(".diy-ctrl-item[data-origin='item']")); //重置ctrl的位置
        }
    }).disableSelection();

    //编辑页面标题
    $(".j-pagetitle").click(function () {
        $(".diy-ctrl-item[data-origin='pagetitle']").show().siblings(".diy-ctrl-item[data-origin='item']").hide();
    });

    //编辑页面标题同步到手机视图中
    $(".j-pagetitle-ipt").change(function () {
        $(".j-pagetitle").text($(this).val());
    });
});