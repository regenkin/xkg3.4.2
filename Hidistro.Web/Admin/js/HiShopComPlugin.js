/*!
 * HiShop 选择链接插件 
 * version: 1.0
 * build: 2015/9/25
 * author: CJZhao
 */

/*!
 *创建链接插件
 *ele:获取返回值的dom，请传入jqueryobj类型
 *obj:创建在那个dom下（请传入jquery obj类型）
 *parameter:参数
 *parameter.createType 构造类型  1：在 obj 下追加 2：插入obj之前 3：插入到obj 之后
 *parameter.showTitle 是否显示“链接”两字
 *parameter.txtContinuity 返回值写入到ele中 false：清空再写 true：追加内容
 *parameter.reWriteSpan 是否将选中的内容  展示出来
 *parameter.iscallback 是否自定义回调
 *parameter.callback  回调函数
 *parameter.style DIV样式
 */
function CreateDropdown(ele, obj, parameter)
{
    var tempHtml = "    <div class=\"form-group\" id =\"dropdow-menu-link\" style=\"" + parameter.style + "\" >\
                                <div class=\"control-action clearfix\">\
                                 <div class=\"pull-left js-link-to link-to\">";
    if (parameter.showTitle) {
        tempHtml+=   "<label ><em>&nbsp;</em>链接：</label>";
    }
    tempHtml+=                      "<div class=\"dropdown hover\">\
                                        <a class=\"dropdown-toggle\" data-toggle=\"dropdown\" href=\"javascript:void(0);\"><span id=\"spLinkTitle\">选择链接</span> <i class=\"caret\"></i></a>\
                                        <ul class=\"dropdown-menu\">\
                                            <li data-val=\"1\"><a href=\"javascript:;\"><#= HiShop.linkType[1] #></a></li>\
                                            <li data-val=\"2\"><a href=\"javascript:;\"><#= HiShop.linkType[2] #></a></li>\
                                            <li data-val=\"3\"><a href=\"javascript:;\"><#= HiShop.linkType[3] #></a></li>\
                                            <li data-val=\"18\"><a href=\"javascript:;\"><#= HiShop.linkType[18] #></a></li>\
                                            <li data-val=\"4\"><a href=\"javascript:;\"><#= HiShop.linkType[4] #></a></li>\
                                            <li data-val=\"5\"><a href=\"javascript:;\"><#= HiShop.linkType[5] #></a></li>\
                                            <li data-val=\"21\"><a href=\"javascript:;\"><#= HiShop.linkType[21] #></a></li>\
                                            <li data-val=\"22\"><a href=\"javascript:;\"><#= HiShop.linkType[22] #></a></li>\
                                            <li data-val=\"6\"><a href=\"javascript:;\"><#= HiShop.linkType[6] #></a></li>\
                                            <li data-val=\"17\"><a href=\"javascript:;\"><#= HiShop.linkType[17] #></a></li>\
                                            <li data-val=\"7\"><a href=\"javascript:;\"><#= HiShop.linkType[7] #></a></li>\
                                            <li data-val=\"8\"><a href=\"javascript:;\"><#= HiShop.linkType[8] #></a></li>\
                                            <li data-val=\"16\"><a href=\"javascript:;\"><#= HiShop.linkType[16] #></a></li>\
                                            <li data-val=\"10\"><a href=\"javascript:;\"><#= HiShop.linkType[10] #></a></li>\
                                            <li data-val=\"9\"><a href=\"javascript:;\"><#= HiShop.linkType[9] #></a></li>\
                                            <li data-val=\"11\"><a href=\"javascript:;\"><#= HiShop.linkType[11] #></a></li>\
                                            <li data-val=\"20\"><a href=\"javascript:;\"><#= HiShop.linkType[20] #></a></li>\
                                            <li data-val=\"13\"><a href=\"javascript:;\"><#= HiShop.linkType[13] #></a></li>\
                                            <li data-val=\"14\"><a href=\"javascript:;\"><#= HiShop.linkType[14] #></a></li>\
                                            <li data-val=\"15\"><a href=\"javascript:;\"><#= HiShop.linkType[15] #></a></li>\
                                            <li data-val=\"19\"><a href=\"javascript:;\"><#= HiShop.linkType[19] #></a></li>\
                                        </ul>\
                                    </div>\
                                    </div>\
                                    </div>\
                            </div> ";
    if (parameter.createType == 1) {
        obj.append(_.template(tempHtml));
    } else if (parameter.createType == 2) {
        obj.before(_.template(tempHtml));
    } else if (parameter.createType == 3) {
        obj.after(_.template(tempHtml));
    }
    if (parameter.callback == undefined) {
        parameter.callback = function () {
            // var index = $(this).parents("li.ctrl-item-list-li").index();
            //alert($(this).data("val"));
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    //ele.show();
                    var link = item.link;
                    if (link.indexOf('http') > -1) {
                        link = item.link;
                    } else {
                        link = "http://" + window.location.host + item.link;
                    }
                    if (parameter.txtContinuity) {
                        if (type == 11) {
                            ele.val(ele.val() + "  " + item.link);
                        } else {
                            ele.val(ele.val() + "  " + link);
                        }
                    } else {
                        if (type == 11) {
                            ele.val( item.link);
                        } else {
                            ele.val( link);
                        }
                    }
                    if (parameter.reWriteSpan) {
                        $("#spLinkTitle").html(item.title);
                    }
                    if (type == 20) {
                        ele.hide();
                        ele.val("")
                    }
               
                }
            });
        }
    }
    $("#dropdow-menu-link li").unbind("click");
    $("#dropdow-menu-link li").click(parameter.callback);
    $('body').on("click", ".selectitem", function () {
        var link = $(this).attr("datalink");
        if (link.indexOf('http') > -1) {
            link = link;
        } else {
            link = "http://" + window.location.host + link;
        }
        //赋值
        $("#txtshowContent").val(link);
        $("#txtContent").val(link);
        $("#urlData").val(link).show();//单图文多图文
        if (parameter.reWriteSpan) {
            $("#spLinkTitle").html($(this).attr("title"));//赋值
        }
        //触发关闭窗口事件
        $('.jbox-close').click();
    });
    $('body').on('click', '.modalshopclasslist .oneclass', function () {
        if ($(this).next().css('display') == 'none') {
            $(this).next().slideDown();
            $(this).find('i').addClass('down');
        } else {
            if ($(this).next().get(0)) {
                $(this).next().slideUp();
                $(this).find('i').removeClass('down');
            }
        }
    });
    $('.modalshopclasslist .oneclass button').click(function (evt) {
        evt.stopPropagation();
    })
}

function tpl_albums_main  () {
        var tempHtml = " <div id=\"albums\">\
            <div class=\"albums-title clearfix\">\
                <span class=\"fl\">我的图库</span>\
                <a href=\"javascript:;\" class=\"fr\" id=\"j-close\" title=\"关闭\"><i class=\"gicon-remove\"></i></a>\
            </div>\
            <div class=\"albums-container clearfix\">\
                <div class=\"albums-cl fl\">\
                    <div class=\"albums-cl-actions\">\
                        <a href=\"javascript:;\" id=\"j-addFolder\"><i class=\"gicon-plus\"></i><span>添加</span></a>\
                        <a href=\"javascript:;\" id=\"j-renameFolder\"><i class=\"gicon-pencil\"></i><span>重命名</span></a>\
                        <a href=\"javascript:;\" id=\"j-delFolder\"><i class=\"gicon-trash\"></i><span>删除</span></a>\
                    </div>\
                    <div class=\"albums-cl-tree\" id=\"j-panelTree\">\
                        <p class=\"txtCenter pdt10 loading j-loading\"><i class=\"icon-loading\"></i></p>\
                    </div>\
                </div>\
                <div class=\"albums-cr fl\">\<div class=\"mate-tabl\">\
                    <ul class=\"nav nav-tabs\" role=\"tablist\" style=\"position: relative;\">\
                        <li role=\"presentation\" type=\"0\"><a href=\"javascript:void(0)\">图片库</a></li>\
                        <li role=\"presentation\" type=\"1\"><a href=\"javascript:void(0)\">图标库</a></li>\
                        <li role=\"presentation\" type=\"3\"><a href=\"javascript:void(0)\">商品图片</a></li>\
                        <span style=\"margin-top: 10px; display: inline-block; margin-left: 30px; color: red;\">双击图片可直接选择</span>\
                    </ul></div>\
                    <div class=\"albums-cr-actions\">\
                        <input type=\"file\" name=\"imgpicker_upload_input\" id=\"j-addImg\">\
                        <a href=\"javascript:;\" id=\"j-moveImg\" class=\"btn btn-primary mgl5\">移动图片到</a>\
                        <a href=\"javascript:;\" id=\"j-cateImg\" class=\"btn btn-primary mgl5\">移动分类图片到</a>\
                        <a href=\"javascript:;\" id=\"j-delImg\" class=\"btn btn-danger mgl5 mgr5\">删除所选图片</a>\
                        <a href=\"javascript:;\" id=\"j-useImg\" class=\"btn btn-success\">使用选中的图片</a>\
        <div class=\"fr\">\
                        <input type=\"text\" placeholder=\"请输入图片名称\" style=\"width: 150px;margin:4px;padding:0px;vertical-align: 0;border-radius: 2px;border: 1px solid #ccc;font-size:12px;line-height:22px;\"><a href=\"javascript:;\" id=\"j-searchImg\" class=\"btn btn-primary mgl10 searchImg\">搜索</a></div>\
                    </div>\
                    <div class=\"albums-cr-imgs\" id=\"j-panelImgs\" style=\"height:350px;\">\
                        <p class=\"txtCenter pdt10 loading j-loading\"><i class=\"icon-loading\"></i></p>\
                    </div>\
                    <div class=\"albums-cr-ctrls clearfix\">\
                        <div class=\"paginate fr\" id=\"j-panelPaginate\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>";
        return tempHtml;
    }

function tpl_albums_overlay() {
        var tempHtml = " <div id=\"albums-overlay\"></div>";
        return tempHtml;
    }

function tpl_albums_tree() {
        var tempHtml = " <dl class=\"j-albumsNodes\">\
            <dt data-id=\"-1\" data-add=\"1\" data-rename=\"0\" data-del=\"0\">\
                <i class=\"icon-folder open\"></i>\
                <span class=\"j-treeShowTxt\"><em class=\"j-name\">所有图片</em>(<em class=\"j-num\"><#=dataset.total#></em>)</span>\
            </dt>\
            <dd><#=nodes#></dd>\
        </dl>";
        return tempHtml;
    }
function tpl_albums_tree_fn () {
        var tempHtml = "     <# _.each(dataset,function(item){#>\
        <dl>\
            <#if(item.id==0){#>\
            <dt data-id=\"<#=item.id#>\" data-add=\"0\" data-rename=\"0\" data-del=\"0\">\
                <#}else{#>\
            <dt data-id=\"<#=item.id#>\" data-add=\"1\" data-rename=\"1\" data-del=\"1\">\
                <#}#>\
                <#if(item.subFolder && item.subFolder.length){#>\
                <i class=\"icon-folder open\"></i>\
                <#}else{#>\
                <i class=\"icon-folder\"></i>\
                <#}#>\
                <span class=\"j-treeShowTxt\"><em class=\"j-name\"><#=item.name#></em>(<em class=\"j-num\"><#=item.picNum#></em>)</span>\
                <#if(item.id!=0){#>\
                <input type=\"text\" class=\"ipt j-ip\" maxlength=\"10\" value=\"<#=item.name#>\"><i class=\"icon-loading j-loading\"></i>\
                <#}#>\
            </dt>\
            <dd>\
                <#if(item.subFolder && item.subFolder.length){#>\
                <#= templateFn({dataset:item.subFolder, templateFn:templateFn}) #>\
                <#}#>\
            </dd>\
        </dl>\
        <#})#>";
        return tempHtml;
    }

function tpl_albums_delFolder  () {
        var tempHtml = "  <div>\
            <p class=\"ftsize14 bold\">删除该文件夹同时会删除其子文件夹，是否继续？</p>\
            <div class=\"radio-group mgt5\">\
                <label><input type=\"radio\" name=\"isDelImgs\" value=\"1\" checked>不删除图片</label>\
                <label><input type=\"radio\" name=\"isDelImgs\" value=\"2\">同时删除图片</label>\
            </div>\
        </div>";
        return tempHtml;
    }
function tpl_albums_imgs () {
        var tempHtml = "   <#if(dataset){#>\
        <ul class=\"clearfix\">\
            <# _.each(dataset,function(item,index){ #>\
            <li class=\"fl\" data-id=\"<#=item.id#>\">\
                <img src=\"<#=item.file#>\" title=\"<#=item.name#>\">\
                <div class=\"albums-cr-imgs-selected\"><i></i></div>\
                <div class=\"albums-edit\">\
                    <span><i class=\"gicon-pencil edit-img-name\"></span></i>\
                    <p><#=item.name#></p>\
                    <div class=\"img-name-edit\">\
                        <input type=\"text\" value=\"<#=item.name#>\" style=\"width:60%;height: 22px;\" name=\"rename\" class=\"file_name\" />\
                        <a href=\"javascript:;\" class=\"renameImg\">确定</a>\
                    </div>\
                </div>\
            </li>\
            <# }) #>\
        </ul>\
        <#}else{#>\
        <p class=\"albums-cr-imgs-noPic j-noPic\">暂无图片</p>\
        <#}#>";
        return tempHtml;
    }

    function tpl_qrcode() {
        var tempHtml = "      <div id=\"qrcode\">\
            <img src=\"<#= src #>\">\
            <a href=\"javascript:;\" class=\"qrcode-btn j-closeQrcode\"><i class=\"gicon-remove white\"></i></a>\
        </div>";
        return tempHtml;
    }
    function tpl_popbox_ImgPicker  () {
        var tempHtml = " <div id=\"ImgPicker\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"imgpicker\" data-index=\"1\">选择图片</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-initupload\" data-origin=\"imgpicker\" data-index=\"2\">上传新图片</a>\
            </div>\
            <!-- end tabs-->\
            <div class=\"tabs-content\" data-origin=\"imgpicker\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"img-list imgpicker-list clearfix\"></ul>\
                    <!-- end img-list -->\
                    <div class=\"imgpicker-actionPanel clearfix\">\
                        <div class=\"fl\">\
                            <a href=\"javascript:;\" class=\"btn btn-primary\" id=\"j-btn-listuse\">使用选中图片</a>\
                        </div>\
                        <div class=\"fr\">\
                            <div class=\"paginate\"></div>\
                        </div>\
                    </div>\
                    <!-- end imgpicker-actionPanel -->\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <div class=\"uploadifyPanel clearfix\">\
                        <ul class=\"img-list imgpicker-upload-preview\"></ul>\
                        <input type=\"file\" name=\"imgpicker_upload_input\" id=\"imgpicker_upload_input\">\
                    </div>\
                    <div class=\"imgpicker-actionPanel\">\
                        <a href=\"javascript:;\" class=\"btn btn-primary\" id=\"j-btn-uploaduse\">使用上传的图片</a>\
                    </div>\
                    <!-- end imgpicker-actionPanel -->\
                </div>\
            </div>\
            <!-- end tabs-content -->\
        </div>";
        return tempHtml;
    }
    function tpl_popbox_ImgPicker_listItem  () {
        var tempHtml = "   <# _.each(dataset,function(url){ #>\
        <li>\
            <span class=\"img-list-overlay\"><i class=\"img-list-overlay-check\"></i></span>\
            <img src=\"<#= url #>\">\
        </li>\
        <# }) #>";
        return tempHtml;
    }

    function tpl_popbox_ImgPicker_listItem () {
        var tempHtml = "   <li>\
            <span class=\"img-list-btndel j-imgpicker-upload-btndel\"><i class=\"gicon-trash white\"></i></span>\
            <span class=\"img-list-overlay\"></span>\
            <img src=\"<#= url #>\">\
        </li>"
        return tempHtml;
    }

    function icon_imgPicker() {
        var tempHtml = " <div id=\"icon-container\">\
            <div class=\"albums-title clearfix\">\
                <span class=\"fl\">选择图片</span>\
                <a href=\"javascript:;\" class=\"fr\" id=\"Jclose\" title=\"关闭\">\
                    <i class=\"gicon-remove\"></i>\
                </a>\
            </div>\
            <div class=\"albums-container\">\
                <div class=\"albums-cr-actions noborder\">\
                    <a href=\"javascript:;\" data-style=\"style1\" class=\"btn btn-primary mgl10 cur\">风格一<i></i></a>\
                    <a href=\"javascript:;\" data-style=\"style2\" class=\"btn btn-primary mgl10\">风格二<i></i></a>\
                    <a href=\"javascript:;\" data-style=\"style3\" class=\"btn btn-primary mgl10\">风格三<i></i></a>\
                </div>\
                <div class=\"albums-color-tab\">\
                    <h2><a href=\"javascript:;\" class=\"btn btn-primary mgl10\">选择颜色</a><span>(小图标下面的文字仅供参考,背景色可自行修改)</span></h2>\
                    <ul class=\"clearfix\">\
                        <li data-color=\"color0\"><span class=\"color color0\"></span><span>黑色</span></li>\
                        <li data-color=\"color1\"><span class=\"color color1\"></span><span>白色</span></li>\
                        <li data-color=\"color2\"><span class=\"color color2\"></span><span>灰色</span></li>\
                        <li data-color=\"color3\"><span class=\"color color3\"></span><span>红色</span></li>\
                        <li data-color=\"color4\"><span class=\"color color4\"></span><span>黄色</span></li>\
                        <li data-color=\"color5\"><span class=\"color color6\"></span><span>蓝色</span></li>\
                        <li data-color=\"color6\"><span class=\"color color5\"></span><span>绿色</span></li>\
                        <li data-color=\"color7\"><span class=\"color color7\"></span><span>紫色</span></li>\
        <li data-color=\"color8\"><span class=\"color color8\"></span><span>橙色</span></li>\
    </ul>\
                </div>\
                <div class=\"albums-icon-tab\"></div>\
                <div class=\"albums-cr-ctrls clearfix\">\
                    <a href=\"javascript:;\" id=\"j-useIcon\" class=\"btn btn-primary fl\">使用选中的图片</a>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }


    function icon_imglist () {
        var tempHtml = "       <ul class=\"clearfix\">\
            <# _.each(data,function(item){ #>\
            <li><img src=\"<#= item #>\" width=\"80\" alt=\"\"><span><i></i></span></li>\
            <# }) #>\
        </ul>"
        return tempHtml;
    }

    function tpl_popbox_GoodsAndGroupPicker () {
        var tempHtml = "<div id=\"GoodsAndGroupPicker\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"goodsandgroup\" data-index=\"1\">商品</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-group\" data-origin=\"goodsandgroup\" data-index=\"2\">商品分组</a>\
            </div>\
            <!-- end tabs -->\
            <div class=\"tabs-content\" data-origin=\"goodsandgroup\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"gagp-goodslist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"fl\">\
                            <a href=\"javascript:;\" class=\"btn btn-primary j-btn-goodsuse\">确定使用</a>\
                        </div>\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"gagp-grouplist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                            <input type=\"hidden\" class=\"j-verify\" name=\"item_id\" value=\"\">\
                            <span class=\"fi-help-text j-verify-linkType\"></span>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_GoodsAndGroupPicker_goodsitem () {
        var tempHtml = "   <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\" data-item=\"<#= data.item_id #>\">\
            <a href=\"<#= data.link #>\" class=\"fl\" target=\"_blank\" title=\"<#= data.title #>\">\
                <div class=\"table-item-img\">\
                    <# if(data.is_compress){ #>\
                    <img src=\"<#= data.pic #>80x80\" alt=\"<#= data.title #>\">\
                    <# }else{ #>\
                    <img src=\"<#= data.pic #>\" alt=\"<#= data.title #>\">\
                    <# } #>\
                </div>\
                <div class=\"table-item-info\">\
                    <p><#= data.title #></p>\
                    <span class=\"price\">&yen;<#= data.price #></span>\
                </div>\
            </a>\
            <a href=\"javascript:;\" class=\"btn fr j-select mgt15 mgr15\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }

    function tpl_popbox_GoodsAndGroupPicker_groupitem () {
        var tempHtml = "    <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\" data-group=\"<#= data.group_id #>\">\
            <a href=\"<#= data.link #>\" class=\"fl a_hover\" target=\"_blank\" title=\"<#= data.title #>\"><#= data.title #></a>\
            <a href=\"javascript:;\" class=\"btn fr j-select\">选取</a>\
        </li>\
        <# }) #>";
        return tempHtml;
    }


    function tpl_popbox_MgzAndMgzCate () {
        var tempHtml = "<div id=\"MgzAndMgzCate\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"MgzAndMgzCate\" data-index=\"1\">专题页面</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-mgzcate\" data-origin=\"MgzAndMgzCate\" data-index=\"2\">页面分类</a>\
            </div>\
            <!-- end tabs -->\
            <div class=\"tabs-content\" data-origin=\"MgzAndMgzCate\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"mgz-list mgz-list-panel1\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"mgz-list mgz-list-panel2\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"fl\">\
                            <a href=\"javascript:;\" class=\"btn btn-primary j-btn-use\">确定使用</a>\
                        </div>\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_MgzAndMgzCate_item  () {
        var tempHtml = "    <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\">\
            <a href=\"<#= data.link #>\" class=\"fl a_hover\" target=\"_blank\" title=\"<#= data.title #>\"><#= data.title #></a>\
            <a href=\"javascript:;\" class=\"btn fr j-select\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }

    function tpl_popbox_GamePicker () {
        var tempHtml = "<div id=\"GamePicker\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"GamePicker\" data-index=\"1\">幸运大转盘</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-game\" data-origin=\"GamePicker\" data-index=\"2\">疯狂砸金蛋</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-game\" data-origin=\"GamePicker\" data-index=\"3\">好运翻翻看</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-game\" data-origin=\"GamePicker\" data-index=\"4\">大富翁</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-game\" data-origin=\"GamePicker\" data-index=\"5\">刮刮乐</a>\
            </div>\
            <!-- end tabs -->\
            <div class=\"tabs-content\" data-origin=\"GamePicker\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"game-list game-list-panel1\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"game-list game-list-panel2\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"3\">\
                    <ul class=\"game-list game-list-panel3\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"4\">\
                    <ul class=\"game-list game-list-panel4\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"5\">\
                    <ul class=\"game-list game-list-panel5\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_GamePicker_item  () {
        var tempHtml = "<# _.each(dataset,function(data){#>\
        <li class=\"clearfix\">\
            <a href=\"<#= data.link #>\" class=\"fl a_hover\" target=\"_blank\" title=\"<#= data.title #>\"><#= data.title #></a>\
            <a href=\"javascript:;\" class=\"btn fr j-select\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }
    function tpl_popbox_CouponPicker  () {
        var tempHtml = " <div id=\"GamePicker\">\
            <div class=\"tabs-content\" data-origin=\"GamePicker\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"game-list game-list-panel1\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"game-list game-list-panel2\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"3\">\
                    <ul class=\"game-list game-list-panel3\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"4\">\
                    <ul class=\"game-list game-list-panel4\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_CouponPicker_item  () {
        var tempHtml = "  <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\">\
            <a href=\"<#= data.link #>\" class=\"fl a_hover\" target=\"_blank\" title=\"<#= data.title #>\"><#= data.title #></a>\
            <a href=\"javascript:;\" class=\"btn fr j-select\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }

    function tpl_popbox_PointExchangePicker  () {
        var tempHtml = " <div id=\"GamePicker\">\
            <div class=\"tabs-content\" data-origin=\"GamePicker\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"game-list game-list-panel1\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"game-list game-list-panel2\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"3\">\
                    <ul class=\"game-list game-list-panel3\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"4\">\
                    <ul class=\"game-list game-list-panel4\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_PointExchangePicker_item  () {
        var tempHtml = "    <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\">\
            <a href=\"<#= data.link #>\" class=\"fl a_hover\" target=\"_blank\" title=\"<#= data.title #>\"><#= data.title #></a>\
            <a href=\"javascript:;\" class=\"btn fr j-select\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }
    function tpl_popbox_VotePicker  () {
        var tempHtml = " <div id=\"GamePicker\">\
            <div class=\"tabs-content\" data-origin=\"GamePicker\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"game-list game-list-panel1\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"game-list game-list-panel2\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"3\">\
                    <ul class=\"game-list game-list-panel3\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"4\">\
                    <ul class=\"game-list game-list-panel4\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_VotePicker_item () {
        var tempHtml = "     <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\">\
            <a href=\"<#= data.link #>\" class=\"fl a_hover\" target=\"_blank\" title=\"<#= data.title #>\"><#= data.title #></a>\
            <a href=\"javascript:;\" class=\"btn fr j-select\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }
    function tpl_popbox_GraphicPicker () {
        var tempHtml = "  <div id=\"GoodsAndGroupPicker\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"goodsandgroup\" data-index=\"1\">商品</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-group\" data-origin=\"goodsandgroup\" data-index=\"2\">商品分组</a>\
            </div>\
            <!-- end tabs -->\
            <div class=\"tabs-content\" data-origin=\"goodsandgroup\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"gagp-goodslist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"fl\">\
                            <a href=\"javascript:;\" class=\"btn btn-primary j-btn-goodsuse\">确定使用</a>\
                        </div>\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
             <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"gagp-grouplist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                            <input type=\"hidden\" class=\"j-verify\" name=\"item_id\" value=\"\">\
                            <span class=\"fi-help-text j-verify-linkType\"></span>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }
    function tpl_popbox_GraphicPicker_graphicsitem  () {
        var tempHtml = "     <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\" data-item=\"<#= data.item_id #>\">\
            <a href=\"<#= data.link #>\" class=\"fl\" target=\"_blank\" title=\"<#= data.title #>\">\
                <div class=\"table-item-img\">\
                    <# if(data.is_compress){ #>\
                    <img src=\"<#= data.pic #>80x80\" alt=\"<#= data.title #>\">\
                    <# }else{ #>\
                    <img src=\"<#= data.pic #>\" alt=\"<#= data.title #>\">\
                    <# } #>\
                </div>\
                <div class=\"table-item-info\">\
                    <p><#= data.title #></p>\
                </div>\
            </a>\
            <a href=\"javascript:;\" class=\"btn fr j-select mgt15 mgr15\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }
    function tpl_popbox_CategoriesPicker () {
        var tempHtml = "  <div id=\"GoodsAndGroupPicker\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"goodsandgroup\" data-index=\"1\">商品</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-group\" data-origin=\"goodsandgroup\" data-index=\"2\">商品分组</a>\
            </div>\
            <!-- end tabs -->\
            <div class=\"tabs-content\" data-origin=\"goodsandgroup\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"gagp-goodslist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"fl\">\
                            <a href=\"javascript:;\" class=\"btn btn-primary j-btn-goodsuse\">确定使用</a>\
                        </div>\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
             <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"gagp-grouplist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                            <input type=\"hidden\" class=\"j-verify\" name=\"item_id\" value=\"\">\
                            <span class=\"fi-help-text j-verify-linkType\"></span>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    //function tpl_popbox_CategoriesPicker_graphicsitem () {
    //    var tempHtml = "   <# _.each(dataset,function(data){#>\
    //    <li class=\"clearfix\" data-item=\"<#= data.item_id #>\">\
    //        <a href=\"<#= data.link #>\" class=\"fl\" target=\"_blank\" title=\"<#= data.title #>\">\
    //        <div class=\"table-item-info\">\
    //                <p><#= data.title #></p>\
    //            </div>\
    //        </a>\
    //        <a href=\"javascript:;\" class=\"btn fr j-select mgt15 mgr15\">选取</a>\
    //    </li>\
    //    <# }) #>"
    //    return tempHtml;
    //}

    function tpl_popbox_CategoriesPicker_graphicsitem() {
        var tempHtml = " <div class=\"modal-body\" style=\"padding:0;\"> \
          <div class=\"modalshopclasslistbox\">\
        <# _.each(dataset,function(data){#>\
          <div class=\"modalshopclasslist\">\
              <div class=\"oneclass clearfix\" >\
                 <p class=\"fl\" >\
                     <# if(data.children!=\"\"){  #>\
                       <i>\</i> \
                     <# } #>\
                    <# if(data.children==\"\"){  #>\
                       <i class=\"down\" >\</i> \
                    <# } #>\
                <#= data.title #></p>\
                 <p class=\"fr\"><a href=\"javascript:;\" class=\"btn btn-primary btn-sm mr30 selectitem\" title=\"<#= data.title #>\" datalink=\"<#= data.link #>\">选取</a></p>\
              </div>\
                <# if(data.children!=\"\"){  #>\
                <div class=\"twoclass\">\
                      <ul>\
                       <# _.each(data.children,function(child){#>\
                           <li> <p class=\"fl\"><#= child.title #></p><p class=\"fr\"><a href=\"javascript:;\" style=\"width:50px\" class=\"btn btn-primary btn-sm mr30 selectitem\" title=\"<#= child.title #>\" datalink=\"<#= child.link #>\">选取</a></p></li>\
                       <# }) #>\
                     </ul>\
                </div>\
                 <# } #>\
          </div>\
        <# }) #>\
        </div></div><div class=\"modal-footer\" style=\"border:none;\"></div>"
        return tempHtml;
    }

    function tpl_popbox_BrandsPicker () {
        var tempHtml = "   <div id=\"GoodsAndGroupPicker\">\
            <div class=\"tabs clearfix\">\
                <a href=\"javascript:;\" class=\"active tabs_a fl\" data-origin=\"goodsandgroup\" data-index=\"1\">商品</a>\
                <a href=\"javascript:;\" class=\"tabs_a fl j-tab-group\" data-origin=\"goodsandgroup\" data-index=\"2\">商品分组</a>\
            </div>\
            <!-- end tabs -->\
            <div class=\"tabs-content\" data-origin=\"goodsandgroup\">\
                <div class=\"tc\" data-index=\"1\">\
                    <ul class=\"gagp-goodslist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"fl\">\
                            <a href=\"javascript:;\" class=\"btn btn-primary j-btn-goodsuse\">确定使用</a>\
                        </div>\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
                <div class=\"tc hide\" data-index=\"2\">\
                    <ul class=\"gagp-grouplist\"></ul>\
                    <div class=\"clearfix mgt10\">\
                        <div class=\"paginate fr\"></div>\
                    </div>\
                </div>\
            </div>\
        </div>"
        return tempHtml;
    }

    function tpl_popbox_BrandsPicker_graphicsitem () {
        var tempHtml = "  <# _.each(dataset,function(data){#>\
        <li class=\"clearfix\" data-item=\"<#= data.item_id #>\">\
            <a href=\"<#= data.link #>\" class=\"fl\" target=\"_blank\" title=\"<#= data.title #>\">\
             <div class=\"table-item-img\">\
                    <# if(data.is_compress){ #>\
                    <img src=\"<#= data.pic #>80x80\" alt=\"<#= data.title #>\">\
                    <# }else{ #>\
                    <img src=\"<#= data.pic #>\" alt=\"<#= data.title #>\">\
                    <# } #>\
                </div>\
                <div class=\"table-item-info\">\
                    <p><#= data.title #></p>\
                </div>\
            </a>\
            <a href=\"javascript:;\" class=\"btn fr j-select mgt15 mgr15\">选取</a>\
        </li>\
        <# }) #>"
        return tempHtml;
    }
    function tpl_tooltips  () {
        var tempHtml = "    <div class=\"tooltips\" style=\"display:none;\">\
            <span class=\"tooltips-arrow tooltips-arrow-<#= placement #>\"><em>◆</em><i>◆</i></span>\
            <#= content #>\
        </div>"
        return tempHtml;
    }
    function tpl_hint  () {
        var tempHtml = "   <div class=\"hint hint-<#= type #>\"><#= content #></div>"
        return tempHtml;
    }
    function tpl_qrcode  () {
        var tempHtml = "  <div id=\"qrcode\">\
            <img src=\"<#= src #>\">\
            <a href=\"javascript:;\" class=\"qrcode-btn j-closeQrcode\"><i class=\"gicon-remove white\"></i></a>\
        </div>"
        return tempHtml;
    }
    function tpl_popbox_ImgPicker_uploadPrvItem () {
        var tempHtml = "   <li>\
            <span class=\"img-list-btndel j-imgpicker-upload-btndel\"><i class=\"gicon-trash white\"></i></span>\
            <span class=\"img-list-overlay\"></span>\
            <img src=\"<#= url #>\">\
        </li>"
        return tempHtml;
    }
