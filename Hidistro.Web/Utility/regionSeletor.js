/// <reference path="jquery-1.6.4.min.js" />



function vShop_RegionSelector(containerId, onSelected, defaultRegionText) {
    /// <param name="onSelected" type="function">选择地址后回调,包括两个参数，依次为址址和地址编码</param>

    var regionHandleUrl='/Vshop/RegionHandler.aspx';
    init();
    var address = '';
    var code = 0;


    function init() {
        if (!defaultRegionText)
            defaultRegionText = '请选择省市区';
        var text = '<div class="btn-group bmargin">\
        <button id="address-check-btn" type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">'+defaultRegionText+'<span class="caret"></span></button>\
        <ul name="province" class="dropdown-menu" role="menu"></ul>\
        <ul name="city" class="dropdown-menu hide" role="menu"></ul>\
        <ul name="district" class="dropdown-menu hide" role="menu"></ul>\
        </div>';

        $('#' + containerId).html(text);

        getRegin("province", 0, function (noSub) { bind(noSub); });
    }

    function getRegin(regionType, parentRegionId, callback) {
        /// <param name="regionType" type="String">"province-省,city-市,district-区"</param>
        var text = '';
        
        if (!parentRegionId) {
            parentRegionId = 0;
            address = '';
        }
        jQuery.ajax({
            type: "get",
            async: false,
            url: regionHandleUrl,
            data: { action: 'getregions', parentId: parentRegionId },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            cache: false,
            success: function (data) {
                var noSub = false;
                if (data.Status == 'OK') {
                    $.each(data.Regions, function (i, province) {
                        text += '<li><a href="#" name="' + province.RegionId + '">' + province.RegionName + '</a></li>';
                    });
                    $('#' + containerId + ' ul[name="' + regionType + '"]').html(text);

                }
                else if (data.Status == 0)
                    noSub = true;
                callback(noSub);
            }
        });
    }
    var citycode = "";
    function bind(noSub) {
        $('#' + containerId + ' ul li a').unbind('click');
        $('#' + containerId + ' ul li a').click(function () {
            var currentUl = $(this).parent().parent();
            var regionId = $(this).attr('name');
          
            var nextRegionUl = currentUl.next();
            var prevRegionUl = currentUl.prev();
            var nextRegionType = nextRegionUl ? $(nextRegionUl).attr('name') : '';

            address += $(this).html() + " ";
          
            if (!noSub && nextRegionType) {
                code = $(this).attr('name');
                citycode = code;
                getRegin(nextRegionType, regionId, function (noSub) {
                   
                    currentUl.addClass('hide');
                    if (noSub) {
                      
                        var first = currentUl.parent().find('ul').first();
                        $(first).removeClass('hide');
                        onSelected(address, code, citycode);
                        address = '';
                        bind();
                    }
                    else {
                        nextRegionUl && !noSub && $(nextRegionUl).removeClass('hide');
                        bind();
                        setTimeout(function () {
                            $(".btn-group").addClass('open');
                        }, 1);
                    }

                });
            }
            else {
                var first = currentUl.parent().find('ul').first();
                $(first).removeClass('hide');
                currentUl.addClass('hide');
                code = $(this).attr('name');
                onSelected(address, code, citycode);
                address = '';

            }
        });
    }
} 