/**
 * Project: Bootstrap Hover Dropdown
 * Author: Cameron Spear
 * Contributors: Mattia Larentis
 *
 * Dependencies: Bootstrap's Dropdown plugin, jQuery
 *
 * A simple plugin to enable Bootstrap dropdowns to active on hover and provide a nice user experience.
 *
 * License: MIT
 *
 * http://cameronspear.com/blog/bootstrap-dropdown-on-hover-plugin/
 */(function (e, t, n) { var r = e(); e.fn.dropdownHover = function (n) { if ("ontouchstart" in document) return this; r = r.add(this.parent()); return this.each(function () { var i = e(this), s = i.parent(), o = { delay: 500, instantlyCloseOthers: !0 }, u = { delay: e(this).data("delay"), instantlyCloseOthers: e(this).data("close-others") }, a = "show.bs.dropdown", f = "hide.bs.dropdown", l = e.extend(!0, {}, o, n, u), c; s.hover(function (e) { if (!s.hasClass("open") && !i.is(e.target)) return !0; l.instantlyCloseOthers === !0 && r.removeClass("open"); t.clearTimeout(c); s.addClass("open"); i.trigger(a) }, function () { c = t.setTimeout(function () { s.removeClass("open"); i.trigger(f) }, l.delay) }); i.hover(function () { l.instantlyCloseOthers === !0 && r.removeClass("open"); t.clearTimeout(c); s.addClass("open"); i.trigger(a) }); s.find(".dropdown-submenu").each(function () { var n = e(this), r; n.hover(function () { t.clearTimeout(r); n.children(".dropdown-menu").show(); n.siblings().children(".dropdown-menu").hide() }, function () { var e = n.children(".dropdown-menu"); r = t.setTimeout(function () { e.hide() }, l.delay) }) }) }) }; e(document).ready(function () { e('[data-hover="dropdown"]').dropdownHover() }) })(jQuery, this);
/* --- Start ZhengLei ---*/

function onImgError(source) {
    source.src = "https://images.pricemestatic.com/images/no_image_available.gif";
    source.onerror = "";
    return true;
}

var isObject = function (p) { return "object" == typeof (p) }
var $$ = function (p, doc) { return isObject(p) ? p : (doc || document).getElementById(p); }

var addRule = function (p, k, asNew) {
    var style = document.createElement("STYLE");
    document.getElementsByTagName("HEAD")[0].appendChild(style);
    var styleSheet = style.styleSheet || style.sheet;

    if (styleSheet.addRule) {
        var rs = p.split(',');
        for (var i = 0; i < rs.length; i++)
            styleSheet.addRule(rs[i], k);
    } else if (styleSheet.insertRule) {
        styleSheet.insertRule(p + "{" + k + "}", styleSheet.cssRules.length);
    }
}

var doFunction = function (fun) {
    var args = [], i;
    for (i = 1; i < arguments.length; i++) {
        args.push(arguments[i]);
    }
    return function () {
        fun.apply(null, args);
        delete args;
    };
}

var getElementsByClassName = function (className, obj) {
    obj = $$(obj) || document;
    var objs = obj.all || obj.getElementsByTagName("*");
    var o, i, arr = [];
    var classNames = "";
    className = "," + className + ",";
    for (i = 0; o = objs[i]; i++) {
        classNames = "," + o.className.split(/\s+/).join(",") + ",";
        if (classNames.indexOf(className) >= 0) {
            arr.push(o);
        }
    }
    delete objs;
    return arr;
}

function redirectURL(a) {
    window.location = a
};

function search(textBoxKeywordsID) {
    var searchTextBox = document.getElementById(textBoxKeywordsID);
    if (searchTextBox.value != "") {
        var url = "/search.aspx?q=" + encodeURIComponent(searchTextBox.value);
        redirectURL(url);
    }
    else {
        searchTextBox.focus();
    }

    return false;
}

function myFocus(sel, start, end) {
    if (sel.setSelectionRange) {
        sel.focus();
        sel.setSelectionRange(start, end);
    }
    else if (sel.createTextRange) {
        var range = sel.createTextRange();
        range.collapse(true);
        range.moveEnd('character', end);
        range.moveStart('character', start);
        range.select();
    }
}
function setFocus(sel) {
    if (sel.value == null || sel.value == "")
        myFocus(sel, 0, 0);
    else {
        length = sel.value.length;
        myFocus(sel, length, length);
    }
}

var myreg = /^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$/;
//var myreg = /^([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+@([a-zA-Z0-9]+[_|\_|\.]?)*[a-zA-Z0-9]+\.[a-zA-Z]{2,3}$/; 

var iContactHelper = {};
(function (_$$) {
    var $c = function (form, name, value) {
        var n = document.createElement("INPUT");
        n.type = "HIDDEN";
        n.name = name;
        n.value = value;
        form.appendChild(n);
    }

    _$$.SignUpFooter = function (homeURL, listid, specialid_value, controllid, formid, url) {

        var email = $$(controllid);
        if (email.value.trim() == "") {
            email.focus();
            $('#NewsletterEmailMsgDiv').css('display', 'block');
            $('#NewsletterEmailMsgDiv .alert label').html(emptyEMsg);

            $('#HomeNewsletterEmailMsgDiv').css('display', 'block');
            $('#HomeNewsletterEmailMsgDiv.alert label').html(emptyEMsg);
            return false;
        }

        if (!myreg.test(email.value.trim())) {
            $('#NewsletterEmailMsgDiv').css('display', 'block');
            $('#NewsletterEmailMsgDiv .alert label').html(invalidEMsg);

            $('#HomeNewsletterEmailMsgDiv').css('display', 'block');
            $('#HomeNewsletterEmailMsgDiv.alert label').html(invalidEMsg);
            return false;
        }

        var form = document.createElement("FORM");
        form.action = 'https://app.icontact.com/icp/signup.php';
        form.method = "POST";
        document.body.appendChild(form);

        $c(form, "redirect", homeURL + "/ThankYou.aspx?url=" + url);
        $c(form, "errorredirect", 'http://www.icontact.com/www/signup/error.html');
        $c(form, "fields_email", email.value);
        $c(form, "listid", listid);
        $c(form, 'specialid:' + listid, specialid_value);
        $c(form, 'clientid', '502867');
        $c(form, 'formid', formid);
        $c(form, 'reallistid', '1');
        $c(form, 'doubleopt', '1');

        $c(form, 'fields_12:30pm', '1');
        form.submit();
    }
    _$$.SignUp = function (homeURL, listid, specialid_value, formid) {

        var email = $$("iContact_fields_email");
        if (email.value.trim() == "") {
            email.focus();
            alert("Please input your email address.");
            return false;
        }

        var firstName = $$("iContact_fields_name");
        if (firstName.value.trim() == "") {
            firstName.focus();
            alert("First Name required.");
            return false;
        }

        var form = document.createElement("FORM");
        form.action = 'https://app.icontact.com/icp/signup.php';
        form.method = "POST";
        document.body.appendChild(form);

        $c(form, "redirect", homeURL + "/ThankYou.aspx");
        $c(form, "errorredirect", 'http://www.icontact.com/www/signup/error.html');
        $c(form, "fields_email", email.value);
        $c(form, "fields_fname", firstName.value);
        $c(form, "listid", listid);
        $c(form, 'specialid:' + listid, specialid_value);
        $c(form, 'clientid', '502867');
        $c(form, 'formid', formid);
        $c(form, 'reallistid', '1');
        $c(form, 'doubleopt', '1');

        $c(form, 'fields_12:30pm', '1');
        form.submit();
    }
    _$$.PriceAlertSignUp = function (homeURL, listid, specialid_value, formid, email) {

        var form = document.createElement("FORM");
        form.action = 'https://app.icontact.com/icp/signup.php';
        form.method = "POST";
        document.body.appendChild(form);

        $c(form, "redirect", homeURL + "/ThankYou.aspx");
        $c(form, "errorredirect", 'http://www.icontact.com/www/signup/error.html');
        $c(form, "fields_email", email);
        $c(form, "listid", listid);
        $c(form, 'specialid:' + listid, specialid_value);
        $c(form, 'clientid', '502867');
        $c(form, 'formid', formid);
        $c(form, 'reallistid', '1');
        $c(form, 'doubleopt', '1');

        $c(form, 'fields_12:30pm', '1');
        form.submit();
    }
    _$$.RegisterSubscribe = function (homeURL, listid, specialid_value, formid, email) {

        var form = document.createElement("FORM");
        form.action = 'https://app.icontact.com/icp/signup.php';
        form.method = "POST";
        document.body.appendChild(form);

        $c(form, "redirect", homeURL + "/Register.aspx?code=1");
        $c(form, "errorredirect", 'http://www.icontact.com/www/signup/error.html');
        $c(form, "fields_email", email);
        $c(form, "listid", listid);
        $c(form, 'specialid:' + listid, specialid_value);
        $c(form, 'clientid', '502867');
        $c(form, 'formid', formid);
        $c(form, 'reallistid', '1');
        $c(form, 'doubleopt', '1');

        $c(form, 'fields_12:30pm', '1');
        form.submit();
    }
})(iContactHelper);

var SuggestHelper = {};
(function ($$) {

    var ajaxObj = null;
    var locationUrl = null;
    var storedSuggest = {
        keyword: null,
        suggest: null
    };
    var directionKey = { 38: "up", 40: "down", 13: "enter", 39: "right", 37: "left" };
    var focusIdx = 0;
    var focusedItem;
    var mouseInSuggest = false;

    //html object
    var input = null;
    var searchSuggest = null;
    var ulRight = null;
    var ulLeft = null;

    var _inputID;
    var _searchSuggestShowAreaID;
    var _suggestULLeft;
    var _suggestULRight;

    $$.setKeyWorks = function (kw) {
        input.value = kw;
    }

    $$.reInit = function () {
        SuggestHelper.init(_inputID, _searchSuggestShowAreaID, _suggestULLeft, _suggestULRight);
    }

    $$.init = function (inputID, searchSuggestShowAreaID, suggestULLeft, suggestULRight) {

        locationUrl = window.location.href;
        if (locationUrl.lastIndexOf('#') > 0)
        {
            locationUrl = locationUrl.substr(0, locationUrl.lastIndexOf('#'));
        }

        _inputID = inputID;
        _searchSuggestShowAreaID = searchSuggestShowAreaID;
        _suggestULLeft = suggestULLeft;
        _suggestULRight = suggestULRight;

        input = document.getElementById(inputID);
        searchSuggest = document.getElementById(searchSuggestShowAreaID);
        ulLeft = document.getElementById(suggestULLeft);
        ulRight = document.getElementById(suggestULRight);

        $('#suggest-div .glyphicon-remove-sign').click(function() {
            searchSuggest.style.display = "none";
        });
        //input.onblur = blur;
        input.onkeydown = keydown;
        input.onkeyup = Get;
        input.setAttribute("autocomplete", "off");

        ulLeft.onmouseover = mouseover;
        ulLeft.onmouseout = mouseout;
        //ulLeft.onclick = click;

        ulRight.onmouseover = mouseover;
        ulRight.onmouseout = mouseout;
        //ulRight.onclick = click;
    }

    $$.popularSearch = function (kw) {
        kw = decodeURIComponent(kw);
        input.value = kw;
        Search(kw);
    }

    var Get = function (evt) {

        evt = window.event || evt;
        if (evt.keyCode in directionKey) return;

        var keyword = '';
        if (!String.prototype.trim) {
            keyword = input.value.replace(/^\s+|\s+$/g, '');
        } else {
            keyword = input.value.trim();
        }
        Search(keyword);

        if (history.state == null) {
            if (keyword.length >= 1) {
                history.pushState(keyword, "", "#" + keyword);
            }
        } else {
            if (keyword.length >= 1) {
                history.replaceState(keyword, "", "#" + keyword);
            } else {
                history.replaceState(keyword, "", locationUrl);
            }
        }
    }

    var Search = function (keyword) {
        if (keyword.length < 1) {
            searchSuggest.style.display = "none";
            return;
        }

        keyword = encodeURIComponent(keyword);

        try { ajaxObj.abort(); } catch (e) { }

        ajaxObj = $.ajax({
            type: 'get',
            url: '/PopularSearch.aspx?q=' + keyword + '&cache=' + generateMixed(5),
            beforeSend: function () {
            },
            success: function (data, textStatu) {
                if (textStatu == 'success') {
                    eval(data);
                }
            }
        });
    }

    $$.BuildSuggest = function (keyword, obj) {
        focusIdx = 0;
        //storedSuggest.keyword = keyword;
        storedSuggest.suggest = obj.p;

        if (obj.p.length == 0 && obj.c.length == 0 && obj.b.length == 0 && obj.r.length == 0 && obj.bac.length == 0) {
            searchSuggest.style.display = "none";
            return;
        }
        searchSuggest.style.display = "";
        ulLeft.innerHTML = "";
        ulRight.innerHTML = "";
        var reg = new RegExp("(" + keyword.replace('+', '\\+').toLowerCase().split(/\s+/).join("|") + ")", "ig");

        //
        if (obj.p.length > 0) {
            var sep = document.createElement("LI");
            sep.className = "popularHead";
            sep.innerHTML = "<span class='glyphicon glyphicon-tag'></span> Products";
            ulLeft.appendChild(sep);
        }
        var rCount = 0;
        for (var i = 0; i < obj.p.length; i++) {
            if (rCount > 11) break;
            var li = document.createElement("LI");
            li.setAttribute("idx", i + 1);
            li.className = "entity";
            ulLeft.appendChild(li);
            if (obj.p[i][1].indexOf('ResponseRedirect') > 0) {
                li.innerHTML = "<div><a href='" + obj.p[i][1].replace(/&amp;/g, "&") + "' target='_blank' rel='nofollow'>"
                                    + "<img class='pImage' src='" + obj.p[i][5] + "' /><span class='aaaa pName'>" + obj.p[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>")
                                    + " <span class='countSpan' >(" + obj.p[i][2].replace("&amp;", "&") + ")</span><span class=\"glyphicon glyphicon-share countSpan\" style=\"font-size: 12px;\"></span></span><span class=\"popularSearchPrice\">" + obj.p[i][4] + "</span></a></div>";
            }
            else {
                li.innerHTML = "<div><img class='pImage' src='" + obj.p[i][5] + "' /><span class='aaaa pName'>" + obj.p[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>")
                                    + " <span class='countSpan' >(" + obj.p[i][2].replace("&amp;", "&") + ")</span></span><span class=\"popularSearchPrice\">" + obj.p[i][4] + "</span></div>";

                li.setAttribute("href", obj.p[i][1].replace("&amp;", "&"));
                li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.p[i][3] + "'\");document.location.href='" + obj.p[i][1] + "';");
            }
            //li.title = obj.p[i][0].replace("&amp;", "&").replace("&quote;", "\"");
            rCount++;
        }

        //
        //
        //
        if (obj.c.length > 0) {
            sep = document.createElement("LI");
            sep.className = "popularHead";
            sep.innerHTML = "<span class='glyphicon glyphicon-bookmark'></span> Categories";
            ulRight.appendChild(sep);
        }

        rCount = 0;
        for (var i = 0; i < obj.c.length; i++) {
            if (rCount > 4) break;
            var li = document.createElement("LI");
            li.setAttribute("idx", i + 1);
            li.className = "entity";
            ulRight.appendChild(li);
            li.innerHTML = "<div><span class='aaaa'>" + obj.c[i][0].replace("&amp;", "&").replace("&lt;", "<").replace("&gt;", ">").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + "</span></div>";
            li.setAttribute("href", obj.c[i][1].replace("&amp;", "&"));
            li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.c[i][3] + "'\");document.location.href='" + obj.c[i][1] + "';");
            //li.title = obj.c[i][2];
            rCount++
        }

        //
        if (obj.bac.length > 0 || obj.b.length > 0) {
            var sep = document.createElement("LI");
            sep.className = "popularHead";
            sep.innerHTML = "<span class='glyphicon glyphicon-filter'></span> Filtered categories";
            ulRight.appendChild(sep);
        }
        //        else{
        //            var li = document.createElement("LI");
        //            li.setAttribute("idx", 0);
        //            li.className = "entity";
        //            ulRight.appendChild(li);
        //            li.innerHTML = "No result";
        //        
        //        }
        
        for (var i = 0; i < obj.b.length; i++) {
            if (rCount > 7) break;
            var li = document.createElement("LI");
            li.setAttribute("idx", i + 1);
            li.className = "entity";
            ulRight.appendChild(li);
            li.innerHTML = "<div><span>" + obj.b[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + " <span class='countSpan'>(Brand)</span>" + "</span></div>";
            li.setAttribute("href", obj.b[i][1].replace("&amp;", "&"));
            li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.b[i][3] + "'\");document.location.href='" + obj.b[i][1] + "';");
            //li.title = obj.b[i][2].replace("&amp;", "&").replace("&quote;", "\"");
            rCount++;
        }

        for (var i = 0; i < obj.bac.length; i++) {
            if (rCount > 11) break;
            var li = document.createElement("LI");
            li.setAttribute("idx", i + 1);
            li.className = "entity";
            ulRight.appendChild(li);
            li.innerHTML = "<div><span>" + obj.bac[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + " <span class='countSpan'>(" + obj.bac[i][2] + ")</span>" + "</span></div>";
            li.setAttribute("href", obj.bac[i][1].replace("&amp;", "&"));
            li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.bac[i][3] + "'\");document.location.href='" + obj.bac[i][1] + "';");
            //li.title = obj.bac[i][0].replace("&amp;", "&").replace("&quote;", "\"");;
            rCount++
        }

        if (obj.r.length > 0) {
            var sep = document.createElement("LI");
            sep.className = "popularHead";
            sep.innerHTML = "<span class='glyphicon glyphicon-home'></span> Retailers";
            ulRight.appendChild(sep);
        }

        for (var i = 0; i < obj.r.length; i++) {
            var li = document.createElement("LI");
            li.setAttribute("idx", i + 1);
            li.className = "entity";
            ulRight.appendChild(li);
            
            li.innerHTML = "<div><span class='aaaa'>" + obj.r[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + "<div class='productReviewDiv'><div class='star-rating reviewDiv'><span style='width:" + obj.r[i][2] + "%;'></span></div></div>" + "</span></div>";
            
            li.setAttribute("href", obj.r[i][1].replace("&amp;", "&"));
            li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.r[i][3] + "'\");document.location.href='" + obj.r[i][1] + "';");
            //li.title = obj.r[i][0].replace("&amp;", "&").replace("&quote;", "\"");
        }
    }

    var keydown = function (evt) {
        evt = window.event || evt;
        if (evt.keyCode == 13) {
            return search(input.id);
        }

        var suggestDivId = 'suggestULLeft';

        if (focusedItem != null) {
            suggestDivId = focusedItem.parentNode.getAttribute('id');
        }

        if (storedSuggest.suggest != null && storedSuggest.suggest.length != 0 && (evt.keyCode in directionKey)) {
            switch (evt.keyCode) {
                case 38: //up
                    focusIdx -= searchSuggest.style.display != "none" ? 1 : 0;
                    break;
                case 40:
                    focusIdx += searchSuggest.style.display != "none" ? 1 : 0;
                    break;
                case 37: //left
                    suggestDivId = 'suggestULLeft';
                    break;
                case 39:
                    suggestDivId = 'suggestULRight';
                    break;

            }
            evt.preventDefault();

            searchSuggest.style.display = "";

            var suggestItems = $("#suggest-div #" + suggestDivId + " li.entity");
            if (focusIdx > suggestItems.length) {
                focusIdx = suggestItems.length;
            } else if (focusIdx <= 0) {
                focusIdx = 1;
                //input.value = storedSuggest.keyword;
                //input.focus();
            }


            try { focusedItem.className = "entity" } catch (e) { }
            suggestItems[focusIdx - 1].className = "entity focus";
            //input.value = suggestItems[focusIdx - 1].title.replace(/(<[^>]*>|\([\s\S]*?\))/g, "").trim();
            input.focus();
            focusedItem = suggestItems[focusIdx - 1];

        }
    }

    var blur = function (evt) {
        searchSuggest.style.display = mouseInSuggest ? "" : "none";
    }

    var mouseover = function (evt) {
        mouseInSuggest = true;
        evt = window.event || evt;
        var obj = evt.srcElement || evt.target;
        if (obj.className.toLowerCase().indexOf("entity") == -1) return;
        obj.className = "entity focus";
        focusIdx = parseInt(obj.getAttribute("idx"))
        if (focusedItem != obj)
            try { focusedItem.className = "entity" } catch (e) { }

        focusedItem = obj;
    }

    var mouseout = function (evt) {
        mouseInSuggest = false;
    }

    var click = function (evt) {
        evt = window.event || evt;
        var obj = evt.srcElement || evt.target;
        if (!(obj.className.toLowerCase().indexOf("entity") == -1 || obj.parentNode.className.toLowerCase().indexOf("entity") == -1)) return;
        if (obj.className.toLowerCase().indexOf("entity"))
            obj = obj.parentNode;

        var url = obj.getAttribute("href");
        if (url) {
            location.href = url;
        } else {
            input.value = obj.title.replace(/(<[^>]*>|\([\s\S]*?\))/g, "").trim();
            input.focus();
            searchSuggest.style.display = "none";
            focusIdx = parseInt(obj.getAttribute("idx"));
            evt.keyCode = 13;
            //allSearchBtnClick(evt);
        }
    }
})(SuggestHelper);

//用于支持popular search的后退功能
window.onpopstate = function (evt) {
    var state = evt.state;

    if (state) {
        SuggestHelper.popularSearch(state);
    } else {
        SuggestHelper.popularSearch('');
    }
}

function generateMixed(n) {
    var chars = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
    var res = "";
    for (var i = 0; i < n; i++) {
        var id = Math.ceil(Math.random() * 35);
        res += chars[id];
    }
    return res;
}

function initTopCategoryNav(topcategoryNenuID, topcategorynavID) {
    $('#' + topcategoryNenuID).click(function () {
        var cName = $('#' + topcategorynavID).attr("class");
        if (cName == 'display-none') {
            $('#' + topcategorynavID).removeClass('display-none');
            $('#' + topcategorynavID).addClass('display-block');
        } else {
            $('#' + topcategorynavID).removeClass('display-block');
            $('#' + topcategorynavID).addClass('display-none');
        }
    });
}
/* --- Home --- */
var Carousel = {};
(function (_$) {

    var vars = {};
    var d = 100;

    _$.init = function (ctxID) {
        vars[ctxID] = {
            time: null,
            interval: null,
            t: 0,
            b: 0,
            c: 0,
            orgPagePoint: null,
            preIdx: 0
        };
        vars[ctxID].idx = 0;

        ctx = $$(ctxID);
        var count = getElementsByClassName("panel", ctx).length;
        vars[ctxID].count = count;

        getElementsByClassName("ctxInline", ctx)[0].style.width = (count + 1) * 100 + '%';
        addRule("#" + ctxID + " .ctxOutline .ctxInline .panel", "width:" + (100 / (count + 1)) + "%;");
        var page = getElementsByClassName("page", ctx)[0];
        var i = 0;
        if (page != null) {
            page.innerHTML = ("<a href='javascript:Carousel.runTo(&quot;" + ctxID + "&quot;,{idx})' class='pagePoint'><span></span></a>").repeat(count).replace(/\{idx\}/g, function () { return i++ });
        }
    }

    _$.pre = function (ctxID) {
        vars[ctxID].preIdx = vars[ctxID].idx;
        vars[ctxID].idx--;
        if (vars[ctxID].idx < 0) vars[ctxID].idx = vars[ctxID].count - 1;
        _$.run(ctxID);
    }

    _$.next = function (ctxID) {
        vars[ctxID].preIdx = vars[ctxID].idx;
        vars[ctxID].idx++;
        if (vars[ctxID].idx >= vars[ctxID].count) vars[ctxID].idx = 0;
        _$.run(ctxID);
    }

    _$.runTo = function (ctxID, idx) {
        vars[ctxID].preIdx = vars[ctxID].idx;
        clearInterval(vars[ctxID].interval);
        vars[ctxID].idx = idx < 0 || idx >= vars[ctxID].count ? 0 : idx;
        _$.run(ctxID);
    }

    var hide = function (ctxID) {
        var panels = getElementsByClassName("panel", ctxID);
        var t = [vars[ctxID].preIdx, vars[ctxID].idx];
        t.sort();
        var min = t[0] + 1;
        var max = t[1];

        for (min; min < max; min++) {
            panels[min].style.display = "none";
        }

        getElementsByClassName("ctxInline", ctxID)[0].style.left = panels[vars[ctxID].preIdx].offsetLeft + "px";
    }

    var reset = function (ctxID) {
        var panels = getElementsByClassName("panel", ctxID);
        var min = 1;
        var max = vars[ctxID].count - 1;
        for (min; min < max; min++) {
            panels[min].style.display = "block";
        }
    }

    _$.run = function (ctxID) {
        reset(ctxID);
        hide(ctxID);
        clearTimeout(vars[ctxID].time);
        vars[ctxID].t = 0;
        vars[ctxID].b = 0;
        vars[ctxID].c = 100;

        getElementsByClassName("ctxInline", ctxID)[0].style.left = -getElementsByClassName("panel", ctxID)[vars[ctxID].idx].offsetLeft + "px";

        try { vars[ctxID].orgPagePoint.className = "pagePoint" } catch (e) { }
        var pagePoint = getElementsByClassName("pagePoint", ctxID)[vars[ctxID].idx]; // objs.pagepoints[vars.idx];
        if (pagePoint != null) {
            pagePoint.className = "pagePoint current";
            vars[ctxID].orgPagePoint = pagePoint;
            run_(ctxID);
        }
    }

    var easeOut = function (t, b, c, d) {
        return -c * ((t = t / d - 1) * t * t * t - 1) + b;
    }

    var run_ = function (ctxID) {
        if (vars[ctxID].t < d) {
            vars[ctxID].t++;
            if (0) {
                getElementsByClassName("ctxInline", ctxID)[0].style.filter = 'alpha(opacity:' + parseInt(easeOut(vars[ctxID].t, vars[ctxID].b, vars[ctxID].c - vars[ctxID].b, d)) + ')';
            } else {
                getElementsByClassName("ctxInline", ctxID)[0].style.opacity = parseInt(easeOut(vars[ctxID].t, vars[ctxID].b, vars[ctxID].c - vars[ctxID].b, d)) / 100;
            }

            vars.time = setTimeout(doFunction(run_, ctxID), 10);
        } else {

        }
    }

    _$.start = function (ctxID) {
        vars[ctxID].interval = setInterval(doFunction(_$.next, ctxID), 7000);
    }

})(Carousel);

$(document).ready(function () {

    var width = $(window).width();
    
    proccessNewSearchBar(width);

    //showWidth(width);

    proccessCategoryNav(width);

    proccessMyList(width);

    prossPopularSearch();

    if ('ontouchstart' in document.documentElement) { // or whatever "is this a touch device?" test we want to use
        $('body').css('cursor', 'pointer');
    }
});

function prossPopularSearch()
{
    var kw = location.hash;
    if (kw.length > 0) {
        SuggestHelper.popularSearch(kw.replace('#', ''));
    }
}

var categoryNavAtTop = false;
function proccessCategoryNav(windowWidth) {
    if (windowWidth < 768) {
        if (!categoryNavAtTop) {
            
            //$('#top-breadCrumb').html($('#new-breadcrumb').html());

            var lis = $('#new-breadcrumb > li');
            for (var i = lis.length - 1; i >= 0; i--) {
                $('#top-breadCrumb').append(lis[i]);
            }

            $('#new-breadcrumb').html('');

            categoryNavAtTop = true;
        }
    } else {
        if (categoryNavAtTop) {
            
            //$('#new-breadcrumb').html($('#top-breadCrumb').html());

            var lis = $('#top-breadCrumb > li');
            for (var i = lis.length - 1; i >= 0; i--) {
                $('#new-breadcrumb').append(lis[i]);
            }

            $('#top-breadCrumb').html('');

            categoryNavAtTop = false;
        }
    }
}

var searchBarInHeader = true;
function proccessSearchBar(windowWidth) {
    
    if (windowWidth < 990) {
        if (searchBarInHeader) {
            $('#fix-search-div').html($('#header-search-div').html());
            $('#header-search-div').html('');
            searchBarInHeader = false;
            SuggestHelper.reInit();
        }
    } else {
        if (!searchBarInHeader) {
            $('#header-search-div').html($('#fix-search-div').html());
            $('#fix-search-div').html('');
            searchBarInHeader = true;
            SuggestHelper.reInit();
        }
    }
}

function proccessNewSearchBar(windowWidth) {

    if (windowWidth < 990) {
        if (searchBarInHeader) {
            $('#out-searchbar').html($('#in-searchbar').html()).css('display', 'table');
            $('#in-searchbar').html('').css('display', 'none');
            searchBarInHeader = false;
            SuggestHelper.reInit();
        }
    } else {
        if (!searchBarInHeader) {
            $('#in-searchbar').html($('#out-searchbar').html()).css('display', 'table');
            $('#out-searchbar').html('').css('display', 'none');
            searchBarInHeader = true;
            SuggestHelper.reInit();
        }
    }
}

var myListAtTop = false;
function proccessMyList(windowWidth) {
    if (windowWidth < 470) {
        if (!myListAtTop) {

            $('#top-MyListContent').html($('#MyListContent').html());

            $('#MyListContent').html('');

            $('#MyListDisplay').css('display', 'none');

            SetMyList('#RecentlyViewed', '#LiRecentlyViewed');

            $('#LiMyComparisonList').css('display', 'none');

            $('#MyListDisplay').css('display', 'none');

            myListAtTop = true;
        }
    } else {
        if (myListAtTop) {

            $('#MyListContent').html($('#top-MyListContent').html());

            $('#top-MyListContent').html('');

            $('#MyListDisplay').css('display', 'block');

            $('#LiMyComparisonList').css('display', 'block');

            SetMyList('#MyComparisonList', '#LiMyComparisonList');

            myListAtTop = false;
        }
    }
}

$(window).resize(function () {
    var width = $(window).width();

    proccessNewSearchBar(width);
    
    //showWidth(width);

    proccessCategoryNav(width);

    proccessMyList(width);
});

function showWidth(windowWidth) {
    $('#widthVal').html(windowWidth);
}

function loadFootEevent(windowWidth) {
    var footDivs = $(".footerCol");
    if (windowWidth < 768) {

        footDivs.each(function (index) {
            var titles = $(this).find("b");
            var menus = $(this).find("ul");

            if (titles.length > 0) {
                $(titles[0]).click(
                function () {
                    var displayCss = $(menus[0]).css("height");
                    //alert(displayCss);
                    if (displayCss != "0px") {
                        $(menus[0]).stop().animate({ height: "0" }, { queue: false, duration: 200, easing: 'swing' });
                    } else {
                        $(menus[0]).stop().animate({ height: "132" }, { queue: false, duration: 200, easing: 'swing' });
                    }
                });

                $(menus[0]).css("height", "0");
            }

        });
    } else {
        footDivs.each(function (index) {
            var titles = $(this).find("b");
            var menus = $(this).find("ul");

            if (titles.length > 0) {
                $(titles[0]).unbind("click");
                $(menus[0]).css("height", "132");
            }

        });
    }

    
}
/* --- End ZhengLei ---*/


/* --- Start Dawn --- */
function on_clickOutRetailer(rid, countryID, redirect) {
    var url = "/ResponseRedirect.aspx?aid=40&pid=&rid=" + rid + "&rpid=&t=r" + "&countryID=" + countryID;
    //pageTracker._trackPageview(url);
    if (redirect == 'True') {
        ga('send', 'pageview', "'" + url + "'");
    }

    //trackClick(url, countryID);
}
function trackClick(url, countryID) {
    url = rootUrl + url;
    window.open(url, generateMixed(5), "", "");
}


function Compare_Click() {
    var obj = $("input[type='checkbox']:checked");
    var pidStr = "";
    if (obj == null || obj.length == 0 || obj.length > 3) {
        alert("Please select 2 - 3 products to compare.");
        return false;
    }
    else {
        for (var i = 0; i < obj.length; i++) {
            pidStr += obj[i].value + ",";
        }
        window.location = "/Compare.aspx?t=js&pids=" + pidStr;
    }
}


function LoadCompareReviews(ids, width) {
    //$.ajax({
    //    url: '/GetCompareReviews.aspx?ids=' + ids + '&width=' + width,
    //    type: 'GET',
    //    dataType: 'html',
    //    error: function (request, error) {
    //    },
    //    success: function (html) {
    //        $('#ReviewsBody').html(html);
    //    }
    //});
}

function ShowCompareReviews(d, b) {
    var item = document.getElementById(b);
    var sp = $('#' + d).find("b")[0];

    if (item.style.display == "none") {
        $('#' + b).removeAttr("style");
        $('#' + d).removeClass("ReviewsDiv");
        $('#' + d).addClass("reviewsDivShow");

        sp.removeClass = "glyphicon-chevron-down";
        sp.addClass = "glyphicon-chevron-up";
    }
    else {
        item.style.display = "none";
        $('#' + d).removeClass("reviewsDivShow");
        $('#' + d).addClass("reviewsDiv");

        sp.removeClass = "glyphicon-chevron-up";
        sp.addClass = "glyphicon-chevron-down";
    }

}

function ConfirmSelectValue2() {
    var rating = document.getElementById('ctl00_ContentPlaceHolder1_RetailerInfoTab1_RetailerReviewDisplay1_starDeliveryValue').value;
    if (rating == -1 || rating == '') { alert('Please select a rating!'); return false; }
    var review = document.getElementById('ctl00_ContentPlaceHolder1_RetailerInfoTab1_RetailerReviewDisplay1_txtReview').value;
    if (review.trim() == '') { alert('Please retype review!'); return false; }
    return true;
}

function initP(className) {
    $('#newCategorySitemapDiv').append($('#pln'));
    $('#newCategorySitemapDiv').addClass(className);

    $('#newCategorySitemapDiv').click(function () {
        //onclickCategoryDiv();
    });

    $(document).mouseup(function (e) {
        var t = $(e.target);
        if ($(e.target).parent("#pln").length == 0) {
            if ($(e.target)[0].id == 'newCategorySitemapDiv') {
                onclickCategoryDiv();
            } else {
                $('#pln').css('display', '');
                if ($('#newCategorySitemapDiv').hasClass('categorySitemapTd')) {
                    $('#newCategorySitemapDiv').removeClass('categorySitemapTdhover');
                } else {
                    $('#newCategorySitemapDiv').removeClass('categorySitemapTdIDhover');
                }
            }
        }

    })
}

function lazyload(option) {
    var settings = { defObj: null, defHeight: 0 };
    settings = $.extend(settings, option || {});
    var defHeight = settings.defHeight, defObj = (typeof settings.defObj == "object") ? settings.defObj.find("img") : $(settings.defObj).find("img");
    var pageTop = function () { var d = document, y = (navigator.userAgent.toLowerCase().match(/iPad/i) == "ipad") ? window.pageYOffset : Math.max(d.documentElement.scrollTop, d.body.scrollTop); return d.documentElement.clientHeight + y - settings.defHeight };
    var imgLoad = function () { defObj.each(function () { if ($(this).offset().top - 150 <= pageTop()) { var src2 = $(this).attr("data-pm-src2"); if (src2) { $(this).attr("src", src2).removeAttr("data-pm-src2") } } }) };
    imgLoad();
    $(window).bind("scroll", function () { imgLoad() })
}

function InitATag(title) {
    $('#twitterATag').attr('href', 'https://twitter.com/home?status=' + title + " @ priceme - " + window.location);
    //$('#facebookATag').attr('href', 'http://www.facebook.com/sharer.php?u=' + window.location + "&t=" + title + " @ priceme");
    $('#emailAtag').attr('href', 'mailto:?subject=' + title + '&body=I%20think%20this%20may%20be%20of%20interest%20to%20you,%20check%20it%20out:%20%0D%0A%20%0D%0A' + window.location + '%0A')
}

function downloadJSAtOnload() {
    var element = document.createElement("script");
    element.src = "/Scripts/defer.js";

    var node = document.body.firstChild;
    if (node) {
        node.parentNode.insertBefore(element, node);
    } else {
        document.body.appendChild(element);
    }

}

/* --- End Dawn --- */


/* --- Yuan xiang --- */
function on_clickOut(ordid, pid, rid, rpid, pname, catId, price, other, countryId, redirect, useGoogleTrackConversion) {
    var url = "/ResponseRedirect.aspx?aid=40&pid=" + pid + "&rid=" + rid + "&rpid=" + rpid + "&countryID=" + countryId + "&cid=" + catId + other;
    //alert(url);
    if (redirect == 'True') {
        ga('send', 'event', 'pm_clo');
        ga('send', 'pageview', "'" + url + "'");
        trackEcom(ordid, rpid, pname, catId, price);
    }

    if (countryId == 3 && useGoogleTrackConversion == 1)
    {
        goog_report_conversion(window.location.href);
    }
}
function trackEcom(ordid, rpid, pname, catId, price) {
    ga('require', 'ecommerce', 'ecommerce.js');

    //_gaq.push(['_addTrans', "'" + ordid + "'", '', '', '', '', '', '', '']);
    ga('ecommerce:addTransaction', {
        'id': ordid,                     // Transaction ID. Required.
        'affiliation': 'store',   // Affiliation or store name.
        'revenue': price,       // Grand Total.
        'shipping': 0,                  // Shipping.
        'tax': 0                     // Tax.
    });

    //_gaq.push(['_addItem', "'" + ordid + "'", "'" + rpid + "'", "'" + pname + "'", "'" + catId + "'", "'" + price + "'", '1']);
    ga('ecommerce:addItem', {
        'id': ordid,                     // Transaction ID. Required.
        'name': pname,    // Product name. Required.
        'sku': rpid,                 // SKU/code.
        'category': catId,         // Category or variation.
        'price': price,                 // Unit price.
        'quantity': 1                   // Quantity.
    });

    //_gaq.push(['_trackTrans']);
    ga('ecommerce:send');
}
/* --- End Yuan xiang --- */

/* --- Start HRL --- */
function ShowMore(obj, tagID) {
    obj.className = "hide";
    var tag = document.getElementById(tagID);
    if (tag != null) {
        tag.className = "show";
    }
}

function noShowMore(obj, tagID, tagID1) {
    //obj.className = "show";
    var tag = document.getElementById(tagID);
    if (tag != null) {
        tag.className = "hide";
        tag.previousElementSibling.className = "show aStyel";
    }
}

function OpenPage(pageUrl) {
    window.location = pageUrl;
}
function OpenNewPage(pageUrl) {
    var tmp = window.open("about:blank", "", "")
    tmp.location = pageUrl;
}

function on_clickAndOut(ordid, pid, rid, rpid, pname, catId, price, other, countryID, redirect, outUrl) {
    var url = "/ResponseRedirect.aspx?aid=40&pid=" + pid + "&rid=" + rid + "&rpid=" + rpid + "&countryID=" + countryID + "&cid=" + catId + other;
    var uuid = "&uuid=" + getuuid(8, 16);
    url = url + uuid;
    if (redirect == 'True') {
        ga('send', 'pageview', "'" + url + "'");
        trackEcom(ordid, rpid, pname, catId, price);
    }

    var tmp = window.open("about:blank", "", "")
    tmp.location = outUrl;
}

function getuuid(len, radix) {
    var chars = '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz'.split('');
    var uuid = [], i;
    radix = radix || chars.length;

    if (len) {
        for (i = 0; i < len; i++) uuid[i] = chars[0 | Math.random() * radix];
    } else {
        var r;
        uuid[8] = uuid[13] = uuid[18] = uuid[23] = '-';
        uuid[14] = '4';

        for (i = 0; i < 36; i++) {
            if (!uuid[i]) {
                r = 0 | Math.random() * 16;
                uuid[i] = chars[(i == 19) ? (r & 0x3) | 0x8 : r];
            }
        }
    }

    return uuid.join('');
}

function GetData(pid, txt, content) {
    var slideDis = $("#MyListContent").css('display');
    if (slideDis == "undefined" || slideDis == "" || slideDis == "none") {
        slideRightShow();
    }
    $.ajax({
        type: 'post',
        url: '/DealPostBack.aspx',
        data: jsonProductData(pid, 'add'),
        dataType: 'json',
        beforeSend: function () {
            $('#add_' + pid).removeClass("addDiv");
            $('#add_' + pid).removeClass("bg1");
            $('#add_' + pid).addClass("added");
            $('#add_' + pid).html("Adding..."); //.css("text-decoration", "none").css("color", "#C7CBBC").css("cursor", "default");
        },
        success: function (data, textStatu) {
            if (textStatu == 'success') {
                if (data.status == 'add') {
                    $('#ProductList').prepend(CreateData(pid, data, content));
                }

                $('#add_' + pid).html(txt); //.css("color", "#C7CBBC").css("cursor", "default").css("width", "123px");
                $('#add_' + pid).unbind('click');
                $('#add_' + pid).attr('onclick', '');
            }
        }
    });
}

function GetProductData(pid, type, content) {
    var slideDis = $("#MyListContent").css('display');
    if (slideDis == "undefined" || slideDis == "" || slideDis == "none") {
        slideRightShow();
    }
    $.ajax({
        type: 'post',
        url: '/DealPostBack.aspx',
        data: jsonProductData(pid, type),
        dataType: 'json',
        beforeSend: function () {
            if (type == "add") {
                $('#pic_' + pid).removeClass("pAddCompareDiv").addClass("pAddedDiv");
                $('#Add_' + pid).removeAttr("class");
                $('#Add_' + pid).html("Adding...").css("text-decoration", "none").css("color", "#C7CBBC")
                    .css("cursor", "default").css("text-align", "left").css("margin-top", "5px");
                $('#bcaddtolisttext').html("Adding...").css("text-decoration", "none").css("color", "#C7CBBC")
                    .css("cursor", "default");
            }
        },
        success: function (data, textStatu) {
            if (textStatu == 'success') {
                if (type == 'add') {
                    if (data.status == 'add') {
                        $('#ProductList').prepend(CreateData(pid, data, content));
                    }
                }
                else if (type == 'remove') {
                    if (pid == '') {
                        $('#ProductList').html("");
                    }
                    else {
                        $('#MyProductList' + pid).remove();
                    }
                }

                if (type == "add") {
                    $('#Add_' + pid).html("&nbsp;&nbsp;" + content).css("text-decoration", "none").css("color", "#C7CBBC")
                        .css("cursor", "default").css("margin-top", "5px");
                    $('#Add_' + pid).unbind('click');
                    $('#Add_' + pid).removeAttr("onclick");
                    $('#Add_' + pid).removeAttr("class");

                    $('#bcaddtolist').removeAttr("onclick");
                    $('#bcaddtolisttext').html("&nbsp;&nbsp;" + content).css("text-decoration", "none").css("color", "#C7CBBC")
                        .css("cursor", "default");

                    $('.listnoproduct').remove();
                }
                else {
                    $('#pic_' + pid).html("<div class=\"btn btn-default btn-xs btnGray\" style=\"margin-right:0px;\" id=\"Add_" + pid + "\" onclick=\"javascript:ga('send', 'event', 'addbasket', 'product', '" + pid + "'); GetProductData('" + pid + "', 'add', 'Added'); return true;\"><span class=\"glyphicon glyphicon-plus-sign iconGray\"></span> Add to list</div>");

                    var products = $('#ProductList .Product');
                    if (products.length == 0) {
                        $('#ProductList').html("<div class=\"listnoproduct\">Your list is empty. Add products to the list using the 'Add to list' button.</div>");
                    }
                }
            }
        }
    });
}
function jsonProductData(pid, type) {
    var jsonStr = "({" + "\"pid\":";
    jsonStr += "\"" + pid + "\"" + ",";
    jsonStr += "\"type\":";
    jsonStr += "\"" + type + "\"" + "})";
    return eval(jsonStr);
}
function CreateData(pid, data, content) {
    var dr = "<div  class=\"Product\" id=\"MyProductList" + pid + "\">";
    dr += "<div class=\"name\"><a href=\"" + data.url + "\">" + data.name + "</a></div>";
    dr += "<div class=\"glyphicon glyphicon-remove\" onclick=\"javascript:GetProductData(\'" + pid + "\', \'remove\',\'" + content + "\');\"></div><div class=\"clr\"></div>";
    return dr;
}

function slideRightShow() {
    if ($("#MyListDisplay").hasClass('Display')) {
        $("#MyListContent").animate({ width: "show" }, 1000);
        $("#MyListDisplay").removeClass('Display');
        $("#MyListDisplay").addClass('Close');
    }
    else {
        $("#MyListContent").animate({ width: "hide" }, 1000);
        $("#MyListDisplay").removeClass('Close');
        $("#MyListDisplay").addClass('Display');
    }
}
function SetMyList(id, liId) {
    $(id).attr('style', 'display:block');
    var ids = ['#MyComparisonList', '#RecentlyViewed'];
    for (var i = 0; i < ids.length; i++) {
        if (ids[i] != id) {
            $(ids[i]).attr('style', 'display:none');
        }
    }

    $(liId).addClass('active');
    var liids = ['#LiMyComparisonList', '#LiRecentlyViewed'];
    for (var i = 0; i < liids.length; i++) {
        if (liids[i] != liId) {
            $(liids[i]).removeClass('active');
        }
    }
}
function ShowCompare() {
    console.info("2"); 
    //var url = "/Compare.aspx?op=cp";
    var productList = $("#ProductList");
    var child = productList.children().count;
    var pids = "";
    if (child <= 3) {
        productList.find(".Product").each(function (i, item) {
            var pid = $(item).attr("id").replace("MyProductList", "");
            pids += pid + ",";
        })
    } else {
        productList.find(".Product").each(function (i, item) {
            if (i <= 2) {
                var pid = $(item).attr("id").replace("MyProductList", "");
                pids += pid + ",";
            }
          
        })
    }
    pids = pids.substring(0, pids.lastIndexOf(","));
    //console.info(pids);
    //return false;
    var url = "/Compare.aspx?t=js&pids=" + pids;
    window.location = url;
}
function ShowManageList(url) {
    window.location = url;
}

function Delete_Click(button) {
    var obj = $("input[type='checkbox']:checked");
    if (obj == null || obj.length == 0) {
        alert("Please select at least one product to delete.");
        return false;
    }
    else {
        if(window.confirm("Are you sure to delete selected item(s)?"))
        {
            document.getElementById(button).click();
        }
    }
}
function stopBubble(e) {
    //如果提供了事件对象，则这是一个非IE浏览器
    if (e && e.stopPropagation)
        //因此它支持W3C的stopPropagation()方法
        e.stopPropagation();
    else
        //否则，我们需要使用IE的方式来取消事件冒泡
        window.event.cancelBubble = true;
}

function stopDefault(e) {
    if (e && e.preventDefault)
        e.preventDefault();
    else
        window.event.returnValue = false;
    return false;
}
/* --- End HRL --- */

function Login(a) { window.location = a };

var PriceMeSurvey = {};
(function (_PS) {

    var _divID;
    _PS.Init = function (divID) {
        _divID = '#' + divID;
    }

    _PS.DoExpand = function ()  {
        $(_divID).removeClass("closed");
        $(_divID).addClass("expanded");
        $(_divID).animate({ bottom: "0" }, 500);
    }

    _PS.DoClose = function () {
        $(_divID).removeClass("expanded");
        $(_divID).addClass("closed");
        $(_divID).animate({ bottom: "-351" }, 500);
    }

    _PS.Survey = function () {
        if ($(_divID).hasClass("expanded")) {
            _PS.DoClose();
            ga('send', 'event', 'Close survey', '', '');
        } else {
            _PS.DoExpand();
            ga('send', 'event', 'Expand survey', '', '');
        }
    }

})(PriceMeSurvey)

function EscapeHTMLTags(id) {
    var test = $('#' + id).val();
    var start_ptn = /\</g;
    var end_ptn = /\>/g;
    var test1 = test.replace(start_ptn, "").replace(end_ptn);

    $('#' + id).val(test1);
}

function ShowHide(id) {
    var item = $('#' + id);
    var itemclass = item.attr("class")
    var span = $('#span' + id);

    if (itemclass == 'display-none faqsspan') {
        item.removeClass('display-none faqsspan');
        item.addClass('display-block faqsspan');

        span.removeClass('glyphicon glyphicon-chevron-down faqsche')
        span.addClass('glyphicon glyphicon-chevron-up faqsche')
    } else {
        item.removeClass('display-block faqsspan');
        item.addClass('display-none faqsspan');

        span.removeClass('glyphicon glyphicon-chevron-up faqsche')
        span.addClass('glyphicon glyphicon-chevron-down faqsche')
    }
}

function FavouriteCatalog(cid, isfavourite, islogin) {
    var item = $('#spancatalog');
    var btn = $('#favcatalog');
    if (islogin != "True") {
        location.href = '/FavouriteCatalog.aspx?cid=' + cid + '&fav=' + isfavourite;
    }
    else {
        $.ajax({
            url: '/FavouriteCatalog.aspx?cid=' + cid + '&fav=' + isfavourite,
            type: 'GET',
            dataType: 'html',
            success: function (html) {
                if (isfavourite == "True") {
                    item.removeClass('glyphicon glyphicon-heart-empty');
                    item.addClass('glyphicon glyphicon-heart');
                    btn.removeClass('sub-favourite iconGrayIm');
                    btn.addClass('sub-favourite iconBlue');
                    btn[0].setAttribute("onclick", "FavouriteCatalog('" + cid + "', 'Flase', '" + islogin + "'); FavisLogin('" + islogin + "', 'favouritecatalog.aspx', 'Flase');");
                }
                else {
                    item.removeClass('glyphicon glyphicon-heart');
                    item.addClass('glyphicon glyphicon-heart-empty');
                    btn.removeClass('sub-favourite iconBlue');
                    btn.addClass('sub-favourite iconGrayIm');
                    btn[0].setAttribute("onclick", "FavouriteCatalog('" + cid + "', 'True', '" + islogin + "'); FavisLogin('" + islogin + "', 'favouritecatalog.aspx', 'True');");
                }
            }
        });
    }
}

function FavouriteSearch(url, isfavourite, islogin) {
    var item = $('#spansearch');
    var btn = $('#favsearch');
    if (islogin != "True") {
        location.href = '/FavoritesSearch.aspx?url=' + url + '&fav=' + isfavourite;
    }
    else {
        $.ajax({
            url: '/FavoritesSearch.aspx?url=' + url + '&fav=' + isfavourite,
            type: 'GET',
            dataType: 'html',
            success: function (html) {
                if (isfavourite == "True") {
                    item.removeClass('glyphicon glyphicon-heart-empty');
                    item.addClass('glyphicon glyphicon-heart');
                    btn.removeClass('sub-favourite iconGrayIm');
                    btn.addClass('sub-favourite iconBlue');
                    btn[0].setAttribute("onclick", "FavouriteSearch('" + url + "', 'Flase', '" + islogin + "'); FavisLogin('" + islogin + "', 'favoritessearch.aspx', 'Flase');");
                }
                else {
                    item.removeClass('glyphicon glyphicon-heart');
                    item.addClass('glyphicon glyphicon-heart-empty');
                    btn.removeClass('sub-favourite iconBlue');
                    btn.addClass('sub-favourite iconGrayIm');
                    btn[0].setAttribute("onclick", "FavouriteSearch('" + url + "', 'True', '" + islogin + "'); FavisLogin('" + islogin + "', 'favoritessearch.aspx', 'True');");
                }
            }
        });
    }
}

function ShowFavourite(id, type) {
    var pop = $('#' + id);
    if (type == 1) {
        if (pop[0].className == "dropdown-menu display-none") {
            pop.animate({ height: "show" }, 1000);
            pop[0].className = "dropdown-menu display-block";
        }
        else {
            pop.animate({ height: "hide" }, 1000);
            pop[0].className = "dropdown-menu display-none";
            return true;
        }
    }
    var ohter = $('#listpop');
    if (ohter.length > 0) {
        ohter[0].className = "dropdown-menu display-none";
        ohter.css("display", "none");
    }

    FavouriteButton('btnFav');

    var content = $('#favpopcontent');

    $.ajax({
        url: '/FavouritePop.aspx',
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            content.html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            content.html(html);
        }
    });
}

function ShowPricedrops(id) {
    var pop = $('#' + id);

    $.ajax({
        url: '/ProductAlertPop.aspx',
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            pop.html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            pop.html(html);
        }
    });
    
    FavouriteButton('btPrice');
}

function ShowRecentlyViewed(id) {
    var pop = $('#' + id);

    $.ajax({
        url: '/GetRecentlyViewed.aspx',
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            pop.html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            pop.html(html);
        }
    });

    FavouriteButton('btnRecently');
}

function FavouriteButton(btn) {
    var btnf = $('#btnFav').removeClass("active");
    var btnp = $('#btPrice').removeClass("active");
    var btnd = $('#btnRecently').removeClass("active");

    var active = $('#' + btn).addClass("active");
}

function ShowList(id) {
    var pop = $('#' + id);
    if (pop[0].className == "dropdown-menu display-none") {
        pop.animate({ height: "show" }, 1000);
        pop[0].className = "dropdown-menu display-block";
        pop.css("display", "block");
    }
    else {
        pop.animate({ height: "hide" }, 1000);
        pop[0].className = "dropdown-menu display-none";
        pop.css("display", "none");
        return true;
    }

    var ohter = $('#favpop');
    if (ohter.length > 0) {
        ohter[0].className = "dropdown-menu display-none";
        ohter.css("display", "none");
    }

    $.ajax({
        url: '/MyListpop.aspx',
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            pop.html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            pop.html(html);
        }
    });
}

//function FavOnMouse(id) {
//    var span = $('#' + id);
//    if (span[0].className == "glyphicon glyphicon-heart-empty iconGrayIm")
//        span[0].className = "glyphicon glyphicon-heart iconBlue";
//    else
//        span[0].className = "glyphicon glyphicon-heart-empty iconGrayIm";
//}

function formatPrice(n) {
    var t = parseInt(n), i, r;
    for (t = t.toString().replace(/^(\d*)$/, "$1."), t = (t + "00").replace(/(\d*\.\d\d)\d*/, "$1"), t = t.replace(".", ","), i = /(\d)(\d{3},)/; i.test(t) ;)
        t = t.replace(i, "$1,$2");
    return t = t.replace(/,(\d\d)$/, ".$1"), r = t.split("."), r[1] == "00" && (t = r[0]), t
}

function toIntValue(num) {
    var tF = parseFloat(num);
    var t = parseInt(num);
    if (tF == t) {
        return t.toString();
    } else {
        return tF.toFixed(1);
    }
}

function openlink(link) {
    location.href = link;
}

function loginfavourite(url, islogin) {
    if (islogin == "True")
        ShowFavourite('favpop', 1);
    else {
        openlink("/login.aspx?url=" + url);
        return false;
    }
}

function delayed(id) {
    var pop = $('#' + id);
    pop.animate({ height: "show" }, 1000);
}

function aboutui(id) {
    for (var i = 1; i < 6; i++) {
        var title = $('#ui-id-' + i);
        var conten = $('#ui-cn-' + i);

        title.removeClass();
        if (id == i) {
            title.addClass("wpb_accordion_header ui-accordion-header-active");
            conten.animate({ height: "show" }, 1000);
            conten.css("display", "block");
        }
        else {
            title.addClass("wpb_accordion_header");
            conten.animate({ height: "hide" }, 1000);
            conten.css("display", "none");
        }
    }
}

function compareBtnLoad(pcCount) {
    var width = $(window).width();
    $(".comparePriceDiv").each(function () {
        if (width < 768) {
            var atag = $(this).find(".compareVS a")[0];
            if (atag.className.indexOf("btn-vs") > 0)
                atag.innerHTML = str_VS;
            else if (atag.className.indexOf("btn-cp") > 0)
                atag.innerHTML = str_C;
        }
        else {
            var atag = $(this).find(".compareVS a")[0];
            if (atag.className.indexOf("btn-vs") > 0)
                atag.innerHTML = str_VS;
            else if (atag.className.indexOf("btn-cp") > 0)
                atag.innerHTML = str_CP;
        }
    });
    setTableWidthCompare(width, pcCount);
}

function setTableWidthCompare(windowswidth, pcCount) {
    var tableWidth = 0;
    var pcCount = pcCount;            
    var thAttrName = 0;
    var tdAttrValue = 0;

    if (windowswidth > 1200) {
        tableWidth = 1170;
        thAttrName = 235;
        console.info("1200");
        console.info(pcCount);
        tdAttrValue = ((tableWidth - thAttrName) / pcCount);

        //thAttrName=235;
        if(pcCount==1){

            tdAttrValue=635;
            var c=865,d=870,e=275;
            resizeWindow(pcCount,tdAttrValue,thAttrName,c,d,e);

        }else if(pcCount==2){

            tdAttrValue=317.5;
            var c=865,d=870,e=275;
            resizeWindow(pcCount,tdAttrValue,thAttrName,c,d,e);

        }else{
            $(".relatedProducts").hide();
        }
                

    } else if (windowswidth > 1007) {
        tableWidth = 990;
        thAttrName = 125;
        console.info("1007");
        tdAttrValue = ((tableWidth - thAttrName) / pcCount);

        if(pcCount==1){

            tdAttrValue=560;
            var c=685,d=690,e=275;
            resizeWindow(pcCount,tdAttrValue,thAttrName,c,d,e);

        }else if(pcCount==2){

            tdAttrValue=282.5;
            var c=680,d=685,e=275;
            resizeWindow(pcCount,tdAttrValue,thAttrName,c,d,e);//width: 100px; height: 110px; margin-top: 20px;

        }else{
            $(".relatedProducts").hide();
        }
               

    }  else if (windowswidth > 750) {
        tableWidth = 750;
        thAttrName = 100;
        console.info("750");
        tdAttrValue = ((tableWidth - thAttrName) / pcCount);

        if(pcCount==1){

            tdAttrValue=345;
            var c=450,d=455,e=275;
            resizeWindow(pcCount,tdAttrValue,thAttrName,c,d,e);

        }else if(pcCount==2){

            thAttrName=111;
            tdAttrValue=150;
            var c=450,d=455,e=275;
            resizeWindow(pcCount,tdAttrValue,thAttrName,c,d,e);

        }else{
            $(".relatedProducts").hide();
        }
                

    } else if (windowswidth > 370) {
        $(".relatedProducts").hide();
        tableWidth = 360;
        thAttrName = 90;

        tdAttrValue = ((tableWidth - thAttrName) / pcCount);

        var c=360,d=360,e=275;
        resizeWindow(1,tdAttrValue,thAttrName,c,d,e);
    } else {
        $(".relatedProducts").hide();
        tableWidth = 300;
        thAttrName = 74;

        tdAttrValue = ((tableWidth - thAttrName) / pcCount);
        var c=300,d=300,e=275;
        resizeWindow(1,tdAttrValue,thAttrName,c,d,e);
    }

    $('.thAttrName').css('width', thAttrName + 'px');
    $('.tdAttrValue').css('width', tdAttrValue + 'px');
}

function resizeWindow(pcCount,value,name,center,back,related){

    tdAttrValue=value;
    thAttrName=name; 

    $(".compareCenter").css({'width':center+'px','float':'left'});
    //$("#lineChart").css("width",center+"px");
    //$("#barChart").css("width",center+"px")
    $(".backResults").css("width",back+"px");
    $(".relatedProducts").show().css({'width':related+'px','margin-top':'5px'});
            
}