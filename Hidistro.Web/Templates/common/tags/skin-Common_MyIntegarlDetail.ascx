<%@ Control Language="C#" %>
<%@ Import Namespace="Hidistro.Core" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<tr>
    <td width="60%;" style="text-align:left;"> 
        <p><%#Eval("GoToUrl").ToString()==""?Eval("IntegralSource"):"<a href=\""+Eval("GoToUrl")+"\"><em class=\"blue\">"+Eval("IntegralSource")+"</em></a>" %></p>
        <p class="ccc"><%#Eval("TrateTime") %></p>
    </td>
    <td width="20%"><%#Convert.ToDecimal( Eval("IntegralChange"))>0?"<em class=\"red\">+"+Eval("IntegralChange","{0:f0}")+"</em>":"<em class=\"ye\">"+Eval("IntegralChange","{0:f0}")+"</em>" %></td>
    <td width="20%"><%#Eval("Remark") %></td>
</tr>
