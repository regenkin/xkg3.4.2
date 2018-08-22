<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SpecificationView.ascx.cs" Inherits="Hidistro.UI.Web.Admin.Goods.ascx.SpecificationView" %>
<%@ Import Namespace="Hidistro.Core"%>
 <%@ Register TagPrefix="UI" Namespace="ASPNET.WebControls" Assembly="ASPNET.WebControls" %>
 <%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.Common.Controls" Assembly="Hidistro.UI.Common.Controls" %>
<%@ Register TagPrefix="Hi" Namespace="Hidistro.UI.ControlPanel.Utility" Assembly="Hidistro.UI.ControlPanel.Utility" %>
   <style>
 
    .SKUValue {
        position: relative;
        float: left;
        margin-top: 3px;
        margin-left: 3px;
        margin-right: 3px;
        white-space: nowrap;
        display: block;
    }

        .SKUValue span.span1 {
            margin: 0px;
            padding: 0px;
        }

            .SKUValue span.span1 a {
                color: #000;
                border: 1px #b2b2b2 solid;
                padding: 0px 10px;
                float: left;
                margin: 0px 5px;
                white-space: nowrap;
            }

        .SKUValue span.span2 {
            position: absolute;
            top: 1px;
            right: 7px;
            width: 10px;
            height: 10px;
            overflow: hidden;
            text-indent: 30px;
            z-index: 99;
            overflow: hidden;
        }

            .SKUValue span.span2 a {
                display: block;
                background: url(../images/tOver.gif) #f2f2f2 no-repeat -8px -7px;
                cursor: pointer;
                overflow: hidden;
            }
            a {
  text-decoration: none;
  color: #0b5ba5;
}
             
        .table_title{background:#f2f2f2}
       table td,th{text-align:center}
    
</style>
  
      <div class="content">
	
            <UI:Grid ID="grdSKU" CssClass="table table-hover mar table-bordered" runat="server" ShowHeader="true" AutoGenerateColumns="false" DataKeyNames="AttributeId" HeaderStyle-CssClass="table_title" GridLines="None" Width="100%">
            <Columns>
                    <asp:TemplateField HeaderText="规格名称" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="25%">
                        <ItemTemplate>
                            规格名[<asp:Literal runat="server" ID="litUseAttributeImage" Text='<%# Eval("UseAttributeImage") %>' />]：
		                    <Hi:HtmlDecodeTextBox ID="txtSKUName" runat="server" Text='<%# Eval("AttributeName") %>' Width="70px" MaxLength="30" />
		                    <asp:Literal ID="lblDisplaySequence" runat="server" Text='<%#Eval("DisplaySequence") %>' Visible=false></asp:Literal>
		                    <asp:LinkButton ID="lbtnAdd" Text="修改"  style="color:#07D;" runat="server" CommandName="saveSKUName" CommandArgument='<%# Container.DataItemIndex  %>' />
                        </ItemTemplate>
                    </asp:TemplateField>  
                    
                      <asp:TemplateField HeaderText="规格值"  HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="35%">
                        <ItemTemplate>
                            <asp:Repeater ID="rptSKUValue" runat="server" DataSource='<%# Eval("AttributeValues") %>'>
   		                               <ItemTemplate>
   		                           <span class="SKUValue">
   		                                <span class="span1"><Hi:SKUImage ID="SKUImage1" runat="server" CssClass="a_none" ImageUrl='<%# Eval("ImageUrl")%>' ValueStr='<%# Eval("ValueStr")%>' /></span>
                                        <span class="span2"><a href="javascript:void(0)" onclick="deleteSKUValue(this, '<%# Eval("ValueId")%>', '<%# Eval("ImageUrl")%>');">删除</a></span>
                                    </span>
   		                        </ItemTemplate>
   		                    </asp:Repeater>
                        </ItemTemplate>
                    </asp:TemplateField>   
                                   
                    <UI:SortImageColumn HeaderText="排序"  ReadOnly="true" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="7%"/>
                     <asp:TemplateField HeaderText="操作" HeaderStyle-CssClass="td_right td_left" ItemStyle-Width="20%">
                         <ItemStyle CssClass="spanD spanN" />
                         <ItemTemplate>
	                         <span class="submit_tiajia"><a href="javascript:void(0)"  style="color:#07D;" onclick="ShowAddSKUValueDiv('<%# Eval("AttributeId") %>','<%# Eval("UseAttributeImage") %>', '<%# Eval("AttributeName") %>');">添加规格值</a></span>	                         	                        
 	                         <span class="submit_bianji"><asp:HyperLink  ID="lkbViewAttribute"  style="color:#07D;" runat="server" Text="编辑" NavigateUrl='<%# Globals.GetAdminAbsolutePath(string.Format("Goods/EditSpecificationValues.aspx?TypeId={0}&AttributeId={1}&UseAttributeImage={2}",Eval("TypeId"),Eval("AttributeId"),Eval("UseAttributeImage")))%>' ></asp:HyperLink></span> 
 	                         <span class="submit_dalata">
<%--                                  <Hi:ImageLinkButton runat="server" ID="lbtnDelete" CommandName="delete" IsShow="true" DeleteMsg="当前操作将彻底删该除规格及下属的所有规格值，确定吗？" Text="删除" />--%>
                                   <asp:Button ID="lbtnDelete" runat="server" IsShow="true"   Class="btnLink" Text="删除" CommandName="delete"   OnClientClick="return HiConform('<strong>当前操作将彻底删该除规格及下属的所有规格值</strong><p>删除规格及下属不可恢复！</p>',this)" ToolTip="" />

 	                         </span>

                         </ItemTemplate>
                     </asp:TemplateField> 
                                     
            </Columns>
        </UI:Grid>
    </div>
        
        <div style=" margin-top:10px; margin-bottom:10px;">
          <input type="button" onclick="AddSkuDiv()" name="button" id="button" value="添加新规格" class="btn btn-success"/>
        </div>


<%--添加新的规格--%>
 
<div class="modal fade" id="myaddSKUModal">

    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" id="attributevalue">添加新的规格</h4>
            </div>
            <div class="modal-body">
                <div class="form-horizontal" id="FormAttributeValue">
                    <div class="form-group" style="height: 30px;">
                        <label for="inputEmail3" class="col-xs-2 control-label"><em>*</em>规格名称</label>
                        <div class="col-xs-5">
                            <asp:TextBox ID="txtName" CssClass="form-control" runat="server" MaxLength="30"></asp:TextBox>
                        </div>
                         <small class="help-block">规格名称长度在1至30个字符之间</small>
                    </div>
                    <div class="form-group" style="height: 30px; display:none">
                        <label for="inputEmail3" class="col-xs-2 control-label">显示类型</label>
                        <div class="col-xs-5">
                          <Hi:UseAttributeImageRadioButtonList runat="server" ID="radIsImage" style="display:inline;" />
                        </div>
                        
                    </div>
                </div>

            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                <asp:Button ID="btnCreate" runat="server" Text="添加新规格" CssClass="btn btn-primary" />

            </div>
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>
<div class="modal fade" id="myShowAddSKUValueDivModal">

    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4 class="modal-title" >添加规格值</h4>
            </div>
            <div class="modal-body">
                <iframe src="" id="MyIframe" width="600" height="130" scrolling="no"></iframe>

            </div>
           
        </div>
        <!-- /.modal-content -->
    </div>
    <!-- /.modal-dialog -->
</div>

 
<script type="text/javascript" language="javascript">
    var formtype = "";
     //判断规格值
    

     //添加新规格
     function AddSkuDiv() {
         //addSKU
         formtype = "addsku";
         $('#myaddSKUModal').modal('toggle').children().css({
             width: '600px',
             height: '100px'
         })
         $("#myaddSKUModal").modal({ show: true });
        
     }

     //添加规格值

     function ShowAddSKUValueDiv(attributeId, useAttributeImage, attributename) {

         var Rand = Math.random();
         var pathurl = "SkuValue.aspx?action=add&attributeId=" + attributeId + "&useImg=" + useAttributeImage + "&Rand=" + Rand;
     
         $("#MyIframe").attr("src", pathurl);
        $('#myShowAddSKUValueDivModal').modal('toggle').children().css({
            width: '650px',
            height: '150px'
        })
        $("#myShowAddSKUValueDivModal").modal({ show: true });

         //if (useAttributeImage == "True") {
        //    DialogFrame(pathurl, title, 420, 200);
        //} else {
        //    DialogFrame(pathurl, title, 440, 180);
        //}
   
    }
     function closeModal(obj)
     {
         $("#myShowAddSKUValueDivModal").modal('hide');
         //location.reload();
         location.href = "EditSpecification.aspx?typeid=<%=typeId%>&t=" + (new Date()).getTime();
     }

     

    function deleteSKUValue(obj, valueId, imageUrl) {
        $.ajax({
            url: "AddSpecification.aspx",
            type: 'post', dataType: 'json', timeout: 10000,
            data: { ValueId: valueId, ImageUrl: imageUrl, isCallback: "true" },
            async: false,
            success: function(data) {
                if (data.Status == "true") 
                {
                    //location.reload();
                    location.href = "EditSpecification.aspx?typeid=<%=typeId%>&t=" + (new Date()).getTime();
                }
                else {
                    ShowMsg("此规格值有商品在使用，删除失败", false);
                }
            }
        });
    }
    
   
</script>

