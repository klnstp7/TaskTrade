// 选项卡标签集合
var itemlist = [];
var itemidlist = [];
// iframe标签集合
var iframelist = [];

//全局变量
var str = '<div class="tab-layout" id="tab-layout"><div class="tab-layout-title"><div class="tab-layout-left-arrow" onclick="prev()"><span class="line"></span><i class="fa fa-caret-left"></i></div> <div class="tab-layout-right-arrow" onclick="next()"><span class="line"></span><i class="fa fa-caret-right"></i></div><div class="tab-layout-content"><ul></ul></div> </div><div class="tab-layout-body"></div></div>';

var container ="";
var tabContent;
var header ="";
var _headerWidth;
var tll_arrow = "";
var tlr_arrow ="";
var ifBod = "";
var _width = 0,
    nm = 0,
    lastWidth = 0,
    _height = 0;
var iframeid = "iframe_";
var flag = true;
var contextmenu = "", str_title="", iFrameID ="";
var loadingidx;
$(function () {



    document.oncontextmenu = function() {
        return false;
    };

    $("body").click(function() {
        if (contextmenu !== "") {
           $("#contextmenu").hide();
        } 
    });
});

function init() {
    if ($("#page-content").find("#tab-layout").length>=1) {
        return null;
    }else{
    $(str).appendTo($("#page-content"));
    container = "#tab-layout";
    tabContent = $(container).find('.tab-layout-content ul');
    header = $(container).children('.tab-layout-title');
    _headerWidth = header.width();
    tll_arrow = $(container).find('.tab-layout-left-arrow');
    tlr_arrow = $(container).find('.tab-layout-right-arrow');
    ifBod = $(container).find('.tab-layout-body');
    }
}


//添加标签
function additem(id, name, url, _del) {

    var _id = id;
    var _name = name;
    var _url = url;
    var isHas = false;
    var _arry = [];
    var _idx = 0;

    for (var i = 0; i < itemlist.length; i++) {
        if (_id == itemlist[i].id) {
            _idx = i;
            isHas = true;
            break;
        }
    }

    init();

    //已有选项卡
    if (isHas) {
        var __width = getAllWidth();
        if (__width <= _headerWidth) {
            lastWidth = 0;
            tll_arrow.attr("disabled", "disabled");
            tlr_arrow.attr("disabled", "disabled");
        } else {
            var leftWidth = 0;
            for (var i = 0; i <= _idx; i++) {
                leftWidth += $('.tab-layout-content ul li#' + itemidlist[i], header).outerWidth(true);
            }
            if (leftWidth <= _headerWidth) {
                lastWidth = 0;
            } else {
                var page = parseInt(leftWidth / _headerWidth);
                lastWidth = page * _headerWidth;
            }
            tll_arrow.attr("disabled", "");
            tlr_arrow.attr("disabled", "");
        }
        tabContent.animate({
            "marginLeft": "-" + lastWidth + "px"
        });
        tabContent.find("li").removeClass("current");
        tabContent.find("li#" + _id).addClass("current");
    } else {
        //添加新选项卡
        loadingidx = layer.load(2, {
            shade: [0.1, '#000']
        });
        tabContent.find("li").removeClass("current");
        if (_del == "true") {
            str_title = '<li class="item-content sm current" onclick=select("' + _id + '",event) id="' + _id + '"><a href="javascript:void(0);">' + name + '</a></li>';
        }else{
            str_title = '<li class="item-content current" onclick=select("' + _id + '",event) id="' + _id + '"><a href="javascript:void(0);">' + name + '</a><span class="item-del fa fa-close" onClick=delitem("' + _id + '")>&nbsp;</span></li>';
        }

        tabContent.append(str_title);
        
        tabContent.find("li").bind('contextmenu', function (e) {
            if (contextmenu !== "") {
                $("body .contextmenu").remove();
            }
            contextmenu = '<div class="contextmenu" id="contextmenu" style="left:' + e.clientX + 'px;top:' + e.clientY + 'px;"><ul><li><a href="javascript:void(0);" onclick=reloadiFrame(event)>刷新</a></li></ul></div>';
            $("body").append(contextmenu).show();
        });
        var item = {
            id: _id,
            name: _name,
            url: _url
        };
        itemlist.push(item);
        itemidlist.push(id);
        if (flag) {
            var __width = 0;
            _arry = itemidlist.slice(0);
            for (var i = 0; i < _arry.length; i++) {
                __width += $('.tab-layout-content ul li#' + _arry[i], header).outerWidth(true);
            }
            if (__width >= _headerWidth) {
                lastWidth = __width - _headerWidth;
                tll_arrow.attr("disabled", "");
                tlr_arrow.attr("disabled", "");
            } else {
                lastWidth = 0;
                tll_arrow.attr("disabled", "disabled");
                tlr_arrow.attr("disabled", "disabled");
            }
        } else {
            if (setarrowflg()) {
                lastWidth = getAllWidth() - _headerWidth;
                tll_arrow.attr("disabled", "");
                tlr_arrow.attr("disabled", "");
            } else {
                lastWidth = 0;
                tll_arrow.attr("disabled", "disabled");
                tlr_arrow.attr("disabled", "disabled");
            }
        }
        tabContent.animate({
            "marginLeft": "-" + lastWidth + "px"
        });
    }
    showFrame(isHas, _id, _url);
}

//添加iframe
function showFrame(isHas, id, url) {
    iFrameID = iframeid + id;
    ifBod.find("iframe").removeClass("iFrameShow");
    ifBod.find("iframe").addClass("iFrameHide");
    if (isHas) {
        //显示，或者显示重新加载
        $("#" + iFrameID).removeClass("iFrameHide");
        $("#" + iFrameID).addClass("iFrameShow");
        //根据自身需求刷新
        var ifIndex = -1;
        for (var i = 0; i < iframelist.length; i++) {
            if (iframelist[i].id == iFrameID) { //
                ifIndex = i;
                break;
            }
        }
        if (ifIndex == -1) { //id不存在
            return;
        }
        var currentUrl = iframelist[ifIndex].url;
        if (currentUrl != url) { //刷新
            $("#" + iFrameID).attr("src", url);
            //替换成新的url
            iframelist[ifIndex].url = url;
        }

    } else { //新增
        _height = $(window).height() - $(container).find(".tab-layout-title").height() - $(container).offset().top;
        _height = (_height <= 0) ? 0 : _height;
        ifBod.append('<iframe id="' + iFrameID + '" class="iFrameShow" scrolling="yes" frameborder="0" src="' + url + '" style="height:' + _height + 'px"></iframe>');
        //加入缓存
        var iFrame = {
            id: iFrameID,
            url: url
        };
        iframelist.push(iFrame);
        $("#" + iFrameID).on("load", function (e) {
            parent.layer.close(parent.loadingidx);
        });
    }
}

//删除iframe
function deliFrame(id) {
    var iFrameID = iframeid + id;
    for (var i = 0; i < iframelist.length; i++) {
        if (iFrameID == iframelist[i].id) {
            //删除缓存中的 iFrame
            iframelist.remove(i);
            break;
        }
    }
    //从 dom 结构中删除
    ifBod.children('#' + iFrameID + "").remove();
}

//删除所选选项卡 并且选中选项卡
function delitem(id) {
    var _id = id;
    var preid;
    var objprev = tabContent.find("li#" + id).prev();
    var obknext = tabContent.find("li#" + id).next();
    var __id = getSelectedID();
    var selectedWidth = $('.tab-layout-content ul li#' + _id, header).outerWidth(true);
    //if (itemlist.length == 1) {
    //   // alert("当前只有1个选项卡，请不要进行删除！");
    //} else {
        flag = false;
        if (_id == __id) {
            preid = objprev.attr("id") || obknext.attr("id");
            select(preid);
        } else {
            select(__id);
        }
        removeItem(_id);
        
    //}
}

//删除所选选项卡
function removeItem(id){
    var _id = id;
    deliFrame(_id);
    tabContent.children("#" + _id).remove();
    for (var i = 0; i < itemlist.length; i++) {
        if (_id == itemlist[i].id) {
            //选定选项卡 删除
            itemlist.remove(i);
            itemidlist.remove(i);
            break;
        }
    }
    if (setarrowflg()) {
        var $width = 0;
        for (var i = 0; i < itemlist.length; i++) {
            $width += $('.tab-layout-content ul li#' + itemlist[i].id, header).outerWidth(true);
        }
        lastWidth = lastWidth - selectedWidth;
    } else {
        lastWidth = 0;
        tll_arrow.attr("disabled", "disabled");
        tlr_arrow.attr("disabled", "disabled");
    }
    tabContent.animate({
        "marginLeft": "-" + lastWidth + "px"
    });

}

//选中选项卡
function select(id, e) {
    if (e && e.stopPropagation) {
        e.stopPropagation();
    } else {
        window.event.cancelBubble = true;
    }
    tabContent.find("li").removeClass("current");
    tabContent.find("li#" + id).addClass("current");
    showFrame(true, id);
}

//选项卡标题宽度与容器宽度对比
function setarrowflg() {
    var _flg = true;
    var _tabsWidth = 0;
    $('.tab-layout-content ul li', header).each(function () {
        _tabsWidth += $(this).outerWidth(true);
    });
    if (_tabsWidth >= _headerWidth) {
        _flg = true;
    } else {
        _flg = false;
    }
    return _flg;
}

//左滑动
function prev() {
    if (tabContent.css("marginLeft") == "0px") {
    } else {
        lastWidth = lastWidth - 60;
        if (lastWidth <= 0) {
            lastWidth = 0;
        }
        tabContent.animate({
            "marginLeft": "-" + lastWidth + "px"
        });
    }
}

//右滑动
function next() {
    var __$width = getAllWidth();
    if (-(parseInt(tabContent.css("marginLeft"))) + _headerWidth >= __$width) {
    } else {
        lastWidth = -(parseInt(tabContent.css("marginLeft"))) + 60;
        if (lastWidth >= (__$width - _headerWidth)) {
            lastWidth = __$width - _headerWidth;
        }
        tabContent.animate({
            "marginLeft": "-" + lastWidth + "px"
        });
    }
}

//获取选项卡标题真实全部宽度
function getAllWidth() {
    var _$width = 0;
    $('.tab-layout-content ul li', header).each(function () {
        _$width += $(this).outerWidth(true);
    });
    return _$width;
}

//获取选中的ID
function getSelectedID() {
    var _selectedID = "";
    $('.tab-layout-content ul li', header).each(function () {
        if ($(this).hasClass('current')) {
            _selectedID = $(this).attr("id");
        }
    });
    return _selectedID;
}

//iframe强制重新加载
function reloadiFrame(e) {
    if (e && e.stopPropagation) {
        e.stopPropagation();
    } else {
        window.event.cancelBubble = true;
    }
    select(getSelectedID(), e);
    loadingidx = layer.load(2, {
        shade: [0.1, '#000']
    });
    $("body .contextmenu").remove();
    $("#" + iframeid + getSelectedID()).attr('src', $("#" + iframeid + getSelectedID()).attr('src'));
}

//重置iframe高度 重置选项卡宽度
function resizeWin() {
    _headerWidth = header.width();
    _height = $(window).height() - $(container).find(".tab-layout-content").height() - $(container).offset().top;
    _height = (_height <= 0) ? 0 : _height;
    $(container).find("iframe").height(_height);
}

//数组添加移除对应索引元素
Array.prototype.remove = function (dx) {
    if (isNaN(dx) || dx > this.length) {
        return false;
    }
    for (var i = 0, n = 0; i < this.length; i++) {
        if (this[i] != this[dx]) {
            this[n++] = this[i];
        }
    }
    this.length -= 1;
};