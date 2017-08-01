
function ReviewOver(val, id) {
    Reviewclear();
    SetTitle(val, id);
    SetReviewSta(val);
}

function ReviewOut(val, id) {
    Reviewclear();

    var clickvalue = $("#" + id).val();
    if (clickvalue == "-1") {
        SetTitle("", id);
    }
    else {
        SetTitle(clickvalue, id);
        SetReviewSta(clickvalue);
    }
}

function ReviewClick(val, id) {
    SetTitle(val, id);
    SetReviewSta(val);
    $("#" + id).val(val);
}

function SetReviewSta(val) {
    switch (val) {
        case "1":
            $("#rat1").removeClass("ratout");
            $("#rat1").addClass("ratover");
            break;
        case "2":
            $("#rat1").removeClass("ratout");
            $("#rat1").addClass("ratover");
            $("#rat2").removeClass("ratout");
            $("#rat2").addClass("ratover");
            break;
        case "3":
            $("#rat1").removeClass("ratout");
            $("#rat1").addClass("ratover");
            $("#rat2").removeClass("ratout");
            $("#rat2").addClass("ratover");
            $("#rat3").removeClass("ratout");
            $("#rat3").addClass("ratover");
            break;
        case "4":
            $("#rat1").removeClass("ratout");
            $("#rat1").addClass("ratover");
            $("#rat2").removeClass("ratout");
            $("#rat2").addClass("ratover");
            $("#rat3").removeClass("ratout");
            $("#rat3").addClass("ratover");
            $("#rat4").removeClass("ratout");
            $("#rat4").addClass("ratover");
            break;
        case "5":
            $("#rat1").removeClass("ratout");
            $("#rat1").addClass("ratover");
            $("#rat2").removeClass("ratout");
            $("#rat2").addClass("ratover");
            $("#rat3").removeClass("ratout");
            $("#rat3").addClass("ratover");
            $("#rat4").removeClass("ratout");
            $("#rat4").addClass("ratover");
            $("#rat5").removeClass("ratout");
            $("#rat5").addClass("ratover");
            break;
        case "":
            selectVal = "";
            break;
    }
}

function SetTitle(val, id) {
    if (val == "") {
        $("#ratingTitle").text("");
        if (id == "ratingValue")
            $("#valueTitle").text("");
    }
    else {
        var selectVal;
        switch (val) {
            case "1":
                selectVal = " Very poor";
                break;
            case "2":
                selectVal = " Poor";
                break;
            case "3":
                selectVal = " OK";
                break;
            case "4":
                selectVal = " Good";
                break;
            case "5":
                selectVal = " Excellent";
                break;
            case "":
                selectVal = "";
                break;
        }
        $("#ratingTitle").text(selectVal);
        if (id == "ratingValue")
            $("#valueTitle").text(selectVal);
    }
}

function Reviewclear() {
    $("#rat1").removeClass("ratover");
    $("#rat1").addClass("ratout");
    $("#rat2").removeClass("ratover");
    $("#rat2").addClass("ratout");
    $("#rat3").removeClass("ratover");
    $("#rat3").addClass("ratout");
    $("#rat4").removeClass("ratover");
    $("#rat4").addClass("ratout");
    $("#rat5").removeClass("ratover");
    $("#rat5").addClass("ratout");
}
