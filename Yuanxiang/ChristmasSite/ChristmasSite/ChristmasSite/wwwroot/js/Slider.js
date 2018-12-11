var MoreFilter = {};
(function (_$) {
    var divID;
    var atagID;
    var _height;
    var _moreText;
    var _lessText;

    _$.init = function (did, aid, more, less) {
        divID = "#" + did;
        atagID = "#" + aid;
        _moreText = more;
        _lessText = less;

        _height = $(divID).height();

        this.hidden(divID, atagID, _moreText, _lessText, _height);
    }

    _$.show = function (divID, atagID, _moreText, _lessText, _height) {
        $(divID).animate(
            { height: _height }, 300, function () {
                $(divID).css('height', '100%');
                $(divID).css('overflow', 'visible');
            }
        );
        $(atagID).attr('href', 'javascript:MoreFilter.hidden("' + divID + '","' + atagID + '","' + _moreText + '","' + _lessText + '",' + _height + ')');
        $(atagID).html('<span class="glyphicon glyphicon-minus-sign"></span> ' + _lessText);
        $('#MoreFilterDiv').addClass('catalogmoreatt');
    }

    _$.hidden = function (divID, atagID, _moreText, _lessText, _height) {
        $(divID).animate(
            { height: 0 }, 300, function () {
                $(divID).css('overflow', 'hidden');
            }
        );
        $(atagID).attr('href', 'javascript:MoreFilter.show("' + divID + '","' + atagID + '","' + _moreText + '","' + _lessText + '",' + _height + ')');
        $(atagID).html('<span class="glyphicon glyphicon-plus-sign"></span> ' + _moreText);
        $('#MoreFilterDiv').removeClass('catalogmoreatt');
    }
})(MoreFilter);

function initFilter() {
    var cn = 'hidded';
    var time = 200;

    $('#newfilter .nbTitle').click(function () {
        var tilte = $(this);
        if (tilte.find('.glyphicon-remove-circle').css('display') == 'block') {
            return;
        }
        var values = tilte.next();
        if (values.hasClass(cn)) {
            values.removeClass(cn).show(time);
            tilte.find('.collapseSpan').removeClass('glyphicon-plus').addClass('glyphicon-minus');
        } else {
            values.addClass(cn).hide(time);
            tilte.find('.collapseSpan').removeClass('glyphicon-minus').addClass('glyphicon-plus');
        }
    });

    $('#newfilter .groupTitle .collapseSpan:not(.checkSpan)').click(function () {
        var tilte = $(this).parent();
        var values = tilte.next();
        if (values.hasClass(cn)) {
            values.removeClass(cn).show(time);
            tilte.find('.collapseSpan:not(.checkSpan)').removeClass('glyphicon-chevron-down').addClass('glyphicon-chevron-up');
        } else {
            values.addClass(cn).hide(time);
            tilte.find('.collapseSpan:not(.checkSpan)').removeClass('glyphicon-chevron-up').addClass('glyphicon-chevron-down');
        }
    });

    $('#newfilter .groupTitle .collapseSpan:not(.checkSpan)').each(function () {
        if ($(this).parent().next().find("input[type=checkbox]:checked").length == 0) {
            $(this).click();
        } else {
            $(this).parent().find('> input[type=checkbox]').prop('checked', true);
            $(this).parent().find('> input[type=checkbox]').next().addClass('checked');
        }
    });

    $('#newfilter input[type=checkbox]').change(function () {
        var parent = $(this).parent();
        var next = $(this).next();
        var parentPrev = parent.parent().prev();
        if ($(this).prop('checked')) {
            next.addClass('checked');
            if (parent.hasClass('groupTitle')) {
                parent.next().find('input[type=checkbox]').prop('checked', true);
                parent.next().find('input[type=checkbox]').next().addClass('checked');
            } else if (parentPrev.hasClass('groupTitle')) {
                parentPrev.find('> input[type=checkbox]').prop('checked', true);
                parentPrev.find('> input[type=checkbox]').next().addClass('checked');
            }
        } else {
            next.removeClass('checked');
            if (parent.hasClass('groupTitle')) {
                parent.next().find('input[type=checkbox]').prop('checked', false);
                parent.next().find('input[type=checkbox]').next().removeClass('checked');
            } else if (parentPrev.hasClass('groupTitle')) {
                if (parent.parent().find("input[type=checkbox]:checked").length == 0) {
                    parentPrev.find('> input[type=checkbox]').prop('checked', false);
                    parentPrev.find('> input[type=checkbox]').next().removeClass('checked');
                }
            }
        }
        
    });


    $('#newfilter .moreButton').click(function () {
        var prev = $(this).prev();
        var more = $(this).find('.more')
        if (prev.hasClass(cn)) {
            prev.removeClass(cn).show(time);
            more.html(more.attr('data-less'));
        } else {
            prev.addClass(cn).hide(time);
            more.html(more.attr('data-more'));
        }
    });

    $('#newfilter .moreButton').each(function () {
        if ($(this).prev().find("input[type=checkbox]:checked").length == 0) {
            $(this).click();
        }
    });

    $('#newfilter .glyphicon-minus').each(function () {
        var gly = $(this);
        if (gly.attr('data-collapse') == "1") {
            if (gly.parent().next().find("input[type=checkbox]:checked").length == 0 && gly.parent().next().find(".selected").length == 0) {
                $(this).click();
            }
        }
    });

    var fc = $('#fCount').html();
    if (fc == "0" || fc == "") {
        $('#sortByDiv').removeClass('hasFilter');
        $('#fCountATag').hide(100);
    } else {
        $('#fCountATag').show(100);
        if (!$('#sortByDiv').hasClass('hasFilter')) {
            $('#sortByDiv').addClass('hasFilter');
        }
    }
}

function showallfilters() {
    $('#filters').removeClass('hideAllFilters');
    $('#filters').addClass('showAllFilters');
    //$('#filters .nbDiv').css('display', 'block');
    $('#filters #showmore-filters').css('display', 'none');
    $('#filters #hide-filters').css('display', 'block');

}

function hideallfilters() {
    $('#filters').removeClass('showAllFilters');
    $('#filters').addClass('hideAllFilters');
    //$('#filters .nbDiv').css('display', '');
    $('#filters #showmore-filters').css('display', 'block');
    $('#filters #hide-filters').css('display', 'none');
}

function initFilters() {
    $('#filters #showmore-filters').css('display', '');
    $('#filters #hide-filters').css('display', '');
    //$('#filters .nbDiv').css('display', '');
    $('#filters').removeClass('showAllFilters');
    $('#filters').removeClass('hideAllFilters');
}

function formatPrice(n) {
    var t = parseInt(n), i, r;
    for (t = t.toString().replace(/^(\d*)$/, "$1."), t = (t + "00").replace(/(\d*\.\d\d)\d*/, "$1"), t = t.replace(".", ","), i = /(\d)(\d{3},)/; i.test(t);)
        t = t.replace(i, "$1,$2");
    return t = t.replace(/,(\d\d)$/, ".$1"), r = t.split("."), r[1] == "00" && (t = r[0]), t
}

function run_waitMe(effect) {
    $('#pcDiv').waitMe({
        effect: effect,
        text: '',
        bg: 'rgba(52,152,219,0.7)',
        color: '#000'
    });
}

function createHeightBar(barId, barValues) {

    var heightBarParent = $('#' + barId).parent();
    heightBarParent.append("<ul id='heightBar_" + barId + "' class='heightBar ajRefreshUL'></ul>");
    var heightBarUL = heightBarParent.find(".heightBar");

    if (barValues != null || barValues.length > 1) {
        var withP = 99.0 / barValues.length;
        for (var i = 0; i < barValues.length; i++) {
            heightBarUL.append("<li id='" + barId + '_' + i + "' class='ajRefreshLI' style='" + "height:" + barValues[i] + "px" + ";" + "width:" + withP + "%" + "'></li>");
        }
    }

}

function createFilterSlider(sliderId, valueId, sliderValueType, minValue, maxValue, fromIndex, toIndex, prefixValue, valueFormat, homeurl) {

    if (valueFormat == 1) {
        $(sliderId).ionRangeSlider({
            type: sliderValueType,
            min: minValue,
            max: maxValue,
            step: 1,
            from: fromIndex,
            to: toIndex,
            prefix: prefixValue,
            onFinish: function (data) {

                var prInput = $(valueId);
                var pr = data.from + "-" + data.to;
                if (valueId == '#pr') {
                    $('#minPrice').val(data.from);
                    $('#maxPrice').val(data.to);
                }
                if (data.from == data.min && data.to == data.max) {//当选中min-max时，去掉参数
                    prInput.val(' ');
                    $('#wizardSlider_-100').css('display', 'none');
                } else {
                    prInput.val(pr);
                    $('#wizardSlider_-100').css('display', 'inline-block');
                }

                location.href = homeurl + 'pr=' + pr;
            },
            prettify: function (num) {
                return formatPrice(num);
            }
        });
    } else {
        $(sliderId).ionRangeSlider({
            type: sliderValueType,
            min: minValue,
            max: maxValue,
            step: 1,
            from: fromIndex,
            to: toIndex,
            prefix: prefixValue,
            onFinish: function (data) {

                var prInput = $(valueId);
                var pr = data.from + "-" + data.to;
                if (data.from == data.min && data.to == data.max)//当选中min-max时，去掉参数
                    prInput.val(' ');
                else
                    prInput.val(pr);

                location.href = homeurl + 'pr=' + pr;
            },
            prettify: function (num) {
                return toIntValue(num);
            }
        });
    }
    var slider = $(sliderId).data("ionRangeSlider");
    return slider;
}

function createFilterAttributeSlider(attTitleId, sliderId, valueId, sliderValueType, minValue, maxValue, fromIndex, toIndex, prefixValue, stepV) {
    $(sliderId).ionRangeSlider({
        type: sliderValueType,
        min: minValue,
        max: maxValue,
        step: stepV,
        grid: true,
        from: fromIndex,
        to: toIndex,
        prefix: prefixValue,
        onFinish: function (data) {

            var prInput = $(valueId);
            var pr = attTitleId + "_" + data.from + "-" + data.to;
            if (data.from == data.min && data.to == data.max) {//当选中min-max时，去掉参数
                prInput.val(' ');
                $('#wizardSlider_' + attTitleId).css('display', 'none');
            } else {
                prInput.val(pr);
                $('#wizardSlider_' + attTitleId).css('display', 'inline-block');
            }

        },
        prettify: function (num) {
            return toIntValue(num);
        }
    });

    var slider = $(sliderId).data("ionRangeSlider");
    return slider;
}

function SetCatalogBuyingWizardClickEvent() {
    $('#BuyingWizardUL > li').on("click", function () {
        if ($(this).hasClass("active")) {
            return;
        } else {
            var tab = $(this).attr("data-tab");
            $('#BuyingWizardUL > li').removeClass("active");
            $(this).addClass("active");
            if (tab == "Filter") {
                $('#newfilter').removeClass('buyingWizardFilters');
            } else {
                $('#newfilter').addClass('buyingWizardFilters');
            }
        }
    });
}

function ClearAttr(attrTitleId) {
    if (attrTitleId == -100) {
        $('#minPrice').val($('#minPrice').attr("min"));
        $('#maxPrice').val($('#maxPrice').attr("max"));
        priceChanged();
    } else {
        var sl = SliderDatastore[attrTitleId];
        if (sl != null) {
            sl.update({
                from: sl.result.min,
                to: sl.result.max
            });
            var prInput = $("#avsr_" + attrTitleId);
            prInput.val(' ');
        }
    }
    $('#wizardSlider_' + attrTitleId).css('display', 'none');
}



function showFullText(titleId) {
    $('#bwq_' + titleId + ' .ellipsis-ghost').css('display', 'none');
    $('#bwq_' + titleId + ' .ellipsis-container').css('display', 'block');
    $('#bwq_' + titleId + '.ellipsis').css('max-height', 'inherit');
}

/*! jQuery UI - v1.11.2 - 2014-12-10
* http://jqueryui.com
* Includes: core.js, widget.js, mouse.js, slider.js
* Copyright 2014 jQuery Foundation and other contributors; Licensed MIT */
(function (e) { "function" == typeof define && define.amd ? define(["jquery"], e) : e(jQuery) })(function (e) { function t(t, s) { var n, a, o, r = t.nodeName.toLowerCase(); return "area" === r ? (n = t.parentNode, a = n.name, t.href && a && "map" === n.nodeName.toLowerCase() ? (o = e("img[usemap='#" + a + "']")[0], !!o && i(o)) : !1) : (/input|select|textarea|button|object/.test(r) ? !t.disabled : "a" === r ? t.href || s : s) && i(t) } function i(t) { return e.expr.filters.visible(t) && !e(t).parents().addBack().filter(function () { return "hidden" === e.css(this, "visibility") }).length } e.ui = e.ui || {}, e.extend(e.ui, { version: "1.11.2", keyCode: { BACKSPACE: 8, COMMA: 188, DELETE: 46, DOWN: 40, END: 35, ENTER: 13, ESCAPE: 27, HOME: 36, LEFT: 37, PAGE_DOWN: 34, PAGE_UP: 33, PERIOD: 190, RIGHT: 39, SPACE: 32, TAB: 9, UP: 38 } }), e.fn.extend({ scrollParent: function (t) { var i = this.css("position"), s = "absolute" === i, n = t ? /(auto|scroll|hidden)/ : /(auto|scroll)/, a = this.parents().filter(function () { var t = e(this); return s && "static" === t.css("position") ? !1 : n.test(t.css("overflow") + t.css("overflow-y") + t.css("overflow-x")) }).eq(0); return "fixed" !== i && a.length ? a : e(this[0].ownerDocument || document) }, uniqueId: function () { var e = 0; return function () { return this.each(function () { this.id || (this.id = "ui-id-" + ++e) }) } }(), removeUniqueId: function () { return this.each(function () { /^ui-id-\d+$/.test(this.id) && e(this).removeAttr("id") }) } }), e.extend(e.expr[":"], { data: e.expr.createPseudo ? e.expr.createPseudo(function (t) { return function (i) { return !!e.data(i, t) } }) : function (t, i, s) { return !!e.data(t, s[3]) }, focusable: function (i) { return t(i, !isNaN(e.attr(i, "tabindex"))) }, tabbable: function (i) { var s = e.attr(i, "tabindex"), n = isNaN(s); return (n || s >= 0) && t(i, !n) } }), e("<a>").outerWidth(1).jquery || e.each(["Width", "Height"], function (t, i) { function s(t, i, s, a) { return e.each(n, function () { i -= parseFloat(e.css(t, "padding" + this)) || 0, s && (i -= parseFloat(e.css(t, "border" + this + "Width")) || 0), a && (i -= parseFloat(e.css(t, "margin" + this)) || 0) }), i } var n = "Width" === i ? ["Left", "Right"] : ["Top", "Bottom"], a = i.toLowerCase(), o = { innerWidth: e.fn.innerWidth, innerHeight: e.fn.innerHeight, outerWidth: e.fn.outerWidth, outerHeight: e.fn.outerHeight }; e.fn["inner" + i] = function (t) { return void 0 === t ? o["inner" + i].call(this) : this.each(function () { e(this).css(a, s(this, t) + "px") }) }, e.fn["outer" + i] = function (t, n) { return "number" != typeof t ? o["outer" + i].call(this, t) : this.each(function () { e(this).css(a, s(this, t, !0, n) + "px") }) } }), e.fn.addBack || (e.fn.addBack = function (e) { return this.add(null == e ? this.prevObject : this.prevObject.filter(e)) }), e("<a>").data("a-b", "a").removeData("a-b").data("a-b") && (e.fn.removeData = function (t) { return function (i) { return arguments.length ? t.call(this, e.camelCase(i)) : t.call(this) } }(e.fn.removeData)), e.ui.ie = !!/msie [\w.]+/.exec(navigator.userAgent.toLowerCase()), e.fn.extend({ focus: function (t) { return function (i, s) { return "number" == typeof i ? this.each(function () { var t = this; setTimeout(function () { e(t).focus(), s && s.call(t) }, i) }) : t.apply(this, arguments) } }(e.fn.focus), disableSelection: function () { var e = "onselectstart" in document.createElement("div") ? "selectstart" : "mousedown"; return function () { return this.bind(e + ".ui-disableSelection", function (e) { e.preventDefault() }) } }(), enableSelection: function () { return this.unbind(".ui-disableSelection") }, zIndex: function (t) { if (void 0 !== t) return this.css("zIndex", t); if (this.length) for (var i, s, n = e(this[0]); n.length && n[0] !== document;) { if (i = n.css("position"), ("absolute" === i || "relative" === i || "fixed" === i) && (s = parseInt(n.css("zIndex"), 10), !isNaN(s) && 0 !== s)) return s; n = n.parent() } return 0 } }), e.ui.plugin = { add: function (t, i, s) { var n, a = e.ui[t].prototype; for (n in s) a.plugins[n] = a.plugins[n] || [], a.plugins[n].push([i, s[n]]) }, call: function (e, t, i, s) { var n, a = e.plugins[t]; if (a && (s || e.element[0].parentNode && 11 !== e.element[0].parentNode.nodeType)) for (n = 0; a.length > n; n++) e.options[a[n][0]] && a[n][1].apply(e.element, i) } }; var s = 0, n = Array.prototype.slice; e.cleanData = function (t) { return function (i) { var s, n, a; for (a = 0; null != (n = i[a]); a++) try { s = e._data(n, "events"), s && s.remove && e(n).triggerHandler("remove") } catch (o) { } t(i) } }(e.cleanData), e.widget = function (t, i, s) { var n, a, o, r, h = {}, l = t.split(".")[0]; return t = t.split(".")[1], n = l + "-" + t, s || (s = i, i = e.Widget), e.expr[":"][n.toLowerCase()] = function (t) { return !!e.data(t, n) }, e[l] = e[l] || {}, a = e[l][t], o = e[l][t] = function (e, t) { return this._createWidget ? (arguments.length && this._createWidget(e, t), void 0) : new o(e, t) }, e.extend(o, a, { version: s.version, _proto: e.extend({}, s), _childConstructors: [] }), r = new i, r.options = e.widget.extend({}, r.options), e.each(s, function (t, s) { return e.isFunction(s) ? (h[t] = function () { var e = function () { return i.prototype[t].apply(this, arguments) }, n = function (e) { return i.prototype[t].apply(this, e) }; return function () { var t, i = this._super, a = this._superApply; return this._super = e, this._superApply = n, t = s.apply(this, arguments), this._super = i, this._superApply = a, t } }(), void 0) : (h[t] = s, void 0) }), o.prototype = e.widget.extend(r, { widgetEventPrefix: a ? r.widgetEventPrefix || t : t }, h, { constructor: o, namespace: l, widgetName: t, widgetFullName: n }), a ? (e.each(a._childConstructors, function (t, i) { var s = i.prototype; e.widget(s.namespace + "." + s.widgetName, o, i._proto) }), delete a._childConstructors) : i._childConstructors.push(o), e.widget.bridge(t, o), o }, e.widget.extend = function (t) { for (var i, s, a = n.call(arguments, 1), o = 0, r = a.length; r > o; o++) for (i in a[o]) s = a[o][i], a[o].hasOwnProperty(i) && void 0 !== s && (t[i] = e.isPlainObject(s) ? e.isPlainObject(t[i]) ? e.widget.extend({}, t[i], s) : e.widget.extend({}, s) : s); return t }, e.widget.bridge = function (t, i) { var s = i.prototype.widgetFullName || t; e.fn[t] = function (a) { var o = "string" == typeof a, r = n.call(arguments, 1), h = this; return a = !o && r.length ? e.widget.extend.apply(null, [a].concat(r)) : a, o ? this.each(function () { var i, n = e.data(this, s); return "instance" === a ? (h = n, !1) : n ? e.isFunction(n[a]) && "_" !== a.charAt(0) ? (i = n[a].apply(n, r), i !== n && void 0 !== i ? (h = i && i.jquery ? h.pushStack(i.get()) : i, !1) : void 0) : e.error("no such method '" + a + "' for " + t + " widget instance") : e.error("cannot call methods on " + t + " prior to initialization; " + "attempted to call method '" + a + "'") }) : this.each(function () { var t = e.data(this, s); t ? (t.option(a || {}), t._init && t._init()) : e.data(this, s, new i(a, this)) }), h } }, e.Widget = function () { }, e.Widget._childConstructors = [], e.Widget.prototype = { widgetName: "widget", widgetEventPrefix: "", defaultElement: "<div>", options: { disabled: !1, create: null }, _createWidget: function (t, i) { i = e(i || this.defaultElement || this)[0], this.element = e(i), this.uuid = s++ , this.eventNamespace = "." + this.widgetName + this.uuid, this.bindings = e(), this.hoverable = e(), this.focusable = e(), i !== this && (e.data(i, this.widgetFullName, this), this._on(!0, this.element, { remove: function (e) { e.target === i && this.destroy() } }), this.document = e(i.style ? i.ownerDocument : i.document || i), this.window = e(this.document[0].defaultView || this.document[0].parentWindow)), this.options = e.widget.extend({}, this.options, this._getCreateOptions(), t), this._create(), this._trigger("create", null, this._getCreateEventData()), this._init() }, _getCreateOptions: e.noop, _getCreateEventData: e.noop, _create: e.noop, _init: e.noop, destroy: function () { this._destroy(), this.element.unbind(this.eventNamespace).removeData(this.widgetFullName).removeData(e.camelCase(this.widgetFullName)), this.widget().unbind(this.eventNamespace).removeAttr("aria-disabled").removeClass(this.widgetFullName + "-disabled " + "ui-state-disabled"), this.bindings.unbind(this.eventNamespace), this.hoverable.removeClass("ui-state-hover"), this.focusable.removeClass("ui-state-focus") }, _destroy: e.noop, widget: function () { return this.element }, option: function (t, i) { var s, n, a, o = t; if (0 === arguments.length) return e.widget.extend({}, this.options); if ("string" == typeof t) if (o = {}, s = t.split("."), t = s.shift(), s.length) { for (n = o[t] = e.widget.extend({}, this.options[t]), a = 0; s.length - 1 > a; a++) n[s[a]] = n[s[a]] || {}, n = n[s[a]]; if (t = s.pop(), 1 === arguments.length) return void 0 === n[t] ? null : n[t]; n[t] = i } else { if (1 === arguments.length) return void 0 === this.options[t] ? null : this.options[t]; o[t] = i } return this._setOptions(o), this }, _setOptions: function (e) { var t; for (t in e) this._setOption(t, e[t]); return this }, _setOption: function (e, t) { return this.options[e] = t, "disabled" === e && (this.widget().toggleClass(this.widgetFullName + "-disabled", !!t), t && (this.hoverable.removeClass("ui-state-hover"), this.focusable.removeClass("ui-state-focus"))), this }, enable: function () { return this._setOptions({ disabled: !1 }) }, disable: function () { return this._setOptions({ disabled: !0 }) }, _on: function (t, i, s) { var n, a = this; "boolean" != typeof t && (s = i, i = t, t = !1), s ? (i = n = e(i), this.bindings = this.bindings.add(i)) : (s = i, i = this.element, n = this.widget()), e.each(s, function (s, o) { function r() { return t || a.options.disabled !== !0 && !e(this).hasClass("ui-state-disabled") ? ("string" == typeof o ? a[o] : o).apply(a, arguments) : void 0 } "string" != typeof o && (r.guid = o.guid = o.guid || r.guid || e.guid++); var h = s.match(/^([\w:-]*)\s*(.*)$/), l = h[1] + a.eventNamespace, u = h[2]; u ? n.delegate(u, l, r) : i.bind(l, r) }) }, _off: function (t, i) { i = (i || "").split(" ").join(this.eventNamespace + " ") + this.eventNamespace, t.unbind(i).undelegate(i), this.bindings = e(this.bindings.not(t).get()), this.focusable = e(this.focusable.not(t).get()), this.hoverable = e(this.hoverable.not(t).get()) }, _delay: function (e, t) { function i() { return ("string" == typeof e ? s[e] : e).apply(s, arguments) } var s = this; return setTimeout(i, t || 0) }, _hoverable: function (t) { this.hoverable = this.hoverable.add(t), this._on(t, { mouseenter: function (t) { e(t.currentTarget).addClass("ui-state-hover") }, mouseleave: function (t) { e(t.currentTarget).removeClass("ui-state-hover") } }) }, _focusable: function (t) { this.focusable = this.focusable.add(t), this._on(t, { focusin: function (t) { e(t.currentTarget).addClass("ui-state-focus") }, focusout: function (t) { e(t.currentTarget).removeClass("ui-state-focus") } }) }, _trigger: function (t, i, s) { var n, a, o = this.options[t]; if (s = s || {}, i = e.Event(i), i.type = (t === this.widgetEventPrefix ? t : this.widgetEventPrefix + t).toLowerCase(), i.target = this.element[0], a = i.originalEvent) for (n in a) n in i || (i[n] = a[n]); return this.element.trigger(i, s), !(e.isFunction(o) && o.apply(this.element[0], [i].concat(s)) === !1 || i.isDefaultPrevented()) } }, e.each({ show: "fadeIn", hide: "fadeOut" }, function (t, i) { e.Widget.prototype["_" + t] = function (s, n, a) { "string" == typeof n && (n = { effect: n }); var o, r = n ? n === !0 || "number" == typeof n ? i : n.effect || i : t; n = n || {}, "number" == typeof n && (n = { duration: n }), o = !e.isEmptyObject(n), n.complete = a, n.delay && s.delay(n.delay), o && e.effects && e.effects.effect[r] ? s[t](n) : r !== t && s[r] ? s[r](n.duration, n.easing, a) : s.queue(function (i) { e(this)[t](), a && a.call(s[0]), i() }) } }), e.widget; var a = !1; e(document).mouseup(function () { a = !1 }), e.widget("ui.mouse", { version: "1.11.2", options: { cancel: "input,textarea,button,select,option", distance: 1, delay: 0 }, _mouseInit: function () { var t = this; this.element.bind("mousedown." + this.widgetName, function (e) { return t._mouseDown(e) }).bind("click." + this.widgetName, function (i) { return !0 === e.data(i.target, t.widgetName + ".preventClickEvent") ? (e.removeData(i.target, t.widgetName + ".preventClickEvent"), i.stopImmediatePropagation(), !1) : void 0 }), this.started = !1 }, _mouseDestroy: function () { this.element.unbind("." + this.widgetName), this._mouseMoveDelegate && this.document.unbind("mousemove." + this.widgetName, this._mouseMoveDelegate).unbind("mouseup." + this.widgetName, this._mouseUpDelegate) }, _mouseDown: function (t) { if (!a) { this._mouseMoved = !1, this._mouseStarted && this._mouseUp(t), this._mouseDownEvent = t; var i = this, s = 1 === t.which, n = "string" == typeof this.options.cancel && t.target.nodeName ? e(t.target).closest(this.options.cancel).length : !1; return s && !n && this._mouseCapture(t) ? (this.mouseDelayMet = !this.options.delay, this.mouseDelayMet || (this._mouseDelayTimer = setTimeout(function () { i.mouseDelayMet = !0 }, this.options.delay)), this._mouseDistanceMet(t) && this._mouseDelayMet(t) && (this._mouseStarted = this._mouseStart(t) !== !1, !this._mouseStarted) ? (t.preventDefault(), !0) : (!0 === e.data(t.target, this.widgetName + ".preventClickEvent") && e.removeData(t.target, this.widgetName + ".preventClickEvent"), this._mouseMoveDelegate = function (e) { return i._mouseMove(e) }, this._mouseUpDelegate = function (e) { return i._mouseUp(e) }, this.document.bind("mousemove." + this.widgetName, this._mouseMoveDelegate).bind("mouseup." + this.widgetName, this._mouseUpDelegate), t.preventDefault(), a = !0, !0)) : !0 } }, _mouseMove: function (t) { if (this._mouseMoved) { if (e.ui.ie && (!document.documentMode || 9 > document.documentMode) && !t.button) return this._mouseUp(t); if (!t.which) return this._mouseUp(t) } return (t.which || t.button) && (this._mouseMoved = !0), this._mouseStarted ? (this._mouseDrag(t), t.preventDefault()) : (this._mouseDistanceMet(t) && this._mouseDelayMet(t) && (this._mouseStarted = this._mouseStart(this._mouseDownEvent, t) !== !1, this._mouseStarted ? this._mouseDrag(t) : this._mouseUp(t)), !this._mouseStarted) }, _mouseUp: function (t) { return this.document.unbind("mousemove." + this.widgetName, this._mouseMoveDelegate).unbind("mouseup." + this.widgetName, this._mouseUpDelegate), this._mouseStarted && (this._mouseStarted = !1, t.target === this._mouseDownEvent.target && e.data(t.target, this.widgetName + ".preventClickEvent", !0), this._mouseStop(t)), a = !1, !1 }, _mouseDistanceMet: function (e) { return Math.max(Math.abs(this._mouseDownEvent.pageX - e.pageX), Math.abs(this._mouseDownEvent.pageY - e.pageY)) >= this.options.distance }, _mouseDelayMet: function () { return this.mouseDelayMet }, _mouseStart: function () { }, _mouseDrag: function () { }, _mouseStop: function () { }, _mouseCapture: function () { return !0 } }), e.widget("ui.slider", e.ui.mouse, { version: "1.11.2", widgetEventPrefix: "slide", options: { animate: !1, distance: 0, max: 100, min: 0, orientation: "horizontal", range: !1, step: 1, value: 0, values: null, change: null, slide: null, start: null, stop: null }, numPages: 5, _create: function () { this._keySliding = !1, this._mouseSliding = !1, this._animateOff = !0, this._handleIndex = null, this._detectOrientation(), this._mouseInit(), this._calculateNewMax(), this.element.addClass("ui-slider ui-slider-" + this.orientation + " ui-widget" + " ui-widget-content" + " ui-corner-all"), this._refresh(), this._setOption("disabled", this.options.disabled), this._animateOff = !1 }, _refresh: function () { this._createRange(), this._createHandles(), this._setupEvents(), this._refreshValue() }, _createHandles: function () { var t, i, s = this.options, n = this.element.find(".ui-slider-handle").addClass("ui-state-default ui-corner-all"), a = "<span class='ui-slider-handle ui-state-default ui-corner-all' tabindex='0'></span>", o = []; for (i = s.values && s.values.length || 1, n.length > i && (n.slice(i).remove(), n = n.slice(0, i)), t = n.length; i > t; t++) o.push(a); this.handles = n.add(e(o.join("")).appendTo(this.element)), this.handle = this.handles.eq(0), this.handles.each(function (t) { e(this).data("ui-slider-handle-index", t) }) }, _createRange: function () { var t = this.options, i = ""; t.range ? (t.range === !0 && (t.values ? t.values.length && 2 !== t.values.length ? t.values = [t.values[0], t.values[0]] : e.isArray(t.values) && (t.values = t.values.slice(0)) : t.values = [this._valueMin(), this._valueMin()]), this.range && this.range.length ? this.range.removeClass("ui-slider-range-min ui-slider-range-max").css({ left: "", bottom: "" }) : (this.range = e("<div></div>").appendTo(this.element), i = "ui-slider-range ui-widget-header ui-corner-all"), this.range.addClass(i + ("min" === t.range || "max" === t.range ? " ui-slider-range-" + t.range : ""))) : (this.range && this.range.remove(), this.range = null) }, _setupEvents: function () { this._off(this.handles), this._on(this.handles, this._handleEvents), this._hoverable(this.handles), this._focusable(this.handles) }, _destroy: function () { this.handles.remove(), this.range && this.range.remove(), this.element.removeClass("ui-slider ui-slider-horizontal ui-slider-vertical ui-widget ui-widget-content ui-corner-all"), this._mouseDestroy() }, _mouseCapture: function (t) { var i, s, n, a, o, r, h, l, u = this, d = this.options; return d.disabled ? !1 : (this.elementSize = { width: this.element.outerWidth(), height: this.element.outerHeight() }, this.elementOffset = this.element.offset(), i = { x: t.pageX, y: t.pageY }, s = this._normValueFromMouse(i), n = this._valueMax() - this._valueMin() + 1, this.handles.each(function (t) { var i = Math.abs(s - u.values(t)); (n > i || n === i && (t === u._lastChangedValue || u.values(t) === d.min)) && (n = i, a = e(this), o = t) }), r = this._start(t, o), r === !1 ? !1 : (this._mouseSliding = !0, this._handleIndex = o, a.addClass("ui-state-active").focus(), h = a.offset(), l = !e(t.target).parents().addBack().is(".ui-slider-handle"), this._clickOffset = l ? { left: 0, top: 0 } : { left: t.pageX - h.left - a.width() / 2, top: t.pageY - h.top - a.height() / 2 - (parseInt(a.css("borderTopWidth"), 10) || 0) - (parseInt(a.css("borderBottomWidth"), 10) || 0) + (parseInt(a.css("marginTop"), 10) || 0) }, this.handles.hasClass("ui-state-hover") || this._slide(t, o, s), this._animateOff = !0, !0)) }, _mouseStart: function () { return !0 }, _mouseDrag: function (e) { var t = { x: e.pageX, y: e.pageY }, i = this._normValueFromMouse(t); return this._slide(e, this._handleIndex, i), !1 }, _mouseStop: function (e) { return this.handles.removeClass("ui-state-active"), this._mouseSliding = !1, this._stop(e, this._handleIndex), this._change(e, this._handleIndex), this._handleIndex = null, this._clickOffset = null, this._animateOff = !1, !1 }, _detectOrientation: function () { this.orientation = "vertical" === this.options.orientation ? "vertical" : "horizontal" }, _normValueFromMouse: function (e) { var t, i, s, n, a; return "horizontal" === this.orientation ? (t = this.elementSize.width, i = e.x - this.elementOffset.left - (this._clickOffset ? this._clickOffset.left : 0)) : (t = this.elementSize.height, i = e.y - this.elementOffset.top - (this._clickOffset ? this._clickOffset.top : 0)), s = i / t, s > 1 && (s = 1), 0 > s && (s = 0), "vertical" === this.orientation && (s = 1 - s), n = this._valueMax() - this._valueMin(), a = this._valueMin() + s * n, this._trimAlignValue(a) }, _start: function (e, t) { var i = { handle: this.handles[t], value: this.value() }; return this.options.values && this.options.values.length && (i.value = this.values(t), i.values = this.values()), this._trigger("start", e, i) }, _slide: function (e, t, i) { var s, n, a; this.options.values && this.options.values.length ? (s = this.values(t ? 0 : 1), 2 === this.options.values.length && this.options.range === !0 && (0 === t && i > s || 1 === t && s > i) && (i = s), i !== this.values(t) && (n = this.values(), n[t] = i, a = this._trigger("slide", e, { handle: this.handles[t], value: i, values: n }), s = this.values(t ? 0 : 1), a !== !1 && this.values(t, i))) : i !== this.value() && (a = this._trigger("slide", e, { handle: this.handles[t], value: i }), a !== !1 && this.value(i)) }, _stop: function (e, t) { var i = { handle: this.handles[t], value: this.value() }; this.options.values && this.options.values.length && (i.value = this.values(t), i.values = this.values()), this._trigger("stop", e, i) }, _change: function (e, t) { if (!this._keySliding && !this._mouseSliding) { var i = { handle: this.handles[t], value: this.value() }; this.options.values && this.options.values.length && (i.value = this.values(t), i.values = this.values()), this._lastChangedValue = t, this._trigger("change", e, i) } }, value: function (e) { return arguments.length ? (this.options.value = this._trimAlignValue(e), this._refreshValue(), this._change(null, 0), void 0) : this._value() }, values: function (t, i) { var s, n, a; if (arguments.length > 1) return this.options.values[t] = this._trimAlignValue(i), this._refreshValue(), this._change(null, t), void 0; if (!arguments.length) return this._values(); if (!e.isArray(arguments[0])) return this.options.values && this.options.values.length ? this._values(t) : this.value(); for (s = this.options.values, n = arguments[0], a = 0; s.length > a; a += 1) s[a] = this._trimAlignValue(n[a]), this._change(null, a); this._refreshValue() }, _setOption: function (t, i) { var s, n = 0; switch ("range" === t && this.options.range === !0 && ("min" === i ? (this.options.value = this._values(0), this.options.values = null) : "max" === i && (this.options.value = this._values(this.options.values.length - 1), this.options.values = null)), e.isArray(this.options.values) && (n = this.options.values.length), "disabled" === t && this.element.toggleClass("ui-state-disabled", !!i), this._super(t, i), t) { case "orientation": this._detectOrientation(), this.element.removeClass("ui-slider-horizontal ui-slider-vertical").addClass("ui-slider-" + this.orientation), this._refreshValue(), this.handles.css("horizontal" === i ? "bottom" : "left", ""); break; case "value": this._animateOff = !0, this._refreshValue(), this._change(null, 0), this._animateOff = !1; break; case "values": for (this._animateOff = !0, this._refreshValue(), s = 0; n > s; s += 1) this._change(null, s); this._animateOff = !1; break; case "step": case "min": case "max": this._animateOff = !0, this._calculateNewMax(), this._refreshValue(), this._animateOff = !1; break; case "range": this._animateOff = !0, this._refresh(), this._animateOff = !1 } }, _value: function () { var e = this.options.value; return e = this._trimAlignValue(e) }, _values: function (e) { var t, i, s; if (arguments.length) return t = this.options.values[e], t = this._trimAlignValue(t); if (this.options.values && this.options.values.length) { for (i = this.options.values.slice(), s = 0; i.length > s; s += 1) i[s] = this._trimAlignValue(i[s]); return i } return [] }, _trimAlignValue: function (e) { if (this._valueMin() >= e) return this._valueMin(); if (e >= this._valueMax()) return this._valueMax(); var t = this.options.step > 0 ? this.options.step : 1, i = (e - this._valueMin()) % t, s = e - i; return 2 * Math.abs(i) >= t && (s += i > 0 ? t : -t), parseFloat(s.toFixed(5)) }, _calculateNewMax: function () { var e = (this.options.max - this._valueMin()) % this.options.step; this.max = this.options.max - e }, _valueMin: function () { return this.options.min }, _valueMax: function () { return this.max }, _refreshValue: function () { var t, i, s, n, a, o = this.options.range, r = this.options, h = this, l = this._animateOff ? !1 : r.animate, u = {}; this.options.values && this.options.values.length ? this.handles.each(function (s) { i = 100 * ((h.values(s) - h._valueMin()) / (h._valueMax() - h._valueMin())), u["horizontal" === h.orientation ? "left" : "bottom"] = i + "%", e(this).stop(1, 1)[l ? "animate" : "css"](u, r.animate), h.options.range === !0 && ("horizontal" === h.orientation ? (0 === s && h.range.stop(1, 1)[l ? "animate" : "css"]({ left: i + "%" }, r.animate), 1 === s && h.range[l ? "animate" : "css"]({ width: i - t + "%" }, { queue: !1, duration: r.animate })) : (0 === s && h.range.stop(1, 1)[l ? "animate" : "css"]({ bottom: i + "%" }, r.animate), 1 === s && h.range[l ? "animate" : "css"]({ height: i - t + "%" }, { queue: !1, duration: r.animate }))), t = i }) : (s = this.value(), n = this._valueMin(), a = this._valueMax(), i = a !== n ? 100 * ((s - n) / (a - n)) : 0, u["horizontal" === this.orientation ? "left" : "bottom"] = i + "%", this.handle.stop(1, 1)[l ? "animate" : "css"](u, r.animate), "min" === o && "horizontal" === this.orientation && this.range.stop(1, 1)[l ? "animate" : "css"]({ width: i + "%" }, r.animate), "max" === o && "horizontal" === this.orientation && this.range[l ? "animate" : "css"]({ width: 100 - i + "%" }, { queue: !1, duration: r.animate }), "min" === o && "vertical" === this.orientation && this.range.stop(1, 1)[l ? "animate" : "css"]({ height: i + "%" }, r.animate), "max" === o && "vertical" === this.orientation && this.range[l ? "animate" : "css"]({ height: 100 - i + "%" }, { queue: !1, duration: r.animate })) }, _handleEvents: { keydown: function (t) { var i, s, n, a, o = e(t.target).data("ui-slider-handle-index"); switch (t.keyCode) { case e.ui.keyCode.HOME: case e.ui.keyCode.END: case e.ui.keyCode.PAGE_UP: case e.ui.keyCode.PAGE_DOWN: case e.ui.keyCode.UP: case e.ui.keyCode.RIGHT: case e.ui.keyCode.DOWN: case e.ui.keyCode.LEFT: if (t.preventDefault(), !this._keySliding && (this._keySliding = !0, e(t.target).addClass("ui-state-active"), i = this._start(t, o), i === !1)) return } switch (a = this.options.step, s = n = this.options.values && this.options.values.length ? this.values(o) : this.value(), t.keyCode) { case e.ui.keyCode.HOME: n = this._valueMin(); break; case e.ui.keyCode.END: n = this._valueMax(); break; case e.ui.keyCode.PAGE_UP: n = this._trimAlignValue(s + (this._valueMax() - this._valueMin()) / this.numPages); break; case e.ui.keyCode.PAGE_DOWN: n = this._trimAlignValue(s - (this._valueMax() - this._valueMin()) / this.numPages); break; case e.ui.keyCode.UP: case e.ui.keyCode.RIGHT: if (s === this._valueMax()) return; n = this._trimAlignValue(s + a); break; case e.ui.keyCode.DOWN: case e.ui.keyCode.LEFT: if (s === this._valueMin()) return; n = this._trimAlignValue(s - a) } this._slide(t, o, n) }, keyup: function (t) { var i = e(t.target).data("ui-slider-handle-index"); this._keySliding && (this._keySliding = !1, this._stop(t, i), this._change(t, i), e(t.target).removeClass("ui-state-active")) } } }) });

/*!
 * jQuery UI Touch Punch 0.2.3
 *
 * Copyright 2011–2014, Dave Furfero
 * Dual licensed under the MIT or GPL Version 2 licenses.
 *
 * Depends:
 *  jquery.ui.widget.js
 *  jquery.ui.mouse.js
 */
!function (a) { function f(a, b) { if (!(a.originalEvent.touches.length > 1)) { a.preventDefault(); var c = a.originalEvent.changedTouches[0], d = document.createEvent("MouseEvents"); d.initMouseEvent(b, !0, !0, window, 1, c.screenX, c.screenY, c.clientX, c.clientY, !1, !1, !1, !1, 0, null), a.target.dispatchEvent(d) } } if (a.support.touch = "ontouchend" in document, a.support.touch) { var e, b = a.ui.mouse.prototype, c = b._mouseInit, d = b._mouseDestroy; b._touchStart = function (a) { var b = this; !e && b._mouseCapture(a.originalEvent.changedTouches[0]) && (e = !0, b._touchMoved = !1, f(a, "mouseover"), f(a, "mousemove"), f(a, "mousedown")) }, b._touchMove = function (a) { e && (this._touchMoved = !0, f(a, "mousemove")) }, b._touchEnd = function (a) { e && (f(a, "mouseup"), f(a, "mouseout"), this._touchMoved || f(a, "click"), e = !1) }, b._mouseInit = function () { var b = this; b.element.bind({ touchstart: a.proxy(b, "_touchStart"), touchmove: a.proxy(b, "_touchMove"), touchend: a.proxy(b, "_touchEnd") }), c.call(b) }, b._mouseDestroy = function () { var b = this; b.element.unbind({ touchstart: a.proxy(b, "_touchStart"), touchmove: a.proxy(b, "_touchMove"), touchend: a.proxy(b, "_touchEnd") }), d.call(b) } } }(jQuery);


// Ion.RangeSlider | version 2.1.4 | https://github.com/IonDen/ion.rangeSlider
; (function (g) { "function" === typeof define && define.amd ? define(["jquery"], function (q) { g(q, document, window, navigator) }) : g(jQuery, document, window, navigator) })(function (g, q, h, t, v) {
    var u = 0, p = function () { var a = t.userAgent, b = /msie\s\d+/i; return 0 < a.search(b) && (a = b.exec(a).toString(), a = a.split(" ")[1], 9 > a) ? (g("html").addClass("lt-ie9"), !0) : !1 }(); Function.prototype.bind || (Function.prototype.bind = function (a) {
        var b = this, d = [].slice; if ("function" != typeof b) throw new TypeError; var c = d.call(arguments, 1), e = function () {
            if (this instanceof
                e) { var f = function () { }; f.prototype = b.prototype; var f = new f, l = b.apply(f, c.concat(d.call(arguments))); return Object(l) === l ? l : f } return b.apply(a, c.concat(d.call(arguments)))
        }; return e
    }); Array.prototype.indexOf || (Array.prototype.indexOf = function (a, b) { var d; if (null == this) throw new TypeError('"this" is null or not defined'); var c = Object(this), e = c.length >>> 0; if (0 === e) return -1; d = +b || 0; Infinity === Math.abs(d) && (d = 0); if (d >= e) return -1; for (d = Math.max(0 <= d ? d : e - Math.abs(d), 0); d < e;) { if (d in c && c[d] === a) return d; d++ } return -1 });
    var r = function (a, b, d) {
        this.VERSION = "2.1.4"; this.input = a; this.plugin_count = d; this.old_to = this.old_from = this.update_tm = this.calc_count = this.current_plugin = 0; this.raf_id = this.old_min_interval = null; this.is_update = this.is_key = this.no_diapason = this.force_redraw = this.dragging = !1; this.is_start = !0; this.is_click = this.is_resize = this.is_active = this.is_finish = !1; this.$cache = {
            win: g(h), body: g(q.body), input: g(a), cont: null, rs: null, min: null, max: null, from: null, to: null, single: null, bar: null, line: null, s_single: null, s_from: null,
            s_to: null, shad_single: null, shad_from: null, shad_to: null, edge: null, grid: null, grid_labels: []
        }; this.coords = { x_gap: 0, x_pointer: 0, w_rs: 0, w_rs_old: 0, w_handle: 0, p_gap: 0, p_gap_left: 0, p_gap_right: 0, p_step: 0, p_pointer: 0, p_handle: 0, p_single_fake: 0, p_single_real: 0, p_from_fake: 0, p_from_real: 0, p_to_fake: 0, p_to_real: 0, p_bar_x: 0, p_bar_w: 0, grid_gap: 0, big_num: 0, big: [], big_w: [], big_p: [], big_x: [] }; this.labels = {
            w_min: 0, w_max: 0, w_from: 0, w_to: 0, w_single: 0, p_min: 0, p_max: 0, p_from_fake: 0, p_from_left: 0, p_to_fake: 0, p_to_left: 0,
            p_single_fake: 0, p_single_left: 0
        }; var c = this.$cache.input; a = c.prop("value"); var e; d = {
            type: "single", min: 10, max: 100, from: null, to: null, step: 1, min_interval: 0, max_interval: 0, drag_interval: !1, values: [], p_values: [], from_fixed: !1, from_min: null, from_max: null, from_shadow: !1, to_fixed: !1, to_min: null, to_max: null, to_shadow: !1, prettify_enabled: !0, prettify_separator: " ", prettify: null, force_edges: !1, keyboard: !1, keyboard_step: 5, grid: !1, grid_margin: !0, grid_num: 4, grid_snap: !1, hide_min_max: !1, hide_from_to: !1, prefix: "",
            postfix: "", max_postfix: "", decorate_both: !0, values_separator: " \u2014 ", input_values_separator: ";", disable: !1, onStart: null, onChange: null, onFinish: null, onUpdate: null
        }; c = {
            type: c.data("type"), min: c.data("min"), max: c.data("max"), from: c.data("from"), to: c.data("to"), step: c.data("step"), min_interval: c.data("minInterval"), max_interval: c.data("maxInterval"), drag_interval: c.data("dragInterval"), values: c.data("values"), from_fixed: c.data("fromFixed"), from_min: c.data("fromMin"), from_max: c.data("fromMax"), from_shadow: c.data("fromShadow"),
            to_fixed: c.data("toFixed"), to_min: c.data("toMin"), to_max: c.data("toMax"), to_shadow: c.data("toShadow"), prettify_enabled: c.data("prettifyEnabled"), prettify_separator: c.data("prettifySeparator"), force_edges: c.data("forceEdges"), keyboard: c.data("keyboard"), keyboard_step: c.data("keyboardStep"), grid: c.data("grid"), grid_margin: c.data("gridMargin"), grid_num: c.data("gridNum"), grid_snap: c.data("gridSnap"), hide_min_max: c.data("hideMinMax"), hide_from_to: c.data("hideFromTo"), prefix: c.data("prefix"), postfix: c.data("postfix"),
            max_postfix: c.data("maxPostfix"), decorate_both: c.data("decorateBoth"), values_separator: c.data("valuesSeparator"), input_values_separator: c.data("inputValuesSeparator"), disable: c.data("disable")
        }; c.values = c.values && c.values.split(","); for (e in c) c.hasOwnProperty(e) && (c[e] || 0 === c[e] || delete c[e]); a && (a = a.split(c.input_values_separator || b.input_values_separator || ";"), a[0] && a[0] == +a[0] && (a[0] = +a[0]), a[1] && a[1] == +a[1] && (a[1] = +a[1]), b && b.values && b.values.length ? (d.from = a[0] && b.values.indexOf(a[0]), d.to =
            a[1] && b.values.indexOf(a[1])) : (d.from = a[0] && +a[0], d.to = a[1] && +a[1])); g.extend(d, b); g.extend(d, c); this.options = d; this.validate(); this.result = { input: this.$cache.input, slider: null, min: this.options.min, max: this.options.max, from: this.options.from, from_percent: 0, from_value: null, to: this.options.to, to_percent: 0, to_value: null }; this.init()
    }; r.prototype = {
        init: function (a) {
            this.no_diapason = !1; this.coords.p_step = this.convertToPercent(this.options.step, !0); this.target = "base"; this.toggleInput(); this.append(); this.setMinMax();
            a ? (this.force_redraw = !0, this.calc(!0), this.callOnUpdate()) : (this.force_redraw = !0, this.calc(!0), this.callOnStart()); this.updateScene()
        }, append: function () {
            this.$cache.input.before('<span class="irs js-irs-' + this.plugin_count + '"></span>'); this.$cache.input.prop("readonly", !0); this.$cache.cont = this.$cache.input.prev(); this.result.slider = this.$cache.cont; this.$cache.cont.html('<span class="irs"><span class="irs-line" tabindex="-1"><span class="irs-line-left"></span><span class="irs-line-mid"></span><span class="irs-line-right"></span></span><span class="irs-min">0</span><span class="irs-max">1</span><span class="irs-from">0</span><span class="irs-to">0</span><span class="irs-single">0</span></span><span class="irs-grid"></span><span class="irs-bar"></span>');
            this.$cache.rs = this.$cache.cont.find(".irs"); this.$cache.min = this.$cache.cont.find(".irs-min"); this.$cache.max = this.$cache.cont.find(".irs-max"); this.$cache.from = this.$cache.cont.find(".irs-from"); this.$cache.to = this.$cache.cont.find(".irs-to"); this.$cache.single = this.$cache.cont.find(".irs-single"); this.$cache.bar = this.$cache.cont.find(".irs-bar"); this.$cache.line = this.$cache.cont.find(".irs-line"); this.$cache.grid = this.$cache.cont.find(".irs-grid"); "single" === this.options.type ? (this.$cache.cont.append('<span class="irs-bar-edge"></span><span class="irs-shadow shadow-single"></span><span class="irs-slider single"></span>'),
                this.$cache.edge = this.$cache.cont.find(".irs-bar-edge"), this.$cache.s_single = this.$cache.cont.find(".single"), this.$cache.from[0].style.visibility = "hidden", this.$cache.to[0].style.visibility = "hidden", this.$cache.shad_single = this.$cache.cont.find(".shadow-single")) : (this.$cache.cont.append('<span class="irs-shadow shadow-from"></span><span class="irs-shadow shadow-to"></span><span class="irs-slider from"></span><span class="irs-slider to"></span>'), this.$cache.s_from = this.$cache.cont.find(".from"),
                    this.$cache.s_to = this.$cache.cont.find(".to"), this.$cache.shad_from = this.$cache.cont.find(".shadow-from"), this.$cache.shad_to = this.$cache.cont.find(".shadow-to"), this.setTopHandler()); this.options.hide_from_to && (this.$cache.from[0].style.display = "none", this.$cache.to[0].style.display = "none", this.$cache.single[0].style.display = "none"); this.appendGrid(); this.options.disable ? (this.appendDisableMask(), this.$cache.input[0].disabled = !0) : (this.$cache.cont.removeClass("irs-disabled"), this.$cache.input[0].disabled =
                        !1, this.bindEvents()); this.options.drag_interval && (this.$cache.bar[0].style.cursor = "ew-resize")
        }, setTopHandler: function () { var a = this.options.max, b = this.options.to; this.options.from > this.options.min && b === a ? this.$cache.s_from.addClass("type_last") : b < a && this.$cache.s_to.addClass("type_last") }, changeLevel: function (a) {
            switch (a) {
                case "single": this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_single_fake); break; case "from": this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_from_fake);
                    this.$cache.s_from.addClass("state_hover"); this.$cache.s_from.addClass("type_last"); this.$cache.s_to.removeClass("type_last"); break; case "to": this.coords.p_gap = this.toFixed(this.coords.p_pointer - this.coords.p_to_fake); this.$cache.s_to.addClass("state_hover"); this.$cache.s_to.addClass("type_last"); this.$cache.s_from.removeClass("type_last"); break; case "both": this.coords.p_gap_left = this.toFixed(this.coords.p_pointer - this.coords.p_from_fake), this.coords.p_gap_right = this.toFixed(this.coords.p_to_fake -
                        this.coords.p_pointer), this.$cache.s_to.removeClass("type_last"), this.$cache.s_from.removeClass("type_last")
            }
        }, appendDisableMask: function () { this.$cache.cont.append('<span class="irs-disable-mask"></span>'); this.$cache.cont.addClass("irs-disabled") }, remove: function () {
            this.$cache.cont.remove(); this.$cache.cont = null; this.$cache.line.off("keydown.irs_" + this.plugin_count); this.$cache.body.off("touchmove.irs_" + this.plugin_count); this.$cache.body.off("mousemove.irs_" + this.plugin_count); this.$cache.win.off("touchend.irs_" +
                this.plugin_count); this.$cache.win.off("mouseup.irs_" + this.plugin_count); p && (this.$cache.body.off("mouseup.irs_" + this.plugin_count), this.$cache.body.off("mouseleave.irs_" + this.plugin_count)); this.$cache.grid_labels = []; this.coords.big = []; this.coords.big_w = []; this.coords.big_p = []; this.coords.big_x = []; cancelAnimationFrame(this.raf_id)
        }, bindEvents: function () {
            if (!this.no_diapason) {
                this.$cache.body.on("touchmove.irs_" + this.plugin_count, this.pointerMove.bind(this)); this.$cache.body.on("mousemove.irs_" + this.plugin_count,
                    this.pointerMove.bind(this)); this.$cache.win.on("touchend.irs_" + this.plugin_count, this.pointerUp.bind(this)); this.$cache.win.on("mouseup.irs_" + this.plugin_count, this.pointerUp.bind(this)); this.$cache.line.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")); this.$cache.line.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")); this.options.drag_interval && "double" === this.options.type ? (this.$cache.bar.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this,
                        "both")), this.$cache.bar.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "both"))) : (this.$cache.bar.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.bar.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click"))); "single" === this.options.type ? (this.$cache.single.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.s_single.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")),
                            this.$cache.shad_single.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.single.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.s_single.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "single")), this.$cache.edge.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.shad_single.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click"))) : (this.$cache.single.on("touchstart.irs_" +
                                this.plugin_count, this.pointerDown.bind(this, null)), this.$cache.single.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, null)), this.$cache.from.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.s_from.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.to.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.s_to.on("touchstart.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")),
                                this.$cache.shad_from.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.shad_to.on("touchstart.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.from.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.s_from.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "from")), this.$cache.to.on("mousedown.irs_" + this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.s_to.on("mousedown.irs_" +
                                    this.plugin_count, this.pointerDown.bind(this, "to")), this.$cache.shad_from.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click")), this.$cache.shad_to.on("mousedown.irs_" + this.plugin_count, this.pointerClick.bind(this, "click"))); if (this.options.keyboard) this.$cache.line.on("keydown.irs_" + this.plugin_count, this.key.bind(this, "keyboard")); p && (this.$cache.body.on("mouseup.irs_" + this.plugin_count, this.pointerUp.bind(this)), this.$cache.body.on("mouseleave.irs_" + this.plugin_count, this.pointerUp.bind(this)))
            }
        },
        pointerMove: function (a) { this.dragging && (this.coords.x_pointer = (a.pageX || a.originalEvent.touches && a.originalEvent.touches[0].pageX) - this.coords.x_gap, this.calc()) }, pointerUp: function (a) {
            if (this.current_plugin === this.plugin_count && this.is_active) {
                this.is_active = !1; this.$cache.cont.find(".state_hover").removeClass("state_hover"); this.force_redraw = !0; p && g("*").prop("unselectable", !1); this.updateScene(); this.restoreOriginalMinInterval(); if (g.contains(this.$cache.cont[0], a.target) || this.dragging) this.is_finish =
                    !0, this.callOnFinish(); this.dragging = !1
            }
        }, pointerDown: function (a, b) {
            b.preventDefault(); var d = b.pageX || b.originalEvent.touches && b.originalEvent.touches[0].pageX; 2 !== b.button && ("both" === a && this.setTempMinInterval(), a || (a = this.target), this.current_plugin = this.plugin_count, this.target = a, this.dragging = this.is_active = !0, this.coords.x_gap = this.$cache.rs.offset().left, this.coords.x_pointer = d - this.coords.x_gap, this.calcPointerPercent(), this.changeLevel(a), p && g("*").prop("unselectable", !0), this.$cache.line.trigger("focus"),
                this.updateScene())
        }, pointerClick: function (a, b) { b.preventDefault(); var d = b.pageX || b.originalEvent.touches && b.originalEvent.touches[0].pageX; 2 !== b.button && (this.current_plugin = this.plugin_count, this.target = a, this.is_click = !0, this.coords.x_gap = this.$cache.rs.offset().left, this.coords.x_pointer = +(d - this.coords.x_gap).toFixed(), this.force_redraw = !0, this.calc(), this.$cache.line.trigger("focus")) }, key: function (a, b) {
            if (!(this.current_plugin !== this.plugin_count || b.altKey || b.ctrlKey || b.shiftKey || b.metaKey)) {
                switch (b.which) {
                    case 83: case 65: case 40: case 37: b.preventDefault();
                        this.moveByKey(!1); break; case 87: case 68: case 38: case 39: b.preventDefault(), this.moveByKey(!0)
                } return !0
            }
        }, moveByKey: function (a) { var b = this.coords.p_pointer, b = a ? b + this.options.keyboard_step : b - this.options.keyboard_step; this.coords.x_pointer = this.toFixed(this.coords.w_rs / 100 * b); this.is_key = !0; this.calc() }, setMinMax: function () {
            this.options && (this.options.hide_min_max ? (this.$cache.min[0].style.display = "none", this.$cache.max[0].style.display = "none") : (this.options.values.length ? (this.$cache.min.html(this.decorate(this.options.p_values[this.options.min])),
                this.$cache.max.html(this.decorate(this.options.p_values[this.options.max]))) : (this.$cache.min.html(this.decorate(this._prettify(this.options.min), this.options.min)), this.$cache.max.html(this.decorate(this._prettify(this.options.max), this.options.max))), this.labels.w_min = this.$cache.min.outerWidth(!1), this.labels.w_max = this.$cache.max.outerWidth(!1)))
        }, setTempMinInterval: function () {
            var a = this.result.to - this.result.from; null === this.old_min_interval && (this.old_min_interval = this.options.min_interval);
            this.options.min_interval = a
        }, restoreOriginalMinInterval: function () { null !== this.old_min_interval && (this.options.min_interval = this.old_min_interval, this.old_min_interval = null) }, calc: function (a) {
            if (this.options) {
                this.calc_count++; if (10 === this.calc_count || a) this.calc_count = 0, this.coords.w_rs = this.$cache.rs.outerWidth(!1), this.calcHandlePercent(); if (this.coords.w_rs) {
                    this.calcPointerPercent(); a = this.getHandleX(); "click" === this.target && (this.coords.p_gap = this.coords.p_handle / 2, a = this.getHandleX(), this.target =
                        this.options.drag_interval ? "both_one" : this.chooseHandle(a)); switch (this.target) {
                            case "base": var b = (this.options.max - this.options.min) / 100; a = (this.result.from - this.options.min) / b; b = (this.result.to - this.options.min) / b; this.coords.p_single_real = this.toFixed(a); this.coords.p_from_real = this.toFixed(a); this.coords.p_to_real = this.toFixed(b); this.coords.p_single_real = this.checkDiapason(this.coords.p_single_real, this.options.from_min, this.options.from_max); this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real,
                                this.options.from_min, this.options.from_max); this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max); this.coords.p_single_fake = this.convertToFakePercent(this.coords.p_single_real); this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real); this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real); this.target = null; break; case "single": if (this.options.from_fixed) break; this.coords.p_single_real = this.convertToRealPercent(a); this.coords.p_single_real =
                                    this.calcWithStep(this.coords.p_single_real); this.coords.p_single_real = this.checkDiapason(this.coords.p_single_real, this.options.from_min, this.options.from_max); this.coords.p_single_fake = this.convertToFakePercent(this.coords.p_single_real); break; case "from": if (this.options.from_fixed) break; this.coords.p_from_real = this.convertToRealPercent(a); this.coords.p_from_real = this.calcWithStep(this.coords.p_from_real); this.coords.p_from_real > this.coords.p_to_real && (this.coords.p_from_real = this.coords.p_to_real);
                                this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max); this.coords.p_from_real = this.checkMinInterval(this.coords.p_from_real, this.coords.p_to_real, "from"); this.coords.p_from_real = this.checkMaxInterval(this.coords.p_from_real, this.coords.p_to_real, "from"); this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real); break; case "to": if (this.options.to_fixed) break; this.coords.p_to_real = this.convertToRealPercent(a); this.coords.p_to_real =
                                    this.calcWithStep(this.coords.p_to_real); this.coords.p_to_real < this.coords.p_from_real && (this.coords.p_to_real = this.coords.p_from_real); this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max); this.coords.p_to_real = this.checkMinInterval(this.coords.p_to_real, this.coords.p_from_real, "to"); this.coords.p_to_real = this.checkMaxInterval(this.coords.p_to_real, this.coords.p_from_real, "to"); this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real);
                                break; case "both": if (this.options.from_fixed || this.options.to_fixed) break; a = this.toFixed(a + .1 * this.coords.p_handle); this.coords.p_from_real = this.convertToRealPercent(a) - this.coords.p_gap_left; this.coords.p_from_real = this.calcWithStep(this.coords.p_from_real); this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max); this.coords.p_from_real = this.checkMinInterval(this.coords.p_from_real, this.coords.p_to_real, "from"); this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real);
                                this.coords.p_to_real = this.convertToRealPercent(a) + this.coords.p_gap_right; this.coords.p_to_real = this.calcWithStep(this.coords.p_to_real); this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max); this.coords.p_to_real = this.checkMinInterval(this.coords.p_to_real, this.coords.p_from_real, "to"); this.coords.p_to_fake = this.convertToFakePercent(this.coords.p_to_real); break; case "both_one": if (!this.options.from_fixed && !this.options.to_fixed) {
                                    var d = this.convertToRealPercent(a);
                                    a = this.result.to_percent - this.result.from_percent; var c = a / 2, b = d - c, d = d + c; 0 > b && (b = 0, d = b + a); 100 < d && (d = 100, b = d - a); this.coords.p_from_real = this.calcWithStep(b); this.coords.p_from_real = this.checkDiapason(this.coords.p_from_real, this.options.from_min, this.options.from_max); this.coords.p_from_fake = this.convertToFakePercent(this.coords.p_from_real); this.coords.p_to_real = this.calcWithStep(d); this.coords.p_to_real = this.checkDiapason(this.coords.p_to_real, this.options.to_min, this.options.to_max); this.coords.p_to_fake =
                                        this.convertToFakePercent(this.coords.p_to_real)
                                }
                        } "single" === this.options.type ? (this.coords.p_bar_x = this.coords.p_handle / 2, this.coords.p_bar_w = this.coords.p_single_fake, this.result.from_percent = this.coords.p_single_real, this.result.from = this.convertToValue(this.coords.p_single_real), this.options.values.length && (this.result.from_value = this.options.values[this.result.from])) : (this.coords.p_bar_x = this.toFixed(this.coords.p_from_fake + this.coords.p_handle / 2), this.coords.p_bar_w = this.toFixed(this.coords.p_to_fake -
                            this.coords.p_from_fake), this.result.from_percent = this.coords.p_from_real, this.result.from = this.convertToValue(this.coords.p_from_real), this.result.to_percent = this.coords.p_to_real, this.result.to = this.convertToValue(this.coords.p_to_real), this.options.values.length && (this.result.from_value = this.options.values[this.result.from], this.result.to_value = this.options.values[this.result.to])); this.calcMinMax(); this.calcLabels()
                }
            }
        }, calcPointerPercent: function () {
            this.coords.w_rs ? (0 > this.coords.x_pointer || isNaN(this.coords.x_pointer) ?
                this.coords.x_pointer = 0 : this.coords.x_pointer > this.coords.w_rs && (this.coords.x_pointer = this.coords.w_rs), this.coords.p_pointer = this.toFixed(this.coords.x_pointer / this.coords.w_rs * 100)) : this.coords.p_pointer = 0
        }, convertToRealPercent: function (a) { return a / (100 - this.coords.p_handle) * 100 }, convertToFakePercent: function (a) { return a / 100 * (100 - this.coords.p_handle) }, getHandleX: function () { var a = 100 - this.coords.p_handle, b = this.toFixed(this.coords.p_pointer - this.coords.p_gap); 0 > b ? b = 0 : b > a && (b = a); return b }, calcHandlePercent: function () {
            this.coords.w_handle =
                "single" === this.options.type ? this.$cache.s_single.outerWidth(!1) : this.$cache.s_from.outerWidth(!1); this.coords.p_handle = this.toFixed(this.coords.w_handle / this.coords.w_rs * 100)
        }, chooseHandle: function (a) { return "single" === this.options.type ? "single" : a >= this.coords.p_from_real + (this.coords.p_to_real - this.coords.p_from_real) / 2 ? this.options.to_fixed ? "from" : "to" : this.options.from_fixed ? "to" : "from" }, calcMinMax: function () {
            this.coords.w_rs && (this.labels.p_min = this.labels.w_min / this.coords.w_rs * 100, this.labels.p_max =
                this.labels.w_max / this.coords.w_rs * 100)
        }, calcLabels: function () {
            this.coords.w_rs && !this.options.hide_from_to && ("single" === this.options.type ? (this.labels.w_single = this.$cache.single.outerWidth(!1), this.labels.p_single_fake = this.labels.w_single / this.coords.w_rs * 100, this.labels.p_single_left = this.coords.p_single_fake + this.coords.p_handle / 2 - this.labels.p_single_fake / 2) : (this.labels.w_from = this.$cache.from.outerWidth(!1), this.labels.p_from_fake = this.labels.w_from / this.coords.w_rs * 100, this.labels.p_from_left =
                this.coords.p_from_fake + this.coords.p_handle / 2 - this.labels.p_from_fake / 2, this.labels.p_from_left = this.toFixed(this.labels.p_from_left), this.labels.p_from_left = this.checkEdges(this.labels.p_from_left, this.labels.p_from_fake), this.labels.w_to = this.$cache.to.outerWidth(!1), this.labels.p_to_fake = this.labels.w_to / this.coords.w_rs * 100, this.labels.p_to_left = this.coords.p_to_fake + this.coords.p_handle / 2 - this.labels.p_to_fake / 2, this.labels.p_to_left = this.toFixed(this.labels.p_to_left), this.labels.p_to_left =
                this.checkEdges(this.labels.p_to_left, this.labels.p_to_fake), this.labels.w_single = this.$cache.single.outerWidth(!1), this.labels.p_single_fake = this.labels.w_single / this.coords.w_rs * 100, this.labels.p_single_left = (this.labels.p_from_left + this.labels.p_to_left + this.labels.p_to_fake) / 2 - this.labels.p_single_fake / 2, this.labels.p_single_left = this.toFixed(this.labels.p_single_left)), this.labels.p_single_left = this.checkEdges(this.labels.p_single_left, this.labels.p_single_fake))
        }, updateScene: function () {
            this.raf_id &&
                (cancelAnimationFrame(this.raf_id), this.raf_id = null); clearTimeout(this.update_tm); this.update_tm = null; this.options && (this.drawHandles(), this.is_active ? this.raf_id = requestAnimationFrame(this.updateScene.bind(this)) : this.update_tm = setTimeout(this.updateScene.bind(this), 300))
        }, drawHandles: function () {
            this.coords.w_rs = this.$cache.rs.outerWidth(!1); if (this.coords.w_rs) {
                this.coords.w_rs !== this.coords.w_rs_old && (this.target = "base", this.is_resize = !0); if (this.coords.w_rs !== this.coords.w_rs_old || this.force_redraw) this.setMinMax(),
                    this.calc(!0), this.drawLabels(), this.options.grid && (this.calcGridMargin(), this.calcGridLabels()), this.force_redraw = !0, this.coords.w_rs_old = this.coords.w_rs, this.drawShadow(); if (this.coords.w_rs && (this.dragging || this.force_redraw || this.is_key)) {
                        if (this.old_from !== this.result.from || this.old_to !== this.result.to || this.force_redraw || this.is_key) {
                            this.drawLabels(); this.$cache.bar[0].style.left = this.coords.p_bar_x + "%"; this.$cache.bar[0].style.width = this.coords.p_bar_w + "%"; if ("single" === this.options.type) this.$cache.s_single[0].style.left =
                                this.coords.p_single_fake + "%", this.$cache.single[0].style.left = this.labels.p_single_left + "%", this.options.values.length ? this.$cache.input.prop("value", this.result.from_value) : this.$cache.input.prop("value", this.result.from), this.$cache.input.data("from", this.result.from); else {
                                this.$cache.s_from[0].style.left = this.coords.p_from_fake + "%"; this.$cache.s_to[0].style.left = this.coords.p_to_fake + "%"; if (this.old_from !== this.result.from || this.force_redraw) this.$cache.from[0].style.left = this.labels.p_from_left +
                                    "%"; if (this.old_to !== this.result.to || this.force_redraw) this.$cache.to[0].style.left = this.labels.p_to_left + "%"; this.$cache.single[0].style.left = this.labels.p_single_left + "%"; this.options.values.length ? this.$cache.input.prop("value", this.result.from_value + this.options.input_values_separator + this.result.to_value) : this.$cache.input.prop("value", this.result.from + this.options.input_values_separator + this.result.to); this.$cache.input.data("from", this.result.from); this.$cache.input.data("to", this.result.to)
                            } this.old_from ===
                                this.result.from && this.old_to === this.result.to || this.is_start || this.$cache.input.trigger("change"); this.old_from = this.result.from; this.old_to = this.result.to; this.is_resize || this.is_update || this.is_start || this.is_finish || this.callOnChange(); if (this.is_key || this.is_click) this.is_click = this.is_key = !1, this.callOnFinish(); this.is_finish = this.is_resize = this.is_update = !1
                        } this.force_redraw = this.is_click = this.is_key = this.is_start = !1
                    }
            }
        }, drawLabels: function () {
            if (this.options) {
                var a = this.options.values.length,
                    b = this.options.p_values, d; if (!this.options.hide_from_to) if ("single" === this.options.type) a = a ? this.decorate(b[this.result.from]) : this.decorate(this._prettify(this.result.from), this.result.from), this.$cache.single.html(a), this.calcLabels(), this.$cache.min[0].style.visibility = this.labels.p_single_left < this.labels.p_min + 1 ? "hidden" : "visible", this.$cache.max[0].style.visibility = this.labels.p_single_left + this.labels.p_single_fake > 100 - this.labels.p_max - 1 ? "hidden" : "visible"; else {
                        a ? (this.options.decorate_both ?
                            (a = this.decorate(b[this.result.from]), a += this.options.values_separator, a += this.decorate(b[this.result.to])) : a = this.decorate(b[this.result.from] + this.options.values_separator + b[this.result.to]), d = this.decorate(b[this.result.from]), b = this.decorate(b[this.result.to])) : (this.options.decorate_both ? (a = this.decorate(this._prettify(this.result.from), this.result.from), a += this.options.values_separator, a += this.decorate(this._prettify(this.result.to), this.result.to)) : a = this.decorate(this._prettify(this.result.from) +
                                this.options.values_separator + this._prettify(this.result.to), this.result.to), d = this.decorate(this._prettify(this.result.from), this.result.from), b = this.decorate(this._prettify(this.result.to), this.result.to)); this.$cache.single.html(a); this.$cache.from.html(d); this.$cache.to.html(b); this.calcLabels(); b = Math.min(this.labels.p_single_left, this.labels.p_from_left); a = this.labels.p_single_left + this.labels.p_single_fake; d = this.labels.p_to_left + this.labels.p_to_fake; var c = Math.max(a, d); this.labels.p_from_left +
                                    this.labels.p_from_fake >= this.labels.p_to_left ? (this.$cache.from[0].style.visibility = "hidden", this.$cache.to[0].style.visibility = "hidden", this.$cache.single[0].style.visibility = "visible", this.result.from === this.result.to ? ("from" === this.target ? this.$cache.from[0].style.visibility = "visible" : "to" === this.target ? this.$cache.to[0].style.visibility = "visible" : this.target || (this.$cache.from[0].style.visibility = "visible"), this.$cache.single[0].style.visibility = "hidden", c = d) : (this.$cache.from[0].style.visibility =
                                        "hidden", this.$cache.to[0].style.visibility = "hidden", this.$cache.single[0].style.visibility = "visible", c = Math.max(a, d))) : (this.$cache.from[0].style.visibility = "visible", this.$cache.to[0].style.visibility = "visible", this.$cache.single[0].style.visibility = "hidden"); this.$cache.min[0].style.visibility = b < this.labels.p_min + 1 ? "hidden" : "visible"; this.$cache.max[0].style.visibility = c > 100 - this.labels.p_max - 1 ? "hidden" : "visible"
                    }
            }
        }, drawShadow: function () {
            var a = this.options, b = this.$cache, d = "number" === typeof a.from_min &&
                !isNaN(a.from_min), c = "number" === typeof a.from_max && !isNaN(a.from_max), e = "number" === typeof a.to_min && !isNaN(a.to_min), f = "number" === typeof a.to_max && !isNaN(a.to_max); "single" === a.type ? a.from_shadow && (d || c) ? (d = this.convertToPercent(d ? a.from_min : a.min), c = this.convertToPercent(c ? a.from_max : a.max) - d, d = this.toFixed(d - this.coords.p_handle / 100 * d), c = this.toFixed(c - this.coords.p_handle / 100 * c), d += this.coords.p_handle / 2, b.shad_single[0].style.display = "block", b.shad_single[0].style.left = d + "%", b.shad_single[0].style.width =
                    c + "%") : b.shad_single[0].style.display = "none" : (a.from_shadow && (d || c) ? (d = this.convertToPercent(d ? a.from_min : a.min), c = this.convertToPercent(c ? a.from_max : a.max) - d, d = this.toFixed(d - this.coords.p_handle / 100 * d), c = this.toFixed(c - this.coords.p_handle / 100 * c), d += this.coords.p_handle / 2, b.shad_from[0].style.display = "block", b.shad_from[0].style.left = d + "%", b.shad_from[0].style.width = c + "%") : b.shad_from[0].style.display = "none", a.to_shadow && (e || f) ? (e = this.convertToPercent(e ? a.to_min : a.min), a = this.convertToPercent(f ?
                        a.to_max : a.max) - e, e = this.toFixed(e - this.coords.p_handle / 100 * e), a = this.toFixed(a - this.coords.p_handle / 100 * a), e += this.coords.p_handle / 2, b.shad_to[0].style.display = "block", b.shad_to[0].style.left = e + "%", b.shad_to[0].style.width = a + "%") : b.shad_to[0].style.display = "none")
        }, callOnStart: function () { if (this.options.onStart && "function" === typeof this.options.onStart) this.options.onStart(this.result) }, callOnChange: function () { if (this.options.onChange && "function" === typeof this.options.onChange) this.options.onChange(this.result) },
        callOnFinish: function () { if (this.options.onFinish && "function" === typeof this.options.onFinish) this.options.onFinish(this.result) }, callOnUpdate: function () { if (this.options.onUpdate && "function" === typeof this.options.onUpdate) this.options.onUpdate(this.result) }, toggleInput: function () { this.$cache.input.toggleClass("irs-hidden-input") }, convertToPercent: function (a, b) { var d = this.options.max - this.options.min; return d ? this.toFixed((b ? a : a - this.options.min) / (d / 100)) : (this.no_diapason = !0, 0) }, convertToValue: function (a) {
            var b =
                this.options.min, d = this.options.max, c = b.toString().split(".")[1], e = d.toString().split(".")[1], f, l, g = 0, k = 0; if (0 === a) return this.options.min; if (100 === a) return this.options.max; c && (g = f = c.length); e && (g = l = e.length); f && l && (g = f >= l ? f : l); 0 > b && (k = Math.abs(b), b = +(b + k).toFixed(g), d = +(d + k).toFixed(g)); a = (d - b) / 100 * a + b; (b = this.options.step.toString().split(".")[1]) ? a = +a.toFixed(b.length) : (a /= this.options.step, a *= this.options.step, a = +a.toFixed(0)); k && (a -= k); k = b ? +a.toFixed(b.length) : this.toFixed(a); k < this.options.min ?
                    k = this.options.min : k > this.options.max && (k = this.options.max); return k
        }, calcWithStep: function (a) { var b = Math.round(a / this.coords.p_step) * this.coords.p_step; 100 < b && (b = 100); 100 === a && (b = 100); return this.toFixed(b) }, checkMinInterval: function (a, b, d) { var c = this.options; if (!c.min_interval) return a; a = this.convertToValue(a); b = this.convertToValue(b); "from" === d ? b - a < c.min_interval && (a = b - c.min_interval) : a - b < c.min_interval && (a = b + c.min_interval); return this.convertToPercent(a) }, checkMaxInterval: function (a, b, d) {
            var c =
                this.options; if (!c.max_interval) return a; a = this.convertToValue(a); b = this.convertToValue(b); "from" === d ? b - a > c.max_interval && (a = b - c.max_interval) : a - b > c.max_interval && (a = b + c.max_interval); return this.convertToPercent(a)
        }, checkDiapason: function (a, b, d) { a = this.convertToValue(a); var c = this.options; "number" !== typeof b && (b = c.min); "number" !== typeof d && (d = c.max); a < b && (a = b); a > d && (a = d); return this.convertToPercent(a) }, toFixed: function (a) { a = a.toFixed(9); return +a }, _prettify: function (a) {
            return this.options.prettify_enabled ?
                this.options.prettify && "function" === typeof this.options.prettify ? this.options.prettify(a) : this.prettify(a) : a
        }, prettify: function (a) { return a.toString().replace(/(\d{1,3}(?=(?:\d\d\d)+(?!\d)))/g, "$1" + this.options.prettify_separator) }, checkEdges: function (a, b) { if (!this.options.force_edges) return this.toFixed(a); 0 > a ? a = 0 : a > 100 - b && (a = 100 - b); return this.toFixed(a) }, validate: function () {
            var a = this.options, b = this.result, d = a.values, c = d.length, e, f; "string" === typeof a.min && (a.min = +a.min); "string" === typeof a.max &&
                (a.max = +a.max); "string" === typeof a.from && (a.from = +a.from); "string" === typeof a.to && (a.to = +a.to); "string" === typeof a.step && (a.step = +a.step); "string" === typeof a.from_min && (a.from_min = +a.from_min); "string" === typeof a.from_max && (a.from_max = +a.from_max); "string" === typeof a.to_min && (a.to_min = +a.to_min); "string" === typeof a.to_max && (a.to_max = +a.to_max); "string" === typeof a.keyboard_step && (a.keyboard_step = +a.keyboard_step); "string" === typeof a.grid_num && (a.grid_num = +a.grid_num); a.max < a.min && (a.max = a.min); if (c) for (a.p_values =
                    [], a.min = 0, a.max = c - 1, a.step = 1, a.grid_num = a.max, a.grid_snap = !0, f = 0; f < c; f++) e = +d[f], isNaN(e) ? e = d[f] : (d[f] = e, e = this._prettify(e)), a.p_values.push(e); if ("number" !== typeof a.from || isNaN(a.from)) a.from = a.min; if ("number" !== typeof a.to || isNaN(a.from)) a.to = a.max; if ("single" === a.type) a.from < a.min && (a.from = a.min), a.from > a.max && (a.from = a.max); else { if (a.from < a.min || a.from > a.max) a.from = a.min; if (a.to > a.max || a.to < a.min) a.to = a.max; a.from > a.to && (a.from = a.to) } if ("number" !== typeof a.step || isNaN(a.step) || !a.step || 0 > a.step) a.step =
                        1; if ("number" !== typeof a.keyboard_step || isNaN(a.keyboard_step) || !a.keyboard_step || 0 > a.keyboard_step) a.keyboard_step = 5; "number" === typeof a.from_min && a.from < a.from_min && (a.from = a.from_min); "number" === typeof a.from_max && a.from > a.from_max && (a.from = a.from_max); "number" === typeof a.to_min && a.to < a.to_min && (a.to = a.to_min); "number" === typeof a.to_max && a.from > a.to_max && (a.to = a.to_max); if (b) {
                            b.min !== a.min && (b.min = a.min); b.max !== a.max && (b.max = a.max); if (b.from < b.min || b.from > b.max) b.from = a.from; if (b.to < b.min || b.to >
                                b.max) b.to = a.to
                        } if ("number" !== typeof a.min_interval || isNaN(a.min_interval) || !a.min_interval || 0 > a.min_interval) a.min_interval = 0; if ("number" !== typeof a.max_interval || isNaN(a.max_interval) || !a.max_interval || 0 > a.max_interval) a.max_interval = 0; a.min_interval && a.min_interval > a.max - a.min && (a.min_interval = a.max - a.min); a.max_interval && a.max_interval > a.max - a.min && (a.max_interval = a.max - a.min)
        }, decorate: function (a, b) {
            var d = "", c = this.options; c.prefix && (d += c.prefix); d += a; c.max_postfix && (c.values.length && a === c.p_values[c.max] ?
                (d += c.max_postfix, c.postfix && (d += " ")) : b === c.max && (d += c.max_postfix, c.postfix && (d += " "))); c.postfix && (d += c.postfix); return d
        }, updateFrom: function () { this.result.from = this.options.from; this.result.from_percent = this.convertToPercent(this.result.from); this.options.values && (this.result.from_value = this.options.values[this.result.from]) }, updateTo: function () { this.result.to = this.options.to; this.result.to_percent = this.convertToPercent(this.result.to); this.options.values && (this.result.to_value = this.options.values[this.result.to]) },
        updateResult: function () { this.result.min = this.options.min; this.result.max = this.options.max; this.updateFrom(); this.updateTo() }, appendGrid: function () {
            if (this.options.grid) {
                var a = this.options, b, d; b = a.max - a.min; var c = a.grid_num, e = 0, f = 0, g = 4, h, k, m = 0, n = ""; this.calcGridMargin(); a.grid_snap ? (c = b / a.step, e = this.toFixed(a.step / (b / 100))) : e = this.toFixed(100 / c); 4 < c && (g = 3); 7 < c && (g = 2); 14 < c && (g = 1); 28 < c && (g = 0); for (b = 0; b < c + 1; b++) {
                    h = g; f = this.toFixed(e * b); 100 < f && (f = 100, h -= 2, 0 > h && (h = 0)); this.coords.big[b] = f; k = (f - e * (b - 1)) /
                        (h + 1); for (d = 1; d <= h && 0 !== f; d++) m = this.toFixed(f - k * d), n += '<span class="irs-grid-pol small" style="left: ' + m + '%"></span>'; n += '<span class="irs-grid-pol" style="left: ' + f + '%"></span>'; m = this.convertToValue(f); m = a.values.length ? a.p_values[m] : this._prettify(m); n += '<span class="irs-grid-text js-grid-text-' + b + '" style="left: ' + f + '%">' + m + "</span>"
                } this.coords.big_num = Math.ceil(c + 1); this.$cache.cont.addClass("irs-with-grid"); this.$cache.grid.html(n); this.cacheGridLabels()
            }
        }, cacheGridLabels: function () {
            var a,
                b, d = this.coords.big_num; for (b = 0; b < d; b++) a = this.$cache.grid.find(".js-grid-text-" + b), this.$cache.grid_labels.push(a); this.calcGridLabels()
        }, calcGridLabels: function () {
            var a, b; b = []; var d = [], c = this.coords.big_num; for (a = 0; a < c; a++) this.coords.big_w[a] = this.$cache.grid_labels[a].outerWidth(!1), this.coords.big_p[a] = this.toFixed(this.coords.big_w[a] / this.coords.w_rs * 100), this.coords.big_x[a] = this.toFixed(this.coords.big_p[a] / 2), b[a] = this.toFixed(this.coords.big[a] - this.coords.big_x[a]), d[a] = this.toFixed(b[a] +
                this.coords.big_p[a]); this.options.force_edges && (b[0] < -this.coords.grid_gap && (b[0] = -this.coords.grid_gap, d[0] = this.toFixed(b[0] + this.coords.big_p[0]), this.coords.big_x[0] = this.coords.grid_gap), d[c - 1] > 100 + this.coords.grid_gap && (d[c - 1] = 100 + this.coords.grid_gap, b[c - 1] = this.toFixed(d[c - 1] - this.coords.big_p[c - 1]), this.coords.big_x[c - 1] = this.toFixed(this.coords.big_p[c - 1] - this.coords.grid_gap))); this.calcGridCollision(2, b, d); this.calcGridCollision(4, b, d); for (a = 0; a < c; a++) b = this.$cache.grid_labels[a][0],
                    b.style.marginLeft = -this.coords.big_x[a] + "%"
        }, calcGridCollision: function (a, b, d) { var c, e, f, g = this.coords.big_num; for (c = 0; c < g; c += a) { e = c + a / 2; if (e >= g) break; f = this.$cache.grid_labels[e][0]; f.style.visibility = d[c] <= b[e] ? "visible" : "hidden" } }, calcGridMargin: function () {
            this.options.grid_margin && (this.coords.w_rs = this.$cache.rs.outerWidth(!1), this.coords.w_rs && (this.coords.w_handle = "single" === this.options.type ? this.$cache.s_single.outerWidth(!1) : this.$cache.s_from.outerWidth(!1), this.coords.p_handle = this.toFixed(this.coords.w_handle /
                this.coords.w_rs * 100), this.coords.grid_gap = this.toFixed(this.coords.p_handle / 2 - .1), this.$cache.grid[0].style.width = this.toFixed(100 - this.coords.p_handle) + "%", this.$cache.grid[0].style.left = this.coords.grid_gap + "%"))
        }, update: function (a) { this.input && (this.is_update = !0, this.options.from = this.result.from, this.options.to = this.result.to, this.options = g.extend(this.options, a), this.validate(), this.updateResult(a), this.toggleInput(), this.remove(), this.init(!0)) }, reset: function () {
            this.input && (this.updateResult(),
                this.update())
        }, destroy: function () { this.input && (this.toggleInput(), this.$cache.input.prop("readonly", !1), g.data(this.input, "ionRangeSlider", null), this.remove(), this.options = this.input = null) }
    }; g.fn.ionRangeSlider = function (a) { return this.each(function () { g.data(this, "ionRangeSlider") || g.data(this, "ionRangeSlider", new r(this, a, u++)) }) }; (function () {
        for (var a = 0, b = ["ms", "moz", "webkit", "o"], d = 0; d < b.length && !h.requestAnimationFrame; ++d) h.requestAnimationFrame = h[b[d] + "RequestAnimationFrame"], h.cancelAnimationFrame =
            h[b[d] + "CancelAnimationFrame"] || h[b[d] + "CancelRequestAnimationFrame"]; h.requestAnimationFrame || (h.requestAnimationFrame = function (b, d) { var f = (new Date).getTime(), g = Math.max(0, 16 - (f - a)), p = h.setTimeout(function () { b(f + g) }, g); a = f + g; return p }); h.cancelAnimationFrame || (h.cancelAnimationFrame = function (a) { clearTimeout(a) })
    })()
});

/*
waitMe - 1.17 [29.07.16]
Author: vadimsva
Github: https://github.com/vadimsva/waitMe
*/
(function ($) {
    $.fn.waitMe = function (method) {
        return this.each(function () {

            var elem = $(this),
                elemClass = 'waitMe',
                waitMe_text,
                effectObj,
                effectElemCount,
                createSubElem = false,
                specificAttr = 'background-color',
                addStyle = '',
                effectElemHTML = '',
                waitMeObj,
                _options,
                currentID;

            var methods = {
                init: function () {
                    var _defaults = {
                        effect: 'bounce',
                        text: '',
                        bg: 'rgba(255,255,255,0.7)',
                        color: '#000',
                        maxSize: '',
                        textPos: 'vertical',
                        source: '',
                        onClose: function () { }
                    };
                    _options = $.extend(_defaults, method);

                    currentID = new Date().getMilliseconds();
                    waitMeObj = $('<div class="' + elemClass + '" data-waitme_id="' + currentID + '"></div>');

                    switch (_options.effect) {
                        case 'none':
                            effectElemCount = 0;
                            break;
                        case 'bounce':
                            effectElemCount = 3;
                            break;
                        case 'rotateplane':
                            effectElemCount = 1;
                            break;
                        case 'stretch':
                            effectElemCount = 5;
                            break;
                        case 'orbit':
                            effectElemCount = 2;
                            break;
                        case 'roundBounce':
                            effectElemCount = 12;
                            break;
                        case 'win8':
                            effectElemCount = 5;
                            createSubElem = true;
                            break;
                        case 'win8_linear':
                            effectElemCount = 5;
                            createSubElem = true;
                            break;
                        case 'ios':
                            effectElemCount = 12;
                            break;
                        case 'facebook':
                            effectElemCount = 3;
                            break;
                        case 'rotation':
                            effectElemCount = 1;
                            specificAttr = 'border-color';
                            break;
                        case 'timer':
                            effectElemCount = 2;
                            if ($.isArray(_options.color)) {
                                var color = _options.color[0];
                            } else {
                                var color = _options.color;
                            }
                            addStyle = 'border-color:' + color;
                            break;
                        case 'pulse':
                            effectElemCount = 1;
                            specificAttr = 'border-color';
                            break;
                        case 'progressBar':
                            effectElemCount = 1;
                            break;
                        case 'bouncePulse':
                            effectElemCount = 3;
                            break;
                        case 'img':
                            effectElemCount = 1;
                            break;
                    }

                    if (addStyle !== '') {
                        addStyle += ';';
                    }

                    if (effectElemCount > 0) {
                        if (_options.effect === 'img') {
                            effectElemHTML = '<img src="' + _options.source + '">';
                        } else {
                            for (var i = 1; i <= effectElemCount; ++i) {
                                if ($.isArray(_options.color)) {
                                    var color = _options.color[i];
                                    if (color == undefined) {
                                        color = '#000';
                                    }
                                } else {
                                    var color = _options.color;
                                }
                                if (createSubElem) {
                                    effectElemHTML += '<div class="' + elemClass + '_progress_elem' + i + '"><div style="' + specificAttr + ':' + color + '"></div></div>';
                                } else {
                                    effectElemHTML += '<div class="' + elemClass + '_progress_elem' + i + '" style="' + specificAttr + ':' + color + '"></div>';
                                }
                            }
                        }
                        effectObj = $('<div class="' + elemClass + '_progress ' + _options.effect + '" style="' + addStyle + '">' + effectElemHTML + '</div>');
                    }

                    if (_options.text && _options.maxSize === '' || _options.textPos == 'horizontal') {
                        if ($.isArray(_options.color)) {
                            var color = _options.color[0];
                        } else {
                            var color = _options.color;
                        }
                        waitMe_text = $('<div class="' + elemClass + '_text" style="color:' + color + '">' + _options.text + '</div>');
                    }
                    var elemObj = elem.find('> .' + elemClass);

                    if (elemObj) {
                        elemObj.remove();
                    }
                    var waitMeDivObj = $('<div class="' + elemClass + '_content ' + _options.textPos + '"></div>');
                    waitMeDivObj.append(effectObj, waitMe_text);
                    waitMeObj.append(waitMeDivObj);
                    if (elem[0].tagName == 'HTML') {
                        elem = $('body');
                    }
                    elem.addClass(elemClass + '_container').attr('data-waitme_id', currentID).append(waitMeObj);
                    elemObj = elem.find('> .' + elemClass);
                    var elemContentObj = elem.find('.' + elemClass + '_content');
                    elemObj.css({ background: _options.bg });


                    if (_options.maxSize !== '') {
                        var elemH = effectObj.outerHeight();
                        var elemW = effectObj.outerWidth();
                        var elemMax = elemH;
                        if (_options.effect === 'img') {
                            effectObj.css({ height: _options.maxSize + 'px' });
                            effectObj.find('>img').css({ maxHeight: '100%' });
                            elemContentObj.css({ marginTop: -elemContentObj.outerHeight() / 2 + 'px' });
                        } else {
                            if (_options.maxSize < elemMax) {
                                if (_options.effect == 'stretch') {
                                    effectObj.css({ height: _options.maxSize + 'px', width: _options.maxSize + 'px' });
                                    effectObj.find('> div').css({ margin: '0 5%' });
                                } else {
                                    effectObj.css({ zoom: _options.maxSize / elemMax - 0.2 });
                                }

                            }
                        }
                    }
                    elemContentObj.css({ marginTop: -elemContentObj.outerHeight() / 2 + 'px' });

                    function setElTop(getTop) {
                        elemContentObj.css({ top: 'auto', transform: 'translateY(' + getTop + 'px) translateZ(0)' });
                    }
                    if (elem.outerHeight() > $(window).height()) {
                        var sTop = $(window).scrollTop(),
                            elH = elemContentObj.outerHeight(),
                            elTop = elem.offset().top,
                            cH = elem.outerHeight(),
                            getTop = sTop - elTop + $(window).height() / 2;
                        if (getTop < 0) {
                            getTop = Math.abs(getTop);
                        }
                        if (getTop - elH >= 0 && getTop + elH <= cH) {
                            if (elTop - sTop > $(window).height() / 2) {
                                getTop = elH;
                            }
                            setElTop(getTop);
                        } else {
                            if (sTop > elTop + cH - elH) {
                                getTop = sTop - elTop - elH;
                            } else {
                                getTop = sTop - elTop + elH;
                            }
                            setElTop(getTop);
                        }
                        $(document).scroll(function () {
                            var sTop = $(window).scrollTop(),
                                getTop = sTop - elTop + $(window).height() / 2;
                            if (getTop - elH >= 0 && getTop + elH <= cH) {
                                setElTop(getTop);
                            }
                        });
                    }

                    elemObj.on('destroyed', function () {
                        if (_options.onClose && $.isFunction(_options.onClose)) {
                            _options.onClose();
                        }
                        elemObj.trigger('close');
                    });

                    $.event.special.destroyed = {
                        remove: function (o) {
                            if (o.handler) {
                                o.handler();
                            }
                        }
                    };

                    return elemObj;
                },
                hide: function () {
                    waitMeClose();
                }
            };

            function waitMeClose() {
                var currentID = elem.attr('data-waitme_id');
                elem.removeClass(elemClass + '_container').removeAttr('data-waitme_id');
                elem.find('.' + elemClass + '[data-waitme_id="' + currentID + '"]').remove();
            }

            if (methods[method]) {
                return methods[method].apply(this, Array.prototype.slice.call(arguments, 1));
            } else if (typeof method === 'object' || !method) {
                return methods.init.apply(this, arguments);
            }

        });

    };
    $(window).on('load', function () {
        $('body.waitMe_body').addClass('hideMe');
        setTimeout(function () {
            $('body.waitMe_body').find('.waitMe_container:not([data-waitme_id])').remove();
            $('body.waitMe_body').removeClass('waitMe_body hideMe');
        }, 200);
    });
})(jQuery);