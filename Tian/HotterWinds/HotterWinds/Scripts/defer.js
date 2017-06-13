
function PostToFB(msg) {
    var val = $('input[name="rating"]').val();

    if (val == "") {
        alert("Please make sure you have selected a rating!");
        return false;
    }

    var title = $('#ctl00_ContentPlaceHolder1_FooterReview1_txtTitle').val();
    if (title == "") {
        alert("Please retype title!");
        return false;
    }

    var review = $('#ctl00_ContentPlaceHolder1_FooterReview1_txtReview').val();

    if (review == "") {
        alert("Please retype review!");
        return false;
    }

    if (confirm('Add link to your review to your Facebook wall?')) {
        FB.login(function (response) {
            if (response.session) {
                // user successfully logged in
                //alert('user successfully logged in');
                if (response.perms) {
                    // user is logged in and granted some permissions.
                    // perms is a comma separated list of granted permissions
                    //alert('user is logged in and granted some permissions.');
                    var body = msg;
                    FB.api('/me/feed', 'post', { message: body }, function (response) {
                        if (!response || response.error) {
                            alert('Error occured');
                        } else {
                            //alert('Post ID: ' + response.id);
                            __doPostBack('ctl00$ContentPlaceHolder1$FooterReview1$btnSave', '');
                        }
                    });

                } else {
                    // user is logged in, but did not grant any permissions
                    //alert('Logged in OK with NO permissions');
                }

            } else {
                //user cancelled login
                //alert('user cancelled login');
            }
        }, { perms: 'read_stream,publish_stream,email, user_interests, user_birthday' });
    }

    return true;
}
function ShowRetailerMap(rid, lcid) {
    var url = "/RetailerGMap.aspx?retailerid=" + rid + "&lcid=" + lcid;
    window.open(url);
} function idsObject() {
    this.pidString = "";
    this.count = 0;
};
function Redirect(pageUrl) {
    //pageTracker._trackPageview(pageUrl);
    ga('send', 'pageview', "'" + pageUrl + "'");
    window.open(pageUrl, generateMixed(5), "", "");
}

function ConfirmSelectValue(txtTitle, txtReview) {

    var obj = document.getElementById('ctl00_ContentPlaceHolder1_FooterRetailerReview1_value');

    if (obj != null) {
        var val = obj.value;
        if (val == "") {
            alert("Please make sure you have selected a rating!");
            return false;
        }
    }

    if (obj == null) {
        var title = document.getElementById(txtTitle).value;
        if (title == "") {
            alert("Please retype title!");
            return false;
        }
    }

    var review = document.getElementById(txtReview).value;

    if (review == "") {
        alert("Please retype review!");
        return false;
    }
    return true;
}
function Show(object) {
    $(object).removeAttr("href");
    $(object).css("color", "#36c")
    $('#productrv').css("display", "block");
}
function Hide(object) {
    $(object).removeAttr("href");
    $(object).css("color", "#36c")
    $('#productrv').css("display", "none");
    $('#retailertrv').css("display", "none");
}