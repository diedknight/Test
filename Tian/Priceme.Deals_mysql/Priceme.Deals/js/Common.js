function GlobalAjax(controller, action, param_json, callback_func) {
    var url = "AjaxPage.aspx";
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


function onImgError(source) {
    source.src = "//images.pricemestatic.com/images/no_image_available.gif";
    source.onerror = "";
    return true;
}