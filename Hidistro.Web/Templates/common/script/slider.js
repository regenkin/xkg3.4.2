 


function gundong(e) {
    e.stopPropagation();
    e.preventDefault();
}
function InitDom(obj) {
 
    this.warp = document.getElementById(obj.warp);
    this.ChildrenLi = this.warp.getElementsByTagName('li');
 
    this.boxHeight = window.innerHeight;
    this.boxWidth = window.innerWidth;
    this.idx = 0;
   
  
    this.InitWarp();
    this.BindEvent();
    this.foranimation(0);
}
InitDom.prototype.InitWarp = function () {
    var len = this.ChildrenLi.length;
    var ChildrenLi = this.ChildrenLi;
    var boxHeight = this.boxHeight;
	var boxWidth = this.boxWidth;
    for (var i = 0; i < len; i++) {
        ChildrenLi[i].style.webkitTransform = 'translate3d(' + i * boxWidth + 'px,0px,0px)';
    }
    ;
};

//绑定事件
InitDom.prototype.BindEvent = function () {
    var BindEventThis = this;
    var boxHeight = this.boxHeight;
    var boxWidth = this.boxWidth;
    var warp = this.warp;

    var tcstart = function (event) {//点击
        BindEventThis.startX = event.touches[0].pageX;
        BindEventThis.offsetX = 0;

    };
    var tcmove = function (event) {//移动
        event.preventDefault();
        BindEventThis.offsetX = event.touches[0].pageX - BindEventThis.startX;
        var ChildrenLi = BindEventThis.ChildrenLi;
        var i = BindEventThis.idx - 1;
        var m = i + 3;

        for (i; i < m; i++) {
            ChildrenLi[i] && (ChildrenLi[i].style.webkitTransform = 'translate3d(' + ((i - BindEventThis.idx) * boxWidth + BindEventThis.offsetX) + 'px,0px,0px)');
            ChildrenLi[i] && (ChildrenLi[i].style.webkitTransition = 'none');
        }


    };
    var tcend = function (event) {//离开
        var boundary = boxWidth / 6;
        if (BindEventThis.offsetX > boundary) {
            BindEventThis.go('-1');
        } else if (BindEventThis.offsetX < -boundary) {
            BindEventThis.go('+1');
        } else {
            BindEventThis.go('0');
        }
    };
    warp.addEventListener('touchstart', tcstart);
    warp.addEventListener('touchmove', tcmove);
    warp.addEventListener('touchend', tcend);
};

InitDom.prototype.go = function (n) {

    var idx = this.idx;
    var gid;
    var ChildrenLi = this.ChildrenLi;
    var len = ChildrenLi.length;
    var boxWidth = this.boxWidth;
    if (typeof n == 'number') {
        gid = idx;
    }
    else if (typeof n == 'string') {
        gid = idx + n * 1;
    }
    if (gid > len - 1) {
        gid = len - 1;
    }
    else if (gid < 0) {
        gid = 0;
    }
    this.idx = gid;
    ChildrenLi[gid].style.webkitTransition = '-webkit-Transform 0.3s ease-out';
    ChildrenLi[gid - 1] && (ChildrenLi[gid - 1].style.webkitTransition = '-webkit-Transform 0.3s ease-out');
    ChildrenLi[gid + 1] && (ChildrenLi[gid + 1].style.webkitTransition = '-webkit-Transform 0.3s ease-out');

    ChildrenLi[gid].style.webkitTransform = 'translate3d(0px,0px,0px)';
    ChildrenLi[gid - 1] && (ChildrenLi[gid - 1].style.webkitTransform = 'translate3d(-' + boxWidth + 'px,0px,0px)');
    ChildrenLi[gid + 1] && (ChildrenLi[gid + 1].style.webkitTransform ='translate3d(' + boxWidth + 'px,0px,0px)');

    this.foranimation(this.idx);
};



function random(min,max){
    return Math.floor(min+Math.random()*(max-min));
}
