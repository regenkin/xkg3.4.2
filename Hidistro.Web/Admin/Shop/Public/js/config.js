/*!
 * HiShop Template Edit Config 
 * version: 1.0
 * build: Tue Aug 11 2015 11:16
 * author: CJZhao
 */

$(function () {
    HiShop.Config = HiShop.Config ? HiShop.Config : {}; //Config 命名空间
    /*!此配置路径在同一站点下，请求
     * 跨域请求就需要改动JS源码 换成dataType换成jsonp  后台返回需要加上callback()
     */
    HiShop.Config.AjaxUrl = {
        /*资源相关 start*/
        getFolderTree: "/Admin/shop/api/Hi_Ajax_GetFolderTree.ashx",//获取图片分类菜单
        getImgList: "/Admin/shop/api/Hi_Ajax_GetImgList.ashx",//获取图片列表
        addImg: "/hieditor/ueditor/net/controller.ashx?action=uploadtemplateimage",//上传图片
        moveImg: "/Admin/shop/api/Hi_Ajax_MoveImg.ashx",//移动图片  将图片移动到某个分类
        delImg: "/Admin/shop/api/Hi_Ajax_DelImg.ashx",//删除图片
        addFolder: "/Admin/shop/api/Hi_Ajax_AddFolder.ashx",//添加图片分类
        renameFolder: "/Admin/shop/api/Hi_Ajax_RenameFolder.ashx",//重命名图片分类
        delFolder: "/Admin/shop/api/Hi_Ajax_DelFolder.ashx",//删除图片分类
        moveCateImg: "/Admin/shop/api/Hi_Ajax_RemoveImgByFolder.ashx",//整个分类移动 
        renameImg: "/Admin/shop/api/Hi_Ajax_RenameImg.ashx",    //重命名图片名称
        /*资源相关 end*/
        /*页面操作相关  start*/
        savePage: "/Admin/shop/api/Hi_Ajax_SaveTemplate.ashx",//保存模板
        pageRecover: "/Admin/shop/api/Hi_Ajax_RenameFolder.ashx",//还原模板
        getPage: "/Admin/shop/api/Hi_Ajax_GetTemplateByID.ashx",//获取模板（用户店铺） 
        /*页面操作相关  end*/
        Coupons: "/Admin/shop/api/Hi_Ajax_Coupons.ashx",//优惠券
        Categories: "/Admin/shop/api/Hi_Ajax_Categories.ashx",//分类
        PointExChanges: "/Admin/shop/api/Hi_Ajax_PointExChange.ashx",//积分兑换
        Votes: "/Admin/shop/api/Hi_Ajax_Votes.ashx",//问卷调查
        Brands: "/Admin/shop/api/Hi_Ajax_Brands.ashx",//问卷调查
        Graphics: "/Admin/shop/api/Hi_Ajax_Graphics.ashx",//图文素材
        /*商品相关 start*/
        goodsList: "/Admin/shop/api/Hi_Ajax_GetItems.ashx",//商品列表
        goodGroup: "/Admin/shop/api/Hi_Ajax_GoodsGourp.ashx",//商品分类
        /*商品相关 end*/
        gamesUrl: "/Admin/shop/api/Hi_Ajax_GetGames.ashx"
    }

    HiShop.Config.CodeBehind = {
        /*解析商品分组数据接口*/
        getGoodGroupUrl: "/api/Hi_Ajax_GoodsListGroup.ashx",//解析商品分组  
        /*解析商品数据接口*/
        getGoodUrl: "/api/Hi_Ajax_GoodsList.ashx"//解析商品 
      
    }
    HiShop.Config.HiTempLatePath = {
        GroupGoodTemp: "/Admin/shop/Modules/GoodGroup",
        TemplateExt:".cshtml"

    }
    



});