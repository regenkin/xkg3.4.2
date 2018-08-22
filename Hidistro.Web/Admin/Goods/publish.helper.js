var selectedCategoryId = 0;
var placeHolder; // 分类选择框容器
var nextButton;
var lblFullname;
var productId = "";
var reurl = "";
// 绑定左右移动按钮的单击事件并设置按钮样式
function BindButtonEvents() {
    // 移除事件,防止多重绑定;
    $(".search_right").unbind("click");
    $(".search_right").click(function () {
        if (parseInt(placeHolder.css("left")) == 0) {
            $(".search_left").addClass("search_leftD");
            $(this).removeClass("search_righD");
            placeHolder.animate({ left: "-=222px" }, 350);
        }
    });
    $(".search_left").unbind("click");
    $(".search_left").click(function () {
        if (parseInt(placeHolder.css("left")) == -222) {
            $(".search_right").addClass("search_righD");
            $(this).removeClass("search_leftD");
            placeHolder.animate({ left: "+=222px" }, 350);
        }
    });
}

// 移除当前已选分类直属下级以外的所有子分类选择框
function RemoveSelectors(startIndex) {
    if ($(".results_ol div").length > 1) {
        $(".results_ol div").each(function (x) {
            if (x > startIndex) {
                $(this).remove();
            }
        });
    }
}

function UpdateBoxes(parentCategoryId, classIndex) {
    var itemWidth = 0;
    var classIndex = parseInt(classIndex) + 1;

    if ($(".results_ol div").length > 4) {
        return;
    }

    var categories = GetCategories(parentCategoryId);
    if (categories == null || categories.length == 0)
        return;

    CreateBox(classIndex, categories);

    $(".results_ol div").each(function (i) {
        itemWidth += parseInt($(".results_ol ul")[i].offsetWidth);
    });

    if (itemWidth > 900) {
        if ($(".results_ol div").length > 5 || parseInt(placeHolder.css("left")) == -222) {
            return;
        }

        placeHolder.animate({ left: "-=222px" }, 350);
        $(".search_left").addClass("search_leftD");
        $(".search_right").removeClass("search_righD");
        BindButtonEvents();
    }
}

// 根据指定的classIndex创建一个分类选择框，并使用categories中包含的分类列表填充选择框
function CreateBox(classIndex, categories) {
    var divBox = $("<div class=\"results_z" + classIndex + " results_margin\" classIndex=" + classIndex + "><\/div>");
    var ulBox = $("<ul><\/ul>");
    placeHolder.append(divBox);

    $.each(categories, function (i, category) {
        var item = $("<li id=\"" + category.CategoryId + "\"  hasChildren=\"" + category.HasChildren + "\">" + category.CategoryName + "<\/li>");
        if (category.HasChildren == "true") {
            item.attr("class", "results_n1");
        }
        else {
            item.attr("class", "");
        }

        item.bind("click", function () { ItemClick($(this), classIndex); });
        ulBox.append(item);
    });

    divBox.empty();
    divBox.append(ulBox);
}

// 商品分类单击事件
function ItemClick(obj, classIndex) {
    // 移除当前分类框的所有选中状态
    $.each($(".results_z" + classIndex + " li"), function (i, item) {
        $(item).removeAttr("selected");
        if ($(item).attr("hasChildren") == "false") {
            $(item).attr("class", "");
        }
        else {
            $(item).attr("class", "results_n1");
        }
    });

    obj.attr("selected", "true");
    RemoveSelectors($(".results_z" + classIndex).attr("classIndex"));

    if (obj.attr("hasChildren") == "false") {
        obj.attr("class", "results_s2");
        // 当前选中的是最后一级分类，设置当前已选分类ID
        selectedCategoryId = parseInt(obj.attr("id"));
    }
    else {
        obj.attr("class", "results_s1");
        // 清空已选分类ID
        selectedCategoryId = 0;
        // 当前选中分类有子分类，则显示子分类，并清空当前已选的分类ID
        UpdateBoxes(parseInt(obj.attr("id")), classIndex);
    }

    UpdateStatus();
}

// 根据指定的上级分类ID获取下级分类列表，parentCategoryId==0表示取所有顶级分类
function GetCategories(parentCategoryId) {
    var categories = null;
    var postUrl = "SelectCategory.aspx?isCallback=true&action=getlist&timestamp=";
    postUrl += new Date().getTime() + "&parentCategoryId=" + parentCategoryId;

    $.ajax({
        url: postUrl,
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                categories = resultData.Categories;
            }
        }
    });

    return categories;
}

function UpdateStatus() {
    if (parseInt(selectedCategoryId) > 0) {
        nextButton.attr("disabled", false);
    }
    else {
        nextButton.attr("disabled", true);
    }

    lblFullname.empty();
    var fullname = "";
    var selectedList = $("li[selected=selected]");

    $.each(selectedList, function (i, element) {
        fullname += $(element).html();
        if (i < selectedList.length - 1)
            fullname += "&nbsp;&raquo;&nbsp;";
    });

    lblFullname.html(fullname);
}

function GotoNext() {
    if (selectedCategoryId == 0) {
        // 请先选择一个商品分类
        ShowMsg("请先选择一个商品分类",false);
        return;
    }

    nextButton.attr("disabled", "disabled");
    var formpost = true;
    //var form = document.forms.item(0);
    //form.method = "post";
    //form.action = "productedit.aspx?categoryId=" + selectedCategoryId + "&productId=" + productId;
    //form.action = "productedit.aspx?categoryId=" + selectedCategoryId;
    //        form.submit();
    //if (window.event) {
    //    var IEVersion = GetIEVersion();
    //    if (isNaN(IEVersion) || IEVersion <= 11) { formpost = true; } else { formpost = false; }
    //}
    //else {
    //    formpost = true;
    //}
    if (formpost) {
        formpost = true;
        //var form = document.forms.item(0);
        //form.method = "post";
        //if (productId != "" && productId.length > 0)
        //    form.action = "productedit.aspx?categoryId=" + selectedCategoryId + "&productId=" + productId;
        //else
        //    form.action = "productedit.aspx?categoryId=" + selectedCategoryId;
        //form.submit();

    }

    var goUrl = "productedit.aspx?categoryId=" + selectedCategoryId;
    if (productId != "" && productId.length > 0) {
        goUrl += "&productId=" + productId;
    }
    if (reurl != "" && reurl.length > 0) {
        goUrl += "&reurl=" + reurl;
    }
    location.href = goUrl;

    //if (window.event)
    //    window.event.returnValue = false;//for ie
    //else
    //    event.preventDefault(); //for firefox  
}

// 初始化操作
$(document).ready(function () {
    placeHolder = $(".results_ol");
    nextButton = $("#btnNext");
    lblFullname = $("#fullName");

    nextButton.bind("click", function () { GotoNext(); return false; });
    var mainCategories = GetCategories(0);

    if (mainCategories == null || mainCategories.length == 0) {
        // 没有可选分类
        ShowMsg("\u6CA1\u6709\u53EF\u9009\u5206\u7C7B",false);
        return;
    }
    else {
        CreateBox(0, mainCategories);

        var currentCategoryId = GetQueryString("categoryId");
        if (currentCategoryId != "" && currentCategoryId.length > 0) {
            productId = GetQueryString("ProductId");
            reurl = GetQueryString("reurl");
            LoadState(currentCategoryId);
        }
    }
});

function LoadState(categoryId) {
    $.ajax({
        url: String.format("SelectCategory.aspx?isCallback=true&action=getinfo&timestamp={0}&categoryId={1}", new Date().getTime(), categoryId),
        type: 'GET', dataType: 'json', timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData.Status == "OK") {
                var pathArr = resultData.Path.split("|");
                if (pathArr.length > 0 && SelectItem(pathArr[0], 0)) {
                    for (index = 1; index < pathArr.length; index++) {
                        if (!SelectItem(pathArr[index], index)) {
                            break;
                        }
                    }
                }
            }
        }
    });
}

function SelectItem(categoryId, classIndex) {
    var item = $("li[id=" + categoryId + "]");
    if (item.length == 0) {
        return false;
    }

    ItemClick(item, classIndex);
    return true;
}