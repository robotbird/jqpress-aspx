/*
 * xzsywToos1.0
 *
 * Copyright (c) 2011 yepeng (jqbird.com)
 * Dual licensed under the MIT (MIT-LICENSE.txt)
 * and GPL (GPL-LICENSE.txt) licenses.
 *
 */
(function($) {

    //插件名称：暂无图片
    jQuery.fn.extend({
        noImg: function(noImgsrc) {
            var img = $("img");
            jQuery.each(img, function(i, n) {
                if (n.src == "" || n.src == "${siteurl}" || n.src.indexOf(".") < 0 || n.src == "http://www.xzsyw.com/") {
                    n.src = noImgsrc;
                }
            });
        }
    });

    //插件名称：Tab选项卡
    jQuery.fn.extend({
        xzsywTab: function(options) {
            //参数和默认值
            var defaults = {
                _tabClass: null,
                // _parents: null,
                _childs: null
            };
            var options = $.extend(defaults, options);
            var o = options;

            var parentCate = jQuery(this);
            var childCate = jQuery(o._childs);

            parentCate.mouseover(function() {
                parentCate.removeClass(o._tabClass);

                jQuery(this).addClass(o._tabClass);
                for (i = 0; i < parentCate.length; i++) {
                    if (parentCate != undefined) {
                        if (parentCate[i].className == o._tabClass) {
                            if (childCate[i] != undefined) {
                                childCate[i].style.display = "block";
                            }

                        } else {
                            if (childCate[i] != undefined) {
                                childCate[i].style.display = "none";
                            }
                        }
                    }

                }

            });
        }
    });

})(jQuery);



/*
* 支付操作类接口 依赖于jquery1.3.2以上
* showbanklist: function(bankid){};//显示所有支付银行
*/

//银行操作js类
var jqForm;
if (!jqForm) {

    //ajax请求当前页面
    var ajaxurl = "";

    jqForm = {
        //获取json数据
        getJsonData: function(id) {
        }, //---
        //删除操作
        deleteRow: function(id) {
        },//---
        //初始化表单
        InitForm: function() {
        },//---
        //初始化表单
        SubmitForm: function() {
        }//---

}//--

} //-