/*!
 * HiShop Template Edit Config 
 * version: 1.0
 * build: Tue Aug 11 2015 11:16
 * author: CJZhao
 */

$(function () {
    HiShop.Goods = HiShop.Goods ? HiShop.Goods : {}; //Convert 命名空间
    /*实时获取商品列表  新增时*/
    HiShop.Goods.GoodsList = function (data) {

        $.ajax({
            url: HiShop.Config.CodeBehind.getGoodGroupUrl,
            type: "post",
            dataType: "json",
            //async: false,
            data: {
                ShowPrice: data.content.showPrice,
                Layout: data.content.layout,
                showName: data.content.showName,
                ShowIco: data.content.showIco,
                GoodListSize: data.content.goodsize,
                FirstPriority: data.content.firstPriority,
                SecondPriority: data.content.secondPriority
            },
            success: function (data) {
                var mouldeData = dataInit(5);
                if (data.goodslist.length > 0) {
                    mouldeData.content = data;
                }
                //HiShop.DIY.add(mouldeData, true);
                var $actionPanel = insertDom(mouldeData);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var mouldeData = dataInit(5);
                var $actionPanel = insertDom(mouldeData);
                return goodslist;
            }
        })
    }



    HiShop.Goods.GoodsListBind = function (data) {

        $.ajax({
            url: HiShop.Config.CodeBehind.getGoodGroupUrl,
            type: "post",
            dataType: "json",
            async: false,
            data: {
                ShowPrice: data.content.showPrice,
                Layout: data.content.layout,
                showName: data.content.showName,
                ShowIco: data.content.showIco,
                GoodListSize: data.content.goodsize,
                FirstPriority: data.content.firstPriority,
                SecondPriority: data.content.secondPriority
            },
            success: function (data) {
                //alert(data.goodslist.length)
                var mouldeData = dataInit(5);
                if (data.goodslist.length > 0) {
                    mouldeData.content = data;
                }
                HiShop.DIY.add(mouldeData, true);
               
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var mouldeData = dataInit(5);
                HiShop.DIY.add(mouldeData, true);
                
            }
        })
    }


    HiShop.Goods.GoodsEvents = function (data,callback) {

        $.ajax({
            url: HiShop.Config.CodeBehind.getGoodGroupUrl,
            type: "post",
            dataType: "json",
            //async: false,
            data: {
                ShowPrice: data.content.showPrice,
                Layout: data.content.layout,
                showName: data.content.showName,
                ShowIco: data.content.showIco,
                GoodListSize: data.content.goodsize,
                FirstPriority: data.content.firstPriority,
                SecondPriority: data.content.secondPriority
            },
            success: function (data) {
                //alert(data.goodslist.length)
                var mouldeData = dataInit(5);
                if (data.goodslist.length > 0) {
                    mouldeData.content = data;
                }
                callback(mouldeData)
                //return mouldeData;

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var mouldeData = dataInit(5);
                //return mouldeData;

            }
        })
    }
});