<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="SkuValue.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.SkuValue" %>

<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>

<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
   <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css">
<script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
<script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
<script type="text/javascript" src="../js/jquery.formvalidation.js"></script>
   <link rel="stylesheet" href="/admin/css/common.css" />
    <script src="/admin/js/Framenew.js"></script>

     
    <script>
     

        function CloseModal() {
            window.parent.closeModal(getParam('action'));
        }

    </script>
</head>
<body>
    <form runat="server">
    <%--添加规格值--%>
    <div class="form-horizontal" id="FormAttributeValue">
        <div id="valueStr" runat="server">
            <div class="form-group" style="height: 30px;">
                <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>规格值名</label>
                <div class="col-xs-8">
                    <asp:textbox id="txtValueStr" cssclass="form-control" MaxLength="50" runat="server" onkeydown="javascript:this.value=this.value.replace('，',',')" />
                </div>
                
            </div>
             <small class="help-block" style="margin-left:100px;"><%--多个规格值可用“,”号隔开，--%>每个值的字符数最多50个字符</small>
        </div>
        <div id="valueImage" runat="server" visible="false">
            <div class="form-group" style="height: 30px; display: none">
                <label for="inputEmail3" class="col-xs-2 control-label">图片地址</label>
                <div class="col-xs-5">
                    <asp:fileupload id="fileUpload" cssclass="input_longest" runat="server" width="250px" onchange="PreviewImg(this)" />
                </div>
                <small class="help-block">仅接受jpg、gif、png、格式的图片</small>
            </div>
            <div class="form-group" style="height: 30px; display: none">
                <label for="inputEmail3" class="col-xs-2 control-label">图片描述</label>
                <div class="col-xs-5">
                    <asp:textbox id="txtValueDec" cssclass="form-control" runat="server" />
                </div>
                <small class="help-block">1到20个字符</small>
            </div>
           
        </div>
         <div class="form-group">
                <div class="col-xs-offset-2 col-xs-10">
                    <asp:button id="btnCreateValue" runat="server" text="确 定" class="btn btn-success" />
                     <button type="button" class="btn btn-default"  onclick="CloseModal();">关闭</button>
                </div>
            </div>
    </div>
    <input runat="server" type="hidden" id="currentAttributeId" />
</form>

<script>
    //function isFlagValue() {
    //    if ($("#ctl00_contentHolder_valueStr").val() != undefined) {
    //        var skuValue = document.getElementById("ctl00_contentHolder_txtValueStr").value.replace(/(^\s*)|(\s*$)/g, "");
    //        if (skuValue.length < 1) {
    //            alert("请输入规格值");
    //            return false;
    //        }
    //        setArryText('ctl00_contentHolder_txtValueStr', skuValue);
    //    }
    //    else {
    //        var imgpath = document.getElementById("ctl00_contentHolder_fileUpload").value;
    //        if (imgpath.length < 1) {
    //            alert("请浏览图片");
    //            return false;
    //        }
    //        var valuedesc = document.getElementById("ctl00_contentHolder_txtValueDec").value.replace(/\s/g, "");
    //        if (valuedesc == "") {
    //            alert("请输入图片描述");
    //            return false;
    //        }
    //        setArryText('ctl00_contentHolder_fileUpload', imgpath);
    //        setArryText('ctl00_contentHolder_txtValueDec', valuedesc);
    //    }

    //    return true;
    //}


    ////添加规格值
    //function ShowAddSKUValueDiv(attributeId, useAttributeImage, attributename) {
    //    if (useAttributeImage == "True") {
    //        document.getElementById("ctl00_contentHolder_valueStr").style.display = "none";
    //        document.getElementById("ctl00_contentHolder_valueImage").style.display = "block";
    //    }
    //    else {
    //        document.getElementById("ctl00_contentHolder_valueImage").style.display = "none";
    //        document.getElementById("ctl00_contentHolder_valueStr").style.display = "block";
    //        $("#ctl00_contentHolder_specificationView_currentAttributeId").val(attributeId);
    //        DialogShow("添加" + attributename + "的规格值", "skuvalueadd", "addSKUValue", "ctl00_contentHolder_specificationView_btnCreateValue");
    //    }

    //}

    //function PreviewImg(imgFile) {
    //    var newPreview = document.getElementById("newPreview");
    //    newPreview.filters.item("DXImageTransform.Microsoft.AlphaImageLoader").src = imgFile.value;
    //    newPreview.style.width = "28px";
    //    newPreview.style.height = "26px";
    //}

    //function validatorForm() {
    //    return isFlagValue();
    //}
    $(function () {
        //$('#ctl01').formvalidation({

        //    'txtValueStr': {
        //        validators: {
        //            notEmpty: {
        //                message: '多个规格值可用“,”号隔开，每个值的字符数最多15个字符'
        //            },
        //            stringLength: {
        //                min: 1,
        //                max: 15,
        //                message: '规格值的名称,1-15个字符'
        //            }
        //        }
        //    }

        //});
    })
</script>
</body>
</html>




