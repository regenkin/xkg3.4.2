<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="Hidistro.UI.Web.Admin.WeiXin.WebForm1" %>
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>消息模板设置</title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="renderer" content="webkit" />
    <link rel="icon" href="../images/hi.ico" />
    <link rel="stylesheet" href="http://apps.bdimg.com/libs/bootstrap/3.3.4/css/bootstrap.min.css" />
    <script src="http://apps.bdimg.com/libs/jquery/2.1.4/jquery.min.js" type="text/javascript"></script>
    <script src="http://apps.bdimg.com/libs/bootstrap/3.3.4/js/bootstrap.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../js/Region.js"></script>
    <link rel="stylesheet" href="../css/common.css" />
    <!--[if lt IE 9]>
      <script src="//cdn.bootcss.com/html5shiv/3.7.2/html5shiv.min.js"></script>
      <script src="//cdn.bootcss.com/respond.js/1.4.2/respond.min.js"></script>
    <![endif]-->
    <meta name="keywords" />
    <meta name="description" />
</head>
<body class="theme theme--blue">
    <header class="ui-header navbar-fixed-top">
        <div class="ui-header-inner clearfix">
            <h1 class="ui-header-logo">
                <a href="#" title="Hishop">
                    <span class="version"><i>3.0</i></span>
                </a>
            </h1>
            <nav class="ui-header-nav">
                <ul class="clearfix">
                    <li class="divide">|</li>
                    <li class="active">
                        <a onclick="ShowMenuLeft('店铺', null, null,this)">店铺</a>
                    </li>
                    <li>
                        <a onclick="ShowMenuLeft('商品', null, null, this)">商品</a>
                    </li>
                    <li>
                        <a onclick="ShowMenuLeft('订单', null, null, this)">订单</a>
                    </li>
                    <li class="divide">|</li>

                    <li><a onclick="ShowMenuLeft('会员', null, null, this)">会员</a></li>
                    <li><a onclick="ShowMenuLeft('分销', null, null, this)">分销</a></li>
                    <li class="divide">|</li>

                    <li class="js-weixin-notify ">
                        <a onclick="ShowMenuLeft('微信', null, null, this)">微信</a>
                    </li>
                    <li class="js-weibo-notify">
                        <a onclick="ShowMenuLeft('微博', null, null, this)">微博<sup class="notify-counter" style="visibility: hidden;"></sup></a>
                    </li>
                    <li class="divide">|</li>
                    <li>
                        <a onclick="ShowMenuLeft('营销', null, null, this)">营销</a>
                    </li>
                    <li class="divide">|</li>

                    <li>
                        <a onclick="ShowMenuLeft('设置', null, null, this)">设置</a>
                    </li>
                    <li>
                        <a onclick="ShowMenuLeft('分析统计', null, null, this)">分析统计</a>
                    </li>
                </ul>
            </nav>
            <div class="ui-header-user ">
                <div class="customer-wrap">
                </div>
                <div class="dropdown hover dropdown-right">
                    <a href="javascript:;" class="dropdown-toggle" data-toggle="dropdown">
                        <span class="txt">
                            <span class="ellipsis team_name" id="spTopSiteName">这里是店名</span><span class="dash"> - </span><span id="spTopUserName">admin</span>
                        </span>
                        <i class="caret"></i>
                    </a>
                    <ul class="dropdown-menu">
                        <li><a href="javascript:DialogFrame('help/about.html','关于我们',550,345)"><span class="glyphicon glyphicon-user"></span>个人信息</a></li>
                        <li><a href="javascript:DialogFrame('help/about.html','关于我们',550,345)"><span class="glyphicon glyphicon-list-alt"></span>修改密码</a></li>
                        <li><a href="javascript:DialogFrame('help/about.html','关于我们',550,345)"><span class="glyphicon glyphicon-comment"></span>关于</a></li>
                        <li class="divide"></li>
                        <li><a href="LoginExit.aspx"><span class="glyphicon glyphicon-indent-right"></span>退出</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </header>



    <div class="container nocopy">
        <div class="app">
            <div class="app-inner clearfix">
                <div class="page-header">
                    <h2>消息模板设置
                        <small class="mt5">通过接入模板消息接口，公众号可向关注其账号的用户发送预设模板的消息，以便向用户及时发送重要的服务通知。</small>
                    </h2>
                </div>
                <div class="mate-tabl">
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a href="#home" aria-controls="home" data-toggle="tab">基本设置</a></li>
                        <li role="presentation"><a href="#profile" aria-controls="profile" data-toggle="tab">消息设置</a></li>
                    </ul>
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="home">
                            <p class="y-textborderleft">您可以根据运营者角色绑定多个微信账户用于接收消息提醒</p>
                            <div class="y-tabinerboxsame mt20 mb20">
                                <button class="btn btn-success bindingmicrochannel">绑定运营者微信</button>
                                <div class="y-charttable mt10">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th width="10%">姓名</th>
                                                <th width="15%">运营者角色</th>
                                                <th width="50%">接收消息类型</th>
                                                <th width="25%">操作</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td>张三</td>
                                                <td>管理员</td>
                                                <td>
                                                    <ul class="clearfix">
                                                        <li>新订单提醒</li>
                                                        <li>订单付款提醒</li>
                                                        <li>退货申请提醒</li>
                                                        <li>提现申请提醒</li>
                                                        <li>用户咨询提醒</li>
                                                        <li>分销商申请成功提醒</li>
                                                    </ul>
                                                </td>
                                                <td>
                                                    <div class="y-messagesetting clearfix">
                                                        <div class="fl mr5">
                                                            <span class="rela">更改消息类型<i class="line"></i></span>
                                                            <div class="absol change">
                                                                <p class="mb10 admin">&nbsp;&nbsp;&nbsp;运营者角色：<input type="text"></p>
                                                                <div class="clearfix">
                                                                    <label class="middle">
                                                                        <input type="checkbox">新订单提醒
                                                                    </label>
                                                                    <label class="middle">
                                                                        <input type="checkbox">订单付款提醒
                                                                    </label>
                                                                    <label class="middle">
                                                                        <input type="checkbox">退货申请提醒
                                                                    </label>
                                                                    <label class="middle">
                                                                        <input type="checkbox">用户咨询提醒
                                                                    </label>
                                                                    <label class="middle">
                                                                        <input type="checkbox">体现申请提醒
                                                                    </label>
                                                                    <label class="middle">
                                                                        <input type="checkbox">分销商申请成功
                                                                    </label>
                                                                </div>
                                                                <div class="btn" style="text-align: center;">
                                                                    <button class="btn btn-primary btn-sm inputw100">保存</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <span class="fl">|</span>
                                                        <div class="fl ml5">
                                                            <span class="rela">取消绑定<i class="line"></i></span>
                                                            <div class="cancel">
                                                                <button class="btn btn-primary btn-sm y-setw">放弃</button>
                                                                <button class="btn btn-success btn-sm y-setw">确定</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>张三</td>
                                                <td>管理员</td>
                                                <td>
                                                    <ul class="clearfix">
                                                        <li>退货申请提醒</li>
                                                        <li>用户咨询提醒</li>
                                                    </ul>
                                                </td>
                                                <td>
                                                    <div class="y-messagesetting clearfix">
                                                        <div class="fl mr5">
                                                            <span class="rela">更改消息类型<i class="line"></i></span>
                                                            <div class="absol change">
                                                                <p class="mb10 admin">&nbsp;&nbsp;&nbsp;运营者角色：<input type="text"></p>
                                                                <div class="clearfix">
                                                                    <label class="middle">
                                                                        <input type="checkbox">退货申请提醒
                                                                    </label>
                                                                    <label class="middle">
                                                                        <input type="checkbox">用户咨询提醒
                                                                    </label>
                                                                </div>
                                                                <div class="btn" style="text-align: center;">
                                                                    <button class="btn btn-primary btn-sm inputw100">保存</button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <span class="fl">|</span>
                                                        <div class="fl ml5">
                                                            <span class="rela">取消绑定<i class="line"></i></span>
                                                            <div class="cancel">
                                                                <button class="btn btn-primary btn-sm y-setw">放弃</button>
                                                                <button class="btn btn-success btn-sm y-setw">确定</button>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                            <p class="y-textborderleft">微信消息模板ID设置<a href="javascript:void(0)">如何获得微信消息模板ID？</a></p>
                            <div class="mt20 y-charttable pb100">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th width="25%">消息标题</th>
                                            <th width="25%">模板编号</th>
                                            <th width="50%">微信模板ID</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>订单消息提醒</td>
                                            <td>OPENTM2051094909</td>
                                            <td><input class="inputw300" type="text" placeholder="输入模板ID"></td>
                                        </tr>
                                        <tr>
                                            <td>服务消息通知</td>
                                            <td>OPENTM2051094909</td>
                                            <td><input class="inputw300" type="text" placeholder="输入模板ID"></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="footer-btn navbar-fixed-bottom">
                                <button type="button" class="btn btn-success" id="btn-save">保存设置</button>
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="profile">
                            <div class="tablemessagesetting">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th align="center">消息接收对象</th>
                                            <th width="85%">消息类型</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td align="center">分销商</td>
                                            <td>
                                                <label class="middle"><input type="checkbox">订单消息提醒</label>
                                                <label class="middle"><input type="checkbox">订单付款提醒</label>
                                                <label class="middle"><input type="checkbox">密码重置提醒</label>
                                                <label class="middle"><input type="checkbox">分销等级变化提醒</label>
                                                <label class="middle"><input type="checkbox">分销商账户解冻提醒</label>
                                                <label class="middle"><input type="checkbox">提现驳回提醒</label>
                                                <label class="middle"><input type="checkbox">提现发放申请</label>
                                                <label class="middle"><input type="checkbox">佣金提醒</label>
                                                <label class="middle"><input type="checkbox">分销商取消资质提醒</label>
                                                <label class="middle"><input type="checkbox">分销商账户冻结提醒</label>
                                                <label class="middle"><input type="checkbox">新品上架提醒</label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">会员</td>
                                            <td>
                                                <label class="middle"><input type="checkbox">订单发货提醒</label>
                                                <label class="middle"><input type="checkbox">密码重置提醒</label>
                                                <label class="middle"><input type="checkbox">会员等级变化提醒</label>
                                                <label class="middle"><input type="checkbox">优惠券过期提醒</label>
                                                <label class="middle"><input type="checkbox">订单成功获得优惠券提醒</label>
                                                <label class="middle"><input type="checkbox">退款成功提醒</label>
                                                <label class="middle"><input type="checkbox">赠品发货提醒</label>
                                                <label class="middle"><input type="checkbox">订单成功获得积分提醒</label>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="footer-btn navbar-fixed-bottom">
                                <button type="button" class="btn btn-success" id="btn-save">保存设置</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <aside class="ui-sidebar sidebar" style="height:100%">
            <nav class="well" id="menu_left">
                <ul>
                    <li class="active"><a href="weibo/setting.aspx" class="new" target="frammain">绑定微博账号</a></li>
                </ul>
                <em class="glyphicon glyphicon-scissors"></em>
                <span>微博管理</span>
                <ul>
                    <li><a href="weibo/timeline.aspx" class="new" target="frammain">好友的微博</a></li>
                    <li><a href="weibo/usertimeline.aspx" class="new" target="frammain">我的微博</a></li>
                    <li><a href="weibo/message.aspx" class="new" target="frammain">微博消息</a></li>
                    <li><a href="weibo/post.aspx" class="new" target="frammain">发布微博</a></li>
                </ul>
                <em class="glyphicon glyphicon-folder-open"></em>
                <span>消息推送</span>
                <ul>
                    <li><a href="weibo/letter.aspx" class="new" target="frammain">群发私信</a></li>
                    <li><a href="weibo/autoreply.aspx" class="new" target="frammain">自动回复</a></li>
                </ul>
                <ul>
                    <li><a href="weibo/menu.aspx" class="new" target="frammain">自定义菜单</a></li>
                </ul>
            </nav>
        </aside>
    </div>


    <div class="modal fade" role="dialog" aria-labelledby="mySmallModalLabel" id="myModal">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="w-modalbox">
                    <h5>绑定运营者微信号</h5>
                    <div class="y-wechatstep">
                        <ul class="clearfix">
                            <li class="triangle active clearfix">1&nbsp;绑定微信号</li>
                            <li class="">2&nbsp;选中接收消息类型</li>
                        </ul>
                    </div>
                    <div class="wechatstepcontentbox">
                        <div class="wechatstepcontent active">
                            <h6 class="mb5">微信扫描二维码后，将自动获取绑定授权。成功获取授权后，可点击下一步继续</h6>
                            <div class="y-qrcode">
                                <img src="http://fpoimg.com/170x170">
                            </div>
                            <label>
                                运营者微信OpenID：
                                <input class="inputw200" type="text" placeholder="扫描上方二维码或手动输入">
                            </label>
                            <p class="mt10 mb5" style="color:#999999;font-size:12px;">openid号可以在<a>会员列表</a>中指定的会员一栏中查看并复制；</p>
                            <p style="color:#999999;font-size:12px;">用运营者微信号扫描二维码，也可用手机拍照后，将二维码发送给运营者扫描</p>
                        </div>
                        <div class="wechatstepcontent">
                            <h6 class="mb20">为运营者指定接收消息的类型，可多选</h6>
                            <div class="form-horizontal mb10">
                                <div class="form-group">
                                    <label class="col-xs-3 pad resetSize control-label" for="pausername">运营者姓名：</label>
                                    <div class="col-xs-9">
                                        <input type="text"  class="form-control resetSize inputw150">
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal mb10">
                                <div class="form-group">
                                    <label class="col-xs-3 pad resetSize control-label" for="pausername">运营者角色：</label>
                                    <div class="col-xs-9">
                                        <input type="text"  class="form-control resetSize inputw150">
                                    </div>
                                </div>
                            </div>
                            <div class="form-horizontal mb10">
                                <div class="form-group">
                                    <label class="col-xs-3 pad resetSize control-label" for="pausername">选择消息类型：</label>
                                    <div class="col-xs-9">
                                        <label class="middle mb5 mr10">
                                            <input type="checkbox">新订单提醒
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <input type="checkbox">订单付款提醒
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <input type="checkbox">退货申请提醒
                                        </label> 
                                        <label class="middle mb5 mr10">
                                            <input type="checkbox">用户咨询提醒
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <input type="checkbox">提现申请提醒
                                        </label>
                                        <label class="middle mb5 mr10">
                                            <input type="checkbox">分销商申请成功提醒
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="y-ikown y-wxbdbtn active pt10 pb10">
                        <input type="submit" value="暂不绑定" class="btn btn-primary inputw100" data-dismiss="modal">
                        <input type="submit" value="保存至下一步" class="btn btn-success inputw100 y-nextstep">
                    </div>
                    <div class="y-ikown y-wxbdbtn pt10 pb10">
                        <input type="submit" value="上一步" class="btn btn-primary inputw100 y-pverstep">
                        <input type="submit" value="保存" class="btn btn-success inputw100" data-dismiss="modal">
                    </div>
                </div>
            </div>
        </div>
    </div>
</body>
<script type="text/javascript">
    $(function (){
        $('.bindingmicrochannel').click(function (){
            $('#myModal').modal('toggle').children().css({
                width: '530px'
            })
        });
        $('.y-nextstep').click(function (){
            $('.y-wechatstep ul li').removeClass('active').last().addClass('active');
            $('.wechatstepcontentbox .wechatstepcontent').removeClass('active').last().addClass('active');
            $('.y-wxbdbtn').removeClass('active').last().addClass('active');
        })
        $('.y-pverstep').click(function (){
            $('.y-wechatstep ul li').removeClass('active').first().addClass('active');
            $('.wechatstepcontentbox .wechatstepcontent').removeClass('active').first().addClass('active');
            $('.y-wxbdbtn').removeClass('active').first().addClass('active');
        })
    })
</script>
</html>
