<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="ManageShippingTemplates.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Settings.ManageShippingTemplates" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="Hidistro.ControlPanel.Settings" %>
<%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style>
.rows_title{background:#f2f2f2}
td{border:1px solid #999}
table th,td{text-align:center}
.Regions,.tdleft{text-align:left;padding-left:20px}
.HassFree{font-size:12px;font-weight:400;color:#126cfb;vertical-align:top;line-height:20px}
.popover-title{padding:8px 14px;margin:0;font-size:14px;background-color:#f7f7f7;border-bottom:1px solid #ebebeb;border-radius:5px 5px 0 0}
.popover{position:absolute;top:0;left:0;z-index:1060;display:none;max-width:350px;padding:1px;font-family:"Helvetica Neue",Helvetica,Arial,sans-serif;font-size:14px;font-style:normal;font-weight:400;line-height:1.42857143;text-align:left;text-align:start;text-decoration:none;text-shadow:none;text-transform:none;letter-spacing:normal;word-break:normal;word-spacing:normal;word-wrap:normal;white-space:normal;background-color:#fff;-webkit-background-clip:padding-box;background-clip:padding-box;border:1px solid #ccc;border:1px solid rgba(0,0,0,.2);border-radius:6px;-webkit-box-shadow:0 5px 10px rgba(0,0,0,.2);box-shadow:0 5px 10px rgba(0,0,0,.2);line-break:auto}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="page-header">
                    <h2>运费模版</h2>
    </div>
        <form runat="server" id="thisForm">
    
         <!--分页功能-->
          <%-- <div class="select-page clearfix">
                    <div class="form-horizontal fl">
                       
                   </div>

                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server"  ShowTotalPages="true" ID="pager" />
                      </div>
                    </div>
                    </div>
        </div>--%>

  
             <div class="form-group mar forced">
                            <div class="checkbox">
                                <label><input type="checkbox" name="selall"   onclick="javascript: SelectAllNew(this);">全选</label>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                              <%--  <Hi:ImageLinkButton ID="lkbDeleteCheck" class="btn resetSize btn-danger" runat="server" Text="批量删除" IsShow="true"/>
                           --%>
                                 <asp:Button ID="lkbDeleteCheck" runat="server" Text="批量删除" IsShow="true" class="btn resetSize btn-danger"    OnClientClick="return HiConform('<strong>确定要删除选择的模板吗？</strong><p>删除模板不可恢复！</p>',this)" ToolTip="" /> 
                                　|　 <a href="AddShippingTemplate.aspx" class="btn  resetSize btn-success">新建运费模板</a>
                                 </div>
                        </div>

    <!--数据列表区域-->
	  <div class="datalist">
     
           <table class="table table-hover mar table-bordered"<%-- style="table-layout:fixed"--%>>
                        <tbody>
                            <!-- DATAITEM -start-->
                            <asp:Repeater runat="server" ID="ListTemplates" OnItemDataBound="rptypelist_ItemDataBound">
                                <ItemTemplate>
                           <tr class="rows_title">
                                <th colspan="4" class="tdleft"><input type="checkbox" name="CheckBoxGroup" value="<%# Eval("TemplateId") %>">&nbsp;&nbsp;&nbsp;<%# Eval("Name") %>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class="HassFree"><%# (bool)Eval("HasFree")==true?"包含指定包邮信息":"" %><span class="HassFree"><%# (bool)Eval("FreeShip")==true?"全国包邮":"" %></span></th>
                                <th colspan="2" style="text-align:right;padding-right:10px">
                                    <a class="btn btn-info btn-xs" href="EditShippingTemplate.aspx?TemplateId=<%# Eval("TemplateId") %>">修改</a>　
                                   <%-- <asp:LinkButton Text="删除" OnClientClick="return confirm('确认删除模板，删除后不可恢复？')"  CssClass="btn btn-danger btn-xs" OnClick="DeleteShiper"  runat="server"  CommandArgument='<%#Eval("TemplateId") %>'   />--%>
                                 <asp:Button ID="lkDelete" runat="server" Text="删除" CommandName="Delete" CssClass="btn btn-danger btn-xs" CommandArgument='<%#Eval("TemplateId") %>' OnClick="DeleteShiper"  OnClientClick="return HiConform('<strong>确定要删除选择的模板吗？</strong><p>删除后不可恢复！</p>',this)" ToolTip="" /> 
                                </th>
                            </tr>
                           <asp:Repeater runat="server" ID="ListShipper">
                              <HeaderTemplate>
                                <tr>
                                <th width="90">运送方式</th>
                                <th style="width:500px">可运送至</th>
                                <th width="100">首(<%# SettingsHelper.getMUnitText((int)DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "MUnit")) %>)</th>
                                <th width="100">运费</th>
                                <th width="100">每增加(<%# SettingsHelper.getMUnitText((int)DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "MUnit")) %>)</th>
                                <th width="100">增加运费</th>
                                </tr>
                              </HeaderTemplate> 
                            <ItemTemplate>  
                                <tr>
                                <td><%# SettingsHelper.getShippingTypeByModeId((int)Eval("ModeId")) %></td>
                                <td class="Regions" tabindex="0" data-trigger="focus" role="button" data-toggle="popover" data-container="body" data-placement="bottom" title="地址详情"   data-content="<%# getRegionNamesByIds(Eval("RegionIds")==null?"":Eval("RegionIds").ToString()) %>"  ><%# SettingsHelper.getDefaultShipText((bool)Eval("IsDefault"))  %></td>
                                <td><%# (int)DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "MUnit")==1?Eval("FristNumber","{0:f0}"):Eval("FristNumber") %></td>
                                <td><%# Math.Round((decimal)Eval("FristPrice"),2) %>元</td>
                                <td ><%# (int)DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "MUnit")==1?Eval("AddNumber","{0:f0}"):Eval("AddNumber") %></td>
                                <td><%# Math.Round((decimal)Eval("AddPrice"),2) %>元</td>
                               </tr>
                            </ItemTemplate>  
                           </asp:Repeater>  
                            <tr >
                                 <td colspan="6" style="border:1px solid #fff;border-bottom:1px solid #ddd"></td>
                            </tr>

                                </ItemTemplate>
                            </asp:Repeater>
                           
                             <!-- DATAITEM -end-->

                        </tbody>
                    </table>


           <!--数据列表底部功能区域-->
        <div class="select-page clearfix" style="margin-top:10px"  ID="TablefFooter" runat="server" >
                    <div class="form-horizontal fl">
                      
                    </div>
                    <div  class="page fr">
                         <div class="pageNumber">
                        <div class="pagination" style="margin:0px">
                        <UI:Pager runat="server" ShowTotalPages="true" ID="pager1" />
                       </div>
                      </div>
                    </div>
                </div>

      <div class="blank5 clearfix"></div>
	  </div>

    </form>


     <script>

         function SelectAllNew(obj) {
             $("[name=CheckBoxGroup]").prop("checked", $(obj).get(0).checked);
             //.attr("checked", $(obj).get(0).checked);//这种方式会有异常，只能执行一次，
         }


         $(function () {
             $(".Regions").each(function () {
               

                 
                 var regionsNames = $(this).attr("data-content");
                
                 if (regionsNames.length > 0) regionsNames = regionsNames.substring(0, regionsNames.length - 1);

                 if (regionsNames != null && regionsNames != "") {
                     var regionsArray = regionsNames.split("，");
                     var showHtml = "";
                     regionsNames="";
                     var provHash = {};
                     for (var i = 0; i < regionsArray.length; i++) {
                         if(regionsArray[i].trim()!=""){
                             var citys = regionsArray[i].trim().split("-");
                             if (!provHash[citys[0]]) {
                                 provHash[citys[0]] = { num: 1, str: citys[1] };
                             } else {
                                 provHash[citys[0]].num = provHash[citys[0]].num * 1 + 1;
                                 provHash[citys[0]].str += ","+citys[1];
                             }
                         }
                     }



                     for (key in provHash) {
                         if (showHtml == "") {
                             showHtml = key + "(" + provHash[key].num + ")";
                             regionsNames = key + "【" + provHash[key].str + "】";
                         } else {
                             showHtml += "，" + key + "(" + provHash[key].num + ")";
                             regionsNames += "<br\><br\>" + key + "【" + provHash[key].str + "】";
                         }  
                     }

                     $(this).attr("data-content", regionsNames);
                     $(this).html(showHtml);
                 }
             });


             $('[data-toggle="popover"]').each(function () {
                 if ($(this).attr("data-content")!="") {
                     $(this).popover({
                         html: true
                     });
                 } else {
                     $(this).removeAttr("role");
                     $(this).removeAttr("tabindex");
                 }
             });
         });
   </script>
</asp:Content>
