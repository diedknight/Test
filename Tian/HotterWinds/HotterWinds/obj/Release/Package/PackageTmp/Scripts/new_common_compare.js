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
function LoadCompareReviews(ids, width) {
    $.ajax({
        url: '/GetCompareReviews.aspx?ids=' + ids + '&width=' + width,
        type: 'GET',
        dataType: 'html',
        error: function (request, error) {
        },
        success: function (html) {
            $('#ReviewsBody').html(html);
            $('#ReviewsBody .thAttrName').css('width', width + 'px');
        }
    });
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
function setFocus2(sel) {
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
        $c(form, "errorredirect", 'https://www.icontact.com/www/signup/error.html');
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
        $c(form, "errorredirect", 'https://www.icontact.com/www/signup/error.html');
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
        $c(form, "errorredirect", 'https://www.icontact.com/www/signup/error.html');
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
        $c(form, "errorredirect", 'https://www.icontact.com/www/signup/error.html');
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

var SuggestHelper2 = {};
(function ($$) {

    var ajaxObj = null;
    var locationUrl = null;
    var storedSuggest = {
        keyword: null,
        suggest: null
    };
    var directionKey = { 38: "up", 40: "down", 13: "enter" };
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
        SuggestHelper2.init(_inputID, _searchSuggestShowAreaID, _suggestULLeft, _suggestULRight);
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

        $('#suggest-div2 .glyphicon-remove-sign').click(function() {
            searchSuggest.style.display = "none";
        });
        //input.onblur = blur;
        input.onkeydown = keydown;
        input.onkeyup = Get;
        input.setAttribute("autocomplete", "off");

        ulLeft.onmouseover = mouseover;
        ulLeft.onmouseout = mouseout;
        ulLeft.onclick = click;

        ulRight.onmouseover = mouseover;
        ulRight.onmouseout = mouseout;
        ulRight.onclick = click;
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

        if (locationUrl.toLowerCase().indexOf('compare.aspx') == -1) {
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
    }
    //获取参数
    var getQueryString = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
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
            url: '/PopularSearch.aspx?type=compare&q=' + keyword + '&cache=' + generateMixed(5) + "&pids=" + getQueryString("pids"),
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
       console.info(getQueryString("pids") + "==========");

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
            li.innerHTML = "<img class='pImage' src='" + obj.p[i][5] + "' /><span class='aaaa pName'>" + obj.p[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>")
                                + " <span class='countSpan' >(" + obj.p[i][2].replace("&amp;", "&") + ")</span></span><span class=\"popularSearchPrice\">" + obj.p[i][4] + "</span>";

            li.setAttribute("href", obj.p[i][1].replace("&amp;", "&"));
            li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.p[i][3] + "'\");document.location.href='" + obj.p[i][1] + "';");
            li.title = obj.p[i][0].replace("&amp;", "&").replace("&quote;", "\"");
            rCount++;
        }
        var width = $(window).width();
        if (width <= 800)
            $("#suggest-div2 li .aaaa").css("width", "200px")
        else
            $("#suggest-div2 li .aaaa").css("width", "300px")
        //
        //
        //
        //if (obj.c.length > 0) {
        //    sep = document.createElement("LI");
        //    sep.className = "popularHead";
        //    sep.innerHTML = "<span class='glyphicon glyphicon-bookmark'></span> Categories";
        //    ulRight.appendChild(sep);
        //}

        //rCount = 0;
        //for (var i = 0; i < obj.c.length; i++) {
        //    if (rCount > 4) break;
        //    var li = document.createElement("LI");
        //    li.setAttribute("idx", i + 1);
        //    li.className = "entity";
        //    ulRight.appendChild(li);
        //    li.innerHTML = "<span class='aaaa'>" + obj.c[i][0].replace("&amp;", "&").replace("&lt;", "<").replace("&gt;", ">").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + "</span>";
        //    li.setAttribute("href", obj.c[i][1].replace("&amp;", "&"));
        //    li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.c[i][3] + "'\");document.location.href='" + obj.c[i][1] + "';");
        //    li.title = obj.c[i][2];
        //    rCount++
        //}

        //
        //if (obj.bac.length > 0 || obj.b.length > 0) {
        //    var sep = document.createElement("LI");
        //    sep.className = "popularHead";
        //    sep.innerHTML = "<span class='glyphicon glyphicon-filter'></span> Filtered categories";
        //    ulRight.appendChild(sep);
        //}
        //        else{
        //            var li = document.createElement("LI");
        //            li.setAttribute("idx", 0);
        //            li.className = "entity";
        //            ulRight.appendChild(li);
        //            li.innerHTML = "No result";
        //        
        //        }
        
        //for (var i = 0; i < obj.b.length; i++) {
        //    if (rCount > 7) break;
        //    var li = document.createElement("LI");
        //    li.setAttribute("idx", i + 1);
        //    li.className = "entity";
        //    ulRight.appendChild(li);
        //    li.innerHTML = "<span>" + obj.b[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + " <span class='countSpan'>(Brand)</span>" + "</span>";
        //    li.setAttribute("href", obj.b[i][1].replace("&amp;", "&"));
        //    li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.b[i][3] + "'\");document.location.href='" + obj.b[i][1] + "';");
        //    li.title = obj.b[i][2].replace("&amp;", "&").replace("&quote;", "\"");
        //    rCount++;
        //}

        //for (var i = 0; i < obj.bac.length; i++) {
        //    if (rCount > 11) break;
        //    var li = document.createElement("LI");
        //    li.setAttribute("idx", i + 1);
        //    li.className = "entity";
        //    ulRight.appendChild(li);
        //    li.innerHTML = "<span>" + obj.bac[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + " <span class='countSpan'>(" + obj.bac[i][2] + ")</span>" + "</span>";
        //    li.setAttribute("href", obj.bac[i][1].replace("&amp;", "&"));
        //    li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.bac[i][3] + "'\");document.location.href='" + obj.bac[i][1] + "';");
        //    li.title = obj.bac[i][0].replace("&amp;", "&").replace("&quote;", "\"");;
        //    rCount++
        //}

        //if (obj.r.length > 0) {
        //    var sep = document.createElement("LI");
        //    sep.className = "popularHead";
        //    sep.innerHTML = "<span class='glyphicon glyphicon-home'></span> Retailers";
        //    ulRight.appendChild(sep);
        //}

        //for (var i = 0; i < obj.r.length; i++) {
        //    var li = document.createElement("LI");
        //    li.setAttribute("idx", i + 1);
        //    li.className = "entity";
        //    ulRight.appendChild(li);
        //    li.innerHTML = "<span class='aaaa'>" + obj.r[i][0].replace("&amp;", "&").replace("&quote;", "\"").replace(reg, "<strong>$1</strong>") + "<img src='" + obj.r[i][2] + "') />" + "</span>";
        //    li.setAttribute("href", obj.r[i][1].replace("&amp;", "&"));
        //    li.setAttribute("onclick", "ga('send', 'pageview', \"'" + obj.r[i][3] + "'\");document.location.href='" + obj.r[i][1] + "';");
        //    li.title = obj.r[i][0].replace("&amp;", "&").replace("&quote;", "\"");
        //}
    }

    var keydown = function (evt) {
        evt = window.event || evt;
        if (evt.keyCode == 13) {
            return search(input.id);
        }

        if (storedSuggest.suggest != null && storedSuggest.suggest.length != 0 && (evt.keyCode in directionKey)) {
            switch (evt.keyCode) {
                case 38: //up
                    focusIdx -= searchSuggest.style.display != "none" ? 1 : 0;
                    break;
                case 40:
                    focusIdx += searchSuggest.style.display != "none" ? 1 : 0;
                    break;
            }

            searchSuggest.style.display = "";

            var suggestItems = $("#searchSuggest li.entity");
            if (focusIdx > suggestItems.length) {
                focusIdx = suggestItems.length;
                return;
            } else if (focusIdx <= 0) {
                focusIdx = 0;
                input.value = storedSuggest.keyword;
                input.focus();
            } else {
                try { focusedItem.className = "entity" } catch (e) { }
                suggestItems[focusIdx - 1].className = "entity focus";
                input.value = suggestItems[focusIdx - 1].title.replace(/(<[^>]*>|\([\s\S]*?\))/g, "").trim();
                input.focus();
                focusedItem = suggestItems[focusIdx - 1];
            }
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
})(SuggestHelper2);