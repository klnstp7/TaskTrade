if(!$)throw new Error(" requires jQuery");

+function ($) {
    'use strict';

    // BUTTON PUBLIC CLASS DEFINITION
    // ==============================

    var Toolbar = function (element, options) {
        this.$element = $(element)
        this.options = $.extend({}, Toolbar.DEFAULTS, options)
        this.isLoading = false,
        this._initTable()
    }

    Toolbar.VERSION = '1.0.0'

    Toolbar.DEFAULTS = {
        columns: [],
        perRow: 3,
        className: "project-input",
        maxlength:50

    }

    Toolbar.prototype = {
        _initTable: function () {
            var _columns = this.options.columns;
            var len = _columns.length,
             rows = len / this.options.perRow;
            len % this.options.perRow != 0 || rows++;

            //for (var i = 0; i < _columns; i++) {
                
            //    if (i % this.options.perRow == 0) {
            //        var _tr = i == 0 ? $("<tr></tr>") : $('<tr tag="arrow" style="display:none;"></tr>');
            //        _tr.appendTo(this.$element);
            //    }


            //}

            for (var i = 0; i < rows; i++) {
               
                var _tr = i == 0 ? $("<tr></tr>") : $('<tr tag="arrow" style="display:none;"></tr>');
                _tr.appendTo(this.$element);
                for (var j = 0; j < this.options.perRow; j++) {
                    var current = i * this.options.perRow + j;
                    if (current < len) {
                        var column = _columns[i * this.options.perRow + j];
                        var _td = $('<td>' + column.title + '：</td>');
                        _td.appendTo(_tr);
                        _td = $('<td></td>');
                        if (column.coltype == "select") {
                            var _selOption = [];
                            var dLen = 0;
                            if(column.data && (dLen=column.data.length)>0){
                                for(var d =0;d<dLen;d++){
                                    _selOption.push('<option value="'+column.data[d]["value"]+'">'+column.data[d]["text"]+'</option>');
                                }
                            }
                            var _sel =_selOption.join("");
                            var _select = $('<select class="' + (column.className ? column.className : this.options.className)
                                + '" id="' + column.id + '" name="' + column.id +  '">' + '<option value="">请选择</option>'
                                + _sel + '</select>');
                            _select.appendTo(_td);
                            _td.appendTo(_tr);
                        }
                        else if (column.coltype == "date") {
                            var inputs = $('<input type="text" id="' + column.id + '_begin" name="' + column.id + '_begin" class="'
                                + (column.className ? column.className : this.options.className) + ' laydate-icon" readonly />'
                                + '-- <input type="text" id="' + column.id + '_end" name="' + column.id + '_end" class="'
                                + (column.className ? column.className : this.options.className) + ' laydate-icon" readonly />');
                            inputs.appendTo(_td);
                            _td.appendTo(_tr);
                        }
                        else if (column.format == "between") {
                            var inputs = $('<input type="text" id="' + column.id + '_begin" name="' + column.id + '_begin" class="'
                                + (column.className ? column.className : this.options.className) + '" />'
                                 + '--<input type="text" id="' + column.id + '_end" name="' + column.id + '_end" class="'
                                 + (column.className ? column.className : this.options.className) + '" />');
                            inputs.appendTo(_td);
                            _td.appendTo(_tr);
                        }
                        else {

                            var inputs = $('<input type="text" id="' + column.id
                                   + '" name="' + column.id + '" class="' + (column.className ? column.className : this.options.className) + '" />');
                            inputs.appendTo(_td);
                            _td.appendTo(_tr);
                        }
                       
                        if (current == 3 && len > this.options.perRow) {
                            _td.append('&nbsp;<i class="fa  fa-arrow-circle-o-down unfold" id="pointer"></i>');
                            _td.appendTo(_tr);
                        }
                           
                    }
                   
                } 
            } //end for

            setTimeout(function () {
                for (var i = 0; i < len; i++) {
                    if (_columns[i].coltype == "date") {
                        laydate({
                            elem: "#" + _columns[i].id + "_begin",
                            format: 'YYYY-MM-DD',
                            event: 'focus',
                            isclear: true,
                            istoday: true,
                            issure: true
                        });

                        laydate({
                            elem: "#" + _columns[i].id + "_end",
                            format: 'YYYY-MM-DD',
                            event: 'focus',
                            isclear: true,
                            istoday: true,
                            issure: true
                        });
                        
                    }
                } 
            },500);
            
        },
         
    }
     

    // Toolbar PLUGIN DEFINITION
    // ========================

    function Plugin(option) {
        return this.each(function () {
            var $this = $(this)
            var data = $this.data('yunfang.toolbar')
            var options = typeof option == 'object' && option

            if (!data) $this.data('yunfang.toolbar', (data = new Toolbar(this, options)))

        })
    }

    var old = $.fn.toolbar

    $.fn.toolbar = Plugin
    $.fn.toolbar.Constructor = Toolbar

   

    // Toolbar NO CONFLICT
    // ==================

    $.fn.toolbar.noConflict = function () {
        $.fn.toolbar = old
        return this
    }
  

}(jQuery);


$.fn.serializeJson = function () {
    var serializeObj = {};
    var array = this.serializeArray();
    var str = this.serialize();
    $(array).each(function () {
        if (serializeObj[this.name]) {
            if ($.isArray(serializeObj[this.name])) {
                serializeObj[this.name].push(this.value);
            } else {
                serializeObj[this.name] = [serializeObj[this.name], this.value];
            }
        } else {
            serializeObj[this.name] = this.value;
        }
    });
    return serializeObj;
};