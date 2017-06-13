function showRetailerFeatureTab(divid, spanid) {
    $(spanid).addClass('active');
    var ids = ['#StoreProducts', '#StoreReviews', '#StoreInfo', '#StoreLocation', '#StoreCategories'];
    for (var i = 0; i < ids.length; i++) {
        if (ids[i] != spanid) {
            $(ids[i]).removeClass('active');
        }
    }

    $(divid).addClass('retaileridisblock');
    $(divid).removeClass('retailerdisnone');
    var divids = ['#StoreProductsPage', '#StoreReviewsPage', '#StoreInfoPage', '#StoreLocationPage', '#StoreCategoriesPage'];
    for (var i = 0; i < divids.length; i++) {
        if (divids[i] != divid) {
            $(divids[i]).removeClass('retaileridisblock');
            $(divids[i]).addClass('retailerdisnone');
        }
    }
    if (divid == '#StoreLocationPage') $(divid).css('height', '100%');
}


function showFeatureTab(tab, idx, cid) {
    tab = $$(tab);
    var pages = getElementsByClassName("pages", tab)[0];
    var item = new Array();
    item = getElementsByClassName("page", pages);

    for (var i = 0; i < item.length; i++) {
        var each = item[i];
        each.style.display = 'none';
    }
    var page = item[idx];

    if (!page) return;
    page.style.display = 'block';
    //pages.scrollTop = page.offsetTop;

    try { getElementsByClassName("active", tab)[0].className = "tab"; } catch (e) { }
    getElementsByClassName("tab", tab)[idx].className = "tab active";
}


$(function () {
    $.ratingDelivery = {
        cancel: 'Cancel Rating',
        cancelValue: '',
        split: 0,
        starWidth: 16,
        currentValue: '',
        groups: {},
        focus: function (value, link) {
            $("#starDeliveryValue").text(link.title);
            $("#starDeliveryText").text(link.title);
        },
        blur: function (value, link) {
            $("#starDeliveryValue").text(link.title);
            $("#starDeliveryText").text($.ratingDelivery.currentValue);
        },
        event: {
            fill: function (n, el, settings, state) { // fill to the current mouse position.
                this.drain(n);
                $(el).addClass('star_' + (state || 'hover'));
                $(el).prevAll('.star_group_' + n).addClass('star_' + (state || 'hover'));
                var lnk = $(el).children('a'); val = lnk.text();
                if (settings.focus) settings.focus.apply($.ratingDelivery.groups[n].valueElem[0], [val, lnk[0]]);
            },
            drain: function (n, el, settings) { // drain all the stars.
                $.ratingDelivery.groups[n].valueElem.siblings('.star_group_' + n).removeClass('star_on').removeClass('star_hover');
            },
            reset: function (n, el, settings) { // Reset the stars to the default index.
                if (!$($.ratingDelivery.groups[n].current).is('.cancel')) {
                    $($.ratingDelivery.groups[n].current).addClass('star_on');
                    $($.ratingDelivery.groups[n].current).prevAll('.star_group_' + n).addClass('star_on');
                }
                var lnk = $(el).children('a'); val = lnk.text();
                if (settings.blur) settings.blur.apply($.ratingDelivery.groups[n].valueElem[0], [val, lnk[0]]);
            },
            click: function (n, el, settings) { // Selected a star or cancelled
                $.ratingDelivery.groups[n].current = el;
                var lnk = $(el).children('a'); val = lnk.text();
                $.ratingDelivery.groups[n].valueElem.val(val);

                // Update display
                $.ratingDelivery.event.drain(n, el, settings);
                $.ratingDelivery.event.reset(n, el, settings);
                if (settings.callback) settings.callback.apply($.ratingDelivery.groups[n].valueElem[0], [val, lnk[0]]);

                $("#ctl00_ContentPlaceHolder1_RetailerInfoTab1_RetailerReviewDisplay1_starDeliveryValue").val(val);
                $("#starDeliveryText").text(lnk.attr("title"));
                $.ratingDelivery.currentValue = lnk.attr("title");
            }
        }// plugin events
    };

    $.fn.ratingDelivery = function (instanceSettings) {
        if (this.length == 0) return this; // quick fail

        instanceSettings = $.extend(
			{}/* new object */,
			$.ratingDelivery/* global settings */,
			instanceSettings || {} /* just-in-time settings */
		);

        this.each(function (i) {

            var settings = $.extend(
				{}/* new object */,
				instanceSettings || {} /* current call settings */,
				($.metadata ? $(this).metadata() : ($.meta ? $(this).data() : null)) || {} /* metadata settings */
			);

            var n = (this.name || 'unnamed-rating').replace(/\[|\]/, "_");

            if (!$.ratingDelivery.groups[n]) $.ratingDelivery.groups[n] = { count: 0 };
            i = $.ratingDelivery.groups[n].count; $.ratingDelivery.groups[n].count++;

            $.ratingDelivery.groups[n].readOnly = $.ratingDelivery.groups[n].readOnly || settings.readOnly || $(this).attr('disabled');

            if (i == 0) {
                // Create value element (disabled if readOnly)
                $.ratingDelivery.groups[n].valueElem = $('<input type="hidden" name="' + n + '" value=""' + (settings.readOnly ? ' disabled="disabled"' : '') + '/>');
                // Insert value element into form
                $(this).before($.ratingDelivery.groups[n].valueElem);
            }; // if (i == 0) (first element)

            // insert rating option right after preview element
            eStar = $('<div class="star"><a title="' + (this.title || this.value) + '">' + this.value + '</a></div>');
            $(this).after(eStar);

            // Half-stars?
            if (settings.half) settings.split = 2;

            // Prepare division settings
            if (typeof settings.split == 'number' && settings.split > 0) {
                var stw = ($.fn.width ? $(eStar).width() : 0) || settings.starWidth;
                var spi = (i % settings.split), spw = Math.floor(stw / settings.split);
                $(eStar)
				.width(spw)
				.find('a').css({ 'margin-left': '-' + (spi * spw) + 'px' })
            };

            $(eStar).addClass('star_group_' + n);

            // readOnly?
            if ($.ratingDelivery.groups[n].readOnly)//{ //save a byte!
                $(eStar).addClass('star_readonly');
                //}  //save a byte!
            else//{ //save a byte!
                $(eStar)
				.addClass('star_live')
				.mouseover(function () { $.ratingDelivery.event.drain(n, this, settings); $.ratingDelivery.event.fill(n, this, settings, 'hover'); })
				.mouseout(function () { $.ratingDelivery.event.drain(n, this, settings); $.ratingDelivery.event.reset(n, this, settings); })
				.click(function () { $.ratingDelivery.event.click(n, this, settings); });
            //}; //save a byte!
            if (this.checked) $.ratingDelivery.groups[n].current = eStar;

            //remove this checkbox
            $(this).remove();

            // reset display if last element
            if (i + 1 == this.length) $.ratingDelivery.event.reset(n, this, settings);

        }); // each element

        // initialize groups...
        for (n in $.ratingDelivery.groups)//{ not needed, save a byte!
            (function (c, v, n) {
                if (!c) return;
                $.ratingDelivery.event.fill(n, c, instanceSettings || {}, 'on');
                $(v).val($(c).children('a').text());
            })
			($.ratingDelivery.groups[n].current, $.ratingDelivery.groups[n].valueElem, n);

        return this;
    };

    $(function () { $('.starDelivery').ratingDelivery(); });
})

function onRetailerPageLoaded() {
    if (location.hash == '#StoreReviews') {
        //showRetailerFeatureTab('#StoreLocationPage', '#StoreLocation');
        setTimeout(onRetailerPageLoaded2, 2000);
    }
}

function onRetailerPageLoaded2() {
    showRetailerFeatureTab('#StoreReviewsPage', '#StoreReviews');
}