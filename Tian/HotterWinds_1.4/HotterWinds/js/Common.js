function GlobalAjax(controller, action, param_json, callback_func) {
    var url = "../AjaxPage.aspx";
    var data = {
        "controller": controller,
        "action": action,
        "url": location.href,
        "param": JSON.stringify(param_json)
    };

    $.ajax({
        url: url,
        method: "POST",
        data: data,
        dataType: "html",
        success: function (msg) { callback_func(msg); },
        error: function () { callback_func("0"); }
    });
}


function onImgError2(source) {
    source.src = "https://s3.pricemestatic.com/Images/HotterWindsVersion/no_image_available2.gif";
    source.onerror = "";
    return true;
}

function onImgError3(source) {
    source.src = "https://s3.pricemestatic.com/Images/HotterWindsVersion/no_image_available3.gif";
    source.onerror = "";
    return true;
}

function onImgError_blog(source) {
    source.src = "https://s3.pricemestatic.com/Images/HotterWindsVersion/no_image_available_blog.gif";
    source.onerror = "";
    return true;
}

function onImgError_catalog(source) {
    source.src = "https://s3.pricemestatic.com/Images/HotterWindsVersion/no_image_available_category.gif";
    source.onerror = "";
    return true;
}