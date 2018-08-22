/// <reference path="common.js" />

function vshopPager(containerId, maxSize, defaultPageNumber, defaultPageSize) {
    /// <param name="containerId" type="String">pager容器id</param>
    /// <param name="defaultPageNumber" type="int">当前页码</param>
    /// <param name="maxSize" type="int">最大内容数目</param>
    /// <param name="defaultPageSize" type="int">单页最大行数</param>

    var searchString = location.search.toString() + '&';
    var queryString = searchString.substring(1, searchString.length - 1);
    var params = queryString.split('&');
    var url = '?';
    var paramName;
    $.each(params, function (i, param) {
        paramName = $.trim(param.split('=')[0]);
        if (paramName && paramName != 'page' && paramName != 'size')
            url += param + '&';
    });


    var page = getParam('page');
    if (!page)
        page = defaultPageNumber;
    page = parseInt(page);

    var size = getParam('size');
    if (!size) {
        size = defaultPageSize;
    }

    size = parseInt(size);

    maxSize = parseInt(maxSize);

    var nextPage = parseInt(page) + 1;
    var prePage = parseInt(page) - 1;
    if (prePage < 1)
        prePage = 1;

    var preUrl, nextUrl, preDisabled, nextDisabled;
    preUrl = nextUrl = 'javascript:;';
    if (page == 1)
        preDisabled = 'disabled';
    else {
        preUrl = url + 'page=' + prePage + '&size=' + size;
    }
    if (page == Math.ceil(maxSize * 1.0 / size))
        nextDisabled = 'disabled';
    else
        nextUrl = url + 'page=' + nextPage + '&size=' + size;


    var html = ' <ul class="pager">\
            <li class="previous ' + preDisabled + '"><a href="' + preUrl + '">上一页</a></li>\
            <li class="next ' + nextDisabled + '"><a href="' + nextUrl + '">下一页</a></li>\
        </ul>';

    if (maxSize > defaultPageSize)
        $('#' + containerId).html(html);
}