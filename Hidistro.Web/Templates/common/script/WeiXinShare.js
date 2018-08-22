function WinXinShareMessage(title,desc,link,imgUrl) {
    wx.ready(function () {

        
        //分享给朋友
        wx.onMenuShareAppMessage({
            title: title,
            desc:unescape(desc),
            link: link,
            imgUrl: imgUrl,
            trigger: function (res) {
                // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
                //alert('用户点击发送给朋友');
            },
            success: function (res) {
                //alert('已分享');
            },
            cancel: function (res) {
                //alert('已取消');
            },
            fail: function (res) {
                alert(JSON.stringify(res));
            }
        });
        //分享到朋友圈
        wx.onMenuShareTimeline({
            title: title + "\r\n" + unescape(desc),
            link: link,
            imgUrl: imgUrl,
            trigger: function (res) {
                // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
                // alert('用户点击分享到朋友圈');
            },
            success: function (res) {
                
            },
            cancel: function (res) {
               
            },
            fail: function (res) {
                alert(JSON.stringify(res));
            }
        });
        wx.onMenuShareQQ({
            title: title, // 分享标题
            desc: unescape(desc), // 分享描述
            link: link, // 分享链接
            imgUrl: imgUrl, // 分享图标
            success: function () {
                // 用户确认分享后执行的回调函数
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            }
        });
        wx.onMenuShareWeibo({
            title: title, // 分享标题
            desc: unescape(desc), // 分享描述
            link: link, // 分享链接
            imgUrl: imgUrl, // 分享图标
            success: function () {
                // 用户确认分享后执行的回调函数
            },
            cancel: function () {
                // 用户取消分享后执行的回调函数
            }
        });
    })
}