<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/AdminNew.Master" AutoEventWireup="true" CodeBehind="Articles.aspx.cs" Inherits="Hidistro.UI.Web.Admin.Shop.Articles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    var reurl=location.href;
    $(document).ready(function () {
        GetArticles(<%=articletype%>,1,'<%=ArticleTitle%>');
    })
    function delOneArticle(articleid){
        $.ajax({
            url: "articles.aspx",
            type: "post",
            data: "posttype=del&id=" + articleid,
            datatype: "json",
            success: function (json) {
                if (json.type == "1") {
                    location.href=reurl;
                }
                else
                {
                    HiTipsShow(json.tips,"error");
                }
            }
        });
    }
    function editOneArticle(articleid,articletype){
        var pagename="articlesedit.aspx";
        if(articletype==4){
            pagename="multiarticlesedit.aspx";
        }
        location.href=pagename+"?id="+articleid+"&reurl="+encodeURIComponent(reurl);
    }
    function GetArticles(type, pageindex, key) {
        var url = "getarticles.aspx?pageindex="+pageindex;
        var parms = { "type": type, "key": key };
        var callBack = function () {

            setTimeout(function(){


            $('#home .mate-list').hover(function () {
                $(this).find('.nav').fadeIn(300);
            }, function () {
                $(this).find('.nav').fadeOut(300);
            });
            $('#home .mate-all').show();

            var attrTop = [0, 0, 0];
            var attrLeft = [0, 310, 620];
            $('#home .mate-list').each(function (i) {
                var index = attrTop.minIndex();
                $(this).css({
                    'top': attrTop[index],
                    'left': attrLeft[index]
                });
                attrTop[index] += $(this).height() + 15;
                
                $('#home .mate-all').height(attrTop.max());
            });
            $(".pagination a").click(function(){
                var pageurl=$(this).attr("href");
                r = pageurl.match(/pageindex=(\d+)/);
                setTimeout(function(){GetArticles(type,r[1],key)},1000);
                return false;
            });
            $(".loading").hide();
            },500);
        }
        $(".app-inner").load(url, parms, callBack);
    }
    Array.prototype.minIndex = function () {
        var min = this[0],
            index = 0,
            len = this.length,
            i = 0;
        for (i; i < len; i++) {
            if (this[i] < min) {
                min = this[i];
                index = i;
            }
        }
        return index;
    }
    Array.prototype.max = function () {
        var max = this[0],
            len = this.length,
            i = 0;
        for (i; i < len; i++) {
            if (this[i] > max) {
                max = this[i];
            }
        }
        return max;
    }
    function searchkey(){
        var key=encodeURIComponent($("#txtKey").val());
        var urlparms="<%=(articletype>0?"?articletype="+articletype:"")%>";
        if($.trim(key)!=""){
            if(urlparms!=""){
                urlparms=""+urlparms+"&key="+key;
            }else{
                urlparms="?key="+key;
            }
        }
        var url="articles.aspx"+urlparms;
        window.location.href=url;
        return false;
    }
    function searchClick(event){if(event.keyCode==13){searchkey(); return false; }}
</script>
    <script type="text/javascript" src="../weixin/GetImagesMsgId.aspx"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">   
</asp:Content>
