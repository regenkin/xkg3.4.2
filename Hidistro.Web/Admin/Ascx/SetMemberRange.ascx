<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetMemberRange.ascx.cs" Inherits="Hidistro.UI.Web.Admin.SetMemberRange" %>

<script type="text/javascript">
    $(document).ready(function () {
        readMemberCustomGroupsAndGrade();                
    });
    //加载会员分组以及等级
    function readMemberCustomGroupsAndGrade() {
        $.ajax({
            type: "post",
            url: "../promotion/GetMemberGradesHandler.ashx?action=getusercustomgroupandgrade",
            data: {},
            dataType: "json",
            success: function (data) {
                if (data.type == "success") {                    
                    var resultArr = data.data;//自定义分组
                    $('#memberCustomGroupdiv').empty();
                    var shtml = '';
                    $.each(resultArr, function (i, result) {
                        if (i == 0)
                            shtml += '<label class="mr20"><input type="checkbox" name="member" class="CustomGroup" onclick="setCheckText()" value="' + result.id + '" />' + result.Name + '</label>';
                        else
                            shtml += '<label class="mr20"><input type="checkbox" name="member" class="CustomGroup" onclick="setCheckText()" value="' + result.id + '" style="margin-left:10px;" />' + result.Name + '</label>';
                    });
                    $('#memberCustomGroupdiv').append(shtml);
                    $('#memberCustomGroupdiv').find('input').each(function () {
                        $(this).click(function () {
                            if ($(this).attr('checked') == "checked") {
                                $(this).removeAttr('checked');
                            }
                            else {
                                $(this).attr('checked', 'checked');
                            }

                            var hasChecked = $("[name='member'][checked]").length;
                            var all = $("[name='member']").length;
                            if (hasChecked == all) {
                                $('#allmember').attr('checked', 'checked');
                                $('#allmember').prop("checked", true);
                            }
                            else {
                                $('#allmember').removeAttr('checked');
                                $('#allmember').prop("checked", false);
                            }
                        });
                    });
                    $('#memberDefualtGroupdiv').find('input').each(function () {
                        $(this).click(function () {
                            if ($(this).attr('checked') == "checked") {
                                $(this).removeAttr('checked');
                            }
                            else {
                                $(this).attr('checked', 'checked');
                            }

                            var hasChecked = $("[name='member'][checked]").length;
                            var all = $("[name='member']").length;
                            if (hasChecked == all) {
                                $('#allmember').attr('checked', 'checked');
                                $('#allmember').prop("checked", true);
                            }
                            else {
                                $('#allmember').removeAttr('checked');
                                $('#allmember').prop("checked", false);
                            }
                        });
                    });
                    if ('<%=DefualtGroup%>' == '0') {
                        $('#memberDefualtGroupdiv').find('input[type="checkbox"]').each(function () {
                            $(this).prop('checked', true);
                        });
                    } else {
                        setDefualtGroupCheck();
                    }
                    if ('<%=CustomGroup%>' == '0') {
                        $('#memberCustomGroupdiv').find('input[type="checkbox"]').each(function () {
                            $(this).prop('checked', true);
                        });
                    } else {
                        setCustomGroupCheck();
                    }
                    
                    var resultGradeArr = data.gradedata;//加载会员等级
                    $('#memberGradediv').empty();
                    var shtml = '';
                    $.each(resultGradeArr, function (i, result) {
                        if (i == 0)
                            shtml += '<label class="mr20"><input type="checkbox" name="member" class="Grade" value="' + result.GradeId + '" onclick="setCheckText()" />' + result.Name + '</label>';
                        else
                            shtml += '<label class="mr20"><input type="checkbox" name="member" class="Grade" value="' + result.GradeId + '" onclick="setCheckText()" style="margin-left:10px;" />' + result.Name + '</label>';
                    });
                    $('#memberGradediv').append(shtml);
                    $('#memberGradediv').find('input').each(function () {
                        $(this).click(function () {
                            if ($(this).attr('checked') == "checked") {
                                $(this).removeAttr('checked');
                            }
                            else {
                                $(this).attr('checked', 'checked');
                            }

                            var hasChecked = $("[name='member'][checked]").length;
                            var all = $("[name='member']").length;
                            if (hasChecked == all) {
                                $('#allmember').attr('checked', 'checked');
                                $('#allmember').prop("checked", true);
                            }
                            else {
                                $('#allmember').removeAttr('checked');
                                $('#allmember').prop("checked", false);
                            }
                        });
                    });
                    if ('<%=Grade%>' != '0') {
                        //修改时 加载数据
                        setGradeCheck();
                    } else {
                        $('#memberGradediv').find('input[type="checkbox"]').each(function () {
                            $(this).prop('checked', true);
                        });
                    }


                } else {
                    ShowMsg("加载会员分组失败!");
                }
            }
        });
        SetCheckStauts();
    }

    //设置全选按钮
    function SetCheckStauts() {
        $('#allmember').click(function () {
            if ($(this).attr('checked') == "checked") {
                $(this).removeAttr('checked');
            }
            else {
                $(this).attr('checked', 'checked');
            }
            var checked = $(this).attr('checked');
            $("[name='member']").each(function () {
                if (checked == "checked") {
                    $(this).attr('checked', 'checked');
                    $(this).prop("checked", true);
                }
                else {
                    $(this).removeAttr('checked');
                    $(this).prop("checked", false);
                }
            });
        });

       if ('<%=Grade%>' == "0" && '<%=DefualtGroup%>' == "0" && '<%=CustomGroup%>' == "0") {
            $('#allmember').prop('checked', true);
            $('#allmember').attr('checked', 'checked');
        }
        else {
            $('#allmember').prop('checked', false);
        }
    }

    //加载默认分组选中状态
    function setDefualtGroupCheck() {
        var DefualtGroup = '<%=DefualtGroup%>';
        if (DefualtGroup != "-1") {
            $('#memberDefualtGroupdiv').find('input[type="checkbox"]').each(function () {
                var vl = $(this).val();
                if (DefualtGroup == "0") {
                    $(this).prop('checked', true);
                }
                else {
                    if ((',' + DefualtGroup + ',').indexOf(',' + vl + ',') >= 0) {
                        $(this).prop('checked', true);
                    }
                }
            });
        }
    }

    //加载自定义分组选中状态
    function setCustomGroupCheck() {
        var CustomGroup = '<%=CustomGroup%>';
        if (CustomGroup != "-1") {
            $('#memberCustomGroupdiv').find('input[type="checkbox"]').each(function () {
                var vl = $(this).val();
                if (CustomGroup == "0") {
                    $(this).prop('checked', true);
                }
                else {
                    if ((',' + CustomGroup + ',').indexOf(',' + vl + ',') >= 0) {
                        $(this).prop('checked', true);
                    }
                }
            });
        }
    }

    //加载会员等级选中状态
    function setGradeCheck() {
        var Grades = '<%=Grade%>';
        if (Grades != "-1") {
            $('#memberGradediv').find('input[type="checkbox"]').each(function () {
                var vl = $(this).val();
                if (Grades == "0") {
                    $(this).prop('checked', true);
                }
                else {
                    if ((',' + Grades + ',').indexOf(',' + vl + ',') >= 0) {
                        $(this).prop('checked', true);
                    }
                }
            });
        }
    }

    //获得选中的会员范围
    function setCheckText() {
        var grade = "";
        var DefualtGroup = "";
        var CustomGroup = "";

        //会员等级
        var checkGrade = $(".Grade:checked");
        if (checkGrade.size() == 0) {
            grade = "-1";
        } else if (checkGrade.size() == $(".Grade").size()) {
            grade = "0";
        } else {
            checkGrade.each(function () {
                grade += $(this).val() + ',';
            });
            grade = grade.substring(0, grade.length - 1);
        }
        //默认分组
        var checkDefualtGroup = $(".DefualtGroup:checked");
        if (checkDefualtGroup.size() == 0) {
            DefualtGroup = "-1";
        } else if (checkDefualtGroup.size() == $(".DefualtGroup").size()) {
            DefualtGroup = "0";
        } else {
            checkDefualtGroup.each(function () {
                DefualtGroup += $(this).val() + ',';
            });
            DefualtGroup = DefualtGroup.substring(0, DefualtGroup.length - 1);
        }
        //自定义分组
        var checkCustomGroup = $(".CustomGroup:checked");
        if (checkCustomGroup.size() == 0) {
            CustomGroup = "-1";
        } else {
            checkCustomGroup.each(function () {
                CustomGroup += $(this).val() + ',';
            });
            CustomGroup = CustomGroup.substring(0, CustomGroup.length - 1);
        }

        $('#<%=txt_Grades.ClientID%>').val(grade);
        $('#<%=txt_DefualtGroup.ClientID%>').val(DefualtGroup);
        $('#<%=txt_CustomGroup.ClientID%>').val(CustomGroup);
    }

    //全选操作
    function setAllmemberCheckText() {
        var grade = "";
        var DefualtGroup = "";
        var CustomGroup = "";

        if ($('#allmember').prop('checked')) {
            grade = "0";
            DefualtGroup = "0";
            //CustomGroup = "0";

            var checkCustomGroup = $(".CustomGroup").each(function () {
                CustomGroup += $(this).val() + ',';
            });
            if (CustomGroup.length > 1) {
                CustomGroup = CustomGroup.substring(0, CustomGroup.length - 1);
            } else {
                CustomGroup = "-1";
            }
        } else {
            grade = "-1";
            DefualtGroup = "-1";
            CustomGroup = "-1";
        }
        $('#<%=txt_Grades.ClientID%>').val(grade);
        $('#<%=txt_DefualtGroup.ClientID%>').val(DefualtGroup);
        $('#<%=txt_CustomGroup.ClientID%>').val(CustomGroup);
    }
</script>
<style>
    #allmemberdiv input,#memberdiv input{
        margin: -2px 0 0 0;
        vertical-align: middle;
        margin-right: 3px;
    }
    #memberDefualtGroupdiv,#memberCustomGroupdiv {
    margin-top:5px;
    }
</style>

<div id="allmemberdiv" class="resetradio mb5 pt3">
    <label class="mr20">
        <input type="checkbox" value="0" id="allmember" name="allmember" onclick="setAllmemberCheckText()" />全部会员</label>
</div>
<div style="height: 10px;"></div>
<div id="memberdiv" class="resetradio mb5 pt3">
    <div id="memberGradediv">
    </div>
    <div style="height: 8px;"></div>
    <div id="memberDefualtGroupdiv">
        <label class="mr20">
            <input type="checkbox" name="member" class="DefualtGroup" value="1" onclick="setCheckText()">新会员</label>
        <label class="mr20">
            <input type="checkbox" name="member" class="DefualtGroup" value="2" onclick="setCheckText()" style="margin-left: 10px;">活跃会员</label>
        <label class="mr20">
            <input type="checkbox" name="member" class="DefualtGroup" value="3" onclick="setCheckText()" style="margin-left: 10px;">沉睡会员</label>
    </div>
    <div style="height: 8px;"></div>
    <div id="memberCustomGroupdiv">
    </div>
</div>
<asp:HiddenField ID="txt_Grades" runat="server" Value="-1" />
<asp:HiddenField ID="txt_DefualtGroup" runat="server" Value="-1" />
<asp:HiddenField ID="txt_CustomGroup" runat="server" Value="-1" />
