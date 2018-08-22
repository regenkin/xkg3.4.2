<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="MultiArticlesEdit.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.MultiArticlesEdit" %>

<%@ Register Src="~/hieditor/ueditor/controls/ucUeditor.ascx" TagName="KindeditorControl" TagPrefix="Kindeditor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link rel="stylesheet" href="/Admin/shop/Public/css/dist/component-min.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/jbox/jbox-min.css">
    <!-- diy css start-->
    <link rel="stylesheet" href="/Admin/shop/PublicMob/css/style.css">
    <link rel="stylesheet" href="/Admin/shop/Public/plugins/uploadify/uploadify-min.css">
    <script src="/Admin/shop/Public/js/dist/underscore.js"></script>
    <script src="/Admin/shop/Public/plugins/jbox/jquery.jBox-2.3.min.js"></script>
<%--    <script src="/Admin/shop/Public/plugins/zclip/jquery.zclip-min.js"></script>--%>
    <script src="/Admin/shop/Public/plugins/uploadify/jquery.uploadify.min.js?ver2016"></script>
    <script src="/Admin/shop/Public/js/jquery-ui/jquery-ui.min.js"></script>
    <script src="/Admin/shop/Public/js/config.js"></script>
    <script src="/Admin/shop/Public/plugins/colorpicker/colorpicker.js"></script>
    <script src="/Admin/js/HiShopComPlugin.js"></script>
    <script src="/Admin/shop/Public/js/dist/componentadmin-min.js?v1023"></script>
    <style type="text/css">
        .jbox-container {
            overflow-x: hidden;
        }
        .gagp-goodslist .twoclass ul li {
            padding: 0;
            border-bottom: none;
        }
        /*.gagp-goodslist{
            min-height:300px !important;
        }*/
        .modalshopclasslist .oneclass p:first-child i.down {
            background: url("../images/iconfont-14052230.png");
        }
    </style>
    <script type="text/javascript">
        var urlStart=window.location.protocol+"//"+window.location.host;
        function callbackData(){
            HiShop.popbox.dplPickerColletion({
                linkType: $(this).data("val"),
                callback: function (item, type) {
                    //alert(item.link);
                    //alert(item.title);
                    var tempLink="";
                    switch(item.title)
                    {
                        case "查看原文":
                            tempLink="";
                            $('#urlData').hide(); 
                            $('#spLinkTitle').html(item.title);
                            $('#hidLinkType').val('1');
                            //$('#divContent').show();
                            break;
                        case "店铺主页":
                            tempLink=item.link;
                            $('#urlData').show();
                            $('#hidLinkType').val('4');
                            $('#spLinkTitle').html(item.title);
                            break;
                        default:
                            tempLink=item.link;
                            $('#urlData').show();
                            //$('#divContent').hide();
                            $('#hidLinkType').val('8');
                            $('#spLinkTitle').html("已选择链接");
                            break;
                    }
                    if(tempLink.length>1){
                        if(tempLink.indexOf("http")==-1){
                            $('#urlData').val(urlStart+tempLink);
                        }else{
                            $('#urlData').val(tempLink);
                        }
                    }
                }
            }); 
        }



        var MaterialID=<%=MaterialID%>;
        
        var twCount = 2;

        var twCountb = 2;

        //当前编辑的图文ID
        var boxIdN = 1;

        //是否为编辑状态
        //var edit = false;

        //图文json对象
        var tws = [];
        var tw = function () { return { "BoxId": "", "Title": "", "Url": "", "LinkType": "1", "Content": "", "ImageUrl": "", "Status": "new" } }// "Description": "",
        ntw = new tw();
        ntw.BoxId = 1;
        tws.push(ntw);
        ntw = new tw();
        ntw.BoxId = 2;
        tws.push(ntw);


        $(document).ready(function () {
            CreateDropdown($("#urlData"), $("#box_move .form-group").eq(1), {createType:3,showTitle:true,txtContinuity:false,reWriteSpan:true,callback:callbackData});
            

            $("#spLinkTitle").html("<%=htmlLinkTypeName%>");
            if($("#hidLinkType").val()=="1"){
                $('#urlData').hide();
            }
            $("#dropdow-menu-link  li[data-val='13']").remove();

          
            $('#Picture,#smallpic').click(function () {
                HiShop.popbox.ImgPicker(function (obj) {
                    BindPicData(obj);
                });
            });

            //$('#Picture').click(function () {
            //    var Rand = Math.random();
            //    $("#MyIframe").attr("src", "../weibo/Pictures.aspx?Rand=" + Rand);
            //    $('#myModal').modal('toggle').children().css({
            //        width: '800px',
            //        height: '700px'
            //    })
            //    $("#myModal").modal({ show: true });
            //});
            setTimeout(function(){loadJsonData();},500);

            
            <%=htmlAddJs%>
            $("#Homepage").click(function () {
                $('#urlData').val(($(this).attr("gotourl"))).keyup();
                $('#title').val($(this).html());
                $('#spLinkTitle').html($(this).html());
                $('#hidLinkType').val('4');
                //$('#divContent').hide();
                $('#urlData').show();
            });
            //$('#GoodsAndType').click(function () {
            //    var Rand = Math.random();
            //    $("#MyGoodsAndTypeIframe").attr("src", "../weibo/Goods.aspx?Rand=" + Rand);
            //    $('#myIframeModal').modal('toggle').children().css({
            //        width: '800px',
            //        height: '700px'
            //    })
            //    $("#myIframeModal").modal({ show: true });
            //});
        })
        
        function closeModal(modalid, txtContentid, value, text) {            
            var linkurl = 'http://' + value;
            $('#urlData').val(linkurl).keyup();
            $('#title').val(text);

            $('#spLinkTitle').html("商品及分类");
            $('#' + modalid).modal('hide');
            $('#hidLinkType').val('2');
            //$('#divContent').hide();
            $('#urlData').show();
        }
        function BindPicData(Imgvalue) {
            var imgSrc = Imgvalue;
            $("#fmSrc").val(imgSrc);
            var smallimg = $("<img class='img-responsive'>");
            $("#smallpic").empty();
            smallimg.attr("src", imgSrc);
            $("#img" + boxIdN).attr("src", imgSrc);
            $("#smallpic").append(smallimg);
            $("#smallpic").show();

            $('#myModal').modal('hide');
            $('#removeimg').click(function () {
                $(this).parent().remove();
            });
        }
        function loadJsonData() {
            if(MaterialID>0){
                tws=<%=articleJson%>;
                boxIdN = 1;
                for (var a in tws) {
                    if (a >= 2) {
                        editSBox();
                    }
                    if (!isNull(tws[a].Title)) {
                        $("#title" + (parseInt(a) + 1)).text(tws[a].Title);
                    }
                    if (!isNull(tws[a].ImageUrl)) {
                        $("#img" + (parseInt(a) + 1)).attr("src", (tws[a].ImageUrl)).show();
                    }
                }    
                loadData();
            }
        }
        //同步标题
        function syncTitle(value) {
            $("#title" + boxIdN).text(value);
        }
        function syncSingleTitle(value) {
            $("#LbimgTitle").text(value);
        }
        //添加图文
        function addSBox() {
            if (twCount >= 10) {
                alert("最多可添加10个图文");
            } else {
                twCountb++;
                twCount++;
                var code = $('#modelSBox').html();
                code = code.replace(/rpcode1366/gm, twCountb);
                $('#addSBoxInfoHere').before(code);
                var ntw = new tw();
                ntw.BoxId = twCountb;
                tws.push(ntw);
            }
        }

        function editSBox() {
            if (twCount >= 10) {
                alert("最多可添加10个图文");
            } else {
                twCountb++;
                twCount++;
                var code = $('#modelSBox').html();
                code = code.replace(/rpcode1366/gm, twCountb);
                $('#addSBoxInfoHere').before(code);
            }
        }

        //删除图文
        function sBoxDel(id) {
            $("#dropdown-menu" + id).show();
            $($("#dropdown-menu" + id).find("*")).mouseover(function () {
                $("#zz_sbox" + id).show()
            })
        }
        function cancelBoxDel(id) {
            $("#dropdown-menu" + id).hide();
        }
        //确认删除图文
        function conformBoxDel(id) {
            saveDataToJson();
            if (twCount <= 2) {
                alert("请至少保留两个图文！");
            } else {
                $('#sbox' + id).remove();
                //json中设置status值为del
                tws[id - 1].Status = "del";
                twCount--;
                //默认回到第一个
                boxIdN = 1;
                loadData();
                //移动模型
                moveBox();
            }
        }

        //修改图文
        function editTW(sBoxId) {
            if(sBoxId>1){
                $("#whtips").html("建议尺寸：200×200像素，小于200KB，支持.jpg、.gif、.png格式");
            }else{
                $("#whtips").html("建议尺寸：900×500像素，小于200KB，支持.jpg、.gif、.png格式");
            }
            //保存对象到json
            saveDataToJson();
            //初始化
            initializeInfoBox();
            boxIdN = sBoxId;
            //载入模型数据对象
            loadData();
            //移动模型
            moveBox();
        }

        function DelCount() {
            var count = 0;
            for (var a in tws) {
                if (a.Status == "del") {
                    count += 1;
                }
            }
        }
        ///递归运算直到下一次不是已删除的。
        function GetNextIndex(CurrIndex) {
            if (CurrIndex < tws.length) {
                CurrIndex += 1;
                if (tws[CurrIndex - 1].Status != "del") {
                    return CurrIndex;
                }
                else {
                    return GetNextIndex(CurrIndex);
                }
            }
            else {
                CurrIndex = 1;
                if (tws[CurrIndex - 1].Status != "del") {
                    return CurrIndex;
                }
                else {
                    return GetNextIndex(CurrIndex);
                }
            }
        }

        function getIndex() {
            if (DelCount() == tws.length) boxIdN = 1;
            else {
                if (boxIdN > tws.length) boxIdN = 1;

                if (tws[boxIdN - 1].Status == "del")
                    boxIdN = GetNextIndex(boxIdN);
            }

        }
        //移动模型
        function moveBox() {
            getIndex();
            var h = $("#sbox" + boxIdN).offset().top - 205;
            $("#box_move").animate({ "margin-top": h + "px" });
        }

        //保存对象到json
        function saveDataToJson() {
            debugger;
            for (var a in tws) {
                var ia = parseInt(a);
                if (ia + 1 == boxIdN) {
                    tws[ia].Title = $("#title").val();
                    tws[ia].Content =UE.getEditor('ctl00_ContentPlaceHolder1_fkContent_txtMemo').getContent();//editor.html();
                    tws[ia].Url = $("#urlData").val();
                    tws[ia].ImageUrl = $("#fmSrc").val();
                    tws[ia].LinkType=$("#hidLinkType").val();
                }
            }
        }

        //载入模型数据对象
        function loadData() {
            getIndex();
            $("#title").val(tws[boxIdN - 1].Title);
            $("#fmSrc").val(tws[boxIdN - 1].ImageUrl);
            $("#urlData").val(tws[boxIdN - 1].Url).keyup();
            //editor.html(tws[boxIdN - 1].Content);
            UE.getEditor('ctl00_ContentPlaceHolder1_fkContent_txtMemo').setContent(tws[boxIdN - 1].Content,false);

            if (tws[boxIdN - 1].ImageUrl != "" && tws[boxIdN - 1].ImageUrl != null) {
                var smallimg = $("<img class='img-responsive'>");
                smallimg.attr("src", tws[boxIdN - 1].ImageUrl);
                $("#smallpic").empty();
                $("#smallpic").append(smallimg);
                $("#smallpic").show();
            }
            $("#hidLinkType").val(tws[boxIdN - 1].LinkType);
            $("#spLinkTitle").html($('a[tid="'+tws[boxIdN - 1].LinkType+'"]').html());
            if(tws[boxIdN - 1].LinkType==1){
                //$("#divContent").show();
                $("#urlData").hide();
            }else{
                //$("#divContent").hide();
                $("#urlData").show();
            }

        }

        //初始化数据录入
        function initializeInfoBox() {
            $("#title").val("");
            $("#fmSrc").val("");
            //editor.html('');
            UE.getEditor('ctl00_ContentPlaceHolder1_fkContent_txtMemo').setContent("",false);
            $("#typeID").val("0");
            //$("#w_url").css("display", "none");
            $("#hidLinkType").val("1");
            $("#smallpic").empty().hide();
            $("#urlData").val("").keyup();
        }

        //显示遮罩
        function sBoxzzShow(id) {
            $('#zz_sbox' + id).css('display', 'block')
        }

        //隐藏遮罩
        function sBoxzzHide(id) {
            $('#zz_sbox' + id).css('display', 'none')
        }

        //是否为空
        function isNull(obj) {
            if (null == obj) {
                return true;
            } else {
                if ("" == obj) {
                    return true;
                }
                return false;
            }
        }

        //修改预览图
        function changeSYLT(sboxID, src) {
            if (sboxID)
                $("#sylt" + sboxID).attr("src", src);
        }

        //载入封面图片
        function loadFW(id, src) {
            $("#img" + id).attr("src", src).css("display", "block");
        }

        function IsLastEdit() {
            var IsLastEdit = true;
            for (var i = boxIdN - 1; i < tws.length; i++) {
                if (tws[i].Status != "del")
                    IsLastEdit = false;
            }
            return IsLastEdit;
        }

        //（可删除）以String的方式查看Json
        function viewJson() {
            $("#viewJson").val(JSON.stringify(tws));
        }

        //验证Json数据
        function checkJson() {
            editTW(boxIdN); //通过此方法保存最后一个编辑的数据
            var errorBoxId;
            var pass = true;
            for (var a in tws) {
                if (tws[a].Status != "del" ) {//&& a < boxIdN
                    errorBoxId = a;
                    if (isNull(tws[a].Title)) {
                        HiTipsShow("标题不能为空！","error");
                        pass = false;
                        break;
                    } else if (isNull(tws[a].ImageUrl)) {
                        HiTipsShow("请选择一张封面！", "error");
                        pass = false;
                        break;
                    } else if (tws[a].LinkType=="1"&& isNull(tws[a].Content)) {
                        HiTipsShow("请输入内容！", "error");
                        pass = false;
                        break;
                    }else if(tws[a].LinkType!="1"&& isNull(tws[a].Url)){
                        HiTipsShow("请选择或输入自定义链接！", "error");
                        pass = false;
                        break;
                    }
                }
            }
            if(!pass)
            {
                //载入错误图文
                editTW(parseInt(errorBoxId) + 1);
                return false;
            }
            else
            {

                var IsShare=$("#ctl00_ContentPlaceHolder1_IsShare").prop("checked");
                
                tws[0].IsShare=IsShare;
               

                $("#Articlejson").val(JSON.stringify(tws));
                //alert(JSON.stringify(tws))
                AddMultArticles("tolist");
                //if(boxIdN < tws.length){
                //    initializeInfoBox();
                //    boxIdN = boxIdN + 1;
                //}
            }



            //if (pass && boxIdN < tws.length) {
            //    initializeInfoBox();

            //    boxIdN = boxIdN + 1;



            //    if (IsLastEdit()) {
            //        pass = true;
            //    }
            //    else {
            //        //载入模型数据对象
            //        loadData();

            //        //移动模型
            //        moveBox();
            //        return false;


            //    }



            //}
            //if (pass) {
            //} else {
               
            //}
        }
        
        function MultiArticleShow(){
            editTW(boxIdN); //通过此方法保存最后一个编辑的数据
            var errorBoxId;
            var pass = true;
            for (var a in tws) {
                if (tws[a].Status != "del" ) {//&& a < boxIdN
                    errorBoxId = a;
                    if (isNull(tws[a].Title)) {
                        HiTipsShow("标题不能为空！","error");
                        pass = false;
                        break;
                    } else if (isNull(tws[a].ImageUrl)) {
                        HiTipsShow("请选择一张封面！", "error");
                        pass = false;
                        break;
                    } else if (tws[a].LinkType=="1"&& isNull(tws[a].Content)) {
                        HiTipsShow("请输入内容！", "error");
                        pass = false;
                        break;
                    }else if(tws[a].LinkType!="1"&& isNull(tws[a].Url)){
                        HiTipsShow("请选择或输入自定义链接！", "error");
                        pass = false;
                        break;
                    }
                }
            }
            if(!pass)
            {
                //载入错误图文
                editTW(parseInt(errorBoxId) + 1);
                return false;
            }
            else
            {
                var IsShare=$("#ctl00_ContentPlaceHolder1_IsShare").prop("checked");
                tws[0].IsShare=IsShare;
               

                $("#Articlejson").val(JSON.stringify(tws));
                AddMultArticles("showinfo");
            }
        }
        

        function AddMultArticles(backtype) {
            $.ajax({
                url: "./multiarticlesedit.aspx?cmd=add&id=" + MaterialID,
                type: "POST",
                dataType: "json",
                data: {
                    "MultiArticle": $("#Articlejson").val()
                },
                success: function (json) {
                    if (json.type == "1") {
                        HiTipsShow(json.tips+(backtype=="showinfo"?"正在生成预览...":""),"success",function(){
                            if(backtype=="showinfo"){
                                ArticleView(json.id);
                                $('#previewshow').on('hidden.bs.modal', function () {
                                    /*模态框关闭的时候页面跳转到列表页*/
                                    window.location.href = "<%=ReUrl%>";
                                })
                            }else{
                                window.location.href = "<%=ReUrl%>";
                            }
                        })
                    }
                    else {
                        HiTipsShow(json.tips,"error");
                    }
                },
                error: function (xmlHttpRequest, error) {
                    HiTipsShow(error,"error");
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="page-header">
        <h2><%=htmlOperName %>多条图文</h2>
    </div>
    <div class="blank">
        <a href="<%=ReUrl %>" class="btn btn-primary">&lt;&lt; 返回图文管理</a>
    </div>
    <div class="edit-text clearfix">
        <div class="edit-text-left">
            <div class="mobile-border">
                <div class="mobile-d">
                    <div class="mobile-header">
                        <i></i>
                        <div class="mobile-title">店铺主页</div>
                    </div>
                    <div class="mobile mate-list">
                        <div class="mate-inner top" id="sbox1">
                            <div id="fm1" class="mate-img" onmousemove="sBoxzzShow('1')" <%-- style="width:320px;height:145px;overflow:hidden"--%>>
                                <img id="img1" src="../images/320x145.gif" class="img-responsive">
                                <div class="mouse-hover" style="display: none;" id="zz_sbox1" onmouseout="sBoxzzHide(1)">
                                    <span>
                                        <button class="glyphicon glyphicon-pencil none" onclick="editTW(1)"></button>
                                    </span>
                                </div>
                                <div class="title" id="title1" onmouseout="sBoxzzHide(1)">这是一个标题</div>
                            </div>
                        </div>
                        <div class="mate-inner" id="sbox2">
                            <div class="child-mate" onmousemove="sBoxzzShow(2)">
                                <div class="child-mate-title clearfix">
                                    <div class="title" id="title2">
                                        <h4>图文标题</h4>
                                    </div>
                                    <div class="img">
                                        <img id="img2" src="../images/80x80.jpg" class='img-responsive'>
                                    </div>
                                </div>
                                <div class="mouse-hover" style="display: none;" id="zz_sbox2" onmouseout="sBoxzzHide(2)">
                                    <span class="dropdown">
                                        <button class="glyphicon glyphicon-trash none" id="dLabel2" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" onclick="sBoxDel(2);"></button>
                                        <div class="dropdown-menu width" aria-labelledby="dLabel2" id="dropdown-menu2">
                                            <p class="dropdown-header">确定删除吗？</p>
                                            <button type="button" class="btn btn-danger marg" onclick="conformBoxDel(2);">删除</button>
                                            <button type="button" class="btn btn-primary" onclick="cancelBoxDel(2)">取消</button>
                                        </div>
                                    </span>
                                    <span>
                                        <button class="glyphicon glyphicon-pencil none" onclick="editTW(2)"></button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <span id="addSBoxInfoHere"></span>
                        <div class="mobile-add">
                            <span class="glyphicon glyphicon-plus" onclick="addSBox()"></span>
                        </div>
                    </div>
                    <div class="mobile-footer"></div>
                </div>


            </div>

        </div>
        <div class="edit-text-right" id="box_move">
            <div class="edit-inner">
                <div class="form-group">
                    <label for="exampleInputEmail1"><em>*</em>标题：</label>
                    <input type="hidden" value="<%=htmlLinkType %>" id="hidLinkType" /><input type="text" class="form-control" id="title" placeholder="多图文标题" onkeyup="syncTitle(this.value)" maxlength="50" />
                    <small class="help-block">建议不多于30个字</small>
                </div>
                <div class="form-group">
                    <label><em>*</em>封面：</label>
                    <div id="smallpic" style="display: none; width: 100px; height: 100px; overflow: hidden"></div>
                    <input id="fmSrc" type="text" value="" style="display: none;" />
                    <button type="button" class="btn btn-primary form-control" id="Picture">选择图片</button>
                    <small class="help-block inline" id="whtips">建议尺寸：900×500像素，小于200KB，支持.jpg、.gif、.png格式</small>
                </div>

                <input type="text" class="form-control" id="urlData" placeholder="" value="<%=htmlUrl %>" style="display: none; margin-top: 5px;" maxlength="255">

                <div class="form-group" id="divContent">
                    <p>
                        <label><em>&nbsp;</em>正文：</label>
                    </p>
                    <form runat="server">
                        <Kindeditor:KindeditorControl ID="fkContent" runat="server" Width="558" Height="200" />
                    </form>
                </div>
                <div class="form-group" id="IsSharePanel">
                    <label>分销商分享素材:<input type="checkbox" id="IsShare" checked="checked" runat="server" style="margin: -3px 5px 0px 10px" />是</label>
                </div>
            </div>
        </div>
    </div>
    <div style="height: 300px; width: 100%">&nbsp;</div>
    <div class="footer-btn navbar-fixed-bottom">
        <input id="Articlejson" name="Articlejson" type="hidden" />
        <input type="hidden" id="viewJson" />
        <button type="button" class="btn btn-success" onclick="return checkJson()">保存</button>
        <button type="button" class="btn btn-success" onclick='MultiArticleShow()'>保存并预览</button>


        <div id="modelSBox" style="display: none;">
            <div class="mate-inner" id="sboxrpcode1366">
                <div class="child-mate" onmousemove="sBoxzzShow(rpcode1366)">
                    <div class="child-mate-title clearfix">
                        <div class="title" id="titlerpcode1366">
                            <h4>图文标题</h4>
                        </div>
                        <div class="img">
                            <img id="imgrpcode1366" src="../images/80x80.jpg" class='img-responsive'>
                        </div>
                    </div>
                    <div class="mouse-hover" style="display: none;" id="zz_sboxrpcode1366" onmouseout="sBoxzzHide(rpcode1366)">
                        <span class="dropdown">
                            <button class="glyphicon glyphicon-trash none" id="dLabelrpcode1366" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" onclick="sBoxDel(rpcode1366);"></button>
                            <div class="dropdown-menu width" id="dropdown-menurpcode1366" aria-labelledby="dLabelrpcode1366">
                                <p class="dropdown-header">确定删除吗？</p>
                                <button type="button" class="btn btn-danger marg" onclick="conformBoxDel(rpcode1366);">删除</button>
                                <button type="button" class="btn btn-primary" onclick="cancelBoxDel(rpcode1366)">取消</button>
                            </div>
                        </span>
                        <span>
                            <button class="glyphicon glyphicon-pencil none" onclick="editTW(rpcode1366)"></button>
                        </span>
                    </div>
                </div>
            </div>
        </div>




    </div>



    <div class="modal fade" id="myIframeModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">商品及分类</h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyGoodsAndTypeIframe" width="750" height="370"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
    </div>



    <!-- 模态框（Modal） -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog"
        aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h4 class="modal-title" id="myModalLabel">图片
                    </h4>
                </div>
                <div class="modal-body">
                    <iframe src="" id="MyIframe" width="750" height="370"></iframe>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default"
                        data-dismiss="modal">
                        关闭
                    </button>

                </div>
            </div>
            <!-- /.modal-content -->
        </div>
        <!-- /.modal -->
    </div>

</asp:Content>
