function getRecentlyViewed() {
    $.ajax({
        type: 'GET',
        url: '/RecentlyViewed.aspx',
        dataType: 'html',
        beforeSend: function () {
            $('#recently-viewed-div').append('<img id="loadImg" src="https://images.pricemestatic.com/images/loading.gif" />');
        },
        success: function (data, textStatu) {
            $('#recently-viewed-div').html(data);
            loadImage('#recently-viewed-div');
        },
        error: function (request, error) {
            $('#loadImg').remove();
        }
    });
}

var cNavBinded = false;
$(document).ready(function(){
    $('#LiRecentlyViewed').css('display', 'none');
    PriceMeSurvey.DoExpand();

    CategoryNavInit();

    var width = $(window).width();

    proccessCategoryNav(width);
});

$(window).resize(function () {
    var width = $(window).width();

    proccessCategoryNav(width);
});

function proccessCategoryNav(windowWidth) {
    if (windowWidth < 768) {
        if (cNavBinded) {
            $(".popupCategoryLI").unbind();
            cNavBinded = false;
        }
    } else {
        if (!cNavBinded) {
            $(".popupCategoryLI").bind("mouseenter", function () {
                var myImage = $(this).find("img");
                var src = myImage.attr("src");
                myImage.attr("src", myImage.attr("data-pm-src3"));
                myImage.attr("data-pm-src3", src);

                $(this).find(".popupSubcategory").show(300);
            });

            $(".popupCategoryLI").bind("mouseleave", function () {
                var myImage = $(this).find("img");
                var src = myImage.attr("src");
                myImage.attr("src", myImage.attr("data-pm-src3"));
                myImage.attr("data-pm-src3", src);

                $(this).find(".popupSubcategory").hide();
            });

            cNavBinded = true;
        }
    }
}

function CategoryNavInit() {
    $(".popupCategoryLI img").each(function () {
        var img = new Image();
        img.src = $(this).attr("data-pm-src3");
    });
}

function loadImage(container) {

    var imgs = $(container).find('img');
    imgs.each(function (){
        var src2 = $(this).attr("data-pm-src2");
        if (src2) {
            $(this).attr("src", src2).removeAttr("data-pm-src2");
        }
    })
}

var DK_Tabs = {};
(function (tabs) {
    tabs.init = function (tabsClass, contentClass, activeClassName) {
        var allTabs = $(tabsClass + " > li");
        var allContents = $(contentClass);
        

        allTabs.each(function (index) {
            $(this).click(function () {
                allTabs.removeClass(activeClassName);
                allContents.css("display", "none");
                $(this).addClass(activeClassName);
                $(allContents[index]).css("display", "block");
            })
        });

        if(allTabs.length > 0){
            $(allTabs[0]).click();
        }
    }
})(DK_Tabs);

var newsletterDivIsMax = true;
function procNewsletterDiv(currentWidth) {

    if (currentWidth < 768 && newsletterDivIsMax) {
        var labelText = $('#HomeNewsletterDiv .input-group .form-control label').html();
        var placeholder = $('#HomeNewsletterDiv .input-group .form-control input').attr('placeholder');

        $('#HomeNewsletterDiv .input-group .form-control input').attr('placeholder', labelText + placeholder);
        $('#HomeNewsletterDiv .input-group .form-control label').css('display', 'none');

        newsletterDivIsMax = false;
    } else if (currentWidth >= 768 && !newsletterDivIsMax) {
        var labelText = $('#HomeNewsletterDiv .input-group .form-control label').html();
        var placeholder = $('#HomeNewsletterDiv .input-group .form-control input').attr('placeholder');

        $('#HomeNewsletterDiv .input-group .form-control input').attr('placeholder', placeholder.replace(labelText, ''));
        $('#HomeNewsletterDiv .input-group .form-control label').css('display', 'table-cell');

        newsletterDivIsMax = true;
    }
}