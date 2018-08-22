<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="EditExpressTemplate.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Trade.EditExpressTemplate" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Import Namespace="Hidistro.Core" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

      <div class="page-header">
                    <h2>编辑快递单</h2>
    </div>
    
      <%--<form id="thisForm" runat="server">--%>
    <div >
			      <table width="100%" id="controls" height="56" class="set-switch" border="0" cellpadding="0" cellspacing="0" style="text-align:center; border:1px #CCC solid;  background-color:#E7E7E7; font-size:12px; padding:0px;
				margin:0px 0px 5px 0px;">
                      <thead>
				  <tr>
					<td height="24" colspan="2" class="tdline">单据名称</td>
					<td colspan="2" class="tdline">单据尺寸</td>
					<td colspan="2" class="tdline">单据背景图</td>
					<td colspan="2" class="tdline">单据打印项</td>
					<td width="146">
                        <form name="form0" style="display:none"></form>
                        <form name="form1" style=" padding:8px 0px 0px 0px">
					  <label>打印偏移修正</label>
					</form></td>
				  </tr></thead><tbody>
				  <tr>
					<td width="108" height="30"><form name="form2">
					  <label><select name="emsname" id="emsname" size="1"><%=ems %></select></label>
					</form></td>
					<td width="42" class="tdline"><form name="form3">
					  <label><button name="btndata" onClick="getData(0)" type="button" class="btn btn-info btn-xs">保存
							</button></label>
					</form></td>
					<td width="112"><form name="form4">
					  <label>宽:<input name="swidth" type="text" id="widths" size="4" value="228" onchange="setfsize()"/>mm</label>
					</form></td>
					<td width="120" class="tdline"><form name="form5">
					  <label>*高:<input name="sheight" type="text" id="heights" size="4" value="127" onchange="setfsize()" />mm</label>
					</form></td>
					<td width="49"><form name="form6">
					  <label><button name="btnclick"
							onClick ="clickbtn();return false;" class="btn btn-info btn-xs">上传
							</button></label>
					</form></td>
					<td width="48" class="tdline"><form name="form7">
					  <label><button name="btnimage"
							onClick ="imagebtn();return false;" class="btn btn-info btn-xs">删除
							</button></label>
					</form></td>
					<td width="125"><form name="form8">
					  <label>
						 <select 
			    name="item" 
				onChange="addbtn(i);tt();return false;"
				size="1">
					<option value="收货人-姓名">添加打印项</option>
					<option value="收货人-姓名">收货人-姓名</option>
					<option value="收货人-地址">收货人-地址</option>
					<option value="收货人-电话">收货人-电话</option>
					<option value="收货人-邮编">收货人-邮编</option>	
					<option value="收货人-手机">收货人-手机</option>	
					<option value="收货人-地区1级">收货人-地区1级</option>
					<option value="收货人-地区2级">收货人-地区2级</option>
					<option value="收货人-地区3级">收货人-地区3级</option>	
					<option value="始发地-地区">始发地-地区</option>	

					<option value="发货人-姓名">发货人-姓名</option>
					<option value="发货人-地区1级">发货人-地区1级</option>
					<option value="发货人-地区2级">发货人-地区2级</option>
					<option value="发货人-地区3级">发货人-地区3级</option>										
					<option value="发货人-地址">发货人-地址</option>
					<option value="发货人-电话">发货人-电话</option>
					<option value="发货人-邮编">发货人-邮编</option>
					<option value="发货人-手机">发货人-手机</option>
                        <option value="目的地-地区">目的地-地区</option>

                        <option value="送货-上门时间">送货-上门时间</option>
								
					<option value="订单-订单号">订单-订单号</option>
					<option value="订单-总金额">订单-总金额</option>
					<option value="订单-物品总重量">订单-物品总重量</option>
					<option value="订单-备注">订单-备注</option>
					<option value="订单-详情">订单-详情</option>
					<option value="网店名称">网店名称</option>
					<option value="√">对号-√</option>
					<option value="自定义内容">自定义内容</option>
			</select></label>
					</form>
					</td>
					<td width="54" class="tdline"><form name="form9">
					  <label><button name="btnitem" onClick="delData()"  type="button" class="btn btn-info btn-xs">删除
							</button></label>
					</form></td>
					<td><a id="preview" class="btn btn-info btn-xs">打印偏移修正</a></td>
				  </tr></tbody>
				</table>
				<div id="writeroot"></div>
		
        	<div id="flashoutput">
			<noscript>
				<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">
				<param name="flashvars" value="bridgeName=example"/>
                <param name="wmode" value="transparent"/>
				<param name="src" value="../../Storage/master/flex/EditExpressTemplate.swf"/>
				<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/EditExpressTemplate.swf" height="600" width="100%"  wmode="transparent"  flashvars="bridgeName=example"/>
				</object>
			</noscript>
			<script language="javascript" charset="utf-8">
			    document.write('<object id="flexApp" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,5,0,0" height="600" width="900">');
			    document.write('<param name="flashvars" value="bridgeName=example"/> <param name="wmode" value="transparent"/>');
			    document.write('<param name="src" value="../../Storage/master/flex/EditExpressTemplate.swf"/>');
			    document.write('<embed name="flexApp" pluginspage="http://www.macromedia.com/go/getflashplayer" src="../../Storage/master/flex/EditExpressTemplate.swf" height="600" width="100%"  wmode="transparent"  flashvars="bridgeName=example"/>');
			    document.write('</object>');
			</script>
		</div>
			  </div>


    <div class="modal fade" id="previewshow">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        <div class="modal-title">打印偏移校正</div>
                    </div>
                    <div class="modal-body">
                        <div class="set-switch">可以通过偏移量来调整打印项在运单上文字的位置！</div>
                       <form class="form-horizontal">
                    <div class="form-group">
                        <label for="inputEmail3" class="col-xs-3 control-label">横向偏移：</label>
                        <div class="col-xs-3">
                            <input type="text" id="offsetX" class="form-control" placeholder="">
                        </div>
                        <div class="col-xs-6 control-label" style="text-align:left">
                           (mm) 正数"向右"移，负数"向左"移
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="inputPassword3" class="col-xs-3 control-label">纵向偏移：</label>
                        <div class="col-xs-3">
                            <input type="text"  id="offsetY" class="form-control" placeholder="">
                        </div>
                          <div class="col-xs-6 control-label" style="text-align:left">
                           (mm) 正数"向下"移，负数"向上"移
                        </div>
                    </div>
                    
                </form>

                    </div>
                    <div class="modal-footer" style="padding-right:50px">
                        <button type="button" onclick="MdPrint()" class="btn btn-primary" >确定</button>
                        <button type="button" class="btn btn-info" data-dismiss="modal">关闭</button>
                    </div>
                </div><!-- /.modal-content -->
            </div><!-- /.modal-dialog -->
        </div>
<%--    </form>--%>
    <script>

        $(function () {
            $('#preview').click(function () {
                $('#previewshow').modal('toggle').children().css({
                    width: '500px',
                    top: '200px'

                });


            })
        })

        function MdPrint() {
            // $('#previewshow').modal('toggle');
            if (offsetSet()) {
                $('#previewshow').modal('toggle');

            };
        }



        var i = 0;
        function tt() {
            i++;
            //document.getElementById("Button2").value=i;
        }

        function InitExpressName() {
            var url = location.href;
            $("#emsname").val(decodeURI(url.split("=")[2].split("&")[0]));
            $("#widths").val("<%= width%>");
            $("#heights").val("<%= height%>");
        }
        $(document).ready(function () {
            InitExpressName();
            setfsize();
        });

    </script>

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../js/ExpressFlex.js" ></script>
</asp:Content>