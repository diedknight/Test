var showPdalert = false;
function ShowProductAlert(pid, bestPrice, return_url) {
    LoadProductAlert(pid, bestPrice, return_url);
}

function LoadProductAlert(pid, bestPrice, return_url) {
    $.ajax({
        url: '/ProductAlertPopup.aspx',
        data: { 'pid': pid, 'bestPrice': bestPrice, 'returnUrl': return_url },
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#popPriceAlter').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            //if (html == "noLogin")
            //    location.href = "/Login.aspx?url=" + return_url;
            //else {
                $('#popPriceAlter').html(html);
                setTimeout("$('#txtPrice').focus()", 500);
           // }
            
        }
    });
}

function ClosePopUpPrice() {
    $('#PopUpBGDivPrice').css('display', 'none');
    $('#PopUpDivPrice').css('display', 'none');
}

var showPh = false;
function ShowProductPh(pid) {
    if (showPh) {
        $('#PopUpBGDivPh').css('display', 'block');
        $('#PopUpDivPh').css('display', 'block');
    }
    else {
        showPh = true;
        $('#PopUpBGDivPh').css('display', 'block');
        if (navigator.userAgent.toLowerCase())
            $('#PopUpBGDivPh').css('filter', 'alpha(opacity=25); ZOOM: 1');
        $('#PopUpBGDivPh').css('height', $(document.body).height() + "px");
        $('#PopUpDivPh').css('display', 'block');
        $('#PopUpDivPh').css('top', "0");
        $('#PopUpDivPh').css('left', "0");
        var height = 720;
        var width = 630;
        var windowHeight = $(window).height();
        if (windowHeight > height) {
            $('#PopUpDivPh').css('top', ($(window).height() - height) / 2 + "px");
        }
        var windowWidth = $(window).width();
        if (windowWidth > width) {
            $('#PopUpDivPh').css('left', ($(window).width() - width) / 2 + "px");
        }

        LoadProductPh(pid);
    }
}

function LoadProductPh(pid) {
    $.ajax({
        url: '/NewGetHistoryPage.aspx?pid=' + pid,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#PopUpContentDivPh').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            var html = '<iframe src="/NewGetHistoryPage.aspx?pid=' + pid + '" style="border: none;overflow: hidden;width:100%;height:100%" scrolling="no">';
            $('#PopUpContentDivPh').html(html);
        },
        error: function(){
            $('#PopUpContentDivPh').html('Sorry, No history.');
        }
    });
}

var sphPid = 0;
function ShowProductPh_PriceDrop(pid) {
    if (sphPid == pid) {
        $('#PopUpBGDivPh').css('display', 'block');
        $('#PopUpDivPh').css('display', 'block');
    }
    else {
        sphPid = pid;
        $('#PopUpBGDivPh').css('display', 'block');
        if (navigator.userAgent.toLowerCase())
            $('#PopUpBGDivPh').css('filter', 'alpha(opacity=25); ZOOM: 1');
        $('#PopUpBGDivPh').css('height', $(document.body).height() + "px");
        $('#PopUpDivPh').css('display', 'block');
        $('#PopUpDivPh').css('top', "0");
        $('#PopUpDivPh').css('left', "0");
        var height = 720;
        var width = 630;
        var windowHeight = $(window).height();
        if (windowHeight > height) {
            $('#PopUpDivPh').css('top', ($(window).height() - height) / 2 + "px");
        }
        var windowWidth = $(window).width();
        if (windowWidth > width) {
            $('#PopUpDivPh').css('left', ($(window).width() - width) / 2 + "px");
        }

        LoadProductPh(pid);
    }
}

function ClosePopUpPh() {
    $('#PopUpBGDivPh').css('display', 'none');
    $('#PopUpDivPh').css('display', 'none');
}

function DisplayShowDiv(divId, showdivId, pricedivId, pricedivrowId) {
    var showItem = document.getElementById(divId);
    var showDivItem = document.getElementById(showdivId);
    var priceDivItem = $('#' + pricedivId);
    var priceDivRowItem = document.getElementById(pricedivrowId);

    if (showDivItem.style.display == "block") {
        showDivItem.style.display = "none";
        showItem.className = "bg1 show countSpan";
        priceDivItem.removeClass("pricesDivAll");
        priceDivItem.addClass("productlist");
        priceDivRowItem.style.cssText = "";
    }
    else if (showDivItem.style.display == "none") {
        showDivItem.style.display = "block";
        showItem.className = "bg1 less countSpan";
        priceDivItem.removeClass("productlist");
        priceDivItem.addClass("pricesDivAll");
        priceDivRowItem.style.cssText = "padding:1px 2px 3px 4px;";
    }
}

function DisplayFeatureMore() {
    var item = document.getElementById('featureMore');
    var text = document.getElementById('featureText');
    if (item.style.display == "none") {
        item.style.display = "block";
        text.innerHTML = "Less features";
    }
    else {
        item.style.display = "none";
        text.innerHTML = "More features";
    }
}

function productCompare(pid) {
    var ids = getCheckedBoxIDs();

    if (ids.count > 0 && ids.count < 3) {
        window.location = "/Compare.aspx?t=js&pids=" + pid + "," + ids.pidString;
    }
    else {
        alert("Please select 1 - 2 products to compare.");
    }
}

function getCheckedBoxIDs() {
    var ids = new pidsObject();
    var checkboxs = $(".psdCheckBox input, .relatedCheckBox input, .itemStyle input");

    for (var i = 0; i < checkboxs.length; i++) {
        if (checkboxs[i].id.indexOf("checkbox_pid") != -1 && checkboxs[i].checked) {
            ids.pidString += checkboxs[i].value + ",";
            ids.count++;
            checkboxs[i].checked = false;
        }
    }
    return ids;
}

function pidsObject() {
    this.pidString = "";
    this.count = 0;
};

function ShowMoreDetail(rpid, fromPage, type) {
    ShowPopDiv();
    LoadProductInfo(rpid, fromPage, type);
}

function ShowPopDiv() {
    $('#PopUpBGDiv').css('display', 'block');
    if (navigator.userAgent.toLowerCase())
        $('#PopUpBGDiv').css('filter', 'alpha(opacity=25); ZOOM: 1');
    $('#PopUpBGDiv').css('height', $(document.body).height() + "px");
    $('#PopUpDiv').css('display', 'block');
    $('#PopUpDiv').css('top', "0");
    $('#PopUpDiv').css('left', "0");
    var height = 400;
    var width = 600;
    var windowHeight = $(window).height();
    if (windowHeight > height) {
        $('#PopUpDiv').css('top', ($(window).height() - height) / 2 + "px");
    }
    var windowWidth = $(window).width();
    if (windowWidth > width) {
        $('#PopUpDiv').css('left', ($(window).width() - width) / 2 + "px");
    }
}

function LoadProductInfo(rpid, fromPage, type) {
    $.ajax({
        url: '/GetRetailerInfo.aspx?rpid=' + rpid + '&from=' + fromPage + '&type=' + type,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#PopUpContentDiv').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
            //  $('#noPointer').html('');
        },
        success: function (html) {
            $('#PopUpContentDiv').html(html);
            // $('#noPointer').html(title);
        }
    });
}

function ClosePopUp() {
    $('#PopUpBGDiv').css('display', 'none');
    $('#PopUpDiv').css('display', 'none');
}

function LoadProductInfo2(rpid, fromPage, title, retailerid, type) {
    $.ajax({
        url: '/GetRetailerInfoSnd.aspx?rpid=' + rpid + '&from=' + fromPage + '&type=' + type,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#RetailerInfoDiv' + retailerid).html('');
        },
        success: function (html) {
            $('#RetailerInfoDiv' + retailerid).html(html);
        }
    });
}

function ShowImageHelp() {
    var item = $("#imgHelp");
    if (item.attr('class') == "imgHelpNone") {
        item.removeClass('imgHelpNone');
        item.addClass('imgHelpShow');
    }
    else {
        item.removeClass('imgHelpShow');
        item.addClass('imgHelpNone');
    }
}

var showImageVideo = false;
function ShowProductImageVideos(pid, type,openType) {
    if (showImageVideo && type == "iv") {
        $('#PopUpBGDivImg').css('display', 'block');
        $('#PopUpDivImg').css('display', 'block');
    }
    else {
        showImageVideo = true;
        $('#PopUpBGDivImg').css('display', 'block');
        if (navigator.userAgent.toLowerCase())
            $('#PopUpBGDivImg').css('filter', 'alpha(opacity=25); ZOOM: 1');
        $('#PopUpBGDivImg').css('height', $(document.body).height() + "px");
        $('#PopUpDivImg').css('display', 'block');
        $('#PopUpDivImg').css('top', "0");
        $('#PopUpDivImg').css('left', "0");


        //var height = 650;
        //var width = 640;
        //var windowHeight = $(window).height();
        //if (windowHeight > height) {
        //    $('#PopUpDivImg').css('top', ($(window).height() - height) / 4 + "px");
        //}
        //var windowWidth = $(window).width();
        //if (windowWidth > width) {
        //    $('#PopUpDivImg').css('left', ($(window).width() - width) / 2 + "px");popImg popVid
        //}

        LoadProductImages(pid, type, openType);
    }
}

function LoadProductImages(pid, type, openType) {
    $.ajax({
        url: '/GetProductImages.aspx?pid=' + pid + '&type=' + type,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#PopUpContentDivImg').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            $('#PopUpContentDivImg').html(html);
        }
    });
}

function ClosePopUpVideo() {
    var videoF = document.getElementById('framev');
    if (videoF != null) {
        videoF.setAttribute("src", "");
    }
    $('#PopUpBGDivVideo').css('display', 'none');
    $('#PopUpDivVideo').css('display', 'none');
}

function ClosePopUpImg() {
    var videoF = document.getElementById('framev');
    if (videoF != null) {
        videoF.setAttribute("src", "");
    }
    $('#PopUpBGDivImg').css('display', 'none');
    $('#PopUpDivImg').css('display', 'none');
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

function OnClickLink(url) {
    window.open(url, "_blank");
}

$(document).ready(function () {
    ProductResponse();
});

$(window).resize(function () {
    ProductResponse();
});

function ProductResponse() {
    $(".productlist").each(function () {
        var width = $(window).width();
        if (width < 768) {
            var atags = $(this).find(".divImgPadding a, .pricesDivPrice a, .pricesDivVS a");
            if (atags != undefined) {
                var isSet = false;
                for (var i = 0; i < atags.length; i++) {
                    var atag = atags[i];
                    if (atag != undefined) {
                        var stringHref = $(atag).attr("href");
                        if (!isSet && stringHref != undefined) {
                            var stringOc = $(atag).attr("onclick");
                            $(this).attr("onclick", "OnClickLink('" + stringHref + "'); " + stringOc);
                            isSet = true;
                        }
                        $(atag).removeAttr("href");
                        $(atag).removeAttr("onclick");
                    }
                }
            }
        }
        else {
            var stringConten = $(this).attr("onclick");
            if (stringConten != undefined) {
                var stringHref = stringConten.split("OnClickLink('")[1].split("'); ")[0];
                var stringOc = stringConten.split("'); ")[1];
                var atags = $(this).find(".divImgPadding a, .pricesDivPrice a, .pricesDivVS a");
                if (atags != undefined) {
                    for (var i = 0; i < atags.length; i++) {
                        var atag = atags[i];
                        if (atag != undefined) {
                            $(atag).attr("href", stringHref);
                            $(atag).attr("onclick", stringOc);
                        }
                    }
                }
                $(this).removeAttr("onclick");
            }
        }

        //if (width < 750) {
        //    var visitBtn = $(this).find(".pricesDivVS .btnVS a")[0];
        //    if (visitBtn != undefined)
        //        visitBtn.innerHTML = " <span class=\"glyphicon glyphicon-chevron-right\"></span> ";
        //    var visitBtnC = $(this).find(".pricesDivVS .btnVSC a")[0];
        //    if (visitBtnC != undefined) {
        //        visitBtnC.innerHTML = " <span class=\"glyphicon glyphicon-chevron-right\"></span> ";
        //        visitBtnC.className = "btn btnConds btn-xs";
        //    }
        //} else {
        //    var visitBtn = $(this).find(".pricesDivVS .btnVS a")[0];
        //    if (visitBtn != undefined)
        //        visitBtn.innerHTML = str_VS;
        //    var visitBtnC = $(this).find(".pricesDivVS .btnVSC a")[0];
        //    if (visitBtnC != undefined) {
        //        visitBtnC.innerHTML = str_VS;
        //        visitBtnC.className = "btn btnCond btn-xs";
        //    }
        //}
    });
}

function GetExpertReviews(pid) {
    $.ajax({
        type: 'get',
        url: '/ProductExperReview.aspx?pid=' + pid,
        beforeSend: function () {
            $('#pnlHistory').html(loadingImageTag);
        },
        success: function (data, textStatu) {
            if (textStatu == 'success') {
                $('#pnlHistory').html(data);
                $('#pnlHistory').append('<br />');
                SetActiveSpan('#ExperReviews');
            }
        }
    });
}
function GetExpertReviewsByStars(pid, stars, pg, sort) {
    $.ajax({
        type: 'get',
        url: '/ProductExperReview.aspx?pid=' + pid + '&st=' + stars + '&sort=' + sort + '&pg=' + pg,
        beforeSend: function () {
            $('#pnlHistory').html(loadingImageTag);
        },
        success: function (data, textStatu) {
            if (textStatu == 'success') {
                $('#pnlHistory').html(data);
                $('#pnlHistory').append('<br />');
                SetActiveSpan('#ExperReviews');
            }
        }
    });
}
function GetExpertReviews(url, stars, pg) {
    var opt = $('#ERsort').val();
    if (opt == undefined)
        opt = $('#ctl00_ContentPlaceHolder1_FooterExpertReview1_ERsort').val();
    if (opt == undefined)
        opt = $('#FooterExpertReview1_ERsort').val();
    var linkUrl = url + "?stars=" + stars + "&sb=" + opt;
    window.location.href = linkUrl;
    //    $.ajax({
    //        type: 'get',
    //        url: '/ProductExperReview.aspx?pid=' + pid + '&st=' + stars + '&sort=' + opt + '&pg=' + pg,
    //        beforeSend: function () {
    //            $('#pnlHistory').html(loadingImageTag);
    //        },
    //        success: function (data, textStatu) {
    //            if (textStatu == 'success') {
    //                $('#pnlHistory').html(data);
    //                $('#pnlHistory').append('<br />');
    //                SetActiveSpan('#ExperReviews');
    //            }
    //        }
    //    });
}

function displayCompare() {
    $('#btnCompare').css('display', 'block');
}

function loadReportDiv(t) {
    $.ajax({
        type: 'get',
        url: '/Report.aspx?t=' + t,
        dataType: 'html',
        beforeSend: function () {

        },
        success: function (data, textStatu) {
            if (textStatu == 'success') {
                $('#reportDiv').html(data);
                $('#pageurlInput').val(window.location.href);
            }
        },
        error: function () {
            $('#reportDiv').html('<strong>Error : Internal server error, please try again later.</strong>');
        }
    });
}

function ResponseReviewer(rpid) {
    var rating = $('#NewRetailerReview1_ratingValue').val();
    var text = $('#NewRetailerReview1_txtReview').val();
    var url = "/GetRetailerInfo.aspx?type=res&rpid=" + rpid + "&from=p&rat=" + rating + "&text=" + text;

    window.location = url;
}

/*Start Dawn*/
function onLargeImgError(source, imgurl) {
    source.src = imgurl;
    source.onerror = "";
    return true;
}

var showPR = false;
function ShowProductPR(pid) {
    if (showPR) {
        $('#PopUpBGDivPR').css('display', 'block');
        $('#PopUpDivPR').css('display', 'block');
    }
    else {
        showPR = true;
        $('#PopUpBGDivPR').css('display', 'block');
        if ($.support.msie)
            $('#PopUpBGDivPR').css('filter', 'alpha(opacity=25); ZOOM: 1');
        $('#PopUpBGDivPR').css('height', $(document.body).height() + "px");
        $('#PopUpDivPR').css('display', 'block');
        $('#PopUpDivPR').css('top', "0");
        $('#PopUpDivPR').css('left', "0");
        var height = 400;
        var width = 600;
        var windowHeight = $(window).height();
        if (windowHeight > height) {
            $('#PopUpDivPR').css('top', ($(window).height() - height) / 2 + "px");
        }
        var windowWidth = $(window).width();
        if (windowWidth > width) {
            $('#PopUpDivPR').css('left', ($(window).width() - width) / 2 + "px");
        }

        LoadProductPR(pid);
    }
}
function LoadProductPR(pid) {
    $.ajax({
        url: '/GetWriterProductReviews.aspx?pid=' + pid,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#PopUpContentDivPR').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            $('#PopUpContentDivPR').html(html);
        }
    });
}

function ShowImageVideos(divId) {
    var divDoc = document.getElementById(divId);
    var imgtab = document.getElementById("popImgLi");
    var videotab = document.getElementById("popVidLi");
    if (divId == "popImg") {
        divDoc.className = "";
        var vid = document.getElementById("popVid");
        vid.className = "DisplayNone";
        imgtab.className = "active";
        videotab.className = "";
    } else {
        divDoc.className = "";
        var vid = document.getElementById("popImg");
        vid.className = "DisplayNone";
        imgtab.className = "";
        videotab.className = "active";
    }
}

function ShowProductVideo(pid, type) {
    $('#PopUpBGDivVideo').css('display', 'block');
    if ($.browser.msie)
        $('#PopUpBGDivVideo').css('filter', 'alpha(opacity=25); ZOOM: 1');
    $('#PopUpBGDivVideo').css('height', $(document.body).height() + "px");
    $('#PopUpDivVideo').css('display', 'block');
    $('#PopUpDivVideo').css('top', "0");
    $('#PopUpDivVideo').css('left', "0");
    var height = 650;
    var width = 640;
    var windowHeight = $(window).height();
    if (windowHeight > height) {
        $('#PopUpDivVideo').css('top', ($(window).height() - height) / 2 + "px");
    }
    var windowWidth = $(window).width();
    if (windowWidth > width) {
        $('#PopUpDivVideo').css('left', ($(window).width() - width) / 2 + "px");
    }

    LoadProductVideo(pid, type);
}

function LoadProductVideo(pid, type) {
    $.ajax({
        url: '/GetProductImages.aspx?pid=' + pid + '&type=' + type,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#PopUpContentDivVideo').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            $('#PopUpContentDivVideo').html(html);
        }
    });
}
/*End Dawn*/

function ShowUpcoming(pid, islogin) {
    if (islogin == 0)
        ShowPopDiv();

    $.ajax({
        url: '/GetUpcoming.aspx?pid=' + pid + '&login=' + islogin,
        type: 'GET',
        dataType: 'html',
        beforeSend: function () {
            $('#PopUpcoming').html('<img src="https://images.pricemestatic.com/images/ajax/ajaxloading.gif" height="16" width="16" />');
        },
        success: function (html) {
            $('#PopUpcoming').html(html);
        }
    });

    if (islogin == 1) {
        var item = $('#upcomingmess');
        item.html("You will be notified when the product becomes available.");
    }
}

function FavouriteProduct(pid, isfavourite, islogin) {
    var item = $('#spanproduct');
    var btn = $('#favproduct');
    if (islogin != "True") {
        location.href = '/FavouriteProduct.aspx?pid=' + pid + '&fav=' + isfavourite;
    }
    else {
        $.ajax({
            url: '/FavouriteProduct.aspx?pid=' + pid + '&fav=' + isfavourite,
            type: 'GET',
            dataType: 'html',
            success: function (html) {
                if (isfavourite == "True") {
                    item.removeClass('glyphicon glyphicon-heart-empty');
                    item.addClass('glyphicon glyphicon-heart iconBlue');
                    btn.removeClass('sub-favourite iconGrayIm');
                    btn.addClass('sub-favourite iconBlue');
                    btn[0].setAttribute("onclick", "FavouriteProduct('" + pid + "', 'Flase', '" + islogin + "'); FavisLogin('" + islogin + "', 'Flase', 'favouriteproduct.aspx');");
                }
                else {
                    item.removeClass('glyphicon glyphicon-heart');
                    item.addClass('glyphicon glyphicon-heart-empty');
                    btn.removeClass('sub-favourite iconBlue');
                    btn.addClass('sub-favourite iconGrayIm');
                    btn[0].setAttribute("onclick", "FavouriteProduct('" + pid + "', 'True', '" + islogin + "'); FavisLogin('" + islogin + "', 'True', 'favouriteproduct.aspx');");
                }
            }
        });
    }
}

function setTableWidthsP() {
    var windowswidth = $(window).width();
    if (windowswidth <= 750)
        $(".anchorSpecs").hide();
    else
        $(".anchorSpecs").show();
}

function setTableWidthPD() {
    var windowswidth = $(window).width();

    if (windowswidth > 1200) {
        $(".rich-button").css({
            "margin-left": "30px",
            "margin-top": "0px"
        })

        $(".rich-left-attr").css({
            "width": "188px"
        })

        $(".rich-right-attr").css({
            "padding-left": "6%"
        })
        //console.info("1200");
    } else if (windowswidth > 1007) {
        $(".rich-button").css({
            "margin-left": "30px",
            "margin-top": "0px"
        })

        $(".rich-right-attr").css({
            "padding-left": "6%"
        })
        $(".rich-left-attr").css({
            "width": "188px"
        })

    } else if (windowswidth > 750) {
        $(".rich-button").css({
            "margin-left": "30px",
            "margin-top": "0px"
        })

        $(".rich-right-attr").css({
            "padding-left": "6%"
        })
        $(".rich-left-attr").css({
            "width": "188px"
        })
    } else if (windowswidth > 400) {
        $(".rich-button").css({
            "margin-left": "0px",
            "margin-top": "5px"
        })
        $(".rich-left-attr").css({
            "width": "150px"
        })

        $(".rich-right-attr").css({
            "padding-left": "1%"
        })
        //console.info("360");
    } else {
        $(".rich-button").css({
            "margin-left": "0px",
            "margin-top": "5px"
        })
        $(".rich-left-attr").css({
            "width": "120px"
        })

        $(".rich-right-attr").css({
            "padding-left": "1%"
        })
    }

}
