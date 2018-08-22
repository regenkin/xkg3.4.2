//保存数据
function getData(f) {

	var dd = document.forms[2].emsname;
	var s = dd.value;



	if(s == null || s == "")
	{
	    alert("请填写单据名称");
	    return;
	}

	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1) ? flexApp : flexApp.getElementsByTagName("embed")[0];

	if (f == 1) {

	    $.post("AddExpressTemplate.aspx", { task: "IsExist", ExpressName: s }, function (msg) {
	        
	        if (msg == "N") {
	            flexApp.saveData(s);
	        } else {
	            alert("该快递公司模板已存在，请不要重添加！");
	        }
	    });

	    return;
	}

	flexApp.saveData(s);
}

//偏移量校正
function offsetSet() {
    var x=document.getElementById("offsetX").value;
    var y = document.getElementById("offsetY").value;
        x=x*1;
        y = y * 1;

        if (isNaN(x) || isNaN(y))
        {
        alert("偏移量请填写数值，单位mm");
        return false;
        }
       
    var flexApp = document.getElementById("flexApp");
    flexApp = (navigator.appName.indexOf("Microsoft") != -1) ? flexApp : flexApp.getElementsByTagName("embed")[0];
    flexApp.OffsetSet(x, y);
    return true;
}

﻿//删除打印项数据
function delData(){
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.dele();
  
}
//设置字体
function fontfamily()
{
	var dd = document.forms[1].ffamily;
	var s = dd.options[dd.selectedIndex].value;
   
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.setfamily( s );
}
//设置字体大小
function fontsize()
{
	var dd = document.forms[0].fsize;
	var s = dd.options[dd.selectedIndex].value;
    
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.setsize( s );
}
//设置文字对齐方式
function updateBottom()
{
	var dd = document.forms[11].bottomDropDown;
	var s = dd.options[dd.selectedIndex].value;
    
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.setlabel(s);
	flexApp.setalign(s);	
}
//设置文字样式
function showstyle()
{
	var dd = document.forms[10].style;
	var s = dd.options[dd.selectedIndex].value;
    
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.setstyle( s );
}
//添加打印项
function addbtn(t){

    //alert(t);
	var dd = document.forms[8].item;
	var s = dd.options[dd.selectedIndex].value +"_"+ t;
	
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];

	if(dd.options[dd.selectedIndex].value != "自定义内容"){

		flexApp.add( s ,false);
	}else{

		flexApp.add( s ,true);
	}

}
//删除背景图片
function imagebtn(){

	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.deleteimage();
}
//设置单据大小
function setfsize(){

	if(document.forms[4].swidth.value < 1001 && document.forms[5].sheight.value < 1001)
    {
	    var aa = document.forms[4].swidth;
		var dd = aa.value;
		var dwidth =aa.value/25.4*96;
		
		var bb = document.forms[5].sheight;
		var dh = bb.value;
		var dheight=bb.value/25.4*96;
		
		flexApp.width =dwidth;
		flexApp.height =dheight;	
		
		var flexA = document.getElementById("flexApp");
		flexA = (navigator.appName.indexOf("Microsoft") != -1)?flexA:flexA.getElementsByTagName("embed")[0];
	
		flexA.setordsize(dd , dh);
		
	}else{
	    alert("单据尺寸宽、高分别不能超过1000mm");
	}

}
//显示隐藏栏
function clickbtn()
{

	var dd = document.forms[6].btnclick;
	
	var flexApp = document.getElementById("flexApp");
	flexApp = (navigator.appName.indexOf("Microsoft") != -1)?flexApp :flexApp .getElementsByTagName("embed")[0];
	flexApp.showbrowse();
}

//显示提示消息
function showTips(s)
{
	alert(s);
}