var BrowserDetect = {
	init: function () {
		this.browser = this.searchString(this.dataBrowser) || "An unknown browser";
		this.version = this.searchVersion(navigator.userAgent)
			|| this.searchVersion(navigator.appVersion)
			|| "an unknown version";
		this.OS = this.searchString(this.dataOS) || "an unknown OS";
	},
	searchString: function (data) {
		for (var i=0;i<data.length;i++)	{
			var dataString = data[i].string;
			var dataProp = data[i].prop;
			this.versionSearchString = data[i].versionSearch || data[i].identity;
			if (dataString) {
				if (dataString.indexOf(data[i].subString) != -1)
					return data[i].identity;
			}
			else if (dataProp)
				return data[i].identity;
		}
	},
	searchVersion: function (dataString) {
		var index = dataString.indexOf(this.versionSearchString);
		if (index == -1) return;
		return parseFloat(dataString.substring(index+this.versionSearchString.length+1));
	},
	dataBrowser: [
		{
			string: navigator.userAgent,
			subString: "Chrome",
			identity: "Chrome"
		},
		{ 	string: navigator.userAgent,
			subString: "OmniWeb",
			versionSearch: "OmniWeb/",
			identity: "OmniWeb"
		},
		{
			string: navigator.vendor,
			subString: "Apple",
			identity: "Safari",
			versionSearch: "Version"
		},
		{
			prop: window.opera,
			identity: "Opera",
			versionSearch: "Version"
		},
		{
			string: navigator.vendor,
			subString: "iCab",
			identity: "iCab"
		},
		{
			string: navigator.vendor,
			subString: "KDE",
			identity: "Konqueror"
		},
		{
			string: navigator.userAgent,
			subString: "Firefox",
			identity: "Firefox"
		},
		{
			string: navigator.vendor,
			subString: "Camino",
			identity: "Camino"
		},
		{		// for newer Netscapes (6+)
			string: navigator.userAgent,
			subString: "Netscape",
			identity: "Netscape"
		},
		{
			string: navigator.userAgent,
			subString: "MSIE",
			identity: "Internet Explorer",
			versionSearch: "MSIE"
		},
		{
			string: navigator.userAgent,
			subString: "Gecko",
			identity: "Mozilla",
			versionSearch: "rv"
		},
		{ 		// for older Netscapes (4-)
			string: navigator.userAgent,
			subString: "Mozilla",
			identity: "Netscape",
			versionSearch: "Mozilla"
		}
	],
	dataOS : [
		{
			string: navigator.platform,
			subString: "Win",
			identity: "Windows"
		},
		{
			string: navigator.platform,
			subString: "Mac",
			identity: "Mac"
		},
		{
			   string: navigator.userAgent,
			   subString: "iPhone",
			   identity: "iPhone/iPod"
	    },
		{
			string: navigator.platform,
			subString: "Linux",
			identity: "Linux"
		}
	]

};
BrowserDetect.init();
window.onload = function () {
    if (BrowserDetect.browser == "Internet Explorer" && BrowserDetect.version <= 8) {
        var oDiv = document.createElement('div');
        var oHeader = document.getElementsByTagName('header')[0];
        oDiv.setAttribute("style", "height:30px; line-height:30px;text-align:center; background-color:#FFFBDD;color:#C81522;");
        oDiv.innerHTML = '<p>您的浏览器版本太低了，为了更好的系统体验，建议您升级IE浏览器或者下载谷歌浏览器（<a href="https://www.baidu.com/s?ie=utf-8&f=3&rsv_bp=0&rsv_idx=1&tn=baidu&wd=%E8%B0%B7%E6%AD%8C%E6%B5%8F%E8%A7%88%E5%99%A8&rsv_pq=ebde107900022645&rsv_t=c61ckUeIP5leDGJ%2FukI2Uu5FEW4w%2BMOPIGZctM8UY%2F5kZ5dQvSkByAv1PWg&rsv_enter=1&rsv_sug3=4&rsv_sug1=4&rsv_sug2=0&prefixsug=guge&rsp=1&inputT=3574&rsv_sug4=3574" target="_blank" style="color:red;">点击下载</a>）</p><span></span>';
        if (oHeader) {
            oHeader.insertBefore(oDiv, oHeader.childNodes[0]);
            var aDiv = document.getElementsByTagName('div'),
                oContainer = null;
            for (var i = 0; i < aDiv.length; i++) {
                if (aDiv[i].className == 'container') {
                    oContainer = aDiv[i];
                    continue;
                }
            }
            oContainer.style.marginTop = '110px';
        } else {
            var oBody = document.getElementsByTagName('body')[0];
            oBody.insertBefore(oDiv, oBody.childNodes[0]);
        }
    }
}