function CheckTagId(checkboxobj) {
   
   //var tagIds=$("#ctl00_ContentPlaceHolder1_txtProductTag").val().replace(/\s/g,"");
   //var arrtag=null;
   //if(tagIds!=""){
   //     if(tagIds.indexOf(',')<0){
   //         arrtag=new Array(1);
   //         arrtag[0]=tagIds;
   //     }else{
   //         arrtag=tagIds.split(',');
   //     }
   //}else{
   //     tagIds=checkboxobj.value;
   //}
   
   //if(arrtag!=null){
   //     var x=-1;
   //     for(var t=0;t<arrtag.length;t++){
   //         if(checkboxobj.value==arrtag[t]){
   //             x=t;
   //             break;
   //         }
   //     }
   //     if(x>=0){
   //         tagIds="";
   //         for(var k=0;k<arrtag.length;k++){
   //             if(k!=x){
   //                 tagIds+=arrtag[k]+",";
   //             }
   //         }
   //         if(tagIds!=""){
   //             tagIds=tagIds.substr(0,tagIds.length-1);
   //         }
   //         //tagIds+=","+checkboxobj.value;
   //     }else{
   //         tagIds+=","+checkboxobj.value;
   //     }
   //}
    //$("#ctl00_ContentPlaceHolder1_txtProductTag").val(tagIds);

    //新方法，只选一个
    if ($(checkboxobj).prop("checked")) {
        $("#div_tags input[type='checkbox']").prop('checked', false);
        $(checkboxobj).prop('checked', true);
        $("#ctl00_ContentPlaceHolder1_txtProductTag").val($(checkboxobj).attr('value'));
    } else {
        $("#ctl00_ContentPlaceHolder1_txtProductTag").html("");
        $("#div_tags input[type='checkbox']").prop('checked', false);
        $(checkboxobj).prop('checked', false);
    }
}


function AddTags(aobj){
    if($("#a_addtag").text()=="添加"){
         $("#div_addtag").show('slow');
          $("#a_addtag").text('取消');
          $("#a_addtag").attr('class','del');
    }else{
         $("#div_addtag").hide('hide');
          $("#a_addtag").text('添加');
          $("#a_addtag").attr('class','add');
    }
   
}

function AddAjaxTags(){
   var tagvalu=$("#txtaddtag").val().replace(/\s/g,"");
    if(tagvalu==""){
        alert('请输入标签名称！');
        return false;
    }
    $("#div_addtag").hide('hide');
    $("#a_addtag").text('添加');
    $("#a_addtag").attr('class','add');
    $.ajax({
            url: "ProductTags.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: {TagValue:tagvalu,Mode:"Add",isAjax: "true"},
            async: false,
            success: function(data)
            {
                if (data.Status == "true")
                {
                    var newtagId=data.msg;
                    if (newtagId != "" && parseInt(newtagId) > 0) {
                        ClearCKB();
                        $("#div_tags").append($("<label><input type=\"checkbox\" value=\""+newtagId+"\" onclick=\"CheckTagId(this)\" checked=\"checked\" />"+tagvalu + "</label>"));
                           //var oldtagId=$("#ctl00_ContentPlaceHolder1_txtProductTag").val().replace(/\s/g,"");
                           //if(oldtagId!=""){
                           //     oldtagId+=","+newtagId;
                           //}else{
                           //     oldtagId=newtagId;
                           //}
                        $("#ctl00_ContentPlaceHolder1_txtProductTag").val(newtagId);
                           ShowMsg("添加标签名成功", true);
                    }
                }
                else {
                    ShowMsg(data.msg, false);
                }
            }
        });
}

function ClearCKB()
{
    $('#div_tags input[type="checkbox"]').removeAttr("checked");
}