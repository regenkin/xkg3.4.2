var frameName = "uploadFrame_" + Math.floor(Math.random() * 1000);
var postForm;
var formAction, formTarget, formMethod, formEncoding;
$(document).ready(function () {
    // 创建提交的iframe

    var isIE = (document.all) ? true : false;
    var ua = navigator.userAgent.toLowerCase();
    if (ua.indexOf("msie 9.0") > -1) {
        isIE = false;
    }

    var postFrame = isIE ? document.createElement("<iframe name=\"" + frameName + "\">") : document.createElement("iframe");
    postFrame.name = frameName;
    postFrame.style.display = "none";
    document.body.insertBefore(postFrame, document.body.childNodes[0]);
    postForm = document.forms.item(0);
    // 保存原form信息
    formAction = postForm.action;
    formTarget = postForm.target;
    formMethod = postForm.method;
    formEncoding = postForm.encoding;
});


function InitUploader(uploaderId, uploadType, snailtype, uploadSize) {
    var fileContent = document.getElementById(uploaderId + "_content");
    var fileInput = document.createElement("input");
    fileInput.name = uploaderId + "_file";
    fileInput.id = uploaderId + "_file";
    fileInput.type = "file";
    fileInput.accept = "image/*";
    fileInput.setAttribute("runat", "server");
    fileContent.appendChild(fileInput);
    fileInput.onchange = function () {
        if (!RegExp("\.(jpg|gif|png|jpeg)$", "i").test(fileInput.value)) {
            ResetUploader(uploadType, uploaderId, snailtype, uploadSize);
            alert("\u8BF7\u9009\u62E9\u4E00\u4E2A\u56FE\u7247\u6587\u4EF6");
            return;
        }
      
        postForm.target = frameName;
        postForm.method = "post";
        postForm.setAttribute("enctype", "multipart/form-data");
        postForm.encoding = "multipart/form-data";
        postForm.action = applicationPath + "/API/UpImg.ashx?action=upload&uploadType=" + uploadType + "&uploaderId=" + uploaderId + "&snailtype=" + snailtype + "&uploadSize=" + uploadSize;
        postForm.submit();

        // 还原form信息
        postForm.target = formTarget;
        postForm.method = formMethod;
        postForm.encoding = formEncoding;
        postForm.setAttribute("enctype", formEncoding);
        postForm.action = formAction;
    };
}

function ResetUploader(uploadType, uploaderId, snailtype, uploadSize) {
    var fileContent = document.getElementById(uploaderId + "_content");
    fileContent.removeChild(document.getElementById(uploaderId + "_file"));
   
    InitUploader(uploaderId, uploadType, snailtype, uploadSize);
}

// 上传回调
function UploadCallback(uploadType, uploaderId, uploadedImageUrl, snailtype, uploadSize) {
    ResetUploader(uploadType, uploaderId, snailtype, uploadSize);

    // 保存上传文件路径
    $("#" + uploaderId + "_uploadedImageUrl").val(uploadedImageUrl);
    // 显示图片预览
    UpdatePreview(uploaderId, uploadedImageUrl);

    // 隐藏上传操作
    var uploadBox = document.getElementById(uploaderId + "_upload");
    uploadBox.style.display = "none";

    // 显示删除操作
    var deleteBox = document.getElementById(uploaderId + "_delete");
    deleteBox.style.display = "";
}

// 错误回调
function ErrorCallback(uploadType, uploaderId, error, snailtype, uploadSize) {
    ResetUploader(uploadType, uploaderId, snailtype, uploadSize);
    alert(error);
}

// 显示图片预览
function UpdatePreview(uploaderId, imageUrl) {
    var previewBox = document.getElementById(uploaderId + "_preview");
    var childs = previewBox.getElementsByTagName("img");

    for (childIndex = 0; childIndex < childs.length; childIndex++) {
        previewBox.removeChild(childs[childIndex]);
    }

    if (imageUrl.length > 0) {
        var img = document.createElement("img");
        img.src = applicationPath + imageUrl;
        img.style.display = "none";
        img.style.backgroundColor="#fff"
        img.id = uploaderId + "_image";
        previewBox.appendChild(img);
        ResizeImage(img.id, 50, 50);
    }
}

function DeleteImage(uploaderId, uploadType, snailtype) {
    // 提示是否确定要删除此图片
    if (!confirm("\u786E\u5B9A\u8981\u5220\u9664\u6B64\u56FE\u7247\u5417\uFF1F")) {
        return;
    }

    postForm.target = frameName;
    postForm.method = "post";
    postForm.action = applicationPath + "/API/UpImg.ashx?action=delete&uploadType=" + uploadType + "&uploaderId=" + uploaderId + "&snailtype=" + snailtype;
    postForm.submit();

    // 还原form信息
    postForm.target = formTarget;
    postForm.method = formMethod;
    postForm.action = formAction;
}

function DeleteCallback(uploaderId) {
    // 删除图片预览
    UpdatePreview(uploaderId, "");

    $("#" + uploaderId + "_uploadedImageUrl").val("");
    // 隐藏删除操作
    var deleteBox = document.getElementById(uploaderId + "_delete");
    deleteBox.style.display = "none";

    // 显示上传操作
    var uploadBox = document.getElementById(uploaderId + "_upload");
    uploadBox.style.display = "";
}