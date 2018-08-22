(function ($) {
    $.fn.extend({
        'formvalidation': function (obj) {

            var that = this;
            var $submit = null;
 
            if (obj["submit"] == null) {
                $submit = $(this).find("input[type=submit]");
                obj["submit"] = "";
            }
            else {
                $submit = $(obj["submit"]);
            }


            //获取默认值
            $(this).find(':input').each(function () {
                var inputname = $(this).attr("name");

                if (obj[inputname] != null) {
                    var nextmsg = $(this).next().text().trim();
                    if (nextmsg != "") {
                        $(this).attr("content", nextmsg);
                    }       
                };;

            });
 

            $(this).find(':input').on('blur', function () {
                $(this).keyupValFn(obj);
            });
            $(this).find(':checkbox').on('change', function () {
                $(this).changValFn(obj)
            })
            $(this).find(':radio').on('change', function () {
                $(this).changValFn(obj)
            });

            if ($submit.length > 0) {

                $submit.click(function () {
                    if (!$(that).IsValid()) {
                        return false;
                    }
                });
            }

        },
        'IsValid': function () {
            $(this).find(":input").trigger("blur");
            $(this).find(':radio').trigger('change');
            $(this).find(':checkbox').trigger('change');


            var numError = $(this).find('.has-error').length;
            //$(this).find('.has-error').each(function () {

            //    alert($(this).parent().html());
            //});
            //alert(numError);
            if (numError) {
                return false;
            }
            return true;
        },
        'keyupValFn': function (obj) {

            var inputName = $(this).attr('name');

            var objName = obj[inputName];

            if (objName) {



                var objVal = objName['validators'];

                if ($(this).val() != "") {

                    if (objVal['notEmpty']) {
                        $(this).successFn(null, obj["submit"]);
                    }

                    if (objVal['repeatPass']) {
                        if ($(this).val() != $(this).parents('.form-group').prev().find('input').val()) {
                            $(this).valError(objVal['repeatPass']['message'], obj["submit"]);
                        } else {
                            $(this).successFn(objVal['repeatPass']['message'], obj["submit"]);
                        }
                    }

                    if (objVal['stringLength'] && !objVal['regexp']) {
                        if ($(this).val().length < objVal['stringLength']['min'] || $(this).val().length > objVal['stringLength']['max']) {
                            $(this).valError(objVal['stringLength']['message'], obj["submit"]);
                        } else {
                            $(this).successFn(objVal['stringLength']['message'], obj["submit"]);
                        }
                    }
                    if (objVal['regexp'] && !objVal['stringLength']) {
                  
                        if (!objVal['regexp']['regexp'].test($(this).val())) {
                            $(this).valError(objVal['regexp']['message'], obj["submit"]);
                        } else {
                            $(this).successFn(objVal['regexp']['message'], obj["submit"]);
                        }
                    }
                    if (objVal['regexp'] && objVal['stringLength']) {
                        if ($(this).val().length < objVal['stringLength']['min'] || $(this).val().length > objVal['stringLength']['max'] || !objVal['regexp']['regexp'].test($(this).val())) {
                            $(this).valError(objVal['regexp']['message'] + objVal['stringLength']['message'], obj["submit"]);
                        } else {
                            $(this).successFn(objVal['regexp']['message'] + objVal['stringLength']['message'], obj["submit"]);
                        }
                    }
                   
                    if (objVal['tell']) {

                        if (! /(^[0-9]{3,4}\-[0-9]{7,8}(\-[0-9]{2,4})?$)|(^[0-9]{7,8}$)|(^[0-9]3,4[0-9]{3,8}$)|(^0{0,1}13[0-9]{9}$)|(13\d{9}$)|(15[01235-9]\d{8}$)|(1[4978][0-9]\d{8}$)|(^400(\-)?[0-9]{3,4}(\-)?[0-9]{3,4}$)/.test($(this).val())) {
                            $(this).valError(objVal['tell']['message'], obj["submit"]);
                        } else {
                            $(this).successFn(objVal['tell']['message'], obj["submit"]);
                        }
                    }

                   

                } else {

                    if (objVal['notEmpty']) {
                        $(this).valError(objVal['notEmpty']['message'], obj["submit"]);
                    } else {
                        $(this).successFn(null, obj["submit"]);
                    }
                }

            }
            //alert("999"+inputName);
        },
        'changValFn': function (obj) {
            var inputName = $(this).attr('name');
            var objName = obj[inputName];
            if (objName) {
                var objVal = objName['validators'];

                if (objVal['notEmpty']) {
                    var flag = false;
                    $(this).parents('.form-group').find(':input').each(function (i) {
                        if (this.checked) {
                            flag = true;
                        }
                    })
                    if (!flag) {
                        $(this).parents('.form-group').addClass('has-error');
                    } else {
                        $(this).parents('.form-group').removeClass('has-error');
                        if (!obj["submit"]) obj["submit"] = "";
                            $(this).inputDisab(obj["submit"]);
                    }
                }
            }
        },
        'valError': function (string, submitId) {

            $(this).parents('.form-group').addClass('has-error');
            if (!$(this).next().get(0)) {
                $(this).parent().append('<small class="help-block" data-bv-validator="notEmpty" data-bv-for="username" data-bv-result="NOT_VALIDATED">' + string + '</small>');
                $(this).inputDisab(submitId);
            } else {
                if ($(this).nextAll().text() != string && (!$(this)[0].getAttribute('content'))) {
                    $(this)[0].setAttribute("content", $(this).nextAll().remove().text());
                } else {
                    $(this).nextAll().remove();
                }
                $(this).parent().append('<small class="help-block" data-bv-validator="notEmpty" data-bv-for="username" data-bv-result="NOT_VALIDATED">' + string + '</small>')
            }
        },
        'successFn': function (string, submitId) {
            $(this).parents('.form-group').removeClass('has-error');
                $(this).nextAll().remove();
                if ($(this)[0].getAttribute('content')) {
                    $(this).parent().append('<small class="help-block">' + $(this)[0].getAttribute("content") + '</small>');
                }
         
            $(this).inputDisab(submitId);
        },
        'inputDisab': function (submitId) {
            var flag = true;
            $(this).parents('form').find('.form-group').each(function () {
                if ($(this).hasClass('has-error')) {
                    flag = false;
                }
            });


           var $submit = $(submitId);

            if ($submit.length < 1) {
                $submit = $(this).parents('form').find("input[type=submit]");
            };

            if ($submit.length > 0) {
                if (!flag) {
                    $submit.attr('disabled', 'disabled')
                } else {
                    $submit.removeAttr('disabled');
                }
            }
           
        }
    })
})(jQuery)