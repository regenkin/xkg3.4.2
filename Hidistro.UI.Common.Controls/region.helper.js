$(document).ready(function() {
    $("select[selectset=regions]").bind("change", function() { ChooiceRegion($(this).attr("id"), $(this).val()); });
});

function ChooiceRegion(currentControlId, selectedRegionId) {
    var depth = GetDepthBySelectId(currentControlId);
    var hasvalue = (selectedRegionId != null && selectedRegionId.length > 0);

    // 更新当前选择的地区
    if (hasvalue)
        $("#regionSelectorValue").val(selectedRegionId);
    else {
        if (depth == 1)
            $("#regionSelectorValue").val("");
        else
            $("#regionSelectorValue").val($("#ddlRegions" + (depth - 1)).val());
    }

    // 重置所有子区域的显示
    var subDepth = depth+1;
    var subSelector = $("#ddlRegions" + subDepth);

    while (subSelector.length > 0) {
        ResetSelector(subSelector, subDepth);        
        subDepth++;
        subSelector = $("#ddlRegions" + subDepth);
    }

    var haschild = (subDepth > (depth + 1));

    // 更新直接子区域的内容
    if (hasvalue && haschild) {
        FillSelector(selectedRegionId, $("#ddlRegions" + (depth + 1)), "");
    }
}

function GetDepthBySelectId(currentControlId) {
    return eval(currentControlId.replace("ddlRegions", ""));
}

// 手工设置当前要选中的地区
function ResetSelectedRegion(regionId) {
    $.ajax({
        url: "RegionHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "getregioninfo", regionId: regionId },
        success: function(resultData) {
            if (resultData.Status != "OK")
                return;

            var depth = parseInt(resultData.Depth);
            if (depth == 1) {
                $("#ddlRegions1").val(resultData.RegionId);
                $("#ddlRegions2").val(null);
                $("#ddlRegions3").val(null);
            }

            else {
                var pathArr = resultData.Path.split(",");
                $("#ddlRegions1").val(pathArr[0]);

                for (index = 1; index < pathArr.length && index < depth; index++) {
                    var selector = $("#ddlRegions" + (index + 1));
                    ResetSelector(selector, depth);
                    FillSelector(pathArr[index - 1], selector, pathArr[index]);
                    if (depth == 2) {
                        $("#ddlRegions3").val(null);
                    }
                }
            }

            $("#regionSelectorValue").val(resultData.RegionId);
        }
    });
}

// 重置指定的下拉选择框
function ResetSelector(selector, depth) {
    $(selector).find("option").remove();
    if (depth == 2)
        $(selector).append("<option value=\"\">" + "-请选择市-" + "</option>");
    else if(depth == 3)
        $(selector).append("<option value=\"\">" + "-请选择区-" + "</option>");
}

// 根据指定的父地区编号填充地区下拉框的可选内容
function FillSelector(parentId, selector, selectedValue) {
    $.ajax({
        url: "RegionHandler.aspx",
        type: 'post', dataType: 'json', timeout: 10000,
        data: { action: "getregions", parentId: parentId },
        success: function(resultData) {
            if (resultData.Status == "OK") {
                $.each(resultData.Regions, function(i, region) {
                    var option = document.createElement("option");
                    option.text = region.RegionName;
                    option.value = region.RegionId;
                    $(selector)[0].options.add(option);
                });

                if (selectedValue.length > 0) {
                    $(selector).val(selectedValue);
                }
            }
        }
    });
}

// 获取当前选择的地区编号
function GetSelectedRegionId() {
    return $("#regionSelectorValue").val();
}