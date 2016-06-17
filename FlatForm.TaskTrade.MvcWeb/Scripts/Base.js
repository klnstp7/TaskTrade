
//扩展方法-String
(function (inputStr) {
    inputStr.prototype.IsNullOrEmpty = function (c) {
        if (this == null)
            return true;
        if ($.trim(this) == "")
            return true;
        return false;
    };
    inputStr.prototype.ToInt = function () {
        if (this.IsNullOrEmpty(this))
            return null;
        if (isNaN(this))
            return null;
        return parseInt(this, 10);
    };
    inputStr.prototype.ToFloat = function () {
        if (this.IsNullOrEmpty(this))
            return null;
        if (isNaN(this))
            return null;
        return parseFloat(this);
    };
})(String);
(function ($) {
    $.prototype.serializeObject = function () {
        var a, o, h, i, e;
        a = this.serializeArray();
        o = {};
        h = o.hasOwnProperty;
        for (i = 0; i < a.length; i++) {
            e = a[i];
            if (!h.call(o, e.name)) {
                o[e.name] = e.value;
            }
        }
        return o;
    };
})(jQuery);
//扩展方法-Array
(function (inputArray) {
    inputArray.prototype.Where = function (fn) {
        return BaseHelper.Where(this, fn);
    },
        inputArray.prototype.Select = function (fn) {
            return BaseHelper.Select(this, fn);
        },
        inputArray.prototype.Sum = function (fn) {
            return BaseHelper.Sum(this, fn);
        },
        inputArray.prototype.First = function (fn) {
            return BaseHelper.First(this, fn);
        };
})(Array);
//扩展方法-Jquery对象。如$("选择器").方法名
(function ($) {
    $.fn.Where = function (fn) {
        return BaseHelper.Where(this, fn);
    },
        $.fn.Select = function (fn) {
            return BaseHelper.Select(this, fn);
        },
        $.fn.First = function (fn) {
            return BaseHelper.First(this, fn);
        },
        $.fn.Sum = function (fn) {
            return BaseHelper.Sum(this, fn);
        },
        $.fn.DivSerialize = function (fn) {
            var data = {};
            this.find("input[type='text'],input[type='hidden'],select,textarea").each(function () {
                var name = $(this).attr("name");
                if (name != null) {
                    if (!name.IsNullOrEmpty()) {
                        data[name] = $(this).val();
                    }
                }
            });
            var paraArray = [];
            for (var item in data) {
                paraArray.push(item + "=" + data[item]);
            }
            var result = BaseHelper.StringJoin(paraArray, "&");
            return result;
        };
})(jQuery);

//write by 徐铮
//array.forEach()不支持ie8以下的版本
//Array.forEach implementation for IE support..  
if (!Array.prototype.forEach) {
    Array.prototype.forEach = function (callback, thisArg) {
        var T, k;
        if (this == null) {
            throw new TypeError(" this is null or not defined");
        }
        var O = Object(this);
        var len = O.length >>> 0; // Hack to convert O.length to a UInt32  
        if ({}.toString.call(callback) != "[object Function]") {
            throw new TypeError(callback + " is not a function");
        }
        if (thisArg) {
            T = thisArg;
        }
        k = 0;
        while (k < len) {
            var kValue;
            if (k in O) {
                kValue = O[k];
                callback.call(T, kValue, k, O);
            }
            k++;
        }
    };
}
//基础方法，请勿随意修改，上面的扩展方法很多也调了这里头的方法
var BaseHelper = {
    loadform: function (formid, url, afterloadfun, dataformatter) {
        $.ajax({
            type: "POST",
            url: url,
            dataType: "json",
            success: function (data) {
                if (dataformatter != null)
                    data = dataformatter(data);
                $("#" + formid + "").form("load", data);
                if (afterloadfun != null) {
                    afterloadfun(data);
                }
            },
            error: function (data) {
                BaseHelper.ShowException(data);
            }
        });
    },
    loadselect: function (selectid, url, async, isloding, afterfun, needempty) {

        if (needempty == null)
            needempty = true;
        var isasync = true;
        if (async != null)
            isasync = async;
        BaseHelper.clearselect(selectid);
        if (isloding)
            loadTips.showLoading();
        $.ajax({
            type: "POST",
            url: url,
            async: isasync,
            dataType: "json",
            success: function (data) {
                if (data != null)
                    BaseHelper.loadselectdata(selectid, data, needempty);
                if (isloding)
                    loadTips.hideLoading();
                if (afterfun != null && afterfun != undefined)
                    afterfun(data);
                return data;
            }
        });
    },
    loadselectdata: function (selectid, data, needempty) {
        var obj = $("#" + selectid);
        var result = "";
        if (needempty)
            result = "<option value=''></option>";
        for (var i = 0; i < data.length; i++) {
            var item = data[i];
            result += "<option value='" + item.Key + "'>" + item.Value + "</option>";
        }
        obj.append(result);
    },
    clearselect: function (selectid) {
        var obj = document.getElementById(selectid);
        var nodes = obj.childNodes;
        var arr = new Array();
        for (var i = 0; i < nodes.length; i++) {
            var temp = nodes[i];
            if (temp.nodeName == "OPTION") {
                if (temp.innerText != "" && temp.innerText != null)
                    arr.push(temp);
            }
        }

        for (var i = 0; i < arr.length; i++) {
            obj.removeChild(arr[i]);
        }
    },
    ViewProject: function (ProjectNo, TID) {
        if (ProjectNo == null)
            return;
        var tabid = "项目详细-" + ProjectNo;
        if (parent.tab.isTabItemExist(tabid))
            parent.tab.removeTabItem(tabid);
        parent.f_addTab(tabid, tabid, '/Partial/ProjectView?TID=' + TID);
    },
    ShowChoseProjectList: function (signselect, callbackMethodName, otherPara, afterCloseAll) {
        if (signselect != true && signselect != false)
            return;
        var url = '/Layer/ChoseProject?callback=' + callbackMethodName + '&singleselect=' + signselect;
        if (afterCloseAll == true)
            url += '&notcloseall=true';
        if (otherPara != null && otherPara != "")
            url = url + "&otherPara=" + encodeURIComponent(otherPara);
        $.layer({
            type: 2,
            title: '选择项目',
            iframe: { src: url },
            area: ['850px', '450px'],
            offset: ['70%', '50%']
        });
    },
    ShowPositiveLayer: function (callbackMethodName, projectno, reportno, projectid, address) {
        //     var encodeAddress = encodeURI(address);
        var url = '/ProjectProcess/ProjectPositiveLayer?callback=' + callbackMethodName + '&projectno=' + projectno + '&reportno=' + reportno + '&projectid=' + projectid + '&address=1';

        $.layer({
            type: 2,
            area: ['850px', '450px'],
            iframe: { src: url }
        });
    },
    ShowArchiveProjectList: function (signselect, callbackMethodName, otherPara, afterCloseAll) {
        if (signselect != true && signselect != false)
            return;
        var url = '/Layer/ChoseArchiveProject?callback=' + callbackMethodName + '&singleselect=' + signselect;
        if (afterCloseAll == true)
            url += '&notcloseall=true';
        if (otherPara != null)
            url = url + "&otherPara=" + encodeURIComponent(JSON.stringify(otherPara));
        $.layer({
            type: 2,
            title: '选择项目',
            iframe: { src: url },
            area: ['800px', '400px'],
            offset: ['50%', '50%']
        });
    },
    ShowException: function (data) {
        var result = data.responseText;
        if (result != null && result != '') { //后台异常时，并在后台捕获  
            $(document.body).html(result);
        }
    },
    StringJoin: function (array, sign, fn) {
        var result = "";
        for (var i = 0; i < array.length; i++) {
            var temp = array[i];
            if (fn != null)
                temp = fn(temp);
            result += temp;
            if (i + 1 != array.length)
                result += sign;
        }
        return result;
    },
    Where: function (array, fn) {
        var result = [];
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            if (fn(item))
                result.push(item);
        }
        return result;
    },
    First: function (array, fn) {
        if (fn == null) return array[0];
        var result = this.Where(array, fn);
        return (result.length == 0 ? null : result[0]);
    },
    IndexOf: function (array, fn) {
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            if (fn(item))
                return i;
        }
    },
    Select: function (array, fn) {
        var result = [];
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            var temp = fn(item);
            result.push(temp);
        }
        return result;
    },
    Sum: function (array, fn) {
        var result = 0;
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            var value = item;
            if (fn)
                value = fn(item);
            result += value;
        }
        return result;
    },
    Foreach: function (array, fn) {
        for (var i = 0; i < array.length; i++) {
            var item = array[i];
            fn(item);
        }
    },
    UTCtoLocation: function (format) {
        var o = {
            "M+": this.getMonth() + 1, //month 
            "d+": this.getDate(), //day 
            "h+": this.getHours(), //hour 
            "m+": this.getMinutes(), //minute 
            "s+": this.getSeconds(), //second 
            "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
            "S": this.getMilliseconds() //millisecond 
        };
        if (/(y+)/.test(format))
            format = format.replace(RegExp.$1,
            (this.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(format))
                format = format.replace(RegExp.$1,
                    RegExp.$1.length == 1 ? o[k] :
                    ("00" + o[k]).substr(("" + o[k]).length));
        return format;
    },
    getUrlParam: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]);
        return null;
    },
    divserializer: function (divid) {
        var data = {};
        $("#" + divid).find("select,input[type='text'],input[type='hidden'],textarea").each(function () {
            var name = this.name || "";
            var value = this.value || "";
            if (name != "" && value != "") {
                data[name] = value;
            }
        });
        var para = "";
        var index = 0;
        for (var item in data) {
            var temp = encodeURI(data[item]);
            para += item + "=" + temp + "&";
            index++;
        }
        if (index > 0)
            para = para.substring(0, para.length - 1);
        return { para: para, data: data };
    },
    ConverDate: function (date) {
        var regex = /Date\(([\d]+)\)/;
        if (!regex.test(date))
            return date;
        try {
            var result = parseInt(date.match(regex)[1], 10);
            var time = new Date(result);
            return time.getFullYear() + "-" + (time.getMonth() + 1) + "-" + time.getDate() + " " + time.getHours() + ":" + time.getMinutes();
        } catch (e) {
            return date;
        }
    }
};

var Project = {
    CheckActivation: function (obj) {
        if (obj.tid == null && obj.projectno == null)
            retrun;
        var ispass = true;
        var isalert = obj.isalert;
        var returnData = null;
        if (isalert == null) {
            $.ajax({
                async: false,
                url: "/Project/GetActivation?tid=" + $.trim(obj.tid) + "&projectno=" + $.trim(obj.projectno),
                success: function (data) {
                    isalert = data.IsAlert;
                    returnData = data;
                }
            });
        }
        if (isalert) {
            if (!confirm(returnData.ProjectNo + "项目已预估转正，需要激活才能继续操作，点击确定后激活项目")) {
                ispass = false;
                return ispass;
            }
            $.ajax({
                async: false,
                url: "/Project/Activation?tid=" + returnData.TID,
                success: function (data) {
                    ispass = data;
                    if (!ispass)
                        alert("项目激活异常");
                }
            });
        }
        return ispass;
    }

};

//弹出/关闭loading层
var loadTips = {
    initLoadTips: function () {
        var LoadingBG, load_Tips;
        if (document.getElementById("load_Tips") != null) {
            document.body.removeChild(document.getElementById("load_Tips"));
        }
        if (document.getElementById("LoadingBG") != null) {
            document.body.removeChild(document.getElementById("LoadingBG"));
        }

        LoadingBG = document.createElement("div");
        LoadingBG.setAttribute("class", "LoadingBG");
        LoadingBG.setAttribute("id", "LoadingBG");
        LoadingBG.style.display = "none";

        load_Tips = document.createElement("div");
        load_Tips.setAttribute("class", "load_Tips");
        load_Tips.setAttribute("id", "load_Tips");
        load_Tips.style.left = "50%";
        load_Tips.style.top = "50%";
        load_Tips.style.marginTop = "-80px";
        load_Tips.style.marginLeft = "-80px";
        load_Tips.style.display = "none";

        var imgload = document.createElement("img");
        imgload.src = "/Content/css/loadTips/loading.gif";
        imgload.setAttribute("class", "imgload");
        imgload.setAttribute("id", "imgload");

        var loadInfo = document.createElement("div");
        loadInfo.setAttribute("class", "loadInfo");
        loadInfo.setAttribute("id", "loadInfo");

        var infoText = document.createElement("span");
        infoText.setAttribute("id", "infoText");

        var divClear = document.createElement("div");
        divClear.setAttribute("class", "clear");

        loadInfo.appendChild(imgload);
        loadInfo.appendChild(infoText);
        loadInfo.appendChild(divClear);
        load_Tips.appendChild(loadInfo);

        document.body.appendChild(load_Tips);
        document.body.appendChild(LoadingBG);
    },
    showLoading: function (info) {
        loadTips.initLoadTips();
        info = info || "正在处理";
        document.getElementById("infoText").innerText = info;
        document.getElementById("infoText").style.fontSize = 12;
        document.getElementById("LoadingBG").style.display = "";
        document.getElementById("load_Tips").style.display = "";
    },
    hideLoading: function () {
        document.getElementById("LoadingBG").style.display = "none";
        document.getElementById("load_Tips").style.display = "none";
    }
};

//自定义阻止事件冒泡   Author：Shuaijian
function auto_stopPropagation(event) {
    try {
        //加try catch防止浏览器不兼容，导致后续代码不执行
        //Jquery阻止事件冒泡  
        if (event.stopPropagation) {   //兼容FireFox、Chrome、Opera浏览器
            event.stopPropagation();
        }
        else if (window.event) {   //兼容 IE 浏览器          
            window.event.cancelBubble = true;
        }
    } catch (e) { }
}


//-----------------------------------------LINQ TO JS---------------------------------------------------
(function () {
    LINQ = function (data) {
        var newArray = new Array();
        for (var i = 0; i < data.length; i++) {
            newArray[i] = data[i];
        }
        this.data = newArray;
    };
    LINQ.prototype = {
        Where: function (predicate) {
            var newArray = new Array();
            for (var i = 0; i < this.data.length; i++) {
                var item = this.data[i];
                var success = predicate(item, i);
                if (success) {
                    newArray.push(item);
                }
            }
            return new LINQ(newArray);
        },
        Select: function (func) {
            var newArray = new Array();
            for (var i = 0; i < this.data.length; i++) {
                var item = this.data[i];
                item = func(item, i);
                newArray.push(item);
            }
            return new LINQ(newArray);
        },
        Sum: function (func) {
            var i = 0;
            var result = this.data[i++];
            for (; i < this.data.length; i++) {
                var item = this.data[i];
                if (func != null)
                    item = func(item);
                result += item;
            }
            return result;
        },
        ToArray: function () {
            return this.data;
        },
        Foreach: function (func) {
            for (var i = 0; i < this.data.length; i++) {
                var item = this.data[i];
                func(item, i);
            }
        },
        Contains: function (inputobj) {
            if (inputobj == null)
                return false;
            var predicate = function (item) {
                return item === inputobj;
            };
            if (inputobj instanceof Function)
                predicate = inputobj;
            for (var i = 0; i < this.data.length; i++) {
                var item = this.data[i];
                if (predicate(item))
                    return true;
            }
            return false;
        },
        Concat: function (array) {
            var inputItemLINQ = new LINQ(array);
            var result = new Array();
            this.Foreach(function (item) {
                result.push(item);
            });
            inputItemLINQ.Foreach(function (item) {
                result.push(item);
            });
            return new LINQ(result);
        },
        OrderBy: function (sortfunc) {
            var result = new LINQ(this.data);
            LINQ.private.QuickSort(result.data, "<", sortfunc);
            return result;
        },
        OrderByDesc: function (sortfunc) {
            var result = new LINQ(this.data);
            LINQ.private.QuickSort(result.data, ">", sortfunc);
            return result;
        },
        Skip: function (count) {
            if (typeof count != "number")
                throw "count必须是数字";
            var skipCount = parseInt(count, 10);
            var newArray = new Array();
            for (var i = count; i < this.data.length; i++) {
                newArray.push(this.data[i]);
            }
            return new LINQ(newArray);
        },
        Take: function (count) {
            if (typeof count != "number")
                throw "count必须是数字";
            var skipCount = parseInt(count, 10);
            var newArray = new Array();
            var length = count > this.data.length ? this.data.length : count;
            for (var i = 0; i < count; i++) {
                newArray.push(this.data[i]);
            }
            return new LINQ(newArray);
        },
        FirstOrDefault: function (predicate) {
            if (predicate == null)
                return this.data[0];
            for (var i = 0; i < this.data.length; i++) {
                var item = this.data[i];
                var success = predicate(item, i);
                if (success)
                    return item;
            }
        },
        Count: function (predicate) {
            if (predicate == null)
                return this.data.length;
            return this.Where(predicate).Count();
        }
    },
        LINQ.private = {
            QuickSort: function (data, operating, func) {
                if (func == null) {
                    func = function (item) { return item };
                }
                var _innerSort = function (data, left, right) {
                    var baseNumber = func(data[left]);
                    while (left < right) {
                        for (; left < right; right--) {
                            if (eval(func(data[right]) + operating + baseNumber)) {
                                data[left] = data[right];
                                break;
                            }
                        }
                        for (; left < right; left++) {
                            if (eval(func(data[left]) + (operating == ">" ? "<" : ">") + baseNumber)) {
                                data[right] = data[left];
                                break;
                            }
                        }
                    }
                    data[left] = baseNumber;
                    return left;
                }
                var _sort = function (data, left, right) {
                    if (left < right) {
                        var middle = _innerSort(data, left, right);
                        _sort(data, left, middle);
                        _sort(data, middle + 1, right);
                    }
                };
                _sort(data, 0, data.length - 1);
            }
        };
})();


